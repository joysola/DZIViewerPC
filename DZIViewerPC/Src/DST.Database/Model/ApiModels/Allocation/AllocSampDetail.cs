using DST.Common.Converter;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Windows;

namespace DST.Database.Model
{
    [NotifyAspect]
    public class AllocSampDetail : ObservableObject
    {
        [JsonProperty("cuttingScanVOList")]
        public List<CuttingScan> CutScanVOList { get; set; }
        /// <summary>
        /// 患者信息
        /// </summary>
        [JsonProperty("patientDetail")]
        public PatDetail PatDetail { get; set; }
        /// <summary>
        /// 报告视野集合
        /// </summary>
        [JsonProperty("reportImageList")]
        public ObservableCollection<ReportImg> ReportImgList { get; set; }
        /// <summary>
        /// 报告结果
        /// </summary>
        [JsonProperty("sampleReportResult")]
        public SampReport SampRprtResult { get; set; }

        [JsonProperty("sampleTctReportResult")]
        public SampTCTRepoResult SampTCTRprtResult { get; set; }
    }
    [NotifyAspect]
    public class CuttingScan : ObservableObject
    {
        [JsonProperty("adviceType")]
        public string AdviceType { get; set; }

        [JsonProperty("adviceTypeName")]
        public string AdvTypeName { get; set; }

        [JsonProperty("aiFeatureList")]
        public List<AIFeature> AIFeatureList { get; set; }

        [JsonProperty("drawMaterialsPlace")]
        public string DrawMaterPlace { get; set; }

        [JsonProperty("inspectionPlace")]
        public string InspPlace { get; set; }

        [JsonProperty("inspectionSample")]
        public string InspSamp { get; set; }

        [JsonProperty("marker")]
        public string Marker { get; set; }

        [JsonProperty("markerName")]
        public string MarkerName { get; set; }

        [JsonProperty("printStatus")]
        public string PrintStatus { get; set; }

        [JsonProperty("productId")]
        public string ProductID { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("sampleId")]
        public string SampleID { get; set; }

        [JsonProperty("sampleSpecimenName")]
        public string SampSpecName { get; set; }

        [JsonProperty("scanImageUrl")]
        public string ScanImgUrl { get; set; }

        [JsonProperty("sliceNumber")]
        public string SliceNum { get; set; }

        [JsonProperty("sliceShortNumber")]
        public string SliceShortNum { get; set; }

        [JsonProperty("sort")]
        public string Sort { get; set; }

        [JsonProperty("waxBlockNumber")]
        public string WaxBlockNum { get; set; }
        /// <summary>
        /// MinIO中切片地址
        /// </summary>
        [JsonIgnore]
        public string SliceImgUrl => $"{ScanImgUrl}/1/0/0.jpg";

    }
    [NotifyAspect]
    public class AIFeature : BaseModel
    {
        [JsonProperty("auto")]
        public string Auto { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("confidenceValue")]
        public string ConfiValue { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("cuttingId")]
        public string CutID { get; set; }

        [JsonProperty("ringList")]
        public string RingList { get; set; }

        [JsonProperty("sampleId")]
        public string SampleID { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        [JsonProperty("thumbUrl")]
        public string ThumbUrl { get; set; }

        [JsonProperty("torchFileName")]
        public string TorchFileName { get; set; }

        [JsonProperty("torchRect")]
        public string TorchRect { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// 转换后的第九层的坐标
        /// </summary>
        [JsonIgnore]
        public (Point point1, Point point2) AIRect { get; set; }
        /// <summary>
        /// 转换后的第九层的坐标
        /// </summary>
        [JsonIgnore]
        public (Point point1, Point point2) AIRect2 { get; set; }
        /// <summary>
        /// AIRect2矩形中心
        /// </summary>
        [JsonIgnore]
        public Point CenterPoint2 => new Point((AIRect2.point1.X + AIRect2.point2.X) / 2, (AIRect2.point1.Y + AIRect2.point2.Y) / 2);
        /// <summary>
        /// 反序列化时，填充AIRect
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (!string.IsNullOrEmpty(TorchRect) && !string.IsNullOrEmpty(TorchFileName))
            {
                // 1. torch坐标
                //var torchRect = JsonConvert.DeserializeObject<int[]>(TorchRect);
                //var torchxystr = TorchFileName.Replace(".jpg", "")?.Split('_').ToList();
                //torchxystr?.RemoveAt(0);
                //var torchxy = torchxystr.Select(x => int.Parse(x)).ToList();
                //var point1 = new Point(256 * torchxy[0] + torchRect[0], 256 * torchxy[1] + torchRect[1]);
                //var point2 = new Point(256 * torchxy[0] + torchRect[2], 256 * torchxy[1] + torchRect[3]);
                //AIRect = (point1, point2);

                // 2. 经纬度坐标
                var ringList = JsonConvert.DeserializeObject<List<List<List<decimal>>>>(RingList);
                if (ringList?.Count == 1 && ringList[0]?.Count == 5)
                {
                    var pontList1 = ringList[0][3];
                    var pontList2 = ringList[0][1];
                    //var pontList3 = ringList[0][0];
                    //var pontList4 = ringList[0][2];
                    if (pontList1?.Count == 2 && pontList2.Count == 2)
                    {
                        var p11 = new Point((double)DSTReviewScanImg.GetPixelX(pontList1[0]), (double)DSTReviewScanImg.GetPixelY(pontList2[1]));
                        var p22 = new Point((double)DSTReviewScanImg.GetPixelX(pontList2[0]), (double)DSTReviewScanImg.GetPixelY(pontList1[1]));
                        //var p33 = new Point((double)DSTReviewScanImg.GetPixel(pontList3[0]), (double)DSTReviewScanImg.GetPixel(-pontList3[1]));
                        //var p44 = new Point((double)DSTReviewScanImg.GetPixel(pontList4[0]), (double)DSTReviewScanImg.GetPixel(-pontList4[1]));
                        AIRect2 = (p11, p22);
                    }
                }
            }
        }
    }


