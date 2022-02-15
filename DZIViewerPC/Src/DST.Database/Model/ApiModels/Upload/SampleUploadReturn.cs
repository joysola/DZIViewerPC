using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class SampleUploadReturn
    {
        public bool success { get; set; }

        public List<SampleUpload> result { get;set;}

        public string msg { get; set; }
    }

    public class ImageUploadReturn
    {
        public bool success { get; set; }

        public List<string> result { get; set; }

        public string msg { get; set; }
    }
}
