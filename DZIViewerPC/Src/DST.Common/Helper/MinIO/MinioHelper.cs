using Minio;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DST.Common.MinioHelper
{
    /// <summary>
    /// Minio帮助类
    /// </summary>
    public class MinioHelper
    {
        /// <summary>
        /// minio客户端
        /// </summary>
        private MinioClient minioClient = null;

        /// <summary>
        /// minio服务器地址，不需要加http,也不要以"/"结尾
        /// </summary>
        private string endPoint = string.Empty;

        /// <summary>
        /// minio授权登录账号
        /// </summary>
        private string accessKey = string.Empty;

        /// <summary>
        /// minio授权登录密码
        /// </summary>
        private string secretKey = string.Empty;

        /// <summary>
        /// minio连接状态
        /// </summary>
        public bool IsConnection
        {
            get
            {
                return this.minioClient != null;
            }
        }

        /// <summary>
        /// Minio访问客户端
        /// </summary>
        public static MinioHelper Client { get; } = new MinioHelper();

        /// <summary>
        /// 单例构造
        /// </summary>
        private MinioHelper()
        {
        }

        /// <summary>
        /// 连接minio
        /// </summary>
        /// <param name="endPoint">minio服务器地址，不需要加http,也不要以"/"结尾</param>
        /// <param name="accessKey">用户名</param>
        /// <param name="secretKey">密码</param>
        /// <returns>连接状态</returns>
        public bool Connection(string endPoint, string accessKey, string secretKey)
        {
            bool result = true;

            if (this.IsConnection)
            {
                return true;
            }

            try
            {
                this.endPoint = endPoint;
                this.accessKey = accessKey;
                this.secretKey = secretKey;
                // 如果Endpoint是https，则需要加 WithSSL(), 不是则不需要
                this.minioClient = new MinioClient(this.endPoint, this.accessKey, this.secretKey).WithSSL();
            }
            catch(Exception e)
            {
                result = false;
                Logger.Error("minio连接异常：" + e.Message);
            }
            return result;
        }


        public async Task<string> UploadFile(string uploadFileFullName, string bucketName, string buckChild, Action<int> processCallBack = null)
        {
            var url = string.Empty;
            var contentType = MimeMapping.GetMimeMapping(uploadFileFullName);
            var objectName = await this.UploadFile(uploadFileFullName, bucketName, contentType, buckChild, processCallBack).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(objectName))
            {
                url = $"https://{this.endPoint}/{bucketName}/{objectName}";
            }

            return url;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadFileFullName">上传文件的完整路径和文件名称</param>
        /// <param name="bucketName">Minio桶名称</param>
        /// <param name="bucketChildName">Minio桶下的子目录,示例 "first/"</param>
        /// <param name="processCallBack">上传进度回传</param>
        /// <returns></returns>
        private async Task<string> UploadFile(string uploadFileFullName, string bucketName, string contentType, string bucketChildName = "", Action<int> processCallBack = null)
        {
            string result = "";
            if (!this.IsConnection)
            {
                return result;
            }

            if (!System.IO.File.Exists(uploadFileFullName))
            {
                return result;
            }

            try
            {
                // 判断桶是否存在
                if (!this.minioClient.BucketExistsAsync(bucketName).Result)
                {
                    // 创建桶
                    await this.minioClient.MakeBucketAsync(bucketName).ConfigureAwait(false);
                }

                System.IO.FileInfo fileInfo = new System.IO.FileInfo(uploadFileFullName);
                string objectName = bucketChildName + @"/" + fileInfo.Name;

                // 添加上传进度日志
                this.minioClient.SetTraceOn(new MinioLogHelper(processCallBack));
                await this.minioClient.PutObjectAsync(bucketName, objectName, uploadFileFullName, contentType).ConfigureAwait(false);
                result = objectName;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.minioClient.SetTraceOff();
            }

            return result;
        }

        /// <summary>
        /// 根据minio链接地址，下载文件到指定位置
        /// </summary>
        /// <param name="fileMinioUrl">要下载的文件地址，完整的minio链接地址</param>
        /// https://image03-sz.deepsight.cloud/deepsight.cs/deepsight.report.system/PDF/345.pdf
        /// <param name="localPath">下载文件的保存地址，包含文件名和后缀：D:\11.pdf</param>
        /// <returns>下载成功与否</returns>
        public async Task<bool> DownloadFile(string fileMinioUrl, string localPath)
        {
            if (!this.IsConnection)
            {
                return false;
            }

            try
            {
                if (string.IsNullOrEmpty(fileMinioUrl))
                {
                    return false;
                }

                // 解析minio地址:$"https://{MinioHelper.Endpoint}/{bucketName}/{objectName}";
                int index = fileMinioUrl.IndexOf(this.endPoint);
                string tmp = fileMinioUrl.Substring(index + this.endPoint.Length + 1);
                index = tmp.IndexOf(@"/");
                string bucketName = tmp.Substring(0, index);
                string objectName = tmp.Substring(index + 1);
                if (System.IO.File.Exists(localPath))
                {
                    System.IO.File.Delete(localPath);
                }
                return this.DownloadFile(bucketName, objectName, localPath).Result;
            }
            catch (Exception ex)
            {
                Logger.Error("下载文件异常：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 根据桶名和对象路径，下载文件
        /// </summary>
        /// <param name="bucketName">桶名</param>
        /// <param name="objectName">对象路径</param>
        /// <param name="localPath">下载文件保存的地址，包含文件名和后缀名</param>
        /// <returns>下载成功状态</returns>
        public async Task<bool> DownloadFile(string bucketName, string objectName, string localPath)
        {
            if (!this.IsConnection)
            {
                return false;
            }

            if (string.IsNullOrEmpty(bucketName) || !await this.minioClient.BucketExistsAsync(bucketName).ConfigureAwait(false))
            {
                return false;
            }

            try
            {
                await this.minioClient.GetObjectAsync(bucketName, objectName, localPath).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error("MinIO的DownloadFile方法出错！", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取文件下载链接(这个有问题...)
        /// </summary>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="objectName">对象名称</param>
        /// <param name="expiresInt">失效时间（以秒为单位），默认是7天，不得大于七天</param>
        /// <returns></returns>
        private async Task<string> GetDownloadUrl(string bucketName, string objectName, int expiresInt = 60 * 60 * 24 * 5)
        {
            if (!this.IsConnection)
            {
                return "minio未连接";
            }

            var result = await this.minioClient.PresignedGetObjectAsync(bucketName, objectName, expiresInt);
            return result;
        }

        /// <summary>
        /// 获取桶下的所有文件
        /// </summary>
        /// <param name="bucketName">桶名</param>
        /// <param name="prefix">对象的前缀，类似通配符</param>
        /// <param name="recursive">true代表递归查找，false代表类似文件夹查找，以'/'分隔，不查子文件夹</param>
        /// <param name="result">返回的结果</param>
        /// <param name="onCompleted">获取完成后的事件</param>
        /// <returns></returns>
        public async Task<bool> GetFileNamesinBucket(string bucketName, string prefix, bool recursive , List<string> result, Action onCompleted = null)
        {
            if (!this.IsConnection || result == null)
            {
                return false;
            }

            try
            {
                bool found = this.minioClient.BucketExistsAsync(bucketName).Result;
                if (found)
                {
                    await Task.Run(() =>
                    {
                        bool isCom = false;
                        IObservable<Minio.DataModel.Item> observable = this.minioClient.ListObjectsAsync(bucketName, prefix, recursive);
                        IDisposable subscription = observable.Subscribe(
                        item => { result.Add(item.Key); },
                        ex => throw ex,
                        () =>
                        {
                            if (onCompleted != null)
                            {
                                onCompleted();
                            }
                            isCom = true;
                        });

                        while(!isCom)
                        {
                            Task.Delay(200);
                        }
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("MinIO的GetFilesinBucket方法报错！", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据url地址，直接下载文件
        /// </summary>
        /// <param name="url">url链接，必须是文件的链接</param>
        /// <param name="localPath">保存的本地位置</param>
        /// <returns>下载成功标志</returns>
        public async Task<bool> DownloadFileByUrl(string url, string localPath)
        {
            if (!this.IsConnection)
            {
                return false;
            }
            bool result = true;
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)";
                request.Proxy = null;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                using (Stream responseStream = response.GetResponseStream())
                {
                    //创建本地文件写入流
                    using (Stream stream = new FileStream(localPath, FileMode.Create))
                    {
                        byte[] bArr = new byte[1024 * 1024];
                        int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                        while (size > 0)
                        {
                            stream.Write(bArr, 0, size);
                            size = responseStream.Read(bArr, 0, (int)bArr.Length);
                        }

                        stream.Close();
                        responseStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Logger.Error($"下载文件 {url} 异常：" + ex.Message);
            }

            return result;
        }
    }
}