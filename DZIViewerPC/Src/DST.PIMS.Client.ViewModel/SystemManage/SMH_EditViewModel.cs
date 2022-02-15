using DST.Controls.Base;
using DST.Database.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class SMH_EditViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 部门信息
        /// </summary>
        [Notification]
        public DeptDetailModel DeptDetail { get; set; }
        /// <summary>
        /// 部门树菜单
        /// </summary>
        private List<DeptInfoModel> DeptTrees { get; set; }
        /// <summary>
        /// 所有部门集合
        /// </summary>
        [Notification]
        public ObservableCollection<DeptInfoModel> AllDepts { get; set; } = new ObservableCollection<DeptInfoModel>();

        /// <summary>
        /// 编辑
        /// </summary>
        public ICommand EditParentDept => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var msg = new ShowContentWindowMessage("SMH_Depts", "部门列表");
            msg.DesignWidth = 400;
            msg.DesignHeight = 500;
            msg.Args = new object[] { DeptTrees };
            msg.CallBackCommand = new RelayCommand<DeptInfoModel>(res =>
            {
                if (DeptDetail != null && res is DeptInfoModel)
                {
                    if (res.ID == DeptDetail.ID)
                    {
                        ShowMessageBox("上级部门不能是自身!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    DeptDetail.ParentID = res.ID; // 将选择的父级部门ID赋值给当前部门的ParentID
                }
            });
            Messenger.Default.Send(msg);
        })).Value;
        /// <summary>
        /// 清理parentid
        /// </summary>
        public ICommand ClearParentCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (DeptDetail != null)
            {
                DeptDetail.ParentID = null;
            }
        })).Value;
        /// <summary>
        /// 取消
        /// </summary>
        public ICommand CancelCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.CloseContentWindow();
        })).Value;
        /// <summary>
        /// 确认
        /// </summary>
        public ICommand OKCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (IsCheckedOK)
            {
                this.Result = DeptDetail;
                this.CloseContentWindow();
            }
        })).Value;


        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 2 && this.Args[0] is DeptDetailModel deptDetail && this.Args[1] is List<DeptInfoModel> deptList)
            {
                DeptDetail = deptDetail;
                DeptTrees = deptList;
                GetAllMenus(deptList);
            }
        }

        /// <summary>
        /// 从树形结构获取所有部门集合
        /// </summary>
        /// <param name="menuTree"></param>
        private void GetAllMenus(List<DeptInfoModel> deptTrees)
        {
            foreach (var dept in deptTrees)
            {
                AllDepts.Add(dept);
                if (dept.Children != null && dept.Children.Count > 0)
                {
                    GetAllMenus(dept.Children);
                }
            }
        }
    }
}
