using DST.Common.Extensions;
using DST.Common.Helper;
using DST.PIMS.Framework.Helper;
using Nico.DeepZoom;
using Nico.TransDat.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.ScaleTileSource
{
    /// <summary>
    /// 读取minio上的9层图
    /// </summary>
    public class NineLayerFromMinioScaleTileSource : MultiScaleTileSource, IDisposable
    {

        private string imageBaseUrl = string.Empty;
        private long imageWidth;
        private long imageHeight;
        private List<TileImage> tileImageList = new List<TileImage>(100000);
        private bool isDispose = false;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        public bool HasLoadFinish = false;

        public NineLayerFromMinioScaleTileSource(string imageBaseUrl) : base()
        {
            this.imageBaseUrl = imageBaseUrl;
            this.LoadSlide();
            this.imageWidth = (long)(256 * Math.Pow(2, 8));
            this.imageHeight = (long)(256 * Math.Pow(2, 8));
            base.InitPar(this.imageWidth, this.imageHeight, 256, 0);
            // this.LoadTileImage();
            this.LoadImages2().NoWarning();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageBaseUrl">图片主地址</param>
        /// <param name="points">AI标记集合</param>
        public NineLayerFromMinioScaleTileSource(string imageBaseUrl, List<System.Windows.Point> points) : base()
        {
            this.imageBaseUrl = imageBaseUrl;
            this.LoadSlide();
            this.imageWidth = (long)(256 * Math.Pow(2, 8));
            this.imageHeight = (long)(256 * Math.Pow(2, 8));
            base.InitPar(this.imageWidth, this.imageHeight, 256, 0);
            // this.LoadTileImage();
            this.LoadImages2(points).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task LoadImages()
        {
            DateTime start = DateTime.Now;
            int maxLoadTaskCount = 50;
            int split = this.tileImageList.Count % maxLoadTaskCount == 0 ? this.tileImageList.Count / maxLoadTaskCount : this.tileImageList.Count / maxLoadTaskCount + 1;
            var taskList = new List<Task>();
            for (int i = 0; i < maxLoadTaskCount; i++)
            {
                List<TileImage> tempTileMsgList = this.tileImageList.Skip(i * split).Take(split).ToList();
                var task = Task.Run(() =>
                {
                    try
                    {
                        for (int j = 0; j < tempTileMsgList.Count; j++)
                        {
                            if (this.isDispose)
                            {
                                break;
                            }
                            string url = this.imageBaseUrl + $@"/{tempTileMsgList[j].Level}/{tempTileMsgList[j].X}/{tempTileMsgList[j].Y}.jpg";
                            tempTileMsgList[j].ImageByte = this.DownFile(url).Result;
                        }
                    }
                    catch
                    {
                    }
                });

                taskList.Add(task);
            }

            await Task.WhenAll(taskList);
            DateTime end = DateTime.Now;
            Console.WriteLine("LoadImages 耗时：" + (end - start).TotalMilliseconds);
            this.HasLoadFinish = true;
        }

        public async Task LoadImages2()
        {
            DateTime start = DateTime.Now;
            int maxLoadTaskCount = 20;
            int split = this.tileImageList.Count % maxLoadTaskCount == 0 ? this.tileImageList.Count / maxLoadTaskCount : this.tileImageList.Count / maxLoadTaskCount + 1;
            //var level8 = this.tileImageList.Where(x => x.Level == 8).ToList();
            var levelHeigh = this.tileImageList.Where(x => x.Level == 7).ToList();
            var levelNormal = this.tileImageList.Where(x => x.Level == 1 || x.Level == 2 || x.Level == 3 || x.Level == 4).ToList();
            var levelLow = this.tileImageList.Except(levelHeigh).Except(levelNormal).ToList();
            var baseUrl = this.imageBaseUrl;
            Task.Run(async () =>
            {

                for (int j = 0; j < levelHeigh.Count; j++)
                {
                    if (this.isDispose)
                    {
                        break;
                    }

                    await LoadTileImg(levelHeigh[j], this.imageBaseUrl).ConfigureAwait(false);
                }
            }).NoWarning();

            Task.Run(async () =>
            {
                for (int j = 0; j < levelNormal.Count; j++)
                {
                    await Task.Yield();
                    if (this.isDispose)
                    {
                        break;
                    }

                    await LoadTileImg(levelNormal[j], this.imageBaseUrl).ConfigureAwait(false);
                }
            }).NoWarning();

            Task.Run(async () =>
            {
                for (int j = 0; j < levelLow.Count; j++)
                {
                    await Task.Yield();
                    await Task.Yield();
                    await Task.Yield();
                    if (this.isDispose)
                    {
                        break;
                    }

                    await LoadTileImg(levelLow[j], this.imageBaseUrl).ConfigureAwait(false);
                }
            }).NoWarning();

            DateTime end = DateTime.Now;
            Console.WriteLine("LoadImages 耗时：" + (end - start).TotalMilliseconds);
            this.HasLoadFinish = true;
        }

        public async Task LoadImages2(List<System.Windows.Point> points)
        {
            DateTime start = DateTime.Now;
            var level8 = this.tileImageList.Where(x => x.Level == 8).ToList();
            var level7 = this.tileImageList.Where(x => x.Level == 7).ToList();
            var tmpTilelist = new List<System.Windows.Point>();
            // 获取临近的图片坐标
            var count = 7; // 取的瓦片横(纵)轴个数
            var strat = -count / 2;
            points?.ForEach(p =>
            {
                for (int i = strat; i < count; i++)
                {
                    for (int j = strat; j < count; j++)
                    {
                        var x = (int)p.X / 256;
                        var y = (int)p.Y / 256;
                        if (x + i < 0 || y + j < 0)
                        {
                            continue;
                        }
                        tmpTilelist.Add(new System.Windows.Point(x + i, y + j));
                    }
                }
            });
            var levelSupreme8 = level8.Join(tmpTilelist, tile => new System.Windows.Point(tile.X, tile.Y), p => p, (tile, p) => tile).ToList();
            var levelSupreme7 = level7.Join(tmpTilelist, tile => new System.Windows.Point(tile.X, tile.Y), p => p, (tile, p) => tile).ToList();

            var levelHeigh = level7.Except(levelSupreme7).ToList();
            var levelNormal = this.tileImageList.Where(x => x.Level == 1 || x.Level == 2 || x.Level == 3 || x.Level == 4).ToList();
            var levelLow = this.tileImageList.Except(levelHeigh).Except(levelNormal).Except(levelSupreme8).Except(levelSupreme7).ToList();

            var token = tokenSource.Token; // 取消令牌

            var taskSupreme8 = SplitLoadTileImgTasks(levelSupreme8, 15);
            var taskSupreme7 = SplitLoadTileImgTasks(levelSupreme7, 15);
            var taskNormal = SplitLoadTileImgTasks(levelNormal, 15);
            var taskHeigh = LoadTileImgTask(levelHeigh, 1, token);
            var taskLow = LoadTileImgTask(levelLow, 3, token);
            await Task.WhenAll(taskNormal).ConfigureAwait(false); // 优先加载1-4层，用于进入时展示
            await Task.WhenAll(taskSupreme7).ConfigureAwait(false); // ai标记框附近7-8层图片加载
            await Task.WhenAll(taskSupreme8).ConfigureAwait(false);
            //var tasks = new List<Task>();
            //tasks.AddRange(taskHeigh);
            //tasks.AddRange(taskLow);
            DateTime end = DateTime.Now;
            Console.WriteLine("LoadImages 耗时：" + (end - start).TotalMilliseconds);
            this.HasLoadFinish = true;
        }
        /// <summary>
        /// 单张图异步下载
        /// </summary>
        /// <param name="tileImages"></param>
        /// <param name="priority">优先级越大越小</param>
        /// <returns></returns>
        private Task LoadTileImgTask(List<TileImage> tileImages, int priority, CancellationToken token)
        {
            var task = Task.Run(async () =>
            {
                for (int j = 0; j < tileImages?.Count; j++)
                {
                    token.ThrowIfCancellationRequested(); // 取消时退出执行（会抛出异常）
                    if (this.isDispose)
                    {
                        break;
                    }
                    for (int k = 0; k < priority; k++)
                    {
                        await Task.Yield();
                    }
                    await LoadTileImg(tileImages[j], this.imageBaseUrl).ConfigureAwait(false);
                }
            }, token);
            return task;
        }
        /// <summary>
        /// 分批次多图下载
        /// </summary>
        /// <param name="tileImages"></param>
        /// <param name="maxTaskCount"></param>
        /// <returns></returns>
        private List<Task> SplitLoadTileImgTasks(List<TileImage> tileImages, int maxTaskCount)
        {
            int maxLoadTaskCount = 20;
            int split = tileImages.Count % maxLoadTaskCount == 0 ? tileImages.Count / maxLoadTaskCount : tileImages.Count / maxLoadTaskCount + 1;
            var tasks = new List<Task>();
            for (int i = 0; i < maxLoadTaskCount; i++)
            {
                List<TileImage> tempTileMsgList = tileImages.Skip(i * split).Take(split).ToList();

                var task = Task.Run(async () =>
                {
                    for (int j = 0; j < tempTileMsgList.Count; j++)
                    {
                        if (this.isDispose)
                        {
                            break;
                        }

                        await LoadTileImg(tempTileMsgList[j], this.imageBaseUrl).ConfigureAwait(false);
                    }
                });
                tasks.Add(task);
            }
            return tasks;
        }

        /// <summary>
        /// 下载瓦片图
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        private async Task LoadTileImg(TileImage tile, string baseUrl)
        {
            var url = $"{baseUrl}/{tile.Level}/{tile.X}/{tile.Y}.jpg";
            byte[] bytes = null;
            bytes = await HttpClientHelper.DownFile(url).ConfigureAwait(false);
            tile.ImageByte = bytes;
        }

        private async Task LoadTileImage()
        {
            // 加载瓦片图
            Task t = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < this.tileImageList?.Count; i++)
                    {
                        string url = this.imageBaseUrl + $@"/{this.tileImageList[i].Level}/{this.tileImageList[i].X}/{this.tileImageList[i].Y}.jpg";
                        this.tileImageList[i].ImageByte = this.DownFile(url).Result;
                        if (this.isDispose)
                        {
                            break;
                        }
                    }
                }
                catch
                {
                }
            });

            await t;
        }

        public override void GetTileLayersAngle(ref double CenterX, ref double CenterY, ref double Angle, ref double OffsetX, ref double OffsetY)
        {
        }

        protected override object GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY)
        {
            if (tileLevel > 8)
            {
                tileLevel = tileLevel - 8;
                try
                {
                    //return new Uri(this.imageBaseUrl + $@"/{tileLevel}/{tilePositionX}/{tilePositionY}.jpg");
                    TileImage tileImg = this.tileImageList.FirstOrDefault(img => img.Level == tileLevel && img.X == tilePositionX && img.Y == tilePositionY);
                    //if(tileImg == null || tileImg.ImageByte == null)
                    //{
                    //    string url = this.imageBaseUrl + $@"/{tileLevel}/{tilePositionX}/{tilePositionY}.jpg";
                    //    tileImg = new TileImage() { Level = tileLevel, X = tilePositionX, Y = tilePositionY };
                    //    tileImg.ImageByte = this.DownFile(url).Result;
                    //    return null;
                    //}

                    if (tileImg == null || tileImg.ImageByte == null)
                    {
                        return null;
                    }

                    Bitmap b = SetByteToImage(tileImg.ImageByte);
                    Bitmap bmp = new Bitmap(b.Width, b.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(b, 0, 0, b.Width, b.Height);
                    g.DrawRectangle(new Pen(Brushes.Red), 0, 0, 256, 256);
                    g.DrawString($"{tileLevel}/{tilePositionX}/{tilePositionY}", new Font("宋体", 18), Brushes.Red, 0, 0);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        b.Dispose();
                        bmp.Dispose();
                        b = null;
                        bmp = null;
                        return new MemoryStream(ms.ToArray());
                    }

                }
                catch (Exception e)
                {

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

        private void LoadSlide()
        {
            string file = $@"{Environment.CurrentDirectory}\Slide.dat";
            // https://image01-sz.deepsight.cloud/images/cycle/20210727/B21072500306C101/Slide.dat
            byte[] bytes = HttpClientHelper.DownFile(this.imageBaseUrl + @"/Slide.dat").ConfigureAwait(false).GetAwaiter().GetResult();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(fs);
                fs.Close();
            }

            if (File.Exists(file))
            {
                string section = IniHelper.CreateInstance(file).GetAllSections().FirstOrDefault(x => x.Equals("LayerMag"));
                if (!string.IsNullOrEmpty(section))
                {
                    List<string> keyList = IniHelper.CreateInstance(file).GetSectionAllKeys(section);

                    for (int i = 0; i < keyList.Count; i += 2)
                    {
                        int level = i / 2 + 1;
                        string maxColKey = keyList[i];
                        string maxRowKey = keyList[i + 1];
                        string maxRowValue = IniHelper.CreateInstance(file).IniReadValue(section, maxRowKey);
                        string maxColValue = IniHelper.CreateInstance(file).IniReadValue(section, maxColKey);
                        int curMaxRow = (int)Math.Ceiling(double.Parse(maxRowValue));
                        int curMaxCol = (int)Math.Ceiling(double.Parse(maxColValue));

                        for (int col = 0; col < curMaxCol; col++)
                        {
                            for (int row = 0; row < curMaxRow; row++)
                            {
                                this.tileImageList.Add(new TileImage() { Level = level, X = col, Y = row });
                            }
                        }

                        // 最后一层
                        if (i == keyList.Count - 2)
                        {
                            this.imageWidth = curMaxCol * 256;
                            this.imageHeight = curMaxRow * 256;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 下载文件，获取二进制流
        /// </summary>
        /// <param name="url"></param>
        private async Task<byte[]> DownFile(string url)
        {
            //DateTime start = DateTime.Now;
            byte[] bytes = null;
            //int? imgSize = 0;
            try
            {
                using (WebClient client = new WebClient())
                {
                    WebProxy proxy = new WebProxy();
                    client.Proxy = proxy;
                    bytes = client.DownloadData(url);
                }

                //imgSize = bytes?.Length;
            }
            catch (Exception e)
            {
            }

            //DateTime end = DateTime.Now;
            //Console.WriteLine($"DownFile:{url}  耗时：{(end - start).TotalMilliseconds}   ---    size：{ imgSize }");
            return bytes;
        }

        public void Dispose()
        {
            try
            {
                tokenSource.Cancel();
                this.isDispose = true;
                this.imageBaseUrl = null;
                this.tileImageList.ForEach(x =>
                {
                    x.ImageByte = null;
                });

                this.tileImageList.Clear();
                this.tileImageList = null;
                GC.Collect();
                GC.Collect();
                GC.Collect();
            }
            catch
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 获取瓦片图流
        /// </summary>
        /// <param name="level"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public async Task<Stream> GetTileStream(int level, int x, int y) => await Task.Run(() => (Stream)GetTileLayers(level + 8, x, y)).ConfigureAwait(false);


    }
}
