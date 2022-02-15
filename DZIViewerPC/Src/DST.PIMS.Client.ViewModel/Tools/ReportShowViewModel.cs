using DST.Common.MinioHelper;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ReportShowViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 当前界面显示的PDF路径
        /// </summary>
        [Notification]
        public string CurShowPdfPath { get; set; }

        /// <summary>
        /// 患者基本信息
        /// </summary>
        [Notification]
        public string PatientInfo { get; set; }

        [Notification]
        public ReportUrl CurReportUrl { get; set; } = new ReportUrl();

        public ReportShowViewModel()
        {
        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if(this.Args != null && this.Args.Length == 2)
            {
                this.PatientInfo = this.Args[0] == null ? "" : this.Args[0].ToString();
                this.CurReportUrl = this.Args[1] as ReportUrl;
            }
        }

        public override void LoadData()
        {
            // 下载PDF文件到本地
            if(this.CurReportUrl != null)
            {
                string localPeport = "";
                string localReportEng = "";

                if(!string.IsNullOrEmpty(this.CurReportUrl.reportUrl))
                {
                    localPeport = ExtendAppContext.Current.SystemTempPath + Path.GetFileName(this.CurReportUrl.reportUrl);
                    if (File.Exists(localPeport))
                    {
                        File.Delete(localPeport);
                    }
                }

                if (!string.IsNullOrEmpty(this.CurReportUrl.reportUrlEnglish))
                {
                    localReportEng = ExtendAppContext.Current.SystemTempPath + Path.GetFileName(this.CurReportUrl.reportUrlEnglish);
                    if (File.Exists(localReportEng))
                    {
                        File.Delete(localReportEng);
                    }
                }

                bool resl = MinioHelper.Client.DownloadFile(this.CurReportUrl.reportUrl, localPeport).Result;
                resl = MinioHelper.Client.DownloadFile(this.CurReportUrl.reportUrlEnglish, localReportEng).Result;
                this.CurReportUrl.LocalReportUrl = localPeport;
                this.CurReportUrl.LocalReportUrlEnglish = localReportEng;
                this.CurShowPdfPath = this.CurReportUrl.LocalReportUrl;
            }
        }
    }
}
