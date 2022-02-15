using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.PIMS.Framework.Model
{
    /// <summary>
    /// 树形控件的节点对象
    /// </summary>
    public class TreeNode : ObservableObject
    {
        private bool? _isSelected = false;
        private ObservableCollection<TreeNode> childNodes = null;

        public string Label { get; set; }
        [Notification]
        public bool? IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                // 向下
                this.CalculateIsSelected();
            }
        }
        public int Level { get; set; }

        public dynamic Tag { get; set; }

        public TreeNode Parent { get; set; }
        /// <summary>
        /// 可见性
        /// </summary>
        [Notification]
        public Visibility Visible { get; set; } = Visibility.Visible;

        public IList<TreeNode> ChildNodes
        {
            get
            {
                return childNodes ?? (childNodes = new ObservableCollection<TreeNode>());
            }
        }

        /// <summary>
        /// 计算是否需要勾选CheckBox
        /// </summary>
        private void CalculateIsSelected()
        {
            // 1. 向下赋值(存在子项，且子项有值)
            if (this.ChildNodes.Count > 0 && _isSelected.HasValue)
            {
                this.ChildNodes.ToList().ForEach(node => node.IsSelected = _isSelected);
            }
            // 向下时，若子项IsSelected和父项IsSelected 相同，认为 向下传播ing，不需要触发 向上反推
            if (this.Parent != null && _isSelected.HasValue && this.Parent.IsSelected.HasValue && _isSelected == this.Parent.IsSelected)
            {
                return;
            }
            // 2. 向上反推
            if (this.Parent != null && this.Parent.ChildNodes.Count > 0)
            {
                var sameLevelChildren = this.Parent.ChildNodes;

                if (sameLevelChildren.All(x => x.IsSelected == true)) // 同级全为true
                {
                    this.Parent.IsSelected = true;
                }
                else if (sameLevelChildren.All(x => x.IsSelected == false)) // 统计全为false
                {
                    this.Parent.IsSelected = false;
                }
                else // 统计 什么都有
                {
                    this.Parent.IsSelected = null;
                }
            }
        }
    }
}
