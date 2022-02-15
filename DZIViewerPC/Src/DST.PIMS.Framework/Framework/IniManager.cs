using DST.Common.Helper;
using DST.PIMS.Framework.ExtendContext;
using Newtonsoft.Json;

namespace DST.PIMS.Framework
{
    /// <summary>
    /// Ini文件读取
    /// </summary>
    public class IniManager
    {
        public static IniManager Instance { get; } = new IniManager();

        private IniManager()
        {

        }

        public T Read<T>(string section, string key, string defValue = "")
        {
            T t = default(T);
            string tmpSwitch = this.Read(section, key, defValue);
            try
            {
                t = JsonConvert.DeserializeObject<T>(tmpSwitch);
            }
            catch
            { 
            }

            return t;
        }

        /// <summary>
        /// 读ini文件，默认返回string
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public string Read(string section, string key, string defValue = "")
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            string tmpSwitch = IniHelper.CreateInstance(path).IniReadValue(section, key, defValue);
            return tmpSwitch;
        }
    }
}
