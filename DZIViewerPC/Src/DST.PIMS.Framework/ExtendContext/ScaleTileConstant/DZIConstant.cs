using DST.Common.Helper;
using DST.Database.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ExtendContext
{
    public class DZIConstant : IDZIConstant
    {
        public static DZIConstant Cons { get; } = new DZIConstant();
        /// <summary>
        /// DZI存放图片位置
        /// </summary>
        public string DZIFilesDir { get; } = "9Levels_files";
        /// <summary>
        /// 导航图片文件夹名称
        /// </summary>
        public string NavImgLevel { get; } = "8";
        /// <summary>
        /// 导航图片名称
        /// </summary>
        public string NavImgName { get; } = "0_0.jpg";
        /// <summary>
        /// 瓦片大小
        /// </summary>
        public int DZITileSzie { get; } = 1024;
        /// <summary>
        /// 总宽度
        /// </summary>
        public double DZIImgMaxHeight { get; } = Math.Pow(2, 16);
        /// <summary>
        /// 总高度
        /// </summary>
        public double DZIImgMaxWidth { get; } = Math.Pow(2, 16);
        /// <summary>
        /// 最小层数显示
        /// </summary>
        public int DZIMinLevel { get; } = 6;

        public string SlideFile { get; } = "Slide.ini";
        /// <summary>
        /// 二维码图片
        /// </summary>
        private string QCodeName { get; } = "AL_BARCODE.jpg";

        /// <summary>
        /// 是否包含17个关键文件夹
        /// </summary>
        private readonly List<string> _dirStdList = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };

        /// <summary>
        /// 判断是否是切片目录
        /// </summary>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public bool IsSlice(string baseDir)
        {
            var isSilceDir = false;
            //Func<DirectoryInfo, IEnumerable<DirectoryInfo>> func = dir => dir?.EnumerateDirectories();
            if (!string.IsNullOrEmpty(baseDir))
            {
                var baseDirInfo = new DirectoryInfo(baseDir);
                var dziDir = baseDirInfo.EnumerateDirectories().FirstOrDefault(d => d.Name == DZIFilesDir);
                var dziIniFile = baseDirInfo.EnumerateFiles().FirstOrDefault(f => f.Name == SlideFile);
                isSilceDir = dziDir != null && dziIniFile != null;
                //if (dziDir != null)
                //{
                //var sildeFile = baseDirInfo.EnumerateFiles().FirstOrDefault(f => f.Name == slideDatName); // 是否包含Slide.dat文件
                //if (sildeFile != null)
                //{
                //var sileCount = dziDir.EnumerateDirectories().Join(_dirStdList, b => b.Name, s => s, (b, s) => b).ToList();
                //isSilceDir = sileCount.Count == _dirStdList.Count; // 是否包含0-16目录
                //}
                //}
            }
            return isSilceDir;
        }


        public List<ImgViewFileInfo> CreateImgViewTileList(IEnumerable<FileSystemInfo> infos)
        {
            var result = new List<ImgViewFileInfo>();
            foreach (var fsi in infos)
            {
                //var file = x.EnumerateDirectories().FirstOrDefault(d => d.Name == "1")?.EnumerateDirectories()?.FirstOrDefault(d => d.Name == "0")?
                //                                    .GetFiles()?.FirstOrDefault(f => f.Name == "0.jpg");
                var img = new ImgViewFileInfo
                {
                    QCodeImgUrl = GetQCodeImg(fsi.FullName),
                    SampleImgUrl = GetNavImg(fsi.FullName),
                    LocalFilePath = fsi.FullName,
                    DicectoryName = fsi.Name,
                };
                result.Add(img);
            }
            return result;
        }

        /// <summary>
        /// 获取导航图片
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public byte[] GetNavImg(string directory)
        {
            byte[] bytes = null;
            if (directory != null)
            {
                var directoryInfo = new DirectoryInfo(directory);
                if (directoryInfo.Exists)
                {
                    var file = directoryInfo.EnumerateDirectories().FirstOrDefault(d => d.Name == DZIFilesDir)?
                                         .EnumerateDirectories()?.FirstOrDefault(d => d.Name == NavImgLevel)?
                                         .GetFiles()?.FirstOrDefault(f => f.Name == NavImgName);
                    if (file != null)
                    {
                        bytes = new byte[file.Length];
                        using var fs = file.OpenRead();
                        fs.Read(bytes, 0, bytes.Length);
                    }
                }
            }
            return bytes;
        }

        /// <summary>
        /// 获取二维码文件
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public byte[] GetQCodeImg(string directory)
        {
            byte[] bytes = null;
            if (directory != null)
            {
                var directoryInfo = new DirectoryInfo(directory);
                if (directoryInfo.Exists)
                {
                    var file = directoryInfo.EnumerateFiles().FirstOrDefault(f => f.Name == QCodeName);
                    if (file != null)
                    {
                        bytes = new byte[file.Length];
                        using var fs = file.OpenRead();
                        fs.Read(bytes, 0, bytes.Length);
                    }
                }
            }
            return bytes;
        }

        public string GetTilePath(int tileLevel, int tilePositionX, int tilePositionY) => $"{tileLevel}\\{tilePositionX}_{tilePositionY}.jpg";

        /// <summary>
        /// 根据路径获取max尺寸
        /// </summary>
        /// <param name="samplePath"></param>
        /// <returns></returns>
        public (double width, double height) GetDZISize(string samplePath)
        {
            var ini = IniHelper.CreateInstance($"{samplePath}\\{SlideFile}");
            if (double.TryParse(ini.IniReadValue("Data", "StitchWidth"), out double width) && double.TryParse(ini.IniReadValue("Data", "StitchHeight"), out double height))
            {
                return (width, height);
            }
            return (DZIImgMaxWidth, DZIImgMaxHeight);
        }


    }
}
