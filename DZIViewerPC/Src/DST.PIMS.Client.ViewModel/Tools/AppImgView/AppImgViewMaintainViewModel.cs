using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework.Extensions;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class AppImgViewMaintainViewModel : CustomBaseViewModel
    {
        private string pathologyId = string.Empty;

        [Notification]
        public string LaboratoryCode { get; set; }

        [Notification]
        public ICommand CloseCommand { get; set; }

        [Notification]
        public ICommand GatherImageCommand { get; set; }

        [Notification]
        public ICommand GatherImageCallBackCommand { get; set; }

        [Notification]
        public ICommand SaveCommand { get; set; }

        [Notification]
        public AppFrmViewModel AppViewModel { get; set; } = new AppFrmViewModel();

        public AppImgViewMaintainViewModel()
        {

        }

        /// <summary>
        /// 回车查询
        /// </summary>
        public void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(this.LaboratoryCode))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    ApplyFrmModel result = ApplyFormService.Instance.GetPathInfobyCode(this.LaboratoryCode);
                    this.pathologyId = result?.PathID;
                    this.AppViewModel.AppModel = result ?? new ApplyFrmModel();
                    //this.AppViewModel.IsAdd = true; // 编辑模式
                    this.AppViewModel.Permissions.SetPermissionIsEdit(true); // 编辑模式
                    this.AppViewModel.IsPhysDist = true; // 前处理
                    this.AppViewModel.SelectedPathSamp = this.AppViewModel.AppModel?.PathSampInfoList?.FirstOrDefault(); // 获取第一项检验项目 使之选中
                    // 存在结果则 清空实验室编号
                    if (!string.IsNullOrEmpty(result?.PathID))
                    {
                        LaboratoryCode = null;
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.CloseCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.SaveCommand = new RelayCommand<object>(data =>
            {
                if (AppViewModel.CheckSaveData())
                {
                    var result = ApplyFormService.Instance.SavePathInfo(this.AppViewModel.AppModel);
                    if (result?.Count > 0)
                    {
                        ShowMessageBox("保存成功！", MessageBoxButton.OK, MessageBoxImage.Information, null, true);
                        this.AppViewModel.AppModel = null; // 清空数据
                        Messenger.Default.Send<object>(null, EnumMessageKey.AppFrmMaintainSaveFocus); // 焦点切换至扫码框
                    }
                    else
                    {
                        ShowMessageBox("保存申请单失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });

            // 申请单采集
            this.GatherImageCommand = new RelayCommand<object>(data =>
            {
                if (string.IsNullOrEmpty(this.pathologyId))
                {
                    this.ShowMessageBox("病理ID不能为空，请检查数据是否完整！");
                    return;
                }

                object[] args = new object[] { "CanCapture", this.pathologyId };
                this.RequestDoc(args);
            });
        }

        private void RequestDoc(object[] ar)
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("RequestDoc", "申请单采集");
            msg.DesignHeight = 600;
            msg.DesignWidth = 800;
            msg.Args = ar;
            Messenger.Default.Send(msg);
        }
    }
}
