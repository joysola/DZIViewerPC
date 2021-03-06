using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;

namespace DST.ApiClient.Api
{
    class MBPSampleApi : BaseApi<MBPSampleApi>
    {
        /// <summary>
        /// 根据分页数据获取sample
        /// </summary>
        /// <param name="size"></param>
        /// <param name="current"></param>
        /// <param name="queryMBP"></param>
        /// <returns></returns>
        [Url("api/deepsight-sample/sample/info/queryCoBuildingSampleList")]
        [HttpPost]
        internal ApiResponse<ResponsePage<MBPSampleModel>> GetMBPSamples(int size, int current, [PostContent] QueryMBPSampleList queryMBP) => GetResult();

        /// <summary>
        /// 更新样本数据(注意 id 属性为空，则默认新增；不会空则为修改)
        /// </summary>
        /// <param name="queryMBP">需要更新的实体</param>
        /// <returns></returns>
        [Url("api/deepsight-sample/sample/info/saveCoBuildingSample")]
        [HttpPost]
        internal ApiResponse<bool> SaveMBPSample([PostContent] MBPSampleModel mbpModel) => GetResult();

        /// <summary>
        /// 样本退单
        /// </summary>
        /// <param name="queryBackMBP"></param>
        /// <returns></returns>
        [Url("api/deepsight-sample/sample/info/saveBackSample")]
        [HttpPost]
        internal ApiResponse<bool> BackMBPSample([PostContent] BackMBPSample backMBPSample) => GetResult();
    }
}