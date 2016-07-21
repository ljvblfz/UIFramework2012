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
  /// <seealso cref="Menu.WebService" />
  /// <seealso cref="Menu.WebServiceMethod" />
  public class MenuWebServiceRequest
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
  /// This is the type returned from the web service method set to handle requests for menu items.
  /// </remarks>
  /// <seealso cref="Menu.WebService" />
  /// <seealso cref="Menu.WebServiceMethod" />
  public class MenuWebServiceResponse : BaseNavigatorWebServiceResponse
  {
    MenuItemCollection _items;

    /// <summary>
    /// Node data to be sent back to the client. Read-only.
    /// </summary>
    public ArrayList Items
    {
      get
      {
        return NodesToArray(_items);
      }
    }

    public MenuWebServiceResponse()
    {
      _items = new MenuItemCollection(null, null);
    }

    public void AddItem(MenuItem oItem)
    {
      _items.Add(oItem);
    }
  }

  #endregion


  /// <summary>
  /// Displays a pop-up menu in the web page.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     Creates a pop-up Menu on the web page.  Depending on its <see cref="ContextMenu"/> property, the Menu can act as a permanent part of the page, 
  ///     or as a context menu that is activated by some user action.
  ///   </para>
  ///   <para>
  ///     Menu <b>contents</b> are organized as a hierarchy of <see cref="MenuItem"/> objects, accessed via the <see cref="Items"/> property.
  ///     There are a number of ways to manipulate the menu's <b>contents</b>:
  ///     <list type="bullet">
  ///       <item>Using the Menu <b>designer</b> to visually set up the structure.</item>
  ///       <item><b>Inline</b> within the aspx (or ascx) file, by nesting the item structure within the Menu tag's inner property tag &lt;Items&gt;.</item>
  ///       <item>From an XML <b>file</b> specified by the <see cref="BaseNavigator.SiteMapXmlFile"/> property.</item>
  ///       <item><b>Programmatically on the server</b> by using the server-side API.  For an introduction, see 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Menu_Creating_a_Menu_in_Code.htm">Creating a Menu in Code</a>.</item>
  ///       <item><b>Programmatically on the client</b> by using the client-side API.  For more information, see 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_ClientSide_API.htm">Overview of Web.UI Client-side Programming</a>
  ///         and client-side reference for <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Menu_class.htm">Menu</a> and 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~MenuItem_class.htm">MenuItem</a> classes.</item>
  ///     </list>
  ///   </para>
  ///   <para>
  ///     Menu <b>styles</b> are largely specified via CSS classes, which need to be defined separate from the Menu.
  ///     The CSS classes and other presentation-related settings are then assigned via various properties of the Menu and related classes.
  ///     For more information see <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_Look_and_Feel_Properties.htm">Look and Feel Properties</a>.
  ///     <list type="bullet">
  ///       <item>In order to streamline the setting of presentation properties for menu items, many of the properties are grouped
  ///         within the <see cref="ItemLook"/> object.  To learn more about <see cref="BaseMenu.ItemLooks"/>,
  ///         see <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_ItemLook_Concepts.htm">ItemLook Concepts</a> and 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_Navigation_ItemLooks.htm">Overview of ItemLooks in ComponentArt Navigation Controls</a>.</item>
  ///       <item>Further customization of item styles and contents can be accomplished with <see cref="BaseNavigator.ServerTemplates"/> and 
  ///         <see cref="BaseNavigator.ClientTemplates"/>.  For more information on templates, see 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_Templates_Overview.htm">Overview of Templates in Web.UI</a>.</item>
  ///       <item>A menu with no information specified will be rendered with a default set of CSS class definitions and assignments.</item>
  ///     </list>
  ///   </para>
  /// </remarks>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [PersistChildren(false)]
  [ParseChildren(true)]
  [Designer(typeof(ComponentArt.Web.UI.MenuItemsDesigner))]
  public sealed class Menu : BaseMenu
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
    /// Whether to perform a postback when an item is checked or unchecked.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a postback when an item is checked or unchecked.")]
    public bool AutoPostBackOnCheckChanged
    {
      get
      {
        return Utils.ParseBool(Properties["AutoPostBackOnCheckChanged"], false);
      }
      set
      {
        Properties["AutoPostBackOnCheckChanged"] = value.ToString();
      }
    }

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
        return Utils.ParseBool(Properties["AutoTheming"], false);
      }
      set
      {
        Properties["AutoTheming"] = value.ToString();
      }
    }

    /// <summary>
    /// String to be prepended to CSS classes used in theming.  Default is 'cart-'.
    /// </summary>
    [DefaultValue("cart-")]
    [Category("Behavior")]
    [Description("String to be prepended to CSS classes used in theming.  Default is 'cart-'.")]
    public string AutoThemingCssClassPrefix
    {
      get
      {
        if (Properties["AutoThemingCssClassPrefix"] == null)
        {
          return "cart-";
        }
        return (string)Properties["AutoThemingCssClassPrefix"];
      }
      set
      {
        Properties["AutoThemingCssClassPrefix"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to collapse the groups in order or simultaneously.
    /// </summary>
    /// <value>
    /// When <b>CascadeCollapse</b> is true, groups are collapsed in order, a parent group starting to collapse only after its 
    /// child group has finished collapsing.  When it is false, all groups collapse at the same time.  Default value is true.
    /// </value>
    /// <remarks>
    /// If <see cref="BaseMenu.CollapseDuration"/> is zero or if both <see cref="BaseMenu.CollapseSlide"/> and 
    /// <see cref="BaseMenu.CollapseTransition"/> are <b>None</b>, <b>CascadeCollapse</b> will have no visible effect.
    /// </remarks>
    [Category("Animation")]
    [DefaultValue(true)]
    [Description("Whether to collapse the groups in order or simultaneously.")]
    public bool CascadeCollapse
    {
      get
      {
        return Utils.ParseBool(Properties["CascadeCollapse"], true);
      }
      set
      {
        Properties["CascadeCollapse"] = value.ToString();
      }
    }

    private MenuClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public MenuClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new MenuClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="MenuClientEvents.ContextMenuHide">ClientEvents.ContextMenuHide</see> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use ClientEvents.ContextMenuHide instead.")]
    [Obsolete("Deprecated.  Use ClientEvents.ContextMenuHide instead.", false)]
    public string ClientSideOnContextMenuHide
    {
      get
      {
        return (string)Properties["ClientSideOnContextMenuHide"];
      }
      set
      {
        Properties["ClientSideOnContextMenuHide"] = value;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="MenuClientEvents.ContextMenuShow">ClientEvents.ContextMenuShow</see> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use ClientEvents.ContextMenuShow instead.")]
    [Obsolete("Deprecated.  Use ClientEvents.ContextMenuShow instead.", false)]
    public string ClientSideOnContextMenuShow
    {
      get
      {
        return (string)Properties["ClientSideOnContextMenuShow"];
      }
      set
      {
        Properties["ClientSideOnContextMenuShow"] = value;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="MenuClientEvents.ItemMouseOut">ClientEvents.ItemMouseOut</see> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use ClientEvents.ItemMouseOut instead.")]
    [Obsolete("Deprecated.  Use ClientEvents.ItemMouseOut instead.", false)]
    public string ClientSideOnItemMouseOut
    {
      get
      {
        return (string)Properties["ClientSideOnItemMouseOut"];
      }
      set
      {
        Properties["ClientSideOnItemMouseOut"] = value;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="MenuClientEvents.ItemMouseOver">ClientEvents.ItemMouseOver</see> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use ClientEvents.ItemMouseOver instead.")]
    [Obsolete("Deprecated.  Use ClientEvents.ItemMouseOver instead.", false)]
    public string ClientSideOnItemMouseOver
    {
      get
      {
        return (string)Properties["ClientSideOnItemMouseOver"];
      }
      set
      {
        Properties["ClientSideOnItemMouseOver"] = value;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="MenuClientEvents.ItemSelect">ClientEvents.ItemSelect</see> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use ClientEvents.ItemSelect instead.")]
    [Obsolete("Deprecated.  Use ClientEvents.ItemSelect instead.", false)]
    public string ClientSideOnItemSelect
    {
      get
      {
        return (string)Properties["ClientSideOnItemSelect"];
      }
      set
      {
        Properties["ClientSideOnItemSelect"] = value;
      }
    }

    /// <summary>
    /// Delay between the mouse leaving the menu and the menu starting to collapse.
    /// </summary>
    /// <value>
    /// Delay is expressed in milliseconds.  Default value is 500.
    /// </value>
    /// <seealso cref="ExpandDelay" />
    [Category("Behavior")]
    [DefaultValue(500)]
    [Description("Delay between the mouse leaving the menu and the menu starting to collapse.")]
    public int CollapseDelay
    {
      get
      {
        return Utils.ParseInt(Properties["CollapseDelay"]);
      }
      set
      {
        Properties["CollapseDelay"] = value.ToString();
      }
    }

    /// <summary>
    /// Client-side ID of the element to which this context menu is bound.
    /// </summary>
    /// <remarks>
    /// This property is only in effect when <see cref="ContextMenu"/> is set to <b>ControlSpecific</b>.
    /// </remarks>
    /// <seealso cref="ContextMenu" />
    [Category("Behavior")]
    [Description("Client-side ID of the element to which this context menu is bound.")]
    public string ContextControlId
    {
      get
      {
        return (string)Properties["ContextControlId"];
      }
      set
      {
        Properties["ContextControlId"] = value;
      }
    }

    /// <summary>
    /// Value that was passed to the context menu by the caller on the client side.
    /// </summary>
    /// <remarks>This property is readonly on the server.  It is intended to be edited only by client-side scripts.
    /// <para>This property is most often used when <see cref="ContextMenu"/> is set to <b>Custom</b>.  A custom context menu
    /// is displayed by using ShowContextMenu method of the client-side Menu object.  The last parameter of this method is
    /// optional and if supplied is used to set <b>ContextData</b>.</para>
    /// </remarks>
    /// <example>
    /// In this example two controls share the same context menu and use its <b>ContextData</b> property to let the server 
    /// know which one caused the postback.  ContextData is set on the client in the ShowContextMenu function call.
    /// <code>
    /// <![CDATA[
    /// &lt;%@ Page Language="C#" %>
    /// &lt;%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
    /// &lt;script runat="server">
    /// void Page_Load()
    /// {
    ///   if (Page.IsPostBack)
    ///   {
    ///     PostBackInfo.Text = "PostBack caused by " + Menu1.ContextData + " label.";
    ///   }
    /// }
    /// &lt;/script>
    /// &lt;html>
    /// &lt;head>
    /// &lt;/head>
    /// &lt;body>
    /// &lt;form runat="server">
    ///   &lt;asp:Label runat="server" ID="Cyan" BackColor="cyan" onclick="Menu1.ShowContextMenu(event,'cyan')">Cyan&lt;/asp:Label>
    ///   &lt;asp:Label runat="server" ID="Pink" BackColor="pink" onclick="Menu1.ShowContextMenu(event,'pink')">Pink&lt;/asp:Label>
    ///   &lt;asp:Label runat="server" ID="PostBackInfo">Not a PostBack&lt;/asp:Label>
    ///   &lt;ComponentArt:Menu runat="server" ID="Menu1" ContextMenu="Custom">
    ///     &lt;Items>
    ///       &lt;ComponentArt:MenuItem Text="Click to PostBack" AutoPostBackOnSelect="True" />
    ///     &lt;/Items>
    ///   &lt;/ComponentArt:Menu>
    /// &lt;/form>
    /// &lt;/body>
    /// &lt;/html>
    /// ]]>
    /// </code>
    /// </example>
    /// <seealso cref="ContextMenu" />
    [Browsable(false)]
    public string ContextData
    {
      get
      {
        return Context == null ? null : Context.Request.Form[this.GetSaneId() + "_ContextData"];
      }
    }

    /// <summary>
    /// Determines whether this menu control is a context menu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to determine whether a Menu will render as a context menu, or as a permanent part of the page. It is also
    /// used to determine the specific type of context menu, affecting the way in which the menu is shown. The value of this property
    /// is represented by the <see cref="Menu.ContextMenuType" /> enumeration. Values of <code>ContextMenuType.Simple</code> 
    /// and <code>ContextMenuType.ControlSpecific</code> result in menus which are displayed after a right-click, 
    /// while a value of <code>ContextMenuType.Custom</code> results in a menu which needs to be shown programatically using one of the following
    /// client-side methods:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Menu_showContextMenuAtEvent_method.htm">showContextMenuAtEvent</a>,
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Menu_showContextMenuAtElement_method.htm">showContextMenuAtElement</a>, or
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Menu_showContextMenuAtPoint_method.htm">showContextMenuAtPoint</a>.
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is ContextMenuType.None.
    /// </value>
    /// <seealso cref="ContextControlId" />
    [Category("Behavior")]
    [DefaultValue(ContextMenuType.None)]
    [Description("Determines whether this menu control is a context menu.")]
    public ContextMenuType ContextMenu
    {
      get
      {
        return Utils.ParseContextMenuType(Properties["ContextMenu"]);
      }
      set
      {
        Properties["ContextMenu"] = value.ToString();
      }
    }

    /// <summary>
    /// Direction in which the groups expand.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property sets the default direction which menu groups will expand in. As with other default properties set at the <see cref="Menu" />
    /// level, this property is inherited by all child groups unless it is overridden at the <see cref="MenuItem" /> level (
    /// <see cref="MenuItem.DefaultGroupExpandDirection" />). The <see cref="Menu.DefaultGroupExpandOffsetX" /> and 
    /// <see cref="Menu.DefaultGroupExpandOffsetX" /> properties can be used to fine-tune the placement of subgroups when they expand.
    /// </para>
    /// <para>
    /// This property is represented by the <see cref="Menu.GroupExpandDirection" /> enumeration.
    /// </para>
    /// <para>
    /// To override this property for a single subgroup, use the MenuItem <see cref="MenuItem.SubGroupExpandDirection" /> property.
    /// To creat a new default for all subgroups of a given MenuItem, use the MenuItem <see cref="MenuItem.DefaultSubGroupExpandDirection" /> property.
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is GroupExpandDirection.Auto.
    /// </value>
    /// <seealso cref="DefaultGroupExpandOffsetX" />
    /// <seealso cref="DefaultGroupExpandOffsetY" />
    /// <seealso cref="TopGroupExpandDirection"/>
    /// <seealso cref="MenuItem.DefaultSubGroupExpandDirection" />
    /// <seealso cref="MenuItem.SubGroupExpandDirection" />
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
    /// <remarks>
    /// <para>
    /// This property sets the default offset value, along the x-axis for descendant groups
    /// of the <see cref="Menu" /> instance. This property along with 
    /// <see cref="DefaultGroupExpandOffsetY" />, is used to fine-tune the placement of subgroups, changing
    /// the visual layout of the rendered menu.
    /// </para> 
    /// <para>
    /// To override this property for a single subgroup, use the <see cref="MenuItem" /> <see cref="MenuItem.SubGroupExpandOffsetX" />
    /// property. To create a new default for all subgroups of a specific item, use the <see cref="MenuItem.DefaultSubGroupExpandOffsetX" />
    /// property.
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is 0.
    /// </value>
    /// <seealso cref="DefaultGroupExpandDirection" />
    /// <seealso cref="DefaultGroupExpandOffsetY" />
    /// <seealso cref="TopGroupExpandOffsetX"/>
    /// <seealso cref="MenuItem.SubGroupExpandOffsetX" />
    /// <seealso cref="MenuItem.DefaultSubGroupExpandOffsetX" />
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
    /// <remarks>
    /// <para>
    /// This property sets the default offset value, along the y-axis for descendant groups of the <see cref="Menu" /> instance.
    /// This property, along with <see cref="DefaultGroupExpandOffsetX" />, is used to fine-tune the placement of subgroups, changing
    /// the visual layout of the rendered menu.
    /// </para>
    /// <para>
    /// To override this property for a single subgroup, use the <see cref="MenuItem" /> <see cref="MenuItem.SubGroupExpandOffsetY" />
    /// property. To create a new default for all subgroups of an item, use the <see cref="MenuItem.DefaultSubGroupExpandOffsetY" />
    /// property.
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is 0.
    /// </value>
    /// <seealso cref="DefaultGroupExpandDirection" />
    /// <seealso cref="DefaultGroupExpandOffsetX" />
    /// <seealso cref="TopGroupExpandOffsetY"/>
    /// <seealso cref="MenuItem.SubGroupExpandOffsetY" />
    /// <seealso cref="MenuItem.DefaultSubGroupExpandOffsetY" />
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
    /// Height of groups.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property defines the default value to use for the height of groups. Note that it is usually
    /// desireable to override this property for the top <see cref="Menu" /> group using the <see cref="Menu.Height" />
    /// property.
    /// </para>
    /// <para>
    /// If a style property is not set for a particular element in a menu, that element will inherit the property
    /// from its parent. For this reason it is beneficial to set default <see cref="ItemLook" /> and style properties
    /// for the control as a whole, then override them for specific <see cref="MenuItem">MenuItems</see> in order to
    /// customize the appearance of the rendered control.
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    /// <seealso cref="System.Web.UI.WebControls.WebControl.Height" />
    /// <seealso cref="MenuItem.SubGroupHeight" />
    /// <seealso cref="MenuItem.DefaultSubGroupHeight" />
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    [Description("Height of groups.")]
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
    /// Spacing between group items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If a style property is not set for a particular element in a <see cref="Menu" />, that element will inherit the property from 
    /// its parent. For this reason, it is beneficial to set default <see cref="ItemLook" /> and style properties for 
    /// the control as a whole, then override them for specific <see cref="MenuItem">MenuItems</see> to further customize the 
    /// appearance of the rendered control. 
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    /// <seealso cref="TopGroupItemSpacing" />
    /// <seealso cref="MenuItem.SubGroupItemSpacing" />
    /// <seealso cref="MenuItem.DefaultSubGroupItemSpacing" />
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    [Description("Spacing between group items.")]
    public Unit DefaultGroupItemSpacing
    {
      get
      {
        return Utils.ParseUnit(Properties["DefaultGroupItemSpacing"]);
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultGroupItemSpacing"] = value.ToString();
        }
        else
        {
          throw new Exception("Item spacing may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Orientation of subgroups.
    /// </summary>
    /// <value>
    /// Default value is GroupOrientation.Vertical.
    /// </value>
    /// <seealso cref="Orientation" />
    /// <seealso cref="MenuItem.DefaultSubGroupOrientation" />
    /// <seealso cref="MenuItem.SubGroupOrientation" />
    [Category("Layout")]
    [DefaultValue(GroupOrientation.Vertical)]
    [Description("Orientation of subgroups.")]
    public GroupOrientation DefaultGroupOrientation
    {
      get
      {
        return Utils.ParseGroupOrientation(Properties["DefaultGroupOrientation"]);
      }
      set
      {
        Properties["DefaultGroupOrientation"] = value.ToString();
      }
    }

    /// <summary>
    /// Width of groups.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property defines the default value to use for the width of groups. Note that it is often desirable to override this 
    /// property for the top menu group using the <see cref="Menu.Width" /> property.
    /// </para>
    /// <para>
    /// If a style property is not set for a particular element in a <see cref="Menu" />, that element will inherit the property from 
    /// its parent. For this reason, it is beneficial to set default <see cref="ItemLook" /> and style properties for 
    /// the control as a whole, then override them for specific <see cref="MenuItem">MenuItems</see> to further customize the 
    /// appearance of the rendered control. 
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    /// <seealso cref="System.Web.UI.WebControls.WebControl.Width" />
    /// <seealso cref="MenuItem.SubGroupWidth" />
    /// <seealso cref="MenuItem.DefaultSubGroupWidth" />
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    [Description("Width of groups.")]
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
    /// Delay between the mouse entering a MenuItem and its subgroup starting to expand.
    /// </summary>
    /// <value>
    /// Delay is expressed in milliseconds.  Default value is 0.
    /// </value>
    /// <seealso cref="CollapseDelay" />
    [Category("Behavior")]
    [DefaultValue(0)]
    [Description("Delay between the mouse entering a MenuItem and its subgroup starting to expand.")]
    public int ExpandDelay
    {
      get
      {
        return Utils.ParseInt(Properties["ExpandDelay"]);
      }
      set
      {
        Properties["ExpandDelay"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to expand a subgroup of an item that is disabled.
    /// </summary>
    /// <value>
    /// When false, the menu will not expand a subgroup of a disabled item.
    /// Default value is true.
    /// </value>
    [Category("Behavior")]
    [DefaultValue(true)]
    [Description("Whether to expand a subgroup of an item that is disabled.")]
    public bool ExpandDisabledItems
    {
      get
      {
        return Utils.ParseBool(Properties["ExpandDisabledItems"], true);
      }
      set
      {
        Properties["ExpandDisabledItems"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to have expanded look take precedence over hover when both are applicable.
    /// </summary>
    /// <value>
    /// When true, a menu item will show in its expanded state when it is both hovered over and its subgroup is expanded.
    /// When false, a menu item will show in its hover state when it is both hovered over and its subgroup is expanded.
    /// Default value is false, meaning that hover look will take precedence.
    /// </value>
    [Category("Appearance")]
    [DefaultValue(false)]
    [Description("Whether to have expanded look take precedence over hover when both are applicable. Default is false.")]
    public bool ExpandedOverridesHover
    {
      get
      {
        return Utils.ParseBool(Properties["ExpandedOverridesHover"], false);
      }
      set
      {
        Properties["ExpandedOverridesHover"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to wait for a click before expanding and collapsing the menu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Default <see cref="Menu" />behaviour is to expand a group when one of the root items is hovered over. When 
    /// this property is set to true, this behavior is changed, and the menu will not expand until one of the root items is clicked. 
    /// Hovering over items will then cause them to expand, as per usual, although a group will remain expanded until the user 
    /// clicks outside of the menu. 
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is false.
    /// </value>
    [Category("Behavior")]
    [DefaultValue(false)]
    [Description("Whether to wait for a click before expanding and collapsing the menu.")]
    public bool ExpandOnClick
    {
      get
      {
        return Utils.ParseBool(Properties["ExpandOnClick"], false);
      }
      set
      {
        Properties["ExpandOnClick"] = value.ToString();
      }
    }

		/// <summary>
		/// Whether to hide the select elements that would obscure the menu.
		/// </summary>
    /// <value>
    /// When true, the menu temporarily hides &lt;select&gt; elements that would obscure its pop-up groups.  Default value is true.
    /// </value>
		/// <remarks>
		/// HideSelectElements only has effect on Windows IE browsers and only when <see cref="OverlayWindowedElements"/> is not in effect.
		/// </remarks>
		/// <seealso cref="OverlayWindowedElements" />
		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("Whether to hide the select elements that would obscure the menu.")]
		public bool HideSelectElements
		{
			get
			{
				return Utils.ParseBool(Properties["HideSelectElements"], true); 
			}
			set
			{
				Properties["HideSelectElements"] = value.ToString();
			}
		}

		/// <summary>
		/// Whether to default to Hover look for the items whose child groups are expanded.
		/// </summary>
    /// <value>
    /// When false, the items default to their normal look when the mouse is over their expanded subgroup.
    /// When true, the items default to their hover look when the mouse is over their expanded subgroup.
    /// Default value is true.
    /// </value>
    /// <remarks>
    /// There are many ways to override and fine-tune the behaviour specified by this property, for example via 
    /// <see cref="ItemLook.ExpandedCssClass">ExpandedCssClass</see> and similar <see cref="ItemLook" /> properties.
    /// </remarks>
		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("Whether to default to Hover look for the items whose child groups are expanded.")]
		public bool HighlightExpandedPath
		{
			get 
			{
				return Utils.ParseBool(Properties["HighlightExpandedPath"], true); 
			}
			set 
			{
				Properties["HighlightExpandedPath"] = value.ToString();
			}
		}
    
		/// <summary>
		/// Collection of root MenuItems.
		/// </summary>
		[Browsable(false)]
		[Description("Collection of root MenuItems.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public MenuItemCollection Items
		{
			get
			{
				return (MenuItemCollection)nodes;
			}
		}

		/// <summary>
		/// Whether to enable keyboard control of the Menu.
		/// </summary>
    /// <value>
    /// When true, the menu responds to keyboard shortcuts.  Default value is true.
    /// </value>
		/// <seealso cref="NavigationNode.KeyboardShortcut" />
		[Category("Behavior")]
		[Description("Whether to enable keyboard control of the Menu.")]
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
		/// Orientation of the top group.
		/// </summary>
    /// <value>
    /// Default value is GroupOrientation.Horizontal.
    /// </value>
		/// <seealso cref="DefaultGroupOrientation" />
		/// <seealso cref="MenuItem.SubGroupOrientation" />
		/// <seealso cref="MenuItem.DefaultSubGroupOrientation" />
		[Category("Layout")]
		[DefaultValue(GroupOrientation.Horizontal)]
		[Description("Orientation of the top group.")]
		public GroupOrientation Orientation
		{
			get 
			{
				object o = Properties["Orientation"];
				return (o!=null && o.ToString()!=string.Empty) ? Utils.ParseGroupOrientation(o) : GroupOrientation.Horizontal;
			}
			set 
			{
				Properties["Orientation"] = value.ToString();
			}
		}

		/// <summary>
    /// Whether to overlay windowed elements that would obscure the menu.
		/// </summary>
    /// <value>
    /// When true, the menu overlays windowed elements that would obscure its pop-up groups.  When false, the menu does not 
    /// attempt to overlay windowed elements.  Default value is true.
    /// </value>
		/// <remarks>
    /// <para>
    /// OverlayWindowedElements only applies to Windows IE browsers.  In particular, windowed element overlay is only 
    /// available in IE 5.5+ on Windows.
    /// </para>
    /// <para>
    /// Windowed elements include select boxes and embedded objects such as ActiveX controls and Java applets.
    /// </para>
    /// </remarks>
		/// <seealso cref="HideSelectElements" />
		[Category("Behavior")]
		[DefaultValue(true)]
    [Description("Whether to overlay windowed elements that would obscure the menu.")]
		public bool OverlayWindowedElements
		{
			get
			{
				return Utils.ParseBool(Properties["OverlayWindowedElements"], true);
			}
			set
			{
				Properties["OverlayWindowedElements"] = value.ToString();
			}
		}

    /// <summary>
    /// zIndex of the first pop-up group.  Default is 999.
    /// </summary>
    /// <remarks>
    /// In order to overlay the contents of the page, Menu's pop-up groups are absolutely positioned and given a high zIndex.
    /// For rendering reasons, the zIndex of every subsequently popped up group is increased by one.
    /// <para>
    /// This property determines the zIndex of the first group that the user pops up on page.  (For context menus this is always
    /// the root group.  For all other menus it is one of the subgroups.)  You should set this property to be higher than the
    /// zIndex of any content that should appear to be behind the menu's pop-up groups.  You should set it to be considerably 
    /// (at least by a few hundred) lower than the zIndex of any content that should appear to be in front of the menu's 
    /// pop-up groups.
    /// </para>
    /// <para>
    /// Note that this zIndex is never applied to the root group of a non-context menu (in other words, a non-pop-up group).
    /// You can set its zIndex via its CSS class, by using the "z-index" style attribute.
    /// </para>
    /// </remarks>
    [Category("Layout")]
    [DefaultValue(999)]
    [Description("zIndex of the first pop-up group.  Default is 999.")]
    public int PopUpZIndexBase
    {
      get
      {
        return Utils.ParseInt(Properties["PopUpZIndexBase"], 999);
      }
      set
      {
        Properties["PopUpZIndexBase"] = value.ToString();
      }
    }

    private ItemLook _scrollDownLook;
    /// <summary>
    /// Look of the down scroller.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the following tutorials for more information on <see cref="ItemLook">ItemLooks</see> and styling the
    /// menu control:
    /// <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_ItemLook_Concepts.htm">ItemLook Concepts</a>,
    /// <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_Navigation_ItemLooks.htm">ItemLooks in Navigation Controls</a>,
    /// and <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_Look_and_Feel_Properties.htm">Look and Feel Properties</a>.
    /// </para>
    /// </remarks>
    /// <seealso cref="ScrollDownLookId" /><seealso cref="ScrollUpLook" /><seealso cref="ScrollingEnabled" />
    [Description("Look of the down scroller.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook ScrollDownLook
    {
      get
      {
        if (_scrollDownLook == null)
        {
          _scrollDownLook = new ItemLook();
        }
        return _scrollDownLook;
      }
      set
      {
        if (value != null)
        {
          _scrollDownLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the look for down scroller.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the following tutorials for more information on <see cref="ItemLook">ItemLooks</see> and styling the
    /// menu control:
    /// <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_ItemLook_Concepts.htm">ItemLook Concepts</a>,
    /// <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_Navigation_ItemLooks.htm">ItemLooks in Navigation Controls</a>,
    /// and <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_Look_and_Feel_Properties.htm">Look and Feel Properties</a>.
    /// </para>
    /// </remarks>
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

    private ItemLook _scrollUpLook;
    /// <summary>
    /// Look of the up scroller.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the following tutorials for more information on <see cref="ItemLook">ItemLooks</see> and styling the
    /// menu control:
    /// <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_ItemLook_Concepts.htm">ItemLook Concepts</a>,
    /// <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_Navigation_ItemLooks.htm">ItemLooks in Navigation Controls</a>,
    /// and <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_Look_and_Feel_Properties.htm">Look and Feel Properties</a>.
    /// </para>
    /// </remarks>
    /// <seealso cref="ScrollUpLookId" /><seealso cref="ScrollDownLook" /><seealso cref="ScrollingEnabled" />
    [Description("Look of the up scroller.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook ScrollUpLook
    {
      get
      {
        if (_scrollUpLook == null)
        {
          _scrollUpLook = new ItemLook();
        }
        return _scrollUpLook;
      }
      set
      {
        if (value != null)
        {
          _scrollUpLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the look for up scroller.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the following tutorials for more information on <see cref="ItemLook">ItemLooks</see> and styling the
    /// menu control:
    /// <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_ItemLook_Concepts.htm">ItemLook Concepts</a>,
    /// <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_Navigation_ItemLooks.htm">ItemLooks in Navigation Controls</a>,
    /// and <a href="ms-help:/../ms-its:ComponentArt.Web.UI.AJAX/WebUI_Look_and_Feel_Properties.htm">Look and Feel Properties</a>.
    /// </para>
    /// </remarks>
    /// <seealso cref="ScrollUpLook" /><seealso cref="ScrollingEnabled" />
    [Category("Appearance")]
    [DefaultValue("")]
    [Description("The ID of the look for up scroller.")]
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
    /// Whether to enable scrolling for this Menu's groups.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a boolean property representing whether or not to enable scrolling for <see cref="MenuItem" /> groups which contain more 
    /// items than will fit in the browser window or the specified height. The default value is false meaning that the group will 
    /// be allowed to grow past its boundaries. 
    /// </para>
    /// </remarks>
    /// <value>
    /// Default is false.
    /// </value>
    /// <seealso cref="ScrollDownLook" /><seealso cref="ScrollUpLook" />
    [Category("Behavior")]
    [DefaultValue(false)]
    [Description("Whether to enable scrolling for this Menu's groups.")]
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
		/// The selected item.
		/// </summary>
    /// <value>
    /// The item that is considered selected, or null if none is.
    /// </value>
    /// <remarks>
    /// This property can be set on the server-side to force an item selection.
    /// </remarks>
    /// <seealso cref="BaseMenu.ForceHighlightedItemID" />
		[Browsable(false)]
    [Description("The selected item.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MenuItem SelectedItem
		{
			get
			{
				return (MenuItem)(base.selectedNode);
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
    /// Whether to calculate item properties on the server.
    /// </summary>
    /// <value>
    /// When true, all menu item properties are pre-calculated on the server.  Default is false.
    /// </value>
    /// <remarks>
    /// If ServerCalculateProperties is set to <b>true</b>, classic item storage is used, with item properties 
    /// being calculated on the server.  This results in reduced client-side API capabilities, but can improve
    /// client-side performance for Menus with very large item structures (with thousands of menu items).
    /// </remarks>
    [DefaultValue(false)]
    [Description("Whether to calculate item properties on the server.")]
    public bool ServerCalculateProperties
    {
      get
      {
        return Utils.ParseBool(Properties["ServerCalculateProperties"],false);
      }
      set
      {
        Properties["ServerCalculateProperties"] = value.ToString();
      }
    }
		
		/// <summary>
		/// Color of pop-up groups' shadows.
		/// </summary>
    /// <value>
    /// Default value is RGB(141,143,149), a shade of gray.
    /// </value>
    /// <remarks>
    /// Shadows always point in the direction of the bottom right.
    /// <para>Only pop-up groups drop shadows</para>
    /// <para>Shadows are only available in Internet Explorer 6 on Windows.</para>
    /// </remarks>
    /// <seealso cref="ShadowEnabled"/>
    /// <seealso cref="ShadowOffset"/>
		[Category("Appearance")]
		[DefaultValue(typeof(System.Drawing.Color),"141, 143, 149")]
    [Description("Color of pop-up groups' shadows.")]
		public Color ShadowColor
		{
			get
			{
				return Utils.ParseColor(Properties["ShadowColor"],Color.FromArgb(141,143,149));
			}
			set
			{
				Properties["ShadowColor"] = ColorTranslator.ToHtml(value);
			}
		}

		/// <summary>
		/// Whether menu's pop-up groups drop shadows.
		/// </summary>
    /// <value>
    /// Default value is true.
    /// </value>
    /// <remarks>
    /// Shadows always point in the direction of the bottom right.
    /// <para>Only pop-up groups drop shadows</para>
    /// <para>Shadows are only available in Internet Explorer 6 on Windows.</para>
    /// </remarks>
    /// <seealso cref="ShadowOffset"/>
    /// <seealso cref="ShadowColor"/>
		[Category("Appearance")]
		[DefaultValue(true)]
		[Description("Whether menu's pop-up groups drop shadows.")]
		public bool ShadowEnabled
		{
			get
			{
				return Utils.ParseBool(Properties["ShadowEnabled"],true);
			}
			set
			{
				Properties["ShadowEnabled"] = value.ToString();
			}
		}

		/// <summary>
		/// Offset of the pop-up groups' shadows.
		/// </summary>
		/// <value>
    /// An integer that determines the shadow's length. Default value is 2.
    /// </value>
		/// <remarks>
    /// Shadows always point in the direction of the bottom right.
		/// <para>Only pop-up groups drop shadows</para>
		/// <para>Shadows are only available in Internet Explorer 6 on Windows.</para>
		/// </remarks>
		/// <seealso cref="ShadowEnabled"/>
		/// <seealso cref="ShadowColor"/>
		[Category("Appearance")]
		[DefaultValue(2)]
		[Description("Offset of the pop-up groups' shadows.")]
		public int ShadowOffset
		{
			get
			{
				return Utils.ParseInt(Properties["ShadowOffset"]);
			}
			set
			{
				int minValue = 1; int maxValue = 255;
				if (minValue <= value && value <= maxValue)
				{
					Properties["ShadowOffset"] = value.ToString();
				}
				else
				{
					throw new FormatException("ShadowOffset must be at least " + minValue.ToString() + " and no more than " + maxValue.ToString() + ".");
				}
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

    /// <summary>
    /// Direction in which the top group expands, in a context menu.
    /// </summary>
    /// <value>
    /// Default value is GroupExpandDirection.Auto.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property is used with context menus to determine which direction the menu will expand. In most instances
    /// the direction will be relative to the point which was clicked. The exception to this is when the menu is expanded programatically
    /// using the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Menu_showContextMenuAtElement_method.htm">showContextMenuAtElement</a>
    /// method. This will cause the menu to be expanded relative to the element which was passed into the method.
    /// </para>
    /// <para>
    /// The location of the menu can also be offset using the <see cref="Menu.TopGroupExpandOffsetX" /> and 
    /// <see cref="Menu.TopGroupExpandOffsetY" /> properties. 
    /// </para>
    /// <para>
    /// The value of this property is represented by the <see cref="Menu.GroupExpandDirection" /> enumeration. 
    /// </para>
    /// </remarks>
    /// <seealso cref="DefaultGroupExpandDirection" />
    /// <seealso cref="TopGroupExpandOffsetX" />
    /// <seealso cref="TopGroupExpandOffsetY" />
    /// <seealso cref="MenuItem.DefaultSubGroupExpandDirection" />
    /// <seealso cref="MenuItem.SubGroupExpandDirection" />
    [Category("Layout")]
    [DefaultValue(GroupExpandDirection.Auto)]
    [Description("Direction in which the top group expands.")]
    public GroupExpandDirection TopGroupExpandDirection
    {
      get
      {
        object o = Properties["TopGroupExpandDirection"];
        return (o != null) ? Utils.ParseGroupExpandDirection(o) : this.DefaultGroupExpandDirection;
      }
      set
      {
        Properties["TopGroupExpandDirection"] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along x-axis from top group's normal expand position.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used with context menus to determine the offset along the x-axis from which the context menu will expand.
    /// In most instances this will be relative to the point which was clicked. When the menu is expanded programatically
    /// using the client-side
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Menu_showContextMenuAtElement_method.htm">showContextMenuAtElement</a>
    /// method. This will cause the menu to be expanded relative to the element which was passed into the method.
    /// </para>
    /// <para>
    /// The location of the menu along the Y-axis can be specified using the <see cref="Menu.TopGroupExpandOffsetY" /> property,
    /// while the expand direction can be set using the <see cref="Menu.TopGroupExpandDirection" />.
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is 0.
    /// </value>
    /// <remarks>
    /// This property only applies to context menus.
    /// </remarks>
    /// <seealso cref="DefaultGroupExpandDirection" />
    /// <seealso cref="DefaultGroupExpandOffsetY" />
    /// <seealso cref="DefaultGroupExpandOffsetX" />
    /// <seealso cref="TopGroupExpandDirection" />
    /// <seealso cref="TopGroupExpandOffsetY" />
    /// <seealso cref="MenuItem.SubGroupExpandOffsetX" />
    /// <seealso cref="MenuItem.DefaultSubGroupExpandOffsetX" />
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along x-axis from top group's normal expand position.")]
    public int TopGroupExpandOffsetX
    {
      get
      {
        object o = Properties["TopGroupExpandOffsetX"];
        return (o != null) ? Utils.ParseInt(o) : this.DefaultGroupExpandOffsetX;
      }
      set
      {
        Properties["TopGroupExpandOffsetX"] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along y-axis from top group's normal expand position.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used with context menus to determine the offset along the Y-axis from which the context menu will expand.
    /// In most instances this will be relative to the point which was clicked. When the menu is expanded programatically
    /// using the client-side
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Menu_showContextMenuAtElement_method.htm">showContextMenuAtElement</a>
    /// method. This will cause the menu to be expanded relative to the element which was passed into the method.
    /// </para>
    /// <para>
    /// The location of the menu along the X-axis can be specified using the <see cref="Menu.TopGroupExpandOffsetX" /> property,
    /// while the expand direction can be set using the <see cref="Menu.TopGroupExpandDirection" />.
    /// </para>
    /// </remarks>
    /// <value>
    /// Default value is 0.
    /// </value>
    /// <seealso cref="DefaultGroupExpandDirection" />
    /// <seealso cref="DefaultGroupExpandOffsetY" />
    /// <seealso cref="DefaultGroupExpandOffsetX" />
    /// <seealso cref="TopGroupExpandDirection" />
    /// <seealso cref="TopGroupExpandOffsetX" />
    /// <seealso cref="MenuItem.SubGroupExpandOffsetX" />
    /// <seealso cref="MenuItem.DefaultSubGroupExpandOffsetX" />
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along y-axis from top group's normal expand position.")]
    public int TopGroupExpandOffsetY
    {
      get
      {
        object o = Properties["TopGroupExpandOffsetY"];
        return (o != null) ? Utils.ParseInt(o) : this.DefaultGroupExpandOffsetY;
      }
      set
      {
        Properties["TopGroupExpandOffsetY"] = value.ToString();
      }
    }

    /// <summary>
    /// Spacing between top group's items.
    /// </summary>
    /// <value>
    /// Unit value for spacing between top group items, expressed in pixels.  Default value is Unit.Empty.
    /// </value>
    /// <seealso cref="DefaultGroupItemSpacing" />
    /// <seealso cref="MenuItem.SubGroupItemSpacing" />
    /// <seealso cref="MenuItem.DefaultSubGroupItemSpacing" />
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    [Description("Spacing between top group's items.")]
    public Unit TopGroupItemSpacing
    {
      get
      {
        object o = Properties["TopGroupItemSpacing"];
        return (o != null) ? Utils.ParseUnit(o) : this.DefaultGroupItemSpacing;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["TopGroupItemSpacing"] = value.ToString();
        }
        else
        {
          throw new Exception("Item spacing may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// The name of the ASP.NET AJAX web service to use for initially populating the Menu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is required for the client-side <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Menu_loadFromWebService_method.htm">loadFromWebService</a>.
    /// It specifies the web service to use for retrieving <see cref="Menu" /> data. As with all web-service functionality, the 
    /// web service must be registered with the Script Manager, and properly configured to provide access to client-side script.
    /// The <see cref="WebServiceMethod" /> property is also required. 
    /// </para>
    /// </remarks>
    /// <seealso cref="WebServiceMethod" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service to use for initially populating the Menu.")]
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
    /// <remarks>
    /// <para>
    /// Whenever data is requested from the web service, the request object is populated with the current value of this property, 
    /// making it accessible from the web service. It can also be set from client code, allowing the web service to dynamically 
    /// populate the returned data based on information sent from the client.
    /// </para>
    /// </remarks>
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
    /// The name of the ASP.NET AJAX web service method to use for initially populating the Menu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is required for the client-side <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Menu_loadFromWebService_method.htm">loadFromWebService</a>.
    /// It specifies the method which will be called on the web service to retrieve <see cref="Menu" /> data. 
    /// As with all web-service functionality, the 
    /// web service must be registered with the Script Manager, and properly configured to provide access to client-side script.
    /// The <see cref="WebService" /> property is also required, and should specify the name of the web service which contains this method.
    /// </para>
    /// </remarks>
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service method to use for initially populating the Menu.")]
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
          _propertyIndex["2"] = "ClientSideCommand";
          _propertyIndex["3"] = "DefaultSubGroupCssClass";
          _propertyIndex["4"] = "DefaultSubGroupExpandDirection";
          _propertyIndex["5"] = "DefaultSubGroupExpandOffsetX";
          _propertyIndex["6"] = "DefaultSubGroupExpandOffsetY";
          _propertyIndex["7"] = "DefaultSubGroupHeight";
          _propertyIndex["8"] = "DefaultSubGroupItemSpacing";
          _propertyIndex["9"] = "DefaultSubGroupOrientation";
          _propertyIndex["10"] = "DefaultSubGroupWidth";
          _propertyIndex["11"] = "DefaultSubItemTextAlign";
          _propertyIndex["12"] = "DefaultSubItemTextWrap";
          _propertyIndex["13"] = "Enabled";
          _propertyIndex["14"] = "Height";
          _propertyIndex["15"] = "ID";
          _propertyIndex["16"] = "KeyboardShortcut";
          _propertyIndex["17"] = "NavigateUrl";
          _propertyIndex["18"] = "PageViewId";
          _propertyIndex["19"] = "SiteMapXmlFile";
          _propertyIndex["20"] = "SubGroupCssClass";
          _propertyIndex["21"] = "SubGroupExpandDirection";
          _propertyIndex["22"] = "SubGroupExpandOffsetX";
          _propertyIndex["23"] = "SubGroupExpandOffsetY";
          _propertyIndex["24"] = "SubGroupHeight";
          _propertyIndex["25"] = "SubGroupItemSpacing";
          _propertyIndex["26"] = "SubGroupOrientation";
          _propertyIndex["27"] = "SubGroupWidth";
          _propertyIndex["28"] = "Target";
          _propertyIndex["29"] = "ServerTemplateId";
          _propertyIndex["30"] = "Text";
          _propertyIndex["31"] = "TextAlign";
          _propertyIndex["32"] = "ToggleType";
          _propertyIndex["33"] = "ToggleGroupId";
          _propertyIndex["34"] = "Checked";
          _propertyIndex["35"] = "TextWrap";
          _propertyIndex["36"] = "ToolTip";
          _propertyIndex["37"] = "Value";
          _propertyIndex["38"] = "Visible";
          _propertyIndex["39"] = "Width";
          _propertyIndex["40"] = "LookId";
          _propertyIndex["41"] = "DisabledLookId";
          _propertyIndex["42"] = "SelectedLookId";
          _propertyIndex["43"] = "ChildSelectedLookId";
          _propertyIndex["44"] = "DefaultSubItemLookId";
          _propertyIndex["45"] = "DefaultSubItemDisabledLookId";
          _propertyIndex["46"] = "DefaultSubItemSelectedLookId";
          _propertyIndex["47"] = "DefaultSubItemChildSelectedLookId";
          _propertyIndex["48"] = "ClientTemplateId";
        }
        return _propertyIndex;
      }
    }

    #endregion

    #region Public Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public Menu() : base()
		{
			// Set some defaults.
			this.ExpandDelay = 0;
			this.CollapseDelay = 500;
			this.ShadowColor = (Color)(new ColorConverter()).ConvertFromInvariantString("141, 143, 149");
			this.ShadowEnabled = true;
			this.ShadowOffset = 2;

      nodes = new MenuItemCollection(this, null);
		}

    /// <summary>
    /// Applies looks to menu items, loading specified looks by their IDs.
    /// </summary>
    /// <remarks>
    /// This method is called automatically and there is usually no need to call it explicitly.
    /// If called explicitly, this method overwrites some look settings of individual nodes.
    /// </remarks>
    public override void ApplyLooks()
    {
      if (this.ScrollDownLookId != string.Empty)
      {
        this.ScrollDownLook = this.ItemLooks[this.ScrollDownLookId];
      }
      if (this.ScrollUpLookId != string.Empty)
      {
        this.ScrollUpLook = this.ItemLooks[this.ScrollUpLookId];
      }

      base.ApplyLooks();
    }

    /// <summary>
    /// Returns the MenuItem that is checked in the toggle group with the given id.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method retrieves the checked <see cref="MenuItem" /> from the specified toggle group. This method is used with toggle groups in which only
    /// a single <code>MenuItem</code> can be checked. This occurs for items with their <see cref="MenuItem.ToggleType" />
    /// property set to <code>ItemToggleType.RadioButton</code> or <code>ItemToggleType.RadioCheckBox</code>. If no item
    /// is checked, the method will return <code>null</code>.
    /// </para>
    /// <para>
    /// Toggle groups are defined using the <see cref="MenuItem.ToggleGroupId" /> property. Items sharing the same value for that
    /// property will be in the same group. All items in a group can be retrieved using the <see cref="Menu.GetToggleGroupItems" /> method. 
    /// </para>
    /// <para>
    /// For groups containing items specified with <code>ItemToggleType.CheckBox</code>, the <see cref="Menu.getToggleGroupCheckedItems" />
    /// method should be used.
    /// </para>
    /// </remarks>
    public MenuItem getToggleGroupCheckedItem(string toggleGroupId)
    {
      MenuItem[] menuItemArray = getToggleGroupCheckedItems(toggleGroupId);
      if (menuItemArray.Length == 0)
      {
        return null;
      }
      else
      {
        return menuItemArray[0];
      }
    }

    /// <summary>
    /// Returns a MenuItem array containing the checked items in the toggle group with the given id.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method retrieves an array of checked <see cref="MenuItem">MenuItems</see> from the specified group. This method is used with 
    /// toggle groups in which any number of items can be checked. This occurs for items with their <see cref="MenuItem.ToggleType" />
    /// property set to <code>ItemToggleType.CheckBox</code>. If no items in the group are checked, the method will return 
    /// an empty array.
    /// </para>
    /// <para>
    /// Toggle groups are defined using the <see cref="MenuItem.ToggleGroupId" /> property. Items sharing the same value for that
    /// property will be in the same group. All items in a group can be retrieved using the <see cref="Menu.GetToggleGroupItems" /> method.  
    /// </para>
    /// <para>
    /// For groups containing items with <code>ToggleType</code> values of <code>ItemToggleType.RadioCheckBox</code> or
    /// <code>ItemToggleType.RadioButton</code>, the <see cref="Menu.getToggleGroupCheckedItem" /> method should be used.
    /// </para>
    /// </remarks>
    public MenuItem[] getToggleGroupCheckedItems(string toggleGroupId)
    {
      ArrayList menuItemList = new ArrayList();
      GetToggleGroupItemsRecurse(toggleGroupId, true, this.Items, ref menuItemList);
      MenuItem[] menuItemArray = new MenuItem[menuItemList.Count];
      menuItemList.CopyTo(menuItemArray);
      return menuItemArray;
    }

    /// <summary>
    /// Returns a string array of all toggle group names.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method returns a string array containing the names of all toggle groups for the <see cref="Menu" /> it is called on.
    /// Toggle groups are defined using the <see cref="MenuItem.ToggleGroupId" /> property. Items sharing the same value for that
    /// property will be in the same group. 
    /// </para>
    /// <para>
    /// All items in a group can be retrieved by passing the group name into the <see cref="Menu.GetToggleGroupItems" /> method.
    /// Checked items can be retrieved using one of the following methods: <see cref="Menu.getToggleGroupCheckedItems" /> or 
    /// <see cref="Menu.getToggleGroupCheckedItem" />.
    /// </para>
    /// </remarks>
    public string[] GetToggleGroupIds()
    {
      StringCollection toggleGroupIdCollection = new StringCollection();
      GetToggleGroupIdsRecurse(this.Items, ref toggleGroupIdCollection);
      string[] toggleGroupIdArray = new String[toggleGroupIdCollection.Count];
      toggleGroupIdCollection.CopyTo(toggleGroupIdArray, 0);
      return toggleGroupIdArray;
    }
    private void GetToggleGroupIdsRecurse(MenuItemCollection items, ref StringCollection toggleGroupIdCollection)
    {
      foreach (MenuItem item in items)
      {
        if (item.ToggleGroupId != null && item.ToggleGroupId != String.Empty)
        {
          toggleGroupIdCollection.Add(item.ToggleGroupId);
        }
        GetToggleGroupIdsRecurse(item.Items, ref toggleGroupIdCollection);
      }
    }

    /// <summary>
    /// Returns a MenuItem array containing the items in the toggle group with the given id.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a toggle group id (string) as an argument, returning an array of <see cref="MenuItem">MenuItems</see>
    /// which are contained in the corresponding group. 
    /// </para>
    /// <para>
    /// Toggle groups are defined using the <see cref="MenuItem.ToggleGroupId" /> property. Items sharing the same value for that
    /// property will be in the same group. All items in a group can be retrieved using the <see cref="Menu.GetToggleGroupItems" /> method.  
    /// </para>
    /// </remarks>
    public MenuItem[] GetToggleGroupItems(string toggleGroupId)
    {
      ArrayList menuItemList = new ArrayList();
      GetToggleGroupItemsRecurse(toggleGroupId, false, this.Items, ref menuItemList);
      MenuItem[] menuItemArray = new MenuItem[menuItemList.Count];
      menuItemList.CopyTo(menuItemArray);
      return menuItemArray;
    }
    private void GetToggleGroupItemsRecurse(string toggleGroupId, bool checkedOnly, MenuItemCollection items, ref ArrayList menuItemList)
    {
      foreach (MenuItem item in items)
      {
        if ((item.ToggleGroupId == toggleGroupId) && (!checkedOnly || item.Checked))
        {
          menuItemList.Add(item);
        }
        GetToggleGroupItemsRecurse(toggleGroupId, checkedOnly, item.Items, ref menuItemList);
      }
    }

    /// <summary>
    /// Searches the entire item tree for an item with the given ID.
    /// </summary>
	/// <remarks>
    /// <para>
    /// This method accepts a string ID, returning the <see cref="MenuItem" /> with that ID. The entire
    /// <see cref="Menu" /> hierarchy is searched, enabling the retrieval of specific items without iterating
    /// through the various collections. 
    /// </para>
    /// </remarks>
    /// <param name="itemId">The identifier for the item to be found.</param>
    /// <returns>The specified menu item, or a null reference if it is not found.</returns>
		public new MenuItem FindItemById(string itemId)
		{
			return (MenuItem)base.FindNodeById(itemId);
		}

    public void ApplyTheming(bool? overwriteSettings)
    {
      bool overwrite = overwriteSettings ?? false;
      string prefix = this.AutoThemingCssClassPrefix ?? "";

      // Base
      if ((this.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        if (this.ContextMenu == ContextMenuType.None)
        {
          this.CssClass = prefix + "menu";
        }
        else
        {
          this.CssClass = prefix + "menu-group";
        }        
      }
      if ((this.DefaultGroupCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultGroupCssClass = prefix + "menu-group";
      }
      if ((this.DefaultItemLookId ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemLookId = "ThemedDefaultItemLook";
      }
      if (Properties["ShadowEnabled"] == null || overwrite)
      {
        Properties["ShadowEnabled"] = false.ToString();
      }
      if (this.DefaultGroupExpandOffsetY == 0 || overwrite)
      {
        this.DefaultGroupExpandOffsetY = -2;
      }
      if ((this.DefaultDisabledItemLookId ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultDisabledItemLookId = "ThemedDefaultDisabledItemLook";
      }
      
      // ItemLooks
      // Default
      ItemLook menuItemLook = new ItemLook();
      menuItemLook.LookId = "ThemedDefaultItemLook";

      if (this.ItemLooks.Count > 0)
      {
        foreach (ItemLook itemLook in this.ItemLooks)
        {
          if (itemLook.LookId == menuItemLook.LookId)
          {
            itemLook.CopyTo(menuItemLook);
            this.ItemLooks.Remove(itemLook);
            break;
          }
        }
      }

      if ((menuItemLook.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        menuItemLook.CssClass = prefix + "item-default";
      }
      if ((menuItemLook.HoverCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        menuItemLook.HoverCssClass = prefix + "item-hover";
      }
      if ((menuItemLook.ExpandedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        menuItemLook.ExpandedCssClass = prefix + "item-expanded";
      }
      if ((menuItemLook.ActiveCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        menuItemLook.ActiveCssClass = prefix + "item-active";
      }
      this.ItemLooks.Add(menuItemLook);

      // Disabled
      menuItemLook = new ItemLook();
      menuItemLook.LookId = "ThemedDefaultDisabledItemLook";

      if (this.ItemLooks.Count > 0)
      {
        foreach (ItemLook itemLook in this.ItemLooks)
        {
          if (itemLook.LookId == menuItemLook.LookId)
          {
            itemLook.CopyTo(menuItemLook);
            this.ItemLooks.Remove(itemLook);
            break;
          }
        }
      }

      if ((menuItemLook.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        menuItemLook.CssClass = prefix + "item-default " + prefix + "item-disabled";
      }
      if ((menuItemLook.HoverCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        menuItemLook.HoverCssClass = prefix + "item-hover " + prefix + "item-disabled";
      }
      if ((menuItemLook.ExpandedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        menuItemLook.ExpandedCssClass = prefix + "item-expanded " + prefix + "item-disabled";
      }
      if ((menuItemLook.ActiveCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        menuItemLook.ActiveCssClass = prefix + "item-active " + prefix + "item-disabled";
      }
      this.ItemLooks.Add(menuItemLook);

      // Client Templates
      StringBuilder templateText = new StringBuilder();
      templateText.Append("<div class=\"");
      templateText.Append(prefix);
      templateText.Append(@"menu-top-item"">
						<a href=""## DataItem.getProperty('NavigateUrl') == '' ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##"" onclick=""this.blur();"">
							<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"txt"">## DataItem.getProperty('Text'); ##</span>
						</a>
					</div>
");
      AddClientTemplate(overwrite, "TopLevelMenuItemTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"");
      templateText.Append(prefix);
      templateText.Append(@"menu-sub-item"">
						<a href=""## DataItem.getProperty('NavigateUrl') == '' ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##"" onclick=""this.blur();"">
							<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"outer"">
								<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"inner## DataItem.getProperty('IconCssClass') == null ? '' : ' ' +  DataItem.getProperty('IconCssClass'); ##"">## DataItem.getProperty('Text'); ##</span>
							</span>
						</a>
					</div>
");
      AddClientTemplate(overwrite, "SubLevelMenuItemTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"");
      templateText.Append(prefix);
      templateText.Append("menu-sub-item ");
      templateText.Append(prefix);
      templateText.Append(@"menu-sub-expandable"">
						<a href=""## DataItem.getProperty('NavigateUrl') == '' ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##"" onclick=""this.blur();"">
							<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"outer"">
								<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"inner## DataItem.getProperty('IconCssClass') == null ? '' : ' ' +  DataItem.getProperty('IconCssClass'); ##"">## DataItem.getProperty('Text'); ##</span>
							</span>
						</a>
					</div>
");
      AddClientTemplate(overwrite, "SubLevelMenuExpandableItemTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"");
      templateText.Append(prefix);
      templateText.Append(@"menu-sub-separator"">
						<div class=""");
      templateText.Append(prefix);
      templateText.Append(@"outer"">
							<div class=""");
      templateText.Append(prefix);
      templateText.Append(@"inner""></div>
						</div>
					</div>
");
      AddClientTemplate(overwrite, "SubLevelMenuSeparatorTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "menu-sub-item " + prefix + "menu-sub-item-left-icon\">");
      templateText.Append("<a href=\"## DataItem.getProperty('NavigateUrl') == '' ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##\" onclick=\"this.blur();\">");
      templateText.Append("<span class=\"" + prefix + "outer\">");
      templateText.Append("<span class=\"" + prefix + "inner\">");
      templateText.Append("<img src=\"" + Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl) + "## DataItem.getProperty('IconUrl'); ##\" width=\"16\" height=\"16\" border=\"0\" />");
      templateText.Append("<span class=\"" + prefix + "text\">## DataItem.getProperty('Text'); ##</span>");
      templateText.Append("</span></span></a></div>");
      AddClientTemplate(overwrite, "SubLevelMenuItemLeftIconTemplate", templateText.ToString());

      // Apply client templates to Items
      ApplyThemedClientTemplatesToItems(overwrite, this.Items);
    }

    private void ApplyThemedClientTemplatesToItems(bool overwrite, MenuItemCollection items)
    {
      foreach (MenuItem item in items)
      {
        if (item.ClientTemplateId == string.Empty || overwrite)
        {
          item.ClientTemplateId = "SubLevelMenuItemTemplate";
          // Hwan: 2010-01-06 Uncomment to make Theming smarter about blank items
          //if (item.Text == string.Empty && (item.nextSibling != null && item.previousSibling != null))
          if (item.Text == string.Empty)
          {
            item.ClientTemplateId = "SubLevelMenuSeparatorTemplate";
          }
          if (item.Items.Count > 0)
          {
            item.ClientTemplateId = "SubLevelMenuExpandableItemTemplate";
          }
          if (item.Depth < 1 && item.ParentMenu.ContextMenu == ContextMenuType.None)
          {
            item.ClientTemplateId = "TopLevelMenuItemTemplate";
          }

          if (item.Text == string.Empty && item.Depth < 1 && item.ParentMenu.ContextMenu == ContextMenuType.None)
          {
            item.ClientTemplateId = string.Empty;
          }
          if (item.Attributes["IconUrl"] != null)
          {
            item.ClientTemplateId = "SubLevelMenuItemLeftIconTemplate";
          }
        }
        ApplyThemedClientTemplatesToItems(overwrite, item.Items);
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
		/// Creates a new MenuItem and adds it as a root.
		/// </summary>
		/// <returns>The new node.</returns>
		protected override NavigationNode AddNode()
		{
			MenuItem newItem = new MenuItem();
			this.Items.Add(newItem);
			return newItem;
		}

    protected override NavigationNode NewNode()
    {
      MenuItem newNode = new MenuItem();
      MenuItemCollection dummy = newNode.Items; // This is a dummy call to ensure that newNode.nodes is not null
      return newNode;
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
        if(!Page.IsClientScriptBlockRegistered("A573W888.js"))
        {
          Page.RegisterClientScriptBlock("A573W888.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.Menu.client_scripts", "A573W888.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573Q288.js"))
        {
          Page.RegisterClientScriptBlock("A573Q288.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.Menu.client_scripts", "A573Q288.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573R388.js"))
        {
          Page.RegisterClientScriptBlock("A573R388.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.Menu.client_scripts", "A573R388.js");
        }
        }
      }

      // do we need default styles?
      if (this.RenderDefaultStyles)
      {
        // render them
        string sDefaultStyles = "<style>" + GetResourceContent("ComponentArt.Web.UI.Menu.defaultStyle.css") + "</style>";
        output.Write(sDefaultStyles);
      }


      if (this.IsBrowserSearchEngine() && this.RenderSearchEngineStructure || this.ForceSearchEngineStructure)
      {
        RenderCrawlerStructure(output);
      }

      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        if (this.ServerCalculateProperties)
        {
          this.ApplyLooks();
        }
        RenderAccessibleMenu(output);
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
        if (this.ServerCalculateProperties)
        {
          this.ApplyLooks();
        }

        if (this.AutoTheming)
        {
          this.ApplyTheming(false);
        }

        // Add menu data
        string menuClientVarName = this.GetSaneId();
        string itemStorageArrayId = "ComponentArt_Storage_" + menuClientVarName;
        string lookStorageArrayId = "ComponentArt_ItemLooks_" + menuClientVarName;
        string scrollLookStorageArrayId = "ComponentArt_ScrollLooks_" + menuClientVarName;

        string storage =
            "window." + itemStorageArrayId + "=" + this.BuildStorage().ToString() + ";\n"
          + "window." + lookStorageArrayId + "=" + this.BuildLooks().ToString() + ";\n"
          + "window." + scrollLookStorageArrayId + "=" + this.BuildScrollLooks().ToString() + ";\n";

        storage = this.DemarcateClientScript(storage, "ComponentArt Web.UI client-side storage for " + menuClientVarName);

        WriteStartupScript(output, storage);

        // Find preloadable images
        this.LoadPreloadImages();
        // Preload images, if any
        if (this.PreloadImages.Count > 0)
        {
          this.RenderPreloadImages(output);
        }

        // Render item server templates, if any
        foreach (Control oTemplate in this.Controls)
        {
          output.Write("<div id=\"" + oTemplate.ID + "\" style=\"display:none;\">");
          oTemplate.RenderControl(output);
          output.Write("</div>");
        }

        // Output the menu's associated element
        // For non-context menus this is a div tag that will house the top group.
        // For context menus, the tag is a dummy, and has style display:none set.
        output.Write("<div");
        output.WriteAttribute("id", menuClientVarName);
        if (this.ContextMenu == ContextMenuType.None)
        {
          if (this.ToolTip != string.Empty)
          {
            output.WriteAttribute("title", this.ToolTip);
          }
          if (!this.Height.IsEmpty || !this.Width.IsEmpty || this.Style.Count > 0)
          {
            output.Write(" style=\"");
            if (!this.Height.IsEmpty)
            {
              output.WriteStyleAttribute("height", this.Height.ToString());
            }
            if (!this.Width.IsEmpty)
            {
              output.WriteStyleAttribute("width", this.Width.ToString());
            }
            foreach (string sKey in this.Style.Keys)
            {
              output.WriteStyleAttribute(sKey, this.Style[sKey]);
            }
            output.Write("\"");
          }
        }
        else
        {
          output.Write(" style=\"display:none;\"");
        }
        output.Write(">");
        output.Write("</div>");

        if (this.EnableViewState)
        {
          // Render client-storage-persisting field.
          output.AddAttribute("id", menuClientVarName + "_Data");
          output.AddAttribute("name", menuClientVarName + "_Data");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render toplevel-property-persisting field.
          output.AddAttribute("id", menuClientVarName + "_Properties");
          output.AddAttribute("name", menuClientVarName + "_Properties");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render field to persist list of checked items.
          output.AddAttribute("id", menuClientVarName + "_CheckedItems");
          output.AddAttribute("name", menuClientVarName + "_CheckedItems");
          output.AddAttribute("type", "hidden");
          output.AddAttribute("value", JoinStringCollectionToString(NewCheckedPostBackIDs()));
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();
        }

        // Render the hidden field that will propagate ContextData.
        output.AddAttribute("id", menuClientVarName + "_ContextData");
        output.AddAttribute("name", menuClientVarName + "_ContextData");
        output.AddAttribute("type", "hidden");
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();

        #region Add menu initialization

        StringBuilder startupSB = new StringBuilder();
        startupSB.Append("window.ComponentArt_Init_" + menuClientVarName + " = function() {\n");

        // Include check for whether everything we need is loaded,
        // and a retry after a delay in case it isn't.
        string readyToInitializeMenu;
        int retryDelay;
        string areMenuScriptsLoaded = "(window.cart_menu_kernel_loaded && window.cart_menu_support_loaded)";
        if (this.ContextMenu == ContextMenuType.ControlSpecific)
        {
          string isContextControlLoaded = "(document.getElementById('" + this.ContextControlId + "') != null)";
          readyToInitializeMenu = "(" + areMenuScriptsLoaded + " && " + isContextControlLoaded + ")";
          retryDelay = 200; // Speed it up a little since we likely won't succed in loading on the first try
        }
        else
        {
          readyToInitializeMenu = areMenuScriptsLoaded;
          retryDelay = 500; // Use the standard 500 because we're probably succeeding on the first try
        }
        startupSB.Append("if (!" + readyToInitializeMenu + ")\n");
        startupSB.Append("{\n\tsetTimeout('ComponentArt_Init_" + menuClientVarName + "()', " + retryDelay.ToString() + ");\n\treturn;\n}\n");

        // Add the code to monitor the document.onmousedown event.
        /* Note that this is only needed by Context menus and menus that ExpandOnClick.
          * However, we could easily BECOME an ExpandOnClick menu on the client side, by simply
          * having our menuObject.ExpandOnClick property set to true.  Because of this, we run this 
          * hookup always, even for menus that don't really need it.  It's the easiest solution, 
          * and the cost of adding this client-side event handler is negligible. */
        startupSB.Append("if (!(window.cart_menu_documentmousedownhandled))\n");
        startupSB.Append("{\n");
        startupSB.Append("ComponentArt_AddEventHandler(document,'mousedown',function(event){ComponentArt_Menu_DocumentMouseDown(event);});\n");
        startupSB.Append("window.cart_menu_documentmousedownhandled = true;\n");
        startupSB.Append("}\n");

        if (this.ContextMenu != ContextMenuType.None)
        {
          // Context menus need to monitor the document.onmouseup event
          startupSB.Append("if (!(window.cart_menu_documentmouseuphandled))\n");
          startupSB.Append("{\n");
          startupSB.Append("ComponentArt_AddEventHandler(document,'mouseup',function(event){ComponentArt_Menu_DocumentMouseUp(event);});\n");
          startupSB.Append("window.cart_menu_documentmouseuphandled = true;\n");
          startupSB.Append("}\n");
        }

        // Instantiate menu object
        startupSB.Append("window." + menuClientVarName + " = new ComponentArt_Menu('" + menuClientVarName + "'," + itemStorageArrayId + "," + lookStorageArrayId + "," + scrollLookStorageArrayId + ",null," + this.ServerCalculateProperties.ToString().ToLower() + ");\n");

        // Write postback function reference
        if (Page != null)
        {
          startupSB.Append(menuClientVarName + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
        }

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != menuClientVarName)
        {
          startupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + menuClientVarName + "; " + menuClientVarName + ".GlobalAlias = '" + ID + "'; }\n");
        }

        // Define properties
        startupSB.Append(menuClientVarName + ".PropertyStorageArray = [\n");
        startupSB.Append("['ApplicationPath'," + Utils.ConvertStringToJSString(Context.Request.ApplicationPath) + "],");
        startupSB.Append("['AutoPostBackOnSelect'," + this.AutoPostBackOnSelect.ToString().ToLower() + "],");
        if (this.AutoTheming)
        {
          startupSB.Append("['AutoTheming',1],");
          startupSB.Append("['AutoThemingCssClassPrefix'," + Utils.ConvertStringToJSString(this.AutoThemingCssClassPrefix) + "],");
        }
        startupSB.Append("['AutoPostBackOnCheckChanged'," + this.AutoPostBackOnCheckChanged.ToString().ToLower() + "],");
        startupSB.Append("['BaseUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context, this.BaseUrl)) + "],");
        startupSB.Append("['CascadeCollapse'," + this.CascadeCollapse.ToString().ToLower() + "],");
        startupSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "],");
        startupSB.Append("['ClientSideOnContextMenuHide'," + Utils.ConvertStringToJSString(this.ClientSideOnContextMenuHide) + "],");
        startupSB.Append("['ClientSideOnContextMenuShow'," + Utils.ConvertStringToJSString(this.ClientSideOnContextMenuShow) + "],");
        startupSB.Append("['ClientSideOnItemMouseOut'," + Utils.ConvertStringToJSString(this.ClientSideOnItemMouseOut) + "],");
        startupSB.Append("['ClientSideOnItemMouseOver'," + Utils.ConvertStringToJSString(this.ClientSideOnItemMouseOver) + "],");
        startupSB.Append("['ClientSideOnItemSelect'," + Utils.ConvertStringToJSString(this.ClientSideOnItemSelect) + "],");
        startupSB.Append("['ClientTemplates'," + this._clientTemplates.ToString() + "],");
        startupSB.Append("['CollapseDelay'," + this.CollapseDelay + "],");
        startupSB.Append("['CollapseDuration'," + this.CollapseDuration + "],");
        startupSB.Append("['CollapseSlide'," + (int)this.CollapseSlide + "],");
        startupSB.Append("['CollapseTransition'," + (int)this.CollapseTransition + "],");
        startupSB.Append("['CollapseTransitionCustomFilter'," + Utils.ConvertStringToJSString(this.CollapseTransitionCustomFilter) + "],");
        startupSB.Append("['ContextControlId'," + Utils.ConvertStringToJSString(this.ContextControlId) + "],");
        startupSB.Append("['ContextData',null],");
        startupSB.Append("['ContextMenu'," + (int)this.ContextMenu + "],");
        startupSB.Append("['ControlId'," + Utils.ConvertStringToJSString(this.UniqueID) + "],");
        startupSB.Append("['CssClass'," + Utils.ConvertStringToJSString(this.CssClass == string.Empty ? this.DefaultGroupCssClass : this.CssClass) + "],");
        startupSB.Append("['DefaultChildSelectedItemLookId'," + Utils.ConvertStringToJSString(this.DefaultChildSelectedItemLookId) + "],");
        startupSB.Append("['DefaultDisabledItemLookId'," + Utils.ConvertStringToJSString(this.DefaultDisabledItemLookId) + "],");
        startupSB.Append("['DefaultGroupCssClass'," + Utils.ConvertStringToJSString(this.DefaultGroupCssClass) + "],");
        startupSB.Append("['DefaultGroupExpandDirection'," + (int)this.DefaultGroupExpandDirection + "],");
        startupSB.Append("['DefaultGroupExpandOffsetX'," + this.DefaultGroupExpandOffsetX + "],");
        startupSB.Append("['DefaultGroupExpandOffsetY'," + this.DefaultGroupExpandOffsetY + "],");
        startupSB.Append("['DefaultGroupHeight'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupHeight) + "],");
        startupSB.Append("['DefaultGroupItemSpacing'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupItemSpacing) + "],");
        startupSB.Append("['DefaultGroupOrientation'," + (int)this.DefaultGroupOrientation + "],");
        startupSB.Append("['DefaultGroupWidth'," + Utils.ConvertUnitToJSConstant(this.DefaultGroupWidth) + "],");
        startupSB.Append("['DefaultItemLookId'," + Utils.ConvertStringToJSString(this.DefaultItemLookId) + "],");
        startupSB.Append("['DefaultItemTextAlign'," + (int)this.DefaultItemTextAlign + "],");
        startupSB.Append("['DefaultItemTextWrap'," + this.DefaultItemTextWrap.ToString().ToLower() + "],");
        startupSB.Append("['DefaultSelectedItemLookId'," + Utils.ConvertStringToJSString(this.DefaultSelectedItemLookId) + "],");
        startupSB.Append("['DefaultTarget'," + Utils.ConvertStringToJSString(this.DefaultTarget) + "],");
        startupSB.Append("['ExpandDelay'," + this.ExpandDelay + "],");
        startupSB.Append("['ExpandDisabledItems'," + this.ExpandDisabledItems.ToString().ToLower() + "],");
        startupSB.Append("['ExpandDuration'," + this.ExpandDuration + "],");
        startupSB.Append("['ExpandedOverridesHover'," + this.ExpandedOverridesHover.ToString().ToLower() + "],");
        startupSB.Append("['ExpandOnClick'," + this.ExpandOnClick.ToString().ToLower() + "],");
        startupSB.Append("['ExpandSlide'," + (int)this.ExpandSlide + "],");
        startupSB.Append("['ExpandTransition'," + (int)this.ExpandTransition + "],");
        startupSB.Append("['ExpandTransitionCustomFilter'," + Utils.ConvertStringToJSString(this.ExpandTransitionCustomFilter) + "],");
        startupSB.Append("['ForceHighlightedItemID'," + Utils.ConvertStringToJSString(this.ForceHighlightedItemID) + "],");
        startupSB.Append("['Height'," + Utils.ConvertUnitToJSConstant(this.Height == Unit.Empty ? this.DefaultGroupHeight : this.Height) + "],");
        startupSB.Append("['HideSelectElements'," + this.HideSelectElements.ToString().ToLower() + "],");
        startupSB.Append("['HighlightExpandedPath'," + this.HighlightExpandedPath.ToString().ToLower() + "],");
        startupSB.Append("['ImagesBaseUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl)) + "],");
        startupSB.Append("['MultiPageId'," + Utils.ConvertStringToJSString(this._multiPageId) + "],");
        startupSB.Append("['Orientation'," + (int)this.Orientation + "],");
        startupSB.Append("['OverlayWindowedElements'," + this.OverlayWindowedElements.ToString().ToLower() + "],");
        startupSB.Append("['PopUpZIndexBase'," + this.PopUpZIndexBase + "],");        
        startupSB.Append("['PlaceHolderId'," + Utils.ConvertStringToJSString(menuClientVarName) + "],");
        startupSB.Append("['ScrollingEnabled'," + this.ScrollingEnabled.ToString().ToLower() + "],");
        startupSB.Append("['SelectedItemPostBackID'," + Utils.ConvertStringToJSString(this.SelectedItem != null ? this.SelectedItem.PostBackID : null) + "],");
        startupSB.Append("['ShadowColor','" + ColorTranslator.ToHtml(this.ShadowColor) + "'],");
        startupSB.Append("['ShadowEnabled'," + this.ShadowEnabled.ToString().ToLower() + "],");
        startupSB.Append("['ShadowOffset'," + ShadowOffset.ToString() + "],");
        startupSB.Append("['SoaService','" + this.SoaService + "'],");        
        startupSB.Append("['TopGroupExpandDirection'," + (int)this.TopGroupExpandDirection + "],");
        startupSB.Append("['TopGroupExpandOffsetX'," + this.TopGroupExpandOffsetX + "],");
        startupSB.Append("['TopGroupExpandOffsetY'," + this.TopGroupExpandOffsetY + "],");
        startupSB.Append("['TopGroupItemSpacing'," + Utils.ConvertUnitToJSConstant(this.TopGroupItemSpacing) + "],");
        startupSB.Append("['WebService','" + this.WebService + "'],");
        startupSB.Append("['WebServiceCustomParameter','" + this.WebServiceCustomParameter + "'],");
        startupSB.Append("['WebServiceMethod','" + this.WebServiceMethod + "'],");
        startupSB.Append("['TopGroupExpandOffsetY'," + this.TopGroupExpandOffsetY + "],");
        startupSB.Append("['Width'," + Utils.ConvertUnitToJSConstant(this.Width == Unit.Empty ? this.DefaultGroupWidth : this.Width) + "]\n];\n");
        startupSB.Append(menuClientVarName + ".LoadProperties();\n");

        if (!this.ServerCalculateProperties)
        {
          startupSB.Append("ComponentArt_Menu_MarkSelectedItem(" + menuClientVarName + ");\n");
          startupSB.Append("ComponentArt_Menu_MarkForceHighlightedItem(" + menuClientVarName + ");\n");
        }

        if (this.EnableViewState && !this.ServerCalculateProperties)
        {
          // add us to the client viewstate-saving mechanism
          startupSB.Append("ComponentArt_ClientStateControls[ComponentArt_ClientStateControls.length] = " + menuClientVarName + ";\n");
        }

        // Initialize the menu
        startupSB.Append(menuClientVarName + ".Initialize();\n");

        // Render the menu
        startupSB.Append("ComponentArt_Menu_RenderMenu(" + menuClientVarName + ");\n");

        // Keyboard
        if (this.KeyboardEnabled)
        {
          // Initialize keyboard
          startupSB.Append("ComponentArt_Menu_InitKeyboard(" + menuClientVarName + ");\n");

          // Create client script to register keyboard shortcuts
          StringBuilder oKeyboardSB = new StringBuilder();
          GenerateKeyShortcutScript(menuClientVarName, this.Items, oKeyboardSB);
          startupSB.Append(oKeyboardSB.ToString());
        }

        // Set the flag that the menu has been initialized.  This is the last action in the menu initialization.
        startupSB.Append("window." + menuClientVarName + "_loaded = true;\n}\n");

        // Call this initialization function.  Remember that it will be repeated after a delay if it's not all ready.
        startupSB.Append("ComponentArt_Init_" + menuClientVarName + "();");

        WriteStartupScript(output, this.DemarcateClientScript(startupSB.ToString(), "ComponentArt_Menu_Startup_" + menuClientVarName + " " + this.VersionString()));

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

			MenuItem oItem = (MenuItem)(this.FindNodeByPostBackId(sPostBackId, this.Items));

			if (oItem == null)
			{
				throw new Exception("Item " + sPostBackId + " not found.");
			}

			// should we validate the page?
			if (Utils.ConvertInheritBoolToBool(oItem.CausesValidation, this.CausesValidation))
			{
				Page.Validate();
			}

			MenuItemEventArgs oArgs = new MenuItemEventArgs();
			oArgs.Command = sCommand;
			oArgs.Item = oItem;

			switch (sCommand)
			{
				case "SELECT":
					this.selectedNode = oArgs.Item;
					this.OnItemSelected(oArgs);
					// If the selected node has a navurl, redirect to it.
					if(oArgs.Item.NavigateUrl != string.Empty)
					{
						oArgs.Item.Navigate();
					}
					break;

				default:
					throw new Exception("Unknown postback command: \"" + sCommand + "\"");
			}
		}

		protected override bool IsDownLevel()
    {
      if (Context == null || Page == null) return true;
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

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad (e);

      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573S188.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573Z388.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Menu.client_scripts.A573W888.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Menu.client_scripts.A573Q288.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Menu.client_scripts.A573R388.js");
      }

      // do we need Accessible styles?
      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        HtmlLink link = new HtmlLink();
        link.Attributes.Add("type", "text/css");
        link.Attributes.Add("rel", "stylesheet");
        link.Attributes.Add("href", Page.ClientScript.GetWebResourceUrl(this.GetType(), "ComponentArt.Web.UI.Menu.accessibleStyle.css"));
        this.Page.Header.Controls.Add(link);
      }

      // Raise OnItemCheckChanged events
      string oldCheckedPostBackIDs = Context.Request.Form[this.GetSaneId() + "_CheckedItems"];
      if (oldCheckedPostBackIDs != null)
      {
        StringCollection oldChecked = SplitStringToStringCollection(oldCheckedPostBackIDs);
        StringCollection newChecked = NewCheckedPostBackIDs();
        ArrayList checkChangedItems = new ArrayList();
        foreach (String postbackID in oldChecked)
        {
          if (!newChecked.Contains(postbackID))
          {
            MenuItem checkChangedItem = (MenuItem)this.FindNodeByPostBackId(postbackID, this.Items);
            if (checkChangedItem != null)
            {
              checkChangedItems.Add(checkChangedItem);
            }
          }
        }
        foreach (String postbackID in newChecked)
        {
          if (!oldChecked.Contains(postbackID))
          {
            MenuItem checkChangedItem = (MenuItem)this.FindNodeByPostBackId(postbackID, this.Items);
            if (checkChangedItem != null)
            {
              checkChangedItems.Add(checkChangedItem);
            }
          }
        }
        foreach (MenuItem item in checkChangedItems)
        {
          MenuItemCheckChangedEventArgs args = new MenuItemCheckChangedEventArgs(item);
          this.OnItemCheckChanged(args);
        }
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

		private JavaScriptArray BuildStorage()
		{
			// Recursively populate a list of data of all menu items that will make up the menu:
			JavaScriptArray itemStorageArray = new JavaScriptArray();
			foreach (MenuItem item in this.GetStartGroupItems())
			{
				BuildItemStorage(item, itemStorageArray, -1, 1);
			}
			return itemStorageArray;
		}

    /// <summary>
    /// Go through the ItemLooks, and build a javascript array representing their data.
    /// </summary>
    private JavaScriptArray BuildLooks()
    {
      JavaScriptArray renderLookList = new JavaScriptArray();
      foreach (ItemLook look in this.ItemLooks)
      {
        renderLookList.Add(ProcessLook(look));
      }
      return renderLookList;
    }

    /// <summary>
    /// Returns a string representation of a two-element javascript array with ScrollDownLook and ScrollUpLook
    /// </summary>
    private JavaScriptArray BuildScrollLooks()
    {
      JavaScriptArray scrollLookList = new JavaScriptArray();
      scrollLookList.Add(this.ProcessScrollLook(this.ScrollDownLook, this.ScrollDownLookId));
      scrollLookList.Add(this.ProcessScrollLook(this.ScrollUpLook, this.ScrollUpLookId));
      return scrollLookList;
    }
    
		/// <summary>
		/// Go through the Menu nodes, determining if default styles are needed anywhere, and if so, apply them.
		/// Returns whether any default styles were applied.
		/// </summary>
		private bool ConsiderDefaultStylesRecurse(MenuItemCollection arItems)
		{
			bool bNeedDefaults = false;

			foreach(MenuItem oItem in arItems)
			{
				// is this item in need of default styles?
				if( this.CssClass == string.Empty && (this.DefaultGroupCssClass == null || this.DefaultGroupCssClass == string.Empty) &&
					oItem.Look.CssClass == null && oItem.Look.ImageUrl == null && (oItem.ParentItem == null || oItem.ParentItem._defaultStyle || oItem.ParentItem.SubGroupCssClass == null || oItem.ParentItem.SubGroupCssClass == string.Empty) )
				{
					bNeedDefaults = true;

					oItem._defaultStyle = true;

					// apply default styles to this item
					if(oItem.parentNode == null)
					{
						if(!this.IsDownLevel())
						{
							oItem.Look.CssClass = "cm_TopItem";
							oItem.Look.HoverCssClass = "cm_TopItemHover";
							oItem.Look.ActiveCssClass = "cm_TopItemActive";
							oItem.Look.ExpandedCssClass="cm_TopItemActive";
						}
						else
						{
							oItem.Attributes.Add("style", "background-color:#3F3F3F;color:white;font-family:verdana;font-size:12px;border:1px;border-color:#3F3F3F;border-style:solid;cursor:pointer;");
						}
						oItem.Look.LabelPaddingLeft = 10;
						oItem.Look.LabelPaddingRight = 10;
						oItem.Look.LabelPaddingTop = 2;
						oItem.Look.LabelPaddingBottom = 2;
					}
					else
					{
						if(!this.IsDownLevel())
						{
							oItem.ParentItem.SubGroupCssClass = "cm_Group";
							oItem.Look.CssClass = "cm_Item";
							oItem.Look.HoverCssClass = "cm_ItemHover";
							oItem.Look.ActiveCssClass = "cm_ItemActive";
							oItem.Look.ExpandedCssClass = "cm_ItemActive";
						}
						else
						{
							oItem.Attributes.Add("style", "background-color:#EEEEEE;color:#333333;font-family:verdana;font-size:11px;border:solid 1px #EEEEEE;border-style:solid;cursor:pointer;");
						}
						oItem.Look.LabelPaddingLeft = 10;
						oItem.Look.LabelPaddingRight = 10;
						oItem.Look.LabelPaddingTop = 2;
						oItem.Look.LabelPaddingBottom = 2;
					}
				}

				if(oItem.nodes != null)
				{
					bNeedDefaults = ConsiderDefaultStylesRecurse(oItem.Items) || bNeedDefaults;
				}
			}

			return bNeedDefaults;
		}

		/// <summary>
		/// Consider using default styles
		/// </summary>
		internal override bool ConsiderDefaultStyles()
		{
      if (this.AutoTheming)
      {
        return false;
      }
      if(!this.RenderDefaultStyles && this.DefaultItemLook != null && !this.DefaultItemLook.IsEmpty)
      {
		    return false;
		  }
			if(ConsiderDefaultStylesRecurse(this.Items) | this.RenderDefaultStyles) /* purposely not using short-circuit OR */ 
			{
        // create default looks for scrolls
        if (this.ScrollUpLook.IsEmpty || this.ScrollDownLook.IsEmpty)
        {
          ItemLook ScrollLook = new ItemLook();
          ScrollLook.CssClass = "cm_Item";
          ScrollLook.HoverCssClass = "cm_ItemHover";
          ScrollLook.ActiveCssClass = "cm_ItemActive";
          ScrollLook.LookId = "_s";
          this.ItemLooks.Add(ScrollLook);
          this.ScrollUpLookId = this.ScrollDownLookId = "_s";
        }
				// register default styles
				if(!this.IsDownLevel())
				{
					// apply styles to the control
					this.CssClass = "cm_TopGroup";
				}
				else
				{
					this.Attributes.Add("style", "background-color:#3F3F3F;border:1px;border-color:black;border-top-color:gray;border-left-color:gray;border-style:solid;");
				}
        return true;
			}
      return false;
		}

		private void GenerateKeyShortcutScript(string sMenuName, MenuItemCollection arItemList, StringBuilder oSB)
		{
      if(arItemList != null)
      {
        foreach(MenuItem oItem in arItemList)
        {
          if(oItem.KeyboardShortcut != string.Empty)
          {
            oSB.Append("ComponentArt_RegisterKeyHandler(" + sMenuName + ",'" + oItem.KeyboardShortcut + "','" + sMenuName + ".SelectItemByPostBackId(\\'" + oItem.PostBackID + "\\')'" + ");\n");
          }
          GenerateKeyShortcutScript(sMenuName, oItem.Items, oSB);
        }
      }
		}

		private MenuItemCollection GetStartGroupItems()
		{
			MenuItemCollection startGroupItems;
			if(this.RenderRootItemId != null && this.RenderRootItemId != string.Empty)
			{
				MenuItem rootItem = this.FindItemById(this.RenderRootItemId);
				if (rootItem != null)
				{
					startGroupItems = rootItem.Items;
				}
				else
				{
					throw new Exception("No item found with ID \"" + this.RenderRootNodeId + "\".");
				}
			}
			else
			{
				startGroupItems = this.Items;
			}
			return startGroupItems;
		}

    private ArrayList GetCheckedItems()
    {
      ArrayList checkedItems = new ArrayList();
      GetCheckedItemsRecurse(this.Items, ref checkedItems);
      return checkedItems;
    }
    private void GetCheckedItemsRecurse(MenuItemCollection items, ref ArrayList checkedItems)
    {
      foreach (MenuItem item in items)
      {
        if (item.Checked)
        {
          checkedItems.Add(item);
        }
        GetCheckedItemsRecurse(item.Items, ref checkedItems);
      }
    }

    private StringCollection NewCheckedPostBackIDs()
    {
      StringCollection checkedPostBackIDs = new StringCollection();
      ArrayList checkedItems = GetCheckedItems();
      foreach (MenuItem checkedItem in checkedItems)
      {
        if (checkedItem.PostBackID != null && checkedItem.PostBackID != String.Empty)
        {
          checkedPostBackIDs.Add(checkedItem.PostBackID);
        }
      }
      return checkedPostBackIDs;
    }

    private string JoinStringCollectionToString(StringCollection sc)
    {
      StringBuilder sb = new StringBuilder();
      foreach (String s in sc)
      {
        sb.Append(s).Append(";");
      }
      if (sb.Length > 0)
      {
        sb.Length -= 1; // Remove the last semicolon.
      }
      return sb.ToString();
    }

    private StringCollection SplitStringToStringCollection(string s)
    {
      String[] sa = s.Split(';');
      StringCollection sc = new StringCollection();
      foreach (string token in sa)
      {
        sc.Add(token);
      }
      return sc;
    }

    /// <summary>
    /// Generates the array of item properties that are to be propagated to the client side.
    /// </summary>
    /// <param name="item">The item that is being processed.</param>
    /// <param name="items">Array of item property arrays. Item property array of the current item gets appended to it.</param>
    /// <param name="parentIndex">Client-side storage index of the parent item of the currently processed item.</param>
    /// <param name="depth">Depth at which we are currently processing. Needed to know when to stop if RenderDrillDownDepth is
    /// significant.</param>
    /// <returns>Client-side storage index of the item that has just been processed.</returns>
    /// <remarks>This is a recursive method which will also process all of the item's child items.<para>
    /// The order of the ArrayList elements must match the order of their client-side equivalents in the menu properties array 
    /// in A573W888.js.</para></remarks>
		private int BuildItemStorage(MenuItem item, ArrayList items, int parentIndex, int depth)
		{
			ArrayList itemData = new ArrayList();
			int itemIndex = items.Count;
			items.Add(itemData);

      itemData.Add(item.PostBackID); // 'PostBackID'
			itemData.Add(parentIndex); // 'ParentIndex'
      
			ArrayList childIndexes = new ArrayList();
			if (item.nodes != null && this.RenderDrillDownDepth == 0 || this.RenderDrillDownDepth > depth)
			{
				foreach (MenuItem childItem in item.Items)
				{
					childIndexes.Add(BuildItemStorage(childItem, items, itemIndex, depth + 1)); // Note the recursion
				}
			}
      itemData.Add(childIndexes); // 'ChildIndexes'
      
      if (this.ServerCalculateProperties) 
      /* Classic item storage.  Properties calculated on the server.
       * Slightly worse server-side performance.  Improved client-side performance, especially for extremely large item structures.
       * Limited client-side capabilities, because of limited client-to-server persistence. */
      {
        
        itemData.Add(item.Visible); // 'Visible'
        itemData.Add(item.Enabled); // 'Enabled'
        itemData.Add((int)item.TextAlign); // 'TextAlign'
        itemData.Add(item.TextWrap); // 'TextWrap'
        itemData.Add(item.AutoPostBackOnSelect); // 'AutoPostBackOnSelect'
        itemData.Add(item.ID); // 'ID'
        itemData.Add(item.NavigateUrl); // 'NavigateUrl'
        itemData.Add(item.Target); // 'Target'
        itemData.Add(item.ClientSideCommand); // 'ClientSideCommand'
        itemData.Add(item.Text); // 'Text'

        ItemLook effectiveLook = item.EffectiveLook; // The look settings that are in effect for this item.
        ItemLook originalLook = this.ItemLooks[effectiveLook.LookId]; // The original look from which this item's look is derived (if any).
        ItemLook significantLook = base.LookDifference(effectiveLook, originalLook); // The look containing only the differences between the two.

        itemData.Add(significantLook.LookId); // 'LookId'
        itemData.Add(significantLook.CssClass); // 'CssClass'
        itemData.Add(significantLook.HoverCssClass); // 'HoverCssClass'
        itemData.Add(item.Width); // 'Width'
        itemData.Add(item.Height); // 'Height'
        itemData.Add(item.ToggleGroupId); // 'ToggleGroupId'
        itemData.Add(item.ToggleType); // 'ToggleType'
        itemData.Add(item.Checked); // 'Checked'
        itemData.Add(significantLook.LabelPaddingBottom); // 'LabelPaddingBottom'
        itemData.Add(significantLook.LabelPaddingLeft); // 'LabelPaddingLeft'
        itemData.Add(significantLook.LabelPaddingRight); // 'LabelPaddingRight'
        itemData.Add(significantLook.LabelPaddingTop); // 'LabelPaddingTop'
        itemData.Add(significantLook.ActiveCssClass); // 'ActiveCssClass'
        itemData.Add(significantLook.LeftIconUrl); // 'LeftIconUrl'
        itemData.Add(significantLook.HoverLeftIconUrl); // 'HoverLeftIconUrl'
        itemData.Add(significantLook.LeftIconWidth); // 'LeftIconWidth'
        itemData.Add(significantLook.LeftIconHeight); // 'LeftIconHeight'
        itemData.Add(significantLook.ActiveLeftIconUrl); // 'ActiveLeftIconUrl'
        itemData.Add(significantLook.RightIconUrl); // 'RightIconUrl'
        itemData.Add(significantLook.HoverRightIconUrl); // 'HoverRightIconUrl'
        itemData.Add(significantLook.RightIconWidth); // 'RightIconWidth'
        itemData.Add(significantLook.RightIconHeight); // 'RightIconHeight'
        itemData.Add(significantLook.ActiveRightIconUrl); // 'ActiveRightIconUrl'
        itemData.Add(significantLook.RightIconVisibility); // 'RightIconVisibility'
        itemData.Add(significantLook.LeftIconVisibility); // 'LeftIconVisibility'
        itemData.Add(significantLook.ImageUrl); // 'ImageUrl'
        itemData.Add(significantLook.HoverImageUrl); // 'HoverImageUrl'
        itemData.Add(significantLook.ImageWidth); // 'ImageWidth'
        itemData.Add(significantLook.ImageHeight); // 'ImageHeight'
        itemData.Add(significantLook.ActiveImageUrl); // 'ActiveImageUrl'

        if (childIndexes.Count > 0)  // relevance of some properties depends on whether the item has children
        {
          /* The following properties are only relevant for items which have subitems. */
          itemData.Add((int)item.SubGroupExpandDirection); // 'SubGroupExpandDirection'
          itemData.Add((int)item.SubGroupOrientation); // 'SubGroupOrientation'
          itemData.Add(item.SubGroupExpandOffsetX); // 'SubGroupExpandOffsetX'
          itemData.Add(item.SubGroupExpandOffsetY); // 'SubGroupExpandOffsetY'
          itemData.Add(item.SubGroupCssClass); // 'SubGroupCssClass'
          itemData.Add(item.SubGroupItemSpacing); // 'SubGroupItemSpacing'
          itemData.Add(item.SubGroupWidth); // 'SubGroupWidth'
          itemData.Add(item.SubGroupHeight); // 'SubGroupHeight'
          itemData.Add(significantLook.ExpandedCssClass); // 'ExpandedCssClass'
          itemData.Add(significantLook.ExpandedLeftIconUrl); // 'ExpandedLeftIconUrl'
          itemData.Add(significantLook.ExpandedRightIconUrl); // 'ExpandedRightIconUrl'
          itemData.Add(significantLook.ExpandedImageUrl); // 'ExpandedImageUrl'
        }
        else
        {
          /* For childless items, we have to insert a null instead of each of the non-applicable properties. */
          const int parentItemPropertyCount = 12; // The number of properties added in the if block above
          itemData.AddRange(ArrayList.Repeat(null, parentItemPropertyCount));
        }
			
        itemData.Add(item.KeyboardShortcut); // 'KeyboardShortcut'
        itemData.Add(item.ToolTip); // 'ToolTip'
        itemData.Add(item.Value); // 'Value'
        itemData.Add((item.TemplateId == null || item.TemplateId == "") ? null : this.ClientID+"_"+item.PostBackID); // 'TemplateInstanceId'
        itemData.Add(item.PageViewId); // 'PageViewId'
        itemData.Add(item.ServerTemplateId); // 'ServerTemplateId'
      }
      else
      /* Modern item storage.  Properties calculated on the client.
       * Slightly better server-side performance.  Slower client-side performance, especially for extremely large item structures.
       * Full client-side capabilities, including full server-to-client-to-server persistence. */
      {
      
        ArrayList itemProperties = new ArrayList();
      
        foreach (string propertyKeyName in item.Properties.Keys)
        {
          string propertyName = item.GetVarAttributeName(propertyKeyName);
          string propertyNameLowerCase = propertyName.ToLower(System.Globalization.CultureInfo.InvariantCulture);
          switch (propertyNameLowerCase)
          {
            case "autopostbackonselect": itemProperties.Add(new object[] { 0, item.AutoPostBackOnSelect }); break;
            case "causesvalidation": itemProperties.Add(new object[] { 1, item.CausesValidation }); break;
            case "clientsidecommand": itemProperties.Add(new object[] { 2, item.ClientSideCommand }); break;
            case "defaultsubgroupcssclass": itemProperties.Add(new object[] { 3, item.DefaultSubGroupCssClass }); break;
            case "defaultsubgroupexpanddirection": itemProperties.Add(new object[] { 4, item.DefaultSubGroupExpandDirection }); break;
            case "defaultsubgroupexpandoffsetx": itemProperties.Add(new object[] { 5, item.DefaultSubGroupExpandOffsetX }); break;
            case "defaultsubgroupexpandoffsety": itemProperties.Add(new object[] { 6, item.DefaultSubGroupExpandOffsetY }); break;
            case "defaultsubgroupheight": itemProperties.Add(new object[] { 7, item.DefaultSubGroupHeight }); break;
            case "defaultsubgroupitemspacing": itemProperties.Add(new object[] { 8, item.DefaultSubGroupItemSpacing }); break;
            case "defaultsubgrouporientation": itemProperties.Add(new object[] { 9, item.DefaultSubGroupOrientation }); break;
            case "defaultsubgroupwidth": itemProperties.Add(new object[] { 10, item.DefaultSubGroupWidth }); break;
            case "defaultsubitemtextalign": itemProperties.Add(new object[] { 11, item.DefaultSubItemTextAlign }); break;
            case "defaultsubitemtextwrap": itemProperties.Add(new object[] { 12, item.DefaultSubItemTextWrap }); break;
            case "enabled": itemProperties.Add(new object[] { 13, item.Enabled }); break;
            case "height": itemProperties.Add(new object[] { 14, item.Height }); break;
            case "id": itemProperties.Add(new object[] { 15, item.ID }); break;
            case "keyboardshortcut": itemProperties.Add(new object[] { 16, item.KeyboardShortcut }); break;
            case "navigateurl": itemProperties.Add(new object[] { 17, Utils.MakeStringXmlSafe(item.NavigateUrl) }); break;
            case "pageviewid": itemProperties.Add(new object[] { 18, item.PageViewId }); break;
            case "sitemapxmlfile": itemProperties.Add(new object[] { 19, item.SiteMapXmlFile }); break;
            case "subgroupcssclass": itemProperties.Add(new object[] { 20, item.SubGroupCssClass }); break;
            case "subgroupexpanddirection": itemProperties.Add(new object[] { 21, item.SubGroupExpandDirection }); break;
            case "subgroupexpandoffsetx": itemProperties.Add(new object[] { 22, item.SubGroupExpandOffsetX }); break;
            case "subgroupexpandoffsety": itemProperties.Add(new object[] { 23, item.SubGroupExpandOffsetY }); break;
            case "subgroupheight": itemProperties.Add(new object[] { 24, item.SubGroupHeight }); break;
            case "subgroupitemspacing": itemProperties.Add(new object[] { 25, item.SubGroupItemSpacing }); break;
            case "subgrouporientation": itemProperties.Add(new object[] { 26, item.SubGroupOrientation }); break;
            case "subgroupwidth": itemProperties.Add(new object[] { 27, item.SubGroupWidth }); break;
            case "target": itemProperties.Add(new object[] { 28, item.Target }); break;
            case "servertemplateid": itemProperties.Add(new object[] { 29, item.ServerTemplateId }); break;
            case "text": itemProperties.Add(new object[] { 30, Utils.MakeStringXmlSafe(item.Text) }); break;
            case "textalign": itemProperties.Add(new object[] { 31, item.TextAlign }); break;
            case "toggletype": itemProperties.Add(new object[] { 32, item.ToggleType }); break;
            case "togglegroupid": itemProperties.Add(new object[] { 33, Utils.MakeStringXmlSafe(item.ToggleGroupId) }); break;
            case "checked": itemProperties.Add(new object[] { 34, item.Checked }); break;
            case "textwrap": itemProperties.Add(new object[] { 35, item.TextWrap }); break;
            case "tooltip": itemProperties.Add(new object[] { 36, Utils.MakeStringXmlSafe(item.ToolTip) }); break;
            case "value": itemProperties.Add(new object[] { 37, Utils.MakeStringXmlSafe(item.Value) }); break;
            case "visible": itemProperties.Add(new object[] { 38, item.Visible }); break;
            case "width": itemProperties.Add(new object[] { 39, item.Width }); break;
            case "lookid": itemProperties.Add(new object[] { 40, item.LookId }); break;
            case "disabledlookid": itemProperties.Add(new object[] { 41, item.DisabledLookId }); break;
            case "selectedlookid": itemProperties.Add(new object[] { 42, item.SelectedLookId }); break;
            case "childselectedlookid": itemProperties.Add(new object[] { 43, item.ChildSelectedLookId }); break;
            case "defaultsubitemlookid": itemProperties.Add(new object[] { 44, item.DefaultSubItemLookId }); break;
            case "defaultsubitemdisabledlookid": itemProperties.Add(new object[] { 45, item.DefaultSubItemDisabledLookId }); break;
            case "defaultsubitemselectedlookid": itemProperties.Add(new object[] { 46, item.DefaultSubItemSelectedLookId }); break;
            case "defaultsubitemchildselectedlookid": itemProperties.Add(new object[] { 47, item.DefaultSubItemChildSelectedLookId }); break;
            case "clienttemplateid": itemProperties.Add(new object[] { 48, item.ClientTemplateId }); break;

            // Up next: look properties which are not of string type:
            case "look-lefticonvisibility": itemProperties.Add(new object[] { "Look-LeftIconVisibility", item.Look.LeftIconVisibility }); break;
            case "look-righticonvisibility": itemProperties.Add(new object[] { "Look-RightIconVisibility", item.Look.RightIconVisibility }); break;
            case "selectedlook-lefticonvisibility": itemProperties.Add(new object[] { "SelectedLook-LeftIconVisibility", item.SelectedLook.LeftIconVisibility }); break;
            case "selectedlook-righticonvisibility": itemProperties.Add(new object[] { "SelectedLook-RightIconVisibility", item.SelectedLook.RightIconVisibility }); break;
            case "childselectedlook-lefticonvisibility": itemProperties.Add(new object[] { "ChildSelectedLook-LeftIconVisibility", item.ChildSelectedLook.LeftIconVisibility }); break;
            case "childselectedlook-righticonvisibility": itemProperties.Add(new object[] { "ChildSelectedLook-RightIconVisibility", item.ChildSelectedLook.RightIconVisibility }); break;
            case "disabledlook-lefticonvisibility": itemProperties.Add(new object[] { "DisabledLook-LeftIconVisibility", item.DisabledLook.LeftIconVisibility }); break;
            case "disabledlook-righticonvisibility": itemProperties.Add(new object[] { "DisabledLook-RightIconVisibility", item.DisabledLook.RightIconVisibility }); break;
            //HACK: The rest of the look properties are handled like custom properties.  We can do this because they're strings.
            //There is a bug - other look properties will not work when OutputCustomAttributes is false.  But this is so obscure it isn't worth fixing yet.

            default:
              if (this.OutputCustomAttributes)
              {
                // This is a custom property.  Treat it as a potentially unsafe string.
                itemProperties.Add(new object[] { propertyName, Utils.MakeStringXmlSafe(item.Properties[propertyKeyName]) });
              }
              break;
          }
        }
        
        itemData.Add(itemProperties);
      
      }
      
			return itemIndex;
		}

    /// <summary>
    /// Generates the array of look properties that are to be propagated to the client side.
    /// </summary>
    /// <param name="look">The look that is being propagated.</param>
    /// <remarks>This is currently used only to propagate the effective look of up and down scrolls.
    /// Since the scrolls ignore many look properties (like all those related to icons or expanded items),
    /// we omit many properties in this method.
    /// In the future we can modify this method to propagate the looks for all the items, too.
    /// Note: this is not the method used to propagate the looks in ItemLooks collection.</remarks>
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

    #region Down-level Rendering

    internal void RenderAccessibleMenu(HtmlTextWriter output)
    {
      output.Write("<div class=\"menu\"");
      output.Write(" id=\"" + this.GetSaneId() + "\"");
      output.Write(">");
      int itemIndex = -1;
      RenderAccessibleGroup(output, null, ref itemIndex);
      output.Write("</div>");
    }

    private void RenderAccessibleGroup(HtmlTextWriter output, MenuItem parentItem, ref int itemIndex)
    {
      bool groupIsVertical = (parentItem == null) ? (this.Orientation == GroupOrientation.Vertical) : (parentItem.SubGroupOrientation == GroupOrientation.Vertical);
      output.Write("<ul");

      string groupId = "G" + this.GetSaneId() + "_" + itemIndex.ToString();
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
        foreach (MenuItem item in (parentItem == null ? this.Items : parentItem.Items))
				{
					maxLeftIconWidth = Math.Max(maxLeftIconWidth, (int)item.EffectiveLook.LeftIconWidth.Value);
					maxRightIconWidth = Math.Max(maxRightIconWidth, (int)item.EffectiveLook.RightIconWidth.Value);
				}
			}

      foreach (MenuItem item in (parentItem == null ? this.Items : parentItem.Items))
      {
        output.Write("<li");

        itemIndex++;

        string itemId = this.GetSaneId() + "_" + itemIndex.ToString();
        output.Write(" id=\"");
        output.Write(itemId);
        output.Write("\"");

        if (item.EffectiveLook.CssClass != null)
        {
          output.Write(" class=\"");
          output.Write(item.EffectiveLook.CssClass);
          output.Write("\"");
        }

        // right icon
        bool rightIconIsVisible = false;
        switch (item.EffectiveLook.RightIconVisibility)
        {
          case ItemIconVisibility.Always:           rightIconIsVisible = true;                  break;
          case ItemIconVisibility.WhenChecked:      rightIconIsVisible = item.Checked;          break;
          case ItemIconVisibility.WhenExpandable:   rightIconIsVisible = item.Items.Count > 0;  break;
        }
        string rightIconStyle = (rightIconIsVisible && item.EffectiveLook.RightIconUrl != null) ? 
          ("background-image:url(" + ConvertImageUrl(item.EffectiveLook.RightIconUrl) + ");background-repeat:no-repeat;background-position:right center;") : null;
        string itemWidthStyle = (!groupIsVertical && item.Width != Unit.Empty) ? ("width:" + item.Width.ToString() + ";") : null;
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
        output.Write(item.NavigateUrl);
        output.Write("\"");

        // left icon
        bool leftIconIsVisible = false;
        switch (item.EffectiveLook.LeftIconVisibility)
        {
          case ItemIconVisibility.Always:           leftIconIsVisible = true;                   break;
          case ItemIconVisibility.WhenChecked:      leftIconIsVisible = item.Checked;           break;
          case ItemIconVisibility.WhenExpandable:   leftIconIsVisible = item.Items.Count > 0;   break;
        }
        string leftIconStyle = (leftIconIsVisible && item.EffectiveLook.LeftIconUrl != null) ? 
          ("background-image:url(" + ConvertImageUrl(item.EffectiveLook.LeftIconUrl) + ");background-repeat:no-repeat;background-position:left center;") : null;
        if (leftIconStyle != null)
        {
          output.Write(" style=\"");
          output.Write(leftIconStyle);
          output.Write("\"");
        }
        output.Write(">");

        output.Write("<span");
        int leftIconPadding = groupIsVertical ? maxLeftIconWidth : (int)item.EffectiveLook.LeftIconWidth.Value;
        int leftTextPadding = (int)item.EffectiveLook.LabelPaddingLeft.Value;
        string leftPaddingStyle = "padding-left:" + (leftIconPadding + leftTextPadding) + "px;";
        int rightIconPadding = groupIsVertical ? maxRightIconWidth : (int)item.EffectiveLook.RightIconWidth.Value;
        int rightTextPadding = (int)item.EffectiveLook.LabelPaddingRight.Value;
        string rightPaddingStyle = "padding-right:" + (rightIconPadding + rightTextPadding) + "px;";
        output.Write(" style=\"");
        output.Write(leftPaddingStyle);
        output.Write(rightPaddingStyle);
        output.Write("\">");

        output.Write(item.Text);

        output.Write("</span>");
        output.Write("</a>");

        if (item.Items.Count > 0)
        {
          RenderAccessibleGroup(output, item, ref itemIndex);
        }

        output.Write("</li>");
      }

      output.Write("</ul>");
    }

		internal void RenderDownLevelContent(HtmlTextWriter output)
		{
			output.RenderBeginTag(HtmlTextWriterTag.Div);

			this.AddAttributesToRender(output);
			RenderDownLevelItems(output, this.Items); // Render the top group

			output.RenderEndTag(); // </div>
		}

		private void RenderDownLevelItems(HtmlTextWriter output, MenuItemCollection arItems)
		{
			output.AddAttribute("cellpadding", "0");
			output.AddAttribute("border", "0");

			Unit groupWidth = this.Width == Unit.Empty ? this.DefaultGroupWidth : this.Width;
			if (groupWidth != Unit.Empty) output.AddAttribute("width", Utils.ConvertUnitToJSConstant(groupWidth));
      
			Unit groupHeight = this.Height == Unit.Empty ? this.DefaultGroupHeight : this.Height;
			if (groupHeight != Unit.Empty) output.AddAttribute("height", Utils.ConvertUnitToJSConstant(groupHeight));

			string groupCssClass = this.CssClass == string.Empty ? this.DefaultGroupCssClass : this.CssClass;
			if (groupCssClass != null) output.AddAttribute("class", groupCssClass);

			Unit groupItemSpacing = this.TopGroupItemSpacing == Unit.Empty ? Unit.Parse("0") : this.TopGroupItemSpacing;
			output.AddAttribute("cellspacing", Utils.ConvertUnitToJSConstant(groupItemSpacing));

			if(this.RenderDefaultStyles)
			{
				output.AddAttribute("style", "background-color:#EEEEEE;border:1px;border-color:#EEEEEE;border-style:solid;");
			}

			output.RenderBeginTag("table");

			output.RenderBeginTag("tr");

			MenuItemCollection items = this.Items;
			bool groupIsVertical = this.Orientation == GroupOrientation.Vertical;
			int maxLeftIconWidth = 0;
			int maxRightIconWidth = 0;
			if (groupIsVertical)
			{
				// If the group is vertical, run through all the items to find the largest left and right icon widths
				foreach (MenuItem item in items)
				{
					maxLeftIconWidth = Math.Max(maxLeftIconWidth, (int)item.EffectiveLook.LeftIconWidth.Value);
					maxRightIconWidth = Math.Max(maxRightIconWidth, (int)item.EffectiveLook.RightIconWidth.Value);
				}
			}

      #region render the HTML for each item

			bool firstItemInGroup = true; /* Whether we are rendering the first item in this group.
                                     * Needed in order to know when to insert row breaks in vertical groups. */
			foreach (MenuItem item in items)
			{
				if (!item.Visible) continue; // Skip invisible items

				if (groupIsVertical) // We need to insert row breaks in vertical groups
				{
					if (firstItemInGroup)
					{
						firstItemInGroup = false; /* No need to insert a row break the first time.  Just make a note of this 
                                       * first item so that we know from now it won't be the first item any more. */
					}
					else
					{
						// Insert a row break
						output.RenderEndTag(); // </tr>
						output.RenderBeginTag("tr");
					}
				}

				if (item.EffectiveLook.ImageUrl != null)
				{
          #region render an image item

					/*
					output.AddAttribute("onmousemove","return false");
					output.AddAttribute("ondblclick","return false");
					output.AddAttribute("onmouseover","ItemMouseOver(this,event)");
					output.AddAttribute("onmouseout","ItemMouseOut(this,event)");
					output.AddAttribute("onmousedown","ItemMouseDown(this)");
					output.AddAttribute("onmouseup","ItemMouseUp(this)");
					if (item.Enabled) output.AddAttribute("onclick","ItemClick(this)");
					*/
					if (item.Width != Unit.Empty) output.AddAttribute("width",Utils.ConvertUnitToJSConstant(item.Width));
					if (item.Height != Unit.Empty) output.AddAttribute("height",Utils.ConvertUnitToJSConstant(item.Height));
					if (item.ToolTip != null) output.AddAttribute("title",item.ToolTip);
          
					output.RenderBeginTag("td");

          if (item.NavigateUrl != null && item.NavigateUrl != "")
          {
            output.AddAttribute("href", item.NavigateUrl);
            output.RenderBeginTag("a");
          }

					output.AddAttribute("border","0");
					output.AddAttribute("alt", (item.ToolTip == null) ? "" : item.ToolTip);
					if(this.RenderDefaultStyles)
					{
						output.AddAttribute("style", item.Attributes["style"]);
					}
					if (item.EffectiveLook.CssClass != null) output.AddAttribute("class",item.EffectiveLook.CssClass);
					if (item.Width != Unit.Empty) output.AddAttribute("width",Utils.ConvertUnitToJSConstant(item.Width));
					if (item.Height != Unit.Empty) output.AddAttribute("height",Utils.ConvertUnitToJSConstant(item.Height));
					output.AddAttribute("src", (item.EffectiveLook.ImageUrl == null) ? "" : ConvertImageUrl(item.EffectiveLook.ImageUrl));
					output.RenderBeginTag("img");
					output.RenderEndTag(); // <img />

          if (item.NavigateUrl != null && item.NavigateUrl != "")
          {
            output.RenderEndTag(); // </a>
          }

					output.RenderEndTag(); // </td>

          #endregion
				}

				else if ( maxLeftIconWidth > 0
					|| item.EffectiveLook.RightIconUrl != null 
					|| maxRightIconWidth > 0 
					|| item.EffectiveLook.LeftIconUrl != null )
				{
          #region render an icon item
          
					output.RenderBeginTag("td");

					/*
					output.AddAttribute("onmousemove","return false");
					output.AddAttribute("ondblclick","return false");
					output.AddAttribute("onmouseover","ItemMouseOver(this,event)");
					output.AddAttribute("onmouseout","ItemMouseOut(this,event)");
					output.AddAttribute("onmousedown","ItemMouseDown(this)");
					output.AddAttribute("onmouseup","ItemMouseUp(this)");
					if (item.Enabled) output.AddAttribute("onclick","ItemClick(this)");
					*/
          
					output.AddStyleAttribute("padding-left","0");
					output.AddStyleAttribute("padding-right","0");
					output.AddAttribute("cellpadding","0");
					output.AddAttribute("cellspacing","0");
					output.AddAttribute("border","0");
					output.AddAttribute("width", (item.Width == Unit.Empty) ? "100%" : Utils.ConvertUnitToJSConstant(item.Width));
					if(this.RenderDefaultStyles)
					{
						output.AddAttribute("style", item.Attributes["style"]);
					}
					if (item.Height != Unit.Empty) output.AddAttribute("height", Utils.ConvertUnitToJSConstant(item.Height));
					if (item.ToolTip != null) output.AddAttribute("title",item.ToolTip);
					if (item.EffectiveLook.CssClass != null) output.AddAttribute("class",item.EffectiveLook.CssClass);
					if(this.RenderDefaultStyles)
					{
						output.AddAttribute("style", item.Attributes["style"]);
					}
					output.RenderBeginTag("table");

					output.RenderBeginTag("tr");

          #region left icon

					int itemEffectiveLeftIconWidth = (item.EffectiveLook.LeftIconWidth != Unit.Empty) ? 
						(int)item.EffectiveLook.LeftIconWidth.Value :
						maxLeftIconWidth;
					if (itemEffectiveLeftIconWidth > 0 || item.EffectiveLook.LeftIconUrl != null)
					{
						output.AddStyleAttribute("padding","0");
						if (itemEffectiveLeftIconWidth > 0) output.AddAttribute("width",itemEffectiveLeftIconWidth.ToString());
						output.RenderBeginTag("td");

						if (item.EffectiveLook.LeftIconUrl != null)
						{
							output.AddAttribute("alt", (item.ToolTip == null) ? "" : item.ToolTip);
							output.AddAttribute("border","0");
							output.AddAttribute("src", ConvertImageUrl(item.EffectiveLook.LeftIconUrl));
							if (itemEffectiveLeftIconWidth > 0) output.AddAttribute("width",itemEffectiveLeftIconWidth.ToString());
							if (item.EffectiveLook.LeftIconHeight != Unit.Empty)
								output.AddAttribute("height", Utils.ConvertUnitToJSConstant(item.EffectiveLook.LeftIconHeight));
							output.RenderBeginTag("img");
							output.RenderEndTag(); // <img />
						}

						output.RenderEndTag(); // </td>
					}

          #endregion

          #region label

					output.AddAttribute("align", item.TextAlign.ToString().ToLower());
					if (item.EffectiveLook.LabelPaddingBottom != Unit.Empty) 
						output.AddStyleAttribute("padding-bottom", Utils.ConvertUnitToJSConstant(item.EffectiveLook.LabelPaddingBottom));
					if (item.EffectiveLook.LabelPaddingLeft != Unit.Empty) 
						output.AddStyleAttribute("padding-left", Utils.ConvertUnitToJSConstant(item.EffectiveLook.LabelPaddingLeft));
					if (item.EffectiveLook.LabelPaddingRight != Unit.Empty) 
						output.AddStyleAttribute("padding-right", Utils.ConvertUnitToJSConstant(item.EffectiveLook.LabelPaddingRight));
					if (item.EffectiveLook.LabelPaddingTop != Unit.Empty) 
						output.AddStyleAttribute("padding-top", Utils.ConvertUnitToJSConstant(item.EffectiveLook.LabelPaddingTop));
					output.RenderBeginTag("td");

          if (item.NavigateUrl != null && item.NavigateUrl != "")
          {
            output.AddAttribute("href", item.NavigateUrl);
            output.RenderBeginTag("a");
          }

					if (!item.TextWrap) output.RenderBeginTag("nobr");

					if (item.ServerTemplateId != null && item.ServerTemplateId != "")
					{
						output.Write("[templated item]"); // We do not actually render the template
					}
					else
					{
						output.Write(item.Text);
					}

					if (!item.TextWrap) output.RenderEndTag(); // </nobr>

          if (item.NavigateUrl != null && item.NavigateUrl != "")
          {
            output.RenderEndTag(); // </a>
          }

					output.RenderEndTag(); // </td>

          #endregion

          #region right icon

					int itemEffectiveRightIconWidth = (item.EffectiveLook.RightIconWidth != Unit.Empty) ? 
						(int)item.EffectiveLook.RightIconWidth.Value :
						maxRightIconWidth;
					if (itemEffectiveRightIconWidth > 0 || item.EffectiveLook.RightIconUrl != null)
					{
						output.AddStyleAttribute("padding","0");
						if (itemEffectiveRightIconWidth > 0) output.AddAttribute("width", itemEffectiveRightIconWidth.ToString());
						output.RenderBeginTag("td");

						if (item.EffectiveLook.RightIconUrl != null)
						{
							output.AddAttribute("alt", (item.ToolTip == null) ? "" : item.ToolTip);
							output.AddAttribute("border","0");
							output.AddAttribute("src", ConvertImageUrl(item.EffectiveLook.RightIconUrl));
							if (itemEffectiveRightIconWidth > 0) output.AddAttribute("width", itemEffectiveRightIconWidth.ToString());
							if (item.EffectiveLook.RightIconHeight != Unit.Empty) 
								output.AddAttribute("height", Utils.ConvertUnitToJSConstant(item.EffectiveLook.RightIconHeight));
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
          #region render a css item

					/*
					output.AddAttribute("onmousemove","return false");
					output.AddAttribute("ondblclick","return false");
					output.AddAttribute("onmouseover","ItemMouseOver(this,event)");
					output.AddAttribute("onmouseout","ItemMouseOut(this,event)");
					output.AddAttribute("onmousedown","ItemMouseDown(this)");
					output.AddAttribute("onmouseup","ItemMouseUp(this)");
					if (item.Enabled) output.AddAttribute("onclick","ItemClick(this)");
					*/
					if (item.Width != Unit.Empty) output.AddAttribute("width",Utils.ConvertUnitToJSConstant(item.Width));
					if (item.Height != Unit.Empty) output.AddAttribute("height",Utils.ConvertUnitToJSConstant(item.Height));
					if (item.ToolTip != null) output.AddAttribute("title",item.ToolTip);
					output.AddAttribute("align", item.TextAlign.ToString().ToLower());
					if (item.EffectiveLook.LabelPaddingBottom != Unit.Empty) 
						output.AddStyleAttribute("padding-bottom", Utils.ConvertUnitToJSConstant(item.EffectiveLook.LabelPaddingBottom));
					if (item.EffectiveLook.LabelPaddingLeft != Unit.Empty) 
						output.AddStyleAttribute("padding-left", Utils.ConvertUnitToJSConstant(item.EffectiveLook.LabelPaddingLeft));
					if (item.EffectiveLook.LabelPaddingRight != Unit.Empty) 
						output.AddStyleAttribute("padding-right", Utils.ConvertUnitToJSConstant(item.EffectiveLook.LabelPaddingRight));
					if (item.EffectiveLook.LabelPaddingTop != Unit.Empty) 
						output.AddStyleAttribute("padding-top", Utils.ConvertUnitToJSConstant(item.EffectiveLook.LabelPaddingTop));
					if (item.EffectiveLook.CssClass != null) output.AddAttribute("class",item.EffectiveLook.CssClass);
 					if(this.RenderDefaultStyles)
					{
						output.AddAttribute("style", item.Attributes["style"]);
					}
					output.RenderBeginTag("td");

          if (item.NavigateUrl != null && item.NavigateUrl != "")
          {
            output.AddAttribute("href", item.NavigateUrl);
            output.RenderBeginTag("a");
          }

					if (!item.TextWrap) output.RenderBeginTag("nobr");
                    
					if (item.ServerTemplateId != null && item.ServerTemplateId != "")
					{
						output.Write("[templated item]"); // We do not actually render the template
					}
					else
					{
						output.Write(item.Text);
					}

					if (!item.TextWrap) output.RenderEndTag(); // </nobr>

          if (item.NavigateUrl != null && item.NavigateUrl != "")
          {
            output.RenderEndTag(); // </a>
          }

					output.RenderEndTag(); // </td>

          #endregion
				}

			}
      #endregion

			output.RenderEndTag(); // </tr>
      
			output.RenderEndTag(); // </table>
		}

    #endregion

    #region Delegates

		/// <summary>
    /// Delegate for <see cref="ItemSelected"/> event of <see cref="Menu"/> class.
    /// </summary>
		public delegate void ItemSelectedEventHandler(object sender, MenuItemEventArgs e);

		/// <summary>
    /// Fires after a menu item is selected.
		/// </summary>
		[ Description("Fires after a menu item is selected."), 
		Category("Menu Events") ]
		public event ItemSelectedEventHandler ItemSelected;

		private void OnItemSelected(MenuItemEventArgs e) 
		{         
			if (ItemSelected != null) 
			{
				ItemSelected(this, e);
			}   
		}

    /// <summary>
    /// Delegate for <see cref="ItemCheckChanged"/> event of <see cref="Menu"/> class.
    /// </summary>
    public delegate void ItemCheckChangedEventHandler(object sender, MenuItemCheckChangedEventArgs e);

    /// <summary>
    /// Fires after an item is checked or unchecked.
    /// </summary>
    [Description("Fires after an item is checked or unchecked."),
    Category("Menu Events")]
    public event ItemCheckChangedEventHandler ItemCheckChanged;

    private void OnItemCheckChanged(MenuItemCheckChangedEventArgs e)
    {
      if (ItemCheckChanged != null)
      {
        ItemCheckChanged(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="ItemDataBound"/> event of <see cref="Menu"/> class.
    /// </summary>
    public delegate void ItemDataBoundEventHandler(object sender, MenuItemDataBoundEventArgs e);
		
    /// <summary>
    /// Fires after a Menu item is data bound.
    /// </summary>
    [ Description("Fires after a Menu item is data bound."),
    Category("Menu Events") ]
    public event ItemDataBoundEventHandler ItemDataBound;

    // generic trigger
    protected override void OnNodeDataBound(NavigationNode oNode, object oDataItem) 
    {
      if (ItemDataBound != null) 
      {
        MenuItemDataBoundEventArgs e = new MenuItemDataBoundEventArgs();
        
        e.Item = (MenuItem)oNode;
        e.DataItem = oDataItem;

        ItemDataBound(this, e);
      }   
    }

    #endregion

  }

	#region Supporting types

  /// <summary>
  /// Specifies the direction of expansion for pop-up groups of items.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used to specify the direction of expansion for pop-up groups of items/nodes. It
  /// describes the orientation of the resulting group using its parent item as a reference.
  /// </para>
  /// </remarks>
  public enum GroupExpandDirection
	{
    /// <summary>Default alignment is used, typically <b>RightDown</b> or <b>BelowLeft</b>.</summary>
    Auto,

    /// <summary>Expand the subgroup below its parent item, and align their right edges.</summary>
    BelowRight,

    /// <summary>Expand the subgroup below its parent item, and align their left edges.</summary>
    BelowLeft,

    /// <summary>Expand the subgroup above its parent item, and align their right edges.</summary>
    AboveRight,

    /// <summary>Expand the subgroup above its parent item, and align their left edges.</summary>
    AboveLeft,

    /// <summary>Expand the subgroup just to the right of its parent item, and align their top edges.</summary>
    RightDown,

    /// <summary>Expand the subgroup just to the right of its parent item, and align their bottom edges.</summary>
    RightUp,

    /// <summary>Expand the subgroup just to the left of its parent item, and align their top edges.</summary>
    LeftDown,

    /// <summary>Expand the subgroup just to the left of its parent item, and align their bottom edges.</summary>
    LeftUp
	}

  /// <summary>
  /// Specifies whether a <see cref="Menu"/> is a context menu, and if so, what type.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the <code>Menu</code> <see cref="Menu.ContextMenu" /> property, and
  /// determines whether a Menu is a context menu. It is also used to determine the specific behavior of a context menu. 
  /// </para>
  /// </remarks>
	public enum ContextMenuType
	{
	  /// <summary>Not a context menu, this menu is a permanent part of the page.</summary>
		None,

    /// <summary>A global context menu, expanding on a right click anywhere on the page.</summary>
    Simple,

    /// <summary>A local context menu, expanding on a right click on a specific element.</summary>
    ControlSpecific,

    /// <summary>A dormant context menu, expanding when called by client-side script.</summary>
    Custom
	}

	/// <summary>
  /// Arguments for <see cref="Menu.ItemSelected"/> server-side event of <see cref="Menu"/> control.
	/// </summary>
	[ToolboxItem(false)]
	public class MenuItemEventArgs : EventArgs
	{
		/// <summary>
		/// The command name.
		/// </summary>
		public string Command;

		/// <summary>
		/// The item in question.
		/// </summary>
		public MenuItem Item;
	}

  /// <summary>
  /// Arguments for <see cref="Menu.ItemDataBound"/> server-side event of <see cref="Menu"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class MenuItemDataBoundEventArgs : EventArgs
  {
    /// <summary>
    /// The Menu item.
    /// </summary>
    public MenuItem Item;

    /// <summary>
    /// The data item bound to.
    /// </summary>
    public object DataItem;
  }

  /// <summary>
  /// Arguments for <see cref="Menu.ItemCheckChanged"/> server-side event of <see cref="Menu"/> control.
  /// </summary>
  public class MenuItemCheckChangedEventArgs : EventArgs
  {
    /// <summary>
    /// The MenuItem which the event relates to.
    /// </summary>
    public MenuItem Item;

    public MenuItemCheckChangedEventArgs(MenuItem oItem)
    {
      Item = oItem;
    }
  }


	#endregion

}
