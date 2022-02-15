using DST.ApiClient.Service;
using DST.Common.Extensions;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class SysManaDocViewModel : CustomBaseViewModel
    {

        /// <summary>
        /// 查询的医生列表
        /// </summary>
        [Notification]
        public List<DoctorInfoModel> DoctorList { get; set; } = new List<DoctorInfoModel>();
        /// <summary>
        /// 查询实体
        /// </summary>
        [Notification]
        public DoctorInfoModel QueryDoctor { get; set; } = new DoctorInfoModel { Name = "", Department = "" };


        /// <summary>
        /// 新增
        /// </summary>
        public ICommand AddCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.AddorEditUserInfo(new DoctorInfoModel(), "新增", true);
        })).Value;

        /// <summary>
        /// 编辑
        /// </summary>
        public ICommand EditCommand => new Lazy<RelayCommand<DoctorInfoModel>>(() => new RelayCommand<DoctorInfoModel>(data =>
        {
            this.AddorEditUserInfo(data.DeepCopy(), "编辑", false);
        })).Value;

        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.PageModel.PageIndex = 1;
            this.LoadData();
        })).Value;

        /// <summary>
        /// 删除单个用户
        /// </summary>
        public ICommand DeleteCommand => new Lazy<RelayCommand<DoctorInfoModel>>(() => new RelayCommand<DoctorInfoModel>(data =>
        {
            ShowMessageBox("请确认是否删除该医生信息？", MessageBoxButton.OKCancel, MessageBoxImage.Warning, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var result = SysManageService.Instance.DeleteDoctor(data);
                        if (!result)
                        {
                            ShowMessageBox("删除失败！", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        this.QueryCommand.Execute(null);
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
        })).Value;

        public SysManaDocViewModel()
        {
            QueryCommand.Execute(null);
        }
        /// <summary>
        /// 查询分页数据
        /// </summary>
        public override void LoadData()
        {
            WhirlingControlManager.ShowWaitingForm();
            try
            {
                DoctorList = SysManageService.Instance.GetDocList(PageModel, QueryDoctor);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
        /// <summary>
        /// 新增或删除弹窗
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <param name="title">标题</param>
        private void AddorEditUserInfo(DoctorInfoModel doctorInfo, string title, bool isAdd)
        {
            var msg = new ShowContentWindowMessage("SMD_Edit", title);
            msg.DesignWidth = 400;
            msg.DesignHeight = 300;
            msg.Args = new object[] { doctorInfo };
            msg.CallBackCommand = new RelayCommand<DoctorInfoModel>(res =>
            {
                if (res != null)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var result = false;
                        if (isAdd)
                        {
                            result = SysManageService.Instance.AddDoctor(res);
                        }
                        else
                        {
                            result = SysManageService.Instance.UpdateDoctor(res);
                        }

                        if (result)
                        {
                            this.QueryCommand.Execute(null);
                        }
                        else
                        {
                            ShowMessageBox($"{title}医生信息失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
            Messenger.Default.Send(msg);
        }
        /// <summary>
        /// 回车搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void QueryPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryCommand.Execute(null);
            }
        }
    }
}
