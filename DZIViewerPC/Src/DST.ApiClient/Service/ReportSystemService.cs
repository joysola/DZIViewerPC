using DST.ApiClient.Api;
using DST.Database.Model;
using HttpClientExtension.Service;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Service
{
    public class ReportSystemService : BaseService<ReportSystemService>
    {
        /// <summary>
        /// 根据医院信息查询报告
        /// </summary>
        public ReportQueryReturn PageByCurrentLoginHospital(ReportQuery sampleReportDto, int current, int size)
        {
            sampleReportDto.gatherTimeEnd = sampleReportDto.gatherTimeEnd?.AddDays(1.0).AddSeconds(-1.0);
            sampleReportDto.reportTimeEnd = sampleReportDto.reportTimeEnd?.AddDays(1.0).AddSeconds(-1.0);
            ReportQueryReturn res = ReportSystemApi.Client.PageByCurrentLoginHospital(sampleReportDto, current, size).Data;
            return res;
        }

        /// <summary>
        /// 根据样本编号获取报告URL
        /// </summary>
        public ReportUrl GetReportUrlBySampleId(string sampleId)
        {
            ReportUrl res = ReportSystemApi.Client.GetReportUrlBySampleId(sampleId).Data;
            return res;
        }

        /// <summary>
        /// 重新生成报告
        /// </summary>
        /// <param name="sampleIdList">样本ID列表</param>
        /// <param name="languageType">：0-中文，1-英文</param>
        public ApiResponse<object> BuildReport(List<string> sampleIdList, int languageType = 0)
        {
            dynamic sampleReportDto = new { sampleIdList = sampleIdList, languageType = languageType };
            ApiResponse<object> res = ReportSystemApi.Client.BuildReport(sampleReportDto);
            return res;
        }

        /// <summary>
        /// 退单
        /// </summary>
        public bool ChargeBack(ChargeBackModel sampleBack)
        {
            ApiResponse<object> res = ReportSystemApi.Client.ChargeBack(sampleBack);
            return res.Success;
        }

        /// <summary>
        /// 分子退回检验
        /// </summary>
        /// <param name="sampleIdList"></param>
        /// <returns></returns>
        public bool BackCheckoutBatch(List<string> sampleIdList)
        {
            ApiResponse<object> res = ReportSystemApi.Client.BackCheckoutBatch(sampleIdList);
            return res.Success;
        }

        /// <summary>
        /// 获取加测项目列表
        /// </summary>
        /// <param name="addStatus"></param>
        /// <returns></returns>
        public List<ProductModel> ListByAddProduct(int addType = 1)
        {
            List<ProductModel> res = ReportSystemApi.Client.ListByAddProduct(addType).Data;
            return res;
        }

        /// <summary>
        /// 新增常规加测
        /// </summary>
        /// <returns></returns>
        public bool SaveAddSample(string pathologyId, string productId, string productType, string screen)
        {
            dynamic sampleAddDto = new { pathologyId = pathologyId, productId = productId, productType = productType, screen = screen};
            ApiResponse<object> res = ReportSystemApi.Client.SaveAddSample(sampleAddDto);
            return res.Success;
        }

        /// <summary>
        /// 新增特检加测
        /// </summary>
        public bool SaveAddSampleDoctorAdviceAudit(ReportDoctAdvice addDoctorAdviceAuditDto)
        {
            ApiResponse<object> res = ReportSystemApi.Client.SaveAddSampleDoctorAdviceAudit(addDoctorAdviceAuditDto);
            return res.Success;
        }

        /// <summary>
        /// 返回切片信息
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        public List<ReportSliceModel> ListCuttingScanVoBySampleId(string sampleId)
        {
            List<ReportSliceModel> res = ReportSystemApi.Client.ListCuttingScanVoBySampleId(sampleId).Data;
            return res;
        }

        /// <summary>
        /// 获取特检加测的项目
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<MarkModel> ListByMarkers(string value = "")
        {
            List<MarkModel> res = ReportSystemApi.Client.ListByMarkers(value).Data;
            return res;
        }
    }
}
