using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    class AllocationApi : BaseApi<AllocationApi>
    {
        [Url("dst-pathology/pathology/sample-report-result/pageWaitReviewCellByCondition")]
        [HttpPost]
        internal ApiResponse<AllocationReturnModel> PageWaitReviewCellByCondition([PostContent] AllocationQueryModel sampleWaitReviewDto, int current, int size) => GetResult();

        [Url("dst-pathology/pathology/sample-scan/listScanImageUrlBySampleId")]
        [HttpGet]
        internal ApiResponse<List<string>> ListScanImageUrlBySampleId(string sampleId) => GetResult();

        //?sampleId=1418189685733330945
        /// <summary>
        /// 获取阅片详细信息
        /// </summary>
        /// <param name="sampleId">样本编号</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-report-result/getSampleDetailBySampleId")]
        [HttpGet]
        internal ApiResponse<AllocSampDetail> GetSampleDetail(string sampleId) => GetResult();
        /// <summary>
        /// 删除报告主图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-report-image/removeById")]
        [HttpGet]
        internal ApiResponse<bool> RemoveReportMainImg(string id) => GetResult();
        /// <summary>
        /// 新增报告主图
        /// </summary>
        /// <param name="reportImg"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample-report-image/saveReportImgage")]
        [HttpPost]
        internal ApiResponse<bool> SaveReportMainImg([PostContent] ReportImg reportImg) => GetResult();
        /// <summary>
        /// 上传报告视野主图
        /// </summary>
        /// <param name="file"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        [Url("dst-file/file/uploadFileListReturnUrlByModule")]
        [HttpPost]
        internal ApiResponse<List<string>> UploadFile(byte[] file, string module = "reportImg") => GetResult();
    }
}
