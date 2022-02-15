using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 制片特染查询实体
    /// </summary>
    public class QueryAddSlice
    {
        /// <summary>
        /// 蜡块编号
        /// </summary>
        public string WaxBlockCode { get; set; } = string.Empty;
        /// <summary>
        /// 标记物/染色剂
        /// </summary>
        public string Marker { get; set; } = string.Empty;
    }
}
