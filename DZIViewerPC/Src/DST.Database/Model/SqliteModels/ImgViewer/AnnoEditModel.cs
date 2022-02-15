using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    [NotifyAspect]
    public class AnnoEditModel : ObservableObject
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 标记名称
        /// </summary>
        public string Anno_Name { get; set; }
    }
}
