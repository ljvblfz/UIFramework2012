using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices; 
using System.Xml;
using ComponentArt.Licensing.Providers;
using System.Web.UI.HtmlControls;

namespace ComponentArt.Web.UI
{
  #region WebService classes

  /// <summary>
  /// Contains information describing a web service request.
  /// </summary>
  /// <remarks>
  /// This is the type passed to the web service method set to handle such requests.
  /// </remarks>
  /// <seealso cref="TabStrip.WebService" />
  /// <seealso cref="TabStrip.WebServiceMethod" />
  public class TabStripWebServiceRequest
  {
    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains information to be included in a web service response.
  /// </summary>
  /// <remarks>
  /// This is the type returned from the web service method set to handle requests for tabs.
  /// </remarks>
  /// <seealso cref="TabStrip.WebService" />
  /// <seealso cref="TabStrip.WebServiceMethod" />
  public class TabStripWebServiceResponse : BaseNavigatorWebServiceResponse
  {
    TabStripTabCollection _tabs;

    /// <summary>
    /// Node data to be sent back to the client. Read-only.
    /// </summary>
    public ArrayList Tabs
    {
      get
      {
        return NodesToArray(_tabs);
      }
    }

    public TabStripWebServiceResponse()
    {
      _tabs = new TabStripTabCollection(null, null);
    }

    public void AddTab(TabStripTab oTab)
    {
      _tabs.Add(oTab);
    }
  }

  #endregion


  /// <summary>
  /// Creates multiple tabs, like dividers in a notebook or labels in a set of folders in a filing cabinet.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     TabStrip <b>contents</b> are organized as a hierarchy of <see cref="TabStripTab"/> objects, accessed via the <see cref="Tabs"/> property.
  ///     There are a number of ways to manipulate the tabstrip's <b>contents</b>:
  ///     <list type="bullet">
  ///       <item>Using the TabStrip <b>designer</b> to visually set up the structure.</item>
  ///       <item><b>Inline</b> within the aspx (or ascx) file, by nesting the tab structure within the TabStrip tag's inner property tag &lt;Tabs&gt;.</item>
  ///       <item>From an XML <b>file</b> specified by the <see cref="BaseNavigator.SiteMapXmlFile"/> property.</item>
  ///       <item>Programmatically on the server by using the server-side API.</item>
  ///       <item>Programmatically on the client by using the client-side API.  For more information, see 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_ClientSide_API.htm">Overview of Web.UI Client-side Programming</a>
  ///         and client-side reference for <a href="ms-help:/../ComponentArt.Web.UI.AJAX/webui_clientside_tabstrip.html">TabStrip</a> and 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/webui_clientside_tabstriptab.html">TabStripTab</a> classes.</item>
  ///     </list>
  ///   </para>
  ///   <para>
  ///     TabStrip <b>styles</b> are largely specified via CSS classes, which need to be defined separate from the TabStrip.
  ///     The CSS classes and other presentation-related settings are then assigned via various properties of the TabStrip and related classes.
  ///     For more information see <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_Look_and_Feel_Properties.htm">Look and Feel Properties</a>.
  ///     <list type="bullet">
  ///       <item>In order to streamline the setting of presentation properties for tabstrip tabs, many of the properties are grouped
  ///         within the <see cref="ItemLook"/> object.  To learn more about <see cref="BaseMenu.ItemLooks"/>,
  ///         see <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_ItemLook_Concepts.htm">ItemLook Concepts</a> and 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_Navigation_ItemLooks.htm">Overview of ItemLooks in ComponentArt Navigation Controls</a>.</item>
  ///       <item>Further customization of tab styles and contents can be accomplished with <see cref="BaseNavigator.ServerTemplates"/> and 
  ///         <see cref="BaseNavigator.ClientTemplates"/>.  For more information on templates, see 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_Templates_Overview.htm">Overview of Templates in Web.UI</a>.</item>
  ///       <item>A tabstrip with no information specified will be rendered with a default set of CSS class definitions and assignments.</item>
  ///     </list>
  ///   </para>
  ///   <para>
  ///     There are a number of settings for customizing TabStrip's <b>layout</b> and behaviour:
  ///     <list type="bullet">
  ///       <item>Tabs can <b>scroll</b> within the tabstrip when <see cref="ScrollingEnabled"/> is set to true.</item>
  ///       <item>While most tabstrips contain only a single level of tabs, multiple levels can be <b>nested</b> automatically, simply by organizing 
  ///         <see cref="Tabs"/> into a deeper hierarchy.</item>
  ///       <item><b>TabStrip</b> can be <b>oriented</b> horizontally or vertically using <see cref="Orientation"/> property.</item>
  ///       <item>Contents of <b>tabs</b> can also be <b>oriented</b> in different ways using <see cref="TabOrientation"/> property.</item>
  ///       <item>Each group of tabs can be <b>aligned</b> in various ways using properties of type <see cref="TabStripAlign"/> such as 
  ///         <see cref="DefaultGroupAlign"/>.</item>
  ///       <item>Tabs can be made to seemingly <b>overlap</b> by using tab separators.  
  ///         For more information see <a href="ms-help:/../ComponentArt.Web.UI.AJAX/TabStrip_Separators.htm">Using Tab Separators</a>.</item>
  ///     </list>
  ///     TabStrip is often used in conjunction with <see cref="MultiPage"/> control.
  ///     For an example see <a href="ms-help:/../ComponentArt.Web.UI.AJAX/TabStrip_Creating_a_Tabbed_Dialog_Code_Walkthrough.htm">Creating a Tabbed Dialog</a>.
  ///   </para>
  /// </remarks>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [PersistChildren(false)]
  [ParseChildren(true)]
  [Designer(typeof(ComponentArt.Web.UI.TabStripTabsDesigner))]
  public sealed class TabStrip : BaseMenu
  {
    #region Public Properties

