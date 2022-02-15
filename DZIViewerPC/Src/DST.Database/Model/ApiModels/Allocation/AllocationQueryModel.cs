using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class AllocationQueryModel
	{
		public string areaId { get; set; }
		public int confirmStatus { get; set; }
		public List<string> dataHospitalIdList { get; set; }
		public List<string> dataProductIdList { get; set; }
		public string dataReviewDoctorId { get; set; }
		public int doctorView { get; set; }
		public List<string> hospitalIdList { get; set; }
		public string inspectionSample { get; set; }
		public string laboratoryCode { get; set; }
		public string pathologyCode { get; set; }
		public string patientName { get; set; }
		public List<string> productIdList { get; set; }
		public string reportLargeResult { get; set; }
		public DateTime? reportTimeEnd { get; set; }
		public DateTime? reportTimeStart { get; set; }
		public string reviewDoctorId { get; set; }
		public DateTime? scanTimeEnd { get; set; }
		public DateTime? scanTimeStart { get; set; }
		public string sort { get; set; }

		public int startIndex { get; set; }
		public string status { get; set; }
		public string virusInfection { get; set; }
	}
}
