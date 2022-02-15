using DST.Common.Helper;
using DST.PIMS.Framework.Helper;
using GalaSoft.MvvmLight;
using MVVMExtension;
using System.Collections.Generic;
using System.Linq;

namespace DST.PIMS.Framework.Model
{
    /// <summary>
    /// 工作站包含的功能按钮列表信息
    /// </summary>
    public class WorkstationToolButtons : ObservableObject
    {
        public WorkstationToolButtons(EnumWorkstationType type, string name = null)
        {
            this.WorkstationType = type;
            this.WorkstationContent = name ?? EnumHelper.GetEnumDesc(type);
            this.ToolButtonList = ToolButtonListHelper.AllToolButtonList.Where(x => x.CanIncludedWorkstation.Contains(type)).ToList();
        }
        /// <summary>
        /// 工作站类型
        /// </summary>
        [Notification]
        public EnumWorkstationType WorkstationType { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        [Notification]
        public string WorkstationContent { get; set; }

        /// <summary>
        /// 工作站包含的工具按钮
        /// </summary>
        [Notification]
        public List<ToolButton> ToolButtonList { get; set; } = new List<ToolButton>();
    }
}
