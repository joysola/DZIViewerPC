using DST.Common.Converter;
using DST.Database.Model.DictModel;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 检查项目
    /// </summary>
    public class Sample : ObservableObject
	{
		[Notification]
		public string applicationCode { get; set; }

		[Notification]
		public string barCode { get; set; }

		[Notification]
		public string clinicalManifestation { get; set; }

		[Notification]
		public string code { get; set; }

		[Notification]
		public string costType { get; set; }

		[Notification]
		public string createDept { get; set; }

		[Notification]
		public string createUser { get; set; }

		[Notification]
		public string customerId { get; set; }

		[Notification]
		public string dept { get; set; }

		[Notification]
		public string doctorId { get; set; }

		[Notification]
		public string doctorName { get; set; }

		[Notification]
		public string ecStatus { get; set; }

		[Notification]
		public string enDept { get; set; }

		[Notification]
		public string expressId { get; set; }

		[Notification]
		[JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<DateTime> gatherTime { get; set; }

		[Notification]
		public string hospitalId { get; set; }

		[Notification]
		public string hospitalName { get; set; }

		[Notification]
		public string id { get; set; }

		[Notification]
		public Nullable<int> inspectionStatus { get; set; }

		[Notification]
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public Nullable<DateTime> inspectionTime { get; set; }

		[Notification]
		public Nullable<int> isDeleted { get; set; }

		[Notification]
		public string laboratoryCode { get; set; }

		[Notification]
		public string mailNo { get; set; }

		[Notification]
		public Nullable<DateTime> makeTime { get; set; }
		[Notification]
		public string markers { get; set; }
		[Notification]
		public string pathologyId { get; set; }

		[Notification]
		public string pathologyType { get; set; }

		[Notification]
		public string patientId { get; set; }

		[Notification]
		public string patientName { get; set; }

		[Notification]
		public decimal? price { get; set; }

		[Notification]
		public string productId { get; set; }

		[Notification]
		public string productName { get; set; }

		[Notification]
		public string productType { get; set; }

		[Notification]
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public Nullable<DateTime> receiverTime { get; set; }

		[Notification]
		public string remark { get; set; }

		[Notification]
		public string salesUserId { get; set; }

		[Notification]
		public string salesUserName { get; set; }

		[Notification]
		public string screen { get; set; }

		[Notification]
		public string settlementMode { get; set; }

		[Notification]
		public string settlementStatus { get; set; }

		[Notification]
		public string signForManner { get; set; }

		[Notification]
		public Nullable<int> status { get; set; }

		[Notification]
		public Nullable<int> systemIntegrating { get; set; }

		[Notification]
		public Nullable<int> trialStatus { get; set; }
		//public Nullable<DateTime> updateTime { get; set; }

		[Notification]
		public string updateUser { get; set; }

		[Notification]
		public string inspectionSample { get; set; }

		[Notification]
		public string addTest { get; set; }

		[Notification]
		public int? age { get; set; }

		[Notification]
		public string sex { get; set; }

		[Notification]
		public string phone { get; set; }

		public bool? isSign { get; set; }

		[Notification]
		public string inpatientNumber { get; set; }

		[Notification]
		public string bedNumber { get; set; }


		private bool isSelected = false;

		[JsonIgnore]
		public bool IsSelected
		{
			get { return this.isSelected; }
			set
			{
				this.isSelected = value;
				this.RaisePropertyChanged("IsSelected");
				this.isSign = value;
			}
		}

		/// <summary>
		/// 标记物/染色列表
		/// </summary>
		[JsonIgnore]
		public List<DictItem> ProdReagentDict { get; set; }

		/// <summary>
		/// 标记物名称
		/// </summary>
		[JsonIgnore]
		public string MarkersName { get; set; }

		/// <summary>
		/// 当前选中的标记物/染色
		/// </summary>
		[JsonIgnore]
		public List<DictItem> CurSelectedProdReagent { get; set; } = new List<DictItem>();
	}
}
