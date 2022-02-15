using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Client.ViewModel.Upload
{
    public class UploadLogViewModel : CustomBaseViewModel
    {
        [Notification]
        public string LogInfo { get; set; }

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1)
            {
                this.LogInfo = (string)this.Args[0];
            }
        }
    }
}
