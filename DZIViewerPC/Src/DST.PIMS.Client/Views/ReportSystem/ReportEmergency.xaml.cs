using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Client.ViewModel;
using HandyControl.Controls;
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
    /// ReportEmergency.xaml 的交互逻辑
    /// </summary>
    public partial class ReportEmergency : BaseUserControl
    {
        private ReportEmergencyViewModel reportEmergencyViewModel = null;

        public ReportEmergency()
        {
            InitializeComponent();
            this.reportEmergencyViewModel = new ReportEmergencyViewModel();
            this.DataContext = this.reportEmergencyViewModel;
            this.Loaded += this.ReportEmergency_Loaded;
        }

        private void ReportEmergency_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.reportEmergencyViewModel.CurSelectedPeport.addType.HasValue)
            {
                switch(this.reportEmergencyViewModel.CurSelectedPeport.addType.Value)
                {
                    case 1:
                        this.TabControlDemo.Items.Remove(this.tabItemDoctAdvice);
                        break;
                    case 2:
                        this.TabControlDemo.Items.Remove(this.tabItemCommon);
                        break;
                }
            }
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void CheckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                this.reportEmergencyViewModel.AddOrRemoveMarkModel(e.AddedItems[0] as MarkModel, true);
            }
            else if (e.RemovedItems != null && e.RemovedItems.Count > 0)
            {
                this.reportEmergencyViewModel.AddOrRemoveMarkModel(e.RemovedItems[0] as MarkModel, false);
            }

            this.RefreshDataGridHeader();
        }

        private void RefreshDataGridHeader()
        {
            foreach (var item in this.dgDocAdv.Items)
            {
                var row = (DataGridRow)dgDocAdv.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                    row.Header = row.GetIndex() + 1;
            }
        }
    }
}
