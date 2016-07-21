using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define TabStripTab objects.
  /// </summary>
    public class TabStripTabBuilder
    {
        private readonly TabStripTab tab;
      /// <summary>
        /// Builder to define TabStripTab objects.
      /// </summary>
      /// <param name="tab"></param>
        public TabStripTabBuilder(TabStripTab tab)
        {
            this.tab = tab;
        }
      /// <summary>
        /// Collection of immediate child TabStripTabs. 
      /// </summary>
      /// <param name="addAction"></param>
      /// <returns></returns>
        public TabStripTabBuilder Tabs(Action<TabStripTabFactory> addAction)
        {
            var factory = new TabStripTabFactory(tab.Tabs);
            addAction(factory);
            return this;
        }
      /// <summary>
        /// Whether to perform a postback when this node is selected. Default: false. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder AutoPostBackOnSelect(bool value)
        {
            tab.AutoPostBackOnSelect = value;
            return this;
        }
      /// <summary>
        /// Client-side command to execute on selection. This can be any valid client script. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder ClientSideCommand(string value)
        {
            tab.ClientSideCommand = value;
            return this;
        }
      /// <summary>
        /// ID of the client template to use for this node. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder ClientTemplateId(string value)
        {
            tab.ClientTemplateId = value;

            return this;
        }
      /// <summary>
        /// Default CSS class to use for this node. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder CssClass(string value)
        {
            tab.CssClass = value;
            return this;
        }
      /// <summary>
        /// ID of this node.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder ID(string value)
        {
            tab.ID = value;
            return this;
        }
      /// <summary>
        /// Whether this navigation node is enabled.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder Enabled(bool value)
        {
            tab.Enabled = value;
            return this;
        }
      /// <summary>
        /// String representing keyboard shortcut for selecting this node.
        /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder KeyboardShortcut(string value)
        {
            tab.KeyboardShortcut = value;
            return this;
        }
      /// <summary>
        /// URL to navigate to when this node is selected.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder NavigateUrl(string value)
        {
            tab.NavigateUrl = value;
            return this;
        }
      /// <summary>
        /// How tabs in subgroups are aligned. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupAlign(TabStripAlign value)
        {
            tab.DefaultSubGroupAlign = value;
            return this;
        }
      /// <summary>
        /// Default CSS class to apply to sub-groups below this item, including this item's subgroup. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupCssClass(string value)
        {
            tab.DefaultSubGroupCssClass = value;
            return this;
        }
      /// <summary>
        /// Offset along x-axis from subgroups' normal expand positions. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupExpandOffsetX(int value)
        {
            tab.DefaultSubGroupExpandOffsetX = value;
            return this;
        }
      /// <summary>
        /// Offset along y-axis from subgroups' normal expand positions. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupExpandOffsetY(int value)
        {
            tab.DefaultSubGroupExpandOffsetY = value;
            return this;
        }    
      /// <summary>
        /// Default first separator height. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupFirstSeparatorHeight(int value)
        {
            tab.DefaultSubGroupFirstSeparatorHeight = value;
            return this;
        }
      /// <summary>
        /// Default first separator width. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupFirstSeparatorWidth(int value)
        {
            tab.DefaultSubGroupFirstSeparatorWidth = value;
            return this;
        }
      /// <summary>
        /// Folder with default group separator images. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupSeparatorImagesFolderUrl(string value)
        {
            tab.DefaultSubGroupSeparatorImagesFolderUrl = value;
            return this;
        }
      /// <summary>
        /// Whether to show separator images for the subgroups. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupShowSeparators(bool value)
        {
            tab.DefaultSubGroupShowSeparators = value;
            return this;
        }
      /// <summary>
        /// Spacing between subgroups' tabs. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupTabSpacing(int value)
        {
            tab.DefaultSubGroupTabSpacing = value;
            return this;
        }
      /// <summary>
        /// Width of subgroups. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupWidth(int value)
        {
            tab.DefaultSubGroupWidth = value;
            return this;
        }
      /// <summary>
        /// Height of subgroups. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubGroupHeight(int value)
        {
            tab.DefaultSubGroupHeight = value;
            return this;
        }
      /// <summary>
        /// How tabs in the subgroup are aligned. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupAlign(TabStripAlign value)
        {
            tab.DefaultSubGroupAlign = value;
            return this;
        }
      /// <summary>
        /// Subgroup's CSS class. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupCssClass(string value)
        {
            tab.DefaultSubGroupCssClass = value;
            return this;
        }
      /// <summary>
        /// Offset along x-axis from subgroup's normal expand position. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupExpandOffsetX(int value)
        {
            tab.SubGroupExpandOffsetX = value;
            return this;
        }
      /// <summary>
        /// Offset along y-axis from subgroup's normal expand position. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupExpandOffsetY(int value)
        {
            tab.SubGroupExpandOffsetY = value;
            return this;
        }
      /// <summary>
        /// Height of the subgroup's first separator. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupFirstSeparatorHeight(int value)
        {
            tab.SubGroupFirstSeparatorHeight = value;
            return this;
        }
      /// <summary>
        /// Width of the subgroup's first separator. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupFirstSeparatorWidth(int value)
        {
            tab.SubGroupFirstSeparatorWidth = value;
            return this;
        }
      /// <summary>
        /// Height of the subgroup's last separator. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupLastSeparatorHeight(int value)
        {
            tab.SubGroupLastSeparatorHeight = value;
            return this;
        }
      /// <summary>
        /// Width of the subgroup's last separator. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupLastSeparatorWidth(int value)
        {
            tab.SubGroupLastSeparatorWidth = value;
            return this;
        }
      /// <summary>
        /// Folder with the subgroup's separator images. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupSeparatorImagesFolderUrl(string value)
        {
            tab.SubGroupSeparatorImagesFolderUrl = value;
            return this;
        }
      /// <summary>
        /// Whether to show separator images for the immediate subgroup. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupShowSeparators(bool value)
        {
            tab.SubGroupShowSeparators = value;
            return this;
        }
      /// <summary>
        /// Spacing between subgroup's tabs. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupTabSpacing(int value)
        {
            tab.SubGroupTabSpacing = value;
            return this;
        }
      /// <summary>
        /// Width of subgroup. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupWidth(int value)
        {
            tab.SubGroupWidth = value;
            return this;
        }
      /// <summary>
        /// Height of subgroup. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SubGroupHeight(int value)
        {
            tab.SubGroupHeight = value;
            return this;
        }
      /// <summary>
        /// The default text alignment to apply to labels of sub-items. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubItemTextAlign(TextAlign value)
        {
            tab.DefaultSubItemTextAlign = value;
            return this;
        }
      /// <summary>
        /// Whether to wrap text in sub-item labels by default.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder DefaultSubItemTextWrap(bool value)
        {
            tab.DefaultSubItemTextWrap = value;
            return this;
        }
      /// <summary>
        /// ID of PageView to switch the target ComponentArt MultiPage control to when this node is selected. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder PageViewId(string value)
        {
            tab.PageViewId = value;
            return this;
        }
      /// <summary>
        /// XML file to load the substructure of this node from. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder SiteMapXmlFile(string value)
        {
            tab.SiteMapXmlFile = value;
            return this;
        }
      /// <summary>
        /// Target frame for this node's navigation.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder Target(string value)
        {
            tab.Target = value;
            return this;
        }
      /// <summary>
        /// Text label of this node.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder Text(string value)
        {
            tab.Text = value;
            return this;
        }
      /// <summary>
        /// ToolTip to display for this item. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder ToolTip(string value)
        {
            tab.ToolTip = value;
            return this;
        }
      /// <summary>
        /// Optional internal string value of this node.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder Value(string value)
        {
            tab.Value = value;
            return this;
        }
      /// <summary>
        /// Whether this node should be displayed.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder Visible(bool value)
        {
            tab.Visible = value;
            return this;
        }
      /// <summary>
        /// The text alignment to apply to the label of this node. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder TextAlign(TextAlign value)
        {
            tab.TextAlign = value;
            return this;
        }
      /// <summary>
        /// Whether to wrap text in this node's label. 
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
        public TabStripTabBuilder TextWrap(bool value)
        {
            tab.TextWrap = value;
            return this;
        }
    }
}
