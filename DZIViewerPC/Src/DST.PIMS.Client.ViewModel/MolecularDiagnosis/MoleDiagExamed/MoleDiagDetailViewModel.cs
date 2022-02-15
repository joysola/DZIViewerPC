using DST.ApiClient.Service;
using DST.Database.Model;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Client.ViewModel
{
    public class MoleDiagDetailViewModel : CustomBaseViewModel
    {
        [Notification]
        public ExamedModel CurExamedModel { get; set; }

        [Notification]
        public List<SamplePcrResult> CurPcrResultList { get; set; }

        public MoleDiagDetailViewModel()
        {

        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if(this.Args != null)
            {
                this.CurExamedModel = this.Args[0] as ExamedModel;
            }
        }

        public override void LoadData()
        {
            base.LoadData();
            if (this.CurExamedModel != null)
            {
                this.CurPcrResultList = MolecularDiagnosisService.Instance.ListBySampleId(this.CurExamedModel.sampleId);
            }
        }
    }
}
