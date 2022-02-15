using DST.ApiClient.Api;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using HttpClientExtension.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Service
{
    public class UploadService : BaseService<UploadService>
    {
        /// <summary>
        /// 根据日期查询
        /// </summary>
        /// <param name="filePath">登录者customid</param>
        /// <param name="samplePath">时间字符串：yyyyMMdd</param>
        /// <returns></returns>
        public List<SampleUpload> GetSampleUploadListByDate(string filePath, string samplePath)
        {
            var result = UploadApi.Client.GetSampleUploadListByDate(filePath,samplePath);
            return result?.result;
        }
        /// <summary>
        /// 获取样本已上传或未上传的文件列表
        /// </summary>
        /// <param name="indexCode">索引编号</param>
        /// <param name="retrievalType">0==未上传文件，1==已上传文件</param>
        /// <returns></returns>
        public List<string> GetUploadImageList(string indexCode, string retrievalType)
        {
            var result = UploadApi.Client.GetUploadImageList(indexCode, retrievalType);
            return result?.result;
        }
    }
}
