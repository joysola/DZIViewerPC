using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using Nico.DeepZoom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ScaleTileSource
{
    public abstract class DZIBaseLayerFromScaleTileSource : MultiScaleTileSource, IDisposable
    {
        public abstract void Dispose();
        /// <summary>
        /// 获取导航图片数据方法
        /// </summary>
        /// <returns></returns>
        public abstract byte[] GetThumbnailImg();
        /// <summary>
        /// 获取对应瓦片源
        /// </summary>
        /// <param name="imgViewFile"></param>
        /// <returns></returns>
        public static DZIBaseLayerFromScaleTileSource GetTileSource(ImgViewFileInfo imgViewFile)
        {
            if (imgViewFile.DicectoryName.EndsWith(DZISingleConstant.Cons.FileExtension))
            {
                //var model = DZISingleConstant.Cons.LoadSingleFile(imgViewFile.LocalFilePath).GetAwaiter().GetResult();
                return new DZILayerFromDSTSingleTileSource(imgViewFile.DZI);
            }
            else
            {
                return new DZILayerFromLocalScaleTileSource(imgViewFile.LocalFilePath);
            }
        }
    }
}
