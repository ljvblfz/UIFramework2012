using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization; 


namespace ComponentArt.Web.UI
{
  #region EventArgs classes
  
  /// <summary>
  /// Arguments for <see cref="Snap.Callback"/> server-side event of <see cref="Snap"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class SnapCallbackEventArgs : EventArgs
  {
    /// <summary>
    /// The Snap which caused this event.
    /// </summary>
    public Snap SnapObject;

    /// <summary>
    /// Whether the Snap is minimized.
    /// </summary>
    public bool IsMinimized;

    /// <summary>
    /// Whether the Snap is collapsed.
    /// </summary>
    public bool IsCollapsed;

    /// <summary>
    /// The ID of the element in which the Snap is docked, if any.
    /// </summary>
    public string DockElement;

    /// <summary>
    /// The index of this Snap within its docking container, if there is one.
    /// </summary>
    public int DockIndex;
  }

  /// <summary>
  /// Arguments for <see cref="Snap.Dock"/> server-side event of <see cref="Snap"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class SnapDockEventArgs : SnapEventArgs
  {
    /// <summary>
    /// The ID of the dock container.
    /// </summary>
    public string Dock;

    /// <summary>
    /// The index within the docking container.
    /// </summary>
    public int DockIndex;
  }

  /// <summary>
  /// Arguments for server-side events of the <see cref="Snap"/> control.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Arguments of this type are used by the following events: <see cref="Snap.Expand"/>, <see cref="Snap.Collapse"/>,
  /// <see cref="Snap.Minimize"/>, and <see cref="Snap.UnMinimize"/>.
  /// </para>
  /// <para>
  /// <see cref="SnapDockEventArgs"/> derives from <b>SnapEventArgs</b> and serves as arguments for <see cref="Snap.Dock"/> event.
  /// </para>
  /// </remarks>
  [ToolboxItem(false)]
  public class SnapEventArgs : EventArgs
  {
    /// <summary>
    /// The Snap which caused this event.
    /// </summary>
    public Snap SnapObject;
  }

  #endregion

  #region Enumerations

  /// <summary>
  /// Specifies how <see cref="Snap"/> control adjusts its position when page is scrolled.
  /// </summary>
	public enum SnapFloatingType
	{
    /// <summary>Floating is disabled.</summary>
		None,

    /// <summary>Control floats by immediately adjusting its position when page is scrolled.</summary>
		Instant,

    /// <summary>Control floats by performing a smooth animated movement to new postion when page is scrolled.</summary>
		Smooth
	}

  /// <summary>
  /// Specifies which point of the browser window <see cref="Snap"/> control should be aligning with.
  /// </summary>
	public enum SnapAlignType
	{
    /// <summary>Default alignment.</summary>
		Default,

    /// <summary>Align to top left corner of the window.</summary>
		TopLeft,

    /// <summary>Align to middle of the top edge of the window.</summary>
		TopCentre,

    /// <summary>Align to top right corner of the window.</summary>
		TopRight,

    /// <summary>Align to bottom left corner of the window.</summary>
		BottomLeft,

    /// <summary>Align to middle of the bottom edge of the window.</summary>
		BottomCentre,

    /// <summary>Align to bottom right corner of the window.</summary>
		BottomRight,

    /// <summary>Align to middle of the left edge of the window.</summary>
		MiddleLeft,

    /// <summary>Align to centre of the window.</summary>
		MiddleCentre,

    /// <summary>Align to middle of the right edge of the window.</summary>
		MiddleRight
	}

  /// <summary>
  /// Specifies how dragging of <see cref="Snap"/> control should be animated.
  /// </summary>
	public enum SnapDraggingStyleType
	{
    /// <summary>Drag the actual Snap.</summary>
		Original,

    /// <summary>Drag a solid outline.</summary>
		SolidOutline,

    /// <summary>Drag a dashed outline.</summary>
		DashedOutline,

    /// <summary>Drag a semi-transparent copy of the Snap.</summary>
		GhostCopy,

    /// <summary>Drag a semi-transparent flat rectangle.</summary>
		TransparentRectangle,

    /// <summary>Drag a blurry shadow.</summary>
		Shadow,

    /// <summary>Disable visual dragging feedback.</summary>
		None
	}

  /// <summary>
  /// Specifies in which directions a <see cref="Snap"/> control can be dragged.
  /// </summary>
	public enum SnapDraggingType
	{
    /// <summary>Drag anywhere.</summary>
		FreeStyle,

    /// <summary>Horizontal dragging allowed only.</summary>
		Horizontal,

    /// <summary>Vertical dragging allowed only.</summary>
		Vertical,

    /// <summary>Dragging disabled.</summary>
		None
	}

  /// <summary>
  /// Specifies how to animate <see cref="Snap"/> control docking.
  /// </summary>
	public enum SnapDockingStyleType
	{
    /// <summary>Show dock as a solid outline.</summary>
		SolidOutline,

    /// <summary>Show dock as a dashed outline.</summary>
		DashedOutline,

    /// <summary>Show dock as a transparent flat rectangle.</summary>
		TransparentRectangle,

    /// <summary>Show dock by placing the Snap inside it.</summary>
		Original,

    /// <summary>Show dock as a blurry shadow.</summary>
		Shadow,

    /// <summary>Disable visual dock feedback.</summary>
		None
	}

  /// <summary>
  /// Specifies ways in which a <see cref="Snap"/> control can be resized.
  /// </summary>
  public enum SnapResizingType
	{
    /// <summary>Disable Snap resizing.</summary>
		None,

    /// <summary>Allow all types of resizing.</summary>
		FreeStyle,

    /// <summary>Allow resizing only at the corners.</summary>
		Corners,

    /// <summary>Allow only horizontal resizing.</summary>
		Horizontal,

    /// <summary>Allow only vertical resizing.</summary>
		Vertical
	}
	
  #endregion 

  #region SnapTemplateContainer

  /// <summary>
  /// Naming container used for housing templated contents of the <see cref="Snap"/> control.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The following are instantiated in <b>SnapTemplateContainer</b> instances: 
  /// <see cref="Snap.HeaderTemplate"/>, <see cref="Snap.CollapsedHeaderTemplate"/>, <see cref="Snap.ContentTemplate"/>,
  /// <see cref="Snap.FooterTemplate"/>, and <see cref="Snap.CollapsedFooterTemplate"/>.
  /// </para>
  /// <para>
  /// Not intended for direct use by developers.
  /// </para>
  /// </remarks>
	[ToolboxItem(false)]
	public class SnapTemplateContainer : Control, INamingContainer
	{
		private Snap parent;
		public SnapTemplateContainer(Snap parent)
		{
			this.parent = parent;
		}
	}

  #endregion

  #region SnapContent

  /// <summary>
  /// Class for content sections of the <see cref="Snap"/> control.
  /// </summary>
  /// <remarks>
  /// Following <b>SnapContent</b> instances are implemented by the <b>Snap</b> control:
  /// <see cref="Snap.HeaderTemplate"/>, <see cref="Snap.CollapsedHeaderTemplate"/>, <see cref="Snap.ContentTemplate"/>,
  /// <see cref="Snap.FooterTemplate"/>, and <see cref="Snap.CollapsedFooterTemplate"/>.
  /// </remarks>
  [ToolboxItem(false)]
  public class SnapContent : Control
  {

  }

  #endregion

