using DST.Common.Helper;
using DST.PIMS.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model
{
    /// <summary>
    /// 按钮枚举
    /// </summary>
    public enum EnumToolButtonType
    {
        /// <summary>
        /// 重置
        /// </summary>
        [Description("重置")]
        [ToolButton("重置", nameof(ToolButtonResource.Reset), nameof(ToolButtonResource.Reset))]
        Reset,
        /// <summary>
        /// 保存
        /// </summary>
        [Description("保存")]
        [ToolButton("保存", nameof(ToolButtonResource.Save), nameof(ToolButtonResource.Save))]
        Save,
        /// <summary>
        /// 保存&打印
        /// </summary>
        [Description("保存&打印")]
        [ToolButton("保存&打印", nameof(ToolButtonResource.SavePrint), nameof(ToolButtonResource.SavePrint))]
        SavePrint,
        /// <summary>
        /// 保存&下例
        /// </summary>
        [Description("保存&下例")]
        [ToolButton("保存&下例", nameof(ToolButtonResource.SaveNext), nameof(ToolButtonResource.SaveNext))]
        SaveNext,
        /// <summary>
        /// 拒收
        /// </summary>
        [Description("拒收")]
        [ToolButton("拒收", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        RejectExpress,
        /// <summary>
        /// 打印条码
        /// </summary>
        [Description("打印条码")]
        [ToolButton("打印条码", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        PrintCode,
        /// <summary>
        /// 上一例
        /// </summary>
        [Description("上一例")]
        [ToolButton("上一例", nameof(ToolButtonResource.PreviousOne), nameof(ToolButtonResource.PreviousOne))]
        PreviousOne,
        /// <summary>
        /// 下一例
        /// </summary>
        [Description("下一例")]
        [ToolButton("下一例", nameof(ToolButtonResource.NextOne), nameof(ToolButtonResource.NextOne))]
        NextOne,
        /// <summary>
        /// 图像采集
        /// </summary>
        [Description("申请单采集")]
        [ToolButton("申请单采集", nameof(ToolButtonResource.CollectAppFrmImage), nameof(ToolButtonResource.CollectAppFrmImage))]
        CollectAppFrmImage,
        /// <summary>
        /// 登记查询
        /// </summary>
        [Description("登记查询")]
        [ToolButton("登记查询", nameof(ToolButtonResource.QueryRegistration), nameof(ToolButtonResource.QueryRegistration))]
        QueryRegistration,
        /// <summary>
        /// 评价
        /// </summary>
        [Description("评价")]
        [ToolButton("评价", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        Evaluate,
        /// <summary>
        /// 批量外送
        /// </summary>
        [Description("批量外送")]
        [ToolButton("批量外送", nameof(ToolButtonResource.BatchSendInsp), nameof(ToolButtonResource.BatchSendInsp))]
        BatchSendInsp,
        /// <summary>
        /// 接收送检
        /// </summary>
        [Description("接收送检")]
        [ToolButton("接收送检", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        ReceiveInsp,
        /// <summary>
        /// 申请单查看
        /// </summary>
        [Description("申请单查看")]
        [ToolButton("申请单查看", nameof(ToolButtonResource.ViewAppFrm), nameof(ToolButtonResource.ViewAppFrm))]
        ViewAppFrm,
        /// <summary>
        /// 延缓取材
        /// </summary>
        [Description("延缓取材")]
        [ToolButton("延缓取材", nameof(ToolButtonResource.DelayDrawMaterial), nameof(ToolButtonResource.DelayDrawMaterial))]
        DelayDrawMaterial,
        /// <summary>
        /// 取材查询
        /// </summary>
        [Description("取材查询")]
        [ToolButton("取材查询", nameof(ToolButtonResource.QueryDrawMaterial), nameof(ToolButtonResource.QueryDrawMaterial))]
        QueryDrawMaterial,
        /// <summary>
        /// 包埋查询
        /// </summary>
        [Description("包埋查询")]
        [ToolButton("包埋查询", nameof(ToolButtonResource.QueryEmbed), nameof(ToolButtonResource.QueryEmbed))]
        QueryEmbed,
        /// <summary>
        /// 异常处理
        /// </summary>
        [Description("异常处理")]
        [ToolButton("异常处理", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        ProcessException,
        /// <summary>
        /// 技术医嘱
        /// </summary>
        [Description("技术医嘱")]
        [ToolButton("技术医嘱", nameof(ToolButtonResource.TechAdvice), nameof(ToolButtonResource.TechAdvice))]
        TechAdvice,
        /// <summary>
        /// 批量分配
        /// </summary>
        [Description("批量分配")]
        [ToolButton("批量分配", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        BatchDistribution,
        /// <summary>
        /// 确认排班
        /// </summary>
        [Description("确认排班")]
        [ToolButton("确认排班", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        CheckSchedule,
        /// <summary>
        /// 历史排班
        /// </summary>
        [Description("历史排班")]
        [ToolButton("历史排班", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        HistorySchedule,
        /// <summary>
        /// 申请调班
        /// </summary>
        [Description("申请调班")]
        [ToolButton("申请调班", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        ApplyAdjustSchedule,
        /// <summary>
        /// 扫码接收
        /// </summary>
        [Description("扫码接收")]
        [ToolButton("扫码接收", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        ScanCodeReceive,
        /// <summary>
        /// 签发
        /// </summary>
        [Description("签发")]
        [ToolButton("签发", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        SigninSend,
        /// <summary>
        /// 批量导入
        /// </summary>
        [Description("批量导入")]
        [ToolButton("批量导入", nameof(ToolButtonResource.BatchImport), nameof(ToolButtonResource.BatchImport))]
        BatchImport,
        /// <summary>
        /// 电子病历
        /// </summary>
        [Description("电子病历")]
        [ToolButton("电子病历", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        EMR,
        /// <summary>
        /// 结果确认
        /// </summary>
        [Description("结果确认")]
        [ToolButton("结果确认", nameof(ToolButtonResource.CheckResult), nameof(ToolButtonResource.CheckResult))]
        CheckResult,
        /// <summary>
        /// 初审
        /// </summary>
        [Description("初审")]
        [ToolButton("初审", nameof(ToolButtonResource.FirstExam), nameof(ToolButtonResource.FirstExam))]
        FirstExam,
        /// <summary>
        /// 复审
        /// </summary>
        [Description("复审")]
        [ToolButton("复审", nameof(ToolButtonResource.ReExam), nameof(ToolButtonResource.ReExam))]
        ReExam,
        /// <summary>
        /// 预览报告
        /// </summary>
        [Description("预览报告")]
        [ToolButton("预览报告", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        PreviewRepot,
        /// <summary>
        /// 补充报告
        /// </summary>
        [Description("补充报告")]
        [ToolButton("补充报告", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        SupplyReport,
        /// <summary>
        /// 重新生成
        /// </summary>
        [Description("重新生成")]
        [ToolButton("重新生成", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        ReGenerate,
        /// <summary>
        /// 批量退单
        /// </summary>
        [Description("批量退单")]
        [ToolButton("批量退单", nameof(ToolButtonResource.BatchChargeback), nameof(ToolButtonResource.BatchChargeback))]
        BatchChargeback,
        /// <summary>
        /// 扫码归档
        /// </summary>
        [Description("扫码归档")]
        [ToolButton("扫码归档", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        ScanCodeArchive,
        /// <summary>
        /// 批量外借
        /// </summary>
        [Description("批量外借")]
        [ToolButton("批量外借", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        BatchLend,
        /// <summary>
        /// 批量归还
        /// </summary>
        [Description("批量归还")]
        [ToolButton("批量归还", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        BatchGiveback,
        /// <summary>
        /// 物流签收
        /// </summary>
        [Description("物流签收")]
        [ToolButton("物流签收", nameof(ToolButtonResource.SigninExpress), nameof(ToolButtonResource.SigninExpress))]
        SigninExpress,
        /// <summary>
        /// 新增物流
        /// </summary>
        [Description("新增物流")]
        [ToolButton("新增物流", nameof(ToolButtonResource.AddExpress), nameof(ToolButtonResource.AddExpress))]
        AddExpress,
        /// <summary>
        /// 申请单维护
        /// </summary>
        [Description("申请单维护")]
        [ToolButton("申请单维护", nameof(ToolButtonResource.MaintainAppFrm), nameof(ToolButtonResource.MaintainAppFrm))]
        MaintainAppFrm,
        /// <summary>
        /// 扫码送检
        /// </summary>
        [Description("扫码送检")]
        [ToolButton("扫码送检", nameof(ToolButtonResource.ScanCodeSendInsp), nameof(ToolButtonResource.ScanCodeSendInsp))]
        ScanCodeSendInsp,
        /// <summary>
        /// 补打条码
        /// </summary>
        [Description("补打条码")]
        [ToolButton("补打条码", nameof(ToolButtonResource.ReprintCode), nameof(ToolButtonResource.ReprintCode))]
        ReprintCode,
        /// <summary>
        /// 送检确认
        /// </summary>
        [Description("送检确认")]
        [ToolButton("送检确认", nameof(ToolButtonResource.CheckSendInsp), nameof(ToolButtonResource.CheckSendInsp))]
        CheckSendInsp,
        /// <summary>
        /// 取消登记
        /// </summary>
        [Description("取消登记")]
        [ToolButton("取消登记", nameof(ToolButtonResource.WithdrawRegistration), nameof(ToolButtonResource.WithdrawRegistration))]
        WithdrawRegistration,
        /// <summary>
        /// 重新取样关联
        /// </summary>
        [Description("重新取样关联")]
        [ToolButton("重新取样关联", nameof(ToolButtonResource.ResampleConnect), nameof(ToolButtonResource.ResampleConnect))]
        ResampleConnect,
        /// <summary>
        /// 路径设置
        /// </summary>
        [Description("路径设置")]
        [ToolButton("路径设置", nameof(ToolButtonResource.SetFilePath), nameof(ToolButtonResource.SetFilePath))]
        SetFilePath,
        /// <summary>
        /// 扫码
        /// </summary>
        [Description("扫码")]
        [ToolButton("扫码", nameof(ToolButtonResource.ScanCode), nameof(ToolButtonResource.ScanCode))]
        ScanCode,
        /// <summary>
        /// 批量打印报告
        /// </summary>
        [Description("批量打印报告")]
        [ToolButton("批量打印报告", nameof(ToolButtonResource.BatchPrintReport), nameof(ToolButtonResource.BatchPrintReport))]
        BatchPrintReport,
        /// <summary>
        /// 重新生成报告
        /// </summary>
        [Description("重新生成报告")]
        [ToolButton("重新生成报告", nameof(ToolButtonResource.ReGenerateReport), nameof(ToolButtonResource.ReGenerateReport))]
        ReGenerateReport,

        /// <summary>
        /// 扫码工作站 扫码接收
        /// </summary>
        [Description("扫码接收")]
        [ToolButton("扫码接收", nameof(ToolButtonResource.save_unselected), nameof(ToolButtonResource.save_selected))]
        ReceiveByScan,

        [Description("退回检验")]
        [ToolButton("退回检验", nameof(ToolButtonResource.BackCheckoutBatch), nameof(ToolButtonResource.BackCheckoutBatch))]
        BackCheckoutBatch,

        [Description("临床维护")]
        [ToolButton("临床维护", nameof(ToolButtonResource.ClinicalManifestation), nameof(ToolButtonResource.ClinicalManifestation))]
        ClinicalManifestation,
        /// <summary>
        /// 保存&外送
        /// </summary>
        [Description("保存&外送")]
        [ToolButton("保存&外送", nameof(ToolButtonResource.SaveSend), nameof(ToolButtonResource.SaveSend))]
        SaveSend,
        /// <summary>
        /// 重置
        /// </summary>
        [Description("刷新")]
        [ToolButton("刷新", nameof(ToolButtonResource.Reset), nameof(ToolButtonResource.Reset))]
        Refresh,

        /// <summary>
        /// 打开目录
        /// </summary>
        [Description("打开目录")]
        [ToolButton("打开目录", nameof(ToolButtonResource.OpenDir), nameof(ToolButtonResource.OpenDir))]
        OpenDir,
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = -1,
    }
}
