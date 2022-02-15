using DST.Common.Helper;
using DST.PIMS.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Helper
{
    public class ToolButtonListHelper
    {
        /// <summary>
        /// 各个工作站包含的工具按钮
        /// </summary>
        private static List<ToolButton> allToolButtonList = new List<ToolButton>();

        /// <summary>
        /// 各个工作站包含的工具按钮
        /// </summary>
        public static List<ToolButton> AllToolButtonList => ExtendContext.ExtendAppContext.Current.AllButtons;
        //{
        //    get
        //    {
        //        if (allToolButtonList.Count == 0)
        //        {
        //            ToolButton btnReset = new ToolButton()
        //            {
        //                Content = "重置",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Register }
        //            };
        //            allToolButtonList.Add(btnReset);

        //            ToolButton btnSave = new ToolButton()
        //            {
        //                Content = "保存",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Register, EnumWorkstationType.Material, EnumWorkstationType.ConventionalDiagnosis, EnumWorkstationType.CytoDiagnosis }
        //            };
        //            allToolButtonList.Add(btnSave);

        //            ToolButton btnSaveAndNext = new ToolButton()
        //            {
        //                Content = "保存&下例",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Material }
        //            };
        //            allToolButtonList.Add(btnSaveAndNext);

        //            ToolButton btnSaveandPrint = new ToolButton()
        //            {
        //                Content = "保存&打印",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Register }
        //            };
        //            allToolButtonList.Add(btnSaveandPrint);

        //            ToolButton btnRegection = new ToolButton()
        //            {
        //                Content = "拒收",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { }
        //            };
        //            allToolButtonList.Add(btnRegection);


        //            ToolButton btnPrintBarcode = new ToolButton()
        //            {
        //                Content = "打印条码",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { }
        //            };
        //            allToolButtonList.Add(btnPrintBarcode);


        //            ToolButton btnPrevious = new ToolButton()
        //            {
        //                Content = "上一例",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Material, EnumWorkstationType.Embedding, EnumWorkstationType.ConventionalDiagnosis, EnumWorkstationType.CytoDiagnosis }
        //            };
        //            allToolButtonList.Add(btnPrevious);

        //            ToolButton btnNext = new ToolButton()
        //            {
        //                Content = "下一例",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Material, EnumWorkstationType.Embedding, EnumWorkstationType.ConventionalDiagnosis, EnumWorkstationType.CytoDiagnosis }
        //            };
        //            allToolButtonList.Add(btnNext);

        //            ToolButton btnCollectionRequestDocImg = new ToolButton()
        //            {
        //                Content = "图像采集",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Register }
        //            };
        //            allToolButtonList.Add(btnCollectionRequestDocImg);

        //            ToolButton btnRegisterQuery = new ToolButton()
        //            {
        //                Content = "登记查询",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Register }
        //            };
        //            allToolButtonList.Add(btnRegisterQuery);

        //            ToolButton btnEvaluate = new ToolButton()
        //            {
        //                Content = "评价",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Scan }
        //            };
        //            allToolButtonList.Add(btnEvaluate);

        //            ToolButton btnBatchSendOut = new ToolButton()
        //            {
        //                Content = "批量外送",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Register }
        //            };
        //            allToolButtonList.Add(btnBatchSendOut);

        //            ToolButton btnReceiveInspect = new ToolButton()
        //            {
        //                Content = "接收送检",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { }
        //            };
        //            allToolButtonList.Add(btnReceiveInspect);

        //            ToolButton btnRequestDocShow = new ToolButton()
        //            {
        //                Content = "申请单查看",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Material, EnumWorkstationType.Embedding, EnumWorkstationType.ConventionalDiagnosis, EnumWorkstationType.CytoDiagnosis, EnumWorkstationType.Production }
        //            };
        //            allToolButtonList.Add(btnRequestDocShow);

        //            ToolButton btnDelayMaterials = new ToolButton()
        //            {
        //                Content = "延缓取材",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Material }
        //            };
        //            allToolButtonList.Add(btnDelayMaterials);

        //            ToolButton btnMaterialsQuery = new ToolButton()
        //            {
        //                Content = "取材查询",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Material }
        //            };
        //            allToolButtonList.Add(btnMaterialsQuery);

        //            ToolButton btnEmbeddingQuery = new ToolButton()
        //            {
        //                Content = "包埋查询",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Embedding }
        //            };
        //            allToolButtonList.Add(btnEmbeddingQuery);

        //            ToolButton btnException = new ToolButton()
        //            {
        //                Content = "异常处理",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { }
        //            };
        //            allToolButtonList.Add(btnException);

        //            ToolButton btnAdvice = new ToolButton()
        //            {
        //                Content = "技术医嘱",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Scan, EnumWorkstationType.ArchivesManagement }
        //            };
        //            allToolButtonList.Add(btnAdvice);

        //            //ToolButton btnEmpty = new ToolButton()
        //            //{
        //            //    Content = "空片处理",
        //            //    BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //            //    BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //            //    CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Scan }
        //            //};
        //            //allToolButtonList.Add(btnEmpty);

        //            ToolButton btnBatchAllocation = new ToolButton()
        //            {
        //                Content = "批量分配",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Allocation }
        //            };
        //            allToolButtonList.Add(btnBatchAllocation);

        //            ToolButton btnSchedule = new ToolButton()
        //            {
        //                Content = "确认排班",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Attendance }
        //            };
        //            allToolButtonList.Add(btnSchedule);

        //            ToolButton btnScheduleHistory = new ToolButton()
        //            {
        //                Content = "历史排班",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Attendance }
        //            };
        //            allToolButtonList.Add(btnScheduleHistory);

        //            ToolButton btnScheduleAdjust = new ToolButton()
        //            {
        //                Content = "申请调班",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Attendance }
        //            };
        //            allToolButtonList.Add(btnScheduleAdjust);

        //            ToolButton btnReceiveByScanBarcode = new ToolButton()
        //            {
        //                Content = "扫码接收",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.MolecularDiagnosis }
        //            };
        //            allToolButtonList.Add(btnReceiveByScanBarcode);

        //            ToolButton btnSign = new ToolButton()
        //            {
        //                Content = "签发",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ConventionalDiagnosis, EnumWorkstationType.CytoDiagnosis }
        //            };
        //            allToolButtonList.Add(btnSign);

        //            ToolButton btnBatchImport = new ToolButton()
        //            {
        //                Content = "批量导入",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.MolecularDiagnosis }
        //            };
        //            allToolButtonList.Add(btnBatchImport);

        //            ToolButton btnMedicalHistory = new ToolButton()
        //            {
        //                Content = "电子病历",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ConventionalDiagnosis, EnumWorkstationType.CytoDiagnosis }
        //            };
        //            allToolButtonList.Add(btnMedicalHistory);

        //            ToolButton btnConfirm = new ToolButton()
        //            {
        //                Content = "结果确认",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.MolecularDiagnosis }
        //            };
        //            allToolButtonList.Add(btnConfirm);

        //            ToolButton btnFirstCheck = new ToolButton()
        //            {
        //                Content = "初审",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.MolecularDiagnosis }
        //            };
        //            allToolButtonList.Add(btnFirstCheck);

        //            ToolButton btnSecondCheck = new ToolButton()
        //            {
        //                Content = "复审",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.MolecularDiagnosis }
        //            };
        //            allToolButtonList.Add(btnSecondCheck);

        //            ToolButton btnReportPriview = new ToolButton()
        //            {
        //                Content = "预览报告",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ConventionalDiagnosis, EnumWorkstationType.CytoDiagnosis }
        //            };
        //            allToolButtonList.Add(btnReportPriview);

        //            ToolButton btnReportSupplement = new ToolButton()
        //            {
        //                Content = "补充报告",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ConventionalDiagnosis, EnumWorkstationType.CytoDiagnosis }
        //            };
        //            allToolButtonList.Add(btnReportSupplement);

        //            ToolButton btnReportPrint = new ToolButton()
        //            {
        //                Content = "打印报告",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ReportSystem }
        //            };
        //            allToolButtonList.Add(btnReportPrint);

        //            ToolButton btnBatchSign = new ToolButton()
        //            {
        //                Content = "重新生成",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ReportSystem }
        //            };
        //            allToolButtonList.Add(btnBatchSign);

        //            ToolButton btnRetreat = new ToolButton()
        //            {
        //                Content = "批量退单",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ReportSystem }
        //            };
        //            allToolButtonList.Add(btnRetreat);

        //            ToolButton btnScanArchived = new ToolButton()
        //            {
        //                Content = "扫码归档",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ArchivesManagement }
        //            };
        //            allToolButtonList.Add(btnScanArchived);

        //            ToolButton btnBatchLendOut = new ToolButton()
        //            {
        //                Content = "批量外借",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ArchivesManagement }
        //            };
        //            allToolButtonList.Add(btnBatchLendOut);

        //            ToolButton btnBatchReturn = new ToolButton()
        //            {
        //                Content = "批量归还",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.ArchivesManagement }
        //            };
        //            allToolButtonList.Add(btnBatchReturn);

        //            ToolButton phyDisCon = new ToolButton()
        //            {
        //                Content = "物流签收",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.PhysicalDistribution }
        //            };
        //            allToolButtonList.Add(phyDisCon);

        //            ToolButton phyDisAdd = new ToolButton()
        //            {
        //                Content = "新增物流",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.PhysicalDistribution }
        //            };
        //            allToolButtonList.Add(phyDisAdd);

        //            ToolButton phyDisRequestDoc = new ToolButton()
        //            {
        //                Content = "申请单维护",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.PhysicalDistribution }
        //            };
        //            allToolButtonList.Add(phyDisRequestDoc);

        //            ToolButton phyDisScan = new ToolButton()
        //            {
        //                Content = "扫码送检",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.PhysicalDistribution }
        //            };
        //            allToolButtonList.Add(phyDisScan);

        //            ToolButton phyDisReparBar = new ToolButton()
        //            {
        //                Content = "补打条码",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.PhysicalDistribution }
        //            };
        //            allToolButtonList.Add(phyDisReparBar);

        //            ToolButton checkSendInsp = new ToolButton()
        //            {
        //                Content = "送检确认",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Register }
        //            };
        //            allToolButtonList.Add(checkSendInsp);

        //            ToolButton cancelRegister = new ToolButton()
        //            {
        //                Content = "取消登记",
        //                BackgroundImage = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_unselected),
        //                BackgroundImageMouseOver = ImageHelper.BitmapToBitmapImage(ToolButtonResource.save_selected),
        //                CanIncludedWorkstation = new List<EnumWorkstationType>() { EnumWorkstationType.Register }
        //            };
        //            allToolButtonList.Add(cancelRegister);
        //        }

        //        return allToolButtonList;
        //    }
        //}

    }
}
