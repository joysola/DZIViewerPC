using DST.ApiClient.Service;
using DST.Common.TscBarcodePrint;
using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework;
using DST.PIMS.Framework.Helper;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using Newtonsoft.Json;
using Nico.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace DST.PIMS.Client.ViewModel
{
    public class MoleDiagBarcodeViewModel : CustomBaseViewModel
    {
        private string _samplecode;
        private static readonly SemaphoreSlim _locker2 = new SemaphoreSlim(1, 1);
        /// <summary>
        /// 样本编号
        /// </summary>
        [Notification]
        public string Samplecode
        {
            get => _samplecode;
            set
            {
                _samplecode = value;
                if (string.IsNullOrEmpty(_samplecode))
                {
                    Barcode = null;
                    SampleTSCtxt = null;
                }
            }
        }
        /// <summary>
        /// 实验室编号（条码号）
        /// </summary>
        [Notification]
        public string Barcode { get; set; }

        /// <summary>
        /// 患者信息
        /// </summary>
        [Notification]
        public string SampleTSCtxt { get; set; }
        /// <summary>
        /// 打印模式（1 单打，2 连打，3 扫描）
        /// </summary>
        [Notification]
        public int? PrintModel { get; set; } = 1;

        /// <summary>
        /// 成功打印的次数
        /// </summary>
        [Notification]
        public int Count2 { get; set; } = 1;
        private TSCBarCodeSetting Setting { get; set; }
        /// <summary>
        /// 打印条码
        /// </summary>
        public ICommand PrintCommand { get; set; }
        /// <summary>
        /// 根据样本编码，显示实验室编码
        /// </summary>
        public ICommand CodeChangedCommand { get; set; }
        /// <summary>
        /// TSC的dll版本
        /// </summary>
        public ICommand TSCVerCommand { get; set; }

        public MoleDiagBarcodeViewModel()
        {
            Setting = PrintSetHelper.GetTSCBarCodeSetting(IniSectionConst.MolecularDiagnosis);
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        protected override void RegisterCommand()
        {
            // 打印条码
            this.PrintCommand = new RelayCommand(() =>
            {
                if (!string.IsNullOrEmpty(Barcode))
                {
                    TSCBarCodeManager.Singleton.Print(Barcode, Setting);
                    Count2++;
                }
            });
            // TSC的dll版本
            this.TSCVerCommand = new RelayCommand(() =>
            {
                TSCLibApi.About();
            });
            // 根据样本编码，显示实验室编码
            this.CodeChangedCommand = new RelayCommand<string>(code =>
            {
                //Barcode = null; // 文本变化，先清空显示的Barcode
                //if (!string.IsNullOrEmpty(code))
                //{
                //    this.SearchSampleTSC(code); // 搜索Sample实例
                //}
            });
        }
        /// <summary>
        /// 搜索sample实例
        /// </summary>
        /// <param name="samplecode"></param>
        private void SearchSampleTSC(string samplecode)
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                var sample = MolecularDiagnosisService.Instance.GetSamplePcrByLaboratory(samplecode);
                if (sample != null && !string.IsNullOrEmpty(sample.laboratoryCode))
                {
                    this.ShowSampeInfo(sample); // 显示样本信息
                    Barcode = sample.laboratoryCode; // 获取实验室编号
                    Task.Run(() =>
                    {
                        Logger.Info($"样本信息:{JsonConvert.SerializeObject(sample)}");
                    });
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
        /// <summary>
        /// 样本编码回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SampleCode_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    await _locker2.WaitAsync(); // 加锁，防止端口被占用
                    if (sender is TextBox tbx)
                    {
                        tbx.SelectAll(); // 文本全选
                                         // Barcode = null; // 文本变化，先清空显示的Barcode
                                         // SampleTSCtxt = null; // 文本框变化，清空样本信息
                        var sampCode = Samplecode;
                        Samplecode = null; // 清空实验室编号
                        if (!string.IsNullOrEmpty(sampCode))
                        {
                            this.SearchSampleTSC(sampCode); // 搜索Sample实例
                            if (PrintModel == 3)
                            {
                                // 停顿
                            }
                            else
                            {
                                this.PrintCommand.Execute(null); // 打印条码
                            }
                        }
                    }
                }
                finally
                {
                    _locker2.Release();
                }
            }
        }
        /// <summary>
        /// 样本编码获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SampleCode_GotFocus(object sender, RoutedEventArgs e)
        {
            var tbx = sender as TextBox;
            if (!string.IsNullOrEmpty(tbx.Text))
            {
                // 必须使用此触发方式，内部事件先执行，后执行此方法
                Dispatcher.CurrentDispatcher.InvokeAsync(() => tbx.SelectAll());
            }
        }
        /// <summary>
        /// 显示样本信息
        /// </summary>
        /// <param name="sampleTSC"></param>
        private void ShowSampeInfo(ExamedPrint sampleTSC)
        {
            Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                this.SampleTSCtxt = $"患者姓名: {sampleTSC.patientName};\r\n患者年龄: {sampleTSC.age};\r\n医院: {sampleTSC.hospitalName};\r\n地址: {sampleTSC.provinceName}/{sampleTSC.cityName}/{sampleTSC.areaName};";
            });
        }
        /// <summary>
        /// 打印二维码
        /// </summary>
        private void TestQ()
        {
            TSCLibApi.OpenPort("TSC TTP-244 Pro"); // 打开端口
            TSCLibApi.Setup("51", "17.2", "5", "15", "0", "2", "0");
            TSCLibApi.SendCommand("SET TEAR ON"); // The label gap will stop at the tear off position after print.
            TSCLibApi.ClearBuffer(); // 清除缓存
            string qstr = "QRCODE 150,20,L,2,A,0,M2,S3,\"320830C19210111001\"";
            TSCLibApi.SendCommand(qstr);
            TSCLibApi.PrintLabel("1", "1");
            TSCLibApi.ClosePort(); // 关闭端口
        }
    }
}
