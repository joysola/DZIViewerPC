using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class ScanQueryModel
	{
		public string adviceType { get; set; }
		public string cellType { get; set; }
		public string laboratoryCode { get; set; }
		public string pathologyCode { get; set; }
		public string patientName { get; set; }
		public string productId { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? productionTimeEnd { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? productionTimeStart { get; set; }

		/// <summary>
		/// 接收状态：0=未接收，1=已接收
		/// </summary>
		public int? receiveStatus { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? scanTimeEnd { get; set; }

		[JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
		public DateTime? scanTimeStart { get; set; }
	}
}
