using Microsoft.Win32;
using Newtonsoft.Json;
using Nico.TransDat.Model;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Nico.TransDat.Base
{
    /// <summary>
    /// Dat模块的基础类型
    /// </summary>
    internal abstract class BaseDatModule
    {
        /// <summary>
        /// Dat 图片信息：ImageDat的存储长度，可在子类里修改大小
        /// </summary>
        public int HeadLength = 1024000 * 200; // 200MB

        /// <summary>
        /// Dat 图片索引：ImageDatIndex 的存储长度，可在子类里修改大小
        /// </summary>
        public int HeadIndexLength = 1024000 * 3;

        /// <summary>
        /// Dat 文件的版本信息的长度，禁止修改长度, 1MB，用于存储版本、总宽、总高、设备等信息
        /// </summary>
        public const int VersionIndexLength = 1024000;

        /// <summary>
        /// 程序集超时标记
        /// </summary>
        private static bool IsTimeOut = TimeOut();

        /// <summary>
        /// 加密KEY
        /// </summary>
        private const string EncryptKey = "Nico";

        /// <summary>
        /// 保护构造，判断程序集是否超时
        /// </summary>
        protected BaseDatModule()
        {
            if(IsTimeOut)
            {
                throw null;
            }
        }

        /// <summary>
        /// 获取Dat文件的版本信息
        /// </summary>
        /// <param name="datPath">Dat文件的</param>
        /// <returns></returns>
        public static DatVersionInfo GetDatVersion(string datPath)
        {
            if (IsTimeOut)
            {
                throw null;
            }

            DatVersionInfo result = null;
            try
            {
                using (FileStream fs = new FileStream(datPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] verByte = new byte[VersionIndexLength];
                    fs.Read(verByte, 0, verByte.Length);
                    string datVersion = TransHelper.Base64StringByteToString(verByte);
                    result = JsonConvert.DeserializeObject<DatVersionInfo>(datVersion);
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 超时计算
        /// </summary>
        /// <returns></returns>
        private static bool TimeOut() 
        {
            bool result = false;
            const string RootKey = @"SOFTWARE\Nico";
            const string SubKey = "TimeOut";
            const string KeyName = "Cai";

            RegistryKey nicoReg = Registry.LocalMachine.OpenSubKey(RootKey, true);
            if(nicoReg == null)
            {
                nicoReg = Registry.LocalMachine.CreateSubKey(RootKey, true);
                //创建子项
                var nameKey = nicoReg.CreateSubKey(SubKey);
                //创建键值
                nameKey.SetValue(KeyName, Encrypt(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            else
            {
                var timeoutKey = nicoReg.OpenSubKey(SubKey, true);
                if(DateTime.TryParse(Decrypt(timeoutKey.GetValue(KeyName).ToString()), out DateTime caiValue))
                {
                    result = (DateTime.Now - caiValue).TotalDays >= 365;
                }
            }

            return result;
        }

        /// <summary> 
        /// 加密字符串  
        /// </summary> 
        /// <param name="str">要加密的字符串</param> 
        /// <returns>加密后的字符串</returns> 
        private static string Encrypt(string str)
        {
            // 实例化加/解密类对象  
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   

            // 定义字节数组，用来存储密钥   
            byte[] key = Encoding.Unicode.GetBytes(EncryptKey);

            // 定义字节数组，用来存储要加密的字符串 
            byte[] data = Encoding.Unicode.GetBytes(str);

            // 实例化内存流对象  
            using (MemoryStream ms = new MemoryStream())  
            {
                //使用内存流实例化加密流对象  
                using (CryptoStream cs = new CryptoStream(ms, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write))
                {
                    //向加密流中写入数据 
                    cs.Write(data, 0, data.Length);
                    //释放加密流     
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());//返回加密后的字符串 
                }
            }
        }

        /// <summary> 
        /// 解密字符串  
        /// </summary> 
        /// <param name="str">要解密的字符串</param> 
        /// <returns>解密后的字符串</returns>   
        private static string Decrypt(string str)
        {
            // 实例化加/解密类对象   
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

            //定义字节数组，用来存储密钥   
            byte[] key = Encoding.Unicode.GetBytes(EncryptKey);

            //定义字节数组，用来存储要解密的字符串 
            byte[] data = Convert.FromBase64String(str);

            //实例化内存流对象  
            using (MemoryStream ms = new MemoryStream())    
            {
                //使用内存流实例化解密流对象      
                using (CryptoStream cs = new CryptoStream(ms, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write))
                {
                    //向解密流中写入数据    
                    cs.Write(data, 0, data.Length);
                    //释放解密流     
                    cs.FlushFinalBlock();               
                    return Encoding.Unicode.GetString(ms.ToArray());       //返回解密后的字符串 
                }
            }
        }
    }
}
