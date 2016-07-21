using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Runtime.InteropServices;
using ComponentArt.Licensing.Providers;
using System.Web.UI.HtmlControls;

namespace ComponentArt.Web.UI
{
  #region ComboBoxContent

  /// <summary>
  /// Class for content sections of the <see cref="ComboBox"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class ComboBoxContent : Control
  {

  }

  #endregion

  #region ComboBoxItemCollectionEditor

  public class ComboBoxItemCollectionEditor : CollectionEditor
  {
    public ComboBoxItemCollectionEditor(Type type)
      : base(type)
    {
    }

    protected override bool CanSelectMultipleInstances()
    {
      return false;
    }

    protected override Type CreateCollectionItemType()
    {
      return typeof(ComboBoxItem);
    }
  }

  #endregion

  /// <summary>
  /// Displays a combo box in a web page, offering functionality to select an item from a list.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The ComboBox control consists of an input box and an associated dropdown, which is displayed on demand. The dropdown
  /// can include data items loaded from a database, defined inline on the ASPX page, or created programmatically. It can also
  /// contained custom ASP.NET content.
  /// </para>
  /// <para>
  /// Notable features of ComboBox include load-on-demand for large datasets (with caching), auto-filter, auto-highlight and auto-complete
  /// functionality, dynamic resizing ability, as well as highly customizable display elements.
  /// </para>
  /// <para>
  /// In addition to the server-side API, ComboBox also exposes rich functionality on the client.
  /// </para>
  /// </remarks>
  [Designer(typeof(ComponentArt.Web.UI.ComboBoxDesigner))]
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:ComboBox Width=400 runat=server></{0}:ComboBox>")]
  [ParseChildren(true)]
  [PersistChildren(false)]
  [ValidationProperty("SelectedItem")]
  public sealed class ComboBox : WebControl, IPostBackEventHandler
  {
    private bool _dataBound = false;
    private bool _raiseChangeEvent = false;


    #region Public Properties

