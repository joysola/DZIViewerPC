using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    class ReSampApi : BaseApi<ReSampApi>
    {
        #region 重新取样管理
        /// <summary>
        /// 分页查询 重新取样列表
        /// </summary>
        /// <param name="reSample">重新取样查询实体</param>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-resample-record/pageByCondition")]
        [HttpPost]
        internal ApiResponse<ResponsePage<ReSampModel>> GetReSampList([PostContent] QueryReSample reSample, int size, int current) => GetResult();
        /// <summary>
        /// 撤销重新取样
        /// </summary>
        /// <param name="id">重新取样实体id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-resample-record/removeResampleById")]
        [HttpGet]
        internal ApiResponse<object> WithdrawReSamp(string id) => GetResult();
        /// <summary>
        /// 提交重新取样样本
        /// </summary>
        /// <param name="submitReSample">提交重新取样实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-resample-record/saveResample")]
        [HttpPost]
        internal ApiResponse<object> SubmitReSample([PostContent] SubmitReSampleModel submitReSample) => GetResult();

        /// <summary>
        /// 关联样本
        /// </summary>
        /// <param name="reSample">提交重新取样实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-resample-record/updateResample")]
        [HttpPost]
        internal ApiResponse<object> RelateReSample([PostContent] ReSampModel reSample) => GetResult();
        #endregion 重新取样管理

        #region 样本管理
        /// <summary>
        /// 条件分页 关联重新样本对应的样本列表
        /// </summary>
        /// <param name="size">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="hospitalId">医院id</param>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample/pageResSampleByCondition")]
        [HttpGet]
        internal ApiResponse<ResponsePage<SampleModel>> GetSampListbyReSampInfo(int size, int current, string hospitalId, string name) => GetResult();
        #endregion 样本管理
    }
}
