using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model.Test
{
    /// <summary>
    /// 常规病理
    /// </summary>
    public class NPRModel
    {
        public DateTime? Birthday { get; set; }
        public string PatName { get; set; }
        public string IDCard { get; set; }
        public double? Age { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public string MPhone { get; set; }
        public string Marriage { get; set; }
        public string Inspect_Hosp { get; set; }
        public string Inspect_Dept { get; set; }
        public string Inspect_Doc { get; set; }
        public string PatType { get; set; }
        public string OutPatNo { get; set; }
        public string INPNo { get; set; }
        public string WardCode { get; set; }
        public string BedNo { get; set; }
        public string ChargeType { get; set; }
        public string SampleType { get; set; }
        public string DrawMat_Doc { get; set; }
        public DateTime? DrawMat_Date { get; set; }
        public string Duty_Doc { get; set; }
        public DateTime? ReceiveMat_Date { get; set; }
        public DateTime? ReceiveMat_Doc { get; set; }
        public string Inspect_Name { get; set; }
        public List<DataDict1> DataList1 { get; set; } = TestDataList.DataList;
        public string Sample_Part { get; set; }
        public string Sample_Size { get; set; }
        public string Sample_Shape { get; set; }
        public string Sample_Color { get; set; }
        public string Sample_Activity { get; set; }
        public string Sample_GrowSpeed { get; set; }
        public string Sample_Hard { get; set; }
        public string Sample_SuspiciousTransfer { get; set; }
        public DateTime? Menstruation_LastDate { get; set; }
        public double Menstruation_BloodOut { get; set; }
        public string Menstruation_Period { get; set; }
        public DateTime? Menstruation_Date { get; set; }
        public double? Menstruation_Dosage { get; set; }
        public DateTime? Curettage_Date { get; set; }
        public string Urine_Pregnancy { get; set; }
        public string Clinical_Diagnosis { get; set; }
        public string Clinical_Data { get; set; }

        public string Case { get; set; }
        public string Pathology_No { get; set; }
        public string PatNo { get; set; }
        public string Identity_Type { get; set; }
        public string Identity_No { get; set; }
    }
    public class DataDict1
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Remark { get; set; }
    }
    public class TestDataList
    {
        public static List<DataDict1> DataList { get; set; } = new List<DataDict1> {
            new DataDict1 { No=1,Name="AAA",Remark="备注备注2",Type="hihh"},
            new DataDict1 { No=2,Name="BBB",Remark="备注备注3",Type="rrr"},
            new DataDict1 { No=3,Name="CCC",Remark="备注备注4",Type="hhvvvh"},
            new DataDict1 { No=4,Name="DDD",Remark="备注备注5",Type="hyyhh"},};
    }
}
