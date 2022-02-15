using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    class MaterialApi : BaseApi<MaterialApi>
    {
        #region 样本管理

        /// <summary>
        /// 条件分页查询待取材样本
        /// </summary>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="drawMaterialsType">取材类型： 0 待取材 1延缓取材 2 补取</param>
        /// <param name="name">姓名</param>
        /// <param name="productId">项目id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue/pageDrawMaterialsByCondition")]
        [HttpGet]
        internal ApiResponse<ResponsePage<SampleModel>> GetMaterialList(int size, int current, string drawMaterialsType, string name, string productId) => GetResult();
        /// <summary>
        /// 保存送检部位
        /// </summary>
        /// <param name="sampSpecDetail">样本送检部位明细</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample/saveSpecimenList")]
        [HttpPost]
        internal ApiResponse<object> SaveSampSpecList([PostContent] SampSpecDetail sampSpecDetail) => GetResult();

    
        #endregion 样本管理

        #region 样本组织管理

      

        /// <summary>
        /// 保存组织
        /// </summary>
        /// <param name="sampTissModel">组织实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue/saveSampleTissue")]
        [HttpPost]
        internal ApiResponse<object> SaveSampleTissue([PostContent] SampTissModel sampTissModel) => GetResult();
        #endregion 样本组织管理

        #region 组织取材包埋管理
     
        /// <summary>
        /// 更新包埋盒数据
        /// </summary>
        /// <param name="embedBox">包埋盒实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-draw-materials/saveSampleTissueDrawMaterials")]
        [HttpPost]
        internal ApiResponse<object> SaveEmbedBoxInfo([PostContent] EmbedBoxModel embedBox) => GetResult();
        /// <summary>
        ///  新增包埋盒
        /// </summary>
        /// <param name="number">数量</param>
        /// <param name="sampleSpecimenId">部位id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-draw-materials/saveDrawMaterials")]
        [HttpGet]
        internal ApiResponse<List<EmbedPrintCode>> AddEmBedBoxInfo(int number, string sampleSpecimenId) => GetResult();
        /// <summary>
        /// 删除包埋盒
        /// </summary>
        /// <param name="id">包埋盒id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-draw-materials/removeById")]
        [HttpGet]
        internal ApiResponse<object> DeleteEmbedInfo(string id) => GetResult();
        /// <summary>
        /// 打印包埋盒数据
        /// </summary>
        /// <param name="ids">包埋盒id集合（可以以逗号隔开）</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-draw-materials/printEmbedding")]
        [HttpGet]
        internal ApiResponse<List<EmbedPrintCode>> PrintEmbedBoxList(string ids) => GetResult();
        /// <summary>
        /// 按样本id打号
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-draw-materials/saveNumBySampleId")]
        [HttpGet]
        internal ApiResponse<List<EmbedPrintCode>> PrintCodebySampID(string sampleId) => GetResult();
        /// <summary>
        /// 更新取材状态（取材确认）
        /// </summary>
        /// <param name="waxBlockCode">蜡块编号</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-draw-materials/updateDrawMaterialsStatus")]
        [HttpGet]
        internal ApiResponse<string> UpdateMaterialStatus(string waxBlockCode) => GetResult();
        #endregion 组织取材包埋管理

        #region  样本组织取材延迟管理
        /// <summary>
        /// 根据样本id获取组织取材延迟信息
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-delay/getSampleTissueBySampleId")]
        [HttpGet]
        internal ApiResponse<SampTissDelayInfo> GetSampTissDelayInfo(string sampleId) => GetResult();

        /// <summary>
        /// 保存组织取材延迟
        /// </summary>
        /// <param name="sampTissDelayInfo"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-delay/saveSampleTissueDelay")]
        [HttpPost]
        internal ApiResponse<object> SaveSampTissDelayInfo([PostContent] SampTissDelayInfo sampTissDelayInfo) => GetResult();
        #endregion  样本组织取材延迟管理

        #region 送检部位管理


        #endregion 送检部位管理

        #region 组织大体图像管理
        /// <summary>
        /// 样本id查询 组织大体图像集合
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-image/listBySampleId")]
        [HttpGet]
        internal ApiResponse<List<SampTissImgModel>> GetSampTissImgList(string sampleId) => GetResult();
        /// <summary>
        /// 删除组织大体图像路径
        /// </summary>
        /// <param name="id">组织大体图像id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-image/removeById")]
        [HttpPost]
        internal ApiResponse<object> DeleteSampTissImg(string id) => GetResult();
        /// <summary>
        /// 组织大体图像路径保存
        /// </summary>
        /// <param name="sampTissImg">组织大体图像实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-image/saveSampleTissueImage")]
        [HttpPost]
        internal ApiResponse<SampTissImgModel> SaveSampTissImg([PostContent] SampTissImgModel sampTissImg) => GetResult();
        /// <summary>
        /// 设置为采图
        /// </summary>
        /// <param name="id">组织大体图像id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-image/updateReportDrawingStatus")]
        [HttpGet]
        internal ApiResponse<object> UpdateReportDrawStatus(string id) => GetResult();
        #endregion 组织大体图像管理
    }
}