  /// <summary>
  /// Enables user manipulation of a page fragment including collapse/expand, dock/undock, drag-and-drop.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The Snap control is a container for ASP.NET content. Contained in a Snap, content can be dragged around the
  /// page, docked inside DOM elements, expanded and collapsed, minimized, or aligned and made to float along as the page is scrolled.
  /// </para>
  /// <para>
  /// ASP.NET content is defined in <see cref="Content" />, <see cref="Header" /> and <see cref="Footer" /> sections. Dragging is controlled by
  /// the <see cref="DraggingMode" /> property and is enabled by calling the client-side StartDragging method in the onmousedown event for one
  /// of the DOM elements placed into the Snap. To define docking containers, set the <see cref="DockingContainers" />
  /// property to a comma-delimited list of DOM element IDs. Floating and alignment are controlled by the <see cref="FloatingMode" /> and
  /// <see cref="Alignment" /> properties. For expand/collapse functionality, use the client-side ToggleExpand method in an onclick
  /// handler for one of the DOM elements placed into the Snap.
  /// </para>
  /// </remarks>
  [ToolboxData("<{0}:Snap Width=100 Height=70 runat=server></{0}:Snap>")]
  [Designer(typeof(ComponentArt.Web.UI.SnapDesigner))]
  [ParseChildren(true)]
  [PersistChildren(false)]
  public sealed class Snap : WebControl, INamingContainer, IPostBackEventHandler
  {
    #region Private Properties

    private enum SnapClientBrowserType
    {
      DownLevel,
      Netscape5,
      Netscape6,
      Netscape7,
      MSIE6,
      MSIE55,
      MSIE5,
      MSIE4,
      Opera7,
      Konqueror
    }

    private SnapContent m_oHeader;
    private ITemplate m_oHeaderTemplate;
    private SnapTemplateContainer m_oHeaderTemplateContainer;

    private SnapContent m_oCollapsedHeader;
    private ITemplate m_oCollapsedHeaderTemplate;
    private SnapTemplateContainer m_oCollapsedHeaderTemplateContainer;

    private SnapContent m_oContent;
    private ITemplate m_oContentTemplate;
    private SnapTemplateContainer m_oContentTemplateContainer;

    private SnapContent m_oFooter;
    private ITemplate m_oFooterTemplate;
    private SnapTemplateContainer m_oFooterTemplateContainer;

    private SnapContent m_oCollapsedFooter;
    private ITemplate m_oCollapsedFooterTemplate;
    private SnapTemplateContainer m_oCollapsedFooterTemplateContainer;

    private int m_iAbsoluteX = int.MinValue;
    private int m_iAbsoluteY = int.MinValue;
		
    private bool m_bIsSynchronized = false;
    private string m_sParamData;		
    private SnapClientBrowserType ClientBrowser;

    #endregion

    #region Public Properties

