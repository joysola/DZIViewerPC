using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.ScaleTileSource;
using Nico.DeepZoom;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.PIMS.Framework.Extensions
{
    public static class MultiScaleTileSourceExtensions
    {
        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="tileSource"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static async Task SelectReportImg(this NineLayerFromMinioScaleTileSource tileSource, int x, int y, int level = 7)
        {
            using (Bitmap bitmap = new Bitmap(256 * 3, 256 * 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    for (int i = -1, ii = 0; i < 2 && ii < 3; i++, ii++)
                    {
                        for (int j = -1, jj = 0; j < 2 && jj < 3; j++, jj++)
                        {
                            using (var stream = await tileSource.GetTileStream(level, x + i, y + j).ConfigureAwait(false))
                            {
                                if (stream != null)
                                {
                                    using (var img = Image.FromStream(stream))
                                    {
                                        g.DrawImage(img, 256 * ii, 256 * jj);
                                    }
                                }
                            }
                        }
                    }
                    Rectangle cropArea = new Rectangle(190, 190, 398, 398); // 剪切矩形
                    using (Bitmap bmpCrop = bitmap.Clone(cropArea, bitmap.PixelFormat)) // 剪切
                    {
                        var dialog = new Microsoft.Win32.SaveFileDialog();
                        dialog.Title = "请选择下载目录";
                        dialog.Filter = "Directory|*.png;*.jpg"; // 只显示png图片
                        dialog.FileName = "切片主图"; // Filename will then be "select.this.directory"
                        if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.FileName))
                        {
                            bmpCrop.Save(dialog.FileName);
                        }
                    }
                }
            }

            //var stream00 = tileSource.GetTileStream(level, x - 1, y - 1);
            //var stream01 = tileSource.GetTileStream(level, x, y - 1);
            //var stream02 = tileSource.GetTileStream(level, x + 1, y - 1);
            //var stream10 = tileSource.GetTileStream(level, x - 1, y);
            //var stream11 = tileSource.GetTileStream(level, x, y);
            //var stream12 = tileSource.GetTileStream(level, x + 1, y);
            //var stream20 = tileSource.GetTileStream(level, x - 1, y + 1);
            //var stream21 = tileSource.GetTileStream(level, x, y + 1);
            //var stream22 = tileSource.GetTileStream(level, x + 1, y + 1);
            //var img00 = Image.FromStream(stream00);
            //var img01 = Image.FromStream(stream01);
            //var img02 = Image.FromStream(stream02);
            //var img10 = Image.FromStream(stream10);
            //var img11 = Image.FromStream(stream11);
            //var img12 = Image.FromStream(stream12);
            //var img20 = Image.FromStream(stream20);
            //var img21 = Image.FromStream(stream21);
            //var img22 = Image.FromStream(stream22);



            //Bitmap bitmap = new Bitmap(256 * 3, 256 * 3);
            //using (Graphics g = Graphics.FromImage(bitmap))
            //{
            //    g.DrawImage(img00, 0, 0);
            //    g.DrawImage(img01, 256, 0);
            //    g.DrawImage(img02, 256 * 2, 0);

            //    g.DrawImage(img00, 0, 256);
            //    g.DrawImage(img01, 256, 256);
            //    g.DrawImage(img02, 256 * 2, 256);

            //    g.DrawImage(img00, 0, 256 * 2);
            //    g.DrawImage(img01, 256, 256 * 2);
            //    g.DrawImage(img02, 256 * 2, 256 * 2);
            //}


        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="tileSource"></param>
        /// <param name="msiPoint"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static async Task SelectReportImg(this NineLayerFromMinioScaleTileSource tileSource, System.Windows.Point msiPoint, int level = 8)
        {
            var x = (int)msiPoint.X / 256;
            var y = (int)msiPoint.Y / 256;
            var ratio = Math.Pow(2, 8) / Math.Pow(2, level);
            var cropAreaX = msiPoint.X - x * 256 + 256 - MSIContext.RprtImgSize / 2;
            var cropAreaY = msiPoint.Y - y * 256 + 256 - MSIContext.RprtImgSize / 2;
            Rectangle cropArea = new Rectangle((int)(cropAreaX / ratio), (int)(cropAreaY / ratio), MSIContext.RprtImgSize, MSIContext.RprtImgSize); // 剪切矩形
            x = x / (int)ratio;
            y = y / (int)ratio;
            using (Bitmap bitmap = new Bitmap(256 * 3, 256 * 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    for (int i = -1, ii = 0; i < 2 && ii < 3; i++, ii++)
                    {
                        for (int j = -1, jj = 0; j < 2 && jj < 3; j++, jj++)
                        {
                            using (var stream = await tileSource.GetTileStream(level, x + i, y + j).ConfigureAwait(false))
                            {
                                if (stream != null)
                                {
                                    using (var img = Image.FromStream(stream))
                                    {
                                        g.DrawImage(img, 256 * ii, 256 * jj);
                                    }
                                }
                            }
                        }
                    }
                    using (Bitmap bmpCrop = bitmap.Clone(cropArea, bitmap.PixelFormat)) // 剪切
                    {
                        var dialog = new Microsoft.Win32.SaveFileDialog();
                        dialog.Title = "请选择下载目录";
                        dialog.Filter = "Directory|*.png;*.jpg"; // 只显示png图片
                        dialog.FileName = "切片主图"; // Filename will then be "select.this.directory"
                        if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.FileName))
                        {
                            bmpCrop.Save(dialog.FileName);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="tileSource"></param>
        /// <param name="msiPoint"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static async Task<byte[]> SelectThumbImg(this NineLayerFromLocalScaleTileSource tileSource, System.Windows.Point msiPoint, int level = 8)
        {
            byte[] result = null;
            var x = (int)msiPoint.X / 256;
            var y = (int)msiPoint.Y / 256;
            var ratio = Math.Pow(2, 8) / Math.Pow(2, level);
            var cropAreaX = msiPoint.X - x * 256 + 256 - MSIContext.ThumbImgSize / 2;
            var cropAreaY = msiPoint.Y - y * 256 + 256 - MSIContext.ThumbImgSize / 2;
            Rectangle cropArea = new Rectangle((int)(cropAreaX / ratio), (int)(cropAreaY / ratio), MSIContext.ThumbImgSize, MSIContext.ThumbImgSize); // 剪切矩形
            x = x / (int)ratio;
            y = y / (int)ratio;
            using (Bitmap bitmap = new Bitmap(256 * 3, 256 * 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    for (int i = -1, ii = 0; i < 2 && ii < 3; i++, ii++)
                    {
                        for (int j = -1, jj = 0; j < 2 && jj < 3; j++, jj++)
                        {
                            using (var stream = await tileSource.GetTileStream(level, x + i, y + j).ConfigureAwait(false))
                            {
                                if (stream != null)
                                {
                                    using (var img = Image.FromStream(stream))
                                    {
                                        g.DrawImage(img, 256 * ii, 256 * jj);
                                    }
                                }
                            }
                        }
                    }
                    using (Bitmap bmpCrop = bitmap.Clone(cropArea, bitmap.PixelFormat)) // 剪切
                    {
                        using (var ms = new MemoryStream())
                        {
                            bmpCrop.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            result = ms.ToArray();
                        }
                    }
                }
            }
            return result;
        }
    }
}
