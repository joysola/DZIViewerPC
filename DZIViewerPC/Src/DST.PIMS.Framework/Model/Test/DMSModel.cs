using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model.Test
{
    public class DMSModel
    {
        public string Identity_Type { get; set; }
        public string Pathology_No_Name { get; set; }
        public PatInfo PatientInfo { get; set; }
        public string SendParts { get; set; }
        public int? PartsNum { get; set; }
        public int? BeatNum { get; set; }
        public string DM_Doc { get; set; }
        public int? PreEmbeddingNum { get; set; }
        public string ThingsbyEye { get; set; }
    }
    public class PatInfo
    {
        public string PatName { get; set; }
        public string SampleCode { get; set; }
        public string Sex { get; set; }
        public string Dept { get; set; }
        public string Specimen { get; set; }
    }
    public class CommonTerm
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
