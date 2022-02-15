using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class MaterialToolsViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 常用词搜索条件
        /// </summary>
        [Notification]
        public string QueryStr { get; set; }
        /// <summary>
        /// 常用词集合
        /// </summary>
        [Notification]
        public List<ComWordModel> ComWordList { get; set; } = new List<ComWordModel>();
        /// <summary>
        /// 选中的常用词
        /// </summary>
        [Notification]
        public ComWordModel SelectedComWord { get; set; }
        /// <summary>
        /// 搜索
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                ComWordList = await Task.Run(() => ComWordService.Instance.GetComWordList(QueryStr, string.Empty, DSTCode.MaterialCode));
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        })).Value;
        /// <summary>
        /// 新增常用词
        /// </summary>
        public ICommand AddCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            ShowComWord("新增", new ComWordModel { SceneCode = DSTCode.MaterialCode }, true);
        })).Value;

        /// <summary>
        /// 编辑常用词
        /// </summary>
        public ICommand EditCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (SelectedComWord != null)
            {
                ShowComWord("编辑", SelectedComWord, false);
            }
        })).Value;

        /// <summary>
        /// 编辑常用词
        /// </summary>
        public ICommand DeleteCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(SelectedComWord.ID))
            {
                ShowMessageBox("确定需要删除该常用词吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, async res =>
                 {
                     if (res == MessageBoxResult.OK)
                     {
                         try
                         {
                             WhirlingControlManager.ShowWaitingForm();
                             var result = await Task.Run(() => ComWordService.Instance.DeleteComWord(SelectedComWord));
                             if (result)
                             {
                                 QueryCommand.Execute(null);
                             }
                             else
                             {
                                 ShowMessageBox($"删除常用词失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                             }
                         }
                         finally
                         {
                             WhirlingControlManager.CloseWaitingForm();
                         }
                     }
                 });
            }
        })).Value;

        /// <summary>
        /// 双击获取常用词
        /// </summary>
        public ICommand FetchComWordCommand { get; set; }


        public MaterialToolsViewModel()
        {
            QueryCommand.Execute(null);
        }
        /// <summary>
        /// 显示常用词
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        private void ShowComWord(string title, ComWordModel comWord, bool isAdd)
        {
            var msg = new ShowContentWindowMessage("CWD_SimpleEdit", $"{title}{DSTCode.MaterialCode}");
            msg.DesignWidth = 400;
            msg.DesignHeight = 250;
            msg.Args = new object[] { comWord };
            msg.CallBackCommand = new RelayCommand<ComWordModel>(async res =>
            {
                if (res != null)
                {
                    var result = false;
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        result = await Task.Run(() => ComWordService.Instance.SaveComWord(res));
                        if (result)
                        {
                            QueryCommand.Execute(null);
                        }
                        else
                        {
                            ShowMessageBox($"{title}{DSTCode.MaterialCode}常用词失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
            Messenger.Default.Send(msg);
        }
    }
}
