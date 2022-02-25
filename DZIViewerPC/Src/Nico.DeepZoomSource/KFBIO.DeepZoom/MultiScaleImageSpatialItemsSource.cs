using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Nico.DeepZoom
{
    public class MultiScaleImageSpatialItemsSource : IList, ICollection, IEnumerable, ZoomableCanvas.ISpatialItemsSource
    {
        private const int CacheCapacity = 0;

        private readonly Dictionary<string, BitmapSource> _tileCache = new Dictionary<string, BitmapSource>();
        private readonly Queue<string> _cachedTiles = new Queue<string>(0);

        private readonly ConcurrentDictionary<string, BitmapSource> _conTileSourceCache = new ConcurrentDictionary<string, BitmapSource>();
        private readonly ConcurrentQueue<string> _conTileIdCache = new ConcurrentQueue<string>();


        private readonly MultiScaleTileSource _tileSource;

        private CancellationTokenSource _currentCancellationTokenSource = new CancellationTokenSource();

        private static readonly object CacheLock = new object();

        private int _currentLevel;

        public Rect Extent => new Rect(_tileSource.ImageSize);

        public int CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
            set
            {
                if (value != _currentLevel)
                {
                    _currentCancellationTokenSource.Cancel();
                    _currentCancellationTokenSource = new CancellationTokenSource();
                    _currentLevel = value;
                }
            }
        }

        #region 初版
        //public object this[int i]
        //{
        //    get
        //    {
        //        double CenterX = 0.0;
        //        double CenterY = 0.0;
        //        double Angle = 0.0;
        //        double OffsetX = 0.0;
        //        double OffsetY = 0.0;
        //        _tileSource.GetTileLayersAngle(ref CenterX, ref CenterY, ref Angle, ref OffsetX, ref OffsetY);
        //        _tileSource.CenterX = CenterX;
        //        _tileSource.CenterY = CenterY;
        //        _tileSource.Angle = Angle;
        //        _tileSource.OffsetX = OffsetX;
        //        _tileSource.OffsetY = OffsetY;
        //        Tile tile = _tileSource.TileFromIndex(i);
        //        string tileId = tile.ToString();
        //        //if (_tileCache.ContainsKey(tileId))
        //        //{
        //        //    return new VisualTile(tile, _tileSource, _tileCache[tileId]);
        //        //}
        //        if (_conTileSourceCache.TryGetValue(tileId, out BitmapSource source))
        //        {
        //            return new VisualTile(tile, _tileSource, source);
        //        }
        //        VisualTile tileVm = new VisualTile(tile, _tileSource);
        //        object tileLayers = _tileSource.GetTileLayers(tile.Level, tile.Column, tile.Row);
        //        Uri uri = tileLayers as Uri;
        //        if (uri != null)
        //        {
        //            CancellationToken token = _currentCancellationTokenSource.Token;
        //            Task.Factory.StartNew(delegate
        //            {
        //                BitmapSource bitmapSource = ImageLoader.LoadImage(uri);
        //                if (bitmapSource != null)
        //                {
        //                    bitmapSource = ConCacheTile(tileId, bitmapSource);
        //                }
        //                return bitmapSource;
        //            }, token, TaskCreationOptions.None, TaskScheduler.Default).ContinueWith(delegate (Task<BitmapSource> t)
        //            {
        //                if (t.Result != null)
        //                {
        //                    tileVm.Source = t.Result;
        //                }
        //            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        //        }
        //        else if (tileLayers is string localUrl) // 本地local链接
        //        {
        //            Application.Current.Dispatcher.InvokeAsync(async () =>
        //            {
        //                using (var stream = (Stream)await _tileSource.ReadImgStream(localUrl))
        //                {
        //                    if (stream == null)
        //                    {
        //                        tileVm = null;
        //                        return;
        //                    }
        //                    try
        //                    {
        //                        var bitmapImage = new BitmapImage();
        //                        bitmapImage.BeginInit();
        //                        bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat; // 必须在BeginInit后设置
        //                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //                        bitmapImage.StreamSource = stream;
        //                        bitmapImage.EndInit();
        //                        bitmapImage.Freeze();
        //                        tileVm.Source = bitmapImage;
        //                        ConCacheTile(tileId, bitmapImage);
        //                    }
        //                    catch (Exception)
        //                    {
        //                        tileVm = null;
        //                    }
        //                }
        //                //var stream = (Stream)await _tileSource.ReadImgStream(localUrl/*, tile.Level, tile.Column, tile.Row*/);
        //            });
        //        }
        //        else
        //        {
        //            var stream = tileLayers as Stream;
        //            if (stream == null)
        //            {
        //                return null;
        //            }
        //            using (stream)
        //            {
        //                try
        //                {
        //                    BitmapImage bitmapImage = new BitmapImage();
        //                    bitmapImage.BeginInit();
        //                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat; // 必须在BeginInit后设置
        //                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //                    bitmapImage.StreamSource = stream;
        //                    bitmapImage.EndInit();
        //                    bitmapImage.Freeze();
        //                    tileVm.Source = bitmapImage;
        //                    //stream.Dispose();
        //                    //stream.Close();
        //                }
        //                catch (Exception)
        //                {
        //                    tileVm = null;
        //                }
        //            }
        //        }
        //        return tileVm;
        //    }
        //    set
        //    {
        //    }
        //}
        #endregion

        public object this[int i]
        {
            get
            {
                double CenterX = 0.0;
                double CenterY = 0.0;
                double Angle = 0.0;
                double OffsetX = 0.0;
                double OffsetY = 0.0;
                _tileSource.GetTileLayersAngle(ref CenterX, ref CenterY, ref Angle, ref OffsetX, ref OffsetY);
                _tileSource.CenterX = CenterX;
                _tileSource.CenterY = CenterY;
                _tileSource.Angle = Angle;
                _tileSource.OffsetX = OffsetX;
                _tileSource.OffsetY = OffsetY;
                Tile tile = _tileSource.TileFromIndex(i);
                string tileId = tile.ToString();
                //if (_conTileSourceCache.TryGetValue(tileId, out BitmapSource source))
                //{
                //    return new VisualTile(tile, _tileSource, source);
                //}
                VisualTile tileVm = new VisualTile(tile, _tileSource);
                object tileLayers = _tileSource.GetTileLayers(tile.Level, tile.Column, tile.Row);
                Uri uri = tileLayers as Uri;
                if (uri != null)
                {
                    CancellationToken token = _currentCancellationTokenSource.Token;
                    Task.Factory.StartNew(delegate
                    {
                        BitmapSource bitmapSource = ImageLoader.LoadImage(uri);
                        if (bitmapSource != null)
                        {
                            bitmapSource = ConCacheTile(tileId, bitmapSource);
                        }
                        return bitmapSource;
                    }, token, TaskCreationOptions.None, TaskScheduler.Default).ContinueWith(delegate (Task<BitmapSource> t)
                    {
                        if (t.Result != null)
                        {
                            tileVm.Source = t.Result;
                        }
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);
                }
                else
                {
                    var token = _currentCancellationTokenSource.Token;
                    Application.Current.Dispatcher.Invoke(async () =>
                    {
                        var stream = await _tileSource.GetTileLayersAsync(tile.Level, tile.Column, tile.Row).ConfigureAwait(false);
                        if (stream == null)
                        {
                            tileVm = null;
                            return;
                        }
                        using (stream)
                        {
                            try
                            {
                                var bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat; // 必须在BeginInit后设置
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.StreamSource = stream;
                                bitmapImage.EndInit();
                                bitmapImage.Freeze();
                                //ConCacheTile(tileId, bitmapImage);
                                tileVm.Source = bitmapImage;
                                //stream.Dispose();
                                //stream.Close();
                            }
                            catch (Exception)
                            {
                                tileVm = null;
                            }
                        }

                    }, DispatcherPriority.Normal, token);

                }
                return tileVm;
            }
            set
            {
            }
        }

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => true;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => null;

        int ICollection.Count => int.MaxValue;

        public event EventHandler ExtentChanged;

        public event EventHandler QueryInvalidated;

        public MultiScaleImageSpatialItemsSource(MultiScaleTileSource tileSource)
        {
            _tileSource = tileSource;
        }

        public void InvalidateSource()
        {
            if (this.ExtentChanged != null)
            {
                this.ExtentChanged(this, EventArgs.Empty);
            }
            if (this.QueryInvalidated != null)
            {
                this.QueryInvalidated(this, EventArgs.Empty);
            }
        }

        public IEnumerable<int> Query(Rect rectangle)
        {
            return from t in _tileSource.VisibleTilesUntilFill(rectangle, CurrentLevel)
                   select _tileSource.GetTileIndex(t);
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        private BitmapSource CacheTile(string tileId, BitmapSource source)
        {
            if (_tileCache.ContainsKey(tileId))
            {
                return _tileCache[tileId];
            }
            lock (CacheLock)
            {
                if (_cachedTiles.Count >= 0)
                {
                    _tileCache.Remove(_cachedTiles.Dequeue());
                }
                _cachedTiles.Enqueue(tileId);
                _tileCache.Add(tileId, source);
                return source;
            }
        }
        /// <summary>
        /// 并发缓存
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private BitmapSource ConCacheTile(string tileId, BitmapSource source)
        {
            if (_conTileSourceCache.TryGetValue(tileId, out BitmapSource bitmapSource))
            {
                return bitmapSource;
            }
            else if (source != null)
            {
                if (_conTileIdCache.Count > 32)
                {
                    if (_conTileIdCache.TryDequeue(out string id))
                    {
                        _conTileSourceCache.TryRemove(id, out BitmapSource removeSource);
                    }
                }
                _conTileIdCache.Enqueue(tileId);
                _conTileSourceCache.TryAdd(tileId, source);
            }
            return source;
        }


        int IList.Add(object value)
        {
            return 0;
        }

        void IList.Clear()
        {
        }

        bool IList.Contains(object value)
        {
            return false;
        }

        int IList.IndexOf(object value)
        {
            return 0;
        }

        void IList.Insert(int index, object value)
        {
        }

        void IList.Remove(object value)
        {
        }

        void IList.RemoveAt(int index)
        {
        }

        void ICollection.CopyTo(Array array, int index)
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield break;
        }
    }
}
