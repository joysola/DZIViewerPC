using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    class RegisterApi : BaseApi<RegisterApi>
    {
        #region 病理
        /// <summary>
        /// 按条件分页查询病理
        /// </summary>
        /// <param name="size">每页数量</param>
        /// <param name="current">当前页</param>
        /// <param name="name">病理号、姓名</param>
        /// <param name="isSampleDay">是否当天接收登记</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/pathology/pageByCondition")]
        [HttpGet]
        internal ApiResponse<ResponsePage<PathInfoModel>> GetPathInfoList(int size, int current, string name, bool isSampleDay) => GetResult();
        /// <summary>
        /// 取消登记
        /// </summary>
        /// <param name="pathID">病理号</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/pathology/removeByPathologyId")]
        [HttpGet]
        internal ApiResponse<object> DeletePathInfo([ParamName("pathologyId")] string pathID) => GetResult();
        #endregion 病理





        #region 样本管理
        /// <summary>
        /// 分页查询样本登记列表（C端）
        /// </summary>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="laboratoryCode">实验室编号</param>
        /// <param name="patientName">患者姓名</param>
        /// <param name="productId">项目id</param>
        /// <param name="startCheckTime">登记开始时间</param>
        /// <param name="endCheckTime">登记结束时间</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample/pageByCheck")]
        [HttpGet]
        internal ApiResponse<ResponsePage<SampleRegisterModel>> GetRegisterList(int size, int current, string laboratoryCode, string patientName, string productId, /*string productType,*/ string startCheckTime, string endCheckTime) => GetResult();

        /// <summary>
        /// 按条形码查询待外送样本
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample/listDeliverySampleByBarCode")]
        [HttpGet]
        internal ApiResponse<List<SampleRegisterModel>> GetDeliverySamplebyBarcode(string barCode) => GetResult();
        /// <summary>
        /// 批量保存外送样本
        /// </summary>
        /// <param name="sampleIds">样本id集合</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample/saveDeliverySampleList")]
        [HttpPost]
        internal ApiResponse<object> SaveDeliverySampleList([PostContent] IEnumerable<string> sampleIds) => GetResult();

        /// <summary>
        /// 根据病理id获取补打的条码
        /// </summary>
        /// <param name="pathologyId">病理id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample/getSampleTscByPathologyId")]
        [HttpGet]
        internal ApiResponse<List<AppFrmPrintCode>> GetAppFrmPrintCodeList(string pathologyId) => GetResult();
        #endregion 样本管理
    }
}
