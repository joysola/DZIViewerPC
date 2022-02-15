
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.Common.Helper
{
    public static class PointExtensions
    {
        private static readonly double picWidth = 4096;
        private static readonly double picHeight = 3072;
        private static double radio = 3.125; // 其他坐标到我们坐标的放大比例
        /// <summary>
        /// 将canvas坐标转换成中台坐标
        /// </summary>
        /// <param name="point"></param>
        /// <param name="canvasSize"></param>
        /// <returns></returns>
        public static string ShowOtherCoordination(this Point point, Size canvasSize)
        {
            if (radio != 3.125)
            {
                radio = picWidth / canvasSize.Width;
            }
            var result = $"({point.X * radio:F0},{picHeight - point.Y * radio:F0})"; // F0浮点没有小数
            return result;
        }
        /// <summary>
        /// 从中台的坐标转换成canvas坐标
        /// </summary>
        /// <param name="otherPoint"></param>
        /// <returns></returns>
        public static Point GetCansPointfromOtherPoint(this Point otherPoint)
        {
            var result = new Point(otherPoint.X / radio, (picHeight - otherPoint.Y) / radio);
            return result;
        }
        /// <summary>
        /// 从canvas坐标转成中台坐标
        /// </summary>
        /// <param name="cansPoint"></param>
        /// <returns></returns>
        public static Point GetOtherPointfromCansPoint(this Point cansPoint)
        {
            var result = new Point(cansPoint.X * radio, picHeight - cansPoint.Y * radio);
            return result;
        }

        /// <summary>
        /// 原始的坐标按倍率转换
        /// </summary>
        /// <param name="sourcePoint">原始坐标</param>
        /// <param name="isTransLocal">true=中台转为本地， false=本地转中台</param>
        /// <returns>新坐标</returns>
        public static Point TransPointByRadio(this Point sourcePoint, bool isTransLocal)
        {
            if (isTransLocal)
            {
                return new Point(sourcePoint.X / radio, sourcePoint.Y / radio);
            }
            else
            {
                return new Point(sourcePoint.X * radio, sourcePoint.Y * radio);
            }
        }
    }
}
