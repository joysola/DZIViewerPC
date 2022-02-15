using DST.ApiClient.Service;
using DST.PIMS.Framework.Model;
using DST.PIMS.Framework.Model.Test;
using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using ProductModel = DST.PIMS.Framework.Model.Test.ProductModel;

namespace DST.PIMS.Client.ViewModel.Test
{
    public class TestViewModel : CustomBaseViewModel
    {
        #region TestDicts
        public Dictionary<string, string> Sex_DataList { get; set; } = new Dictionary<string, string> { { "1", "男" }, { "2", "女" } };
        public Dictionary<string, string> Nationality_Dict { get; set; } = new Dictionary<string, string> { { "1", "汉族" }, { "2", "维吾尔族" }, { "3", "朝鲜族" } };
        public Dictionary<string, string> Marriage_Dict { get; set; } = new Dictionary<string, string> { { "1", "已婚" }, { "2", "未婚" }, { "3", "离异" }, { "4", "不详" } };
        public Dictionary<string, string> InspectHosp_Dict { get; set; } = new Dictionary<string, string> { { "1", "本院" }, { "2", "外院" } };
        public Dictionary<string, string> InspectDept_Dict { get; set; } = new Dictionary<string, string> { { "1", "麻醉科" }, { "2", "心外科" }, { "3", "呼吸科" }, { "4", "太平间" } };
        public Dictionary<string, string> Doc_Dict { get; set; } = new Dictionary<string, string> { { "123", "蔡文姬" }, { "456", "储物间" }, { "789", "传文件" }, { "012", "除味剂" }, { "345", "长尾夹" }, { "678", "出无尽" }, { "901", "厨卫间" } };
        public Dictionary<string, string> PatType_Dict { get; set; } = new Dictionary<string, string> { { "1", "门诊" }, { "2", "住院" } };
        public Dictionary<string, string> Ward_Dict { get; set; } = new Dictionary<string, string> { { "1", "A区" }, { "2", "B区" } };
        public Dictionary<string, string> ChargeType_Dict { get; set; } = new Dictionary<string, string> { { "1", "普通医保" }, { "2", "个人自费" } };
        public Dictionary<string, string> SampleType_Dict { get; set; } = new Dictionary<string, string> { { "1", "脏器切除" }, { "2", "胃镜" }, { "3", "肠镜" } };
        public Dictionary<string, string> Case_Dict { get; set; } = new Dictionary<string, string> { { "1", "小组织" }, { "2", "大组织" }, { "3", "免疫组化" }, { "4", "特殊染色" } };
        public Dictionary<string, string> Identity_Dict { get; set; } = new Dictionary<string, string> { { "1", "小组织" }, { "2", "大组织" }, { "3", "免疫组化" }, { "4", "特殊染色" } };
        public Dictionary<string, string> ReportState_Dict { get; set; } = new Dictionary<string, string> { { "1", "待取材" }, { "2", "待包埋" }, { "3", "待制片" }, { "4", "待检测" }, { "5", "待扫描" }, { "6", "待分配" }, { "7", "待初诊" }, { "8", "待审核" }, { "9", "待签发" }, { "10", "重新取样" }, { "11", "拒收" }, { "12", "已签发" } };
        public Dictionary<string, string> MyomaUterus_Dict { get; set; } = new Dictionary<string, string> { { "1", "小组织" }, { "2", "大组织" }, { "3", "免疫组化" }, { "4", "特殊染色" } };

