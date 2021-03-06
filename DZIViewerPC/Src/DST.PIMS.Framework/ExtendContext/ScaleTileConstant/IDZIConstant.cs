using DST.Database.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ExtendContext
{
    public interface IDZIConstant
    {
        /// <summary>
        /// 导航图片文件夹名称
        /// </summary>
        public string NavImgLevel { get; }
        /// <summary>
        /// 导航图片名称
        /// </summary>
        public string NavImgName { get; }
        /// <summary>
        /// 瓦片大小
        /// </summary>
        public int DZITileSzie { get; }
        /// <summary>
        /// 总宽度
        /// </summary>
        public double DZIImgMaxHeight { get; }
        /// <summary>
        /// 总高度
        /// </summary>
        public double DZIImgMaxWidth { get; }
        /// <summary>
        /// 最小层数显示
        /// </summary>
        public int DZIMinLevel { get; }
        /// <summary>
        /// 信息文件
        /// </summary>
        public string SlideFile { get; }
        /// <summary>
        /// 是否是切片
        /// </summary>
        /// <returns></returns>
        public bool IsSlice(string baseDir);
        /// <summary>
        /// 获取瓦片路径
        /// </summary>
        /// <param name="tileLevel"></param>
        /// <param name="tilePositionX"></param>
        /// <param name="tilePositionY"></param>
        /// <returns></returns>
        public string GetTilePath(int tileLevel, int tilePositionX, int tilePositionY);

        /// <summary>
        /// 根据系统目录信息获取展示表格信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public List<ImgViewFileInfo> CreateImgViewTileList(IEnumerable<FileSystemInfo> infos);
    }
}
