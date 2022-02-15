using DST.ApiClient.Service;
using DST.Database.Model;
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
    public class MaterialAllQueryViewModel : CustomBaseViewModel
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
            RegSampList = RegisterService.Instance.GetRegisterList(PageModel, QueryModel);
        })).Value;

        /// <summary>
        /// 导出
        /// </summary>
        public ICommand ExportCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            //RegisterService.Instance
        })).Value;
    }
}
