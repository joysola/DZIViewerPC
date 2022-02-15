using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AutoUpdate
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += this.MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.IO.Directory.Exists(ExtendData.SystemVersionPath))
            {
                System.IO.Directory.CreateDirectory(ExtendData.SystemVersionPath);
            }
            
            bool res = MinioHelper.Client.Connection(ExtendData.MinIO_Endpoint, ExtendData.MinIO_AccessKey, ExtendData.MinIO_SecretKey);
            if (res)
            {
                Task.Run(() =>
                {
                    this.DelayDown();
                });
            }
            else
            {
                System.Windows.MessageBox.Show("连接Minio失败，无法下载最新版本！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(0);
            }
        }

        private async void DelayDown()
        {
            System.Threading.Thread.Sleep(500);
            await this.DownNewVersion();
        }

        private async Task DownNewVersion()
        {
            this.InsertLastListBox("获取最新版本！");
            Version curMainVersion = new Version(ExtendData.MainClientVersion);
            List<Version> verList = new List<Version>();
            List<string> res = new List<string>();
            await MinioHelper.Client.GetFileNamesinBucket("version", null, false, res);
            res.ForEach(x =>
            {
                string tmp = x.Replace("/", "");
                verList.Add(new Version(tmp));
            });

            Version maxVersion = verList.Max();
            if (curMainVersion < maxVersion)
            {
                this.InsertLastListBox("开始下载更新文件！");
                res.Clear();
                string prefix = maxVersion.ToString();
                await MinioHelper.Client.GetFileNamesinBucket("version", prefix, true, res);
                List<string> downFiles = new List<string>();
                // 下载文件
                res.ForEach(x =>
                {
                    try
                    {
                        string filePath = ExtendData.SystemVersionPath + x.Replace("/", "\\");
                        string path = System.IO.Path.GetDirectoryName(filePath);
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path);
                        }
                        bool isDown = MinioHelper.Client.DownloadFile("version", x, filePath).Result;
                        downFiles.Add(filePath);
                        this.InsertLastListBox("下载文件：" + filePath);
                    }
                    catch(Exception e)
                    {
                        Logger.Error($"下载文件:{x}失败：" + e.Message);
                    }
                });

                System.Threading.Thread.Sleep(500);
                this.InsertLastListBox("关闭工作站进程！");
                // 杀进程
                Process[] ps = Process.GetProcessesByName("DST.PIMS.Client");
                foreach (Process p in ps)
                {
                    p.Kill();
                    p.WaitForExit(2000);
                }

                // 复制文件
                this.InsertLastListBox("开始复制更新到对应路径！");
                downFiles.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x) && File.Exists(x))
                    {
                        // 如果包含子文件夹，则创建对应的目录结构
                        string tp = x.Replace(ExtendData.SystemVersionPath + maxVersion.ToString(), "");
                        string targetPath = Environment.CurrentDirectory + tp;
                        string targetDir = Path.GetDirectoryName(targetPath);
                        if(!Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }
                        try
                        {
                            System.IO.File.Copy(x, targetPath, true);
                        }
                        catch(Exception e)
                        {
                            this.InsertLastListBox("复制文件失败：" + x);
                            Logger.Error("复制文件失败：" + e.Message);
                        }

                        this.InsertLastListBox("复制文件：" + x);
                    }
                });

                this.InsertLastListBox("结束复制更新到对应路径！");
                this.InsertLastListBox("更新完成！");
                this.InsertLastListBox("启动工作站！");
                //this.proBar.Value = this.proBar.Maximum;

                string clientExe = Environment.CurrentDirectory + @"\DST.PIMS.Client.exe";
                if (File.Exists(clientExe))
                {
                    this.StartClientExe(clientExe);
                }
                else
                {
                    System.Windows.MessageBox.Show("未找到工作站客户端EXE！");
                }
                
                System.Environment.Exit(0);
            }
        }

        /// <summary>
        /// 插入信息
        /// </summary>
        /// <param name="msg"></param>
        private void InsertLastListBox(string msg)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.lbInfo.Items.Insert(this.lbInfo.Items.Count, msg);
                this.lbInfo.SelectedIndex = this.lbInfo.Items.Count - 1;
                this.lbInfo.ScrollIntoView(this.lbInfo.SelectedItem);
            });
        }

        /// <summary>
        /// 启动exe程序
        /// </summary>
        private void StartClientExe(string path)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = path;
            info.WindowStyle = ProcessWindowStyle.Minimized;
            Process.Start(info);
        }
    }
}