        public Dictionary<string, string> Case_Dict2 { get; set; } = new Dictionary<string, string> { { "1", "液基细胞" }, { "2", "非妇细胞" }, { "3", "体液" }, { "4", "细胞学" }, { "5", "穿刺细胞学" } };
        public Dictionary<string, string> AdviceState_Dict { get; set; } = new Dictionary<string, string> { { "1", "未执行" }, { "2", "暂缓执行" } };
        public Dictionary<string, string> AdviceType_Dict { get; set; } = new Dictionary<string, string> { { "1", "重切" }, { "2", "深切" }, { "3", "薄切" }, { "4", "多切" }, { "5", "白片" }, { "6", "连切" }, { "7", "补切" }, { "8", "重新制片" }, { "9", "重新扫描" } };
        public List<CommonTerm> CommTerms { get; set; } = new List<CommonTerm> { new CommonTerm { Name = "ccc", Value = "111" }, new CommonTerm { Name = "scs", Value = "234" }, new CommonTerm { Name = "收到是", Value = "345" }, new CommonTerm { Name = "提供给", Value = "678" } };
        public Dictionary<string, string> Receive_Dict { get; set; } = new Dictionary<string, string> { { "1", "未接收" }, { "2", "接收" } };


        /// <summary>
        /// HierarchicalDataTemplate 使用数据实体
        /// </summary>
        public class HHH
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public string Url { get; set; }
            public List<HHH> DataList { get; set; }
        }

        public List<HHH> H_Dict { get; set; }
        /// <summary>
        /// 系统管理菜单
        /// </summary>
        public List<HHH> SysMenus { get; set; } = new Lazy<List<HHH>>(() =>
        {
            var totalMenus = new List<HHH>();


            var subList1 = new List<HHH>();
            var subModel1_1 = new HHH { Key = Guid.NewGuid().ToString(), Value = "用户管理", Url = "/DST.PIMS.Client;component/Images/Test/Menu1.png" };
            var subModel1_2 = new HHH { Key = Guid.NewGuid().ToString(), Value = "业务角色管理", Url = "/DST.PIMS.Client;component/Images/Test/Menu2.png" };
            subList1.Add(subModel1_1);
            subList1.Add(subModel1_2);
            var menu1 = new HHH { Key = Guid.NewGuid().ToString(), Value = "系统管理", DataList = subList1, Url = "/DST.PIMS.Client;component/Images/Test/Menu3.png" };

            var subList2 = new List<HHH>();
            var subModel2_1 = new HHH { Key = Guid.NewGuid().ToString(), Value = "医院管理", Url = "/DST.PIMS.Client;component/Images/Test/Menu4.png" };
            subList2.Add(subModel2_1);
            var menu2 = new HHH { Key = Guid.NewGuid().ToString(), Value = "科室管理", DataList = subList2, Url = "/DST.PIMS.Client;component/Images/Test/Menu5.png" };

            totalMenus.Add(menu1);
            totalMenus.Add(menu2);
            return totalMenus;

        }).Value;

        public List<HHH> SysPermission { get; set; } = new Lazy<List<HHH>>(() =>
        {
            var action = new Action<HHH>(model =>
            {
                model.DataList = new List<HHH>();
                for (int i = 0; i < 5; i++)
                {
                    var bottomModel = new HHH { Key = Guid.NewGuid().ToString(), Value = $"名称{i}{i}{i}{i}{i}" };
                    model.DataList.Add(bottomModel);
                }

            });


            var totalMenus = new List<HHH>();


            var subList1 = new List<HHH>();
            var subModel1_1 = new HHH { Key = Guid.NewGuid().ToString(), Value = "字典管理" };
            var subModel1_2 = new HHH { Key = Guid.NewGuid().ToString(), Value = "顶级菜单" };
            var subModel1_3 = new HHH { Key = Guid.NewGuid().ToString(), Value = "菜单管理" };
            var subModel1_4 = new HHH { Key = Guid.NewGuid().ToString(), Value = "用户列表" };
            var subModel1_5 = new HHH { Key = Guid.NewGuid().ToString(), Value = "部门菜单" };
            subList1.Add(subModel1_1);
            subList1.Add(subModel1_2);
            subList1.Add(subModel1_3);
            subList1.Add(subModel1_4);
            subList1.Add(subModel1_5);
            action(subModel1_1);
            action(subModel1_2);
            action(subModel1_3);
            action(subModel1_4);
            action(subModel1_5);
            var menu1 = new HHH { Key = Guid.NewGuid().ToString(), Value = "系统管理", DataList = subList1 };

            var subList2 = new List<HHH>();
            var subModel2_1 = new HHH { Key = Guid.NewGuid().ToString(), Value = "医院管理" };
            var subModel2_2 = new HHH { Key = Guid.NewGuid().ToString(), Value = "哈哈管理" };
            subList2.Add(subModel2_1);
            subList2.Add(subModel2_2);
            action(subModel2_1);
            action(subModel2_2);
            var menu2 = new HHH { Key = Guid.NewGuid().ToString(), Value = "科室管理", DataList = subList2 };

            totalMenus.Add(menu1);
            totalMenus.Add(menu2);
            return totalMenus;
        }).Value;

