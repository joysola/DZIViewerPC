using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ExtendContext
{
    public static class DSTCode
    {
        /// <summary>
        /// 取材工作站肉眼
        /// </summary>
        public static readonly string MaterialCode = "取材工作站";
        /// <summary>
        /// 样本名称常用词scenecode
        /// </summary>
        public static readonly string SampleStr = "标本名称";

        /// <summary>
        /// 诊断名称常用词scenecode
        /// </summary>
        public static readonly string DiagnosisStr = "临床诊断";

        /// <summary>
        /// 技术医嘱细胞 对应的技术医嘱类型 字典键值
        /// </summary>
        public static readonly List<string> TechCellAdvices = new List<string> { "7", "8" };

        /// <summary>
        /// 胃镜项目id
        /// </summary>
        public static readonly string GastroscopeProdID = "1395619316226138114";

        /// <summary>
        /// TCT项目id
        /// </summary>
        public static readonly string TCTProdID = "1395618831419121666";
        /// <summary>
        /// 免疫组化 项目id
        /// </summary>
        public static readonly string ImmuhistchmProdID = "1395620999601659906";
        /// <summary>
        /// 特殊染色 项目id
        /// </summary>
        public static readonly string SpecicStainProdID = "1395621093667315714";
        /// <summary>
        /// 细胞双染 项目id
        /// </summary>
        public static readonly string CellDoubDyeProdID = "1395620901236842498";
        /// <summary>
        /// 穿刺活检
        /// </summary>
        public static readonly string AspireBiopsProdID = "1395619087691096065";
        /// <summary>
        /// 内镜活检
        /// </summary>
        public static readonly string EndoscBiopsProdID = "1395619184072007681";
        /// <summary>
        /// 局部切除
        /// </summary>
        public static readonly string LocalExcisnProdID = "1395619467200110593";
        /// <summary>
        /// 骨髓活检
        /// </summary>
        public static readonly string BoneMarBiopProdID = "1395619630564057090";
        /// <summary>
        /// 手术小标本
        /// </summary>
        public static readonly string SurgSpecimnProdID = "1395619773594017794";
        /// <summary>
        /// 手术中标本
        /// </summary>
        public static readonly string IntrSpecimnProdID = "1395619884743073794";
        /// <summary>
        /// 手术大标本
        /// </summary>
        public static readonly string LargSrgSpmnProdID = "1395620154382295042";

        /// <summary>
        /// 免疫组化 特殊染色 
        /// </summary>

        public static readonly List<string> ImmuSpecProdIDList = new List<string> { ImmuhistchmProdID, SpecicStainProdID };

        /// <summary>
        /// 技术医嘱细胞列表（TCT、细胞双染需要特殊处理）项目id集合
        /// </summary>
        public static readonly List<string> TechAdCellProdIDList = new List<string> { TCTProdID, CellDoubDyeProdID };
        /// <summary>
        /// 常规病理
        /// </summary>
        public static readonly List<string> ConvenPathList = new List<string> { AspireBiopsProdID, EndoscBiopsProdID, LocalExcisnProdID, BoneMarBiopProdID, SurgSpecimnProdID, IntrSpecimnProdID, LargSrgSpmnProdID, GastroscopeProdID };
    }
}
