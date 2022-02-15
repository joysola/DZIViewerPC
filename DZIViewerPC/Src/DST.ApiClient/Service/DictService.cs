using DST.ApiClient.Api;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using HttpClientExtension.Service;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DST.ApiClient.Service
{
    /// <summary>
    /// 字典服务
    /// </summary>
    public class DictService : BaseService<DictService>
    {
        /// <summary>
        /// 获取字典的通用方法
        /// </summary>
        /// <param name="code">键值</param>
        /// <returns></returns>
        private async Task<List<DictItem>> GetDict(string code)
        {
            var dictRes = await Task.Run(() => DictApi.Client.GetDict(code)).ConfigureAwait(false);
            var dictModel = dictRes.Data;
            var result = new List<DictItem>();
            if (dictModel?.Count > 0)
            {
                result = dictModel[0].children;
            }
            return result;
        }

        /// <summary>
        /// 获取检查项目的活动列表 38妇女节
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetActivityTypeList()
        {
            string code = "activityTypeList";
            var res = await GetDict(code).ConfigureAwait(false);
            return res;
        }

        /// <summary>
        /// 获取性别字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetSexDict()
        {
            string code = "sex";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取导出状态字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetDownFlagDict()
        {
            string code = "downFlag";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取检查项目状态状态字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetCheckProjectStatusDict()
        {
            string code = "checkProjectStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取活检字典（0 无活检、1 有活检）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetBiopsyFlagDict()
        {
            string code = "biopsyFlagList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取HPV结果字典（0 阴性、1 阳性）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetHPVResultDict()
        {
            string code = "HPVResultList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取腺上皮细胞分析结果字典（-1 无、0 未见腺上皮内病变及恶性病变、1 非典型腺细胞（无指定）、2 原位腺癌 、3 非典型腺细胞（倾向瘤变）、4 腺癌）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetGlandularEpithelialCellResultDict()
        {
            string code = "glandularEpithelialCellResult";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取样本TCT诊断结果字典（0 未见上皮内病变及恶性病变（NILM)、1 非典型鳞状上皮细胞，无明确诊断意义（ASC-US)、2 非典型鳞状上皮细胞，不除外高度病变（ASC-H)、3 低度鳞状上皮内病变（LSIL)、4 高度鳞状上皮内病变（H-SIL)、5 鳞状细胞癌）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetSampleTctResultDict()
        {
            string code = "sampleTctResult";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取样本标记状态字典（0 待发送、1 已发送、2 待标记、3 已完成）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetSampleSignStatusDict()
        {
            string code = "sampleSignStatusList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取标记结果字典（1_1 ASC-US、1_2 ASC-H、1_3 LSIL、1_4 HSIL、1_5 gandular、1_6 glandular-adace、1_7 atrophy、1_8 repair、1_9 metaplastic）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetSignResultDict()
        {
            string code = "signResultList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 获取共建病理科医院信息
        /// </summary>
        /// <returns></returns>
        public async Task<HospitalReturnModel> PageByHospital(HospitalModel hospitalDto, int current, int size)
        {
            HospitalReturnModel res = DictApi.Client.PageByHospital(hospitalDto, current, size).Data;
            return res;
        }

        /// <summary>
        /// 获取所有送检医生字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<SubmitDoctorModel>> GetSubmitDoctorDict()
        {
            var res = await Task.Run(() => DictApi.Client.GetSubmitDoctors()).ConfigureAwait(false);
            var result = res.Data;
            return result;
        }

        /// <summary>
        /// 根据医院ID获取所有检查项目字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductModel>> GetProductDict(string hospitalId)
        {
            var res = await Task.Run(() => DictApi.Client.GetProductModels(hospitalId)).ConfigureAwait(false);
            var result = res.Data;
            return result;
        }
        /// <summary>
        /// C端获取所有检查项目字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductModel>> GetCSProductDict()
        {
            var res = await Task.Run(() => DictApi.Client.GetCSProductModels()).ConfigureAwait(false);
            var result = res.Data;
            return result;
        }
        /// <summary>
        /// 获取实验室状态
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetExperimentStatusDict()
        {
            string code = "experimentStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取物流快递状态：已发货、已收货、以确认
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetExpressStatusDict()
        {
            string code = "expressStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public List<DoctorInfoModel> ListByHospitalId(string hospitalId)
        {
            List<DoctorInfoModel> res = DictApi.Client.ListByHospitalId(hospitalId).Data;
            return res;
        }

        /// <summary>
        /// 获取海世嘉激光打码机字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetHsjPrintType()
        {
            string code = "HsjPrintType";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 附言
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetPostscriptDict()
        {
            string code = "materialPostscript";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 包埋盒打印状态
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetEmbedPrintStatusDict()
        {
            string code = "embedPrintStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 样本取材延迟原因字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetSamplTissDelayDict()
        {
            string code = "SamplTissDelay";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 样本状态字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetRegisterSampleStatusDict()
        {
            string code = "RegisterSampleStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 取材状态字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetDrawMaterStatusDict()
        {
            string code = "DrawMaterStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        
        /// <summary>
        /// 医嘱状态字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetAdviceStatusDict()
        {
            string code = "AdviceStatusDict";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 技术医嘱类型字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetTechAdviceDict()
        {
            string code = "technicalAdviceList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 特检医嘱类型字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetDoctAdviceDict()
        {
            string code = "DoctAdviceDict";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 医嘱类型字典（不存在）
        /// </summary>
        /// <returns></returns>
        //public async Task<List<DictItem>> GetAdviceDict()
        //{
        //    string code = "docAdviceList";
        //    var result = await GetDict(code).ConfigureAwait(false);
        //    return result;
        //}

        /// <summary>
        /// 项目试剂字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetProdReagentDict()
        {
            string code = "productReagent";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 胃镜字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetGastroscopyTissueDict()
        {
            string code = "gastroscopy_tissue";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 重新取样状态字典字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetReSampStatusDict()
        {
            string code = "ReSampStatusDict";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 重新取样原因字典字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetReSampReasonDict()
        {
            string code = "ReSampReasonDict";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// 重新取样撤销原因字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetReSampWithdrawReasonDict()
        {
            string code = "ReSampWithdrawReasonDict";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
        
        /// <summary>
        /// 获取实验状态
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetTrialStatus()
        {
            string code = "trialStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取导入状态
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetHpvStatus()
        {
            string code = "hpvStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取PCR 检测结果字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetReportLargeResult()
        {
            string code = "reportLargeResult";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取扫描接收状态结果字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetReceiveStatus()
        {
            string code = "ReceiveStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取报告确认状态
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetReportStatusList()
        {
            string code = "reportStatusList";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取扫描状态
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetScanStatus()
        {
            string code = "scanStatus";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 获取病理类型，常规、细胞、分子
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetPathologyTypeList()
        {
            string code = "productType";
            var result = await GetDict(code).ConfigureAwait(false);
            return result;
        }
    }
}