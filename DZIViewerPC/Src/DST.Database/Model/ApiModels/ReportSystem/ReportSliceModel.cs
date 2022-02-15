using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 切片信息
    /// </summary>
    public class ReportSliceModel
    {
        public string sampleId { get; set; }
        public string cuttingId { get; set; }
        public string sliceShortNumber { get; set; }
        public string sliceNumber { get; set; }
        public string sampleSpecimenName { get; set; }
        public string inspectionPlace { get; set; }
        public string drawMaterialsPlace { get; set; }
        public string productId { get; set; }
        public string waxBlockNumber { get; set; }
        public string remark { get; set; }
        public string marker { get; set; }
        public string scanImageUrl { get; set; }
        public string printStatus { get; set; }
        public string sort { get; set; }
        public string aiFeatureList { get; set; }
    }
}
