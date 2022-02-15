using DST.Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ExtendContext
{
    public class DZIConstant
    {
        /// <summary>
        /// DZI存放图片位置
        /// </summary>
        public static string DZIFilesDir { get; } = "9Levels_files";
        /// <summary>
        /// 导航图片文件夹名称
        /// </summary>
        public static string NavImgLevel { get; } = "8";
        /// <summary>
        /// 导航图片名称
        /// </summary>
        public static string NavImgName { get; } = "0_0.jpg";
        /// <summary>
        /// 瓦片大小
        /// </summary>
        public static int DZIImgSzie { get; } = 1024;
        /// <summary>
        /// 总宽度
        /// </summary>
        public static double DZIImgMaxHeight { get; } = Math.Pow(2, 16);
        /// <summary>
        /// 总高度
        /// </summary>
        public static double DZIImgMaxWidth { get; } = Math.Pow(2, 16);
        /// <summary>
        /// 最小层数显示
        /// </summary>
        public static int DZIMinLevel { get; } = 6;

        public static string SlideFile { get; } = "Slide.ini";
        /// <summary>
        /// 二维码图片
        /// </summary>
        private static string QCodeName { get; } = "AL_BARCODE.jpg";

        /// <summary>
        /// 是否包含17个关键文件夹
        /// </summary>
        private static readonly List<string> _dirStdList = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };

        /// <summary>
        /// 判断是否是切片目录
        /// </summary>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public static bool IsSliceDir(string baseDir)
        {
            var isSilceDir = false;
            //Func<DirectoryInfo, IEnumerable<DirectoryInfo>> func = dir => dir?.EnumerateDirectories();
            if (!string.IsNullOrEmpty(baseDir))
            {
                var baseDirInfo = new DirectoryInfo(baseDir);
                var dziDir = baseDirInfo.EnumerateDirectories().FirstOrDefault(d => d.Name == DZIFilesDir);
                if (dziDir != null)
                {
                    //var sildeFile = baseDirInfo.EnumerateFiles().FirstOrDefault(f => f.Name == slideDatName); // 是否包含Slide.dat文件
                    //if (sildeFile != null)
                    //{
                    var sileCount = dziDir.EnumerateDirectories().Join(_dirStdList, b => b.Name, s => s, (b, s) => b).ToList();
                    isSilceDir = sileCount.Count == _dirStdList.Count; // 是否包含0-16目录
                                                                       //}
                }
            }
            return isSilceDir;
        }
        /// <summary>
        /// 获取导航图片
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public static FileInfo GetNavImg(DirectoryInfo directoryInfo)
        {
            FileInfo file = null;
            if (directoryInfo != null && Directory.Exists(directoryInfo.FullName))
            {
                file = directoryInfo.EnumerateDirectories().FirstOrDefault(d => d.Name == DZIFilesDir)?
                                    .EnumerateDirectories()?.FirstOrDefault(d => d.Name == NavImgLevel)?
                                    .GetFiles()?.FirstOrDefault(f => f.Name == NavImgName);
            }
            return file;
        }
        /// <summary>
        /// 获取二维码文件
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public static FileInfo GetQCodeImg(DirectoryInfo directoryInfo)
        {
            FileInfo file = null;
            if (directoryInfo != null && Directory.Exists(directoryInfo.FullName))
            {
                file = directoryInfo.EnumerateFiles().FirstOrDefault(f => f.Name == QCodeName);
            }
            return file;
        }

        public static string GetImgFilePath(int tileLevel, int tilePositionX, int tilePositionY) => $"{tileLevel}\\{tilePositionX}_{tilePositionY}.jpg";

        /// <summary>
        /// 根据路径获取max尺寸
        /// </summary>
        /// <param name="samplePath"></param>
        /// <returns></returns>
        public static (double width, double height) GetDZISize(string samplePath)
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
