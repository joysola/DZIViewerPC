using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ExtendContext
{
    public class MSIContext
    {
        /// <summary>
        /// msi的图像与显示的参数
        /// </summary>
        public static double ImgScrnParam { get; } = 0.57895351358564218;
        /// <summary>
        /// 倍率相关参数（9层图目前是1）
        /// </summary>
        public static int SlideZoom { get; } = 1;
        /// <summary>
        /// 实际距离和像素参数关系
        /// </summary>
        public static double Calibration { get;} = 0.24;
        /// <summary>
        /// 报告视野尺寸
        /// </summary>
        public static int RprtImgSize { get; } = 398;
        /// <summary>
        /// 标记缩略图
        /// </summary>
        public static int ThumbImgSize { get; } = 90;
        /// <summary>
        /// 缩放最大倍率
        /// </summary>
        public static int MaxScale { get; } = 50;
    }
}
