using DST.Common.Helper;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using DST.PIMS.Framework.Model.Enum;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace DST.PIMS.Client.ViewModel.Upload
{
    public class TreeMenuViewModel : CustomBaseViewModel
    {
        [Notification]
        public bool TreeViewIsEnable { get; set; } = true;

        /// <summary>
        /// 样本路径
        /// </summary>
        [Notification]
        public string FilePath { get; set; }

        /// <summary>
        /// 树形节点列表
        /// </summary>
        [Notification]
        public TreeViewData TreeViewData { get; set; }

        /// <summary>
        /// 点击树的节点
        /// </summary>
        public ICommand TreeViewItemClickCommand { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public TreeMenuViewModel()
        {
        }

        public override void OnViewLoaded()
        {
            this.RefreshTreeView();
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.TreeViewItemClickCommand = new RelayCommand<TreeNode>(key =>
            {
                // 正在上传不允许切换目录
                if (ExtendAppContext.Current.CurLoginType == EnumLoginType.Uploading)
                {
                    Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                        this.ShowMessageBox("有文件正在上传，请等待其上传完成！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning));
                    return;
                }

                if (key.Tag == null) // 点击根目录刷新左侧列表
                {
                    this.RefreshTreeView();
                }
                else // 点击二级子目录
                {
                    this.ShowNewUploadSamples(key.Tag);
                }
            });

            // 是否需要禁用树形控件
            Messenger.Default.Register<bool>(this, EnumMessageKey.UploadScanImgTreeViewDisabled, data =>
            {
                this.TreeViewIsEnable = data;
            });

            Messenger.Default.Register<object>(this, EnumMessageKey.RefreshUploadTreeMenu, data =>
            {
                this.RefreshTreeView();
            });
        }

        /// <summary>
        /// 刷新树形结构数据
        /// </summary>
        /// <param name="pathName"></param>
        private void RefreshTreeView()
        {
            // 初始化树状目录
            string iniPath = ExtendAppContext.Current.ConfigurationIniPath;
            string path = IniHelper.CreateInstance(iniPath).IniReadValue(IniSectionConst.Upload, "UploadImageRootPath", @"D:\WSITestData\ImagePath");
            if(string.IsNullOrEmpty(path))
            {
                path = @"D:\\DST\\ImagePath";
                IniHelper.CreateInstance(iniPath).IniWriteValue(IniSectionConst.Upload, "UploadImageRootPath", path);
            }
            // Directory.CreateDirectory(path);
            DirectoryInfo directory = new DirectoryInfo(path);
            var subDirects = directory.GetDirectories(); // 获取一级子目录
            this.TreeViewData = new TreeViewData();
            var rootNode = new TreeNode { Label = path, Level = 0 };
            foreach (var direct in subDirects)
            {
                var subNode = new TreeNode { Label = direct.Name, Level = 1, Tag = direct.FullName };
                rootNode.ChildNodes.Add(subNode);
            }
            this.TreeViewData.RootNodes.Add(rootNode);
            // 默认点击第一项
            if (rootNode.ChildNodes.Count > 0)
            {
                rootNode.ChildNodes[0].IsSelected = true;
                TreeViewItemClickCommand.Execute(rootNode.ChildNodes[0]);
            }
        }

        /// <summary>
        /// 展现某目录下所有切片信息
        /// </summary>
        /// <param name="parentPathName">目录名</param>
        private void ShowNewUploadSamples(string parentPathName)
        {
            Messenger.Default.Send<string>(parentPathName, EnumMessageKey.UploadScanImgRefresh);
        }

        public DST_SAMPLE_UPLOAD CreateSampleUploadModel(string samplePath)
        {
            string csvFile = samplePath + "\\" + Path.GetFileName(samplePath) + ".csv"; 
            if(!File.Exists(csvFile))
            {
                return null;
            }

            DST_SAMPLE_UPLOAD model = new DST_SAMPLE_UPLOAD();
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(samplePath);
            model.SAMPLE_PATH = samplePath;
            model.SAMPLE_CODE = dirInfo.Name;
            // parent： 20201104文件夹
            model.PARENT_PATH_NAME = dirInfo.Parent.Name;
            model.CsvFile = csvFile;
            return model;
        }

        /// <summary>
        /// 刷新数据，计算zip文件的大小等信息
        /// </summary>
        public bool RefreshData(DST_SAMPLE_UPLOAD model)
        {
            bool result = System.IO.File.Exists(model.SampleZipFile);
            if (result)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(model.SampleZipFile);
                model.FILE_SIZE = fileInfo.Length;
                model.TotalPartNumber = (int)(model.FILE_SIZE / DST_SAMPLE_UPLOAD.MinimumPartSize + 1);
                model.CurPartNumber = 0;
            }
            else
            {
                model.TotalPartNumber = int.MaxValue; // 若没有压缩包，先假设总上传包数很大
            }

            return result;
        }
    }
}
