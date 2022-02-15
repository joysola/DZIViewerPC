using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// PhysDistReceipt.xaml 的交互逻辑
    /// </summary>
    public partial class PhysDistReceipt : BaseUserControl
    {
        private PhysDistReceiptViewModel physDistReceiptViewModel = null;

        public PhysDistReceipt()
        {
            InitializeComponent();
            this.physDistReceiptViewModel = new PhysDistReceiptViewModel();
            this.DataContext = this.physDistReceiptViewModel;
        }
    }
}
