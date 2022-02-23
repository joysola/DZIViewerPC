using DST.Common.Extensions;
using DST.Common.Helper;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    [NotifyAspect]
    public class ImageViewerPCViewModel : CustomBaseViewModel
    {
        private readonly string directoryPath = nameof(directoryPath);
        private readonly string qCodeName = "AL_BARCODE.jpg";
        private readonly string slideDatName = "Slide.dat";
        private readonly List<string> _dirStdList = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public ImgViewerManagerViewModel ImgManagerVM { get; } = new ImgViewerManagerViewModel();
        /// <summary>
        /// 查询编码
        /// </summary>
        public string QueryCode { get; set; }
        /// <summary>
        /// 右侧切片目录所有集合
        /// </summary>
        public List<ImgViewFileInfo> ImgViewFileList { get; set; }
        /// <summary>
        /// 右侧切片筛选集合
        /// </summary>
        public List<ImgViewFileInfo> TmpImgFileList { get; set; }
        /// <summary>
        /// 当前路径
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// 树型菜单（左侧）
        /// </summary>
        public TreeViewData TreeData { get; set; }
        /// <summary>
        /// 生成节点委托
        /// </summary>
        readonly Func<DirectoryInfo, TreeNode, TreeNode> func = (dir, parent) => new TreeNode { Label = dir.Name, Tag = dir.FullName, Parent = parent };

        /// <summary>
        /// 主切片
        /// </summary>
        public ImgViewFileInfo MainImgVwFileInfo { get; set; }

        /// <summary>
        /// 阅片
        /// </summary>
        public ICommand ViewCommand => new Lazy<RelayCommand<ImgViewFileInfo>>(() => new RelayCommand<ImgViewFileInfo>(data =>
        {
            if (!string.IsNullOrEmpty(data.LocalFilePath))
            {
                var msg = new ShowContentWindowMessage("ImgVwMng", "阅片")
                {
                    BorderMargin = new Thickness(5, 25, 5, 25), // 接近全屏
                                                                //msg.DesignHeight = 1200;
                                                                //msg.DesignWidth = 1500;
                    Args = new object[] { data }
                };
                Messenger.Default.Send(msg);
            }
        })).Value;

        /// <summary>
        /// 分屏浏览
        /// </summary>
        public ICommand SplitCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var selectedList = ImgViewFileList?.Where(x => x.IsSelected).ToList();
            if (selectedList?.Count == 2)
            {
                if (selectedList.Contains(MainImgVwFileInfo))
                {
                    selectedList.Remove(MainImgVwFileInfo);
                    selectedList.Insert(0, MainImgVwFileInfo); // 将主切片放入第一个
                    var msg = new ShowContentWindowMessage("ImgVwMng", "阅片")
                    {
                        BorderMargin = new Thickness(5, 25, 5, 25), // 接近全屏
                        //DesignHeight = 1200,
                        //DesignWidth = 1500,
                        Args = new object[] { selectedList }
                    };
                    Messenger.Default.Send(msg);
                }
                else
                {
                    ShowMessageBox("请选择主切片！", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                ShowMessageBox("请选择两个切片进行分屏操作", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        })).Value;
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            TmpImgFileList = ImgViewFileList?.Where(x => string.IsNullOrEmpty(QueryCode) || x.DicectoryName.Contains(QueryCode)).ToList();
        })).Value;

        public ImageViewerPCViewModel()
        {
            Refresh();
        }

        /// <summary>
        /// 刷新目录
        /// </summary>
        protected override void Refresh()
        {
            var directory = ConfigurationManager.AppSettings[directoryPath];
            var flag = false;
            if (Directory.Exists(directory)) // 存在配置则读取
            {
                DirectoryPath = directory;
                flag = true;
            }
            else if (Directory.Exists(DirectoryPath)) // 没有配置则读取打开的目录
            {
                flag = true;
            }
            if (flag)
            {
                var (treeData, imgViewFiles) = CreateNewTreeViewData(DirectoryPath);
                TreeData = treeData;
                ImgViewFileList = imgViewFiles;
                QueryCommand.Execute(null);
            }

        }

        /// <summary>
        /// 打开目录
        /// </summary>
        protected override void OpenDir()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                // dialog.InitialDirectory = textbox.Text; // Use current value for initial dir
                Title = "请选择切片目录",
                Filter = "Directory|*.this.directory", //
                FileName = "select" // Filename will then be "select.this.directory"
            };
            if (dialog.ShowDialog() == true)
            {
                var directtory = Path.GetDirectoryName(dialog.FileName);
                if (!string.IsNullOrEmpty(directtory))
                {
                    DirectoryPath = directtory;
                    ConfigHelper.SaveAppseting(directoryPath, DirectoryPath);
                    Refresh();
                }
            }
        }


        /// <summary>
        /// 创建目录树（只保留存在切片的目录）
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private (TreeViewData treeData, List<ImgViewFileInfo> imgViewFiles) CreateNewTreeViewData(string directory)
        {
            var treeData = new TreeViewData();
            var imgViewFiles = new List<ImgViewFileInfo>();
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                if (!string.IsNullOrEmpty(directory))
                {
                    var topDir = new DirectoryInfo(directory);
                    // 1. 获取单文件
                    var fileInfos = topDir.EnumerateFiles($"*{DZISingleConstant.Cons.FileExtension}", SearchOption.AllDirectories).ToList();
                    imgViewFiles = DZISingleConstant.Cons.CreateImgViewTileList(fileInfos); // 创建表格
                    // 获取所有目录
                    var subDirects2 = topDir.EnumerateDirectories("*", SearchOption.AllDirectories).ToList();
                    // 2. 获取所有底层目录
                    var bottomDirs = subDirects2.Where(x => DZIConstant.Cons.IsSlice(x.FullName)).ToList();
                    // 表格 中加入 目录型文件
                    imgViewFiles.AddRange(DZIConstant.Cons.CreateImgViewTileList(bottomDirs));
                    // 获取根据底层而来的所有目录
                    var realDirs = subDirects2.Where(s => s.GetDirectories().Length > 0 ?
                                                        bottomDirs.Exists(b => b.FullName.Contains(s.FullName)) : bottomDirs.Exists(b => b.FullName == s.FullName)).ToList();
                    fileInfos?.ForEach(x =>
                    {
                        if (!realDirs.Exists(d => d.FullName == x.Directory.FullName))
                        {
                            realDirs.Add(x.Directory);
                        }
                    });
                    var allNodes = realDirs.Select(x => func(x, func(x.Parent, null))).ToList();

                    var parents = new List<TreeNode>();

                    foreach (var y in allNodes)
                    {
                        if (!string.IsNullOrEmpty(y.Parent.Tag) && y.Parent.Tag != directory)
                        {
                            var parent = allNodes.FirstOrDefault(x => x.Tag == y.Parent.Tag);
                            if (parent != null)
                            {
                                y.Parent = parent;
                                parent.ChildNodes.Add(y);
                            }
                            else
                            {
                                parents.Add(y);
                            }
                        }
                        else
                        {
                            parents.Add(y);
                        }
                    }

                    var topnode = func(topDir, null);

                    foreach (var node in parents)
                    {
                        topnode.ChildNodes.Add(node);
                    }
                    treeData.RootNodes.Add(topnode);
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
            return (treeData, imgViewFiles);
        }
    }
}
