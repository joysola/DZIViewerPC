using DST.Controls.Base;
using DST.PIMS.Client.ViewModel.Test;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// DrawMaterialSpecimen.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialSpecimen : BaseUserControl
    {
        public MaterialSpecimen()
        {
            InitializeComponent();
            // 更新图像后触发contextmenu的展示和关闭，用以防止窗体冻结
            Messenger.Default.Register<bool>(this, EnumMessageKey.IntendtoRepairFreeze, data =>
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    this.cntM.IsOpen = true;
                    this.cntM.IsOpen = false;
                });
            });
        }

        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }

        //private void Expander_Expanded(object sender, RoutedEventArgs e)
        //{
        //    this.Dispatcher.InvokeAsync(() =>
        //    {
        //        var bindExp = BindingOperations.GetMultiBindingExpression(this.EmbedGrid, HeightProperty); // 获取绑定表达式
        //        bindExp?.UpdateTarget(); // 更新所绑定的属性
        //    });
        //}
    }
}
