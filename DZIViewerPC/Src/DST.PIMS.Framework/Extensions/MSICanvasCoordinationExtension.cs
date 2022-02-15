using DST.PIMS.Framework.Controls;
using Nico.DeepZoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.PIMS.Framework.Extensions
{
    public static class MSICanvasCoordinationExtension
    {
        #region AnnoBase坐标转换扩展方法
        /// <summary>
        /// Canvas坐标转Msi图像坐标
        /// </summary>
        /// <param name="annoBase"></param>
        /// <param name="canPiont"></param>
        /// <returns></returns>
        public static Point CanvasToMsi(this AnnoBase annoBase, Point canPiont)
        {
            Point offset = annoBase.MSI.ZoomableCanvas.Offset;
            Point result = default(Point);
            result.X = (canPiont.X + offset.X) / (annoBase.MSI.ZoomableCanvas.Scale * annoBase.SlideZoom);
            result.Y = (canPiont.Y + offset.Y) / (annoBase.MSI.ZoomableCanvas.Scale * annoBase.SlideZoom);
            return result;
        }
        /// <summary>
        /// Msi图像坐标转Canvas坐标
        /// </summary>
        /// <param name="annoBase"></param>
        /// <param name="msiPiont"></param>
        /// <returns></returns>
        public static Point MsiToCanvas(this AnnoBase annoBase, Point msiPiont)
        {
            Point offset = annoBase.MSI.ZoomableCanvas.Offset;
            Point result = default(Point);
            result.X = msiPiont.X * (annoBase.MSI.ZoomableCanvas.Scale * annoBase.SlideZoom) - offset.X;
            result.Y = msiPiont.Y * (annoBase.MSI.ZoomableCanvas.Scale * annoBase.SlideZoom) - offset.Y;
            return result;
        }
        #endregion AnnoBase坐标转换扩展方法

        #region Point坐标转换扩展方法
        /// <summary>
        /// Canvas坐标转Msi图像坐标
        /// </summary>
        /// <param name="canPiont"></param>
        /// <param name="msi"></param>
        /// <param name="slideZoom"></param>
        /// <returns></returns>
        public static Point CanvasToMsi(this Point canPiont, MultiScaleImage msi, int slideZoom = 1)
        {
            Point offset = msi.ZoomableCanvas.Offset;
            Point result = default;
            result.X = (canPiont.X + offset.X) / (msi.ZoomableCanvas.Scale * slideZoom);
            result.Y = (canPiont.Y + offset.Y) / (msi.ZoomableCanvas.Scale * slideZoom);
            return result;
        }
        /// <summary>
        /// Msi图像坐标转Canvas坐标
        /// </summary>
        /// <param name="msiPiont"></param>
        /// <param name="msi"></param>
        /// <param name="slideZoom"></param>
        /// <returns></returns>
        public static Point MsiToCanvas(this Point msiPiont, MultiScaleImage msi, int slideZoom = 1)
        {
            Point offset = msi.ZoomableCanvas.Offset;
            Point result = default;
            result.X = msiPiont.X * (msi.ZoomableCanvas.Scale * slideZoom) - offset.X;
            result.Y = msiPiont.Y * (msi.ZoomableCanvas.Scale * slideZoom) - offset.Y;
            return result;
        }
        // 旋转中心是(0,0)时的坐标旋转公式
        // x1 = cos(angle) * x - sin(angle) * y;
        // y1 = cos(angle) * y + sin(angle) * x;
        /// <summary>
        /// 坐标旋转公式
        /// </summary>
        /// <param name="strat">旋转起点坐标</param>
        /// <param name="center">旋转中心坐标</param>
        /// <param name="angle">旋转角度</param>
        /// <returns></returns>
        public static Point GetRotatePoint(this Point strat, Point center, double angle)
        {
            Point result = default;
            double radian = angle * 2 * Math.PI / 360.0; // 度数转弧度
            // strat.X - center.X 是 实际旋转的 投影x轴的距离
            result.X = (strat.X - center.X) * Math.Cos(radian) + (strat.Y - center.Y) * Math.Sin(radian) + center.X;
            result.Y = (strat.Y - center.Y) * Math.Cos(radian) - (strat.X - center.X) * Math.Sin(radian) + center.Y;
            return result;
        }
        #endregion Point坐标转换扩展方法


    }
}
