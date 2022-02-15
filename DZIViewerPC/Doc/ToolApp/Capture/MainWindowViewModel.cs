using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class MainWindowViewModel
    {
        /// <summary>
        /// 缩略图列表
        /// </summary>
        [Notification]
        public ObservableCollection<ImageModel> ThumbnailList { get; set; } = new ObservableCollection<ImageModel>();

        public MainWindowViewModel()
        {
            //this.ThumbnailList.Add(this.BitmapToBitmapImage((Bitmap)Image.FromFile(@"D:\WSITestData\申请单\1.jpg")));
            //this.ThumbnailList.Add(this.BitmapToBitmapImage((Bitmap)Image.FromFile(@"D:\WSITestData\申请单\2.jpg")));
            //this.ThumbnailList.Add(this.BitmapToBitmapImage((Bitmap)Image.FromFile(@"D:\WSITestData\申请单\3.jpg")));
        }

        public BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                BitmapImage bitImage = new BitmapImage();
                bitImage.BeginInit();
                bitImage.StreamSource = ms;
                bitImage.EndInit();
                return bitImage;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class ImageModel
    {
        [Notification]
        public BitmapImage CurImage { get; set; }

        [Notification]
        public string ImagePath { get; set; }

        [Notification]
        public bool IsChecked { get; set; }
    }
}
