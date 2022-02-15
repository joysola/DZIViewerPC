using DST.Common.Extensions;
using DST.PIMS.Client.ViewModel;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DST.PIMS.Client
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private LoginViewModel loginViewModel;
        public Login()
        {
            InitializeComponent();
            this.RegisterMessenger();
            this.InitVerify();

            loginViewModel = new LoginViewModel();
            this.DataContext = loginViewModel;


            this.Loaded += (s, e) => this.Init();

            this.Unloaded += (s, e) => Messenger.Default.Unregister(this);
        }
        private void Init()
        {
            var hcs = this.GetAllVerifyHCCtls().ToList(); // 获取所有需要验证的HC控件
                                                          //hcs.ForEach(hc => hc.VerifyData()); // 初次验证
            loginViewModel.HCVeryControls = hcs;

            this.Dispatcher.InvokeAsync(() =>
            {
                if (string.IsNullOrEmpty(this.tb.Text))
                {
                    this.tb.Focus();
                }
                else
                {
                    //if (loginViewModel.IsRememberPw) // 需要记住密码，则填充密码
                    //{
                    // this.pw.Password = loginViewModel.LoginModel.Password;
                    //}
                    this.pw.ActualPasswordBox.Focus(); // 一定要让hc的passwordbox 的 ActualPasswordBox选中
                }

            });

            //this.Topmost = true;
            this.ShowActivated = true;
            this.Activate();
        }

        /// <summary>
        /// 验证初始化
        /// </summary>
        private void InitVerify()
        {
            this.tb.VerifyFunc = data =>
            {
                if (string.IsNullOrEmpty(data))
                {
                    return OperationResult.Failed("请输入用户名!");
                }
                return OperationResult.Success();
            };

            this.pw.VerifyFunc = data =>
            {
                if (string.IsNullOrEmpty(data))
                {
                    return OperationResult.Failed("请输入密码!");
                }
                return OperationResult.Success();
            };
        }

        private void RegisterMessenger()
        {
            Messenger.Default.Register<object>(this, EnumMessageKey.CloseLogin, msg =>
            {
                if (Application.Current.MainWindow.GetType() != typeof(MainWindow))
                {
                    var main = new MainWindow();
                    main.Show();
                    main.ShowActivated = true;
                    Application.Current.MainWindow = main;
                }
                else
                {
                    // 注销后再次登录，需要刷新菜单栏
                    Messenger.Default.Send<object>(null, EnumMessageKey.RefreshMenus);
                    Application.Current.MainWindow.Show();
                }
                this.Close();
            });
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (Application.Current.MainWindow is MainWindow mainWindow) // 主窗体时mainwindow时，需要关闭它
            {
                mainWindow.Close();
            }
        }
        /// <summary>
        /// 失去焦点后关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pop_LostFocus(object sender, RoutedEventArgs e)
        {
            this.pop.IsOpen = false;
        }
    }
}
