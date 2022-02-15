using DST.ApiClient.Service;
using DST.Database.Model;
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
    public class PhysDistInfoViewModel : CustomBaseViewModel
    {
        [Notification]
        public ICommand CloseCommand { get; set; }

        [Notification]
        public PhysDistInfoModel CurPhysDistInfo { get; set; }

        [Notification]
        public int SampleCount { get; set; }

        public PhysDistInfoViewModel()
        {

        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if(this.Args != null && this.Args.Length == 1 && this.Args[0] != null)
            {
                string id = this.Args[0].ToString();
                this.CurPhysDistInfo = PhysicalDistributionService.Instance.GetDetailsByExpressId(id);
                if (this.CurPhysDistInfo.expressSampleVOList != null)
                {
                    this.SampleCount = this.CurPhysDistInfo.expressSampleVOList.Count;
                    this.CurPhysDistInfo.expressSampleVOList.ForEach(x =>
                    {
                        x.ProdReagentDict = ApplyFormService.Instance.GenerateMarkerList(x.productId);
                        if (!string.IsNullOrEmpty(x.markers))
                        {
                            // 胃镜单独处理
                            if (DSTCode.GastroscopeProdID == x.productId)
                            {
                                x.MarkersName = x.ProdReagentDict.FirstOrDefault(t => t.dictKey.Equals(x.markers))?.dictValue;
                            }
                            else if (DSTCode.ImmuhistchmProdID == x.productId)
                            {
                                string[] arr = x.markers.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < arr.Length; i++)
                                {
                                    if (x.ProdReagentDict.FirstOrDefault(t => t.dictKey.Equals(arr[i])) != null)
                                    {
                                        x.MarkersName += x.ProdReagentDict.FirstOrDefault(t => t.dictKey.Equals(arr[i]))?.dictValue + ",";
                                    }
                                }
                            }
                        }

                        if (x.MarkersName != null && x.MarkersName.EndsWith(","))
                        {
                            x.MarkersName = x.MarkersName.Substring(0, x.MarkersName.Length - 1);
                        }
                    });
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
        }
    }
}
