using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ProdSliceListViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        [Notification]
        public QuerySample QueryModel { get; set; } = new QuerySample();
        /// <summary>
        /// 制片集合
        /// </summary>
        [Notification]
        public ObservableCollection<SliceProdModel> SliceProdList { get; set; } = new ObservableCollection<SliceProdModel>();
        /// <summary>
        /// 选中项目
        /// </summary>
        [Notification]
        public SliceProdModel SelectedModel { get; set; }
        /// <summary>
        /// 检验项目字典
        /// </summary>
        public List<ProductModel> ProductDict => ExtendApiDict.Instance.ProductDict;
        /// <summary>
        /// 技术医嘱
        /// </summary>
        public List<DictItem> TechAdviceDict => ExtendApiDict.Instance.TechAdviceDict;
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.PageModel.PageIndex = 1;
            LoadData();
        })).Value;
        /// <summary>
        /// 全选
        /// </summary>
        public ICommand SelectAllCommand => new Lazy<RelayCommand<bool>>(() => new RelayCommand<bool>(data =>
        {
            foreach (var item in SliceProdList)
            {
                item.IsSelected = data;
            }
        })).Value;
        /// <summary>
        /// 打印
        /// </summary>
        public ICommand PrintCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var list = SliceProdList.Where(x => x.IsSelected);
            if (list?.Count() > 0)
            {
                var result = ProductService.Instance.PrintSliceCodeList(list);
                if (result.Count > 0)
                {
                    Task.Run(() => PrintCode(result));
                }
                LoadData();
            }
        })).Value;
        
        public ProdSliceListViewModel()
        {
            QueryCommand.Execute(null);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        public override void LoadData()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                SliceProdList.Clear();
                var result = ProductService.Instance.GetSliceProdList(PageModel, QueryModel);
                if (result?.Count > 0)
                {
                    result?.ForEach(s => SliceProdList.Add(s));
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printCodeList"></param>
        private void PrintCode(List<SlicePrintCode> printCodeList)
        {
            //TSCPrintManager.Instance.Print(printCodeList);
            PrintLabelManager.Singleton.Print(printCodeList);
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