    /// <summary>
    /// Whether to adjust the position of the dropdown to account for relative or absolute positioning. Default: true.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to adjust the position of the dropdown to account for relative or absolute positioning. Default: true.")]
    [DefaultValue(true)]
    public bool AdjustPositioning
    {
      get
      {
        object o = ViewState["AdjustPositioning"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["AdjustPositioning"] = value;
      }
    }

    /// <summary>
    /// Whether to auto-complete text in the input field as the user types.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to auto-complete text in the input field as the user types.")]
    [DefaultValue(true)]
    public bool AutoComplete
    {
      get
      {
        object o = ViewState["AutoComplete"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["AutoComplete"] = value;
      }
    }

    /// <summary>
    /// Whether to filter dropdown options as the user types.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to filter dropdown options as the user types.")]
    [DefaultValue(true)]
    public bool AutoFilter
    {
      get
      {
        object o = ViewState["AutoFilter"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["AutoFilter"] = value;
      }
    }

    /// <summary>
    /// Whether to auto-highlight the closest match as the user types.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to auto-highlight the closest match as the user types.")]
    [DefaultValue(true)]
    public bool AutoHighlight
    {
      get
      {
        object o = ViewState["AutoHighlight"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["AutoHighlight"] = value;
      }
    }
    
    /// <summary>
    /// Whether to perform a postback when the selection is changed on the client. Default: false.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to perform a postback when the selection is changed on the client. Default: false.")]
    [DefaultValue(false)]
    public bool AutoPostBack
    {
      get
      {
        object o = ViewState["AutoPostBack"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["AutoPostBack"] = value;
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
    /// Whether to display a visual indicator of loaded data.
    /// </summary>
    [Category("Appearance")]
    [Description("Whether to display a visual indicator of loaded data.")]
    [DefaultValue(false)]
    public bool CacheMapEnabled
    {
      get
      {
        object o = ViewState["CacheMapEnabled"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["CacheMapEnabled"] = value;
      }
    }

    /// <summary>
    /// The color of loaded areas on the data map.
    /// </summary>
    [Category("Appearance")]
    [Description("The color of loaded areas on the data map.")]
    [DefaultValue("#808089")]
    public string CacheMapLoadedColor
    {
      get
      {
        object o = ViewState["CacheMapLoadedColor"];
        return (o == null) ? "#808089" : (string)o;
      }
      set
      {
        ViewState["CacheMapLoadedColor"] = value;
      }
    }

    /// <summary>
    /// The color of areas about to be loaded on the data map.
    /// </summary>
    [Category("Appearance")]
    [Description("The color of areas about to be loaded on the data map.")]
    [DefaultValue("#f07070")]
    public string CacheMapLoadingColor
    {
      get
      {
        object o = ViewState["CacheMapLoadingColor"];
        return (o == null) ? "#f07070" : (string)o;
      }
      set
      {
        ViewState["CacheMapLoadingColor"] = value;
      }
    }

    /// <summary>
    /// The color of non-loaded areas on the data map.
    /// </summary>
    [Category("Appearance")]
    [Description("The color of non-loaded areas on the data map.")]
    [DefaultValue("#f0f0f0")]
    public string CacheMapNotLoadedColor
    {
      get
      {
        object o = ViewState["CacheMapNotLoadedColor"];
        return (o == null) ? "#f0f0f0" : (string)o;
      }
      set
      {
        ViewState["CacheMapNotLoadedColor"] = value;
      }
    }

    /// <summary>
    /// The width of the data map in pixels.
    /// </summary>
    [Category("Appearance")]
    [Description("The width of the data map in pixels.")]
    [DefaultValue(10)]
    public int CacheMapWidth
    {
      get
      {
        object o = ViewState["CacheMapWidth"];
        return (o == null) ? 10 : (int)o;
      }
      set
      {
        ViewState["CacheMapWidth"] = value;
      }
    }

    /// <summary>
    /// The maximum number of items to keep loaded.
    /// </summary>
    [Category("Data")]
    [Description("The maximum number of items to keep loaded.")]
    [DefaultValue(200)]
    public int CacheSize
    {
      get
      {
        object o = ViewState["CacheSize"];
        return (o == null) ? 200 : (int)o;
      }
      set
      {
        ViewState["CacheSize"] = value;
      }
    }

    /// <summary>
    /// Time to wait before performing a callback filter on the typed text, in milliseconds.
    /// </summary>
    [Category("Behavior")]
    [Description("Time to wait before performing a callback filter on the typed text, in milliseconds.")]
    [DefaultValue(250)]
    public int CallbackFilterDelay
    {
      get
      {
        object o = ViewState["CallbackFilterDelay"];
        return (o == null) ? 250 : (int)o;
      }
      set
      {
        ViewState["CallbackFilterDelay"] = value;
      }
    }

    /// <summary>
    /// Minimum number of characters typed in the ComboBox needed to trigger a callback filter.
    /// A callback will always be triggered if the ComboBox becomes blank.
    /// </summary>
    [Category("Behavior")]
    [Description("Minimum number of characters typed in the ComboBox needed to trigger a callback filter.")]
    [DefaultValue(0)]
    public int CallbackFilterMinimumLength
    {
      get
      {
        object o = ViewState["CallbackFilterMinimumLength"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["CallbackFilterMinimumLength"] = value;
      }
    }

    private ComboBoxClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public ComboBoxClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new ComboBoxClientEvents();
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

    internal ClientTemplateCollection _clientTemplates = new ClientTemplateCollection();
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
        return (ClientTemplateCollection)_clientTemplates;
      }
    }

    /// <summary>
    /// The duration (in milliseconds) of the dropdown collapse animation.
    /// </summary>
    /// <seealso cref="CollapseSlide" />
    [Description("The duration (in milliseconds) of the dropdown collapse animation.")]
    [DefaultValue(200)]
    [Category("Animation")]
    public int CollapseDuration
    {
      get
      {
        return Utils.ParseInt(ViewState["CollapseDuration"], 200);
      }
      set
      {
        ViewState["CollapseDuration"] = value.ToString();
      }
    }

    /// <summary>
    /// The slide type to use for the dropdown collapse animation.
    /// </summary>
    /// <seealso cref="CollapseDuration" />
    [Description("The slide type to use for the dropdown collapse animation.")]
    [DefaultValue(SlideType.ExponentialDecelerate)]
    [Category("Animation")]
    public SlideType CollapseSlide
    {
      get
      {
        return Utils.ParseSlideType(ViewState["CollapseSlide"], SlideType.ExponentialDecelerate);
      }
      set
      {
        ViewState["CollapseSlide"] = value.ToString();
      }
    }

    /// <summary>
    /// The comma-delimited list of extra fields to load from the data source.
    /// </summary>
    [Category("Data")]
    [Description("The comma-delimited list of extra fields to load from the data source.")]
    [DefaultValue("")]
    public string DataFields
    {
      get
      {
        object o = ViewState["DataFields"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DataFields"] = value;
      }
    }

    /// <summary>
    /// The member in the DataSource from which to load items.
    /// </summary>
    [Category("Data")]
    [Description("The member in the DataSource from which to load items.")]
    [DefaultValue("")]
    public string DataMember
    {
      get
      {
        object o = ViewState["DataMember"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DataMember"] = value;
      }
    }

    private object _dataSource;
    /// <summary>
    /// The DataSource to bind to.
    /// </summary>
    [DefaultValue(null)]
    [Description("The DataSource to bind to.")]
    public object DataSource
    {
      set
      {
        _dataSource = value;
      }
      get
      {
        return _dataSource;
      }
    }


    /// <summary>
    /// The ID of the data source control to bind to."
    /// </summary>
    [DefaultValue("")]
    [Description("The ID of the data source control to bind to.")]
    public string DataSourceID
    {
      get
      {
        object o = ViewState["DataSourceID"];
        return (o == null) ? string.Empty : (string)o;
      }
      set
      {
        ViewState["DataSourceID"] = value;
      }
    }


    /// <summary>
    /// The format in which to display the text field.
    /// </summary>
    [Category("Data")]
    [Description("The format in which to display the text field.")]
    [DefaultValue("")]
    public string DataTextFormatString
    {
      get
      {
        object o = ViewState["DataTextFormatString"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DataTextFormatString"] = value;
      }
    }

    /// <summary>
    /// The field in the data source from which to load text values.
    /// </summary>
    [Category("Data")]
    [Description("The field in the data source from which to load text values.")]
    [DefaultValue("")]
    public string DataTextField
    {
      get
      {
        object o = ViewState["DataTextField"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DataTextField"] = value;
      }
    }

    /// <summary>
    /// The field in the data source from which to load item values.
    /// </summary>
    [Category("Data")]
    [Description("The field in the data source from which to load item values.")]
    [DefaultValue("")]
    public string DataValueField
    {
      get
      {
        object o = ViewState["DataValueField"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DataValueField"] = value;
      }
    }

    /// <summary>
    /// Whether to enabling debugging feedback.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to enabling debugging feedback.")]
    [DefaultValue(false)]
    public bool Debug
    {
      get
      {
        object o = ViewState["Debug"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["Debug"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use when the ComboBox is disabled.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use when the ComboBox is disabled.")]
    [DefaultValue("")]
    public string DisabledCssClass
    {
      get
      {
        object o = ViewState["DisabledCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DisabledCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for disabled items.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for disabled items.")]
    [DefaultValue("")]
    public string DisabledItemCssClass
    {
      get
      {
        object o = ViewState["DisabledItemCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DisabledItemCssClass"] = value;
      }
    }

    /// <summary>
    /// The image to use for expanding the dropdown, on mouse down.
    /// </summary>
    [Category("Appearance")]
    [Description("The image to use for expanding the dropdown, on mouse down.")]
    [DefaultValue("")]
    public string DropActiveImageUrl
    {
      get
      {
        object o = ViewState["DropActiveImageUrl"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DropActiveImageUrl"] = value;
      }
    }

    private ComboBoxContent _dropDownContent;
    /// <summary>
    /// Content for the dropdown.
    /// </summary>
    [Browsable(false)]
    [Category("Layout")]
    [Description("Content for the dropdown.")]
    [DefaultValue(null)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ComboBoxContent DropDownContent
    {
      get
      {
        return _dropDownContent;
      }
      set
      {
        if (_dropDownContent != null)
        {
          this.Controls.Remove(_dropDownContent);
        }

        _dropDownContent = value;

        this.Controls.Add(_dropDownContent);
      }
    }

    private ComboBoxContent _dropDownFooter;
    /// <summary>
    /// Footer content for the dropdown.
    /// </summary>
    [Category("Layout")]
    [Description("Footer content for the dropdown.")]
    [DefaultValue(null)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ComboBoxContent DropDownFooter
    {
      get
      {
        return _dropDownFooter;
      }
      set
      {
        if (_dropDownFooter != null)
        {
          this.Controls.Remove(_dropDownFooter);
        }

        _dropDownFooter = value;

        this.Controls.Add(_dropDownFooter);
      }
    }

    private ComboBoxContent _dropDownHeader;
    /// <summary>
    /// Header content for the dropdown.
    /// </summary>
    [Category("Layout")]
    [Description("Header content for the dropdown.")]
    [DefaultValue(null)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ComboBoxContent DropDownHeader
    {
      get
      {
        return _dropDownHeader;
      }
      set
      {
        if (_dropDownHeader != null)
        {
          this.Controls.Remove(_dropDownHeader);
        }

        _dropDownHeader = value;

        this.Controls.Add(_dropDownHeader);
      }
    }

    /// <summary>
    /// The CSS class to use for the dropdown content area.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for the dropdown content area.")]
    [DefaultValue("")]
    public string DropDownContentCssClass
    {
      get
      {
        object o = ViewState["DropDownContentCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DropDownContentCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the dropdown.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for the dropdown.")]
    [DefaultValue("")]
    public string DropDownCssClass
    {
      get
      {
        object o = ViewState["DropDownCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DropDownCssClass"] = value;
      }
    }

    /// <summary>
    /// The height of the dropdown, in pixels.
    /// </summary>
    [Category("Layout")]
    [Description("The height of the dropdown, in pixels.")]
    [DefaultValue(0)]
    public int DropDownHeight
    {
      get
      {
        object o = ViewState["DropDownHeight"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["DropDownHeight"] = value;
      }
    }

    /// <summary>
    /// The offset to be applied to the default x coordinate of the dropdown.
    /// </summary>
    [Category("Layout")]
    [Description("The offset to be applied to the default x coordinate of the dropdown.")]
    [DefaultValue(0)]
    public int DropDownOffsetX
    {
      get
      {
        object o = ViewState["DropDownOffsetX"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["DropDownOffsetX"] = value;
      }
    }

    /// <summary>
    /// The offset to be applied to the default y coordinate of the dropdown.
    /// </summary>
    [Category("Layout")]
    [Description("The offset to be applied to the default y coordinate of the dropdown.")]
    [DefaultValue(0)]
    public int DropDownOffsetY
    {
      get
      {
        object o = ViewState["DropDownOffsetY"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["DropDownOffsetY"] = value;
      }
    }

    /// <summary>
    /// The height of the dropdown, in the number of items.
    /// </summary>
    [Category("Layout")]
    [Description("The height of the dropdown, in the number of items.")]
    [DefaultValue(10)]
    public int DropDownPageSize
    {
      get
      {
        object o = ViewState["DropDownPageSize"];
        return (o == null) ? 10 : (int)o;
      }
      set
      {
        ViewState["DropDownPageSize"] = value;
      }
    }

    /// <summary>
    /// Whether to enable user resizing of the dropdown.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to enable user resizing of the dropdown.")]
    [DefaultValue(ComboBoxResizingMode.Off)]
    public ComboBoxResizingMode DropDownResizingMode
    {
      get
      {
        object o = ViewState["DropDownResizingMode"];
        return (o == null) ? ComboBoxResizingMode.Off : (ComboBoxResizingMode)o;
      }
      set
      {
        ViewState["DropDownResizingMode"] = value;
      }
    }

    /// <summary>
    /// The style of the visual feedback to provide while resizing the dropdown.
    /// </summary>
    [Category("Behavior")]
    [Description("The style of the visual feedback to provide while resizing the dropdown.")]
    [DefaultValue(ComboBoxResizingStyle.Live)]
    public ComboBoxResizingStyle DropDownResizingStyle
    {
      get
      {
        object o = ViewState["DropDownResizingStyle"];
        return (o == null) ? ComboBoxResizingStyle.Live : (ComboBoxResizingStyle)o;
      }
      set
      {
        ViewState["DropDownResizingStyle"] = value;
      }
    }

    /// <summary>
    /// The width of the dropdown, in pixels.
    /// </summary>
    [Category("Layout")]
    [Description("The width of the dropdown, in pixels.")]
    [DefaultValue(0)]
    public int DropDownWidth
    {
      get
      {
        object o = ViewState["DropDownWidth"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["DropDownWidth"] = value;
      }
    }

    /// <summary>
    /// The image to use for expanding the dropdown, on hover.
    /// </summary>
    [Category("Appearance")]
    [Description("The image to use for expanding the dropdown, on hover.")]
    [DefaultValue("")]
    public string DropHoverImageUrl
    {
      get
      {
        object o = ViewState["DropHoverImageUrl"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DropHoverImageUrl"] = value;
      }
    }

    /// <summary>
    /// The width of the drop image in pixels.
    /// </summary>
    [Category("Appearance")]
    [Description("The width of the drop image in pixels.")]
    [DefaultValue(0)]
    public int DropImageHeight
    {
      get
      {
        object o = ViewState["DropImageHeight"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["DropImageHeight"] = value;
      }
    }

    /// <summary>
    /// The image to use for expanding the dropdown.
    /// </summary>
    [Category("Appearance")]
    [Description("The image to use for expanding the dropdown.")]
    [DefaultValue("")]
    public string DropImageUrl
    {
      get
      {
        object o = ViewState["DropImageUrl"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["DropImageUrl"] = value;
      }
    }

    /// <summary>
    /// The width of the drop image in pixels.
    /// </summary>
    [Category("Appearance")]
    [Description("The width of the drop image in pixels.")]
    [DefaultValue(0)]
    public int DropImageWidth
    {
      get
      {
        object o = ViewState["DropImageWidth"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["DropImageWidth"] = value;
      }
    }

    /// <summary>
    /// The direction of the dropdown expansion.
    /// </summary>
    /// <seealso cref="ExpandDirection" />
    [Description("The direction of the dropdown expansion.")]
    [DefaultValue(ComboBoxExpandDirection.Down)]
    [Category("Layout")]
    public ComboBoxExpandDirection ExpandDirection
    {
      get
      {
        object o = ViewState["ExpandDirection"];
        return o == null ? ComboBoxExpandDirection.Down : (ComboBoxExpandDirection)o;
      }
      set
      {
        ViewState["ExpandDirection"] = value;
      }
    }

    /// <summary>
    /// The duration (in milliseconds) of the dropdown expand animation.
    /// </summary>
    /// <seealso cref="ExpandSlide" />
    [Description("The duration (in milliseconds) of the dropdown expand animation.")]
    [DefaultValue(200)]
    [Category("Animation")]
    public int ExpandDuration
    {
      get
      {
        return Utils.ParseInt(ViewState["ExpandDuration"], 200);
      }
      set
      {
        ViewState["ExpandDuration"] = value.ToString();
      }
    }

    /// <summary>
    /// The slide type to use for the dropdown expand animation.
    /// </summary>
    /// <seealso cref="ExpandDuration" />
    [Description("The slide type to use for the dropdown expand animation.")]
    [DefaultValue(SlideType.ExponentialDecelerate)]
    [Category("Animation")]
    public SlideType ExpandSlide
    {
      get
      {
        return Utils.ParseSlideType(ViewState["ExpandSlide"], SlideType.ExponentialDecelerate);
      }
      set
      {
        ViewState["ExpandSlide"] = value.ToString();
      }
    }

    /// <summary>
    /// The number of callback filter result sets to cache.
    /// </summary>
    [Category("Data")]
    [Description("The number of callback filter result sets to cache.")]
    [DefaultValue(10)]
    public int FilterCacheSize
    {
      get
      {
        object o = ViewState["FilterCacheSize"];
        return (o == null) ? 10 : (int)o;
      }
      set
      {
        ViewState["FilterCacheSize"] = value;
      }
    }

    /// <summary>
    /// The field to filter by, when filtering is required.
    /// </summary>
    [Category("Data")]
    [Description("The field to filter by, when filtering is required.")]
    [DefaultValue("")]
    public string FilterField
    {
      get
      {
        object o = ViewState["FilterField"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["FilterField"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the combobox when it's focused.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for the combobox when it's focused.")]
    [DefaultValue("")]
    public string FocusedCssClass
    {
      get
      {
        object o = ViewState["FocusedCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["FocusedCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the combobox, on hover.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for the combobox, on hover.")]
    [DefaultValue("")]
    public string HoverCssClass
    {
      get
      {
        object o = ViewState["HoverCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["HoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for items.
    /// </summary>
    [Category("Appearance")]
    [Description("The ID of the client template to use for items.")]
    [DefaultValue("")]
    public string ItemClientTemplateId
    {
      get
      {
        object o = ViewState["ItemClientTemplateId"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["ItemClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The total number of items in the data set.
    /// </summary>
    /// <remarks>This should be programmatically specified when using load-on-demand
    /// to inform the ComboBox of the total extent of the record set beyond the loaded
    /// items.</remarks>
    [Category("Data")]
    [Description("The total number of items in the data set.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int ItemCount
    {
      get
      {
        object o = ViewState["ItemCount"];
        return (o == null) ? this.Items.Count : (int)o;
      }
      set
      {
        ViewState["ItemCount"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for dropdown items.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for dropdown items.")]
    [DefaultValue("")]
    public string ItemCssClass
    {
      get
      {
        object o = ViewState["ItemCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["ItemCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for dropdown items on hover.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for dropdown items on hover.")]
    [DefaultValue("")]
    public string ItemHoverCssClass
    {
      get
      {
        object o = ViewState["ItemHoverCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["ItemHoverCssClass"] = value;
      }
    }

    private ComboBoxItemCollection _items;
    /// <summary>
    /// The collection of ComboBox items.
    /// </summary>
    [Category("Data")]
    [Description("The collection of ComboBox items.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    //[Browsable(false)]
    [Editor(typeof(ComboBoxItemCollectionEditor), typeof(UITypeEditor))]
    public ComboBoxItemCollection Items
    {
      get
      {
        if (_items == null)
        {
          _items = new ComboBoxItemCollection();
        }

        return _items;
      }
    }

    /// <summary>
    /// Whether to enable keyboard navigation through items.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to enable keyboard navigation through items.")]
    [DefaultValue(true)]
    public bool KeyboardEnabled
    {
      get
      {
        object o = ViewState["KeyboardEnabled"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["KeyboardEnabled"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to display to indicate that the record-set is reloading.
    /// </summary>
    [Category("Appearance")]
    [Description("The ID of the client template to display to indicate that the record-set is reloading.")]
    [DefaultValue("")]
    public string LoadingClientTemplateId
    {
      get
      {
        object o = ViewState["LoadingClientTemplateId"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["LoadingClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The text to display to indicate that the record-set is reloading.
    /// </summary>
    [Category("Appearance")]
    [Description("The text to display to indicate that the record-set is reloading.")]
    [DefaultValue("")]
    public string LoadingText
    {
      get
      {
        object o = ViewState["LoadingText"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["LoadingText"] = value;
      }
    }

    private StringCollection _preloadImages;
    /// <summary>
    /// A list of images to preload.
    /// </summary>
    [Description("A list of images to preload.")]
    private StringCollection PreloadImages
    {
      get
      {
        if (_preloadImages == null)
        {
          _preloadImages = new StringCollection();
        }

        return _preloadImages;
      }
    }

    /// <summary>
    /// The running mode for this ComboBox.
    /// </summary>
    [Category("Behavior")]
    [Description("The running mode for this ComboBox.")]
    [DefaultValue(ComboBoxRunningMode.Client)]
    public ComboBoxRunningMode RunningMode
    {
      get
      {
        object o = ViewState["RunningMode"];
        return (o == null) ? ComboBoxRunningMode.Client : (ComboBoxRunningMode)o;
      }
      set
      {
        ViewState["RunningMode"] = value;
      }
    }

    /// <summary>
    /// The index of the selected item.
    /// </summary>
    [Category("Data")]
    [Description("The index of the selected item.")]
    [DefaultValue(-1)]
    public int SelectedIndex
    {
      get
      {
        object o = ViewState["SelectedIndex"];
        return (o == null) ? -1 : (int)o;
      }
      set
      {
        object oldIndex = ViewState["SelectedIndex"];

        ViewState["SelectedIndex"] = value;

        // update text
        if (this.SelectedItem != null)
        {
          this.Text = this.SelectedItem.Text;
        }
      }
    }

    /// <summary>
    /// The currently selected item.
    /// </summary>
    [Category("Data")]
    [Description("The currently selected item.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DefaultValue(null)]
    public ComboBoxItem SelectedItem
    {
      get
      {
        int iIndex = this.SelectedIndex;

        if (iIndex >= 0 && iIndex < this.Items.Count)
        {
          return this.Items[iIndex];
        }

        return null;
      }
      set
      {
        if (value != null)
        {
          int iIndex = this.Items.IndexOf(value);

          if (iIndex >= 0)
          {
            this.SelectedIndex = iIndex;
          }
        }
        else
        {
          this.SelectedIndex = -1;
        }
      }
    }

    /// <summary>
    /// The currently selected value.
    /// </summary>
    [Category("Data")]
    [Description("The currently selected value.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DefaultValue(null)]
    public string SelectedValue
    {
      get
      {
        ComboBoxItem oItem = this.SelectedItem;

        if (oItem != null)
        {
          return oItem.Value;
        }

        return null;
      }
      set
      {
        this.SelectedItem = this.Items.FindByValue(value);
      }
    }

    /// <summary>
    /// The CSS class to use for the selected item.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for for the selected item.")]
    [DefaultValue("")]
    public string SelectedItemCssClass
    {
      get
      {
        object o = ViewState["SelectedItemCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["SelectedItemCssClass"] = value;
      }
    }

    /// <summary>
    /// The name of the standard SOA.UI service to connect with.
    /// </summary>
    [Category("Data")]
    [DefaultValue("")]
    [Description("The name of the standard SOA.UI service to connect with.")]
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
    /// The custom tag to attach to SOA.UI requests.
    /// </summary>
    /// <seealso cref="SoaService" />
    [Category("Data")]
    [DefaultValue("")]
    [Description("The custom tag to attach to SOA.UI requests.")]
    public object SoaRequestTag
    {
      get
      {
        object o = ViewState["SoaRequestTag"];
        return o;
      }
      set
      {
        ViewState["SoaRequestTag"] = value;
      }
    }

    /// <summary>
    /// The text value of the input box.
    /// </summary>
    [Category("Data")]
    [Description("The text value of the input box.")]
    [DefaultValue("")]
    public string Text
    {
      get
      {
        object o = ViewState["Text"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["Text"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the text box.
    /// </summary>
    /// <remarks>
    /// Note that, in general, using this feature makes the input text box non-editable. However, the reserved identifier #$InputBox can be used
    /// inside the client template as a placeholder for an operational client-rendered input box.
    /// </remarks>
    [Category("Appearance")]
    [Description("The ID of the client template to use for the text box.")]
    [DefaultValue("")]
    public string TextBoxClientTemplateId
    {
      get
      {
        object o = ViewState["TextBoxClientTemplateId"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["TextBoxClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the text box.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for for the text box.")]
    [DefaultValue("")]
    public string TextBoxCssClass
    {
      get
      {
        object o = ViewState["TextBoxCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["TextBoxCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the text box when it is disabled.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for for the text box when it is disabled.")]
    [DefaultValue("")]
    public string TextBoxDisabledCssClass
    {
      get
      {
        object o = ViewState["TextBoxDisabledCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["TextBoxDisabledCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to enable manual entry into the text box.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to enable manual entry into the text box.")]
    [DefaultValue(true)]
    public bool TextBoxEnabled
    {
      get
      {
        object o = ViewState["TextBoxEnabled"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["TextBoxEnabled"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the text box when it's focused.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for for the text box when it's focused.")]
    [DefaultValue("")]
    public string TextBoxFocusedCssClass
    {
      get
      {
        object o = ViewState["TextBoxFocusedCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["TextBoxFocusedCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the text box on hover.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for for the text box on hover.")]
    [DefaultValue("")]
    public string TextBoxHoverCssClass
    {
      get
      {
        object o = ViewState["TextBoxHoverCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["TextBoxHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to use the client-side page HREF as the prefix URL for callback requests.
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to use the client-side page HREF as the prefix URL for callback requests.")]
    public bool UseClientUrlAsPrefix
    {
      get
      {
        object o = ViewState["UseClientUrlAsPrefix"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["UseClientUrlAsPrefix"] = value;
      }
    }

    #endregion

    #region Public Methods

    public void ApplyTheming(bool? overwriteSettings)
    {
      bool overwrite = overwriteSettings ?? false;
      string prefix = this.AutoThemingCssClassPrefix ?? "";

      // Base
      if ((this.CssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.CssClass = prefix + "combobox";
      }
      if ((this.DisabledCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DisabledCssClass = prefix + "combobox-disabled";
      }
      if ((this.HoverCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.HoverCssClass = prefix + "combobox-hover";
      }
      if ((this.FocusedCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.FocusedCssClass = prefix + "combobox-focused";
      }
      if ((this.TextBoxCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.TextBoxCssClass = prefix + "combobox-textbox";
      }
      if ((this.DropDownCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DropDownCssClass = prefix + "combobox-dropdown";
      }
      if ((this.DropDownContentCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DropDownContentCssClass = prefix + "combobox-dropdown-content";
      }
      if ((this.ItemCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.ItemCssClass = prefix + "item-default";
      }
      if ((this.ItemHoverCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.ItemHoverCssClass = prefix + "item-hover";
      }
      if ((this.SelectedItemCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.SelectedItemCssClass = prefix + "item-selected";
      }
      if ((this.DisabledItemCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DisabledItemCssClass = prefix + "item-disabled";
      }
      if ((this.ItemClientTemplateId ?? string.Empty) == string.Empty || overwrite)
      {
        this.ItemClientTemplateId = "ComboBoxItemTemplate";
      }

      if (ViewState["DropDownResizingMode"] == null || overwrite)
      {
        ViewState["DropDownResizingMode"] = ComboBoxResizingMode.Bottom;
      }
      if (ViewState["DropDownWidth"] == null || overwrite)
      {
        if (this.Width != null)
        {
          ViewState["DropDownWidth"] = Int32.Parse(this.Width.Value.ToString());
        }
        else
        {
          ViewState["DropDownWidth"] = 188;
        }
      }
      if (ViewState["DropDownHeight"] == null || overwrite)
      {
        ViewState["DropDownHeight"] = 300;
      }

      if (_dropDownFooter == null || overwrite)
      {
        if (_dropDownFooter != null)
        {
          this.Controls.Remove(_dropDownFooter);
        }

        _dropDownFooter = new ComboBoxContent();
        Literal L2 = new Literal();
        L2.Text = "<div class=\"" + prefix + "combobox-footer\"></div>";
        _dropDownFooter.Controls.Add(L2);

        this.Controls.Add(_dropDownFooter);
      }

      // Client Templates
      StringBuilder templateText = new StringBuilder();
      templateText.Append("<div class=\"" + prefix + "combobox-item\">");
	    templateText.Append("<a href=\"javascript:void(0);\" onclick=\"this.blur();\">");
      templateText.Append("<span class=\"" + prefix + "outer\">");
      templateText.Append("<span class=\"" + prefix + "inner## DataItem.getProperty('Icon') == null ? '' : ' ' +  DataItem.getProperty('Icon'); ##\">## DataItem.getProperty('Text'); ##</span>");
      templateText.Append("</span></a></div>");
      AddClientTemplate(overwrite, "ComboBoxItemTemplate", templateText.ToString());
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
    /// Bind to the set DataSource.
    /// </summary>
    /// <seealso cref="DataSource" />
    public override void DataBind()
    {
      string sComboBoxVar = this.GetSaneId();


      // Convert the data source control into disconnected data we can bind with
      if (_dataSource == null && DataSourceID != "")
      {
        Control oDS = Utils.FindControl(this, this.DataSourceID);

        if (oDS == null)
        {
          throw new Exception("Data source control '" + this.DataSourceID + "' not found.");
        }

        if (oDS is SqlDataSource)
        {
          SqlDataSource oSqlDS = (SqlDataSource)oDS;

          if (oSqlDS.DataSourceMode != SqlDataSourceMode.DataSet)
          {
            throw new Exception("DataSourceMode must be set to DataSet on the SqlDataSource control.");
          }

          _dataSource = oSqlDS.Select(DataSourceSelectArguments.Empty);
        }
        else if (oDS is ObjectDataSource)
        {
          _dataSource = ((ObjectDataSource)oDS).Select();
        }
        else if (oDS is IListSource)
        {
          IListSource oDSC = (IListSource)oDS;
          _dataSource = oDSC.GetList();
        }
        else
        {
          throw new Exception("Data source control must be a SqlDataSource or ObjectDataSource or must implement IListSource.");
        }
      }


      this.LoadData(0, int.MaxValue);

      ItemCount = this.Items.Count;

      base.DataBind();

      // set pointers
      foreach (ComboBoxItem oItem in this.Items)
      {
        oItem.ParentComboBox = this;
      }

      _dataBound = true;
    }

    /// <summary>
    /// Raise a postback event.
    /// </summary>
    /// <param name="eventArgument">Postback argument</param>
    public void RaisePostBackEvent(string eventArgument)
    {
    }

    #endregion

    #region Private Methods

    private void BuildImageList()
    {
      if (this.DropActiveImageUrl != "") this.PreloadImages.Add(this.DropActiveImageUrl);
      if (this.DropImageUrl != "") this.PreloadImages.Add(this.DropImageUrl);
      if (this.DropHoverImageUrl != "") this.PreloadImages.Add(this.DropHoverImageUrl);
    }

    private string BuildStorage()
    {
      JavaScriptArray arNodeList = new JavaScriptArray();

      foreach (ComboBoxItem oItem in this.Items)
      {
        ProcessItem(oItem, arNodeList);
      }

      return arNodeList.ToString();
    }

    protected override void ComponentArtPreRender(EventArgs oArgs)
    {
      // make sure we're data-bound
      if (!_dataBound && this.Items.Count == 0)
      {
        this.DataBind();
      }

      if (!this.IsDownLevel())
      {
        // Tack this to the bottom of the page, to know when we've loaded.
        this.RegisterStartupScript("ComponentArt_Page_Loaded", this.DemarcateClientScript("window.ComponentArt_Page_Loaded = true;"));
      }
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      // adjust ItemCount if things were added
      if (ItemCount < Items.Count) ItemCount = Items.Count;

      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        RenderAccessibleContent(output);
      }
      else if (this.IsDownLevel())
      {
        RenderDownLevel(output);
        return;
      }

      if (Page != null)
      {
        // Add core code
        if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
        {
          Page.RegisterClientScriptBlock("A573G988.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
        }
        if (!Page.IsClientScriptBlockRegistered("A573P123.js"))
        {
          Page.RegisterClientScriptBlock("A573P123.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.ComboBox.client_scripts", "A573P123.js");
        }
        if (!Page.IsClientScriptBlockRegistered("A573P456.js"))
        {
          Page.RegisterClientScriptBlock("A573P456.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.ComboBox.client_scripts", "A573P456.js");
        }
        if (this.KeyboardEnabled)
        {
          if (!Page.IsClientScriptBlockRegistered("A573Z388.js"))
          {
            Page.RegisterClientScriptBlock("A573Z388.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573Z388.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A573P124.js"))
          {
            Page.RegisterClientScriptBlock("A573P124.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.ComboBox.client_scripts", "A573P124.js");
          }
        }
      }

      if (this.ClientTarget != ClientTargetLevel.Accessible)
      {

        if (this.AutoTheming)
        {
          this.ApplyTheming(false);
        }

      // render preload images
      if (!this.IsDownLevel())
      {
        this.BuildImageList();

        // Preload images, if any
        if (this.PreloadImages.Count > 0)
        {
          this.RenderPreloadImages(output);
        }
      }

      // do we need a client template?
      if (!this.TextBoxEnabled && this.TextBoxClientTemplateId == "")
      {
        string sTextBoxClientTemplateId = Guid.NewGuid().ToString();

        ClientTemplate oTemplate = new ClientTemplate();
        oTemplate.ID = sTextBoxClientTemplateId;
        oTemplate.Text = "<nobr>## DataItem? DataItem.Text : '&nbsp;' ##</nobr>";
        this.ClientTemplates.Add(oTemplate);

        this.TextBoxClientTemplateId = sTextBoxClientTemplateId;
      }

      output = new HtmlTextWriter(output, string.Empty);

      string sComboBoxVarName = this.GetSaneId();

      // make sure Width makes sense
      if (this.Width.IsEmpty)
      {
        this.Width = 150;
      }

      // Save the tabindex?
      int iTabIndex = -1;
      if (this.TabIndex > 0)
      {
        iTabIndex = this.TabIndex;
        this.TabIndex = 0;
      }

      // Render frame

      output.Write("<table"); // begin <table>
      output.WriteAttribute("id", sComboBoxVarName);
      output.WriteAttribute("cellpadding", "0");
      output.WriteAttribute("cellspacing", "0");
      output.WriteAttribute("border", "0");
      output.WriteAttribute("onmouseout", sComboBoxVarName + ".HandleInputMouseOut()");
      output.WriteAttribute("onmouseover", sComboBoxVarName + ".HandleInputMouseOver()");
      
      if (this.CssClass != string.Empty)
      {
        output.WriteAttribute("class", this.CssClass);
      }
      if (!this.Enabled)
      {
        output.WriteAttribute("disabled", "disabled");
      }

      // Output style
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
      if (!this.BackColor.IsEmpty)
      {
        output.WriteStyleAttribute("background-color", System.Drawing.ColorTranslator.ToHtml(this.BackColor));
      }
      if (!this.BorderWidth.IsEmpty)
      {
        output.WriteStyleAttribute("border-width", this.BorderWidth.ToString());
      }
      if (this.BorderStyle != BorderStyle.NotSet)
      {
        output.WriteStyleAttribute("border-style", this.BorderStyle.ToString());
      }
      if (!this.BorderColor.IsEmpty)
      {
        output.WriteStyleAttribute("border-color", System.Drawing.ColorTranslator.ToHtml(this.BorderColor));
      }
      if (!this.IsCallback)
      {
        output.WriteStyleAttribute("visibility", "hidden");
      }
      output.Write("\">"); // end <table>

      output.RenderBeginTag(HtmlTextWriterTag.Tr);

      if (!this.TextBoxEnabled)
      {
        output.AddAttribute("onmousedown", sComboBoxVarName + ".HandleDropClick(event,this)");
        
        // style goes on TD in this case
        if (this.TextBoxCssClass != "")
        {
          output.AddAttribute("class", this.TextBoxCssClass);
        }
      }
      output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
      output.AddAttribute(HtmlTextWriterAttribute.Id, sComboBoxVarName + "_TextBox");
      output.RenderBeginTag(HtmlTextWriterTag.Td);

      // Render input field.
      if (this.TextBoxEnabled)
      {
        output.AddAttribute(HtmlTextWriterAttribute.Id, sComboBoxVarName + "_Input");
        output.AddAttribute(HtmlTextWriterAttribute.Name, sComboBoxVarName + "_Input");
        output.AddAttribute("type", "text");

        if (this.Text != "")
        {
          output.AddAttribute("value", this.Text);
        }
        else if (this.SelectedItem != null)
        {
          output.AddAttribute("value", this.SelectedItem.Text);
        }
        if (this.TextBoxCssClass != "")
        {
          output.AddAttribute("class", this.TextBoxCssClass);
        }

        if (iTabIndex >= 0)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Tabindex, iTabIndex.ToString());
        }

        // Prevent FF from showing input text suggestions
        output.AddAttribute("autocomplete", "off");
 
        output.AddStyleAttribute("display", "none");
        output.AddAttribute("onfocus", sComboBoxVarName + ".HandleFocus()");
        output.AddAttribute("onblur", sComboBoxVarName + ".HandleBlur(event)");
        output.AddAttribute("onkeydown", sComboBoxVarName + ".HandleKeyPress(event,this)");
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();
      }

      output.RenderEndTag(); // </td>

      if (this.DropImageUrl != "")
      {
        output.RenderBeginTag(HtmlTextWriterTag.Td);
        output.AddStyleAttribute("display", "block");
        output.AddAttribute("onmouseup", sComboBoxVarName + ".HandleDropMouseUp(event,this)");
        output.AddAttribute("onmousedown", sComboBoxVarName + ".HandleDropClick(event,this)");
        if (DropImageWidth > 0)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Width, DropImageWidth.ToString());
        }
        if (DropImageHeight > 0)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Height, DropImageHeight.ToString());
        }
        output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertUrl(Context, "", this.DropImageUrl));
        output.AddAttribute(HtmlTextWriterAttribute.Alt, "");
        output.AddAttribute(HtmlTextWriterAttribute.Id, sComboBoxVarName + "_DropImage");
        output.RenderBeginTag(HtmlTextWriterTag.Img);
        output.RenderEndTag(); // </img>

        output.RenderEndTag(); // </td>
      }
      else if (this.AutoTheming)
      {
        output.RenderBeginTag(HtmlTextWriterTag.Td);
        output.AddAttribute("class", AutoThemingCssClassPrefix + "combobox-drop");
        output.AddAttribute(HtmlTextWriterAttribute.Id, sComboBoxVarName + "_DropDiv");
        output.AddAttribute("onmouseup", sComboBoxVarName + ".HandleDropMouseUp(event,this)");
        output.AddAttribute("onmousedown", sComboBoxVarName + ".HandleDropClick(event,this)");
        output.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
        output.RenderEndTag(); // </div>
        output.RenderEndTag(); // </td>
      }
      
      output.RenderEndTag(); // </tr>
      
      output.Write("</table>"); // </table>

      // render focus-holding element for templated combobox
      if (!this.TextBoxEnabled)
      {
        if (iTabIndex >= 0)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Tabindex, iTabIndex.ToString());
        }
        output.AddAttribute("href", "javascript:void(0)");
        output.AddAttribute("onfocus", sComboBoxVarName + ".HandleFocus()");
        output.AddAttribute("onblur", sComboBoxVarName + ".HandleBlur(event)");
        output.AddAttribute(HtmlTextWriterAttribute.Id, sComboBoxVarName + "_Input");
        /*
        output.AddStyleAttribute("position", "absolute");
        output.AddStyleAttribute("left", "-2000px");
        output.AddStyleAttribute("top", "-2000px");
        */
        output.AddStyleAttribute("z-index", "99");

        output.RenderBeginTag(HtmlTextWriterTag.A);
        output.RenderEndTag();
      }

      // begin dropdown

      if (this.DropDownResizingMode != ComboBoxResizingMode.Off)
      {
        output.AddAttribute("onmousemove", sComboBoxVarName + ".HandleMouseMove(event,this)");
        output.AddAttribute("onmouseout", sComboBoxVarName + ".HandleMouseOut(event,this)");
        output.AddAttribute("onmousedown", sComboBoxVarName + ".HandleMouseDown(event,this)");
      }
      if (this.DropDownCssClass != "")
      {
        output.AddAttribute(HtmlTextWriterAttribute.Class, this.DropDownCssClass);
      }
      output.AddStyleAttribute("display", "none");
      output.AddAttribute("id", sComboBoxVarName + "_DropDown");
      output.RenderBeginTag(HtmlTextWriterTag.Div);

      output.AddAttribute("cellpadding", "0");
      output.AddAttribute("cellspacing", "0");
      output.AddAttribute("border", "0");
      output.AddAttribute("width", "100%");
      output.RenderBeginTag(HtmlTextWriterTag.Table);

      if (DropDownHeader != null)
      {
        output.RenderBeginTag(HtmlTextWriterTag.Tr);
        if (CacheMapEnabled)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
        }
        output.RenderBeginTag(HtmlTextWriterTag.Td);
        DropDownHeader.RenderControl(output);
        output.RenderEndTag(); // </td>
        output.RenderEndTag(); // </tr>
      }

      // render dropdown
      output.RenderBeginTag(HtmlTextWriterTag.Tr);
      if (this.DropDownContentCssClass != "")
      {
        output.AddAttribute(HtmlTextWriterAttribute.Class, this.DropDownContentCssClass);
      }
      output.RenderBeginTag(HtmlTextWriterTag.Td);
      output.AddAttribute(HtmlTextWriterAttribute.Id, sComboBoxVarName + "_DropDownContent");
      //output.AddAttribute("onscroll", sComboBoxVarName + ".HandleScroll(event,this)");
      output.AddAttribute("onmousedown", "ComponentArt_CancelEvent(event)");
      output.AddAttribute("onmouseup", sComboBoxVarName + ".HandleMouseUp(event,this)");
      output.RenderBeginTag(HtmlTextWriterTag.Div);
      if (DropDownContent != null)
      {
        DropDownContent.RenderControl(output);
      }
      output.RenderEndTag(); // </div>
      output.RenderEndTag(); // </td>

      if (CacheMapEnabled)
      {
        output.AddAttribute(HtmlTextWriterAttribute.Width, this.CacheMapWidth.ToString());
        output.AddAttribute(HtmlTextWriterAttribute.Id, sComboBoxVarName + "_CacheMap");
        output.RenderBeginTag(HtmlTextWriterTag.Td);

        output.RenderEndTag(); // </td>
      }
      output.RenderEndTag(); // </tr>

      if (DropDownFooter != null)
      {
        output.RenderBeginTag(HtmlTextWriterTag.Tr);
        if (CacheMapEnabled)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
        }
        output.RenderBeginTag(HtmlTextWriterTag.Td);
        DropDownFooter.RenderControl(output);
        output.RenderEndTag(); // </td>
        output.RenderEndTag(); // </tr>
      }

      output.RenderEndTag(); // </table>

      output.RenderEndTag(); // </div>


      // render hidden fields
      output.AddAttribute("id", sComboBoxVarName + "_SelectedIndex");
      output.AddAttribute("name", sComboBoxVarName + "_SelectedIndex");
      output.AddAttribute("type", "hidden");
      output.AddAttribute("value", this.SelectedIndex.ToString());
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      output.AddAttribute("id", sComboBoxVarName + "_Data");
      output.AddAttribute("name", sComboBoxVarName + "_Data");
      output.AddAttribute("type", "hidden");
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      // End content render

      // Render client-side object initiation.
      StringBuilder oStartupSB = new StringBuilder();
      oStartupSB.Append("/*** ComponentArt.Web.UI.ComboBox ").Append(this.VersionString()).Append(" ").Append(sComboBoxVarName).Append(" ***/\n");
      oStartupSB.Append("function ComponentArt_Init_" + sComboBoxVarName + "() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      string sDependencies = "window.ComponentArt_Page_Loaded && window.ComponentArt_ComboBox_Kernel_Loaded && window.ComponentArt_ComboBox_Support_Loaded && window.ComponentArt_Utils_Loaded && document.getElementById('" + sComboBoxVarName + "')";
      if (this.KeyboardEnabled)
      {
        sDependencies += " && window.ComponentArt_ComboBox_Keyboard_Loaded && window.ComponentArt_Keyboard_Loaded";
      }
      oStartupSB.Append("if(!(" + sDependencies + "))\n");
      oStartupSB.Append("\t{setTimeout('ComponentArt_Init_" + sComboBoxVarName + "()', 100); return; }\n\n");

      // Instantiate object
      oStartupSB.Append("window." + sComboBoxVarName + " = new ComponentArt_ComboBox('" + sComboBoxVarName + "');\n");

      // Write data
      oStartupSB.Append(sComboBoxVarName + ".Data = ");
      oStartupSB.Append(this.BuildStorage());
      oStartupSB.Append(";\n");

      // Write postback function reference
      if (Page != null)
      {
        oStartupSB.Append(sComboBoxVarName + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
      }

      // Hook the actual ID if available and different from effective client ID
      if (this.ID != sComboBoxVarName)
      {
        oStartupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window['" + sComboBoxVarName + "']; " + sComboBoxVarName + ".GlobalAlias = '" + ID + "'; }\n");
      }
      
      // Properties
      // Write properties
      oStartupSB.Append("var properties = [\n");
      if (AdjustPositioning) oStartupSB.Append("['AdjustPositioning',1],");
      if (AutoComplete) oStartupSB.Append("['AutoComplete',1],");
      if (AutoFilter) oStartupSB.Append("['AutoFilter',1],");
      if (AutoHighlight) oStartupSB.Append("['AutoHighlight',1],");
      if (AutoPostBack) oStartupSB.Append("['AutoPostBack',1],");
      if (AutoTheming)
      {
        oStartupSB.Append("['AutoTheming',1],");
        oStartupSB.Append("['AutoThemingCssClassPrefix','" + AutoThemingCssClassPrefix + "'],");
      }
      if (this.CacheMapEnabled)
      {
        oStartupSB.Append("['CacheMapLoadedColor','" + CacheMapLoadedColor + "'],");
        oStartupSB.Append("['CacheMapLoadingColor','" + CacheMapLoadingColor + "'],");
        oStartupSB.Append("['CacheMapNotLoadedColor','" + CacheMapNotLoadedColor + "'],");
        oStartupSB.Append("['CacheMapWidth'," + CacheMapWidth + "],");
      }
      oStartupSB.Append("['CacheSize'," + CacheSize + "],");
      oStartupSB.Append("['CallbackFilterDelay'," + CallbackFilterDelay + "],");
      oStartupSB.Append("['CallbackFilterMinimumLength'," + CallbackFilterMinimumLength + "],");
      oStartupSB.Append("['CallbackPrefix',unescape('" + (Utils.GetResponseUrl(Context) + (Context.Request.QueryString.Count > 0 ? "&" : "?") + "Cart_" + sComboBoxVarName + "_Callback=yes").Replace("'", "\\'") + "')],");          
      oStartupSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "],");
      if (ClientInitCondition != "") oStartupSB.Append("['ClientInitCondition','" + this.ClientInitCondition.Replace("'", "\\'") + "'],");
      if (ClientRenderCondition != "") oStartupSB.Append("['ClientRenderCondition','" + this.ClientRenderCondition.Replace("'", "\\'") + "'],");
      oStartupSB.Append("['ClientTemplates'," + this._clientTemplates.ToString() + "],");
      oStartupSB.Append("['ControlId','" + this.UniqueID + "'],");
      oStartupSB.Append("['CollapseSlide'," + ((int)this.CollapseSlide).ToString() + "],");
      oStartupSB.Append("['CollapseDuration'," + this.CollapseDuration.ToString() + "],");
      if (CssClass != "") oStartupSB.Append("['CssClass','" + this.CssClass + "'],");
      if (DataTextField != "") oStartupSB.Append("['DataTextField','" + DataTextField + "'],");
      if (Debug) oStartupSB.Append("['Debug',1],");
      if (DisabledCssClass != "") oStartupSB.Append("['DisabledCssClass','" + this.DisabledCssClass + "'],");
      if (DisabledItemCssClass != "") oStartupSB.Append("['DisabledItemCssClass','" + this.DisabledItemCssClass + "'],");
      oStartupSB.Append("['DropDownHeight'," + DropDownHeight + "],");
      oStartupSB.Append("['DropDownOffsetX'," + DropDownOffsetX + "],");
      oStartupSB.Append("['DropDownOffsetY'," + DropDownOffsetY + "],");
      oStartupSB.Append("['DropDownPageSize'," + DropDownPageSize + "],");
      oStartupSB.Append("['DropDownResizingMode','" + DropDownResizingMode.ToString() + "'],");
      oStartupSB.Append("['DropDownResizingStyle','" + DropDownResizingStyle.ToString() + "'],");
      oStartupSB.Append("['DropDownWidth'," + DropDownWidth + "],");
      if (DropActiveImageUrl != "") oStartupSB.Append("['DropActiveImageUrl','" + Utils.ConvertUrl(Context, "", DropActiveImageUrl) + "'],");
      if (DropImageUrl != "") oStartupSB.Append("['DropImageUrl','" + Utils.ConvertUrl(Context, "", DropImageUrl) + "'],");
      if (DropHoverImageUrl != "") oStartupSB.Append("['DropHoverImageUrl','" + Utils.ConvertUrl(Context, "", DropHoverImageUrl) + "'],");
      if (Enabled) oStartupSB.Append("['Enabled',1],");
      if (EnableViewState) oStartupSB.Append("['EnableViewState',1],");
      oStartupSB.Append("['ExpandDirection'," + ((int)this.ExpandDirection).ToString() + "],");
      oStartupSB.Append("['ExpandDuration'," + this.ExpandDuration.ToString() + "],");
      oStartupSB.Append("['ExpandSlide'," + ((int)this.ExpandSlide).ToString() + "],");
      if (DropDownContent != null) oStartupSB.Append("['HasDropDownContent',1],");
      oStartupSB.Append("['FilterCacheSize'," + FilterCacheSize + "],");
      if (FilterField != "") oStartupSB.Append("['FilterField','" + this.FilterField + "'],");
      if (FocusedCssClass != "") oStartupSB.Append("['FocusedCssClass','" + this.FocusedCssClass + "'],");
      if (HoverCssClass != "") oStartupSB.Append("['HoverCssClass','" + this.HoverCssClass + "'],");
      if (ItemClientTemplateId != "") oStartupSB.Append("['ItemClientTemplateId','" + ItemClientTemplateId + "'],");
      oStartupSB.Append("['ItemCount'," + this.ItemCount + "],");
      if (ItemCssClass != "") oStartupSB.Append("['ItemCssClass','" + ItemCssClass + "'],");
      if (ItemHoverCssClass != "") oStartupSB.Append("['ItemHoverCssClass','" + ItemHoverCssClass + "'],");
      if (LoadingClientTemplateId != "") oStartupSB.Append("['LoadingClientTemplateId','" + LoadingClientTemplateId + "'],");
      oStartupSB.Append("['LoadingText','" + (LoadingText != "" ? LoadingText : "Loading...") + "'],");
      if (KeyboardEnabled) oStartupSB.Append("['KeyboardEnabled',1],");
      oStartupSB.Append("['RunningMode'," + (int)this.RunningMode + "],");
      oStartupSB.Append("['SelectedIndex'," + SelectedIndex + "],");
      if (SelectedItemCssClass != "") oStartupSB.Append("['SelectedItemCssClass','" + SelectedItemCssClass + "'],");
      if (SoaService != "") oStartupSB.Append("['SoaService','" + SoaService + "'],");
      if (SoaRequestTag != null) oStartupSB.Append("['SoaRequestTag','" + SoaRequestTag + "'],");
      if (TextBoxClientTemplateId != "") oStartupSB.Append("['TextBoxClientTemplateId','" + TextBoxClientTemplateId + "'],");
      if (TextBoxCssClass != "") oStartupSB.Append("['TextBoxCssClass','" + TextBoxCssClass + "'],");
      if (TextBoxDisabledCssClass != "") oStartupSB.Append("['TextBoxDisabledCssClass','" + TextBoxDisabledCssClass + "'],");
      if (TextBoxEnabled) oStartupSB.Append("['TextBoxEnabled',1],");
      if (TextBoxFocusedCssClass != "") oStartupSB.Append("['TextBoxFocusedCssClass','" + TextBoxFocusedCssClass + "'],");
      if (TextBoxHoverCssClass != "") oStartupSB.Append("['TextBoxHoverCssClass','" + TextBoxHoverCssClass + "'],");
      if (UseClientUrlAsPrefix) oStartupSB.Append("['UseClientUrlAsPrefix',1],");
      if (!Width.IsEmpty && Width.Type == UnitType.Pixel) oStartupSB.Append("['Width'," + Width.Value + "],");

      // End properties
      oStartupSB.Append("];\n");

      // Set properties
      oStartupSB.AppendFormat("ComponentArt_SetProperties({0}, properties);\n", sComboBoxVarName);

      if (this.KeyboardEnabled)
      {
        oStartupSB.Append(sComboBoxVarName + ".InitKeyboard();\n\n");
      }

      if (ClientInitCondition != "")
      {
        oStartupSB.Append("ComponentArt_WaitOnCondition(" + sComboBoxVarName + ".ClientInitCondition,'" + sComboBoxVarName + ".Initialize()');\n}\n");
      }
      else
      {
        oStartupSB.Append(sComboBoxVarName + ".Initialize();\n}\n");
      }

      // Initiate ComboBox creation
      oStartupSB.Append("ComponentArt_Init_" + sComboBoxVarName + "();");

      WriteStartupScript(output, this.DemarcateClientScript(oStartupSB.ToString()));

      }
    }

    private string GenerateXmlData()
    {
      StringBuilder oSB = new StringBuilder();

      oSB.Append("<Data><![CDATA[");
      oSB.Append(BuildStorage());
      oSB.Append("]]></Data>");

      return oSB.ToString();
    }

    private string GenerateXmlParams()
    {
      StringBuilder oSB = new StringBuilder();

      oSB.Append("<Params>");

      //oSB.AppendFormat("<DropDownPageSize>{0}</DropDownPageSize>", this.DropDownPageSize);
      oSB.AppendFormat("<ItemCount>{0}</ItemCount>", this.ItemCount);
      
      oSB.Append("</Params>");

      return oSB.ToString();
    }

    private void HandleCallback()
    {
      string sVarPrefix = "Cart_" + this.GetSaneId();

      // get callback level
      string sStartIndex = Context.Request.Params[sVarPrefix + "_Callback_StartIndex"];
      string sNumItems = Context.Request.Params[sVarPrefix + "_Callback_NumItems"];

      int iStartIndex = sStartIndex != null ? int.Parse(sStartIndex) : 0;
      int iNumItems = sNumItems != null ? int.Parse(sNumItems) : 0;

      string sFilterString = Context.Request.Params[sVarPrefix + "_Callback_Filter"];
      if (sFilterString == null) sFilterString = "";
      
      // load appropriate data
      if (this.DataRequested != null)
      {
        ComboBoxDataRequestedEventArgs args = new ComboBoxDataRequestedEventArgs();
        
        args.StartIndex = iStartIndex;
        args.NumItems = iNumItems;

        args.Filter = sFilterString;

        this.OnDataRequested(args);
      }
      else
      {
        this.LoadData(iStartIndex, iStartIndex + iNumItems);
      }
      
      Context.Response.Clear();
      Context.Response.ContentType = "text/xml";
      Context.Response.Write("<ComboBoxResponse>");
      //Context.Response.Write(this.GenerateXmlTemplates(arItems, iLevel));
      Context.Response.Write(this.GenerateXmlData());
      Context.Response.Write(this.GenerateXmlParams());
      Context.Response.Write("</ComboBoxResponse>");
      Context.Response.End();
    }

    protected override bool IsDownLevel()
    {
      if (this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }

      if(Context == null || Page == null)
      {
        return true;
      }

      string sUserAgent = Context.Request.UserAgent;

      if (sUserAgent == null || sUserAgent == "") return true;

      int iMajorVersion = 0;

      try
      {
        iMajorVersion = Context.Request.Browser.MajorVersion;
      }
      catch { }

      if (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion < 8)
      {
        return true;
      }
      else if ( // We are good if:

        // 1. We have IE 5 or greater on a non-Mac
        (sUserAgent.IndexOf("MSIE") >= 0 && iMajorVersion >= 5 && !Context.Request.Browser.Platform.ToUpper().StartsWith("MAC")) ||

        // 2. We have Gecko-based browser (Mozilla, Firefox, Netscape 6+)
        (sUserAgent.IndexOf("Gecko") >= 0) ||

        // 3. We have Opera 8 or later
        (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion >= 8)
        )
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    private void LoadClientData(string sData)
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
            this.LoadClientXmlNodes(oRootNode.ChildNodes);
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error loading client data: " + ex);
      }
    }

    private void LoadClientXmlNode(XmlNodeList arProperties)
    {
      ComboBoxItem oItem = new ComboBoxItem();

      foreach (XmlNode oProperty in arProperties)
      {
        string sProperty = oProperty.FirstChild.FirstChild.InnerXml;
        string sValue = oProperty.FirstChild.FirstChild == oProperty.FirstChild.LastChild?
          null : oProperty.FirstChild.LastChild.InnerXml;

        if (sValue != null)
        {
          sValue = sValue.Replace("#$cAmp@*", "&");
          sValue = sValue.Replace("#%cLt#%", "<");
        }

        oItem.Properties[sProperty] = sValue;
      }

      this.Items.Add(oItem);
    }

    private void LoadClientXmlNodes(XmlNodeList arNodes)
    {
      foreach (XmlNode oNode in arNodes)
      {
        if (oNode.FirstChild != null && oNode.FirstChild != null)
        {
          this.LoadClientXmlNode(oNode.FirstChild.ChildNodes);
        }
      }
    }

    private void LoadData(int iStartIndex, int iEndIndex)
    {
      DataView _dataView = null;

      // Load the data
      if (_dataSource != null)
      {
        if (_dataSource is DataView || _dataSource is DataSet || _dataSource is DataTable)
        {
          if (_dataSource is DataView)
          {
            _dataView = (DataView)_dataSource;
          }
          else if (_dataSource is DataSet)
          {
            if (this.DataMember != string.Empty)
            {
              _dataView = ((DataSet)_dataSource).Tables[this.DataMember].DefaultView;
            }
            else
            {
              _dataView = ((DataSet)_dataSource).Tables[0].DefaultView;
            }
          }
          else // DataTable
          {
            _dataView = ((DataTable)_dataSource).DefaultView;
          }

          this.LoadDataFromDataView(_dataView, iStartIndex, iEndIndex);
        }
        else if (_dataSource is IEnumerable)
        {
          this.LoadDataFromEnumerable((IEnumerable)_dataSource, iStartIndex, iEndIndex);
        }
        else
        {
          throw new Exception("Cannot bind to data source of type " + _dataSource.GetType().ToString());
        }
      }
    }

    private void LoadDataFromDataView(DataView oDataView, int iStartIndex, int iEndIndex)
    {
      this.Items.Clear();

      for (int i = iStartIndex; i < Math.Min(iEndIndex, oDataView.Count); i++)
      {
        ComboBoxItem oItem = new ComboBoxItem();

        if (DataTextField != "")
        {
          if (DataTextFormatString != "")
          {
            oItem.Text = string.Format(this.DataTextFormatString, oDataView[i][DataTextField]);
          }
          else
          {
            oItem.Text = oDataView[i][DataTextField].ToString();
          }
        }
        if (DataValueField != "")
        {
          oItem.Value = oDataView[i][DataValueField].ToString();
        }
        if (DataFields != "")
        {
          string[] arFields = DataFields.Split(',');

          foreach (string sField in arFields)
          {
            if (sField != "")
            {
              oItem.Properties[sField] = oDataView[i][sField].ToString();
            }
          }
        }

        Items.Add(oItem);
      }
    }

    private void LoadDataFromEnumerable(IEnumerable oEnumerable, int iStartIndex, int iEndIndex)
    {
      IEnumerator oEnumerator = oEnumerable.GetEnumerator();
      
      this.Items.Clear();
      
      // go to the beginning
      oEnumerator.Reset();
      oEnumerator.MoveNext();

      int iCount = 0;

      // skip some items?
      while (iCount < iStartIndex)
      {
        oEnumerator.MoveNext();
        iCount++;
      }
      
      // Are we good?
      try
      {
        if (oEnumerator.Current == null)
        {
          return;
        }
      }
      catch
      {
        return;
      }

      while (oEnumerator.Current != null && iCount < iEndIndex)
      {
        object oCurrentObject = oEnumerator.Current;

        ComboBoxItem oItem = new ComboBoxItem();

        if (oCurrentObject is string)
        {
          oItem.Text = (string)oCurrentObject;
        }
        else
        {
          // Load item
          if (DataTextField != "")
          {
            LoadValueFromObject(oItem, oCurrentObject, DataTextField, "Text");
          }
          if (DataValueField != "")
          {
            LoadValueFromObject(oItem, oCurrentObject, DataValueField, "Value");
          }
          if (DataFields != "")
          {
            string[] arFields = DataFields.Split(',');

            foreach (string sField in arFields)
            {
              if (sField != "")
              {
                LoadValueFromObject(oItem, oCurrentObject, sField, sField);
              }
            }
          }
        }

        this.Items.Add(oItem);

        if (!oEnumerator.MoveNext())
        {
          break;
        }

        iCount++;
      }
    }

    private void LoadValueFromObject(ComboBoxItem oItem, object oObject, string sField, string sProperty)
    {
      if (sField.IndexOf(".") > 0)
      {
        string[] arProperties = sField.Split('.');
        object o = oObject;

        for (int prop = 0; prop < arProperties.Length; prop++)
        {
          PropertyInfo oProperty = o.GetType().GetProperty(arProperties[prop]);
          if (oProperty != null)
          {
            o = oProperty.GetValue(o, null);
          }
        }

        oItem.Properties[sProperty] = (o != null)? o.ToString() : o;
      }
      else
      {
        PropertyInfo oProperty = oObject.GetType().GetProperty(sField);
        if (oProperty != null)
        {
          object o = oProperty.GetValue(oObject, null);

          oItem.Properties[sProperty] = (o != null) ? o.ToString() : o;
        }
      }
    }

    protected override void LoadViewState(object savedState)
    {
      base.LoadViewState(savedState);

      string sComboBoxId = this.GetSaneId();

      // Load text
      string sInputText = Context.Request.Form[sComboBoxId + "_Input"];
      if (sInputText != null)
      {
        this.Text = sInputText;
      }
      
      // Load client storage data
      string sViewStateData = Context.Request.Form[sComboBoxId + "_Data"];
      if (sViewStateData != null)
      {
        this.LoadClientData(sViewStateData);
      }

      // Load index
      string sSelectedIndex = Context.Request.Form[sComboBoxId + "_SelectedIndex"];
      if (sSelectedIndex != null)
      {
        int iNewIndex = int.Parse(sSelectedIndex);
        if (iNewIndex != this.SelectedIndex)
        {
          // bypass event triggering code by going straight to viewstate
          ViewState["SelectedIndex"] = iNewIndex;

          _raiseChangeEvent = true;
        }
      }

      _dataBound = true;
    }

    protected override void OnLoad(EventArgs e)
    {
      // set pointers
      foreach (ComboBoxItem oItem in this.Items)
      {
        oItem.ParentComboBox = this;
      }

      base.OnLoad(e);

      if (SelectedIndexChanged != null && _raiseChangeEvent)
      {
        this.OnSelectedIndexChanged(EventArgs.Empty);
      }

      // Is this a callback? Handle it now
      if (this.CausedCallback)
      {
        this.HandleCallback();
        return;
      }

    }

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);


      // Convert the data source control into disconnected data we can bind with
      if (_dataSource == null && DataSourceID != "")
      {
        Control oDS = Utils.FindControl(this, this.DataSourceID);

        if (oDS == null)
        {
          throw new Exception("Data source control '" + this.DataSourceID + "' not found.");
        }

        if (oDS is SqlDataSource)
        {
          SqlDataSource oSqlDS = (SqlDataSource)oDS;

          if (oSqlDS.DataSourceMode != SqlDataSourceMode.DataSet)
          {
            throw new Exception("DataSourceMode must be set to DataSet on the SqlDataSource control.");
          }

          _dataSource = oSqlDS.Select(DataSourceSelectArguments.Empty);
        }
        else if (oDS is ObjectDataSource)
        {
          _dataSource = ((ObjectDataSource)oDS).Select();
        }
        else if (oDS is IListSource)
        {
          IListSource oDSC = (IListSource)oDS;
          _dataSource = oDSC.GetList();
        }
        else
        {
          throw new Exception("Data source control must be a SqlDataSource or ObjectDataSource or must implement IListSource.");
        }
      }

      // set pointers
      foreach (ComboBoxItem oItem in this.Items)
      {
        oItem.ParentComboBox = this;
      }
    }

    private void ProcessItem(ComboBoxItem oItem, ArrayList arNodeList)
    {
      ArrayList itemProperties = new ArrayList();
      foreach (string propertyName in oItem.Properties.Keys)
      {
        switch (propertyName.ToLower())
        {
          // bools
          case "enabled": itemProperties.Add(new object[] { "Enabled", oItem.Enabled }); break;

          // normal string handling
          default:
            itemProperties.Add(new object[] { propertyName, oItem.Properties[propertyName] });
            break;
        }
      }
      arNodeList.Add(itemProperties);
    }

    internal void RenderAccessibleContent(HtmlTextWriter output)
    {
      output.Write("<select");

      output.Write(" id=\"");
      output.Write(this.GetSaneId());
      output.Write("\"");

      output.Write(">");

      foreach (ComboBoxItem item in this.Items)
      {
        output.Write("<option");
        if (item.Value != String.Empty)
        {
          output.Write(" value=\"");
          output.Write(item.Value);
          output.Write("\"");
        }
        output.Write(">");

        output.Write(item.Text);

        output.Write("</option>");
      }

      output.Write("</select>");
    }

    internal void RenderDownLevel(HtmlTextWriter output)
    {
      AddAttributesToRender(output);

      output.RenderBeginTag(HtmlTextWriterTag.Select);

      foreach (ComboBoxItem oItem in this.Items)
      {
        if (oItem.Value != "")
        {
          output.AddAttribute(HtmlTextWriterAttribute.Value, oItem.Value);
        }
        output.RenderBeginTag(HtmlTextWriterTag.Option);
        output.Write(oItem.Text);
        output.RenderEndTag();
      }

      output.RenderEndTag(); // </select>
    }

    private void RenderPreloadImages(HtmlTextWriter output)
    {
      output.Write("<div style=\"position:absolute;top:0px;left:0px;visibility:hidden;\">");
      foreach (string sImage in this.PreloadImages)
      {
        output.Write("<img src=\"" + Utils.ConvertUrl(Context, "", sImage) + "\" width=\"0\" height=\"0\" alt=\"\" />\n");
      }
      output.Write("</div>");
    }

    protected override object SaveViewState()
    {
      if (EnableViewState)
      {
        ViewState["EnableViewState"] = true;
      }

      return base.SaveViewState();
    }

    #endregion

    #region Events

    /// <summary>
    /// Delegate for <see cref="DataRequested"/> event of <see cref="ComboBox"/> class.
    /// </summary>
    public delegate void DataRequestedEventHandler(object sender, ComboBoxDataRequestedEventArgs e);

    /// <summary>
    /// Fires when data is requested via callback.
    /// </summary>
    [Description("Fires when data is requested via callback.")]
    [Category("ComboBox Events")]
    public event DataRequestedEventHandler DataRequested;

    private void OnDataRequested(ComboBoxDataRequestedEventArgs e)
    {
      if (DataRequested != null)
      {
        DataRequested(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="SelectedIndexChanged"/> event of <see cref="ComboBox"/> class.
    /// </summary>
    public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Fires when the selected index is changed.
    /// </summary>
    [Description("Fires when the selected index is changed.")]
    [Category("ComboBox Events")]
    public event SelectedIndexChangedEventHandler SelectedIndexChanged;

    private void OnSelectedIndexChanged(EventArgs e)
    {
      if (SelectedIndexChanged != null)
      {
        SelectedIndexChanged(this, e);
      }
    }

    #endregion
  }
}

/// <summary>
/// Specifies the direction of the dropdown expansion.
/// </summary>
public enum ComboBoxExpandDirection
{
  /// <summary>
  /// Dropdown expands below the main area.
  /// </summary>
  Down,

  /// <summary>
  /// Dropdown expands above the main area.
  /// </summary>
  Up
}

/// <summary>
/// Specifies how the dynamic resizing of the dropdown should behave.
/// </summary>
public enum ComboBoxResizingMode
{
  /// <summary>
  /// Resizing is disabled.
  /// </summary>
  Off,

  /// <summary>
  /// Resizing is done by the dragging the lower-right corner of the dropdown.
  /// </summary>
  Corner,

  /// <summary>
  /// Resizing is done by dragging the bottom of the dropdown vertically.
  /// </summary>
  Bottom
}

/// <summary>
/// Specifies the style of the visual feedback for resizing the dropdown.
/// </summary>
public enum ComboBoxResizingStyle
{
  /// <summary>
  /// The dropdown is resized as the user drags.
  /// </summary>
  Live,

  /// <summary>
  /// A rectangular outline indicates the dimensions of the dropdown as it is resized.
  /// </summary>
  Outline
}

/// <summary>
/// Specifies the running mode of the <see cref="ComboBox"/> control.
/// </summary>
/// <remarks>
/// The running mode determines whether all the data is loaded on the client or whether
/// subsequent callback requests should be made to load data as it is needed and to perform
/// other actions, like filtering.
/// </remarks>
public enum ComboBoxRunningMode
{
  /// <summary>
  /// Client running mode: all actions are performed on the client
  /// </summary>
  Client,

  /// <summary>
  /// Callback running mode: data is fetched via callbacks as required
  /// </summary>
  CallBack
}
