using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 经纬度坐标算法
    /// </summary>
    public class DSTReviewScanImg
    {
        public static decimal EQUATORIAL_CIRCUMFERENCE { get; } = 40075016.68557849m;
        public static decimal MERCATOR_MIN { get; } = -20037508.342789244m;
        public static decimal MERCATOR_MAX { get; } = 20037508.342789244m;
        //1, 2, 3, 4, 5, 8, 10, 20, 40
        //public static Dictionary<int, (double s, int length)> SparamDict { get; } = new Dictionary<int, (double, int)>
        //{
        //    {1, (20.0,      256 * (int)Math.Pow(2,0)) },      // 1
        //    {2, (10.0,      256 * (int)Math.Pow(2,1)) },      // 2
        //    {3, (20.0/3.0,  256 * (int)Math.Pow(2,2)) },      // 3
        //    {4, (2.5,       256 * (int)Math.Pow(2,3)) },      // 4
        //    {5, (4.0,       256 * (int)Math.Pow(2,4)) },      // 5
        //    {6, (2.5,       256 * (int)Math.Pow(2,5)) },      // 8
        //    {7, (2.0,       256 * (int)Math.Pow(2,6)) },      // 10
        //    {8, (1.0,       256 * (int)Math.Pow(2,8)) },      // 20
        //    {9, (0.5,       256 * (int)Math.Pow(2,8)) },      // 40
        //};

        //x =  (xPixel* s / widthPixel ) * EQUATORIAL_CIRCUMFERENCE + MERCATOR_MIN
        /// <summary>
        /// 经纬度转图像坐标
        /// </summary>
        /// <param name="latlon"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static decimal GetPixel(decimal latlon, int level = 8)
        {
            var result = (latlon - MERCATOR_MIN) / EQUATORIAL_CIRCUMFERENCE * 65536.0m / 1.0m;
            return result;
        }
        /// <summary>
        /// 经纬度转图像横坐标
        /// </summary>
        /// <param name="latlon"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static decimal GetPixelX(decimal latlon, int level = 8)
        {
            var result = GetPixel(latlon, level);
            return result;
        }
        /// <summary>
        /// 经纬度转图像纵坐标
        /// </summary>
        /// <param name="latlon"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static decimal GetPixelY(decimal latlon, int level = 8)
        {
            var result = GetPixel(-latlon, level);
            return result;
        }
    }
}
