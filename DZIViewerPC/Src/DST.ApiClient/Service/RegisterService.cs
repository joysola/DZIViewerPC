using DST.ApiClient.Api;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using HttpClientExtension.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DST.ApiClient.Service
{
    public class RegisterService : BaseService<RegisterService>
    {
        #region 病理
        /// <summary>
        /// 按条件分页查询病理
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="name">病理号 、姓名</param>
        /// <param name="isSampleDay">是否当天接收登记</param>
        /// <returns></returns>
        public List<PathInfoModel> GetPathInfoList(CustomPageModel pageModel, string name, bool isSampleDay)
        {
            var result = new List<PathInfoModel>();
            name = name ?? string.Empty;
            var response = RegisterApi.Client.GetPathInfoList(pageModel.PageSize, pageModel.PageIndex, name, isSampleDay);
            if (response.Success && response.Data != null)
            {
                result = response.Data.Records;
                pageModel.TotalNum = response.Data.Total;
                pageModel.TotalPage = response.Data.Pages;
            }
            return result;
        }
        /// <summary>
        /// 取消登记
        /// </summary>
        /// <param name="applyFrm">申请单实体（含病理号）</param>
        /// <returns></returns>
        public bool DeletePathInfo(ApplyFrmModel applyFrm)
        {
            if (!string.IsNullOrEmpty(applyFrm?.PathID)) // 需要病理号
            {
                var response = RegisterApi.Client.DeletePathInfo(applyFrm.PathID);
                return response.Success;
            }
            return false;
        }
        #endregion 病理

        #region 样本管理
        /// <summary>
        /// 分页查询样本登记列表（C端）
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public List<SampleRegisterModel> GetRegisterList(CustomPageModel pageModel, QueryAllRegModel queryModel)
        {
            var result = new List<SampleRegisterModel>();
            if (queryModel != null)
            {
                var response = RegisterApi.Client.GetRegisterList(pageModel.PageSize, pageModel.PageIndex,
                    queryModel.LaboratoryCode ?? string.Empty,
                    queryModel.Name ?? string.Empty,
                    queryModel.ProductID ?? string.Empty,
                    queryModel.StartDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    queryModel.EndDate?.ToString("yyyy-MM-dd") ?? string.Empty);
                if (response.Success && response.Data != null)
                {
                    result = response.Data.Records;
                    pageModel.TotalNum = response.Data.Total;
                    pageModel.TotalPage = response.Data.Pages;
                }
            }
            return result;
        }

        /// <summary>
        /// 按条形码查询待外送样本
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <returns></returns>
        public List<SampleRegisterModel> GetDeliverySamplebyBarcode(string barcode)
        {
            var result = new List<SampleRegisterModel>();
            if (!string.IsNullOrEmpty(barcode))
            {
                var response = RegisterApi.Client.GetDeliverySamplebyBarcode(barcode);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 批量保存外送样本
        /// </summary>
        /// <param name="samples">样本集合</param>
        /// <returns></returns>
        public bool SaveDeliverySampleList(IEnumerable<SampleRegisterModel> samples)
        {
            if (samples?.Count() > 0)
            {
                var sampIDs = samples.Select(x => x.ID);
                var response = RegisterApi.Client.SaveDeliverySampleList(sampIDs);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 根据病理id获取 补打条码数据
        /// </summary>
        /// <param name="pathID"></param>
        /// <returns></returns>
        public List<AppFrmPrintCode> GetAppFrmPrintCodeList(string pathID)
        {
            var result = new List<AppFrmPrintCode>();
            if (!string.IsNullOrEmpty(pathID))
            {
                var response = RegisterApi.Client.GetAppFrmPrintCodeList(pathID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        #endregion 样本管理
    }
}
