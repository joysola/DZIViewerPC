using DST.ApiClient.Api;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using HttpClientExtension.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Service
{
    public class AllocationService : BaseService<AllocationService>
    {
        public AllocationReturnModel PageWaitReviewCellByCondition(AllocationQueryModel sampleWaitReviewDto, int current, int size)
        {
            AllocationReturnModel res = AllocationApi.Client.PageWaitReviewCellByCondition(sampleWaitReviewDto, current, size).Data;
            return res;
        }

        public List<string> ListScanImageUrlBySampleId(string sampleId)
        {
            return AllocationApi.Client.ListScanImageUrlBySampleId(sampleId).Data;
        }
        /// <summary>
        /// 获取AI标记信息
        /// </summary>
        /// <param name="sampleID"></param>
        /// <returns></returns>
        public AllocSampDetail GetSampleDetail(string sampleID)
        {
            AllocSampDetail allocSampDetail = null;
            var response = AllocationApi.Client.GetSampleDetail(sampleID);
            if (response.Success)
            {
                allocSampDetail = response.Data;
            }
            return allocSampDetail;
        }
        /// <summary>
        /// 删除报告主图
        /// </summary>
        /// <param name="reportImg">报告视野实体</param>
        /// <returns></returns>
        public bool RemoveReportMainImg(ReportImg reportImg)
        {
            var result = false;
            if (!string.IsNullOrEmpty(reportImg.ID))
            {
                var response = AllocationApi.Client.RemoveReportMainImg(reportImg.ID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
    }
}
