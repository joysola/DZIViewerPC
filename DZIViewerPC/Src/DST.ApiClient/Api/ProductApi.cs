using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    class ProductApi : BaseApi<ProductApi>
    {
        #region 样本组织管理
        /// <summary>
        /// 条件分页查询待 切片(制片)样本
        /// </summary>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="name">姓名</param>
        /// <param name="productId">项目id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue/pageCuttingByCondition")]
        [HttpGet]
        internal ApiResponse<ResponsePage<SampleModel>> GetProductList(int size, int current, string name, string productId) => GetResult();

        #endregion 样本组织管理

        #region 样本切片管理
        /// <summary>
        /// 样本id获取切片列表
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/listCuttingBySampleId")]
        [HttpGet]
        internal ApiResponse<List<SliceModel>> GetSliceList(string sampleId) => GetResult();
        /// <summary>
        /// 样本id获取制片的组织切片列表
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/listTissueCuttingBySampleId")]
        [HttpGet]
        internal ApiResponse<List<SliceModel>> GetProdSliceList(string sampleId) => GetResult();
        /// <summary>
        /// 保存组织切片信息
        /// </summary>
        /// <param name="sliceModel">切片实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/saveSampleCutting")]
        [HttpPost]
        internal ApiResponse<object> SaveSlice([PostContent] SliceModel sliceModel) => GetResult();
        /// <summary>
        /// 删除组织切片
        /// </summary>
        /// <param name="id">切片id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/removeTissueById")]
        [HttpGet]
        internal ApiResponse<object> DeleteSlice(string id) => GetResult();
        /// <summary>
        /// 批量获取待打印切片样本
        /// </summary>
        /// <param name="cuttingIds">切片id集合</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/listSampleCutTscByCuttingIds")]
        [HttpPost]
        internal ApiResponse<List<SlicePrintCode>> PrintSliceCodeList([PostContent] IEnumerable<string> cuttingIds) => GetResult();
        /// <summary>
        /// 组织蜡块生成切片(单扫)
        /// </summary>
        /// <param name="waxBlockCode">蜡块编号</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/saveByTissueByWaxBlockCode")]
        [HttpGet]
        internal ApiResponse<List<SlicePrintCode>> ScanCodeGeneSlice(string waxBlockCode) => GetResult();
        /// <summary>
        /// 组织蜡块生成切片(特染)
        /// </summary>
        /// <param name="waxBlockCode">蜡块编号</param>
        /// <param name="marker">标记物</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/saveByTissueByWaxBlockCodeAndMarker")]
        [HttpGet]
        internal ApiResponse<List<SlicePrintCode>> ScanCodeGeneSliceSpec(string waxBlockCode, string marker) => GetResult();

        /// <summary>
        /// 条件分页查询细胞制片样本
        /// </summary>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="name">实验室编号或姓名</param>
        /// <param name="productId">项目id</param>
        /// <param name="startInspectionTime">送检开始时间</param>
        /// <param name="endInspectionTime">送检结束时间</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/pageBySampleTctCut")]
        [HttpGet]
        internal ApiResponse<ResponsePage<SliceTCTModel>> GetSliceTCTList(int size, int current, string name, string productId, string startInspectionTime, string endInspectionTime) => GetResult();
        /// <summary>
        /// 批量获取"细胞"待打印切片样本
        /// </summary>
        /// <param name="cuttingIds">切片号集合</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/listSampleTctCutTscByCuttingIds")]
        [HttpPost]
        internal ApiResponse<List<SlicePrintCode>> PrintTCTSliceCodeList([PostContent] IEnumerable<string> cuttingIds) => GetResult();

        /// <summary>
        /// 条件分页查询制片样本
        /// </summary>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="name">姓名</param>
        /// <param name="productId">项目id</param>
        /// <param name="adviceType">医嘱类型</param>
        /// <param name="laboratoryCode">实验室编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/pageBySampleCutByCondition")]
        [HttpGet]
        internal ApiResponse<ResponsePage<SliceProdModel>> GetSliceProdList(int size, int current, string name, string productId, string adviceType, string laboratoryCode, string startTime, string endTime) => GetResult();
        /// <summary>
        /// 批量获取医嘱待打印切片样本
        /// </summary>
        /// <param name="queryAdvicePrint">医嘱切片打印获取实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/listSampleAdviceCutTscByCuttingIds")]
        [HttpPost]
        internal ApiResponse<List<SlicePrintCode>> GetAdviceSliceCodeList([PostContent] QueryAdvicePrint queryAdvicePrint) => GetResult();
        
        #endregion 样本切片管理

        #region 样本组织切片管理
        /// <summary>
        /// 根据样本id获取蜡块切片统计信息
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-cutting-rel/getCountBySampleId")]
        [HttpGet]
        internal ApiResponse<SliceStatistics> GetSliceStatistics(string sampleId) => GetResult();

        /// <summary>
        /// 样本id获取技术医嘱切片列表
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/listTechnicalCuttingBySampleId")]
        [HttpGet]
        internal ApiResponse<List<SliceModel>> GetTechAdviSliceList(string sampleId) => GetResult();
        /// <summary>
        /// 保存技术医嘱、特检医嘱 切片信息
        /// </summary>
        /// <param name="slice">切片实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-cutting/saveTechnicalSampleCutting")]
        [HttpPost]
        internal ApiResponse<SliceModel> SaveTechAdviSlice([PostContent] SliceModel slice) => GetResult();
        #endregion

        #region 技术医嘱审核管理
        /// <summary>
        /// 分页条件查询技术医嘱（制片环节）
        /// </summary>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="name">实验室编号或姓名</param>
        /// <param name="productId">项目id</param>
        /// <param name="startAdviceTime">医嘱开始时间</param>
        /// <param name="endAdviceTime">医嘱结束时间</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-technical-advice-audit/pageBySampleTechnicalAdvice")]
        [HttpGet]
        internal ApiResponse<ResponsePage<SampleModel>> GetTechAdviceSampList(int size, int current, string name, string productId, string startAdviceTime, string endAdviceTime) => GetResult();

        #endregion 技术医嘱审核管理

        #region 特检医嘱审核管理
        /// <summary>
        /// 分页条件查询特检医嘱（制片环节）
        /// </summary>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="name">实验室编号或姓名</param>
        /// <param name="productId">项目id</param>
        /// <param name="startAdviceTime">医嘱开始时间</param>
        /// <param name="endAdviceTime">医嘱结束时间</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-doctor-advice-audit/pageBySampleTechnicalAdvice")]
        [HttpGet]
        internal ApiResponse<ResponsePage<SampleModel>> GetDoctAdviceSampList(int size, int current, string name, string productId, string startAdviceTime, string endAdviceTime) => GetResult();

        #endregion 特检医嘱审核管理

        #region 组织取材包埋管理
        /// <summary>
        /// 按病理id查询切片列表
        /// </summary>
        /// <param name="pathologyId"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-draw-materials/listByPathologyId")]
        [HttpGet]
        internal ApiResponse<List<EmbedBoxModel>> GetSliceListbyPathID(string pathologyId) => GetResult();
        #endregion 组织取材包埋管理

        #region 产品特检配置管理
        /// <summary>
        /// 根据项目Id查询特检配置列表
        /// </summary>
        /// <param name="productId">项目id</param>
        /// <returns></returns>
        [Url("dst-fund/fund/product-special-survey/listByProductId")]
        [HttpGet]
        internal ApiResponse<List<SpecStainSetting>> GetSpecStainSetList(string productId) => GetResult();
        #endregion 产品特检配置管理
    }
}