    /// <summary>
    /// The type of alignment to apply to this Snap. Default: Default.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(SnapAlignType.Default)]
    public SnapAlignType Alignment
    {
      get 
      {
        object o = ViewState["Alignment"]; 
        return (o == null) ? SnapAlignType.Default : (SnapAlignType) o; 
      }
      set 
      {
        ViewState["Alignment"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback (and fire the server-side OnCollapse event) when this Snap is collapsed. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnCollapse
    {
      get 
      {
        object o = ViewState["AutoPostBackOnCollapse"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoPostBackOnCollapse"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback (and fire the server-side OnExpand event) when this Snap is expanded. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnExpand
    {
      get 
      {
        object o = ViewState["AutoPostBackOnExpand"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoPostBackOnExpand"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback (and fire the server-side OnDock event) when this Snap is docked. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnDock
    {
      get 
      {
        object o = ViewState["AutoPostBackOnDock"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoPostBackOnDock"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback (and fire the server-side OnMinimize event) when this Snap is minimized. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnMinimize
    {
      get 
      {
        object o = ViewState["AutoPostBackOnMinimize"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoPostBackOnMinimize"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback (and fire the server-side OnUnMinimize event) when this Snap is un-minimized. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnUnMinimize
    {
      get 
      {
        object o = ViewState["AutoPostBackOnUnMinimize"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoPostBackOnUnMinimize"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback (and fire the server-side OnCallback event) when this Snap is collapsed. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoCallBackOnCollapse
    {
      get 
      {
        object o = ViewState["AutoCallBackOnCollapse"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoCallBackOnCollapse"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback (and fire the server-side OnCallback event) when this Snap is expanded. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoCallBackOnExpand
    {
      get 
      {
        object o = ViewState["AutoCallBackOnExpand"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoCallBackOnExpand"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback (and fire the server-side OnCallback event) when this Snap is docked. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoCallBackOnDock
    {
      get 
      {
        object o = ViewState["AutoCallBackOnDock"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoCallBackOnDock"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback (and fire the server-side OnCallback event) when this Snap is minimized. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoCallBackOnMinimize
    {
      get 
      {
        object o = ViewState["AutoCallBackOnMinimize"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoCallBackOnMinimize"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback (and fire the server-side OnCallback event) when this Snap is un-minimized. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoCallBackOnUnMinimize
    {
      get 
      {
        object o = ViewState["AutoCallBackOnUnMinimize"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoCallBackOnUnMinimize"] = value;
      }
    }

    /// <summary>
    /// Whether to bring this Snap to top on mouse click. Default: true.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool BringToTopOnClick
    {
      get 
      {
        object o = ViewState["BringToTopOnClick"]; 
        return (o == null) ? true : (bool) o; 
      }
      set 
      {
        ViewState["BringToTopOnClick"] = value;
      }
    }

    private SnapClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public SnapClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new SnapClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// Domain to use for the client side cookie.
    /// </summary>
    [DefaultValue("")]
    public string ClientSideCookieDomain
    {
      get
      {
        object o = ViewState["ClientSideCookieDomain"]; 
        return (o == null) ? (Context != null ? Context.Request.Url.Host : string.Empty) : (string) o; 
      }
      set
      {
        ViewState["ClientSideCookieDomain"] = value;
      }
    }

    /// <summary>
    /// Whether to emit a client side cookie for maintaining state. Default: false.
    /// </summary>
    [DefaultValue(false)]
    public bool ClientSideCookieEnabled
    {
      get 
      {
        object o = ViewState["ClientSideCookieEnabled"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["ClientSideCookieEnabled"] = value;
      }
    }

    /// <summary>
    /// The time at which to expire the client side cookie.
    /// </summary>
    public DateTime ClientSideCookieExpires
    {
      get 
      {
        object o = ViewState["ClientSideCookieExpires"]; 
        return (o == null) ? DateTime.Today.AddDays(7): (DateTime) o; 
      }
      set 
      {
        ViewState["ClientSideCookieExpires"] = value;
      }
    }

    /// <summary>
    /// The name to give the client side cookie.
    /// </summary>
    [DefaultValue("")]
    public string ClientSideCookieName
    {
      get
      {
        object o = ViewState["ClientSideCookieName"]; 
        return (o == null) ? (Context != null ? Context.Request.Url.AbsoluteUri + "." + this.ClientID : string.Empty) : (string) o; 
      }
      set
      {
        ViewState["ClientSideCookieName"] = value;
      }
    }

    /// <summary>
    /// The name of client-side handler (function) for collapse event.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    public string ClientSideOnCollapse
    {
      get
      {
        object o = ViewState["ClientSideOnCollapse"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set
      {
        ViewState["ClientSideOnCollapse"] = value;
      }
    }

    /// <summary>
    /// The name of client-side handler (function) for dock event.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    public string ClientSideOnDock
    {
      get
      {
        object o = ViewState["ClientSideOnDock"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set
      {
        ViewState["ClientSideOnDock"] = value;
      }
    }

    /// <summary>
    /// The name of client-side handler (function) for expand event.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    public string ClientSideOnExpand
    {
      get
      {
        object o = ViewState["ClientSideOnExpand"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set
      {
        ViewState["ClientSideOnExpand"] = value;
      }
    }

    /// <summary>
    /// The duration (in milliseconds) of the collapse animation.
    /// </summary>
    public int CollapseDuration
    {
      get 
      {
        return Utils.ParseInt(ViewState["CollapseDuration"]);
      }
      set 
      {
        ViewState["CollapseDuration"] = value;
      }
    }

    /// <summary>
    /// The slide type to use for the collapse animation.
    /// </summary>
    public SlideType CollapseSlide
    {
      get 
      {
        return Utils.ParseSlideType(ViewState["CollapseSlide"]);
      }
      set 
      {
        ViewState["CollapseSlide"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for the collapse animation.
    /// </summary>
    public TransitionType CollapseTransition
    {
      get 
      {
        return Utils.ParseTransitionType(ViewState["CollapseTransition"]);
      }
      set 
      {
        ViewState["CollapseTransition"] = value;
      }
    }

    /// <summary>
    /// The string defining a custom transition filter to use for the collapse animation.
    /// </summary>
    public string CollapseTransitionCustomFilter
    {
      get
      {
        return (string)ViewState["CollapseTransitionCustomFilter"];
      }
      set
      {
        ViewState["CollapseTransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// The current (or initial) docking container.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue("")]
    public string CurrentDockingContainer
    {
      get
      {
        //this.EnsureParamsSynchronized();

        object o = ViewState["CurrentDockingContainer"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set
      {
        ViewState["CurrentDockingContainer"] = value;
      }
    }

    /// <summary>
    /// The index (order) of this Snap within the current docking container.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int CurrentDockingIndex
    {
      get
      {
        //this.EnsureParamsSynchronized();

        object o = ViewState["CurrentDockingIndex"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["CurrentDockingIndex"] = value;
      }
    }

    /// <summary>
    /// A comma-delimited list of client-side objects to use as potential containers for this Snap.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue("")]
    public string DockingContainers
    {
      get
      {
        object o = ViewState["DockingContainers"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set
      {
        ViewState["DockingContainers"] = value;
      }
    }

    /// <summary>
    /// The style to use for docking. Default: SolidOutline
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(SnapDockingStyleType.SolidOutline)]
    public SnapDockingStyleType DockingStyle
    {
      get
      {
        object o = ViewState["DockingStyle"]; 
        return (o == null) ? SnapDockingStyleType.SolidOutline : (SnapDockingStyleType) o; 
      }
      set
      {
        ViewState["DockingStyle"] = value;
      }
    }

    /// <summary>
    /// The type of dragging to allow for this Snap. Default: FreeStyle.
    /// </summary>
    /// <seealso cref="DockingContainers" />
    [Category("Behavior")]
    [DefaultValue(SnapDraggingType.FreeStyle)]
    public SnapDraggingType DraggingMode
    {
      get
      {
        object o = ViewState["DraggingMode"]; 
        return (o == null) ? SnapDraggingType.FreeStyle : (SnapDraggingType) o; 
      }
      set
      {
        ViewState["DraggingMode"] = value;
      }
    }

    /// <summary>
    /// The style to use for dragging. Default: Original.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(SnapDraggingStyleType.Original)]
    public SnapDraggingStyleType DraggingStyle
    {
      get
      {
        object o = ViewState["DraggingStyle"]; 
        return (o == null) ? SnapDraggingStyleType.Original : (SnapDraggingStyleType) o; 
      }
      set
      {
        ViewState["DraggingStyle"] = value;
      }
    }

    /// <summary>
    /// The duration (in milliseconds) of the expand animation.
    /// </summary>
    public int ExpandDuration
    {
      get 
      {
        return Utils.ParseInt(ViewState["ExpandDuration"]);
      }
      set 
      {
        ViewState["ExpandDuration"] = value;
      }
    }

    /// <summary>
    /// The slide type to use for the expand animation.
    /// </summary>
    public SlideType ExpandSlide
    {
      get
      {
        return Utils.ParseSlideType(ViewState["ExpandSlide"]);
      }
      set
      {
        ViewState["ExpandSlide"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for the expand animation.
    /// </summary>
    public TransitionType ExpandTransition
    {
      get
      {
        return Utils.ParseTransitionType(ViewState["ExpandTransition"]);
      }
      set
      {
        ViewState["ExpandTransition"] = value;
      }
    }

    /// <summary>
    /// The string defining a custom transition filter to use for the expand animation.
    /// </summary>
    public string ExpandTransitionCustomFilter
    {
      get
      {
        return (string)ViewState["ExpandTransitionCustomFilter"];
      }
      set
      {
        ViewState["ExpandTransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// The behaviour to use when floating. Default: None.
    /// </summary>
    /// <seealso cref="Alignment" />
    [Category("Behavior")]
    [DefaultValue(SnapFloatingType.None)]
    public SnapFloatingType FloatingMode
    {
      get
      {
        object o = ViewState["FloatingMode"]; 
        return (o == null) ? SnapFloatingType.None : (SnapFloatingType) o; 
      }
      set
      {
        ViewState["FloatingMode"] = value;
      }
    }

    /// <summary>
    /// Whether this Snap is collapsed.
    /// </summary>
    [DefaultValue(false)]
    public bool IsCollapsed
    {
      get
      {
        //this.EnsureParamsSynchronized();

        object o = ViewState["IsCollapsed"]; 
        return (o == null) ? false : (bool) o;
      }
      set
      {
        ViewState["IsCollapsed"] = value;
      }
    }

    /// <summary>
    /// Whether this Snap is minimized.
    /// </summary>
    [DefaultValue(false)]
    public bool IsMinimized
    {
      get
      {
        object o = ViewState["IsMinimized"]; 
        return (o == null) ? false : (bool) o;
      }
      set
      {
        ViewState["IsMinimized"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to the minimize animation object.
    /// </summary>
    public string MinimizeCssClass
    {
      get
      {
        string s = (string)ViewState["MinimizeCssClass"]; 
        return (s == null) ? string.Empty : s; 
      }
      set
      {
        ViewState["MinimizeCssClass"] = value;
      }
    }

    /// <summary>
    /// The ID of the DOM element toward which the minimize animation should proceed.
    /// </summary>
    [Category("Behavior")]
    public string MinimizeDirectionElement
    {
      get
      {
        string s = (string)ViewState["MinimizeDirectionElement"]; 
        return (s == null) ? string.Empty : s; 
      }
      set
      {
        ViewState["MinimizeDirectionElement"] = value;
      }
    }

    /// <summary>
    /// The duration of the minimize animation.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int MinimizeDuration
    {
      get
      {
        object o = ViewState["MinimizeDuration"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["MinimizeDuration"] = value;
      }
    }

    /// <summary>
    /// The duration of the minimize animation.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(SlideType.Linear)]
    public SlideType MinimizeSlide
    {
      get
      {
        object o = ViewState["MinimizeSlide"]; 
        return (o == null) ? SlideType.Linear : (SlideType) o; 
      }
      set
      {
        ViewState["MinimizeSlide"] = value;
      }
    }

    /// <summary>
    /// The minimum height to allow for this Snap. Default: 0.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int MinHeight
    {
      get
      {
        object o = ViewState["MinHeight"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["MinHeight"] = value;
      }
    }

    /// <summary>
    /// The minimum offset from the left edge of the page to allow for this Snap. Default: 0.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int MinLeft
    {
      get
      {
        object o = ViewState["MinLeft"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["MinLeft"] = value;
      }
    }

    /// <summary>
    /// The minimum offset from the top edge of the page to allow for this Snap. Default: 0.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int MinTop
    {
      get
      {
        object o = ViewState["MinTop"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["MinTop"] = value;
      }
    }

    /// <summary>
    /// The minimum width to allow for this Snap. Default: 0.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int MinWidth
    {
      get
      {
        object o = ViewState["MinWidth"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["MinWidth"] = value;
      }
    }

    /// <summary>
    /// Whether this Snap must always be docked. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool MustBeDocked
    {
      get
      {
        object o = ViewState["MustBeDocked"]; 
        return (o == null) ? false : (bool) o; 
      }
      set
      {
        ViewState["MustBeDocked"] = value;
      }
    }

    /// <summary>
    /// Offset along the X axis when aligning. Default: 0.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    public int OffsetX
    {
      get
      {
        //this.EnsureParamsSynchronized();

        object o = ViewState["OffsetX"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["OffsetX"] = value;
      }
    }

    /// <summary>
    /// Offset along the Y axis when aligning. Default: 0.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    public int OffsetY
    {
      get
      {
        //this.EnsureParamsSynchronized();

        object o = ViewState["OffsetY"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["OffsetY"] = value;
      }
    }

    [Browsable(false)]
    private string ParamData
    {
      get
      {
        if(m_sParamData == null)
        {
          m_sParamData = m_iAbsoluteX + "," + m_iAbsoluteY + "," + this.Width + "," + this.Height + "," +
            this.CurrentDockingContainer + "," + this.CurrentDockingIndex + "," +
            (this.IsCollapsed ? "1" : "0") + "," + (this.IsMinimized ? "1" : "0");
        }

        return m_sParamData;
      }
      set
      {
        this.InitFromData(value);
      }
    }
		
    /// <summary>
    /// Whether to ensure we render over windowed objects (ie. select boxes). Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool RenderOverWindowedObjects
    {
      get
      {
        object o = ViewState["RenderOverWindowedObjects"]; 
        return (o == null) ? false : (bool) o; 
      }
      set
      {
        ViewState["RenderOverWindowedObjects"] = value;
      }
    }

    /// <summary>
    /// The behaviour to use when resizing this Snap. Default: None.
    /// </summary>
    /// <seealso cref="ResizingBorderWidth" />
    [Category("Behavior")]
    [DefaultValue(SnapResizingType.None)]
    public SnapResizingType ResizingMode
    {
      get
      {
        object o = ViewState["ResizingMode"]; 
        return (o == null) ? SnapResizingType.None : (SnapResizingType) o; 
      }
      set
      {
        ViewState["ResizingMode"] = value;
      }
    }	
		
    /// <summary>
    /// The width of the area near the edge of the Snap which triggers resizing. Default: 5.
    /// </summary>
    /// <seealso cref="ResizingMode" />
    [Category("Behavior")]
    [DefaultValue(5)]
    public int ResizingBorderWidth
    {
      get
      {
        object o = ViewState["ResizingBorderWidth"]; 
        return (o == null) ? 5 : (int) o; 
      }
      set
      {
        ViewState["ResizingBorderWidth"] = value;
      }
    }

    /// <summary>
    /// CssClass to use for the Snap when it is undocked.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue("")]
    public string UndockedCssClass
    {
      get
      {
        object o = ViewState["UndockedCssClass"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set
      {
        ViewState["UndockedCssClass"] = value;
      }
    }

    /// <summary>
    /// Snap header container.
    /// </summary>
    [
    Browsable(false),
    DefaultValue(null),
    Description("The header content container."),
    PersistenceMode(PersistenceMode.InnerProperty)
    ]
    public SnapContent Header
    {
      get
      {
        return m_oHeader;
      }
      set
      {
        m_oHeader = value;
      }
    }

    /// <summary>
    /// Template for the Snap header.
    /// </summary>
    /// <remarks>
    /// Deprecated. Use <see cref="Header"/> instead.
    /// </remarks>
    [Browsable(false)]
    [DefaultValue(null)]
    [Description("Deprecated.  Use Header instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use Header instead.", false)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(ComponentArt.Web.UI.SnapTemplateContainer))]
    public ITemplate HeaderTemplate
    {
      get
      {
        return m_oHeaderTemplate;
      }
      set
      {
        m_oHeaderTemplate = value;
      }
    }

    /// <summary>
    /// Snap collapsed header container.
    /// </summary>
    [
    Browsable(false),
    DefaultValue(null),
    Description("The collapsed header content container."),
    PersistenceMode(PersistenceMode.InnerProperty)
    ]
    public SnapContent CollapsedHeader
    {
      get
      {
        return m_oCollapsedHeader;
      }
      set
      {
        m_oCollapsedHeader = value;
      }
    }

    /// <summary>
    /// Template for the header when the Snap is collapsed.
    /// </summary>
    /// <remarks>
    /// Deprecated. Use <see cref="CollapsedHeader"/> instead.
    /// </remarks>
    [Browsable(false)]
    [DefaultValue(null)]
    [Description("Deprecated.  Use CollapsedHeader instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use CollapsedHeader instead.", false)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(ComponentArt.Web.UI.SnapTemplateContainer))]
    public ITemplate CollapsedHeaderTemplate
    {
      get
      {
        return m_oCollapsedHeaderTemplate;
      }
      set
      {
        m_oCollapsedHeaderTemplate = value;
      }
    }

    /// <summary>
    /// Snap content container.
    /// </summary>
    [
    Browsable(true),
    DefaultValue(null),
    Description("The content container."),
    PersistenceMode(PersistenceMode.InnerProperty),
    NotifyParentProperty(true),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
    ]
    public SnapContent Content
    {
      get
      {
        return m_oContent;
      }
      set
      {
        m_oContent = value;
      }
    }

    /// <summary>
    /// Template for the inner contents of the Snap.
    /// </summary>
    /// <remarks>
    /// Deprecated. Use <see cref="Content"/> instead.
    /// </remarks>
    [Browsable(false)]
    [DefaultValue(null)]
    [Description("Deprecated.  Use Content instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use Content instead.", false)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(ComponentArt.Web.UI.SnapTemplateContainer))]
    public ITemplate ContentTemplate
    {
      get
      {
        return m_oContentTemplate;
      }
      set
      {
        m_oContentTemplate = value;
      }
    }

    /// <summary>
    /// Snap footer content container.
    /// </summary>
    [
    Browsable(false),
    DefaultValue(null),
    Description("The footer content container."),
    PersistenceMode(PersistenceMode.InnerProperty)
    ]
    public SnapContent Footer
    {
      get
      {
        return m_oFooter;
      }
      set
      {
        m_oFooter = value;
      }
    }

    /// <summary>
    /// Template for the Snap footer.
    /// </summary>
    /// <remarks>
    /// Deprecated. Use <see cref="Footer"/> instead.
    /// </remarks>
    [Browsable(false)]
    [DefaultValue(null)]
    [Description("Deprecated.  Use Footer instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use Footer instead.", false)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(ComponentArt.Web.UI.SnapTemplateContainer))]
    public ITemplate FooterTemplate
    {
      get
      {
        return m_oFooterTemplate;
      }
      set
      {
        m_oFooterTemplate = value;
      }
    }

    /// <summary>
    /// Snap collapsed footer content container.
    /// </summary>
    [
    Browsable(false),
    DefaultValue(null),
    Description("The collapsed footer content container."),
    PersistenceMode(PersistenceMode.InnerProperty)
    ]
    public SnapContent CollapsedFooter
    {
      get
      {
        return m_oCollapsedFooter;
      }
      set
      {
        m_oCollapsedFooter = value;
      }
    }

    /// <summary>
    /// Template for the footer when the Snap is collapsed.
    /// </summary>
    /// <remarks>
    /// Deprecated. Use <see cref="CollapsedFooter"/> instead.
    /// </remarks>
    [Browsable(false)]
    [DefaultValue(null)]
    [Description("Deprecated.  Use CollapsedFooter instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated.  Use CollapsedFooter instead.", false)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(ComponentArt.Web.UI.SnapTemplateContainer))]
    public ITemplate CollapsedFooterTemplate
    {
      get
      {
        return m_oCollapsedFooterTemplate;
      }
      set
      {
        m_oCollapsedFooterTemplate = value;
      }
    }

    [Browsable(false)]
    public override ControlCollection Controls
    {
      get
      {
        EnsureChildControls();
        return base.Controls;
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Constructor
    /// </summary>
    public Snap() : base()
    {
      // Set some defaults.
      this.ExpandSlide = SlideType.ExponentialDecelerate;
      this.ExpandDuration = 200;
      this.CollapseSlide = SlideType.ExponentialDecelerate;
      this.CollapseDuration = 200;
    }

    public override bool IsLicensed()
    {
      return true; 
    }

    private Control FindControlRecursive(ControlCollection arControls, string sID)
    {
      foreach(Control oControl in arControls)
      {
        if(oControl.ID == sID)
        {
          return oControl;
        }

        Control oFoundControl = FindControlRecursive(oControl.Controls, sID);
        if(oFoundControl != null)
        {
          return oFoundControl;
        }
      }

      return null;
    }

    public override Control FindControl(string sID)
    {
      return FindControlRecursive(this.Controls, sID);
    }

    protected override void OnInit(EventArgs e)
    {
      this.EnsureChildControls();

      // Proceed with initialization.
      base.OnInit(e);

      if(Context != null && Page != null)
      {
        string dummy = Page.GetPostBackEventReference(this); // Ensure that __doPostBack is output to client side
      }

      // Determine browser type
      if(!this.IsDownLevel() && Context != null && Page != null)
      {
        this.ClientBrowser = this.DetermineBrowser(Page.Request);
      }
      else
      {
        this.ClientBrowser = SnapClientBrowserType.DownLevel;
      }

      // Make sure there are no spaces in our containers list
      if(this.DockingContainers != null)
      {
        this.DockingContainers = this.DockingContainers.Replace(" ", "");
      }

      try
      {
        if(this.Style["top"] != null)
        {
          m_iAbsoluteX = int.Parse(this.Style["top"].Replace("px", ""));
        }
        if(this.Style["left"] != null)
        {
          m_iAbsoluteY = int.Parse(this.Style["left"].Replace("px", ""));
        }
      }
      catch {}

      // Restore coordinates if we have some.
      this.EnsureParamsSynchronized();
    }

    protected override void OnLoad(EventArgs oArgs)
    {
      base.OnLoad(oArgs);

      string sSaneId = this.GetSaneId();

      // do we have a callback?
      if(Context != null && Context.Request.QueryString[string.Format("Cart_{0}_Callback", sSaneId)] != null)
      {
        if(this.Callback != null)
        {
          string sVarPrefix = "Cart_" + sSaneId;

          SnapCallbackEventArgs oCallbackArgs = new SnapCallbackEventArgs();
          oCallbackArgs.SnapObject = this;
          oCallbackArgs.DockElement = Context.Request.Params[sVarPrefix + "_DockElement"];
          oCallbackArgs.DockIndex = int.Parse(Context.Request.Params[sVarPrefix + "_DockIndex"]);
          oCallbackArgs.IsCollapsed = bool.Parse(Context.Request.Params[sVarPrefix + "_IsCollapsed"]);
          oCallbackArgs.IsMinimized = bool.Parse(Context.Request.Params[sVarPrefix + "_IsMinimized"]);

          this.OnCallback(oCallbackArgs);

          Context.Response.End();
        }
      }

      if (!this.IsDownLevel())
      {
        if (ScriptManager.GetCurrent(Page) != null)
        {
          if (this.ClientBrowser != SnapClientBrowserType.DownLevel)
          {
            this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
            this.RegisterScriptForAtlas("ComponentArt.Web.UI.Snap.client_scripts.A573J988.js");
            this.RegisterScriptForAtlas("ComponentArt.Web.UI.Snap.client_scripts.A573P288.js");
            this.RegisterScriptForAtlas("ComponentArt.Web.UI.Snap.client_scripts.A573U699.js");
            this.RegisterScriptForAtlas("ComponentArt.Web.UI.Snap.client_scripts.A573K688.js");
            this.RegisterScriptForAtlas("ComponentArt.Web.UI.Snap.client_scripts.A573X288.js");
            this.RegisterScriptForAtlas("ComponentArt.Web.UI.Snap.client_scripts.A573V588.js");
            this.RegisterScriptForAtlas("ComponentArt.Web.UI.Snap.client_scripts.A573W988.js");
          }
          else
          {
            this.RegisterScriptForAtlas("ComponentArt.Web.UI.Snap.client_scripts.A573T388.js");
          }
        }
      }
    }

    protected override void LoadViewState(object state)
    {
      // do nothing - we do not use viewstate!
      return;
    }

    /// <summary>
    /// Raise a postback event.
    /// </summary>
    /// <param name="eventArgument">Postback argument</param>
    public void RaisePostBackEvent(string eventArgument)
    {
      SnapEventArgs oArgs;

      string [] arArguments = eventArgument.Split(' ');

      string sCommand = arArguments[0];

      switch(sCommand)
      {
        case "DOCK":
          // Call event handler.
          SnapDockEventArgs oDockArgs = new SnapDockEventArgs();
          oDockArgs.SnapObject = this;
          oDockArgs.Dock = this.CurrentDockingContainer;
          oDockArgs.DockIndex = this.CurrentDockingIndex;
          this.OnDock(oDockArgs);
          break;
        case "EXPAND":
          this.IsCollapsed = false;

          // Call event handler.
          oArgs = new SnapEventArgs();
          oArgs.SnapObject = this;
          this.OnExpand(oArgs);
          break;
        case "COLLAPSE":
          this.IsCollapsed = true;
          
          // Call event handler.
          oArgs = new SnapEventArgs();
          oArgs.SnapObject = this;
          this.OnCollapse(oArgs);
          break;
        case "MINIMIZE":
          this.IsMinimized = true;

          // Call event handler.
          oArgs = new SnapEventArgs();
          oArgs.SnapObject = this;
          this.OnMinimize(oArgs);
          break;
        case "UNMINIMIZE":
          this.IsMinimized = false;

          // Call event handler.
          oArgs = new SnapEventArgs();
          oArgs.SnapObject = this;
          this.OnUnMinimize(oArgs);
          break;
        default:
          throw new Exception("Unknown postback command: " + sCommand);
      }
    }

    protected override void CreateChildControls()
    {
      if (this.Header != null)
      {
        System.Web.UI.WebControls.WebControl oHeaderSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oHeaderSpan.ID = "HeaderSpan";

        oHeaderSpan.Controls.Add(Header);

        Controls.Add(oHeaderSpan);
      }
      else if (this.HeaderTemplate != null)
      {
        System.Web.UI.WebControls.WebControl oHeaderSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oHeaderSpan.ID = "HeaderSpan";

        m_oHeaderTemplateContainer = new SnapTemplateContainer(this);
        this.HeaderTemplate.InstantiateIn(m_oHeaderTemplateContainer);
        oHeaderSpan.Controls.Add(m_oHeaderTemplateContainer);

        Controls.Add(oHeaderSpan);
      }
      else
      {
        // do default header template here
      }

      if (this.CollapsedHeader != null)
      {
        System.Web.UI.WebControls.WebControl oCollapsedHeaderSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oCollapsedHeaderSpan.ID = "CollapsedHeaderSpan";

        oCollapsedHeaderSpan.Controls.Add(CollapsedHeader);

        Controls.Add(oCollapsedHeaderSpan);
      }
      else if (this.CollapsedHeaderTemplate != null)
      {
        System.Web.UI.WebControls.WebControl oCollapsedHeaderSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oCollapsedHeaderSpan.ID = "CollapsedHeaderSpan";

        m_oCollapsedHeaderTemplateContainer = new SnapTemplateContainer(this);
        this.CollapsedHeaderTemplate.InstantiateIn(m_oCollapsedHeaderTemplateContainer);
        oCollapsedHeaderSpan.Controls.Add(m_oCollapsedHeaderTemplateContainer);

        Controls.Add(oCollapsedHeaderSpan);
      }
      else
      {
        // do default header template here
      }

      // Create inner panel
      System.Web.UI.WebControls.WebControl oInnerSpan =
        new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
      oInnerSpan.ID = "InnerSpan";

      if (this.Content != null)
      {
        oInnerSpan.Controls.Add(this.Content);
      }
      else if (this.ContentTemplate != null)
      {
        m_oContentTemplateContainer = new SnapTemplateContainer(this);
        this.ContentTemplate.InstantiateIn(m_oContentTemplateContainer);

        oInnerSpan.Controls.Add(m_oContentTemplateContainer);
      }
      else
      {
        // do default content template here
      }

      Controls.Add(oInnerSpan);

      if (this.Footer != null)
      {
        System.Web.UI.WebControls.WebControl oFooterSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oFooterSpan.ID = "FooterSpan";

        oFooterSpan.Controls.Add(this.Footer);

        Controls.Add(oFooterSpan);
      }
      else if (this.FooterTemplate != null)
      {
        System.Web.UI.WebControls.WebControl oFooterSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oFooterSpan.ID = "FooterSpan";

        m_oFooterTemplateContainer = new SnapTemplateContainer(this);
        this.FooterTemplate.InstantiateIn(m_oFooterTemplateContainer);
        oFooterSpan.Controls.Add(m_oFooterTemplateContainer);

        Controls.Add(oFooterSpan);
      }
      else
      {
        // do default footer template here
      }

      if (this.CollapsedFooter != null)
      {
        System.Web.UI.WebControls.WebControl oCollapsedFooterSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oCollapsedFooterSpan.ID = "CollapsedFooterSpan";

        oCollapsedFooterSpan.Controls.Add(this.CollapsedFooter);

        Controls.Add(oCollapsedFooterSpan);
      }
      else if (this.CollapsedFooterTemplate != null)
      {
        System.Web.UI.WebControls.WebControl oCollapsedFooterSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oCollapsedFooterSpan.ID = "CollapsedFooterSpan";

        m_oCollapsedFooterTemplateContainer = new SnapTemplateContainer(this);
        this.CollapsedFooterTemplate.InstantiateIn(m_oCollapsedFooterTemplateContainer);
        oCollapsedFooterSpan.Controls.Add(m_oCollapsedFooterTemplateContainer);

        Controls.Add(oCollapsedFooterSpan);
      }
      else
      {
        // do default header template here
      }
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      // we have a non-aligned element that wants to float
      if(m_iAbsoluteX != int.MinValue && m_iAbsoluteY != int.MinValue)
      {
        if(this.Alignment == SnapAlignType.Default && this.FloatingMode != SnapFloatingType.None)
        {
          // we have overriding coordinates
          this.OffsetX = m_iAbsoluteX;
          this.OffsetY = m_iAbsoluteY;
        }
        else
        {
          // we have overriding coordinates
          this.Style["left"] = m_iAbsoluteX.ToString();
          this.Style["top"] = m_iAbsoluteY.ToString();
        }
      }

      if(!this.IsDownLevel())
      {
        // Tack this to the bottom of the page, to know when we've loaded.
        RegisterStartupScript("ComponentArt_Page_Loaded", this.DemarcateClientScript("var ComponentArt_Page_Loaded = true;"));
      }
    }

    protected override void RenderContents(HtmlTextWriter output)
    {
      // render each div and its contents
      foreach(Control oControl in Controls)
      {
        output.Write("<div");
        output.WriteAttribute("id", oControl.ClientID);
        if(oControl.ID.StartsWith("Collapsed"))
        {
          output.WriteAttribute("style", "display:none;");
        }
        output.Write(">");
        foreach(Control oChildControl in oControl.Controls)
        {
          oChildControl.RenderControl(output);
        }
        output.Write("</div>");
      }
    }

    /// <summary>
    /// Render this control to the output parameter specified.
    /// </summary>
    /// <param name="output"> The HTML writer to write out to </param>
    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      string sSaneId = this.GetSaneId();

      if (!this.IsDownLevel())
      {
        RenderScripts(output);
      }

      if(this.UndockedCssClass != string.Empty)
      {
        this.CssClass = this.UndockedCssClass;
      }

      // Begin rendering...
      output.Write("<div");

      output.WriteAttribute("id", sSaneId);

      if(this.CssClass != string.Empty)
      {
        output.WriteAttribute("class", this.CssClass);
      }
      if(this.ToolTip != string.Empty)
      {
        output.WriteAttribute("title", this.ToolTip);
      }

      // Decide some styles.
      if(!this.IsDownLevel())
      {
        // Some basic styles
        this.Style["z-index"] = "30";
        this.Style["position"] = "absolute";
			
        if(this.Alignment != SnapAlignType.Default || this.CurrentDockingContainer != string.Empty)
        {
          this.Style["visibility"] = "hidden";
        }
      }
      else
      {
        this.Style["position"] = "relative";
      }

      // Output style
      output.Write(" style=\"");
      output.WriteStyleAttribute("height", this.Height.ToString());
      output.WriteStyleAttribute("width", this.Width.ToString());
      
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
      output.Write("\">");

      // Dump contents
      RenderContents(output);
			
      // If necessary, dump an iframe (IE only)
      if(!this.IsDownLevel() && this.RenderOverWindowedObjects && this.ClientBrowser.ToString().StartsWith("MSIE"))
      {
        output.Write("<iframe");
        output.WriteAttribute("id", "Art_IFrame_" + sSaneId);
        output.WriteAttribute("frameborder", "0");
        output.WriteAttribute("scrolling", "no");
				
        output.Write(" style=\"");
        output.WriteStyleAttribute("position", "absolute");
        output.WriteStyleAttribute("top", "0");
        output.WriteStyleAttribute("left", "0");
        output.WriteStyleAttribute("width", "100%");
        output.WriteStyleAttribute("height", "100%");
        output.WriteStyleAttribute("display", "block");
        output.WriteStyleAttribute("z-index", "-1");
        output.WriteStyleAttribute("filter", "progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)");
        output.Write("\"></iframe>");
      }

      output.Write("</div>");

      // Render hidden field to store our position
      if(!this.IsDownLevel() && this.EnableViewState)
      {
        output.AddAttribute("type", "hidden");
        output.AddAttribute("id", "Art_Situation_" + sSaneId);
        output.AddAttribute("name", "Art_Situation_" + sSaneId);
        output.AddAttribute("value", this.ParamData);
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();
      }

      // Emit cookie if such functionality is desired
      if(!this.IsDownLevel() && this.ClientSideCookieEnabled)
      {
        HttpCookie oCookie = new HttpCookie(this.ClientSideCookieName, this.ParamData);
        oCookie.Domain = this.ClientSideCookieDomain;
        oCookie.Expires = this.ClientSideCookieExpires;

        Page.Response.SetCookie(oCookie);
      }

      // Init code has to be rendered after the client script block, but before the startup script block.
      // Therefore, we emit it manually with the control.
      if(!this.IsDownLevel() && Context != null && Page.Request.Browser.JavaScript)
      {
        string sSnapVarName = sSaneId;

        StringBuilder oInitSB = new StringBuilder();
        oInitSB.Append("/*** ComponentArt.Web.UI.Snap ").Append(this.VersionString()).Append(" ").Append(sSnapVarName).Append(" ***/\n");
        oInitSB.Append("window." + sSnapVarName + " = new ComponentArt_Snap('" + sSaneId + "');\n");

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != sSnapVarName)
        {
          oInitSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sSnapVarName + "; " + sSnapVarName + ".GlobalAlias = '" + ID + "'; }\n");
        }

        // initialize some properties
        if(AutoCallBackOnDock) oInitSB.Append(sSnapVarName + ".AutoCallBackOnDock = true;\n");
        if(AutoCallBackOnExpand) oInitSB.Append(sSnapVarName + ".AutoCallBackOnExpand = true;\n");
        if(AutoCallBackOnCollapse) oInitSB.Append(sSnapVarName + ".AutoCallBackOnCollapse = true;\n");
        if(AutoCallBackOnMinimize) oInitSB.Append(sSnapVarName + ".AutoCallBackOnMinimize = true;\n");
        if(AutoCallBackOnUnMinimize) oInitSB.Append(sSnapVarName + ".AutoCallBackOnUnMinimize = true;\n");
        if(AutoPostBackOnDock) oInitSB.Append(sSnapVarName + ".AutoPostBackOnDock = true;\n");
        if(AutoPostBackOnExpand) oInitSB.Append(sSnapVarName + ".AutoPostBackOnExpand = true;\n");
        if(AutoPostBackOnCollapse) oInitSB.Append(sSnapVarName + ".AutoPostBackOnCollapse = true;\n");
        if(AutoPostBackOnMinimize) oInitSB.Append(sSnapVarName + ".AutoPostBackOnMinimize = true;\n");
        if(AutoPostBackOnUnMinimize) oInitSB.Append(sSnapVarName + ".AutoPostBackOnUnMinimize = true;\n");
        oInitSB.Append(sSnapVarName + ".BringToTopOnClick = " + this.BringToTopOnClick.ToString().ToLower() + ";\n");
        oInitSB.Append(sSnapVarName + ".CallbackPrefix = '" + Utils.GetResponseUrl(Context).Replace("'", "\\'") + (Context.Request.QueryString.Count > 0 ? "&" : "?") + "Cart_" + sSnapVarName + "_Callback=yes';\n");
        oInitSB.Append(sSnapVarName + ".ClientEvents = " + Utils.ConvertClientEventsToJsObject(this._clientEvents) + ";\n");
        oInitSB.Append(sSnapVarName + ".ClientPrefix = '" + this.Controls[0].ClientID.Replace(this.Controls[0].ID, "") + "';\n");
        if(ClientSideOnExpand != string.Empty) oInitSB.Append(sSnapVarName + ".ClientSideOnExpand = " + this.ClientSideOnExpand + ";\n");
        if(ClientSideOnCollapse != string.Empty) oInitSB.Append(sSnapVarName + ".ClientSideOnCollapse = " + this.ClientSideOnCollapse + ";\n");
        if(ClientSideOnDock != string.Empty) oInitSB.Append(sSnapVarName + ".ClientSideOnDock = " + this.ClientSideOnDock + ";\n");
        oInitSB.Append(sSnapVarName + ".CollapseDuration = " + this.CollapseDuration + ";\n");
        oInitSB.Append(sSnapVarName + ".CollapseSlide = " + ((int)this.CollapseSlide).ToString() + ";\n");
        oInitSB.Append(sSnapVarName + ".CollapseTransition = " + ((int)this.CollapseTransition).ToString() + ";\n");
        oInitSB.Append(sSnapVarName + ".CollapseTransitionCustomFilter = '" + this.CollapseTransitionCustomFilter + "';\n");
        oInitSB.Append(sSnapVarName + ".ControlId = '" + this.UniqueID + "';\n");
        if(ClientSideCookieEnabled && ClientSideCookieName != string.Empty) oInitSB.Append(sSnapVarName + ".CookieName = '" + this.ClientSideCookieName + "';\n");
        oInitSB.Append(sSnapVarName + ".ExpandDuration = " + this.ExpandDuration + ";\n");
        oInitSB.Append(sSnapVarName + ".ExpandSlide = " + ((int)this.ExpandSlide).ToString() + ";\n");
        oInitSB.Append(sSnapVarName + ".ExpandTransition = " + ((int)this.ExpandTransition).ToString() + ";\n");
        oInitSB.Append(sSnapVarName + ".ExpandTransitionCustomFilter = '" + this.ExpandTransitionCustomFilter + "';\n");
        if(MinimizeCssClass != string.Empty) oInitSB.Append(sSnapVarName + ".MinimizeCssClass = '" + this.MinimizeCssClass + "';\n");
        if(MinimizeDirectionElement != string.Empty) oInitSB.Append(sSnapVarName + ".MinimizeDirectionElement = '" + this.MinimizeDirectionElement + "';\n");
        oInitSB.Append(sSnapVarName + ".MinimizeDuration = " + this.MinimizeDuration + ";\n");
        oInitSB.Append(sSnapVarName + ".MinimizeSlide = " + ((int)this.MinimizeSlide).ToString() + ";\n");
        oInitSB.Append(sSnapVarName + ".RenderOverWindowedObjects = " + this.RenderOverWindowedObjects.ToString().ToLower() + ";\n");
        oInitSB.Append(sSnapVarName + ".ResizeThreshold = " + this.ResizingBorderWidth.ToString() + ";\n");

        oInitSB.Append("function art_RepositionFloater_" + sSaneId +
          "(){var posObj = art_GetInstance('" + sSaneId + "');art_RepositionFloater(posObj);}\n");
        oInitSB.Append("ComponentArt_Init_" + sSaneId + "();");
          
        WriteStartupScript(output, this.DemarcateClientScript(oInitSB.ToString()));
      }
    }

    // Initialize this control's position information from parameter data
    private void InitFromData(string sParamData)
    {
      string [] arParams = sParamData.Split(',');

      try
      {
        m_iAbsoluteX = int.Parse(arParams[0].Replace("px", ""));
        m_iAbsoluteY = int.Parse(arParams[1].Replace("px", ""));
      }
      catch {}

      try
      {
        this.Width = int.Parse(arParams[2].Replace("px", ""));
        this.Height = int.Parse(arParams[3].Replace("px", ""));
      }
      catch {}

      try
      {
        this.CurrentDockingContainer = arParams[4];
      }
      catch {}

      try
      {
        this.CurrentDockingIndex = int.Parse(arParams[5]);
      }
      catch {}

      try
      {
        this.IsCollapsed = (int.Parse(arParams[6]) == 1);
      }
      catch {}

      try
      {
        this.IsMinimized = (int.Parse(arParams[7]) == 1);
      }
      catch {}
    }

    protected override bool IsDownLevel()
    {
      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        return true;
      }

      if (this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }

      if (Context != null)
      {
        return (this.DetermineBrowser(Context.Request) == SnapClientBrowserType.DownLevel);
      }
      else
        return false;
    }

    // Make sure our parameters are synchronized with any postback or cookie settings
    private void EnsureParamsSynchronized()
    {
      string sSaneId = this.GetSaneId();

      // This only needs to be done once.
      if(m_bIsSynchronized)
        return;

      if(Page.IsPostBack && Context.Request["Art_Situation_" + sSaneId] != null)
      {
        string sParamData = Context.Request["Art_Situation_" + sSaneId];
        
        // Initialize coords based on postback data
        this.ParamData = sParamData;
      }
      else if(this.ClientSideCookieEnabled)
      {
        HttpCookie oCookie = Context.Request.Cookies[this.ClientSideCookieName];

        if(oCookie != null)
        {
          string sParamData = oCookie.Value;

          // Initialize coords based on cookie data
          this.ParamData = sParamData;
        }
      }

      m_bIsSynchronized = true;
    }

    private void RenderScripts(HtmlTextWriter output)
    {
      string sSaneId = this.GetSaneId();

      // Do we need to emit some client script?
      string sOnLoadCommands = "";

      // Is this one of the advanced (supported) browsers?
      if (this.ClientBrowser != SnapClientBrowserType.DownLevel)
      {
        // do we need to render scripts for non-Atlas?
        ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
        if (oScriptManager == null)
        {
          // Utils
          if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
          {
            Page.RegisterClientScriptBlock("A573G988.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
          }

          // Core
          if (!Page.IsClientScriptBlockRegistered("A573J988.js"))
          {
            Page.RegisterClientScriptBlock("A573J988.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Snap.client_scripts", "A573J988.js");
          }

          if (!Page.IsClientScriptBlockRegistered("A573P288.js"))
          {
            Page.RegisterClientScriptBlock("A573P288.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Snap.client_scripts", "A573P288.js");
          }

          // Align/float
          if (!Page.IsClientScriptBlockRegistered("A573U699.js"))
          {
            Page.RegisterClientScriptBlock("A573U699.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Snap.client_scripts", "A573U699.js");
          }

          if (!Page.IsClientScriptBlockRegistered("A573K688.js"))
          {
            Page.RegisterClientScriptBlock("A573K688.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Snap.client_scripts", "A573K688.js");
          }

          // Drag
          if (!Page.IsClientScriptBlockRegistered("A573X288.js"))
          {
            Page.RegisterClientScriptBlock("A573X288.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Snap.client_scripts", "A573X288.js");
          }

          // Docking
          if (!Page.IsClientScriptBlockRegistered("A573V588.js"))
          {
            Page.RegisterClientScriptBlock("A573V588.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Snap.client_scripts", "A573V588.js");
          }

          // Resize
          if (!Page.IsClientScriptBlockRegistered("A573W988.js"))
          {
            Page.RegisterClientScriptBlock("A573W988.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Snap.client_scripts", "A573W988.js");
          }
        }

        sOnLoadCommands += "art_InitCore('" + sSaneId + "'," + this.IsCollapsed.ToString().ToLower() + "," + this.IsMinimized.ToString().ToLower() + ");\n";

        sOnLoadCommands += "art_InitFloater('" + sSaneId + "','" + this.Alignment + "'," +
          this.MinLeft + "," + this.MinTop + "," + this.OffsetX + "," +
          this.OffsetY + "," + (this.FloatingMode == SnapFloatingType.Smooth).ToString().ToLower() + "," +
          (this.FloatingMode != SnapFloatingType.None).ToString().ToLower() + ");\n";

        sOnLoadCommands += "art_InitDraggable('" + sSaneId + "','" +
          this.DraggingMode + "','" + this.DraggingStyle + "');\n";

        if (this.DockingContainers != string.Empty)
        {
          sOnLoadCommands += "art_InitDockable('" + sSaneId + "','" +
            this.DockingContainers + "','" + this.CurrentDockingContainer + "'," +
            this.CurrentDockingIndex + ",'" + this.DockingStyle + "'," +
            this.MustBeDocked.ToString().ToLower() + ",'" +
            (this.UndockedCssClass == string.Empty ? string.Empty : this.CssClass) + "','" +
            (this.UndockedCssClass == string.Empty ? string.Empty : this.UndockedCssClass) + "');\n";
        }

        if (this.ResizingMode != SnapResizingType.None)
        {
          sOnLoadCommands += "art_InitResizing('" + sSaneId + "'," +
            this.MinWidth + "," + this.MinHeight + ",'" + this.ResizingMode + "');\n";
        }

        // Write out init commands
        if (sOnLoadCommands.Length > 0)
        {
          StringBuilder startupScript = new StringBuilder();
          startupScript.Append("function ComponentArt_Init_" + sSaneId + "() {\n");
          startupScript.Append("if(!window.ComponentArt_Page_Loaded || !window.ComponentArt_Utils_Loaded || !window.ComponentArt_Snap_Align_Loaded || !window.ComponentArt_Snap_Collapse_Loaded || !window.ComponentArt_Snap_Core_Loaded || !window.ComponentArt_Snap_Dock_Loaded || !window.ComponentArt_Snap_Drag_Loaded || !window.ComponentArt_Snap_Float_Loaded || !window.ComponentArt_Snap_Resize_Loaded)\n");
          startupScript.Append("\t{setTimeout('ComponentArt_Init_" + sSaneId + "()', 50); return; }\n\n");
          startupScript.Append(sOnLoadCommands);
          startupScript.Append("}");
          WriteStartupScript(output, this.DemarcateClientScript(startupScript.ToString()));
        }
      }
      else // We have an unsupported browser
      {
        // do we need to render scripts for non-Atlas?
        ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
        if (oScriptManager == null)
        {
          if (!Page.IsClientScriptBlockRegistered("A573T388.js"))
          {
            Page.RegisterClientScriptBlock("A573T388.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Snap.client_scripts", "A573T388.js");
          }
        }
      }
    }
  
    private SnapClientBrowserType DetermineBrowser(System.Web.HttpRequest oRequest)
    {
      string sUserAgent = oRequest.UserAgent;

      if (sUserAgent == null || sUserAgent == "")
      {
        return SnapClientBrowserType.DownLevel;
      }

      int iMajorVersion = 0;
      double iMinorVersion = 0;

      try
      {
        iMajorVersion = oRequest.Browser.MajorVersion;
        iMinorVersion = oRequest.Browser.MinorVersion;
      }
      catch {}

      // Do we...

      // ... have IE
      if(sUserAgent.IndexOf("MSIE") >= 0)
      {
        if(iMajorVersion == 4)
        {
          return SnapClientBrowserType.MSIE4;
        }
        else if(iMajorVersion == 5)
        {
          if(iMinorVersion >= 5)
          {
            return SnapClientBrowserType.MSIE55;
          }
          else
          {
            return SnapClientBrowserType.MSIE5;
          }
        }
        else if(iMajorVersion >= 6)
        {
          return SnapClientBrowserType.MSIE6;
        }
      } // or Opera?
      else if(sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion >= 7)
      {
        return SnapClientBrowserType.Opera7;
      } // or Safari?
      else if(sUserAgent.IndexOf("Safari") >= 0)
      {
        return SnapClientBrowserType.Netscape5;
      } // or Netscape?
      else if(sUserAgent.IndexOf("Gecko") >= 0)
      {
        if(iMajorVersion == 5)
        {
          return SnapClientBrowserType.Netscape5; 
        }
        else if(iMajorVersion == 6)
        {
          return SnapClientBrowserType.Netscape6;
        }
        else
        {
          return SnapClientBrowserType.Netscape7;
        }
      }
      else if(sUserAgent.IndexOf("Konqueror") >= 0)
      {
        return SnapClientBrowserType.Konqueror;
      }

      // ... or none of the above?
      return SnapClientBrowserType.DownLevel;
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate for <see cref="Dock"/> event of <see cref="Snap"/> class.
    /// </summary>
    public delegate void DockEventHandler(object sender, SnapDockEventArgs e);

    /// <summary>
    /// Fires after a Snap is docked.
    /// </summary>
    [ Description("Fires after a Snap is docked."), 
    Category("Snap Events") ]
    public event DockEventHandler Dock;

    private void OnDock(SnapDockEventArgs e) 
    {         
      if (Dock != null) 
      {
        Dock(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="Expand"/> event of <see cref="Snap"/> class.
    /// </summary>
    public delegate void ExpandEventHandler(object sender, SnapEventArgs e);

    /// <summary>
    /// Fires after a Snap is expanded.
    /// </summary>
    [ Description("Fires after a Snap is expanded."), 
    Category("Snap Events") ]
    public event ExpandEventHandler Expand;

    private void OnExpand(SnapEventArgs e) 
    {         
      if (Expand != null) 
      {
        Expand(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="Collapse"/> event of <see cref="Snap"/> class.
    /// </summary>
    public delegate void CollapseEventHandler(object sender, SnapEventArgs e);

    /// <summary>
    /// Fires after a Snap is collapsed.
    /// </summary>
    [ Description("Fires after a Snap is collapsed."), 
    Category("Snap Events") ]
    public event CollapseEventHandler Collapse;

    private void OnCollapse(SnapEventArgs e) 
    {         
      if (Collapse != null) 
      {
        Collapse(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="Minimize"/> event of <see cref="Snap"/> class.
    /// </summary>
    public delegate void MinimizeEventHandler(object sender, SnapEventArgs e);

    /// <summary>
    /// Fires after a Snap is minimized.
    /// </summary>
    [ Description("Fires after a Snap is minimized."), 
    Category("Snap Events") ]
    public event MinimizeEventHandler Minimize;

    private void OnMinimize(SnapEventArgs e) 
    {         
      if (Minimize != null) 
      {
        Minimize(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="UnMinimize"/> event of <see cref="Snap"/> class.
    /// </summary>
    public delegate void UnMinimizeEventHandler(object sender, SnapEventArgs e);

    /// <summary>
    /// Fires after a Snap is un-minimized.
    /// </summary>
    [ Description("Fires after a Snap is un-minimized."), 
    Category("Snap Events") ]
    public event UnMinimizeEventHandler UnMinimize;

    private void OnUnMinimize(SnapEventArgs e) 
    {         
      if (UnMinimize != null) 
      {
        UnMinimize(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="Callback"/> event of <see cref="Snap"/> class.
    /// </summary>
    public delegate void CallbackEventHandler(object sender, SnapCallbackEventArgs e);

    /// <summary>
    /// Fires when a Snap performs a callback.
    /// </summary>
    [ Description("Fires when a Snap performs a callback"), 
    Category("Snap Events") ]
    public event CallbackEventHandler Callback;

    private void OnCallback(SnapCallbackEventArgs e) 
    {         
      if (Callback != null) 
      {
        Callback(this, e);
      }   
    }

    #endregion
	}
}
