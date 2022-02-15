using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 临床表现实体
    /// </summary>
    public class PhysDistClinManifModel
    {
        //@ApiModelProperty(value = "病理id")
        public string pathologyId { get; set; }

        //@ApiModelProperty(value = "医院名称")
        public string hospitalName { get; set; }

        //@ApiModelProperty(value = "接收时间")
        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? receiverTime { get; set; }

        //@ApiModelProperty(value = "病人名称")
        public string patientName { get; set; }

        //@ApiModelProperty(value = "年龄")
        public int? age { get; set; }

        //@ApiModelProperty(value = "姓别")
        public string sex { get; set; }

        //@ApiModelProperty(value = "病理号")
        public string code { get; set; }

        //@ApiModelProperty(value = "产品名称集")
        public string productNames { get; set; }

        //@ApiModelProperty(value = "临床诊断")
        public string clinicalDiagnosis { get; set; }

    }
}
