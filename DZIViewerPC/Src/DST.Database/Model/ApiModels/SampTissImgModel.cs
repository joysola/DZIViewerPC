using DST.Common.Helper;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DST.Database.Model
{
    /// <summary>
    /// 组织大体图像
    /// </summary>
    public class SampTissImgModel : BaseModel
    {
        /// <summary>
        /// 大体图像
        /// </summary>
        [JsonProperty("generalImage")]
        [Notification]
        public string GeneralImg { get; set; }
        /// <summary>
        /// 报告采图状态 0 否 1是
        /// </summary>
        [JsonProperty("reportDrawingStatus")]
        [Notification]
        public string ReportDrawStatus { get; set; }
        /// <summary>
        /// 样本ID
        /// </summary>
        [JsonProperty("sampleId")]
        [Notification]
        public string SampleID { get; set; }

        /// <summary>
        /// 图片信息
        /// </summary>
        public BitmapImage CurImage => new Lazy<BitmapImage>(() =>
        {
            BitmapImage result = null;
            try
            {
                if (GeneralImg.StartsWith("https:"))
                {
                    // minio上的图片
                    result = new BitmapImage(new Uri(GeneralImg));
                }
                else if (System.IO.File.Exists(GeneralImg))
                {
                    // 本地图片
                    result = ImageHelper.BitmapToBitmapImage((Bitmap)Image.FromFile(GeneralImg));
                }
            }
            catch
            {
            }
            return result;
        }).Value;
    }
}
