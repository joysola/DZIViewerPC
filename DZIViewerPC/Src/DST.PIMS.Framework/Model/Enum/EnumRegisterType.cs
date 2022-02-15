using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model
{
    public enum EnumRegisterType
    {
        /// <summary>
        /// 常规
        /// </summary>
        [Description("常规病理")]
        Normal,
        /// <summary>
        /// 细胞
        /// </summary>
        [Description("细胞病理")]
        Cell,
        /// <summary>
        /// 分子
        /// </summary>
        [Description("分子病理")]
        Molecular,
        /// <summary>
        /// 术中快速
        /// </summary>
        [Description("术中快速病理")]
        InOperQuick
    }
}
