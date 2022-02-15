using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 特检加测
    /// </summary>
    public class ReportDoctAdvice
	{
		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? applyTime { get; set; }

		public string applyUser { get; set; }
		public string approveStatus { get; set; }
		public string bindingSampleId { get; set; }
		public string createDept { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? createTime { get; set; }
		public string createUser { get; set; }
		public string id { get; set; }
		public int? isDeleted { get; set; }
		public string productId { get; set; }
		
		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? reviewTime { get; set; }
		public string reviewUser { get; set; }
		public string sampleId { get; set; }
		public string screen { get; set; }
		public int? status { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? updateTime { get; set; }
		public string updateUser { get; set; }

		public ObservableCollection<DocAdviceModel> sampleTissueDoctorAdviceList { get; set; } = new ObservableCollection<DocAdviceModel>();
	}

	public class DocAdviceModel
    {
		public string adviceAuditId { get; set; }
		public string adviceType { get; set; }
		public string code { get; set; }
		public string createDept { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? createTime { get; set; }
		public string createUser { get; set; }
		public string id { get; set; }
		public int? isDeleted { get; set; }
		public string marker { get; set; }
		public string remark { get; set; }
		public string sliceShortNumber { get; set; }
		public int? status { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? updateTime { get; set; }
		public string updateUser { get; set; }
		public string waxBlockNumber { get; set; }

		[JsonIgnore]
		public string markerValue { get; set; }
	}
}
