using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    class EmbedApi : BaseApi<EmbedApi>
    {
        #region 样本组织管理
        /// <summary>
        /// 条件分页查询待包埋样本
        /// </summary>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="name">姓名</param>
        /// <param name="productId">项目id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-tissue/pageEmbeddingByCondition")]
        [HttpGet]
        internal ApiResponse<ResponsePage<SampleModel>> GetEmbedList(int size, int current, string name, string productId) => GetResult();
        #endregion 样本组织管理


        #region 组织取材包埋管理
        /// <summary>
        /// 更新包埋状态
        /// </summary>
        /// <param name="waxBlockCode">蜡块编号</param>
        /// <returns>样本编号sampleID</returns>
        [Url("dst-pathology/pathology/sample-tissue-draw-materials/updateEmbeddingStatus")]
        [HttpGet]
        internal ApiResponse<string> UpdateEmbedStatus(string waxBlockCode) => GetResult();

        #endregion 组织取材包埋管理
    }
}
