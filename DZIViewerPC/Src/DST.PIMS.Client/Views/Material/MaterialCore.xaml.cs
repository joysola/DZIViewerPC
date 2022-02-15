using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using DST.PIMS.Client.ViewModel.Test;
using DST.PIMS.Framework.Model;
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
    /// DrawMaterialsStation.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialCore : BaseUserControl
    {
        //public static readonly DependencyProperty MaterialTypeProperty = DependencyProperty.Register(
        //  nameof(MaterialType), typeof(EnumMaterialType), typeof(MaterialCore), new PropertyMetadata(EnumMaterialType.Remain));

        ///// <summary>
        ///// 取材类型 0 待取材 1延缓取材 2 补取
        ///// </summary>
        //public EnumMaterialType MaterialType
        //{
        //    get => (EnumMaterialType)GetValue(MaterialTypeProperty);
        //    set => SetValue(MaterialTypeProperty, value);
        //}

        public MaterialCore()
        {
            InitializeComponent();
        }
    }
}
