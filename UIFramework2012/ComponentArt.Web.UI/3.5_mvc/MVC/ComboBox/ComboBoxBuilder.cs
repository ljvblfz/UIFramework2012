using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.UI.WebControls;  

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define a ComboBox object.
  /// </summary>
  public class ComboBoxBuilder : ControlBuilder
  {
    ComboBox combobox = new ComboBox();
    object boundModel;
    ViewContext viewContext;

    bool bindOnLoad = true;
    /// <summary>
    /// Builder to generate a DataGrid object on the client.  If a Model is passed, it should be one of 
    /// a) ComponentArt.Web.UI.ComboBoxActionResponse, b) the string name of the datasource in ViewData
    /// or c) an object with an IEnumerable interface.
    /// </summary>
    /// <param name="boundModel"></param>
    /// <param name="viewContext"></param>
    public ComboBoxBuilder(object boundModel, ViewContext viewContext)
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
    public ComboBoxBuilder ID(string id)
    {
      combobox.ID = id;

      return this;
    }
    /// <summary>
    /// Collection of client event handler definitions. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public ComboBoxBuilder ClientEvents(Action<ComboBoxClientEventFactory> addAction)
    {
      var factory = new ComboBoxClientEventFactory(combobox.ClientEvents);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of in-line ComboBoxItem objects.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public ComboBoxBuilder Items(Action<ComboBoxItemFactory> addAction)
    {
      bindOnLoad = false;
      var factory = new ComboBoxItemFactory(combobox.Items, combobox);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of client-templates that may be used by this ComboBox.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public ComboBoxBuilder ClientTemplates(Action<ClientTemplateFactory> addAction)
    {
      var factory = new ClientTemplateFactory(combobox.ClientTemplates);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Whether to adjust the position of the dropdown to account for relative or absolute positioning. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder AdjustPositioning(bool value)
    {
      combobox.AdjustPositioning = value;
      return this;
    }
    /// <summary>
    /// Whether to auto-complete text in the input field as the user types. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder AutoComplete(bool value)
    {
      combobox.AutoComplete = value;
      return this;
    }
    /// <summary>
    /// Whether to filter dropdown options as the user types. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder AutoFilter(bool value)
    {
      combobox.AutoFilter = value;
      return this;
    }
    /// <summary>
    /// Whether to auto-highlight the closest match as the user types. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder AutoHighlight(bool value)
    {
      combobox.AutoHighlight = value;
      return this;
    }
/// <summary>
    /// Whether to perform a postback when the selection is changed on the client. Default: false. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public ComboBoxBuilder AutoPostBack(bool value)
    {
      combobox.AutoPostBack = value;
      return this;
    }
    /// <summary>
    /// Whether to use predefined CSS classes for theming. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder AutoTheming(bool value)
    {
      combobox.AutoTheming = value;
      return this;
    }
    /// <summary>
    /// String to be prepended to CSS classes used in theming. Default is 'cart-'. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder AutoThemingCssClassPrefix(string value)
    {
      combobox.AutoThemingCssClassPrefix = value;
      return this;
    }
    /// <summary>
    /// Whether to display a visual indicator of loaded data. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder CacheMapEnabled(bool value)
    {
      combobox.CacheMapEnabled = value;
      return this;
    }
    /// <summary>
    /// The color of loaded areas on the data map. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder CacheMapLoadedColor(string value)
    {
      combobox.CacheMapLoadedColor = value;
      return this;
    }
    /// <summary>
    /// e color of areas about to be loaded on the data map. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder CacheMapLoadingColor(string value)
    {
      combobox.CacheMapLoadingColor = value;
      return this;
    }
    /// <summary>
    /// The color of non-loaded areas on the data map. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder CacheMapNotLoadedColor(string value)
    {
      combobox.CacheMapNotLoadedColor = value;
      return this;
    }
    /// <summary>
    /// The width of the data map in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder CacheMapWidth(int value)
    {
      combobox.CacheMapWidth = value;
      return this;
    }
    /// <summary>
    /// The maximum number of items to keep loaded. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder CacheSize(int value)
    {
      combobox.CacheSize = value;
      return this;
    }
    /// <summary>
    /// The client-side (JavaScript) condition to satisfy before initializing. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder ClientInitCondition(string value)
    {
      combobox.ClientInitCondition = value;
      return this;
    }
    /// <summary>
    /// The client-side (JavaScript) condition to satisfy before rendering. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder ClientRenderCondition(string value)
    {
      combobox.ClientRenderCondition = value;
      return this;
    }
/// <summary>
    /// Specifies the level of client-side content that the control renders. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public ComboBoxBuilder ClientTarget(ClientTargetLevel value)
    {
      combobox.ClientTarget = value;
      return this;
    }
    /// <summary>
    /// The duration (in milliseconds) of the dropdown collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder CollapseDuration(int value)
    {
      combobox.CollapseDuration = value;
      return this;
    }
    /// <summary>
    /// The slide type to use for the dropdown collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder CollapseSlide(SlideType value)
    {
      combobox.CollapseSlide = value;
      return this;
    }
    /// <summary>
    /// CSS class applied to the ComboBox element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder CssClass(string value)
    {
      combobox.CssClass = value;
      return this;
    }
    /// <summary>
    /// The comma-delimited list of extra fields to load from the data source. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DataFields(string value)
    {
      combobox.DataFields = value;
      return this;
    }
    /// <summary>
    /// The member in the DataSource from which to load items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DataMember(string value)
    {
      combobox.DataMember = value;
      return this;
    }
    /// <summary>
    /// The DataSource to bind to. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DataSource(object value)
    {
      combobox.DataSource = value;
      return this;
    }
    /// <summary>
    /// The field in the data source from which to load text values. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DataTextField(string value)
    {
      combobox.DataTextField = value;
      return this;
    }
    /// <summary>
    /// The format in which to display the text field. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DataTextFormatString(string value)
    {
      combobox.DataTextFormatString = value;
      return this;
    }
    /// <summary>
    /// The field in the data source from which to load item values. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DataValueField(string value)
    {
      combobox.DataValueField = value;
      return this;
    }
    /// <summary>
    /// Whether to enabling debugging feedback.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder Debug(bool value)
    {
      combobox.Debug = value;
      return this;
    }
    /// <summary>
    /// CSS class to use when the ComboBox is disabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DisabledCssClass(string value)
    {
      combobox.DisabledCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for disabled items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DisabledItemCssClass(string value)
    {
      combobox.DisabledItemCssClass = value;
      return this;
    }
    /// <summary>
    /// Image to use for expanding the dropdown, on mouse down. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropActiveImageUrl(string value)
    {
      combobox.DropActiveImageUrl = value;
      return this;
    }
    /// <summary>
    /// Content for the dropdown. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownContent(string value)
    {
      ComboBoxContent content = new ComboBoxContent();
      Literal L2 = new Literal();
      L2.Text = value;

      content.Controls.Add(L2);

      combobox.DropDownContent = content;
      return this;
    }
    /// <summary>
    /// CSS class to use for the dropdown content area. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownContentCssClass(string value)
    {
      combobox.DropDownContentCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for the dropdown. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownCssClass(string value)
    {
      combobox.DropDownCssClass = value;
      return this;
    }
    /// <summary>
    /// Footer content for the dropdown. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownFooter(string value)
    {
      ComboBoxContent content = new ComboBoxContent();
      Literal L2 = new Literal();
      L2.Text = value;

      content.Controls.Add(L2);

      combobox.DropDownFooter = content;
      return this;
    }
    /// <summary>
    /// Header content for the dropdown. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownHeader(string value)
    {
      ComboBoxContent content = new ComboBoxContent();
      Literal L2 = new Literal();
      L2.Text = value;

      content.Controls.Add(L2);

      combobox.DropDownHeader = content;
      return this;
    }
    /// <summary>
    /// Height of the dropdown, in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownHeight(int value)
    {
      combobox.DropDownHeight = value;
      return this;
    }
    /// <summary>
    /// Offset to be applied to the default x coordinate of the dropdown. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownOffsetX(int value)
    {
      combobox.DropDownOffsetX = value;
      return this;
    }
    /// <summary>
    /// Offset to be applied to the default y coordinate of the dropdown. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownOffsetY(int value)
    {
      combobox.DropDownOffsetY = value;
      return this;
    }
    /// <summary>
    /// Height of the dropdown, in the number of items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownPageSize(int value)
    {
      combobox.DropDownPageSize = value;
      return this;
    }
    /// <summary>
    /// Whether to enable user resizing of the dropdown. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownResizingMode(ComboBoxResizingMode value)
    {
      combobox.DropDownResizingMode = value;
      return this;
    }
    /// <summary>
    /// The style of the visual feedback to provide while resizing the dropdown.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownResizingStyle(ComboBoxResizingStyle value)
    {
      combobox.DropDownResizingStyle = value;
      return this;
    }
    /// <summary>
    /// Width of the dropdown, in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropDownWidth(int value)
    {
      combobox.DropDownWidth = value;
      return this;
    }
    /// <summary>
    /// Image to use for expanding the dropdown, on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropHoverImageUrl(string value)
    {
      combobox.DropHoverImageUrl = value;
      return this;
    }
    /// <summary>
    /// Width of the drop image in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropImageHeight(int value)
    {
      combobox.DropImageHeight = value;
      return this;
    }
    /// <summary>
    /// Image to use for expanding the dropdown. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropImageUrl(string value)
    {
      combobox.DropImageUrl = value;
      return this;
    }
    /// <summary>
    /// Width of the drop image in pixels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder DropImageWidth(int value)
    {
      combobox.DropImageWidth = value;
      return this;
    }
    /// <summary>
    /// Whether the ComboBox object is enabled.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder Enabled(bool value)
    {
      combobox.Enabled = value;
      return this;
    }
    /// <summary>
    /// Direction of the dropdown expansion. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder ExpandDirection(ComboBoxExpandDirection value)
    {
      combobox.ExpandDirection = value;
      return this;
    }
    /// <summary>
    /// Duration (in milliseconds) of the dropdown expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder ExpandDuration(int value)
    {
      combobox.ExpandDuration = value;
      return this;
    }
    /// <summary>
    /// Slide type to use for the dropdown expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder ExpandSlide(SlideType value)
    {
      combobox.ExpandSlide = value;
      return this;
    }
    /// <summary>
    /// Number of callback filter result sets to cache. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder FilterCacheSize(int value)
    {
      combobox.FilterCacheSize = value;
      return this;
    }
    /// <summary>
    /// Field to filter by, when filtering is required. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder FilterField(string value)
    {
      combobox.FilterField = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for the combobox when it's focused. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder FocusedCssClass(string value)
    {
      combobox.FocusedCssClass = value;
      return this;
    }
    /// <summary>
    /// Applied height to the ComboBox element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder Height(Unit value)
    {
      combobox.Height = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for the combobox, on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder HoverCssClass(string value)
    {
      combobox.HoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether the ComboBox should attempt to bind to its datasource before rendering to the client.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder InitialDataBind(bool value)
    {
      bindOnLoad = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder ItemClientTemplateId(string value)
    {
      combobox.ItemClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// Total number of items in the data set. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder ItemCount(int value)
    {
      combobox.ItemCount = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for dropdown items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder ItemCssClass(string value)
    {
      combobox.ItemCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for dropdown items on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder ItemHoverCssClass(string value)
    {
      combobox.ItemHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to enable keyboard navigation through items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder KeyboardEnabled(bool value)
    {
      combobox.KeyboardEnabled = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to display to indicate that the record-set is reloading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder LoadingClientTemplateId(string value)
    {
      combobox.LoadingClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// Text to display to indicate that the record-set is reloading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder LoadingText(string value)
    {
      combobox.LoadingText = value;
      return this;
    }
    /// <summary>
    /// Custom URL that MVC actions should post to when performing DataGrid server operations.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder MvcAction(string value)
    {
      combobox.MvcAjaxUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine stamp.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder RenderSearchEngineStamp(bool value)
    {
      combobox.RenderSearchEngineStamp = value;
      return this;
    }
    /// <summary>
    /// Running mode for this ComboBox. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder RunningMode(ComboBoxRunningMode value)
    {
      combobox.RunningMode = value;

      return this;
    }
    /// <summary>
    /// Index of the selected item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder SelectedIndex(int value)
    {
      combobox.SelectedIndex = value;
      return this;
    }
    /// <summary>
    /// The currently selected item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder SelectedItem(ComboBoxItem value)
    {
      combobox.SelectedItem = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use for the selected item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder SelectedItemCssClass(string value)
    {
      combobox.SelectedItemCssClass = value;
      return this;
    }
    /// <summary>
    /// The currently selected value. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder SelectedValue(string value)
    {
      combobox.SelectedValue = value;
      return this;
    }
    /// <summary>
    /// The custom tag to attach to SOA.UI requests. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder SoaRequestTag(string value)
    {
      combobox.SoaRequestTag = value;
      return this;
    }
    /// <summary>
    /// The name of the standard SOA.UI service to connect with. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder SoaService(string value)
    {
      combobox.SoaService = value;
      return this;
    }
    /// <summary>
    /// HTML tab-index to assign to ComboBox on the client.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder TabIndex(short value)
    {
      combobox.TabIndex = value;
      return this;
    }
    /// <summary>
    /// The text value of the input box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder Text(string value)
    {
      combobox.Text = value;
      return this;
    }
    /// <summary>
    /// The ID of the client template to use for the text box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder TextBoxClientTemplateId(string value)
    {
      combobox.TextBoxClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use for the text box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder TextBoxCssClass(string value)
    {
      combobox.TextBoxCssClass = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use for the text box when it is disabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder TextBoxDisabledCssClass(string value)
    {
      combobox.TextBoxDisabledCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to enable manual entry into the text box. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder TextBoxEnabled(bool value)
    {
      combobox.TextBoxEnabled = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use for the text box when it's focused. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder TextBoxFocusedCssClass(string value)
    {
      combobox.TextBoxFocusedCssClass = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use for the text box on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder TextBoxHoverCssClass(string value)
    {
      combobox.TextBoxHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// Whether to use the client-side page HREF as the prefix URL for callback requests. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder UseClientUrlAsPrefix(bool value)
    {
      combobox.UseClientUrlAsPrefix = value;
      return this;
    }
    /// <summary>
    /// Whether the ComboBox's markup is rendered on the page.  To hide the ComboBox, use CSS.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxBuilder Visible(bool value)
    {
      combobox.Visible = value;
      return this;
    }
/// <summary>
/// Assigned width of the ComboBox element.
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public ComboBoxBuilder Width(Unit value)
    {
      combobox.Width = value;
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
        int skip = Int32.Parse(viewContext.HttpContext.Request.Params[combobox.ClientObjectId + "_Skip"] ?? "0");
        int take = Int32.Parse(viewContext.HttpContext.Request.Params[combobox.ClientObjectId + "_Take"] ?? "10");
        string filter = viewContext.HttpContext.Request.Params[combobox.ClientObjectId + "_Filter"] ?? "";
        object datasource = new object();
        // Attempt to be smartly calculate the currentpageindex
        int currentPageIndex = skip / (combobox.DropDownPageSize != 0 ? combobox.DropDownPageSize : take);
        int recordCount = 0;

        object model = viewContext.ViewData.Model;
        ComboBoxActionResponse actionResponse = null;

        if (boundModel != null)
        {
          model = boundModel;
        }

        if (model != null)
        {
          switch (model.GetType().FullName)
          {
            case "ComponentArt.Web.UI.ComboBoxActionResponse":
              actionResponse = (ComboBoxActionResponse)model;
              break;
            case "System.String":
              actionResponse = (ComboBoxActionResponse)viewContext.ViewData[model.ToString()];
              break;
            default:
              datasource = model;
              break;
          }
          if (actionResponse != null)
          {
            datasource = actionResponse.Data;
            filter = actionResponse.Filter;
            recordCount = actionResponse.ItemCount;
          }
        }
        combobox.DataSource = datasource;
        combobox.DataBind();

        combobox.ItemCount = recordCount;
        combobox.Text = filter; 
      }

      // mvc-specific settings
      combobox.IsMvc = true;
      combobox.EnableViewState = false;

      System.IO.StringWriter stringWriter = new System.IO.StringWriter();
      System.Web.UI.HtmlTextWriter htmlTextWriter1 = new System.Web.UI.HtmlTextWriter(stringWriter);
      combobox.RenderControl(htmlTextWriter1);

      // Older method of verifying control is loaded
      string sb = "<script type=\"text/javascript\">window.ComponentArt_Page_Loaded = true;</script>";
      stringWriter.Write(sb);

      return stringWriter.ToString();
    }
  }
}