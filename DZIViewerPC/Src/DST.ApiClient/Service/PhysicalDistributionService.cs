using DST.ApiClient.Api;
using DST.Database.Model;
using HttpClientExtension.Exceptions;
using HttpClientExtension.Service;
using Newtonsoft.Json;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DST.ApiClient.Service
{
    /// <summary>
    /// 前处理工作站（物流工作站）
    /// </summary>
    public class PhysicalDistributionService : BaseService<PhysicalDistributionService>
    {
        /// <summary>
        /// 物流查询
        /// </summary>
        /// <param name="expressDto">查询条件实体</param>
        /// <param name="current">当前页面</param>
        /// <param name="size">每页的记录数量</param>
        public ExpressInfo PageByExpress(QueryExpress expressDto, int current, int size)
        {
            ExpressInfo result = PhysicalDistributionApi.Client.PageByExpress(expressDto, current, size).Data;
            return result;
        }

        /// <summary>
        /// 根据ID查询物流明细信息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ExpressDetail GetDetailsByExpress(string id)
        {
            ExpressDetail res = PhysicalDistributionApi.Client.GetDetailsByExpress(id).Data;
            return res;
        }

        /// <summary>
        /// 根据ID删除物流信息，返回信息，如果信息为空则删除成功
        /// </summary>
        public bool RemoveExpressById(string id)
        {
            ApiResponse<object> res = PhysicalDistributionApi.Client.RemoveExpressById(id);
            return res.Success;
        }

        /// <summary>
        /// 保存物流信息
        /// </summary>
        /// <param name="expressDetailsVO"></param>
        /// <returns></returns>
        public bool SaveExpress(ExpressDetail expressDetailsVO)
        {
            bool res = PhysicalDistributionApi.Client.SaveExpress(expressDetailsVO).Success;
            return res;
        }

        /// <summary>
        /// 物流扫码签收
        /// </summary>
        /// <param name="expressScanDto"></param>
        /// <returns></returns>
        public List<PhysDistReceiptBarcodeModel> SaveSignExpressByScan(SignExpressByScan expressScanDto)
        {
            List<PhysDistReceiptBarcodeModel> res = null;
            ApiResponse<List<PhysDistReceiptBarcodeModel>> respo = PhysicalDistributionApi.Client.SaveSignExpressByScan(expressScanDto);
            if (!respo.Success)
            {
                Logger.Error("前处理工作站>物流签收>扫码签收 失败：" + respo.Msg);
            }
            else
            {
                res = respo.Data;
            }

            return res;
        }

        /// <summary>
        /// 物流人工签收，返回条码信息
        /// </summary>
        /// <param name="expressSignVO"></param>
        /// <returns></returns>
        public List<PhysDistReceiptBarcodeModel> SaveSignExpressByHand(PhysDistReceiptHumanModel expressSignVO)
        {
            List<PhysDistReceiptBarcodeModel> resTemp = PhysicalDistributionApi.Client.SaveSignExpressByHand(expressSignVO).Data;
            return resTemp;
        }

        /// <summary>
        /// 送检查询
        /// </summary>
        /// <param name="queryInspection">送检查询条件</param>
        /// <param name="current">当前页码</param>
        /// <param name="size">当前页码条数</param>
        /// <returns></returns>
        public InspectionInfo PageByInspection(QueryInspection queryInspection, int current, int size)
        {
            string endReceivingTime = queryInspection.endReceivingTime.HasValue ? queryInspection.endReceivingTime.Value.AddDays(1.0).AddSeconds(-1.0).ToString("yyyy-MM-dd HH:mm:ss") : "";
            string startReceivingTime = queryInspection.startReceivingTime.HasValue ? queryInspection.startReceivingTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            InspectionInfo res = PhysicalDistributionApi.Client.PageByInspection(current, size, queryInspection.code, endReceivingTime, queryInspection.laboratoryCode, queryInspection.mailNo, queryInspection.patientName, queryInspection.productId, startReceivingTime, queryInspection.pathologyType).Data;
            return res;
        }

        /// <summary>
        /// 批量送检
        /// </summary>
        /// <param name="sampleIds">送检ID列表</param>
        /// <returns></returns>
        public ApiResponse<object> SaveInspectionSampleList(List<string> sampleIds)
        {
            ApiResponse<object> res = PhysicalDistributionApi.Client.SaveInspectionSampleList(sampleIds);
            if (!res.Success)
            {
                Logger.Error("批量送检失败：" + res.Msg);
            }

            return res;
        }

        /// <summary>
        /// 批量送检
        /// </summary>
        /// <param name="sampleIds">送检ID列表</param>
        /// <returns></returns>
        public async Task<bool> SaveInspeSampleListandGetExcel(List<string> sampleIds)
        {
            HttpResponseMessage downresult = PhysicalDistributionApi.Client.SaveInspeSampleListandGetExcel(sampleIds);
            var responsHeaders = downresult.Content.Headers;
            var info = responsHeaders.ContentDisposition; // 获取Content-Disposition请求头
            var fileName = info?.FileName; // 读取文件的UTF-8命名
            if (string.IsNullOrEmpty(fileName)) // 没有文件名，认为报错了
            {
                var json = await downresult.Content.ReadAsStringAsync().ConfigureAwait(false); // 报错的异常是json字符串构成，所以读取字符串
                ApiResponse<object> response = null;
                try
                {
                    response = JsonConvert.DeserializeObject<ApiResponse<object>>(json);
                }
                catch (Exception ex)
                {
                    Logger.Error("序列化失败", ex);
                }
                if (response != null)
                {
                    Logger.Info($"批量送检失败,原因：{response.Msg}");
                    if (!response.Success)
                    {
                        throw new HttpClientException(response.Msg);
                    }
                }
                return false;
            }

            var trueFileName = HttpUtility.UrlDecode(fileName, Encoding.UTF8); // 文件名解码
            using (var stream = await downresult.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                var buffersize = stream.Length;
                var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\{trueFileName}";
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, (int)buffersize, FileOptions.Asynchronous))
                {
                    await stream.CopyToAsync(fileStream).ConfigureAwait(false);
                }
            }
            var result = downresult.StatusCode == System.Net.HttpStatusCode.OK;
            if (!result)
            {
                Logger.Error("批量送检失败：");
            }

            return result;
        }
        /// <summary>
        /// 扫码获取送检信息
        /// </summary>
        /// <param name="laboratoryCode">实验室编号</param>
        /// <returns></returns>
        public Inspection GetInspectSampleByLaboratoryCode(string laboratoryCode)
        {
            Inspection res = PhysicalDistributionApi.Client.GetInspectSampleByLaboratoryCode(laboratoryCode).Data;
            return res;
        }

        /// <summary>
        /// 根据ID获取物流明细, C端专用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PhysDistInfoModel GetDetailsByExpressId(string id)
        {
            PhysDistInfoModel res = PhysicalDistributionApi.Client.GetDetailsByExpressId(id).Data;
            return res;
        }

        /// <summary>
        /// 根据物流编号获取物流明细
        /// </summary>
        public PhysDistReceiptHumanModel GetSignListByMailNo(string mailNo)
        {
            PhysDistReceiptHumanModel res = PhysicalDistributionApi.Client.GetSignListByMailNo(mailNo).Data;
            return res;
        }

        /// <summary>
        /// 查询已经签收的样本
        /// </summary>
        /// <returns></returns>
        public List<Sample> GetListScanSign()
        {
            List<Sample> result = PhysicalDistributionApi.Client.GetListScanSign().Data;
            return result;
        }

        /// <summary>
        /// 根据实验室编码获取打码数据
        /// </summary>
        /// <param name="laboratoryCode"></param>
        /// <returns></returns>
        public List<PhysDistReceiptBarcodeModel> GetSampleTscByLaboratoryCode(string laboratoryCode)
        {
            List<PhysDistReceiptBarcodeModel> res = PhysicalDistributionApi.Client.GetSampleTscByLaboratoryCode(laboratoryCode).Data;
            return res;
        }

        /// <summary>
        /// 根据患者姓名获取临床表现信息
        /// </summary>
        public List<PhysDistClinManifModel> ListPathologyByName(string name)
        {
            return PhysicalDistributionApi.Client.ListPathologyByName(name).Data;
        }

        /// <summary>
        /// 保存临床表现
        /// </summary>
        /// <param name="clinicalDiagnosis"></param>
        /// <param name="pathologyId"></param>
        /// <returns></returns>
        public bool SaveClinicalDiagnosis(string clinicalDiagnosis, string pathologyId)
        {
            dynamic pathologyCaseDTO = new { clinicalDiagnosis = clinicalDiagnosis, pathologyId = pathologyId };
            ApiResponse<object> res = PhysicalDistributionApi.Client.SaveClinicalDiagnosis(pathologyCaseDTO);
            return res.Success;
        }
    }
}
