using Nico.DeepZoom;
using Nico.TransDat.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ScaleTileSource
{
    /// <summary>
    /// 从本地直接读取9层图
    /// </summary>
    public class NineLayerFromLocalScaleTileSource : MultiScaleTileSource, IDisposable
    {
        private string SamplePath { get; set; } = string.Empty;

        public NineLayerFromLocalScaleTileSource(string samplePath) : base()
        {
            this.SamplePath = samplePath;
            base.InitPar((long)(256 * Math.Pow(2, 8)), (long)(256 * Math.Pow(2, 8)), 256, 0);
        }


        public override void GetTileLayersAngle(ref double CenterX, ref double CenterY, ref double Angle, ref double OffsetX, ref double OffsetY)
        {
        }

        protected override object GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY)
        {
            if (tileLevel > 8)
            {
                tileLevel -= 8;
                string imgPath = $"{this.SamplePath}\\{tileLevel}\\{tilePositionX}\\{tilePositionY}.jpg";

                if (File.Exists(imgPath))
                {
                    Bitmap b = (Bitmap)Image.FromFile(imgPath);
                    Bitmap bmp = new Bitmap(b.Width, b.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(b, 0, 0, b.Width, b.Height);
                    if (ConfigurationManager.AppSettings["IsDebugMode"] == "true")
                    {
                        g.DrawRectangle(new Pen(Brushes.Red), 0, 0, b.Width, b.Height);
                        g.DrawString($"{tileLevel}\\{tilePositionX}\\{tilePositionY}.jpg", new Font("宋体", 20), Brushes.Red, 0, 0);
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
            }

            return null;
        }

        public void Dispose()
        {
        }

        /// <summary>
		/// 获取瓦片图流
		/// </summary>
		/// <param name="level"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public async Task<Stream> GetTileStream(int level, int x, int y)
        {
            return await Task.Run(() => (Stream)GetTileLayers(level + 8, x, y)).ConfigureAwait(false);
        }
    }
}
