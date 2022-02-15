using DST.ApiClient.Service;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class PhysDistReceiptHumanViewModel : CustomBaseViewModel
    {
        private bool selectAll = false;

        /// <summary>
        /// 全选
        /// </summary>
        public bool SelectAll
        {
            get { return this.selectAll; }
            set
            {
                this.selectAll = value;
                this.RaisePropertyChanged("SelectAll");
                if (this.CurPhysDistReceiptHumanModel.sampleVOList != null)
                {
                    this.CurPhysDistReceiptHumanModel.sampleVOList.ToList().ForEach(x =>
                    {
                        x.IsSelected = value;
                    });
                }
            }
        }

        [Notification]
        public string MailNo { get; set; } = string.Empty;

        [Notification]
        public Sample CurSelectedSample { get; set; }

        [Notification]
        public PhysDistReceiptHumanModel CurPhysDistReceiptHumanModel { get; set; }

        [Notification]
        public ICommand EditCommand { get; set; }

        [Notification]
        public ICommand DeleteCommand { get; set; }

        [Notification]
        public ICommand CloseCommand { get; set; }

        [Notification]
        public ICommand SaveCommand { get; set; }

        [Notification]
        public List<DictItem> SexDict { get; set; }

        public PhysDistReceiptHumanViewModel()
        {
            this.SexDict = ExtendApiDict.Instance.SexDict;
        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.EditCommand = new RelayCommand<object>(data =>
            {
                ShowContentWindowMessage msg = new ShowContentWindowMessage("PhysDistReceiptEdit", "编辑");
                msg.DesignWidth = 500;
                msg.DesignHeight = 500;
                msg.Args = new object[] { this.CurSelectedSample };
                Messenger.Default.Send(msg);
            });

            this.DeleteCommand = new RelayCommand<object>(data =>
            {

            });

            this.CloseCommand = new RelayCommand<object>(data =>
            {
                Messenger.Default.Send<object>(data, EnumMessageKey.PhysDistReceiptClose);
            });

            this.SaveCommand = new RelayCommand<object>(data =>
            {
                this.Save();
            });

            Messenger.Default.Register<object[]>(this, EnumMessageKey.PhysDistReceiptArgsSendToHuman, data =>
            {
                if(data != null && data[0] != null)
                {
                    this.MailNo = data[0]?.ToString();
                    this.CurPhysDistReceiptHumanModel = PhysicalDistributionService.Instance.GetSignListByMailNo(this.MailNo.Trim());
                    this.MailNo = "";
                }
            });
        }

        /// <summary>
        /// 手动查询，因为物流编号存在为空的情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(this.MailNo.Trim()))
            {
                this.CurPhysDistReceiptHumanModel = PhysicalDistributionService.Instance.GetSignListByMailNo(this.MailNo.Trim());
            }
        }

        public override void LoadData()
        {
            
        }

        protected override void Save()
        {
            if (null != this.CurPhysDistReceiptHumanModel)
            {
                this.CurPhysDistReceiptHumanModel.amount = this.CurPhysDistReceiptHumanModel.sampleVOList.Count;
                List<PhysDistReceiptBarcodeModel> barcodeList = PhysicalDistributionService.Instance.SaveSignExpressByHand(this.CurPhysDistReceiptHumanModel);
                Task.Run(() => PrintLabelManager.Singleton.Print(barcodeList));
            }

            this.CurPhysDistReceiptHumanModel = null;
            this.MailNo = "";
            Messenger.Default.Send<object>(null, EnumMessageKey.PhysDistReceiptHumanFocus);
        }
    }
}
