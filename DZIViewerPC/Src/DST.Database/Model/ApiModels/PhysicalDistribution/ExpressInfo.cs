using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 物流查询结果
    /// </summary>
    public class ExpressInfo
    {
        public List<Express> records { get; set; }

        public Nullable<int> total { get; set; }

        public Nullable<int> size { get; set; }

        public Nullable<int> current { get; set; }

        public Nullable<int> pages { get; set; }
    }
}
