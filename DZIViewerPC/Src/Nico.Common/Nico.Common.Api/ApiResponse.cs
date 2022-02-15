using System;
using System.Collections.Generic;
using System.Text;

namespace Nico.Common
{
    /// <summary>
    /// api返回结果的通用泛型
    /// </summary>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
    /// <summary>
    /// 响应基类
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否成功请求
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }
    }
}
