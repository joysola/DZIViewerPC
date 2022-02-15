using System.ComponentModel;

namespace DST.PIMS.Framework.Model
{
    public enum EnumMessageKey
    {
        /// <summary>
        /// 刷新登录者信息
        /// </summary>
        [Description("刷新登录者信息")]
        RefreshLogin,

        /// <summary>
        /// 刷新当前工作站
        /// </summary>
        [Description("刷新当前工作站")]
        RefreshWorkstation,

        /// <summary>
        /// 基础命令-保存数据
        /// </summary>
        [Description("基础命令-保存数据")]
        BaseSaveData,

        /// <summary>
        /// 基础命令-更新数据
        /// </summary>
        [Description("基础命令-更新数据")]
        BaseUpdateData,

        /// <summary>
        /// 基础命令-删除数据
        /// </summary>
        [Description("基础命令-删除数据")]
        BaseDeleteData,

        /// <summary>
        /// 申请单查看
        /// </summary>
        [Description("申请单查看")]
        RequestDoc,

        /// <summary>
        /// 刷新样本上传列表
        /// </summary>
        [Description("刷新样本上传列表")]
        UploadScanImgRefresh,

        /// <summary>
        /// 样本上传工作站树形菜单可用状态切换
        /// </summary>
        [Description("样本上传工作站树形菜单可用状态切换")]
        UploadScanImgTreeViewDisabled,

        /// <summary>
        /// 工具栏上的功能按钮响应
        /// </summary>
        [Description("工具栏上的功能按钮响应")]
        ToolButtonResponse,

        /// <summary>
        /// 物流签收界面关闭
        /// </summary>
        [Description("物流签收界面关闭")]
        PhysDistReceiptClose,
        /// <summary>
        /// 关闭登录界面
        /// </summary>
        [Description("关闭登录界面")]
        CloseLogin,

        /// <summary>
        /// 保存配置文件
        /// </summary>
        [Description("保存配置文件")]
        SaveConfiguration,

        /// <summary>
        /// 重置焦点
        /// </summary>
        [Description("重置焦点")]
        ResetFocus,

        /// <summary>
        /// 扫码取材确认
        /// </summary>
        [Description("扫码取材确认")]
        ScanMaterialConfirm,

        /// <summary>
        /// 关闭分子条码配置窗口
        /// </summary>
        [Description("关闭分子条码配置窗口")]
        CloseMoleDiagBarcodeConfig,

        /// <summary>
        /// 扫码取材确认
        /// </summary>
        [Description("扫码包埋确认")]
        ScanEmbedConfirm,
        /// <summary>
        /// 刷新菜单按钮列表
        /// </summary>
        [Description("刷新菜单按钮列表")]
        RefreshMenus,

        /// <summary>
        /// 刷新上传工作站树形菜单
        /// </summary>
        [Description("刷新上传工作站树形菜单")]
        RefreshUploadTreeMenu,

        /// <summary>
        /// 刷新分子工作站
        /// </summary>
        [Description("刷新分子工作站")]
        RefreshMoleDiagExamed,
        /// <summary>
        /// 大体图像
        /// </summary>
        [Description("大体图像")]
        GeneralImg,
        /// <summary>
        /// 大体图像切换修复
        /// </summary>
        [Description("大体图像切换修复")]
        IntendtoRepairFreeze,

        /// <summary>
        /// 刷新物流页面
        /// </summary>
        [Description("刷新物流页面")]
        RefreshPhysDistSign,

        /// <summary>
        /// 扫码签收定位焦点
        /// </summary>
        [Description("扫码签收定位焦点")]
        PhysDistReceiptBarcodeFocus,

        /// <summary>
        /// 扫码工作站接收无焦点条码，接收玻片
        /// </summary>
        [Description("扫码工作站接收无焦点条码，接收玻片")]
        ScanMainReceive,

        /// <summary>
        /// 物流人工签收重定义焦点
        /// </summary>
        [Description("物流人工签收重定义焦点")]
        PhysDistReceiptHumanFocus,
        /// <summary>
        /// 扫码制片确认
        /// </summary>
        [Description("扫码制片确认")]
        ScanProdConfirm,
        /// <summary>
        /// 申请单维护保存后焦点切换
        /// </summary>
        [Description("申请单维护保存后焦点切换")]
        AppFrmMaintainSaveFocus,

        /// <summary>
        /// 物流签收的参数转发给人工签收
        /// </summary>
        [Description("物流签收的参数转发给人工签收")]
        PhysDistReceiptArgsSendToHuman,
        /// <summary>
        /// 报告视野定位阅片具体位置
        /// </summary>
        [Description("报告视野定位阅片具体位置")]
        LocateRptImgViewer,
        /// <summary>
        /// 刷新阅片标记框
        /// </summary>
        [Description("刷新阅片标记框")]
        RefreshImgViewerAnnos,
        /// <summary>
        /// 刷新阅片标记框
        /// </summary>
        [Description("关闭阅片弹窗")]
        CloseImgViewerPopups,
        /// <summary>
        /// 关闭标记列表
        /// </summary>
        [Description("关闭标记列表")]
        CloseAnnoLitPop,
        /// <summary>
        /// 是否需要分屏
        /// </summary>
        [Description("是否需要分屏")]
        SplitImgVm,
    }
}