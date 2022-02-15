using DST.ApiClient.Api;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using HttpClientExtension.Service;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DST.ApiClient.Service
{
    public class ApplyFormService : BaseService<ApplyFormService>
    {
        /// <summary>
        /// 获取申请单信息
        /// </summary>
        /// <param name="code">实验室编号（条码号、his识别号）</param>
        /// <returns></returns>
        public ApplyFrmModel GetPathInfobyCode(string code)
        {
            ApplyFrmModel result = null;
            if (!string.IsNullOrEmpty(code))
            {
                var response = ApplyFormApi.Client.GetPathInfobyCode(code);
                if (response.Success)
                {
                    result = response.Data;
                    foreach (var samp in result?.PathSampInfoList)
                    {
                        samp.MarkerList = GenerateMarkerList(samp.ProductID); // 生成标记物染色剂集合
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 保存申请单
        /// </summary>
        /// <param name="applyInfo">申请单信息</param>
        /// <returns></returns>
        public List<AppFrmPrintCode> SavePathInfo(ApplyFrmModel applyInfo)
        {
            var result = new List<AppFrmPrintCode>();
            var response = ApplyFormApi.Client.SavePathInfo(applyInfo);
            if (response.Success)
            {
                result = response.Data;
            }
            return result;
        }

        /// <summary>
        /// 保存外送
        /// </summary>
        /// <param name="applyInfo">申请单信息</param>
        /// <returns></returns>
        public List<AppFrmPrintCode> SavePathandSend(ApplyFrmModel applyInfo)
        {
            var result = new List<AppFrmPrintCode>();
            var response = ApplyFormApi.Client.SavePathandSend(applyInfo);
            if (response.Success)
            {
                result = response.Data;
            }
            return result;
        }
        /// <summary>
        /// 根据病理ID获取申请单图片列表
        /// </summary>
        public List<PathologyImage> ListByPathologyId(string pathologyId)
        {
            var resp = ApplyFormApi.Client.ListByPathologyId(pathologyId);
            if (resp != null)
            {
                return resp.Data;
            }
            else
            {
                return new List<PathologyImage>();
            }
        }

        /// <summary>
        /// 删除病理图片
        /// </summary>
        /// <param name="id">主键，不是病理ID</param>
        /// <returns></returns>
        public bool RemoveById(string id)
        {
            ApiResponse<object> res = ApplyFormApi.Client.RemoveById(id);
            return res.Success;
        }

        /// <summary>
        /// 保存申请单图片
        /// </summary>
        /// <param name="pathologyImage"></param>
        /// <returns></returns>
        public bool SavePathologyImage(PathologyImage pathologyImage)
        {
            ApiResponse<object> res = ApplyFormApi.Client.SavePathologyImage(pathologyImage);
            return res.Success;
        }

        /// <summary>
        /// 查询特检配置列表
        /// </summary>
        /// <param name="productID">样本id</param>
        /// <returns></returns>
        public List<SpecStainSetting> GetSpecStainSetList(string productID)
        {
            var result = new List<SpecStainSetting>();
            if (!string.IsNullOrEmpty(productID))
            {
                var response = ProductApi.Client.GetSpecStainSetList(productID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }

        /// <summary>
        /// 生成标记物染色剂 集合(非api方法)
        /// </summary>
        /// <param name="productID">项目id</param>
        /// <returns></returns>
        public List<DictItem> GenerateMarkerList(string productID)
        {
            var result = new List<DictItem>();
            if (!string.IsNullOrEmpty(productID))
            {

                // 胃镜特殊处理
                if (DSTCode.GastroscopeProdID == productID)
                {
                    var specStnSetList = GetSpecStainSetList(productID); // 查找项目对应的特殊染色配置
                    result = specStnSetList.Select(x => new DictItem { dictKey = x.GroupKey, dictValue = x.Name }).ToList();
                }
                // 免疫组化处理
                else if (DSTCode.ImmuhistchmProdID == productID)
                {
                    result = ExtendApiDict.Instance.ProdReagentDict;
                }
            }
            return result;
        }
    }
}
