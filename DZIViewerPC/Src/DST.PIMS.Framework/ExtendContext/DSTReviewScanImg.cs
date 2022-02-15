using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ExtendContext
{
    public class DSTReviewScanImg
    {
        public static double EQUATORIAL_CIRCUMFERENCE { get; } = 40075016.68557849;
        public static double MERCATOR_MIN { get; } = -20037508.342789244;
        public static double MERCATOR_MAX { get; } = 20037508.342789244;
        //1, 2, 3, 4, 5, 8, 10, 20, 40
        public static Dictionary<int, (double s, int length)> SparamDict { get; } = new Dictionary<int, (double, int)>
        {
            {1, (20.0,      256 * (int)Math.Pow(2,0)) },      // 1
            {2, (10.0,      256 * (int)Math.Pow(2,1)) },      // 2
            {3, (20.0/3.0,  256 * (int)Math.Pow(2,2)) },      // 3
            {4, (2.5,       256 * (int)Math.Pow(2,3)) },      // 4
            {5, (4.0,       256 * (int)Math.Pow(2,4)) },      // 5
            {6, (2.5,       256 * (int)Math.Pow(2,5)) },      // 8
            {7, (2.0,       256 * (int)Math.Pow(2,6)) },      // 10
            {8, (1.0,       256 * (int)Math.Pow(2,7)) },      // 20
            {9, (0.5,       256 * (int)Math.Pow(2,8)) },      // 40
        };
        //x =  (xPixel* s / widthPixel ) * EQUATORIAL_CIRCUMFERENCE + MERCATOR_MIN
        public static double GetPixel(double latlon, int level = 8)
        {
            var result = (latlon - MERCATOR_MIN) / EQUATORIAL_CIRCUMFERENCE * SparamDict[level].length / SparamDict[level].s;
            return result;
        }


    }
}
