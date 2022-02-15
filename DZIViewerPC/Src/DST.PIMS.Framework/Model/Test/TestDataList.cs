using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model.Test
{
    public class DataDict1
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Remark { get; set; }
    }
    public class TestDataList
    {
        public static List<DataDict1> DataList { get; set; } = new List<DataDict1> {
            new DataDict1 { No=1,Name="AAA",Remark="备注备注2",Type="hihh"},
            new DataDict1 { No=2,Name="BBB",Remark="备注备注3",Type="rrr"},
            new DataDict1 { No=3,Name="CCC",Remark="备注备注4",Type="hhvvvh"},
            new DataDict1 { No=4,Name="DDD",Remark="备注备注5",Type="hyyhh"},};
    }
}
