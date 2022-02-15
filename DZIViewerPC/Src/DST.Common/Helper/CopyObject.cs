using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DST.Common.Helper
{
    public class CopyObject
    {
        /// <summary>
        /// 根据反射复制同一类型的对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>新对象</returns>
        public static T CopyObjectByReflection<T>(T source)
        {
            T result = Activator.CreateInstance<T>();
            Type tType = source.GetType();
            foreach (var itemOut in result.GetType().GetProperties())
            {
                var itemIn = tType.GetProperty(itemOut.Name);
                if (itemIn != null)
                {
                    itemOut.SetValue(result, itemIn.GetValue(source));
                }
            }
            return result;
        }

        /// <summary>
        /// 使用JSon复制对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>新对象</returns>
        public static T CopyObjectByJson<T>(T source)
        {
            string strSource = JsonConvert.SerializeObject(source);
            T result = JsonConvert.DeserializeObject<T>(strSource);
            return result;
        }

        /// <summary>
        /// 将对象序列化为Base64字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            string result = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter _binaryFormatter = new BinaryFormatter();
                _binaryFormatter.Serialize(ms, obj);
                var bytes = ms.ToArray();
                result = Convert.ToBase64String(bytes); // 配置字符串
            }
            return result;
        }
        /// <summary>
        /// 将base64字符串解析成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string base64Str)
        {
            T result;
            var bytes = Convert.FromBase64String(base64Str);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                BinaryFormatter _binaryFormatter = new BinaryFormatter();
                result = (T)_binaryFormatter.Deserialize(ms);
            }
            return result;
        }
    }
}