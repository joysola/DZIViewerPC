using DST.Common.Helper;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Helper;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class TSCBarcodeSettingViewModel : CustomBaseViewModel
    {
        [Notification]
        public PrintSetting Setting { get; set; } = new PrintSetting { PrintType = nameof(TSCBarCodeSetting) };

        private string SectionName { get; set; } 

        /// <summary>
        /// 条码文字对齐方式字典
        /// </summary>
        public Dictionary<string, string> HumanReadDict { get; } = new Dictionary<string, string> { { "0", "无" }, { "1", "左对齐" }, { "2", "居中" }, { "3", "右对齐" } };

        /// <summary>
        /// 条码类型字典
        /// </summary>
        public Dictionary<string, string> CodeTypeDict { get; } = new Dictionary<string, string> {
            {"93" ,"93(推荐)"},
            {"128" ,"128"}, { "128M", "128M" }, { "EAN128", "EAN128" },{"EAN128M" ,"EAN128M"},
            {"25" ,"25(纯数字)"},{"25C" ,"25C(纯数字)"},{"25S" ,"25S(纯数字)"},
            {"TELEPEN" ,"TELEPEN"},{ "DPI","DPI(纯数字)"},{"DPL" ,"DPL(纯数字)"}
        };
        /// <summary>
        /// 保存
        /// </summary>
        public ICommand SaveConfigCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.SaveConfigur();
        })).Value;

        public TSCBarcodeSettingViewModel(string sectionName)
        {
            SectionName = sectionName;
            LoadConfigur();
        }

        /// <summary>
        /// 加载本地数据
        /// </summary>
        private void LoadConfigur()
        {
            if (!string.IsNullOrEmpty(SectionName))
            {
                var result = PrintSetHelper.GetPrintSetting(SectionName);
                if (result != null)
                {
                    Setting = result;
                }
            }
        }
        /// <summary>
        /// 保存本地数据
        /// </summary>
        private void SaveConfigur()
        {
            if (!string.IsNullOrEmpty(SectionName))
            {
                PrintSetHelper.SavePrintsetting(SectionName, Setting);
            }
        }
    }
}
