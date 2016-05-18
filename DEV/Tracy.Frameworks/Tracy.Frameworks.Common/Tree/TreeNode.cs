using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tracy.Frameworks.Common.Tree
{
    /// <summary>
    /// 樹形菜單節點
    /// </summary>
    /// <typeparam name="TNode">樹形菜單節點內容類別型</typeparam>
    [DataContract]
    public class TreeNode<TNode>
    {
        /// <summary>
        /// 樹形功能表節點內容
        /// </summary>
        [DataMember]
        public TNode Content { get; set; }

        /// <summary>
        /// 樹形菜單節點子集
        /// </summary>
        [DataMember]
        public List<TreeNode<TNode>> Children { get; set; }
    }
}
