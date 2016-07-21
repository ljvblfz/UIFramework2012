using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
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
  /// <seealso cref="NavBar.WebService" />
  /// <seealso cref="NavBar.WebServiceMethod" />
  public class NavBarWebServiceRequest
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
  /// This is the type returned from the web service method set to handle requests for navbar items.
  /// </remarks>
  /// <seealso cref="NavBar.WebService" />
  /// <seealso cref="NavBar.WebServiceMethod" />
  public class NavBarWebServiceResponse : BaseNavigatorWebServiceResponse
  {
    NavBarItemCollection _items;

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

    public NavBarWebServiceResponse()
    {
      _items = new NavBarItemCollection(null, null);
    }

    public void AddItem(NavBarItem oItem)
    {
      _items.Add(oItem);
    }
  }

  #endregion


  #region EventArgs classes
  
  /// <summary>
  /// Arguments for <see cref="NavBarItem">item</see>-centric server-side events of the <see cref="NavBar"/> control.
  /// </summary>
  /// <remarks>
  /// Arguments of this type are used by the following events: <see cref="NavBar.ItemSelected"/>, <see cref="NavBar.ItemExpanded"/>,
  /// and <see cref="NavBar.ItemCollapsed"/>.
  /// </remarks>
  [ToolboxItem(false)]
  public class NavBarItemEventArgs : EventArgs
  {
    /// <summary>
    /// The command name.
    /// </summary>
    public string Command;

    /// <summary>
    /// The node in question.
    /// </summary>
    public NavBarItem Item;
  }

  /// <summary>
  /// Arguments for <see cref="NavBar.ItemDataBound"/> server-side event of <see cref="NavBar"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class NavBarItemDataBoundEventArgs : EventArgs
  {
    /// <summary>
    /// The NavBar node.
    /// </summary>
    public NavBarItem Item;

    /// <summary>
    /// The data item bound to.
    /// </summary>
    public object DataItem;
  }

