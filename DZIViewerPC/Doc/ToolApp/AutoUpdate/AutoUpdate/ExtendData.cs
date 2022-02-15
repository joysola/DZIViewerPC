using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    public class ExtendData
    {
        /// <summary>
        /// 客户端的版本信息，不是当前工具的版本
        /// </summary>
        public static string MainClientVersion { get; set; }

        /// <summary>
        /// Minio 地址
        /// </summary>
        public static string MinIO_Endpoint { get; set; }

        /// <summary>
        /// minio账户
        /// </summary>
        public static string MinIO_AccessKey { get; set; }

        /// <summary>
        /// minio密码
        /// </summary>
        public static string MinIO_SecretKey { get; set; }

        /// <summary>
        /// 下载下来的文件所存放的地址
        /// </summary>
        public static string SystemVersionPath { get; } = Environment.CurrentDirectory + "\\Temp\\Version\\";
    }
}
