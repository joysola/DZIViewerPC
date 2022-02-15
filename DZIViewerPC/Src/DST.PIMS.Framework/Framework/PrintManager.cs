using Nico.Common;
using Spire.Pdf;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DST.PIMS.Framework
{
    /// <summary>
    /// 文件打印模块统一处理
    /// </summary>
    public class PrintManager
    {
        public static PrintManager Instance { get; } = new PrintManager();

        private PrintManager()
        {
        }

        /// <summary>
        /// 打印PDF文档
        /// </summary>
        /// <param name="pdfPath">pdf文件路径</param>
        /// <param name="printerName">打印机名称，如果不填写则使用默认打印机</param>
        public async Task<bool> PrintPDF(string pdfPath, string printerName = "")
        {
            if(!File.Exists(pdfPath))
            {
                return false;
            }

            try
            {
                //加载PDF文档
                using (PdfDocument doc = new PdfDocument())
                {
                    doc.LoadFromFile(pdfPath);

                    //指定打印机
                    if (!string.IsNullOrEmpty(printerName))
                    {
                        doc.PrintSettings.PrinterName = printerName;
                    }

                    //打印PDF文档
                    doc.Print();
                }
            }
            catch(Exception ex)
            {
                Logger.Error("打印文档失败：" + ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> PrintFile(string filePath, string printerName = "")
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.Arguments = "\"" + printerName + "\"";
                info.Verb = "PrintTo";
                info.FileName = filePath;
                info.CreateNoWindow = true;
                info.WindowStyle = ProcessWindowStyle.Hidden;

                Process p = new Process();
                p.StartInfo = info;
                p.Start();
                p.WaitForInputIdle();
            }
            catch (Exception ex)
            {
                Logger.Error("打印文档失败：" + ex.Message);
                return false;
            }

            return true;
        }
    }
}
