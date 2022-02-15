using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework.Controls;
using DST.PIMS.Framework.DBContext.ImgAnnoDB;
using DST.PIMS.Framework.Model;
using DST.PIMS.Framework.ScaleTileSource;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    [NotifyAspect]
    public class ImgVwViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageBasePath { get; set; }

        public ImgViewFileInfo CurImgViewFile { get; set; }
        /// <summary>
        /// 导航缩略图
        /// </summary>

        public Uri ImgThumbnail { get; set; }
        /// <summary>
        /// 标记信息源
        /// </summary>
        public AnnoInfo AnnoInfos { get; set; }
        /// <summary>
        /// 标记集合
        /// </summary>
        public ObservableCollection<Img_Anno> Annos { get; set; } = new ObservableCollection<Img_Anno>();

        /// <summary>
        /// AI标记集合
        /// </summary>
        //public List<AIFeature> AIFigs => CuttingScan?.AIFeatureList;
        /// <summary>
        /// 核心瓦片图源
        /// </summary>
        public DZILayerFromLocalScaleTileSource CurTileSource { get; set; }
        /// <summary>
        /// 保存
        /// </summary>
        public ICommand SaveCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            ShowMessageBox("确定保存吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var result = ImgAnnoDB.Instance.SaveAnnoList(AnnoInfos?.AnnoList, Annos, CurImgViewFile.DicectoryName);
                        if (result)
                        {
                            this.GetImgAnnos(CurImgViewFile.DicectoryName);
                            Messenger.Default.Send<object>(null, EnumMessageKey.RefreshImgViewerAnnos); // 刷新标记数据
                            ShowMessageBox("保存成功！", MessageBoxButton.OK, MessageBoxImage.Information, null, true);
                        }
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
        })).Value;
        /// <summary>
        /// 编辑标记
        /// </summary>
        public ICommand EditCommand => new Lazy<RelayCommand<AnnoBase>>(() => new RelayCommand<AnnoBase>(data =>
        {
            if (data != null)
            {
                var msg = new ShowContentWindowMessage("AnnoEdit", "编辑");
                msg.DesignHeight = 350;
                msg.DesignWidth = 400;
                msg.Args = new object[] { new AnnoEditModel { Description = data.Description, Anno_Name = data.Anno_Name } };
                msg.CallBackCommand = new RelayCommand<AnnoEditModel>(res =>
                {
                    if (res != null)
                    {
                        data.Description = res.Description;
                        data.Anno_Name = res.Anno_Name;
                    }
                    else
                    {
                        data.IsCanceled = true; // 取消
                    }
                });
                Messenger.Default.Send(msg);
            }
        })).Value;

        public ImgVwViewModel() { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="imageBasePath"></param>
        /// <param name="cuttingScan"></param>
        public void InitScaleTileSource(ImgViewFileInfo imgViewFile)
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                CurImgViewFile = imgViewFile;
                ImageBasePath = imgViewFile.LocalFilePath;
                GetImgAnnos(CurImgViewFile.DicectoryName);
                if (!string.IsNullOrEmpty(ImageBasePath))
                {
                    this.CurTileSource?.Dispose();
                    //var points = AIFigs?.Select(x => x.CenterPoint2).ToList(); // 获取标记中心集合
                    this.CurTileSource = new DZILayerFromLocalScaleTileSource(ImageBasePath);
                    ImgThumbnail = this.CurTileSource.GetThumbnailImg(ImageBasePath);
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
        /// 获取坐标集合
        /// </summary>
        /// <param name="sampleId"></param>
        public void GetImgAnnos(string sampleId)
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                var result = ImgAnnoDB.Instance.GetImgAnnoListbySampleId(sampleId);
                Annos = new ObservableCollection<Img_Anno>(result);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

    }
}
