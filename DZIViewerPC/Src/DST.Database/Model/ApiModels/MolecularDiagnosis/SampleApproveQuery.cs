using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class SampleApproveQuery
	{
		/// <summary>
		/// 1=初审，3=复核
		/// </summary>
		public int auditStatus { get; set; } = 1;
		public string laboratoryCode { get; set; } = "";
		public string name { get; set; } = "";
		public string productId { get; set; } = "";
		public string productType { get; set; } = "";
		public string reportLargeResult { get; set; } = "";
	}
}
