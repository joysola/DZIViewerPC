using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using Nico.Common;
using System;
using System.Collections.Generic;

namespace DST.Database.Model
{
    /// <summary>
    /// 样本上传信息
    /// </summary>
    public class SampleUpload : ObservableObject
    {
        [Notification]
        public int? bucket_id { get; set; }

        [Notification]
        public int? storage_id { get; set; }

        [Notification]
        public int? file_num { get; set; }

        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        [Notification]
        public DateTime? created_time { get; set; }

        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        [Notification]
        public DateTime? updated_time { get; set; }

        [Notification]
        public string id { get; set; }

        [Notification]
        public string url { get; set; }

        [Notification]
        public string sample_code { get; set; }

        [Notification]
        public string sample_path { get; set; }

        [Notification]
        public string key { get; set; }

        [Notification]
        public bool? completed { get; set; }

        [Notification]
        public bool? is_deleted { get; set; }

        [Notification]
        public string index_code { get; set; }

        [Notification]
        public DateTime? completed_time { get; set; }

        private string localPathName;
        private string localPath;
        /// <summary>
        /// 样本本地路径
        /// </summary>
        [JsonIgnore]
        public string LocalPath 
        {
            get { return this.localPath; }
            set
            {
                this.localPath = value;
                this.RaisePropertyChanged("LocalPath");
                if(System.IO.Directory.Exists(this.localPath))
                {
                    this.LocalPathName = new System.IO.DirectoryInfo(this.localPath).Name;
                }
            }
        }

        /// <summary>
        /// 文件夹名称
        /// </summary>
        [JsonIgnore]
        public string LocalPathName
        {
            get { return this.localPathName; }
            set
            {
                this.localPathName = value;
                this.RaisePropertyChanged("LocalPathName");
            }
        }

        private bool isSelected = false;

        [JsonIgnore]
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// 已上传的文件列表
        /// </summary>
        [JsonIgnore]
        [Notification]
        public List<string> HasUploaded { get; set; }

        private int totalpartnumber;
        private int curpartnumber;
        private string percent;
        private int status;

        /// <summary>
        /// 未上传的文件列表
        /// </summary>
        [JsonIgnore]
        [Notification]
        public List<string> UnUploaded { get; set; }

        /// <summary>
        /// 当前已经上传的数量
        /// </summary>
        [JsonIgnore]
        public int CurPartNumber
        {
            get => curpartnumber;
            set
            {
                curpartnumber = value; RaisePropertyChanged("CurPartNumber");
                if (this.TotalPartNumber > 0)
                {
                    this.PERCENT = string.Format("{0:0.00%}", (float)this.curpartnumber / this.TotalPartNumber);
                }
            }
        }

        /// <summary>
        /// 进度条总数值
        /// </summary>
        [JsonIgnore]
        public int TotalPartNumber 
        { 
            get => totalpartnumber; 
            set 
            { totalpartnumber = value; RaisePropertyChanged("TotalPartNumber"); 
            } 
        }

        /// <summary>
        /// 当前进度
        /// </summary>
        [JsonIgnore]
        public string PERCENT 
        { 
            get => percent; 
            set 
            { 
                percent = value; RaisePropertyChanged("Percent"); 
            } 
        }

        /// <summary>
        /// 上传状态：0=未上传，1=上传部分，还未上传完，2=正在上传，3=上传完成
        /// </summary>
        [JsonIgnore]
        public int STATUS { get => status; set { status = value; RaisePropertyChanged("STATUS"); } }

        /// <summary>
        /// 上传日志信息
        /// </summary>
        [Notification]
        [JsonIgnore]
        public string LogInfo { get; set; }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="log"></param>
        public void WriteLog(string log)
        {
            this.LogInfo += $"{DateTime.Now:yyyyMMdd HH:mm:ss} {log}{Environment.NewLine}";
            Logger.Error($"{DateTime.Now:yyyyMMdd HH:mm:ss} {log}{Environment.NewLine}");
        }
    }
}
