using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
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
    public class RegisterAllQueryViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 查询实体
        /// </summary>
        [Notification]
        public QueryAllRegModel QueryModel { get; set; } = new QueryAllRegModel();
        /// <summary>
        /// 检查项目字典
        /// </summary>
        public List<ProductModel> ProductDict => ExtendApiDict.Instance.ProductDict;
        /// <summary>
        /// 登记列表
        /// </summary>
        [Notification]
        public List<SampleRegisterModel> RegSampList { get; set; } = new List<SampleRegisterModel>();
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            PageModel.PageIndex = 1;
            LoadData();
        })).Value;

        /// <summary>
        /// 导出
        /// </summary>
        public ICommand ExportCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            //RegisterService.Instance
        })).Value;
        
        public RegisterAllQueryViewModel()
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
                RegSampList = RegisterService.Instance.GetRegisterList(PageModel, QueryModel);
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
