using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model
{
    /// <summary>
    /// 树形控件的节点数据
    /// </summary>
    public class TreeViewData
    {
        private ObservableCollection<TreeNode> rootNodes = null;

        public IList<TreeNode> RootNodes
        {
            get
            {
                return rootNodes ?? (rootNodes = new ObservableCollection<TreeNode>());
            }
        }
    }
}
