using DST.Controls.Base;
using System;
using System.IO;
using System.Windows;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// ShowPDF.xaml 的交互逻辑
    /// </summary>
    public partial class ShowPDF : BaseUserControl
    {
        /// <summary>
        /// PDF文件的完整路径
        /// </summary>
        public string PdfPath
        {
            get { return (string)GetValue(PdfPathProperty); }
            set { SetValue(PdfPathProperty, value); }
        }

        public static readonly DependencyProperty PdfPathProperty =
            DependencyProperty.Register("PdfPath", typeof(string), typeof(ShowPDF), new PropertyMetadata(new PropertyChangedCallback(PdfPath_PropertyChangedCallback)));

        /// <summary>
        /// 加载对应型号的DLL
        /// </summary>
        static ShowPDF()
        {
            var path = new Uri(typeof(ShowPDF).Assembly.CodeBase).LocalPath; // 当前程序集路径
            var folder = Path.GetDirectoryName(path); // 程序集文件夹
            var subfolder = Environment.Is64BitProcess ? "MoonPdfLib-0.3.0-x64" : "MoonPdfLib-0.3.0-x86"; // 选择64或32位dll文件夹

            FileInfo[] dllFiles = new DirectoryInfo($"{folder}/DLL/{subfolder}").GetFiles();
            for(int i = 0; i < dllFiles.Length; i++)
            {
                File.Copy(dllFiles[i].FullName, folder + "\\" + dllFiles[i].Name, true);
            }
        }
        /// <summary>
        /// PDF文档显示
        /// </summary>
        public ShowPDF()
        {
            InitializeComponent();
            this.Loaded += ShowPDF_Loaded;
        }

        private void ShowPDF_Loaded(object sender, RoutedEventArgs e)
        {
            this.OpenPdf();
        }

        public static void PdfPath_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ShowPDF showPdf = (ShowPDF)d;
            if(showPdf.IsLoaded && e.NewValue != null)
            {
                showPdf.OpenPdf();
            }
        }

        /// <summary>
        /// 显示PDF文件
        /// </summary>
        /// <param name="pdfPath">pdf完整路径</param>
        public bool OpenPdf()
        {
            bool result = System.IO.File.Exists(this.PdfPath);
            if (result)
            {
                this.moonPdfPanel.Visibility = Visibility.Visible;
                this.moonPdfPanel.OpenFile(this.PdfPath);
                this.moonPdfPanel.ZoomStep = 0.4;
                this.moonPdfPanel.ZoomToWidth();
            }
            else
            {
                this.moonPdfPanel.Visibility = Visibility.Collapsed;
            }

            return result;
        }
    }
}
