using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define a NavBar object.
  /// </summary>
  public class NavBarBuilder : ControlBuilder
  {
    NavBar navbar = new NavBar();
    /// <summary>
    /// Builder to generate a NavBar object on the client.
    /// </summary>
    public NavBarBuilder()
    {
    
    }
    /// <summary>
    /// Programmatic identifier assigned to the object. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public NavBarBuilder ID(string id)
    {
      navbar.ID = id;
      return this;
    }
    /// <summary>
    /// Collection of root NavBarItems. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public NavBarBuilder Items(Action<NavBarItemFactory> addAction)
    {
      var factory = new NavBarItemFactory(navbar.Items);
      addAction(factory);
      return this;
    }
    /// <summary>
    /// Client event handler definitions. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public NavBarBuilder ClientEvents(Action<NavBarClientEventFactory> addAction)
    {
      var factory = new NavBarClientEventFactory(navbar.ClientEvents);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of client-templates that may be used by this control. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public NavBarBuilder ClientTemplates(Action<ClientTemplateFactory> addAction)
    {
      var factory = new ClientTemplateFactory(navbar.ClientTemplates);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Custom attribute mappings. Provides the ability to re-map property names when they are looked up in XML. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public NavBarBuilder CustomAttributeMappings(Action<CustomAttributeMappingFactory> addAction)
    {
      var factory = new CustomAttributeMappingFactory(navbar.CustomAttributeMappings);

      addAction(factory);
      return this;
    }
/// <summary>
    /// Whether to perform a postback when a node is selected. Default: false. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public NavBarBuilder AutoPostBackOnSelect(bool value)
    {
      navbar.AutoPostBackOnSelect = value;
      return this;
    }
    /// <summary>
    /// Whether to use predefined CSS classes for theming. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder AutoTheming(bool value)
    {
      navbar.AutoTheming = value;
      return this;
    }
    /// <summary>
    /// String to be prepended to CSS classes used in theming. Default is 'cart-'. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder AutoThemingCssClassPrefix(string value)
    {
      navbar.AutoThemingCssClassPrefix = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for all non-image URL paths. For images, use ImagesBaseUrl. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder BaseUrl(string value)
    {
      navbar.BaseUrl = value;
      return this;
    }
    /// <summary>
    /// Relative or absolute path to the folder containing the client-side script file(s). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ClientScriptLocation(string value)
    {
      navbar.ClientScriptLocation = value;
      return this;
    }
/// <summary>
    /// Specifies the level of client-side content that the control renders. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public NavBarBuilder ClientTarget(ClientTargetLevel value)
    {
      navbar.ClientTarget = value;
      return this;
    }
     
    /// <summary>
    /// The duration of the collapse animation, in milliseconds. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder CollapseDuration(int value)
    {
      navbar.CollapseDuration = value;
      return this;
    }
    /// <summary>
    /// The slide type to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder CollapseSlide(SlideType value)
    {
      navbar.CollapseSlide = value;
      return this;
    }
    /// <summary>
    /// The transition effect to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder CollapseTransition(TransitionType value)
    {
      navbar.CollapseTransition = value;
      return this;
    }
    /// <summary>
    /// The custom transition filter to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder CollapseTransitionCustomFilter(string value)
    {
      navbar.CollapseTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// CSS class applied to the frame of the NavBar 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder CssClass(string value)
    {
      navbar.CssClass = value;
      return this;
    }
    /// <summary>
    /// The default CSS class to apply to groups.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder DefaultGroupCssClass(string value)
    {
      navbar.DefaultGroupCssClass = value;
      return this;
    }
    /// <summary>
    /// Default height to apply to items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder DefaultItemHeight(Unit value)
    {
      navbar.DefaultItemHeight = value;
      return this;
    }
    /// <summary>
    /// Default spacing to provide between items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder DefaultItemSpacing(int value)
    {
      navbar.DefaultItemSpacing = value;
      return this;
    }
    /// <summary>
    /// The default text alignment to apply to labels.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder DefaultItemTextAlign(TextAlign value)
    {
      navbar.DefaultItemTextAlign = value;
      return this;
    }
    /// <summary>
    /// Whether to permit text wrapping in labels by default.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder DefaultItemTextWrap(bool value)
    {
      navbar.DefaultItemTextWrap = value;
      return this;
    }
    /// <summary>
    /// The default text alignment to apply to labels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder DefaultSubItemTextAlign(TextAlign value)
    {
      navbar.DefaultSubItemTextAlign = value;
      return this;
    }
    /// <summary>
    /// Default target (frame or window) to use when navigating. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder DefaultTarget(string value)
    {
      navbar.DefaultTarget = value;
      return this;
    }
    /// <summary>
    /// The duration of the expand animation, in milliseconds. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ExpandDuration(int value)
    {
      navbar.ExpandDuration = value;
      return this;
    }
    /// <summary>
    /// Whether to only permit a single path in the NavBar to be expanded at a time. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ExpandSinglePath(bool value)
    {
      navbar.ExpandSinglePath = value;
      return this;
    }
    /// <summary>
    /// The slide type to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ExpandSlide(SlideType value)
    {
      navbar.ExpandSlide = value;
      return this;
    }
    /// <summary>
    /// The transition effect to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ExpandTransition(TransitionType value)
    {
      navbar.ExpandTransition = value;
      return this;
    }
    /// <summary>
    /// The custom transition filter to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ExpandTransitionCustomFilter(string value)
    {
      navbar.ExpandTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// Whether to take on the dimensions of the containing DOM element. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder FillContainer(bool value)
    {
      navbar.FillContainer = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for this NavBar when it has keyboard focus. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder FocusedCssClass(string value)
    {
      navbar.FocusedCssClass = value;
      return this;
    }
    /// <summary>
    /// ID of item to forcefully highlight. This will make it appear as it would when selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ForceHighlightedItemID(string value)
    {
      navbar.ForceHighlightedItemID = value;
      return this;
    }
    /// <summary>
    /// Whether to force the rendering of the search engine structure. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ForceSearchEngineStructure(bool value)
    {
      navbar.ForceSearchEngineStructure = value;
      return this;
    }
    /// <summary>
    /// Whether to expand groups so they fill exactly the height of the NavBar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder FullExpand(bool value)
    {
      navbar.FullExpand = value;
      return this;
    }
    /// <summary>
    /// Whether to visually distinguish Selected node and its parent nodes from other non-Selected nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder HighlightSelectedPath(bool value)
    {
      navbar.HighlightSelectedPath = value;
      return this;
    }
    /// <summary>
    /// Assigned height of the NavBar element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder Height(Unit value)
    {
      navbar.Height = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for all image URL paths. For non-image URLs, use BaseUrl. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ImagesBaseUrl(string value)
    {
      navbar.ImagesBaseUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to enable keyboard control of the NavBar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder KeyboardEnabled(bool value)
    {
      navbar.KeyboardEnabled = value;
      return this;
    }
    /// <summary>
    /// ID of ComponentArt MultiPage to control from this navigator. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder MultiPageId(string value)
    {
      navbar.MultiPageId = value;
      return this;
    }
    /// <summary>
    /// Whether to persist custom attributes of nodes to JavaScript. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder OutputCustomAttributes(bool value)
    {
      navbar.OutputCustomAttributes = value;
      return this;
    }
    /// <summary>
    /// Whether to pre-render the entire structure on the client,
    /// instead of only the initially visible parts. Default: false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder PreRenderAllLevels(bool value)
    {
      navbar.PreRenderAllLevels = value;
      return this;
    }
    /// <summary>
    /// ID of item to begin rendering down from.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder RenderRootItemId(string value)
    {
      navbar.RenderRootItemId = value;
      return this;
    }
    /// <summary>
    /// Whether to include the RenderRootItem when rendering, instead of only its children. Default: false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder RenderRootItemInclude(bool value)
    {
      navbar.RenderRootItemInclude = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine stamp.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder RenderSearchEngineStamp(bool value)
    {
      navbar.RenderSearchEngineStamp = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine structure for detected crawlers. Default: true.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder RenderSearchEngineStructure(bool value)
    {
      navbar.RenderSearchEngineStructure = value;
      return this;
    }
/// <summary>
    /// Image to use for scrolling down within a group, when the mouse is down. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public NavBarBuilder ScrollDownActiveImageUrl(string value)
    {
      navbar.ScrollDownActiveImageUrl = value;
      return this;
    }
    /// <summary>
    /// Image to use for scrolling down within a group, on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollDownHoverImageUrl(string value)
    {
      navbar.ScrollDownHoverImageUrl = value;
      return this;
    }
    /// <summary>
    /// Height to apply to scroll-down image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollDownImageHeight(int value)
    {
      navbar.ScrollDownImageHeight = value;
      return this;
    }
    /// <summary>
    /// Image to use for scrolling down within a group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollDownImageUrl(string value)
    {
      navbar.ScrollDownImageUrl = value;
      return this;
    }
    /// <summary>
    /// Width to apply to scroll-down image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollDownImageWidth(int value)
    {
      navbar.ScrollDownImageWidth = value;
      return this;
    }
    /// <summary>
    /// Id of template to use for groups' scroll-down bars. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollDownTemplateId(string value)
    {
      navbar.ScrollDownTemplateId = value;
      return this;
    }
    /// <summary>
    /// Image to use for scrolling up within a group, when the mouse is down. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollUpActiveImageUrl(string value)
    {
      navbar.ScrollUpActiveImageUrl = value;
      return this;
    }
    /// <summary>
    /// Image to use for scrolling up within a group, on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollUpHoverImageUrl(string value)
    {
      navbar.ScrollUpHoverImageUrl = value;
      return this;
    }
    /// <summary>
    /// Height to apply to scroll-up image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollUpImageHeight(int value)
    {
      navbar.ScrollUpImageHeight = value;
      return this;
    }
    /// <summary>
    /// Image to use for scrolling up within a group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollUpImageUrl(string value)
    {
      navbar.ScrollUpImageUrl = value;
      return this;
    }
    /// <summary>
    /// Width to apply to scroll-up image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollUpImageWidth(int value)
    {
      navbar.ScrollUpImageWidth = value;
      return this;
    }
    /// <summary>
    /// Id of template to use for groups' scroll-up bars. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ScrollUpTemplateId(string value)
    {
      navbar.ScrollUpTemplateId = value;
      return this;
    }
    /// <summary>
    /// Whether to display the side scrollbar when group contents don't fit in a group.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ShowScrollBar(bool value)
    {
      navbar.ShowScrollBar = value;
      return this;
    }
    /// <summary>
    /// Path to the site map XML file.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder SiteMapXmlFile(string value)
    {
      navbar.SiteMapXmlFile = value;
      return this;
    }
    /// <summary>
    /// The name of the standard SOA.UI service to use in web service mode 
    /// (to be used instead of WebService/WebServiceMethod). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder SoaService(string value)
    {
      navbar.SoaService = value;
      return this;
    }
    /// <summary>
    /// The text displayed when the mouse pointer hovers over the Web server control.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder ToolTip(string value)
    {
      navbar.ToolTip = value;
      return this;
    }
    /// <summary>
    /// Whether the NavBar's markup is rendered on the page.  To hide the NavBar, use CSS.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder Visible(bool value)
    {
      navbar.Visible = value;
      return this;
    }
    /// <summary>
    /// The name of the ASP.NET AJAX web service to use for initially populating the NavBar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder WebService(string value)
    {
      navbar.WebService = value;
      return this;
    }
    /// <summary>
    /// The (optional) custom parameter to send with each web service request. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder WebServiceCustomParameter(string value)
    {
      navbar.WebServiceCustomParameter = value;
      return this;
    }
    /// <summary>
    /// The name of the ASP.NET AJAX web service method to use for initially populating the NavBar. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder WebServiceMethod(string value)
    {
      navbar.WebServiceMethod = value;
      return this;
    }
    /// <summary>
    /// Applied width of the NavBar object.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarBuilder Width(Unit value)
    {
      navbar.Width = value;
      return this;
    }

    /// <summary>
    /// Output the markup to generate a NavBar object in HTML and, if necessary, the associated engine scripts.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {

      System.IO.StringWriter stringWriter = new System.IO.StringWriter();
      System.Web.UI.HtmlTextWriter htmlTextWriter1 = new System.Web.UI.HtmlTextWriter(stringWriter);
      navbar.RenderControl(htmlTextWriter1);

      // Older method of verifying control is loaded
      string sb = "<script type=\"text/javascript\">window.ComponentArt_Page_Loaded = true;</script>";
      stringWriter.Write(sb);

      return stringWriter.ToString();
    }
  }
}
