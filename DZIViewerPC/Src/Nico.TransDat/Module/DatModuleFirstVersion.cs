using Newtonsoft.Json;
using Nico.TransDat.Base;
using Nico.TransDat.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nico.TransDat.Module
{
    /// <summary>
    /// Dat文件生成和解析的第一版本
    /// </summary>
    internal class DatModuleFirstVersion : BaseDatModule, IBaseDatModule
    {
        /// <summary>
        /// 当前版本信息
        /// </summary>
        public string Version { get => "V1.0.0.0"; set => throw new NotImplementedException(); }

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

                if(File.Exists(targetPath))
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

                // 小于等于5层的头数据
                List<ImageDat> fiveLevel = imageDatList.Where(x => x.Level <= 5).ToList();
                minX = fiveLevel.Min(t => t.X);
                maxX = fiveLevel.Max(t => t.X);
                string fiveLevelStr = JsonConvert.SerializeObject(fiveLevel);
                byte[] fiveLevelByte = TransHelper.StringToBase64StringByte(fiveLevelStr);
                Buffer.BlockCopy(fiveLevelByte, 0, allBytes, imageDatByteIndex, fiveLevelByte.Length);
                datIndexList.Add(new ImageDatIndex() { Level = 5, ByteIndex = imageDatByteIndex, MinX = minX, MaxX = maxX, Size = fiveLevelByte.Length });
                imageDatByteIndex += fiveLevelByte.Length;

                // 第6层数据
                List<ImageDat> sixLevel = imageDatList.Where(x => x.Level == 6).ToList();
                minX = sixLevel.Min(t => t.X);
                maxX = sixLevel.Max(t => t.X);
                string sixLevelStr = JsonConvert.SerializeObject(sixLevel);
                byte[] sixLevelByte = TransHelper.StringToBase64StringByte(sixLevelStr);
                Buffer.BlockCopy(sixLevelByte, 0, allBytes, imageDatByteIndex, sixLevelByte.Length);
                datIndexList.Add(new ImageDatIndex() { Level = 6, ByteIndex = imageDatByteIndex, MinX = minX, MaxX = maxX, Size = sixLevelByte.Length });
                imageDatByteIndex += sixLevelByte.Length;

                // 第7层数据，拆分成2个Iindex
                int indexSplit = 33;
                int xCount = imageDatList.Where(t => t.Level == 7).Max(t => t.X) + 1;
                int indexCount = (int)Math.Ceiling(1.0 * xCount / indexSplit);
                for (int i = 0; i < indexCount; i++)
                {
                    minX = i * indexSplit;
                    maxX = (i + 1) * indexSplit;
                    List<ImageDat> sevenLevel = imageDatList.Where(x => x.Level == 7 && x.X <= maxX && x.X >= minX).ToList();
                    string sevenLevelOneStr = JsonConvert.SerializeObject(sevenLevel);
                    byte[] sevenLevelOneByte = TransHelper.StringToBase64StringByte(sevenLevelOneStr);
                    Buffer.BlockCopy(sevenLevelOneByte, 0, allBytes, imageDatByteIndex, sevenLevelOneByte.Length);
                    datIndexList.Add(new ImageDatIndex() { Level = 7, ByteIndex = imageDatByteIndex, MinX = minX, MaxX = maxX, Size = sevenLevelOneByte.Length });
                    imageDatByteIndex += sevenLevelOneByte.Length;
                }

                // 第8层数据，拆分成4个index
                indexSplit = 33;
                xCount = imageDatList.Where(t => t.Level == 8).Max(t => t.X) + 1;
                indexCount = (int)Math.Ceiling(1.0 * xCount / indexSplit);
                for (int i = 0; i < indexCount; i++)
                {
                    minX = i * indexSplit;
                    maxX = (i + 1) * indexSplit;
                    List<ImageDat> eightLevel = imageDatList.Where(x => x.Level == 8 && x.X <= maxX && x.X >= minX).ToList();
                    string eightLevelStr = JsonConvert.SerializeObject(eightLevel);
                    byte[] eightLevelByte = TransHelper.StringToBase64StringByte(eightLevelStr);
                    Buffer.BlockCopy(eightLevelByte, 0, allBytes, imageDatByteIndex, eightLevelByte.Length);
                    datIndexList.Add(new ImageDatIndex() { Level = 8, ByteIndex = imageDatByteIndex, MinX = minX, MaxX = maxX, Size = eightLevelByte.Length });
                    imageDatByteIndex += eightLevelByte.Length;
                }

                // 第9层数据，拆分成16个index
                indexSplit = 17;
                xCount = imageDatList.Where(t => t.Level == 9).Max(t => t.X) + 1;
                indexCount = (int)Math.Ceiling(1.0 * xCount / indexSplit);
                for (int i = 0; i < indexCount; i++)
                {
                    minX = i * indexSplit;
                    maxX = (i + 1) * indexSplit;
                    List<ImageDat> nightLevel = imageDatList.Where(x => x.Level == 9 && x.X <= maxX && x.X >= minX).ToList();
                    string nightLevelStr = JsonConvert.SerializeObject(nightLevel);
                    byte[] nightLevelByte = TransHelper.StringToBase64StringByte(nightLevelStr);
                    Buffer.BlockCopy(nightLevelByte, 0, allBytes, imageDatByteIndex, nightLevelByte.Length);
                    datIndexList.Add(new ImageDatIndex() { Level = 9, ByteIndex = imageDatByteIndex, MinX = minX, MaxX = maxX, Size = nightLevelByte.Length });
                    imageDatByteIndex += nightLevelByte.Length;
                }

                // 索引数据
                string datIndex = JsonConvert.SerializeObject(datIndexList);
                byte[] datIndexByte = TransHelper.StringToBase64StringByte(datIndex);

                // 写入索引，从版本信息的位置
                Buffer.BlockCopy(datIndexByte, 0, allBytes, VersionIndexLength, datIndexByte.Length);

                // 写入版本信息
                int width = (imageDatList.Where(x => x.Level == 9).Max(t => t.X) + 1) * 256;
                int height = (imageDatList.Where(x => x.Level == 9).Max(t => t.Y) + 1) * 256;
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
                int tempLevel = level <= 5 ? 5 : level; // 1-5层是合并在一起的
                byte[] datIndexByte = new byte[HeadIndexLength - VersionIndexLength];
                fs.Position = VersionIndexLength;
                fs.Read(datIndexByte, 0, datIndexByte.Length);
                string datIndexStr = TransHelper.Base64StringByteToString(datIndexByte);
                List<ImageDatIndex> datIndexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(datIndexStr);
                ImageDatIndex curIndex = datIndexList.FirstOrDefault(i => i.Level == tempLevel && i.MinX <= x && i.MaxX >= x);
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
                int tempLevel = level <= 5 ? 5 : level; // 1-5层是合并在一起的
                byte[] datIndexByte = new byte[HeadIndexLength - VersionIndexLength];
                fs.Position = VersionIndexLength;
                fs.Read(datIndexByte, 0, datIndexByte.Length);
                string datIndexStr = TransHelper.Base64StringByteToString(datIndexByte);
                List<ImageDatIndex> datIndexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(datIndexStr);

                datIndexList = datIndexList.Where(i => i.Level == tempLevel && ((i.MinX <= startX && i.MaxX >= startX) || (i.MinX <= endX && i.MaxX >= endX))).ToList();
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
                byte[] datIndexByte = new byte[HeadIndexLength - VersionIndexLength];
                fs.Position = VersionIndexLength;
                fs.Read(datIndexByte, 0, datIndexByte.Length);
                string datIndexStr = TransHelper.Base64StringByteToString(datIndexByte);
                List<ImageDatIndex> datIndexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(datIndexStr);
                ImageDatIndex curIndex = datIndexList.FirstOrDefault(i => i.Level == 5);
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
                byte[] datIndexByte = new byte[HeadIndexLength - VersionIndexLength];
                Buffer.BlockCopy(allBytes, VersionIndexLength, datIndexByte, 0, datIndexByte.Length);
                string datIndexStr = TransHelper.Base64StringByteToString(datIndexByte);
                List<ImageDatIndex> datIndexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(datIndexStr);
                datIndexList.ForEach(i =>
                {
                    byte[] imageDatByte = new byte[i.Size];
                    Buffer.BlockCopy(allBytes, i.ByteIndex, imageDatByte, 0, imageDatByte.Length);
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
                });

                allBytes = null;
            }
            catch(Exception ex)
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
        /// <param name="isLoadImageByte">是否加载图片二进制</param>
        public List<TileImage> GetAllTileImage(string datPath)
        {
            List<TileImage> result = new List<TileImage>(100000);
            byte[] allByte;
            byte[] imageDatIndexByte;
            using (FileStream fs = new FileStream(datPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                allByte = new byte[fs.Length];
                fs.Read(allByte, 0, allByte.Length);
            }

            imageDatIndexByte = new byte[HeadIndexLength - VersionIndexLength];
            Buffer.BlockCopy(allByte, VersionIndexLength, imageDatIndexByte, 0, imageDatIndexByte.Length);
            string imageDatIndexStr = TransHelper.Base64StringByteToString(imageDatIndexByte);
            List<ImageDatIndex> datIndexList = JsonConvert.DeserializeObject<List<ImageDatIndex>>(imageDatIndexStr);
            List<Task> tasks = new List<Task>();
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            datIndexList.ForEach(datIndex =>
            {
                tasks.Add(Task.Factory.StartNew((tt) => 
                {
                    ImageDatIndex tmpDatIndex = tt as ImageDatIndex;
                    byte[] imageDatByte = new byte[tmpDatIndex.Size];
                    Buffer.BlockCopy(allByte, tmpDatIndex.ByteIndex, imageDatByte, 0, imageDatByte.Length);
                    string imageDatStr = TransHelper.Base64StringByteToString(imageDatByte);
                    List<ImageDat> imageDatList = JsonConvert.DeserializeObject<List<ImageDat>>(imageDatStr);
                    List<TileImage> tmpResult = new List<TileImage>(imageDatList.Count);
                    imageDatList.ForEach(imgDat =>
                    {
                        imgDat.ImageByte = new byte[imgDat.Size];
                        Buffer.BlockCopy(allByte, imgDat.ByteIndex, imgDat.ImageByte, 0, imgDat.Size);
                        tmpResult.Add(new TileImage() { Level = imgDat.Level, X = imgDat.X, Y = imgDat.Y, ImageByte = imgDat.ImageByte });
                    });
                    imageDatList.Clear();
                    imageDatList = null;
                    result.AddRange(tmpResult);
                }, datIndex, token));
            });

            Task.WaitAll(tasks.ToArray());
            cts.Dispose();
            datIndexList.Clear();
            datIndexList = null;
            imageDatIndexStr = null;
            allByte = null;
            imageDatIndexByte = null;
            tasks.Clear();
            tasks = null;
            return result;
        }
    }
}
