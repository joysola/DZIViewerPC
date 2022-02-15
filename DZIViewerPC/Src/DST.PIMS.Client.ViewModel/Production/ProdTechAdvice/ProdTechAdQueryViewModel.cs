using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
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
    public class ProdTechAdQueryViewModel : CustomBaseViewModel
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
        public List<ProductModel> ProductDict => ExtendApiDict.Instance.ProductDict;

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
            this.PageModel.PageIndex = 1;
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
                }
                LoadData();
            }
        })).Value;

        /// <summary>
        /// 选中命令（用于其他viewmodel注册用）
        /// </summary>
        public List<ICommand> SelectedCommandList { get; set; } = new List<ICommand>();

        public ProdTechAdQueryViewModel()
        {
            // QueryCommand.Execute(null);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        public override async void LoadData()
        {
            WhirlingControlManager.ShowWaitingForm();
            try
            {
                SampleList = await Task.Run(() => ProductService.Instance.GetTechAdviceSampList(PageModel, QueryModel));
                if (SampleList.Count > 0) // 找到之前选中的项目，找不到则选择第一个
                {
                    SelectedModel = SampleList?.FirstOrDefault(x => x.SampleID == PreSelectedSample?.SampleID) ?? SampleList?.First();
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
