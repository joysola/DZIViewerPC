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
    public class DZILayerFromDSTSingleTileSource : DZIBaseLayerFromScaleTileSource
    {
        private readonly DZIModel _dziModel;
        private FileStream _stream;
        public DZILayerFromDSTSingleTileSource(DZIModel model) : base()
        {
            this._dziModel = model;
            this._stream = new FileStream(model.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024, true);
            //var (width, height) = DZIConstant.GetDZISize(samplePath);
            base.InitPar((long)DZISingleConstant.Cons.DZIImgMaxWidth, (long)DZISingleConstant.Cons.DZIImgMaxHeight, DZISingleConstant.Cons.DZIImgSzie, 0);
        }

        public override void Dispose()
        {
            this._stream.Dispose();
            this._stream = null;
        }

        public override byte[] GetThumbnailImg() => DZISingleConstant.Cons.GetNavImg(_dziModel).ConfigureAwait(false).GetAwaiter().GetResult();

        public override void GetTileLayersAngle(ref double CenterX, ref double CenterY, ref double Angle, ref double OffsetX, ref double OffsetY) { }

        protected override object GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY)
        {
            //Stream stream = null;
            //if (tileLevel > DZISingleConstant.Cons.DZIMinLevel) // 防止DZI阅片出现残影
            //{
            //    string imgFilePath = DZISingleConstant.Cons.GetTilePath(tileLevel, tilePositionX, tilePositionY);
            //    var bytes = DZISingleConstant.Cons.GetTileData(_stream, _dziModel, imgFilePath).ConfigureAwait(false).GetAwaiter().GetResult();
            //    if (bytes != null)
            //    {
            //        stream = new MemoryStream(bytes);
            //    }
            //}
            //return stream;
            return null;
        }


        protected override async Task<Stream> GetTileLayersAsync(int tileLevel, int tilePositionX, int tilePositionY)
        {
            Stream stream = null;
            if (tileLevel > DZISingleConstant.Cons.DZIMinLevel) // 防止DZI阅片出现残影
            {
                string imgFilePath = DZISingleConstant.Cons.GetTilePath(tileLevel, tilePositionX, tilePositionY);
                //stream = new FileStream(_dziModel.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024, true);

                var bytes = await DZISingleConstant.Cons.GetTileData(_stream, _dziModel, imgFilePath).ConfigureAwait(false);
                if (bytes != null)
                {
                    stream = new MemoryStream(bytes);
                }
            }
            return stream;
        }

    }
}
