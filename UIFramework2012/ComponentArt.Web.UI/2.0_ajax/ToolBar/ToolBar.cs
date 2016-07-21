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
using System.IO;
using System.Globalization;
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
  /// <seealso cref="ToolBar.WebService" />
  /// <seealso cref="ToolBar.WebServiceMethod" />
  public class ToolBarWebServiceRequest
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
  /// <seealso cref="ToolBar.WebService" />
  /// <seealso cref="ToolBar.WebServiceMethod" />
  public class ToolBarWebServiceResponse
  {
    ToolBarItemCollection _items;

    /// <summary>
    /// Items to be sent back to the client. Read-only.
    /// </summary>
    public ArrayList Items
    {
      get
      {
        return NodesToArray(_items);
      }
    }

    public ToolBarWebServiceResponse()
    {
      _items = new ToolBarItemCollection(null);
    }

    public void AddItem(ToolBarItem oItem)
    {
      _items.Add(oItem);
    }

    protected ArrayList NodesToArray(ToolBarItemCollection arItems)
    {
      ArrayList arList = new ArrayList();

      for (int i = 0; i < arItems.Count; i++)
      {
        ArrayList props = new ArrayList();

        ToolBarItem oItem = arItems[i];

        foreach (string sKey in oItem.Properties.Keys)
        {
          props.Add(new object[] { sKey, oItem.Properties[sKey] });
        }

        arList.Add(props);
      }

      return arList;
    }
  }

  #endregion

  /// <summary>
  /// Displays a toolbar in the web page.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     Creates a toolbar on the web page.
  ///   </para>
  ///   <para>
  ///     ToolBar <b>contents</b> are organized as an ordered collection of <see cref="ToolBarItem"/> objects, accessed via the <see cref="Items"/> property.
  ///     There are a number of ways to manipulate the toolbar's <b>contents</b>:
  ///     <list type="bullet">
  ///       <item>Using the ToolBar <b>designer</b> to visually set up the structure.</item>
  ///       <item><b>Inline</b> within the aspx (or ascx) file, by nesting the item list within the ToolBar tag's inner property tag &lt;Items&gt;.</item>
  ///       <item>From an XML <b>file</b> specified by the <see cref="SiteMapXmlFile"/> property.</item>
  ///       <item><b>Programmatically on the server</b> by using the server-side API.</item>
  ///       <item><b>Programmatically on the client</b> by using the client-side API.  For more information, see client-side reference for 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/webui_clientside_toolbar.html">ToolBar</a> and 
  ///         <a href="ms-help:/../ComponentArt.Web.UI.AJAX/webui_clientside_toolbaritem.html">ToolBarItem</a> classes.</item>
  ///     </list>
  ///   </para>
  ///   <para>
  ///     ToolBar <b>styles</b> are largely specified via CSS classes, which need to be defined separate from the ToolBar.
  ///     The CSS classes and other presentation-related settings are then assigned via various properties of the ToolBar and related classes.
  ///   </para>
  ///   <para>
  ///     Further customization of item designs and contents can be accomplished with <see cref="ServerTemplates"/> and <see cref="ClientTemplates"/> 
  ///     and custom <see cref="Content"/> collections.
  ///   </para>
  ///   <para>
  ///     A toolbar with no information specified will be rendered with a default set of CSS class definitions and assignments.
  ///   </para>
  /// </remarks>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [PersistChildren(false)]
	[ParseChildren(true)]
  [Designer(typeof(ComponentArt.Web.UI.ToolBarItemsDesigner))]
  public sealed class ToolBar : WebControl, IPostBackEventHandler, INamingContainer
	{
    public ToolBar() : base()
    {
      this._items = new ToolBarItemCollection(this);
    }

    #region Unused hidden inheritance

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string AccessKey
    {
      get { return base.AccessKey; }
    }

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override System.Drawing.Color BackColor
		{
			get { return base.BackColor; }
		}

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
    public override FontInfo Font
    {
      get { return base.Font; }
    }

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override System.Drawing.Color ForeColor
		{
			get { return base.ForeColor; }
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override short TabIndex
		{
			get { return base.TabIndex; }
		}

    #endregion

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
    /// Whether to perform a postback when an item is checked or unchecked.  Default value is false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    [Description("Whether to perform a postback when an item is checked or unchecked.  Default value is false.")]
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
    /// Whether to perform a postback when an item is selected.  Default value is false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    [Description("Whether to perform a postback when an item is selected.  Default value is false.")]
    public bool AutoPostBackOnSelect
    {
      get
      {
        return Utils.ParseBool(Properties["AutoPostBackOnSelect"], false);
      }
      set
      {
        Properties["AutoPostBackOnSelect"] = value.ToString();
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
    /// Whether to trigger ASP.NET page validation when an item is selected. Default value is true.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to trigger ASP.NET page validation when an item is selected. Default value is true.")]
    [DefaultValue(true)]
    public bool CausesValidation
    {
      get
      {
        return Utils.ParseBool(Properties["CausesValidation"], true);
      }
      set
      {
        Properties["CausesValidation"] = value.ToString();
      }
    }

    private ToolBarClientEvents _clientEvents = null;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ToolBarClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new ToolBarClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// The client-side (JavaScript) condition to satisfy before initializing.
    /// </summary>
    [Description("The client-side (JavaScript) condition to satisfy before initializing.")]
    [DefaultValue("")]
    public string ClientInitCondition
    {
      get
      {
        object o = ViewState["ClientInitCondition"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["ClientInitCondition"] = value;
      }
    }

    /// <summary>
    /// The client-side (JavaScript) condition to satisfy before rendering.
    /// </summary>
    [Description("The client-side (JavaScript) condition to satisfy before rendering.")]
    [DefaultValue("")]
    public string ClientRenderCondition
    {
      get
      {
        object o = ViewState["ClientRenderCondition"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["ClientRenderCondition"] = value;
      }
    }

    private ClientTemplateCollection _clientTemplates = new ClientTemplateCollection();
    /// <summary>
    /// Collection of client-templates that may be used by this control.
    /// </summary>
    [Browsable(false)]
    [Description("Collection of client-templates that may be used by this control.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplateCollection ClientTemplates
    {
      get
      {
        return _clientTemplates;
      }
    }

    /// <summary>
    /// Creates a new collection of child controls for the current control.
    /// </summary>
    /// <returns>A PageViewCollection object that contains the currents control's children.</returns>
    protected override ControlCollection CreateControlCollection()
    {
      return new ToolBarItemContentCollection(this);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ToolBarItemContentCollection Content
    {
      get
      {
        return (ToolBarItemContentCollection)this.Controls;
      }
    }

    private CustomAttributeMappingCollection _customAttributeMappings;
    /// <summary>
    /// Custom attribute mappings. Provides the ability to re-map property names when they are looked up in XML.
    /// </summary>
    [Category("Data")]
    [Description("Custom attribute mappings. Provides the ability to re-map property names when they are looked up in XML.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [NotifyParentProperty(true)]
    public CustomAttributeMappingCollection CustomAttributeMappings
    {
      get
      {
        if (_customAttributeMappings == null)
        {
          _customAttributeMappings = new CustomAttributeMappingCollection();
        }
        return _customAttributeMappings;
      }
    }


    /// <summary>
    /// The ID of the data source control to bind to. The control can be a SiteMapDataSource or XmlDataSource.
    /// </summary>
    [IDReferenceProperty(typeof(HierarchicalDataSourceControl))]
    [Description("The ID of the data source control to bind to.")]
    [DefaultValue("")]
    [Category("Data")]
    public string DataSourceID
    {
      get
      {
        object o = Properties["DataSourceID"];
        return (o == null) ? String.Empty : (string)o;
      }

      set
      {
        Properties["DataSourceID"] = value;
      }
    }

 

    /// <summary>
    /// The default CSS class to apply to an item when it is active (on mouse down).
    /// </summary>
    [Description("The default CSS class to apply to an item when it is active (on mouse down).")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultItemActiveCssClass
    {
      get 
      {
        return Properties["DefaultItemActiveCssClass"];
      }
      set 
      {
        Properties["DefaultItemActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// The default CSS class to apply to a <see cref="ToolBarItem.Checked">Checked</see> item when it is active (on mouse down).
    /// </summary>
    [Description("The default CSS class to apply to a Checked item when it is active (on mouse down).")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultItemCheckedActiveCssClass
    {
      get 
      {
        return Properties["DefaultItemCheckedActiveCssClass"];
      }
      set 
      {
        Properties["DefaultItemCheckedActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// The default CSS class to apply to a <see cref="ToolBarItem.Checked">Checked</see> item.
    /// </summary>
    [Description("The default CSS class to apply to a Checked item.")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultItemCheckedCssClass
    {
      get 
      {
        return Properties["DefaultItemCheckedCssClass"];
      }
      set 
      {
        Properties["DefaultItemCheckedCssClass"] = value;
      }
    }

    /// <summary>
    /// The default CSS class to apply to a <see cref="ToolBarItem.Checked">Checked</see> item on hover (on mouse over).
    /// </summary>
    [Description("The default CSS class to apply to a Checked item on hover (on mouse over).")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultItemCheckedHoverCssClass
    {
      get 
      {
        return Properties["DefaultItemCheckedHoverCssClass"];
      }
      set 
      {
        Properties["DefaultItemCheckedHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The default CSS class to apply to an item.
    /// </summary>
    [Description("The default CSS class to apply to an item.")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultItemCssClass
    {
      get 
      {
        return Properties["DefaultItemCssClass"];
      }
      set 
      {
        Properties["DefaultItemCssClass"] = value;
      }
    }

    /// <summary>
    /// The default CSS class to apply to an item that is not <see cref="ToolBarItem.Enabled">Enabled</see> 
    /// and is <see cref="ToolBarItem.Checked">Checked</see>.
    /// </summary>
    [Description("The default CSS class to apply to an item that is not Enabled and is Checked.")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultItemDisabledCheckedCssClass
    {
      get 
      {
        return Properties["DefaultItemDisabledCheckedCssClass"];
      }
      set 
      {
        Properties["DefaultItemDisabledCheckedCssClass"] = value;
      }
    }

    /// <summary>
    /// The default CSS class to apply to an item that is not <see cref="ToolBarItem.Enabled">Enabled</see>.
    /// </summary>
    [Description("The default CSS class to apply to an item that is not Enabled.")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultItemDisabledCssClass
    {
      get 
      {
        return Properties["DefaultItemDisabledCssClass"];
      }
      set 
      {
        Properties["DefaultItemDisabledCssClass"] = value;
      }
    }

    /// <summary>
    /// The default height of items' dropdown images.  Default value is Unit.Empty.
    /// </summary>
    [Description("The default height of items' dropdown images.  Default value is Unit.Empty.")] 
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    public Unit DefaultItemDropDownImageHeight
    {
      get
      {
        return Utils.ParseUnit(Properties["DefaultItemDropDownImageHeight"]);
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultItemDropDownImageHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Item dropdown image dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// The default position of item dropdown image relative to rest of the item.  Default value is ToolBarDropDownImagePosition.Right.
    /// </summary>
    [Description("The default position of item dropdown image relative to rest of the item.  Default value is ToolBarDropDownImagePosition.Right.")]
    [Category("Appearance")]
    [DefaultValue(ToolBarDropDownImagePosition.Right)]
    public ToolBarDropDownImagePosition DefaultItemDropDownImagePosition
    {
      get
      {
        return Utils.ParseToolBarDropDownImagePosition(ViewState["DefaultItemDropDownImagePosition"], ToolBarDropDownImagePosition.Right);
      }
      set
      {
        ViewState["DefaultItemDropDownImagePosition"] = value;
      }
    }

    /// <summary>
    /// The default width of items' dropdown images.  Default value is Unit.Empty.
    /// </summary>
    [Description("The default width of items' dropdown images.  Default value is Unit.Empty.")]
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    public Unit DefaultItemDropDownImageWidth
    {
      get
      {
        return Utils.ParseUnit(Properties["DefaultItemDropDownImageWidth"]);
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultItemDropDownImageWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Item dropdown image dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// The default CSS class to apply to an item when its dropdown menu is expanded.
    /// </summary>
    [Description("The default CSS class to apply to an item when its dropdown menu is expanded.")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultItemExpandedCssClass
    {
      get 
      {
        return Properties["DefaultItemExpandedCssClass"];
      }
      set 
      {
        Properties["DefaultItemExpandedCssClass"] = value;
      }
    }

    /// <summary>
    /// The default height of items.  Default value is Unit.Empty.
    /// </summary>
    [Description("The default height of items.  Default value is Unit.Empty.")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    [Category("Layout")]
    public Unit DefaultItemHeight
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultItemHeight"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties["DefaultItemHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Item dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// The default CSS class to apply to an item on hover (on mouse over).
    /// </summary>
    [Description("The default CSS class to apply to an item on hover (on mouse over).")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultItemHoverCssClass
    {
      get
      {
        return Properties["DefaultItemHoverCssClass"];
      }
      set 
      {
        Properties["DefaultItemHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The default height of items' images.  Default value is Unit.Empty.
    /// </summary>
    [Description("The default height of items' images.  Default value is Unit.Empty.")]
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit),"")]
    public Unit DefaultItemImageHeight
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultItemImageHeight"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultItemImageHeight"] = value.ToString();
        }
        else
        {
          throw new Exception("Item image dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// The default width of items' images.  Default value is Unit.Empty.
    /// </summary>
    [Description("The default width of items' images.  Default value is Unit.Empty.")]
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit),"")]
    public Unit DefaultItemImageWidth
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultItemImageWidth"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties["DefaultItemImageWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Item image dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// The default position of item text and image relative to each other.  Default value is ToolBarTextImageRelation.ImageBeforeText.
    /// </summary>
    [Description("The default position of item text and image relative to each other.  Default value is ToolBarTextImageRelation.ImageBeforeText.")]
    [Category("Appearance")]
    [DefaultValue(ToolBarTextImageRelation.ImageBeforeText)]
    public ToolBarTextImageRelation DefaultItemTextImageRelation
    {
      get
      {
        return Utils.ParseToolBarTextImageRelation(ViewState["DefaultItemTextImageRelation"], ToolBarTextImageRelation.ImageBeforeText);
      }
      set
      {
        ViewState["DefaultItemTextImageRelation"] = value;
      }
    }

    /// <summary>
    /// The default gap in pixels between item text and image.  Default value is 0.
    /// </summary>
    [Description("The default gap in pixels between item text and image.  Default value is 0.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int DefaultItemTextImageSpacing
    {
      get 
      {
        return Utils.ParseInt(Properties["DefaultItemTextImageSpacing"]);
      }
      set 
      {
        Properties["DefaultItemTextImageSpacing"] = value.ToString();
      }
    }

    /// <summary>
    /// The default width of items.  Default value is Unit.Empty.
    /// </summary>
    [Description("The default width of items.  Default value is Unit.Empty.")]
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit),"")]
    public Unit DefaultItemWidth
    {
      get 
      {
        return Utils.ParseUnit(Properties["DefaultItemWidth"]);
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties["DefaultItemWidth"] = value.ToString();
        }
        else
        {
          throw new Exception("Item dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// The default text alignment to apply to labels.  Default value is TextAlign.Left.
    /// </summary>
    /// <seealso cref="ToolBarItem.TextAlign" />
    [Category("Appearance")]
    [DefaultValue(TextAlign.Left)]
    [Description("The default text alignment to apply to items.  Default value is TextAlign.Left.")]
    public TextAlign DefaultItemTextAlign
    {
      get
      {
        return Utils.ParseTextAlign(Properties["DefaultItemTextAlign"]);
      }
      set
      {
        Properties["DefaultItemTextAlign"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to permit text wrapping in labels by default.  Default value is false.
    /// </summary>
    /// <seealso cref="ToolBarItem.TextWrap" />
    [Category("Layout")]
    [DefaultValue(false)]
    [Description("Whether to permit text wrapping in labels by default.  Default value is false.")]
    public bool DefaultItemTextWrap
    {
      get
      {
        return Utils.ParseBool(Properties["DefaultItemTextWrap"], false);
      }
      set
      {
        Properties["DefaultItemTextWrap"] = value.ToString();
      }
    }

    /// <summary>
    /// Prefix to use for image URL paths.
    /// </summary>
    [Category("Support")]
    [Description("Prefix to use for image URL paths.")]
    [DefaultValue("")]
    public string ImagesBaseUrl
    {
      get
      {
        object o = Properties["ImagesBaseUrl"];
        return (o == null) ? String.Empty : Utils.ConvertUrl(Context, string.Empty, (string)o);
      }

      set
      {
        Properties["ImagesBaseUrl"] = value;
      }
    }

    private ToolBarItemCollection _items;
		/// <summary>
		/// Collection of ToolBar items.
		/// </summary>
		[Browsable(false)]
    [Description("Collection of ToolBar items.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
    public ToolBarItemCollection Items
		{
			get
			{
        return _items;
			}
		}

    /// <summary>
    /// Spacing between toolbar items.
    /// </summary>
    /// <value>
    /// Unit value for spacing between items, expressed in pixels.  Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(System.Web.UI.WebControls.Unit), "")]
    [Description("Spacing between toolbar items.  Default value is Unit.Empty.")]
    public Unit ItemSpacing
    {
      get
      {
        return Utils.ParseUnit(Properties["ItemSpacing"]);
			}
			set 
			{
        if (value.Type == UnitType.Pixel)
        {
          Properties["ItemSpacing"] = value.ToString();
        }
        else
        {
          throw new Exception("Item spacing may only be specified in pixels.");
        }
      }
    }

		/// <summary>
		/// Whether to enable keyboard control of the ToolBar.
		/// </summary>
    /// <value>
    /// When true, the ToolBar responds to keyboard shortcuts.  Default value is true.
    /// </value>
		/// <seealso cref="ToolBarItem.KeyboardShortcut" />
		[Category("Behavior")]
    [Description("Whether to enable keyboard control of the ToolBar.  Default value is true.")]
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
		/// Orientation of the ToolBar.
		/// </summary>
    /// <value>
    /// Default value is GroupOrientation.Horizontal.
    /// </value>
		[Category("Layout")]
		[DefaultValue(GroupOrientation.Horizontal)]
    [Description("Orientation of the ToolBar.  Default value is GroupOrientation.Horizontal.")]
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
    /// Whether to persist custom attributes of nodes to JavaScript.
    /// </summary>
    /// <value>
    /// Default value is true.
    /// </value>
    [Description("Whether to persist custom attributes of nodes to JavaScript.  Default value is true.")]
    [DefaultValue(true)]
    public bool OutputCustomAttributes
    {
      get
      {
        object o = Properties["OutputCustomAttributes"];
        return (o == null) ? true : Utils.ParseBool(o, true);
      }
      set
      {
        Properties["OutputCustomAttributes"] = value.ToString();
      }
    }

    /// <summary>
    /// ID of the XML node to use as the parent of toolbar item collection.
    /// </summary>
    /// <remarks>
    /// This property only applies when toolbar data is imported from XML.
    /// </remarks>
    [Description("ID of the XML node to use as the parent of toolbar item collection.")]
    [DefaultValue(null)]
    [Category("Data")]
    public string RenderRootNodeId
    {
      get
      {
        return Properties["RenderRootNodeId"];
      }
      set
      {
        Properties["RenderRootNodeId"] = value;
      }
    }

    /// <summary>
    /// The name of the ASP.NET AJAX web service to use for initially populating the ToolBar.
    /// </summary>
    /// <seealso cref="WebServiceMethod" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service to use for initially populating the ToolBar.")]
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
    /// The name of the ASP.NET AJAX web service method to use for initially populating the ToolBar.
    /// </summary>
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service method to use for initially populating the ToolBar.")]
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


    private StringCollection PreloadImages = new StringCollection();

    private void LoadPreloadImages()
    {
      foreach (ToolBarItem item in this.Items)
      {
        string[] itemImages = new string[] {
          item.ActiveDropDownImageUrl,
          item.ActiveImageUrl,
          item.CheckedActiveImageUrl,
          item.CheckedHoverImageUrl,
          item.CheckedImageUrl,
          item.DisabledCheckedImageUrl,
          item.DisabledDropDownImageUrl,
          item.DisabledImageUrl,
          item.DropDownImageUrl,
          item.ExpandedDropDownImageUrl,
          item.ExpandedImageUrl,
          item.HoverDropDownImageUrl,
          item.HoverImageUrl,
          item.ImageUrl};
        foreach (string imageUrl in itemImages)
        {
          if (imageUrl != null && imageUrl != string.Empty)
          {
            string imageFullUrl = Utils.ConvertUrl(this.Context, this.ImagesBaseUrl, imageUrl);
            if (!this.PreloadImages.Contains(imageFullUrl))
            {
              this.PreloadImages.Add(imageFullUrl);
            }
          }
        }
      }
    }

    private void RenderPreloadImages(HtmlTextWriter output)
    {
      output.Write("<div style=\"position:absolute;top:0px;left:0px;visibility:hidden;\">");
      foreach (string imageUrl in this.PreloadImages)
      {
        output.Write("<img src=\"" + imageUrl + "\" width=\"0\" height=\"0\" alt=\"\" />\n");
      }
      output.Write("</div>");
    }

    private System.Web.UI.AttributeCollection _properties;
    private System.Web.UI.AttributeCollection Properties
    {
      get
      {
        if (_properties == null)
        {
          StateBag oBag = new StateBag(true);
          _properties = new System.Web.UI.AttributeCollection(oBag);
        }
        return _properties;
      }
      set
      {
        _properties = value;
      }
    }

    private ToolBarCustomTemplateCollection _serverTemplates;
    /// <summary>
    /// Custom server templates which are referenced by items with special needs.
    /// </summary>
    /// <seealso cref="ToolBarItem.ServerTemplateId" />
    [Browsable(false)]
    [Description("Collection of CustomTemplate controls.")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ToolBarCustomTemplateCollection ServerTemplates
    {
      get
      {
        if (_serverTemplates == null)
        {
          _serverTemplates = new ToolBarCustomTemplateCollection();
        }
        return _serverTemplates;
      }
    }

    /// <summary>
    /// Path to the site map XML file.
    /// </summary>
    /// <seealso cref="DataSourceID" />
    /// <seealso cref="LoadXml(string)" /><seealso cref="LoadXml(System.Xml.XmlDocument)" />
    [Category("Data")]
    [DefaultValue("")]
    [Description("Path to the site map XML file. ")]
    [Editor(typeof(System.Web.UI.Design.XmlUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public string SiteMapXmlFile
    {
      get
      {
        object o = Properties["SiteMapXmlFile"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        Properties["SiteMapXmlFile"] = value;
        this.LoadSiteMapXmlFile(value);
      }
    }

    /// <summary>
    /// Whether to use fading effects when changing the appearance of items.  Default value is true.
    /// </summary>
    /// <remarks>
    /// Fade effects are only available in Internet Explorer 5.5+ for Windows.
    /// </remarks>
    [Category("Appearance")]
    [DefaultValue(true)]
    [Description("Whether to use fading effects when changing the appearance of items.  Default value is true.")]
    public bool UseFadeEffect
    {
      get
      {
        return Utils.ParseBool(Properties["UseFadeEffect"], true);
      }
      set
      {
        Properties["UseFadeEffect"] = value.ToString();
      }
    }

    private string GenerateCustomContentIdMap()
    {
      string result = "";
      if (this.Content.Count > 0)
      {
        string[] customContentIdMappings = new string[this.Content.Count];
        int index = 0;
        foreach (Control childControl in this.Content)
        {
          customContentIdMappings[index] = "'" + childControl.ID + "': '" + childControl.ClientID + "'";
          index++;
        }
        result = String.Join(",", customContentIdMappings);
      }
      return "{" + result + "}";
    }

    private string GenerateDropDownIdMap()
    {
// Not Implemented in MVC Currently
      Hashtable dropDownIdMap = new Hashtable();
      foreach (ToolBarItem item in this.Items)
      {
        if (item.DropDownId != null && item.DropDownId != String.Empty && !dropDownIdMap.Contains(item.DropDownId))
        {
          dropDownIdMap.Add(item.DropDownId, (Utils.FindControl(this, item.DropDownId)).ClientID);
        }
      }
      if (dropDownIdMap.Count > 0)
      {
        string[] dropDownIdMapArray = new string[dropDownIdMap.Count];
        int index = 0;
        foreach (DictionaryEntry dropDownIdMapEntry in dropDownIdMap)
        {
          dropDownIdMapArray[index] = "'" + dropDownIdMapEntry.Key.ToString() + "': '" + dropDownIdMapEntry.Value.ToString() + "'";
          index++;
        }
        return "{" + String.Join(",", dropDownIdMapArray) + "}";
      }
      else
      {
        return "{}";
      }
 
    }

    private JavaScriptArray GenerateItemStorage()
    {
      JavaScriptArray itemStorage = new JavaScriptArray();
      foreach (ToolBarItem item in this.Items)
      {
        ArrayList itemProperties = new ArrayList();

        itemProperties.Add(new object[] { 31, item.PostBackID });

        foreach (string propertyName in item.Properties.Keys)
        {
          switch (item.GetVarAttributeName(propertyName).ToLower(System.Globalization.CultureInfo.InvariantCulture))
          {
            case "activecssclass": 
              itemProperties.Add(new object[] { 0, item.ActiveCssClass }); 
              break;
            case "activeimageurl": 
              itemProperties.Add(new object[] { 1, item.ActiveImageUrl }); 
              break;
            case "causesvalidation": 
              itemProperties.Add(new object[] { 2, item.CausesValidation }); 
              break;
            case "checked": 
              itemProperties.Add(new object[] { 3, item.Checked }); 
              break;
            case "checkedactivecssclass": 
              itemProperties.Add(new object[] { 4, item.CheckedActiveCssClass }); 
              break;
            case "checkedactiveimageurl": 
              itemProperties.Add(new object[] { 5, item.CheckedActiveImageUrl }); 
              break;
            case "checkedcssclass": 
              itemProperties.Add(new object[] { 6, item.CheckedCssClass }); 
              break;
            case "checkedhovercssclass": 
              itemProperties.Add(new object[] { 7, item.CheckedHoverCssClass }); 
              break;
            case "checkedhoverimageurl": 
              itemProperties.Add(new object[] { 8, item.CheckedHoverImageUrl }); 
              break;
            case "checkedimageurl": 
              itemProperties.Add(new object[] { 9, item.CheckedImageUrl }); 
              break;
            case "clientsidecommand": 
              itemProperties.Add(new object[] { 10, Utils.MakeStringXmlSafe(item.ClientSideCommand) }); 
              break;
            case "clienttemplateid": 
              itemProperties.Add(new object[] { 11, item.ClientTemplateId }); 
              break;
            case "cssclass": 
              itemProperties.Add(new object[] { 12, item.CssClass }); 
              break;
            case "customcontentid": 
              itemProperties.Add(new object[] { 13, item.CustomContentId }); 
              break;
            case "disabledcheckedcssclass": 
              itemProperties.Add(new object[] { 14, item.DisabledCheckedCssClass }); 
              break;
            case "disabledcheckedimageurl": 
              itemProperties.Add(new object[] { 15, item.DisabledCheckedImageUrl }); 
              break;
            case "disabledcssclass": 
              itemProperties.Add(new object[] { 16, item.DisabledCssClass }); 
              break;
            case "disabledimageurl": 
              itemProperties.Add(new object[] { 17, item.DisabledImageUrl }); 
              break;
            case "dropdownid": 
              itemProperties.Add(new object[] { 18, item.DropDownId }); 
              break;
            case "enabled": 
              itemProperties.Add(new object[] { 19, item.Enabled }); 
              break;
            case "expandedcssclass": 
              itemProperties.Add(new object[] { 20, item.ExpandedCssClass }); 
              break;
            case "expandedimageurl": 
              itemProperties.Add(new object[] { 21, item.ExpandedImageUrl }); 
              break;
            case "height": 
              itemProperties.Add(new object[] { 22, item.Height }); 
              break;
            case "hovercssclass": 
              itemProperties.Add(new object[] { 23, item.HoverCssClass }); 
              break;
            case "hoverimageurl": 
              itemProperties.Add(new object[] { 24, item.HoverImageUrl }); 
              break;
            case "id":
              itemProperties.Add(new object[] { 25, item.ID }); 
              break;
            case "imageheight": 
              itemProperties.Add(new object[] { 26, item.ImageHeight }); 
              break;
            case "imageurl": 
              itemProperties.Add(new object[] { 27, item.ImageUrl }); 
              break;
            case "imagewidth": 
              itemProperties.Add(new object[] { 28, item.ImageWidth }); 
              break;
            case "itemtype": 
              itemProperties.Add(new object[] { 29, item.ItemType }); 
              break;
            case "keyboardshortcut": 
              itemProperties.Add(new object[] { 30, item.KeyboardShortcut }); 
              break;

            //TODO: This is obsolete, since we output PostBackID in another spot.
            case "postbackid":
              itemProperties.Add(new object[] { 31, item.PostBackID });
              break;

            case "servertemplateid": 
              itemProperties.Add(new object[] { 32, item.ServerTemplateId }); 
              break;
            case "text": 
              itemProperties.Add(new object[] { 33, Utils.MakeStringXmlSafe(item.Text) }); 
              break;
            case "textalign": 
              itemProperties.Add(new object[] { 34, item.TextAlign }); 
              break;
            case "textimagerelation": 
              itemProperties.Add(new object[] { 35, item.TextImageRelation }); 
              break;
            case "textimagespacing": 
              itemProperties.Add(new object[] { 36, item.TextImageSpacing }); 
              break;
            case "textwrap": 
              itemProperties.Add(new object[] { 37, item.TextWrap }); 
              break;
            case "togglegroupid":
              itemProperties.Add(new object[] { 38, item.ToggleGroupId });
              break;
            case "tooltip": 
              itemProperties.Add(new object[] { 39, Utils.MakeStringXmlSafe(item.ToolTip) }); 
              break;
            case "value": 
              itemProperties.Add(new object[] { 40, Utils.MakeStringXmlSafe(item.Value) }); 
              break;
            case "visible": 
              itemProperties.Add(new object[] { 41, item.Visible }); 
              break;
            case "width": 
              itemProperties.Add(new object[] { 42, item.Width }); 
              break;
            case "activedropdownimageurl":
              itemProperties.Add(new object[] { 43, item.ActiveDropDownImageUrl });
              break;
            case "disableddropdownimageurl":
              itemProperties.Add(new object[] { 44, item.DisabledDropDownImageUrl });
              break;
            case "dropdownimageheight":
              itemProperties.Add(new object[] { 45, item.DropDownImageHeight });
              break;
            case "dropdownimageposition": 
              itemProperties.Add(new object[] { 46, item.DropDownImagePosition }); 
              break;
            case "dropdownimageurl":
              itemProperties.Add(new object[] { 47, item.DropDownImageUrl });
              break;
            case "dropdownimagewidth":
              itemProperties.Add(new object[] { 48, item.DropDownImageWidth });
              break;
            case "expandeddropdownimageurl":
              itemProperties.Add(new object[] { 49, item.ExpandedDropDownImageUrl });
              break;
            case "hoverdropdownimageurl":
              itemProperties.Add(new object[] { 50, item.HoverDropDownImageUrl });
              break;
            case "allowhtmlcontent":
              itemProperties.Add(new object[] { 51, item.AllowHtmlContent });
              break;
            case "autopostbackonselect":
              itemProperties.Add(new object[] { 52, item.AutoPostBackOnSelect });
              break;
            default:
              if (this.OutputCustomAttributes)
              {
                itemProperties.Add(new object[] { item.GetVarAttributeName(propertyName), Utils.MakeStringXmlSafe(item.Properties[propertyName]) });
              }
              break;
          }
        }

        itemStorage.Add(itemProperties);
      }
      return itemStorage;
    }

    private StringBuilder GeneratePropertyStorage()
    {
      StringBuilder propertyStorage = new StringBuilder();

      // Define properties
      propertyStorage.Append("[\n");
      propertyStorage.Append("['ApplicationPath'," + Utils.ConvertStringToJSString(Context.Request.ApplicationPath) + "],\n");
      propertyStorage.Append("['AutoPostBackOnCheckChanged'," + this.AutoPostBackOnCheckChanged.ToString().ToLower() + "],\n");
      propertyStorage.Append("['AutoPostBackOnSelect'," + this.AutoPostBackOnSelect.ToString().ToLower() + "],\n");
      if (this.AutoTheming)
      {
        propertyStorage.Append("['AutoTheming',1],");
        propertyStorage.Append("['AutoThemingCssClassPrefix'," + Utils.ConvertStringToJSString(this.AutoThemingCssClassPrefix) + "],");
      }
      propertyStorage.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "],\n");
      propertyStorage.Append("['ClientInitCondition'," + Utils.ConvertStringToJSString(this.ClientInitCondition) + "],\n");
      propertyStorage.Append("['ClientRenderCondition'," + Utils.ConvertStringToJSString(this.ClientRenderCondition) + "],\n");
      propertyStorage.Append("['ClientTemplates'," + this.ClientTemplates.ToString() + "],\n");
      propertyStorage.Append("['ControlId'," + Utils.ConvertStringToJSString(this.UniqueID) + "],\n");
      propertyStorage.Append("['CssClass'," + Utils.ConvertStringToJSString(this.CssClass) + "],\n");
      propertyStorage.Append("['CustomContentIdMap'," + this.GenerateCustomContentIdMap() + "],\n");
      propertyStorage.Append("['DropDownIdMap'," + this.GenerateDropDownIdMap() + "],\n");
      propertyStorage.Append("['DefaultItemActiveCssClass'," + Utils.ConvertStringToJSString(this.DefaultItemActiveCssClass) + "],\n");
      propertyStorage.Append("['DefaultItemCheckedActiveCssClass'," + Utils.ConvertStringToJSString(this.DefaultItemCheckedActiveCssClass) + "],\n");
      propertyStorage.Append("['DefaultItemCheckedCssClass'," + Utils.ConvertStringToJSString(this.DefaultItemCheckedCssClass) + "],\n");
      propertyStorage.Append("['DefaultItemCheckedHoverCssClass'," + Utils.ConvertStringToJSString(this.DefaultItemCheckedHoverCssClass) + "],\n");
      propertyStorage.Append("['DefaultItemCssClass'," + Utils.ConvertStringToJSString(this.DefaultItemCssClass) + "],\n");
      propertyStorage.Append("['DefaultItemDisabledCheckedCssClass'," + Utils.ConvertStringToJSString(this.DefaultItemDisabledCheckedCssClass) + "],\n");
      propertyStorage.Append("['DefaultItemDisabledCssClass'," + Utils.ConvertStringToJSString(this.DefaultItemDisabledCssClass) + "],\n");
      propertyStorage.Append("['DefaultItemDropDownImageHeight'," + Utils.ConvertUnitToJSConstant(this.DefaultItemDropDownImageHeight) + "],\n");
      propertyStorage.Append("['DefaultItemDropDownImagePosition'," + (int)this.DefaultItemDropDownImagePosition + "],\n");
      propertyStorage.Append("['DefaultItemDropDownImageWidth'," + Utils.ConvertUnitToJSConstant(this.DefaultItemDropDownImageWidth) + "],\n");
      propertyStorage.Append("['DefaultItemExpandedCssClass'," + Utils.ConvertStringToJSString(this.DefaultItemExpandedCssClass) + "],\n");
      propertyStorage.Append("['DefaultItemHeight'," + Utils.ConvertUnitToJSConstant(this.DefaultItemHeight) + "],\n");
      propertyStorage.Append("['DefaultItemHoverCssClass'," + Utils.ConvertStringToJSString(this.DefaultItemHoverCssClass) + "],\n");
      propertyStorage.Append("['DefaultItemImageHeight'," + Utils.ConvertUnitToJSConstant(this.DefaultItemImageHeight) + "],\n");
      propertyStorage.Append("['DefaultItemImageWidth'," + Utils.ConvertUnitToJSConstant(this.DefaultItemImageWidth) + "],\n");
      propertyStorage.Append("['DefaultItemTextAlign'," + (int)this.DefaultItemTextAlign + "],\n");
      propertyStorage.Append("['DefaultItemTextImageRelation'," + (int)this.DefaultItemTextImageRelation + "],\n");
      propertyStorage.Append("['DefaultItemTextImageSpacing'," + this.DefaultItemTextImageSpacing + "],\n");
      propertyStorage.Append("['DefaultItemTextWrap'," + this.DefaultItemTextWrap.ToString().ToLower() + "],\n");
      propertyStorage.Append("['DefaultItemWidth'," + Utils.ConvertUnitToJSConstant(this.DefaultItemWidth) + "],\n");
      propertyStorage.Append("['Enabled'," + this.Enabled.ToString().ToLower() + "],\n");
      propertyStorage.Append("['ItemSpacing'," + Utils.ConvertUnitToJSConstant(this.ItemSpacing) + "],\n");
      propertyStorage.Append("['ImagesBaseUrl'," + Utils.ConvertStringToJSString(Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl)) + "],");
      propertyStorage.Append("['Height'," + Utils.ConvertUnitToJSConstant(this.Height) + "],\n");
      propertyStorage.Append("['Orientation'," + (int)this.Orientation + "],\n");
      propertyStorage.Append("['PlaceHolderId'," + Utils.ConvertStringToJSString(this.GetSaneId()) + "],\n");
      propertyStorage.Append("['UseFadeEffect'," + this.UseFadeEffect.ToString().ToLower() + "],\n");
      propertyStorage.Append("['SoaService','" + this.SoaService + "'],");
      propertyStorage.Append("['WebService','" + this.WebService + "'],");
      propertyStorage.Append("['WebServiceCustomParameter','" + this.WebServiceCustomParameter + "'],");
      propertyStorage.Append("['WebServiceMethod','" + this.WebServiceMethod + "'],");
      propertyStorage.Append("['Width'," + Utils.ConvertUnitToJSConstant(this.Width) + "]\n]");

      return propertyStorage;
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      if (!this.IsDownLevel() && Page != null)
      {
        // do we need to render scripts for non-Atlas?
        ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
        if (oScriptManager == null)
        {
        // Add core code

        if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
        {
          Page.RegisterClientScriptBlock("A573G988.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
        }

        if (!Page.IsClientScriptBlockRegistered("A573Z388.js"))
        {
          Page.RegisterClientScriptBlock("A573Z388.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573Z388.js");
        }

        if (!Page.IsClientScriptBlockRegistered("A573B288.js"))
        {
          Page.RegisterClientScriptBlock("A573B288.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.ToolBar.client_scripts", "A573B288.js");
        }

        if (!Page.IsClientScriptBlockRegistered("A573I788.js"))
        {
          Page.RegisterClientScriptBlock("A573I788.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.ToolBar.client_scripts", "A573I788.js");
        }
        if (!Page.IsClientScriptBlockRegistered("A573H988.js"))
        {
          Page.RegisterClientScriptBlock("A573H988.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.ToolBar.client_scripts", "A573H988.js");
        }
        }
      }

      if (this.AutoTheming)
      {
        this.ApplyTheming(false);
      }

      // Output default styles and set appropriate toolbar properties (only done if default styles are needed).
      this.RenderDefaultStyles(output);

      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        this.RenderAccessibleContent(output);
      }
      else if (this.IsDownLevel())
      {
        this.RenderDownLevelToolBar(output);
      }
      else
      {
        string toolbarClientVarName = this.GetSaneId();

        string itemStorageArrayId = "ComponentArt_ItemStorage_" + toolbarClientVarName;
        string propertyStorageArrayId = "ComponentArt_PropertyStorage_" + toolbarClientVarName;
        string storage = "window." + itemStorageArrayId + "=" + this.GenerateItemStorage().ToString() + ";\n";
        storage += "window." + propertyStorageArrayId + "=" + this.GeneratePropertyStorage().ToString() + ";\n";
        storage = this.DemarcateClientScript(storage, "ComponentArt Web.UI client-side storage for " + toolbarClientVarName);
        WriteStartupScript(output, storage);

        this.LoadPreloadImages();
        if (this.PreloadImages.Count > 0)
        {
          this.RenderPreloadImages(output);
        }

        // Render custom content items and item server template instances
        foreach (Control childControl in this.Content)
        {
          output.Write("<div id=\"" + childControl.ClientID + "\" style=\"display:none;\">");
          childControl.RenderControl(output);
          output.Write("</div>");
        }

        // Output the placeholder tag
        output.Write("<div");
        output.WriteAttribute("id", toolbarClientVarName);
        bool useDisplayInlineBlock = Utils.BrowserCapabilities(Context.Request).IsBrowserWinIE5point5plus;
        if (useDisplayInlineBlock || !this.Height.IsEmpty || !this.Width.IsEmpty || this.Style.Count > 0)
        {
          output.Write(" style=\"");
          if (useDisplayInlineBlock && this.Style["display"] == null)
          {
            output.WriteStyleAttribute("display", "inline-block"); // default to inline-block display for IE 5.5+
          }
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
        output.Write(">");
        output.Write("</div>");

        if (this.EnableViewState)
        {
          // Render item-storage-persisting field.
          output.AddAttribute("id", toolbarClientVarName + "_ItemStorage");
          output.AddAttribute("name", toolbarClientVarName + "_ItemStorage");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render toplevel-property-persisting field.
          output.AddAttribute("id", toolbarClientVarName + "_Properties");
          output.AddAttribute("name", toolbarClientVarName + "_Properties");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render field to persist list of checked items.
          output.AddAttribute("id", toolbarClientVarName + "_CheckedItems");
          output.AddAttribute("name", toolbarClientVarName + "_CheckedItems");
          output.AddAttribute("type", "hidden");
          output.AddAttribute("value", JoinStringCollectionToString(NewCheckedPostBackIDs()));
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();
        }

        StringBuilder startupSB = new StringBuilder();
        startupSB.Append("window.ComponentArt_Init_" + toolbarClientVarName + " = function() {\n");

        // Include check for whether everything we need is loaded, and a retry after a delay in case it isn't.
        string areScriptsLoaded = "(window.cart_toolbar_kernel_loaded && window.cart_toolbar_support_loaded)";
        startupSB.Append("if (!" + areScriptsLoaded + ")\n");
        int retryDelay = 100; // 100 ms retry time sounds about right
        startupSB.Append("{\n\tsetTimeout('ComponentArt_Init_" + toolbarClientVarName + "()', " + retryDelay.ToString() + ");\n\treturn;\n}\n");

        // Instantiate toolbar object
        startupSB.Append("window." + toolbarClientVarName + " = new ComponentArt_ToolBar('" + toolbarClientVarName + "');\n");

        // Write postback function reference
        if (Page != null)
        {
          startupSB.Append(toolbarClientVarName + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
        }

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != toolbarClientVarName)
        {
          startupSB.Append("if(!window['" + this.ID + "']) { window['" + this.ID + "'] = window." + toolbarClientVarName + "; " + toolbarClientVarName + ".GlobalAlias = '" + ID + "'; }\n");
        }

        startupSB.Append(toolbarClientVarName + ".LoadProperties(" + propertyStorageArrayId + ");\n");
        startupSB.Append(toolbarClientVarName + ".LoadItems(" + itemStorageArrayId + ");\n");

        if (this.EnableViewState)
        {
          // add us to the client viewstate-saving mechanism
          startupSB.Append("ComponentArt_ClientStateControls[ComponentArt_ClientStateControls.length] = " + toolbarClientVarName + ";\n");
        }

        if (ClientInitCondition != "")
        {
          startupSB.Append("ComponentArt_WaitOnCondition(" + toolbarClientVarName + ".ClientInitCondition,'" + toolbarClientVarName + ".Initialize()');\n");
        }
        else
        {
          startupSB.Append(toolbarClientVarName + ".Initialize();\n");
        }

        // Keyboard
        if (this.KeyboardEnabled)
        {
          // Initialize keyboard
          startupSB.Append("ComponentArt_ToolBar_InitKeyboard(" + toolbarClientVarName + ");\n");

          // Create client script to register keyboard shortcuts
          StringBuilder oKeyboardSB = new StringBuilder();
          GenerateKeyShortcutScript(toolbarClientVarName, oKeyboardSB);
          startupSB.Append(oKeyboardSB.ToString());
        }

        // Set the flag that the toolbar has been initialized.  This is the last action in the toolbar initialization.
        startupSB.Append("window." + toolbarClientVarName + "_loaded = true;\n}\n");

        // Call this initialization function.  Remember that it will be repeated after a delay if it's not all ready.
        startupSB.Append("ComponentArt_Init_" + toolbarClientVarName + "();");

        WriteStartupScript(output, this.DemarcateClientScript(startupSB.ToString(), "ComponentArt_ToolBar_Startup_" + toolbarClientVarName + " " + this.VersionString()));
      }
    }

    internal void RenderDefaultStyles(HtmlTextWriter output)
    {
      if ((this.CssClass == null || this.CssClass == string.Empty || this.CssClass == "ctb_ToolBar")
       && (this.DefaultItemCssClass == null || this.DefaultItemCssClass == string.Empty || this.DefaultItemCssClass == "ctb_Item"))
      {
        // render them
        string sDefaultStyles = "<style type=\"text/css\">" + GetResourceContent("ComponentArt.Web.UI.ToolBar.defaultStyle.css") + "</style>";
        output.Write(sDefaultStyles);
        this.CssClass = "ctb_ToolBar";
        this.DefaultItemCssClass = "ctb_Item";
        this.DefaultItemHoverCssClass = "ctb_ItemHover";
        this.DefaultItemExpandedCssClass = this.DefaultItemActiveCssClass = "ctb_ItemActive";
        this.DefaultItemCheckedCssClass = "ctb_ItemChecked";
        this.DefaultItemCheckedHoverCssClass = "ctb_ItemCheckedHover";
        this.DefaultItemCheckedActiveCssClass = "ctb_ItemCheckedActive";
      }
    }

    protected override void CreateChildControls()
    {
      if (this.ServerTemplates.Count > 0 && this.Items != null && this.Items.Count > 0)
      {
        //this.Controls.Clear();  //This was OK when we only had ServerTemplates, but now we keep paralel contents
        this.ComponentArtFixStructure();
        this.InstantiateTemplatedItems();
      }
    }

    /// <summary>
    /// Ensures that Items form a sound structure.
    /// </summary>
    private void ComponentArtFixStructure()
    {
      this.SetPostBackIDs();
      this.SetParentToolBar();
    }

    private void SetPostBackIDs()
    {
      for (int i = 0; i < this.Items.Count; i++)
      {
        ToolBarItem item = this.Items[i];
        if (item.ID == null || item.ID == string.Empty)
        {
          item.PostBackID = String.Format("p{0:X}", i);
        }
        else
        {
          item.PostBackID = "p_" + item.ID;
        }
      }
    }

    private void SetParentToolBar()
    {
      foreach (ToolBarItem item in this.Items)
      {
        item._parentToolBar = this;
      }
    }

    /// <summary>
    /// Go through items, finding ones that reference templates, and instantiate those templates using the items.
    /// </summary>
    private void InstantiateTemplatedItems()
    {
      foreach (ToolBarItem item in this.Items)
      {
        if (item.ServerTemplateId != string.Empty)
        {
          ToolBarCustomTemplate template = this.FindTemplateById(item.ServerTemplateId);
          if (template == null)
          {
            throw new Exception("Template not found: " + item.ServerTemplateId);
          }
          ToolBarTemplateContainer container = new ToolBarTemplateContainer(item);
          template.Template.InstantiateIn(container);
          container.ID = this.ClientID + "_" + item.PostBackID;
          container.DataBind();
          this.Controls.Add(container);
        }
      }
    }

    internal ToolBarCustomTemplate FindTemplateById(string templateId)
    {
      foreach (ToolBarCustomTemplate template in this.ServerTemplates)
      {
        if (template.ID == templateId)
        {
          return template;
        }
      }
      return null;
    }

    private void GenerateKeyShortcutScript(string toolbarClientVarName, StringBuilder oSB)
    {
      foreach (ToolBarItem item in this.Items)
      {
        if (item.KeyboardShortcut != string.Empty)
        {
          oSB.Append("ComponentArt_RegisterKeyHandler(" + toolbarClientVarName + ",'" + item.KeyboardShortcut + "','" + toolbarClientVarName + ".SelectItemByPostBackId(\\'" + item.PostBackID + "\\')'" + ");\n");
        }
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

        if ( // We are good if:

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

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      if (Context != null && Page != null)
      {
        string dummy = Page.GetPostBackEventReference(this); // Ensure that __doPostBack is output to client side
      }

      // Do nothing if this isn't an initial run.
      if (EnableViewState && Context != null && Page.IsPostBack)
      {
        return;
      }


      if (this.DataSourceID != "")
      {
        Control oControl = Utils.FindControl(this, this.DataSourceID);

        if (oControl != null)
        {
          if (oControl is SiteMapDataSource)
          {
            SiteMapDataSource oDS = (SiteMapDataSource)oControl;

            this.LoadFromSiteMap(oDS.Provider.RootNode);
          }
          else if (oControl is XmlDataSource)
          {
            XmlDataSource oDS = (XmlDataSource)oControl;
            XmlDocument oXmlDoc = oDS.GetXmlDocument();

            this.LoadXml(oXmlDoc);
          }
          else
          {
            throw new Exception("DataSourceID must be set to the ID of a SiteMapDataSource or XmlDataSource control.");
          }
        }
        else
        {
          throw new Exception("Data source control '" + DataSourceID  + "' not found.");
        }
      }


      // at this point, we have the structure loaded.
      ComponentArtFixStructure();
    }

    protected override void LoadViewState(object savedState)
    {
      base.LoadViewState(savedState);

      // Load top-level properties
      //
      //TODO: Build in persistence of control-level properties
      //string sViewStateProperties = Context.Request.Form[this.ClientObjectId + "_Properties"];
      //if (sViewStateProperties != null)
      //{
      //  this.LoadClientProperties(sViewStateProperties);
      //}

      // Load item storage data
      string sViewStateData = Context.Request.Form[this.ClientObjectId + "_ItemStorage"];
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

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573Z388.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.ToolBar.client_scripts.A573B288.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.ToolBar.client_scripts.A573I788.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.ToolBar.client_scripts.A573H988.js");
      }

      // do we have no content and should load it from a SiteMapXmlFile?
      if ((this.Items == null || this.Items.Count == 0) && (this.SiteMapXmlFile != null && this.SiteMapXmlFile != string.Empty))
      {
        this.LoadSiteMapXmlFile(this.SiteMapXmlFile);
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
            ToolBarItem checkChangedItem = this.GetItemByPostBackID(postbackID);
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
            ToolBarItem checkChangedItem = this.GetItemByPostBackID(postbackID);
            if (checkChangedItem != null)
            {
              checkChangedItems.Add(checkChangedItem);
            }
          }
        }
        foreach (ToolBarItem item in checkChangedItems)
        {
          ToolBarItemEventArgs args = new ToolBarItemEventArgs(item);
          this.OnItemCheckChanged(args);
        }
      }
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      this.ComponentArtFixStructure();
    }

    /// <summary>
    /// Implements <see cref="IPostBackEventHandler.RaisePostBackEvent"/> method.
    /// </summary>
    public void RaisePostBackEvent(string eventArgument)
    {
      if (eventArgument == null || eventArgument == String.Empty)
      {
        return; // No action to execute.
      }
      string itemPostBackID = eventArgument;
      ToolBarItem item = this.GetItemByPostBackID(itemPostBackID);
      if (item == null)
      {
        throw new Exception("Item " + itemPostBackID + " not found.");
      }
      // should we validate the page?
      if (Utils.ConvertInheritBoolToBool(item.CausesValidation, this.CausesValidation))
      {
        Page.Validate();
      }
      if (item != null)
      {
        ToolBarItemEventArgs args = new ToolBarItemEventArgs(item);
        this.OnItemCommand(args);
      }
    }


    protected void LoadFromSiteMap(System.Web.SiteMapNode siteMapRoot)
    {
      foreach (System.Web.SiteMapNode siteMapNode in siteMapRoot.ChildNodes)
      {
        ToolBarItem item = new ToolBarItem();
        this.Items.Add(item);
        this.LoadFromSiteMapNode(item, siteMapNode);
        this.OnItemDataBound(item, siteMapNode);
      }
    }

    protected void LoadFromSiteMapNode(ToolBarItem item, System.Web.SiteMapNode siteMapNode)
    {
      item.Text = siteMapNode.Title;
      item.ToolTip = siteMapNode.Description;
      item.ID = siteMapNode.Key;
      // use attribute mappings to access additional properties
      foreach (CustomAttributeMapping mapping in this.CustomAttributeMappings)
      {
        string value = siteMapNode[mapping.From];
        if (value != null)
        {
          item.Properties[mapping.From] = value;
        }
      }
    }


    private void LoadSiteMapXmlFile(string fileName)
    {
      if (Context == null)
      {
        return;
      }
      string sServerPath = Context.Server.MapPath(fileName);
      if (!File.Exists(sServerPath))
      {
        throw new FileNotFoundException("Specified SiteMapXmlFile (" + fileName + ") does not exist.");
      }
      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.Load(sServerPath);
      this.LoadXml(oXmlDoc);
      this.ComponentArtFixStructure();
    }

    /// <summary>
    /// Load structure from given XmlDocument.
    /// </summary>
    /// <param name="oXmlDoc">XmlDocument to load from</param>
    public void LoadXml(XmlDocument oXmlDoc)
    {
      this.Items.Clear();
      XmlNodeList nodes = this.GetNodes(oXmlDoc);
      foreach (XmlNode node in nodes)
      {
        if (node.NodeType == XmlNodeType.Element) // Only process Xml elements (ignore comments, etc)
        {
          ToolBarItem newItem = new ToolBarItem();
          this.Items.Add(newItem);
          newItem.ReadXmlAttributes(node.Attributes);
          this.OnItemDataBound(newItem, node);
        }
      }
    }

    /// <summary>
    /// Returns the list of XML nodes that the items in this toolbar will be imported from.
    /// </summary>
    private XmlNodeList GetNodes(XmlDocument oXmlDoc)
    {
      if (this.RenderRootNodeId != null && this.RenderRootNodeId != string.Empty)
      {
        XmlElement rootElement = FindElementById(oXmlDoc.ChildNodes, this.RenderRootNodeId);
        if (rootElement != null)
        {
          return rootElement.ChildNodes;
        }
        else
        {
          throw new Exception("No item found with ID \"" + this.RenderRootNodeId + "\".");
        }
      }
      else
      {
        return oXmlDoc.DocumentElement.ChildNodes;
      }
    }

    /// <summary>
    /// Performs a depth-first search for an element with the given ID.
    /// </summary>
    private XmlElement FindElementById(XmlNodeList nodeList, string id)
    {
      foreach (XmlNode node in nodeList)
      {
        if (node.NodeType == XmlNodeType.Element)
        {
          foreach (XmlAttribute attribute in node.Attributes)
          {
            if (attribute.Name.ToLower(CultureInfo.InvariantCulture) == "id" && attribute.Value == id)
            {
              return (XmlElement)node;
            }
          }
          XmlElement foundBelow = (XmlElement)FindElementById(node.ChildNodes, id);
          if (foundBelow != null)
          {
            return foundBelow;
          }
        }
      }
      return null;
    }

    /// <summary>
    /// Load structure from given XML string.
    /// </summary>
    /// <param name="sXml">XML string to load from</param>
    public void LoadXml(string sXml)
    {
      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.LoadXml(sXml);
      this.LoadXml(oXmlDoc);
    }

    protected void LoadClientData(string sData)
    {
      try
      {
        if (sData != string.Empty)
        {
          this.Items.Clear();

          sData = HttpUtility.UrlDecode(sData, Encoding.UTF8);

          // make it xml-safe
          sData = sData.Replace("&", "#$cAmp@*");

          XmlDocument oXmlDoc = new XmlDocument();
          oXmlDoc.LoadXml(sData);

          XmlNode oRootNode = oXmlDoc.DocumentElement;

          if (oRootNode != null && oRootNode.ChildNodes.Count > 0)
          {
            foreach (XmlNode oNode in oRootNode.ChildNodes)
            {
              this.LoadClientXmlNode(oNode);
            }
            // fix up pointers
            this.ComponentArtFixStructure();
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error loading client data: " + ex);
      }
    }

    protected ToolBarItem LoadClientXmlNode(XmlNode clientXmlNode /*XmlNodeList arClientNodes, XmlNodeList arXmlMembers*/)
    {
      ToolBarItem newItem = new ToolBarItem();
      this.Items.Add(newItem);

      // are there properties on this node?
      if (clientXmlNode.FirstChild.ChildNodes.Count > 0)
      {
        XmlNodeList arProperties = clientXmlNode.FirstChild.ChildNodes;
        this.LoadClientXmlNodeProperties(arProperties, newItem);
      }

      return newItem;
    }

    protected void LoadClientXmlNodeProperties(XmlNodeList arXmlProperties, ToolBarItem item)
    {
      foreach (XmlNode arProperty in arXmlProperties)
      {
        string sPropertyName = arProperty.FirstChild.FirstChild.InnerText;

        // postback ID
        if (sPropertyName == "31")
        {
          item.PostBackID = arProperty.FirstChild.LastChild.InnerText;
          continue;
        }

        if (Utils.CanParseAsInt(sPropertyName))
        {
          sPropertyName = this.PropertyIndex[sPropertyName].ToString();
        }

        string sPropertyValue = (arProperty.FirstChild.ChildNodes.Count > 1) ? arProperty.FirstChild.LastChild.InnerText : ""; // handle undefined values

        sPropertyValue = sPropertyValue.Replace("#$cAmp@*", "&");

        item.Properties[item.GetAttributeVarName(sPropertyName)] = sPropertyValue;
      }
    }

    private Hashtable _propertyIndex;
    internal Hashtable PropertyIndex
    {
      get
      {
        if (_propertyIndex == null)
        {
          _propertyIndex = new Hashtable();
          _propertyIndex["0"] = "ActiveCssClass";
          _propertyIndex["1"] = "ActiveImageUrl";
          _propertyIndex["2"] = "CausesValidation";
          _propertyIndex["3"] = "Checked";
          _propertyIndex["4"] = "CheckedActiveCssClass";
          _propertyIndex["5"] = "CheckedActiveImageUrl";
          _propertyIndex["6"] = "CheckedCssClass";
          _propertyIndex["7"] = "CheckedHoverCssClass";
          _propertyIndex["8"] = "CheckedHoverImageUrl";
          _propertyIndex["9"] = "CheckedImageUrl";
          _propertyIndex["10"] = "ClientSideCommand";
          _propertyIndex["11"] = "ClientTemplateId";
          _propertyIndex["12"] = "CssClass";
          _propertyIndex["13"] = "CustomContentId";
          _propertyIndex["14"] = "DisabledCheckedCssClass";
          _propertyIndex["15"] = "DisabledCheckedImageUrl";
          _propertyIndex["16"] = "DisabledCssClass";
          _propertyIndex["17"] = "DisabledImageUrl";
          _propertyIndex["18"] = "DropDownId";
          _propertyIndex["19"] = "Enabled";
          _propertyIndex["20"] = "ExpandedCssClass";
          _propertyIndex["21"] = "ExpandedImageUrl";
          _propertyIndex["22"] = "Height";
          _propertyIndex["23"] = "HoverCssClass";
          _propertyIndex["24"] = "HoverImageUrl";
          _propertyIndex["25"] = "ID";
          _propertyIndex["26"] = "ImageHeight";
          _propertyIndex["27"] = "ImageUrl";
          _propertyIndex["28"] = "ImageWidth";
          _propertyIndex["29"] = "ItemType";
          _propertyIndex["30"] = "KeyboardShortcut";
          _propertyIndex["31"] = "PostBackID";
          _propertyIndex["32"] = "ServerTemplateId";
          _propertyIndex["33"] = "Text";
          _propertyIndex["34"] = "TextAlign";
          _propertyIndex["35"] = "TextImageRelation";
          _propertyIndex["36"] = "TextImageSpacing";
          _propertyIndex["37"] = "TextWrap";
          _propertyIndex["38"] = "ToggleGroupId";
          _propertyIndex["39"] = "ToolTip";
          _propertyIndex["40"] = "Value";
          _propertyIndex["41"] = "Visible";
          _propertyIndex["42"] = "Width";
          _propertyIndex["43"] = "ActiveDropDownImageUrl";
          _propertyIndex["44"] = "DisabledDropDownImageUrl";
          _propertyIndex["45"] = "DropDownImageHeight";
          _propertyIndex["46"] = "DropDownImagePosition"; 
          _propertyIndex["47"] = "DropDownImageUrl";
          _propertyIndex["48"] = "DropDownImageWidth";
          _propertyIndex["49"] = "ExpandedDropDownImageUrl";
          _propertyIndex["50"] = "HoverDropDownImageUrl";
          _propertyIndex["51"] = "AllowHtmlContent";
          _propertyIndex["52"] = "AutoPostBackOnSelect";
        }
        return _propertyIndex;
      }
    }

    protected void LoadClientProperties(string sData)
    {
      try
      {
        if (sData != string.Empty)
        {
          sData = HttpUtility.UrlDecode(sData, Encoding.UTF8);

          // make it xml-safe
          sData = sData.Replace("&", "#$cAmp@*");

          XmlDocument oXmlDoc = new XmlDocument();
          oXmlDoc.LoadXml(sData);

          XmlNode oRootNode = oXmlDoc.DocumentElement;

          if (oRootNode != null)
          {
            if (oRootNode.ChildNodes.Count > 0)
            {
              this.LoadClientXmlProperties(oRootNode.ChildNodes, this.Properties);
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error loading client properties: " + ex);
      }
    }

    protected void LoadClientXmlProperties(XmlNodeList arXmlProperties, System.Web.UI.AttributeCollection arProperties)
    {
      foreach (XmlNode arProperty in arXmlProperties)
      {
        string sPropertyName = arProperty.FirstChild.FirstChild.InnerText;
        string sPropertyValue = (arProperty.FirstChild.ChildNodes.Count > 1) ? arProperty.FirstChild.LastChild.InnerText : ""; // handle undefined values

        sPropertyValue = sPropertyValue.Replace("#$cAmp@*", "&");

        arProperties[sPropertyName] = sPropertyValue;
      }
    }

    /// <summary>
    /// GetXml method.
    /// </summary>
    /// <returns>XML string represending the current structure of the data.</returns>
    /// <seealso cref="LoadXml(string)" /><seealso cref="LoadXml(System.Xml.XmlDocument)" />
    public string GetXml()
    {
      XmlDocument oXmlDoc = new XmlDocument();
      XmlNode oTopNode = oXmlDoc.CreateElement("SiteMap");
      if (this.Items != null)
      {
        this.GetXml(oXmlDoc, oTopNode);
      }
      return oTopNode.OuterXml;
    }

    private void GetXml(XmlDocument oXmlDoc, XmlNode oXmlNode)
    {
      foreach (ToolBarItem item in this.Items)
      {
        XmlElement oNewElement = oXmlDoc.CreateElement("Item");
        foreach (string sKey in item.Properties.Keys)
        {
          oNewElement.SetAttribute(sKey, item.Properties[sKey]);
        }
        oXmlNode.AppendChild(oNewElement);
      }
    }

    /// <summary>
    /// Returns the item that is checked in the toggle group with the given id.
    /// </summary>
    public ToolBarItem GetToggleGroupCheckedItem(string toggleGroupId)
    {
      ToolBarItem[] itemArray = this.GetToggleGroupCheckedItems(toggleGroupId);
      if (itemArray.Length == 0)
      {
        return null;
      }
      else
      {
        return itemArray[0];
      }
    }

    /// <summary>
    /// Returns a ToolBarItem array containing the checked items in the toggle group with the given id.
    /// </summary>
    public ToolBarItem[] GetToggleGroupCheckedItems(string toggleGroupId)
    {
      ArrayList itemList = new ArrayList();
      foreach (ToolBarItem item in this.Items)
      {
        if (item.ToggleGroupId == toggleGroupId && item.Checked)
        {
          itemList.Add(item);
        }
      }
      ToolBarItem[] itemArray = new ToolBarItem[itemList.Count];
      itemList.CopyTo(itemArray);
      return itemArray;
    }

    /// <summary>
    /// Returns a string array of all toggle group names.
    /// </summary>
    public string[] GetToggleGroupIds()
    {
      StringCollection toggleGroupIdCollection = new StringCollection();
      foreach (ToolBarItem item in this.Items)
      {
        if (item.ToggleGroupId != null && item.ToggleGroupId != String.Empty)
        {
          toggleGroupIdCollection.Add(item.ToggleGroupId);
        }
      }
      string[] toggleGroupIdArray = new String[toggleGroupIdCollection.Count];
      toggleGroupIdCollection.CopyTo(toggleGroupIdArray, 0);
      return toggleGroupIdArray;
    }

    /// <summary>
    /// Returns a ToolBarItem array containing the items in the toggle group with the given id.
    /// </summary>
    public ToolBarItem[] GetToggleGroupItems(string toggleGroupId)
    {
      ArrayList itemList = new ArrayList();
      foreach (ToolBarItem item in this.Items)
      {
        if (item.ToggleGroupId == toggleGroupId)
        {
          itemList.Add(item);
        }
      }
      ToolBarItem[] itemArray = new ToolBarItem[itemList.Count];
      itemList.CopyTo(itemArray);
      return itemArray;
    }


    private StringCollection NewCheckedPostBackIDs()
    {
      StringCollection checkedPostBackIDs = new StringCollection();
      ArrayList checkedItems = GetCheckedItems();
      foreach (ToolBarItem checkedItem in checkedItems)
      {
        if (checkedItem.PostBackID != null && checkedItem.PostBackID != String.Empty)
        {
          checkedPostBackIDs.Add(checkedItem.PostBackID);
        }
      }
      return checkedPostBackIDs;
    }

    private ArrayList GetCheckedItems()
    {
      ArrayList checkedItems = new ArrayList();
      foreach (ToolBarItem item in this.Items)
      {
        if (item.Checked)
        {
          checkedItems.Add(item);
        }
      }
      return checkedItems;
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

    public void ApplyTheming(bool? overwriteSettings)
    {
      bool overwrite = overwriteSettings ?? false;
      string prefix = this.AutoThemingCssClassPrefix ?? "";

      // Base
      if ((this.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.CssClass = prefix + "toolbar";
      }
      if ((this.DefaultItemCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemCssClass = prefix + "item-default";
      }
      if ((this.DefaultItemHoverCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemHoverCssClass = prefix + "item-hover";
      }
      if ((this.DefaultItemCheckedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemCheckedCssClass = prefix + "item-checked";
      }
      if ((this.DefaultItemExpandedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemExpandedCssClass = prefix + "item-expanded";
      }
      if ((this.DefaultItemActiveCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemActiveCssClass = prefix + "item-active";
      }
      if ((this.DefaultItemDisabledCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemDisabledCssClass = prefix + "item-disabled";
      }
      if ((this.DefaultItemDisabledCheckedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DefaultItemDisabledCheckedCssClass = prefix + "item-disabled-checked";
      }

      if (Properties["DefaultItemImageHeight"] == null || overwrite)
      {
        Properties["DefaultItemImageHeight"] = Unit.Pixel(16).ToString();
      }
      if (Properties["DefaultItemImageWidth"] == null || overwrite)
      {
        Properties["DefaultItemImageWidth"] = Unit.Pixel(16).ToString();
      }
      if (Properties["UseFadeEffect"] == null || overwrite)
      {
        Properties["UseFadeEffect"] = false.ToString();
      }
      if (this.Height.IsEmpty || overwrite)
      {
        this.Height = Unit.Pixel(25);
      }

      // Client Templates
      StringBuilder templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "toolbar-item\">");
      templateText.Append("<a href=\"javascript:void(0);\" onclick=\"this.blur();\">");
      templateText.Append("<span class=\"" + prefix + "outer\">");
      templateText.Append("<span class=\"" + prefix + "inner## DataItem.getProperty('IconCssClass') == null ? '' : ' ' +  DataItem.getProperty('IconCssClass'); ##\"></span>");
      templateText.Append("</span></a></div>");
      AddClientTemplate(overwrite, "ToolBarItemTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "toolbar-item " + prefix + "toolbar-item-label\">");
			templateText.Append("<a href=\"javascript:void(0);\" onclick=\"this.blur();\">");
      templateText.Append("<span class=\"" + prefix + "outer\">");
      templateText.Append("<span class=\"" + prefix + "inner## DataItem.getProperty('IconCssClass') == null ? '' : ' ' +  DataItem.getProperty('IconCssClass'); ##\">## DataItem.getProperty('Text').replace(/&lt;/gi, '<'); ##</span>");
      templateText.Append("</span></a></div>");
      AddClientTemplate(overwrite, "ToolBarItemLabelTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "toolbar-separator\" >");
      templateText.Append("<div class=\"" + prefix + "outer\">");
      templateText.Append("<div class=\"" + prefix + "inner\"></div>");
      templateText.Append("</div></div>");
      AddClientTemplate(overwrite, "ToolBarSeparatorTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "toolbar-separator\" >");
      templateText.Append("<div class=\"" + prefix + "outer\">");
      templateText.Append("<div class=\"" + prefix + "inner\">");
      templateText.Append("<img src=\"" + Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl) + "## DataItem.getProperty('IconUrl') ##\" width=\"## DataItem.getProperty('ImageWidth') == null ? '16' : DataItem.getProperty('ImageWidth') ##\" height=\"## DataItem.getProperty('ImageHeight') == null ? '16' : DataItem.getProperty('ImageHeight') ##\" border=\"0\" />");
      templateText.Append("</div></div></div>");
      AddClientTemplate(overwrite, "ToolBarSeparatorIconTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "toolbar-item " + prefix + "toolbar-item-left-icon\">");
      templateText.Append("<a href=\"javascript:void(0);\" onclick=\"this.blur();\">");
      templateText.Append("<span class=\"" + prefix + "outer\">");
      templateText.Append("<span class=\"" + prefix + "inner\">");
      templateText.Append("<img src=\"" + Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl) + "## DataItem.getProperty('IconUrl') ##\" width=\"16\" height=\"16\" border=\"0\" />");
      templateText.Append("</span></span></a></div>");
      AddClientTemplate(overwrite, "ToolBarItemLeftIconTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "toolbar-item " + prefix + "toolbar-item-left-icon " + prefix + "toolbar-item-label\">");
      templateText.Append("<a href=\"javascript:void(0);\" onclick=\"this.blur();\">");
      templateText.Append("<span class=\"" + prefix + "outer\">");
      templateText.Append("<span class=\"" + prefix + "inner\">");
      templateText.Append("<img src=\"" + Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl) + "## DataItem.getProperty('IconUrl') ##\" width=\"16\" height=\"16\" border=\"0\" />");
      templateText.Append("<span class=\"" + prefix + "text\">## DataItem.getProperty('Text').replace(/&lt;/gi, '<'); ##</span>");
      templateText.Append("</span></span></a></div>");
      AddClientTemplate(overwrite, "ToolBarItemLeftIconLabelTemplate", templateText.ToString());

      // Apply client templates to Items
      ApplyThemeToItems(overwrite, this.Items);
    }

    private void ApplyThemeToItems(bool overwrite, ToolBarItemCollection items)
    {
      foreach (ToolBarItem item in items)
      {
        if (item.ClientTemplateId == string.Empty || overwrite)
        {
          item.ClientTemplateId = "ToolBarItemTemplate";
          if (item.Properties["IconUrl"] != null)
          {
            item.ClientTemplateId = "ToolBarItemLeftIconTemplate";
          }

          if (item.Text != string.Empty)
          {
            item.ClientTemplateId = "ToolBarItemLabelTemplate";
            if (item.Properties["IconUrl"] != null)
            {
              item.ClientTemplateId = "ToolBarItemLeftIconLabelTemplate";
            }
          }

          if (item.ItemType == ToolBarItemType.Separator)
          {
            item.ClientTemplateId = "ToolBarSeparatorTemplate";
            if (item.Properties["IconUrl"] != null)
            {
              item.ClientTemplateId = "ToolBarSeparatorIconTemplate";
            }
          }

        }
        // Copy Text to ToolTip if ToolTip is not set
        if (item.ToolTip == string.Empty && item.Text != string.Empty)
        {
          item.ToolTip = item.Text;
        }
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

    public ToolBarItem GetItemById(string id)
    {
      foreach (ToolBarItem item in this.Items)
      {
        if (item.ID == id)
        {
          return item;
        }
      }
      return null;
    }

    private ToolBarItem GetItemByPostBackID(string postBackID)
    {
      foreach (ToolBarItem item in this.Items)
      {
        if (item.PostBackID == postBackID)
        {
          return item;
        }
      }
      return null;
    }

    #region Accessible Rendering

    internal void RenderAccessibleContent(HtmlTextWriter output)
    {
      output.Write("<span class=\"toolbar\">");

      int itemIndex = -1;

      output.Write("<ul>");

      foreach (ToolBarItem item in this.Items)
      {
        itemIndex++;

        output.Write("<li");

        string itemId = this.GetSaneId() + "_" + itemIndex.ToString();
        output.Write(" id=\"");
        output.Write(itemId);
        output.Write("\"");

        string itemCssClass = (item.CssClass != string.Empty && item.CssClass != null) ? item.CssClass : this.DefaultItemCssClass;
        if (itemCssClass != string.Empty && itemCssClass != null)
        {
          output.Write(" class=\"");
          output.Write(itemCssClass);
          output.Write("\"");
        }

        output.Write(">");

        output.Write("<a");
        output.Write(" href=\"");
        output.Write("#");
        output.Write("\"");
        output.Write(">");

        output.Write("<span>");

        output.Write(item.Text);

        output.Write("</span>");

        output.Write("</a>");

        output.Write("</li>");
      }

      output.Write("</ul>");

      output.Write("</span>");
    }

    #endregion Accessible Rendering

    #region Down-level Rendering

    internal void RenderDownLevelToolBar(HtmlTextWriter output)
    {
      output.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"");
      output.Write(this.ItemSpacing.IsEmpty ? "0" : this.ItemSpacing.Value.ToString());
      output.Write("\"");
      if (this.CssClass != null && this.CssClass != string.Empty)
      {
        output.Write(" class=\"");
        output.Write(this.CssClass);
        output.Write("\"");
      }
      output.Write(" style=\"vertical-align:middle;text-align:center;");
      if (!this.Width.IsEmpty)
      {
        output.Write("width:");
        output.Write(this.Width.ToString());
        output.Write(";");
      }
      if (!this.Height.IsEmpty)
      {
        output.Write("height:");
        output.Write(this.Height.ToString());
        output.Write(";");
      }
      output.Write("\"><tr>");
      bool isVertical = this.Orientation == GroupOrientation.Vertical;
      foreach (ToolBarItem item in this.Items)
      {
        if (item.Visible)
        {
          RenderDownLevelToolBarItem(item, output);
          if (isVertical)
          {
            output.Write("</tr><tr>");
          }
        }
      }
      output.Write("</tr></table>");
    }

    private void RenderDownLevelToolBarItem(ToolBarItem item, HtmlTextWriter output)
    {
      output.Write("<td");
      if (item.ToolTip != null && item.ToolTip != string.Empty)
      {
        output.Write(" title=\"");
        output.Write(item.ToolTip);
        output.Write("\"");
      }
      output.Write(" class=\"");
      output.Write(item.CssClass != null && item.CssClass != string.Empty ? item.CssClass : this.DefaultItemCssClass);
      output.Write("\" style=\"vertical-align:middle;text-align:center;");
      if (!item.Width.IsEmpty)
      {
        output.Write("width:");
        output.Write(item.Width.ToString());
        output.Write(";");
      }
      if (!item.Height.IsEmpty)
      {
        output.Write("height:");
        output.Write(item.Height.ToString());
        output.Write(";");
      }
      output.Write("\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr></td>");
      bool showItemText = item.Text != null && item.Text != string.Empty && item.TextImageRelation != ToolBarTextImageRelation.ImageOnly;
      string itemTextHtml = showItemText ? DownLevelItemTextHtml(item) : string.Empty;
      bool showItemImage = item.ImageUrl != null && item.ImageUrl != string.Empty && item.TextImageRelation != ToolBarTextImageRelation.TextOnly;
      string itemImageHtml = showItemImage ? DownLevelItemImageHtml(item, showItemText) : string.Empty;
      bool showItemDropDownImage = item.DropDownImageUrl != null && item.DropDownImageUrl != string.Empty && (item.ItemType == ToolBarItemType.DropDown || item.ItemType ==ToolBarItemType.SplitDropDown);
      int itemCellCount = 0;
      if (showItemText) {itemCellCount++;}
      if (showItemImage) {itemCellCount++;}
      if (itemCellCount > 0 || showItemDropDownImage)
      {
        bool itemIsVertical = item.TextImageRelation == ToolBarTextImageRelation.ImageAboveText || item.TextImageRelation == ToolBarTextImageRelation.TextAboveImage;
        int itemRowCount = (itemCellCount == 2 && itemIsVertical) ? 2 : 1;
        string itemDropDownImageHtml = showItemDropDownImage ? DownLevelItemDropDownImageUrl(item, itemRowCount) : string.Empty;
        string firstCellHtml = string.Empty;
        string secondCellHtml = string.Empty;
        if (itemCellCount == 1)
        {
          firstCellHtml = showItemText ? itemTextHtml : itemImageHtml;
        }
        else if (itemCellCount == 2)
        {
          bool imagePrecedesText = item.TextImageRelation == ToolBarTextImageRelation.ImageOnly || item.TextImageRelation == ToolBarTextImageRelation.ImageAboveText || item.TextImageRelation == ToolBarTextImageRelation.ImageBeforeText;
          firstCellHtml = imagePrecedesText ? itemImageHtml : itemTextHtml;
          secondCellHtml = imagePrecedesText ? itemTextHtml : itemImageHtml;
        }
        if (showItemDropDownImage && item.DropDownImagePosition == ToolBarDropDownImagePosition.Left)
        {
          output.Write(itemDropDownImageHtml);
        }
        output.Write(firstCellHtml);
        if (itemCellCount == 2 && itemRowCount == 1)
        {
          output.Write(secondCellHtml);
        }
        if (showItemDropDownImage && item.DropDownImagePosition == ToolBarDropDownImagePosition.Right)
        {
          output.Write(itemDropDownImageHtml);
        }
        if (itemRowCount == 2)
        {
          output.Write("</tr><tr>");
          output.Write(secondCellHtml);
        }
      }
      output.Write("</tr></table></td>");
    }

    private string DownLevelItemTextHtml(ToolBarItem item)
    {
      return "<td class=\"ca_tb_txt\">" + item.Text + "</td>";
    }

    private string DownLevelItemImageHtml(ToolBarItem item, bool showTextImageSpacing)
    {
      StringBuilder html = new StringBuilder();
      html.Append("<td class=\"ca_tb_img\"><img border=\"0\" style=\"display:block;");
      if (showTextImageSpacing && item.TextImageSpacing > 0)
      {
        switch (item.TextImageRelation)
        {
          case ToolBarTextImageRelation.ImageAboveText:
            html.Append("margin-bottom:" + item.TextImageSpacing.ToString() + "px;");
            break;
          case ToolBarTextImageRelation.TextAboveImage:
            html.Append("margin-top:" + item.TextImageSpacing.ToString() + "px;");
            break;
          case ToolBarTextImageRelation.ImageBeforeText:
            html.Append("margin-right:" + item.TextImageSpacing.ToString() + "px;");
            break;
          case ToolBarTextImageRelation.TextBeforeImage:
            html.Append("margin-left:" + item.TextImageSpacing.ToString() + "px;");
            break;
        }
      }
      html.Append("\" alt=\"");
      html.Append(item.ToolTip);
      html.Append("\"");
      if (!item.ImageWidth.IsEmpty)
      {
        html.Append(" width=\"");
        html.Append(item.ImageWidth.Value.ToString());
        html.Append("\"");
      }
      if (!item.ImageHeight.IsEmpty)
      {
        html.Append(" height=\"");
        html.Append(item.ImageHeight.Value.ToString());
        html.Append("\"");
      }
      html.Append(" src=\"");
      html.Append(Utils.ConvertUrl(Context, this.ImagesBaseUrl, item.ImageUrl));
      html.Append("\" /></td>");
      return html.ToString();
    }

    private string DownLevelItemDropDownImageUrl(ToolBarItem item, int rowCount)
    {
      StringBuilder html = new StringBuilder();
      html.Append("<td class=\"ca_tb_ddn\" rowspan=\"");
      html.Append(rowCount);
      html.Append("\"><img border=\"0\" style=\"display:block;\" alt=\"");
      html.Append(item.ToolTip);
      html.Append("\"");

      if (!item.DropDownImageWidth.IsEmpty)
      {
        html.Append(" width=\"");
        html.Append(item.DropDownImageWidth.Value.ToString());
        html.Append("\"");
      }
      if (!item.DropDownImageHeight.IsEmpty)
      {
        html.Append(" height=\"");
        html.Append(item.DropDownImageHeight.Value.ToString());
        html.Append("\"");
      }
      html.Append(" src=\"");
      html.Append(Utils.ConvertUrl(Context, this.ImagesBaseUrl, item.DropDownImageUrl));
      html.Append("\" /></td>");
      return html.ToString();
    }

    #endregion

    #region Events

    /// <summary>
    /// Delegate for <see cref="ItemCheckChanged"/> event of <see cref="ToolBar"/> class.
    /// </summary>
    public delegate void ItemCheckChangedEventHandler(object sender, ToolBarItemEventArgs e);

    /// <summary>
    /// Fires after an item is checked or unchecked.
    /// </summary>
    [Description("Fires after an item is checked or unchecked.")]
    [Category("Events")]
    public event ItemCheckChangedEventHandler ItemCheckChanged;

    private void OnItemCheckChanged(ToolBarItemEventArgs e)
    {
      if (this.ItemCheckChanged != null)
      {
        this.ItemCheckChanged(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="ItemDataBound"/> event of <see cref="ToolBar"/> class.
    /// </summary>
    public delegate void ItemDataBoundEventHandler(object sender, ToolBarItemDataBoundEventArgs e);

    /// <summary>
    /// Fires after an item is data bound.
    /// </summary>
    [Description("Fires after an item is data bound.")]
    [Category("Events")]
    public event ItemDataBoundEventHandler ItemDataBound;

    private void OnItemDataBound(ToolBarItem item, object dataItem)
    {
      if (this.ItemDataBound != null)
      {
        ToolBarItemDataBoundEventArgs e = new ToolBarItemDataBoundEventArgs();
        e.Item = item;
        e.DataItem = dataItem;
        this.ItemDataBound(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="ItemCommand"/> event of <see cref="ToolBar"/> class.
    /// </summary>
    public delegate void ItemCommandEventHandler(object sender, ToolBarItemEventArgs e);

    /// <summary>
    /// Fires if the postback was caused by a toolbar item.
    /// </summary>
    /// <remarks>
    /// This is only possible when <see cref="ToolBar.AutoPostBackOnSelect"/> is set to true.
    /// </remarks>
    [Description("Fires if the postback was caused by a toolbar item.")]
    [Category("Events")]
    public event ItemCommandEventHandler ItemCommand;

    private void OnItemCommand(ToolBarItemEventArgs e)
    {
      if (ItemCommand != null)
      {
        ItemCommand(this, e);
      }
    }

    #endregion
    
  }

  #region Events

  /// <summary>
  /// Arguments for <see cref="ToolBar.ItemCheckChanged">ItemCheckChanged</see> and <see cref="ToolBar.ItemCommand">ItemCommand</see> 
  /// server-side events of <see cref="ToolBar"/> control.
  /// </summary>
  public class ToolBarItemEventArgs : EventArgs
  {
    /// <summary>
    /// The ToolBarItem which the event relates to.
    /// </summary>
    public ToolBarItem Item;

    public ToolBarItemEventArgs(ToolBarItem item)
    {
      this.Item = item;
    }
  }

  /// <summary>
  /// Arguments for <see cref="ToolBar.ItemDataBound"/> server-side event of <see cref="ToolBar"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class ToolBarItemDataBoundEventArgs : EventArgs
  {
    /// <summary>
    /// The toolbar item which the event relates to.
    /// </summary>
    public ToolBarItem Item;

    /// <summary>
    /// The data item bound to.
    /// </summary>
    public object DataItem;
  }

  #endregion

  #region Template Classes

  /// <summary>
  /// Template class used for specifying customized rendering for <see cref="ToolBarItem"/> instances.
  /// </summary>
  [DefaultProperty("Template")]
  [ParseChildren(true)]
  [PersistChildren(false)]
  [ToolboxItem(false)]
  public class ToolBarCustomTemplate : System.Web.UI.WebControls.WebControl
  {
    private ITemplate _template;
    /// <summary>
    /// The template.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(ComponentArt.Web.UI.ToolBarTemplateContainer))]
    [NotifyParentProperty(true)]
    public ITemplate Template
    {
      get
      {
        return this._template;
      }
      set
      {
        this._template = value;
      }
    }
  }

  /// <summary>
  /// Collection of <see cref="ToolBarCustomTemplate"/> objects.
  /// </summary>
  public class ToolBarCustomTemplateCollection : CollectionBase
  {
    public new ToolBarCustomTemplate this[int index]
    {
      get
      {
        return (ToolBarCustomTemplate)base.List[index];
      }
      set
      {
        base.List[index] = value;
      }
    }

    public new int Add(ToolBarCustomTemplate template)
    {
      return this.List.Add(template);
    }
  }

  /// <summary>
  /// Naming container for a customized <see cref="ToolBarItem"/> instance.
  /// </summary>
  [ToolboxItem(false)]
  public class ToolBarTemplateContainer : ToolBarItemContent, INamingContainer
  {
    private ToolBarItem _item;
    private System.Web.UI.AttributeCollection _attributes;
    /// <summary>
    /// ToolBarTemplateContainer constructor.
    /// </summary>
    /// <param name="item">Item acting as the data source.</param>
    public ToolBarTemplateContainer(ToolBarItem item)
    {
      this._item = item;
      if (item != null)
      {
        this._attributes = item.Attributes;
      }
    }
    /// <summary>
    /// Item containing data to bind to (a ToolBarItem).
    /// </summary>
    public ToolBarItem DataItem
    {
      get
      {
        return this._item;
      }
    }
    /// <summary>
    /// Attributes of the given data item.
    /// </summary>
    public System.Web.UI.AttributeCollection Attributes
    {
      get
      {
        return this._attributes;
      }
    }
  }

  #endregion

  #region Content Classes

  /// <summary>
  /// Houses the content of a <see cref="ToolBar"/> <see cref="ToolBarItem">item</see>.
  /// </summary>
  [ParseChildren(false)]
  [PersistChildren(true)]
  [ToolboxItem(false)]
  public class ToolBarItemContent : System.Web.UI.Control
  {
  }

  /// <summary>
  /// Collection of <see cref="ToolBarItemContent"/> controls.
  /// </summary>
  [Editor("System.Windows.Forms.Design.CollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
  public class ToolBarItemContentCollection : ControlCollection
  {
    // Needed by Intellisense in framework 2, but causes a runtime error in framework 1
    public new ToolBarItemContent this[int index]
    {
      get
      {
        return (ToolBarItemContent)base[index];
      }
    }

    /// <summary>
    /// Initializes a new instance of a ToolBarItemContentCollection. 
    /// </summary>
    /// <param name="owner">The parent ToolBar control.</param>
    public ToolBarItemContentCollection(ToolBar owner)
      : base(owner)
    {
    }
    public new void Add(ToolBarItemContent child)
    {
      base.Add(child);
    }
    public new void AddAt(int index, ToolBarItemContent child)
    {
      base.AddAt(index, child);
    }
  }

  #endregion

  #region Supporting types

  /// <summary>
  /// Specifies how a <see cref="ToolBarItem"/> functions.
  /// </summary>
  public enum ToolBarItemType
  {
    /// <summary>The item functions as a button.</summary>
    Command,

    /// <summary>A break item that has no functionality.</summary>
    Separator,

    /// <summary>The item functions as a checkbox.</summary>
    ToggleCheck,

    /// <summary>The item functions as a radio button in a group of items where exactly one is checked at any time.</summary>
    ToggleRadio,

    /// <summary>The item functions as a radio button in a group of items where at most one is checked at any time.</summary>
    ToggleRadioCheck,

    /// <summary>The item has an associated drop down <see cref="Menu"/>.</summary>
    DropDown,

    /// <summary>The item is split, one half acting as a command item, and the other half acting as a drop down item.</summary>
    SplitDropDown
  }

  /// <summary>
  /// Specifies how the image and the text are positioned within the toolbar item.
  /// </summary>
  public enum ToolBarTextImageRelation
  {
    /// <summary>Image is on top, text is on the bottom.</summary>
    ImageAboveText,

    /// <summary>Image is on the left, text is on the right.</summary>
    ImageBeforeText,

    /// <summary>Only display the image.</summary>
    ImageOnly,

    /// <summary>Text is on top, image is on the bottom.</summary>
    TextAboveImage,

    /// <summary>Text is on the left, image is on the right.</summary>
    TextBeforeImage,

    /// <summary>Only display the text.</summary>
    TextOnly
  }

  /// <summary>
  /// Specifies where the dropdown image is positioned relative to the rest of toolbar item contents.
  /// </summary>
  public enum ToolBarDropDownImagePosition
  {
    /// <summary>The drop down image is on the left, the rest of item contents are on the right.</summary>
    Left,

    /// <summary>The drop down image is on the right, the rest of item contents are on the left.</summary>
    Right    
  }

  #endregion

}
