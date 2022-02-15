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
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class PhysDistAddViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 当前是否为编辑。新增=true，编辑=false
        /// </summary>
        [Notification]
        public bool IsAdd { get; set; }

        /// <summary>
        /// 性别字典
        /// </summary>
        [Notification]
        public List<DictItem> SexDict { get; set; }

        /// <summary>
        /// 当前选中的项
        /// </summary>
        [Notification]
        public ExpressPathology CurSelectedExpressPathology { get; set; }

        /// <summary>
        /// 新增或编辑的物流对象
        /// </summary>
        [Notification]
        public ExpressDetail CurExpressDetail { get; set; } = new ExpressDetail();

        /// <summary>
        /// 医院的医生，在切换医院时需要刷新
        /// </summary>
        [Notification]
        public ObservableCollection<DoctorInfoModel> DoctorList { get; set; } = new ObservableCollection<DoctorInfoModel>();

        /// <summary>
        /// 保存确认命令
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// 取消命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 编辑检查项目
        /// </summary>
        public ICommand EditItemCommand { get; set; }

        /// <summary>
        /// 复制命令
        /// </summary>
        public ICommand CopyCommand { get; set; }

        /// <summary>
        /// 删除
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        public PhysDistAddViewModel()
        {
            this.SexDict = ExtendApiDict.Instance.SexDict;
        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if(this.Args != null && this.Args.Length == 1 && null != this.Args[0])
            {
                this.IsAdd = false;
                string id = this.Args[0].ToString();
                this.CurExpressDetail = PhysicalDistributionService.Instance.GetDetailsByExpress(id);
                if(this.CurExpressDetail.expressPathologyVOList.Count == 0)
                {
                    this.CurExpressDetail.expressPathologyVOList.Add(new ExpressPathology());
                }
                this.DoctorList = new ObservableCollection<DoctorInfoModel>(DictService.Instance.ListByHospitalId(this.CurExpressDetail.hospitalId));
            }
            else
            {
                this.IsAdd = true;
                this.CurExpressDetail.deliveryDate = DateTime.Now;
                this.CurExpressDetail.expressPathologyVOList.Add(new ExpressPathology() { sex = "2" });
            }

            this.RefreshAmount();
        }

        private void RefreshAmount()
        {
            this.CurExpressDetail.amount = 0;

            if (null != this.CurExpressDetail && null != this.CurExpressDetail.expressPathologyVOList)
            {
                this.CurExpressDetail.expressPathologyVOList.ToList().ForEach(x =>
                {
                    if (x != null && x.sampleList != null)
                    {
                        this.CurExpressDetail.amount += x.sampleList.Count;
                    }
                });
            }
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.SaveCommand = new RelayCommand<object>(data =>
            {
                if(string.IsNullOrEmpty(this.CurExpressDetail.mailNo))
                {
                    this.ShowMessageBox("物流单号不能为空！");
                    return;
                }
                if (this.CurExpressDetail.deliveryDate == null || this.CurExpressDetail.deliveryDate.Value == DateTime.MinValue || this.CurExpressDetail.deliveryDate.Value == DateTime.MaxValue)
                {
                    this.ShowMessageBox("发货日期不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(this.CurExpressDetail.hospitalName))
                {
                    this.ShowMessageBox("医院信息不能为空！");
                    return;
                }

                if (this.CurExpressDetail.expressPathologyVOList.FirstOrDefault(x => x.sampleList == null || x.sampleList.Count == 0) != null)
                {
                    this.ShowMessageBox("检查项目不能为空！");
                    return;
                }

                this.RefreshAmount();
                bool res = PhysicalDistributionService.Instance.SaveExpress(this.CurExpressDetail);
                if(res)
                {
                    Messenger.Default.Send<object>(null, EnumMessageKey.RefreshPhysDistSign);
                    this.CloseContentWindow();
                }
            });

            this.CancelCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.EditItemCommand = new RelayCommand<object>(data =>
            {
                if(string.IsNullOrEmpty(this.CurExpressDetail.hospitalId))
                {
                    this.ShowMessageBox("请先选择医院！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ShowContentWindowMessage msg = new ShowContentWindowMessage("PhysDistAddEditItem", "编辑项目");
                msg.DesignWidth = 1200;
                msg.DesignHeight = 450;
                msg.Args = new object[] { this.CurExpressDetail.hospitalId, this.CurSelectedExpressPathology.sampleList };
                msg.CallBackCommand = new RelayCommand<List<Sample>>(res => 
                {
                    if (res != null)
                    {
                        this.CurSelectedExpressPathology.sampleList = res;
                        this.RefreshAmount();
                    }
                });
                Messenger.Default.Send(msg);
            });

            this.CopyCommand = new RelayCommand<object>(data =>
            {
                ExpressPathology newExpPath = new ExpressPathology(); // JsonConvert.DeserializeObject<ExpressPathology>(JsonConvert.SerializeObject(this.CurSelectedExpressPathology));
                newExpPath.id = null;
                newExpPath.sex = "2";
                this.CurExpressDetail.expressPathologyVOList.Add(newExpPath);
                this.CurSelectedExpressPathology = newExpPath;
                this.RefreshAmount();
            });

            this.DeleteCommand = new RelayCommand<object>(data =>
            {
                this.CurExpressDetail.expressPathologyVOList.Remove(this.CurSelectedExpressPathology);
                this.CurSelectedExpressPathology = null;
                this.RefreshAmount();

                if (this.CurExpressDetail.expressPathologyVOList.Count == 0)
                {
                    this.CopyCommand.Execute(null);
                }
            });
        }

        public void ShowHostitalDict()
        {
            ShowContentWindowMessage msg = new ShowContentWindowMessage("HospitalDict", "选择医院");
            msg.DesignWidth = 800;
            msg.DesignHeight = 450;
            msg.CallBackCommand = new RelayCommand<object>(res =>
            {
                HospitalModel hosp = res as HospitalModel;
                if(null != hosp)
                {
                    this.CurExpressDetail.hospitalName = hosp.name;
                    this.CurExpressDetail.hospitalId = hosp.id;

                    this.DoctorList = new ObservableCollection<DoctorInfoModel>(DictService.Instance.ListByHospitalId(hosp.id));
                }
            });
            Messenger.Default.Send(msg);
        }
    }
}
