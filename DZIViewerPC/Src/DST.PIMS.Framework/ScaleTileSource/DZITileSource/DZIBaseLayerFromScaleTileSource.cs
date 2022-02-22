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
        public abstract byte[] GetThumbnailImg();
        public static DZIBaseLayerFromScaleTileSource GetTileSource(ImgViewFileInfo imgViewFile)
        {
            if (imgViewFile.DicectoryName.EndsWith(".dst"))
            {
                var model = DZISingleConstant.Cons.LoadSingleFile(imgViewFile.LocalFilePath).GetAwaiter().GetResult();
                return new DZILayerFromDSTSingleTileSource(model);
            }
            else
            {
                return new DZILayerFromLocalScaleTileSource(imgViewFile.LocalFilePath);
            }
        }
    }
}
