using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define a DataGrid object.
  /// </summary>
  public class DataGridBuilder : ControlBuilder
  {
    DataGrid grid = new DataGrid();
    object boundModel;

    ViewContext viewContext;

    bool bindOnLoad = false;
    bool bindOnLoadUser = false;

    /// <summary>
    /// Builder to generate a DataGrid object on the client.  If a Model is passed, it should be one of 
    /// a) ComponentArt.Web.UI.DataGridActionResponse, b) the string name of the datasource in ViewData
    /// or c) an object with an IEnumerable interface.
    /// </summary>
    /// <param name="boundModel"></param>
    /// <param name="viewContext"></param>
    public DataGridBuilder(object boundModel, ViewContext viewContext)
    {
      if (boundModel != null)
      {
        this.boundModel = boundModel;
      }
      this.viewContext = viewContext;
    }
    /// <summary>
    /// Programmatic identifier assigned to the object. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DataGridBuilder ID(string id)
    {
      grid.ID = id;

      return this;
    }
    /// <summary>
    /// Custom URL that MVC actions should post to when performing DataGrid server operations.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder MvcAction(string value)
    {
      grid.MvcAjaxUrl = value;
      return this;
    }

    /// <summary>
    /// The collection of DataGridLevels in this DataGrid. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public DataGridBuilder Levels(Action<DataGridLevelFactory> addAction)
    {
      DataGridLevelFactory factory = new DataGridLevelFactory(grid.Levels);
      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of client event handler definitions. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public DataGridBuilder ClientEvents(Action<DataGridClientEventFactory> addAction)
    {
      var factory = new DataGridClientEventFactory(grid.ClientEvents);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of in-line DataGridItem objects.  The DataGridLevel must be defined before these can be added.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public DataGridBuilder Items(Action<DataGridItemFactory> addAction)
    {
      var factory = new DataGridItemFactory(grid.Items, grid);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of client-templates that may be used by this DataGrid. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public DataGridBuilder ClientTemplates(Action<ClientTemplateFactory> addAction)
    {
      var factory = new ClientTemplateFactory(grid.ClientTemplates);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Whether to allow the runtime resizing of columns in this Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AllowColumnResizing(bool value)
    {
      grid.AllowColumnResizing = value;
      return this;
    }
    /// <summary>
    /// Whether to allow editing of data in this Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AllowEditing(bool value)
    {
      grid.AllowEditing = value;
      return this;
    }
    /// <summary>
    /// Whether to allow horizontal scrolling of this Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AllowHorizontalScrolling(bool value)
    {
      grid.AllowHorizontalScrolling = value;
      return this;
    }
    /// <summary>
    /// Whether to allow HTML content in this Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AllowHtmlContent(bool value)
    {
      grid.AllowHtmlContent = value;
      return this;
    }
    /// <summary>
    /// Whether to allow multiple selection in this Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AllowMultipleSelect(bool value)
    {
      grid.AllowMultipleSelect = value;
      return this;
    }
    /// <summary>
    /// Whether to allow paging in this Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AllowPaging(bool value)
    {
      grid.AllowPaging = value;
      return this;
    }
    /// <summary>
    /// Whether to allow text selection in this Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AllowTextSelection(bool value)
    {
      grid.AllowTextSelection = value;
      return this;
    }
    /// <summary>
    /// Whether to allow vertical scrolling of the Grid's data area, when its height exceeds the allocated height. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AllowVerticalScrolling(bool value)
    {
      grid.AllowVerticalScrolling = value;
      return this;
    }
    /// <summary>
    /// Whether to automatically adjust the page size to the height of the Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoAdjustPageSize(bool value)
    {
      grid.AutoAdjustPageSize = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a callback when an item is checked or unchecked under a column of type CheckBox. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoCallBackOnCheckChanged(bool value)
    {
      grid.AutoCallBackOnCheckChanged = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a callback when columns are reordered. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoCallBackOnColumnReorder(bool value)
    {
      grid.AutoCallBackOnColumnReorder = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a callback when an item is deleted. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoCallBackOnDelete(bool value)
    {
      grid.AutoCallBackOnDelete = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a callback when an item is added. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoCallBackOnInsert(bool value)
    {
      grid.AutoCallBackOnInsert = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a callback when an item is edited. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoCallBackOnUpdate(bool value)
    {
      grid.AutoCallBackOnUpdate = value;
      return this;
    }
    /// <summary>
    /// Whether to automatically set focus to the search box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoFocusSearchBox(bool value)
    {
      grid.AutoFocusSearchBox = value;
      return this;
    }
    /// <summary>
    /// Whether to force a sort on the column that is being grouped by. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoSortOnGroup(bool value)
    {
      grid.AutoSortOnGroup = value;
      return this;
    }
    /// <summary>
    /// Whether to use predefined CSS classes for theming. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoTheming(bool value)
    {
      grid.AutoTheming = value;
      return this;
    }
    /// <summary>
    /// String to be prepended to CSS classes used in theming. Default is 'cart-'. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder AutoThemingCssClassPrefix(string value)
    {
      grid.AutoThemingCssClassPrefix = value;
      return this;
    }
    /// <summary>
    /// The number of pages to load per callback request, when caching is enabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CallbackCacheLookAhead(int value)
    {
      grid.CallbackCacheLookAhead = value;
      return this;
    }
    /// <summary>
    /// The maximum number of pages to keep in the cache. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CallbackCacheSize(int value)
    {
      grid.CallbackCacheSize = value;
      return this;
    }
    /// <summary>
    /// Whether to enable caching of data in callback mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CallbackCachingEnabled(bool value)
    {
      grid.CallbackCachingEnabled = value;
      return this;
    }
    /// <summary>
    /// Spacing to apply between cells in the Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CellSpacing(int value)
    {
      grid.CellSpacing = value;
      return this;
    }
    /// <summary>
    /// Relative or absolute path to the folder containing the client-side script file(s).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ClientScriptLocation(string value)
    {
      grid.ClientScriptLocation = value;
      return this;
    }
    /// <summary>
    /// Specifies the level of client-side content that the control renders.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ClientTarget(ClientTargetLevel value)
    {
      grid.ClientTarget = value;
      return this;
    }
    /// <summary>
    /// The duration (in milliseconds) of the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CollapseDuration(int value)
    {
      grid.CollapseDuration = value;
      return this;
    }
    /// <summary>
    /// URL of image to use for indicating the collapsability of a Grid item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CollapseImageUrl(string value)
    {
      grid.CollapseImageUrl = value;
      return this;
    }
    /// <summary>
    /// The slide type to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CollapseSlide(SlideType value)
    {
      grid.CollapseSlide = value;
      return this;
    }
    /// <summary>
    /// The transition effect to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CollapseTransition(TransitionType value)
    {
      grid.CollapseTransition = value;
      return this;
    }
    /// <summary>
    /// The string defining a custom transition filter to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CollapseTransitionCustomFilter(string value)
    {
      grid.CollapseTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// Whether to distribute the width difference to all columns to the right, when a column is resized. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ColumnResizeDistributeWidth(bool value)
    {
      grid.ColumnResizeDistributeWidth = value;
      return this;
    }
    /// <summary>
    /// The Cascading Style Sheet (CSS) class assigned to the DataGrid element on the client.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CssClass(string value)
    {
      grid.CssClass = value;
      return this;
    }
    /// <summary>
    /// The current page of data that the DataGrid is on. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder CurrentPageIndex(int value)
    {
      grid.CurrentPageIndex = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use for the data area panel of the DataGrid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder DataAreaCssClass(string value)
    {
      grid.DataAreaCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to provide some debugging feedback at runtime. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder Debug(bool value)
    {
      grid.Debug = value;
      return this;
    }
    /// <summary>
    /// Whether to switch a selected item into edit mode when it is clicked.
    /// Note that this feature can consume clicks meant for double-clicking.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder EditOnClickSelectedItem(bool value)
    {
      grid.EditOnClickSelectedItem = value;
      return this;
    }
    /// <summary>
    /// Text to render to indicate that the Grid has no data. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder EmptyGridText(string value)
    {
      grid.EmptyGridText = value;
      return this;
    }
    /// <summary>
    /// Whether the DataGrid object is enabled.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder Enabled(bool value)
    {
      grid.Enabled = value;
      return this;
    }
    /// <summary>
    /// The ID of the client template to use for expand/collapse cells in a hierarhical Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ExpandCollapseClientTemplateId(string value)
    {
      grid.ExpandCollapseClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// The height (in pixels) of the expand and collapse images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ExpandCollapseImageHeight(int value)
    {
      grid.ExpandCollapseImageHeight = value;
      return this;
    }
    /// <summary>
    /// The width (in pixels) of the expand and collapse images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ExpandCollapseImageWidth(int value)
    {
      grid.ExpandCollapseImageWidth = value;
      return this;
    }
    /// <summary>
    /// The duration (in milliseconds) of the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ExpandDuration(int value)
    {
      grid.ExpandDuration = value;
      return this;
    }
    /// <summary>
    /// URL of image to use for indicating the expandability of a Grid item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ExpandImageUrl(string value)
    {
      grid.ExpandImageUrl = value;
      return this;
    }
    /// <summary>
    /// The slide type to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ExpandSlide(SlideType value)
    {
      grid.ExpandSlide = value;
      return this;
    }
    /// <summary>
    /// The transition effect to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ExpandTransition(TransitionType value)
    {
      grid.ExpandTransition = value;
      return this;
    }
    /// <summary>
    /// The string defining a custom transition filter to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ExpandTransitionCustomFilter(string value)
    {
      grid.ExpandTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// The comma-separated list of IDs of DOM elements and ComponentArt controls which this Grid's items can be dropped onto. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ExternalDropTargets(string value)
    {
      grid.ExternalDropTargets = value;
      return this;
    }
    /// <summary>
    /// Whether to take on the dimensions of the containing element. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder FillContainer(bool value)
    {
      grid.FillContainer = value;
      return this;
    }
    /// <summary>
    /// The filter (SQL WHERE expression) to apply to the data. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder Filter(string value)
    {
      grid.Filter = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use on the footer of the Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder FooterCssClass(string value)
    {
      grid.FooterCssClass = value;
      return this;
    }
    /// <summary>
    /// The height (in pixels) to force for the Grid footer. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder FooterHeight(int value)
    {
      grid.FooterHeight = value;
      return this;
    }
    /// <summary>
    /// The grouping (SQL GROUP BY expression) to use on the data. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupBy(string value)
    {
      grid.GroupBy = value;
      return this;
    }
    /// <summary>
    /// The ID of the client template to use for the drop-to-group panel on the Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupByClientTemplateId(string value)
    {
      grid.GroupByClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// The CssClass to use for the drop-to-group panel on the Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupByCssClass(string value)
    {
      grid.GroupByCssClass = value;
      return this;
    }
    /// <summary>
    /// The CssClass to apply to each section (grouping) in the drop-to-group feedback. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupBySectionCssClass(string value)
    {
      grid.GroupBySectionCssClass = value;
      return this;
    }
    /// <summary>
    /// The CssClass to apply to each section (grouping) separator area in the drop-to-group feedback. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupBySectionSeparatorCssClass(string value)
    {
      grid.GroupBySectionSeparatorCssClass = value;
      return this;
    }
    /// <summary>
    /// The URL of the image to use for indicating that the grouping sort order is ascending. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupBySortAscendingImageUrl(string value)
    {
      grid.GroupBySortAscendingImageUrl = value;
      return this;
    }
    /// <summary>
    /// The URL of the image to use for indicating that the grouping sort order is descending. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupBySortDescendingImageUrl(string value)
    {
      grid.GroupBySortDescendingImageUrl = value;
      return this;
    }
    /// <summary>
    /// The height (in pixels) of the grouping sort order images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupBySortImageHeight(int value)
    {
      grid.GroupBySortImageHeight = value;
      return this;
    }
    /// <summary>
    /// The width (in pixels) of the grouping sort order images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupBySortImageWidth(int value)
    {
      grid.GroupBySortImageWidth = value;
      return this;
    }
    /// <summary>
    /// The text to display ahead of grouping sections in the header. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupByText(string value)
    {
      grid.GroupByText = value;
      return this;
    }
    /// <summary>
    /// The CSS class to apply to the drop-to-group feedback text. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupByTextCssClass(string value)
    {
      grid.GroupByTextCssClass = value;
      return this;
    }
    /// <summary>
    /// The text to display in a group header when the group continues from the previous page. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupContinuedText(string value)
    {
      grid.GroupContinuedText = value;
      return this;
    }
    /// <summary>
    /// The text to display in a group header when the group continues on the next page. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupContinuingText(string value)
    {
      grid.GroupContinuingText = value;
      return this;
    }
    /// <summary>
    /// The type of grouping behaviour to employ. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupingMode(GridGroupingMode value)
    {
      grid.GroupingMode = value;
      return this;
    }
    /// <summary>
    /// The location in the Grid to render the drop-to-group indicator. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupingNotificationPosition(GridElementPosition value)
    {
      grid.GroupingNotificationPosition = value;
      return this;
    }
    /// <summary>
    /// The text to use for indicating drop-to-group functionality. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupingNotificationText(string value)
    {
      grid.GroupingNotificationText = value;
      return this;
    }
    /// <summary>
    /// The CSS class to apply to the grouping notification text. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupingNotificationTextCssClass(string value)
    {
      grid.GroupingNotificationTextCssClass = value;
      return this;
    }
    /// <summary>
    /// Page size to use for grouped data. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder GroupingPageSize(int value)
    {
      grid.GroupingPageSize = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use for the DataGrid header. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder HeaderCssClass(string value)
    {
      grid.HeaderCssClass = value;
      return this;
    }
    /// <summary>
    /// The height (in pixels) to apply to the DataGrid header.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder HeaderHeight(int value)
    {
      grid.HeaderHeight = value;
      return this;
    }
    /// <summary>
    /// Assigned height of the DataGrid element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder Height(System.Web.UI.WebControls.Unit value)
    {
      grid.Height = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for URLs of images used in DataGrid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ImagesBaseUrl(string value)
    {
      grid.ImagesBaseUrl = value;
      return this;
    }
    /// <summary>
    /// The CSS class to apply to indentation cells in hierarchical DataGrids. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder IndentCellCssClass(string value)
    {
      grid.IndentCellCssClass = value;
      return this;
    }
    /// <summary>
    /// The width (in pixels) of the indent cells in a hierarchical DataGrid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder IndentCellWidth(int value)
    {
      grid.IndentCellWidth = value;
      return this;
    }

    /// <summary>
    /// Whether the DataGrid should attempt to bind to its datasource before rendering to the client.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder InitialDataBind(bool value)
    {
      bindOnLoad = value;
      bindOnLoadUser = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the item being dragged. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ItemDraggingClientTemplateId(string value)
    {
      grid.ItemDraggingClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the item being dragged. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ItemDraggingCssClass(string value)
    {
      grid.ItemDraggingCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to permit items to be dragged. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ItemDraggingEnabled(bool value)
    {
      grid.ItemDraggingEnabled = value;
      return this;
    }
    /// <summary>
    /// Whether to enable keyboard control of this Grid. 
    /// If set, use Ctrl + left/right arrows to page, up/down arrows to move through items, 
    /// and Enter to select the item currently highlighted. 
    /// Highlighting is done by applying the the GridLevel hover row styles. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder KeyboardEnabled(bool value)
    {
      grid.KeyboardEnabled = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for feedback while waiting on a callback to complete. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder LoadingPanelClientTemplateId(string value)
    {
      grid.LoadingPanelClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// Whether to display a special feedback panel while callback data is loading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder LoadingPanelEnabled(bool value)
    {
      grid.LoadingPanelEnabled = value;
      return this;
    }
    /// <summary>
    /// Duration of the fade effect when transitioning the loading template, in milliseconds. 
    /// A value of 0 (default) turns off the fade effect. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder LoadingPanelFadeDuration(int value)
    {
      grid.LoadingPanelFadeDuration = value;
      return this;
    }
    /// <summary>
    /// The maximum opacity percentage to fade to. Between 0 and 100. Default: 100. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder LoadingPanelFadeMaximumOpacity(int value)
    {
      grid.LoadingPanelFadeMaximumOpacity = value;
      return this;
    }
    /// <summary>
    /// The X offset (in pixels) of the loading panel from its relative position. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder LoadingPanelOffsetX(int value)
    {
      grid.LoadingPanelOffsetX = value;
      return this;
    }
    /// <summary>
    /// The Y offset (in pixels) of the loading panel from its relative position. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder LoadingPanelOffsetY(int value)
    {
      grid.LoadingPanelOffsetY = value;
      return this;
    }
    /// <summary>
    /// The position of the loading panel relative to the Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder LoadingPanelPosition(GridRelativePosition value)
    {
      grid.LoadingPanelPosition = value;
      return this;
    }
    /// <summary>
    /// Whether to enable manual paging in this Grid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ManualPaging(bool value)
    {
      grid.ManualPaging = value;
      return this;
    }
    /// <summary>
    /// URL of image to use for indicating the non-expandability of a Grid item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder NoExpandImageUrl(string value)
    {
      grid.NoExpandImageUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to pad incomplete pages by rendering empty rows. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagePaddingEnabled(bool value)
    {
      grid.PagePaddingEnabled = value;
      return this;
    }
    /// <summary>
    /// Whether to look for and use active images for the pager (on mouse down). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerButtonActiveEnabled(bool value)
    {
      grid.PagerButtonActiveEnabled = value;
      return this;
    }
    /// <summary>
    /// The height (in pixels) of pager buttons. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerButtonHeight(int value)
    {
      grid.PagerButtonHeight = value;
      return this;
    }
    /// <summary>
    /// Whether to look for and use hover images for the pager (on mouse over). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerButtonHoverEnabled(bool value)
    {
      grid.PagerButtonHoverEnabled = value;
      return this;
    }
    /// <summary>
    /// The padding (in pixels) to apply to the pager buttons. Default: 5. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerButtonPadding(int value)
    {
      grid.PagerButtonPadding = value;
      return this;
    }
    /// <summary>
    /// The width (in pixels) of pager buttons. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerButtonWidth(int value)
    {
      grid.PagerButtonWidth = value;
      return this;
    }
    /// <summary>
    /// URL of the folder which contains pager images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerImagesFolderUrl(string value)
    {
      grid.PagerImagesFolderUrl = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the pager info panel. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerInfoClientTemplateId(string value)
    {
      grid.PagerInfoClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the pager info panel. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerInfoPosition(GridElementPosition value)
    {
      grid.PagerInfoPosition = value;
      return this;
    }
    /// <summary>
    /// Relative position within the DataGrid of the pager info panel. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerPosition(GridElementPosition value)
    {
      grid.PagerPosition = value;
      return this;
    }
    /// <summary>
    /// Type of pager to render. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerStyle(GridPagerStyle value)
    {
      grid.PagerStyle = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the pager text.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PagerTextCssClass(string value)
    {
      grid.PagerTextCssClass = value;
      return this;
    }
    /// <summary>
    /// Number of items to render per page of the DataGrid. Default: 20. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PageSize(int value)
    {
      grid.PageSize = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the area at the bottom of the DataGrid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PostFooterClientTemplateId(string value)
    {
      grid.PostFooterClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the area at the bottom of the DataGrid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PostFooterCssClass(string value)
    {
      grid.PostFooterCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to pre-expand all groups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PreExpandOnGroup(bool value)
    {
      grid.PreExpandOnGroup = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the area at the top of the DataGrid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PreHeaderClientTemplateId(string value)
    {
      grid.PreHeaderClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for the area at the top of the DataGrid. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PreHeaderCssClass(string value)
    {
      grid.PreHeaderCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to pre-load all levels. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder PreloadLevels(bool value)
    {
      grid.PreloadLevels = value;
      return this;
    }
    /// <summary>
    /// Number of items (records) that this DataGrid contains. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder RecordCount(int value)
    {
      grid.RecordCount = value;
      return this;
    }
    /// <summary>
    /// The offset to render items from. Setting this will override the paging mechanism. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder RecordOffset(int value)
    {
      grid.RecordOffset = value;
      return this;
    }
    /// <summary>
    /// Type of client-side rendering to perform. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder RenderingMode(GridRenderingMode value)
    {
      grid.RenderingMode = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine stamp. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder RenderSearchEngineStamp(bool value)
    {
      grid.RenderSearchEngineStamp = value;
      return this;
    }
    /// <summary>
    /// Method used to handle DataGrid paging/sorting/grouping operations.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder RunningMode(GridRunningMode value)
    {
      grid.RunningMode = value;

      if (!bindOnLoadUser && grid.RunningMode != GridRunningMode.Callback)
      {
        this.bindOnLoad = true;
      }

      return this;
    }
    /// <summary>
    /// Whether to display the vertical scroll bar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollBar(GridScrollBarMode value)
    {
      grid.ScrollBar = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the scroll bar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollBarCssClass(string value)
    {
      grid.ScrollBarCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the scroll bar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollBarWidth(int value)
    {
      grid.ScrollBarWidth = value;
      return this;
    }
    /// <summary>
    /// Whether to look for and use the scroll button active (on mouse down) images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollButtonActiveEnabled(bool value)
    {
      grid.ScrollButtonActiveEnabled = value;
      return this;
    }
    /// <summary>
    /// Height (in pixels) of the scroll buttons. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollButtonHeight(int value)
    {
      grid.ScrollButtonHeight = value;
      return this;
    }
    /// <summary>
    /// Whether to look for and use the scroll button active (on mouse over) images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollButtonHoverEnabled(bool value)
    {
      grid.ScrollButtonHoverEnabled = value;
      return this;
    }
    /// <summary>
    /// Width (in pixels) of the scroll buttons. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollButtonWidth(int value)
    {
      grid.ScrollButtonWidth = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the scroll grip. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollGripCssClass(string value)
    {
      grid.ScrollGripCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the scroll header cell. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollHeaderCssClass(string value)
    {
      grid.ScrollHeaderCssClass = value;
      return this;
    }
    /// <summary>
    /// Folder which contains images for the scroll bar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollImagesFolderUrl(string value)
    {
      grid.ScrollImagesFolderUrl = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the scroll popup. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollPopupClientTemplateId(string value)
    {
      grid.ScrollPopupClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// Height (in pixels) of the scrollbar top and bottom images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollTopBottomImageHeight(int value)
    {
      grid.ScrollTopBottomImageHeight = value;
      return this;
    }
    /// <summary>
    /// Whether to render scrollbar top and bottom images.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollTopBottomImagesEnabled(bool value)
    {
      grid.ScrollTopBottomImagesEnabled = value;
      return this;
    }
    /// <summary>
    /// The width (in pixels) of the scrollbar top and bottom images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ScrollTopBottomImageWidth(int value)
    {
      grid.ScrollTopBottomImageWidth = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the search box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SearchBoxCssClass(string value)
    {
      grid.SearchBoxCssClass = value;
      return this;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SearchBoxPosition(GridElementPosition value)
    {
      grid.SearchBoxPosition = value;
      return this;
    }
    /// <summary>
    /// Relative position within the DataGrid to use for the search box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SearchOnKeyPress(bool value)
    {
      grid.SearchOnKeyPress = value;
      return this;
    }
    /// <summary>
    /// Whether to perform searches every time a key is pressed inside the search box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SearchOnKeyPressDelay(int value)
    {
      grid.SearchOnKeyPressDelay = value;
      return this;
    }
    /// <summary>
    /// Text to render next to the search box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SearchText(string value)
    {
      grid.SearchText = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to text in the search area. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SearchTextCssClass(string value)
    {
      grid.SearchTextCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether the data source is self-referencing. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SelfReferencing(bool value)
    {
      grid.SelfReferencing = value;
      return this;
    }
    /// <summary>
    /// Whether to render the DataGrid footer. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ShowFooter(bool value)
    {
      grid.ShowFooter = value;
      return this;
    }
    /// <summary>
    /// Whether to render the DataGrid header. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ShowHeader(bool value)
    {
      grid.ShowHeader = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder ShowSearchBox(bool value)
    {
      grid.ShowSearchBox = value;
      return this;
    }
    /// <summary>
    /// Width (in pixels) of the (often rounded) edges of the slider image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderEdgeWidth(int value)
    {
      grid.SliderEdgeWidth = value;
      return this;
    }
    /// <summary>
    /// Delay (in milliseconds) after which the hovered page is fetched to cache, if caching is enabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderFetchDelay(int value)
    {
      grid.SliderFetchDelay = value;
      return this;
    }
    /// <summary>
    /// Width (in pixels) of the slider grip. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderGripWidth(int value)
    {
      grid.SliderGripWidth = value;
      return this;
    }
    /// <summary>
    /// Height (in pixels) of the slider. Default: 20. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderHeight(int value)
    {
      grid.SliderHeight = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the slider popup, when hovering on a cached page. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderPopupCachedClientTemplateId(string value)
    {
      grid.SliderPopupCachedClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the slider popup. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderPopupClientTemplateId(string value)
    {
      grid.SliderPopupClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for the slider popup when the DataGrid is grouped. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderPopupGroupedClientTemplateId(string value)
    {
      grid.SliderPopupGroupedClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// X offset (in pixels) to use when displaying the slider popup. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderPopupOffsetX(int value)
    {
      grid.SliderPopupOffsetX = value;
      return this;
    }
    /// <summary>
    /// Y offset (in pixels) to use when displaying the slider popup. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderPopupOffsetY(int value)
    {
      grid.SliderPopupOffsetY = value;
      return this;
    }
    /// <summary>
    /// Width (in pixels) of the slider. Default: 300. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SliderWidth(int value)
    {
      grid.SliderWidth = value;
      return this;
    }
    /// <summary>
    /// The name of the standard SOA.UI service to use in web service mode (to be used instead of WebService/WebServiceMethod). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder SoaService(string value)
    {
      grid.SoaService = value;
      return this;
    }
    /// <summary>
    /// Height (in pixels) of the tree line images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder TreeLineImageHeight(int value)
    {
      grid.TreeLineImageHeight = value;
      return this;
    }
    /// <summary>
    /// URL to the folder in which the tree line images are located.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder TreeLineImagesFolderUrl(string value)
    {
      grid.TreeLineImagesFolderUrl = value;
      return this;
    }
    /// <summary>
    /// Width (in pixels) of the tree line images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder TreeLineImageWidth(int value)
    {
      grid.TreeLineImageWidth = value;
      return this;
    }
    /// <summary>
    /// Whether to use the client-side page HREF as the prefix URL for callback requests.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder UseClientUrlAsPrefix(bool value)
    {
      grid.UseClientUrlAsPrefix = value;
      return this;
    }
    /// <summary>
    /// Name of the ASP.NET AJAX web service to use for WebService running mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder WebService(string value)
    {
      grid.WebService = value;
      return this;
    }
    /// <summary>
    /// Whether to enable caching of data in webservice mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder WebServiceCachingEnabled(bool value)
    {
      grid.WebServiceCachingEnabled = value;
      return this;
    }
    /// <summary>
    /// Name of the method to use for fetching configuration info in WebService running mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder WebServiceConfigMethod(string value)
    {
      grid.WebServiceConfigMethod = value;
      return this;
    }
    /// <summary>
    /// The optional custom parameter to send with each web service request. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder WebServiceCustomParameter(string value)
    {
      grid.WebServiceCustomParameter = value;
      return this;
    }
    /// <summary>
    /// Name of the method to use for deleting records in WebService running mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder WebServiceDeleteMethod(string value)
    {
      grid.WebServiceDeleteMethod = value;
      return this;
    }
    /// <summary>
    /// Name of the method to use for retrieving groups in WebService running mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder WebServiceGroupMethod(string value)
    {
      grid.WebServiceGroupMethod = value;
      return this;
    }
    /// <summary>
    /// Name of the method to use for inserting records in WebService running mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder WebServiceInsertMethod(string value)
    {
      grid.WebServiceInsertMethod = value;
      return this;
    }
    /// <summary>
    /// Name of the method to use for fetching records in WebService running mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder WebServiceSelectMethod(string value)
    {
      grid.WebServiceSelectMethod = value;
      return this;
    }
    /// <summary>
    /// Name of the method to use for updating records in WebService running mode.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder WebServiceUpdateMethod(string value)
    {
      grid.WebServiceUpdateMethod = value;
      return this;
    }
    /// <summary>
    /// Assigned width of the DataGrid element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridBuilder Width(int value)
    {
      grid.Width = value;
      return this;
    }

    /// <summary>
    /// Output the markup to generate a DataGrid object in HTML and, if necessary, the associated engine scripts.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      if (bindOnLoad)
      {
        // Extract params from querystring, if possible
        int skip = Int32.Parse(viewContext.HttpContext.Request.Params[grid.ClientObjectId + "_Skip"] ?? "0");
        int take = Int32.Parse(viewContext.HttpContext.Request.Params[grid.ClientObjectId + "_Take"] ?? "10");
        string order = viewContext.HttpContext.Request.Params[grid.ClientObjectId + "_Order"] ?? "";
        string groupOrder = viewContext.HttpContext.Request.Params[grid.ClientObjectId + "_GroupOrder"] ?? "";
        string filter = viewContext.HttpContext.Request.Params[grid.ClientObjectId + "_Filter"] ?? "";
        object datasource = new object();
        // Attempt to be smartly calculate the currentpageindex
        int currentPageIndex = skip / (grid.PageSize != 0 ? grid.PageSize : take);
        int recordCount = 0;

        object model = viewContext.ViewData.Model;
        datasource = model;

        if (boundModel != null)
        {
          model = boundModel;
        }

        if (model != null)
        {
          switch (model.GetType().FullName)
          {
            case "ComponentArt.Web.UI.DataGridActionResponse":
              DataGridActionResponse actionResponse = (DataGridActionResponse)model;
              datasource = actionResponse.Data;
              order = actionResponse.Sort;
              recordCount = actionResponse.RecordCount;
              currentPageIndex = actionResponse.CurrentPageIndex;
              filter = actionResponse.Filter;
              groupOrder = actionResponse.GroupBy;
              break;
            case "System.String":
              datasource = viewContext.ViewData[model.ToString()];
              break;
            default:
              datasource = model;
              break;
          }
        }

        if (grid.ManualPaging)
        {
          grid.DataSource = datasource;
          grid.Sort = order;

          grid.DataBind();

          grid.RecordCount = recordCount;
          grid.CurrentPageIndex = currentPageIndex;
          grid.Filter = filter;
          grid.GroupBy = groupOrder;
        }
        else
        {
          grid.CurrentPageIndex = currentPageIndex;
          grid.Sort = order;
          grid.GroupBy = groupOrder;
          grid.Filter = filter;

          grid.DataSource = datasource;
          grid.DataBind();
        }
      }

      // mvc-specific settings
      grid.IsMvc = true;
      grid.EnableViewState = false;

      System.IO.StringWriter stringWriter = new System.IO.StringWriter();
      System.Web.UI.HtmlTextWriter htmlTextWriter1 = new System.Web.UI.HtmlTextWriter(stringWriter);
      grid.RenderControl(htmlTextWriter1);

      // Older method of verifying control is loaded
      string sb = "<script type=\"text/javascript\">";
      sb += "window.ComponentArt_Page_Loaded = true;";
      sb += "</script>";
      stringWriter.Write(sb);

      return stringWriter.ToString();
    }
  }
}