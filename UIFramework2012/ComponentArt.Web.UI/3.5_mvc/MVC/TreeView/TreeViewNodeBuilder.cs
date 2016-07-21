using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder to define TreeViewNode objects.
  /// </summary>
    public class TreeViewNodeBuilder
    {
        private readonly TreeViewNode node;
/// <summary>
        /// Builder to define TreeViewNode objects.
/// </summary>
/// <param name="node"></param>
        public TreeViewNodeBuilder(TreeViewNode node)
        {
            this.node = node;
        }
      /// <summary>
        /// Collection of this node's child nodes. 
      /// </summary>
      /// <param name="addAction"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder Nodes(Action<TreeViewNodeFactory> addAction)
        {
            var factory = new TreeViewNodeFactory(node.Nodes);
            addAction(factory);
            return this;
        }
      /// <summary>
        /// Whether to perform a post when this node is selected. Default: false. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder AutoPostBackOnSelect(bool value)
        {
            node.AutoPostBackOnSelect = value;
            return this;
        }
      /// <summary>
        /// Client-side command to execute on selection. This can be any valid client script. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder ClientSideCommand(string value)
        {
            node.ClientSideCommand = value;
            return this;
        }
      /// <summary>
        /// ID of the client template to use for this node. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder ClientTemplateId(string value)
        {
            node.ClientTemplateId = value;

            return this;
        }
        /// <summary>
        /// Default CSS class to use for this node.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TreeViewNodeBuilder CssClass(string value)
        {
            node.CssClass = value;
            return this;
        }
      /// <summary>
        /// URL to load sub-tree contents from XML. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder ContentCallbackUrl(string value)
        {
          node.ContentCallbackUrl = value;
          return this;
        }
/// <summary>
/// CSS class to apply in a Themes' designated icon area for this item.
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
        public TreeViewNodeBuilder IconCssClass(string value)
        {
          node.Properties["IconCssClass"] = value;
          return this;
        }
      /// <summary>
        /// Image to draw in a Themes' designated icon area for this item.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder IconUrl(string value)
        {
          node.Properties["IconUrl"] = value;
          return this;
        }
      /// <summary>
      /// ID of this node.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder ID(string value)
        {
            node.ID = value;
            return this;
        }
      /// <summary>
        /// Whether this navigation node is enabled. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder Enabled(bool value)
        {
            node.Enabled = value;
            return this;
        }
      /// <summary>
        /// String representing keyboard shortcut for selecting this node. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder KeyboardShortcut(string value)
        {
            node.KeyboardShortcut = value;
            return this;
        }
      /// <summary>
        /// URL to navigate to when this node is selected. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder NavigateUrl(string value)
        {
            node.NavigateUrl = value;
            return this;
        }
      /// <summary>
        /// ID of PageView to switch the target ComponentArt MultiPage control to when this node is selected.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder PageViewId(string value)
        {
            node.PageViewId = value;
            return this;
        }
      /// <summary>
        /// XML file to load the substructure of this node from. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder SiteMapXmlFile(string value)
        {
            node.SiteMapXmlFile = value;
            return this;
        }
      /// <summary>
        /// Target frame for this node's navigation.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder Target(string value)
        {
            node.Target = value;
            return this;
        }
      /// <summary>
        /// Text label of this node. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder Text(string value)
        {
            node.Text = value;
            return this;
        }
      /// <summary>
        /// ToolTip to display for this item.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder ToolTip(string value)
        {
            node.ToolTip = value;
            return this;
        }
      /// <summary>
        /// Whether this node should be populated using the specified web service. Default: false. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder UseWebService(bool value)
        {
          node.UseWebService = value;
          return this;
        }
      /// <summary>
        /// Optional internal string value of this node. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder Value(string value)
        {
            node.Value = value;
            return this;
        }
      /// <summary>
        /// Whether this node should be displayed.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TreeViewNodeBuilder Visible(bool value)
        {
            node.Visible = value;
            return this;
        }
    }
}
