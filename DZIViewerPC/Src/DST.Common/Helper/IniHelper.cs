using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace DST.Common.Helper
{
    /// <summary>
    /// ini文件读写帮助类
    /// </summary>
    public class IniHelper
    {
        private string iniPath = string.Empty;
        private static readonly SemaphoreSlim locker = new SemaphoreSlim(1, 1);
        /// <summary>
        /// 创建ini文件读取对象
        /// </summary>
        /// <param name="iniPath">ini文件路径</param>
        /// <returns>新的对象</returns>
        public static IniHelper CreateInstance(string iniPath)
        {
            return new IniHelper(iniPath);
        }

        /// <summary>
        /// 私有构造
        /// </summary>
        /// <param name="iniPath"></param>
        private IniHelper(string iniPath)
        {
            this.iniPath = iniPath;
        }

        /// <summary> 
        /// 写入INI文件，如果ini文件不存在则会自动创建
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string section, string key, string value)
        {
            locker.Wait();
            WritePrivateProfileString(section, key, value, this.iniPath);
            locker.Release();
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="defValue">默认值，当ini文件中没有数据时，则返回默认值</param> 
        public string IniReadValue(string section, string key, string defValue = "")
        {
            try
            {
                StringBuilder temp = new StringBuilder(4096);
                int i = GetPrivateProfileString(section, key, defValue, temp, temp.Capacity, this.iniPath);
                return temp.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistIniFile()
        {
            return File.Exists(this.iniPath);
        }

        /// <summary>
        /// 获取ini文件中所有的Sections
        /// </summary>
        public List<string> GetAllSections()
        {
            List<string> result = new List<string>();
            byte[] buf = new byte[65536];
            uint len = GetPrivateProfileStringArr(null, null, null, buf, buf.Length, this.iniPath);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        /// <summary>
        /// 获取section下的所有参数列表
        /// </summary>
        /// <param name="SectionName"></param>
        /// <param name="iniFilename"></param>
        /// <returns></returns>
        public List<string> GetSectionAllKeys(string section)
        {
            List<string> result = new List<string>();
            byte[] buf = new byte[65536];
            uint len = GetPrivateProfileStringArr(section, null, null, buf, buf.Length, this.iniPath);
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            }

            return result;
        }

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringArr(string section, string key,string def, byte[] retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    }
}
