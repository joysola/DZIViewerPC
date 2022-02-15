using DST.ApiClient.Api;
using DST.Database.Model;
using HttpClientExtension.Service;
using Newtonsoft.Json;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace DST.ApiClient.Service
{
    public class MolecularDiagnosisService : BaseService<MolecularDiagnosisService>
    {
        /// <summary>
        /// PCR 分页查询
        /// </summary>
        public ExamedReturnModel PagePcrByCondition(ExamedQueryModel samplePcrDto, int current, int size)
        {
            ExamedReturnModel res = MolecularDiagnosisApi.Client.PagePcrByCondition(samplePcrDto, current, size).Data;
            return res;
        }

        /// <summary>
        /// 根据实验编号获取将要打印条码的信息
        /// </summary>
        /// <param name="laboratory">实验编号</param>
        public ExamedPrint GetSamplePcrByLaboratory(string laboratory)
        {
            ExamedPrint exPrint = MolecularDiagnosisApi.Client.GetSamplePcrByLaboratory(laboratory).Data;
            return exPrint;
        }

        /// <summary>
        /// 标记复测
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        public bool UpdateMarkRetest(string sampleId)
        {
            ApiResponse<object> res = MolecularDiagnosisApi.Client.UpdateMarkRetest(sampleId);
            if(!res.Success)
            {
                Logger.Error("标记复测失败：" + res.Msg);
            }

            return res.Success;
        }

        /// <summary>
        /// 批量确认PCR样本
        /// </summary>
        /// <param name="sampleIds"></param>
        /// <returns></returns>
        public bool BatchConfirmPcrSampleList(List<string> sampleIds)
        {
            ApiResponse<object> res = MolecularDiagnosisApi.Client.BatchConfirmPcrSampleList(sampleIds);
            if(!res.Success)
            {
                Logger.Error("批量确认PCR样本失败：" + res.Msg);
            }

            return res.Success;
        }

        /// <summary>
        /// PCR 初审、复审查询
        /// </summary>
        /// <param name="sampleApproveDto"></param>
        /// <param name="current"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public SampleApproveReturn PagePcrByCondition(SampleApproveQuery sampleApproveDto, int current, int size)
        {
            SampleApproveReturn res = MolecularDiagnosisApi.Client.PagePcrByCondition(sampleApproveDto, current, size).Data;
            return res;
        }

        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="approveStatus">2=驳回，1=通过</param>
        /// <param name="auditStatus">1=初审，3=复核</param>
        /// <param name="sampleIds">样本ID列表</param>
        /// <returns></returns>
        public bool BatchApproveSampleList(int approveStatus, int auditStatus, List<string> sampleIds)
        {
            dynamic sampleApproveStatusDto = new { approveStatus = approveStatus, auditStatus = auditStatus, sampleIds = sampleIds };
            ApiResponse<object> res = MolecularDiagnosisApi.Client.BatchApproveSampleList(sampleApproveStatusDto);
            return true;
        }

        /// <summary>
        /// 根据ID查询PCR检测结果
        /// </summary>
        public List<SamplePcrResult> ListBySampleId(string sampleId)
        {
            List<SamplePcrResult> res = MolecularDiagnosisApi.Client.ListBySampleId(sampleId).Data;
            return res;
        }

        /// <summary>
        /// 获取结果确认列表
        /// </summary>
        /// <returns></returns>
        public List<SampleRcrResultConfirm> ListPositiveResultBySampleIds(List<string> sampleIds)
        {
            List<SampleRcrResultConfirm> res = MolecularDiagnosisApi.Client.ListPositiveResultBySampleIds(sampleIds).Data;
            return res;
        }

        /// <summary>
        /// PCR 导入数据
        /// </summary>
        /// <param name="filePath">文件名称</param>
        /// <returns></returns>
        public bool LaboratoryImportExcelPcrData(string filePath, string url, string token)
        {
            bool result = false;
            try
            {
                HttpClient client = new HttpClient();
                if (client.DefaultRequestHeaders.Contains("deepsight-auth")) // 注销后，需要更新token
                {
                    client.DefaultRequestHeaders.Remove("deepsight-auth");
                }
                client.DefaultRequestHeaders.Add("deepsight-auth", token);

                string fileName = Path.GetFileName(filePath);
                var postContent = new MultipartFormDataContent();
                postContent.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(filePath)), "fileData", fileName);

                HttpResponseMessage response = client.PostAsync(url, postContent).Result;
                string str = response.Content.ReadAsStringAsync().Result;
                Logger.Error("PCR文件上传结果：" + str);
                ApiResponse<object> res = JsonConvert.DeserializeObject<ApiResponse<object>>(str);
                result = res.Success;
                client.Dispose();
                client = null;
                str = null;
                response = null;
            }
            catch (Exception ex)
            {
                result = false;
                Logger.Error("PCR文件上传失败：" + ex.Message);
            }

            return result;
        }
    }
}
