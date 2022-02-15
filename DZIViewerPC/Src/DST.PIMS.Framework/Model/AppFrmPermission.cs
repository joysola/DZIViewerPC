using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.PIMS.Framework.Model
{
    /// <summary>
    /// 申请单expander中的权限
    /// </summary>
    public class AppFrmPermission : ObservableObject
    {
        /// <summary>
        /// 申请单expander的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        [Notification]
        public Visibility Visible { get; set; } = Visibility.Visible;
        /// <summary>
        /// 是否可用
        /// </summary>
        [Notification]
        public bool IsEnable { get; set; } = true;
        /// <summary>
        /// 是否折叠
        /// </summary>
        [Notification]
        public bool IsExpanded { get; set; } = true;
    }
}
