using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    class SampTissMaterApi : BaseApi<SampTissMaterApi>
    {
        #region 样本管理
        /// <summary>
        /// 根据样本id获取送检部位集合
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample/getSpecimenList")]
        [HttpGet]
        internal ApiResponse<List<InspSpecimen>> GetSampSpecList(string sampleId) => GetResult();

        #endregion 样本管理

        #region 样本组织管理
        /// <summary>
        /// 根据样本id 获取组织信息
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue/getSampleTissueBySampleId")]
        [HttpGet]
        internal ApiResponse<SampTissModel> GetSampleTissueBySampID(string sampleId) => GetResult();

        #endregion 样本组织管理

        #region 组织取材包埋管理
        /// <summary>
        /// 根据样本id获取包埋盒列表
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue-draw-materials/listBySampleId")]
        [HttpGet]
        internal ApiResponse<List<EmbedBoxModel>> GetEmbedList(string sampleId) => GetResult();

        #endregion 组织取材包埋管理

        #region 送检部位管理
        /// <summary>
        /// 按样本id查询送检部位集合
        /// </summary>
        /// <param name="sampleId">样本id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-specimen/listBySampleId")]
        [HttpGet]
        internal ApiResponse<List<InspSpecimen>> GetInspSpecList(string sampleId) => GetResult();

        #endregion 送检部位管理
    }
}
