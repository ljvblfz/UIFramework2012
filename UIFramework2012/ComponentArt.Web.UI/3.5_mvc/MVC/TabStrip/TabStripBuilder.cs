using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define a TabStrip object.
  /// </summary>
  public class TabStripBuilder : ControlBuilder
  {
    TabStrip tabstrip = new TabStrip();
    /// <summary>
    /// Builder to generate a TabStrip object on the client.
    /// </summary>
    public TabStripBuilder()
    {

    }
    /// <summary>
    /// Programmatic identifier assigned to the object. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TabStripBuilder ID(string id)
    {
      tabstrip.ID = id;
      return this;
    }
    /// <summary>
    /// Collection of root TabStripTabs.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public TabStripBuilder Tabs(Action<TabStripTabFactory> addAction)
    {
      var factory = new TabStripTabFactory(tabstrip.Tabs);
      addAction(factory);
      return this;
    }
    /// <summary>
    /// Client event handler definitions. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public TabStripBuilder ClientEvents(Action<TabStripClientEventFactory> addAction)
    {
      var factory = new TabStripClientEventFactory(tabstrip.ClientEvents);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of client-templates that may be used by this control.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public TabStripBuilder ClientTemplates(Action<ClientTemplateFactory> addAction)
    {
      var factory = new ClientTemplateFactory(tabstrip.ClientTemplates);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Custom attribute mappings. Provides the ability to re-map property names when they are looked up in XML. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public TabStripBuilder CustomAttributeMappings(Action<CustomAttributeMappingFactory> addAction)
    {
      var factory = new CustomAttributeMappingFactory(tabstrip.CustomAttributeMappings);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Applied width to the TabStrip element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder Width(Unit value)
    {
      tabstrip.Width = value;
      return this;
    }
    /// <summary>
    /// Applied height to the TabStrip element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder Height(Unit value)
    {
      tabstrip.Height = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a postback when a node is selected. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder AutoPostBackOnSelect(bool value)
    {
      tabstrip.AutoPostBackOnSelect = value;
      return this;
    }
    /// <summary>
    /// Whether to use predefined CSS classes for theming. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder AutoTheming(bool value)
    {
      tabstrip.AutoTheming = value;
      return this;
    }
    /// <summary>
    /// String to be prepended to CSS classes used in theming. Default is 'cart-'. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder AutoThemingCssClassPrefix(string value)
    {
      tabstrip.AutoThemingCssClassPrefix = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for all non-image URL paths. For images, use ImagesBaseUrl. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder BaseUrl(string value)
    {
      tabstrip.BaseUrl = value;
      return this;
    }
    /// <summary>
    /// Relative or absolute path to the folder containing the client-side script file(s).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ClientScriptLocation(string value)
    {
      tabstrip.ClientScriptLocation = value;
      return this;
    }
    /// <summary>
    /// Specifies the level of client-side content that the control renders.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ClientTarget(ClientTargetLevel value)
    {
      tabstrip.ClientTarget = value;
      return this;
    }

    /// <summary>
    /// The duration of the collapse animation, in milliseconds.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder CollapseDuration(int value)
    {
      tabstrip.CollapseDuration = value;
      return this;
    }
    /// <summary>
    /// The slide type to use for the collapse animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder CollapseSlide(SlideType value)
    {
      tabstrip.CollapseSlide = value;
      return this;
    }
    /// <summary>
    /// The transition effect to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder CollapseTransition(TransitionType value)
    {
      tabstrip.CollapseTransition = value;
      return this;
    }
    /// <summary>
    /// The custom transition filter to use for the collapse animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder CollapseTransitionCustomFilter(string value)
    {
      tabstrip.CollapseTransitionCustomFilter = value;
      return this;
    }
    /// <summary>
    /// CSS class applied to the TabStrip element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder CssClass(string value)
    {
      tabstrip.CssClass = value;
      return this;
    }
    /// <summary>
    /// The default CSS class to apply to groups.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupCssClass(string value)
    {
      tabstrip.DefaultGroupCssClass = value;
      return this;
    }
    /// <summary>
    /// Alignment of tabs in tab groups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupAlign(TabStripAlign value)
    {
      tabstrip.DefaultGroupAlign = value;
      return this;
    }
    /// <summary>
    /// Offset along x-axis from groups' normal expand positions. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupExpandOffsetX(int value)
    {
      tabstrip.DefaultGroupExpandOffsetX = value;
      return this;
    }
    /// <summary>
    /// Offset along y-axis from groups' normal expand positions. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupExpandOffsetY(int value)
    {
      tabstrip.DefaultGroupExpandOffsetY = value;
      return this;
    }
    /// <summary>
    /// Default height for the last separator in a group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupFirstSeparatorHeight(int value)
    {
      tabstrip.DefaultGroupFirstSeparatorHeight = value;
      return this;
    }
    /// <summary>
    /// Default width for the separators in a group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupFirstSeparatorWidth(int value)
    {
      tabstrip.DefaultGroupFirstSeparatorWidth = value;
      return this;
    }
    /// <summary>
    /// File folder where the separator images for tab groups are located. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupSeparatorImagesFolderUrl(string value)
    {
      tabstrip.DefaultGroupSeparatorImagesFolderUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to show separator images for the tab groups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupShowSeparators(bool value)
    {
      tabstrip.DefaultGroupShowSeparators = value;
      return this;
    }
    /// <summary>
    /// Spacing between group tabs. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupTabSpacing(int value)
    {
      tabstrip.DefaultGroupTabSpacing = value;
      return this;
    }
    /// <summary>
    /// Width of tab groups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupWidth(int value)
    {
      tabstrip.DefaultGroupWidth = value;
      return this;
    }
    /// <summary>
    /// Height of tab groups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultGroupHeight(int value)
    {
      tabstrip.DefaultGroupHeight = value;
      return this;
    }
    /// <summary>
    /// The default text alignment to apply to labels.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultItemTextAlign(TextAlign value)
    {
      tabstrip.DefaultItemTextAlign = value;
      return this;
    }
    /// <summary>
    /// Whether to permit text wrapping in labels by default.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultItemTextWrap(bool value)
    {
      tabstrip.DefaultItemTextWrap = value;
      return this;
    }
    /// <summary>
    /// Default target (frame or window) to use when navigating. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder DefaultTarget(string value)
    {
      tabstrip.DefaultTarget = value;
      return this;
    }
    /// <summary>
    /// How tabs in top tab group are aligned. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupAlign(TabStripAlign value)
    {
      tabstrip.TopGroupAlign = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply to the top group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupCssClass(string value)
    {
      tabstrip.TopGroupCssClass = value;
      return this;
    }
    /// <summary>
    /// Height of the first separator in the top group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupFirstSeparatorHeight(int value)
    {
      tabstrip.TopGroupFirstSeparatorHeight = value;
      return this;
    }
    /// <summary>
    /// Height of the first separator in the top group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupFirstSeparatorWidth(int value)
    {
      tabstrip.TopGroupFirstSeparatorWidth = value;
      return this;
    }
    /// <summary>
    /// Width of the last separator in the top group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupLastSeparatorWidth(int value)
    {
      tabstrip.TopGroupFirstSeparatorWidth = value;
      return this;
    }
    /// <summary>
    /// Height of the last separator in the top group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupLastSeparatorHeight(int value)
    {
      tabstrip.TopGroupLastSeparatorHeight = value;
      return this;
    }
    /// <summary>
    /// Folder with top group's separator images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupSeparatorImagesFolderUrl(string value)
    {
      tabstrip.TopGroupSeparatorImagesFolderUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to show separator images for the top tab group. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupShowSeparators(bool value)
    {
      tabstrip.TopGroupShowSeparators = value;
      return this;
    }
    /// <summary>
    /// Spacing between top group's tabs. Default is Unit.Empty. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupTabSpacing(int value)
    {
      tabstrip.TopGroupTabSpacing = value;
      return this;
    }
    /// <summary>
    /// Top group width. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupWidth(int value)
    {
      tabstrip.TopGroupWidth = value;
      return this;
    }
    /// <summary>
    /// Top group height. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder TopGroupHeight(int value)
    {
      tabstrip.TopGroupHeight = value;
      return this;
    }
    /// <summary>
    /// ID of item to forcefully highlight. This will make it appear as it would when selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ForceHighlightedItemID(string value)
    {
      tabstrip.ForceHighlightedItemID = value;
      return this;
    }
    /// <summary>
    /// The duration of the expand animation, in milliseconds. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ExpandDuration(int value)
    {
      tabstrip.ExpandDuration = value;
      return this;
    }
    /// <summary>
    /// Slide type to use for the expand animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ExpandSlide(SlideType value)
    {
      tabstrip.ExpandSlide = value;
      return this;
    }
    /// <summary>
    /// The transition effect to use for the expand animation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ExpandTransition(TransitionType value)
    {
      tabstrip.ExpandTransition = value;
      return this;
    }
    /// <summary>
    /// The custom transition filter to use for the expand animation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ExpandTransitionCustomFilter(string value)
    {
      tabstrip.ExpandTransitionCustomFilter = value;
      return this;
    }

    /// <summary>
    /// Whether to force the rendering of the search engine structure. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ForceSearchEngineStructure(bool value)
    {
      tabstrip.ForceSearchEngineStructure = value;
      return this;
    }

    /// <summary>
    /// Whether to visually distinguish Selected node and its parent nodes from other non-Selected nodes.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder HighlightSelectedPath(bool value)
    {
      tabstrip.HighlightSelectedPath = value;
      return this;
    }
    /// <summary>
    /// Prefix to use for all image URL paths. For non-image URLs, use BaseUrl. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ImagesBaseUrl(string value)
    {
      tabstrip.ImagesBaseUrl = value;
      return this;
    }
    /// <summary>
    /// Whether to enable keyboard control of the TabStrip. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder KeyboardEnabled(bool value)
    {
      tabstrip.KeyboardEnabled = value;
      return this;
    }
    /// <summary>
    /// ID of ComponentArt MultiPage to control from this navigator. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder MultiPageId(string value)
    {
      tabstrip.MultiPageId = value;
      return this;
    }
    /// <summary>
    /// Whether to persist custom attributes of nodes to JavaScript. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder OutputCustomAttributes(bool value)
    {
      tabstrip.OutputCustomAttributes = value;
      return this;
    }
    /// <summary>
    /// Whether to pre-render the entire structure on the client,
    /// instead of only the initially visible parts. Default: false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder PreRenderAllLevels(bool value)
    {
      tabstrip.PreRenderAllLevels = value;
      return this;
    }
    /// <summary>
    /// ID of item to begin rendering down from. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder RenderRootItemId(string value)
    {
      tabstrip.RenderRootItemId = value;
      return this;
    }
    /// <summary>
    /// Whether to include the RenderRootItem when rendering, instead of only its children. Default: false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder RenderRootItemInclude(bool value)
    {
      tabstrip.RenderRootItemInclude = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine stamp.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder RenderSearchEngineStamp(bool value)
    {
      tabstrip.RenderSearchEngineStamp = value;
      return this;
    }
    /// <summary>
    /// Whether to render the search engine structure for detected crawlers. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder RenderSearchEngineStructure(bool value)
    {
      tabstrip.RenderSearchEngineStructure = value;
      return this;
    }
    /// <summary>
    /// Path to the site map XML file.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder SiteMapXmlFile(string value)
    {
      tabstrip.SiteMapXmlFile = value;
      return this;
    }
    /// <summary>
    /// The name of the standard SOA.UI service to use in
    /// web service mode (to be used instead of WebService/WebServiceMethod). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder SoaService(string value)
    {
      tabstrip.SoaService = value;
      return this;
    }
    /// <summary>
    /// Text displayed when the mouse pointer hovers over the TabStrip element.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder ToolTip(string value)
    {
      tabstrip.ToolTip = value;
      return this;
    }
    /// <summary>
    /// Whether the TabStrip's markup is rendered on the page.  To hide the TabStrip, use CSS.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder Visible(bool value)
    {
      tabstrip.Visible = value;
      return this;
    }
    /// <summary>
    /// The name of the ASP.NET AJAX web service to use for initially populating the TabStrip. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder WebService(string value)
    {
      tabstrip.WebService = value;
      return this;
    }
    /// <summary>
    /// The (optional) custom parameter to send with each web service request. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder WebServiceCustomParameter(string value)
    {
      tabstrip.WebServiceCustomParameter = value;
      return this;
    }
    /// <summary>
    /// The name of the ASP.NET AJAX web service method to use for initially populating the TabStrip. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TabStripBuilder WebServiceMethod(string value)
    {
      tabstrip.WebServiceMethod = value;
      return this;
    }
    /// <summary>
    /// Output the markup to generate a TabStrip object in HTML and, if necessary, the associated engine scripts.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {

      System.IO.StringWriter stringWriter = new System.IO.StringWriter();
      System.Web.UI.HtmlTextWriter htmlTextWriter1 = new System.Web.UI.HtmlTextWriter(stringWriter);
      tabstrip.RenderControl(htmlTextWriter1);

      return stringWriter.ToString();
    }
  }
}
