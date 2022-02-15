using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Client.ViewModel
{
    public class ProdDoctAdviceViewModel : CustomBaseViewModel
    {
        public ProdDoctAdQueryViewModel DoctAdQueryVM { get; set; } = new ProdDoctAdQueryViewModel();
        public ProdDoctAdSliceViewModel DoctAdSliceVM { get; set; } = new ProdDoctAdSliceViewModel();

        public ProdDoctAdviceViewModel()
        {
            DoctAdQueryVM.SelectedCommandList.Add(DoctAdSliceVM.QueryCommand); // 左侧选中样本，右侧刷新对应数据
            DoctAdSliceVM.ChangeSelectCommand = DoctAdQueryVM.ChangeSelectedCommand; // 右侧更新，左侧选中对应样本
        }
    }
}
