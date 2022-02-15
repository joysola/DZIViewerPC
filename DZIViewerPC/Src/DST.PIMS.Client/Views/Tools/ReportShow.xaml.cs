using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// ReportShow.xaml 的交互逻辑
    /// </summary>
    public partial class ReportShow : BaseUserControl
    {
        private ReportShowViewModel reportShowViewModel = null;

        public ReportShow()
        {
            InitializeComponent();
            this.reportShowViewModel = new ReportShowViewModel();
            this.DataContext = this.reportShowViewModel;
            this.Loaded += this.ReportShow_Loaded;
        }

        private void ReportShow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if(null != this.reportShowViewModel.CurReportUrl)
            {
                if(string.IsNullOrEmpty(this.reportShowViewModel.CurReportUrl.LocalReportUrl))
                {
                    this.TabControlDemo.Items.Remove(this.tabReport);
                }

                if (string.IsNullOrEmpty(this.reportShowViewModel.CurReportUrl.LocalReportUrlEnglish))
                {
                    this.TabControlDemo.Items.Remove(this.tabReportEng);
                }
            }
            else
            {
                this.TabControlDemo.Items.Clear();
            }

            this.TabControlDemo.SelectionChanged += this.TabControlDemo_SelectionChanged;
        }

        private void TabControlDemo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(this.tabReportEng.IsSelected)
            {
                this.reportShowViewModel.CurShowPdfPath = this.reportShowViewModel.CurReportUrl.LocalReportUrlEnglish;
            }
            else if(this.tabReport.IsSelected)
            {
                this.reportShowViewModel.CurShowPdfPath = this.reportShowViewModel.CurReportUrl.LocalReportUrl;
            }
        }
    }
}
