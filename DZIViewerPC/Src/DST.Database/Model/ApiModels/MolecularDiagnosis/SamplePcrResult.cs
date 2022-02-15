using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class SamplePcrResult
    {
        public string samplePcrId { get; set; }

        public string sampleId { get; set; }

        public string type { get; set; }

        public string result { get; set; }

        public string ranges { get; set; }

        public int? sort { get; set; }
    }
}
