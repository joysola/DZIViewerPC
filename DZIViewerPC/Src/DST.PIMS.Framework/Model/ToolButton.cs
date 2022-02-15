using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace DST.PIMS.Framework.Model
{
    /// <summary>
    /// 工具按钮
    /// </summary>
    public class ToolButton
    {
        /// <summary>
        /// 控件默认的背景图片
        /// </summary>
        public BitmapImage BackgroundImage { get; set; }

        /// <summary>
        /// 鼠标在图标上悬浮时显示的背景图片
        /// </summary>
        public BitmapImage BackgroundImageMouseOver { get; set; }

        /// <summary>
        /// 控件名称
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 该按钮可以被包含的工作站列表
        /// </summary>
        public List<EnumWorkstationType> CanIncludedWorkstation { get; set; } = new List<EnumWorkstationType>();
        /// <summary>
        /// 控件按钮类型
        /// </summary>
        public EnumToolButtonType ToolBtnType { get; set; }
        public string EnumName { get; set; }
    }
}
