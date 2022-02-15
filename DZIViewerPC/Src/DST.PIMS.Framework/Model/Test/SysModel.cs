using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model.Test
{
    public class SysModel
    {
        public RoleInfo Role { get; set; }
    }
    public class RoleInfo
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string OtherName { get; set; }
        public RoleInfo UpRole { get; set; }
    }
}
