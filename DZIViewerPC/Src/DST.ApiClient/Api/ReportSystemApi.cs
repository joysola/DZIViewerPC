using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    internal class ReportSystemApi : BaseApi<ReportSystemApi>
    {
        [Url("dst-pathology/pathology/report/pageByCurrentLoginHospital")]
        [HttpPost]
        internal ApiResponse<ReportQueryReturn> PageByCurrentLoginHospital([PostContent]ReportQuery sampleReportDto, int current, int size) => GetResult();

        [Url("dst-pathology/pathology/report/getReportUrlBySampleId")]
        [HttpGet]
        internal ApiResponse<ReportUrl> GetReportUrlBySampleId(string sampleId) => GetResult();

        [Url("dst-pathology/pathology/report/buildReport")]
        [HttpPost]
        internal ApiResponse<object> BuildReport([PostContent]dynamic sampleReportDto) => GetResult();

        [Url("dst-pathology/pathology/sample-back/chargeBack")]
        [HttpPost]
        internal ApiResponse<object> ChargeBack([PostContent] ChargeBackModel sampleBack) => GetResult();

        [Url("dst-fund/fund/product/listByAddProduct")]
        [HttpGet]
        internal ApiResponse<List<ProductModel>> ListByAddProduct(int addType) => GetResult();

        [Url("dst-pathology/pathology/sample/saveAddSample")]
        [HttpPost]
        internal ApiResponse<object> SaveAddSample([PostContent] dynamic sampleAddDto) => GetResult();

        [Url("dst-pathology/pathology/sample-doctor-advice-audit/saveAddSampleDoctorAdviceAudit")]
        [HttpPost]
        internal ApiResponse<object> SaveAddSampleDoctorAdviceAudit([PostContent] ReportDoctAdvice addDoctorAdviceAuditDto) => GetResult();

        [Url("dst-pathology/pathology/sample-scan/listCuttingScanVoBySampleId")]
        [HttpGet]
        internal ApiResponse<List<ReportSliceModel>> ListCuttingScanVoBySampleId(string sampleId) => GetResult();

        [Url("dst-fund/fund/markers/listByMarkers")]
        [HttpGet]
        internal ApiResponse<List<MarkModel>> ListByMarkers(string value) => GetResult();

        [Url("dst-pathology/pathology/sample-report-result/backCheckoutBatch")]
        [HttpPost]
        internal ApiResponse<object> BackCheckoutBatch([PostContent] List<string> sampleIdList) => GetResult();
    }
}
