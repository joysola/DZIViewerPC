using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 物流扫描签收
    /// </summary>
    public class SignExpressByScan
    {
        /// <summary>
        /// 条形码
        /// </summary>
        public string barCode { get; set; }

        /// <summary>
        /// 物流id
        /// </summary>
        public string expressId { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string mailNo { get; set; }
    }
}
