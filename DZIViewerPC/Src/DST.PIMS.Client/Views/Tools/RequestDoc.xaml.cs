using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using DST.PIMS.Framework.Model;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DST.PIMS.Client.Views.Tools
{
    /// <summary>
    /// RequestDoc.xaml 的交互逻辑
    /// </summary>
    public partial class RequestDoc : BaseUserControl
    {
        private RequestDocViewModel requestDocViewModel = null;

        public RequestDoc()
        {
            InitializeComponent();
            this.requestDocViewModel = new RequestDocViewModel();
            this.DataContext = this.requestDocViewModel;
            this.Loaded += this.RequestDoc_Loaded;
        }

        private void RequestDoc_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
