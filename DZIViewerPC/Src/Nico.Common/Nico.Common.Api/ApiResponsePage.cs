using System;
using System.Collections.Generic;
using System.Text;

namespace Nico.Common
{
    /// <summary>
    /// api返回的列表结果的通用泛型
    /// </summary>
    public class ApiResponsePage<T>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 当前分页条件下共多少页
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// 每页记录的数量
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 当前页数下的列表
        /// </summary>
        public List<T> Records { get; set; }
    }
}
