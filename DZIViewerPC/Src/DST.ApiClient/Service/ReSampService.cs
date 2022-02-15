using DST.ApiClient.Api;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using HttpClientExtension.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Service
{
    public class ReSampService : BaseService<ReSampService>
    {
        #region 重新取样管理
        /// <summary>
        /// 分页查询 重新取样列表
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="query">重新取样查询实体</param>
        /// <returns></returns>
        public List<ReSampModel> GetReSampList(CustomPageModel pageModel, QueryReSample query)
        {
            var result = new List<ReSampModel>();
            if (query != null)
            {
                var response = ReSampApi.Client.GetReSampList(query, pageModel.PageSize, pageModel.PageIndex);
                if (response.Success && response.Data != null)
                {
                    result = response.Data.Records;
                    pageModel.TotalNum = response.Data.Total;
                    pageModel.TotalPage = response.Data.Pages;
                }
            }
            return result;
        }
        /// <summary>
        /// 撤销重新取样
        /// </summary>
        /// <param name="model">重新取样实体</param>
        /// <returns></returns>
        public bool WithdrawReSamp(ReSampModel model)
        {
            if (!string.IsNullOrEmpty(model?.ID))
            {
                var response = ReSampApi.Client.WithdrawReSamp(model?.ID);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 提交重新取样样本
        /// </summary>
        /// <param name="model">提交重新取样实体</param>
        /// <returns></returns>
        public bool SubmitReSample(SubmitReSampleModel model)
        {
            if (!string.IsNullOrEmpty(model?.SampleID))
            {
                var response = ReSampApi.Client.SubmitReSample(model);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 关联样本
        /// </summary>
        /// <param name="model">提交重新取样实体</param>
        /// <returns></returns>
        public bool RelateReSample(ReSampModel model)
        {
            if (!string.IsNullOrEmpty(model?.ID) && !string.IsNullOrEmpty(model?.ReSampleID) && !string.IsNullOrEmpty(model?.SampleID))
            {
                var response = ReSampApi.Client.RelateReSample(model);
                return response.Success;
            }
            return false;
        }
        #endregion 重新取样管理



        #region 样本管理
        public List<SampleModel> GetSampListbyReSampInfo(CustomPageModel pageModel, ReSampModel reSamp)
        {
            var result = new List<SampleModel>();
            if (!string.IsNullOrEmpty(reSamp.ID))
            {
                var response = ReSampApi.Client.GetSampListbyReSampInfo(pageModel.PageSize, pageModel.PageIndex, reSamp.HospitalID, reSamp.PatientName);
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
    }
}
