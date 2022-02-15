using DST.ApiClient.Service;
using DST.Controls.Base;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class EmbeddingMainViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 查询样本列表
        /// </summary>
        public QueryPathListViewModel QueryPathListVM { get; set; } = new QueryPathListViewModel(EmbedService.Instance.GetEmbedList);
        /// <summary>
        /// 包埋盒
        /// </summary>
        public EmbedSpecimenViewModel EmbedSpecimenVM { get; set; } = new EmbedSpecimenViewModel();

        /// <summary>
        /// 查询申请单
        /// </summary>
        public ICommand AppViewCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var code = QueryPathListVM?.SelectedModel?.LabCode;
            if (!string.IsNullOrEmpty(code))
            {
                var msg = new ShowContentWindowMessage("AppImgView", $"申请单查看");
                msg.DesignWidth = 1200;
                msg.DesignHeight = 900;
                msg.Args = new object[] { code };
                Messenger.Default.Send(msg);
            }

        })).Value;

        public EmbeddingMainViewModel()
        {
            //this.StartHsjPrint();
            QueryPathListVM.SelectedCommandList.Add(EmbedSpecimenVM.QueryCommand); // 左侧选中样本，右侧刷新对应包埋盒数据
            EmbedSpecimenVM.ChangeSelectCommand = QueryPathListVM.ChangeSelectedCommand; // 右侧扫完包埋盒，左侧选中对应样本
        }

        /// <summary>
        /// 查看申请单
        /// </summary>
        protected override void ViewAppFrm()
        {
            AppViewCommand.Execute(null);
        }
        /// <summary>
        /// 查询包埋
        /// </summary>
        protected override void QueryEmbed()
        {

        }
        /// <summary>
        /// 上一列
        /// </summary>
        protected override void PreviousOne()
        {
            MoveSelectSamp(-1);
        }

        /// <summary>
        /// 下一例
        /// </summary>
        protected override void NextOne()
        {
            MoveSelectSamp(1);
        }
        /// <summary>
        /// 移动选择的位置
        /// </summary>
        /// <param name="step"></param>
        private void MoveSelectSamp(int step)
        {
            var list = QueryPathListVM?.SampleList;
            if (list?.Count > 0)
            {
                var current = QueryPathListVM?.SelectedModel;
                if (current != null)
                {
                    var curIndex = list.IndexOf(current); // 当前位置
                    int moveIndex = curIndex + step; // 下一步移动的位置
                    if (curIndex == 0 && moveIndex < 0) // 上一例到顶部，直接去最后
                    {
                        moveIndex = list.Count - 1;
                    }
                    if (curIndex == list.Count - 1 && moveIndex > list.Count - 1) // 下一例到底部，直接去顶部
                    {
                        moveIndex = 0;
                    }
                    QueryPathListVM.SelectedModel = list.ElementAt(moveIndex);
                }
                else
                {
                    QueryPathListVM.SelectedModel = list[0];
                }
            }
        }
    }
}
