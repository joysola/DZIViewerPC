using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class AllocationModel
    {
        public int? age { get; set; }
        public string hospitalName { get; set; }
        public string inspectionSample { get; set; }
        public string laboratoryCode { get; set; }
        public string pathologyCode { get; set; }
        public string pathologyType { get; set; }
        public string pathologyTypeName { get; set; }
        public string patientName { get; set; }
        public string productId { get; set; }
        public string productName { get; set; }
        public string reportResult { get; set; }
        public string reviewCount { get; set; }
        public string reviewDoctorId { get; set; }
        public string reviewDoctorName { get; set; }
        public string sampleId { get; set; }

        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? scanTime { get; set; }
        public int? sex { get; set; }
        public int? status { get; set; }
    }
}
