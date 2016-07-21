using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define a TreeView object.
  /// </summary>
  public class TreeViewBuilder : ControlBuilder
  {
    TreeView treeview = new TreeView();
    /// <summary>
    /// Builder to generate a TreeView object on the client.
    /// </summary>
    public TreeViewBuilder()
    {

    }
    /// <summary>
    /// Programmatic identifier assigned to the object. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TreeViewBuilder ID(string id)
    {
      treeview.ID = id;
      return this;
    }
    /// <summary>
    /// Collection of root TreeViewNodes.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public TreeViewBuilder Nodes(Action<TreeViewNodeFactory> addAction)
    {
      var factory = new TreeViewNodeFactory(treeview.Nodes);
      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of client event handler definitions.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public TreeViewBuilder ClientEvents(Action<TreeViewClientEventFactory> addAction)
    {
      var factory = new TreeViewClientEventFactory(treeview.ClientEvents);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of client-templates that may be used by this TreeView. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public TreeViewBuilder ClientTemplates(Action<ClientTemplateFactory> addAction)
    {
      var factory = new ClientTemplateFactory(treeview.ClientTemplates);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Custom attribute mappings. Provides the ability to re-map property names when they are looked up in XML. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public TreeViewBuilder CustomAttributeMappings(Action<CustomAttributeMappingFactory> addAction)
    {
      var factory = new CustomAttributeMappingFactory(treeview.CustomAttributeMappings);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Whether to allow text selection in this TreeView. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder AllowTextSelection(bool value)
    {
      treeview.AllowTextSelection = value;
      return this;
    }
    /// <summary>
    /// Whether to automatically assign IDs to nodes programmatically created on the client. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder AutoAssignNodeIDs(bool value)
    {
      treeview.AutoAssignNodeIDs = value;
      return this;
    }
    /// <summary>
    /// Whether to apply scrollbars to the TreeView frame as its contents grow, keepings its dimensions constant,
    /// instead of letting the frame grow with the content. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder AutoScroll(bool value)
    {
      treeview.AutoScroll = value;
      return this;
    }
    /// <summary>
    /// Whether to use predefined CSS classes for theming.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder AutoTheming(bool value)
    {
      treeview.AutoTheming = value;
      return this;
    }
    /// <summary>
    /// String to be prepended to CSS classes used in theming. Default is 'cart-'.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder AutoThemingCssClassPrefix(string value)
    {
      treeview.AutoThemingCssClassPrefix = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for all non-image URL paths. For images, use ImagesBaseUrl. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder BaseUrl(string value)
    {
      treeview.BaseUrl = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to ancestors of the selected node, on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ChildSelectedHoverNodeCssClass(string value)
    {
      treeview.ChildSelectedHoverNodeCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the rows of ancestors of the selected node, on hover.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ChildSelectedHoverNodeRowCssClass(string value)
    {
      treeview.ChildSelectedHoverNodeRowCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to ancestors of the selected node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ChildSelectedNodeCssClass(string value)
    {
      treeview.ChildSelectedNodeCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to rows of ancestors of the selected node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ChildSelectedNodeRowCssClass(string value)
    {
      treeview.ChildSelectedNodeRowCssClass = value;
      return this;
    }
    /// <summary>
    /// Relative or absolute path to the folder containing the client-side script file(s). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ClientScriptLocation(string value)
    {
      treeview.ClientScriptLocation = value;
      return this;
    }
    /// <summary>
    /// Specifies the level of client-side content that the control renders. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ClientTarget(ClientTargetLevel value)
    {
      treeview.ClientTarget = value;
      return this;
    }
    /// <summary>
    /// Whether to collapse a parent node when it is selected on the client. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder CollapseNodeOnSelect(bool value)
    {
      treeview.CollapseNodeOnSelect = value;
      return this;
    }
    /// <summary>
    /// The image to show in place of the expand/collapse image to indicate that child nodes
    /// are loading (for load-on-demand nodes). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ContentLoadingImageUrl(string value)
    {
      treeview.ContentLoadingImageUrl = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for nodes when they are cut (with Ctrl+X). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder CutNodeCssClass(string value)
    {
      treeview.CutNodeCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for rows of nodes when they are cut (with Ctrl+X). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder CutNodeRowCssClass(string value)
    {
      treeview.CutNodeRowCssClass = value;
      return this;
    }
    /// <summary>
    /// Default height to use for margin images, in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DefaultMarginImageHeight(int value)
    {
      treeview.DefaultMarginImageHeight = value;
      return this;
    }
    /// <summary>
    /// Default width to use for margin images, in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DefaultMarginImageWidth(int value)
    {
      treeview.DefaultMarginImageWidth = value;
      return this;
    }
    /// <summary>
    /// Default target (frame or window) to use when navigating. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DefaultTarget(string value)
    {
      treeview.DefaultTarget = value;
      return this;
    }
    /// <summary>
    /// Whether to display a margin on the left-hand side of the TreeView. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DisplayMargin(bool value)
    {
      treeview.DisplayMargin = value;
      return this;
    }
    /// <summary>
    /// Whether dragging to and dropping from another TreeView is enabled by default. Default: false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DragAndDropAcrossTreesEnabled(bool value)
    {
      treeview.DragAndDropAcrossTreesEnabled = value;
      return this;
    }
    /// <summary>
    /// Whether dragging and dropping is enabled by default. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DragAndDropEnabled(bool value)
    {
      treeview.DragAndDropEnabled = value;
      return this;
    }
    /// <summary>
    /// The delay (in milliseconds) after which to expand collapsed parent nodes when hovering over them while dragging. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DragHoverExpandDelay(int value)
    {
      treeview.DragHoverExpandDelay = value;
      return this;
    }
    /// <summary>
    /// CSS class to use on items when about to drop into them (and create new children for them). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DropChildCssClass(string value)
    {
      treeview.DropChildCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to allow dropping to create child nodes (drop into nodes). Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DropChildEnabled(bool value)
    {
      treeview.DropChildEnabled = value;
      return this;
    }
    /// <summary>
    /// Whether to allow dropping to create new root nodes, via drag/drop. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DropRootEnabled(bool value)
    {
      treeview.DropRootEnabled = value;
      return this;
    }
    /// <summary>
    /// CSS class to use on the visual feedback element (a black line by default) when about to drop a sibling. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DropSiblingCssClass(string value)
    {
      treeview.DropSiblingCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to allow dropping to create siblings (drop between nodes). Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DropSiblingEnabled(bool value)
    {
      treeview.DropSiblingEnabled = value;
      return this;
    }
    /// <summary>
    /// Whether to render expand/collapse images at the left edge of the TreeView (before
    /// the indentation) instead of just before the node. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandCollapseInFront(bool value)
    {
      treeview.ExpandCollapseInFront = value;
      return this;
    }
    /// <summary>
    /// Default icon to use for expanded parent nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandedParentNodeImageUrl(string value)
    {
      treeview.ExpandedParentNodeImageUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to expand a parent node when it is selected on the client. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandNodeOnSelect(bool value)
    {
      treeview.ExpandNodeOnSelect = value;
      return this;
    }
    /// <summary>
    /// Whether to force expansion of the path leading down to the selected node. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandSelectedPath(bool value)
    {
      treeview.ExpandSelectedPath = value;
      return this;
    }
    /// <summary>
    /// Default width to apply to expand/collapse images, in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandCollapseImageWidth(int value)
    {
      treeview.ExpandCollapseImageWidth = value;
      return this;
    }
    /// <summary>
    /// Default height to apply to expand/collapse images, in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandCollapseImageHeight(int value)
    {
      treeview.ExpandCollapseImageHeight = value;
      return this;
    }
    /// <summary>
    /// The image (often a 'minus') indicating the collapsibility of a parent node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder CollapseImageUrl(string value)
    {
      treeview.CollapseImageUrl = value;
      return this;
    }
    /// <summary>
    /// The slide type to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandSlide(SlideType value)
    {
      treeview.ExpandSlide = value;
      return this;
    }
    /// <summary>
    /// The duration (in milliseconds) of the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandDuration(int value)
    {
      treeview.ExpandDuration = value;
      return this;
    }
    /// <summary>
    /// Whether to collapse all other paths when expanding a node,
    /// ensuring that only one path is expanded. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandSinglePath(bool value)
    {
      treeview.ExpandSinglePath = value;
      return this;
    }
    /// <summary>
    /// Image (often a 'plus') to use to indicate expandability of parent nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandImageUrl(string value)
    {
      treeview.ExpandImageUrl = value;
      return this;
    }
    /// <summary>
    /// The transition effect to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandTransition(TransitionType value)
    {
      treeview.ExpandTransition = value;
      return this;
    }
    /// <summary>
    /// The string defining a custom transition filter to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExpandTransitionCustomFilter(string value)
    {
      treeview.ExpandTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// A comma-delimited list of IDs of DOM elements on which dropping nodes from this TreeView is allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExternalDropTargets(string value)
    {
      treeview.ExternalDropTargets = value;
      return this;
    }
    /// <summary>
    /// Whether to take on dimensions which fill the containing DOM element entirely. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder FillContainer(bool value)
    {
      treeview.FillContainer = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the TreeView frame when it has keyboard focus. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder FocusedCssClass(string value)
    {
      treeview.FocusedCssClass = value;
      return this;
    }
    /// <summary>
    /// ID of node to forcefully highlight. This will make it appear as it would when selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ForceHighlightedNodeID(string value)
    {
      treeview.ForceHighlightedNodeID = value;
      return this;
    }
    /// <summary>
    /// Whether to force the rendering of the search engine structure. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ForceSearchEngineStructure(bool value)
    {
      treeview.ForceSearchEngineStructure = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use for nodes on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder HoverNodeRowCssClass(string value)
    {
      treeview.HoverNodeRowCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to show hover popups on obscured nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder HoverPopupEnabled(bool value)
    {
      treeview.HoverPopupEnabled = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use for hover popups on obscured nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder HoverPopupNodeCssClass(string value)
    {
      treeview.HoverPopupNodeCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to enable node editing (triggered by clicking twice on a node, as in Windows Explorer). Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder NodeEditingEnabled(bool value)
    {
      treeview.NodeEditingEnabled = value;
      return this;
    }
    /// <summary>
    /// Spacing (in pixels) to render between node rows. Default: 0. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ItemSpacing(int value)
    {
      treeview.ItemSpacing = value;
      return this;
    }
    /// <summary>
    /// Whether to enable keyboard cut/copy/pasting of nodes via Ctrl+X/C/V. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder KeyboardCutCopyPasteEnabled(bool value)
    {
      treeview.KeyboardCutCopyPasteEnabled = value;
      return this;
    }
    /// <summary>
    /// Whether to enable keyboard control. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder KeyboardEnabled(bool value)
    {
      treeview.KeyboardEnabled = value;
      return this;
    }
    /// <summary>
    /// Assigned height of the TreeView object.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder Height(Unit value)
    {
      treeview.Height = value;
      return this;
    }
    /// <summary>
    /// Assigned width of the TreeView object.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder Width(Unit value)
    {
      treeview.Width = value;
      return this;
    }
    /// <summary>
    /// Height to apply on line images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder LineImageHeight(int value)
    {
      treeview.LineImageHeight = value;
      return this;
    }
    /// <summary>
    /// Width to apply on line images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder LineImageWidth(int value)
    {
      treeview.LineImageWidth = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for feedback while loading load-on-demand content. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder LoadingFeedbackCssClass(string value)
    {
      treeview.LoadingFeedbackCssClass = value;
      return this;
    }
    /// <summary>
    /// Text to use for feedback while loading load-on-demand content. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder LoadingFeedbackText(string value)
    {
      treeview.LoadingFeedbackText = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for left-hand margin cells. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder MarginCssClass(string value)
    {
      treeview.MarginCssClass = value;
      return this;
    }
    /// <summary>
    /// Width of left-hand margin in pixels. Default: 32. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder MarginWidth(int value)
    {
      treeview.MarginWidth = value;
      return this;
    }

    /// <summary>
    /// Default height to use for icon images, in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DefaultImageHeight(int value)
    {
      treeview.DefaultImageHeight = value;
      return this;
    }
    /// <summary>
    /// Default width to use for icon images, in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder DefaultImageWidth(int value)
    {
      treeview.DefaultImageWidth = value;
      return this;
    }
    /// <summary>
    /// Padding to include between a node's icon and its label (in pixels). Default: 0.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder NodeLabelPadding(int value)
    {
      treeview.NodeLabelPadding = value;
      return this;
    }
    /// <summary>
    /// Duration (in milliseconds) of the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder CollapseDuration(int value)
    {
      treeview.CollapseDuration = value;
      return this;
    }
    /// <summary>
    /// Slide type to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder CollapseSlide(SlideType value)
    {
      treeview.CollapseSlide = value;
      return this;
    }
    /// <summary>
    /// Transition effect to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder CollapseTransition(TransitionType value)
    {
      treeview.CollapseTransition = value;
      return this;
    }
    /// <summary>
    /// Custom transition filter to use for the collapse animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder CollapseTransitionCustomFilter(string value)
    {
      treeview.CollapseTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// CSS class applied to the frame of the TreeView. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder CssClass(string value)
    {
      treeview.CssClass = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use for the selected node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder SelectedNodeCssClass(string value)
    {
      treeview.SelectedNodeCssClass = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use on the row of the selected node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder SelectedNodeRowCssClass(string value)
    {
      treeview.SelectedNodeRowCssClass = value;
      return this;
    }
    /// <summary>
    /// Default icon to use for the selected node if it is a parent. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder SelectedParentNodeImageUrl(string value)
    {
      treeview.SelectedParentNodeImageUrl = value;
      return this;
    }

    /// <summary>
    /// Default CSS class to use for nodes on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder HoverNodeCssClass(string value)
    {
      treeview.HoverNodeCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to use on the input field while editing a node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder NodeEditCssClass(string value)
    {
      treeview.NodeEditCssClass = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use for node rows. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder NodeRowCssClass(string value)
    {
      treeview.NodeRowCssClass = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use on nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder NodeCssClass(string value)
    {
      treeview.NodeCssClass = value;
      return this;
    }
    /// <summary>
    /// Image to use to indicate the absence of expandability. 
    /// This is rendered for leaf nodes in the place of the expand/collapse images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder NoExpandImageUrl(string value)
    {
      treeview.NoExpandImageUrl = value;
      return this;
    }

    /// <summary>
    /// Whether to extend node cells to the right edge of the TreeView. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ExtendNodeCells(bool value)
    {
      treeview.ExtendNodeCells = value;
      return this;
    }
    /// <summary>
    /// Width (in pixels) to indent each level of the TreeView. Default: 16. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder NodeIndent(int value)
    {
      treeview.NodeIndent = value;
      return this;
    }
    /// <summary>
    /// Whether to render a line structure for the tree. Default: false. If true, LineImagesFolderUrl must be specified. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ShowLines(bool value)
    {
      treeview.ShowLines = value;
      return this;
    }
    /// <summary>
    /// Whether to visually distinguish Selected node and its parent nodes from other non-Selected nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder HighlightSelectedPath(bool value)
    {
      treeview.HighlightSelectedPath = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for all image URL paths. For non-image URLs, use BaseUrl. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ImagesBaseUrl(string value)
    {
      treeview.ImagesBaseUrl = value;
      return this;
    }
    /// <summary>
    /// Default icon to use for parent nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ParentNodeImageUrl(string value)
    {
      treeview.ParentNodeImageUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to load the entire structure, including all load-on-demand nodes, when searching for the current node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder PreloadCurrentPath(bool value)
    {
      treeview.PreloadCurrentPath = value;
      return this;
    }

    /// <summary>
    /// Default icon to use for leaf nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder LeafNodeImageUrl(string value)
    {
      treeview.LeafNodeImageUrl = value;
      return this;
    }
    /// <summary>
    /// ID of ComponentArt MultiPage to control from this navigator.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder MultiPageId(string value)
    {
      treeview.MultiPageId = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use on a multiple-selected node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder MultipleSelectedNodeCssClass(string value)
    {
      treeview.MultipleSelectedNodeCssClass = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use on the row of a multiple-selected node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder MultipleSelectedNodeRowCssClass(string value)
    {
      treeview.MultipleSelectedNodeRowCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to enable multiple node select (via Ctrl+Click). Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder MultipleSelectEnabled(bool value)
    {
      treeview.MultipleSelectEnabled = value;
      return this;
    }
    /// <summary>
    /// The ID of the client template to use on nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder NodeClientTemplateId(string value)
    {
      treeview.NodeClientTemplateId = value;
      return this;
    }

    /// <summary>
    /// Whether to persist custom attributes of nodes to JavaScript. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder OutputCustomAttributes(bool value)
    {
      treeview.OutputCustomAttributes = value;
      return this;
    }
    /// <summary>
    /// Whether to pre-render the entire structure on the client, 
    /// instead of only the initially visible parts. Default: false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder PreRenderAllLevels(bool value)
    {
      treeview.PreRenderAllLevels = value;
      return this;
    }
    /// <summary>
    /// Depth from RenderRoot(Node|Item) to render to. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder RenderDrillDownDepth(int value)
    {
      treeview.RenderDrillDownDepth = value;
      return this;
    }
    /// <summary>
    /// ID of node to begin rendering down from. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder RenderRootNodeId(string value)
    {
      treeview.RenderRootNodeId = value;
      return this;
    }
    /// <summary>
    /// Whether to include the RenderRootNode when rendering, instead of only its children. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder RenderRootItemInclude(bool value)
    {
      treeview.RenderRootNodeInclude = value;
      return this;
    }
    /// <summary>
    /// Folder to look for line images in. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder LineImagesFolderUrl(string value)
    {
      treeview.LineImagesFolderUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine stamp. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder RenderSearchEngineStamp(bool value)
    {
      treeview.RenderSearchEngineStamp = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine structure for detected crawlers. Default: true.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder RenderSearchEngineStructure(bool value)
    {
      treeview.RenderSearchEngineStructure = value;
      return this;
    }
    /// <summary>
    /// Default icon to use for the selected node if it is an expanded parent. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder SelectedExpandedParentNodeImageUrl(string value)
    {
      treeview.SelectedExpandedParentNodeImageUrl = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use for the selected node on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder SelectedHoverNodeCssClass(string value)
    {
      treeview.SelectedHoverNodeCssClass = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to use on the row of the selected node on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder SelectedHoverNodeRowCssClass(string value)
    {
      treeview.SelectedHoverNodeRowCssClass = value;
      return this;
    }
    /// <summary>
    /// Default icon to use on the selected node if it is a leaf. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder SelectedLeafNodeImageUrl(string value)
    {
      treeview.SelectedLeafNodeImageUrl = value;
      return this;
    }

    /// <summary>
    /// Path to the site map XML file.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder SiteMapXmlFile(string value)
    {
      treeview.SiteMapXmlFile = value;
      return this;
    }
    /// <summary>
    /// The name of the standard SOA.UI service to use in web service mode
    /// (to be used instead of WebService/WebServiceMethod). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder SoaService(string value)
    {
      treeview.SoaService = value;
      return this;
    }
    /// <summary>
    /// Text displayed when the mouse pointer hovers over the TreeView element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder ToolTip(string value)
    {
      treeview.ToolTip = value;
      return this;
    }
    /// <summary>
    /// Whether the TreeView's markup is rendered on the page.  To hide the TreeView, use CSS.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder Visible(bool value)
    {
      treeview.Visible = value;
      return this;
    }
    /// <summary>
    /// The name of the ASP.NET AJAX web service to use for initially populating the TreeView. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder WebService(string value)
    {
      treeview.WebService = value;
      return this;
    }
    /// <summary>
    /// The (optional) custom parameter to send with each web service request. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder WebServiceCustomParameter(string value)
    {
      treeview.WebServiceCustomParameter = value;
      return this;
    }
    /// <summary>
    /// The name of the ASP.NET AJAX web service method to use for initially populating the TreeView. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TreeViewBuilder WebServiceMethod(string value)
    {
      treeview.WebServiceMethod = value;
      return this;
    }

    /// <summary>
    /// Output the markup to generate a TreeView object in HTML and, if necessary, the associated engine scripts.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      // mvc-specifc
      treeview.EnableViewState = false;

      System.IO.StringWriter stringWriter = new System.IO.StringWriter();
      System.Web.UI.HtmlTextWriter htmlTextWriter1 = new System.Web.UI.HtmlTextWriter(stringWriter);
      treeview.RenderControl(htmlTextWriter1);

      // Older method of verifying control is loaded
      string sb = "<script type=\"text/javascript\">window.ComponentArt_Page_Loaded = true;</script>";
      stringWriter.Write(sb);

      return stringWriter.ToString();
    }
  }
}
