using DST.Common.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
	/// <summary>
	/// 物流查询条件实体
	/// </summary>
    public class QueryExpress
	{
		/// <summary>
		/// 结束发货日期
		/// </summary>
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public Nullable<DateTime> endDeliveryTime { get; set; }

		/// <summary>
		/// 结束接收日期
		/// </summary>
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public Nullable<DateTime> endReceivingTime { get; set; }

		/// <summary>
		/// 医院名称
		/// </summary>
		public string hospitalName { get; set; }

		/// <summary>
		/// 发货三天未确认收货
		/// </summary>
		public bool isItLongerThanThreeDays { get; set; }

		/// <summary>
		/// 快递单号
		/// </summary>
		public string mailNo { get; set; }

		/// <summary>
		/// 负责人
		/// </summary>
		public string salesUserName { get; set; }

		/// <summary>
		/// 样本异常
		/// </summary>
		public bool sampleAnomaly { get; set; }

		/// <summary>
		/// 开发发货日期
		/// </summary>
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public Nullable<DateTime> startDeliveryTime { get; set; }

		/// <summary>
		/// 开始接收日期
		/// </summary>
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public Nullable<DateTime> startReceivingTime { get; set; }

		/// <summary>
		/// 快递状态
		/// </summary>
		public Nullable<int> status { get; set; }
	}
}
