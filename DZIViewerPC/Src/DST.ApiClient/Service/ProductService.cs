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
    public class ProductService : BaseService<ProductService>
    {
        #region 样本组织管理
        /// <summary>
        /// 条件分页查询待 切片(制片)样本
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="queryModel">取材查询实体</param>
        /// <returns></returns>
        public List<SampleModel> GetProductList(CustomPageModel pageModel, QuerySample queryModel)
        {
            var result = new List<SampleModel>();
            if (queryModel != null)
            {
                var response = ProductApi.Client.GetProductList(pageModel.PageSize, pageModel.PageIndex,
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
        #endregion 样本组织管理

        #region 样本切片管理
        /// <summary>
        /// 根据样本获取切片列表
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public List<SliceModel> GetSliceList(SampleModel sample)
        {
            var result = new List<SliceModel>();
            if (!string.IsNullOrEmpty(sample?.SampleID))
            {
                var response = ProductApi.Client.GetSliceList(sample?.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 样本id获取制片的组织切片列表
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public List<SliceModel> GetProdSliceList(SampleModel sample)
        {
            var result = new List<SliceModel>();
            if (!string.IsNullOrEmpty(sample?.SampleID))
            {
                var response = ProductApi.Client.GetProdSliceList(sample?.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 保存组织切片信息
        /// </summary>
        /// <param name="slice">切片实体</param>
        /// <returns></returns>
        public bool SaveSlice(SliceModel slice)
        {
            if (!string.IsNullOrEmpty(slice?.SampleID))
            {
                var response = ProductApi.Client.SaveSlice(slice);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 删除组织切片
        /// </summary>
        /// <param name="slice">切片实体</param>
        /// <returns></returns>
        public bool DeleteSlice(SliceModel slice)
        {
            if (!string.IsNullOrEmpty(slice?.ID))
            {
                var response = ProductApi.Client.DeleteSlice(slice?.ID);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 批量打印切片样本
        /// </summary>
        /// <param name="sliceList">切片列表</param>
        /// <returns></returns>
        public List<SlicePrintCode> PrintSliceCodeList(IEnumerable<SliceModel> sliceList)
        {
            var result = new List<SlicePrintCode>();
            if (sliceList?.Count() > 0)
            {
                var sliceIDs = sliceList.Select(x => x.ID);
                var response = ProductApi.Client.PrintSliceCodeList(sliceIDs);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 组织蜡块生成切片(单扫)
        /// </summary>
        /// <param name="waxBlockCode">蜡块编号</param>
        /// <returns></returns>
        public SlicePrintCode ScanCodeGeneSlice(string waxBlockCode)
        {
            var result = new List<SlicePrintCode>();
            if (!string.IsNullOrEmpty(waxBlockCode))
            {
                var response = ProductApi.Client.ScanCodeGeneSlice(waxBlockCode);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result?.FirstOrDefault();
        }
        /// <summary>
        /// 组织蜡块生成切片(特染)
        /// </summary>
        /// <param name="query">制片特染查询实体</param>
        /// <returns></returns>
        public SlicePrintCode ScanCodeGeneSliceSpec(QueryAddSlice query)
        {
            var result = new List<SlicePrintCode>();
            if (!string.IsNullOrEmpty(query?.WaxBlockCode))
            {
                var response = ProductApi.Client.ScanCodeGeneSliceSpec(query.WaxBlockCode, query.Marker ?? string.Empty);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result?.FirstOrDefault();
        }
        /// <summary>
        /// 条件分页查询细胞制片样本
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="queryModel">查询实体</param>
        /// <returns></returns>
        public List<SliceTCTModel> GetSliceTCTList(CustomPageModel pageModel, QuerySample queryModel)
        {
            var result = new List<SliceTCTModel>();
            if (queryModel != null)
            {
                var response = ProductApi.Client.GetSliceTCTList(pageModel.PageSize, pageModel.PageIndex,
                    queryModel.Name,
                    queryModel.ProductID,
                    queryModel.StartTime?.ToString("yyyy-MM-dd"),
                    queryModel.EndTime?.ToString("yyyy-MM-dd"));
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
        /// 批量打印细胞切片样本
        /// </summary>
        /// <param name="sliceList">切片实体集合</param>
        /// <returns></returns>
        public List<SlicePrintCode> PrintTCTSliceCodeList(IEnumerable<SliceTCTModel> sliceList)
        {
            var result = new List<SlicePrintCode>();
            if (sliceList?.Count() > 0)
            {
                var sliceIDs = sliceList.Select(x => x.ID);
                var response = ProductApi.Client.PrintTCTSliceCodeList(sliceIDs);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 条件分页查询制片样本
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="query">查询实体</param>
        /// <returns></returns>
        public List<SliceProdModel> GetSliceProdList(CustomPageModel pageModel, QuerySample query)
        {
            var result = new List<SliceProdModel>();
            if (query != null)
            {
                var response = ProductApi.Client.GetSliceProdList(pageModel.PageSize, pageModel.PageIndex,
                    query.Name, query.ProductID,
                    query.AdviceType,
                    query.LabCode,
                    query.StartTime?.ToString("yyyy-MM-dd"),
                    query.EndTime?.ToString("yyyy-MM-dd"));
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
        /// 打印医嘱
        /// </summary>
        /// <param name="slices">切片集合</param>
        /// <param name="sampleModel">样本实体</param>
        /// <param name="cellType">医嘱的类型 技术医嘱3， 特检是4</param>
        /// <returns></returns>
        private List<SlicePrintCode> GetAdviceSliceCodeList(IEnumerable<SliceModel> slices, SampleModel sampleModel, string cellType)
        {
            var result = new List<SlicePrintCode>();
            if (!string.IsNullOrEmpty(sampleModel?.SampleID) && !string.IsNullOrEmpty(cellType) && slices?.Count() > 0 && !string.IsNullOrEmpty(sampleModel?.AdviceAuditID))
            {
                var cuttingIDs = slices.Select(x => x.ID)?.ToList();
                var response = ProductApi.Client.GetAdviceSliceCodeList(new QueryAdvicePrint { AdviceAuditID = sampleModel?.AdviceAuditID, CellType = cellType, SampleID = sampleModel.SampleID, CuttingIDs = cuttingIDs });
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 技术医嘱 打印切片
        /// </summary>
        /// <param name="slices">切片实体</param>
        /// <param name="sampleModel">样本实体</param>
        /// <returns></returns>
        public List<SlicePrintCode> GetTechAdSliceCodeList(IEnumerable<SliceModel> slices, SampleModel sampleModel) =>
            GetAdviceSliceCodeList(slices, sampleModel, "3");


        /// <summary>
        /// 特检医嘱 打印切片
        /// </summary>
        /// <param name="slices">切片实体</param>
        /// <param name="sampleModel">样本实体</param>
        /// <returns></returns>
        public List<SlicePrintCode> GetDoctAdSliceCodeList(IEnumerable<SliceModel> slices, SampleModel sampleModel) =>
            GetAdviceSliceCodeList(slices, sampleModel, "4");

        #endregion 样本切片管理

        #region 样本组织切片管理
        /// <summary>
        /// 根据样本获取蜡块切片统计信息
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public SliceStatistics GetSliceStatistics(SampleModel sample)
        {
            var result = new SliceStatistics();
            if (!string.IsNullOrEmpty(sample?.SampleID))
            {
                var response = ProductApi.Client.GetSliceStatistics(sample?.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 样本获取技术医嘱切片列表
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public List<SliceModel> GetTechAdviSliceList(SampleModel sample)
        {
            var result = new List<SliceModel>();
            if (!string.IsNullOrEmpty(sample?.SampleID))
            {
                var response = ProductApi.Client.GetTechAdviSliceList(sample?.SampleID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 保存技术医嘱、特检医嘱 切片信息
        /// </summary>
        /// <param name="slice">切片实体</param>
        /// <returns></returns>
        public SliceModel SaveTechAdviSlice(SliceModel slice)
        {
            SliceModel result = null;
            if (!string.IsNullOrEmpty(slice?.SampleID))
            {
                var response = ProductApi.Client.SaveTechAdviSlice(slice);
                if(response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        #endregion 样本组织切片管理


        #region 技术医嘱审核管理
        /// <summary>
        /// 分页条件查询技术医嘱（制片环节）
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="queryModel">查询实体</param>
        /// <returns></returns>
        public List<SampleModel> GetTechAdviceSampList(CustomPageModel pageModel, QuerySample queryModel)
        {
            var result = new List<SampleModel>();
            if (queryModel != null)
            {
                var response = ProductApi.Client.GetTechAdviceSampList(pageModel.PageSize, pageModel.PageIndex,
                    queryModel.Name,
                    queryModel.ProductID,
                    queryModel.StartTime?.ToString("yyyy-MM-dd"),
                    queryModel.EndTime?.ToString("yyyy-MM-dd"));
                if (response.Success && response.Data != null)
                {
                    result = response.Data.Records;
                    pageModel.TotalNum = response.Data.Total;
                    pageModel.TotalPage = response.Data.Pages;
                }
            }
            return result;
        }

        #endregion 技术医嘱审核管理

        #region 特检医嘱审核管理
        /// <summary>
        /// 分页条件查询特检医嘱（制片环节）
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="queryModel">查询实体</param>
        /// <returns></returns>
        public List<SampleModel> GetDoctAdviceSampList(CustomPageModel pageModel, QuerySample queryModel)
        {
            var result = new List<SampleModel>();
            if (queryModel != null)
            {
                var response = ProductApi.Client.GetDoctAdviceSampList(pageModel.PageSize, pageModel.PageIndex,
                    queryModel.Name,
                    queryModel.ProductID,
                    queryModel.StartTime?.ToString("yyyy-MM-dd"),
                    queryModel.EndTime?.ToString("yyyy-MM-dd"));
                if (response.Success && response.Data != null)
                {
                    result = response.Data.Records;
                    pageModel.TotalNum = response.Data.Total;
                    pageModel.TotalPage = response.Data.Pages;
                }
            }
            return result;
        }
        #endregion 特检医嘱审核管理


        #region 组织取材包埋管理
        /// <summary>
        /// 按病理id查询切片列表
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        public List<EmbedBoxModel> GetEmbedBoxListbyPathID(SampleModel sample)
        {
            var result = new List<EmbedBoxModel>();
            if (!string.IsNullOrEmpty(sample?.PathID))
            {
                var response = ProductApi.Client.GetSliceListbyPathID(sample?.PathID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        #endregion 组织取材包埋管理

        #region 产品特检配置管理
        /// <summary>
        /// 查询特检配置列表
        /// </summary>
        /// <param name="sample">样本实体</param>
        /// <returns></returns>
        public List<SpecStainSetting> GetSpecStainSetList(SampleModel sample)
        {
            var result = new List<SpecStainSetting>();
            if (!string.IsNullOrEmpty(sample?.ProductID))
            {
                var response = ProductApi.Client.GetSpecStainSetList(sample?.ProductID);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        #endregion 产品特检配置管理
    }
}
