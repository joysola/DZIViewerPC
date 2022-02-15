using Newtonsoft.Json;
using System.Collections.Generic;

namespace DST.Database.Model
{
    /// <summary>
    /// 返回实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //public class ApiResponse<T>
    //{
    //    /// <summary>
    //    /// 状态码
    //    /// </summary>
    //    [JsonProperty("code")]
    //    public string Code { get; set; }

    //    /// <summary>
    //    /// 是否成功请求
    //    /// </summary>
    //    [JsonProperty("success")]
    //    public bool Success { get; set; }

    //    /// <summary>
    //    /// 数据
    //    /// </summary>
    //    [JsonProperty("data")]
    //    public T Data { get; set; }

    //    /// <summary>
    //    /// 返回消息
    //    /// </summary>
    //    [JsonProperty("msg")]
    //    public string Msg { get; set; }
    //}

    /// <summary>
    /// 分页实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponsePage<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [JsonProperty("current")]
        public int Current { get; set; }

        /// <summary>
        /// 当前分页条件下共多少页
        /// </summary>
        [JsonProperty("pages")]
        public int Pages { get; set; }

        /// <summary>
        /// 每页记录的数量
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// 具体实例
        /// </summary>
        [JsonProperty("records")]
        public List<T> Records { get; set; }
    }
}