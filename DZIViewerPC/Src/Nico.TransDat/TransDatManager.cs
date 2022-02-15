using Nico.TransDat.Base;
using Nico.TransDat.Model;
using Nico.TransDat.Module;
using System.Collections.Generic;

namespace Nico.TransDat
{
    /// <summary>
    /// Dat文件提供给外部调用接口
    /// </summary>
    public class TransDatManager
    {
        public static TransDatManager Instance = new TransDatManager();

        /// <summary>
        /// 私有构造
        /// </summary>
        private TransDatManager()
        {
        }

        /// <summary>
        /// 将9层图所有瓦片图写入到一个Dat文件中，默认以最新的版本方式生成Dat文件
        /// </summary>
        /// <param name="simplePath">样本路径</param>
        /// <param name="targetPath">dat文件的生成路径,包含.dat 扩展名，默认为空，则dat文件自动存放在SimplePath</param>
        /// <returns>返回异常信息，如果未空则解析成功，如果不为空则解析失败</returns>
        public string CreateDatFileFromTileImage(string simplePath, string targetPath = "", string version = "V1.0.0.0")
        {
            string result = string.Empty;
            switch(version)
            {
                case "V1.0.0.0":
                    result = new DatModuleFirstVersion().CreateDatFileFromTileImage(simplePath, targetPath);
                    break;

                case "V2.0.0.0":
                    result = new DatModuleSecondVersion().CreateDatFileFromTileImage(simplePath, targetPath);
                    break;

                default:
                    result = new DatModuleSecondVersion().CreateDatFileFromTileImage(simplePath, targetPath);
                    break;
            }
          
            return result;
        }

        /// <summary>
        /// 解析dat文件成9层图
        /// </summary>
        /// <param name="datPath">dat文件的完整路径</param>
        /// <param name="targetPath">dat文件解析出来的文件所存放的地址</param>
        /// <returns>返回异常信息，如果未空则解析成功，如果不为空则解析失败</returns>
        public string ResolverDatFileToTileImage(string datPath, string targetPath = "")
        {
            string result = string.Empty;

            DatVersionInfo datVer = BaseDatModule.GetDatVersion(datPath);
            switch(datVer.Version)
            {
                case "V1.0.0.0":
                    result = new DatModuleFirstVersion().ResolverDatFileToTileImage(datPath, targetPath);
                    break;
                case "V2.0.0.0":
                    result = new DatModuleSecondVersion().ResolverDatFileToTileImage(datPath, targetPath);
                    break;
                default:
                    result = new DatModuleFirstVersion().ResolverDatFileToTileImage(datPath, targetPath);
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取Dat中的单个瓦片图
        /// </summary>
        /// <param name="datPath">dat路径</param>
        /// <param name="level">层</param>
        /// <param name="x">x信息，也就是第几列</param>
        /// <param name="y">y信息，也就是第几行</param>
        public byte[] GetSingleTileImageFromDat(string datPath, int level, int x, int y)
        {
            byte[] result = null;

            DatVersionInfo datVer = BaseDatModule.GetDatVersion(datPath);
            switch (datVer.Version)
            {
                case "V1.0.0.0":
                    result = new DatModuleFirstVersion().GetSingleTileImageFromDat(datPath, level, x, y);
                    break;

                case "V2.0.0.0":
                    result = new DatModuleSecondVersion().GetSingleTileImageFromDat(datPath, level, x, y);
                    break;

                default:
                    result = new DatModuleFirstVersion().GetSingleTileImageFromDat(datPath, level, x, y);
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取某一层，某个范围内的瓦片图
        /// </summary>
        /// <param name="datPath">Dat文件完整路径</param>
        /// <param name="level">第几层</param>
        /// <param name="startX">起始列，包含该值</param>
        /// <param name="endX">终止列，包含该值</param>
        /// <param name="startY">起始行，包含该值</param>
        /// <param name="endY">终止列，包含该值</param>
        public List<TileImage> GetTileImageListFromDat(string datPath, int level, int startX, int endX, int startY, int endY)
        {
            List<TileImage> result = null;
            DatVersionInfo datVer = BaseDatModule.GetDatVersion(datPath);
            switch (datVer.Version)
            {
                case "V1.0.0.0":
                    result = new DatModuleFirstVersion().GetTileImageListFromDat(datPath, level, startX, endX, startY, endY);
                    break;
                case "V2.0.0.0":
                    result = new DatModuleSecondVersion().GetTileImageListFromDat(datPath, level, startX, endX, startY, endY);
                    break;
                default:
                    result = new DatModuleFirstVersion().GetTileImageListFromDat(datPath, level, startX, endX, startY, endY);
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取Dat文件中非瓦片图的其他文件数据
        /// </summary>
        /// <param name="datPath">Dat文件路径</param>
        /// <param name="otherFileName">要查找的文件名称，包含后缀信息，录入：Slide.dat</param>
        /// <returns>文件的二进制数据</returns>
        public byte[] GetOtherFileFromDat(string datPath, string otherFileName)
        {
            byte[] result = null;

            DatVersionInfo datVer = BaseDatModule.GetDatVersion(datPath);
            switch (datVer.Version)
            {
                case "V1.0.0.0":
                    result = new DatModuleFirstVersion().GetOtherFileFromDat(datPath, otherFileName);
                    break;
                case "V2.0.0.0":
                    result = new DatModuleSecondVersion().GetOtherFileFromDat(datPath, otherFileName);
                    break;

                default:
                    result = new DatModuleFirstVersion().GetOtherFileFromDat(datPath, otherFileName);
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取Dat文件中的所有图片信息
        /// </summary>
        /// <param name="datPath">Dat文件路径</param>
        /// <param name="isLoadImageByte">是否加载图片二进制，默认false，不加载实际图片</param>
        public List<TileImage> GetAllTileImage(string datPath)
        {
            List<TileImage> result = new List<TileImage>();
            DatVersionInfo datVer = BaseDatModule.GetDatVersion(datPath);
            switch (datVer.Version)
            {
                case "V1.0.0.0":
                    result = new DatModuleFirstVersion().GetAllTileImage(datPath);
                    break;
                case "V2.0.0.0":
                    result = new DatModuleSecondVersion().GetAllTileImage(datPath);
                    break;
                default:
                    result = new DatModuleFirstVersion().GetAllTileImage(datPath);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取Dat文件的版本信息，包含版本、最大的宽度高度
        /// </summary>
        /// <param name="datPath">Dat文件路径</param>
        public DatVersionInfo GetDatVersionInfo(string datPath)
        {
            return BaseDatModule.GetDatVersion(datPath);
        }
    }
}
