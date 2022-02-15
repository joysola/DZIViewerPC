using DST.ApiClient.Service;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class PhysDistSigninViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 物流查询条件实体
        /// </summary>
        [Notification]
        public QueryExpress QueryExpressModel { get; set; } = new QueryExpress();

        /// <summary>
        /// 物流收货状态
        /// </summary>
        [Notification]
        public List<DictItem> ExpressStatusDict { get; set; } = new List<DictItem>();

        /// <summary>
        /// 物流信息列表
        /// </summary>
        [Notification]
        public ObservableCollection<Express> ExpressList { get; set; } = new ObservableCollection<Express>();

        /// <summary>
        /// 当前界面选中的物流项
        /// </summary>
        [Notification]
        public Express CurSelectedExpress { get; set; }

        /// <summary>
        /// 搜索按钮命令
        /// </summary>
        public ICommand QueryCommand { get; set; }

        /// <summary>
        /// 查看明细命令
        /// </summary>
        public ICommand DetailCommand { get; set; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public ICommand EditCommand { get; set; }

        /// <summary>
        /// 删除命令
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// 确认名
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public PhysDistSigninViewModel()
        {
            ExtendApiDict.Instance.ExpressStatusDict = DictService.Instance.GetExpressStatusDict().Result;
            this.ExpressStatusDict = ExtendApiDict.Instance.ExpressStatusDict;
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.QueryCommand = new RelayCommand<object>(data =>
            {
                this.PageModel.PageIndex = 1;
                this.LoadData();
            });

            this.DetailCommand = new RelayCommand<object>(data =>
            {
                object ss = this.CurSelectedExpress;
                ShowContentWindowMessage msg = new ShowContentWindowMessage("PhysDistInfo", "查看物流");
                msg.DesignWidth = 1600;
                msg.DesignHeight = 600;
                msg.Args = new object[] { this.CurSelectedExpress.id };
                Messenger.Default.Send(msg);
            });

            this.EditCommand = new RelayCommand<object>(data =>
            {
                ShowContentWindowMessage msg = new ShowContentWindowMessage("PhysDistAdd", "编辑物流");
                msg.DesignWidth = 1500;
                msg.DesignHeight = 700;
                msg.Args = new object[] { this.CurSelectedExpress.id };
                Messenger.Default.Send(msg);
            });

            this.DeleteCommand = new RelayCommand<object>(data =>
            {
                if (null != this.CurSelectedExpress)
                {
                    if(this.CurSelectedExpress.status.HasValue && this.CurSelectedExpress.status.Value == 3)
                    {
                        this.ShowMessageBox("已经确认的物流无法进行删除！");
                        return;
                    }
                    this.ShowMessageBox("确定删除该条数据吗?", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question,
                        dilres => 
                        {
                            if(dilres == System.Windows.MessageBoxResult.OK || dilres == System.Windows.MessageBoxResult.Yes)
                            {
                                bool res = PhysicalDistributionService.Instance.RemoveExpressById(this.CurSelectedExpress.id);
                                if (res)
                                {
                                    this.QueryCommand.Execute(null);
                                }
                            }
                        });
                }

                this.CurSelectedExpress = null;
            });

            // 确认命令
            this.ConfirmCommand = new RelayCommand<object>(data =>
            {
                if (null != this.CurSelectedExpress)
                {
                    ShowContentWindowMessage msg = new ShowContentWindowMessage("PhysDistReceipt", "物流签收");
                    msg.DesignWidth = 1600;
                    msg.DesignHeight = 700;
                    msg.Args = new object[] { this.CurSelectedExpress.mailNo };
                    Messenger.Default.Send(msg);
                }
            });

            Messenger.Default.Register<object>(this, EnumMessageKey.RefreshPhysDistSign, data =>
            {
                this.QueryCommand.Execute(null);
            });
        }

        public override void LoadData()
        {
            ExpressInfo res = PhysicalDistributionService.Instance.PageByExpress(this.QueryExpressModel, this.PageModel.PageIndex, this.PageModel.PageSize);
            if (res != null)
            {
                this.ExpressList = new ObservableCollection<Express>(res.records);
                this.PageModel.TotalPage = res.pages.Value;
                this.PageModel.TotalNum = res.total.Value;
            }
        }

        public void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.QueryCommand.Execute(null);
            }
        }
    }
}
