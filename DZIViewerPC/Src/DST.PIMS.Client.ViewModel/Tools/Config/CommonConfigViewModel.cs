using DST.Common.Helper;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class CommonConfigViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// socket服务开关
        /// </summary>
        [Notification]
        public bool SocketSwitch { get; set; } = false;

        /// <summary>
        /// socket 服务监听的端口
        /// </summary>
        [Notification]
        public int SocketPort { get; set; } = 7523;

        /// <summary>
        /// Socket服务IP地址
        /// </summary>
        [Notification]
        public string SocketServerIP { get; set; }

        /// <summary>
        /// 默认打印机名称
        /// </summary>
        [Notification]
        public string DefaultPrinterName { get; set; }

        /// <summary>
        /// 端口测试命令
        /// </summary>
        public ICommand TestPortCommand { get; set; }

        public CommonConfigViewModel()
        {
            this.LoadConfigur();
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            // 保存配置
            Messenger.Default.Register<string>(this, EnumMessageKey.SaveConfiguration, data =>
            {
                this.SaveConfigur();
            });

            this.TestPortCommand = new RelayCommand<object>(data =>
            {
                bool hasUsed = false;
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();//IP端口

                foreach (IPEndPoint endPoint in ipEndPoints)
                {
                    if (endPoint.Port == this.SocketPort)
                    {
                        hasUsed = true;
                        break;
                    }
                }

                ipEndPoints = ipProperties.GetActiveUdpListeners();//UDP端口
                foreach (IPEndPoint endPoint in ipEndPoints)
                {
                    if (endPoint.Port == this.SocketPort)
                    {
                        hasUsed = true;
                        break;
                    }
                }

                if(hasUsed)
                {
                    this.ShowMessageBox("端口已被占用，请重新输入端口！");
                }
                else
                {
                    this.ShowMessageBox("端口可以使用！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            });
        }

        /// <summary>
        /// 加载本地数据
        /// </summary>
        private void LoadConfigur()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            string tmpSwitch = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.CommonConfig, "SocketSwitch");
            bool.TryParse(tmpSwitch, out bool tmpSocketSwitch);
            this.SocketSwitch = tmpSocketSwitch;

            string tmpPort = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.CommonConfig, "SocketPort");
            int port = 7523;
            int.TryParse(tmpPort, out port);
            this.SocketPort = port;

            this.SocketServerIP = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.CommonConfig, "SocketServerIP");
            this.DefaultPrinterName = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.CommonConfig, "DefaultPrinterName");
        }

        /// <summary>
        /// 保存本地数据
        /// </summary>
        public void SaveConfigur()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.CommonConfig, "SocketSwitch", this.SocketSwitch.ToString());
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.CommonConfig, "SocketPort", this.SocketPort.ToString());
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.CommonConfig, "SocketServerIP", this.SocketServerIP);
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.CommonConfig, "DefaultPrinterName", this.DefaultPrinterName);
        }
    }
}
