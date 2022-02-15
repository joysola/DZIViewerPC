using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using Newtonsoft.Json;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace DST.PIMS.Framework
{
    /// <summary>
    /// 上传样本原始图POST服务调用
    /// </summary>
    public class UploadServer
    {
        public static UploadServer Instance { get; } = new UploadServer();

        private UploadServer()
        {
        }

        /// <summary>
        /// 上传索引文件
        /// </summary>
        /// <param name="csvFile">csv完整路径</param>
        /// <param name="customid">当前登录者的客户ID</param>
        /// <returns>index_code，后续上传图片时有用</returns>
        public string UploadIndex(string csvFile, string customid)
        {
            string index_code = string.Empty;
            if (!File.Exists(csvFile))
            {
                return index_code;
            }

            string url = @"sample/upload/index";
            string csvName = Path.GetFileName(csvFile);
            var content = new MultipartFormDataContent();
            content.Add(new StringContent("mincloud"), "bucket");
            content.Add(new StringContent(csvName), "file_key");
            content.Add(new StringContent(customid), "file_path");
            content.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(csvFile)), "csv_file", csvName);
            Logger.Info($"上传索引文件{csvFile}");
            string res = this.UploadFile(url, content);
            Logger.Info($"上传索引文件{csvFile}返回结果：{res}");
            if (res.Contains("index_code"))
            {
                index_code = JsonConvert.DeserializeObject<dynamic>(res).result.index_code;
            }

            return index_code;
        }

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="imgFile">图片文件</param>
        /// <returns>index_code，后续上传图片时有用</returns>
        public async Task<bool> UploadImg(string imgFile, string index_code)
        {
            bool result = false;
            if (!File.Exists(imgFile))
            {
                return result;
            }

            try
            {
                string url = @"sample/upload/file";
                string imgName = Path.GetFileName(imgFile);
                var content = new MultipartFormDataContent();

                content.Add(new StringContent(index_code), "index_code");
                content.Add(new StringContent(imgName), "file_key");
                content.Add(new ByteArrayContent(File.ReadAllBytes(imgFile)), "file", imgName);
                Logger.Info($"上传图片：{imgFile}， index_code={index_code}");
                string res = this.UploadFile(url, content);
                Logger.Info($"上传图片{imgFile}返回结果：{res}");
                if (res.Contains("success"))
                {
                    result = JsonConvert.DeserializeObject<dynamic>(res).success;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"上传文件：{imgFile} 失败：" + e.Message);
            }

            return result;
        }

        public bool IsPropertyExist(dynamic data, string propertyname)
        {
            if (data is ExpandoObject)
                return ((IDictionary<string, object>)data).ContainsKey(propertyname);
            return data.GetType().GetProperty(propertyname) != null;
        }

        /// <summary>
        /// 获取样本上传状态
        /// </summary>
        /// <param name="sample_code"></param>
        /// <returns></returns>
        public SampleUpload GetSampleUploadListByCode(string sample_code)
        {
            // 记得在 ? 前面有个斜杠
            string url = $@"sample/mincloud/search/index/?sample_code={sample_code}";
            SampleUpload result = null;
            Uri uri = new Uri(CommonConfiguration.UploadUrl + url);
            string strResponse = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                    HttpResponseMessage response = client.GetAsync(uri).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        strResponse = response.Content.ReadAsStringAsync().Result;
                        SampleUploadReturn tmpRes = JsonConvert.DeserializeObject<SampleUploadReturn>(strResponse);
                        if (tmpRes != null && tmpRes.result != null && tmpRes.result.Count > 0)
                        {
                            result = tmpRes.result[0];
                        }
                    }

                    string res = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                Logger.Error("根据样本编号查询上传记录失败：" + e.Message);
            }

            return result;
        }

        /// <summary>
        /// 根据日期查询
        /// </summary>
        /// <param name="file_path">登录者customid</param>
        /// <param name="sample_path">时间字符串：yyyyMMdd</param>
        /// <returns></returns>
        public List<SampleUpload> GetSampleUploadListByDate(string file_path, string sample_path)
        {
            List<SampleUpload> result = new List<SampleUpload>();

            // 记得在 ? 前面有个斜杠
            string url = $@"sample/mincloud/search/index/?file_path={file_path}&sample_path={sample_path}";
            Uri uri = new Uri(CommonConfiguration.UploadUrl + url);
            string strResponse = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                    HttpResponseMessage response = client.GetAsync(uri).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        strResponse = response.Content.ReadAsStringAsync().Result;
                        SampleUploadReturn tmpRes = JsonConvert.DeserializeObject<SampleUploadReturn>(strResponse);
                        if (tmpRes != null)
                        {
                            result = tmpRes.result;
                        }
                    }

                    string res = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                Logger.Error("根据日期查询样本上传记录失败：" + e.Message);
            }

            return result;
        }

        /// <summary>
        /// 获取样本已上传或未上传的文件列表
        /// </summary>
        /// <param name="index_code">索引编号</param>
        /// <param name="retrieval_type">0==未上传文件，1==已上传文件</param>
        /// <returns></returns>
        public List<string> GetUploadImageList(string index_code, int retrieval_type)
        {
            List<string> result = new List<string>();
            string url = $@"sample/index/retrieval?index_code={index_code}&retrieval_type={retrieval_type}";
            Uri uri = new Uri(CommonConfiguration.UploadUrl + url);
            string strResponse = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(uri).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        strResponse = response.Content.ReadAsStringAsync().Result;
                        ImageUploadReturn tmpRes = JsonConvert.DeserializeObject<ImageUploadReturn>(strResponse);
                        if (tmpRes != null)
                        {
                            result = tmpRes.result;
                        }
                    }
                    else
                    {
                        Logger.Info("GetUploadImageList 返回 异常:" + response.ToString());
                    }

                    strResponse = null;
                }
            }
            catch (Exception e)
            {
                Logger.Error("获取样本已上传或者未上传文件列表失败：" + e.Message);
            }

            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url">api地址</param>
        /// <param name="content">上传内容</param>
        /// <returns></returns>
        private string UploadFile(string url, MultipartFormDataContent content)
        {
            string requestUrl = string.Empty;
            string result = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    requestUrl = CommonConfiguration.UploadUrl + url;
                    result = client.PostAsync(requestUrl, content).Result.Content.ReadAsStringAsync().Result;
                    
                }
            }
            catch (Exception e)
            {
                Logger.Error($"上传文件失败：{requestUrl} 。异常信息：" + e.Message);
            }
            finally
            {
                requestUrl = null;
            }

            return result;
        }
    }

}
