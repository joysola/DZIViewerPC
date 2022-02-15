using DST.ApiClient.Api;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using HttpClientExtension.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Service
{
    /// <summary>
    /// 样本组织取材包埋制片
    /// </summary>
    public class SampTissMaterService : BaseService<SampTissMaterService>
    {
        #region 样本管理
        /// <summary>
        /// 根据样本获取送检部位集合
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public List<InspSpecimen> GetSampSpecList(SampleModel sample)
        {
            var result = new List<InspSpecimen>();
            if (!string.IsNullOrEmpty(sample?.SampleID))
            {
                var response = SampTissMaterApi.Client.GetSampSpecList(sample?.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        #endregion 样本管理

        #region 样本组织管理
        /// <summary>
        /// 获取组织信息
        /// </summary>
        /// <param name="material">样本信息</param>
        /// <returns></returns>
        public SampTissModel GetSampleTissue(SampleModel material)
        {
            SampTissModel result = null;
            if (!string.IsNullOrEmpty(material.SampleID))
            {
                var response = SampTissMaterApi.Client.GetSampleTissueBySampID(material.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }


        #endregion 样本组织管理


        #region 组织取材包埋管理
        /// <summary>
        /// 根据样本id获取包埋盒列表
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public List<EmbedBoxModel> GetEmbedList(SampleModel sample)
        {
            var result = new List<EmbedBoxModel>();
            if (!string.IsNullOrEmpty(sample?.SampleID))
            {
                var response = SampTissMaterApi.Client.GetEmbedList(sample.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 根据样本id获取包埋盒列表
        /// </summary>
        /// <param name="sample">样本id</param>
        /// <returns></returns>
        public List<EmbedBoxModel> GetEmbedList(string sampleID)
        {
            var result = new List<EmbedBoxModel>();
            if (!string.IsNullOrEmpty(sampleID))
            {
                var response = SampTissMaterApi.Client.GetEmbedList(sampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }

        #endregion 组织取材包埋管理

        #region 送检部位管理
        /// <summary>
        /// 查询送检部位集合
        /// </summary>
        /// <param name="model">样本实体</param>
        /// <returns></returns>
        public List<InspSpecimen> GetInspSpecList(SampleModel model)
        {
            var result = new List<InspSpecimen>();
            if (!string.IsNullOrEmpty(model?.SampleID))
            {
                var response = SampTissMaterApi.Client.GetInspSpecList(model.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 查询送检部位集合
        /// </summary>
        /// <param name="sampID">样本id</param>
        /// <returns></returns>
        public List<InspSpecimen> GetInspSpecList(string sampID)
        {
            var result = new List<InspSpecimen>();
            if (!string.IsNullOrEmpty(sampID))
            {
                var response = SampTissMaterApi.Client.GetInspSpecList(sampID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        #endregion 送检部位管理
    }
}
