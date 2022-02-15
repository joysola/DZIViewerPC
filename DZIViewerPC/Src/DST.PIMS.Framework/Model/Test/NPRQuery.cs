using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model.Test
{
    public class NPRQuery
    {
        public int FrozenNum { get; set; }
        public int OtherNum { get; set; }
        public string PrintState { get; set; }
        public string CollectState { get; set; }
        public string ReportState { get; set; }
        public string UrgentState { get; set; }
        public NPRQuery_Collection MyCollections { get; set; }
        public NPRQuery_Collection TodayRegister { get; set; }
        public NPRQuery_Collection RecentlyOpen { get; set; }
        public string QuickBarCode { get; set; }


    }
    public class NPRQuery_Collection
    {
        public string BarCode { get; set; }
        public string ReportState { get; set; }
        public List<NPRQueryDataTest> Data { get; set; }
    }

    public class NPRQueryDataTest
    {
        public string Pathology_No { get; set; }
        public string Report_State { get; set; }
        public string DeptDoc { get; set; }
        public string BaseInfo { get; set; }
    }
}
