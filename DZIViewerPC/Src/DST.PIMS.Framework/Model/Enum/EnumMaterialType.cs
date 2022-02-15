using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model
{
    /// <summary>
    /// 取材类型 0 待取材 1延缓取材 2 补取 3 完成
    /// </summary>
    public enum EnumMaterialType
    {
        /// <summary>
        /// 0 待取材
        /// </summary>
        Remain = 0,
        /// <summary>
        /// 延缓取材
        /// </summary>
        Delay = 1,
        /// <summary>
        /// 2 补取
        /// </summary>
        Supply = 2,
        /// <summary>
        /// 3 完成
        /// </summary>
        Completed = 3,
    }
}
