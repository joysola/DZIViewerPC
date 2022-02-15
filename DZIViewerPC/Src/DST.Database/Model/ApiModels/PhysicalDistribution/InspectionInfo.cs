using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 送检查询列表
    /// </summary>
    public class InspectionInfo
    {
        public List<Inspection> records { get; set; } = new List<Inspection>();

        public Nullable<int> total { get; set; }

        public Nullable<int> size { get; set; }

        public Nullable<int> current { get; set; }

        public Nullable<int> pages { get; set; }
    }
}
