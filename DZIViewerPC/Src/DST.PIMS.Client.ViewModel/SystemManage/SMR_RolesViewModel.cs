using DST.Database.Model;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class SMR_RolesViewModel : CustomBaseViewModel
    {
        [Notification]
        public List<RoleInfoModel> RoleInfoList { get; set; }

        [Notification]
        public RoleInfoModel SelectRole { get; set; }
        /// <summary>
        /// 取消
        /// </summary>
        public ICommand CancelCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.CloseContentWindow();
        })).Value;

        /// <summary>
        /// 确定
        /// </summary>
        public ICommand OKCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (SelectRole != null)
            {
                this.Result = SelectRole; // 将选择的菜单ID付给结果
                this.CloseContentWindow();
            }
        })).Value;

        /// <summary>
        /// 选中项
        /// </summary>
        public ICommand SelectCommand => new Lazy<RelayCommand<RoleInfoModel>>(() => new RelayCommand<RoleInfoModel>(data =>
        {
            SelectRole = data;
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1 && this.Args[0] is List<RoleInfoModel> roleList)
            {
                RoleInfoList = roleList;
            }
        }
    }
}
