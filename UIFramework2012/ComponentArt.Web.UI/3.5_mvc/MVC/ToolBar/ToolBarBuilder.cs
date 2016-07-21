using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define a ToolBar object.
  /// </summary>
  public class ToolBarBuilder : ControlBuilder
  {
    ToolBar toolbar = new ToolBar();

    /// <summary>
    /// Builder to generate a ToolBar object on the client.
    /// </summary>
    public ToolBarBuilder()
    {
    
    }
    /// <summary>
    /// Programmatic identifier assigned to the object. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ToolBarBuilder ID(string id)
    {
      toolbar.ID = id;

      return this;
    }

    /// <summary>
    /// Collection of ToolBar items. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public ToolBarBuilder Items(Action<ToolBarItemFactory> addAction)
    {
      var factory = new ToolBarItemFactory(toolbar.Items);
      addAction(factory);
      return this;
    }
    /// <summary>
    /// Client event handler definitions. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public ToolBarBuilder ClientEvents(Action<ToolBarClientEventFactory> addAction)
    {
      var factory = new ToolBarClientEventFactory(toolbar.ClientEvents);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of client-templates that may be used by this control. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public ToolBarBuilder ClientTemplates(Action<ClientTemplateFactory> addAction)
    {
      var factory = new ClientTemplateFactory(toolbar.ClientTemplates);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Custom attribute mappings. Provides the ability to re-map property names when they are looked up in XML. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public ToolBarBuilder CustomAttributeMappings(Action<CustomAttributeMappingFactory> addAction)
    {
      var factory = new CustomAttributeMappingFactory(toolbar.CustomAttributeMappings);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Whether to perform a postback when an item is selected. Default value is false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder AutoPostBackOnSelect(bool value)
    {
      toolbar.AutoPostBackOnSelect = value;
      return this;
    }
    /// <summary>
    /// Whether to use predefined CSS classes for theming. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder AutoTheming(bool value)
    {
      toolbar.AutoTheming = value;
      return this;
    }
    /// <summary>
    /// String to be prepended to CSS classes used in theming. Default is 'cart-'. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder AutoThemingCssClassPrefix(string value)
    {
      toolbar.AutoThemingCssClassPrefix = value;
      return this;
    }
    /// <summary>
    /// Relative or absolute path to the folder containing the client-side script file(s). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder ClientScriptLocation(string value)
    {
      toolbar.ClientScriptLocation = value;
      return this;
    }
    /// <summary>
    /// Specifies the level of client-side content that the control renders.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder ClientTarget(ClientTargetLevel value)
    {
      toolbar.ClientTarget = value;
      return this;
    }
     
    /// <summary>
    /// CSS class applied to the ToolBar element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder CssClass(string value)
    {
      toolbar.CssClass = value;
      return this;
    }
    /// <summary>
    /// The default CSS class to apply to an item when it is active (on mouse down).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemActiveCssClass(string value)
    {
        toolbar.DefaultItemActiveCssClass = value;
        return this;
    }
    /// <summary>
    /// The default CSS class to apply to a Checked item when it is active (on mouse down). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemCheckedActiveCssClass(string value)
    {
        toolbar.DefaultItemCheckedActiveCssClass = value;
        return this;
    }
    /// <summary>
    /// The default CSS class to apply to a Checked item when it is active (on mouse down). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemCheckedCssClass(string value)
    {
        toolbar.DefaultItemCheckedCssClass = value;
        return this;
    }
    /// <summary>
    /// The default CSS class to apply to a Checked item on hover (on mouse over). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemCheckedHoverCssClass(string value)
    {
        toolbar.DefaultItemCheckedHoverCssClass = value;
        return this;
    }
    /// <summary>
    /// The default CSS class to apply to an item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemCssClass(string value)
    {
        toolbar.DefaultItemCssClass = value;
        return this;
    }
    /// <summary>
    /// The default CSS class to apply to an item that is not Enabled and is Checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder  DefaultItemDisabledCheckedCssClass(string value)
    {
        toolbar.DefaultItemDisabledCheckedCssClass = value;
        return this;
    }
    /// <summary>
    /// The default CSS class to apply to an item that is not Enabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemDisabledCssClass(string value)
    {
        toolbar.DefaultItemDisabledCssClass = value;
        return this;
    }
    /// <summary>
    /// The default CSS class to apply to an item when its dropdown menu is expanded. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemExpandedCssClass(string value)
    {
        toolbar.DefaultItemExpandedCssClass = value;
        return this;
    }
    /// <summary>
    /// The default height of items' dropdown images. Default value is Unit.Empty. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemDropDownImageHeight(Unit value)
    {
        toolbar.DefaultItemDropDownImageHeight = value;
        return this;
    }
    /// <summary>
    /// The default width of items' dropdown images. Default value is Unit.Empty. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemDropDownImageWidth(Unit value)
    {
        toolbar.DefaultItemDropDownImageWidth = value;
        return this;
    }
    /// <summary>
    /// The default position of item dropdown image relative to rest of the item. 
    /// Default value is ToolBarDropDownImagePosition.Right. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemDropDownImagePosition(ToolBarDropDownImagePosition value)
    {
        toolbar.DefaultItemDropDownImagePosition = value;
        return this;
    }
    /// <summary>
    /// The default height of items. Default value is Unit.Empty. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemHeight(System.Web.UI.WebControls.Unit value)
    {
      toolbar.DefaultItemHeight = value;
      return this;
    }
    /// <summary>
    /// The default CSS class to apply to an item on hover (on mouse over). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemHoverCssClass(string value)
    {
      toolbar.DefaultItemHoverCssClass = value;
      return this;
    }
    /// <summary>
    /// The default width of items. Default value is Unit.Empty. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemWidth(System.Web.UI.WebControls.Unit value)
    {
        toolbar.DefaultItemWidth = value;
        return this;
    }
    /// <summary>
    /// The default height of items' images. Default value is Unit.Empty. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemImageHeight(System.Web.UI.WebControls.Unit value)
    {
        toolbar.DefaultItemImageHeight = value;
        return this;
    }
    /// <summary>
    /// The default width of items' images. Default value is Unit.Empty. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemImageWidth(System.Web.UI.WebControls.Unit value)
    {
        toolbar.DefaultItemImageWidth = value;
        return this;
    }
    /// <summary>
    /// Applied height to the ToolBar element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder Height(System.Web.UI.WebControls.Unit value)
    {
        toolbar.Height = value;
        return this;
    }
    /// <summary>
    /// Applied width to the ToolBar element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder Width(System.Web.UI.WebControls.Unit value)
    {
        toolbar.Width = value;
        return this;
    }
    /// <summary>
    /// The default text alignment to apply to labels. Default value is TextAlign.Left. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemTextAlign(TextAlign value)
    {
        toolbar.DefaultItemTextAlign = value;
        return this;
    }
    /// <summary>
    /// The default position of item text and image relative to each other. 
    /// Default value is ToolBarTextImageRelation.ImageBeforeText. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemTextImageRelation(ToolBarTextImageRelation value)
    {
        toolbar.DefaultItemTextImageRelation = value;
        return this;
    }
    /// <summary>
    /// The default gap in pixels between item text and image. Default value is 0. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemTextImageSpacing(int value)
    {
        toolbar.DefaultItemTextImageSpacing = value;
        return this;
    }
    /// <summary>
    /// Spacing between toolbar items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder ItemSpacing(int value)
    {
        toolbar.ItemSpacing = value;
        return this;
    }
    /// <summary>
    /// Whether to use fading effects when changing the appearance of items. Default value is true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder UseFadeEffect(bool value)
    {
        toolbar.UseFadeEffect = value;
        return this;
    }
    /// <summary>
    /// Orientation of the ToolBar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder Orientation(GroupOrientation value)
    {
        toolbar.Orientation = value;
        return this;
    }
    /// <summary>
    /// Whether to permit text wrapping in labels by default. Default value is false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder DefaultItemTextWrap(bool value)
    {
        toolbar.DefaultItemTextWrap = value;
        return this;
    }
    /// <summary>
    /// Prefix to use for image URL paths. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder ImagesBaseUrl(string value)
    {
      toolbar.ImagesBaseUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to enable keyboard control of the ToolBar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder KeyboardEnabled(bool value)
    {
      toolbar.KeyboardEnabled = value;
      return this;
    }
    /// <summary>
    /// Whether to persist custom attributes of nodes to JavaScript. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder OutputCustomAttributes(bool value)
    {
      toolbar.OutputCustomAttributes = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine stamp.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder RenderSearchEngineStamp(bool value)
    {
      toolbar.RenderSearchEngineStamp = value;
      return this;
    }
    /// <summary>
    /// Path to the site map XML file. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder SiteMapXmlFile(string value)
    {
      toolbar.SiteMapXmlFile = value;
      return this;
    }
    /// <summary>
    /// The name of the standard SOA.UI service to use in web
    /// service mode (to be used instead of WebService/WebServiceMethod). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder SoaService(string value)
    {
      toolbar.SoaService = value;
      return this;
    }
    /// <summary>
    /// The text displayed when the mouse pointer hovers over the Web server control.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder ToolTip(string value)
    {
      toolbar.ToolTip = value;
      return this;
    }
    /// <summary>
    /// Whether the ToolBar's markup is rendered on the page.  To hide the ToolBar, use CSS.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder Visible(bool value)
    {
      toolbar.Visible = value;
      return this;
    }
    /// <summary>
    /// The name of the ASP.NET AJAX web service to use for initially populating the ToolBar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder WebService(string value)
    {
      toolbar.WebService = value;
      return this;
    }
    /// <summary>
    /// The (optional) custom parameter to send with each web service request. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder WebServiceCustomParameter(string value)
    {
      toolbar.WebServiceCustomParameter = value;
      return this;
    }
    /// <summary>
    /// The name of the ASP.NET AJAX web service method to use for initially populating the ToolBar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarBuilder WebServiceMethod(string value)
    {
      toolbar.WebServiceMethod = value;
      return this;
    }
    /// <summary>
    /// Output the markup to generate a ToolBar object in HTML and, if necessary, the associated engine scripts.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {

      System.IO.StringWriter stringWriter = new System.IO.StringWriter();
      System.Web.UI.HtmlTextWriter htmlTextWriter1 = new System.Web.UI.HtmlTextWriter(stringWriter);
      toolbar.RenderControl(htmlTextWriter1);

      // Hwan 2009-12-02
      // This will need to be removed/moved into jQuery document.ready event
      // Or removed from MVC ToolBar, ComboBox, Grid

      string sb = "<script type=\"text/javascript\">window.ComponentArt_Page_Loaded = true;</script>";
      stringWriter.Write(sb);

      return stringWriter.ToString();
    }
  }
}
