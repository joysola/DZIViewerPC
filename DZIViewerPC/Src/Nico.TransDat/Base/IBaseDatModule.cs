using Nico.TransDat.Model;
using System.Collections.Generic;

namespace Nico.TransDat.Base
{
    /// <summary>
    /// Dat文件的操作接口
    /// </summary>
    interface IBaseDatModule
    {
        /// <summary>
        /// Dat的版本信息
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// 将9层图所有瓦片图写入到一个Dat文件中
        /// </summary>
        /// <param name="simplePath">样本路径</param>
        /// <param name="targetPath">dat文件的生成路径,包含.dat 扩展名，默认为空，则dat文件自动存放在SimplePath</param>
        /// <returns>返回异常信息，如果未空则解析成功，如果不为空则解析失败</returns>
        string CreateDatFileFromTileImage(string simplePath, string targetPath = "");

        /// <summary>
        /// 解析dat文件
        /// </summary>
        /// <param name="datPath">dat文件的完整路径</param>
        /// <param name="targetPath">dat文件解析出来的文件所存放的地址</param>
        /// <returns>返回异常信息，如果未空则解析成功，如果不为空则解析失败</returns>
        string ResolverDatFileToTileImage(string datPath, string targetPath = "");

        /// <summary>
        /// 根据索引获取瓦片图
        /// </summary>
        byte[] GetSingleTileImageFromDat(string datPath, int level, int x, int y);

        /// <summary>
        /// 获取某一层，某个范围内的瓦片图
        /// </summary>
        /// <param name="datPath">Dat文件完整路径</param>
        /// <param name="level">第几层</param>
        /// <param name="startX">起始列，包含该值</param>
        /// <param name="endX">终止列，包含该值</param>
        /// <param name="startY">起始行，包含该值</param>
        /// <param name="endY">终止列，包含该值</param>
        /// <returns></returns>
        List<TileImage> GetTileImageListFromDat(string datPath, int level, int startX, int endX, int startY, int endY);

        /// <summary>
        /// 获取Dat文件中非瓦片图的其他文件数据
        /// </summary>
        /// <param name="datPath">Dat文件路径</param>
        /// <param name="otherFileName">要查找的文件名称，包含后缀信息，录入：Slide.dat</param>
        /// <returns>文件的二进制数据</returns>
        byte[] GetOtherFileFromDat(string datPath, string otherFileName);

        /// <summary>
        /// 获取Dat文件中的所有图片信息
        /// </summary>
        /// <param name="datPath">Dat文件路径</param>
        List<TileImage> GetAllTileImage(string datPath);

        /// <summary>
        /// 等待结束
        /// </summary>
        void WaitFinish();
    }
}
