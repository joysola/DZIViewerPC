using DST.Common.MinioHelper;
using DST.Controls.Base;
using DST.PIMS.Framework.ExtendContext;
using System;
using System.Collections.Generic;
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
    /// UploadMain.xaml 的交互逻辑
    /// </summary>
    public partial class UploadMain : BaseUserControl
    {
        public UploadMain()
        {
            InitializeComponent();
            this.Loaded += this.UploadMain_Loaded;
        }

        private void UploadMain_Loaded(object sender, RoutedEventArgs e)
        {
            // MinioHelper.Client.Connection(CommonConfiguration.MinIO_Endpoint, CommonConfiguration.MinIO_AccessKey, CommonConfiguration.MinIO_SecretKey);
        }
    }
}
