using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    internal class ScanApi : BaseApi<ScanApi>
    {
        [Url("dst-pathology/pathology/sample-scan/pageByCellType")]
        [HttpPost]
        internal ApiResponse<ScanReturnModel> PageByCellType([PostContent] ScanQueryModel sampleScanDto, int current, int size) => GetResult();

        [Url("dst-pathology/pathology/sample-scan/bathReceive")]
        [HttpPost]
        internal ApiResponse<object> BathReceive([PostContent] List<string> sampleScanIdList) => GetResult();

        [Url("dst-pathology/pathology/sample-scan/pageScanQuery")]
        [HttpPost]
        internal ApiResponse<ScanReturnModel> PageScanQuery([PostContent] ScanQueryModel sampleScanDto, int current, int size) => GetResult();

        [Url("dst-pathology/pathology/sample-cutting/productionSlice")]
        [HttpPost]
        internal ApiResponse<object> ProductionSlice([PostContent] List<string> codeList) => GetResult();

        [Url("dst-pathology/pathology/sample-scan/receiveByCode")]
        [HttpGet]
        internal ApiResponse<object> ReceiveByCode(string code) => GetResult();

        [Url("dst-pathology/pathology/sample-scan/getSampleScanByCode")]
        [HttpGet]
        internal ApiResponse<ScanModel> GetSampleScanByCode(string code) => GetResult();

        [Url("dst-pathology/pathology/sample-scan/receiveByCodeList")]
        [HttpPost]
        internal ApiResponse<object> ReceiveByCodeList([PostContent] List<string> codeList) => GetResult();
    }
}