    [NotifyAspect]
    public class PatDetail : ObservableObject
    {
        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("applyTime")]
        public DateTime? ApplyTime { get; set; }

        [JsonProperty("clinicalManifestation")]
        public string CliniManifest { get; set; }

        [JsonProperty("inspectionSample")]
        public string InspecSample { get; set; }

        [JsonProperty("pathologyCode")]
        public string PathCode { get; set; }

        [JsonProperty("patientId")]
        public string patientID { get; set; }


        [JsonProperty("patientName")]
        public string PatientName { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

    }
    /// <summary>
    /// 报告视野实体
    /// </summary>
    [NotifyAspect]
    public class ReportImg : ObservableObject
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("imageCoordinate")]
        public string ImgCoordinate { get; set; }

        [JsonProperty("imageUrl")]
        public string ImgUrl { get; set; }


        [JsonProperty("sampleId")]
        public string SampleID { get; set; }

        [JsonProperty("sliceNumber")]
        public string SliceNum { get; set; }

    }
    [NotifyAspect]
    public class SampReport : ObservableObject
    {

        [JsonProperty("abnormalNote")]
        public string AbnormNote { get; set; }

        [JsonProperty("adviceStatus")]
        public int? AdviceStatus { get; set; }

        [JsonProperty("auditStatus")]
        public int? AuditStatus { get; set; }

        [JsonProperty("confirmStatus")]
        public int? ConfirmStatus { get; set; }

        [JsonProperty("hpvResult")]
        public string HPVResult { get; set; }

        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }

        [JsonProperty("pathologyType")]
        public string PathType { get; set; }

        [JsonProperty("pathologyTypeName")]
        public string PathTypeName { get; set; }

        [JsonProperty("productId")]
        public string ProductID { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("reportDoctor")]
        public string ReportDoctor { get; set; }

        [JsonProperty("reportLargeResult")]
        public string ReportLargeResult { get; set; }

        [JsonProperty("reportResult")]
        public string ReportResult { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        [JsonProperty("reportTime")]
        public DateTime? ReportTime { get; set; }

        [JsonProperty("reportUrl")]
        public string ReportUrl { get; set; }

        [JsonProperty("reportUrlEnglish")]
        public string ReportUrlEng { get; set; }

        [JsonProperty("reviewCount")]
        public int? ReviewCount { get; set; }

        [JsonProperty("reviewDoctor")]
        public string ReviewDoctor { get; set; }

        [JsonProperty("reviewDoctorId")]
        public string ReviewDocID { get; set; }

        [JsonProperty("sampleId")]
        public string SampleID { get; set; }

        [JsonProperty("scanStatus")]
        public int? ScanStatus { get; set; }

        [JsonProperty("scanTime")]
        public DateTime? ScanTime { get; set; }

        [JsonProperty("seenMicroscopically")]
        public string SeenMicroscope { get; set; }

        [JsonProperty("seenNakedEyes")]
        public string SeenNakedEyes { get; set; }
    }
    [NotifyAspect]
    public class SampTCTRepoResult : ObservableObject
    {
        [JsonProperty("cellAmount")]
        public string CellAmount { get; set; }

        [JsonProperty("cervicalCell")]
        public string CerviCell { get; set; }

        [JsonProperty("glandularEpithelialCellResult")]
        public string GlandEpithCellResult { get; set; }

        [JsonProperty("inflammation")]
        public string Inflammation { get; set; }

        [JsonProperty("metaplasiaCell")]
        public string MetaplCell { get; set; }

        [JsonProperty("microorganismProject")]
        public string MicroOrganProject { get; set; }

        [JsonProperty("microorganismProjectList")]
        public List<string> MicroOrganProjList { get; set; }

        [JsonProperty("sampleId")]
        public string SampleID { get; set; }

        [JsonProperty("sampleReportId")]
        public string SampReportID { get; set; }

        [JsonProperty("satisfaction")]
        public string Satisfaction { get; set; }

        [JsonProperty("squamousEpithelium")]
        public string SquamEpithe { get; set; }

        [JsonProperty("squamousEpitheliumResult")]
        public string SquamEpitheResult { get; set; }

        [JsonProperty("virusInfection")]
        public string VirusInfect { get; set; }
    }
}
