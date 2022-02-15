using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model.Test
{
    public class CDModel
    {
        public string ExpressNo { get; set; } = "SF12738917239";
        public int? ExpSampNum { get; set; } = 12345;
        public string Search_Path { get; set; }
        public string Pathology_No { get; set; } = "1234567";
        public string PathCaseBase { get; set; } = "大组织";
        public string HPVResult { get; set; } = "HPV16分型阳性，其他分型未检出。HPV16分型阳性，其他分型未检出。HPV16分型阳性，其他分型未检出。HPV16分型阳性，其他分型未检出。";
        public AIResult AIRes { get; set; } = new AIResult();
        public string PatInfos => $"{PatInfos2.Name}/{PatInfos2.Sex}/{PatInfos2.Age}/{PatInfos2.Marriage}/{PatInfos2.Nation}";
        public PatInfo PatInfos2 { get; set; } = new PatInfo();
        public List<DiagResult> DiagHisRes { get; set; } = new Lazy<List<DiagResult>>(() =>
        {
            var xx = new List<DiagResult>();
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫1颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫2颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫4颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫3颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫5颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫ww颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫ff颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫bb颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫hdfh颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫sada颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫yyy5颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫yyy5颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫yyy5颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫yyy5颈管黏膜息肉" });
            xx.Add(new DiagResult { Diag_Date = DateTime.Now, Diag_Doc = Guid.NewGuid().ToString(), Diag_Res = "(宫颈）子宫yyy5颈管黏膜息肉" });
            return xx;
        })?.Value;
        public List<ImgInfo> ImgList { get; set; } = new Lazy<List<ImgInfo>>(() =>
        {
            var xx = new List<ImgInfo>();
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_104.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件1", Remark = "未见标本，制空片", Sample_Type = "液基细胞A" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_105.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件2", Remark = "未见标本，制空片", Sample_Type = "液基细胞B" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_106.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件3", Sample_Type = "液基细胞C" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_107.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件4", Sample_Type = "液基细胞G" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_108.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件5", Remark = "未见标本，制空片", Sample_Type = "液基细胞D" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_109.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件6", Sample_Type = "液基细胞F" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_110.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件7", Remark = "未见标本，制空片", Sample_Type = "液基细胞E" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_111.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件8", Sample_Type = "液基细胞F" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_111.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件9", Remark = "未见标本，制空片", Sample_Type = "液基细胞H" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_111.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件10", Remark = "未见标本，制空片", Sample_Type = "液基细胞I" });
            xx.Add(new ImgInfo { Url = "/DST.PIMS.Client;component/Images/Test/151_111.jpg", Desc = Guid.NewGuid().ToString(), DW_Part = "全子宫+双侧附件11", Remark = "未见标本，制空片", Sample_Type = "液基细胞J" });
            return xx;
        })?.Value;
        public DateTime? Apply_Date { get; set; } = DateTime.Now;
        public string Inspect_Dept { get; set; } = "妇科";
        public string Inspect_Doc { get; set; } = "张逸杰";
        public string Sample_Type { get; set; } = "脏器切除";
        public string Sample_Name { get; set; } = "全子宫+双侧附件";
        public string Diagnosis { get; set; } = "宫颈上皮内瘤样病变";
        public string ThingsbyEyes { get; set; } = @"全子宫附件，大小约9.5cm×5cm×4cm，肌层厚2cm，
肌层见肌瘤，大小约3.5cm×2.5cm×3cm，宫颈直径2.5cm，宫颈管长2.2cm； 
左侧输卵管，大小约5cm×1.2cm，伞端完整，切开见暗红液体，左卵巢大小约2cm×2cm×1.3cm； 
右输卵管，大小约4.2cm×1.4cm，伞端完整，切开见暗红液体，左卵巢大小约2.2cm×1.5cm×1cm。 
肌瘤/A，内膜/BC，肌层/D，宫颈/E，左输卵管/F，左卵巢/G，右输卵管/H，右卵巢/I。";
        public class PatInfo
        {
            public string Name { get; set; } = "张娟";
            public string Sex { get; set; } = "女";
            public string Age { get; set; } = "52岁";
            public string Marriage { get; set; } = "已婚";
            public string Nation { get; set; } = "汉族";
        }
        public class DiagResult
        {
            public string Diag_Doc { get; set; }
            public DateTime? Diag_Date { get; set; }
            public string Diag_Res { get; set; }
        }
        public class ImgInfo
        {
            public string Url { get; set; }
            public string Desc { get; set; }
            public string DW_Part { get; set; }
            public string Remark { get; set; }
            public string Sample_Type { get; set; }
        }
        public class AIResult
        {
            public string Analysis_State { get; set; } = "分析完成";
            public string Sample_Model { get; set; } = "阳性";
            public string Satisfy { get; set; } = "不满意";
            public string Inflam_State { get; set; } = "轻度";
            public string Abnormal_State { get; set; } = "有";
        }
    }

}
