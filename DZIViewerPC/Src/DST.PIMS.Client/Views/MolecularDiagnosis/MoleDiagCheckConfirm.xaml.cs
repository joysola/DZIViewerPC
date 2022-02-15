﻿using DST.Controls.Base;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// MoleDiagCheckConfirm.xaml 的交互逻辑
    /// </summary>
    public partial class MoleDiagCheckConfirm : BaseUserControl
    {
        private MoleDiagCheckConfirmViewModel moleDiagCheckConfirmViewModel = null;

        public MoleDiagCheckConfirm()
        {
            InitializeComponent();
            this.moleDiagCheckConfirmViewModel = new MoleDiagCheckConfirmViewModel();
            this.DataContext = this.moleDiagCheckConfirmViewModel;
        }
    }
}
