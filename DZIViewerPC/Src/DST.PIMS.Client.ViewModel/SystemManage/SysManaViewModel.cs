using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DST.PIMS.Client.ViewModel
{
    public class SysManaViewModel : CustomBaseViewModel
    {
        private TreeNode _selectedNode;
        public SysManaRoleViewModel RoleVM { get; set; } = new SysManaRoleViewModel();
        public SysManaDocViewModel DocVM { get; set; } = new SysManaDocViewModel();
        public SysManaHospViewModel HospVM { get; set; } = new SysManaHospViewModel();
        /// <summary>
        /// 菜单树
        /// </summary>
        [Notification]
        public ObservableCollection<TreeNode> MenuTree => new Lazy<ObservableCollection<TreeNode>>(() =>
        {
            // 1.
            var sysNode = new TreeNode
            {
                Label = "系统管理",
                Tag = new MenuInfoModel { Code = "systemes", Url = "/DST.PIMS.Client;component/Images/Test/Menu3.png" }
            };
            // 系统配置权限判断
            var buttonList = ExtendAppContext.Current.LoginMenu.FirstOrDefault(x => x.Code == nameof(EnumWorkstationType.SystemManage))?.CodeList ?? new List<string>();
            // 1-1. 角色
            if (buttonList.Exists(x => x == RoleNode.Tag.Code))
            {
                MenuList.Add(RoleNode);
                RoleNode.Parent = sysNode;
                sysNode.ChildNodes.Add(RoleNode);
            }
            // 2.
            var hospNode = new TreeNode
            {
                Label = "医院管理",
                Tag = new MenuInfoModel { Code = "hospmanage", Url = "/DST.PIMS.Client;component/Images/Test/Menu3.png" }
            };
            // 2-2. 医院（用户）管理
            if (buttonList.Exists(x => x == UserNode.Tag.Code))
            {
                MenuList.Add(UserNode);
                UserNode.Parent = hospNode;
                hospNode.ChildNodes.Add(UserNode);
            }
            // 2-3. 医生管理
            if (buttonList.Exists(x => x == DocNode.Tag.Code))
            {
                MenuList.Add(DocNode);
                DocNode.Parent = hospNode;
                hospNode.ChildNodes.Add(DocNode);
            }
           
            var list = new List<TreeNode> { sysNode, hospNode };
            return new ObservableCollection<TreeNode>(list);
        }).Value;

        #region 可使用菜单
        /// <summary>
        /// 业务角色管理
        /// </summary>
        [Notification]
        public TreeNode RoleNode { get; } = new TreeNode
        {
            Label = "业务角色管理",
            Tag = new MenuInfoModel { Code = "Business-RoleManage", Url = "/DST.PIMS.Client;component/Images/Test/Menu2.png" },
            Visible = Visibility.Visible
        };
        /// <summary>
        /// 医院管理
        /// </summary>
        [Notification]
        public TreeNode UserNode { get; } = new TreeNode
        {
            Label = "医院管理",
            Tag = new MenuInfoModel { Code = "Hosp-UserManage", Url = "/DST.PIMS.Client;component/Images/Test/Menu4.png" },
            Visible = Visibility.Collapsed
        };
        /// <summary>
        /// 医生管理
        /// </summary>
        [Notification]
        public TreeNode DocNode { get; } = new TreeNode
        {
            Label = "医生管理",
            Tag = new MenuInfoModel { Code = "Hosp-DocManage", Url = "/DST.PIMS.Client;component/Images/Test/Menu5.png" },
            Visible = Visibility.Collapsed
        };

        #endregion 可使用菜单
        /// <summary>
        /// 可使用菜单集合
        /// </summary>
        private List<TreeNode> MenuList { get; set; } = new List<TreeNode>();

        /// <summary>
        /// 菜单选中项目
        /// </summary>
        [Notification]
        public TreeNode SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;
                // 未选中 或 选中的不在菜单中 则不操作
                if (_selectedNode != null && MenuList.Exists(x => x.Tag.Code == _selectedNode.Tag.Code))
                {
                    foreach (var menu in MenuList)
                    {
                        if (menu.Tag.Code == _selectedNode.Tag.Code) // 相同code显示
                        {
                            menu.Visible = Visibility.Visible;
                        }
                        else // 其余关闭
                        {
                            menu.Visible = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        public SysManaViewModel()
        {
            RoleVM.UpdateCommand = HospVM.InitCommand;
        }


    }
}
