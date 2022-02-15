using DST.Common.Extensions;
using DST.Database.Model;
using DST.PIMS.Framework.Attributes;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Helper;
using DST.PIMS.Framework.Model;
using HSPrint;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework
{
    [PrintManager(nameof(HSJPrintSetting))]
    public class HsjPrintManager : IPrintLabelManager
    {
        /// <summary>
        /// 当前的配置信息
        /// </summary>
        public HSJPrintSetting HSJSetting => Setting.HSJPrint;

        private PrintSetting Setting { get; set; }
        /// <summary>
        /// 重置海世嘉打印机
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scanDir"></param>
        /// <param name="templateFile"></param>
        public static void ResetPrinter(string type, string scanDir, string templateFile)
        {
            try
            {
                if (string.IsNullOrEmpty(type) || !int.TryParse(type, out int res))
                {
                    return;
                }

                LabelPrint hsjPrint = new LabelPrint(int.Parse(type));
                hsjPrint.Close();//初始化新的设备之前必须关闭旧设备
                hsjPrint.OpenImageLog(true);
                hsjPrint.SearchMachine();

                if (!string.IsNullOrEmpty(scanDir))
                {
                    hsjPrint.SetScanDir(scanDir);
                }

                if (!string.IsNullOrEmpty(templateFile))
                {
                    hsjPrint.LoadTemplateFile(templateFile);
                }

                hsjPrint.StartPrint();
            }
            catch (Exception e)
            {
                Logger.Error("启动海世嘉激光打码机失败：" + e.Message);
            }
        }
        /// <summary>
        /// 启动海世嘉包埋打印机
        /// </summary>
        /// <param name="sectionName">节名称</param>
        private void StartHsjPrint(string sectionName)
        {
            try
            {
                var setting = PrintSetHelper.GetPrintSetting(sectionName);
                if (setting != null)
                {
                    Setting = setting;
                    ResetPrinter(HSJSetting.PrintType, HSJSetting.ScanDir, HSJSetting.TemplateFile);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("启动海世嘉包埋打印机失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 生成海世嘉需要的打印文件
        /// </summary>
        /// <param name="dirPath">文件路径</param>
        /// <param name="fileTag">文件头</param>
        /// <param name="content">文件内容</param>
        /// <param name="flag">唯一识别</param>
        /// <returns></returns>
        private async Task GenerateFile(string dirPath, string fileTag, string content, string flag = null)
        {
            flag = flag ?? $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}_{Guid.NewGuid()}";
            string fileName = $@"{dirPath}\{fileTag}_{flag}.txt";
            var bytes = Encoding.UTF8.GetBytes(content);
            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, bytes.Length, true))
                {
                    await fileStream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("生成海世嘉文件失败!", ex);
            }
        }

        #region 取材
        /// <summary>
        /// 打印包埋盒
        /// </summary>
        /// <param name="barcodeModelList">包埋盒列表</param>
        public void Print(List<EmbedPrintCode> barcodeModelList)
        {
            barcodeModelList?.ForEach(printCode =>
            {
                var content = $"{printCode.WaxBlockCode}${printCode.LabCode}${printCode.WaxBlockNum}${printCode.PatientName}${printCode.DrawMaterPlace}";
                GenerateFile(HSJSetting.ScanDir, IniSectionConst.MaterialSection, content).NoWarning();
            });
        }
        #endregion 取材

        #region 前处理
        /// <summary>
        /// 前处理补打条码
        /// </summary>
        /// <param name="barcodeModelList"></param>
        public void Print(List<PhysDistReceiptBarcodeModel> barcodeModelList)
        {
            barcodeModelList?.ForEach(printCode =>
            {
                string patName = printCode.patientName;
                if (patName?.Length >= 6)
                {
                    patName = patName.Substring(0, 6);
                }

                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == printCode.Sex)?.dictValue;

                string hospitalName = printCode.hospitalName;
                if (hospitalName != null && hospitalName.Length >= 8)
                {
                    hospitalName = hospitalName.Substring(0, 8);
                }

                string content = $"{printCode.code}${printCode.productName}${patName}{sex}{printCode.age}${hospitalName}${printCode.provinceName?.Trim()} {printCode.cityName?.Trim()} {printCode.areaName?.Trim()}${printCode.code}${printCode.laboratoryCode}";
                GenerateFile(HSJSetting.ScanDir, IniSectionConst.RegisterSection, content).NoWarning();
            });
        }

        #endregion 前处理

        #region 登记
        /// <summary>
        /// 补打条码
        /// </summary>
        /// <param name="appFrmPrintCodes"></param>
        public void Print(List<AppFrmPrintCode> appFrmPrintCodes)
        {
            appFrmPrintCodes?.ForEach(printCode =>
            {
                string patName = printCode.PatientName;
                if (patName?.Length >= 6)
                {
                    patName = patName.Substring(0, 6);
                }

                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == printCode.Sex)?.dictValue;

                string hospitalName = printCode.HospName;
                if (hospitalName != null && hospitalName.Length >= 8)
                {
                    hospitalName = hospitalName.Substring(0, 8);
                }

                string content = $"{printCode.Code}${printCode.ProductName}${patName}{sex}{printCode.Age}${hospitalName}${printCode.ProvinceName?.Trim()} {printCode.CityName?.Trim()} {printCode.AreaName?.Trim()}${printCode.Code}${printCode.LabCode}";
                GenerateFile(HSJSetting.ScanDir, IniSectionConst.RegisterSection, content).NoWarning();
            });
        }
        #endregion 登记

        #region 制片
        /// <summary>
        /// 制片打印包埋盒
        /// </summary>
        /// <param name="slicePrintCodes"></param>
        public void Print(List<SlicePrintCode> slicePrintCodes)
        {
            slicePrintCodes?.ForEach(printCode =>
            {
                // 混合打印判断
                if (Setting.IsMix && HSJSetting.Products?.Count > 0 && !HSJSetting.Products.Contains(printCode.ProductID))
                {
                    return;
                }
                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == printCode.Sex);

                string content = $"{printCode.SliceNum}${printCode.LabCode}${printCode.PatientName} {sex?.dictValue} {printCode.Age}";
                //if (printCode.ProductID == DSTCode.TCTProdID)
                //{
                content += $"${printCode.ProvinceName?.Trim()} {printCode.CityName?.Trim()} {printCode.AreaName?.Trim()}";
                //}
                //else
                //{
                content += $"${printCode.InspecPlace}/{printCode.SliceShortNum}";
                //}
                content += $"${printCode.HospName}";
                //if (DSTCode.ImmuSpecProdIDList.Exists(x => x == printCode.ProductID) || printCode.ProductID == DSTCode.GastroscopeProdID)// 免疫组化 特殊染色  比组织多一个 标记物、染色物
                //{
                var marker = ExtendApiDict.Instance.ProdReagentDict?.FirstOrDefault(x => x.dictKey == printCode.Marker)?.dictValue;
                content += $"${marker}";
                //}
                GenerateFile(HSJSetting.ScanDir, IniSectionConst.ProductionSection, content).NoWarning();
            });
        }

        private void PrintCell(List<SlicePrintCode> slicePrintCodes)
        {
            slicePrintCodes?.ForEach(printCode =>
            {
                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == printCode.Sex);

                string content = $"{printCode.LabCode}${printCode.SliceNum}";
                content += $"${printCode.PatientName} {sex?.dictValue} {printCode.Age}${printCode.ProvinceName?.Trim()} {printCode.CityName?.Trim()} {printCode.AreaName?.Trim()}";
                content += $"${printCode.HospName}";

                GenerateFile(HSJSetting.ScanDir, IniSectionConst.ProductionSection, content).NoWarning();
            });
        }
        private void PrintTiss(List<SlicePrintCode> slicePrintCodes)
        {
            slicePrintCodes?.ForEach(printCode =>
            {
                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == printCode.Sex);

                string content = $"{printCode.LabCode}${printCode.SliceNum}";

                content += $"${printCode.PatientName} {sex?.dictValue} {printCode.Age}";
                content += $"${printCode.InspecPlace}/{printCode.SliceShortNum}";

                content += $"${printCode.HospName}";
                if (DSTCode.ImmuSpecProdIDList.Exists(x => x == printCode.ProductID) || printCode.ProductID == DSTCode.GastroscopeProdID)// 免疫组化 特殊染色 或者 胃镜 比组织多一个 标记物、染色物
                {
                    content += $"{printCode.ProductID}";
                }
                GenerateFile(HSJSetting.ScanDir, IniSectionConst.ProductionSection, content).NoWarning();
            });
        }
        /// <summary>
        /// 打印机启动方法
        /// </summary>
        /// <param name="sectionName"></param>
        public void InitPrint(string sectionName)
        {
            this.StartHsjPrint(sectionName);
        }
        #endregion 制片
    }
}
