using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Client.ViewModel
{
    public class MolecularDiagnosisMainViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 扫码接收
        /// </summary>
        protected override void ScanCodeReceive()
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("MoleDiagBarcode", "扫码接收");
            msg.DesignHeight = 550;
            msg.DesignWidth = 700;
            msg.CallBackCommand = new RelayCommand(() => Messenger.Default.Send<object>(null, EnumMessageKey.RefreshMoleDiagExamed));
            Messenger.Default.Send(msg);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        protected override void BatchImport()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择模板文件";
            dialog.Filter = "excel|*.xls;*.xlsx";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    string url = CommonConfiguration.BaseApi + "dst-pathology/pathology/sample-pcr/laboratoryImportExcelPcrData";
                    string token = ExtendAppContext.Current.LoginModel.Token_Type + " " + ExtendAppContext.Current.LoginModel.Access_Token;
                    bool res = MolecularDiagnosisService.Instance.LaboratoryImportExcelPcrData(dialog.FileName, url, token);
                    WhirlingControlManager.CloseWaitingForm();
                    if (res)
                    {
                        this.ShowMessageBox("导入成功！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }
                    
                    Messenger.Default.Send<object>(null, EnumMessageKey.RefreshMoleDiagExamed);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        }

        /// <summary>
        /// 初审
        /// </summary>
        protected override void FirstExam()
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("MoleDiagCheck", "初审");
            msg.DesignHeight = 750;
            msg.DesignWidth = 1200;
            msg.Args = new object[] { 1 };
            Messenger.Default.Send(msg);
        }

        /// <summary>
        /// 复审
        /// </summary>
        protected override void ReExam()
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("MoleDiagCheck", "复审");
            msg.DesignHeight = 750;
            msg.DesignWidth = 1200;
            msg.Args = new object[] { 3 };
            Messenger.Default.Send(msg);
        }
    }
}
