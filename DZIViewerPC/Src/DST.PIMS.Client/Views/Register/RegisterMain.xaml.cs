using DST.Controls.Base;
using DST.PIMS.Client.ViewModel.Test;
using DST.PIMS.Framework.Model;
using HandyControl.Controls;
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
using System.Reflection;
using System.ComponentModel;
using DST.PIMS.Client.ViewModel;
using DST.Common.Extensions;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// NormalPathologyRegister.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterMain : BaseUserControl
    {
        ///// <summary>
        ///// 登记枚举字段集合
        ///// </summary>
        //private static readonly List<FieldInfo> EnumRegisterTypeFields = typeof(EnumRegisterType).GetFields().ToList();

        //public static readonly DependencyProperty RegisterTypeProperty = DependencyProperty.Register(
        //  "RegisterType", typeof(EnumRegisterType), typeof(RegisterMain), new PropertyMetadata(EnumRegisterType.Normal,
        //      (d, e) =>
        //      {
        //          // 根据RegisterType 显示不同的状态
        //          var ctl = (RegisterMain)d;
        //          if (e.NewValue is EnumRegisterType rType)
        //          {
        //              switch (rType)
        //              {
        //                  case EnumRegisterType.Cell:
        //                      ctl.samplePart.GastroscopeCB.Visibility = Visibility.Collapsed; // 胃镜checkbox不显示
        //                      break;
        //                  case EnumRegisterType.Molecular:
        //                      ctl.samplePart.GastroscopeCB.Visibility = Visibility.Collapsed; // 胃镜checkbox不显示
        //                      ctl.samplePart.SampleDataGrid.Visibility = Visibility.Collapsed; // 样本表格不显示
        //                      break;
        //                  case EnumRegisterType.InOperQuick:
        //                      ctl.samplePart.SyncGenCB.Visibility = Visibility.Visible; // 同步生成常规病例checkbox显示
        //                      ctl.samplePart.GastroscopeCB.Visibility = Visibility.Collapsed; // 胃镜checkbox不显示
        //                      InfoElement.SetNecessary(ctl.samplePart.DutyDocCB, true); // 责任医生必填
        //                      break;
        //                  case EnumRegisterType.Normal:
        //                  default:
        //                      break;
        //              }
        //              // Description的值提取
        //              ctl.samplePart.Title = EnumRegisterTypeFields?.FirstOrDefault(x => x.Name == rType.ToString())?.GetCustomAttribute<DescriptionAttribute>()?.Description;
        //          }
        //      }));
        ///// <summary>
        ///// 登记类型
        ///// </summary>
        //public EnumRegisterType RegisterType
        //{
        //    get => (EnumRegisterType)GetValue(RegisterTypeProperty);
        //    set => SetValue(RegisterTypeProperty, value);
        //}

        private RegisterMainViewModel registerViewModel;

        public RegisterMain()
        {
            InitializeComponent();
            registerViewModel = new RegisterMainViewModel();
            this.DataContext = registerViewModel;

        }

    }
}
