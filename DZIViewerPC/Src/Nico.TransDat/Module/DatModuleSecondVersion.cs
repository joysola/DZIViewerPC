using Newtonsoft.Json;
using Nico.TransDat.Base;
using Nico.TransDat.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nico.TransDat.Module
{
    /// <summary>
    /// Dat文件生成和解析的2.0版本
    /// </summary>
    internal class DatModuleSecondVersion : BaseDatModule, IBaseDatModule
    {
        /// <summary>
        /// 每一层的索引存储需要的长度
        /// </summary>
        private static int DatIndexLength = 2048; // 2k
        /// <summary>
        /// 版本信息
        /// </summary>
        public string Version { get => "V2.0.0.0"; set => throw new NotImplementedException(); }

        /// <summary>
        /// 每一层索引所在的位置
        /// </summary>
        private static readonly Dictionary<int, int> DatIndexStartPoint = new Dictionary<int, int> {{ -1, VersionIndexLength },
                                                                                           { 1, VersionIndexLength + DatIndexLength * 1 }, 
                                                                                           { 2, VersionIndexLength + DatIndexLength * 2 },
                                                                                           { 3, VersionIndexLength + DatIndexLength * 3 },
                                                                                           { 4, VersionIndexLength + DatIndexLength * 4 },
                                                                                           { 5, VersionIndexLength + DatIndexLength * 5 },
                                                                                           { 6, VersionIndexLength + DatIndexLength * 6 },
                                                                                           { 7, VersionIndexLength + DatIndexLength * 7 },
                                                                                           { 8, VersionIndexLength + DatIndexLength * 9 },
                                                                                           { 9, VersionIndexLength + DatIndexLength * 13 } ,
                                                                                           { 10, VersionIndexLength + DatIndexLength * 24 }};

        /// <summary>
        /// 将9层图所有瓦片图写入到一个Dat文件中
        /// </summary>
        /// <param name="simplePath">样本路径</param>
        /// <param name="targetPath">dat文件的生成路径,包含.dat 扩展名，默认为空，则dat文件自动存放在SimplePath</param>
        /// <returns>返回异常信息，如果未空则解析成功，如果不为空则解析失败</returns>
        public string CreateDatFileFromTileImage(string simplePath, string targetPath = "")
        {
            string result = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(targetPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                }
                else
                {
                    targetPath = simplePath + "\\" + System.IO.Path.GetFileName(simplePath) + ".dat";
                }

                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }

                // 文件头信息，包含每张图片的层数，x,y，图片大小，二进制所在的位置
                List<ImageDat> imageDatList = new List<ImageDat>();
                int byteIndex = HeadLength;

                // 加载当前文件夹下文件，非瓦片图
                FileInfo[] otherFiles = new DirectoryInfo(simplePath).GetFiles();
                for (int i = 0; i < otherFiles.Length; i++)
                {
                    byte[] buffer = TransHelper.ReadByteFile(otherFiles[i].FullName);
                    imageDatList.Add(new ImageDat() { Level = -1, OtherFileName = otherFiles[i].Name, Size = buffer.Length, ByteIndex = byteIndex, ImageByte = buffer });
                    byteIndex += buffer.Length;
                    buffer = null;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(simplePath);
                DirectoryInfo[] layArr = dirInfo.GetDirectories();
                for (int i = 0; i < layArr.Length; i++)
                {
                    DirectoryInfo layDirInfo = new DirectoryInfo(layArr[i].FullName);
                    int layIndex = int.Parse(layDirInfo.Name);
                    DirectoryInfo[] slightArr = layDirInfo.GetDirectories();
                    for (int x = 0; x < slightArr.Length; x++)
                    {
                        int xIndex = int.Parse(slightArr[x].Name);
                        // 循环每一列的所有图片
                        FileInfo[] imgArr = slightArr[x].GetFiles();
                        for (int y = 0; y < imgArr.Length; y++)
                        {
                            int yIndex = int.Parse(imgArr[y].Name.ToUpper().Replace(".JPG", ""));
                            byte[] imageByte = TransHelper.ReadImage(imgArr[y].FullName);
                            imageDatList.Add(new ImageDat() { Level = layIndex, X = xIndex, Y = yIndex, Size = imageByte.Length, ByteIndex = byteIndex, ImageByte = imageByte });
                            byteIndex += imageByte.Length;
                            imageByte = null;
                        }

                        imgArr = null;
                    }

                    slightArr = null;
                }

                layArr = null;

                // 定义dat文件总大小
                byte[] allBytes = new byte[byteIndex];

                // 图片写入总字节数组
                imageDatList.ForEach(x =>
                {
                    Buffer.BlockCopy(x.ImageByte, 0, allBytes, x.ByteIndex, x.Size);
                    x.ImageByte = null;
                });

                // 索引计算
                int imageDatByteIndex = HeadIndexLength;
                List<ImageDatIndex> datIndexList = new List<ImageDatIndex>();
                int minX = 0;
                int maxX = 0;
                int splitX = 8;
                int splitXCount = 0;
                List<ImageDat> tmpLevel = null;
                int minLevel = imageDatList.Min(t => t.Level);
                int maxLevel = imageDatList.Max(t => t.Level);
                for (int level = minLevel; level <= maxLevel; level++)
                {
                    tmpLevel = imageDatList.Where(x => x.Level == level).ToList();
                    if (tmpLevel.Count == 0)
                    {
                        continue;
                    }

                    // 层数越大，则分隔的越细
                    splitX = level - 5 >= 0 ? 8 - (level - 5) : 8;
                    minX = tmpLevel.Min(t => t.X);
                    maxX = tmpLevel.Max(t => t.X);
                    int tmpMaxX = 0;
                    splitXCount = (int)Math.Ceiling(1.0 * (maxX + 1) / splitX);
                    for (int i = 0; i < splitXCount; i++)
                    {
                        minX = i * splitX;
                        tmpMaxX = maxX > (i + 1) * splitX - 1 ? (i + 1) * splitX - 1 : maxX;
                        tmpLevel = imageDatList.Where(x => x.Level == level && x.X >= minX && x.X <= tmpMaxX).ToList();
                        string tmpLevelStr = JsonConvert.SerializeObject(tmpLevel);
                        byte[] tmpLevelByte = TransHelper.StringToBase64StringByte(tmpLevelStr);
                        Buffer.BlockCopy(tmpLevelByte, 0, allBytes, imageDatByteIndex, tmpLevelByte.Length);
                        datIndexList.Add(new ImageDatIndex() { Level = level, ByteIndex = imageDatByteIndex, MinX = minX, MaxX = tmpMaxX, Size = tmpLevelByte.Length });
                        imageDatByteIndex += tmpLevelByte.Length;
                        tmpLevel.Clear();
                        tmpLevel = null;
                    }
                }

                // 写入索引到指定位置
                for (int level = minLevel; level <= maxLevel; level++)
                {
                    List<ImageDatIndex> tmpIndex = datIndexList.Where(x => x.Level == level).ToList();
                    if(tmpIndex.Count == 0)
                    {
                        continue;
                    }

                    string strIndex = JsonConvert.SerializeObject(tmpIndex);
                    byte[] indexByte = TransHelper.StringToBase64StringByte(strIndex);
                    Buffer.BlockCopy(indexByte, 0, allBytes, DatIndexStartPoint[level], indexByte.Length);
                }

                // 写入版本信息
                int width = (imageDatList.Where(x => x.Level == maxLevel).Max(t => t.X) + 1) * 256;
                int height = (imageDatList.Where(x => x.Level == maxLevel).Max(t => t.Y) + 1) * 256;
                DatVersionInfo datVer = new DatVersionInfo() { Version = Version, Width = width, Height = height };
                byte[] verByte = TransHelper.StringToBase64StringByte(JsonConvert.SerializeObject(datVer));
                Buffer.BlockCopy(verByte, 0, allBytes, 0, verByte.Length);

                // 保存文件
                string datFullPath = targetPath;
                TransHelper.WriteByte(datFullPath, allBytes);
                imageDatList.Clear();
                imageDatList = null;
                allBytes = null;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.Collect();
                GC.Collect();
            }

            return result;
        }

        /// <summary>
        /// 获取Dat中的单个瓦片图
        /// </summary>
        /// <param name="datPath">dat路径</param>
        /// <param name="level">层</param>
        /// <param name="x">x信息，也就是第几列</param>
        /// <param name="y">y信息，也就是第几行</param>
        public byte[] GetSingleTileImageFromDat(string datPath, int level, int x, int y)
        {
            byte[] tileImage = null;
            using (FileStream fs = new FileStream(datPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int datIndexLength = DatIndexStartPoint[level + 1] - DatIndexStartPoint[level];
                byte[] datIndexByte = new byte[datIndexLength];
                fs.Position = DatIndexStartPoint[level];
                fs.Read(datIndexByte, 0, datIndexByte.Length);
                string datIndexStr = TransHelper.Base64StringByteToString(datIndexByte);
                List<ImageDatIndex> datIndexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(datIndexStr);
                ImageDatIndex curIndex = datIndexList.FirstOrDefault(i => i.Level == level && i.MinX <= x && i.MaxX >= x);
                if (curIndex != null)
                {
                    byte[] imageDatByte = new byte[curIndex.Size];
                    fs.Position = curIndex.ByteIndex;
                    fs.Read(imageDatByte, 0, imageDatByte.Length);
                    string imageDatStr = TransHelper.Base64StringByteToString(imageDatByte);
                    List<ImageDat> imageDatList = JsonConvert.DeserializeObject<List<ImageDat>>(imageDatStr);
                    ImageDat imgDat = imageDatList.FirstOrDefault(i => i.Level == level && i.X == x && i.Y == y);
                    if (imgDat != null)
                    {
                        tileImage = new byte[imgDat.Size];
                        fs.Position = imgDat.ByteIndex;
                        fs.Read(tileImage, 0, tileImage.Length);
                    }
                }

                WaitFinish();
            }

            return tileImage;
        }

        /// <summary>
        /// 获取某一层，某个范围内的瓦片图
        /// </summary>
        /// <param name="datPath">Dat文件完整路径</param>
        /// <param name="level">第几层</param>
        /// <param name="startX">起始列，包含该值</param>
        /// <param name="endX">终止列，包含该值</param>
        /// <param name="startY">起始行，包含该值</param>
        /// <param name="endY">终止列，包含该值</param>
        public List<TileImage> GetTileImageListFromDat(string datPath, int level, int startX, int endX, int startY, int endY)
        {
            List<TileImage> result = new List<TileImage>();
            using (FileStream fs = new FileStream(datPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int datIndexLength = DatIndexStartPoint[level + 1] - DatIndexStartPoint[level];
                byte[] datIndexByte = new byte[datIndexLength];
                fs.Position = DatIndexStartPoint[level];
                fs.Read(datIndexByte, 0, datIndexByte.Length);
                string datIndexStr = TransHelper.Base64StringByteToString(datIndexByte);
                List<ImageDatIndex> datIndexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(datIndexStr);

                datIndexList = datIndexList.Where(i => i.Level == level && ((i.MaxX >= startX && i.MinX <= startX) || (i.MinX <= endX && i.MaxX >= endX))).ToList();
                // 不允许用多线程或者Parallel，因为fs.Position是变动的，会导致位置错乱
                datIndexList.ForEach(curIndex =>
                {
                    if (curIndex != null)
                    {
                        byte[] imageDatByte = new byte[curIndex.Size];
                        fs.Position = curIndex.ByteIndex;
                        fs.Read(imageDatByte, 0, imageDatByte.Length);
                        string imageDatStr = TransHelper.Base64StringByteToString(imageDatByte);
                        List<ImageDat> imageDatList = JsonConvert.DeserializeObject<List<ImageDat>>(imageDatStr);
                        imageDatList = imageDatList.Where(i => i.Level == level && i.X >= startX && i.X <= endX && i.Y >= startY && i.Y <= endY).ToList();
                        imageDatList.ForEach(imgDat =>
                        {
                            if (imgDat != null)
                            {
                                byte[] tileImage = new byte[imgDat.Size];
                                fs.Position = imgDat.ByteIndex;
                                fs.Read(tileImage, 0, tileImage.Length);
                                result.Add(new TileImage() { Level = level, X = imgDat.X, Y = imgDat.Y, ImageByte = tileImage });
                            }
                        });
                    }
                });

                datIndexByte = null;
                datIndexStr = "";
                datIndexList.Clear();
                datIndexList = null;
            }

            WaitFinish();
            return result;
        }

        /// <summary>
        /// 获取Dat文件中非瓦片图的其他文件数据
        /// </summary>
        /// <param name="datPath">Dat文件路径</param>
        /// <param name="otherFileName">要查找的文件名称，包含后缀信息，录入：Slide.dat</param>
        /// <returns>文件的二进制数据</returns>
        public byte[] GetOtherFileFromDat(string datPath, string otherFileName)
        {
            byte[] result = null;
            using (FileStream fs = new FileStream(datPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int datIndexLength = DatIndexStartPoint[1] - DatIndexStartPoint[-1];
                byte[] datIndexByte = new byte[datIndexLength];
                fs.Position = DatIndexStartPoint[-1];
                fs.Read(datIndexByte, 0, datIndexLength);
                string datIndexStr = TransHelper.Base64StringByteToString(datIndexByte);
                List<ImageDatIndex> datIndexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(datIndexStr);
                ImageDatIndex curIndex = datIndexList.FirstOrDefault(i => i.Level == -1);
                if (curIndex != null)
                {
                    byte[] imageDatByte = new byte[curIndex.Size];
                    fs.Position = curIndex.ByteIndex;
                    fs.Read(imageDatByte, 0, imageDatByte.Length);
                    string imageDatStr = TransHelper.Base64StringByteToString(imageDatByte);
                    List<ImageDat> imageDatList = JsonConvert.DeserializeObject<List<ImageDat>>(imageDatStr);
                    ImageDat imgDat = imageDatList.FirstOrDefault(i => i.Level == -1 && i.OtherFileName.Equals(otherFileName));
                    if (imgDat != null)
                    {
                        result = new byte[imgDat.Size];
                        fs.Position = imgDat.ByteIndex;
                        fs.Read(result, 0, result.Length);
                    }
                }

                datIndexByte = null;
                WaitFinish();
            }

            return result;
        }

        /// <summary>
        /// 解析dat文件成9层图
        /// </summary>
        /// <param name="datPath">dat文件的完整路径</param>
        /// <param name="targetPath">dat文件解析出来的文件所存放的地址</param>
        /// <returns>返回异常信息，如果未空则解析成功，如果不为空则解析失败</returns>
        public string ResolverDatFileToTileImage(string datPath, string targetPath = "")
        {
            string result = string.Empty;
            try
            {
                if (!File.Exists(datPath))
                {
                    return "未找到Dat文件";
                }

                if (string.IsNullOrEmpty(targetPath))
                {
                    targetPath = Path.GetDirectoryName(datPath) + "\\" + Path.GetFileNameWithoutExtension(datPath);
                }
                Directory.CreateDirectory(targetPath);

                byte[] allBytes = TransHelper.ReadByteFile(datPath);
                List<int> datIndexStartPointKeys = new List<int>(DatIndexStartPoint.Keys);
                // 解析出索引
                for(int i = 0; i < datIndexStartPointKeys.Count; i++)
                {
                    int level = datIndexStartPointKeys[i];
                    int startPoint = DatIndexStartPoint[level];
                    int nextLevelStartPoint = i == datIndexStartPointKeys.Count - 1 ? HeadIndexLength : DatIndexStartPoint[i + 1];
                    int levelLenght = nextLevelStartPoint - startPoint;

                    byte[] indexByte = new byte[levelLenght];
                    Buffer.BlockCopy(allBytes, startPoint, indexByte, 0, levelLenght);
                    string indexStr = TransHelper.Base64StringByteToString(indexByte);
                    List<ImageDatIndex> indexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(indexStr);
                    indexByte = null;
                    indexStr = null;

                    if(indexList == null)
                    {
                        continue;
                    }

                    indexList.ForEach(index =>
                    {
                        byte[] imageDatByte = new byte[index.Size];
                        Buffer.BlockCopy(allBytes, index.ByteIndex, imageDatByte, 0, imageDatByte.Length);
                        string imageDatStr = TransHelper.Base64StringByteToString(imageDatByte);
                        List<ImageDat> imageDatList = JsonConvert.DeserializeObject<List<ImageDat>>(imageDatStr);
                        byte[] imgByte;
                        string jpgPath = string.Empty;
                        imageDatList.ForEach(x =>
                        {
                            imgByte = new byte[x.Size];
                            Buffer.BlockCopy(allBytes, x.ByteIndex, imgByte, 0, x.Size);
                            if (x.Level != -1)
                            {
                                jpgPath = targetPath + $"\\{x.Level}\\{x.X}\\{x.Y}.jpg";
                                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(jpgPath));
                                TransHelper.SetByteToImageNoReturn(imgByte, jpgPath);
                            }
                            else
                            {
                                // 非瓦片图文件
                                jpgPath = targetPath + $"\\{x.OtherFileName}";
                                TransHelper.WriteFileFromByte(imgByte, jpgPath);
                            }
                            imgByte = null;
                            jpgPath = null;
                        });

                        imageDatList.Clear();
                        imageDatList = null;
                        imageDatByte = null;
                    });

                    indexList.Clear();
                    indexList = null;
                }

                allBytes = null;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.Collect();
                GC.Collect();
            }

            return result;
        }

        public void WaitFinish()
        {
            System.Threading.Thread.Sleep(25);
        }

        /// <summary>
        /// 获取Dat文件中的所有图片信息
        /// </summary>
        /// <param name="datPath">Dat文件路径</param>
        public List<TileImage> GetAllTileImage(string datPath)
        {
            ConcurrentBag<TileImage> rest = new ConcurrentBag<TileImage>();
            byte[] allByte;
            using (FileStream fs = new FileStream(datPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                allByte = new byte[fs.Length];
                fs.Read(allByte, 0, allByte.Length);
            }
            
            List<int> datIndexStartPointKeys = new List<int>(DatIndexStartPoint.Keys);
            // 解析出索引
            for (int i = 0; i < datIndexStartPointKeys.Count; i++)
            {
                int level = datIndexStartPointKeys[i];
                int startPoint = DatIndexStartPoint[level];
                int nextLevelStartPoint = i == datIndexStartPointKeys.Count - 1 ? HeadIndexLength : DatIndexStartPoint[i + 1];
                int levelLenght = nextLevelStartPoint - startPoint;

                byte[] indexByte = new byte[levelLenght];
                Buffer.BlockCopy(allByte, startPoint, indexByte, 0, levelLenght);
                string indexStr = TransHelper.Base64StringByteToString(indexByte);
                List<ImageDatIndex> indexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(indexStr);
                indexByte = null;
                indexStr = null;

                if (indexList == null)
                {
                    continue;
                }

                Parallel.ForEach<ImageDatIndex>(indexList, index =>
                {
                    byte[] imageDatByte = new byte[index.Size];
                    Buffer.BlockCopy(allByte, index.ByteIndex, imageDatByte, 0, imageDatByte.Length);
                    string imageDatStr = TransHelper.Base64StringByteToString(imageDatByte);
                    List<ImageDat> imageDatList = JsonConvert.DeserializeObject<List<ImageDat>>(imageDatStr);

                    Parallel.ForEach<ImageDat>(imageDatList, x =>
                    {
                        x.ImageByte = new byte[x.Size];
                        Buffer.BlockCopy(allByte, x.ByteIndex, x.ImageByte, 0, x.Size);
                        rest.Add(new TileImage() { Level = x.Level, X = x.X, Y = x.Y, ImageByte = x.ImageByte });
                    });

                    imageDatList.Clear();
                    imageDatList = null;
                    imageDatByte = null;
                    imageDatStr = null;
                });

                indexList.Clear();
                indexList = null;
            }

            allByte = null;
            datIndexStartPointKeys.Clear();
            datIndexStartPointKeys = null;
            return rest.ToList(); ;
        }
    }
}
