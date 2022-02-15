using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Controls.Controls;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class AppFrmViewModel : CustomBaseViewModel
    {
        private ProductModel _selectedProduct;
        private PathSampInfo _selectedPathSamp;

        #region 字典
        /// <summary>
        /// 性别
        /// </summary>
        public List<DictItem> SexDict => ExtendApiDict.Instance.SexDict;
        /// <summary>
        /// 活动类型
        /// </summary>
        public List<DictItem> ActivityTypeDict => ExtendApiDict.Instance.ActivityTypeDict;
        /// <summary>
        /// 检查项目字典
        /// </summary>
        public List<ProductModel> ProductDict => ExtendApiDict.Instance.ProductDict;
        /// <summary>
        /// 医生字典
        /// </summary>
        public List<DoctorInfoModel> DoctorDict => new Lazy<List<DoctorInfoModel>>(() =>
        {
            PageModel.PageSize = 10000;
            PageModel.PageIndex = 1;
            var result = SysManageService.Instance.GetDocList(PageModel, new DoctorInfoModel { Name = string.Empty, Department = string.Empty });
            return result;
        }).Value;

        #endregion 字典

        [Notification]
        public FullyObservableCollection<AppFrmPermission> Permissions { get; set; }
        /// <summary>
        /// 胃镜相关部位(从字典获取胃镜)
        /// </summary>
        private List<SampSpecInfo> Gastroscope => new Lazy<List<SampSpecInfo>>(() =>
        {
            var list = new List<SampSpecInfo>();
            ExtendApiDict.Instance.GastroscopyTissueDict?.ForEach(g => list.Add(new SampSpecInfo { Name = g.dictValue, DictKey = g.dictKey, Number = 0 }));
            //list.Add(new SampSpecInfo { Name = "胃窦", Number = 1 });
            //list.Add(new SampSpecInfo { Name = "胃角", Number = 1 });
            //list.Add(new SampSpecInfo { Name = "贲门", Number = 1 });
            //list.Add(new SampSpecInfo { Name = "胃体", Number = 1 });
            //list.Add(new SampSpecInfo { Name = "幽门", Number = 1 });
            //list.Add(new SampSpecInfo { Name = "吻合口", Number = 1 });
            //list.Add(new SampSpecInfo { Name = "食道", Number = 1 });
            return list;
        }).Value;
        /// <summary>
        /// 申请单实体
        /// </summary>
        [Notification]
        public ApplyFrmModel AppModel { get; set; }
        /// <summary>
        /// 检查类型字典
        /// </summary>
        [Notification]
        public List<ProductType> ProductTypeDict { get; set; }
        /// <summary>
        /// 标记物/染色剂 集合
        /// </summary>
        [Notification]
        public List<DictItem> MarkerList { get; set; } = new List<DictItem>();
        /// <summary>
        /// 选择的检验项目
        /// </summary>
        [Notification]
        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                //SelectedProductType = null; // 改变选中检验项目下拉后，先清空检验类型
                //if (_selectedProduct != null)
                //{
                //    var curProduct = ProductDict?.FirstOrDefault(x => x.id == _selectedProduct.id);
                //    ProductTypeDict = curProduct?.productTypeList;
                //}
            }
        }
        /// <summary>
        /// 选择的检测项目类型
        /// </summary>
        [Notification]
        public ProductType SelectedProductType { get; set; }

        /// <summary>
        /// 选中的样本信息
        /// </summary>
        [Notification]
        public PathSampInfo SelectedPathSamp
        {
            get => _selectedPathSamp;
            set
            {
                _selectedPathSamp = value;
                // 当选中检查项目后，对应的部位集合写入productid
                if (_selectedPathSamp?.SampSpecList?.Count > 0)
                {
                    foreach (var spec in _selectedPathSamp?.SampSpecList)
                    {
                        spec.GastroscopeProdID = DSTCode.GastroscopeProdID;
                        spec.ProductID = _selectedPathSamp.ProductID;
                    }
                }
                // 当选择了胃镜时(且 集合中没有其他选项)，需要特殊集合
                if (_selectedPathSamp?.SampSpecList.Count == 0 && _selectedPathSamp.ProductID == DSTCode.GastroscopeProdID)
                {
                    Gastroscope.ForEach(g =>
                    {
                        _selectedPathSamp.SampSpecList.Add(g);
                    });
                }

                if(_selectedPathSamp != null && _selectedPathSamp?.ProductID == DSTCode.GastroscopeProdID && string.IsNullOrEmpty(_selectedPathSamp.InspecSample))
                {
                    _selectedPathSamp.InspecSample = "胃镜活检组织";
                }
            }
        }
        /// <summary>
        /// 是否是编辑模式
        /// </summary>
        [Notification]
        public bool IsAdd { get; set; } = true;
        /// <summary>
        /// 是否是前处理
        /// </summary>
        [Notification]
        public bool IsPhysDist { get; set; } = false;
        /// <summary>
        /// 新增项目
        /// </summary>
        public ICommand AddProductCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var result = true; // 验证结果
            if (string.IsNullOrEmpty(SelectedProduct?.id))
            {
                result = false;
                ShowMessageBox("请选择检测项目！", MessageBoxButton.OK, MessageBoxImage.Warning, null, true);
            }
            else
            {
                if (SelectedProduct?.productTypeList?.Count > 0 && string.IsNullOrEmpty(SelectedProductType?.id))
                {
                    result = false;
                    ShowMessageBox("请选择检测类型！", MessageBoxButton.OK, MessageBoxImage.Warning, null, true);
                }
                else
                {
                    var existedItem = AppModel?.PathSampInfoList?.FirstOrDefault(x => x.ProductID == SelectedProduct.id); // 找到已经存在的检测项目
                    if (existedItem != null)
                    {
                        result = false;
                        ShowMessageBox("请勿添加相同的检测项目！", MessageBoxButton.OK, MessageBoxImage.Warning, null, true);
                    }
                }
            }

            if (result)
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var newItem = new PathSampInfo
                    {
                        ProductID = SelectedProduct.id,
                        ProductName = !(SelectedProduct?.productTypeList?.Count > 0) ? $"{SelectedProduct.name}" : $"{SelectedProduct.name}/{SelectedProductType.name}",
                        ProductType = SelectedProductType?.value,
                        ProductCode = SelectedProduct.code, // 将选中的检查项目code赋值
                        MarkerList = ApplyFormService.Instance.GenerateMarkerList(SelectedProduct.id), // 标记物染色剂字典集合
                        GatherTime = DateTime.Now,
                        Screen = ActivityTypeDict.FirstOrDefault()?.dictKey, // 活动类型默认第一个
                    };
                    AppModel?.PathSampInfoList?.Add(newItem);
                    AppModel.PathSampInfoList = new ObservableCollection<PathSampInfo>(AppModel?.PathSampInfoList);
                    SelectedPathSamp = newItem; // 自动选中样本
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;

        /// <summary>
        /// 删除项目
        /// </summary>
        public ICommand DeleteProductCommand => new Lazy<RelayCommand<PathSampInfo>>(() => new RelayCommand<PathSampInfo>(data =>
        {
            ShowMessageBox("是否删除该检测项目？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    AppModel?.PathSampInfoList?.Remove(data);
                    AppModel.PathSampInfoList = new ObservableCollection<PathSampInfo>(AppModel?.PathSampInfoList);
                }
            });
        })).Value;

        /// <summary>
        /// 样本常用词
        /// </summary>
        public ICommand SampleNameCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            ShowComWord(DSTCode.SampleStr, "标本名称常用词选择", new RelayCommand<ComWordModel>(res =>
            {
                if (res != null && this.SelectedPathSamp != null)
                {
                    this.SelectedPathSamp.InspecSample = res.Content;
                }
            }));

        })).Value;
        /// <summary>
        /// 诊断常用词
        /// </summary>
        public ICommand DiagnosisNameCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            ShowComWord(DSTCode.DiagnosisStr, "临床诊断常用词选择", new RelayCommand<ComWordModel>(res =>
            {
                if (res != null && this.AppModel != null)
                {
                    this.AppModel.ClinicDiagnosis = res.Content;
                }
            }));

        })).Value;
        /// <summary>
        /// 新增送检部位
        /// </summary>
        public ICommand AddSpecCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (SelectedPathSamp != null)
            {
                var newSpecItem = new SampSpecInfo
                {
                    SampleID = SelectedPathSamp.ID,
                    Number = 1,
                    ProductID = SelectedPathSamp.ProductID,
                };
                SelectedPathSamp.SampSpecList.Add(newSpecItem);
            }
        })).Value;

        /// <summary>
        /// 删除送检部位
        /// </summary>
        public ICommand DeleteSpecCommand => new Lazy<RelayCommand<SampSpecInfo>>(() => new RelayCommand<SampSpecInfo>(data =>
        {
            ShowMessageBox("是否删除该取材部位？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    SelectedPathSamp?.SampSpecList?.Remove(data);
                }
            });

        })).Value;

        public AppFrmViewModel() =>
            this.InitPermission(); // 初始化权限

        /// <summary>
        /// 显示常用词
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        private void ShowComWord(string type, string title, ICommand command)
        {
            var msg = new ShowContentWindowMessage("ComWordDict", title);
            msg.DesignWidth = 600;
            msg.DesignHeight = 400;
            msg.Args = new object[] { type };
            msg.CallBackCommand = command;
            Messenger.Default.Send(msg);
        }
        /// <summary>
        /// 胃镜校验
        /// </summary>
        /// <returns></returns>
        private bool CheckGastroscope()
        {
            var result = true;
            var gastroscope = AppModel?.PathSampInfoList?.FirstOrDefault(x => x.ProductID == DSTCode.GastroscopeProdID);
            var count = gastroscope?.SampSpecList?.Count(x => x.Number == 0); // 获取没有数量的
            if (count.HasValue && count == gastroscope?.SampSpecList?.Count) // 胃镜送检部位 的数量 不能全为0
            {
                ShowMessageBox("胃镜送检部位数量至少填写一个！", MessageBoxButton.OK, MessageBoxImage.Error, null, true);
                result = false;
            }
            //if (!string.IsNullOrEmpty(gastroscope?.ProductType))
            //{
            //    if (int.TryParse(gastroscope.ProductType, out int maxCount))
            //    {
            //        var totalNum = gastroscope?.SampSpecList?.Sum(x => x.Number);
            //        if (maxCount != totalNum)
            //        {
            //            ShowMessageBox("胃镜对应部位数量总和应与填写数量一致！", MessageBoxButton.OK, MessageBoxImage.Error, null, true);
            //            result = false;
            //        }
            //    }
            //}
            return result;
        }
        /// <summary>
        /// 检查常规病理
        /// </summary>
        /// <returns></returns>
        private bool CheckConvenPath()
        {
            bool result = true;
            var convenPathList = AppModel?.PathSampInfoList?.Where(x => DSTCode.ConvenPathList.Contains(x.ProductID)).ToList();
            foreach (var path in convenPathList)
            {
                var errorSpecList = path.SampSpecList?.Where(x => string.IsNullOrEmpty(x.Name)).ToList(); // 部位名称不为空
                if (errorSpecList?.Count > 0 || path.SampSpecList?.Count == 0) // 必须存在部位
                {
                    result = false;
                    ShowMessageBox("常规病理部位为必填项，不可为空！", MessageBoxButton.OK, MessageBoxImage.Error, null, true);
                    break;
                }
                if (!string.IsNullOrEmpty(path?.ProductType))
                {
                    if (int.TryParse(path.ProductType, out int maxCount))
                    {
                        var totalNum = path?.SampSpecList?.Sum(x => x.Number);
                        if (maxCount != totalNum)
                        {
                            ShowMessageBox("常规病理对应部位数量总和应与填写数量一致！", MessageBoxButton.OK, MessageBoxImage.Error, null, true);
                            result = false;
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(path.InspecSample)) // 常规病理送检样本名称必填
                {
                    ShowMessageBox("常规病理送检样本名称,不可为空！", MessageBoxButton.OK, MessageBoxImage.Error, null, true);
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// 校验检验项目的取样时间
        /// </summary>
        /// <returns></returns>
        private bool CheckGatherTime()
        {
            bool result = true;
            var errorSampList = AppModel?.PathSampInfoList?.Where(x => x.GatherTime == null).ToList();
            if (errorSampList?.Count > 0)
            {
                ShowMessageBox("取样时间不能为空！", MessageBoxButton.OK, MessageBoxImage.Error, null, true);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 校验免疫组化
        /// </summary>
        /// <returns></returns>
        private bool CheckImmuhist()
        {
            bool result = true;
            var immuList = AppModel?.PathSampInfoList?.Where(x => x.ProductID == DSTCode.ImmuhistchmProdID)?.ToList();
            foreach (var imm in immuList)
            {
                if (imm.SelectedMarkers?.Count > 0)
                {
                    // do nothing
                }
                else
                {
                    ShowMessageBox("免疫组化的标记物/染色剂为必填项，不可为空！", MessageBoxButton.OK, MessageBoxImage.Error, null, true);
                    result = false;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 检验数据
        /// </summary>
        /// <returns></returns>
        internal bool CheckSaveData()
        {
            var res = true;
            //Messenger.Default.Send<object>(null, "RefreshAllHCCtls"); // 重新获取hc所有控件，因为nesceesary可能变化了
            if (IsCheckedOK) // 验证在此viewmodel中
            {
                if (AppModel?.PathSampInfoList?.Count <= 0)
                {
                    ShowMessageBox("检查项目至少填写一个！", MessageBoxButton.OK, MessageBoxImage.Warning, null, true);
                    res = false;
                }
                if (!CheckConvenPath()) // 校验常规病理
                {
                    res = false;
                }
                if (!CheckGastroscope()) // 校验胃镜
                {
                    res = false;
                }
                if (!CheckImmuhist()) // 校验免疫组化
                {
                    res = false;
                }
                if (!CheckGatherTime()) // 校验取样时间
                {
                    res = false;
                }
            }
            else
            {
                res = false;
            }
            return res;
        }
        /// <summary>
        /// 初始化申请单权限
        /// </summary>
        private void InitPermission()
        {
            var list = new List<AppFrmPermission>();
            list.Add(new AppFrmPermission { Name = "基本信息" });
            list.Add(new AppFrmPermission { Name = "检查项目信息" });
            list.Add(new AppFrmPermission { Name = "标本信息" });
            list.Add(new AppFrmPermission { Name = "临床诊断相关" });
            list.Add(new AppFrmPermission { Name = "其他信息" });
            list.Add(new AppFrmPermission { Name = "肿瘤相关" });
            list.Add(new AppFrmPermission { Name = "妇科相关" });
            var result = new FullyObservableCollection<AppFrmPermission>(list);
            // 集合元素改变时，主动触发属性改变事件
            result.ItemPropertyChanged += (s, e) =>
            {
                RaisePropertyChanged(nameof(Permissions));
            };
            Permissions = result;
        }
    }
}
