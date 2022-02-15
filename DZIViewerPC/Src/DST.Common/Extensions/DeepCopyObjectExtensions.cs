using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Common.Extensions
{
    public static class DeepCopyObjectExtensions
    {
        /// <summary>
        /// 深度复制对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T obj)
        {
            var result = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
            return result;
        }
    }
}
