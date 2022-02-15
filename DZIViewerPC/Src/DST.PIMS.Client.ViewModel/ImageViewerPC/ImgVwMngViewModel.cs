using DST.Common.Extensions;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework.Controls;
using DST.PIMS.Framework.DBContext.ImgAnnoDB;
using DST.PIMS.Framework.Model;
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
using System.Windows.Media;

namespace DST.PIMS.Client.ViewModel
{
    [NotifyAspect]
    public class ImgVwMngViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 主阅片vewimodel
        /// </summary>
        public ImgVwViewModel ImgViewModel { get; } = new ImgVwViewModel();
        /// <summary>
        /// 分屏的第二个阅片viewmodel
        /// </summary>
        public ImgVwViewModel ImgViewModel2 { get; } = new ImgVwViewModel();
        /// <summary>
        /// 分屏状态 由 分屏控件提供
        /// </summary>
        public bool IsSpliting { get; set; }

        public string SamplePath { get; set; }
        /// <summary>
        /// 切片路径集合
        /// </summary>
        public List<ImgViewFileInfo> ImgPathList { get; set; }

        public ImgViewFileInfo CurImgFile { get; set; }


        public ImgVwMngViewModel()
        {
        }

        public override void OnViewLoaded()
        {
            if (this.Args?.Length == 1)
            {
                if (this.Args[0] is List<ImgViewFileInfo> imgPaths && imgPaths.Count > 1)
                {
                    IsSpliting = true;
                    CurImgFile = imgPaths[0];
                    ImgViewModel2.InitScaleTileSource(imgPaths[1]);
                    Messenger.Default.Send(true, EnumMessageKey.SplitImgVm);
                }
                else if (this.Args[0] is ImgViewFileInfo imgViewFile)
                {
                    IsSpliting = false;
                    CurImgFile = imgViewFile;
                    Messenger.Default.Send(false, EnumMessageKey.SplitImgVm);
                }
                ImgViewModel.InitScaleTileSource(CurImgFile);
            }
        }
    }
}
