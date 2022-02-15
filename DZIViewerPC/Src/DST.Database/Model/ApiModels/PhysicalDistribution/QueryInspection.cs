using DST.Common.Converter;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 送检查询条件
    /// </summary>
    public class QueryInspection : ObservableObject
    {
        /// <summary>
        /// 病理号
        /// </summary>
        [Notification]
        public string code { get; set; }
        /// <summary>
        /// 收货结束时间
        /// </summary>
        [Notification]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<DateTime> endReceivingTime { get; set; }
        /// <summary>
        /// 实验室编号
        /// </summary>
        [Notification]
        public string laboratoryCode { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        [Notification]
        public string mailNo { get; set; }
        /// <summary>
        /// 患者名称
        /// </summary>
        [Notification]
        public string patientName { get; set; }
        /// <summary>
        /// 项目id，检查项目
        /// </summary>
        [Notification]
        public string productId { get; set; }

        /// <summary>
        /// 病理类型：常规、分子、细胞等
        /// </summary>
        public string pathologyType { get; set; }

        /// <summary>
        /// 收货开始时间
        /// </summary>
        [Notification]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<DateTime> startReceivingTime { get; set; }
    }
}
