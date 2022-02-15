using DST.ApiClient.Service;
using DST.Database.Model;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class PhysDistClinManifViewModel : CustomBaseViewModel
    {
        [Notification]
        public string PatientName { get; set; }

        [Notification]
        public PhysDistClinManifModel CurSelected { get; set; }

        [Notification]
        public ObservableCollection<PhysDistClinManifModel> ClinManifList { get; set; } = new ObservableCollection<PhysDistClinManifModel>();

        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public PhysDistClinManifViewModel()
        {

        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.CancelCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.SaveCommand = new RelayCommand<object>(data =>
            {
                if (this.CurSelected != null)
                {
                    bool res = PhysicalDistributionService.Instance.SaveClinicalDiagnosis(this.CurSelected.clinicalDiagnosis, this.CurSelected.pathologyId);
                    if (res)
                    {
                        this.ShowMessageBox("保存成功！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information, null, true, 1000);
                    }
                }
            });
        }

        public void PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ClinManifList = new ObservableCollection<PhysDistClinManifModel>(PhysicalDistributionService.Instance.ListPathologyByName(this.PatientName));
                if (this.ClinManifList == null || this.ClinManifList.Count == 0)
                {
                    this.ShowMessageBox("未查询到相应数据！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    this.PatientName = "";
                }
                else
                {
                    this.CurSelected = this.ClinManifList[0];
                    Messenger.Default.Send<bool>(true, EnumMessageKey.ResetFocus);
                }
            }
        }

        public void Save_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.SaveCommand.Execute(null);
            }
        }
    }
}
