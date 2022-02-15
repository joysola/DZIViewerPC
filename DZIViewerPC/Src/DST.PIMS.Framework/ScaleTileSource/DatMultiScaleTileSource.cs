using Nico.DeepZoom;
using Nico.TransDat;
using Nico.TransDat.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ScaleTileSource
{
    public class DatMultiScaleTileSource : MultiScaleTileSource
    {
        private int count = 0;
        private string datFullPath = string.Empty;
        private List<TileImage> tileImageList = new List<TileImage>(100000);

        public DatMultiScaleTileSource(string datFullPath) : base()
        {
            DatVersionInfo datVer = TransDatManager.Instance.GetDatVersionInfo(datFullPath);
            this.datFullPath = datFullPath;
            base.InitPar(datVer.Width, datVer.Height, 256, 0);
            this.LoadTileImage();
        }

        public DatMultiScaleTileSource(long imageWidth, long imageHeight, int tileSize, int tileOverlap, string datFullPath) :
            base(imageWidth, imageHeight, tileSize, tileOverlap)
        {
            this.datFullPath = datFullPath;
            this.LoadTileImage();
        }

        private void LoadTileImage()
        {
            // 加载瓦片图
            Task t = Task.Run(() =>
            {
                DateTime start = DateTime.Now;
                tileImageList.AddRange(TransDatManager.Instance.GetAllTileImage(this.datFullPath).OrderBy(x => x.Level).ThenBy(x => x.X).ThenBy(x => x.Y));
                if (tileImageList.FindAll(x => x == null).Count > 0)
                {
                    Console.WriteLine($"tileImageList 包含 null 对象 {tileImageList.FindAll(x => x == null).Count}");
                }
                else
                {
                    Console.WriteLine($"tileImageList 不包含 null 对象 ！");
                }
                DateTime end = DateTime.Now;
                Console.WriteLine($"GetAllTileImage 耗时：{(end - start).TotalMilliseconds}");
            });
            // t.Wait();
        }

        public override void GetTileLayersAngle(ref double CenterX, ref double CenterY, ref double Angle, ref double OffsetX, ref double OffsetY)
        {
        }
        
        protected override object GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY)
        {
            if (tileLevel > 8)
            {
                tileLevel = tileLevel - 8;
                byte[] imgDat = null;
                // 优先加载内存的数据
                if (tileImageList != null)
                {
                    TileImage ti = this.tileImageList.FirstOrDefault(x => x != null && x.Level == tileLevel && x.X == tilePositionX && x.Y == tilePositionY);
                    if(ti != null)
                    {
                        imgDat = ti.ImageByte;
                    }
                }

                // 加载Dat数据
                if (imgDat == null)
                {
                    count++;
                    imgDat = TransDatManager.Instance.GetSingleTileImageFromDat(this.datFullPath, tileLevel, tilePositionX, tilePositionY);
                    Console.WriteLine("count:" + count + "   tileLevel:" + tileLevel);
                }

                if (imgDat != null)
                {
                    Bitmap b = SetByteToImage(imgDat);
                    Bitmap bmp = new Bitmap(b.Width, b.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(b, 0, 0, b.Width, b.Height);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return new MemoryStream(ms.ToArray());
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 二进制转换成图片
        /// </summary>
        public Bitmap SetByteToImage(byte[] mybyte, string targetPath = null)
        {
            using (MemoryStream ms = new MemoryStream(mybyte))
            {
                System.Drawing.Image outputImg = System.Drawing.Image.FromStream(ms);
                if (!string.IsNullOrEmpty(targetPath))
                {
                    outputImg.Save(targetPath);
                }

                return (Bitmap)outputImg;
            }
        }
    }
}
