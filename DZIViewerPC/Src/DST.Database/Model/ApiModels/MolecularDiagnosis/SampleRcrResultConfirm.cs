using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class SampleRcrResultConfirm
    {
        /// @ApiModelProperty(value = "实验室编号")
        public string laboratoryCode { get; set; }

        // @ApiModelProperty(value = "姓名")
        public string patientName { get; set; }

        // @ApiModelProperty(value = "类型")
        public string type { get; set; }

        //@ApiModelProperty(value = "结果 阴性阳性")
        public string result { get; set; }

        //@ApiModelProperty(value = "范围")
        public string ranges { get; set; }
    }
}
