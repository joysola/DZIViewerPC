using DST.PIMS.Framework.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Extensions
{
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// 从已经构建完成的普通树里找到各个节点的父节点
        /// </summary>
        /// <param name="treeNodes"></param>
        public static void FindParent(this IList<TreeNode> treeNodes, TreeNode parent = null)
        {
            if (treeNodes != null)
            {
                foreach (var node in treeNodes)
                {
                    node.Parent = parent;
                    foreach (var chid in node.ChildNodes)
                    {
                        chid.Parent = node;
                        FindParent(chid.ChildNodes, chid);
                    }
                }
            }
        }
    }
}
