using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// PhysDistReceiptHuman.xaml 的交互逻辑
    /// </summary>
    public partial class PhysDistReceiptHuman : BaseUserControl
    {
        private PhysDistReceiptHumanViewModel physDistReceiptHumanViewModel = null;

        public PhysDistReceiptHuman()
        {
            InitializeComponent();
            this.physDistReceiptHumanViewModel = new PhysDistReceiptHumanViewModel();
            this.DataContext = this.physDistReceiptHumanViewModel;
            this.Loaded += this.PhysDistReceiptHuman_Loaded;
        }

        private void PhysDistReceiptHuman_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Messenger.Default.Register<object>(this, EnumMessageKey.PhysDistReceiptHumanFocus, data =>
            {
                this.tbMailNo.Focus();
            });
            this.tbMailNo.Focus();
        }

        private void DataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
