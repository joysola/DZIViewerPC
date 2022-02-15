using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using DST.PIMS.Framework.Model.Enum;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DST.PIMS.Client.ViewModel.Upload
{
    public class UploadScanImgViewModel : CustomBaseViewModel
    {
        private bool selectAll = false;
        /// <summary>
        /// 父文件名称
        /// </summary>
        private string curDatePath;

        /// <summary>
        /// 该目录下所有样本信息原始集合
        /// </summary>
        private List<SampleUpload> originSampleList = new List<SampleUpload>();

        /// <summary>
        /// 单个上传
        /// </summary>
        public ICommand StatusChangedCommand { get; set; }

        /// <summary>
        /// 批量上传
        /// </summary>
        public ICommand BatchUploadCommand { get; set; }

        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand { get; set; }

        /// <summary>
        /// 查看日志
        /// </summary>
        public ICommand CheckLogCommand { get; set; }

        [Notification]
        public int? QueryStatus { get; set; }

        /// <summary>
        /// 查询编号
        /// </summary>
        [Notification]
        public string QueryCode { get; set; }

        /// <summary>
        /// 上传状态：0=未上传，1=上传部分，还未上传完，2=正在上传，3=上传完成
        /// </summary>
        [Notification]
        public Dictionary<string, int?> UploadDict => new Dictionary<string, int?> { { "未上传", 0 },
                                                                                     { "上传部分", 1 },
                                                                                     { "正在上传", 2 }, 
                                                                                     { "上传成功", 3 }};

        [Notification]
        public bool SelectAll
        {
            get { return this.selectAll; }
            set
            {
                this.selectAll = value;
                this.RaisePropertyChanged("SelectAll");
                this.SampleUploadModelList.ToList().ForEach(x =>
                {
                    x.IsSelected = value;
                });
            }
        }

        /// <summary>
        /// 过滤后的样本信息集合
        /// </summary>
        [Notification]
        public ObservableCollection<SampleUpload> SampleUploadModelList { get; set; } = new ObservableCollection<SampleUpload>();

        /// <summary>
        /// 无参构造
        /// </summary>
        public UploadScanImgViewModel()
        {
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            Messenger.Default.Register<string>(this, EnumMessageKey.UploadScanImgRefresh, data =>
            {
                if (string.IsNullOrEmpty(data))
                {
                    return;
                }

                this.curDatePath = data;
                this.RefreshData();
            });

            // 单个样本文件上传
            this.StatusChangedCommand = new RelayCommand<object>(async par =>
            {
                var sample = par as SampleUpload;
                if (!IsUploading())
                {
                    await this.UploadSamples(this.SingleUpload(sample));
                    this.RefreshData();
                }
            });

            this.BatchUploadCommand = new RelayCommand(async () =>
            {
                if (!IsUploading())
                {
                    ExtendAppContext.Current.CurLoginType = EnumLoginType.Uploading;
                    Messenger.Default.Send(false, EnumMessageKey.UploadScanImgTreeViewDisabled); // 树形控件不可用
                    // 单线程上传
                    var selectedList = this.SampleUploadModelList.Where(x => x.IsSelected).ToList();
                    foreach (var sample in selectedList)
                    {
                        // 单个样本循环上传，不需要并发上传
                        await this.SingleUpload(sample);
                    }

                    // 多线程上传样本
                    //var selectedList = this.SampleUploadModelList.Where(x => x.IsSelected).ToList();
                    //var taskList = new List<Task>();
                    //foreach (var sample in selectedList)
                    //{
                    //    // 单个样本循环上传，不需要并发上传
                    //    var task = Task.Run(async () =>
                    //    {
                    //        await this.SingleUpload(sample);
                    //    });

                    //    taskList.Add(task);
                    //}

                    //await this.UploadSamples(taskList.ToArray());
                    ExtendAppContext.Current.CurLoginType = EnumLoginType.NormalLogin;
                    Messenger.Default.Send(true, EnumMessageKey.UploadScanImgTreeViewDisabled); // 树形控件可以使用
                    this.RefreshData();
                }
            });

            this.QueryCommand = new RelayCommand(() =>
            {
                if (!this.IsUploading())
                {
                    this.Query();
                }
            });

            this.CheckLogCommand = new RelayCommand<string>(par =>
            {
                ShowContentWindowMessage msg = new ShowContentWindowMessage("UploadLog", "查看日志");
                msg.Width = 600;
                msg.Height = 300;
                msg.Args = new[] { par }; // 日志信息
                Messenger.Default.Send(msg);
            });
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            try
            {
                this.originSampleList.Clear();
                this.originSampleList = null;
                this.originSampleList = new List<SampleUpload>();
                WhirlingControlManager.ShowWaitingForm();
                this.SelectAll = false; // 切目录时取消全选

                // 获取本地的样本数据
                DirectoryInfo dir = new DirectoryInfo(this.curDatePath);

                // 获取上传的情况
                List<SampleUpload> sampleList = UploadServer.Instance.GetSampleUploadListByDate(ExtendAppContext.Current.LoginModel.Customer_id, dir.Name);
                SampleUpload tmpSam = null;
                
                DirectoryInfo[] sampleDir = dir.GetDirectories();
                for (int i = 0; i < sampleDir.Length; i++)
                {
                    if (!File.Exists(sampleDir[i].FullName + $"\\{sampleDir[i].Name}.csv"))
                    {
                        continue;
                    }

                    tmpSam = sampleList.Where(x => x.key.Equals($"{ExtendAppContext.Current.LoginModel.Customer_id}/{sampleDir[i].Name}.csv")).OrderByDescending(x => x.created_time).ToList().FirstOrDefault(x => x.sample_path.EndsWith(sampleDir[i].Name));
                    if (tmpSam == null)
                    {
                        tmpSam = new SampleUpload();
                        tmpSam.LocalPath = sampleDir[i].FullName;
                        tmpSam.TotalPartNumber = sampleDir[i].GetFiles().Length - 2;
                        tmpSam.CurPartNumber = 0;
                        tmpSam.STATUS = 0;
                    }
                    else
                    {
                        tmpSam.LocalPath = sampleDir[i].FullName;
                        if (tmpSam.completed.HasValue && tmpSam.completed.Value)
                        {
                            tmpSam.TotalPartNumber = sampleDir[i].GetFiles().Length - 2;
                            tmpSam.CurPartNumber = sampleDir[i].GetFiles().Length - 2;
                            tmpSam.STATUS = 3;
                        }
                        else
                        {
                            tmpSam.HasUploaded = UploadServer.Instance.GetUploadImageList(tmpSam.index_code, 1);
                            tmpSam.UnUploaded = UploadServer.Instance.GetUploadImageList(tmpSam.index_code, 0);
                            tmpSam.TotalPartNumber = sampleDir[i].GetFiles().Length - 2;
                            tmpSam.CurPartNumber = tmpSam.HasUploaded.Count;
                            tmpSam.STATUS = tmpSam.HasUploaded.Count > 0 ? 1 : 0;
                        }
                    }

                    this.originSampleList.Add(tmpSam);
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
            this.Query();
        }

        /// <summary>
        /// 样本任务处理
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        private async Task UploadSamples(params Task[] tasks)
        {
            ExtendAppContext.Current.CurLoginType = EnumLoginType.Uploading;
            Messenger.Default.Send(false, EnumMessageKey.UploadScanImgTreeViewDisabled); // 树形控件不可用
            // 执行上传任务
            if (tasks != null)
            {
                await Task.WhenAll(tasks);
            }
            ExtendAppContext.Current.CurLoginType = EnumLoginType.NormalLogin;
            Messenger.Default.Send(true, EnumMessageKey.UploadScanImgTreeViewDisabled); // 树形控件可以使用
        }

        /// <summary>
        /// 上传单个样本
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task SingleUpload(SampleUpload data)
        {
            try
            {
                data.LogInfo = string.Empty; // 每次上传，先清空上次的日志
                Logger.Info($"样本：{data.sample_code}开始上传！");
                data.WriteLog($"样本：{data.sample_code}开始上传！");
                if (null == data)
                {
                    return;
                }

                if (data.completed.HasValue && data.completed.Value)
                {
                    this.ShowMessageBox("文件已经上传成功，是否重新上传？", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question,
                    (result) =>
                    {
                        if (result == System.Windows.MessageBoxResult.Yes || result == System.Windows.MessageBoxResult.OK)
                        {
                            Logger.Info($"样本：{data.sample_code}重新上传！");
                            data.WriteLog($"样本：{data.sample_code}重新上传！");
                            data.STATUS = 0;
                            data.CurPartNumber = 0;
                        }
                    });
                }

                await Task.Run(async () =>
                {
                    await UploadManager.Instance.UploadSample(data);
                    if (data.STATUS == 3)
                    {
                        data.CurPartNumber = data.TotalPartNumber;
                        data.completed = true;
                    }
                });
            }
            catch(Exception ex)
            {
                Logger.Error("上传文件失败：" + ex.Message);
            }
            
            return;
        }


        /// <summary>
        /// 是否有消息正在上传
        /// </summary>
        /// <returns></returns>
        private bool IsUploading()
        {
            bool result = false;
            if (ExtendAppContext.Current.CurLoginType == EnumLoginType.Uploading)
            {
                result = true;
                Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                        this.ShowMessageBox("有文件正在上传，请等待其上传完成！", MessageBoxButton.OK, MessageBoxImage.Warning));
            }
            return result;
        }

        /// <summary>
        /// 初始化切片目录信息
        /// </summary>
        /// <param name="data"></param>
        private void Query()
        {
            try
            {
                // 上传状态：0=未上传，1=上传部分，还未上传完，2=正在上传，3=上传完成
                switch (this.QueryStatus)
                {
                    case 0:
                        this.SampleUploadModelList = new ObservableCollection<SampleUpload>(this.originSampleList.Where(x => x.STATUS == 0 && (string.IsNullOrEmpty(this.QueryCode) || x.sample_code.Contains(this.QueryCode))));
                        break;
                    case 1:
                        this.SampleUploadModelList = new ObservableCollection<SampleUpload>(this.originSampleList.Where(x => x.STATUS == 1 && (string.IsNullOrEmpty(this.QueryCode) || x.sample_code.Contains(this.QueryCode))));
                        break;
                    case 2:
                        this.SampleUploadModelList = new ObservableCollection<SampleUpload>(this.originSampleList.Where(x => x.STATUS == 2 && (string.IsNullOrEmpty(this.QueryCode) || x.sample_code.Contains(this.QueryCode))));
                        break;
                    case 3:
                        this.SampleUploadModelList = new ObservableCollection<SampleUpload>(this.originSampleList.Where(x => x.STATUS == 3 && (string.IsNullOrEmpty(this.QueryCode) || x.sample_code.Contains(this.QueryCode))));
                        break;
                    default:
                        this.SampleUploadModelList = new ObservableCollection<SampleUpload>(this.originSampleList.Where(x => string.IsNullOrEmpty(this.QueryCode) || x.sample_path.Contains(this.QueryCode)));
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("初始化样本原始信息出错（InitSamplesInfo）！", ex);
            }
        }

        public void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.QueryCommand.Execute(null);
            }
        }
    }
}
