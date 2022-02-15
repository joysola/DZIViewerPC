using DST.Common.Helper;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DST.PIMS.Framework
{
    /// <summary>
    /// 上传样本图片
    /// </summary>
    public class UploadManager
    {
        public static UploadManager Instance { get; } = new UploadManager();

        private string uploadImageRootPath = @"D:\dst";

        private UploadManager()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            this.uploadImageRootPath = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.Upload, "UploadImageRootPath");
        }

        public void UploadSampleTask(string filePath)
        {
            Task.Run(() => 
            {
                this.UploadSample(filePath);
            });
        }


        /// <summary>
        /// 自动上传样本逻辑
        /// </summary>
        /// <param name="filePath">socket传送过来的样本路径</param>
        public void UploadSample(string filePath)
        {
            string dirPath = this.uploadImageRootPath + "\\" + filePath;
            if (!Directory.Exists(dirPath))
            {
                return;
            }

            try
            {
                SampleUpload curUp = new SampleUpload();
                curUp.LocalPath = dirPath;
                curUp.TotalPartNumber = new DirectoryInfo(curUp.LocalPath).GetFiles().Length - 1;
                curUp.CurPartNumber = 0;
                curUp.STATUS = 0;
                int index = 0;
                while (index < 2)
                {
                    if (curUp.STATUS == 0 || curUp.STATUS == 1)
                    {
                        this.UploadSample(curUp);
                    }
                    else
                    {
                        break;
                    }

                    index++;
                }
            }
            catch(Exception e)
            {
                Logger.Error($"上传样本{dirPath}失败：" + e.Message);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        public async Task<bool> UploadSample(SampleUpload curUp)
        {
            if(curUp == null)
            {
                return false;
            }

            try
            {
                if (curUp.STATUS == 0)
                {
                    curUp.STATUS = 2;
                    string index_code = UploadServer.Instance.UploadIndex(curUp.LocalPath + $"\\{curUp.LocalPathName}.csv", ExtendAppContext.Current.LoginModel.Customer_id);
                    Logger.Info($"上传状态：0, Index_code：{index_code}");
                    if (!string.IsNullOrEmpty(index_code))
                    {
                        curUp.index_code = index_code;
                        FileInfo[] allFiles = new DirectoryInfo(curUp.LocalPath).GetFiles();

                        // 多线程
                        string path = ExtendAppContext.Current.ConfigurationIniPath;
                        int maxUploadCount = int.Parse(IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.Upload, "MaxUploadCount", "1"));
                        int split = allFiles.Length % maxUploadCount == 0 ? allFiles.Length / maxUploadCount : allFiles.Length / maxUploadCount + 1;
                        var taskList = new List<Task>();
                        for(int i = 0; i < maxUploadCount; i++)
                        {
                            FileInfo[] tmpFils = allFiles.Skip(i * split).Take(split).ToArray();
                            var task = Task.Run(async () =>
                            { 
                                for(int j = 0; j < tmpFils.Length; j++)
                                {
                                    if (tmpFils[j].Name.Equals(curUp.LocalPathName + ".csv"))
                                    {
                                        continue;
                                    }

                                    if (await UploadServer.Instance.UploadImg(tmpFils[j].FullName, index_code))
                                    {
                                        curUp.CurPartNumber++;
                                    }
                                    else
                                    {
                                        curUp.WriteLog($"上传{tmpFils[j].FullName}失败！");
                                        curUp.STATUS = 1;
                                    }
                                }
                            });

                            taskList.Add(task);
                        }

                        await Task.WhenAll(taskList);

                        // 单线程
                        //for (int i = 0; i < allFiles.Length; i++)
                        //{
                        //    if (allFiles[i].Name.Equals(curUp.LocalPathName + ".csv"))
                        //    {
                        //        continue;
                        //    }

                        //    if (await UploadServer.Instance.UploadImg(allFiles[i].FullName, index_code))
                        //    {
                        //        curUp.CurPartNumber++;
                        //    }
                        //    else
                        //    {
                        //        curUp.WriteLog($"上传{allFiles[i].FullName}失败！");
                        //        curUp.STATUS = 1;
                        //    }
                        //}

                        if (curUp.STATUS == 2)
                        {
                            curUp.STATUS = 3;
                        }
                    }
                }
                
                if(curUp.STATUS != 3)
                {
                    await this.UploadConfirm(curUp);
                }

                // 上传部分，再次上传
            }
            catch(Exception e)
            {
                curUp.STATUS = 1;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 上传部分后需要循环再次上传
        /// </summary>
        /// <param name="curUp"></param>
        private async Task<bool> UploadConfirm(SampleUpload curUp)
        {
            bool result = true;
            int index = 0;
            try
            {
                while (curUp != null && curUp.STATUS != 3 && index < 5)
                {
                    if (curUp.STATUS == 1)
                    {
                        Logger.Info($"上传状态：{curUp.STATUS},Index_code：{curUp.index_code}");
                        curUp.STATUS = 2;
                        curUp.UnUploaded = UploadServer.Instance.GetUploadImageList(curUp.index_code, 0);
                        curUp.UnUploaded.ForEach(x =>
                        {
                            if (UploadServer.Instance.UploadImg(curUp.LocalPath + $"\\{x}", curUp.index_code).Result)
                            {
                                curUp.CurPartNumber++;
                            }
                            else
                            {
                                curUp.WriteLog($"上传{curUp.LocalPath}\\{x}失败！");
                                curUp.STATUS = 1;
                            }
                        });

                        if (curUp.STATUS == 2)
                        {
                            curUp.STATUS = 3;
                        }
                    }

                    index++;
                }
            }
            catch(Exception ex)
            {
                Logger.Error($"UploadConfirm 异常：" + ex.Message);
                result = false;
            }

            return result;
        }
    }
}
