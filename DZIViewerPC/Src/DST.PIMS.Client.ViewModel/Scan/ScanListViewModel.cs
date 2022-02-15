using DST.ApiClient.Service;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ScanListViewModel : CustomBaseViewModel
    {
        [Notification]
        public ScanQueryModel CurScanQuery { get; set; } = new ScanQueryModel() {  };

        [Notification]
        public ScanReturnModel CurScanReturn { get; set; } = new ScanReturnModel();

        public ICommand QueryCommand { get; set; }

        public ICommand ShowSourceImgCommand { get; set; }

        [Notification]
        public List<ProductModel> ProductDict { get; set; }

        [Notification]
        public List<DictItem> DoctAdviceDict { get; set; }

        [Notification]
        public List<DictItem> TechAdviceDict { get; set; }
        
        public ScanListViewModel()
        {
            this.ProductDict = ExtendApiDict.Instance.ProductDict;
            this.DoctAdviceDict = ExtendApiDict.Instance.DoctAdviceDict;
            this.TechAdviceDict = ExtendApiDict.Instance.TechAdviceDict;
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.QueryCommand = new RelayCommand<object>(data =>
            {
                this.PageModel.PageIndex = 1;
                this.LoadData();
            });

            this.ShowSourceImgCommand = new RelayCommand<object>(data =>
            {
                if (this.CurScanReturn.records.FirstOrDefault(x => x.IsSelected) == null)
                {
                    this.ShowMessageBox("未选中数据，请重新选择！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return;
                }

                ScanService.Instance.BathReceive(this.CurScanReturn.records.Where(x => x.IsSelected).Select(x => x.id).ToList());

                this.QueryCommand.Execute(null);
            });
        }

        public override void LoadData()
        {
            this.CurScanReturn = ScanService.Instance.PageScanQuery(this.CurScanQuery, this.PageModel.PageIndex, this.PageModel.PageSize);
            this.PageModel.TotalPage = this.CurScanReturn.pages.Value;
            this.PageModel.TotalNum = this.CurScanReturn.total.Value;
        }

        public void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.QueryCommand.Execute(null);
            }
        }
    }
}
