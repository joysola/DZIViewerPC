using DST.ApiClient.Service;
using DST.Controls;
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
    public class RegisterQueryViewModel : CustomBaseViewModel
    {
        private bool _isToday;
        private PathInfoModel _selectedPathInfo;
        /// <summary>
        /// 查询病理号、姓名
        /// </summary>
        [Notification]
        public string QueryName { get; set; }
        /// <summary>
        /// 是否当天接收登记
        /// </summary>
        [Notification]
        public bool IsToday
        {
            get => _isToday;
            set
            {
                _isToday = value;
                QueryCommand.Execute(null);
            }
        }
        /// <summary>
        /// 病理信息集合
        /// </summary>
        [Notification]
        public List<PathInfoModel> PathInfoList { get; set; }

        /// <summary>
        /// 选中的病理信息
        /// </summary>
        [Notification]
        public PathInfoModel SelectedPathInfo
        {
            get => _selectedPathInfo;
            set
            {
                _selectedPathInfo = value;
                if (_selectedPathInfo != null && _selectedPathInfo?.Code != PreSelectedPathInfo?.Code) // 记录选中项
                {
                    PreSelectedPathInfo = _selectedPathInfo;
                }
                QueryRegSampleCommand?.Execute(_selectedPathInfo?.Code); // 选中后查询登记相关信息
            }
        }
        /// <summary>
        /// 之前选中项目
        /// </summary>
        private PathInfoModel PreSelectedPathInfo { get; set; }
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(async isExisted =>
        {
            PageModel.PageIndex = 1;
            if (isExisted == "False") // 新增
            {
                PathInfoList = await GetPathInfoList();
                SelectedPathInfo = PathInfoList?.FirstOrDefault();
            }
            else
            {
                LoadData();
            }
        })).Value;
        /// <summary>
        /// 对应RegisterSampleViewModel 的QueryCommand
        /// </summary>
        public ICommand QueryRegSampleCommand { get; set; }
        /// <summary>
        /// 分页查询
        /// </summary>
        public override async void LoadData()
        {
            PathInfoList = await GetPathInfoList();
            if (PathInfoList?.Count > 0) // 找到之前选中的项目，找不到则选择第一个
            {
                SelectedPathInfo = PathInfoList?.FirstOrDefault(x => x.Code == PreSelectedPathInfo?.Code) ?? PathInfoList?.FirstOrDefault();
            }
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
        /// <summary>
        /// 获取病理列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<PathInfoModel>> GetPathInfoList()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                var result = await Task.Run(() => RegisterService.Instance.GetPathInfoList(PageModel, QueryName, IsToday));
                return result;
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
    }
}