		#region Unused hidden inheritance

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]	
    public override Color BorderColor
    {
      get { return base.BorderColor; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]	
    public override BorderStyle BorderStyle
    {
      get { return base.BorderStyle; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]	
    public override Unit BorderWidth
    {
      get { return base.BorderWidth; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Enabled
    {
      get { return base.Enabled; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool PreRenderAllLevels
    {
      get { return base.PreRenderAllLevels; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override short TabIndex
    {
      get { return base.TabIndex; }
    }

		#endregion

    /// <summary>
    /// Whether to use predefined CSS classes for theming.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to use predefined CSS classes for theming.")]
    public bool AutoTheming
    {
      get
      {
        object o = ViewState["AutoTheming"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["AutoTheming"] = value;
      }
    }

    /// <summary>
    /// String to be prepended to CSS classes used in theming.  Default is 'cart-'.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("String to be prepended to CSS classes used in theming.  Default is 'cart-'.")]
    public string AutoThemingCssClassPrefix
    {
      get
      {
        object o = ViewState["AutoThemingCssClassPrefix"];
        return o == null ? "cart-" : (string)o;
      }
      set
      {
        ViewState["AutoThemingCssClassPrefix"] = value;
      }
    }

    /// <summary>
    /// The name of the standard SOA.UI service to use in web service mode (to be used instead of WebService/WebServiceMethod).
    /// </summary>
    [DefaultValue("")]
    [Description("The name of the standard SOA.UI service to use in web service mode (to be used instead of WebService/WebServiceMethod).")]
    public string SoaService
    {
        get
        {
            object o = ViewState["SoaService"];
            return o == null ? string.Empty : (string)o;
        }
        set
        {
            ViewState["SoaService"] = value;
        }
    }

    private TabStripClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public TabStripClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new TabStripClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// Client-side command to call when the mouse pointer leaves a tab. This can be any valid client script.
    /// </summary>
    /// <remarks>
    /// <para>Deprecated.  Use <see cref="TabStripClientEvents.TabMouseOut">ClientEvents.TabMouseOut</see> instead.</para>
    /// The function is passed a client-side TabStripTab object corresponding to the tab that fired the mouseout.
    /// <para>This is a tabstrip-wide event, called whenever the mouse pointer leaves any tab.</para>
    /// </remarks>
    /// <example>
    /// <see cref="ClientSideOnTabMouseOver" /> contains an example that uses <b>ClientSideOnTabMouseOut</b>.
    /// </example>
    /// <seealso cref="ClientSideOnTabMouseOver" />
    [Browsable(false)]
    [Category("Behavior")]
    [Description("Deprecated.  Use ClientEvents.TabMouseOut instead.")]
    [Obsolete("Deprecated.  Use ClientEvents.TabMouseOut instead.", false)]
    public string ClientSideOnTabMouseOut
    {
      get
      {
        return (string)Properties["ClientSideOnTabMouseOut"];
      }
      set
      {
        Properties["ClientSideOnTabMouseOut"] = value;
      }
    }

    /// <summary>
    /// Client-side command to call when the mouse pointer enters a tab. This can be any valid client script.
    /// </summary>
    /// <remarks>
    /// <para>Deprecated.  Use <see cref="TabStripClientEvents.TabMouseOver">ClientEvents.TabMouseOver</see> instead.</para>
    /// The function is passed a client-side TabStripTab object corresponding to the tab that fired the mouseover.
    /// <para>This is a tabstrip-wide event, called whenever the mouse pointer enters any tab.</para>
    /// </remarks>
    /// <example>
    /// The following example displays in the window's status bar the text of the tab that the mouse pointer is over. 
    /// <see cref="ClientSideOnTabMouseOut" /> is used in order to clear the window status when the mouse pointer leaves
    /// the tab.
    /// <code>
    /// <![CDATA[
    /// &lt;%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
    /// &lt;html>
    /// &lt;head>
    ///   &lt;script type="text/javascript">
    ///   function tabMouseOver(tabstripTab)
    ///   {
    ///     window.status = tabstripTab.Text;
    ///   }
    ///   function tabMouseOut(tabstripTab)
    ///   {
    ///     window.status = '';
    ///   }
    ///   &lt;/script>
    /// &lt;/head>
    /// &lt;body>
    /// &lt;form runat="server">
    /// &lt;ComponentArt:TabStrip ID="TabStrip1" runat="server" 
    ///   ClientSideOnTabMouseOver="tabMouseOver"
    ///   ClientSideOnTabMouseOut="tabMouseOut">
    ///   &lt;tabs>
    ///     &lt;ComponentArt:TabStripTab Text="Tab One" />
    ///     &lt;ComponentArt:TabStripTab Text="Tab Two" />
    ///   &lt;/tabs>
    /// &lt;/ComponentArt:TabStrip>
    /// &lt;/form>
    /// &lt;/body>
    /// &lt;/html>
    /// ]]>
    /// </code>
    /// </example>
    /// <seealso cref="ClientSideOnTabMouseOut" />
    [Browsable(false)]
    [Category("Behavior")]
    [Description("Deprecated.  Use ClientEvents.TabMouseOver instead.")]
    [Obsolete("Deprecated.  Use ClientEvents.TabMouseOver instead.", false)]
    public string ClientSideOnTabMouseOver
    {
      get
      {
        return (string)Properties["ClientSideOnTabMouseOver"];
      }
      set
      {
        Properties["ClientSideOnTabMouseOver"] = value;
      }
    }

    /// <summary>
    /// Client-side function to call when a tab is selected. This can be any valid client script.
    /// </summary>
    /// <remarks>
    /// <para>Deprecated.  Use <see cref="TabStripClientEvents.TabSelect">ClientEvents.TabSelect</see> instead.</para>
    /// The function is passed a client-side TabStripTab object corresponding to the tab that was clicked.
    /// The function can cancel the select event by returning false.
    /// <para>This is a TabStrip-wide event, called whenever any tab is selected.  For a tab-specific client-side 
    /// action, use <see cref="NavigationNode.ClientSideCommand" />.  Note that <b>ClientSideOnTabSelect</b> runs before 
    /// <b>ClientSideCommand</b>, and can cancel its execution by returning false.</para>
    /// </remarks>
    /// <example>
    /// The following example prompts the user to confirm the selection of the tab, and performs the postback
    /// only if the user chooses so.
    /// <code>
    /// <![CDATA[
    /// &lt;%@ Register Prefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
    /// &lt;html>
    /// &lt;head>
    ///   &lt;script type="text/javascript">
    ///   function ConfirmTabSelection(tab)
    ///   {
    ///     return confirm('Are you sure you want to select "' + tab.Text + '"?');
    ///   }
    ///   &lt;/script>
    /// &lt;/head>
    /// &lt;body>
    /// &lt;form runat="server">
    ///   &lt;ComponentArt:TabStrip ID="TabStrip1" runat="server" AutoPostBackOnSelect="true" 
    ///     ClientSideOnTabSelect="ConfirmTabSelection">
    ///     &lt;tabs>
    ///       &lt;ComponentArt:TabStripTab Text="Tab One" />
    ///       &lt;ComponentArt:TabStripTab Text="Tab Two" />
    ///     &lt;/tabs>
    ///   &lt;/ComponentArt:TabStrip>
    /// &lt;/form>
    /// &lt;/body>
    /// &lt;/html>
    /// ]]>
    /// </code>
    /// </example>
    [Browsable(false)]
    [Category("Behavior")]
    [Description("Deprecated.  Use ClientEvents.TabSelect instead.")]
    [Obsolete("Deprecated.  Use ClientEvents.TabSelect instead.", false)]
    public string ClientSideOnTabSelect
    {
      get
      {
        return (string)Properties["ClientSideOnTabSelect"];
      }
      set
      {
        Properties["ClientSideOnTabSelect"] = value;
      }
    }

    /// <summary>
    /// Alignment of tabs in tab groups.
    /// </summary>
    /// <value>
    /// Default value is TabStripAlign.Left.
    /// </value>
    /// <seealso cref="TopGroupAlign" />
    /// <seealso cref="TabStripTab.SubGroupAlign" />
    /// <seealso cref="TabStripTab.DefaultSubGroupAlign" />
    [Category("Layout")]
    [DefaultValue(TabStripAlign.Left)]
    [Description("Alignment of tabs in tab groups.")]
    public TabStripAlign DefaultGroupAlign
    {
      get
      {
        return Utils.ParseTabStripAlign(Properties["DefaultGroupAlign"]);
      }
      set
      {
        Properties["DefaultGroupAlign"] = value.ToString();
      }
    }

    /// <summary>
    /// Direction in which the groups expand.
    /// </summary>
    /// <value>
    /// Default value is GroupExpandDirection.Auto.
    /// </value>
    /// <seealso cref="DefaultGroupExpandOffsetX" />
    /// <seealso cref="DefaultGroupExpandOffsetY" />
    /// <seealso cref="TabStripTab.DefaultSubGroupExpandDirection" />
    /// <seealso cref="TabStripTab.SubGroupExpandDirection" />
    [Category("Layout")]
    [DefaultValue(GroupExpandDirection.Auto)]
    [Description("Direction in which the groups expand.")]
    public GroupExpandDirection DefaultGroupExpandDirection
    {
      get 
      {
        return Utils.ParseGroupExpandDirection(Properties["DefaultGroupExpandDirection"]);
      }
      set 
      {
        Properties["DefaultGroupExpandDirection"] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along x-axis from groups' normal expand positions.
    /// </summary>
    /// <value>
    /// Default value is 0.
    /// </value>
    /// <seealso cref="DefaultGroupExpandDirection" />
    /// <seealso cref="DefaultGroupExpandOffsetY" />
    /// <seealso cref="TabStripTab.SubGroupExpandOffsetX" />
    /// <seealso cref="TabStripTab.DefaultSubGroupExpandOffsetX" />
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along x-axis from groups' normal expand positions.")]
    public int DefaultGroupExpandOffsetX
    {
      get 
      {
        return Utils.ParseInt(Properties["DefaultGroupExpandOffsetX"]);
      }
      set 
      {
        Properties["DefaultGroupExpandOffsetX"] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along y-axis from groups' normal expand positions.
    /// </summary>
    /// <value>
    /// Default value is 0.
    /// </value>
    /// <seealso cref="DefaultGroupExpandDirection" />
    /// <seealso cref="DefaultGroupExpandOffsetX" />
    /// <seealso cref="TabStripTab.SubGroupExpandOffsetY" />
    /// <seealso cref="TabStripTab.DefaultSubGroupExpandOffsetY" />
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along y-axis from groups' normal expand positions.")]
    public int DefaultGroupExpandOffsetY
    {
      get 
      {
        return Utils.ParseInt(Properties["DefaultGroupExpandOffsetY"]);
      }
      set 
      {
        Properties["DefaultGroupExpandOffsetY"] = value.ToString();
      }
    }

    /// <summary>
    /// Default height for the first separator in a group.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default height for the first separator in a group.")]
    public Unit DefaultGroupFirstSeparatorHeight
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultGroupFirstSeparatorHeight"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultGroupFirstSeparatorHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Default width for the first separator in a group.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default width for the first separator in a group.")]
    public Unit DefaultGroupFirstSeparatorWidth
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultGroupFirstSeparatorWidth"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultGroupFirstSeparatorWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Whether to expand the tab groups so they fill the dimensions of the TabStrip.
    /// </summary>
    /// <value>
    /// Default value is true.
    /// </value>
    [Category("Appearance")]
    [Description("Whether to expand the tab groups so they fill the dimensions of the TabStrip.")]
    [DefaultValue(true)]
    public bool DefaultGroupFullExpand
    {
      get 
      {
        object o = Properties["DefaultGroupFullExpand"]; 
        return (o == null) ? true : Utils.ParseBool(o,true); 
      }
      set 
      {
        Properties["DefaultGroupFullExpand"] = value.ToString();
      }
    }

    /// <summary>
    /// Height of tab groups.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    /// <seealso cref="System.Web.UI.WebControls.WebControl.Height" />
    /// <seealso cref="TabStripTab.SubGroupHeight" />
    /// <seealso cref="TabStripTab.DefaultSubGroupHeight" />
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Height of tab groups.")]
    public Unit DefaultGroupHeight
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultGroupHeight"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties["DefaultGroupHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Default height for the last separator in a group.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default height for the last separator in a group.")]
    public Unit DefaultGroupLastSeparatorHeight
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultGroupLastSeparatorHeight"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultGroupLastSeparatorHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Default width for the last separator in a group.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default width for the last separator in a group.")]
    public Unit DefaultGroupLastSeparatorWidth
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultGroupLastSeparatorWidth"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultGroupLastSeparatorWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Default height for the separators in a group.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default height for the separators in a group.")]
    public Unit DefaultGroupSeparatorHeight
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultGroupSeparatorHeight"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultGroupSeparatorHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Default width for the separators in a group.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default width for the separators in a group.")]
    public Unit DefaultGroupSeparatorWidth
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultGroupSeparatorWidth"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultGroupSeparatorWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Whether to show separator images for the tab groups.
    /// </summary>
    /// <value>
    /// Default value is false.
    /// </value>
    [Category("Appearance")]
    [Description("Whether to show separator images for the tab groups.")]
    [DefaultValue(false)]
    public bool DefaultGroupShowSeparators
    {
      get 
      {
        object o = Properties["DefaultGroupShowSeparators"]; 
        return (o == null) ? false : Utils.ParseBool(o,false); 
      }
      set 
      {
        Properties["DefaultGroupShowSeparators"] = value.ToString();
      }
    }

    /// <summary>
    /// Spacing between group tabs.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    /// <seealso cref="TopGroupTabSpacing" />
    /// <seealso cref="TabStripTab.SubGroupTabSpacing" />
    /// <seealso cref="TabStripTab.DefaultSubGroupTabSpacing" />
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Spacing between group tabs.")]
    public Unit DefaultGroupTabSpacing
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultGroupTabSpacing"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultGroupTabSpacing"] = value.ToString();
        }
        else
        {
          throw new Exception("Tab spacing may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// File folder where the separator images for tab groups are located.
    /// </summary>
    /// <value>
    /// Default value is null.
    /// </value>
    [Description("File folder where the separator images for tab groups are located.")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultGroupSeparatorImagesFolderUrl
    {
      get 
      {
        return (string)Properties["DefaultGroupSeparatorImagesFolderUrl"];
      }
      set 
      {
        Properties["DefaultGroupSeparatorImagesFolderUrl"] = value;
      }
    }

    /// <summary>
    /// Width of tab groups.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    /// <seealso cref="System.Web.UI.WebControls.WebControl.Width" />
    /// <seealso cref="TabStripTab.SubGroupWidth" />
    /// <seealso cref="TabStripTab.DefaultSubGroupWidth" />
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Width of tab groups.")]
    public Unit DefaultGroupWidth
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultGroupWidth"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties["DefaultGroupWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Collection of root TabStripTabs.
    /// </summary>
    [Browsable(false)]
    [Description("Collection of root TabStripTabs.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public TabStripTabCollection Tabs
    {
      get
      {
        return (TabStripTabCollection)nodes;
      }
    }

    /// <summary>
    /// Whether to enable keyboard control of the TabStrip.
    /// </summary>
    /// <value>
    /// When true, the tabstrip responds to keyboard shortcuts.  Default value is true.
    /// </value>
    /// <seealso cref="NavigationNode.KeyboardShortcut" />
    [Category("Behavior")]
    [Description("Whether to enable keyboard control of the TabStrip.")]
    [DefaultValue(true)]
    public bool KeyboardEnabled
    {
      get 
      {
        object o = Properties["KeyboardEnabled"]; 
        return (o == null) ? true : Utils.ParseBool(o,true); 
      }
      set 
      {
        Properties["KeyboardEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// Orientation of the TabStrip.
    /// </summary>
    /// <value>
    /// Default value is TabStripOrientation.HorizontalTopToBottom.
    /// </value>
    /// <seealso cref="TabOrientation"/>
    [Category("Layout")]
    [DefaultValue(TabStripOrientation.HorizontalTopToBottom)]
    [Description("Orientation of the TabStrip.")]
    public TabStripOrientation Orientation
    {
      get 
      {
        object o = Properties["Orientation"];
        return (o!=null && o.ToString()!=string.Empty) ? Utils.ParseTabStripOrientation(o) : TabStripOrientation.HorizontalTopToBottom;
      }
      set 
      {
        Properties["Orientation"] = value.ToString();
      }
    }

    private ItemLook _scrollDownLook;
    /// <summary>
    /// Look of the down scroller.
    /// </summary>
    /// <seealso cref="ScrollDownLookId" /><seealso cref="ScrollLeftLook" /><seealso cref="ScrollRightLook" /><seealso cref="ScrollUpLook" /><seealso cref="ScrollingEnabled" />
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook ScrollDownLook
    {
      get
      {
        if(_scrollDownLook == null)
        {
          _scrollDownLook = new ItemLook();
        }
        return _scrollDownLook;
      }
      set
      {
        if(value != null)
        {
          _scrollDownLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the look for down scroller.
    /// </summary>
    /// <seealso cref="ScrollDownLook" /><seealso cref="ScrollingEnabled" />
    [Description("The ID of the look for down scroller.")]
    [DefaultValue("")]
    [Category("Appearance")]
    public string ScrollDownLookId
    {
      get 
      {
        object o = Properties["ScrollDownLookId"]; 
        return (o == null) ? this.DefaultItemLookId : (string)o; 
      }
      set 
      {
        Properties["ScrollDownLookId"] = value;
      }
    }

    private ItemLook _scrollLeftLook;
    /// <summary>
    /// Look of the left scroller.
    /// </summary>
    /// <seealso cref="ScrollLeftLookId" /><seealso cref="ScrollDownLook" /><seealso cref="ScrollRightLook" /><seealso cref="ScrollUpLook" /><seealso cref="ScrollingEnabled" />
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook ScrollLeftLook
    {
      get
      {
        if(_scrollLeftLook == null)
        {
          _scrollLeftLook = new ItemLook();
        }
        return _scrollLeftLook;
      }
      set
      {
        if(value != null)
        {
          _scrollLeftLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the look for left scroller.
    /// </summary>
    /// <seealso cref="ScrollLeftLook" /><seealso cref="ScrollingEnabled" />
    [Description("The ID of the look for left scroller.")]
    [DefaultValue("")]
    [Category("Appearance")]
    public string ScrollLeftLookId
    {
      get 
      {
        object o = Properties["ScrollLeftLookId"]; 
        return (o == null) ? this.DefaultItemLookId : (string)o; 
      }
      set 
      {
        Properties["ScrollLeftLookId"] = value;
      }
    }

    private ItemLook _scrollRightLook;
    /// <summary>
    /// Look of the right scroller.
    /// </summary>
    /// <seealso cref="ScrollRightLookId" /><seealso cref="ScrollDownLook" /><seealso cref="ScrollLeftLook" /><seealso cref="ScrollUpLook" /><seealso cref="ScrollingEnabled" />
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook ScrollRightLook
    {
      get
      {
        if(_scrollRightLook == null)
        {
          _scrollRightLook = new ItemLook();
        }
        return _scrollRightLook;
      }
      set
      {
        if(value != null)
        {
          _scrollRightLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the look for right scroller.
    /// </summary>
    /// <seealso cref="ScrollRightLook" /><seealso cref="ScrollingEnabled" />
    [Description("The ID of the look for right scroller.")]
    [DefaultValue("")]
    [Category("Appearance")]
    public string ScrollRightLookId
    {
      get 
      {
        object o = Properties["ScrollRightLookId"]; 
        return (o == null) ? this.DefaultItemLookId : (string)o; 
      }
      set 
      {
        Properties["ScrollRightLookId"] = value;
      }
    }

    private ItemLook _scrollUpLook;
    /// <summary>
    /// Look of the up scroller.
    /// </summary>
    /// <seealso cref="ScrollUpLookId" /><seealso cref="ScrollDownLook" /><seealso cref="ScrollLeftLook" /><seealso cref="ScrollRightLook" /><seealso cref="ScrollingEnabled" />
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook ScrollUpLook
    {
      get
      {
        if(_scrollUpLook == null)
        {
          _scrollUpLook = new ItemLook();
        }
        return _scrollUpLook;
      }
      set
      {
        if(value != null)
        {
          _scrollUpLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the look for up scroller.
    /// </summary>
    /// <seealso cref="ScrollUpLook" /><seealso cref="ScrollingEnabled" />
    [Description("The ID of the look for up scroller.")]
    [DefaultValue("")]
    [Category("Appearance")]
    public string ScrollUpLookId
    {
      get 
      {
        object o = Properties["ScrollUpLookId"]; 
        return (o == null) ? this.DefaultItemLookId : (string)o; 
      }
      set 
      {
        Properties["ScrollUpLookId"] = value;
      }
    }

    /// <summary>
    /// Whether to enable scrolling for this TabStrip's groups.
    /// </summary>
    /// <value>
    /// When true, the tabstrip implements scrolling of horizontal groups that cannot fit all their items within their space.
    /// When false, the tabstrip allows the groups to grow past their specified space.  Default is false.
    /// </value>
    /// <seealso cref="ScrollDownLook" /><seealso cref="ScrollLeftLook" /><seealso cref="ScrollRightLook" /><seealso cref="ScrollUpLook" />
    [Category("Behavior")]
    [DefaultValue(false)]
    [Description("Whether to enable scrolling for this TabStrip's groups.")]
    public bool ScrollingEnabled
    {
      get
      {
        return Utils.ParseBool(Properties["ScrollingEnabled"], false); 
      }
      set
      {
        Properties["ScrollingEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// The selected tab.
    /// </summary>
    /// <value>
    /// The tab that is considered selected, or null if none is.
    /// </value>
    /// <remarks>
    /// This property can be set on the server-side to force a tab selection.
    /// </remarks>
    /// <seealso cref="BaseMenu.ForceHighlightedItemID" />
    [Browsable(false)]
    [Description("The selected tab.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TabStripTab SelectedTab
    {
      get
      {
        return (TabStripTab)(base.selectedNode);
      }
      set
      {
        base.selectedNode = value;

        if(value != null)
        {
          base.selectedNodePostbackId = value.PostBackID;
        }
      }
    }

    /// <summary>
    /// Extension for separator image files.
    /// </summary>
    /// <value>
    /// Default value is "gif".
    /// </value>
    [Description("Extension for separator image files.")]
    [DefaultValue("gif")]
    [Category("Appearance")]
    public string SeparatorImagesExtension
    {
      get
      {
        object o = Properties["SeparatorImagesExtension"];
        return (o == null) ? "gif" : (string)o;
      }
      set
      {
        Properties["SeparatorImagesExtension"] = value;
      }
    }

    /// <summary>
    /// How each tab's contents are oriented.
    /// </summary>
    /// <value>
    /// Default value is TabOrientation.Horizontal.
    /// </value>
    /// <seealso cref="Orientation"/>
    [Category("Layout")]
    [DefaultValue(TabOrientation.Horizontal)]
    [Description("How each tab's contents are oriented.")]
    public TabOrientation TabOrientation
    {
      get 
      {
        return Utils.ParseTabOrientation(Properties["TabOrientation"]);
      }
      set 
      {
        Properties["TabOrientation"] = value.ToString();
      }
    }

    /// <summary>
    /// How tabs in top tab group are aligned.
    /// </summary>
    /// <seealso cref="DefaultGroupAlign" />
    /// <seealso cref="TabStripTab.SubGroupAlign" />
    /// <seealso cref="TabStripTab.DefaultSubGroupAlign" />
    [Category("Layout")]
    [DefaultValue(TabStripAlign.Left)]
    [Description("How tabs in top tab group are aligned.")]
    public TabStripAlign TopGroupAlign
    {
      get 
      {
        object o = Properties["TopGroupAlign"];
        return (o != null) ? Utils.ParseTabStripAlign(o) : this.DefaultGroupAlign;
      }
      set 
      {
        Properties["TopGroupAlign"] = value.ToString();
      }
    }

    /// <summary>
    /// CSS class to apply to the top group.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(null)]
    [Description("CSS class to apply to the top group.")]
    public string TopGroupCssClass
    {
      get 
      {
        object o = Properties["TopGroupCssClass"];
        return (o != null) ? o.ToString() : this.DefaultGroupCssClass;
      }
      set 
      {
        Properties["TopGroupCssClass"] = value;
      }
    }

    /// <summary>
    /// Height of the first separator in the top group.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Height of the first separator in the top group.  Default is Unit.Empty.")]
    public Unit TopGroupFirstSeparatorHeight
    {
      get 
      {
        object o = Properties["TopGroupFirstSeparatorHeight"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupFirstSeparatorHeight;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["TopGroupFirstSeparatorHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Width of the first separator in the top group.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Width of the first separator in the top group.  Default is Unit.Empty.")]
    public Unit TopGroupFirstSeparatorWidth
    {
      get 
      {
        object o = Properties["TopGroupFirstSeparatorWidth"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupFirstSeparatorWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["TopGroupFirstSeparatorWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Whether to expand the top group so that it fills exactly the dimensions of the TabStrip.
    /// </summary>
    [Category("Appearance")]
    [Description("Whether to expand the top group so that it fills exactly the dimensions of the TabStrip.")]
    [DefaultValue(true)]
    public bool TopGroupFullExpand
    {
      get 
      {
        object o = Properties["TopGroupFullExpand"]; 
        return (o != null) ? Utils.ParseBool(o,true) : this.DefaultGroupFullExpand; 
      }
      set 
      {
        Properties["TopGroupFullExpand"] = value.ToString();
      }
    }

    /// <summary>
    /// Top group height.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Top group height.")]
    public Unit TopGroupHeight
    {
      get 
      {
        object o = Properties["TopGroupHeight"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupHeight;
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties["TopGroupHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Height of the last separator in the top group.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Height of the last separator in the top group.  Default is Unit.Empty.")]
    public Unit TopGroupLastSeparatorHeight
    {
      get 
      {
        object o = Properties["TopGroupLastSeparatorHeight"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupLastSeparatorHeight;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["TopGroupLastSeparatorHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Width of the last separator in the top group.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Width of the last separator in the top group.  Default is Unit.Empty.")]
    public Unit TopGroupLastSeparatorWidth
    {
      get 
      {
        object o = Properties["TopGroupLastSeparatorWidth"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupLastSeparatorWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["TopGroupLastSeparatorWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Whether to show separator images for the top tab group.
    /// </summary>
    [Category("Appearance")]
    [Description("Whether to show separator images for the top group.  Default is false.")]
    [DefaultValue(false)]
    public bool TopGroupShowSeparators
    {
      get 
      {
        object o = Properties["TopGroupShowSeparators"]; 
        return (o != null) ? Utils.ParseBool(o,false) : this.DefaultGroupShowSeparators; 
      }
      set 
      {
        Properties["TopGroupShowSeparators"] = value.ToString();
      }
    }

    /// <summary>
    /// Top group separator height.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Top group separator height.  Default is Unit.Empty.")]
    public Unit TopGroupSeparatorHeight
    {
      get 
      {
        object o = Properties["TopGroupSeparatorHeight"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupSeparatorHeight;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["TopGroupSeparatorHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Folder with top group's separator images.
    /// </summary>
    [Description("Folder with top group's separator images.")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string TopGroupSeparatorImagesFolderUrl
    {
      get 
      {
        object o = Properties["TopGroupSeparatorImagesFolderUrl"];
        return (o != null) ? (string)o : this.DefaultGroupSeparatorImagesFolderUrl;
      }
      set 
      {
        Properties["TopGroupSeparatorImagesFolderUrl"] = value;
      }
    }

    /// <summary>
    /// Top group separator width.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Top group separator width.  Default is Unit.Empty.")]
    public Unit TopGroupSeparatorWidth
    {
      get 
      {
        object o = Properties["TopGroupSeparatorWidth"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupSeparatorWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["TopGroupSeparatorWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Spacing between top group's tabs.  Default is Unit.Empty.
    /// </summary>
    /// <seealso cref="DefaultGroupTabSpacing" />
    /// <seealso cref="TabStripTab.SubGroupTabSpacing" />
    /// <seealso cref="TabStripTab.DefaultSubGroupTabSpacing" />
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Spacing between top group's tabs.  Default is Unit.Empty.")]
    public Unit TopGroupTabSpacing
    {
      get 
      {
        object o = Properties["TopGroupTabSpacing"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupTabSpacing;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["TopGroupTabSpacing"] = value.ToString();
        }
        else
        {
          throw new Exception("Tab spacing may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Top group width.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Top group width.")]
    public Unit TopGroupWidth
    {
      get 
      {
        object o = Properties["TopGroupWidth"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties["TopGroupWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// The name of the ASP.NET AJAX web service to use for initially populating the TabStrip.
    /// </summary>
    /// <seealso cref="WebServiceMethod" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service to use for initially populating the TabStrip.")]
    public string WebService
    {
      get
      {
        object o = ViewState["WebService"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["WebService"] = value;
      }
    }

    /// <summary>
    /// The (optional) custom parameter to send with each web service request.
    /// </summary>
    /// <seealso cref="WebService" />
    /// <seealso cref="WebServiceMethod" />
    [DefaultValue("")]
    [Description("The (optional) custom parameter to send with each web service request.")]
    public string WebServiceCustomParameter
    {
      get
      {
        object o = ViewState["WebServiceCustomParameter"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["WebServiceCustomParameter"] = value;
      }
    }

    /// <summary>
    /// The name of the ASP.NET AJAX web service method to use for initially populating the TabStrip.
    /// </summary>
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service method to use for initially populating the TabStrip.")]
    public string WebServiceMethod
    {
      get
      {
        object o = ViewState["WebServiceMethod"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["WebServiceMethod"] = value;
      }
    }

    #endregion

    #region Protected Properties
    #endregion

    #region Internal Properties
    #endregion

    #region Private Properties
    
    // This array will hold the PostBackIDs of all the tabs with IsChildSelected==true.
    // It is used by the code that generates client-side storage.
    private JavaScriptArray _childSelectedPostBackIDs;

    // The order of image names must match with the order of corresponding property names on the client-side
    private string[] _separatorImageNames =
      {
        "nrm_nrm",
        "nrm_end",
        "end_nrm",
        "sel_nrm",
        "nrm_sel",
        "sel_end",
        "end_sel",
        "csl_nrm",
        "nrm_csl",
        "csl_end",
        "end_csl",
        "dis_dis",
        "dis_nrm",
        "nrm_dis",
        "dis_end",
        "end_dis",
        "dis_sel",
        "sel_dis",
        "dis_csl",
        "csl_dis"
      };

    private Hashtable _propertyIndex;
    internal override Hashtable PropertyIndex
    {
      get
      {
        if (_propertyIndex == null)
        {
          _propertyIndex = new Hashtable();
          _propertyIndex["0"] = "AutoPostBackOnSelect";
          _propertyIndex["1"] = "CausesValidation";
          _propertyIndex["2"] = "ChildSelectedLookId";
          _propertyIndex["3"] = "ClientSideCommand";
          _propertyIndex["4"] = "ClientTemplateId";
          _propertyIndex["5"] = "DefaultSubGroupAlign";
          _propertyIndex["6"] = "DefaultSubGroupCssClass";
          _propertyIndex["7"] = "DefaultSubGroupExpandDirection";
          _propertyIndex["8"] = "DefaultSubGroupExpandOffsetX";
          _propertyIndex["9"] = "DefaultSubGroupExpandOffsetY";
          _propertyIndex["10"] = "DefaultSubGroupFirstSeparatorHeight";
          _propertyIndex["11"] = "DefaultSubGroupFirstSeparatorWidth";
          _propertyIndex["12"] = "DefaultSubGroupFullExpand";
          _propertyIndex["13"] = "DefaultSubGroupHeight";
          _propertyIndex["14"] = "DefaultSubGroupLastSeparatorHeight";
          _propertyIndex["15"] = "DefaultSubGroupLastSeparatorWidth";
          _propertyIndex["16"] = "DefaultSubGroupSeparatorHeight";
          _propertyIndex["17"] = "DefaultSubGroupSeparatorImagesFolderUrl";
          _propertyIndex["18"] = "DefaultSubGroupSeparatorWidth";
          _propertyIndex["19"] = "DefaultSubGroupShowSeparators";
          _propertyIndex["20"] = "DefaultSubGroupTabSpacing";
          _propertyIndex["21"] = "DefaultSubGroupWidth";
          _propertyIndex["22"] = "DefaultSubItemChildSelectedLookId";
          _propertyIndex["23"] = "DefaultSubItemDisabledLookId";
          _propertyIndex["24"] = "DefaultSubItemLookId";
          _propertyIndex["25"] = "DefaultSubItemSelectedLookId";
          _propertyIndex["26"] = "DefaultSubItemTextAlign";
          _propertyIndex["27"] = "DefaultSubItemTextWrap";
          _propertyIndex["28"] = "DisabledLookId";
          _propertyIndex["29"] = "Enabled";
          _propertyIndex["30"] = "Height";
          _propertyIndex["31"] = "ID";
          _propertyIndex["32"] = "KeyboardShortcut";
          _propertyIndex["33"] = "LookId";
          _propertyIndex["34"] = "NavigateUrl";
          _propertyIndex["35"] = "PageViewId";
          _propertyIndex["36"] = "SelectedLookId";
          _propertyIndex["37"] = "ServerTemplateId";
          _propertyIndex["38"] = "SiteMapXmlFile";
          _propertyIndex["39"] = "SubGroupAlign";
          _propertyIndex["40"] = "SubGroupCssClass";
          _propertyIndex["41"] = "SubGroupExpandDirection";
          _propertyIndex["42"] = "SubGroupExpandOffsetX";
          _propertyIndex["43"] = "SubGroupExpandOffsetY";
          _propertyIndex["44"] = "SubGroupFirstSeparatorHeight";
          _propertyIndex["45"] = "SubGroupFirstSeparatorWidth";
          _propertyIndex["46"] = "SubGroupFullExpand";
          _propertyIndex["47"] = "SubGroupHeight";
          _propertyIndex["48"] = "SubGroupLastSeparatorHeight";
          _propertyIndex["49"] = "SubGroupLastSeparatorWidth";
          _propertyIndex["50"] = "SubGroupSeparatorHeight";
          _propertyIndex["51"] = "SubGroupSeparatorImagesFolderUrl";
          _propertyIndex["52"] = "SubGroupSeparatorWidth";
          _propertyIndex["53"] = "SubGroupShowSeparators";
          _propertyIndex["54"] = "SubGroupTabSpacing";
          _propertyIndex["55"] = "SubGroupWidth";
          _propertyIndex["56"] = "Target";
          _propertyIndex["57"] = "Text";
          _propertyIndex["58"] = "TextAlign";
          _propertyIndex["59"] = "TextWrap";
          _propertyIndex["60"] = "ToolTip";
          _propertyIndex["61"] = "Value";
          _propertyIndex["62"] = "Visible";
          _propertyIndex["63"] = "Width";
        }
        return _propertyIndex;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Constructor
    /// </summary>
    public TabStrip() : base()
    {
      // Set some defaults.
      this.nodes = new TabStripTabCollection(this, null);
    }

    /// <summary>
    /// Searches the entire tab tree for an item with the given ID.
    /// </summary>
    public new TabStripTab FindItemById(string itemId)
    {
      return (TabStripTab)base.FindNodeById(itemId);
    }

    /// <summary>
    /// Apply looks to the data: Load specified looks by ID, and apply them.
    /// If called explicitly, this method will overwrite some look settings which were set on individual nodes.
    /// </summary>
    public override void ApplyLooks()
    {
      if (this.ScrollDownLookId != string.Empty)
      {
        this.ScrollDownLook = this.ItemLooks[this.ScrollDownLookId];
      }
      if (this.ScrollLeftLookId != string.Empty)
      {
        this.ScrollLeftLook = this.ItemLooks[this.ScrollLeftLookId];
      }
      if (this.ScrollRightLookId != string.Empty)
      {
        this.ScrollRightLook = this.ItemLooks[this.ScrollRightLookId];
      }
      if (this.ScrollUpLookId != string.Empty)
      {
        this.ScrollUpLook = this.ItemLooks[this.ScrollUpLookId];
      }

      base.ApplyLooks();
    }

    public void ApplyTheming(bool? overwriteSettings)
    {
      bool overwrite = overwriteSettings ?? false;
      string prefix = this.AutoThemingCssClassPrefix ?? "";

      // Base
      if ((this.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.CssClass = prefix + "tabstrip";
      }
      if ((this.DefaultGroupCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultGroupCssClass = prefix + "tabstrip-group";
      }
      if ((this.DefaultItemLookId ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemLookId = "TabStripItemLook";
      }
      if ((this.DefaultSelectedItemLookId ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultSelectedItemLookId= "TabStripSelectedItemLook";
      }
      if ((this.DefaultDisabledItemLookId ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultDisabledItemLookId = "TabStripDisabledItemLook";
      }
      
      // ItemLooks
      ItemLook tabstripItemLook = new ItemLook();
      tabstripItemLook.LookId = "TabStripItemLook";
      if (this.ItemLooks.Count > 0)
      {
        foreach (ItemLook itemLook in this.ItemLooks)
        {
          if (itemLook.LookId == tabstripItemLook.LookId)
          {
            itemLook.CopyTo(tabstripItemLook);
            this.ItemLooks.Remove(itemLook);
            break;
          }
        }
      }
      if ((tabstripItemLook.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.CssClass = prefix + "item-default";
      }
      if ((tabstripItemLook.HoverCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.HoverCssClass = prefix + "item-hover";
      }
      if ((tabstripItemLook.ExpandedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.ExpandedCssClass = prefix + "item-expanded";
      }
      if ((tabstripItemLook.ActiveCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.ActiveCssClass = prefix + "item-active";
      }
      this.ItemLooks.Add(tabstripItemLook);

      tabstripItemLook = new ItemLook();
      tabstripItemLook.LookId = "TabStripSelectedItemLook";
      if (this.ItemLooks.Count > 0)
      {
        foreach (ItemLook itemLook in this.ItemLooks)
        {
          if (itemLook.LookId == tabstripItemLook.LookId)
          {
            itemLook.CopyTo(tabstripItemLook);
            this.ItemLooks.Remove(itemLook);
            break;
          }
        }
      }
      if ((tabstripItemLook.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.CssClass = prefix + "item-selected" + " " + prefix + "item-default";
      }
      if ((tabstripItemLook.HoverCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.HoverCssClass = prefix + "item-selected" + " " + prefix + "item-hover";
      }
      if ((tabstripItemLook.ExpandedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.ExpandedCssClass = prefix + "item-selected" + " " + prefix + "item-expanded";
      }
      if ((tabstripItemLook.ActiveCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.ActiveCssClass = prefix + "item-selected" + " " + prefix + "item-active";
      }
      this.ItemLooks.Add(tabstripItemLook);


      tabstripItemLook = new ItemLook();
      tabstripItemLook.LookId = "TabStripDisabledItemLook";
      if (this.ItemLooks.Count > 0)
      {
        foreach (ItemLook itemLook in this.ItemLooks)
        {
          if (itemLook.LookId == tabstripItemLook.LookId)
          {
            itemLook.CopyTo(tabstripItemLook);
            this.ItemLooks.Remove(itemLook);
            break;
          }
        }
      }
      if ((tabstripItemLook.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.CssClass = prefix + "item-default " + prefix + "item-disabled";
      }
      if ((tabstripItemLook.HoverCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.HoverCssClass = prefix + "item-hover " + prefix + "item-disabled";
      }
      if ((tabstripItemLook.ExpandedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.ExpandedCssClass = prefix + "item-expanded " + prefix + "item-disabled";
      }
      if ((tabstripItemLook.ActiveCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        tabstripItemLook.ActiveCssClass = prefix + "item-active " + prefix + "item-disabled";
      }
      this.ItemLooks.Add(tabstripItemLook);


      // Client Templates
      StringBuilder templateText = new StringBuilder();
      templateText.Append("<div class=\"");
      templateText.Append(prefix);
      templateText.Append(@"tabstrip-top-item"">
								<a href=""## DataItem.getProperty('NavigateUrl') == '' ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##"" onclick=""this.blur();"">
									<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"outer"">
										<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"inner## DataItem.getProperty('IconCssClass') == null ? ' ");
      templateText.Append(prefix);
      templateText.Append(@"none' : ' ' +  DataItem.getProperty('IconCssClass'); ##"">## DataItem.getProperty('Text'); ##</span>
									</span>
								</a>
							</div>
");
      AddClientTemplate(overwrite, "TopLevelTabStripItemTemplate", templateText.ToString());

      // Apply client templates to Tabs
      ApplyThemedClientTemplatesToTabs(overwrite, this.Tabs);
    }

    private void ApplyThemedClientTemplatesToTabs(bool overwrite, TabStripTabCollection items)
    {
      foreach (TabStripTab item in items)
      {
        if (item.ClientTemplateId == string.Empty || overwrite)
        {
          if (item.Text != string.Empty && item.ParentTab == null)
          {
            item.ClientTemplateId = "TopLevelTabStripItemTemplate";
          }
        }

        ApplyThemedClientTemplatesToTabs(overwrite, item.Tabs);
      }
    }

    private void AddClientTemplate(bool overwrite, string id, string text)
    {
      int index = 0;
      bool clientTemplateFound = false;

      while (index < this.ClientTemplates.Count)
      {
        if (this.ClientTemplates[index].ID == id)
        {
          if (overwrite)
          {
            this.ClientTemplates.RemoveAt(index);
          }
          else
          {
            clientTemplateFound = true;
          }
          break;
        }
        index++;
      }

      if (!clientTemplateFound)
      {
        ClientTemplate clientTemplate = new ClientTemplate();
        clientTemplate.ID = id;
        clientTemplate.Text = text;
        this.ClientTemplates.Add(clientTemplate);
      }
    }
    #endregion

    #region Protected Methods

    /// <summary>
    /// Creates a new TabStripTab and adds it as a root.
    /// </summary>
    /// <returns>The new node.</returns>
    protected override NavigationNode AddNode()
    {
      TabStripTab newTab = new TabStripTab();
      this.Tabs.Add(newTab);
      return newTab;
    }

    protected override NavigationNode NewNode()
    {
      TabStripTab newNode = new TabStripTab();
      TabStripTabCollection dummy = newNode.Tabs; // This is a dummy call to ensure that newNode.nodes is not null
      return newNode;
    }

    protected override void ComponentArtFixStructure()
    {
      base.ComponentArtFixStructure();

      // Maintain selected node if we're doing that.
      if (this.selectedNodePostbackId != null && this.nodes != null)
      {
        this.SelectedTab = (TabStripTab)this.FindNodeByPostBackId(this.selectedNodePostbackId, this.nodes);
      }
    }

    /// <summary>
    /// Prepare to render this control.
    /// </summary>
    /// <param name="e">PreRender event arguments.</param>
    protected override void ComponentArtPreRender(EventArgs e)
    {
      base.ComponentArtPreRender(e);

      if (!this.IsDownLevel() && this.ScrollingEnabled)
      {
        // Tack this to the bottom of the page, to know when we've loaded.
        RegisterStartupScript("ComponentArt_Page_Loaded", this.DemarcateClientScript("window.ComponentArt_Page_Loaded = true;"));
      }
    }
    
    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      base.ComponentArtRender(output);

      if(!this.IsDownLevel() && Page != null)
      {
        // do we need to render scripts for non-Atlas?
        ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
        if (oScriptManager == null)
        {
        // Add core code
        if(!Page.IsClientScriptBlockRegistered("A573G988.js"))
        {
          Page.RegisterClientScriptBlock("A573G988.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573S188.js"))
        {
          Page.RegisterClientScriptBlock("A573S188.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573S188.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573Z388.js"))
        {
          Page.RegisterClientScriptBlock("A573Z388.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573Z388.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573C488.js"))
        {
          Page.RegisterClientScriptBlock("A573C488.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.TabStrip.client_scripts", "A573C488.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573I688.js"))
        {
          Page.RegisterClientScriptBlock("A573I688.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.TabStrip.client_scripts", "A573I688.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573B188.js"))
        {
          Page.RegisterClientScriptBlock("A573B188.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.TabStrip.client_scripts", "A573B188.js");
        }
        }
      }

      // do we need default styles?
      if (this.RenderDefaultStyles)
      {
        // render default styles into the page
        string sDefaultStyles = "<style>" + GetResourceContent("ComponentArt.Web.UI.TabStrip.defaultStyle.css") + "</style>";
        output.Write(sDefaultStyles);
      }

      if (this.IsBrowserSearchEngine() && this.RenderSearchEngineStructure || this.ForceSearchEngineStructure)
      {
        RenderCrawlerStructure(output);
      }

      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        RenderAccessibleContent(output);
      }
      else if (this.IsDownLevel())
      {
        if (this.RenderDefaultStyles)
        {
          this.ApplyLooks();
        }
        this.RenderDownLevelContent(output);
      }
      else
      {
        string tabStripClientVarName = this.GetSaneId();

        if (this.AutoTheming)
        {
          this.ApplyTheming(false);
        }

        // Find preloadable images
        this.LoadPreloadImages();
        // Preload images, if any
        if(this.PreloadImages.Count > 0)
        {
          this.RenderPreloadImages(output);
        }

        // Render tab server templates, if any
        foreach(Control oTemplate in this.Controls)
        {
          output.Write("<div id=\"" + oTemplate.ID + "\" style=\"display:none;\">");
          oTemplate.RenderControl(output);
          output.Write("</div>");
        }

        // Output the div tag that will house our top group
        output.Write("<div ");
        output.WriteAttribute("id", tabStripClientVarName);
        if(this.ToolTip != string.Empty)
        {
          output.WriteAttribute("title", this.ToolTip);
        }
        // Output its class
        if (this.CssClass != null && this.CssClass != string.Empty)
        {
          output.WriteAttribute("class", this.CssClass);
        }
        // Output its style
        if (this.Style.Keys.Count > 0 || !this.Width.IsEmpty || !this.Height.IsEmpty)
        {
          output.Write(" style=\"");
          if (!this.Width.IsEmpty) output.WriteStyleAttribute("width", this.Width.ToString());
          if (!this.Height.IsEmpty) output.WriteStyleAttribute("height", this.Height.ToString());
          foreach(string sKey in this.Style.Keys)
          {
            output.WriteStyleAttribute(sKey, this.Style[sKey]);
          }
          output.Write("\"");
        }
        output.Write(">");
        // Mozillas have trouble deciding when to show tabstrip scrollers unless we add some fake content to the div:
        output.Write("<div style=\"width:1px;height:1px;overflow:hidden;visibility:hidden;\">.</div>");
        // Finally close the div tag:
        output.Write("</div>");

        if (this.EnableViewState)
        {
          // Render client-storage-persisting field.
          output.AddAttribute("id", tabStripClientVarName + "_Data");
          output.AddAttribute("name", tabStripClientVarName + "_Data");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render toplevel-property-persisting field.
          output.AddAttribute("id", tabStripClientVarName + "_Properties");
          output.AddAttribute("name", tabStripClientVarName + "_Properties");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();
        }

        // Render the hidden field that will propagate SelectedNode.
        output.AddAttribute("id", tabStripClientVarName + "_SelectedNode");
        output.AddAttribute("name", tabStripClientVarName + "_SelectedNode");
        output.AddAttribute("type", "hidden");
        output.AddAttribute("value", SelectedTab!=null ? SelectedTab.PostBackID : null);
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();

        // Render scroll-maintaining field.
        output.AddAttribute("id", tabStripClientVarName + "_ScrollData");
        output.AddAttribute("name", tabStripClientVarName + "_ScrollData");
        output.AddAttribute("type", "hidden");
        output.AddAttribute("value", (Page.Request.Form[tabStripClientVarName + "_ScrollData"] == null ? "0" : Page.Request.Form[tabStripClientVarName + "_ScrollData"]));
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();

        #region Add tabstrip initialization

        // Add tabstrip data
        string tabStorageArray = "ComponentArt_Storage_" + tabStripClientVarName;
        string lookStorageArray = "ComponentArt_ItemLooks_" + tabStripClientVarName;
        string scrollLookStorageArray = "ComponentArt_ScrollLooks_" + tabStripClientVarName;
        string storage = "window." + tabStorageArray + "=" + this.BuildStorage() + ";\n"
          + "window." + lookStorageArray + "=" + this.BuildLooks() + ";\n"
          + "window." + scrollLookStorageArray + "=" + this.BuildScrollLooks() + ";\n";
        storage = this.DemarcateClientScript(storage, "ComponentArt Web.UI client-side storage for " + tabStripClientVarName);

        WriteStartupScript(output, storage);

        StringBuilder startupSB = new StringBuilder();
        startupSB.Append("window.ComponentArt_Init_" + tabStripClientVarName + " = function() {\n");

        // Include check for whether everything we need is loaded,
        // and a retry after a delay in case it isn't.
        int retryDelay = 50; // Use the quick 50 because if we're scrolling we're expecting to fail on the first try
        string readyToInitializeTabStrip = "window.cart_tabstrip_kernel_loaded && window.cart_tabstrip_support_loaded && document.getElementById('" + tabStripClientVarName + "')";
        if (this.ScrollingEnabled)
        {
          // Scrolling TabStrips need to wait until the entire page is loaded
          readyToInitializeTabStrip += " && window.ComponentArt_Page_Loaded";
        }
        startupSB.Append("if (!(" + readyToInitializeTabStrip + "))\n");
        startupSB.Append("{\n\tsetTimeout('ComponentArt_Init_" + tabStripClientVarName + "()', " + retryDelay.ToString() + ");\n\treturn;\n}\n");

        // Instantiate tabstrip object
        startupSB.Append("window." + tabStripClientVarName + " = new ComponentArt_TabStrip('" + tabStripClientVarName + "'," + tabStorageArray + "," + lookStorageArray + "," + scrollLookStorageArray + ");\n");

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != tabStripClientVarName)
        {
          startupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + tabStripClientVarName + "; " + tabStripClientVarName + ".GlobalAlias = '" + ID + "'; }\n");
        }

        // Define properties
        startupSB.Append(tabStripClientVarName + ".PropertyStorageArray = [\n");
        startupSB.Append("['ApplicationPath'," + Utils.ConvertStringToJSString(Context.Request.ApplicationPath) + "],");
        startupSB.Append("['AutoPostBackOnSelect'," + this.AutoPostBackOnSelect.ToString().ToLower() + "],");
        if (this.AutoTheming)
        {
          startupSB.Append("['AutoTheming',1],");
          startupSB.Append("['AutoThemingCssClassPrefix'," + Utils.ConvertStringToJSString(this.AutoThemingCssClassPrefix) + "],");
        }
        startupSB.Append("['BaseUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context,this.BaseUrl)) + "],");
        startupSB.Append("['ChildSelectedTabPostBackIDs'," + this._childSelectedPostBackIDs.ToString() + "],");
        startupSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "],");
        startupSB.Append("['ClientSideOnTabMouseOut'," + Utils.ConvertStringToJSString(this.ClientSideOnTabMouseOut) + "],");
        startupSB.Append("['ClientSideOnTabMouseOver'," + Utils.ConvertStringToJSString(this.ClientSideOnTabMouseOver) + "],");
        startupSB.Append("['ClientSideOnTabSelect'," + Utils.ConvertStringToJSString(this.ClientSideOnTabSelect) + "],");
        startupSB.Append("['ClientTemplates'," + this._clientTemplates.ToString() +"],");
        startupSB.Append("['ControlId'," + Utils.ConvertStringToJSString(this.UniqueID) + "],");
        startupSB.Append("['DefaultChildSelectedItemLookId'," + Utils.ConvertStringToJSString(this.DefaultChildSelectedItemLookId) + "],");
        startupSB.Append("['DefaultDisabledItemLookId'," + Utils.ConvertStringToJSString(this.DefaultDisabledItemLookId) + "],");
        startupSB.Append("['DefaultGroupAlign'," + (int)this.DefaultGroupAlign + "],");
        startupSB.Append("['DefaultGroupCssClass'," + Utils.ConvertStringToJSString(this.DefaultGroupCssClass) + "],");
        startupSB.Append("['DefaultGroupExpandDirection'," + (int)this.DefaultGroupExpandDirection + "],");
        startupSB.Append("['DefaultGroupExpandOffsetX'," + this.DefaultGroupExpandOffsetX + "],");
        startupSB.Append("['DefaultGroupExpandOffsetY'," + this.DefaultGroupExpandOffsetY + "],");
        startupSB.Append("['DefaultGroupFirstSeparatorHeight'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupFirstSeparatorHeight) + "],");
        startupSB.Append("['DefaultGroupFirstSeparatorWidth'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupFirstSeparatorWidth) + "],");
        startupSB.Append("['DefaultGroupFullExpand'," + this.DefaultGroupFullExpand.ToString().ToLower() + "],");
        startupSB.Append("['DefaultGroupHeight'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupHeight) + "],");
        startupSB.Append("['DefaultGroupLastSeparatorHeight'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupLastSeparatorHeight) + "],");
        startupSB.Append("['DefaultGroupLastSeparatorWidth'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupLastSeparatorWidth) + "],");
        startupSB.Append("['DefaultGroupSeparatorHeight'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupSeparatorHeight) + "],");
        startupSB.Append("['DefaultGroupSeparatorImagesFolderUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context,this.DefaultGroupSeparatorImagesFolderUrl)) + "],");
        startupSB.Append("['DefaultGroupSeparatorWidth'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupSeparatorWidth) + "],");
        startupSB.Append("['DefaultGroupShowSeparators'," + this.DefaultGroupShowSeparators.ToString().ToLower() + "],");
        startupSB.Append("['DefaultGroupTabSpacing'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupTabSpacing) + "],");
        startupSB.Append("['DefaultGroupWidth'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupWidth) + "],");
        startupSB.Append("['DefaultItemLookId'," + Utils.ConvertStringToJSString(this.DefaultItemLookId) + "],");
        startupSB.Append("['DefaultItemTextAlign'," + (int)this.DefaultItemTextAlign + "],");
        startupSB.Append("['DefaultItemTextWrap'," + this.DefaultItemTextWrap.ToString().ToLower() + "],");
        startupSB.Append("['DefaultSelectedItemLookId'," + Utils.ConvertStringToJSString(this.DefaultSelectedItemLookId) + "],");
        startupSB.Append("['DefaultTarget'," + Utils.ConvertStringToJSString(this.DefaultTarget) + "],");
        startupSB.Append("['Height'," + Utils.ConvertUnitToJSConstant(this.Height) + "],");
        startupSB.Append("['ImagesBaseUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context,this.ImagesBaseUrl)) + "],");
        startupSB.Append("['MultiPageId'," + Utils.ConvertStringToJSString(this._multiPageId) + "],");
        startupSB.Append("['Orientation'," + (int)this.Orientation + "],");
        startupSB.Append("['PlaceHolderId'," + Utils.ConvertStringToJSString(tabStripClientVarName) + "],");
        startupSB.Append("['ScrollingEnabled'," + this.ScrollingEnabled.ToString().ToLower() + "],");
        startupSB.Append("['ScrollDataInput','" + tabStripClientVarName + "_ScrollData'],");
        startupSB.Append("['SelectedNodeInput','" + tabStripClientVarName + "_SelectedNode'],");
        startupSB.Append("['SelectedTabPostBackID','" + (SelectedTab!=null ? SelectedTab.PostBackID : null) + "'],");
        startupSB.Append("['SeparatorImagesExtension'," + Utils.ConvertStringToJSString(this.SeparatorImagesExtension) + "],");
        startupSB.Append("['TabOrientation'," + (int)this.TabOrientation + "],");
        startupSB.Append("['TopGroupAlign'," + (int)this.TopGroupAlign + "],");
        startupSB.Append("['TopGroupCssClass'," + Utils.ConvertStringToJSString(this.TopGroupCssClass) + "],");
        startupSB.Append("['TopGroupFirstSeparatorHeight'," + Utils.ConvertUnitToJSConstant(this.TopGroupFirstSeparatorHeight) + "],");
        startupSB.Append("['TopGroupFirstSeparatorWidth'," + Utils.ConvertUnitToJSConstant(this.TopGroupFirstSeparatorWidth) + "],");
        startupSB.Append("['TopGroupFullExpand'," + this.TopGroupFullExpand.ToString().ToLower() + "],");
        startupSB.Append("['TopGroupHeight'," + Utils.ConvertUnitToJSConstant(this.TopGroupHeight) + "],");
        startupSB.Append("['TopGroupLastSeparatorHeight'," + Utils.ConvertUnitToJSConstant(this.TopGroupLastSeparatorHeight) + "],");
        startupSB.Append("['TopGroupLastSeparatorWidth'," + Utils.ConvertUnitToJSConstant(this.TopGroupLastSeparatorWidth) + "],");
        startupSB.Append("['TopGroupSeparatorHeight'," + Utils.ConvertUnitToJSConstant(this.TopGroupSeparatorHeight) + "],");
        startupSB.Append("['TopGroupSeparatorImagesFolderUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context,this.TopGroupSeparatorImagesFolderUrl)) + "],");
        startupSB.Append("['TopGroupSeparatorWidth'," + Utils.ConvertUnitToJSConstant(this.TopGroupSeparatorWidth) + "],");
        startupSB.Append("['TopGroupShowSeparators'," + this.TopGroupShowSeparators.ToString().ToLower() + "],");
        startupSB.Append("['TopGroupTabSpacing'," + Utils.ConvertUnitToJSConstant(this.TopGroupTabSpacing) + "],");
        startupSB.Append("['TopGroupWidth'," + Utils.ConvertUnitToJSConstant(this.TopGroupWidth) + "],");
        startupSB.Append("['SoaService','" + this.SoaService + "'],");
        startupSB.Append("['WebService','" + this.WebService + "'],");
        startupSB.Append("['WebServiceCustomParameter','" + this.WebServiceCustomParameter + "'],");
        startupSB.Append("['WebServiceMethod','" + this.WebServiceMethod + "'],");
        startupSB.Append("['Width'," + Utils.ConvertUnitToJSConstant(this.Width) + "]\n];\n");
        startupSB.Append(tabStripClientVarName + ".LoadProperties();\n");

        if (this.EnableViewState)
        {
          // add us to the client viewstate-saving mechanism
          startupSB.Append("ComponentArt_ClientStateControls[ComponentArt_ClientStateControls.length] = " + tabStripClientVarName + ";\n");
        }

        // Initialize the tabstrip
        startupSB.Append(tabStripClientVarName + ".Initialize();\n");

        // Render the tabstrip
        startupSB.Append(tabStripClientVarName + ".Render();\n");

        // Keyboard
        if(this.KeyboardEnabled)
        {
          // Initialize keyboard
          startupSB.Append("ComponentArt_TabStrip_InitKeyboard(" + tabStripClientVarName + ");\n");

          // Create client script to register keyboard shortcuts
          StringBuilder oKeyboardSB = new StringBuilder();
          GenerateKeyShortcutScript(tabStripClientVarName, this.Tabs, oKeyboardSB);
          startupSB.Append(oKeyboardSB.ToString());
        }
        
        // Set the flag that the tabstrip has been initialized.  This is the last action in the menu initialization.
        startupSB.Append("window." + tabStripClientVarName + "_loaded = true;\n}\n");

        // Call this initialization function.  Remember that it will be repeated after a delay if it's not all ready.
        startupSB.Append("ComponentArt_Init_" + tabStripClientVarName + "();");

        WriteStartupScript(output, this.DemarcateClientScript(startupSB.ToString(), "ComponentArt_TabStrip_Startup_" + tabStripClientVarName + " " + this.VersionString()));

#endregion
      }
    }

    /// <summary>
    /// Handle a postback request on this control.
    /// </summary>
    /// <param name="stringArgument">Postback argument.</param>
    protected override void HandlePostback(string stringArgument)
    {
      if (stringArgument == "")
      {
        // we don't have any actions to execute
        return;
      }

      string [] arArguments = stringArgument.Split(' ');

      string sCommand;
      string sPostBackId;

      // if there is only one argument, it is the select id...
      if(arArguments.Length < 2)
      {
        sCommand = "SELECT";
        sPostBackId = stringArgument;
      }
      else
      {
        sCommand = arArguments[0];
        sPostBackId = arArguments[1];
      }

      TabStripTab oTab = (TabStripTab)(this.FindNodeByPostBackId(sPostBackId, this.Tabs));

      if (oTab == null)
      {
        throw new Exception("Tab " + sPostBackId + " not found.");
      }

      // should we validate the page?
      if (Utils.ConvertInheritBoolToBool(oTab.CausesValidation, this.CausesValidation))
      {
        Page.Validate();
      }

      TabStripTabEventArgs oArgs = new TabStripTabEventArgs();
      oArgs.Command = sCommand;
      oArgs.Tab = oTab;

      switch (sCommand)
      {
        case "SELECT":
          this.selectedNode = oArgs.Tab;
          this.OnItemSelected(oArgs);
          // If the selected node has a navurl, redirect to it.
          if(oArgs.Tab.NavigateUrl != string.Empty)
          {
            oArgs.Tab.Navigate();
          }
          break;

        default:
          throw new Exception("Unknown postback command: \"" + sCommand + "\"");
      }
    }

    protected override bool IsDownLevel()
    {
      if(Context == null || Page == null)
      {
        return true;
      }

      if (this.ClientTarget == ClientTargetLevel.Downlevel) return true;
      if (this.ClientTarget == ClientTargetLevel.Uplevel) return false;

      Utils._BrowserCapabilities bc = Utils.BrowserCapabilities(Context.Request);

      if( // We are good if:

        // 0. We have the W3C Validator
        (Context.Request.UserAgent != null && Context.Request.UserAgent.IndexOf("Validator") >= 0) ||
      
        // 1. We have Win IE 5 or greater
        (bc.IsBrowserWinIE && bc.Version >= 5) ||

        // 1b. We have IE 7+ on any platform
        (bc.IsBrowserIE && bc.Version >= 7) ||

        // 2. We have Mac IE 5.1 or greater
        (bc.IsBrowserMacIE && bc.Version >= 5.1) ||

        // 3. We have Opera 7 or greater
        (bc.IsBrowserOpera && bc.Version >= 7) ||

        // 4. We have a Mozilla-compatible other than Netscape 6.0*
        (bc.IsBrowserMozilla && !bc.IsBrowserNetscape60)

        )

      {
        return false;
      }
      return true; // All others are down-level
    }

    protected override void LoadPreloadImages()
    {
      base.LoadPreloadImages();

      // Load separator images for top group:
      if (this.TopGroupShowSeparators)
      {
        LoadSeparatorImages(this.TopGroupSeparatorImagesFolderUrl);
      }

      // Recurse into subgroups:
      foreach (TabStripTab rootTab in this.Tabs)
        LoadPreloadImagesRecursive(rootTab);
    }
    /// <summary>
    /// Recursively preload the separator images for the given tab's hierarchy.
    /// </summary>
    /// <param name="tab"></param>
    private void LoadPreloadImagesRecursive(TabStripTab tab)
    {
      if (tab.Tabs.Count > 0) // if this Tab has a subgroup
      {
        // Load separator images for the subgroup:
        if (tab.SubGroupShowSeparators)
        {
          LoadSeparatorImages(tab.SubGroupSeparatorImagesFolderUrl);
        }

        // Recurse into subgroups:
        foreach (TabStripTab childTab in tab.Tabs)
          LoadPreloadImagesRecursive(childTab);
      }
    }
    /// <summary>
    /// Preload the separator images from the given folder.
    /// </summary>
    /// <param name="separatorImagesFolder">The folder containing the separator images.</param>
    private void LoadSeparatorImages(string separatorImagesFolder)
    {
      string separatorImagesLocation = Utils.ResolveBaseUrl(this.Context,separatorImagesFolder);
      foreach (string fileName in this._separatorImageNames)
      {
        string separatorImageUrl = separatorImagesLocation + fileName + "." + this.SeparatorImagesExtension;
        if (!this.PreloadImages.Contains(separatorImageUrl))
          this.PreloadImages.Add(separatorImageUrl);
      }
    }
    
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      string tabStripClientVarName = this.ClientID.Replace("$", "_");
      if (Context != null && 
        Context.Request != null && 
        Context.Request.Form != null && 
        Context.Request.Form[tabStripClientVarName + "_SelectedNode"] != null)
      {
        this.selectedNodePostbackId = Context.Request.Form[tabStripClientVarName + "_SelectedNode"];
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      // If we don't have a SelectedTab and are connected to a valid MultiPage control,
      // let the MultiPage choose it for us.
      if (this.SelectedTab == null && this.MultiPageId != null && this.MultiPageId != "")
      {
        // First try to find the corresponding MultiPage nearby (maybe within a user control we're both in)
        Control control = Utils.FindControl(this, this.MultiPageId);

        if (control != null && control is MultiPage)
        {
          MultiPage multiPage = (MultiPage)control;
          if (multiPage.SelectedIndex >= 0)
          {
            this.SelectedTab = this.FindMultiPageSelectedTab(multiPage);
          }
        }
      }

      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573S188.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573Z388.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.TabStrip.client_scripts.A573C488.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.TabStrip.client_scripts.A573I688.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.TabStrip.client_scripts.A573B188.js");
      }
    }
    
    protected override void LoadViewState(object savedState)
    {
      base.LoadViewState(savedState);

      // Load top-level properties
      string sViewStateProperties = Context.Request.Form[this.ClientObjectId + "_Properties"];
      if (sViewStateProperties != null)
      {
        this.LoadClientProperties(sViewStateProperties);
      }

      // Load item storage data
      string sViewStateData = Context.Request.Form[this.ClientObjectId + "_Data"];
      if (sViewStateData != null)
      {
        this.LoadClientData(sViewStateData);
      }
    }

    protected override object SaveViewState()
    {
      if (this.EnableViewState)
      {
        ViewState["EnableViewState"] = true; // dummy just to ensure we have a ViewState.
      }
      return base.SaveViewState();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Go through the TabStrip nodes, and build a javascript array representing their data.
    /// </summary>
    /// <remarks>The method modifies this._childSelectedPostBackIDs.</remarks>
    private string BuildStorage()
    {
      // Reset the list of ChildSelected tabs
      this._childSelectedPostBackIDs = new JavaScriptArray(JavaScriptArrayType.Dense);

      // Recursively populate a list of data of all tabstrip tabs that will make up the tabstrip:
      JavaScriptArray renderTabList = new JavaScriptArray();
      foreach (TabStripTab tab in this.GetStartGroupTabs())
      {
        ProcessTab(tab, ref renderTabList, -1, 1);
      }
      
      // Return a JavaScript representation of this list:
      return renderTabList.ToString();
    }

    /// <summary>
    /// Go through the TabStrip ItemLooks, and build a javascript array representing their data.
    /// </summary>
    private string BuildLooks()
    {
      JavaScriptArray renderLookList = new JavaScriptArray();
      foreach (ItemLook look in this.ItemLooks)
      {
        renderLookList.Add(ProcessLook(look));
      }
      return  renderLookList.ToString();
    }

    /// <summary>
    /// Returns a string representation of a four-element javascript array with ScrollDownLook, 
    /// ScrollLeftLook, ScrollRightLook and ScrollUpLook
    /// </summary>
    private string BuildScrollLooks()
    {
      JavaScriptArray scrollLookList = new JavaScriptArray();
      scrollLookList.Add(this.ProcessScrollLook(this.ScrollDownLook, this.ScrollDownLookId));
      scrollLookList.Add(this.ProcessScrollLook(this.ScrollLeftLook, this.ScrollLeftLookId));
      scrollLookList.Add(this.ProcessScrollLook(this.ScrollRightLook, this.ScrollRightLookId));
      scrollLookList.Add(this.ProcessScrollLook(this.ScrollUpLook, this.ScrollUpLookId));
      return scrollLookList.ToString();
    }

    /// <summary>
    /// Check whether we should use the default styles, and if so, assign them
    /// </summary>
    /// <returns>true if we should use the default styles</returns>
    /// <remarks>modifies this.renderDefaultStyles</remarks>
    internal override bool ConsiderDefaultStyles()
    {
      if (this.AutoTheming)
      {
        return false;
      }

      bool needsDefaultStyles = this.NeedsDefaultStyles();
      if (needsDefaultStyles || this.RenderDefaultStyles)
      {
        ApplyDefaultStyles();
      }
      return needsDefaultStyles;
    }

    /// <summary>
    /// Whether we should render the control with the default design
    /// </summary>
    internal bool NeedsDefaultStyles()
    {
      bool controlHasNoLooksDefined = this.ItemLooks.Count == 0;
      bool controlHasNoGroupClassesDefined = (this.DefaultGroupCssClass == null || this.DefaultGroupCssClass == "")
        && (this.TopGroupCssClass == null || this.TopGroupCssClass == "");
      return controlHasNoLooksDefined && controlHasNoGroupClassesDefined;
    }

    /// <summary>
    /// Apply default styles to the control
    /// </summary>
    internal void ApplyDefaultStyles()
    {
      // First off, define the looks
  
      ItemLook topTabLook;      // Vertical   top-level                    tab look
      ItemLook topSelTabLook;   // Vertical   top-level    selected        tab look
      ItemLook topDTabLook;     // Vertical   top-level    disabled        tab look
      ItemLook otherTabLook;    // Vertical   other-level                  tab look
      ItemLook otherSelTabLook; // Vertical   other-level  selected        tab look
      ItemLook otherCSTabLook;  // Vertical   other-level  child selected  tab look
      ItemLook otherDTabLook;   // Vertical   other-level  disabled        tab look

      topTabLook = new ItemLook();
      topTabLook.LabelPaddingRight = topTabLook.LabelPaddingLeft = Unit.Parse("15px");
      topTabLook.LabelPaddingBottom = topTabLook.LabelPaddingTop = Unit.Parse("4px");
      // Since the paddings are the same for all top-level looks, let's just clone
      topSelTabLook = (ItemLook)topTabLook.Clone();
      topDTabLook = (ItemLook)topTabLook.Clone();

      topTabLook.LookId = "_t";
      topTabLook.CssClass = "cts_TopTab";
      topTabLook.HoverCssClass = "cts_TopTabHover";
      this.ItemLooks.Add(topTabLook);

      topSelTabLook.LookId = "_ts";
      topSelTabLook.CssClass = "cts_TopTabSelected";
      this.ItemLooks.Add(topSelTabLook);

      topDTabLook.LookId = "_td";
      topDTabLook.CssClass = "cts_TopTabDisabled";
      this.ItemLooks.Add(topDTabLook);

      otherTabLook = new ItemLook();
      otherTabLook.LabelPaddingBottom = otherTabLook.LabelPaddingTop = Unit.Parse("2px");
      otherTabLook.LabelPaddingLeft = Unit.Parse("5px");
      otherTabLook.LabelPaddingRight = Unit.Parse("10px");
      // Since the paddings are the same for all other-level looks, let's just clone
      otherSelTabLook = (ItemLook)otherTabLook.Clone();
      otherCSTabLook = (ItemLook)otherTabLook.Clone();
      otherDTabLook = (ItemLook)otherTabLook.Clone();

      otherTabLook.LookId = "_o";
      otherTabLook.CssClass = "cts_Tab";
      otherTabLook.HoverCssClass = "cts_TabHover";
      this.ItemLooks.Add(otherTabLook);

      otherSelTabLook.LookId = "_os";
      otherSelTabLook.CssClass = "cts_TabSelected";
      this.ItemLooks.Add(otherSelTabLook);

      otherCSTabLook.LookId = "_oc";
      otherCSTabLook.CssClass = "cts_TabChildSelected";
      this.ItemLooks.Add(otherCSTabLook);

      otherDTabLook.LookId = "_od";
      otherDTabLook.CssClass = "cts_TabDisabled";
      this.ItemLooks.Add(otherDTabLook);

      // Then assign the looks

      this.TopGroupCssClass = "cts_TopGroup";
      this.DefaultGroupCssClass = "cts_Group";

      this.DefaultItemLookId = "_o";
      this.DefaultSelectedItemLookId = "_os";
      this.DefaultChildSelectedItemLookId = "_oc";
      this.DefaultDisabledItemLookId = "_od";

      foreach (TabStripTab topLevelTab in this.Tabs)
      {
        topLevelTab.LookId = "_t";
        topLevelTab.SelectedLookId = topLevelTab.ChildSelectedLookId = "_ts";
        topLevelTab.DisabledLookId = "_td";
      }

      this.ApplyLooks();
    }

    private TabStripTab FindMultiPageSelectedTab(MultiPage multiPage)
    {
      if (multiPage.PageViews == null || multiPage.PageViews.Count == 0 || multiPage.SelectedIndex < 0)
        return null;
      string selectedPageViewId = multiPage.PageViews[multiPage.SelectedIndex].ID;
      if (selectedPageViewId != null && selectedPageViewId != "")
      {
        TabStripTab selectedTab = this.FindTabByPageViewId(selectedPageViewId);
        if (SelectedTab != null)
          return SelectedTab;
      }
      return this.FindTabByIndex(multiPage.SelectedIndex);
    }

    private TabStripTab FindTabByPageViewId(string pageViewId)
    {
      return this.FindTabByPageViewId(pageViewId, this.Tabs);
    }
    private TabStripTab FindTabByPageViewId(string pageViewId, TabStripTabCollection tabs)
    {
      foreach (TabStripTab tab in tabs)
      {
        if (tab.PageViewId == pageViewId)
        {
          return tab;
        }
				
        if (tab.Tabs != null)
        {
          TabStripTab tabFromBelow = FindTabByPageViewId(pageViewId, tab.Tabs);
          if (tabFromBelow != null)
          {
            return tabFromBelow;
          }
        }
      }
      return null;
    }

    private int _searchIndex;
    private TabStripTab FindTabByIndex(int index)
    {
      _searchIndex = 0;
      return this.FindTabByIndex(index, ref _searchIndex, this.Tabs);
    }
    private TabStripTab FindTabByIndex(int index, ref int currentIndex, TabStripTabCollection tabs)
    {
      if (tabs == null) return null;

      foreach (TabStripTab tab in tabs)
      {
        if (currentIndex == index)
          return tab;

        currentIndex++;

        TabStripTab tabFromBelow = FindTabByIndex(index, ref currentIndex, tab.Tabs);

        if (tabFromBelow != null) return tabFromBelow;
      }

      return null;
    }

    private void GenerateKeyShortcutScript(string sTabStripName, TabStripTabCollection arTabList, StringBuilder oSB)
    {
      if(arTabList != null)
      {
        foreach(TabStripTab oTab in arTabList)
        {
          if(oTab.KeyboardShortcut != string.Empty)
          {
            oSB.Append("ComponentArt_RegisterKeyHandler(" + sTabStripName + ",'" + oTab.KeyboardShortcut + "','" + sTabStripName + ".SelectTabByPostBackId(\\'" + oTab.PostBackID + "\\')'" + ");\n");
          }
          GenerateKeyShortcutScript(sTabStripName, oTab.Tabs, oSB);
        }
      }
    }

    private TabStripTabCollection GetStartGroupTabs()
    {
      TabStripTabCollection startGroupTabs;
      if(this.RenderRootItemId != null && this.RenderRootItemId != string.Empty)
      {
        TabStripTab rootTab = this.FindItemById(this.RenderRootItemId);
        if (rootTab != null)
        {
          startGroupTabs = rootTab.Tabs;
        }
        else
        {
          throw new Exception("No tab found with ID \"" + this.RenderRootNodeId + "\".");
        }
      }
      else
      {
        startGroupTabs = this.Tabs;
      }
      return startGroupTabs;
    }

    /// <summary>
    /// Generates the array of tab properties that are to be propagated to the client side.
    /// </summary>
    /// <param name="tab">The tab that is being processed.</param>
    /// <param name="tabs">Array of tab property arrays. Tab property array of the current tab gets appended to it.</param>
    /// <param name="parentIndex">Client-side storage index of the parent tab of the currently processed tab.</param>
    /// <param name="depth">Depth at which we are currently processing. Needed to know when to stop if RenderDrillDownDepth is
    /// significant.</param>
    /// <returns>Client-side storage index of the tab that has just been processed.</returns>
    /// <remarks>This is a recursive method which will also process all of the tab's child tabs.</remarks>
    /// <remarks>The method modifies this._childSelectedPostBackIDs.</remarks>
    private int ProcessTab(TabStripTab tab, ref JavaScriptArray tabs, int parentIndex, int depth)
    {
      // Update the ChildSelected list if appropriate
      if (tab.IsChildSelected)
        this._childSelectedPostBackIDs.Add(tab.PostBackID);

      ArrayList tabData = new ArrayList();
      int tabIndex = tabs.Count;
      tabs.Add(tabData);

      tabData.Add(tab.PostBackID); // 'PostBackID'
      tabData.Add(parentIndex); // 'ParentIndex'
      
      ArrayList childIndexes = new ArrayList();
      if (tab.nodes != null && this.RenderDrillDownDepth == 0 || this.RenderDrillDownDepth > depth)
      {
        foreach (TabStripTab childTab in tab.Tabs)
        {
          childIndexes.Add(ProcessTab(childTab, ref tabs, tabIndex, depth + 1)); // Note the recursion
        }
      }
      tabData.Add(childIndexes); // 'ChildIndexes'

      ArrayList tabProperties = new ArrayList();
      foreach (string propertyName in tab.Properties.Keys)
      {
        switch (tab.GetVarAttributeName(propertyName).ToLower(System.Globalization.CultureInfo.InvariantCulture))
        {
          case "autopostbackonselect": tabProperties.Add(new object [] {0, tab.AutoPostBackOnSelect}); break;
          case "causesvalidation": tabProperties.Add(new object [] {1, tab.CausesValidation}); break;
          case "childselectedlookid": tabProperties.Add(new object [] {2, tab.ChildSelectedLookId}); break;
          case "clientsidecommand": tabProperties.Add(new object [] {3, tab.ClientSideCommand}); break;
          case "clienttemplateid": tabProperties.Add(new object [] {4, tab.ClientTemplateId}); break;
          case "defaultsubgroupalign": tabProperties.Add(new object [] {5, tab.DefaultSubGroupAlign}); break;
          case "defaultsubgroupcssclass": tabProperties.Add(new object [] {6, tab.DefaultSubGroupCssClass}); break;
          case "defaultsubgroupexpanddirection": tabProperties.Add(new object [] {7, tab.DefaultSubGroupExpandDirection}); break;
          case "defaultsubgroupexpandoffsetx": tabProperties.Add(new object [] {8, tab.DefaultSubGroupExpandOffsetX}); break;
          case "defaultsubgroupexpandoffsety": tabProperties.Add(new object [] {9, tab.DefaultSubGroupExpandOffsetY}); break;
          case "defaultsubgroupfirstseparatorheight": tabProperties.Add(new object [] {10, tab.DefaultSubGroupFirstSeparatorHeight}); break;
          case "defaultsubgroupfirstseparatorwidth": tabProperties.Add(new object [] {11, tab.DefaultSubGroupFirstSeparatorWidth}); break;
          case "defaultsubgroupfullexpand": tabProperties.Add(new object [] {12, tab.DefaultSubGroupFullExpand}); break;
          case "defaultsubgroupheight": tabProperties.Add(new object [] {13, tab.DefaultSubGroupHeight}); break;
          case "defaultsubgrouplastseparatorheight": tabProperties.Add(new object [] {14, tab.DefaultSubGroupLastSeparatorHeight}); break;
          case "defaultsubgrouplastseparatorwidth": tabProperties.Add(new object [] {15, tab.DefaultSubGroupLastSeparatorWidth}); break;
          case "defaultsubgroupseparatorheight": tabProperties.Add(new object [] {16, tab.DefaultSubGroupSeparatorHeight}); break;
          case "defaultsubgroupseparatorimagesfolderurl": tabProperties.Add(new object [] {17, tab.DefaultSubGroupSeparatorImagesFolderUrl}); break;
          case "defaultsubgroupseparatorwidth": tabProperties.Add(new object [] {18, tab.DefaultSubGroupSeparatorWidth}); break;
          case "defaultsubgroupshowseparators": tabProperties.Add(new object [] {19, tab.DefaultSubGroupShowSeparators}); break;
          case "defaultsubgrouptabspacing": tabProperties.Add(new object [] {20, tab.DefaultSubGroupTabSpacing}); break;
          case "defaultsubgroupwidth": tabProperties.Add(new object [] {21, tab.DefaultSubGroupWidth}); break;
          case "defaultsubitemchildselectedlookid": tabProperties.Add(new object [] {22, tab.DefaultSubItemChildSelectedLookId}); break;
          case "defaultsubitemdisabledlookid": tabProperties.Add(new object [] {23, tab.DefaultSubItemDisabledLookId}); break;
          case "defaultsubitemlookid": tabProperties.Add(new object [] {24, tab.DefaultSubItemLookId}); break;
          case "defaultsubitemselectedlookid": tabProperties.Add(new object [] {25, tab.DefaultSubItemSelectedLookId}); break;
          case "defaultsubitemtextalign": tabProperties.Add(new object [] {26, tab.DefaultSubItemTextAlign}); break;
          case "defaultsubitemtextwrap": tabProperties.Add(new object [] {27, tab.DefaultSubItemTextWrap}); break;
          case "disabledlookid": tabProperties.Add(new object [] {28, tab.DisabledLookId}); break;
          case "enabled": tabProperties.Add(new object [] {29, tab.Enabled}); break;
          case "height": tabProperties.Add(new object [] {30, tab.Height}); break;
          case "id": tabProperties.Add(new object [] {31, tab.ID}); break;
          case "keyboardshortcut": tabProperties.Add(new object [] {32, tab.KeyboardShortcut}); break;
          case "lookid": tabProperties.Add(new object [] {33, tab.LookId}); break;
          case "navigateurl": tabProperties.Add(new object [] {34, Utils.MakeStringXmlSafe(tab.NavigateUrl)}); break;
          case "pageviewid": tabProperties.Add(new object [] {35, tab.PageViewId}); break;
          case "selectedlookid": tabProperties.Add(new object [] {36, tab.SelectedLookId}); break;
          case "servertemplateid": tabProperties.Add(new object [] {37, tab.ServerTemplateId}); break;
          case "sitemapxmlfile": tabProperties.Add(new object [] {38, tab.SiteMapXmlFile}); break;
          case "subgroupalign": tabProperties.Add(new object [] {39, tab.SubGroupAlign}); break;
          case "subgroupcssclass": tabProperties.Add(new object [] {40, tab.SubGroupCssClass}); break;
          case "subgroupexpanddirection": tabProperties.Add(new object [] {41, tab.SubGroupExpandDirection}); break;
          case "subgroupexpandoffsetx": tabProperties.Add(new object [] {42, tab.SubGroupExpandOffsetX}); break;
          case "subgroupexpandoffsety": tabProperties.Add(new object [] {43, tab.SubGroupExpandOffsetY}); break;
          case "subgroupfirstseparatorheight": tabProperties.Add(new object [] {44, tab.SubGroupFirstSeparatorHeight}); break;
          case "subgroupfirstseparatorwidth": tabProperties.Add(new object [] {45, tab.SubGroupFirstSeparatorWidth}); break;
          case "subgroupfullexpand": tabProperties.Add(new object [] {46, tab.SubGroupFullExpand}); break;
          case "subgroupheight": tabProperties.Add(new object [] {47, tab.SubGroupHeight}); break;
          case "subgrouplastseparatorheight": tabProperties.Add(new object [] {48, tab.SubGroupLastSeparatorHeight}); break;
          case "subgrouplastseparatorwidth": tabProperties.Add(new object [] {49, tab.SubGroupLastSeparatorWidth}); break;
          case "subgroupseparatorheight": tabProperties.Add(new object [] {50, tab.SubGroupSeparatorHeight}); break;
          case "subgroupseparatorimagesfolderurl": tabProperties.Add(new object [] {51, tab.SubGroupSeparatorImagesFolderUrl}); break;
          case "subgroupseparatorwidth": tabProperties.Add(new object [] {52, tab.SubGroupSeparatorWidth}); break;
          case "subgroupshowseparators": tabProperties.Add(new object [] {53, tab.SubGroupShowSeparators}); break;
          case "subgrouptabspacing": tabProperties.Add(new object [] {54, tab.SubGroupTabSpacing}); break;
          case "subgroupwidth": tabProperties.Add(new object [] {55, tab.SubGroupWidth}); break;
          case "target": tabProperties.Add(new object [] {56, tab.Target}); break;
          case "text": tabProperties.Add(new object [] {57, Utils.MakeStringXmlSafe(tab.Text)}); break;
          case "textalign": tabProperties.Add(new object [] {58, tab.TextAlign}); break;
          case "textwrap": tabProperties.Add(new object [] {59, tab.TextWrap}); break;
          case "tooltip": tabProperties.Add(new object [] {60, Utils.MakeStringXmlSafe(tab.ToolTip)}); break;
          case "value": tabProperties.Add(new object [] {61, Utils.MakeStringXmlSafe(tab.Value)}); break;
          case "visible": tabProperties.Add(new object [] {62, tab.Visible}); break;
          case "width": tabProperties.Add(new object [] {63, tab.Width}); break;
          default:
            if (this.OutputCustomAttributes)
            {
              tabProperties.Add(new object [] {tab.GetVarAttributeName(propertyName), Utils.MakeStringXmlSafe(tab.Properties[propertyName])});
            }
          break;
        }
      }
      tabData.Add(tabProperties);
      
      return tabIndex;
    }

    /// <summary>
    /// Generates the array of look properties that are to be propagated to the client side.
    /// </summary>
    /// <param name="look">The look that is being propagated.</param>
    /// <remarks>This is currently used only to propagate the effective look of down, left, right and up scrolls.
    /// Since the scrolls ignore many look properties (like all those related to icons),
    /// we omit many properties in this method.
    /// In the future we can modify this method to propagate the looks for all the tabs, too.
    /// Note: this is not the method used to propagate the looks in the looks collection.</remarks>
    private JavaScriptArray ProcessScrollLook(ItemLook look, string lookId)
    {
      ItemLook originalLook = this.ItemLooks[lookId]; // The original look from which this item's look is derived (if any).
      ItemLook significantLook = base.LookDifference(look, originalLook); // The look containing only the differences between the two.

      JavaScriptArray lookProperties = new JavaScriptArray(JavaScriptArrayType.Sparse);

      lookProperties.Add(significantLook.LookId); // 'LookId'
      lookProperties.Add(significantLook.CssClass); // 'CssClass'
      lookProperties.Add(significantLook.HoverCssClass); // 'HoverCssClass'
      lookProperties.Add(significantLook.ImageHeight); // 'ImageHeight'
      lookProperties.Add(significantLook.ImageWidth); // 'ImageWidth'
      lookProperties.Add(significantLook.LabelPaddingBottom); // 'LabelPaddingBottom'
      lookProperties.Add(significantLook.LabelPaddingLeft); // 'LabelPaddingLeft'
      lookProperties.Add(significantLook.LabelPaddingRight); // 'LabelPaddingRight'
      lookProperties.Add(significantLook.LabelPaddingTop); // 'LabelPaddingTop'
      lookProperties.Add(significantLook.ActiveCssClass); // 'ActiveCssClass'
      lookProperties.Add(null); // 'ExpandedCssClass'
      lookProperties.Add(null); // 'LeftIconUrl'
      lookProperties.Add(null); // 'HoverLeftIconUrl'
      lookProperties.Add(null); // 'LeftIconWidth'
      lookProperties.Add(null); // 'LeftIconHeight'
      lookProperties.Add(null); // 'ActiveLeftIconUrl'
      lookProperties.Add(null); // 'ExpandedLeftIconUrl'
      lookProperties.Add(null); // 'RightIconUrl'
      lookProperties.Add(null); // 'HoverRightIconUrl'
      lookProperties.Add(null); // 'RightIconWidth'
      lookProperties.Add(null); // 'RightIconHeight'
      lookProperties.Add(null); // 'ActiveRightIconUrl'
      lookProperties.Add(null); // 'ExpandedRightIconUrl'
      lookProperties.Add(significantLook.ImageUrl); // 'ImageUrl'
      lookProperties.Add(significantLook.HoverImageUrl); // 'HoverImageUrl'
      lookProperties.Add(significantLook.ActiveImageUrl); // 'ActiveImageUrl'
      // 'ExpandedImageUrl'
      
      return lookProperties;
    }
    
    #endregion

    #region Accessible Rendering

    internal void RenderAccessibleContent(HtmlTextWriter output)
    {
      output.Write("<span class=\"tabstrip\">");
      int tabIndex = -1;
      RenderAccessibleGroup(output, null, ref tabIndex);
      output.Write("</span>");
    }

    private void RenderAccessibleGroup(HtmlTextWriter output, TabStripTab parentTab, ref int tabIndex)
    {
      bool groupIsVertical = this.Orientation == TabStripOrientation.VerticalLeftToRight || this.Orientation == TabStripOrientation.VerticalRightToLeft;
      output.Write("<ul");

      string groupId = "G" + this.GetSaneId() + "_" + tabIndex.ToString();
      output.Write(" id=\"");
      output.Write(groupId);
      output.Write("\"");

      // CSS class
      output.Write(" class=\"");
      output.Write(groupIsVertical ? "vertical" : "horizontal");
      string groupCssClass = this.CssClass == string.Empty ? this.DefaultGroupCssClass : this.CssClass;
      if (groupCssClass != null) output.Write(" " + groupCssClass);
      output.Write("\"");

      // style tag
      Unit groupWidth = this.Width == Unit.Empty ? this.DefaultGroupWidth : this.Width;
      Unit groupHeight = this.Height == Unit.Empty ? this.DefaultGroupHeight : this.Height;
      if (groupWidth != Unit.Empty && groupHeight != Unit.Empty)
      {
        output.Write(" style=\"");
        output.Write("width:" + groupWidth.ToString() + ";");
        output.Write("height:" + groupHeight.ToString() + ";");
        output.Write("\"");
      }

      output.Write(">");

      int maxLeftIconWidth = 0;
      int maxRightIconWidth = 0;
      if (groupIsVertical)
      {
        // If the group is vertical, run through all the items to find the largest left and right icon widths
        foreach (TabStripTab tab in (parentTab == null ? this.Tabs : parentTab.Tabs))
        {
          maxLeftIconWidth = Math.Max(maxLeftIconWidth, (int)tab.EffectiveLook.LeftIconWidth.Value);
          maxRightIconWidth = Math.Max(maxRightIconWidth, (int)tab.EffectiveLook.RightIconWidth.Value);
        }
      }

      foreach (TabStripTab tab in (parentTab == null ? this.Tabs : parentTab.Tabs))
      {
        output.Write("<li");

        tabIndex++;

        string tabId = this.GetSaneId() + "_" + tabIndex.ToString();
        output.Write(" id=\"");
        output.Write(tabId);
        output.Write("\"");

        if (tab.EffectiveLook.CssClass != null)
        {
          output.Write(" class=\"");
          output.Write(tab.EffectiveLook.CssClass);
          output.Write("\"");
        }

        // right icon
        string rightIconStyle = (tab.EffectiveLook.RightIconUrl != null) ?
          ("background-image:url(" + ConvertImageUrl(tab.EffectiveLook.RightIconUrl) + ");background-repeat:no-repeat;background-position:right center;") : null;
        string itemWidthStyle = (!groupIsVertical && tab.Width != Unit.Empty) ? ("width:" + tab.Width.ToString() + ";") : null;
        if (rightIconStyle != null || itemWidthStyle != null)
        {
          output.Write(" style=\"");
          output.Write(rightIconStyle);
          output.Write(itemWidthStyle);
          output.Write("\"");
        }
        output.Write(">");

        output.Write("<a");
        output.Write(" href=\"");
        output.Write(tab.NavigateUrl);
        output.Write("\"");

        // left icon
        string leftIconStyle = (tab.EffectiveLook.LeftIconUrl != null) ?
          ("background-image:url(" + ConvertImageUrl(tab.EffectiveLook.LeftIconUrl) + ");background-repeat:no-repeat;background-position:left center;") : null;
        if (leftIconStyle != null)
        {
          output.Write(" style=\"");
          output.Write(leftIconStyle);
          output.Write("\"");
        }
        output.Write(">");

        output.Write("<span");
        int leftIconPadding = groupIsVertical ? maxLeftIconWidth : (int)tab.EffectiveLook.LeftIconWidth.Value;
        int leftTextPadding = (int)tab.EffectiveLook.LabelPaddingLeft.Value;
        string leftPaddingStyle = "padding-left:" + (leftIconPadding + leftTextPadding) + "px;";
        int rightIconPadding = groupIsVertical ? maxRightIconWidth : (int)tab.EffectiveLook.RightIconWidth.Value;
        int rightTextPadding = (int)tab.EffectiveLook.LabelPaddingRight.Value;
        string rightPaddingStyle = "padding-right:" + (rightIconPadding + rightTextPadding) + "px;";
        output.Write(" style=\"");
        output.Write(leftPaddingStyle);
        output.Write(rightPaddingStyle);
        output.Write("\">");

        output.Write(tab.Text);

        output.Write("</span>");
        output.Write("</a>");

        if (tab.Tabs.Count > 0)
        {
          RenderAccessibleGroup(output, tab, ref tabIndex);
        }

        output.Write("</li>");
      }

      output.Write("</ul>");
    }

    #endregion Accessible Rendering


    #region Down-level Rendering

    internal void RenderDownLevelContent(HtmlTextWriter output)
    {
      StringDictionary defaultStyles = new StringDictionary();
      defaultStyles["cts_TopGroup"] = "background-color:#3F3F3F;border:1px;border-color:white;border-top-color:#CCCCCC;border-left-color:#3F3F3F;border-style:solid;border-right:none;cursor:default;";
      defaultStyles["cts_TopTab"] = "color:white;font-family:verdana;font-size:11px;border:1px;border-color:#3F3F3F;border-style:solid;cursor:pointer;";
      defaultStyles["cts_TopTabHover"] = "color:white;font-family:verdana;font-size:11px;border:1px;border-color:#808080;border-right-color:black;border-bottom-color:black;border-style:solid;cursor:pointer;";
      defaultStyles["cts_TopTabSelected"] = "color:#3F3F3F;background-color:#CCCCCC;font-family:verdana;font-size:11px;border:1px;border-style:solid;border-color:white;border-left-color:gray;border-top-color:gray;border-bottom-color:#CCCCCC;position:relative;top:1px;cursor:default;";
      defaultStyles["cts_TopTabDisabled"] = "color:#9F9F9F;font-family:verdana;font-size:11px;border:1px;border-color:#3F3F3F;border-style:solid;cursor:pointer;";
      defaultStyles["cts_Group"] = "background-color:#CCCCCC;border:1px;border-style:solid;border-color:#CCCCCC;border-bottom-color:#999999;border-right-color:white;border-right:none;cursor:default;";
      defaultStyles["cts_Tab"] = "color:#3F3F3F;font-family:verdana;font-size:10px;border:1px;border-style:solid;border-color:#CCCCCC;cursor:pointer;";
      defaultStyles["cts_TabHover"] = "color:#3F3F3F;font-family:verdana;font-size:10px;border:1px;border-style:solid;border-color:white;border-right-color:#999999;border-bottom-color:#999999;cursor:pointer;";
      defaultStyles["cts_TabSelected"] = "color:#3F3F3F;background-color:#F6F6F6;font-family:verdana;font-size:10px;border:1px;border-style:solid;border-color:white;border-left-color:#999999;border-top-color:#999999;border-bottom-color:#F6F6F6;position:relative;top:2px;cursor:default;";
      defaultStyles["cts_TabChildSelected"] = "color:#3F3F3F;background-color:#CCCCCC;font-family:verdana;font-size:10px;border:1px;border-style:solid;border-color:white;border-bottom-color:#CCCCCC;border-left-color:#999999;border-top-color:#999999;position:relative;top:2px;cursor:default;";
      defaultStyles["cts_TabDisabled"] = "color:#888888;font-family:verdana;font-size:10px;border:1px;border-style:solid;border-color:#CCCCCC;cursor:pointer;";

      // Output div
      output.Write("<div");

      if (!this.RenderDefaultStyles && this.TopGroupCssClass!=null && this.TopGroupCssClass!="")
      {
        output.WriteAttribute("class", this.TopGroupCssClass);
      }

      if(this.ToolTip != null && this.ToolTip != "")
      {
        output.WriteAttribute("title", this.ToolTip);
      }

      // Output style
      output.Write(" style=\"");
      foreach(string sKey in this.Style.Keys)
      {
        output.WriteStyleAttribute(sKey, this.Style[sKey]);
      }
      if(!this.BackColor.IsEmpty)
      {
        output.WriteStyleAttribute("background-color", ColorTranslator.ToHtml(this.BackColor));
      }
      if(!this.BorderWidth.IsEmpty)
      {
        output.WriteStyleAttribute("border-width", this.BorderWidth.ToString());
      }
      if(this.BorderStyle != BorderStyle.NotSet)
      {
        output.WriteStyleAttribute("border-style", this.BorderStyle.ToString());
      }
      if(!this.BorderColor.IsEmpty)
      {
        output.WriteStyleAttribute("border-color", ColorTranslator.ToHtml(this.BorderColor));
      }
      if (!this.Height.IsEmpty)
      {
        output.WriteStyleAttribute("height", this.Height.ToString());
      }
      if (!this.Width.IsEmpty)
      {
        output.WriteStyleAttribute("width", this.Width.ToString());
      }
      if (this.RenderDefaultStyles)
      {
        output.Write(defaultStyles["cts_TopGroup"]);
      }
      output.Write("\">");

      output.AddAttribute("cellpadding", "0");
      output.AddAttribute("border", "0");

      Unit groupTabSpacing = this.TopGroupTabSpacing == Unit.Empty ? Unit.Parse("0") : this.TopGroupTabSpacing;
      output.AddAttribute("cellspacing", Utils.ConvertUnitToJSConstant(groupTabSpacing));

      output.RenderBeginTag("table");

      output.RenderBeginTag("tr");

      TabStripTabCollection tabs = this.Tabs;
      bool groupIsVertical = this.Orientation == TabStripOrientation.VerticalLeftToRight || this.Orientation == TabStripOrientation.VerticalRightToLeft;
      int maxLeftIconWidth = 0;
      int maxRightIconWidth = 0;
      if (groupIsVertical)
      {
        // If the group is vertical, run through all the tabs to find the largest left and right icon widths
        foreach (TabStripTab tab in tabs)
        {
          maxLeftIconWidth = Math.Max(maxLeftIconWidth, (int)tab.EffectiveLook.LeftIconWidth.Value);
          maxRightIconWidth = Math.Max(maxRightIconWidth, (int)tab.EffectiveLook.RightIconWidth.Value);
        }
      }

      #region render the HTML for each tab

      
      bool firstTabInGroup = true; /* Whether we are rendering the first tab in this group.
                                     * Needed in order to know when to insert row breaks in vertical groups. */
      string effectiveCssClass; // Used as a temporary variable later on
      foreach (TabStripTab tab in tabs)
      {
        if (!tab.Visible) continue; // Skip invisible tabs

        if (groupIsVertical) // We need to insert row breaks in vertical groups
        {
          if (firstTabInGroup)
          {
            firstTabInGroup = false; /* No need to insert a row break the first time.  Just make a note of this 
                                      * first tab so that we know from now it won't be the first tab any more. */
          }
          else
          {
            // Insert a row break
            output.RenderEndTag(); // </tr>
            output.RenderBeginTag("tr");
          }
        }

        if (tab.EffectiveLook.ImageUrl != null)
        {
          #region render an image tab

          /*
          output.AddAttribute("onmousemove","return false");
          output.AddAttribute("ondblclick","return false");
          output.AddAttribute("onmouseover","TabMouseOver(this,event)");
          output.AddAttribute("onmouseout","TabMouseOut(this,event)");
          output.AddAttribute("onmousedown","TabMouseDown(this)");
          output.AddAttribute("onmouseup","TabMouseUp(this)");
          if (tab.Enabled) output.AddAttribute("onclick","TabClick(this)");
          */
          if (tab.Width != Unit.Empty) output.AddAttribute("width",Utils.ConvertUnitToJSConstant(tab.Width));
          if (tab.Height != Unit.Empty) output.AddAttribute("height",Utils.ConvertUnitToJSConstant(tab.Height));
          if (tab.ToolTip != null) output.AddAttribute("title",tab.ToolTip);
          
          output.RenderBeginTag("td");

          output.AddAttribute("border","0");
          output.AddAttribute("alt", (tab.ToolTip == null) ? "" : tab.ToolTip);
          effectiveCssClass = tab.EffectiveLook.CssClass;
          if (effectiveCssClass != null && effectiveCssClass != "")
          {
            if(this.RenderDefaultStyles)
            {
              output.AddAttribute("style", defaultStyles[effectiveCssClass]);
            }
            else
            {
              output.AddAttribute("class", effectiveCssClass);
            }
          }
          if (tab.Width != Unit.Empty) output.AddAttribute("width",Utils.ConvertUnitToJSConstant(tab.Width));
          if (tab.Height != Unit.Empty) output.AddAttribute("height",Utils.ConvertUnitToJSConstant(tab.Height));
          output.AddAttribute("src", (tab.EffectiveLook.ImageUrl == null) ? "" : ConvertImageUrl(tab.EffectiveLook.ImageUrl));
          output.RenderBeginTag("img");
          output.RenderEndTag(); // <img />

          output.RenderEndTag(); // </td>

          #endregion
        }

        else if ( maxLeftIconWidth > 0
          || tab.EffectiveLook.RightIconUrl != null 
          || maxRightIconWidth > 0 
          || tab.EffectiveLook.LeftIconUrl != null )
        {
          #region render an icon tab
          
          output.RenderBeginTag("td");

          /*
          output.AddAttribute("onmousemove","return false");
          output.AddAttribute("ondblclick","return false");
          output.AddAttribute("onmouseover","TabMouseOver(this,event)");
          output.AddAttribute("onmouseout","TabMouseOut(this,event)");
          output.AddAttribute("onmousedown","TabMouseDown(this)");
          output.AddAttribute("onmouseup","TabMouseUp(this)");
          if (tab.Enabled) output.AddAttribute("onclick","TabClick(this)");
          */
          
          output.AddStyleAttribute("padding-left","0");
          output.AddStyleAttribute("padding-right","0");
          output.AddAttribute("cellpadding","0");
          output.AddAttribute("cellspacing","0");
          output.AddAttribute("border","0");
          output.AddAttribute("width", (tab.Width == Unit.Empty) ? "100%" : Utils.ConvertUnitToJSConstant(tab.Width));
          if (tab.Height != Unit.Empty) output.AddAttribute("height", Utils.ConvertUnitToJSConstant(tab.Height));
          if (tab.ToolTip != null) output.AddAttribute("title",tab.ToolTip);
          effectiveCssClass = tab.EffectiveLook.CssClass;
          if (effectiveCssClass != null && effectiveCssClass != "")
          {
            if(this.RenderDefaultStyles)
            {
              output.AddAttribute("style", defaultStyles[effectiveCssClass]);
            }
            else
            {
              output.AddAttribute("class", effectiveCssClass);
            }
          }
          output.RenderBeginTag("table");

          output.RenderBeginTag("tr");

          #region left icon

          int tabEffectiveLeftIconWidth = (tab.EffectiveLook.LeftIconWidth != Unit.Empty) ? 
            (int)tab.EffectiveLook.LeftIconWidth.Value :
            maxLeftIconWidth;
          if (tabEffectiveLeftIconWidth > 0 || tab.EffectiveLook.LeftIconUrl != null)
          {
            output.AddStyleAttribute("padding","0");
            if (tabEffectiveLeftIconWidth > 0) output.AddAttribute("width",tabEffectiveLeftIconWidth.ToString());
            output.RenderBeginTag("td");

            if (tab.EffectiveLook.LeftIconUrl != null)
            {
              output.AddAttribute("alt", (tab.ToolTip == null) ? "" : tab.ToolTip);
              output.AddAttribute("border","0");
              output.AddAttribute("src", ConvertImageUrl(tab.EffectiveLook.LeftIconUrl));
              if (tabEffectiveLeftIconWidth > 0) output.AddAttribute("width",tabEffectiveLeftIconWidth.ToString());
              if (tab.EffectiveLook.LeftIconHeight != Unit.Empty)
                output.AddAttribute("height", Utils.ConvertUnitToJSConstant(tab.EffectiveLook.LeftIconHeight));
              output.RenderBeginTag("img");
              output.RenderEndTag(); // <img />
            }

            output.RenderEndTag(); // </td>
          }

          #endregion

          #region label

          output.AddAttribute("align", tab.TextAlign.ToString().ToLower());
          if (tab.EffectiveLook.LabelPaddingBottom != Unit.Empty) 
            output.AddStyleAttribute("padding-bottom", Utils.ConvertUnitToJSConstant(tab.EffectiveLook.LabelPaddingBottom));
          if (tab.EffectiveLook.LabelPaddingLeft != Unit.Empty) 
            output.AddStyleAttribute("padding-left", Utils.ConvertUnitToJSConstant(tab.EffectiveLook.LabelPaddingLeft));
          if (tab.EffectiveLook.LabelPaddingRight != Unit.Empty) 
            output.AddStyleAttribute("padding-right", Utils.ConvertUnitToJSConstant(tab.EffectiveLook.LabelPaddingRight));
          if (tab.EffectiveLook.LabelPaddingTop != Unit.Empty) 
            output.AddStyleAttribute("padding-top", Utils.ConvertUnitToJSConstant(tab.EffectiveLook.LabelPaddingTop));
          output.RenderBeginTag("td");

          if (!tab.TextWrap) output.RenderBeginTag("nobr");

          if (tab.ServerTemplateId != null && tab.ServerTemplateId != "")
          {
            output.Write("[templated tab]"); // We do not actually render the template
          }
          else
          {
            output.Write(tab.Text);
          }

          if (!tab.TextWrap) output.RenderEndTag(); // </nobr>

          output.RenderEndTag(); // </td>

          #endregion

          #region right icon

          int tabEffectiveRightIconWidth = (tab.EffectiveLook.RightIconWidth != Unit.Empty) ? 
            (int)tab.EffectiveLook.RightIconWidth.Value :
            maxRightIconWidth;
          if (tabEffectiveRightIconWidth > 0 || tab.EffectiveLook.RightIconUrl != null)
          {
            output.AddStyleAttribute("padding","0");
            if (tabEffectiveRightIconWidth > 0) output.AddAttribute("width", tabEffectiveRightIconWidth.ToString());
            output.RenderBeginTag("td");

            if (tab.EffectiveLook.RightIconUrl != null)
            {
              output.AddAttribute("alt", (tab.ToolTip == null) ? "" : tab.ToolTip);
              output.AddAttribute("border","0");
              output.AddAttribute("src", ConvertImageUrl(tab.EffectiveLook.RightIconUrl));
              if (tabEffectiveRightIconWidth > 0) output.AddAttribute("width", tabEffectiveRightIconWidth.ToString());
              if (tab.EffectiveLook.RightIconHeight != Unit.Empty) 
                output.AddAttribute("height", Utils.ConvertUnitToJSConstant(tab.EffectiveLook.RightIconHeight));
              output.RenderBeginTag("img");
              output.RenderEndTag(); // <img />
            }

            output.RenderEndTag(); // </td>
          }

          #endregion

          output.RenderEndTag(); // </tr>

          output.RenderEndTag(); // </table>

          output.RenderEndTag(); // </td>

          #endregion
        }

        else
        {
          #region render a css tab

          /*
          output.AddAttribute("onmousemove","return false");
          output.AddAttribute("ondblclick","return false");
          output.AddAttribute("onmouseover","TabMouseOver(this,event)");
          output.AddAttribute("onmouseout","TabMouseOut(this,event)");
          output.AddAttribute("onmousedown","TabMouseDown(this)");
          output.AddAttribute("onmouseup","TabMouseUp(this)");
          if (tab.Enabled) output.AddAttribute("onclick","TabClick(this)");
          */
          if (tab.Width != Unit.Empty) output.AddAttribute("width",Utils.ConvertUnitToJSConstant(tab.Width));
          if (tab.Height != Unit.Empty) output.AddAttribute("height",Utils.ConvertUnitToJSConstant(tab.Height));
          if (tab.ToolTip != null) output.AddAttribute("title",tab.ToolTip);
          output.AddAttribute("align", tab.TextAlign.ToString().ToLower());

          string tabStyle = "";
          if (tab.EffectiveLook.LabelPaddingBottom != Unit.Empty)
            tabStyle += "padding-bottom:" + tab.EffectiveLook.LabelPaddingBottom.ToString() + ";";
          if (tab.EffectiveLook.LabelPaddingLeft != Unit.Empty) 
            tabStyle += "padding-left:" + tab.EffectiveLook.LabelPaddingLeft.ToString() + ";";
          if (tab.EffectiveLook.LabelPaddingRight != Unit.Empty) 
            tabStyle += "padding-right:" + tab.EffectiveLook.LabelPaddingRight.ToString() + ";";
          if (tab.EffectiveLook.LabelPaddingTop != Unit.Empty) 
            tabStyle += "padding-top:" + tab.EffectiveLook.LabelPaddingTop.ToString() + ";";
          effectiveCssClass = tab.EffectiveLook.CssClass;
          if (effectiveCssClass != null && effectiveCssClass != "" && this.RenderDefaultStyles)
          {
            tabStyle += defaultStyles[effectiveCssClass];
          }
          if (tabStyle != "")
          {
            output.AddAttribute("style", tabStyle);
          }

          if (effectiveCssClass != null && effectiveCssClass != "" && !this.RenderDefaultStyles)
          {
            output.AddAttribute("class", effectiveCssClass);
          }
          output.RenderBeginTag("td");

          if (!tab.TextWrap) output.RenderBeginTag("nobr");
                    
          if (tab.ServerTemplateId != null && tab.ServerTemplateId != "")
          {
            output.Write("[templated tab]"); // We do not actually render the template
          }
          else
          {
            output.Write(tab.Text);
          }

          if (!tab.TextWrap) output.RenderEndTag(); // </nobr>

          output.RenderEndTag(); // </td>

          #endregion
        }

      }
      #endregion

      output.RenderEndTag(); // </tr>
      
      output.RenderEndTag(); // </table>

      output.Write("</div>");
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate for <see cref="ItemSelected"/> event of <see cref="TabStrip"/> class.
    /// </summary>
    public delegate void ItemSelectedEventHandler(object sender, TabStripTabEventArgs e);

    /// <summary>
    /// Fires after a tab is selected.
    /// </summary>
    [ Description("Fires after a tab is selected."), 
    Category("TabStrip Events") ]
    public event ItemSelectedEventHandler ItemSelected;

    private void OnItemSelected(TabStripTabEventArgs e) 
    {
      if (ItemSelected != null) 
      {
        ItemSelected(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="ItemDataBound"/> event of <see cref="TabStrip"/> class.
    /// </summary>
    public delegate void ItemDataBoundEventHandler(object sender, TabStripTabDataBoundEventArgs e);
		
    /// <summary>
    /// Fires after a TabStrip tab is data bound.
    /// </summary>
    [ Description("Fires after a TabStrip tab is data bound."),
    Category("TabStrip Events") ]
    public event ItemDataBoundEventHandler ItemDataBound;

    // generic trigger
    protected override void OnNodeDataBound(NavigationNode oNode, object oDataItem) 
    {
      if (ItemDataBound != null) 
      {
        TabStripTabDataBoundEventArgs e = new TabStripTabDataBoundEventArgs();
        
        e.Tab = (TabStripTab)oNode;
        e.DataItem = oDataItem;

        ItemDataBound(this, e);
      }   
    }

    #endregion

  }

	#region Supporting types

  /// <summary>
  /// Arguments for <see cref="TabStrip.ItemSelected"/> server-side event of <see cref="TabStrip"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class TabStripTabEventArgs : EventArgs
  {
    /// <summary>
    /// The command name.
    /// </summary>
    public string Command;

    /// <summary>
    /// The tab in question.
    /// </summary>
    public TabStripTab Tab;
  }

  /// <summary>
  /// Arguments for <see cref="TabStrip.ItemDataBound"/> server-side event of <see cref="TabStrip"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class TabStripTabDataBoundEventArgs : EventArgs
  {
    /// <summary>
    /// The TabStripTab node.
    /// </summary>
    public TabStripTab Tab;

    /// <summary>
    /// The data item bound to.
    /// </summary>
    public object DataItem;
  }

  /// <summary>
  /// Specifies how a <see cref="TabStrip"/> control is oriented.
  /// </summary>
  public enum TabStripOrientation
  {
    /// <summary>Horizontal TabStrip, subgroups expanding below. (Default)</summary>
    HorizontalTopToBottom,

    /// <summary>Horizontal TabStrip, subgroups expanding above.</summary>
    HorizontalBottomToTop,

    /// <summary>Vertical TabStrip, subgroups expanding to the right.</summary>
    VerticalLeftToRight,

    /// <summary>Vertical TabStrip, subgroups expanding to the left.</summary>
    VerticalRightToLeft
  }

  /// <summary>
  /// Specifies how tabs within a <see cref="TabStrip"/> tab group are aligned.
  /// </summary>
  public enum TabStripAlign
  {
    /// <summary>Align tabs to the left. (Default)</summary>
    Left,

    /// <summary>Align tabs in the middle.</summary>
    Center,

    /// <summary>Align tabs to the right.</summary>
    Right,

    /// <summary>Justify the tabs across the group.</summary>
    Justify
  }

  /// <summary>
  /// Specifies how contents of tabs of a <see cref="TabStrip"/> control are arranged.
  /// </summary>
  public enum TabOrientation
  {
    /// <summary>Horizontal, like most text. (Default)</summary>
    Horizontal,

    /// <summary>Each character on new line, single file from top to bottom.</summary>
    VerticalColumn,

    /// <summary>Like usual horizontal text rotated clockwise by 90 degrees.</summary>
    VerticalTopToBottom,

    /// <summary>Like usual horizontal text rotated counterclockwise by 90 degrees.</summary>
    VerticalBottomToTop
  }

	#endregion

}
