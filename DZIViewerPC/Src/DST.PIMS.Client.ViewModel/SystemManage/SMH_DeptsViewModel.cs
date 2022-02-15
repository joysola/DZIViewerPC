using DST.Database.Model;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class SMH_DeptsViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 部门树
        /// </summary>
        [Notification]
        public List<DeptInfoModel> DeptList { get; set; }
        /// <summary>
        /// 选中的部门
        /// </summary>
        [Notification]
        public DeptInfoModel SelectDept { get; set; }
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
            if (SelectDept != null)
            {
                this.Result = SelectDept; // 将选择的菜单ID付给结果
                this.CloseContentWindow();
            }
        })).Value;

        /// <summary>
        /// 选中项
        /// </summary>
        public ICommand SelectCommand => new Lazy<RelayCommand<DeptInfoModel>>(() => new RelayCommand<DeptInfoModel>(data =>
        {
            SelectDept = data;
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1 && this.Args[0] is List<DeptInfoModel> deptList)
            {
                DeptList = deptList;
            }
        }
    }
}
