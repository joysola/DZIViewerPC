using System.ComponentModel;

namespace DST.PIMS.Framework.Model
{
    /// <summary>
    /// 工作站类别
    /// </summary>
    public enum EnumWorkstationType
    {
        /// <summary>
        /// 前处理工作站，物流确认
        /// </summary>
        [Description("前处理工作站")]
        PhysicalDistribution,

        /// <summary>
        /// 样本登记工作站
        /// </summary>
        [Description("样本登记工作站")]
        Register,

        /// <summary>
        /// 样本取材工作站
        /// </summary>
        [Description("样本取材工作站")]
        Material,

        /// <summary>
        /// 样本包埋工作站
        /// </summary>
        [Description("样本包埋工作站")]
        Embedding,

        /// <summary>
        /// 样本制片工作站
        /// </summary>
        [Description("样本制片工作站")]
        Production,

        /// <summary>
        /// 样本扫描工作站
        /// </summary>
        [Description("扫描管理工作站")]
        Scan,

        /// <summary>
        /// 分配阅片工作站
        /// </summary>
        [Description("分配阅片工作站")]
        Allocation,

        /// <summary>
        /// 常规诊断工作站
        /// </summary>
        [Description("常规诊断工作站")]
        ConventionalDiagnosis,

        /// <summary>
        /// 细胞学诊断工作站
        /// </summary>
        [Description("细胞学诊断工作站")]
        CytoDiagnosis,

        /// <summary>
        /// 分子病理诊断工作站
        /// </summary>
        [Description("分子病理检测工作站")]
        MolecularDiagnosis,

        /// <summary>
        /// 报告管理工作站
        /// </summary>
        [Description("报告管理工作站")]
        ReportSystem,

        /// <summary>
        /// 出勤管理工作站
        /// </summary>
        [Description("出勤管理工作站")]
        Attendance,

        /// <summary>
        /// 档案管理工作站
        /// </summary>
        [Description("档案管理工作站")]
        ArchivesManagement,

        /// <summary>
        /// 远程会诊工作
        /// </summary>
        [Description("远程会诊工作")]
        Telemedicine,

        /// <summary>
        /// 特检审核工作站
        /// </summary>
        [Description("特检审核工作站")]
        SpecialSurveyAudit,

        /// <summary>
        /// 样本上传工作站
        /// </summary>
        [Description("扫描上传工作站")]
        SampleDataUpload,

        /// <summary>
        /// 系统管理
        /// </summary>
        [Description("系统管理")]
        SystemManage,
        /// <summary>
        /// 阅片工作站
        /// </summary>
        [Description("阅片工作站")]
        ImageViewer,
        /// <summary>
        /// 无工作站
        /// </summary>
        [Description("无")]
        None = -1,
    }
}