        public List<TreeNode> PermissionTree { get; set; } = new Lazy<List<TreeNode>>(() =>
        {
            Action<TreeNode> action = node =>
            {
                for (int i = 0; i < 5; i++)
                {
                    var bottomModel = new TreeNode { Tag = Guid.NewGuid().ToString(), Label = $"名称{i}{i}{i}{i}{i}", Parent = node };
                    node.ChildNodes.Add(bottomModel);
                }
            };

            var result = new List<TreeNode>();

            var menu1 = new TreeNode { Label = "系统管理", Tag = Guid.NewGuid().ToString() };
            var model1 = new TreeNode { Label = "字典管理", Tag = Guid.NewGuid().ToString(), Parent = menu1 };
            var model2 = new TreeNode { Label = "顶级菜单", Tag = Guid.NewGuid().ToString(), Parent = menu1 };
            var model3 = new TreeNode { Label = "菜单管理", Tag = Guid.NewGuid().ToString(), Parent = menu1 };
            var model4 = new TreeNode { Label = "用户列表", Tag = Guid.NewGuid().ToString(), Parent = menu1 };
            var model5 = new TreeNode { Label = "部门菜单", Tag = Guid.NewGuid().ToString(), Parent = menu1 };

            menu1.ChildNodes.Add(model1);
            menu1.ChildNodes.Add(model2);
            menu1.ChildNodes.Add(model3);
            menu1.ChildNodes.Add(model4);
            menu1.ChildNodes.Add(model5);
            action(model1);
            action(model2);
            action(model3);
            action(model4);
            action(model5);

            var menu2 = new TreeNode { Label = "科室管理", Tag = Guid.NewGuid().ToString() };
            var model2_1 = new TreeNode { Label = "医院管理", Tag = Guid.NewGuid().ToString(), Parent = menu2 };
            var model2_2 = new TreeNode { Label = "哈哈管理", Tag = Guid.NewGuid().ToString(), Parent = menu2 };
            menu2.ChildNodes.Add(model2_1);
            menu2.ChildNodes.Add(model2_2);
            action(model2_1);
            action(model2_2);

            result.Add(menu1);
            result.Add(menu2);
            return result;
        }).Value;
        #endregion TestDicts

