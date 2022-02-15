using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class PhysDistReceiptEditViewModel : CustomBaseViewModel
    {
        private Sample sampleArgs = null;

        [Notification]
        public Sample CurSelectedSample { get; set; }

        [Notification]
        public List<ProductModel> ProductModelList { get; set; }

        [Notification]
        public List<DictItem> ScreenList { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        [Notification]
        public List<DictItem> SexDict { get; set; }

        /// <summary>
        /// 当前选中的检查项目
        /// </summary>
        [Notification]
        public ProductModel CurProductModel { get; set; }

        /// <summary>
        /// 检查项目的子项目
        /// </summary>
        [Notification]
        public ProductType CurProductType { get; set; }

        public PhysDistReceiptEditViewModel()
        {
            this.ProductModelList = ExtendApiDict.Instance.ProductDict;
            this.ScreenList = ExtendApiDict.Instance.ActivityTypeDict;
            this.SexDict = ExtendApiDict.Instance.SexDict;
        }

        /// <summary>
        /// 接收传递参数
        /// </summary>
        public override void OnViewLoaded()
        {
            base.OnViewLoaded();

            if(this.Args != null)
            {
                // 由于涉及到取消修改，不能直接绑定传过来的数据。
                this.sampleArgs = this.Args[0] as Sample;
                this.CurSelectedSample = new Sample() { productType = this.sampleArgs.productType, patientName = this.sampleArgs.patientName, age = this.sampleArgs.age, sex = this.sampleArgs.sex, phone = this.sampleArgs.phone, productId = this.sampleArgs.productId, screen = this.sampleArgs.screen, gatherTime = this.sampleArgs.gatherTime, remark = this.sampleArgs.remark};

                this.CurProductModel = this.ProductModelList.FirstOrDefault(x => x.id.Equals(this.CurSelectedSample.productId));
                if(!string.IsNullOrEmpty(this.CurSelectedSample.productType) && 
                   this.CurProductModel != null && 
                   this.CurProductModel.productTypeList != null)
                {
                    this.CurProductType = this.CurProductModel.productTypeList.FirstOrDefault(x => x.id.Equals(this.CurSelectedSample.productType));
                }
            }
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();
            this.CloseCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.SaveCommand = new RelayCommand<object>(data =>
            {
                this.sampleArgs.patientName = this.CurSelectedSample.patientName;
                this.sampleArgs.age = this.CurSelectedSample.age;
                this.sampleArgs.sex = this.CurSelectedSample.sex;
                this.sampleArgs.phone = this.CurSelectedSample.phone;
                this.sampleArgs.screen = this.CurSelectedSample.screen;
                this.sampleArgs.gatherTime = this.CurSelectedSample.gatherTime;
                this.sampleArgs.remark = this.CurSelectedSample.remark;

                if(this.CurProductModel != null)
                {
                    this.sampleArgs.productId = this.CurProductModel.id;
                    this.sampleArgs.productName = this.CurProductModel.name;
                    if(this.CurProductType != null)
                    {
                        this.sampleArgs.productType = this.CurProductType.id;
                        this.sampleArgs.productName = this.CurProductModel.name + "/" + this.CurProductType.name;
                    }
                }

                this.CloseContentWindow();
            });
        }
    }
}
