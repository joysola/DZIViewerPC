using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    [NotifyAspect]
    public class SqliteEntity : ObservableObject
    {
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Create_User { get; set; }
        /// <summary>
        /// 创建部门
        /// </summary>

        public string Create_Dept { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime? Create_Time { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>

        public string Update_User { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>

        public DateTime? Update_Time { get; set; }
    }
}
