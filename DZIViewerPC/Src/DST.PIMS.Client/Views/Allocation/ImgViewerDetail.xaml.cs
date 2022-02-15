using DST.Common.Helper;
using DST.Controls.Base;
using DST.PIMS.Framework.Controls;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// ImgViewerDetail.xaml 的交互逻辑
    /// </summary>
    [NotifyAspect]
    public partial class ImgViewerDetail : BaseUserControl
    {
        /// <summary>
        /// 当前选中的标记
        /// </summary>
        public AnnoBase CurAnnoBase
        {
            get => (AnnoBase)GetValue(CurAnnoBaseProperty);
            set => SetValue(CurAnnoBaseProperty, value);
        }

        // Using a DependencyProperty as the backing store for AnnoInfos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurAnnoBaseProperty =
            DependencyProperty.Register(nameof(CurAnnoBase), typeof(AnnoBase), typeof(ImgViewerDetail), new PropertyMetadata(null));

        public ImgViewerDetail()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 报告视野下载主图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportViewList_DownloadImg(object sender, EventArgs e)
        {
            if (sender is ReportViewList report)
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                // dialog.InitialDirectory = textbox.Text; // Use current value for initial dir
                dialog.Title = "请选择下载目录";
                dialog.Filter = "Directory|*.png;*.jpg"; // 只显示png图片
                dialog.FileName = "切片主图"; // Filename will then be "select.this.directory"
                if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.FileName))
                {
                    if (report.FindName("lbImgs") is ListBox box && box.ItemContainerGenerator.ContainerFromItem(box.SelectedItem) is ListBoxItem item)
                    {
                        var imgs = item.FindChildren<Image>();
                        if (imgs?.Count == 1 && imgs[0].Source is BitmapSource bitmapSource)
                        {
                            using (var fs = new FileStream(dialog.FileName, FileMode.CreateNew))
                            {
                                var bitmapFrame = BitmapFrame.Create(bitmapSource);
                                var encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(bitmapFrame);
                                encoder.Save(fs);
                            }
                        }
                    }
                }
            }
        }
    }
}
