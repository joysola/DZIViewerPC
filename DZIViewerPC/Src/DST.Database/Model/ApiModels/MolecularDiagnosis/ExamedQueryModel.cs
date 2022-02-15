using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class ExamedQueryModel
	{
		public string code { get; set; }
		public DateTime? endInspectionTime { get; set; }
		public DateTime? endReceivingTime { get; set; }
		public string importStatus { get; set; }
		public string laboratoryCode { get; set; }
		public string patientName { get; set; }
		public string productId { get; set; }
		public string productType { get; set; }
		public string reportLargeResult { get; set; }
		public DateTime? startInspectionTime { get; set; }
		public DateTime? startReceivingTime { get; set; }
		public string trialStatus { get; set; }
	}
}
