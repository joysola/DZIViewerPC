/********************************************************************************
 *
 * 文件名称：CustomBaseViewModel.cs
 * 作    者：许文龙
 * 日    期：2020-08-21
 * 描    述：各个界面对应的ViewModel的父类，不让界面ViewMode直接继承BaseViewModel
 *           因为BaseViewModel是和BaseControl配套使用，是基础类型。
 *           而各个界面的ViewModel的共用逻辑，不适合放在BaseViemModel，所以抽象出
 *           CustomBaseViewModel
 *
 * ******************************************************************************/

using DST.Controls.Base;
using DST.Database.WPFCommonModels;
using DST.PIMS.Framework.Extensions;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    /// <summary>
    /// 各个界面对应的ViewModel的父类，不让界面ViewMode直接继承BaseViewModel
    /// </summary>
    public class CustomBaseViewModel : BaseViewModel
    {
        private CustomPageModel pageModel = new CustomPageModel();

        /// <summary>
        /// 分页信息对象
        /// </summary>
        public CustomPageModel PageModel
        {
            get { return this.pageModel; }
            set
            {
                this.pageModel = value;
                this.RaisePropertyChanged("PageModel");
            }
        }

        /// <summary>
        /// 分页导航栏：首页
        /// </summary>
        public ICommand FirstPageCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    this.PageModel.PageIndex = 1;
                    this.LoadData();
                });
            }
        }

        /// <summary>
        /// 分页导航栏：上一页
        /// </summary>
        public ICommand PreviousPageCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    if (this.PageModel.PageIndex > 1)
                    {
                        this.PageModel.PageIndex = this.PageModel.PageIndex - 1;
                        this.LoadData();
                    }
                });
            }
        }

        /// <summary>
        /// 分页导航栏：下一页
        /// </summary>
        public ICommand NextPageCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    if (this.PageModel.PageIndex < this.PageModel.TotalPage)
                    {
                        this.PageModel.PageIndex = this.PageModel.PageIndex + 1;
                        this.LoadData();
                    }
                });
            }
        }

        /// <summary>
        /// 分页导航栏：末页
        /// </summary>
        public ICommand LastPageCommand
        {
            get
            {
                return new RelayCommand<object>(data =>
                {
                    if (this.PageModel.PageIndex != this.PageModel.TotalPage)
                    {
                        this.PageModel.PageIndex = this.PageModel.TotalPage;
                        this.LoadData();
                    }
                });
            }
        }

        /// <summary>
        /// 分页导航栏：每页显示的条数
        /// </summary>
        public ICommand PaginPageChangedCommand
        {
            get
            {
                return new RelayCommand<int>(data =>
                {
                    this.PageModel.PageIndex = 1;
                    this.LoadData();
                });
            }
        }
      
        /// <summary>
        /// 无参构造
        /// </summary>
        public CustomBaseViewModel() : base()
        {
            this.RegisterCommand();
        }

        protected virtual void RegisterCommand()
        {
            // 注册功能按钮响应
            Messenger.Default.Register<EnumToolButtonType>(this, EnumMessageKey.ToolButtonResponse, data =>
            {
                this.InvokeToolButtonMethod(data);
            });
        }


        #region 顶部按钮
        protected virtual void ScanCode() => this.ScanCodeReceive();

        /// <summary>
        /// 重新取样关联
        /// </summary>
        protected virtual void ResampleConnect() { }

        /// <summary>
        /// 批量退单
        /// </summary>
        protected virtual void BatchChargeback() { }

        /// <summary>
        /// 重新生成 报告
        /// </summary>
        protected virtual void ReGenerateReport() { }

        /// <summary>
        /// 打印报告
        /// </summary>
        protected virtual void BatchPrintReport() { }

        /// <summary>
        /// 查询包埋
        /// </summary>
        protected virtual void QueryEmbed() { }

        /// <summary>
        /// 延迟取材
        /// </summary>
        protected virtual void DelayDrawMaterial() { }

        /// <summary>
        /// 申请单查看
        /// </summary>
        protected virtual void ViewAppFrm() { }

        /// <summary>
        /// 扫码接收
        /// </summary>
        protected virtual void ScanCodeReceive() { }

        /// <summary>
        /// 批量导入
        /// </summary>
        protected virtual void BatchImport() { }

        /// <summary>
        /// 结果确认
        /// </summary>
        protected virtual void CheckResult() { }

        /// <summary>
        /// 初审
        /// </summary>
        protected virtual void FirstExam() { }

        /// <summary>
        /// 复审
        /// </summary>
        protected virtual void ReExam() { }
        /// <summary>
        /// 补打条码
        /// </summary>
        protected virtual void ReprintCode() { }

        /// <summary>
        /// 扫码送检
        /// </summary>
        protected virtual void ScanCodeSendInsp() { }

        /// <summary>
        /// 物流签收
        /// </summary>
        protected virtual void SigninExpress() { }

        /// <summary>
        /// 新增物流
        /// </summary>
        protected virtual void AddExpress() { }

        /// <summary>
        /// 申请单维护
        /// </summary>
        protected virtual void MaintainAppFrm() { }

        /// <summary>
        /// 自定义加载字典
        /// </summary>
        public virtual void LoadCustomDict() { }
        /// <summary>
        /// 重置
        /// </summary>
        protected virtual void Reset() { }
        /// <summary>
        /// 保存
        /// </summary>
        protected virtual void Save() { }
        /// <summary>
        /// 保存&打印
        /// </summary>
        protected virtual void SavePrint() { }

        /// <summary>
        /// 登记查询
        /// </summary>
        protected virtual void QueryRegistration() { }

        /// <summary>
        /// 批量外送
        /// </summary>
        protected virtual void BatchSendInsp() { }
        /// <summary>
        /// 申请单图像采集
        /// </summary>
        protected virtual void CollectAppFrmImage() { }
        /// <summary>
        /// 送检确认
        /// </summary>
        protected virtual void CheckSendInsp() { }
        /// <summary>
        /// 取消登记
        /// </summary>
        protected virtual void WithdrawRegistration() { }
        /// <summary>
        /// 上一例
        /// </summary>
        protected virtual void PreviousOne() { }
        /// <summary>
        /// 下一例
        /// </summary>
        protected virtual void NextOne() { }
        /// <summary>
        /// 保存&下例
        /// </summary>
        protected virtual void SaveNext() { }

        /// <summary>
        /// 扫码工作站扫码接收
        /// </summary>
        protected virtual void ReceiveByScan() { }

        /// <summary>
        /// 退回检验
        /// </summary>
        protected virtual void BackCheckoutBatch() { }

        /// <summary>
        /// 临床维护
        /// </summary>
        protected virtual void ClinicalManifestation()
        {
        }
        /// <summary>
        /// 保存&外送
        /// </summary>
        protected virtual void SaveSend() { }
        /// <summary>
        /// 刷新
        /// </summary>
        protected virtual void Refresh() { }
        /// <summary>
        /// 打开目录
        /// </summary>
        protected virtual void OpenDir() { }
        #endregion 顶部按钮

        #region 分页导航栏
        /// <summary>
        /// 分页导航栏：首页
        /// </summary>
        public virtual void PaginationFirstPage()
        {
        }

        /// <summary>
        /// 分页导航栏：上一页
        /// </summary>
        public virtual void PaginationPreviousPage()
        {
        }

        /// <summary>
        /// 分页导航栏：下一页
        /// </summary>
        public virtual void PaginationNextPage()
        {
        }

        /// <summary>
        /// 分页导航栏：最后一页
        /// </summary>
        public virtual void PaginationLastPagePage()
        {
        }

        /// <summary>
        /// 分页导航栏：切换每页条数
        /// </summary>
        public virtual void PaginPageChanged()
        {
        }
        #endregion 分页导航栏


    }
}