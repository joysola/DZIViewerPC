using DST.Common.Helper;
using DST.PIMS.Framework.ExtendContext;
using Nico.DeepZoom;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ScaleTileSource
{

    public class DZILayerFromLocalScaleTileSource : MultiScaleTileSource, IDisposable
    {
        private string SamplePath { get; set; } = string.Empty;

        public DZILayerFromLocalScaleTileSource(string samplePath) : base()
        {
            this.SamplePath = samplePath;
            //var (width, height) = DZIConstant.GetDZISize(samplePath);
            base.InitPar((long)DZIConstant.DZIImgMaxWidth, (long)DZIConstant.DZIImgMaxHeight, DZIConstant.DZIImgSzie, 0);
        }

        public void Dispose()
        {
        }

        public override void GetTileLayersAngle(ref double CenterX, ref double CenterY, ref double Angle, ref double OffsetX, ref double OffsetY)
        {
        }

        protected override object GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY)
        {
            if (tileLevel > DZIConstant.DZIMinLevel) // 防止DZI阅片出现残影
            {
                string imgFilePath = DZIConstant.GetImgFilePath(tileLevel, tilePositionX, tilePositionY);
                string imgTotalPath = $"{this.SamplePath}\\{DZIConstant.DZIFilesDir}\\{imgFilePath}";

                return imgTotalPath;
            }
            return null;
        }
        /// <summary>
        /// GetTileLayers返回值是string的时候读取流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tileLevel"></param>
        /// <param name="tilePositionX"></param>
        /// <param name="tilePositionY"></param>
        /// <returns></returns>
        protected override async Task<MemoryStream> ReadImgStream(string url/*, int tileLevel, int tilePositionX, int tilePositionY*/)
        {
            MemoryStream ms = null;
            if (File.Exists(url))
            {
                byte[] bytes;
                using (var fs = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.None, 4 * 1024, FileOptions.Asynchronous))
                {
                    bytes = new byte[fs.Length];
                    await fs.ReadAsync(bytes, 0, (int)fs.Length);
                }
                if (bytes != null)
                {
                    ms = new MemoryStream(bytes);
                }
            }
            return ms;
        }
        protected object GetTileLayers2(int tileLevel, int tilePositionX, int tilePositionY)
        {
            string imgFilePath = DZIConstant.GetImgFilePath(tileLevel, tilePositionX, tilePositionY);
            string imgTotalPath = $"{this.SamplePath}\\{DZIConstant.DZIFilesDir}\\{imgFilePath}";

            if (File.Exists(imgTotalPath))
            {
                using Bitmap bmp = (Bitmap)Image.FromFile(imgTotalPath);
                //Bitmap b = (Bitmap)Image.FromFile(imgTotalPath)
                //Bitmap bmp = new Bitmap(b.Width, b.Height);
                //Graphics g = Graphics.FromImage(bmp)
                //g.DrawImage(b, 0, 0, b.Width, b.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                    if (ConfigurationManager.AppSettings["IsDebugMode"] == "true")
                    {
                        g.DrawRectangle(new Pen(Brushes.Red), 0, 0, bmp.Width, bmp.Height);
                        g.DrawString(imgFilePath, new Font("宋体", 20), Brushes.Red, 0, 0);
                    }
                }
                byte[] bytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bytes = ms.ToArray();
                }
                return new MemoryStream(bytes);
            }
            else
            {
                return null;
            }
            //return imgTotalPath;
        }
        /// <summary>
        /// Navmap图像加载地址
        /// </summary>
        /// <param name="imageBasePath"></param>
        /// <returns></returns>
        public Uri GetThumbnailImg(string imageBasePath) => new Uri($"{imageBasePath}/{DZIConstant.DZIFilesDir}/{DZIConstant.NavImgLevel}/{DZIConstant.NavImgName}", UriKind.RelativeOrAbsolute);


    }
}
