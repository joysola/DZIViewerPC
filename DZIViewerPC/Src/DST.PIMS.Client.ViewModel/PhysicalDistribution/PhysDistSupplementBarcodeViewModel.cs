using DST.ApiClient.Service;
using DST.Common.TscBarcodePrint;
using DST.Database.Model;
using DST.PIMS.Framework;
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
    public class PhysDistSupplementBarcodeViewModel : CustomBaseViewModel
    {
        [Notification]
        public ICommand CloseCommand { get; set; }

        [Notification]
        public ICommand PrintBarcodeCommand { get; set; }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();
            this.CloseCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.PrintBarcodeCommand = new RelayCommand<string>(data =>
            {
                List<PhysDistReceiptBarcodeModel> res = PhysicalDistributionService.Instance.GetSampleTscByLaboratoryCode(data);
                if(res != null && res.Count > 0)
                {
                    //TSCPrintManager.Instance.Print(res);
                    PrintLabelManager.Singleton.Print(res);
                }
            });
        }

        
    }
}
