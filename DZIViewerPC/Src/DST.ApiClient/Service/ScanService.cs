using DST.ApiClient.Api;
using DST.Database.Model;
using HttpClientExtension.Service;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Service
{
    public class ScanService : BaseService<ScanService>
    {
        /// <summary>
        /// 分页查询扫描工作站的数据
        /// </summary>
        public ScanReturnModel PageByCellType(ScanQueryModel sampleScanDto, int current, int size)
        {
            sampleScanDto.productionTimeEnd = sampleScanDto.productionTimeEnd?.AddDays(1.0).AddSeconds(-1.0);
            sampleScanDto.scanTimeEnd = sampleScanDto.scanTimeEnd?.AddDays(1.0).AddSeconds(-1.0);
            ScanReturnModel res = ScanApi.Client.PageByCellType(sampleScanDto, current, size).Data;
            return res;
        }

        /// <summary>
        /// 批量接收数据
        /// </summary>
        /// <param name="sampleScanIdList">样本ID List</param>
        /// <returns></returns>
        public bool BathReceive(List<string> sampleScanIdList)
        {
            ApiResponse<object> res = ScanApi.Client.BathReceive(sampleScanIdList);
            return res.Success;
        }

        /// <summary>
        /// 扫描列表查询
        /// </summary>
        /// <returns></returns>
        public ScanReturnModel PageScanQuery(ScanQueryModel sampleScanDto, int current, int size)
        {
            sampleScanDto.productionTimeEnd = sampleScanDto.productionTimeEnd?.AddDays(1.0).AddSeconds(-1.0);
            sampleScanDto.scanTimeEnd = sampleScanDto.scanTimeEnd?.AddDays(1.0).AddSeconds(-1.0);
            ScanReturnModel res = ScanApi.Client.PageScanQuery(sampleScanDto, current, size).Data;
            return res;
        }

        /// <summary>
        /// 重新制片
        /// </summary>
        /// <returns></returns>
        public ApiResponse<object> SaveTechnicalAdviceByScan(List<string> codeList)
        {
            return ScanApi.Client.ProductionSlice(codeList);
        }

        /// <summary>
        /// 扫码接收
        /// </summary>
        public ApiResponse<object> ReceiveByCode(string code)
        {
            return ScanApi.Client.ReceiveByCode(code);
        }

        /// <summary>
        /// 根据切片编号查询
        /// </summary>
        public ScanModel GetSampleScanByCode(string code)
        {
            return ScanApi.Client.GetSampleScanByCode(code).Data;
        }

        /// <summary>
        /// 批量接收
        /// </summary>
        /// <param name="codeList"></param>
        /// <returns></returns>
        public bool ReceiveByCodeList(List<string> codeList)
        {
            ApiResponse<object> res = ScanApi.Client.ReceiveByCodeList(codeList);
            return res.Success;
        }
    }
}
