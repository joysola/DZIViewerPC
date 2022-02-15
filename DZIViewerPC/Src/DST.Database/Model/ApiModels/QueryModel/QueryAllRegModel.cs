using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class QueryAllRegModel
    {
        /// <summary>
        /// 检查项目ID
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// 病理号
        /// </summary>
        public string PathCode { get; set; }
        /// <summary>
        /// 实验室编号
        /// </summary>
        public string LaboratoryCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 登记起始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 登记结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
