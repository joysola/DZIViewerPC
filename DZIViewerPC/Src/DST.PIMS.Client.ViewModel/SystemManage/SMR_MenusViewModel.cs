using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework.Extensions;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
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
    public class SMR_MenusViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 该角色已经选中的菜单
        /// </summary>
        private List<string> SelectedMenus { get; set; }
        /// <summary>
        /// 菜单树
        /// </summary>
        private List<MenuInfoModel> AllMenuTrees { get; set; }
        /// <summary>
        /// 角色信息
        /// </summary>
        private RoleInfoModel RoleInfo { get; set; }
        /// <summary>
        /// 中台需要的结果
        /// </summary>
        private QueryMenusSetting MenusSetting { get; set; } = new QueryMenusSetting();
        /// <summary>
        /// 通用树
        /// </summary>
        [Notification]
        public ObservableCollection<TreeNode> MenuTreeNodes { get; set; } = new ObservableCollection<TreeNode>();
        /// <summary>
        /// 取消
        /// </summary>
        public ICommand CancelCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.CloseContentWindow();
        })).Value;
        /// <summary>
        /// 保存
        /// </summary>
        public ICommand SaveCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            MenusSetting.ID = RoleInfo.ID;
            GetMenusSetting(MenuTreeNodes); // 从普通书找到需要的信息
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                var result = SysManageService.Instance.UpdateRoleMenus(MenusSetting);
                if (result)
                {
                    this.Result = true;
                    this.CloseContentWindow();
                }
                else
                {
                    ShowMessageBox($"设置角色权限失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 3
                && this.Args[0] is List<string> selectedMenuIDs
                && this.Args[1] is List<MenuInfoModel> allMenuTrees
                && this.Args[2] is RoleInfoModel role)
            {
                this.SelectedMenus = selectedMenuIDs;
                this.AllMenuTrees = allMenuTrees;
                this.RoleInfo = role;
                // 将菜单树转换为通用树
                this.AllMenuTrees.ForEach(menu =>
                {
                    MenuTreeNodes.Add(GenerateTreeNode(menu));
                });
                MenuTreeNodes.FindParent(); // 将通用树的父节点进行设置
                SetTreeNodeSelect(MenuTreeNodes); // 设置是否选中
            }
        }
        /// <summary>
        /// 创建通用树节点
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private TreeNode GenerateTreeNode(MenuInfoModel menu)
        {
            if (menu != null)
            {
                var treeNode = new TreeNode
                {
                    Label = menu.Name,
                    Tag = menu.ID,
                    // 不可以在这里设置是否选中，这样会无法触发层级选中，因为parent不存在
                    //IsSelected = SelectedMenus.Exists(x => x == menu.ID),
                };
                if (menu.Children != null)
                {
                    foreach (var chid in menu.Children)
                    {
                        treeNode.ChildNodes.Add(GenerateTreeNode(chid));
                    }
                }
                return treeNode;
            }
            return null;
        }
        /// <summary>
        /// 设置通用树是否选中
        /// </summary>
        /// <param name="treeNodes"></param>
        private void SetTreeNodeSelect(IList<TreeNode> treeNodes)
        {
            foreach (var node in treeNodes)
            {
                node.IsSelected = SelectedMenus.Exists(x => x == node.Tag);
                SetTreeNodeSelect(node.ChildNodes);
            }
        }
        /// <summary>
        /// 将通用树转换为需要的信息
        /// </summary>
        /// <param name="treeNode"></param>
        private void GetMenusSetting(IList<TreeNode> treeNodes)
        {
            foreach (var treeNode in treeNodes)
            {
                if (treeNode.IsSelected == true)
                {
                    MenusSetting.MenuIDs.Add(treeNode.Tag);
                }
                else if (!treeNode.IsSelected.HasValue)
                {
                    MenusSetting.HalfMenuIDs.Add(treeNode.Tag);
                }
                GetMenusSetting(treeNode.ChildNodes);
            }
        }
    }
}
