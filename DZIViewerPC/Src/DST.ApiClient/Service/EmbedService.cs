using DST.ApiClient.Api;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using HttpClientExtension.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Service
{
    public class EmbedService : BaseService<EmbedService>
    {
        #region 样本管理
        /// <summary>
        /// 条件分页查询待取材样本
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="queryModel">取材查询实体</param>
        /// <returns></returns>
        public List<SampleModel> GetEmbedList(CustomPageModel pageModel, QuerySample queryModel)
        {
            var result = new List<SampleModel>();
            if (queryModel != null)
            {
                var response = EmbedApi.Client.GetEmbedList(pageModel.PageSize, pageModel.PageIndex,
                    queryModel.Name ?? string.Empty,
                    queryModel.ProductID ?? string.Empty);
                if (response.Success && response.Data != null)
                {
                    result = response.Data.Records;
                    pageModel.TotalNum = response.Data.Total;
                    pageModel.TotalPage = response.Data.Pages;
                }
            }
            return result;
        }

        #endregion 样本管理

        #region 组织取材包埋管理
        /// <summary>
        /// 更新取材状态（取材确认）
        /// </summary>
        /// <param name="waxBlockCode">蜡块编号</param>
        /// <returns>返回样本编号sampleID</returns>
        public string UpdateEmbedStatus(string waxBlockCode)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(waxBlockCode))
            {
                var response = EmbedApi.Client.UpdateEmbedStatus(waxBlockCode);
                if (response.Success)
                {
                    result = response.Data;
                } 
            }
            return result;
        }
        #endregion 组织取材包埋管理
    }
}
