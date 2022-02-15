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
    /// MoleDiagResultConfirm.xaml 的交互逻辑
    /// </summary>
    public partial class MoleDiagResultConfirm : BaseUserControl
    {
        private MoleDiagResultConfirmViewModel moleDiagResultConfirmViewModel = null;

        public MoleDiagResultConfirm()
        {
            InitializeComponent();
            this.moleDiagResultConfirmViewModel = new MoleDiagResultConfirmViewModel();
            this.DataContext = this.moleDiagResultConfirmViewModel;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
