using DST.Common.TscBarcodePrint;
using DST.Database.Model;
using DST.PIMS.Framework.Attributes;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Helper;
using DST.PIMS.Framework.Model;
using Newtonsoft.Json;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DST.PIMS.Framework
{
    /// <summary>
    /// TSC打码机
    /// </summary>
    [PrintManager(nameof(TSCPrintSetting))]
    public class TSCPrintManager : IPrintLabelManager
    {

        #region 前处理
        /// <summary>
        /// 打印物流签收条码
        /// </summary>
        public void Print(PhysDistReceiptBarcodeModel barcodeModel)
        {
            if (barcodeModel != null)
            {
                TSCLibApi.PrintStart();
                this.SetTSCCommand(barcodeModel);
                TSCLibApi.PrintEnd();
            }
        }

        /// <summary>
        /// 打印物流签收条码
        /// </summary>
        public void Print(List<PhysDistReceiptBarcodeModel> barcodeModelList)
        {
            if (barcodeModelList != null && barcodeModelList.Count > 0)
            {
                var setting = PrintSetHelper.GetTSCTPrintSetting(IniSectionConst.PhysDistSection);

                for (int i = 0; i < barcodeModelList.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        TSCLibApi.PrintStart();
                    }
                    this.SetTSCCommand(barcodeModelList[i], setting, i % 2);
                    if (i % 2 == 1 || i == barcodeModelList.Count - 1)
                    {
                        TSCLibApi.PrintEnd();
                    }
                }
            }
        }

        /// <summary>
        /// TSC条码打印，同时设置打印位置
        /// </summary>
        /// <param name="testcode">条码对象</param>
        /// <param name="leftOrRight">左=0，右=1</param>
        private void SetTSCCommand(PhysDistReceiptBarcodeModel testcode, int leftOrRight = 0)
        {
            try
            {
                int offset = leftOrRight == 0 ? 0 : 200;
                int y = 5;
                string command = $"QRCODE {145 + offset},20,L,2,A,0,M2,S3,\"" + testcode.code + "\"";// 打印二维码
                TSCLibApi.SendCommand(command);
                //样本一
                TSCLibApi.WindowsFont(30 + offset, y + 0, 14, 0, 0, 0, "SimSun", testcode.productName);
                //样本二
                TSCLibApi.WindowsFont(90 + offset, y + 25, 15, 0, 0, 0, "SimSun", testcode.age + "");
                //样本三
                string hospitalName = testcode.hospitalName;
                if (hospitalName != null && hospitalName.Length >= 8)
                {
                    hospitalName = hospitalName.Substring(0, 8);
                }

                TSCLibApi.WindowsFont(30 + offset, y + 50, 12, 0, 0, 0, "SimSun", hospitalName);

                TSCLibApi.WindowsFont(30 + offset, y + 25, 12, 0, 0, 0, "SimSun", testcode.patientName);

                //样本四
                TSCLibApi.WindowsFont(40 + offset, y + 70, 15, 0, 0, 0, "SimSun", testcode.provinceName);
                //样本五
                TSCLibApi.WindowsFont(76, y + 70, 15, 0, 0, 0, "SimSun", testcode.cityName);
                //样本六
                TSCLibApi.WindowsFont(116 + offset, y + 70, 15, 0, 0, 0, "SimSun", testcode.areaName);
                //样本七
                TSCLibApi.WindowsFont(40 + offset, y + 90, 15, 0, 0, 0, "SimSun", testcode.code);
                //样本八
                TSCLibApi.WindowsFont(110 + offset, y + 0, 14, 0, 0, 0, "SimSun", testcode.laboratoryCode);
            }
            catch (Exception e)
            {
                Logger.Error("打印条码失败：" + JsonConvert.SerializeObject(testcode));
            }
        }

        private void SetTSCCommand(PhysDistReceiptBarcodeModel physDistReceiptBarcode, TSCPrintSetting setting, int leftOrRight = 0)
        {
            try
            {
                int offset = leftOrRight == 0 ? 0 : setting.Second_X;
                int y = setting.Y;
                string command = $"QRCODE {setting.First_X + 115 + offset},{20},L,2,A,0,M2,S3,\"" + physDistReceiptBarcode.laboratoryCode + "\"";// 打印二维码
                TSCLibApi.SendCommand(command);
                //样本一
                TSCLibApi.WindowsFont(setting.First_X + offset, y + 0, 12, 0, 0, 0, "SimSun", physDistReceiptBarcode.productName);

                string patName = physDistReceiptBarcode.patientName;
                if (patName?.Length >= 6)
                {
                    patName = patName.Substring(0, 6);
                }

                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == physDistReceiptBarcode.Sex)?.dictValue;
                TSCLibApi.WindowsFont(setting.First_X + offset, y + 25, 14, 0, 0, 0, "SimSun", $"{physDistReceiptBarcode.patientName} {sex} {physDistReceiptBarcode.age}");

                string hospitalName = physDistReceiptBarcode.hospitalName;
                if (hospitalName != null && hospitalName.Length >= 8)
                {
                    hospitalName = hospitalName.Substring(0, 8);
                }

                TSCLibApi.WindowsFont(setting.First_X + offset, y + 50, 12, 0, 0, 0, "SimSun", hospitalName);

                TSCLibApi.WindowsFont(setting.First_X + offset, y + 65, 12, 0, 0, 0, "SimSun", $"{physDistReceiptBarcode.provinceName?.Trim()} {physDistReceiptBarcode.cityName?.Trim()} {physDistReceiptBarcode.areaName?.Trim()}");

                TSCLibApi.WindowsFont(setting.First_X + offset, y + 80, 13, 0, 0, 0, "SimSun", physDistReceiptBarcode.code);

                TSCLibApi.WindowsFont(setting.First_X + 70 + offset, y + 0, 12, 0, 0, 0, "SimSun", physDistReceiptBarcode.laboratoryCode);
            }
            catch (Exception e)
            {
                Logger.Error("打印条码失败：" + JsonConvert.SerializeObject(physDistReceiptBarcode));
            }
        }
        #endregion 前处理

        #region 登记
        /// <summary>
        /// 打印物流签收条码
        /// </summary>
        public void Print(List<AppFrmPrintCode> appFrmPrintCodes)
        {
            var setting = PrintSetHelper.GetTSCTPrintSetting(IniSectionConst.RegisterSection);
            if (appFrmPrintCodes != null && appFrmPrintCodes.Count > 0)
            {
                for (int i = 0; i < appFrmPrintCodes.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        TSCLibApi.PrintStart();
                    }
                    this.SetTSCCommand(appFrmPrintCodes[i], setting, i % 2);
                    if (i % 2 == 1 || i == appFrmPrintCodes.Count - 1)
                    {
                        TSCLibApi.PrintEnd();
                    }
                }
            }
        }

        /// <summary>
        /// TSC条码打印，同时设置打印位置
        /// </summary>
        /// <param name="appFrmPrintCode">条码对象</param>
        /// <param name="leftOrRight">左=0，右=1</param>
        private void SetTSCCommand(AppFrmPrintCode appFrmPrintCode, int leftOrRight = 0)
        {
            try
            {
                int offset = leftOrRight == 0 ? 0 : 200;
                int y = 5;
                string command = $"QRCODE {145 + offset},{20},L,2,A,0,M2,S3,\"" + appFrmPrintCode.LabCode + "\"";// 打印二维码
                TSCLibApi.SendCommand(command);
                //样本一
                TSCLibApi.WindowsFont(30 + offset, y + 0, 12, 0, 0, 0, "SimSun", appFrmPrintCode.ProductName);

                string patName = appFrmPrintCode.PatientName;
                if (patName?.Length >= 6)
                {
                    patName = patName.Substring(0, 6);
                }
                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == appFrmPrintCode.Sex)?.dictValue;
                TSCLibApi.WindowsFont(30 + offset, y + 25, 12, 0, 0, 0, "SimSun", $"{appFrmPrintCode.PatientName} {sex} {appFrmPrintCode.Age}");

                string hospitalName = appFrmPrintCode.HospName;
                if (hospitalName != null && hospitalName.Length >= 8)
                {
                    hospitalName = hospitalName.Substring(0, 8);
                }

                TSCLibApi.WindowsFont(30 + offset, y + 50, 12, 0, 0, 0, "SimSun", hospitalName);



                //样本四
                TSCLibApi.WindowsFont(30 + offset, y + 65, 12, 0, 0, 0, "SimSun", $"{appFrmPrintCode.ProvinceName?.Trim()} {appFrmPrintCode.CityName?.Trim()} {appFrmPrintCode.AreaName?.Trim()}");

                //样本七
                TSCLibApi.WindowsFont(30 + offset, y + 80, 13, 0, 0, 0, "SimSun", appFrmPrintCode.Code);
                //样本八
                TSCLibApi.WindowsFont(30 + 70 + offset, y + 0, 12, 0, 0, 0, "SimSun", appFrmPrintCode.LabCode);
            }
            catch (Exception e)
            {
                Logger.Error("打印条码失败：" + JsonConvert.SerializeObject(appFrmPrintCode));
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="appFrmPrintCode"></param>
        /// <param name="setting"></param>
        /// <param name="leftOrRight"></param>
        private void SetTSCCommand(AppFrmPrintCode appFrmPrintCode, TSCPrintSetting setting, int leftOrRight = 0)
        {
            try
            {
                int offset = leftOrRight == 0 ? 0 : setting.Second_X;
                int y = setting.Y;
                var labcode = appFrmPrintCode.LabCode ?? string.Empty;
                string command = $"QRCODE {setting.First_X + 115 + offset},{20},L,2,A,0,M2,S3,\"{labcode}\"";// 打印二维码
                TSCLibApi.SendCommand(command);

                TSCLibApi.WindowsFont(setting.First_X + offset, y + 0, 12, 0, 0, 0, "SimSun", appFrmPrintCode.ProductName);

                string patName = appFrmPrintCode.PatientName;
                if (patName?.Length >= 6)
                {
                    patName = patName.Substring(0, 6);
                }

                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == appFrmPrintCode.Sex)?.dictValue;
                TSCLibApi.WindowsFont(setting.First_X + offset, y + 25, 14, 0, 0, 0, "SimSun", $"{appFrmPrintCode.PatientName} {sex} {appFrmPrintCode.Age}");

                string hospitalName = appFrmPrintCode.HospName;
                if (hospitalName != null && hospitalName.Length >= 8)
                {
                    hospitalName = hospitalName.Substring(0, 8);
                }

                TSCLibApi.WindowsFont(setting.First_X + offset, y + 50, 12, 0, 0, 0, "SimSun", hospitalName);

                TSCLibApi.WindowsFont(setting.First_X + offset, y + 65, 12, 0, 0, 0, "SimSun", $"{appFrmPrintCode.ProvinceName?.Trim()} {appFrmPrintCode.CityName?.Trim()} {appFrmPrintCode.AreaName?.Trim()}");

                TSCLibApi.WindowsFont(setting.First_X + offset, y + 80, 13, 0, 0, 0, "SimSun", appFrmPrintCode.Code ?? string.Empty);

                TSCLibApi.WindowsFont(setting.First_X + 65 + offset, y + 0, 12, 0, 0, 0, "SimSun", appFrmPrintCode.LabCode ?? string.Empty);
            }
            catch (Exception e)
            {
                Logger.Error("打印条码失败：" + JsonConvert.SerializeObject(appFrmPrintCode));
            }
        }
        #endregion 登记

        #region 取材
        /// <summary>
        /// 打印物流签收条码
        /// </summary>
        public void Print(List<EmbedPrintCode> embedPrintCodes)
        {
            if (embedPrintCodes?.Count > 0)
            {
                var setting = PrintSetHelper.GetTSCTPrintSetting(IniSectionConst.MaterialSection);
                for (int i = 0; i < embedPrintCodes.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        TSCLibApi.PrintStart();
                    }
                    this.SetTSCCommand(embedPrintCodes[i], setting, i % 2);
                    if (i % 2 == 1 || i == embedPrintCodes.Count - 1)
                    {
                        TSCLibApi.PrintEnd();
                    }
                }
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="embedPrintCode">包埋盒打印实体</param>
        /// <param name="leftOrRight">左=0，右=1</param>
        private void SetTSCCommand(EmbedPrintCode embedPrintCode, int leftOrRight = 0)
        {
            try
            {
                int offset = leftOrRight == 0 ? 0 : 200;
                int y = 5;
                string command = $"QRCODE {20 + offset},20,L,2,A,0,M2,S3,\"" + embedPrintCode.WaxBlockCode + "\"";// 打印二维码
                TSCLibApi.SendCommand(command);


                TSCLibApi.WindowsFont(20 + offset, y - 5, 12, 0, 0, 0, "SimSun", embedPrintCode.LabCode);

                TSCLibApi.WindowsFont(130 + offset, y - 5, 12, 0, 0, 0, "SimSun", embedPrintCode.WaxBlockNum);


                TSCLibApi.WindowsFont(20 + offset, y + 65, 12, 0, 0, 0, "SimSun", embedPrintCode.PatientName);

                TSCLibApi.WindowsFont(130 + offset, y + 65, 12, 0, 0, 0, "SimSun", embedPrintCode.DrawMaterPlace);

            }
            catch (Exception e)
            {
                Logger.Error("打印条码失败：" + JsonConvert.SerializeObject(embedPrintCode));
            }
        }

        private void SetTSCCommand(EmbedPrintCode embedPrintCode, TSCPrintSetting setting, int leftOrRight = 0)
        {
            try
            {
                int offset = leftOrRight == 0 ? 0 : setting.Second_X;
                int y = setting.Y;
                string command = $"QRCODE {setting.First_X + offset},{y + 15},L,2,A,0,M2,S3,\"" + embedPrintCode.WaxBlockCode + "\"";// 打印二维码
                TSCLibApi.SendCommand(command);


                TSCLibApi.WindowsFont(setting.First_X + offset, y - 5, 12, 0, 0, 0, "SimSun", embedPrintCode.LabCode);

                TSCLibApi.WindowsFont(setting.First_X + 110 + offset, y - 5, 12, 0, 0, 0, "SimSun", embedPrintCode.WaxBlockNum);


                TSCLibApi.WindowsFont(setting.First_X + offset, y + 65, 12, 0, 0, 0, "SimSun", embedPrintCode.PatientName);

                TSCLibApi.WindowsFont(setting.First_X + 110 + offset, y + 65, 12, 0, 0, 0, "SimSun", embedPrintCode.DrawMaterPlace);

            }
            catch (Exception e)
            {
                Logger.Error("打印条码失败：" + JsonConvert.SerializeObject(embedPrintCode));
            }
        }
        #endregion 取材


        #region 制片
        /// <summary>
        /// 制片打印
        /// </summary>
        /// <param name="slicePrintCodes"></param>
        public void Print(List<SlicePrintCode> slicePrintCodes)
        {
            if (slicePrintCodes?.Count > 0)
            {
                var setting = PrintSetHelper.GetPrintSetting(IniSectionConst.ProductionSection);
                for (int i = 0; i < slicePrintCodes.Count; i++)
                {
                    // 混合打印判断
                    if (setting.IsMix && setting.TSCPrint.Products?.Count > 0 && !setting.TSCPrint.Products.Contains(slicePrintCodes[i].ProductID))
                    {
                        continue;
                    }
                    if (i % 2 == 0)
                    {
                        TSCLibApi.PrintStart();
                    }

                    this.SetTSCCommand(slicePrintCodes[i], setting, i % 2);

                    if (i % 2 == 1 || i == slicePrintCodes.Count - 1)
                    {
                        TSCLibApi.PrintEnd();
                    }
                }
            }
        }
        private void SetTSCCommand(SlicePrintCode slicePrintCode, int leftOrRight = 0)
        {
            try
            {
                int offset = leftOrRight == 0 ? 0 : 200;
                int y = 5;
                string command = $"QRCODE {25 + 120 + offset},{5 - 5},L,2,A,0,M2,S3,\"" + slicePrintCode.SliceNum + "\"";// 打印二维码
                TSCLibApi.SendCommand(command);

                TSCLibApi.WindowsFont(25 + offset, y - 5, 13, 0, 0, 0, "SimSun", slicePrintCode.LabCode);
                TSCLibApi.WindowsFont(25 + offset, y + 80, 13, 0, 0, 0, "SimSun", slicePrintCode.SliceNum);

                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == slicePrintCode.Sex);


                if (slicePrintCode.ProductID == DSTCode.TCTProdID)
                {
                    TSCLibApi.WindowsFont(25 + offset, y + 20, 12, 0, 0, 0, "SimSun", $"{slicePrintCode.PatientName} {sex?.dictValue} {slicePrintCode.Age} ");
                    TSCLibApi.WindowsFont(25 + offset, y + 50, 12, 0, 0, 0, "SimSun", $"{slicePrintCode.ProvinceName?.Trim()} {slicePrintCode.CityName?.Trim()} {slicePrintCode.AreaName?.Trim()} ");
                }
                else
                {
                    TSCLibApi.WindowsFont(25 + offset, y + 45, 12, 0, 0, 0, "SimSun", $"{slicePrintCode.PatientName} {sex?.dictValue} {slicePrintCode.Age} ");
                    TSCLibApi.WindowsFont(25 + offset, y + 10, 12, 0, 0, 0, "SimSun", $"{slicePrintCode.DrawMaterPlace}/{slicePrintCode.SliceShortNum}");
                }


                TSCLibApi.WindowsFont(25 + offset, y + 65, 12, 0, 0, 0, "SimSun", slicePrintCode.HospName);

                // 免疫组化 特殊染色  比组织多一个 标记物、染色物
                if (DSTCode.ImmuSpecProdIDList.Exists(x => x == slicePrintCode.ProductID))
                {
                    TSCLibApi.WindowsFont(25 + offset, y + 30, 12, 0, 0, 0, "SimSun", $"{slicePrintCode.ProductID}");
                }
            }
            catch (Exception e)
            {
                Logger.Error("打印条码失败：" + JsonConvert.SerializeObject(slicePrintCode));
            }
        }

        private void SetTSCCommand(SlicePrintCode slicePrintCode, PrintSetting setting, int leftOrRight = 0)
        {
            var tscSetting = setting.TSCPrint;
            try
            {
                int offset = leftOrRight == 0 ? 0 : tscSetting.Second_X;
                int y = tscSetting.Y;
                string command = $"QRCODE {tscSetting.First_X + 120 + offset},{y - 5},L,2,A,0,M2,S3,\"" + slicePrintCode.SliceNum + "\"";// 打印二维码
                TSCLibApi.SendCommand(command);

                TSCLibApi.WindowsFont(tscSetting.First_X + offset, y - 5, 13, 0, 0, 0, "SimSun", slicePrintCode.LabCode);
                // TSCLibApi.WindowsFont(tscSetting.First_X + offset, y + 80, 13, 0, 0, 0, "SimSun", slicePrintCode.SliceNum);

                var sex = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == slicePrintCode.Sex);


                if (slicePrintCode.ProductID == DSTCode.TCTProdID) // TCT特殊处理
                {
                    TSCLibApi.WindowsFont(tscSetting.First_X + offset, y + 20, 12, 0, 0, 0, "SimSun", $"{slicePrintCode.PatientName} {sex?.dictValue} {slicePrintCode.Age} ");
                    TSCLibApi.WindowsFont(tscSetting.First_X + offset, y + 50, 12, 0, 0, 0, "SimSun", $"{slicePrintCode.ProvinceName?.Trim()} {slicePrintCode.CityName?.Trim()} {slicePrintCode.AreaName?.Trim()} ");
                }
                else
                {
                    TSCLibApi.WindowsFont(tscSetting.First_X + offset, y + 45, 12, 0, 0, 0, "SimSun", $"{slicePrintCode.PatientName} {sex?.dictValue} {slicePrintCode.Age} ");
                    TSCLibApi.WindowsFont(tscSetting.First_X + offset, y + 10, 12, 0, 0, 0, "SimSun", $"{slicePrintCode.InspecPlace}/{slicePrintCode.SliceShortNum}");
                }


                TSCLibApi.WindowsFont(tscSetting.First_X + offset, y + 65, 12, 0, 0, 0, "SimSun", slicePrintCode.HospName);

                // 免疫组化 特殊染色、或者胃镜项目  比组织多一个 标记物、染色物
                if (DSTCode.ImmuSpecProdIDList.Exists(x => x == slicePrintCode.ProductID) || DSTCode.GastroscopeProdID == slicePrintCode.ProductID)
                {
                    var marker = ExtendApiDict.Instance.ProdReagentDict?.FirstOrDefault(x => x.dictKey == slicePrintCode.Marker)?.dictValue;
                    TSCLibApi.WindowsFont(tscSetting.First_X + offset, y + 30, 12, 0, 0, 0, "SimSun", $"{marker}");
                }
            }
            catch (Exception e)
            {
                Logger.Error("打印条码失败：" + JsonConvert.SerializeObject(slicePrintCode), e);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sectionName"></param>
        public void InitPrint(string sectionName)
        {

        }
        #endregion 制片
    }
}
