using DST.Common.Helper;
using DST.Common.MinioHelper;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace DST.Database.Model
{
    /// <summary>
    /// 申请单图片返回
    /// </summary>
    public class PathologyImage : ObservableObject
	{
        private string _applicationFormUrl = string.Empty;

        public string applicationFormUrl 
        {
            get { return this._applicationFormUrl; }
            set
            {
                this._applicationFormUrl = value;
                this.RaisePropertyChanged("applicationFormUrl");
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        if (value.StartsWith("https:"))
                        {
                            // minio上的图片
                            this.CurImage = new BitmapImage(new Uri(value));
                        }
                        else if (System.IO.File.Exists(value))
                        {
                            // 本地图片
                            this.CurImage = ImageHelper.BitmapToBitmapImage((Bitmap)Image.FromFile(value));
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

		public string createDept { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? createTime { get; set; }

		public string createUser { get; set; }

		public string id { get; set; }

		public int? isDeleted { get; set; }

		public string pathologyId { get; set; }

		public int? status { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? updateTime { get; set; }

		public string updateUser { get; set; }


        [JsonIgnore]
        [Notification]
        public BitmapImage CurImage { get; set; }
    }
}
