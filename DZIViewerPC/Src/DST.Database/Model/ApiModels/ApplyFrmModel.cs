using DST.Common.Converter;
using DST.Database.Model.DictModel;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 申请单信息
    /// </summary>
    public class ApplyFrmModel : BaseModel
    {
        /// <summary>
        /// 末次生产或流产日期
        /// </summary>
        [JsonProperty("abortionDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Notification]
        public DateTime? AbortionDate { get; set; }

        /// <summary>
        /// 活动度
        /// </summary>
        [JsonProperty("activity")]
        [Notification]
        public string Activity { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [JsonProperty("age")]
        [Notification]
        public int? Age { get; set; }
        /// <summary>
        /// 床位号
        /// </summary>
        [JsonProperty("bedNumber")]
        [Notification]
        public string BedNumber { get; set; }
        /// <summary>
        /// 出血量
        /// </summary>
        [JsonProperty("bloodLoss")]
        [Notification]
        public string BloodLoss { get; set; }
        /// <summary>
        /// 临床诊断
        /// </summary>
        [JsonProperty("clinicalDiagnosis")]
        [Notification]
        public string ClinicDiagnosis { get; set; }
        /// <summary>
        /// 费用类别
        /// </summary>
        [JsonProperty("costCategory")]
        [Notification]
        public string CostCategory { get; set; }
        /// <summary>
        /// 周期
        /// </summary>
        [JsonProperty("cycle")]
        [Notification]
        public string Cycle { get; set; }
        /// <summary>
        /// 诊断
        /// </summary>
        [JsonProperty("diagnosis")]
        [Notification]
        public string Diagnosis { get; set; }
        /// <summary>
        /// 送检医生
        /// </summary>
        [JsonProperty("doctorId")]
        [Notification]
        public string DoctorID { get; set; }
        /// <summary>
        /// 送检医生名称
        /// </summary>
        [JsonProperty("doctorName")]
        [Notification]
        public string DoctorName { get; set; }
        /// <summary>
        /// 内分泌治疗剂量
        /// </summary>
        [JsonProperty("dose")]
        [Notification]
        public string Dose { get; set; }
        /// <summary>
        /// 持续时间
        /// </summary>
        [JsonProperty("duration")]
        [Notification]
        public string Duration { get; set; }
        /// <summary>
        /// 生长速度
        /// </summary>
        [JsonProperty("growthRate")]
        [Notification]
        public string GrowRate { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        [JsonProperty("hospitalId")]
        public string HospitalID { get; set; }
        /// <summary>
        /// 病区
        /// </summary>
        [JsonProperty("inpatientArea")]
        [Notification]
        public string InPatArea { get; set; }
        /// <summary>
        /// 住院号
        /// </summary>
        [JsonProperty("inpatientNumber")]
        [Notification]
        public string InPatNO { get; set; }
        /// <summary>
        /// 本次检查要求
        /// </summary>
        [JsonProperty("inspectionRequirements")]
        [Notification]
        public string InspectRequires { get; set; }
        /// <summary>
        /// 用何内分泌制剂治疗
        /// </summary>
        [JsonProperty("internalSecretion")]
        [Notification]
        public string InterSecret { get; set; }
        /// <summary>
        /// 内分泌治疗日期
        /// </summary>
        [JsonProperty("internalSecretionDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Notification]
        public DateTime? InterSecretDate { get; set; }


        /// <summary>
        /// 实验室编号 月日+序号  
        /// </summary>
        [JsonProperty("laboratoryCode")]
        [Notification]
        public string LabCode { get; set; }
        /// <summary>
        /// 实验室检查（肿瘤指标、临检、生化、免疫等）
        /// </summary>
        [JsonProperty("laboratoryTests")]
        [Notification]
        public string LabTests { get; set; }
        /// <summary>
        /// 实验室检查（肿瘤指标、临检、生化、免疫等）
        /// </summary>
        [JsonProperty("pathologicalExamination")]
        [Notification]
        public string PathExam { get; set; }
        /// <summary>
        /// 末次月经时间
        /// </summary>
        [JsonProperty("lastMensesDate")]
        [Notification]
        public DateTime? LastMensesDate { get; set; }
        /// <summary>
        /// 婚姻(已婚 1、未婚 2、离异 3、不详 4)
        /// </summary>
        [JsonProperty("marriage")]
        [Notification]
        public int? Marriage { get; set; }
        /// <summary>
        /// 病史摘要及临床检查所见
        /// </summary>
        [JsonProperty("medicalHistory")]
        [Notification]
        public string MedHistory { get; set; }
        /// <summary>
        /// 有无转移或可疑的转移
        /// </summary>
        [JsonProperty("metastases")]
        [Notification]
        public string Metastases { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        [JsonProperty("occupation")]
        [Notification]
        public string Occupation { get; set; }
        /// <summary>
        /// 手术名称及手术所见
        /// </summary>
        [JsonProperty("operationFindings")]
        [Notification]
        public string OperFindings { get; set; }
        /// <summary>
        /// 病理编号
        /// </summary>
        [JsonProperty("Code")]
        [Notification]
        public string PathCode { get; set; }

        /// <summary>
        /// 病理id
        /// </summary>
        [JsonProperty("pathologyId")]
        [Notification]
        public string PathID { get; set; }
        /// <summary>
        /// 医院病理号
        /// </summary>
        [JsonProperty("pathologyNumber")]
        [Notification]
        public string PathNumber { get; set; }
        /// <summary>
        /// 病人类别 1  门诊 2 住院
        /// </summary>
        [JsonProperty("patientCategory")]
        [Notification]
        public string PatCategory { get; set; }
        /// <summary>
        /// 病人id
        /// </summary>
        [JsonProperty("patientId")]
        [Notification]
        public string PatientID { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        [JsonProperty("patientName")]
        [Notification]
        public string PatientName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonProperty("phone")]
        [Notification]
        public string Phone { get; set; }
        /// <summary>
        /// 部位
        /// </summary>
        [JsonProperty("place")]
        [Notification]
        public string Place { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        [JsonProperty("placeSize")]
        [Notification]
        public string PlaceSize { get; set; }
        /// <summary>
        /// 小便妊娠反应
        /// </summary>
        [JsonProperty("pregnancyReaction")]
        [Notification]
        public string PregReact { get; set; }
        /// <summary>
        /// 项目名称集
        /// </summary>
        [JsonProperty("productNames")]
        [Notification]
        public string ProductNames { get; set; }
        /// <summary>
        /// 有关X光摄片号数
        /// </summary>
        [JsonProperty("rayNumber")]
        [Notification]
        public string RayNum { get; set; }
        /// <summary>
        /// 坚度
        /// </summary>
        [JsonProperty("rigidity")]
        [Notification]
        public string Rigidity { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [JsonProperty("sex")]
        [Notification]
        public string Sex { get; set; }
        /// <summary>
        /// 形状
        /// </summary>
        [JsonProperty("shape")]
        [Notification]
        public string Shape { get; set; }
        /// <summary>
        /// 色泽
        /// </summary>
        [JsonProperty("tinct")]
        [Notification]
        public string Tinct { get; set; }
        /// <summary>
        /// 刮宫日期
        /// </summary>
        [JsonProperty("uterineCurettageDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Notification]
        public DateTime? UterCuretDate { get; set; }
        /// <summary>
        /// 取样时间
        /// </summary>
        [JsonProperty("gatherTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Notification]
        public DateTime? GatherTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 项目列表
        /// </summary>
        [Notification]
        [JsonProperty("pathologyFormSampleVOList")]
        public ObservableCollection<PathSampInfo> PathSampInfoList { get; set; } = new ObservableCollection<PathSampInfo>();

        /// <summary>
        /// 序列化时，求解ProductNames
        /// </summary>
        /// <param name="context"></param>
        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            if (PathSampInfoList?.Count > 0)
            {
                var names = PathSampInfoList.Select(x => x.ProductName);
                ProductNames = string.Join(",", names);
            }
        }
    }

    /// <summary>
    /// 病理样本项目
    /// </summary>
    public class PathSampInfo : BaseModel
    {
        /// <summary>
        /// 是否加测项目
        /// </summary>
        [Notification]
        [JsonProperty("addTest")]
        public string AddTest { get; set; }

        /// <summary>
        /// 申请单编号
        /// </summary>
        [Notification]
        [JsonProperty("applicationCode")]
        public string AppCode { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        [JsonProperty("barCode")]
        public string BarCode { get; set; }
        /// <summary>
        /// 临床表现
        /// </summary>
        [Notification]
        [JsonProperty("clinicalManifestation")]
        public string ClinicManifest { get; set; }
        /// <summary>
        /// 样本编码
        /// </summary>
        [Notification]
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 费用类型
        /// </summary>
        [Notification]
        [JsonProperty("costType")]
        public string CostType { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [Notification]
        [JsonProperty("dept")]
        public string Dept { get; set; }
        /// <summary>
        /// 送检医生
        /// </summary>
        [Notification]
        [JsonProperty("doctorId")]
        public string DoctorID { get; set; }
        /// <summary>
        /// 送检医生名称
        /// </summary>
        [Notification]
        [JsonProperty("doctorName")]
        public string DoctorName { get; set; }
        /// <summary>
        /// 是否电商
        /// </summary>
        [Notification]
        [JsonProperty("ecStatus")]
        public string EcStatus { get; set; }
        /// <summary>
        /// 科室英文
        /// </summary>
        [Notification]
        [JsonProperty("enDept")]
        public string EnDept { get; set; }
        /// <summary>
        /// 物流id
        /// </summary>
        [Notification]
        [JsonProperty("expressId")]
        public string ExpressID { get; set; }
        /// <summary>
        /// 取样时间
        /// </summary>
        [JsonProperty("gatherTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Notification]
        public DateTime? GatherTime { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        [JsonProperty("hospitalId")]
        [Notification]
        public string HospitalID { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        [JsonProperty("hospitalName")]
        [Notification]
        public string HospitalName { get; set; }
        /// <summary>
        /// 送检样本
        /// </summary>
        [JsonProperty("inspectionSample")]
        [Notification]
        public string InspecSample { get; set; }
        /// <summary>
        /// 送检状态 0：未送检 1：已送检
        /// </summary>
        [JsonProperty("inspectionStatus")]
        [Notification]
        public string InspectStatus { get; set; }
        /// <summary>
        /// 样本送检时间
        /// </summary>
        [JsonProperty("inspectionTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Notification]
        public DateTime? InspecTime { get; set; }

        /// <summary>
        /// 实验室编号 月日+序号  
        /// </summary>
        [JsonProperty("laboratoryCode")]
        [Notification]
        public string LabCode { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        [JsonProperty("mailNo")]
        [Notification]
        public string MailNo { get; set; }
        /// <summary>
        /// 制片完成时间
        /// </summary>
        [JsonProperty("makeTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Notification]
        public DateTime? MakeTime { get; set; }
        /// <summary>
        /// 多个标记物,逗号隔开
        /// </summary>
        [JsonProperty("markers")]
        [Notification]
        public string Markers { get; set; }

        /// <summary>
        /// 病理id
        /// </summary>
        [JsonProperty("pathologyId")]
        [Notification]
        public string PathID { get; set; }
        /// <summary>
        /// 病理类型
        /// </summary>
        [JsonProperty("pathologyType")]
        [Notification]
        public string PathType { get; set; }
        /// <summary>
        /// 病人id
        /// </summary>
        [JsonProperty("patientId")]
        [Notification]
        public string PatientID { get; set; }
        /// <summary>
        /// 病人名称
        /// </summary>
        [JsonProperty("patientName")]
        [Notification]
        public string PatientName { get; set; }
        /// <summary>
        /// 费用价格
        /// </summary>
        [JsonProperty("price")]
        [Notification]
        public decimal? Price { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [JsonProperty("productId")]
        [Notification]
        public string ProductID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [JsonProperty("productName")]
        [Notification]
        public string ProductName { get; set; }

        /// <summary>
        /// 项目子类型
        /// </summary>
        [JsonProperty("productType")]
        [Notification]
        public string ProductType { get; set; }
        /// <summary>
        /// 接收时间
        /// </summary>
        [JsonProperty("receiverTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Notification]
        public DateTime? ReceiveTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("remark")]
        [Notification]
        public string Remark { get; set; }
        /// <summary>
        /// 销售负责人id
        /// </summary>
        [JsonProperty("salesUserId")]
        [Notification]
        public string SaleUserID { get; set; }
        /// <summary>
        /// 销售代表名称
        /// </summary>
        [JsonProperty("salesUserName")]
        [Notification]
        public string SaleUserName { get; set; }
        /// <summary>
        /// 活动状态  0否 1 两癌 2 三八妇女节
        /// </summary>
        [JsonProperty("screen")]
        [Notification]
        public string Screen { get; set; }


        /// <summary>
        /// 结算方式
        /// </summary>
        [JsonProperty("settlementMode")]
        [Notification]
        public string SettleMode { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        [JsonProperty("settlementStatus")]
        [Notification]
        public string SettleStatus { get; set; }
        /// <summary>
        /// 签收方式 0:人工签收 1:扫码签收
        /// </summary>
        [JsonProperty("signForManner")]
        [Notification]
        public string SignManner { get; set; }


        /// <summary>
        /// 是否是系统对接样本 0:否 1:是
        /// </summary>
        [JsonProperty("systemIntegrating")]
        [Notification]
        public string SysIntegration { get; set; }
        /// <summary>
        /// 实验状态
        /// </summary>
        [JsonProperty("trialStatus")]
        [Notification]
        public string TrialStatus { get; set; }


        /// <summary>
        /// 送检部位列表
        /// </summary>
        [Notification]
        [JsonProperty("sampleSpecimenList")]
        public ObservableCollection<SampSpecInfo> SampSpecList { get; set; } = new ObservableCollection<SampSpecInfo>();

        /// <summary>
        /// 检验项目code（不需要传输）
        /// </summary>
        [JsonIgnore]
        public string ProductCode { get; set; }
        /// <summary>
        /// 标记物/染色剂 集合
        /// </summary>
        [JsonIgnore]
        [Notification]
        public List<DictItem> MarkerList { get; set; } = new List<DictItem>();
        /// <summary>
        /// 标记物、染色剂集合
        /// </summary>
        [JsonIgnore]
        [Notification]
        public List<string> SelectedMarkers { get; set; } = new List<string>();

        /// <summary>
        /// 反序列化时，填充SelectedMarkers
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (ProductID == ImmuhistchmProdID)
            {
                SelectedMarkers = Markers?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
            }
        }
        /// <summary>
        /// 序列化时，合并求解Markers
        /// </summary>
        /// <param name="context"></param>
        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            if (ProductID == ImmuhistchmProdID)
            {
                Markers = string.Join(",", SelectedMarkers);
            }
        }
        /// <summary>
        /// 获取DSTCode的免疫组化id
        /// </summary>
        public static string ImmuhistchmProdID { get; } = new Lazy<string>(() =>
        {
            var dstCode = Type.GetType("DST.PIMS.Framework.ExtendContext.DSTCode,DST.PIMS.Framework");
            var ImmuhistchmProdIDField = dstCode.GetField("ImmuhistchmProdID", BindingFlags.Static | BindingFlags.Public);
            return (string)ImmuhistchmProdIDField?.GetValue(null);
        }).Value;
    }


    /// <summary>
    /// 送检部位信息
    /// </summary>
    public class SampSpecInfo : BaseModel
    {
        private string _name;
        /// <summary>
        /// 关联字典名称Key值
        /// </summary>
        [JsonProperty("dictKey")]
        [Notification]
        public string DictKey { get; set; }


        /// <summary>
        /// 标本名称
        /// </summary>
        [JsonProperty("name")]
        [Notification]
        public string Name
        {
            get => _name;
            set
            {
                if (value?.Length > 4 && !string.IsNullOrEmpty(ProductID) && ProductID == GastroscopeProdID) // 胃镜只能填写4位名称
                {
                    _name = value.Substring(0, 4);
                }
                else
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        [JsonProperty("number")]
        [Notification]
        public int? Number { get; set; }
        /// <summary>
        /// 样本ID
        /// </summary>
        [JsonProperty("sampleId")]
        [Notification]
        public string SampleID { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [JsonProperty("sort")]
        [Notification]
        public int? Sort { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [JsonIgnore]
        public string ProductID { get; set; }
        /// <summary>
        /// 胃镜项目id
        /// </summary>
        [JsonIgnore]
        public string GastroscopeProdID { get; set; }
    }

    /// <summary>
    /// 申请打印实体
    /// </summary>
    public class AppFrmPrintCode
    {
        /// <summary>
        /// 患者姓名
        /// </summary>
        [JsonProperty("patientName")]
        public string PatientName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [JsonProperty("age")]
        public int? Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [JsonProperty("sex")]
        public string Sex { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        [JsonProperty("hospitalName")]
        public string HospName { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        [JsonProperty("provinceName")]
        public string ProvinceName { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        [JsonProperty("cityName")]
        public string CityName { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        [JsonProperty("areaName")]
        public string AreaName { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 实验室编号
        /// </summary>
        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        /// <summary>
        /// 项目子类
        /// </summary>
        [JsonProperty("productType")]
        public string ProductType { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        [JsonProperty("hospitalId")]
        public string HospID { get; set; }
        /// <summary>
        /// 外送条码号(一维码)
        /// </summary>
        [JsonProperty("barCode")]
        public string BarCode { get; set; }
    }
}
