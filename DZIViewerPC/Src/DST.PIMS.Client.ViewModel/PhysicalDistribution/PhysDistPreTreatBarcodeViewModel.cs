using DST.ApiClient.Service;
using DST.Common.Helper.ExcelHelper;
using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class PhysDistPreTreatBarcodeViewModel : CustomBaseViewModel
    {
        [Notification]
        public string LaboratoryCode { get; set; }

        [Notification]
        public Inspection CurSelectedInspection { get; set; }

        [Notification]
        public ObservableCollection<Inspection> InspectionList { get; set; } = new ObservableCollection<Inspection>();

        [Notification]
        public ICommand DeleteCommand { get; set; }

        [Notification]
        public ICommand CloseCommand { get; set; }

        [Notification]
        public ICommand SaveCommand { get; set; }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.DeleteCommand = new RelayCommand<object>(data =>
            {
                this.InspectionList.Remove(this.CurSelectedInspection);
            });

            this.CloseCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.SaveCommand = new RelayCommand<object>(async data =>
            {
                //ApiResponse<object> res = PhysicalDistributionService.Instance.SaveInspectionSampleList(this.InspectionList.Select(x => x.id).ToList());
                //if (res.Success)
                //{
                //    DataTable dt = TreatRepertManager.Instance.TreatRepertListToDataTable(JsonConvert.DeserializeObject<List<TreatRepertModel>>(res.Data?.ToString()));
                //    string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + $"\\{DateTime.Now.Ticks}.xlsx";
                //    ExcelHelper.WriteExcelFile(deskDir, dt);
                //    Messenger.Default.Send<object>(null, EnumMessageKey.RefreshPhysDistSign);
                //    this.CloseContentWindow();
                //}
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var res = await PhysicalDistributionService.Instance.SaveInspeSampleListandGetExcel(this.InspectionList.Select(x => x.id).ToList());
                    if (res)
                    {
                        Messenger.Default.Send<object>(null, EnumMessageKey.RefreshPhysDistSign);
                        this.CloseContentWindow();
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            });
        }

        /// <summary>
        /// 根据实验室编号查询
        /// </summary>
        public void PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Inspection newIns = PhysicalDistributionService.Instance.GetInspectSampleByLaboratoryCode(this.LaboratoryCode);
                if (newIns == null)
                {
                    Messenger.Default.Send<bool>(false, EnumMessageKey.ResetFocus);
                    this.ShowMessageBox("未找到数据！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning,
                        res =>
                        {

                            Messenger.Default.Send<bool>(true, EnumMessageKey.ResetFocus);

                        }, true, 1000);
                    this.LaboratoryCode = "";
                    return;
                }

                if (this.InspectionList.FirstOrDefault(x => x.laboratoryCode.Equals(newIns.laboratoryCode)) != null)
                {
                    Messenger.Default.Send<bool>(false, EnumMessageKey.ResetFocus);
                    this.CurSelectedInspection = this.InspectionList.FirstOrDefault(x => x.laboratoryCode.Equals(newIns.laboratoryCode));
                    this.ShowMessageBox("请勿重复扫码！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning,
                        res =>
                        {

                            Messenger.Default.Send<bool>(true, EnumMessageKey.ResetFocus);

                        }, true, 1000);
                    this.LaboratoryCode = "";
                    return;
                }

                this.InspectionList.Insert(0, newIns);
                this.InspectionList = new ObservableCollection<Inspection>(this.InspectionList);
                this.LaboratoryCode = "";
            }
        }
    }
}
