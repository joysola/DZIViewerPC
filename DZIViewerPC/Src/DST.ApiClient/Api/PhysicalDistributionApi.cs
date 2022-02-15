using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DST.ApiClient.Api
{
    internal class PhysicalDistributionApi : BaseApi<PhysicalDistributionApi>
    {
        //[Url("dst-pathology/pathology/express/pageByExpress")]
        [Url("dst-pathology/pathology/express/client/pageByExpress")]
        [HttpPost]
        internal ApiResponse<ExpressInfo> PageByExpress([PostContent] QueryExpress expressDto, int current, int size) => GetResult();

        [Url("dst-pathology/pathology/express/getDetailsByExpress")]
        [HttpGet]
        internal ApiResponse<ExpressDetail> GetDetailsByExpress(string id) => GetResult();

        [Url("dst-pathology/pathology/express/removeById")]
        [HttpGet]
        internal ApiResponse<object> RemoveExpressById(string id) => GetResult();

        [Url("dst-pathology/pathology/express/saveExpress")]
        [HttpPost]
        internal ApiResponse<object> SaveExpress([PostContent]ExpressDetail expressDetailsVO) => GetResult();

        [Url("dst-pathology/pathology/express/saveSignExpressByScan")]
        [HttpPost]
        internal ApiResponse<List<PhysDistReceiptBarcodeModel>> SaveSignExpressByScan([PostContent] SignExpressByScan expressScanDto) => GetResult();

        [Url("dst-pathology/pathology/express/saveSignExpressByHand")]
        [HttpPost]
        internal ApiResponse<List<PhysDistReceiptBarcodeModel>> SaveSignExpressByHand([PostContent] PhysDistReceiptHumanModel expressSignVO) => GetResult();

        [Url("dst-pathology/pathology/sample/pageByInspection")]
        [HttpGet]
        internal ApiResponse<InspectionInfo> PageByInspection(int current, int size, string code, string endReceivingTime, string laboratoryCode, string mailNo, string patientName, string productId, string startReceivingTime, string pathologyType) => GetResult();


        [Url("dst-pathology/pathology/sample/saveInspectionSampleList")]
        [HttpPost]
        internal ApiResponse<object> SaveInspectionSampleList([PostContent] List<string> sampleIds) => GetResult();
        /// <summary>
        /// 批量送检 并 获取excel
        /// </summary>
        /// <param name="sampleIds">样本id集合</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/sample/saveInspectionSampleList")]
        [HttpPost]
        internal HttpResponseMessage SaveInspeSampleListandGetExcel([PostContent] List<string> sampleIds) => GetResult();

        [Url("dst-pathology/pathology/sample/getInspectSampleByLaboratoryCode")]
        [HttpGet]
        internal ApiResponse<Inspection> GetInspectSampleByLaboratoryCode(string laboratoryCode) => GetResult();
        
        [Url("dst-pathology/pathology/express/getDetailsByExpressId")]
        [HttpGet]
        internal ApiResponse<PhysDistInfoModel> GetDetailsByExpressId(string id) => GetResult();

        [Url("dst-pathology/pathology/express/getSignListByMailNo")]
        [HttpGet]
        internal ApiResponse<PhysDistReceiptHumanModel> GetSignListByMailNo(string mailNo) => GetResult();

        [Url("dst-pathology/pathology/sample/listScanSign")]
        [HttpGet]
        internal ApiResponse<List<Sample>> GetListScanSign() => GetResult();

        [Url("dst-pathology/pathology/sample/getSampleTscByLaboratoryCode")]
        [HttpGet]
        internal ApiResponse<List<PhysDistReceiptBarcodeModel>> GetSampleTscByLaboratoryCode(string laboratoryCode) => GetResult();

        [Url("dst-pathology/pathology/pathology/listPathologyByName")]
        [HttpGet]
        internal ApiResponse<List<PhysDistClinManifModel>> ListPathologyByName(string name) => GetResult();

        [Url("dst-pathology/pathology/pathology-case/saveClinicalDiagnosis")]
        [HttpPost]
        internal ApiResponse<object> SaveClinicalDiagnosis([PostContent] dynamic pathologyCaseDTO) => GetResult();
    }
}
