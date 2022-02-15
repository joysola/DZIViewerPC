using DST.Common.AppStart;
using DST.Common.Helper.Factory;
using DST.Common.MinioHelper;
using DST.Controls;
using DST.Controls.Base;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace DST.PIMS.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 无参构造
        /// </summary>
        public App()
        {
            AppStart.Init();
        }

        /// <summary>
        /// 重写Startup事件响应
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 添加全局异常捕获
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DispatcherHelper.Initialize();
            this.Register();

            // 自动更新
            //this.StartAutoUpdateExe();

            // 注册webapi
            //RegisterWebApi();
        }

        /// <summary>
        /// 注册webapi访问
        /// </summary>
        private void RegisterWebApi()
        {
        }

        /// <summary>
        /// 启动exe程序
        /// </summary>
        private async void StartAutoUpdateExe()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                // 关闭可能存在的自动更新进程
                Process[] ps = Process.GetProcessesByName("AutoUpdate");
                foreach (Process p in ps)
                {
                    p.Kill();
                    p.WaitForExit(2000);
                }

                MinioHelper.Client.Connection(CommonConfiguration.MinIO_Endpoint, CommonConfiguration.MinIO_AccessKey, CommonConfiguration.MinIO_SecretKey);
                string curVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                List<Version> verList = new List<Version>();
                List<string> res = new List<string>();
                await MinioHelper.Client.GetFileNamesinBucket("version", null, false, res);
                res.ForEach(x =>
                {
                    string tmp = x.Replace("/", "");
                    verList.Add(new Version(tmp));
                });

                Version maxVersion = verList.Max();
                if (new Version(curVersion) < maxVersion)
                {
                    string autoUpdataExe = Environment.CurrentDirectory + @"\AutoUpdate.exe";
                    if (System.IO.File.Exists(autoUpdataExe))
                    {
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.FileName = autoUpdataExe;
                        info.Arguments = $"{CommonConfiguration.MinIO_Endpoint} {CommonConfiguration.MinIO_AccessKey} {CommonConfiguration.MinIO_SecretKey} {curVersion}";
                        info.WindowStyle = ProcessWindowStyle.Minimized;
                        Process.Start(info);
                        System.Threading.Thread.Sleep(500);
                        System.Environment.Exit(0);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("未找到自动更新工具，无法获取最新版本！");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("自动更新异常：" + e.Message);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        private void Register()
        {
            // 注册消息，弹出的对话框需要要主线程中弹出
            Messenger.Default.Register<ShowMessageBoxMessage>(this, message =>
            {
                if (message.IsAsyncShow)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var result = ConfirmMessageBox.Show(message.Text, message.SubMessage, message.Button, message.Icon, message.IsAutoClose, message.AutoCloseTime);
                        if (message.CallBack != null)
                        {
                            message.CallBack(result);
                        }
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        var result = ConfirmMessageBox.Show(message.Text, message.SubMessage, message.Button, message.Icon, message.IsAutoClose, message.AutoCloseTime);
                        if (message.CallBack != null)
                        {
                            message.CallBack(result);
                        }
                    }));
                }
            });

            ///注册各个页面类型
            Messenger.Default.Register<ShowContentWindowMessage>(this, message =>
            {
                Type type = ShowContentWindowMessageFactory.CreateContent(message.ContentName); // 由ContentWindow工厂生产对应类型
                if (type != null)
                {
                    message.Content = type;
                    var action = new ShowContentWindowAction(message);
                    action.CallInvoke();
                }
            });
        }

        /// <summary>
        /// UI线程抛出全局异常处理
        /// </summary>
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Logger.Error("UI线程全局异常", e.Exception);
                // 针对Api访问的请求处理
                //if (e.Exception.InnerException is HttpClientException)
                //{
                ConfirmMessageBox.Show("", e.Exception.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
                //}
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Logger.Error("不可恢复的UI线程全局异常", ex);
                ConfirmMessageBox.Show("应用程序发生不可恢复的异常，即将退出！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常处理
        /// </summary>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    ConfirmMessageBox.Show("", exception.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
                    Logger.Error("非UI线程全局异常", exception);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("不可恢复的非UI线程全局异常", ex);
                ConfirmMessageBox.Show("应用程序发生不可恢复的异常，即将退出！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
