using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model.Test
{
    public class EmbedModel
    {
        public string Postscript { get; set; } = "脱钙";
        public string DW_Doc { get; set; } = "老王？";
        public List<TestData> DataList { get; set; } = new List<TestData> {
            new TestData { WaxStoneNo="123",SendParts="xqxxa",DW_Site="哈q哈哈",DW_Confirm_Date=DateTime.Now,Remark="sda",Confirm_DW_State="确认",Confirm_Embed_Date=DateTime.Now.AddDays(2)},
            new TestData { WaxStoneNo="233",SendParts="xxsxa",DW_Site="哈s哈a哈",DW_Confirm_Date=DateTime.Now,Remark="da",Confirm_DW_State="确认",Confirm_Embed_Date=DateTime.Now.AddDays(2)},
            new TestData { WaxStoneNo="14423",SendParts="x2xxa",DW_Site="哈sd哈s哈",DW_Confirm_Date=DateTime.Now,Remark="c",Confirm_DW_State="确认",Confirm_Embed_Date=DateTime.Now.AddDays(2)},
            new TestData { WaxStoneNo="1ee23",SendParts="xxsfdxa",DW_Site="哈xx哈哈",DW_Confirm_Date=DateTime.Now,Remark="dsadsa",Confirm_DW_State="确认",Confirm_Embed_Date=DateTime.Now.AddDays(2)},
            new TestData { WaxStoneNo="1ff23",SendParts="xxcc2xa",DW_Site="哈哈s哈",DW_Confirm_Date=DateTime.Now,Remark="czx",Confirm_DW_State="确认",Confirm_Embed_Date=DateTime.Now.AddDays(2)},
            new TestData { WaxStoneNo="12ff3",SendParts="xxxssa",DW_Site="哈sxx哈哈",DW_Confirm_Date=DateTime.Now,Remark="sadhjak",Confirm_DW_State="确认",Confirm_Embed_Date=DateTime.Now.AddDays(2)},
            new TestData { WaxStoneNo="12dds3",SendParts="xxcxa",DW_Site="哈xcc哈aa哈",DW_Confirm_Date=DateTime.Now,Remark="dfgdg",Confirm_DW_State="确认",Confirm_Embed_Date=DateTime.Now.AddDays(2)},
        };
        public string ThingsbyEye { get; set; } = @"（宫颈3点）组织1块，大小0.5cm×0.4cm×0.2cm，全取/A； 
（宫颈6点）组织1块，大小0.5cm×0.4cm×0.2cm，全取/B； 
（宫颈9点）组织1块，大小0.4cm×0.3cm×0.2cm，全取/C； 
（宫颈12点）组织1块，大小0.2cm×0.2cm×0.2cm，全取/D； 
（宫颈管内膜）暗红粘液样组织一堆，大小1.5cm×1cm×1cm，全取/E。";
        public class TestData
        {
            public string WaxStoneNo { get; set; }
            public string SendParts { get; set; }
            public string DW_Site { get; set; }
            public DateTime? DW_Confirm_Date { get; set; }
            public string Remark { get; set; }
            public string Confirm_DW_State { get; set; }
            public DateTime? Confirm_Embed_Date { get; set; }
        }
    }

}
