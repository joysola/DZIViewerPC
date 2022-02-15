using DST.Common.Helper;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using DST.PIMS.Framework.ExtendContext;
using DST.Controls.Base;
using System;
using DST.PIMS.Framework.Helper;
using DST.PIMS.Framework.Model.Enum;

namespace DST.PIMS.Client.ViewModel
{
    public class ToolBarViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 功能按钮点击事件
        /// </summary>
        public ICommand ToolButtonClick { get; set; }

        /// <summary>
        /// 工作站列表
        /// </summary>
        [Notification]
        public ObservableCollection<WorkstationToolButtons> WorkstationList { get; set; } = new ObservableCollection<WorkstationToolButtons>();

        /// <summary>
        /// 当前显示的工作站
        /// </summary>
        [Notification]
        public WorkstationToolButtons CurWorkstation { get; set; }

        /// <summary>
        /// 工具栏
        /// </summary>
        public ToolBarViewModel()
        {
            this.InitWorkstationList();
            this.RegisterMessenger();

        }

        private void RegisterMessenger()
        {
            // 刷新菜单（注销后）
            Messenger.Default.Register<object>(this, EnumMessageKey.RefreshMenus, data =>
            {
                this.InitWorkstationList();
            });
        }

        /// <summary>
        /// 初始化工作站列表
        /// </summary>
        private void InitWorkstationList()
        {
            this.WorkstationList.Clear();
            MenuButtonHelper.ClearAllToolButtonIncludeWorks(ExtendAppContext.Current.AllButtons); // 初始化按钮所包含的工作站
            foreach (var item in ExtendAppContext.Current.LoginMenu) // 登录权限
            {
                var menu = ExtendAppContext.Current.AllMenus.FirstOrDefault(x => x.Key == item.Code); // 找出本地对应菜单
                if (menu.Key != null)
                {
                    if (item.CodeList?.Count > 0) // 获取对应工作站按钮
                    {
                        item.CodeList?.ForEach(code =>
                        {
                            var toolButton = ExtendAppContext.Current.AllButtons?.FirstOrDefault(x => x.EnumName == code);
                            if (toolButton != null)
                            {
                                toolButton.CanIncludedWorkstation.Add(menu.Value);
                            }
                        });
                    }
                    var workstationTBtns = new WorkstationToolButtons(menu.Value, item.Name);
                    workstationTBtns.ToolButtonList = workstationTBtns.ToolButtonList.OrderBy(x => item.CodeList.IndexOf(x.EnumName)).ToList(); // 按中台codelist排序
                    this.WorkstationList.Add(workstationTBtns);
                }
            }

            //WorkstationToolButtons allocation = new WorkstationToolButtons(EnumWorkstationType.Allocation, "分配阅片工作站");
            //this.WorkstationList.Add(allocation);

            WorkstationToolButtons imgViewer = new WorkstationToolButtons(EnumWorkstationType.ImageViewer, "阅片工作站");
            this.WorkstationList.Add(imgViewer);
            var imgViewerBtns = new List<EnumToolButtonType> { EnumToolButtonType.Refresh, EnumToolButtonType.OpenDir }; // 所含按钮
            var btns = ExtendAppContext.Current.AllButtons.Where(x => imgViewerBtns.Contains(x.ToolBtnType));
            imgViewer.ToolButtonList.AddRange(btns);
            foreach (var btn in btns)
            {
                btn.CanIncludedWorkstation.Add(EnumWorkstationType.ImageViewer);
            }

            this.CurWorkstation = this.WorkstationList?.FirstOrDefault();
            ExtendAppContext.Current.CurWorkstationType = this.CurWorkstation?.WorkstationType ?? EnumWorkstationType.None;
        }

        protected override void RegisterCommand()
        {
            // base.RegisterCommand();
            this.ToolButtonClick = new RelayCommand<ToolButton>(toolButton =>
            {
                this.ResponseToolButton(toolButton.ToolBtnType);
            });
        }

        public void SwitchWorkstation(object selectedItem)
        {
            if (ExtendAppContext.Current.CurLoginType == EnumLoginType.Uploading)
            {
                this.ShowMessageBox("正在上传文件，请勿切换！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                return;
            }

            WorkstationToolButtons newStation = selectedItem as WorkstationToolButtons;
            if (newStation != null)
            {
                this.CurWorkstation = newStation;
                ExtendAppContext.Current.CurWorkstationType = newStation.WorkstationType;
            }
        }

        /// <summary>
        /// 发送对应的命令到具体工作站
        /// </summary>
        /// <param name="content">按钮文本</param>
        private void ResponseToolButton(EnumToolButtonType enumType)
        {
            Messenger.Default.Send(enumType, EnumMessageKey.ToolButtonResponse);
        }
    }
}
