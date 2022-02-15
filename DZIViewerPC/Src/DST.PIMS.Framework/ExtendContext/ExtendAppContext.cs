using DST.Database;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.Model;
using DST.PIMS.Framework.Model.Enum;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.ComponentModel;
using DST.PIMS.Framework.Helper;
using DST.PIMS.Framework.Extensions;

namespace DST.PIMS.Framework.ExtendContext
{
    /// <summary>
    /// 全局共享数据类，包含所有的共享数据信息
    /// </summary>
    public class ExtendAppContext : ViewModelBase
    {
        private EnumWorkstationType curWorkstationType = EnumWorkstationType.PhysicalDistribution;


        public static ExtendAppContext Current { get; } = new ExtendAppContext();

        private ExtendAppContext()
        {
        }

        /// <summary>
        /// 当前操作的工作站
        /// </summary>
        public EnumWorkstationType CurWorkstationType
        {
            get { return this.curWorkstationType; }
            set
            {
                this.curWorkstationType = value;
                Messenger.Default.Send(value, EnumMessageKey.RefreshWorkstation);
            }
        }

        /// <summary>
        /// 登录信息
        /// </summary>
        [Notification]
        public LoginModel LoginModel { get; set; } = new LoginModel();

        /// <summary>
        /// 登录者的菜单信息
        /// </summary>
        public List<MenuInfoModel> LoginMenu { get; set; } = new List<MenuInfoModel>();

        /// <summary>
        /// 用户状态
        /// </summary>
        public EnumLoginType CurLoginType { get; set; } = EnumLoginType.NormalLogin;

        /// <summary>
        /// Ini配置文件路径
        /// </summary>
        public string ConfigurationIniPath { get; } = Environment.CurrentDirectory + "\\Configuration.ini";

        /// <summary>
        /// 所有权限工作站菜单(名称+枚举)
        /// </summary>
        public Dictionary<string, EnumWorkstationType> AllMenus { get; set; } = MenuButtonHelper.GetEnumNameValueDict<EnumWorkstationType>();
        /// <summary>
        /// 所有工作站按钮(名称+枚举)
        /// </summary>
        public List<ToolButton> AllButtons { get; set; } = MenuButtonHelper.GetAllToolButtons();

        /// <summary>
        /// 系统的临时目录
        /// </summary>
        public string SystemTempPath { get; } = Environment.CurrentDirectory + "\\Temp\\";

        /// <summary>
        /// 下载下来的版本数据
        /// </summary>
        public string SystemVersionPath { get; } = Environment.CurrentDirectory + "\\Temp\\Version\\";
    }
}