        public ICommand TestCommand { get; set; }
        public ICommand ShowSliceCommand { get; set; }
        public ICommand ShowReportCommand { get; set; }
        public TestViewModel()
        {
            this.RegisterCommand();
            // 我的收藏
            this.NPRQuery.MyCollections = new NPRQuery_Collection
            {
                Data = new List<NPRQueryDataTest>
                {
                    new NPRQueryDataTest{ Pathology_No="123",Report_State="1",DeptDoc="ABC/哈哈哈",BaseInfo="怎么想/女/38岁"},
                    new NPRQueryDataTest{ Pathology_No="456",Report_State="2",DeptDoc="DEF/取文去",BaseInfo="不那么/男/80岁"},
                    new NPRQueryDataTest{ Pathology_No="789",Report_State="3",DeptDoc="GHI/成本高",BaseInfo="有魄力/女/20岁"},
                    new NPRQueryDataTest{ Pathology_No="012",Report_State="4",DeptDoc="JKL/接口路",BaseInfo="宣传部/男/301岁"},
                }
            };
            this.NPRQuery.TodayRegister = new NPRQuery_Collection
            {
                Data = new List<NPRQueryDataTest>
                {
                    new NPRQueryDataTest{ Pathology_No="xc",Report_State="6",DeptDoc="x换行/哈哦哈",BaseInfo="lk想/女/381岁"},
                    new NPRQueryDataTest{ Pathology_No="4bn56",Report_State="7",DeptDoc="DE的F/取p去",BaseInfo="老婆么/男/820岁"},
                    new NPRQueryDataTest{ Pathology_No="ghj",Report_State="8",DeptDoc="GH是I/就本高",BaseInfo="濮阳力/女/201岁"},
                    new NPRQueryDataTest{ Pathology_No="34df",Report_State="9",DeptDoc="J是KL/篇口路",BaseInfo="符合部/男/31岁"},
                }
            };
            this.NPRQuery.RecentlyOpen = new NPRQuery_Collection
            {
                Data = new List<NPRQueryDataTest>
                {
                    new NPRQueryDataTest{ Pathology_No="bnm",Report_State="s",DeptDoc="sdc/哈是",BaseInfo="是的v吧/女/81岁"},
                    new NPRQueryDataTest{ Pathology_No="yut",Report_State="7",DeptDoc="收藏的F/取撒",BaseInfo="士大夫/男/20岁"},
                    new NPRQueryDataTest{ Pathology_No="piuhj",Report_State="j",DeptDoc="G飒飒是I/分隔符",BaseInfo="首付挺/女/21岁"},
                    new NPRQueryDataTest{ Pathology_No="21cv",Report_State="k",DeptDoc="撒KL/篇飒飒",BaseInfo="也很难/男/221岁"},
                }
            };

            H_Dict = new List<HHH>();
            for (int i = 0; i < 20; i++)
            {
                var list = new List<HHH>();
                for (int j = 0; j < 5; j++)
                {
                    list.Add(new HHH { Key = $"{i}{i}{i}", Value = $"hhh{i}" });
                }
                var model = new HHH
                {
                    Key = $"{i}",
                    Value = $"XXX{i}",
                    DataList = list
                };
                H_Dict.Add(model);
            }

            this.TestApi();
        }

        protected override void RegisterCommand()
        {
            this.TestCommand = new RelayCommand(() =>
            {
                var xx = this.NPRModel;
                var yy = this.NPRQuery;
            });

            this.ShowSliceCommand = new RelayCommand(() =>
            {
                this.IsSliceOpen = !this.IsSliceOpen;
            });
            this.ShowReportCommand = new RelayCommand(() =>
            {
                this.IsReportOpen = !this.IsReportOpen;
            });
        }

        #region TestProperties

        #region NormalProps
        [Notification]
        public bool IsSliceOpen { get; set; }

        private bool _isReportOpen;
        [Notification]
        public bool IsReportOpen
        {
            get { return _isReportOpen; }
            set
            {
                _isReportOpen = value;
                //Messenger.Default.Send(_isReportOpen, EnumTest.ActivaWin);
            }
        }
        #endregion NormalProps

        #region CollectionProps
        [Notification]
        public NPRModel NPRModel { get; set; } = new NPRModel();
        [Notification]
        public NPRQuery NPRQuery { get; set; } = new NPRQuery();
        [Notification]
        public DMSModel DMSModel { get; set; } = new DMSModel { PatientInfo = new PatInfo { Dept = "神经外科", PatName = "海棠花", SampleCode = "1234567", Sex = "女", Specimen = "子宫肌瘤" } };

        [Notification]
        public EmbedModel EModel { get; set; } = new EmbedModel();
        [Notification]
        public ProductModel PModel { get; set; } = new Framework.Model.Test.ProductModel();
        [Notification]
        public CDModel CdModel { get; set; } = new CDModel();
        [Notification]
        public SysModel SModel { get; set; } = new SysModel();
        #endregion CollectionProps

