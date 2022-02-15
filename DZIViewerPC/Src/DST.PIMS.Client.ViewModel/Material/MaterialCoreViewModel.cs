using DST.ApiClient.Service;
using DST.Database.Model;
using DST.PIMS.Framework.Model;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.PIMS.Client.ViewModel
{
    public class MaterialCoreViewModel : CustomBaseViewModel
    {
        private EnumMaterialType _materialType;
        public QueryPathListViewModel QueryPathListVM { get; set; } = new QueryPathListViewModel(MaterialService.Instance.GetMaterialList);
        public QueryPathTitleViewModel QueryPathTitleVM { get; set; } = new QueryPathTitleViewModel();
        public MaterialSpecViewModel MaterialSpecVM { get; set; } = new MaterialSpecViewModel();
        public MaterialToolsViewModel MaterialToolsVM { get; set; } = new MaterialToolsViewModel();

        /// <summary>
        /// 取材类型
        /// </summary>
        public EnumMaterialType MaterialType
        {
            get => _materialType;
            set
            {
                _materialType = value;
                MaterialSpecVM.MaterialType = _materialType; // 将取材类型送给样本部位viewmodel
                ResetData();
            }
        }


        public MaterialCoreViewModel()
        {
            MaterialToolsVM.FetchComWordCommand = MaterialSpecVM.ChangeSampTissEyeCommand; // 双击常用词改变肉眼所见
            QueryPathListVM.SelectedCommandList.Add(MaterialSpecVM.QueryAllCommand); // 选择左侧样本后，查询对应组织和部位信息
            QueryPathListVM.SelectedCommandList.Add(QueryPathTitleVM.ChangePathModelCommand); // 选择左侧样本后，刷新标题栏信息
            MaterialSpecVM.QueryPathList = QueryPathListVM.QueryCommand; // 样本列表查询 赋值 给送检部位用于更新
            MaterialSpecVM.ChangeSelectCommand = QueryPathListVM.ChangeSelectedCommand; // 右侧扫完包埋盒，左侧选中对应样本
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        private void ResetData()
        {
            MaterialSpecVM.SampTiss = new SampTissModel();
            MaterialSpecVM.SelectedSamp = null;
            MaterialSpecVM.SelectedSpec = null;
            MaterialSpecVM.EmbedBoxList.Clear();
            MaterialSpecVM.SpecList.Clear();
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MaterialSpecVM.MaterialTissImgVM.CurBitmapFrame = null;
                MaterialSpecVM.MaterialTissImgVM.ThumbnailList?.Clear();
            });


            QueryPathListVM.QueryModel.DrawMaterialsType = $"{(int)MaterialType}";
            QueryPathTitleVM.PathModel = null;
            QueryPathListVM.SelectedModel = null;
            QueryPathListVM.QueryCommand.Execute(null);

        }
    }
}
