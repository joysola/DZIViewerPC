using DST.Controls.Base;
using DST.PIMS.Framework;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Client.ViewModel
{
    public class PhysDistMainViewModel : CustomBaseViewModel
    {
        public PhysDistMainViewModel()
        {
            PrintLabelManager.Singleton.InitPrintManager(IniSectionConst.PhysDistSection);
        }

        /// <summary>
        /// 新增物流
        /// </summary>
        protected override void AddExpress()
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("PhysDistAdd", "新增物流");
            msg.DesignWidth = 1500;
            msg.DesignHeight = 700;
            Messenger.Default.Send(msg);
        }

        protected override void SigninExpress()
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("PhysDistReceipt", "物流签收");
            msg.DesignWidth = 1600;
            msg.DesignHeight = 700;
            Messenger.Default.Send(msg);
        }

        protected override void ScanCodeSendInsp()
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("PhysDistPreTreatBarcode", "扫码送检");
            msg.DesignWidth = 1000;
            msg.DesignHeight = 450;
            Messenger.Default.Send(msg);
        }

        /// <summary>
        /// 补打条码
        /// </summary>
        protected override void ReprintCode()
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("PhysDistSupplementBarcode", "补打条码");
            msg.DesignWidth = 400;
            msg.DesignHeight = 200;
            Messenger.Default.Send(msg);
        }

        protected override void MaintainAppFrm()
        {
            ShowContentWindowMessage scw = new ShowContentWindowMessage("AppImgViewMaintain", "申请单维护");
            scw.DesignWidth = 1200;
            scw.DesignHeight = 800;
            Messenger.Default.Send(scw);
        }
        /// <summary>
        /// 重新取样关联
        /// </summary>
        protected override void ResampleConnect()
        {
            var msg = new ShowContentWindowMessage("RegReSample", " 重新取样列表");
            msg.DesignWidth = 1400;
            msg.DesignHeight = 800;
            msg.CallBackCommand = new RelayCommand(() =>
            {
                //RegQueryViewModel.QueryCommand.Execute(null); // 刷新登记列表
            });
            Messenger.Default.Send(msg);
        }

        /// <summary>
        /// 临床维护
        /// </summary>
        protected override void ClinicalManifestation()
        {
            var msg = new ShowContentWindowMessage("PhysDistClinManif", " 临床表现");
            msg.DesignWidth = 900;
            msg.DesignHeight = 600;
            Messenger.Default.Send(msg);
        }
    }
}
