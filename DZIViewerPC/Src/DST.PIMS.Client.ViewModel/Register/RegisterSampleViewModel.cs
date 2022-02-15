using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework.Extensions;
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
    public class RegisterSampleViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 申请单ViewModel
        /// </summary>
        public AppFrmViewModel AppViewModel { get; set; } = new Lazy<AppFrmViewModel>(() =>
        {
            var result = new AppFrmViewModel();
            result.Permissions.SetPermissionIsEdit(true); // 可以编辑
            return result;
        }).Value;// { IsAdd = true };
        /// <summary>
        /// 码字典
        /// </summary>
        public Dictionary<string, string> CodeDict { get; set; } = new Dictionary<string, string> { { "1", "实验室编号" }, { "2", "条码号" }, { "3", "HIS识别号" } };
        /// <summary>
        /// 搜索条码类型
        /// </summary>
        [Notification]
        public string CodeType { get; set; } = "1";
        /// <summary>
        /// 识别号
        /// </summary>
        [Notification]
        public string QueryCode { get; set; } = string.Empty;
        /// <summary>
        /// 数据是否已经存在
        /// </summary>
        public bool IsExisted { get; set; } = false;
        /// <summary>
        /// 查询申请单
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(async data =>
        {
            var queryStr = string.Empty;

            if (!string.IsNullOrEmpty(data)) // 参数来自于RegisterQueryViewModel
            {
                queryStr = data;
            }
            else if (!string.IsNullOrEmpty(QueryCode)) // 本页面搜索
            {
                queryStr = QueryCode;
            }
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                var result = await Task.Run(() => ApplyFormService.Instance.GetPathInfobyCode(queryStr)) ?? new ApplyFrmModel();
                IsExisted = string.IsNullOrEmpty(result.ID) ? false : true; // 记录是否存在
                AppViewModel.SelectedProduct = null; // 清空检验项目
                AppViewModel.AppModel = result;
                //AppViewModel.IsAdd = true; // 编辑模式
                AppViewModel.Permissions.SetPermissionIsEdit(true); // 编辑模式
                AppViewModel.SelectedPathSamp = AppViewModel.AppModel?.PathSampInfoList?.FirstOrDefault(); // 获取第一项检验项目 使之选中
                // 结果存在则清空搜索文本框
                if (!string.IsNullOrEmpty(result?.ID))
                {
                    QueryCode = null;
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }

        })).Value;

        public RegisterSampleViewModel()
        {
            QueryCommand.Execute(null);
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
                QueryCommand.Execute(QueryCode);
            }
        }
    }
}
