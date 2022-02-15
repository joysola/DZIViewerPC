using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model.Test
{
    public class ProductModel
    {
        public string Case { get; set; }
        public string PatNameNo { get; set; }
        public DateTime? EmbedStartDate { get; set; }
        public DateTime? EmbedEndDate { get; set; }
        public string AdviceState { get; set; }
        public string AdviceType { get; set; }
        public string Postscript { get; set; } = "脱钙";
        public string DW_Doc { get; set; } = "老王？";
        public int? TotalNum { get; set; } = 145;
        public string ThingsbyEye { get; set; } = @"（宫颈3点）组织1块，大小0.5cm×0.4cm×0.2cm，全取/A； 
（宫颈6点）组织1块，大小0.5cm×0.4cm×0.2cm，全取/B； 
（宫颈9点）组织1块，大小0.4cm×0.3cm×0.2cm，全取/C； 
（宫颈12点）组织1块，大小0.2cm×0.2cm×0.2cm，全取/D； 
（宫颈管内膜）暗红粘液样组织一堆，大小1.5cm×1cm×1cm，全取/E。";
        public string Receive_State { get; set; }
        public List<TestData> DataList { get; set; } = new List<TestData>
        {
            new TestData{WaxStoneNo="123",SendParts="贺卡手机号",DW_Site="收到",Enbed_Date=DateTime.Now,SlideNum=14,Remark="失败了！",Product_State="Failed",Confirm_Product_Date=DateTime.Now.AddDays(2)},
            new TestData{WaxStoneNo="345",SendParts="是多少",DW_Site="双打",Enbed_Date=DateTime.Now,SlideNum=14,Remark="失败了！",Product_State="Success",Confirm_Product_Date=DateTime.Now.AddDays(2)},
            new TestData{WaxStoneNo="fsas",SendParts="是大多数",DW_Site="反对法",Enbed_Date=DateTime.Now,SlideNum=14,Remark="失败了吗！",Product_State="Failed",Confirm_Product_Date=DateTime.Now.AddDays(2)},
            new TestData{WaxStoneNo="789",SendParts="是非常",DW_Site="二太多",Enbed_Date=DateTime.Now,SlideNum=14,Remark="失败了吗！",Product_State="Success",Confirm_Product_Date=DateTime.Now.AddDays(2)},
            new TestData{WaxStoneNo="63d",SendParts="换行",DW_Site="uio",Enbed_Date=DateTime.Now,SlideNum=14,Remark="失败了？不可能",Product_State="Failed",Confirm_Product_Date=DateTime.Now.AddDays(2)},
            new TestData{WaxStoneNo="hki",SendParts="与i但是",DW_Site="萨达",Enbed_Date=DateTime.Now,SlideNum=14,Remark="失败了！",Product_State="Success",Confirm_Product_Date=DateTime.Now.AddDays(2)},
        };
        public class TestData
        {
            public string WaxStoneNo { get; set; }
            public string SendParts { get; set; }
            public string DW_Site { get; set; }
            public DateTime? Enbed_Date { get; set; }
            public int? SlideNum { get; set; }
            public string Remark { get; set; }
            public string Product_Doc { get; set; }
            public string Product_Type { get; set; }
            public string Product_State { get; set; }
            public string Scan_State { get; set; }
            public DateTime? Product_Date { get; set; }
            public DateTime? Confirm_Product_Date { get; set; }
        }
    }
}
