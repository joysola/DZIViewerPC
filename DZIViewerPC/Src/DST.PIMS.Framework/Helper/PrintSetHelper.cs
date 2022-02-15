using DST.Common.Helper;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using Newtonsoft.Json;
using Nico.Common;
using System;

namespace DST.PIMS.Framework.Helper
{
    public class PrintSetHelper
    {
        /// <summary>
        /// 获取的打印配置信息
        /// </summary>
        /// <param name="sectionName">节名称</param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static PrintSetting GetPrintSetting(string sectionName)
        {
            PrintSetting result = null;
            try
            {
                var json = IniHelper.CreateInstance(ExtendAppContext.Current.ConfigurationIniPath).IniReadValue(sectionName, nameof(PrintSetting));
                if (!string.IsNullOrEmpty(json))
                {
                    //var json = Encrypt.Base64Decrypt(base64Str);
                    try
                    {
                        result = JsonConvert.DeserializeObject<PrintSetting>(json);
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug("反序列化获取打印配置失败！", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug($"获取ini配置文件{sectionName}信息失败！", ex);
            }
            return result;
        }
        /// <summary>
        /// 保存打印配置信息
        /// </summary>
        /// <param name="sectionName">节名称</param>
        /// <param name="printSetting">打印实体</param>
        public static void SavePrintsetting(string sectionName, PrintSetting printSetting)
        {
            var json = JsonConvert.SerializeObject(printSetting);
            //var base64Str = Encrypt.Base64Encrypt(result);
            IniHelper.CreateInstance(ExtendAppContext.Current.ConfigurationIniPath).IniWriteValue(sectionName, nameof(PrintSetting), json);
        }

        /// <summary>
        /// 获取TSC配置信息
        /// </summary>
        /// <param name="sectionName">节名称</param>
        /// <returns></returns>
        public static TSCPrintSetting GetTSCTPrintSetting(string sectionName)
        {
            var printSetting = GetPrintSetting(sectionName);
            return printSetting?.TSCPrint;
        }
        /// <summary>
        /// 获取HSJ配置信息
        /// </summary>
        /// <param name="sectionName">节名称</param>
        /// <returns></returns>
        public static HSJPrintSetting GetHSJPrintSetting(string sectionName)
        {
            var printSetting = GetPrintSetting(sectionName);
            return printSetting?.HSJPrint;
        }
        /// <summary>
        /// 获取TSCBarcode配置
        /// </summary>
        /// <param name="sectionName">节名称</param>
        /// <returns></returns>
        public static TSCBarCodeSetting GetTSCBarCodeSetting(string sectionName)
        {
            var printSetting = GetPrintSetting(sectionName);
            return printSetting?.TSCBarCode;
        }
    }
}
