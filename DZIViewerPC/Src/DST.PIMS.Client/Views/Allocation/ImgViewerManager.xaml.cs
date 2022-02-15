using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// ImgViewerManager.xaml 的交互逻辑
    /// </summary>
    public partial class ImgViewerManager : BaseUserControl
    {
        /// <summary>
        /// ImgViewer的Height属性绑定（自身）
        /// </summary>
        private Binding ImgviewSelfHeightBinding { get; } = new Binding { RelativeSource = new RelativeSource { Mode = RelativeSourceMode.Self }, Path = new PropertyPath(nameof(ActualWidth)) };
        /// <summary>
        /// ImgViewer的Height属性绑定（imgsGrid的height）
        /// </summary>
        private Binding ImgviewGridHeightBinding { get; } = new Binding { ElementName = nameof(imgsGrid), Path = new PropertyPath(nameof(ActualHeight)) };
        public ImgViewerManager()
        {
            this.DataContext = new ImgViewerManagerViewModel();
            InitializeComponent();
            // 加载完成后再加载白平衡的shaderEffect
            //this.Loaded += (s, e) => this.Dispatcher.InvokeAsync(() => this.imgViewer.msi.Effect = (ShaderEffect)this.grid.TryFindResource("shader"));
        }

        /// <summary>
        /// 诊断报告单开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgDetailBtn_Click(object sender, RoutedEventArgs e)
        {
            ImgDetailPop.IsOpen = !ImgDetailPop.IsOpen;
        }
        /// <summary>
        /// 报告视野开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectReportImg_Click(object sender, RoutedEventArgs e)
        {
            this.imgViewer.SelectReportImg();
        }
        /// <summary>
        /// 切片列表弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutScanBtn_Click(object sender, RoutedEventArgs e)
        {
            CutScanPop.IsOpen = !CutScanPop.IsOpen;
        }
        /// <summary>
        /// 分屏事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutScanList_StartSplitScreen(object sender, EventArgs e)
        {
            this.lineBorder.Visibility = Visibility.Visible;// 分割线
            Grid.SetColumnSpan(this.imgViewer, 1);
            BindingOperations.SetBinding(this.imgViewer, ImageViewer.HeightProperty, ImgviewSelfHeightBinding); // 设置高度绑定
            if (this.DataContext is ImgViewerManagerViewModel imgVMVM && imgVMVM.SelectedCutScanList?.Count == 2)
            {
                var leftImgPath = imgVMVM.ImgPathList.FirstOrDefault(x => x == imgVMVM.SelectedCutScanList[0].ScanImgUrl);
                var rightImgPath = imgVMVM.ImgPathList.FirstOrDefault(x => x == imgVMVM.SelectedCutScanList[1].ScanImgUrl);
                imgVMVM.ImgViewModel.InitScaleTileSource(leftImgPath, imgVMVM.SelectedCutScanList[0]);
                imgVMVM.ImgViewModel2.InitScaleTileSource(rightImgPath, imgVMVM.SelectedCutScanList[1]);
                this.imgViewer2.Visibility = Visibility.Visible;
                this.CutScanPop.IsOpen = false;
            }
        }
        /// <summary>
        /// 取消分屏事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutScanList_CancelScreen(object sender, EventArgs e)
        {
            this.lineBorder.Visibility = Visibility.Collapsed; // 分割线
            BindingOperations.SetBinding(this.imgViewer, ImageViewer.HeightProperty, ImgviewGridHeightBinding); // 重新设置高度绑定
            this.imgViewer2.Visibility = Visibility.Collapsed;
            Grid.SetColumnSpan(this.imgViewer, 3);
            this.CutScanPop.IsOpen = false;
        }
        /// <summary>
        /// 关闭切片popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosePopups(object sender, MouseButtonEventArgs e)
        {
            this.CutScanPop.IsOpen = false;
        }
    }
}
