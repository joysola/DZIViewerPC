using Nico.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Helper
{
    public class HttpClientHelper
    {
        private static HttpClient Client { get; } = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<byte[]> DownFile(string url)
        {
            byte[] bytes = null;
            //int? imgSize = 0;
            try
            {
                var httpResponse = await Client.GetAsync(url).ConfigureAwait(false);
                if (httpResponse.IsSuccessStatusCode)
                {
                    bytes = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Logger.Error("HttpClientHelper的DownFile方法失败！,原因", e);
            }
            return bytes;
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<byte[]> DownFile(Uri uri) => await DownFile(uri.AbsoluteUri).ConfigureAwait(false);
    }
}
