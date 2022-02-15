using Nico.Common;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace DST.Common.AppStart
{
    /// <summary>
    /// app 对象启动时初始化判断
    /// </summary>
    public class AppStart
    {
        private static Mutex mut = null;            // 线程信息，判断软件是否已经启动
        private static string restartFile = System.IO.Directory.GetCurrentDirectory() + "\\test.dat";
        private static string ExeName;
        private static string ExeFullPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

        /// <summary>
        /// 设置单利启动，管理员账户启用
        /// </summary>
        public static void Init()
        {
            ExeName = System.IO.Path.GetFileName(ExeFullPath).Replace(".exe", "");
            CheckIsRestart();
            CheckIsNeedRestart();
            SetSystemPression();
        }

        /// <summary>
        /// 判断当前运行是否为重启状态
        /// </summary>
        private static void CheckIsRestart()
        {
            try
            {
                // 判断当前启动是否为重启路径
                if (System.IO.File.Exists(restartFile))
                {
                    int index = 0;
                    Process[] pros = Process.GetProcessesByName(ExeName);
                    for(int i = 0; i < pros.Length; i++)
                    {
                        if(pros[i].Id != Process.GetCurrentProcess().Id)
                        {
                            pros[i].Kill();
                        }
                    }

                    while (index <= 30 && Process.GetProcessesByName(ExeName).Length > 1)
                    {
                        WindowsAPI.WindowsAPI.KillWindow(ExeName);
                        System.Threading.Thread.Sleep(1000);
                        index++;
                    }

                    System.IO.File.Delete(restartFile);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 判断当前系统是否已经启动
        /// </summary>
        private static void CheckIsNeedRestart()
        {
            bool requestInitialOwnerShip = true;
            bool mutexWasCreated = false;
            mut = new Mutex(requestInitialOwnerShip, ExeName, out mutexWasCreated);
            if (!(requestInitialOwnerShip && mutexWasCreated))
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("系统正在运行，是否关闭当前系统重新启动？", "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes || result == System.Windows.Forms.DialogResult.OK)
                {
                    // 杀掉当前正在运行的窗口
                    WindowsAPI.WindowsAPI.KillWindow(ExeName);
                    try
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(restartFile, System.IO.FileMode.Create);
                        fs.Close();
                        System.Threading.Thread.Sleep(1000);
                    }
                    catch
                    { }

                    System.Windows.Forms.Application.Restart();
                    System.Environment.Exit(0);
                }
                else
                {
                    ActivateWindow();
                    System.Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// 激活原先窗口
        /// </summary>
        private static void ActivateWindow()
        {
            Process[] prs = Process.GetProcessesByName(ExeName);
            for (int i = 0; i < prs.Length; i++)
            {
                if (prs[i].MainWindowHandle != IntPtr.Zero)
                {
                    WindowsAPI.WindowsAPI.SwitchToThisWindow(prs[i].MainWindowHandle, true);
                    break;
                }
            }
        }

        /// <summary>
        /// 当前用户是管理员的时候，直接启动应用程序.
        /// 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
        /// </summary>
        private static void SetSystemPression()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);

                //判断当前登录用户是否为管理员
                if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    //创建启动对象
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    startInfo.FileName = ExeFullPath;
                    //设置启动动作,确保以管理员身份运行
                    startInfo.Verb = "runas";
                    try
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(restartFile, System.IO.FileMode.Create);
                        fs.Close();
                        System.Diagnostics.Process.Start(startInfo);
                    }
                    catch(Exception exx)
                    {
                        System.Windows.Forms.MessageBox.Show("管理器启动失败：" + exx.Message);
                    }

                    //退出
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("设置管理员权限启动失败！" + ex.Message);
            }
        }
    }
}