        #endregion TestProperties

        private void TestApi()
        {
            var xx1 = ApplyFormService.Instance.GetPathInfobyCode("C001-21-000002");
            #region   
            //var xx1 = SysManageService.Instance.GetCSLoginUserMenus(); // 获取登录用户的所有菜单
            //var xx = SysManageService.Instance.GetRoleListTree(new RoleInfoModel { RoleName="",RoleAlias=""}); // 角色树
            ////var xx1_2 = SysManageService.Instance.GetBSLoginUserMenus();
            //var xx2 = SysManageService.Instance.GetMenusbyRoleId("1392400504156524545");
            //var xx3 = SysManageService.Instance.GetCustomMenus();
            //var xx4 = SysManageService.Instance.GetMenuDetailsbyId("1391683281414774787");
            //ApiClientSetting.SetHttpClientEx();
            //var xx = LoginService.Instance.Login(new QueryLoginModel { UserName = "joysola", Password = "sumisora" });
            //var xx1 = SysManageService.Instance.GetUserDetailbyId("1");
            //var xx2 = SysManageService.Instance.GetLoginUserDetail();
            //var xx3 = SysManageService.Instance.GetUserListbyPage(new CustomPageModel { PageSize = 30, PageIndex = 1 }, new UserInfoModel { UserName = "", RealName = "" });

            //var d1 = DictService.Instance.GetSexDict().Result;
            //var joy = xx3.FirstOrDefault(x => x.UserName == "joysola");
            //SysManageService.Instance.DeleteUser(joy);
            //var xx4 = SysManageService.Instance.AddUser(new UserInfoModel { UserName = "xwl", RealName = "许文龙" });
            //var xx5 = SysManageService.Instance.AddUser(new UserInfoModel { UserName = "joysola2", RealName = "厨卫间" });
            //var xx6 = SysManageService.Instance.AddUser(new UserInfoModel { UserName = "joysola3", RealName = "出无尽" });
            //var xx7 = SysManageService.Instance.AddUser(new UserInfoModel { UserName = "joysola4", RealName = "除味剂" });
            //var xx8 = SysManageService.Instance.GetUserListbyPage(new CustomPageModel { PageSize = 30, PageIndex = 1 }, new UserInfoModel { UserName = "", RealName = "" });
            //var deleteList = xx8.Where(x => x.UserName.Contains("xxx"));
            //var xx9 = SysManageService.Instance.DeleteUserList(deleteList);

            //joy.Sex = 1;
            //joy.UserName = "joysola";
            //joy.Password = "hhh";
            //var xx10 = SysManageService.Instance.UpdateUser(joy);

            //var xx11 = SysManageService.Instance.ResetPassword(joy);
            //SysManageService.Instance.UpdatePassword(joy, "123456", "sumisora");
            //LoginService.Instance.Login(new QueryLoginModel { UserName = "joysola", Password = "sumisora" });


            //var xx12 = SysManageService.Instance.LockUser(joy, true);
            //var xx122 = SysManageService.Instance.LockUser(joy, false);

            //var xx13 = SysManageService.Instance.UpdatePassword(joy, "sumisora", "Sumisora");
            //LoginService.Instance.Login(new QueryLoginModel { UserName = "joysola", Password = "Sumisora" });
            //SysManageService.Instance.GetUserListbyPage(new CustomPageModel { PageSize = 30, PageIndex = 1 }, new UserInfoModel { UserName = "", RealName = "" });


            //SysManageService.Instance.UpdatePassword(joy, "Sumisora", "sumisora");
            //LoginService.Instance.Login(new QueryLoginModel { UserName = "joysola", Password = "sumisora" });
            //SysManageService.Instance.GetUserListbyPage(new CustomPageModel { PageSize = 30, PageIndex = 1 }, new UserInfoModel { UserName = "", RealName = "" });
            #endregion
        }
    }
}