#endregion

	/// <summary>
  /// Displays a sliding menu in the web page.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Data is loaded via the <see cref="BaseNavigator.SiteMapXmlFile"/> property (for local XML files), <see cref="BaseNavigator.DataSource" />
  /// property (for XmlDocument or DataSet objects). Data can also be added programmatically
  /// or inline, by populating the <see cref="Items" /> collection with <see cref="NavBarItem" /> objects.
  /// </para>
  /// <para>
  /// Besides CSS, NavBar's presentation can be modified using templates. There are two kinds of templates which can be used:
  /// <see cref="BaseNavigator.ServerTemplates" /> and <see cref="BaseNavigator.ClientTemplates" />. Client templates consist of markup and client-side binding expressions and are
  /// the suggested way of templating for situations where ASP.NET controls are not required.
  /// </para>
  /// </remarks>
  [GuidAttribute("53f586d7-6911-4cb0-82bd-564a9f882220")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:NavBar Width=180 Height=250 runat=server></{0}:NavBar>")]
  [PersistChildren(false)]
  [ParseChildren(true)]
  [Designer(typeof(ComponentArt.Web.UI.NavBarItemsDesigner))]
  public sealed class NavBar : BaseMenu
	{
    #region Private Properties

    private ArrayList _expandedList;
    private ArrayList ClientExpandedList
    {
      get
      {
        if(_expandedList == null)
        {
          _expandedList = new ArrayList();
        }

        return _expandedList;
      }
    }

    private Hashtable _propertyIndex;
    internal override Hashtable PropertyIndex
    {
      get
      {
        if (_propertyIndex == null)
        {
          _propertyIndex = new Hashtable();
          _propertyIndex["0"] = "AutoPostBackOnCollapse";
          _propertyIndex["1"] = "AutoPostBackOnExpand";
          _propertyIndex["2"] = "AutoPostBackOnSelect";
          _propertyIndex["3"] = "CausesValidation";
          _propertyIndex["4"] = "ChildSelectedLookId";
          _propertyIndex["5"] = "ClientSideCommand";
          _propertyIndex["6"] = "ClientTemplateId";
          _propertyIndex["7"] = "DefaultSubGroupCssClass";
          _propertyIndex["8"] = "DefaultSubItemChildSelectedLookId";
          _propertyIndex["9"] = "DefaultSubItemDisabledLookId";
          _propertyIndex["10"] = "DefaultSubItemLookId";
          _propertyIndex["11"] = "DefaultSubItemSelectedLookId";
          _propertyIndex["12"] = "DefaultSubItemTextAlign";
          _propertyIndex["13"] = "DefaultSubItemTextWrap";
          _propertyIndex["14"] = "DisabledLookId";
          _propertyIndex["15"] = "Enabled";
          _propertyIndex["16"] = "Expanded";
          _propertyIndex["17"] = "Height";
          _propertyIndex["18"] = "ID";
          _propertyIndex["19"] = "KeyboardShortcut";
          _propertyIndex["20"] = "LookId";
          _propertyIndex["21"] = "NavigateUrl";
          _propertyIndex["22"] = "PageViewId";
          _propertyIndex["23"] = "Selectable";
          _propertyIndex["24"] = "SelectedLookId";
          _propertyIndex["25"] = "ServerTemplateId";
          _propertyIndex["26"] = "SiteMapXmlFile";
          _propertyIndex["27"] = "SubGroupCssClass";
          _propertyIndex["28"] = "SubGroupHeight";
          _propertyIndex["29"] = "SubGroupItemSpacing";
          _propertyIndex["30"] = "Target";
          _propertyIndex["31"] = "Text";
          _propertyIndex["32"] = "TextAlign";
          _propertyIndex["33"] = "TextWrap";
          _propertyIndex["34"] = "ToolTip";
          _propertyIndex["35"] = "Value";
          _propertyIndex["36"] = "Visible";
        }
        return _propertyIndex;
      }
    }

    #endregion

    #region Public Properties

    private NavBarClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public NavBarClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new NavBarClientEvents();
        }
        return _clientEvents;
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
    [DefaultValue("cart-")]
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
    /// Identifier of client-side handler to call when an item is collapsed.
    /// </summary>
    /// <remarks>
    /// Deprecated.  Use <see cref="NavBarClientEvents.ItemCollapse">ClientEvents.ItemCollapse</see> instead.
    /// </remarks>
    [Browsable(false)]
    [Category("Behavior")]
    [DefaultValue("")]
    [Description("Deprecated.  Use ClientEvents.ItemCollapse instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use ClientEvents.ItemCollapse instead.", false)]
    public string ClientSideOnItemCollapse
    {
      get 
      {
        object o = Properties["ClientSideOnItemCollapse"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ClientSideOnItemCollapse"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side handler to call when an item is expanded.
    /// </summary>
    /// <remarks>
    /// Deprecated.  Use <see cref="NavBarClientEvents.ItemExpand">ClientEvents.ItemExpand</see> instead.
    /// </remarks>
    [Browsable(false)]
    [Category("Behavior")]
    [DefaultValue("")]
    [Description("Deprecated.  Use ClientEvents.ItemExpand instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use ClientEvents.ItemExpand instead.", false)]
    public string ClientSideOnItemExpand
    {
      get 
      {
        object o = Properties["ClientSideOnItemExpand"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ClientSideOnItemExpand"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side handler to call when the mouse leaves an item.
    /// </summary>
    /// <remarks>
    /// Deprecated.  Use <see cref="NavBarClientEvents.ItemMouseOut">ClientEvents.ItemMouseOut</see> instead.
    /// </remarks>
    [Browsable(false)]
    [Category("Behavior")]
    [DefaultValue("")]
    [Description("Deprecated.  Use ClientEvents.ItemMouseOut instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use ClientEvents.ItemMouseOut instead.", false)]
    public string ClientSideOnItemMouseOut
    {
      get 
      {
        object o = Properties["ClientSideOnItemMouseOut"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ClientSideOnItemMouseOut"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side handler to call when the mouse goes over an item.
    /// </summary>
    /// <remarks>
    /// Deprecated.  Use <see cref="NavBarClientEvents.ItemMouseOver">ClientEvents.ItemMouseOver</see> instead.
    /// </remarks>
    [Browsable(false)]
    [Category("Behavior")]
    [DefaultValue("")]
    [Description("Deprecated.  Use ClientEvents.ItemMouseOver instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use ClientEvents.ItemMouseOver instead.", false)]
    public string ClientSideOnItemMouseOver
    {
      get 
      {
        object o = Properties["ClientSideOnItemMouseOver"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ClientSideOnItemMouseOver"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side handler to call when an item is selected.
    /// </summary>
    /// <remarks>
    /// Deprecated.  Use <see cref="NavBarClientEvents.ItemSelect">ClientEvents.ItemSelect</see> instead.
    /// </remarks>
    [Browsable(false)]
    [Category("Behavior")]
    [DefaultValue("")]
    [Description("Deprecated.  Use ClientEvents.ItemSelect instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use ClientEvents.ItemSelect instead.", false)]
    public string ClientSideOnItemSelect
    {
      get 
      {
        object o = Properties["ClientSideOnItemSelect"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ClientSideOnItemSelect"] = value;
      }
    }

    /// <summary>
    /// Default height to apply to items.
    /// </summary>
    [Description("Default height to apply to items.")]
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    public Unit DefaultItemHeight
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultItemHeight"]); 
      }
      set 
      {
        Properties["DefaultItemHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// Default spacing to provide between items.
    /// </summary>
    [Description("Default spacing to provide between items.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int DefaultItemSpacing
    {
      get 
      {
        object o = Properties["DefaultItemSpacing"]; 
        return (o == null) ? 0 : Utils.ParseInt(o,0); 
      }
      set 
      {
        Properties["DefaultItemSpacing"] = value.ToString();
      }
    }

    private ItemLook _defaultSubItemLook;
    /// <summary>
    /// The default look to apply to second-level items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.DefaultSubItemLook" /> or <see cref="BaseMenuItem.Look" />).
    /// </para>
    /// </remarks>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook DefaultSubItemLook
    {
      get
      {
        if (_defaultSubItemLook == null)
        {
          _defaultSubItemLook = new ItemLook(false, true);
        }
        return _defaultSubItemLook;
      }
      set
      {
        if (value != null)
        {
          _defaultSubItemLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the default look to apply to second-level items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.DefaultSubItemLookId" /> or <see cref="BaseMenuItem.LookId" />).
    /// </para>
    /// </remarks>
    [Description("The ID of the default look to apply to second-level items.")]
    [DefaultValue("")]
    [Category("ItemLook")]
    public string DefaultSubItemLookId
    {
      get
      {
        object o = ViewState["DefaultSubItemLookId"];
        return (o == null) ? string.Empty : (string)o;
      }
      set
      {
        ViewState["DefaultSubItemLookId"] = value;
      }
    }

    /// <summary>
    /// The default text alignment to apply to labels of second-level items.
    /// </summary>
    /// <seealso cref="BaseMenuItem.TextAlign" />
    [Category("Appearance")]
    [DefaultValue(TextAlign.Left)]
    [Description("The default text alignment to apply to labels of second-level items.")]
    public TextAlign DefaultSubItemTextAlign
    {
      get
      {
        return Utils.ParseTextAlign(ViewState["DefaultSubItemTextAlign"]);
      }
      set
      {
        ViewState["DefaultSubItemTextAlign"] = value;
      }
    }

    /// <summary>
    /// Whether to expand groups so they fill exactly the height of the NavBar.
    /// </summary>
    /// <remarks>
    /// If this property is set to <b>true</b>, ExpandSinglePath should also be set to <b>true</b>. The height of the
    /// NavBar will be constant and every group, when expanded, will expand to the same height, possibly causing some groups to
    /// have empty space below the items, and others to require scrolling.
    /// </remarks>
    /// <seealso cref="ExpandSinglePath" />
    [Description("Whether to expand groups so they fill exactly the height of the NavBar.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool FullExpand
    {
      get 
      {
        object o = Properties["FullExpand"]; 
        return (o == null) ? false : Utils.ParseBool(o,false); 
      }
      set 
      {
        Properties["FullExpand"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to only permit a single path in the NavBar to be expanded at a time.
    /// </summary>
    /// <remarks>
    /// If this is set to <b>true</b>, every expansion will cause other groups at the same or lower level to collapse.
    /// </remarks>
    [Description("Whether to only permit a single path in the NavBar to be expanded at a time.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool ExpandSinglePath
    {
      get 
      {
        object o = Properties["ExpandSinglePath"]; 
        return (o == null) ? false : Utils.ParseBool(o,false); 
      }
      set 
      {
        Properties["ExpandSinglePath"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to take on the dimensions of the containing DOM element.
    /// </summary>
    /// <remarks>
    /// If this is set to <b>true</b>, Every client-side call to Render() will make the NavBar take on the dimensions of its parent DOM element.
    /// </remarks>
    [Description("Whether to take on the dimensions of the containing DOM element.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool FillContainer
    {
      get 
      {
        object o = Properties["FillContainer"]; 
        return (o == null) ? false : Utils.ParseBool(o,false); 
      }
      set 
      {
        Properties["FillContainer"] = value.ToString();
      }
    }

    /// <summary>
    /// CssClass to use for this NavBar when it has keyboard focus.
    /// </summary>
    [Description("CssClass to use for this NavBar when it has keyboard focus.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string FocusedCssClass
    {
      get 
      {
        object o = Properties["FocusedCssClass"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["FocusedCssClass"] = value;
      }
    }

    /// <summary>
    /// Collection of root NavBarItems.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Browsable(false)]
    public NavBarItemCollection Items
    {
      get
      {
        if(this.nodes == null)
        {
          nodes = new NavBarItemCollection(this, null);
        }

        return (NavBarItemCollection)nodes;
      }
    }

    /// <summary>
    /// Whether to enable keyboard control of the NavBar.
    /// </summary>
    [Description("Whether to enable keyboard control of the NavBar.")]
    [Category("Behavior")]
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
    /// The NavigateUrl of the next item in the NavBar.
    /// </summary>
    [Description("The NavigateUrl of the next item in the NavBar. ")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new string NextUrl
    {
      get
      {
        if(this.selectedNode == null)
        {
          // Maintain selected node if we're doing that.
          if(this.selectedNodePostbackId != null && this.nodes != null)
          {
            this.SelectedItem = (NavBarItem)this.FindNodeByPostBackId(selectedNodePostbackId, this.nodes);
          }
        }

        return base.NextUrl;
      }
    }

    /// <summary>
    /// Image to use for scrolling down within a group, on hover.
    /// </summary>
    [Description("Image to use for scrolling down within a group, on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ScrollDownHoverImageUrl
    {
      get 
      {
        object o = Properties["ScrollDownHoverImageUrl"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ScrollDownHoverImageUrl"] = value;
      }
    }

    /// <summary>
    /// Image to use for scrolling down within a group, when the mouse is down.
    /// </summary>
    [Description("Image to use for scrolling down within a group, when the mouse is down.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ScrollDownActiveImageUrl
    {
      get 
      {
        object o = Properties["ScrollDownActiveImageUrl"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ScrollDownActiveImageUrl"] = value;
      }
    }

    /// <summary>
    /// Height to apply to scroll-down image.
    /// </summary>
    [Description("Height to apply to scroll-down image.")]
    [Category("Appearance")]
    [DefaultValue(0)]
    public int ScrollDownImageHeight
    {
      get 
      {
        object o = Properties["ScrollDownImageHeight"]; 
        return (o == null) ? 0 : Utils.ParseInt(o,0); 
      }
      set 
      {
        Properties["ScrollDownImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// Width to apply to scroll-down image.
    /// </summary>
    [Description("Width to apply to scroll-down image.")]
    [Category("Appearance")]
    [DefaultValue(0)]
    public int ScrollDownImageWidth
    {
      get 
      {
        object o = Properties["ScrollDownImageWidth"]; 
        return (o == null) ? 0 : Utils.ParseInt(o,0); 
      }
      set 
      {
        Properties["ScrollDownImageWidth"] = value.ToString();
      }
    }


    /// <summary>
    /// Image to use for scrolling down within a group.
    /// </summary>
    [Description("Image to use for scrolling down within a group.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ScrollDownImageUrl
    {
      get 
      {
        object o = Properties["ScrollDownImageUrl"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ScrollDownImageUrl"] = value;
      }
    }

    /// <summary>
    /// Image to use for scrolling up within a group, when the mouse is down.
    /// </summary>
    [Description("Image to use for scrolling up within a group, when the mouse is down.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ScrollUpActiveImageUrl
    {
      get 
      {
        object o = Properties["ScrollUpActiveImageUrl"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ScrollUpActiveImageUrl"] = value;
      }
    }

    /// <summary>
    /// Image to use for scrolling up within a group, on hover.
    /// </summary>
    [Description("Image to use for scrolling up within a group, on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ScrollUpHoverImageUrl
    {
      get 
      {
        object o = Properties["ScrollUpHoverImageUrl"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ScrollUpHoverImageUrl"] = value;
      }
    }

    /// <summary>
    /// Height to apply to scroll-up image.
    /// </summary>
    [Description("Height to apply to scroll-up image.")]
    [Category("Appearance")]
    [DefaultValue(0)]
    public int ScrollUpImageHeight
    {
      get 
      {
        object o = Properties["ScrollUpImageHeight"]; 
        return (o == null) ? 0 : Utils.ParseInt(o,0); 
      }
      set 
      {
        Properties["ScrollUpImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// Width to apply to scroll-up image.
    /// </summary>
    [Description("Width to apply to scroll-up image.")]
    [Category("Appearance")]
    [DefaultValue(0)]
    public int ScrollUpImageWidth
    {
      get 
      {
        object o = Properties["ScrollUpImageWidth"]; 
        return (o == null) ? 0 : Utils.ParseInt(o,0); 
      }
      set 
      {
        Properties["ScrollUpImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// Image to use for scrolling up within a group.
    /// </summary>
    [Description("Image to use for scrolling up within a group.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ScrollUpImageUrl
    {
      get 
      {
        object o = Properties["ScrollUpImageUrl"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ScrollUpImageUrl"] = value;
      }
    }

    /// <summary>
    /// Id of template to use for groups' scroll-down bars.
    /// </summary>
    [Description("Id of template to use for groups' scroll-down bars.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ScrollDownTemplateId
    {
      get 
      {
        object o = Properties["ScrollDownTemplateId"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ScrollDownTemplateId"] = value;
      }
    }

    /// <summary>
    /// Id of template to use for groups' scroll-up bars.
    /// </summary>
    [Description("Id of template to use for groups' scroll-up bars.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ScrollUpTemplateId
    {
      get 
      {
        object o = Properties["ScrollUpTemplateId"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        Properties["ScrollUpTemplateId"] = value;
      }
    }

    /// <summary>
    /// The selected item. This can be set on the server-side to force a item selection.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public NavBarItem SelectedItem
    {
      get
      {
        return (NavBarItem)(base.selectedNode);
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
    /// Whether to display the side scrollbar when group contents don't fit in a group.
    /// </summary>
    [Description("Whether to display the side scrollbar when group contents don't fit in a group.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool ShowScrollBar
    {
      get 
      {
        object o = Properties["ShowScrollBar"]; 
        return (o == null) ? false : Utils.ParseBool(o,false); 
      }
      set 
      {
        Properties["ShowScrollBar"] = value.ToString();
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
    /// The name of the ASP.NET AJAX web service to use for initially populating the NavBar.
    /// </summary>
    /// <seealso cref="WebServiceMethod" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service to use for initially populating the NavBar.")]
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
    /// The name of the ASP.NET AJAX web service method to use for initially populating the NavBar.
    /// </summary>
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service method to use for initially populating the NavBar.")]
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

    #region Protected Methods

    protected override NavigationNode AddNode()
    {
      NavBarItem oNewItem = new NavBarItem();
      this.Items.Add(oNewItem);
      return oNewItem;
    }

    protected override NavigationNode NewNode()
    {
      NavBarItem newNode = new NavBarItem();
      NavBarItemCollection dummy = newNode.Items; // This is a dummy call to ensure that newNode.nodes is not null
      return newNode;
    }
    
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      string sNavBarName = this.GetSaneId();

      if(Context != null)
      {
        if( Context.Request.Form[sNavBarName + "_ExpandedList"] != null &&
          Context.Request.Form[sNavBarName + "_ExpandedList"].Length > 0)
        {
          this.ClientExpandedList.AddRange(Context.Request.Form[sNavBarName + "_ExpandedList"].Split(','));
        }

        this.selectedNodePostbackId = Context.Request.Form[sNavBarName + "_SelectedItem"];
      }
    }

    /// <summary>
    /// React to being loaded.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573S188.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573Z388.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.NavBar.client_scripts.A573E888.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.NavBar.client_scripts.A573D588.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.NavBar.client_scripts.A573M488.js");        
      }
    }

    protected override void ComponentArtFixStructure()
    {
      base.ComponentArtFixStructure();

      // Maintain selected node if we're doing that.
      if(this.selectedNodePostbackId != null && this.nodes != null)
      {
        this.SelectedItem = (NavBarItem)this.FindNodeByPostBackId(selectedNodePostbackId, this.nodes);
      }

      // Maintain expanded states from client, if we're doing that.
      if(this.ClientExpandedList.Count > 0 && this.nodes != null)
      {
        SetExpanded(this.Items, this.ClientExpandedList);
        this.ClientExpandedList.Clear();
      }
    }
    
    /// <summary>
    /// Prepare to render this control.
    /// </summary>
    /// <param name="e">PreRender event arguments.</param>
    protected override void ComponentArtPreRender(EventArgs e)
    {
      base.ComponentArtPreRender(e);

      if (!this.IsDownLevel())
      {
        // Tack this to the bottom of the page, to know when we've loaded.
        RegisterStartupScript("ComponentArt_Page_Loaded",
          "<script type=\"text/javascript\">\n//<![CDATA[\nwindow.ComponentArt_Page_Loaded = true;\n//]]>\n</script>");
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
        if(!Page.IsClientScriptBlockRegistered("A573E888.js"))
        {
          Page.RegisterClientScriptBlock("A573E888.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.NavBar.client_scripts", "A573E888.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573D588.js"))
        {
          Page.RegisterClientScriptBlock("A573D588.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.NavBar.client_scripts", "A573D588.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573M488.js"))
        {
          Page.RegisterClientScriptBlock("A573M488.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.NavBar.client_scripts", "A573M488.js");
        }
      }
      }

      string sNavBarVarName = this.GetSaneId();

      if (this.AutoTheming)
      {
        this.ApplyTheming(false);
        // Hwan: 2010-01-11
        // this.RenderDefaultStyles = false;
      }

      // do we need default styles?
      if(this.RenderDefaultStyles)
      {
        // output default styles
        string sDefaultStyles = "<style>" + GetResourceContent("ComponentArt.Web.UI.NavBar.defaultStyle.css") + "</style>\n";
        output.Write(sDefaultStyles);
      }

      // Render storage
      if (!this.IsDownLevel())
      {
        string sStorage = "window.ComponentArt_Storage_" + sNavBarVarName + " = ";
        sStorage += this.BuildStorage();
        sStorage += ";\nwindow.ComponentArt_ItemLooks_" + sNavBarVarName + " = ";
        sStorage += this.BuildLooks();
        WriteStartupScript(output, this.DemarcateClientScript(sStorage, "ComponentArt.Web.UI.NavBar " + this.VersionString() + " " + sNavBarVarName));
      }

      // Find preloadable images
      this.LoadPreloadImages();

      // Preload images, if any
      if(this.PreloadImages.Count > 0)
      {
        this.RenderPreloadImages(output);
      }

      // Render client-side-selection-propagating field.
      output.AddAttribute("id", sNavBarVarName + "_SelectedItem");
      output.AddAttribute("name", sNavBarVarName + "_SelectedItem");
      output.AddAttribute("type", "hidden");
      output.AddAttribute("value", (this.SelectedItem == null? "" : this.SelectedItem.PostBackID));
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      // Render expand-state-maintaining hidden field.
      output.AddAttribute("id", sNavBarVarName + "_ExpandedList");
      output.AddAttribute("name", sNavBarVarName + "_ExpandedList");
      output.AddAttribute("type", "hidden");
      output.AddAttribute("value", GetExpanded(this.Items));
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      if(this.IsBrowserSearchEngine() && this.RenderSearchEngineStructure || this.ForceSearchEngineStructure)
      {
        RenderCrawlerStructure(output);
      }

      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        RenderAccessibleContent(output);
      }
      else if (this.IsDownLevel())
      {
        RenderDownLevelContent(output);
      }
      else
      {
        // Render scroll up/down templates if both are defined
        if(this.ScrollDownTemplateId != string.Empty && this.ScrollUpTemplateId != string.Empty)
        {
          NavigationCustomTemplate oDownTemplate = this.FindTemplateById(this.ScrollDownTemplateId);
          NavigationCustomTemplate oUpTemplate = this.FindTemplateById(this.ScrollUpTemplateId);

          if(oUpTemplate == null)
          {
            throw new Exception("Template not found: " + this.ScrollUpTemplateId);
          }
          if(oDownTemplate == null)
          {
            throw new Exception("Template not found: " + this.ScrollDownTemplateId);
          }

          NavigationTemplateContainer oUpContainer = new NavigationTemplateContainer(null);
          oUpTemplate.Template.InstantiateIn(oUpContainer);
          oUpContainer.ID = sNavBarVarName + "_ScrollUpTemplate";
          this.Controls.Add(oUpContainer);

          NavigationTemplateContainer oDownContainer = new NavigationTemplateContainer(null);
          oDownTemplate.Template.InstantiateIn(oDownContainer);
          oDownContainer.ID = sNavBarVarName + "_ScrollDownTemplate";
          this.Controls.Add(oDownContainer);
        }

        // Render item templates, if any
        foreach(Control oTemplate in this.Controls)
        {
          output.Write("<div id=\"" + oTemplate.ID + "\" style=\"display:none;\">");
          oTemplate.RenderControl(output);
          output.Write("</div>");
        }
        
        if (this.EnableViewState)
        {
          // Render client-storage-persisting field.
          output.AddAttribute("id", sNavBarVarName + "_Data");
          output.AddAttribute("name", sNavBarVarName + "_Data");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render toplevel-property-persisting field.
          output.AddAttribute("id", sNavBarVarName + "_Properties");
          output.AddAttribute("name", sNavBarVarName + "_Properties");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();
        }

        // Output div
        output.Write("<div");
        output.WriteAttribute("id", sNavBarVarName);
        if(this.ToolTip != string.Empty)
        {
          output.WriteAttribute("title", this.ToolTip);
        }
        if(!this.Enabled)
        {
          output.WriteAttribute("disabled", "disabled");
        }

        // Output style
        output.Write(" style=\"");
        if(!this.Height.IsEmpty)
        {
          output.WriteStyleAttribute("height", this.Height.ToString());
        }
        if(!this.Width.IsEmpty)
        {
          output.WriteStyleAttribute("width", this.Width.ToString());
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
        foreach(string sKey in this.Style.Keys)
        {
          output.WriteStyleAttribute(sKey, this.Style[sKey]);
        }
        output.Write("\"></div>");

        // Render tab-focus-getting element.
        if(this.KeyboardEnabled)
        {
          output.AddAttribute("href", "#");
          output.AddAttribute("onfocus", "ComponentArt_NavBar_SetKeyboardFocusedNavBar(" + sNavBarVarName + ");");
          output.AddStyleAttribute("position", "absolute");
          output.AddStyleAttribute("z-index", "99");
          output.RenderBeginTag(HtmlTextWriterTag.A);
          output.RenderEndTag();
        }

        // Render startup script

        // Add navbar initialization
        StringBuilder oStartupSB = new StringBuilder();
        oStartupSB.Append("window.ComponentArt_Init_" + sNavBarVarName + " = function() {\n");

        // Include check for whether everything we need is loaded,
        // and a retry after a delay in case it isn't.
        oStartupSB.Append("if(!window.ComponentArt_Page_Loaded || !window.ComponentArt_Utils_Loaded || !window.ComponentArt_NavBar_Kernel_Loaded || !window.ComponentArt_NavBar_Keyboard_Loaded || !window.ComponentArt_NavBar_Support_Loaded)\n");
        oStartupSB.Append("\t{setTimeout('ComponentArt_Init_" + sNavBarVarName + "()', 50); return; }\n\n");

        // Instantiate object
        oStartupSB.Append("window." + sNavBarVarName + " = new ComponentArt_NavBar('" + sNavBarVarName + "', ComponentArt_Storage_" + sNavBarVarName + ", ComponentArt_ItemLooks_" + sNavBarVarName + ");\n");

        // Write postback function reference
        if (Page != null)
        {
          oStartupSB.Append(sNavBarVarName + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
        }

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != sNavBarVarName)
        {
          oStartupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sNavBarVarName + "; " + sNavBarVarName + ".GlobalAlias = '" + ID + "'; }\n");
        }

        // Define properties
        oStartupSB.Append(sNavBarVarName + ".PropertyStorageArray = [\n");
        oStartupSB.Append("['ApplicationPath'," + Utils.ConvertStringToJSString(Context.Request.ApplicationPath) + "],");
        oStartupSB.Append("['AutoPostBackOnSelect'," + this.AutoPostBackOnSelect.ToString().ToLower() + "],");
        if (this.AutoTheming)
        {
          oStartupSB.Append("['AutoTheming',1],");
          oStartupSB.Append("['AutoThemingCssClassPrefix'," + Utils.ConvertStringToJSString(this.AutoThemingCssClassPrefix) + "],");
        }
        oStartupSB.Append("['BaseUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context, this.BaseUrl)) + "],");
        oStartupSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "],");
        oStartupSB.Append("['ClientSideOnItemCollapse'," + Utils.ConvertStringToJSString(this.ClientSideOnItemCollapse) + "],");
        oStartupSB.Append("['ClientSideOnItemExpand'," + Utils.ConvertStringToJSString(this.ClientSideOnItemExpand) + "],");
        oStartupSB.Append("['ClientSideOnItemMouseOut'," + Utils.ConvertStringToJSString(this.ClientSideOnItemMouseOut) + "],");
        oStartupSB.Append("['ClientSideOnItemMouseOver'," + Utils.ConvertStringToJSString(this.ClientSideOnItemMouseOver) + "],");
        oStartupSB.Append("['ClientSideOnItemSelect'," + Utils.ConvertStringToJSString(this.ClientSideOnItemSelect) + "],");
        oStartupSB.Append("['ClientTemplates'," + this._clientTemplates.ToString() +"],");
        oStartupSB.Append("['CollapseDuration'," + this.CollapseDuration.ToString() + "],");
        oStartupSB.Append("['CollapseSlide'," + (int)this.CollapseSlide + "],");
        oStartupSB.Append("['CollapseTransition'," + (int)this.CollapseTransition + "],");
        oStartupSB.Append("['CollapseTransitionCustomFilter'," + Utils.ConvertStringToJSString(this.CollapseTransitionCustomFilter) + "],");
        oStartupSB.Append("['ControlId'," + Utils.ConvertStringToJSString(this.UniqueID) + "],");
        oStartupSB.Append("['CssClass'," + Utils.ConvertStringToJSString(this.CssClass) + "],");
        oStartupSB.Append("['DefaultDisabledItemLookId'," + Utils.ConvertStringToJSString(this.DefaultDisabledItemLookId) + "],");
        oStartupSB.Append("['DefaultGroupCssClass'," + Utils.ConvertStringToJSString(this.DefaultGroupCssClass) + "],");
        oStartupSB.Append("['DefaultItemLookId'," + Utils.ConvertStringToJSString(this.DefaultItemLookId) + "],");
        oStartupSB.Append("['DefaultItemSpacing'," + this.DefaultItemSpacing.ToString() + "],");
        oStartupSB.Append("['DefaultItemTextAlign'," + (int)this.DefaultItemTextAlign + "],");
        oStartupSB.Append("['DefaultItemTextWrap'," + this.DefaultItemTextWrap.ToString().ToLower() + "],");
        oStartupSB.Append("['DefaultSelectedItemLookId'," + Utils.ConvertStringToJSString(this.DefaultSelectedItemLookId) + "],");
        oStartupSB.Append("['DefaultSubItemLookId'," + Utils.ConvertStringToJSString(this.DefaultSubItemLookId) + "],");
        oStartupSB.Append("['DefaultSubItemTextAlign'," + (int)this.DefaultSubItemTextAlign + "],");
        oStartupSB.Append("['DefaultTarget'," + Utils.ConvertStringToJSString(this.DefaultTarget) + "],");
        oStartupSB.Append("['ExpandDuration'," + this.ExpandDuration.ToString() + "],");
        oStartupSB.Append("['ExpandSinglePath'," + this.ExpandSinglePath.ToString().ToLower() + "],");
        oStartupSB.Append("['ExpandSlide'," + (int)this.ExpandSlide + "],");
        oStartupSB.Append("['ExpandTransition'," + (int)this.ExpandTransition + "],");
        oStartupSB.Append("['ExpandTransitionCustomFilter'," + Utils.ConvertStringToJSString(this.ExpandTransitionCustomFilter) + "],");
        oStartupSB.Append("['FillContainer'," + this.FillContainer.ToString().ToLower() + "],");
        oStartupSB.Append("['FocusedCssClass'," + Utils.ConvertStringToJSString(this.FocusedCssClass) + "],");
        oStartupSB.Append("['ForceHighlightedNodeID'," + Utils.ConvertStringToJSString(this.ForceHighlightedNodeID) + "],");
        oStartupSB.Append("['FullExpand'," + this.FullExpand.ToString().ToLower() + "],");
        oStartupSB.Append("['ImagesBaseUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl)) + "],");
        oStartupSB.Append("['KeyboardEnabled'," + this.KeyboardEnabled.ToString().ToLower() + "],");
        oStartupSB.Append("['MultiPageId'," + Utils.ConvertStringToJSString(this._multiPageId) + "],");
        oStartupSB.Append("['PreRenderAllLevels'," + this.PreRenderAllLevels.ToString().ToLower() + "],");
        oStartupSB.Append("['ScrollDownActiveImageUrl'," + Utils.ConvertStringToJSString(this.ScrollDownActiveImageUrl) + "],");
        oStartupSB.Append("['ScrollDownHoverImageUrl'," + Utils.ConvertStringToJSString(this.ScrollDownHoverImageUrl) + "],");
        oStartupSB.Append("['ScrollDownImageHeight'," + this.ScrollDownImageHeight.ToString() + "],");
        oStartupSB.Append("['ScrollDownImageUrl'," + Utils.ConvertStringToJSString(this.ScrollDownImageUrl) + "],");
        oStartupSB.Append("['ScrollDownImageWidth'," + this.ScrollDownImageWidth.ToString() + "],");
        oStartupSB.Append("['ScrollUpActiveImageUrl'," + Utils.ConvertStringToJSString(this.ScrollUpActiveImageUrl) + "],");
        oStartupSB.Append("['ScrollUpHoverImageUrl'," + Utils.ConvertStringToJSString(this.ScrollUpHoverImageUrl) + "],");
        oStartupSB.Append("['ScrollUpImageHeight'," + this.ScrollUpImageHeight.ToString() + "],");
        oStartupSB.Append("['ScrollUpImageUrl'," + Utils.ConvertStringToJSString(this.ScrollUpImageUrl) + "],");
        oStartupSB.Append("['ScrollUpImageWidth'," + this.ScrollUpImageWidth.ToString() + "],");
        oStartupSB.Append("['SoaService','" + this.SoaService + "'],");
        oStartupSB.Append("['WebService','" + this.WebService + "'],");
        oStartupSB.Append("['WebServiceCustomParameter','" + this.WebServiceCustomParameter + "'],");
        oStartupSB.Append("['WebServiceMethod','" + this.WebServiceMethod + "'],");
        oStartupSB.Append("['ShowScrollBar'," + this.ShowScrollBar.ToString().ToLower() + "]\n];\n");
        oStartupSB.Append(sNavBarVarName + ".LoadProperties();\n");

        // Initialize navbar
        oStartupSB.Append(sNavBarVarName + ".Initialize();\n");

        if (this.EnableViewState)
        {
          // add us to the client viewstate-saving mechanism
          oStartupSB.Append("ComponentArt_ClientStateControls[ComponentArt_ClientStateControls.length] = " + sNavBarVarName + ";\n");
        }

        // Keyboard
        if(this.KeyboardEnabled)
        {
          // Initialize keyboard
          oStartupSB.Append("ComponentArt_NavBar_InitKeyboard(" + sNavBarVarName + ");\n");

          // Create client script to register keyboard shortcuts
          StringBuilder oKeyboardSB = new StringBuilder();
          GenerateKeyShortcutScript(sNavBarVarName, this.Items, oKeyboardSB);
          oStartupSB.Append(oKeyboardSB.ToString());
        }

        if(this.SelectedItem != null)
        {
          oStartupSB.Append(sNavBarVarName + ".SelectItemById('" + this.SelectedItem.PostBackID + "',true);\n");
        }

        oStartupSB.Append("\nwindow." + sNavBarVarName + "_loaded = true;\n}\n");
        
        // Initiate NavBar creation
        oStartupSB.Append("setTimeout('ComponentArt_Init_" + sNavBarVarName + "();', 50);");
        
        WriteStartupScript(output, this.DemarcateClientScript(oStartupSB.ToString()));
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
      string sPostBackID;

      // if there is only one argument, it is the select id...
      if(arArguments.Length < 2)
      {
        sCommand = "SELECT";
        sPostBackID = stringArgument;
      }
      else
      {
        sCommand = arArguments[0];
        sPostBackID = arArguments[1];
      }

      NavBarItem oItem = (NavBarItem)(this.FindNodeByPostBackId(sPostBackID, this.Items));

      if(oItem == null)
      {
        throw new Exception("Item " + sPostBackID + " not found.");
      }

			// should we validate the page?
			if (Utils.ConvertInheritBoolToBool(oItem.CausesValidation, this.CausesValidation))
			{
				Page.Validate();
			}

      NavBarItemEventArgs oArgs = new NavBarItemEventArgs();
      oArgs.Command = sCommand;
      oArgs.Item = oItem;

      if(sCommand == "SELECT")
      {
        this.selectedNode = oArgs.Item;

        this.OnItemSelected(oArgs);

        // If the selected node has a navurl, redirect to it.
        if(oArgs.Item.NavigateUrl != string.Empty)
        {
          oArgs.Item.Navigate();
        }
      }
      else if(sCommand == "EXPAND")
      {
        oItem.Expanded = true;
        this.OnItemExpanded(oArgs);
      }
      else if(sCommand == "COLLAPSE")
      {
        oItem.Expanded = false;
        this.OnItemCollapsed(oArgs);
      }
      else
      {
        throw new Exception("Unknown postback command.");
      }
		}

    protected override bool IsDownLevel()
    {
      if(this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }


      string sUserAgent = Context.Request.UserAgent;

      if (sUserAgent == null || sUserAgent == "") return true;

      int iMajorVersion = 0;
      try
      {
        iMajorVersion = Context.Request.Browser.MajorVersion;
      }
      catch {}

      if( // We are good if:

        // 0. We have the W3C Validator
        (sUserAgent.IndexOf("Validator") >= 0) ||

        // 1. We have IE 5 or greater on a non-Mac
        (sUserAgent.IndexOf("MSIE") >= 0 && iMajorVersion >= 5 && !Context.Request.Browser.Platform.ToUpper().StartsWith("MAC")) ||

        // 2. We have Gecko-based browser (Netscape 6+, Mozilla, FireFox)
        (sUserAgent.IndexOf("Gecko") >= 0) ||

        // 3. We have Opera 7 or later
        (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion >= 7) ||

        // 4. We have Safari
        (sUserAgent.IndexOf("Safari") >= 0)
        )
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    protected override void LoadPreloadImages()
    {
      base.LoadPreloadImages();

      string [] arProperties = new string [] {
                                               this.ScrollDownActiveImageUrl, 
                                               this.ScrollDownHoverImageUrl, 
                                               this.ScrollDownImageUrl, 
                                               this.ScrollUpActiveImageUrl, 
                                               this.ScrollUpHoverImageUrl, 
                                               this.ScrollUpImageUrl };

      // if its an image, add to preloadimages
      foreach(string sValue in arProperties)
      {
        if(sValue != null && sValue != string.Empty)
        {
          string sPreloadImage = ConvertImageUrl(sValue);

          // add sValue to menu.preloadimages if not already there
          if(!this.PreloadImages.Contains(sPreloadImage))
          {
            this.PreloadImages.Add(sPreloadImage);
          } 
        }
      }
    }

    protected override void LoadViewState(object state)
    {
      base.LoadViewState(state);

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

    #region Public Methods

    public NavBar()
    {
      this.nodes = new NavBarItemCollection(this, null);
    }

    public void ApplyTheming(bool? overwriteSettings)
    {
      bool overwrite = overwriteSettings ?? false;
      string prefix = this.AutoThemingCssClassPrefix ?? "";

      // Base
      if ((this.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.CssClass = prefix + "navbar";
      }
      if ((this.DefaultGroupCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultGroupCssClass = prefix + "navbar-group";
      }
      if ((this.DefaultItemLookId ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemLookId = "NavBarItemLook";
      }
      if ((this.DefaultSelectedItemLookId ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultSelectedItemLookId = "NavBarSelectedItemLook";
      }
      if ((this.DefaultDisabledItemLookId ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultDisabledItemLookId = "NavBarDisabledItemLook";
      }
      if (Properties["ExpandSinglePath"] == null || overwrite)
      {
        Properties["ExpandSinglePath"] = true.ToString();
      }
      if ((!this.Height.IsEmpty && Properties["FullExpand"] == null) || overwrite)
      {
        Properties["FullExpand"] = true.ToString();
      }

      // Default ItemLook
      ItemLook navbarItemLook = new ItemLook();
      navbarItemLook.LookId = "NavBarItemLook";

      if (this.ItemLooks.Count > 0)
      {
        foreach (ItemLook itemLook in this.ItemLooks)
        {
          if (itemLook.LookId == navbarItemLook.LookId)
          {
            itemLook.CopyTo(navbarItemLook);
            this.ItemLooks.Remove(itemLook);
            break;
          }
        }
      }

      if ((navbarItemLook.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        navbarItemLook.CssClass = prefix + "item-default";
      }
      if ((navbarItemLook.HoverCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        navbarItemLook.HoverCssClass = prefix + "item-hover";
      }
      if ((navbarItemLook.ExpandedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        navbarItemLook.ExpandedCssClass = prefix + "item-expanded";
      }
      if ((navbarItemLook.ActiveCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        navbarItemLook.ActiveCssClass = prefix + "item-active";
      }
      this.ItemLooks.Add(navbarItemLook);

      // selected ItemLook
      navbarItemLook = new ItemLook();
      navbarItemLook.LookId = "NavBarSelectedItemLook";
      navbarItemLook.CssClass = prefix + "item-selected" + " " + prefix + "item-default";
      navbarItemLook.HoverCssClass = prefix + "item-selected" + " " + prefix + "item-hover";
      navbarItemLook.ExpandedCssClass = prefix + "item-selected" + " " + prefix + "item-expanded";
      navbarItemLook.ActiveCssClass = prefix + "item-selected" + " " + prefix + "item-active";
      this.ItemLooks.Add(navbarItemLook);

      // disabled look
      navbarItemLook = new ItemLook();
      navbarItemLook.LookId = "NavBarDisabledItemLook";
      navbarItemLook.CssClass = prefix + "item-disabled " + prefix + "item-default";
      navbarItemLook.HoverCssClass = prefix + "item-disabled " + prefix + "item-hover";
      navbarItemLook.ExpandedCssClass = prefix + "item-disabled " + prefix + "item-expanded";
      navbarItemLook.ActiveCssClass = prefix + "item-disabled " + prefix + "item-active";      
      this.ItemLooks.Add(navbarItemLook);


      // Client Templates
      StringBuilder templateText = new StringBuilder();
      templateText.Append("<div class=\"");
      templateText.Append(prefix);
      templateText.Append(@"navbar-top-item"">
						<a href=""## DataItem.getProperty('NavigateUrl') == '' ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##"" onclick=""this.blur();"">
							<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"outer"">
								<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"inner## DataItem.getProperty('IconCssClass') == null ? '' : ' ' +  DataItem.getProperty('IconCssClass'); ##"">## DataItem.getProperty('Text'); ##</span>
							</span>
						</a>
					</div>");
      AddClientTemplate(overwrite, "TopLevelNavBarItemTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"");
      templateText.Append(prefix);
      templateText.Append(@"navbar-sub-item"">
						<a href=""## DataItem.getProperty('NavigateUrl') == '' ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##"" onclick=""this.blur();"">
							<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"outer"">
								<span class=""");
      templateText.Append(prefix);
      templateText.Append(@"inner## DataItem.getProperty('IconCssClass') == null ? '' : ' ' +  DataItem.getProperty('IconCssClass'); ##"">## DataItem.getProperty('Text'); ##</span>
							</span>
						</a>
					</div>");
      AddClientTemplate(overwrite, "SubLevelNavBarItemTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "navbar-top-item " + prefix + "navbar-top-item-left-icon\">");
      templateText.Append("<a href=\"## DataItem.getProperty('NavigateUrl') == '' ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##\" onclick=\"this.blur();\">");
      templateText.Append("<span class=\"" + prefix + "outer\">");
      templateText.Append("<span class=\"" + prefix + "inner\">");
      templateText.Append("<img src=\"" + Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl) + "## DataItem.getProperty('IconUrl'); ##\" width=\"16\" height=\"16\" border=\"0\" />");
      templateText.Append("<span class=\"" + prefix + "text\">## DataItem.getProperty('Text'); ##</span>");
      templateText.Append("</span></span></a></div>");
      AddClientTemplate(overwrite, "TopLevelNavBarItemLeftIconTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "navbar-sub-item " + prefix + "navbar-sub-item-left-icon\">");
      templateText.Append("<a href=\"## DataItem.getProperty('NavigateUrl') == '' ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##\" onclick=\"this.blur();\">");
      templateText.Append("<span class=\"" + prefix + "outer\">");
      templateText.Append("<span class=\"" + prefix + "inner\">");
      templateText.Append("<img src=\"" + Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl) + "## DataItem.getProperty('IconUrl'); ##\" width=\"16\" height=\"16\" border=\"0\" />");
      templateText.Append("<span class=\"" + prefix + "text\">## DataItem.getProperty('Text'); ##</span>");
      templateText.Append("</span></span></a></div>");
      AddClientTemplate(overwrite, "SubLevelNavBarItemLeftIconTemplate", templateText.ToString());

      // Apply client templates to Items
      ApplyThemedClientTemplatesToItems(overwrite, this.Items);
    }

    private void ApplyThemedClientTemplatesToItems(bool overwrite, NavBarItemCollection items)
    {
      foreach (NavBarItem item in items)
      {
        if (item.ClientTemplateId == string.Empty || overwrite)
        {
          item.ClientTemplateId = "SubLevelNavBarItemTemplate";

          if (item.Depth < 1)
          {
            item.ClientTemplateId = "TopLevelNavBarItemTemplate";
          }

          if (item.Text == string.Empty && item.Depth < 1)
          {
            item.ClientTemplateId = string.Empty;
          }

          if (item.Properties["IconUrl"] != null)
          {
            item.ClientTemplateId = "SubLevelNavBarItemLeftIconTemplate";

            if (item.Depth < 1)
            {
              item.ClientTemplateId = "TopLevelNavBarItemLeftIconTemplate";
            }
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

    /// <summary>
    /// Find the NavBarItem with the given ID.
    /// </summary>
    /// <param name="sNodeID">The ID to search for.</param>
    /// <returns>The found node or null.</returns>
    public new NavBarItem FindItemById(string sNodeID)
    {
      return (NavBarItem)base.FindNodeById(sNodeID);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Put together a client-script string representation of this TreeView.
    /// </summary>
    /// <returns></returns>
    private string BuildStorage()
    {
      NavBarItemCollection arItems;

      if(this.RenderRootNodeId != string.Empty)
      {
        NavBarItem oRootNode = this.FindItemById(this.RenderRootNodeId);

        if(oRootNode == null)
        {
          throw new Exception("No node found with ID \"" + this.RenderRootNodeId + "\".");
        }
        else
        {
          if(this.RenderRootNodeInclude)
          {
            NavBarItemCollection oRootNodes = new NavBarItemCollection(this, null);
            oRootNodes.Add(oRootNode);
            arItems = oRootNodes;
          }
          else
          {
            arItems = oRootNode.Items;	
          }
        }
      }
      else
      {
        arItems = this.Items;
      }

      JavaScriptArray arNodeList = new JavaScriptArray();

      foreach(NavBarItem oItem in arItems)
      {
        ProcessNode(oItem, arNodeList, -1, 1);
      }

      return arNodeList.ToString();
    }

    /// <summary>
    /// Go through the TabStrip ItemLooks, and build a javascript array representing their data.
    /// </summary>
    private string BuildLooks()
    {
      JavaScriptArray renderLookList = new JavaScriptArray();
      foreach (ItemLook look in this.ItemLooks)
      {
        renderLookList.Add(base.ProcessLook(look));
      }
      return renderLookList.ToString();
    }

    /// <summary>
    /// Go through the NavBar nodes, determining if default styles are needed anywhere, and if so, apply them.
    /// Returns whether any default styles were applied.
    /// </summary>
    private bool ConsiderDefaultStylesRecurse(NavBarItemCollection arItems)
    {
      bool bNeedDefaults = false;

      foreach(NavBarItem oItem in arItems)
      {
        // is this item in need of default styles?
        if( this.CssClass == string.Empty && (this.DefaultGroupCssClass == null || this.DefaultGroupCssClass == string.Empty) &&
          oItem.Look.IsEmpty && oItem.SelectedLook.IsEmpty && (oItem.ParentItem == null || oItem.ParentItem._defaultStyle || oItem.ParentItem.SubGroupCssClass == null || oItem.ParentItem.SubGroupCssClass == string.Empty) )
        {
          bNeedDefaults = true;

          // apply default styles to this item
          oItem._defaultStyle = true;
          if(oItem.nodes != null && oItem.Items.Count > 0)
          {
            if(!this.IsRunningInDesignMode())
            {
              oItem.Look.CssClass = "cnb_TopItem";
              oItem.SubGroupCssClass = "cnb_Group";
              oItem.Look.HoverCssClass = "cnb_TopItemHover";
              oItem.Look.ActiveCssClass = "cnb_TopItemActive";
            }
            else
            {
              oItem.Attributes.Add("style", "background-color:#3F3F3F;color:white;font-family:verdana;font-size:12px;border:1px;border-color:#000000;border-top-color:#808080;border-left-color:#808080;border-style:solid;cursor:pointer;");
            }
            
            oItem.Look.LabelPaddingLeft = 8;
            oItem.Look.LabelPaddingRight = 10;
            oItem.Look.LabelPaddingTop = 3;
            oItem.Look.LabelPaddingBottom = 3;
          }
          else
          {
            if(!this.IsRunningInDesignMode())
            {
              oItem.Look.CssClass = "cnb_Item";
              oItem.Look.HoverCssClass = "cnb_ItemHover";
              oItem.Look.ActiveCssClass = "cnb_ItemActive";

              oItem.SelectedLook.CssClass = "cnb_ItemActive";
              oItem.SelectedLook.HoverCssClass = "cnb_ItemActive";
              oItem.SelectedLook.ActiveCssClass = "cnb_ItemActive";
              oItem.SelectedLook.LabelPaddingLeft = 15;
              oItem.SelectedLook.LabelPaddingRight = 10;
              oItem.SelectedLook.LabelPaddingTop = 3;
              oItem.SelectedLook.LabelPaddingBottom = 3;
            }
            else
            {
              oItem.Attributes.Add("style", "background-color:#EEEEEE;color:#333333;font-family:verdana;font-size:11px;border:solid 1px #EEEEEE;border-style:solid;cursor:pointer;");
            }

            oItem.Look.LabelPaddingLeft = 15;
            oItem.Look.LabelPaddingRight = 10;
            oItem.Look.LabelPaddingTop = 3;
            oItem.Look.LabelPaddingBottom = 3;
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

      bool needsDefaultStyles = this.NeedsDefaultStyles();
      bool renderDefaultStyles = needsDefaultStyles || this.RenderDefaultStyles;
      if(!renderDefaultStyles)
      {
        return false;
      }		  
      if(ConsiderDefaultStylesRecurse(this.Items) | this.RenderDefaultStyles) /* purposely not using short-circuit OR */ 
      {
        if(!this.IsRunningInDesignMode())
        {
          // apply cssclass to the control
          this.CssClass = "cnb_TopGroup";
        }
        else
        {
          this.Attributes.Add("style", "background-color:#EEEEEE;border:1px;border-color:black;border-top-color:gray;border-left-color:gray;border-style:solid;");
        }

        return true;
      }

      return false;
    }

    /// <summary>
    /// Whether we should render the control with the default design
    /// </summary>
    internal bool NeedsDefaultStyles()
    {
      bool controlHasNoLooksDefined = this.ItemLooks.Count == 0;
      bool controlHasNoGroupClassesDefined = this.DefaultGroupCssClass == null || this.DefaultGroupCssClass == string.Empty;
      return controlHasNoLooksDefined && controlHasNoGroupClassesDefined;
    }

    /// <summary>
    /// Put together client script that registers all keyboard shortcuts contained in the given tree structure.
    /// </summary>
    /// <param name="sNavBarName">Client-side NavBar object identifier.</param>
    /// <param name="arItemList">Root items to begin searching from.</param>
    /// <param name="oSB">StringBuilder to add content to.</param>
    private void GenerateKeyShortcutScript(string sNavBarName, NavBarItemCollection arItemList, StringBuilder oSB)
    {
      if(arItemList != null)
      {
        foreach(NavBarItem oItem in arItemList)
        {
          if(oItem.KeyboardShortcut != string.Empty)
          {
            oSB.Append("ComponentArt_RegisterKeyHandler(" + sNavBarName + ",'" + oItem.KeyboardShortcut + "', '" + sNavBarName + ".SelectItemById(\\'" + oItem.PostBackID + "\\',true)');\n");
          }

          GenerateKeyShortcutScript(sNavBarName, oItem.Items, oSB);
        }
      }
    }

    /// <summary>
    /// Process a node in the process of building client-side storage.
    /// </summary>
    /// <param name="oItem">The node to process.</param>
    /// <param name="arNodeList">List to add processed nodes to, including this one.</param>
    /// <param name="iParentIndex">Index of the given node's parent in the storage list.</param>
    /// <param name="depth">The depth of this node in the tree structure.</param>
    /// <returns>The index in the array of the newly added node.</returns>
    private int ProcessNode(NavBarItem oItem, ArrayList arNodeList, int iParentIndex, int depth)
    {
      ArrayList arNodeItems = new ArrayList();
      int iNewNodeIndex = arNodeList.Count;
      arNodeList.Add(arNodeItems);

      arNodeItems.Add(oItem.PostBackID); // 'PostBackID'
			arNodeItems.Add(iParentIndex); // 'ParentIndex'

      ArrayList arChildIndexes = new ArrayList();
      if(oItem.nodes != null && this.RenderDrillDownDepth == 0 || this.RenderDrillDownDepth > depth)
      {
        foreach(NavBarItem oChildNode in oItem.Items)
        {
          arChildIndexes.Add(ProcessNode(oChildNode, arNodeList, iNewNodeIndex, depth + 1));
        }
      }
      arNodeItems.Add(arChildIndexes); // 'ChildIndexes'
      
      ArrayList itemProperties = new ArrayList();
      foreach (string propertyName in oItem.Properties.Keys)
      {
        switch (oItem.GetVarAttributeName(propertyName).ToLower(System.Globalization.CultureInfo.InvariantCulture))
        {
          case "autopostbackoncollapse": itemProperties.Add(new object [] {0, oItem.AutoPostBackOnCollapse}); break;
          case "autopostbackonexpand": itemProperties.Add(new object [] {1, oItem.AutoPostBackOnExpand}); break;
          case "autopostbackonselect": itemProperties.Add(new object [] {2, oItem.AutoPostBackOnSelect}); break;
          case "causesvalidation": itemProperties.Add(new object [] {3, oItem.CausesValidation}); break;
          case "childselectedlookid": itemProperties.Add(new object [] {4, oItem.ChildSelectedLookId}); break;
          case "clientsidecommand": itemProperties.Add(new object [] {5, oItem.ClientSideCommand}); break;
          case "clienttemplateid": itemProperties.Add(new object [] {6, oItem.ClientTemplateId}); break;
          case "defaultsubgroupcssclass": itemProperties.Add(new object [] {7, oItem.DefaultSubGroupCssClass}); break;
          case "defaultsubitemchildselectedlookid": itemProperties.Add(new object [] {8, oItem.DefaultSubItemChildSelectedLookId}); break;
          case "defaultsubitemdisabledlookid": itemProperties.Add(new object [] {9, oItem.DefaultSubItemDisabledLookId}); break;
          case "defaultsubitemlookid": itemProperties.Add(new object [] {10, oItem.DefaultSubItemLookId}); break;
          case "defaultsubitemselectedlookid": itemProperties.Add(new object [] {11, oItem.DefaultSubItemSelectedLookId}); break;
          case "defaultsubitemtextalign": itemProperties.Add(new object [] {12, oItem.DefaultSubItemTextAlign}); break;
          case "defaultsubitemtextwrap": itemProperties.Add(new object [] {13, oItem.DefaultSubItemTextWrap}); break;
          case "disabledlookid": itemProperties.Add(new object [] {14, oItem.DisabledLookId}); break;
          case "enabled": itemProperties.Add(new object [] {15, oItem.Enabled}); break;
          case "expanded": itemProperties.Add(new object [] {16, oItem.Expanded}); break;
          case "height": itemProperties.Add(new object [] {17, oItem.Height}); break;
          case "id": itemProperties.Add(new object [] {18, oItem.ID}); break;
          case "keyboardshortcut": itemProperties.Add(new object [] {19, oItem.KeyboardShortcut}); break;
          case "lookid": itemProperties.Add(new object [] {20, oItem.LookId}); break;
          case "navigateurl": itemProperties.Add(new object [] {21, Utils.MakeStringXmlSafe(oItem.NavigateUrl)}); break;
          case "pageviewid": itemProperties.Add(new object [] {22, oItem.PageViewId}); break;
          case "selectable": itemProperties.Add(new object [] {23, oItem.Selectable}); break;
          case "selectedlookid": itemProperties.Add(new object [] {24, oItem.SelectedLookId}); break;
          case "servertemplateid": itemProperties.Add(new object [] {25, oItem.ServerTemplateId}); break;
          case "sitemapxmlfile": itemProperties.Add(new object [] {26, oItem.SiteMapXmlFile}); break;
          case "subgroupcssclass": itemProperties.Add(new object [] {27, oItem.SubGroupCssClass}); break;
          case "subgroupheight": itemProperties.Add(new object [] {28, oItem.SubGroupHeight}); break;
          case "subgroupitemspacing": itemProperties.Add(new object [] {29, oItem.SubGroupItemSpacing}); break;
          case "target": itemProperties.Add(new object [] {30, oItem.Target}); break;
          case "text": itemProperties.Add(new object [] {31, Utils.MakeStringXmlSafe(oItem.Text)}); break;
          case "textalign": itemProperties.Add(new object [] {32, oItem.TextAlign}); break;
          case "textwrap": itemProperties.Add(new object [] {33, oItem.TextWrap}); break;
          case "tooltip": itemProperties.Add(new object [] {34, Utils.MakeStringXmlSafe(oItem.ToolTip)}); break;
          case "value": itemProperties.Add(new object [] {35, Utils.MakeStringXmlSafe(oItem.Value)}); break;
          case "visible": itemProperties.Add(new object [] {36, oItem.Visible}); break;
          default:
            if (this.OutputCustomAttributes)
            {
              itemProperties.Add(new object [] {oItem.GetVarAttributeName(propertyName), Utils.MakeStringXmlSafe(oItem.Properties[propertyName])});
            }
          break;
        }
      }
      arNodeItems.Add(itemProperties);      

      return iNewNodeIndex;
    }

    /// <summary>
    /// Build a comma-delimited list of expanded item ID's.
    /// </summary>
    /// <param name="arItems">Root items to start building from.</param>
    /// <returns>The list in a string.</returns>
    private string GetExpanded(NavBarItemCollection arItems)
    {
      if(arItems != null)
      {
        ArrayList oStringList = new ArrayList();
			
        foreach(NavBarItem oItem in arItems)
        {
          if(oItem.Expanded)
          {
            oStringList.Add(oItem.PostBackID);
          }

          string sSubExpanded = GetExpanded(oItem.Items);
				
          if(sSubExpanded != string.Empty)
          {
            oStringList.Add(sSubExpanded);
          }
        }

        string [] arArray = (string [])(oStringList.ToArray(typeof(System.String)));
        return string.Join(",", arArray);
      }
      else
        return string.Empty;
    }

    /// <summary>
    /// Make sure the items contained in the given list are expanded, and only those.
    /// </summary>
    /// <param name="arItems">Root items to begin working from.</param>
    /// <param name="arExpandedList">List of PostBackID's of expanded items.</param>
    private void SetExpanded(NavBarItemCollection arItems, ArrayList arExpandedList)
    {
      if(arItems != null)
      {
        foreach(NavBarItem oItem in arItems)
        {
          // Only set if value is different
          bool bNewValue = arExpandedList.Contains(oItem.PostBackID);
          if(oItem.Expanded != bNewValue)
          {
            oItem.Expanded = bNewValue;
          }

          SetExpanded(oItem.Items, arExpandedList);
        }
      }
    }

    #endregion

    #region Accessible Rendering

    internal void RenderAccessibleContent(HtmlTextWriter output)
    {
      output.Write("<span class=\"navbar\">");
      int itemIndex = -1;
      RenderAccessibleGroup(output, null, ref itemIndex);
      output.Write("</span>");
    }

    private void RenderAccessibleGroup(HtmlTextWriter output, NavBarItem parentItem, ref int itemIndex)
    {
      output.Write("<ul");

      string groupId = "G" + this.GetSaneId() + "_" + itemIndex.ToString();
      output.Write(" id=\"");
      output.Write(groupId);
      output.Write("\"");

      // CSS class
      string groupCssClass = parentItem != null ? parentItem.SubGroupCssClass : this.DefaultGroupCssClass;
      if (groupCssClass != String.Empty && groupCssClass != null)
      {
        output.Write(" class=\"");
        output.Write(groupCssClass);
        output.Write("\"");
      }

      output.Write(">");

      foreach (NavBarItem item in (parentItem == null ? this.Items : parentItem.Items))
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
        string rightIconStyle = (item.EffectiveLook.RightIconUrl != null) ?
          ("background-image:url(" + ConvertImageUrl(item.EffectiveLook.RightIconUrl) + ");background-repeat:no-repeat;background-position:right center;") : null;
        string itemWidthStyle = (item.Width != Unit.Empty) ? ("width:" + item.Width.ToString() + ";") : null;
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
        string leftIconStyle = (item.EffectiveLook.LeftIconUrl != null) ?
          ("background-image:url(" + ConvertImageUrl(item.EffectiveLook.LeftIconUrl) + ");background-repeat:no-repeat;background-position:left center;") : null;
        if (leftIconStyle != null)
        {
          output.Write(" style=\"");
          output.Write(leftIconStyle);
          output.Write("\"");
        }
        output.Write(">");

        output.Write("<span");
        int leftIconPadding = (int)item.EffectiveLook.LeftIconWidth.Value;
        int leftTextPadding = (int)item.EffectiveLook.LabelPaddingLeft.Value;
        string leftPaddingStyle = "padding-left:" + (leftIconPadding + leftTextPadding) + "px;";
        int rightIconPadding = (int)item.EffectiveLook.RightIconWidth.Value;
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

    #endregion Accessible Rendering

    #region Down-level Rendering

    internal void RenderDownLevelContent(HtmlTextWriter output)
    {
      // Output div
      output.Write("<div");
      output.WriteAttribute("id", this.ClientID);
      if(this.CssClass != string.Empty)
      {
        output.WriteAttribute("class", this.CssClass);
      }
      if(this.ToolTip != string.Empty)
      {
        output.WriteAttribute("title", this.ToolTip);
      }

      // Output style
      output.Write(" style=\"");
      output.WriteStyleAttribute("height", this.Height.ToString());
      output.WriteStyleAttribute("width", this.Width.ToString());
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
      output.Write("\">");

      NavBarItemCollection arItems;

      if(this.RenderRootItemId != string.Empty)
      {
        NavBarItem oRootItem = this.FindItemById(this.RenderRootItemId);

        if(oRootItem == null)
        {
          throw new Exception("No item found with specified RenderRootItemId.");
        }
        else
        {
          if(this.RenderRootItemInclude)
          {
            NavBarItemCollection oRootItems = new NavBarItemCollection(this, null);
            oRootItems.Add(oRootItem);
            arItems = oRootItems;
          }
          else
          {
            arItems = oRootItem.Items;
          }
        }
      }
      else
      {
        arItems = this.Items;
      }

		  RenderDownLevelItems(output, arItems);

      output.Write("</div>");
    }

    private void RenderDownLevelItems(HtmlTextWriter output, NavBarItemCollection arItems)
    {
      foreach(NavBarItem oItem in arItems)
      {
        string sHref =
          (Context != null && oItem.NavigateUrl == string.Empty) ?
          "javascript:" + Page.GetPostBackEventReference(this,
            (oItem.nodes != null && oItem.nodes.Count > 0 ?
          ((oItem.Expanded ? "COLLAPSE " : "EXPAND ")+ oItem.PostBackID) : oItem.PostBackID)) : oItem.NavigateUrl;

        output.AddAttribute("cellpadding", "0");
        output.AddAttribute("cellspacing", "0");
        output.AddAttribute("width", "100%");
        if(oItem.EffectiveLook.CssClass != null)
        {
          output.AddAttribute("class", oItem.EffectiveLook.CssClass);
        }

	      // do we need to specify item height?
        if(!oItem.Height.IsEmpty)
        {
          output.AddAttribute("height", oItem.Height.ToString());
        }
	
        if(oItem.ToolTip != string.Empty)
        {
          output.AddAttribute("title", oItem.ToolTip);
        }

        if(this.RenderDefaultStyles && this.IsRunningInDesignMode())
        {
          output.AddAttribute("style", oItem.Attributes["style"]);
        }
        output.RenderBeginTag(HtmlTextWriterTag.Table); // <table>
        output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
	
        // do we have a template?
        if(oItem.TemplateId != string.Empty)
        {
          string sTemplateId = this.ClientID + "_" + oItem.PostBackID;

          // we have a template for this item; find it.
          foreach(Control oControl in this.Controls)
          {
            if(oControl.ID == sTemplateId)
            {
              oControl.RenderControl(output);
            }
          }
        }
        else
        {
          // do we have a left icon?
          if(oItem.EffectiveLook.LeftIconUrl != null && oItem.EffectiveLook.LeftIconUrl != string.Empty)
          {
            output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            
            // We'll have to perform a postback on click for each node, if possible.
            if(Context != null)
            {
              output.AddAttribute("href", sHref);
              if(oItem.Target != string.Empty)
              {
                output.AddAttribute("target", oItem.Target);
              }
            }
            output.RenderBeginTag(HtmlTextWriterTag.A); // <a>

            output.AddAttribute("src", ConvertImageUrl(oItem.EffectiveLook.LeftIconUrl));
            output.AddAttribute("border", "0");
            output.AddAttribute("alt", "");
            output.RenderBeginTag(HtmlTextWriterTag.Img); // <img>
            output.RenderEndTag(); // </img>
          
            output.RenderEndTag(); // </a>

            output.RenderEndTag(); // </td>
          }
	  
          // do we have an image?
          if(oItem.EffectiveLook.ImageUrl != null && oItem.EffectiveLook.ImageUrl != string.Empty)
          {
            output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            
            // We'll have to perform a postback on click for each node, if possible.
            if(Context != null)
            {
              output.AddAttribute("href", sHref);
              if(oItem.Target != string.Empty)
              {
                output.AddAttribute("target", oItem.Target);
              }
            }
            output.RenderBeginTag(HtmlTextWriterTag.A); // <a>

            output.AddAttribute("src", ConvertImageUrl(oItem.EffectiveLook.ImageUrl));
            output.AddAttribute("border", "0");
            output.AddAttribute("alt", "");
            output.RenderBeginTag(HtmlTextWriterTag.Img); // <img>
            output.RenderEndTag(); // </img>
          
            output.RenderEndTag(); // </a>

            output.RenderEndTag(); // </td>
            output.RenderEndTag(); // </tr>
            output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
          }
	  
          // Apply padding...
          output.AddStyleAttribute("padding-top", oItem.EffectiveLook.LabelPaddingTop.IsEmpty? "0" : oItem.EffectiveLook.LabelPaddingTop.ToString());
          output.AddStyleAttribute("padding-left", oItem.EffectiveLook.LabelPaddingLeft.IsEmpty? "0" : oItem.EffectiveLook.LabelPaddingLeft.ToString());
          output.AddStyleAttribute("padding-right", oItem.EffectiveLook.LabelPaddingRight.IsEmpty? "0" : oItem.EffectiveLook.LabelPaddingRight.ToString());
          output.AddStyleAttribute("padding-bottom", oItem.EffectiveLook.LabelPaddingBottom.IsEmpty? "0" : oItem.EffectiveLook.LabelPaddingBottom.ToString());
          
          output.AddAttribute("width", "100%");
          output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

          // We'll have to perform a postback on click for each node, if possible.
          if(Context != null)
          {
            output.AddAttribute("href", sHref);
            if(oItem.Target != string.Empty)
            {
              output.AddAttribute("target", oItem.Target);
            }
          }
          output.RenderBeginTag(HtmlTextWriterTag.A); // <a>

          output.Write("<nobr>" + oItem.Text + "</nobr>");

          output.RenderEndTag(); // </a>

          output.RenderEndTag(); // </td>

          // do we have a right icon?
          if(oItem.EffectiveLook.RightIconUrl != null && oItem.EffectiveLook.RightIconUrl != string.Empty)
          {
            output.AddStyleAttribute("padding-left", "0");
            output.AddStyleAttribute("padding-right", "0");
            output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

            output.AddAttribute("src", ConvertImageUrl(oItem.EffectiveLook.RightIconUrl));
            output.AddAttribute("border", "0");
            output.AddAttribute("alt", "");
            output.RenderBeginTag(HtmlTextWriterTag.Img); // <img>
            output.RenderEndTag(); // </img>
            
            output.RenderEndTag(); // </td>
          }
        }
	
        output.RenderEndTag(); // </tr>
        output.RenderEndTag(); // </table>

        // sub-items
        if(oItem.Expanded && oItem.nodes != null && oItem.Items.Count > 0)
        {
          output.AddStyleAttribute("width", "100%");
          if(this.RenderDefaultStyles && this.IsRunningInDesignMode())
          {
            output.AddAttribute("style", "background-color:#EEEEEE;border:1px;border-color:#EEEEEE;border-style:solid;");
          }
          else
          {
            if(oItem.SubGroupCssClass != string.Empty)
            {
              output.AddAttribute("class", oItem.SubGroupCssClass);
            }
          }

          output.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
          this.RenderDownLevelItems(output, oItem.Items);
          output.RenderEndTag(); // </div>
        }
        // end sub-items
	
        // optional spacing
        int iSpacing = oItem.ParentItem == null ? this.DefaultItemSpacing : oItem.ParentItem.SubGroupItemSpacing;
        if(iSpacing > 0)
        {
          output.AddStyleAttribute("overflow", "hidden");
          output.AddStyleAttribute("height", iSpacing.ToString());
          output.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
          output.RenderEndTag(); // </div>
        }
      }
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate for <see cref="ItemSelected"/> event of <see cref="NavBar"/> class.
    /// </summary>
    public delegate void ItemSelectedEventHandler(object sender, NavBarItemEventArgs e);

    /// <summary>
    /// Fires after a navbar item is selected.
    /// </summary>
    [ Description("Fires after a navbar item is selected."), 
    Category("NavBar Events") ]
    public event ItemSelectedEventHandler ItemSelected;

    private void OnItemSelected(NavBarItemEventArgs e) 
    {         
      if (ItemSelected != null) 
      {
        ItemSelected(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="ItemExpanded"/> event of <see cref="NavBar"/> class.
    /// </summary>
    public delegate void ItemExpandedEventHandler(object sender, NavBarItemEventArgs e);

    /// <summary>
    /// Fires after a navbar item is expanded.
    /// </summary>
    [ Description("Fires after a navbar item is expanded."), 
    Category("NavBar Events") ]
    public event ItemExpandedEventHandler ItemExpanded;

    private void OnItemExpanded(NavBarItemEventArgs e) 
    {         
      if (ItemExpanded != null) 
      {
        ItemExpanded(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="ItemCollapsed"/> event of <see cref="NavBar"/> class.
    /// </summary>
    public delegate void ItemCollapsedEventHandler(object sender, NavBarItemEventArgs e);

		/// <summary>
    /// Fires after a navbar item is collapsed.
    /// </summary>
    [ Description("Fires after a navbar item is collapsed."), 
    Category("NavBar Events") ]
    public event ItemCollapsedEventHandler ItemCollapsed;

    private void OnItemCollapsed(NavBarItemEventArgs e) 
    {         
      if (ItemCollapsed != null) 
      {
        ItemCollapsed(this, e);
      }   
    }


    /// <summary>
    /// Delegate for <see cref="ItemDataBound"/> event of <see cref="NavBar"/> class.
    /// </summary>
    public delegate void ItemDataBoundEventHandler(object sender, NavBarItemDataBoundEventArgs e);
		
    /// <summary>
    /// Fires after a navbar item is data bound.
    /// </summary>
    [ Description("Fires after a navbar item is data bound."),
    Category("NavBar Events") ]
    public event ItemDataBoundEventHandler ItemDataBound;

    // generic trigger
    protected override void OnNodeDataBound(NavigationNode oNode, object oDataItem) 
    {
      if (ItemDataBound != null) 
      {
        NavBarItemDataBoundEventArgs e = new NavBarItemDataBoundEventArgs();
        
        e.Item = (NavBarItem)oNode;
        e.DataItem = oDataItem;

        ItemDataBound(this, e);
      }   
    }

    #endregion
	}
}
