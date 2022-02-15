using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using HttpClientExtension.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    class UploadApi : BaseApi<UploadApi>
    {
#if DEBUG
        public const string UploadUrl = "https://upload.spt.deepsight.cloud/";
#else
        public const string UploadUrl = "https://upload.spt.deepsight.cloud/";
#endif

        [Url(UploadUrl, "sample/mincloud/search/index/")]
        [HttpGet]
        internal SampleUploadReturn GetSampleUploadListByDate([ParamName("file_path")] string filePath, [ParamName("sample_path")] string samplePath) => GetResult();


        [Url(UploadUrl, "sample/index/retrieval")]
        [HttpGet]
        internal ImageUploadReturn GetUploadImageList([ParamName("index_code")] string indexCode, [ParamName("retrieval_type")] string retrievalType) => GetResult();

    }
}
