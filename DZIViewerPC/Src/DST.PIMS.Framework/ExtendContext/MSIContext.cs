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
        public static double Calibration { get; } = 0.24;
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
        public static int MaxScale { get; } = 42;

        private static readonly double[] m_magV = new double[] {320.0, 128.0, 60.0, 40.0, 30.0, 20.0, 10.0, 8.0, 5.0, 2.0, 1.5, 0.0 };
        private static readonly double[] m_adjV = new double[] {10.0,  5.0,   4.0,  3.0,  2.0,  1.5,  1.0,  0.7, 0.5, 0.4, 0.3, 0.2 };

        public static double GetAdjustSplitScaleByCurScale(double curscale, int timeSpan)
        {
            double result = 0.0;
            for (int i = 0; i < 11; i++)
            {
                double num = m_magV[i + 1];
                double num2 = m_magV[i];
                double y = m_adjV[i + 1];
                double y2 = m_adjV[i];
                if (curscale >= num && curscale <= num2)
                {
                    result = CalcFixValueY(num, num2, y, y2, curscale);
                    break;
                }
            }
            return result * CalcSpeed(timeSpan);
        }

        private static double CalcFixValueY(double secLess, double secGreat, double stepLess, double stepGreat, double curScale)
        {
            double secLength = secGreat - secLess;
            double num2 = secGreat - curScale;
            double stepLength = stepGreat - stepLess;
            double num4 = stepLength * num2 / (1.0 * secLength);
            return stepGreat - num4;
        }
        private static double CalcSpeed(int timeDelta)
        {
            double num2 = 3.0;
            double num3 = 1.0;
            double num4 = 80.0;
            double num;
            if (timeDelta <= 0)
            {
                num = num2;
            }
            else if ((double)timeDelta >= num4)
            {
                num = num3;
            }
            else
            {
                num = (num4 - (double)timeDelta) * (num2 - num3) / num4 + num3;
                if (num < num3)
                {
                    num = num3;
                }
                if (num > num2)
                {
                    num = num2;
                }
            }
            return num;
        }
    }
}
