using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework.Controls;
using DST.PIMS.Framework.ScaleTileSource;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DST.PIMS.Client.ViewModel
{
    [NotifyAspect]
    public class ImageViewerViewModel : CustomBaseViewModel
    {

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageBasePath { get; set; }

        /// <summary>
        /// 导航缩略图
        /// </summary>

        public Uri ImgThumbnail { get; set; }
        /// <summary>
        /// 相关标记数据
        /// </summary>
        public CuttingScan CuttingScan { get; set; }
        /// <summary>
        /// 标记信息源
        /// </summary>
        public AnnoInfo AnnoInfos { get; set; }
        /// <summary>
        /// AI标记集合
        /// </summary>
        public List<AIFeature> AIFigs => CuttingScan?.AIFeatureList;
        /// <summary>
        /// 核心瓦片图源
        /// </summary>
        public NineLayerFromMinioScaleTileSource CurTileSource { get; set; }

        public ImageViewerViewModel() { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="imageBasePath"></param>
        /// <param name="cuttingScan"></param>
        public void InitScaleTileSource(string imageBasePath, CuttingScan cuttingScan)
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                ImageBasePath = imageBasePath;
                CuttingScan = cuttingScan;

                Console.WriteLine(ImageBasePath);
                if (!string.IsNullOrEmpty(ImageBasePath))
                {
                    this.CurTileSource?.Dispose();
                    var points = AIFigs?.Select(x => x.CenterPoint2).ToList(); // 获取标记中心集合
                    this.CurTileSource = new NineLayerFromMinioScaleTileSource(ImageBasePath, points);
                    ImgThumbnail = GetThumbnailImg(ImageBasePath);
                }
                else
                {
                    ShowMessageBox("阅片地址为空！", MessageBoxButton.OK, MessageBoxImage.Error);
                    //throw new Exception("阅片地址为空！");
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

        /// <summary>
        /// Navmap图像加载地址
        /// </summary>
        /// <param name="imageBasePath"></param>
        /// <returns></returns>
        private Uri GetThumbnailImg(string imageBasePath) => new Uri($"{imageBasePath}/1/0/0.jpg", UriKind.RelativeOrAbsolute);

    }
}
