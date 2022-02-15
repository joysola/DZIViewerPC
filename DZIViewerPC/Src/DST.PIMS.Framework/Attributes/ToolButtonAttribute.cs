using DST.Common.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DST.PIMS.Framework.Attributes
{
    /// <summary>
    /// 按钮特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    sealed class ToolButtonAttribute : Attribute
    {
        readonly string _content;
        readonly string _backgroundImage;
        readonly string _backgroundImageMouseOver;
        /// <summary>
        /// 按钮特性
        /// </summary>
        /// <param name="content">按钮初始化名称</param>
        /// <param name="backgroundImage">按钮图片资源名称</param>
        /// <param name="backgroundImageMouseOver">按钮图片（鼠标移动）资源名称</param>
        public ToolButtonAttribute(string content, string backgroundImage, string backgroundImageMouseOver)
        {
            this._content = content;
            this._backgroundImage = backgroundImage;
            this._backgroundImageMouseOver = backgroundImageMouseOver;
        }
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string Content => _content;
        /// <summary>
        /// 按钮图片
        /// </summary>
        public BitmapImage BackgroundImage => ImageHelper.BitmapToBitmapImage((Bitmap)ToolButtonResource.ResourceManager.GetObject(_backgroundImage));
        /// <summary>
        /// 按钮鼠标移上图片
        /// </summary>
        public BitmapImage BackgroundImageMouseOver => ImageHelper.BitmapToBitmapImage((Bitmap)ToolButtonResource.ResourceManager.GetObject(_backgroundImageMouseOver));
    }
}
