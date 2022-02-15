using DST.Common.Helper;
using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using DST.PIMS.Framework.Model.Enum;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Nico.Common;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// TitleBar.xaml 的交互逻辑
    /// </summary>
    public partial class TitleBar : BaseUserControl
    {
        /// <summary>
        /// 标题栏
        /// </summary>
        public TitleBar()
        {
            InitializeComponent();
            this.RegisterCommand();
            this.DataContext = new TitleBarViewModel();
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        private void RegisterCommand()
        {
            Messenger.Default.Register<object>(this, EnumMessageKey.RefreshLogin, msg =>
            {

            });

            Messenger.Default.Register<EnumWorkstationType>(this, EnumMessageKey.RefreshWorkstation, msg =>
            {
                this.tbWorkstation.Text = $"({EnumHelper.GetEnumDesc(msg)})";
#if DEBUG
                this.tbWorkstation.Text += "----测试版";
#endif
            });
        }

        /// <summary>
        /// 弹框显示
        /// </summary>
        private void BtnLoginChange_Click(object sender, RoutedEventArgs e)
        {
            this.popupChange.IsOpen = true;
        }

        /// <summary>
        /// 列表选择
        /// </summary>
        private void LbChangeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = this.lbChangeType.SelectedItem as ListBoxItem;
            if (lbi == null || lbi.Tag == null)
            {
                return;
            }

            switch (lbi.Tag.ToString())
            {
                case "锁屏":
                    break;
                case "注销":
                    Logout();
                    break;
                case "退出":
                    Logger.Info("退出系统！");
                    Application.Current.MainWindow.Close();
                    break;
                case "关于":
                    // 关于界面
                    ShowAbout();
                    break;
                case "修改密码":
                    ModifyPassword();
                    break;

                case "配置":
                    this.ShowCustomConfiguration();
                    break;
                default:
                    return;
            }

            this.popupChange.IsOpen = false;
            this.lbChangeType.SelectedIndex = -1;
        }

        private void ShowCustomConfiguration()
        {
            DispatcherHelper.UIDispatcher.BeginInvoke((Action)delegate ()
            {
                ShowContentWindowMessage msg = new ShowContentWindowMessage("CustomConfiguration", "配置");
                msg.DesignHeight = 450;
                msg.DesignWidth = 650;
                Messenger.Default.Send(msg);

            }, System.Windows.Threading.DispatcherPriority.Send, null);
        }
        /// <summary>
        /// 登出
        /// </summary>
        private void Logout()
        {
            if (ExtendAppContext.Current.CurLoginType == EnumLoginType.NormalLogin)
            {
                Application.Current.MainWindow?.Hide();
                Login login = new Login();
                login.Show();
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        private void ModifyPassword()
        {
            var msg = new ShowContentWindowMessage("ModifyPassword", "修改密码");
            msg.DesignHeight = 250;
            msg.DesignWidth = 400;
            msg.CallBackCommand = new RelayCommand<bool>(res =>
            {
                if (res) // 修改密码成功则注销当前用户
                {
                    Logout();
                }
            });
            Messenger.Default.Send(msg);
        }
        /// <summary>
        /// 关于
        /// </summary>
        private void ShowAbout()
        {
            var msg = new ShowContentWindowMessage(nameof(About), "关于");
            msg.DesignHeight = 320;
            msg.DesignWidth = 400;
            Messenger.Default.Send(msg);
        }
    }
}
