using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoUpdate
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 接收参数数组
            string[] args = e.Args;
            if(args != null && args.Length == 4)
            {
                ExtendData.MinIO_Endpoint = args[0];
                ExtendData.MinIO_AccessKey = args[1];
                ExtendData.MinIO_SecretKey = args[2];
                ExtendData.MainClientVersion = args[3];
            }
        }
    }
}
