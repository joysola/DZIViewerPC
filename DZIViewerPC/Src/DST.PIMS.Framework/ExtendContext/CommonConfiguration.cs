using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ExtendContext
{
    /// <summary>
    /// 系统内部配置中心，包含测试版和正式版。
    /// DEBUG是测试版，Release是正式版本。
    /// 每一个参数都必须包含DEBUG和Release版本
    /// </summary>
    public static class CommonConfiguration
    {
        /// <summary>
        /// 服务端地址
        /// 正式环境：https://patho.deepsight.cloud/
        /// 测试环境：https://dst-sz.deepsight.cloud/
        /// </summary>
        public static string BaseApi
        {
            get
            {
#if DEBUG
                //return "http://192.168.101.92:15000";
                return "https://dst-sz.deepsight.cloud/api/";
            
#else
                return "https://patho.deepsight.cloud/api/";
#endif
            }
        }

        /// <summary>
        /// Minio地址，无需加http
        /// </summary>
        public static string MinIO_Endpoint
        {
            get
            {
#if DEBUG
                return "image03-sz.deepsight.cloud";
#else
                return "image02-sz.deepsight.cloud";
#endif
            }
        }

        /// <summary>
        /// minio账户
        /// </summary>
        public static string MinIO_AccessKey
        {
            get
            {
#if DEBUG
                return "minio";
#else
                return "minio";
#endif
            }
        }

        /// <summary>
        /// minio密码
        /// </summary>
        public static string MinIO_SecretKey
        {
            get
            {
#if DEBUG
                return "deepsight0110";
#else
                return "deepsight0110";
#endif
            }
        }

        /// <summary>
        /// minio通用桶名称
        /// </summary>
        public static string BucketName
        {
            get
            {
#if DEBUG
                return "deepsight2.0";
#else
                return "deepsight3.0";
#endif
            }
        }

        /// <summary>
        /// 上传服务端URL地址
        /// </summary>
        public static string UploadUrl
        {
            get
            {
#if DEBUG
                return "https://upload.spt.deepsight.cloud/";
#else
                return "https://upload.spt.deepsight.cloud/";
#endif
            }
        }
        /// <summary>
        /// api版本
        /// </summary>
        public static string ApiVersion => "1";
    }
}
