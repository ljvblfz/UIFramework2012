using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define a Menu object.
  /// </summary>
  public class MenuBuilder : ControlBuilder
  {
    Menu menu = new Menu();
    /// <summary>
    /// Builder to generate a Menu object on the client.
    /// </summary>
    public MenuBuilder()
    {

    }
    /// <summary>
    /// Programmatic identifier assigned to the object. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MenuBuilder ID(string id)
    {
      menu.ID = id;

      return this;
    }
    /// <summary>
    /// Collection of root MenuItems.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public MenuBuilder Items(Action<MenuItemFactory> addAction)
    {
      var factory = new MenuItemFactory(menu.Items);
      addAction(factory);
      return this;
    }
/// <summary>
    /// Collection of client event handler definitions.
/// </summary>
/// <param name="addAction"></param>
/// <returns></returns>
    public MenuBuilder ClientEvents(Action<MenuClientEventFactory> addAction)
    {
      var factory = new MenuClientEventFactory(menu.ClientEvents);

      addAction(factory);
      return this;
    }
/// <summary>
    /// Collection of client-templates that may be used by this Menu.
/// </summary>
/// <param name="addAction"></param>
/// <returns></returns>
    public MenuBuilder ClientTemplates(Action<ClientTemplateFactory> addAction)
    {
      var factory = new ClientTemplateFactory(menu.ClientTemplates);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Custom attribute mappings. Provides the ability to re-map property names when they are looked up in XML.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public MenuBuilder CustomAttributeMappings(Action<CustomAttributeMappingFactory> addAction)
    {
      var factory = new CustomAttributeMappingFactory(menu.CustomAttributeMappings);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Whether to perform a postback when a node is selected. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder AutoPostBackOnSelect(bool value)
    {
      menu.AutoPostBackOnSelect = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a postback when an item is checked or unchecked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder AutoPostBackOnCheckChanged(bool value)
    {
      menu.AutoPostBackOnCheckChanged = value;
      return this;
    }
    /// <summary>
    /// Whether to use predefined CSS classes for theming.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder AutoTheming(bool value)
    {
      menu.AutoTheming = value;
      return this;
    }
    /// <summary>
    /// String to be prepended to CSS classes used in theming. Default is 'cart-'.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder AutoThemingCssClassPrefix(string value)
    {
      menu.AutoThemingCssClassPrefix = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for all non-image URL paths. For images, use ImagesBaseUrl. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder BaseUrl(string value)
    {
      menu.BaseUrl = value;
      return this;
    }
    /// <summary>
    /// Relative or absolute path to the folder containing the client-side script file(s). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ClientScriptLocation(string value)
    {
      menu.ClientScriptLocation = value;
      return this;
    }
    /// <summary>
    /// Specifies the level of client-side content that the control renders.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ClientTarget(ClientTargetLevel value)
    {
      menu.ClientTarget = value;
      return this;
    }
    /// <summary>
    /// Delay between the mouse leaving the menu and the menu starting to collapse. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder CollapseDelay(int value)
    {
      menu.CollapseDelay = value;
      return this;
    }
    /// <summary>
    /// The duration of the collapse animation, in milliseconds.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder CollapseDuration(int value)
    {
      menu.CollapseDuration = value;
      return this;
    }
    /// <summary>
    /// The slide type to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder CollapseSlide(SlideType value)
    {
      menu.CollapseSlide = value;
      return this;
    }
    /// <summary>
    /// The transition effect to use for the collapse animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder CollapseTransition(TransitionType value)
    {
      menu.CollapseTransition = value;
      return this;
    }
    /// <summary>
    /// The custom transition filter to use for the collapse animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder CollapseTransitionCustomFilter(string value)
    {
      menu.CollapseTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// Client-side ID of the element to which this context menu is bound. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ContextControlId(string value)
    {
      menu.ContextControlId = value;
      return this;
    }
    /// <summary>
    /// Determines whether this menu control is a context menu. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ContextMenu(ContextMenuType value)
    {
      menu.ContextMenu = value;
      return this;
    }
    /// <summary>
    /// CSS class applied to the frame of the Menu. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder CssClass(string value)
    {
      menu.CssClass = value;
      return this;
    }
    /// <summary>
    /// Default CSS class to apply to groups.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultGroupCssClass(string value)
    {
      menu.DefaultGroupCssClass = value;
      return this;
    }
    /// <summary>
    /// Direction in which the groups expand. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultGroupExpandDirection(GroupExpandDirection value)
    {
      menu.DefaultGroupExpandDirection = value;
      return this;
    }
    /// <summary>
    /// Offset along x-axis from groups' normal expand positions. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultGroupExpandOffsetX(int value)
    {
      menu.DefaultGroupExpandOffsetX = value;
      return this;
    }
    /// <summary>
    /// Offset along y-axis from groups' normal expand positions. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultGroupExpandOffsetY(int value)
    {
      menu.DefaultGroupExpandOffsetY = value;
      return this;
    }
    /// <summary>
    /// Height of groups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultGroupHeight(int value)
    {
      menu.DefaultGroupHeight = value;
      return this;
    }
    /// <summary>
    /// Spacing between group items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultGroupItemSpacing(System.Web.UI.WebControls.Unit value)
    {
      menu.DefaultGroupItemSpacing = value;
      return this;
    }
    /// <summary>
    /// Orientation of subgroups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultGroupOrientation(GroupOrientation value)
    {
      menu.DefaultGroupOrientation = value;
      return this;
    }
    /// <summary>
    /// Width of groups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultGroupWidth(int value)
    {
      menu.DefaultGroupWidth = value;
      return this;
    }
    /// <summary>
    /// The default text alignment to apply to labels. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultItemTextAlign(TextAlign value)
    {
      menu.DefaultItemTextAlign = value;
      return this;
    }
    /// <summary>
    /// Whether to permit text wrapping in labels by default.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultItemTextWrap(bool value)
    {
      menu.DefaultItemTextWrap = value;
      return this;
    }
    /// <summary>
    /// Default target (frame or window) to use when navigating.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder DefaultTarget(string value)
    {
      menu.DefaultTarget = value;
      return this;
    }
    /// <summary>
    /// Delay between the mouse entering a MenuItem and its subgroup starting to expand. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ExpandDelay(int value)
    {
      menu.ExpandDelay = value;
      return this;
    }
    /// <summary>
    /// Whether to expand a subgroup of an item that is disabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ExpandDisabledItems(bool value)
    {
      menu.ExpandDisabledItems = value;
      return this;
    }
    /// <summary>
    /// The duration of the expand animation, in milliseconds.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ExpandDuration(int value)
    {
      menu.ExpandDuration = value;
      return this;
    }
    /// <summary>
    /// Whether to have expanded look take precedence over hover when both are applicable. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ExpandedOverridesHover(bool value)
    {
      menu.ExpandedOverridesHover = value;
      return this;
    }
    /// <summary>
    /// Whether to wait for a click before expanding and collapsing the menu. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ExpandOnClick(bool value)
    {
      menu.ExpandOnClick = value;
      return this;
    }
    /// <summary>
    /// The slide type to use for the expand animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ExpandSlide(SlideType value)
    {
      menu.ExpandSlide = value;
      return this;
    }
    /// <summary>
    /// The transition effect to use for the expand animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ExpandTransition(TransitionType value)
    {
      menu.ExpandTransition = value;
      return this;
    }
    /// <summary>
    /// The custom transition filter to use for the expand animation. (
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ExpandTransitionCustomFilter(string value)
    {
      menu.ExpandTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// ID of item to forcefully highlight. This will make it appear as it would when selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ForceHighlightedItemID(string value)
    {
      menu.ForceHighlightedItemID = value;
      return this;
    }
    /// <summary>
    /// Whether to force the rendering of the search engine structure. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ForceSearchEngineStructure(bool value)
    {
      menu.ForceSearchEngineStructure = value;
      return this;
    }
    /// <summary>
    /// Assigned height of the Menu element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder Height(Unit value)
    {
      menu.Height = value;
      return this;
    }
    /// <summary>
    /// Whether to hide the SELECT elements that would obscure the menu. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder HideSelectElements(bool value)
    {
      menu.HideSelectElements = value;
      return this;
    }
    /// <summary>
    /// Whether to default to Hover look for the items whose child groups are expanded. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder HighlightExpandedPath(bool value)
    {
      menu.HighlightExpandedPath = value;
      return this;
    }
    /// <summary>
    /// Whether to visually distinguish Selected node and its parent nodes from other non-Selected nodes. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder HighlightSelectedPath(bool value)
    {
      menu.HighlightSelectedPath = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for all image URL paths. For non-image URLs, use BaseUrl. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ImagesBaseUrl(string value)
    {
      menu.ImagesBaseUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to enable keyboard control of the Menu. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder KeyboardEnabled(bool value)
    {
      menu.KeyboardEnabled = value;
      return this;
    }
    /// <summary>
    /// ID of ComponentArt MultiPage to control from this navigator.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder MultiPageId(string value)
    {
      menu.MultiPageId = value;
      return this;
    }
    /// <summary>
    /// Orientation of the top group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder Orientation(GroupOrientation value)
    {
      menu.Orientation = value;
      return this;
    }
    /// <summary>
    /// Whether to overlay windowed elements that would obscure the menu. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder OverlayWindowedElements(bool value)
    {
      menu.OverlayWindowedElements = value;
      return this;
    }
/// <summary>
    /// Whether to persist custom attributes of nodes to JavaScript. Default: true. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuBuilder OutputCustomAttributes(bool value)
    {
      menu.OutputCustomAttributes = value;
      return this;
    }
    /// <summary>
    /// zIndex of the first pop-up group. Default is 999. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder PopUpZIndexBase(int value)
    {
      menu.PopUpZIndexBase = value;
      return this;
    }
    /// <summary>
    /// Whether to pre-render the entire structure on the client, 
    /// instead of only the initially visible parts. Default: false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder PreRenderAllLevels(bool value)
    {
      menu.PreRenderAllLevels = value;
      return this;
    }
    /// <summary>
    /// ID of item to begin rendering down from.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder RenderRootItemId(string value)
    {
      menu.RenderRootItemId = value;
      return this;
    }
    /// <summary>
    /// Whether to include the RenderRootItem when rendering, instead of only its children. Default: false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder RenderRootItemInclude(bool value)
    {
      menu.RenderRootItemInclude = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine stamp.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder RenderSearchEngineStamp(bool value)
    {
      menu.RenderSearchEngineStamp = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine structure for detected crawlers. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder RenderSearchEngineStructure(bool value)
    {
      menu.RenderSearchEngineStructure = value;
      return this;
    }
    /// <summary>
    /// Whether to enable scrolling for this Menu's groups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ScrollingEnabled(bool value)
    {
      menu.ScrollingEnabled = value;
      return this;
    }
    /// <summary>
    /// Color of pop-up groups' shadows. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ShadowColor(System.Drawing.Color value)
    {
      menu.ShadowColor = value;
      return this;
    }
    /// <summary>
    /// Whether menu's pop-up groups drop shadows. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ShadowEnabled(bool value)
    {
      menu.ShadowEnabled = value;
      return this;
    }
    /// <summary>
    /// Offset of the pop-up groups' shadows. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder ShadowOffset(int value)
    {
      menu.ShadowOffset = value;
      return this;
    }    
    /// <summary>
    /// Path to the site map XML file. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder SiteMapXmlFile(string value)
    {
      menu.SiteMapXmlFile = value;
      return this;
    }
    /// <summary>
    /// The name of the standard SOA.UI service to use in web service mode (to be used instead of WebService/WebServiceMethod).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder SoaService(string value)
    {
      menu.SoaService = value;
      return this;
    }
/// <summary>
    /// The text displayed when the mouse pointer hovers over Menu element. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuBuilder ToolTip(string value)
    {
      menu.ToolTip = value;
      return this;
    }
    /// <summary>
    /// Direction in which the top group expands, in a context menu. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder TopGroupExpandDirection(GroupExpandDirection value)
    {
      menu.TopGroupExpandDirection = value;
      return this;
    }
/// <summary>
    /// Offset along x-axis from top group's normal expand position. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuBuilder TopGroupExpandOffsetX(int value)
    {
      menu.TopGroupExpandOffsetX = value;
      return this;
    }
    /// <summary>
    /// Offset along y-axis from top group's normal expand position. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder TopGroupExpandOffsetY(int value)
    {
      menu.TopGroupExpandOffsetY = value;
      return this;
    }
    /// <summary>
    /// Spacing between top group's items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder TopGroupItemSpacing(int value)
    {
      menu.TopGroupItemSpacing = value;
      return this;
    }
/// <summary>
    /// Whether the Menu's markup is rendered on the page.  To hide the Menu, use CSS.
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuBuilder Visible(bool value)
    {
      menu.Visible = value;
      return this;
    }
/// <summary>
    /// The name of the ASP.NET AJAX web service to use for initially populating the Menu. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuBuilder WebService(string value)
    {
      menu.WebService = value;
      return this;
    }
    /// <summary>
    /// The (optional) custom parameter to send with each web service request. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder WebServiceCustomParameter(string value)
    {
      menu.WebServiceCustomParameter = value;
      return this;
    }
    /// <summary>
    /// The name of the ASP.NET AJAX web service method to use for initially populating the Menu. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder WebServiceMethod(string value)
    {
      menu.WebServiceMethod = value;
      return this;
    }
    /// <summary>
    /// Assigned width of the Menu object.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuBuilder Width(Unit value)
    {
      menu.Width = value;
      return this;
    }
    /// <summary>
    /// Output the markup to generate a Menu object in HTML and, if necessary, the associated engine scripts.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {

      System.IO.StringWriter stringWriter = new System.IO.StringWriter();
      System.Web.UI.HtmlTextWriter htmlTextWriter1 = new System.Web.UI.HtmlTextWriter(stringWriter);
      menu.RenderControl(htmlTextWriter1);

      return stringWriter.ToString();
    }
  }
}
