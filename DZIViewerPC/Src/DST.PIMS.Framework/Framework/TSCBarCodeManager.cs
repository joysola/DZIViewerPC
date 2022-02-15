using DST.Common.TscBarcodePrint;
using DST.Database.Model;
using DST.PIMS.Framework.Attributes;
using Nico.Common;
using System.Collections.Generic;

namespace DST.PIMS.Framework
{
    [PrintManager(nameof(TSCBarCodeSetting))]
    public class TSCBarCodeManager
    {
        public static TSCBarCodeManager Singleton { get; } = new TSCBarCodeManager();

        #region 通用打印
        /// <summary>
        /// 打印多个条码
        /// </summary>
        /// <param name="barcodes"></param>
        /// <param name="setting"></param>
        public void Print(List<string> barcodes, TSCBarCodeSetting setting)
        {
            if (setting != null)
            {
                foreach (var barcode in barcodes)
                {
                    try
                    {
                        Logger.Info($"条码{barcode}开始打印!");
                        TSCLibApi.PrintStart();

                        string barcodeCommandStr = $"{setting.Code} {setting.X},{setting.Y},\"{setting.CodeType}\",{setting.Height},{setting.HumanReadable},{setting.Rotation},{setting.Narrow},{setting.Width},{setting.Alignment},\"{barcode}\"";
                        TSCLibApi.SendCommand(barcodeCommandStr);
                        // TSCLibApi.WindowsFont(20, 0, 15, 0, 0, 0, "SimSun", Barcode);
                    }
                    finally
                    {
                        TSCLibApi.PrintEnd();
                        Logger.Info($"条码{barcode}打印完成!");
                    }
                }
            }
        }

        /// <summary>
        /// 打印单个条码
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="setting"></param>
        public void Print(string barcode, TSCBarCodeSetting setting)
        {
            Print(new List<string> { barcode }, setting);
        }
        #endregion 通用打印

        #region 登记接收

        /// <summary>
        /// 登记接收，外送打印
        /// </summary>
        /// <param name="printCodes"></param>
        /// <param name="setting"></param>
        public void Print(List<AppFrmPrintCode> printCodes, TSCBarCodeSetting setting)
        {
            if (setting != null)
            {
                foreach (var code in printCodes)
                {
                    try
                    {
                        Logger.Info($"条码{code.BarCode}开始打印!");
                        TSCLibApi.PrintStart();
                        TSCLibApi.WindowsFont(setting.X - 80, setting.Y - 15, 13, 0, 0, 0, "SimSun", code.PatientName);
                        TSCLibApi.WindowsFont(setting.X, setting.Y - 15, 13, 0, 0, 0, "SimSun", code.ProductName);
                        string barcodeCommandStr = $"{setting.Code} {setting.X},{setting.Y},\"{setting.CodeType}\",{setting.Height},{setting.HumanReadable},{setting.Rotation},{setting.Narrow},{setting.Width},{setting.Alignment},\"{code.BarCode}\"";
                        TSCLibApi.SendCommand(barcodeCommandStr);
                        // TSCLibApi.WindowsFont(20, 0, 15, 0, 0, 0, "SimSun", Barcode);
                    }
                    finally
                    {
                        TSCLibApi.PrintEnd();
                        Logger.Info($"条码{code}打印完成!");
                    }
                }
            }
        }
        #endregion 登记接收
    }
}
