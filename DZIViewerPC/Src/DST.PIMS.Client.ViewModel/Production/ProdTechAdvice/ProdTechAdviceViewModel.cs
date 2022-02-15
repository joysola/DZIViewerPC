using DST.ApiClient.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Client.ViewModel
{
    public class ProdTechAdviceViewModel : CustomBaseViewModel
    {
        public ProdTechAdQueryViewModel TechAdQueryVM { get; set; } = new ProdTechAdQueryViewModel();
        public ProdTechAdSliceViewModel TechAdSliceVM { get; set; } = new ProdTechAdSliceViewModel();

        public ProdTechAdviceViewModel()
        {
            TechAdQueryVM.SelectedCommandList.Add(TechAdSliceVM.QueryCommand); // 左侧选中样本，右侧刷新对应数据
            TechAdSliceVM.ChangeSelectCommand = TechAdQueryVM.ChangeSelectedCommand; // 右侧更新，左侧选中对应样本
        }
    }
}
