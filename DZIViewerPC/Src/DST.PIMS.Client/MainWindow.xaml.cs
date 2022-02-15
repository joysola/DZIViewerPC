using DST.Common.Barcode;
using DST.Common.Helper;
using DST.Controls;
using DST.Controls.Base;
using DST.PIMS.Client.Views;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
using Nico.Common;
using System;
using System.Windows;

namespace DST.PIMS.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private BaseUserControl curWorkstationView = null;

        public MainWindow()
        {
            InitializeComponent();
            this.InitResize();
            this.RegisterCommand();
            this.InitBarcode();
            this.InitSocketServer();
            this.Loaded += this.MainWindow_Loaded;
            this.Closed += this.MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            BarcodeHook.Instance.Stop();
            SocketServer.Instance.StopListen();
        }

        /// <summary>
        /// 初始化界面，全屏，保留显示任务栏
        /// </summary>
        private void InitResize()
        {
            this.ResizeMode = ResizeMode.CanMinimize;
            this.WindowStyle = WindowStyle.None;
            base.Left = 0.0;
            base.Top = 0.0;
            base.Height = SystemParameters.WorkArea.Height;
            base.Width = SystemParameters.WorkArea.Width;
            this.mainGrid.Width = base.Width;
            this.mainGrid.Height = base.Height;
        }

        /// <summary>
        /// 初始化socket server
        /// </summary>
        private void InitSocketServer()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            string tmpSwitch = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.CommonConfig, "SocketSwitch");
            bool.TryParse(tmpSwitch, out bool tmpSocketSwitch);
            if(tmpSocketSwitch)
            {
                if(!SocketServer.Instance.StartListen())
                {
                    ConfirmMessageBox.Show("", "SocketServer启动监听失败，请查看日志信息！", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    SocketServer.Instance.ReceiveMessage += this.SocketServer_ReceiveMessage;
                }
            }
        }

        /// <summary>
        /// socket服务接受到信息
        /// </summary>
        /// <param name="message">key:value</param>
        private void SocketServer_ReceiveMessage(string message)
        {
            Logger.Info("Socket接收消息：" + message);
            // $scandone,20210529\410482A02210526104_181152
            string[] msg = message.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if(msg == null || string.IsNullOrEmpty(msg[0]))
            {
                return;
            }

            switch(msg[0])
            {
                case "$scandone": // 扫描仪扫描完成后，通知自动上传
                    if (msg.Length >= 2 && msg[1] != null)
                    {
                        string tarPath = msg[1];
                        if(tarPath.EndsWith("\0"))
                        {
                            tarPath = tarPath.Replace('\0', ' ').Trim();
                        }
                        UploadManager.Instance.UploadSample(tarPath);
                    }
                    break;
            }
        }

        /// <summary>
        /// 消息注册
        /// </summary>
        private void RegisterCommand()
        {
            Messenger.Default.Register<EnumWorkstationType>(this, EnumMessageKey.RefreshWorkstation, msg =>
            {
                this.RefreshWorkstation();
            });
        }

        /// <summary>
        /// 注册条码枪信息
        /// </summary>
        private void InitBarcode()
        {
            BarcodeHook.Instance.ReceiveBarcode += this.ReceiveBarcode;
            BarcodeHook.Instance.Start();
        }

        /// <summary>
        /// 接收条码
        /// </summary>
        /// <param name="barcode">条码号，获取到的条码号可能包含 "\", "\r"等特殊字符，需要处理</param>
        private void ReceiveBarcode(string barcode)
        {
            Logger.Info("扫码枪数据接收：" + barcode);
            string ss = barcode.Replace("\r", "").Replace("\0", "");
            ss = ss.ToUpper();
            Messenger.Default.Send(ss, EnumMessageKey.ScanMaterialConfirm); // 取材确认
            Messenger.Default.Send(ss, EnumMessageKey.ScanEmbedConfirm); // 包埋确认
            Messenger.Default.Send(ss, EnumMessageKey.ScanProdConfirm); // 制片确认

            // Messenger.Default.Send(ss, EnumMessageKey.ScanMainReceive);
        }

        /// <summary>
        /// 加载工作站
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(!System.IO.Directory.Exists(ExtendAppContext.Current.SystemTempPath))
            {
                System.IO.Directory.CreateDirectory(ExtendAppContext.Current.SystemTempPath);
            }
            if(!System.IO.Directory.Exists(ExtendAppContext.Current.SystemVersionPath))
            {
                System.IO.Directory.CreateDirectory(ExtendAppContext.Current.SystemVersionPath);
            }
            this.RefreshWorkstation();
        }

        /// <summary>
        /// 刷新工作站
        /// </summary>
        private void RefreshWorkstation()
        {
            BaseUserControl newWorkstation = null;
            switch (ExtendAppContext.Current.CurWorkstationType)
            {
                case EnumWorkstationType.PhysicalDistribution:
                    newWorkstation = new PhysDistMain();
                    break;

                case EnumWorkstationType.Register:
                    newWorkstation = new RegisterMain();
                    break;

                case EnumWorkstationType.Allocation:
                    newWorkstation = new AllocationMain();
                    break;

                case EnumWorkstationType.ArchivesManagement:
                    newWorkstation = new ArchivesManagementMain();
                    break;

                case EnumWorkstationType.Attendance:
                    newWorkstation = new AttendanceMain();
                    break;

                case EnumWorkstationType.ConventionalDiagnosis:
                    newWorkstation = new ConventionalDiagnosisMain();
                    break;

                case EnumWorkstationType.CytoDiagnosis:
                    newWorkstation = new CytoDiagnosisMain();
                    break;

                case EnumWorkstationType.Embedding:
                    newWorkstation = new EmbeddingMain();
                    break;

                case EnumWorkstationType.Material:
                    newWorkstation = new MaterialMain();
                    break;

                case EnumWorkstationType.MolecularDiagnosis:
                    newWorkstation = new MolecularDiagnosisMain();
                    break;

                case EnumWorkstationType.Production:
                    newWorkstation = new ProductionMain();
                    break;

                case EnumWorkstationType.ReportSystem:
                    newWorkstation = new ReportSystemMain();
                    break;

                case EnumWorkstationType.Scan:
                    newWorkstation = new ScanMain();
                    break;

                case EnumWorkstationType.Telemedicine:
                    newWorkstation = new TelemedicineMain();
                    break;

                case EnumWorkstationType.SampleDataUpload:
                    newWorkstation = new UploadMain();
                    break;
                case EnumWorkstationType.SystemManage:
                    newWorkstation = new SysManage();
                    break;
                case EnumWorkstationType.ImageViewer:
                    newWorkstation = new ImageViewerPC(); // 阅片
                    break;
                default:
                    break;
            }

            if(this.curWorkstationView != null)
            {
                this.gridWorkstation.Children.Remove(this.curWorkstationView);
                this.curWorkstationView.Dispose();
                this.curWorkstationView = null;
            }

            this.curWorkstationView = newWorkstation;
            if (this.curWorkstationView != null)
            {
                this.gridWorkstation.Children.Add(this.curWorkstationView);
            }
        }
    }
}
