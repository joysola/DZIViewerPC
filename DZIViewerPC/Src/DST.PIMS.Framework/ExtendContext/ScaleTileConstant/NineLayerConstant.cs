using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ExtendContext
{
    public class NineLayerConstant
    {
        /// <summary>
        /// 导航图片文件夹等级
        /// </summary>
        public static string NavImgLevel { get; } = "1";
        /// <summary>
        /// 导航图片文件夹名称
        /// </summary>
        public static string NavImgDir { get; } = "0";
        /// <summary>
        /// 导航图片名称
        /// </summary>
        public static string NavImgName { get; } = "0.jpg";
        /// <summary>
        /// 瓦片大小
        /// </summary>
        public static int NLImgSzie { get; } = 256;
        /// <summary>
        /// 总宽度
        /// </summary>
        public static double NLImgMaxHeight { get; } = Math.Pow(2, 16);
        /// <summary>
        /// 总高度
        /// </summary>
        public static double NLImgMaxWidth { get; } = Math.Pow(2, 16);
        /// <summary>
        /// 最小层数显示
        /// </summary>
        public static int NLMinLevel { get; } = 8;

        public static string SlideFile { get; } = "Slide.dat";
        /// <summary>
        /// 二维码图片
        /// </summary>
        private static string QCodeName { get; } = "AL_BARCODE.jpg";

        /// <summary>
        /// 是否包含17个关键文件夹
        /// </summary>
        private static readonly List<string> _dirStdList = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        /// <summary>
        /// 判断是否是切片目录
        /// </summary>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public static bool IsSliceDir(string baseDir)
        {
            var isSilceDir = false;
            if (!string.IsNullOrEmpty(baseDir))
            {
                var baseDirInfo = new DirectoryInfo(baseDir);
                var sildeFile = baseDirInfo.EnumerateFiles().FirstOrDefault(f => f.Name == SlideFile); // 是否包含Slide.dat文件
                if (sildeFile != null)
                {
                    var sileCount = baseDirInfo.EnumerateDirectories().Join(_dirStdList, b => b.Name, s => s, (b, s) => b).ToList();
                    isSilceDir = sileCount.Count == _dirStdList.Count; // 是否包含1-9目录
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
                file = directoryInfo.EnumerateDirectories().FirstOrDefault(d => d.Name == NavImgLevel)?
                   .EnumerateDirectories()?.FirstOrDefault(d => d.Name == NavImgDir)?
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
        public static string GetImgFilePath(int tileLevel, int tilePositionX, int tilePositionY) => $"{tileLevel}\\{tilePositionX}\\{tilePositionY}.jpg";
    }
}
