using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
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
    public class QueryPathListViewModel : CustomBaseViewModel
    {
        private SampleModel _selectModel;
        /// <summary>
        /// 取材列表
        /// </summary>
        [Notification]
        public List<SampleModel> SampleList { get; set; } = new List<SampleModel>();
        /// <summary>
        /// 查询条件
        /// </summary>
        [Notification]
        public QuerySample QueryModel { get; set; } = new QuerySample();
        /// <summary>
        /// 检验项目字典
        /// </summary>
        public List<ProductModel> ProductDict { get; } = new Lazy<List<ProductModel>>(() =>
        {
            var result = ExtendApiDict.Instance.ProductDict?.Where(x => DSTCode.ConvenPathList.Contains(x.id))?.ToList();
            return result;
        }).Value;

        /// <summary>
        /// 选中的样本实体
        /// </summary>
        [Notification]
        public SampleModel SelectedModel
        {
            get => _selectModel;
            set
            {
                _selectModel = value;
                if (_selectModel != null)
                {
                    if (_selectModel.SampleID != PreSelectedSample?.SampleID) // sampid不一样则认为不同样本
                    {
                        PreSelectedSample = _selectModel;
                    }
                    SelectedCommandList?.ForEach(command =>
                    {
                        Application.Current.Dispatcher.InvokeAsync(() => command?.Execute(_selectModel));
                    });
                }
            }
        }
        /// <summary>
        /// 之前选中的样本
        /// </summary>
        private SampleModel PreSelectedSample { get; set; }
        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            PageModel.PageIndex = 1;
            LoadData();
        })).Value;

        /// <summary>
        /// 接收其他viewmodel传来的sampid用于更新选中项
        /// </summary>
        public ICommand ChangeSelectedCommand => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(data =>
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (data != PreSelectedSample?.SampleID)
                {
                    var selectedItem = SampleList?.FirstOrDefault(x => x.SampleID == data);
                    if (selectedItem != null)
                    {
                        PreSelectedSample = selectedItem;
                    }
                    //SampleList = QueryFunc?.Invoke(PageModel, QueryModel); // 更新样本列表
                }
                LoadData();
            }
        })).Value;

        /// <summary>
        /// 选中命令（用于其他viewmodel注册用）
        /// </summary>
        public List<ICommand> SelectedCommandList { get; set; } = new List<ICommand>();

        /// <summary>
        /// 查询函数
        /// </summary>
        public Func<CustomPageModel, QuerySample, List<SampleModel>> QueryFunc { get; set; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="func">注入查询样本函数</param>
        public QueryPathListViewModel(Func<CustomPageModel, QuerySample, List<SampleModel>> func)
        {
            QueryFunc = func;
            //QueryCommand.Execute(null);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        public override async void LoadData()
        {
            WhirlingControlManager.ShowWaitingForm();
            try
            {
                SampleList = await Task.Run(() => QueryFunc?.Invoke(PageModel, QueryModel));//MaterialService.Instance.GetMaterialList(PageModel, QueryModel);
                if (SampleList?.Count > 0) // 找到之前选中的项目，找不到则选择第一个
                {
                    SelectedModel = SampleList?.FirstOrDefault(x => x.SampleID == PreSelectedSample?.SampleID) ?? SampleList?.FirstOrDefault();
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
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
    }
}
