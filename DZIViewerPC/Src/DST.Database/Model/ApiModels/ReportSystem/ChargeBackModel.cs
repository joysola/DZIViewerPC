using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
	/// <summary>
	/// 报告退单数据
	/// </summary>
    public class ChargeBackModel
	{
		public string chargeBackCause { get; set; }
		public DateTime? chargeBackTime { get; set; }
		public string createDept { get; set; }
		public DateTime? createTime { get; set; }
		public string createUser { get; set; }
		public string id { get; set; }
		public int? isDeleted { get; set; }
		public string sampleId { get; set; }
		public int? status { get; set; }
		public DateTime? updateTime { get; set; }
		public string updateUser { get; set; }
	}
}
