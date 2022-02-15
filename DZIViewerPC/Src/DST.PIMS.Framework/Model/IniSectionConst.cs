using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model
{
    /// <summary>
    /// Ini文件的Section列表，都是常量，禁止修改
    /// </summary>
    public class IniSectionConst
    {
        /// <summary>
        /// 制片工作站
        /// </summary>
        public const string ProductionSection = "Production";

        /// <summary>
        /// 前处理工作站
        /// </summary>
        public const string PhysDistSection = "PhysDist";

        /// <summary>
        /// 登记工作站
        /// </summary>
        public const string RegisterSection = "Register";

        /// <summary>
        /// 取材工作站
        /// </summary>
        public const string MaterialSection = "Material";
        /// <summary>
        /// 包埋工作站
        /// </summary>
        public const string EmbedSection = "Embed";
        /// <summary>
        /// 分子诊断工作站
        /// </summary>
        public const string MolecularDiagnosis = "MolecularDiagnosis";

        /// <summary>
        /// 上传工作站
        /// </summary>
        public const string Upload = "Upload";

        /// <summary>
        /// 通用配置
        /// </summary>
        public const string CommonConfig = "CommonConfig";
        /// <summary>
        /// 制片工作站
        /// </summary>
        public const string ProdBarcodeSection = "ProdBarcode";
    }
}
