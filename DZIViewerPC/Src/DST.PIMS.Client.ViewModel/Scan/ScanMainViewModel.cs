using DST.ApiClient.Service;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Client.ViewModel
{
    public class ScanMainViewModel : CustomBaseViewModel
    {
        public ScanMainViewModel()
        {
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            // 扫码接收确认
            Messenger.Default.Register<string>(this, EnumMessageKey.ScanMainReceive, data =>
            {
                if (!string.IsNullOrEmpty(data))
                {
                    ApiResponse<object> res = ScanService.Instance.ReceiveByCode(data);
                    if(!res.Success)
                    {
                        this.ShowMessageBox("扫码接收失败：" + res.Msg, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
            });
        }

        protected override void ReceiveByScan()
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("ReceiveByScan", "扫码接收");
            msg.DesignHeight = 700;
            msg.DesignWidth = 1000;
            Messenger.Default.Send(msg);
        }
    }
}
