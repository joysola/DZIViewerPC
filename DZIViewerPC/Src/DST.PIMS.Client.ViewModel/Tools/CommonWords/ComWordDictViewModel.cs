using DST.ApiClient.Service;
using DST.Common.Extensions;
using DST.Controls.Base;
using DST.Database.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ComWordDictViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 场景
        /// </summary>
        private string SceneCode { get; set; }
        /// <summary>
        /// 查询名称
        /// </summary>
        [Notification]
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// 查询类型
        /// </summary>
        [Notification]
        public string TypeID { get; set; } = string.Empty;
        /// <summary>
        /// 类型字典
        /// </summary>
        [Notification]
        public List<ComWordType> ComWordTypeDict { get; set; } = new List<ComWordType>();
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
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            ComWordList = ComWordService.Instance.GetComWordList(Content, TypeID, SceneCode);
        })).Value;

        /// <summary>
        /// 新增类型
        /// </summary>
        public ICommand AddTypeCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var msg = new ShowContentWindowMessage("CWD_AddType", $"新增{SceneCode}类型");
            msg.DesignWidth = 450;
            msg.DesignHeight = 200;
            msg.Args = new object[] { new ComWordType { SceneCode = this.SceneCode } };
            msg.CallBackCommand = new RelayCommand<ComWordType>(res =>
            {
                if (res != null)
                {
                    var result = false;
                    result = ComWordService.Instance.SaveComWordType(res);
                    if (result)
                    {
                        QueryComWordTypeList();
                    }
                    else
                    {
                        ShowMessageBox($"新增{SceneCode}常用词类型失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
            Messenger.Default.Send(msg);
        })).Value;

        /// <summary>
        /// 新增常用词
        /// </summary>
        public ICommand AddCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            ShowComWord("新增", new ComWordModel { SceneCode = this.SceneCode }, true);
        })).Value;

        /// <summary>
        /// 编辑常用词
        /// </summary>
        public ICommand EditCommand => new Lazy<RelayCommand<ComWordModel>>(() => new RelayCommand<ComWordModel>(data =>
        {
            ShowComWord("编辑", data.DeepCopy(), false);
        })).Value;
        /// <summary>
        /// 编辑常用词
        /// </summary>
        public ICommand DeleteCommand => new Lazy<RelayCommand<ComWordModel>>(() => new RelayCommand<ComWordModel>(data =>
        {
            if (!string.IsNullOrEmpty(data?.ID))
            {
                ShowMessageBox("确定需要删除该常用词吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        var result = ComWordService.Instance.DeleteComWord(data);
                        if (result)
                        {
                            QueryCommand.Execute(null);
                        }
                        else
                        {
                            ShowMessageBox($"删除常用词失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        })).Value;

        /// <summary>
        /// 选择常用词
        /// </summary>
        public ICommand FetchComWordCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(SelectedComWord?.ID))
            {
                this.Result = SelectedComWord;
                this.CloseContentWindow();
            }
        })).Value;


        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1 && this.Args[0] is string code)
            {
                SceneCode = code;
                QueryComWordTypeList(); // 加载类型字典
                QueryCommand.Execute(null); // 查询常用词
            }
        }




        /// <summary>
        /// 查询常用词类型
        /// </summary>
        private void QueryComWordTypeList()
        {
            ComWordTypeDict = ComWordService.Instance.GetComWordTypeList(SceneCode);
        }

        /// <summary>
        /// 显示常用词
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        private void ShowComWord(string title, ComWordModel comWord, bool isAdd)
        {
            var msg = new ShowContentWindowMessage("CWD_Edit", $"{title}{SceneCode}");
            msg.DesignWidth = 400;
            msg.DesignHeight = 350;
            msg.Args = new object[] { comWord, ComWordTypeDict };
            msg.CallBackCommand = new RelayCommand<ComWordModel>(res =>
            {
                if (res != null)
                {
                    var result = false;
                    result = ComWordService.Instance.SaveComWord(res);
                    if (result)
                    {
                        QueryCommand.Execute(null);
                    }
                    else
                    {
                        ShowMessageBox($"{title}{SceneCode}常用词失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
            Messenger.Default.Send(msg);
        }
    }
}
