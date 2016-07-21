using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory to define TreeViewNode objects for collections.
  /// </summary>
    public class TreeViewNodeFactory
    {
        private TreeViewNodeCollection nodes;
      /// <summary>
        /// Factory to define TreeViewNode objects for collections.
      /// </summary>
      /// <param name="nodes"></param>
        public TreeViewNodeFactory(TreeViewNodeCollection nodes)
        {
            this.nodes = nodes;
        }
      /// <summary>
        /// Add a TreeViewNode to a collection.
      /// </summary>
      /// <returns></returns>
        public virtual TreeViewNodeBuilder Add()
        {
            TreeViewNode node = new TreeViewNode();

            nodes.Add(node);

            return new TreeViewNodeBuilder(node);
        }
    }
}
