using DST.ApiClient.Service;
using DST.Database.Model;
using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class HospitalDictViewModel : CustomBaseViewModel
    {
        [Notification]
        public HospitalModel CurHospitalModel { get; set; } = new HospitalModel();

        [Notification]
        public HospitalModel CurSelectedHospital { get; set; }

        [Notification]
        public ObservableCollection<HospitalModel> HospitalList { get; set; } = new ObservableCollection<HospitalModel>();

        public ICommand QueryCommand { get; set; }

        public HospitalDictViewModel()
        {
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();
            this.QueryCommand = new RelayCommand<object>(data =>
            {
                this.PageModel.PageIndex = 1;
                this.LoadData();
            });
        }

        public void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Result = this.CurSelectedHospital;
            this.CloseContentWindow();
        }

        public override void LoadData()
        {
            HospitalReturnModel res = DictService.Instance.PageByHospital(this.CurHospitalModel, this.PageModel.PageIndex, this.PageModel.PageSize).Result;
            this.HospitalList = new ObservableCollection<HospitalModel>(res?.records);
            this.PageModel.TotalPage = res.pages.Value;
            this.PageModel.TotalNum = res.total.Value;
        }
    }
}
