using DST.ApiClient.Service;
using DST.Common.Extensions;
using DST.Common.Helper;
using DST.Database.Model;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using MVVMExtension;
using Newtonsoft.Json;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class LoginViewModel : CustomBaseViewModel
    {
        private readonly string loginAccount = "loginAccount";
        private readonly string loginPw = "loginPw";
        private readonly string loginHistory = "loginHistory";
        /// <summary>
        /// 登录实体
        /// </summary>
        [Notification]
        public QueryLoginModel LoginModel { get; set; } = new QueryLoginModel();

        /// <summary>
        /// 是否记住密码
        /// </summary>
        [Notification]
        public bool IsRememberPw { get; set; }
        /// <summary>
        /// 是否正在加载
        /// </summary>
        [Notification]
        public bool IsLoading { get; set; }
        /// <summary>
        /// 历史账号
        /// </summary>
        [Notification]
        public ObservableCollection<QueryLoginModel> HistoryLoginInfos { get; set; } = new ObservableCollection<QueryLoginModel>();
        [Notification]
        public bool IsHisOpen { get; set; }
        /// <summary>
        /// 前台验证控件
        /// </summary>
        public List<IDataInput> HCVeryControls { get; set; }
        /// <summary>
        /// 登录
        /// </summary>
        public ICommand LoginCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            try
            {
                if (CheckData())
                {
                    this.IsHisOpen = false; // 切焦点关闭历史用户
                    this.IsLoading = true;
                    if (ConfigurationManager.AppSettings["AppVersion"] == nameof(EnumWorkstationType.ImageViewer)) // 判断工作站类型
                    {
                        ExtendAppContext.Current.LoginModel = new Database.LoginModel { RealName = "管理员", User_Name = "超级管理员" };
                    }
                    else
                    {
                        ExtendAppContext.Current.LoginModel = await Task.Run(() => LoginService.Instance.Login(LoginModel)).ConfigureAwait(false);
                        if (ExtendAppContext.Current.LoginModel == null) // 登录失败
                        {
                            return;
                        }
                        var dictTask = InitDictAsync();
                        // 加载菜单
                        var menuTask = Task.Run(() => ExtendAppContext.Current.LoginMenu = SysManageService.Instance.GetCSLoginUserMenus());
                        await Task.WhenAll(dictTask, menuTask).ConfigureAwait(false);
                    }
                    Task.Run(() =>
                    {
                        SaveLoginInfo(LoginModel);

                    }).NoWarning();
                    // 加载字典
                    // 更新MainWindow，刷新菜单
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Messenger.Default.Send<object>(this, EnumMessageKey.CloseLogin);
                    });
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                //WhirlingControlManager.CloseWaitingForm();
                this.IsLoading = false;
            }

        })).Value;
        /// <summary>
        /// 选中历史账号登录
        /// </summary>
        public ICommand SelectHisLogin => new Lazy<RelayCommand<QueryLoginModel>>(() => new RelayCommand<QueryLoginModel>(data =>
        {
            if (data != null)
            {
                IsRememberPw = !string.IsNullOrEmpty(data.Password);
                LoginModel = data.DeepCopy();
                IsHisOpen = false;
            }
        })).Value;
        /// <summary>
        /// 历史记录是否显示
        /// </summary>
        public ICommand UserNameFocus => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            IsHisOpen = true;
        })).Value;
        /// <summary>
        /// 删除历史用户
        /// </summary>
        public ICommand RemoveHisLogin => new Lazy<RelayCommand<QueryLoginModel>>(() => new RelayCommand<QueryLoginModel>(data =>
        {
            ShowMessageBox("确认要删除次用户登录信息吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    HistoryLoginInfos.Remove(data);
                    if (data.UserName == LoginModel?.UserName)
                    {
                        LoginModel = null;
                    }
                }
            });
        })).Value;

        public LoginViewModel()
        {
            ApiClientSetting.SetHttpClientEx();
            this.InitAccount();
        }
        /// <summary>
        /// 初始化账户
        /// </summary>
        private void InitAccount()
        {
            var savedLoginName = ConfigurationManager.AppSettings[loginAccount];
            var savedPw = ConfigurationManager.AppSettings[loginPw];
            var savedHistory = ConfigurationManager.AppSettings[loginHistory];
            if (!string.IsNullOrEmpty(savedLoginName))
            {
                LoginModel.UserName = savedLoginName;
            }
            if (!string.IsNullOrEmpty(savedPw))
            {
                IsRememberPw = true;
                LoginModel.Password = ConfigHelper.GetEncryptAppsetting(loginPw);
            }
            if (!string.IsNullOrEmpty(savedHistory))
            {
                var historyLoginStr = ConfigHelper.GetEncryptAppsettingbyUnicode(savedHistory);
                HistoryLoginInfos = JsonConvert.DeserializeObject<ObservableCollection<QueryLoginModel>>(historyLoginStr);
            }
        }

        protected override bool CheckData()
        {
            // 验证全部hc需要验证的控件
            return HCVeryControls.VerifyHCCtlsData();
        }

        /// <summary>
        /// 登录按键回车键触发切换焦点
        /// </summary>
        public void Login_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // 如果焦点是 真实的 PasswordBox，回车 则进行登录验证
                if (Keyboard.FocusedElement is System.Windows.Controls.PasswordBox password)
                {
                    this.LoginCommand.Execute(null);
                }
                else
                {
                }
                this.IsHisOpen = false; // 切焦点关闭历史用户
                SwitchMoveFocus(e.OriginalSource);
            }
        }
        /// <summary>
        /// 移动到下一焦点
        /// </summary>
        /// <param name="element"></param>
        private void SwitchMoveFocus(object element)
        {
            if (element is UIElement uIElement)
            {
                uIElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
        /// <summary>
        /// 异步获取字典
        /// </summary>
        /// <returns></returns>
        private async Task InitDictAsync()
        {
            try
            {
                var t1 = DictService.Instance.GetBiopsyFlagDict();
                var t2 = DictService.Instance.GetHPVResultDict();
                var t3 = DictService.Instance.GetGlandularEpithelialCellResultDict();
                var t4 = DictService.Instance.GetSampleTctResultDict();
                var t5 = DictService.Instance.GetSampleSignStatusDict();
                var t6 = DictService.Instance.GetSignResultDict();
                var t7 = DictService.Instance.GetSexDict();
                var t8 = DictService.Instance.GetActivityTypeList();
                var t9 = DictService.Instance.GetCSProductDict();
                var t10 = DictService.Instance.GetPostscriptDict();
                var t11 = DictService.Instance.GetEmbedPrintStatusDict();
                var t12 = DictService.Instance.GetSamplTissDelayDict();
                var t13 = DictService.Instance.GetRegisterSampleStatusDict();
                var t14 = DictService.Instance.GetDrawMaterStatusDict();
                var t15 = DictService.Instance.GetAdviceStatusDict();
                var t16 = DictService.Instance.GetTechAdviceDict();
                var t17 = DictService.Instance.GetDoctAdviceDict();
                //var t18 = DictService.Instance.GetAdviceDict();
                var t19 = DictService.Instance.GetProdReagentDict();
                var t20 = DictService.Instance.GetGastroscopyTissueDict();
                var t21 = DictService.Instance.GetReSampStatusDict();
                var t22 = DictService.Instance.GetReSampReasonDict();
                var t23 = DictService.Instance.GetReSampWithdrawReasonDict();
                var t24 = DictService.Instance.GetReceiveStatus();
                var t25 = DictService.Instance.GetScanStatus();
                var t26 = DictService.Instance.GetHsjPrintType();
                await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, /*t18,*/ t19, t20, t21, t22, t23, t24, t25, t26).ConfigureAwait(false);
                ExtendApiDict.Instance.BiopsyFlagDict = t1.Result;
                ExtendApiDict.Instance.HPVResultDict = t2.Result;
                ExtendApiDict.Instance.GlandularEpithelialCellResultDict = t3.Result;
                ExtendApiDict.Instance.SampleTctResultDict = t4.Result;
                ExtendApiDict.Instance.SampleSignStatusDict = t5.Result;
                ExtendApiDict.Instance.SignResultDict = t6.Result;
                ExtendApiDict.Instance.SexDict = t7.Result;
                ExtendApiDict.Instance.ActivityTypeDict = t8.Result;
                ExtendApiDict.Instance.ProductDict = t9.Result;
                ExtendApiDict.Instance.PostscriptDict = t10.Result;
                ExtendApiDict.Instance.EmbedPrintStatusDict = t11.Result;
                ExtendApiDict.Instance.SamplTissDelayDict = t12.Result;
                ExtendApiDict.Instance.RegisterSampleStatusDict = t13.Result;
                ExtendApiDict.Instance.DrawMaterStatusDict = t14.Result;
                ExtendApiDict.Instance.AdviceStatusDict = t15.Result;
                ExtendApiDict.Instance.TechAdviceDict = t16.Result;
                ExtendApiDict.Instance.DoctAdviceDict = t17.Result;
                //ExtendApiDict.Instance.AdviceDict = t18.Result;
                ExtendApiDict.Instance.ProdReagentDict = t19.Result;
                ExtendApiDict.Instance.GastroscopyTissueDict = t20.Result;
                ExtendApiDict.Instance.ReSampStatusDict = t21.Result;
                ExtendApiDict.Instance.ReSampReasonDict = t22.Result;
                ExtendApiDict.Instance.ReSampWithdrawReasonDict = t23.Result;
                ExtendApiDict.Instance.ReceiveStatus = t24.Result;
                ExtendApiDict.Instance.ScanStatus = t25.Result;
                ExtendApiDict.Instance.HsjPrintType = t26.Result;
            }
            catch (Exception ex)
            {
                Logger.Error("初始化Api字典数据失败！", ex);
                throw;
            }
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="loginModel"></param>
        private void SaveLoginInfo(QueryLoginModel loginModel)
        {
            ConfigHelper.SaveAppseting(loginAccount, LoginModel.UserName); // 更新账户
            QueryLoginModel existedLogin = null;
            if (HistoryLoginInfos?.Count >= 0)
            {
                existedLogin = HistoryLoginInfos.FirstOrDefault(x => x.UserName == loginModel.UserName);
                if (existedLogin != null)
                {
                    existedLogin.Password = loginModel.Password;
                }
                else
                {
                    existedLogin = loginModel;
                    Application.Current.Dispatcher.Invoke(() => HistoryLoginInfos.Add(loginModel));
                }
            }
            // 是否记忆密码
            if (IsRememberPw)
            {
                ConfigHelper.SaveEncryptAppsetting(loginPw, LoginModel.Password);
            }
            else
            {
                ConfigHelper.RemoveAppseting(loginPw);
                existedLogin.Password = null;
            }
            var histroyStr = JsonConvert.SerializeObject(HistoryLoginInfos);
            ConfigHelper.SaveEncryptAppsettingbyUnicode(loginHistory, histroyStr);
        }
    }
}
