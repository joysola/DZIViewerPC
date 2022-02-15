using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
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
    public class ProdTCTListViewModel : CustomBaseViewModel
    {
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
            return ExtendApiDict.Instance.ProductDict?.Where(x => DSTCode.TechAdCellProdIDList.Contains(x.id)).ToList();
        }).Value;
        /// <summary>
        /// 选中项目
        /// </summary>
        [Notification]
        public SliceTCTModel SelectedModel { get; set; }
        /// <summary>
        /// TCT制片集合
        /// </summary>
        [Notification]
        public ObservableCollection<SliceTCTModel> SliceTCTList { get; set; } = new ObservableCollection<SliceTCTModel>();
        /// <summary>
        /// 全选
        /// </summary>
        public ICommand SelectAllCommand => new Lazy<RelayCommand<bool>>(() => new RelayCommand<bool>(data =>
        {
            foreach (var item in SliceTCTList)
            {
                item.IsSelected = data;
            }
        })).Value;
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.PageModel.PageIndex = 1;
            LoadData();
        })).Value;

        /// <summary>
        /// 打印
        /// </summary>
        public ICommand PrintCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var list = SliceTCTList.Where(x => x.IsSelected);
            if (list?.Count() > 0)
            {
                var result = ProductService.Instance.PrintTCTSliceCodeList(list);
                if (result?.Count > 0)
                {
                    //result.ForEach(x => x.ProductID = DSTCode.TCTProdID); // tct制片所以赋予tct项目id
                    Task.Run(() => PrintCode(result));
                }
                LoadData();
            }
        })).Value;


        public ProdTCTListViewModel()
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
                SliceTCTList.Clear();
                var result = ProductService.Instance.GetSliceTCTList(PageModel, QueryModel);
                if (result?.Count > 0)
                {
                    result?.ForEach(s => SliceTCTList.Add(s));
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

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
