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
    public class MaterialService : BaseService<MaterialService>
    {
        #region 样本管理
        /// <summary>
        /// 条件分页查询待取材样本
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="queryModel">取材查询实体</param>
        /// <returns></returns>
        public List<SampleModel> GetMaterialList(CustomPageModel pageModel, QuerySample queryModel)
        {
            var result = new List<SampleModel>();
            if (queryModel != null)
            {
                var response = MaterialApi.Client.GetMaterialList(pageModel.PageSize, pageModel.PageIndex,
                    queryModel.DrawMaterialsType ?? string.Empty,
                    queryModel.Name ?? string.Empty,
                    queryModel.ProductID ?? string.Empty);
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
        /// 保存送检部位
        /// </summary>
        /// <param name="sampSpecDetail"></param>
        /// <returns></returns>
        public bool SaveSampSpecList(SampSpecDetail sampSpecDetail)
        {
            if (!string.IsNullOrEmpty(sampSpecDetail?.SampleId))
            {
                var response = MaterialApi.Client.SaveSampSpecList(sampSpecDetail);
                return response.Success;
            }
            return false;
        }

        #endregion 样本管理

        #region 样本组织管理

        /// <summary>
        /// 保存组织
        /// </summary>
        /// <param name="sampTiss">组织实体</param>
        /// <returns></returns>
        public bool SaveSampleTissue(SampTissModel sampTiss)
        {
            if (sampTiss != null)
            {
                var response = MaterialApi.Client.SaveSampleTissue(sampTiss);
                return response.Success;
            }
            return false;
        }

        #endregion 样本组织管理

        #region 组织取材包埋管理
        /// <summary>
        /// 按样本id打号
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public List<EmbedPrintCode> PrintCodebySampID(SampleModel sample)
        {
            var result = new List<EmbedPrintCode>();
            if (!string.IsNullOrEmpty(sample?.SampleID))
            {
                var response = MaterialApi.Client.PrintCodebySampID(sample.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 更新包埋盒数据
        /// </summary>
        /// <param name="embedBox"></param>
        /// <returns></returns>
        public bool SaveEmbedBoxInfo(EmbedBoxModel embedBox)
        {
            if (embedBox != null)
            {
                var response = MaterialApi.Client.SaveEmbedBoxInfo(embedBox);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        ///  新增包埋盒
        /// </summary>
        /// <param name="number">新增数量</param>
        /// <param name="embedBox">包埋盒实体</param>
        /// <returns></returns>
        public List<EmbedPrintCode> AddEmBedBoxInfo(int number, EmbedBoxModel embedBox)
        {
            var result = new List<EmbedPrintCode>();
            if (!string.IsNullOrEmpty(embedBox?.SampSpecID) && number > 0)
            {
                var response = MaterialApi.Client.AddEmBedBoxInfo(number, embedBox.SampSpecID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 删除包埋盒
        /// </summary>
        /// <param name="embedBox"></param>
        /// <returns></returns>
        public bool DeleteEmbedInfo(EmbedBoxModel embedBox)
        {
            if (!string.IsNullOrEmpty(embedBox?.ID))
            {
                var response = MaterialApi.Client.DeleteEmbedInfo(embedBox?.ID);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 打印包埋盒数据
        /// </summary>
        /// <param name="embedBoxs">包埋盒集合</param>
        /// <returns></returns>
        public List<EmbedPrintCode> PrintEmbedBoxList(params EmbedBoxModel[] embedBoxs)
        {
            var result = new List<EmbedPrintCode>();
            if (embedBoxs != null && embedBoxs.Length > 0)
            {
                var ids = embedBoxs.Select(x => x.ID); // 获取id集合
                var queryStr = string.Join(",", ids);
                var response = MaterialApi.Client.PrintEmbedBoxList(queryStr);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新取材状态（取材确认）
        /// </summary>
        /// <param name="waxBlockCode">蜡块编号</param>
        /// <returns></returns>
        public string UpdateMaterialStatus(string waxBlockCode)
        {
            string result = null;
            if (!string.IsNullOrEmpty(waxBlockCode))
            {
                var response = MaterialApi.Client.UpdateMaterialStatus(waxBlockCode);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        #endregion 组织取材包埋管理


        #region  样本组织取材延迟管理
        /// <summary>
        /// 根据样本获取组织取材延迟信息
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public SampTissDelayInfo GetSampTissDelayInfo(SampleModel sample)
        {
            SampTissDelayInfo sampTissDelay = null;
            if (!string.IsNullOrEmpty(sample?.SampleID))
            {
                var response = MaterialApi.Client.GetSampTissDelayInfo(sample?.SampleID);
                if (response.Success)
                {
                    sampTissDelay = response.Data;
                }
            }
            return sampTissDelay;
        }
        /// <summary>
        /// 保存组织取材延迟
        /// </summary>
        /// <param name="sampTissDelay">延缓取材实体</param>
        /// <returns></returns>
        public bool SaveSampTissDelayInfo(SampTissDelayInfo sampTissDelay)
        {
            if (!string.IsNullOrEmpty(sampTissDelay?.SampleId))
            {
                var response = MaterialApi.Client.SaveSampTissDelayInfo(sampTissDelay);
                return response.Success;
            }
            return false;
        }

        #endregion  样本组织取材延迟管理


        #region 送检部位管理

        #endregion 送检部位管理

        #region 组织大体图像管理
        /// <summary>
        /// 样本id查询 组织大体图像集合
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public List<SampTissImgModel> GetSampTissImgList(SampleModel sample)
        {
            var result = new List<SampTissImgModel>();
            if (!string.IsNullOrEmpty(sample?.SampleID))
            {
                var response = MaterialApi.Client.GetSampTissImgList(sample?.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 删除组织大体图像路径
        /// </summary>
        /// <param name="sampTissImg">组织大体图像实体</param>
        /// <returns></returns>
        public bool DeleteSampTissImg(SampTissImgModel sampTissImg)
        {
            if (!string.IsNullOrEmpty(sampTissImg?.ID))
            {
                var response = MaterialApi.Client.DeleteSampTissImg(sampTissImg?.ID);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 组织大体图像路径保存
        /// </summary>
        /// <param name="sampTissImg">组织大体图像实体</param>
        /// <returns></returns>
        public SampTissImgModel SaveSampTissImg(SampTissImgModel sampTissImg)
        {
            SampTissImgModel result = null;
            if (!string.IsNullOrEmpty(sampTissImg?.SampleID))
            {
                var response = MaterialApi.Client.SaveSampTissImg(sampTissImg);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 设置为采图
        /// </summary>
        /// <param name="sampTissImg">组织大体图像实体</param>
        /// <returns></returns>
        public bool UpdateReportDrawStatus(SampTissImgModel sampTissImg)
        {
            if (!string.IsNullOrEmpty(sampTissImg?.ID))
            {
                var response = MaterialApi.Client.UpdateReportDrawStatus(sampTissImg?.ID);
                return response.Success;
            }
            return false;
        }
        #endregion 组织大体图像管理
    }
}
