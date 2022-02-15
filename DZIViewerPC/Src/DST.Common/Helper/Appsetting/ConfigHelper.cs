using System;
using System.Configuration;
using System.Text;
using System.Xml;

namespace DST.Common.Helper
{
    public class ConfigHelper
    {
        /// <summary>
        /// 保存appsetting节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveAppseting(string key, string value)
        {
            if (ConfigurationManager.AppSettings[key] == value) // 相等不更新
            {
                return;
            }
            else
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove(key); // 先删除
                config.AppSettings.Settings.Add(key, value); // 新增此节点
                config.Save(ConfigurationSaveMode.Modified); // 保存
                ConfigurationManager.RefreshSection("appSettings"); // 刷新配置
            }
        }

        /// <summary>
        /// config文件的appsettings节点保存。使用Configuration 类型操作config，频繁操作会出现“集合对象已经修改”的错误
        /// </summary>
        public static void SaveConfig(string key, string value)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径  
            string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            // string  strFileName= AppDomain.CurrentDomain.BaseDirectory + "\\exe.config";  
            doc.Load(strFileName);
            //找出名称为“add”的所有元素  
            bool hasFind = false;
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性  
                XmlAttribute att = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素  
                if (att.Value == key)
                {
                    //对目标元素中的第二个属性赋值  
                    att = nodes[i].Attributes["value"];
                    att.Value = value;
                    hasFind = true;
                    break;
                }
            }

            if(!hasFind)
            {
                XmlElement newAdd = doc.CreateElement("add");//创建一个节点
                newAdd.SetAttribute("key", key);//设置该节点genre属性
                newAdd.SetAttribute("value", value);//设置该节点ISBN属性

                XmlNode app = doc.SelectSingleNode(@"configuration/appSettings");
                if (app == null)
                {
                    XmlNode root = doc.SelectSingleNode(@"configuration");
                    XmlElement appElement = doc.CreateElement("appSettings");
                    root.AppendChild(appElement);
                    appElement.AppendChild(newAdd);
                }
                else
                {
                    app.AppendChild(newAdd);
                }
            }

            //保存上面的修改  
            doc.Save(strFileName);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveAppseting(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key); // 先删除
            config.Save(ConfigurationSaveMode.Modified); // 保存
            ConfigurationManager.RefreshSection("appSettings"); // 刷新配置
        }
        /// <summary>
        /// 保存加密节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveEncryptAppsetting(string key, string value)
        {
            var encryptArray  = Encoding.UTF8.GetBytes(value);
            var encryptValue = Convert.ToBase64String(encryptArray);
            SaveAppseting(key, encryptValue);
        }
        /// <summary>
        /// 保存加密节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveEncryptAppsettingbyUnicode(string key, string value)
        {
            var encryptArray = UnicodeEncoding.Unicode.GetBytes(value);
            var encryptValue = Convert.ToBase64String(encryptArray);
            SaveAppseting(key, encryptValue);
        }
        /// <summary>
        /// 获取加密节点实际数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetEncryptAppsetting(string key)
        {
            var encryptvalue = ConfigurationManager.AppSettings[key];
            string value = null;
            if (!string.IsNullOrEmpty(encryptvalue))
            {
                var encryptArray = Convert.FromBase64String(encryptvalue);
                value = Encoding.UTF8.GetString(encryptArray);
            }
            return value;
        }
        /// <summary>
        /// 获取加密节点实际数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetEncryptAppsettingbyUnicode(string encryptvalue)
        {
            string value = null;
            if (!string.IsNullOrEmpty(encryptvalue))
            {
                var encryptArray = Convert.FromBase64String(encryptvalue);
                value = UnicodeEncoding.Unicode.GetString(encryptArray);
            }
            return value;
        }
    }
}
