using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    internal class MolecularDiagnosisApi : BaseApi<MolecularDiagnosisApi>
    {
        [Url("dst-pathology/pathology/sample-pcr/pagePcrByCondition")]
        [HttpPost]
        internal ApiResponse<ExamedReturnModel> PagePcrByCondition([PostContent] ExamedQueryModel samplePcrDto, int current, int size) => GetResult();

        [Url("dst-pathology/pathology/sample-pcr/getSamplePcrByLaboratory")]
        [HttpGet]
        internal ApiResponse<ExamedPrint> GetSamplePcrByLaboratory(string laboratory) => GetResult();

        [Url("dst-pathology/pathology/sample-pcr/updateMarkRetest")]
        [HttpGet]
        internal ApiResponse<object> UpdateMarkRetest(string sampleId) => GetResult();

        [Url("dst-pathology/pathology/sample-pcr/batchConfirmPcrSampleList")]
        [HttpPost]
        internal ApiResponse<object> BatchConfirmPcrSampleList([PostContent] List<string> sampleIds) => GetResult();

        [Url("dst-pathology/pathology/sample-report-result/pagePcrByCondition")]
        [HttpPost]
        internal ApiResponse<SampleApproveReturn> PagePcrByCondition([PostContent] SampleApproveQuery sampleApproveDto, int current, int size) => GetResult();

        [Url("dst-pathology/pathology/sample-report-result/batchApproveSampleList")]
        [HttpPost]
        internal ApiResponse<object> BatchApproveSampleList([PostContent] dynamic sampleApproveStatusDto) => GetResult();


        [Url("dst-pathology/pathology/sample-pcr-result/listBySampleId")]
        [HttpGet]
        internal ApiResponse<List<SamplePcrResult>> ListBySampleId(string sampleId) => GetResult();

        [Url("dst-pathology/pathology/sample-pcr-result/listPositiveResultBySampleIds")]
        [HttpPost]
        internal ApiResponse<List<SampleRcrResultConfirm>> ListPositiveResultBySampleIds([PostContent] List<string> sampleIds) => GetResult();
    }
}
