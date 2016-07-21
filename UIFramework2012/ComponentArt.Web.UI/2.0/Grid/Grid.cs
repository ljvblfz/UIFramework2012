using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices; 
using ComponentArt.Licensing.Providers;


namespace ComponentArt.Web.UI
{
  #region Templates

  /// <summary>
  /// Template class used for specifying customized rendering for cells in a <see cref="Grid"/> control.
  /// </summary>
  [
  ToolboxItem(false),
  DefaultProperty("Template"),
  ParseChildren(true),
  PersistChildren(false)
  ]
  public class GridServerTemplate : System.Web.UI.WebControls.WebControl
  {
    private ITemplate m_oTemplate;

    /// <summary>
    /// The ITemplate to be used in this GridServerTemplate.
    /// </summary>
    [
    Browsable(false),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
    PersistenceMode(PersistenceMode.InnerProperty),
    TemplateContainer(typeof(ComponentArt.Web.UI.GridServerTemplateContainer)),
    NotifyParentProperty(true)
    ]
    public virtual ITemplate Template
    {
      get
      {
        return m_oTemplate;
      }
      set
      {
        m_oTemplate = value;
      }
    }
  }

  /// <summary>
  /// Collection of <see cref="GridServerTemplate"/> objects.
  /// </summary>
  public class GridServerTemplateCollection : CollectionBase
  {
    public new GridServerTemplate this[int index]
    {
      get
      {
        return (GridServerTemplate)base.List[index];
      }
      set
      {
        base.List[index] = value;
      }
    }

    public new int Add(GridServerTemplate template)
    {
      return this.List.Add(template);
    }
  }

  /// <summary>
  /// Naming container for a customized <see cref="GridItem"/> instance.
  /// </summary>
  [ToolboxItem(false)]
  public class GridServerTemplateContainer : Control, INamingContainer
  {
    private GridItem _dataItem;

    public GridServerTemplateContainer(GridItem parent)
    {
      _dataItem = parent;
    }

    public virtual GridItem DataItem
    {
      get
      {
        return _dataItem;
      }
    }
  }

  #endregion

  #region Server Groups

  internal class ServerGroup
  {
    public ArrayList SubGroup;
    
    public int Index;
    public object Value;
    internal int RenderCount = 0;
    public string Path;
    public ServerGroup ParentGroup;

    public ServerGroup()
    {
      SubGroup = new ArrayList();
    }

    public ServerGroup(object value)
    {
      Value = value;
      SubGroup = new ArrayList();
    }
  }

  #endregion

  /// <summary>
  /// This class is an alias for Grid.
  /// </summary>
  /// <seealso cref="Grid" />
  [ToolboxData("<{0}:DataGrid runat=\"server\" Width=\"500\"></{0}:DataGrid>")]
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  public class DataGrid : Grid
  {
  }
  
  /// <summary>
  /// Displays data in a manipulable table which enables operations like paging, selecting, sorting, and editing of its items.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The ComponentArt Grid control provides a visual representation of an underlying data source on a web page.
  /// </para>
  /// <para>
  /// Grid has the ability to function in various "running modes". The running mode is selected using the <see cref="RunningMode" /> property and determines
  /// how the Grid performs various data-related actions such as paging, sorting, filtering, and grouping. There are four available
  /// running modes: Server, Client, Callback, and WebService. The following tutorials contain information on using the different running modes:
  /// </para>
  /// <list type="bullet">
  /// <item>
  /// <description><a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Server_Running_Mode.htm">Using Server Running Mode</a></description>
  /// </item>
  /// <item>
  /// <description><a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Client_Running_Mode.htm">Using Client Running Mode</a></description>
  /// </item>
  /// <item>
  /// <description><a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Callback_AJAX_Running_Mode.htm">Using CallBack Running Mode</a></description>
  /// </item>
  /// <item>
  /// <description><a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Grid_WebService_RunningMode.htm">Using WebService Running Mode</a></description>
  /// </item>
  /// </list>
  /// <para>
  /// General styling of the Grid's frame, header and footer areas is done via *CssClass properties on the Grid class itself.
  /// Styling of column headings and data cells is done on the <see cref="GridLevel" /> 
  /// class, which is used to control aspects of a table of data. This class
  /// also contains definitions for the columns to use from the data source. For this, the <see cref="GridColumn" /> class is used. After
  /// data-binding, data is loaded from the data source into the <see cref="Items" /> collection, in the form of <see cref="GridItem" /> objects.
  /// Each GridItem corresponds to a row of data, and can be indexed by column name or index for accessing values in cells.
  /// </para>
  /// <para>
  /// Besides CSS, Grid's presentation can be modified using templates. There are two kinds of templates which can be used:
  /// <see cref="ServerTemplates" /> and <see cref="ClientTemplates" />. Client templates consist of markup and client-side binding expressions and are
  /// the suggested way of templating for situations where ASP.NET controls are not required.
  /// </para>
  /// <para>
  /// For more information on templates in Web.UI, see 
  /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_Templates_Overview.htm">Overview of Templates in Web.UI</a>
  /// </para>
  /// </remarks>
  [ToolboxData("<{0}:Grid runat=\"server\" Width=\"500\"></{0}:Grid>")]
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [Designer(typeof(ComponentArt.Web.UI.GridDesigner))]
  [ParseChildren(true)]
  public class Grid : WebControl, IPostBackEventHandler
  {

    #region Private Properties

    private string _editingId = null;
    private GridItem _editingItem = null;

    private bool _dataBound = false;
    private bool _addRow = false;
    private bool _haveUpdate = false;

    private ArrayList EventList = null;
    
    private int NumGroupings = 0;

    private ArrayList CheckedList
    {
      get
      {
        ArrayList _checkedList = (ArrayList)ViewState["CheckedList"];

        if(_checkedList == null)
        {
          _checkedList = new ArrayList();
          ViewState["CheckedList"] = _checkedList;
        }

        return _checkedList;
      }
    }

    private string ClientSearchString
    {
      get
      {
        object o = ViewState["ClientSearchString"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSearchString"] = value;
      }
    }

    private ArrayList ExpandedList
    {
      get
      {
        ArrayList _expandedList = (ArrayList)ViewState["ExpandedList"];

        if(_expandedList == null)
        {
          _expandedList = new ArrayList();
          ViewState["ExpandedList"] = _expandedList;
        }

        return _expandedList;
      }
    }

    private Hashtable _expandedGroups;
    private Hashtable ExpandedGroupInfo
    {
      get
      {
        if (_expandedGroups == null)
        {
          _expandedGroups = new Hashtable();
        }

        return _expandedGroups;
      }
    }

    private bool _serverGroupsContinued = false;
    private bool _serverGroupsContinuing = false;

    private bool NeedServerGroups
    {
      get
      {
        return (this.GroupBy != "" && this.ManualPaging && this.GroupingMode != GridGroupingMode.ConstantRecords &&
          (this.RunningMode == GridRunningMode.Server || this.RunningMode == GridRunningMode.Callback));
      }
    }

    private ArrayList SelectedList
    {
      get
      {
        ArrayList _selectedList = (ArrayList)ViewState["SelectedList"];

        if(_selectedList == null)
        {
          _selectedList = new ArrayList();
          ViewState["SelectedList"] = _selectedList;
        }

        return _selectedList;
      }
    }

    private ArrayList _serverGroups;
    private ArrayList ServerGroups
    {
      get
      {
        if (_serverGroups == null)
        {
          _serverGroups = new ArrayList();
        }

        return _serverGroups;
      }
    }

    private DataView _dataView;

    #endregion

    #region Public Properties

    /// <summary>
    /// Whether to allow the runtime resizing of columns in this Grid.
    /// </summary>
    /// <remarks>
    /// Setting this to false can greatly simplify the dynamic sizing of the Grid. For instance, if a Grid is to be placed in a container which is
    /// expected to dynamically size itself with the window, setting this property to false is recommended.
    /// </remarks>
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to allow the runtime resizing of columns in this Grid.")]
    public bool AllowColumnResizing
    {
      get
      {
        object o = ViewState["AllowColumnResizing"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["AllowColumnResizing"] = value;
      }
    }

    /// <summary>
    /// Whether to allow editing of data in this Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property sets the default for the Grid. It can be overridden at the column level with the
    /// <see cref="GridColumn" /> <see cref="GridColumn.AllowEditing" /> property. The way that editing
    /// is handled depends on the value of the <see cref="Grid.RunningMode" /> property. 
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to allow editing of data in this Grid.")]
    public bool AllowEditing
    {
      get
      {
        object o = ViewState["AllowEditing"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AllowEditing"] = value;
      }
    }

    /// <summary>
    /// Whether to allow horizontal scrolling of this Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the total <see cref="GridColumn.Width" /> of all columns is greater than the <see cref="Grid.Width" /> of the
    /// <see cref="Grid" />, the default behavior is to stretch the Grid to accomodate all of the columns. If this property
    /// is set to true, that behavior will be overridden, and the Grid will maintain its width. The user will then be able to scroll
    /// horizontally to access all of the columns.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to allow horizontal scrolling of this Grid.")]
    public bool AllowHorizontalScrolling
    {
      get
      {
        object o = ViewState["AllowHorizontalScrolling"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AllowHorizontalScrolling"] = value;
      }
    }

    /// <summary>
    /// Whether to allow HTML content in this Grid.
    /// </summary>
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to allow HTML content in this Grid.")]
    public bool AllowHtmlContent
    {
      get
      {
        object o = ViewState["AllowHtmlContent"];
        return o == null? true : (bool)o;
      }
      set
      {
        ViewState["AllowHtmlContent"] = value;
      }
    }

    /// <summary>
    /// Whether to allow multiple selection in this Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this property is false, only a single <see cref="GridItem" /> can be selected at a given time. The currently
    /// selected item(s) can be retrieved through the <see cref="Grid.SelectedItems" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to allow multiple selection in this Grid.")]
    public bool AllowMultipleSelect
    {
      get
      {
        object o = ViewState["AllowMultipleSelect"];
        return o == null? true : (bool)o;
      }
      set
      {
        ViewState["AllowMultipleSelect"] = value;
      }
    }

    /// <summary>
    /// Whether to allow paging in this Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property enables/disables automatic paging.
    /// If paging is enabled (the default), the type of pager can be selected using the <see cref="Grid.PagerStyle" />
    /// property. The pager information, which is located at the right-side of the footer by default can be customized
    /// using the <see cref="Grid.PagerInfoClientTemplateId" /> property, and its position can be altered using the
    /// <see cref="Grid.PagerInfoPosition" /> property.
    /// </para>
    /// <para>
    /// In order to use manual paging, the <see cref="Grid.ManualPaging" /> property must be set to true.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to allow paging in this Grid.")]
    public bool AllowPaging
    {
      get
      {
        object o = ViewState["AllowPaging"];
        return o == null? true : (bool)o;
      }
      set
      {
        ViewState["AllowPaging"] = value;
      }
    }

    /// <summary>
    /// Whether to allow vertical scrolling of the Grid's data area, when its height exceeds the allocated height. Default: false.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to allow vertical scrolling of the Grid's data area, when its height exceeds the allocated height.")]
    public bool AllowVerticalScrolling
    {
      get
      {
        object o = ViewState["AllowVerticalScrolling"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["AllowVerticalScrolling"] = value;
      }
    }

    /// <summary>
    /// Whether to allow text selection in this Grid.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to allow text selection in this Grid.")]
    public bool AllowTextSelection
    {
      get
      {
        object o = ViewState["AllowTextSelection"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AllowTextSelection"] = value;
      }
    }

    /// <summary>
    /// Whether to automatically adjust the page size to the height of the Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When <see cref="Grid.FillContainer" /> is set to true, this property enables the <see cref="Grid" /> to dynamically set its
    /// <see cref="Grid.PageSize" /> property. The result is that the Grid will render the correct number of rows to fill the specified
    /// Grid <see cref="Grid.Height" />.
    /// </para>
    /// </remarks>
    /// <seealso cref="FillContainer" />
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to automatically adjust the page size to the height of the Grid.")]
    public bool AutoAdjustPageSize
    {
      get
      {
        object o = ViewState["AutoAdjustPageSize"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoAdjustPageSize"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback when an item is checked or unchecked under a column
    /// of type CheckBox.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Checkboxes.htm">Using Checkboxes in Grid</a>
    /// tutorial for more information on checkbox columns.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a callback when an item is checked or unchecked.")]
    public bool AutoCallBackOnCheckChanged
    {
      get
      {
        object o = ViewState["AutoCallBackOnCheckChanged"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoCallBackOnCheckChanged"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback when columns are reordered.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether to trigger a callback immediately when the column order is changed.
    /// Re-order functionality can be enabled or disabled for a column with the <see cref="Grid.AllowReordering" />
    /// property.
    /// </para>
    /// <para>
    /// In callback running mode, since not all of the data is present on the client, certain actions require
    /// a callback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next callback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on callback running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Callback_AJAX_Running_Mode.htm">Using Callback Running Mode</a>
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a callback when columns are reordered.")]
    public bool AutoCallBackOnColumnReorder
    {
      get
      {
        object o = ViewState["AutoCallBackOnColumnReorder"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoCallBackOnColumnReorder"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback when an item is deleted.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether to trigger a callback immediately when a <see cref="GridItem" /> is deleted.
    /// The client-side <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_deleteItem_method.htm">deleteItem</a> and
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_deleteSelected_method.htm">deleteSelected</a>
    /// methods are used to delete items on the client. Reacting to the deletion on the server is covered in the tutorial
    /// mentioned below.
    /// </para>
    /// <para>
    /// In callback running mode, since not all of the data is present on the client, certain actions require
    /// a callback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next callback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on callback running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Callback_AJAX_Running_Mode.htm">Using Callback Running Mode</a>
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a callback when an item is deleted.")]
    public bool AutoCallBackOnDelete
    {
      get
      {
        object o = ViewState["AutoCallBackOnDelete"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoCallBackOnDelete"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback when an item is added.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether or not to trigger a callback immediately when a new <see cref="GridItem" />
    /// is added to the <see cref="Grid" />. New rows are added on the client using the <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_addRow_method.htm">addRow</a> and
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_addEmptyRow.htm">addEmptyRow</a>
    /// methods. Reacting to the insertion on the server is covered in the tutorial mentioned below.
    /// </para>
    /// <para>
    /// In callback running mode, since not all of the data is present on the client, certain actions require
    /// a callback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next callback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on callback running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Callback_AJAX_Running_Mode.htm">Using Callback Running Mode</a>
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a callback when an item is added.")]
    public bool AutoCallBackOnInsert
    {
      get
      {
        object o = ViewState["AutoCallBackOnInsert"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoCallBackOnInsert"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a callback when an item is edited.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether to trigger a callback immediately when a <see cref="GridItem" /> has been edited.
    /// The <see cref="Grid" /> <see cref="Grid.AllowEditing" /> and <see cref="GridColumn" /> <see cref="GridColumn.AllowEditing" />
    /// properties are used to enable editing functionality. The client-side 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_edit_method.htm">edit</a>
    /// method initiates editing, and the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_editComplete.htm">editComplete</a>
    /// method accepts the changes. Handling the update on the server is covered in the tutorial mentioned below.
    /// </para>
    /// <para>
    /// In callback running mode, since not all of the data is present on the client, certain actions require
    /// a callback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next callback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on callback running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Callback_AJAX_Running_Mode.htm">Using Callback Running Mode</a>
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a callback when an item is edited.")]
    public bool AutoCallBackOnUpdate
    {
      get
      {
        object o = ViewState["AutoCallBackOnUpdate"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoCallBackOnUpdate"] = value;
      }
    }

    /// <summary>
    /// Whether to automatically set focus to the search box.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this property is set to true, the cursor will be placed into the search box automatically when the
    /// <see cref="Grid" /> loads. The searchbox is hidden by default, so the <see cref="Grid.ShowSearchBox" />
    /// and <see cref="Grid.ShowHeader" /> properties must both be set to true for the box to display.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to automatically set focus to the search box.")]
    public bool AutoFocusSearchBox
    {
      get
      {
        object o = ViewState["AutoFocusSearchBox"];
        return o == null? true : (bool)o;
      }
      set
      {
        ViewState["AutoFocusSearchBox"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback when an item is checked or unchecked under
    /// a column of type CheckBox.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In server running mode, since not all of the data is present on the client, certain actions require
    /// a postback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next postback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on server running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Server_Running_Mode.htm">Using Server Running Mode</a>,
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Tips.htm">Common Server-Side Programming Tips</a>,
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Checkboxes.htm">Using Checkboxes in Grid</a>
    /// tutorial for more information on checkbox columns.
    /// </para>
    /// <para>
    /// Note that in client running mode, if this property is set to true, it will also cause a postback to be triggered
    /// when a checkbox has changed.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a postback when an item is checked or unchecked.")]
    public bool AutoPostBackOnCheckChanged
    {
      get
      {
        object o = ViewState["AutoPostBackOnCheckChanged"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoPostBackOnCheckChanged"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback when columns are reordered.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether to trigger a postback immediately when the column order is changed.
    /// Re-order functionality can be enabled or disabled for a column with the <see cref="Grid.AllowReordering" />
    /// property.
    /// </para>
    /// <para>
    /// In server running mode, since not all of the data is present on the client, certain actions require
    /// a postback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next postback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on server running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Server_Running_Mode.htm">Using Server Running Mode</a>,
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Tips.htm">Common Server-Side Programming Tips</a>,
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// <para>
    /// Note that in client running mode, if this property is set to true, it will also cause a postback to be triggered
    /// when column order is changed.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a postback when columns are reordered.")]
    public bool AutoPostBackOnColumnReorder
    {
      get
      {
        object o = ViewState["AutoPostBackOnColumnReorder"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoPostBackOnColumnReorder"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback when an item is deleted.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether to trigger a postback immediately when a <see cref="GridItem" /> is deleted.
    /// The client-side <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_deleteItem_method.htm">deleteItem</a> and
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_deleteSelected_method.htm">deleteSelected</a>
    /// methods are used to delete items on the client. Reacting to the deletion on the server is covered in the tutorial
    /// mentioned below.
    /// </para>
    /// <para>
    /// In server running mode, since not all of the data is present on the client, certain actions require
    /// a postback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next postback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on server running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Server_Running_Mode.htm">Using Server Running Mode</a>,
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Tips.htm">Common Server-Side Programming Tips</a>,
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// <para>
    /// Note that in client running mode, if this property is set to true, it will also cause a postback to be triggered
    /// when an item is deleted.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a postback when an item is deleted.")]
    public bool AutoPostBackOnDelete
    {
      get
      {
        object o = ViewState["AutoPostBackOnDelete"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoPostBackOnDelete"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback when an item is added.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether or not to trigger a postback immediately when a new <see cref="GridItem" />
    /// is added to the <see cref="Grid" />. New rows are added on the client using the <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_addRow_method.htm">addRow</a> and
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_addEmptyRow.htm">addEmptyRow</a>
    /// methods. Reacting to the insertion on the server is covered in the tutorial mentioned below.
    /// </para>
    /// <para>
    /// In server running mode, since not all of the data is present on the client, certain actions require
    /// a postback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next postback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on server running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Server_Running_Mode.htm">Using Server Running Mode</a>,
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Tips.htm">Common Server-Side Programming Tips</a>,
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// <para>
    /// Note that in client running mode, if this property is set to true, it will also cause a postback to be triggered
    /// when an item is inserted.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a postback when an item is added.")]
    public bool AutoPostBackOnInsert
    {
      get
      {
        object o = ViewState["AutoPostBackOnInsert"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoPostBackOnInsert"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback when an item is selected.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether to trigger a postback immediately when an item is selected. Selecting 
    /// occurs when an item is clicked, or when one of the following client-side methods is called: 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_select_method.htm">select</a>,
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_selectAll_method.htm">selectAll</a>,
    /// or <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_selectByKey.htm">selectByKey</a>.
    /// Although it doesn't really related to this property, items can also be selected from server-side code using the following methods:
    /// <see cref="Grid.Select" />, <see cref="Grid.SelectAll" />, <see cref="Grid.SelectAllKeys" />, and <see cref="Grid.SelectKey" />.
    /// </para>
    /// <para>
    /// In server running mode, since not all of the data is present on the client, certain actions require
    /// a postback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next postback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on server running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Server_Running_Mode.htm">Using Server Running Mode</a>,
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Tips.htm">Common Server-Side Programming Tips</a>,
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// <para>
    /// Note that in client running mode, if this property is set to true, it will also cause a postback to be triggered
    /// when an item is selected.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a postback when an item is selected.")]
    public bool AutoPostBackOnSelect
    {
      get
      {
        object o = ViewState["AutoPostBackOnSelect"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoPostBackOnSelect"] = value;
      }
    }

    /// <summary>
    /// Whether to perform a postback when an item is edited.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether to trigger a postback immediately when a <see cref="GridItem" /> has been edited.
    /// The <see cref="Grid" /> <see cref="Grid.AllowEditing" /> and <see cref="GridColumn" /> <see cref="GridColumn.AllowEditing" />
    /// properties are used to enable editing functionality. The client-side 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_edit_method.htm">edit</a>
    /// method initiates editing, and the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_editComplete.htm">editComplete</a>
    /// method accepts the changes. Handling the update on the server is covered in the tutorial mentioned below.
    /// </para>
    /// <para>
    /// In server running mode, since not all of the data is present on the client, certain actions require
    /// a postback immediately to enable that piece of functionality. There are other actions, however, which can be
    /// stored on the client until the next postback takes place. When this occurs, a server event is fired for every
    /// change that has taken place on the client.
    /// </para>
    /// <para>
    /// For more information on server running mode and handling server events, see the following tutorials:
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Server_Running_Mode.htm">Using Server Running Mode</a>,
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Tips.htm">Common Server-Side Programming Tips</a>,
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Events.htm">Using Server Events in Grid</a>.
    /// </para>
    /// <para>
    /// Note that in client running mode, if this property is set to true, it will also cause a postback to be triggered
    /// when an item is updated.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to perform a postback when an item is edited.")]
    public bool AutoPostBackOnUpdate
    {
      get
      {
        object o = ViewState["AutoPostBackOnUpdate"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["AutoPostBackOnUpdate"] = value;
      }
    }

    /// <summary>
    /// Whether to force a sort on the column that is being grouped by.
    /// </summary>
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to force a sort on the column that is being grouped by.")]
    public bool AutoSortOnGroup
    {
      get
      {
        object o = ViewState["AutoSortOnGroup"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["AutoSortOnGroup"] = value;
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
    /// Whether to use predefined CSS classes for theming.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to use predefined CSS classes for theming.")]
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
    /// The number of pages to load per callback request, when caching is enabled.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="Grid.CallbackCacheSize" /> property
    /// which specifies the maximum number of pages to keep in the cache at any one time.
    /// </para>
    /// </remarks>
    /// <seealso cref="CallbackCachingEnabled" />
    [DefaultValue(3)]
    [Category("Data")]
    [Description("The number of pages to load per callback request, when caching is enabled.")]
    public int CallbackCacheLookAhead
    {
      get
      {
        object o = ViewState["CallbackCacheLookAhead"];
        return o == null ? 3 : (int)o;
      }
      set
      {
        ViewState["CallbackCacheLookAhead"] = value;
      }
    }

    /// <summary>
    /// The maximum number of pages to keep in the cache.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is applicable only when <see cref="Grid.CallbackCachingEnabled" /> property is set to true.
    /// It is used in conjunction with the <see cref="Grid.CallbackCacheLookAhead" /> property which determines
    /// the number of pages to load with each request.
    /// </para>
    /// </remarks>
    /// <seealso cref="CallbackCachingEnabled" />
    [DefaultValue(20)]
    [Category("Data")]
    [Description("The maximum number of pages to keep in the cache.")]
    public int CallbackCacheSize
    {
      get
      {
        object o = ViewState["CallbackCacheSize"];
        return o == null ? 20 : (int)o;
      }
      set
      {
        ViewState["CallbackCacheSize"] = value;
      }
    }

    /// <summary>
    /// Whether to enable caching of data in callback mode.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <see cref="Grid.CallbackCacheLookAhead" /> and <see cref="Grid.CallbackCacheSize" /> properties
    /// to control the specifics of the callback cache.
    /// </para>
    /// <para>
    /// The <see cref="Grid.WebServiceCachingEnabled" /> property is used to enable caching in WebService <see cref="Grid.RunningMode" />.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Data")]
    [Description("Whether to enable caching of data in callback mode.")]
    public bool CallbackCachingEnabled
    {
      get
      {
        object o = ViewState["CallbackCachingEnabled"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["CallbackCachingEnabled"] = value;
      }
    }

    /// <summary>
    /// The optional callback parameter passed from the client.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is accessible on the client-side using the <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_callbackParameter_property.htm">callbackParameter</a>
    /// property. It is used to pass information between the client and server, and can assist in more complicated
    /// scenarios by allowing a callback to return different data based on its value.
    /// Also related to this property are the <see cref="Grid.BeforeCallback" /> which is fired before all other server-side events, 
    /// and <see cref="Grid.CallbackComplete" /> which is fired after, during a callback request.
    /// </para>
    /// </remarks>
    [Description("The optional callback parameter passed from the client.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public string CallbackParameter
    {
      get
      {
        object o = ViewState["CallbackParameter"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["CallbackParameter"] = value;
      }
    }

    /// <summary>
    /// Callback prefix to use instead of the computed one.
    /// </summary>
    [DefaultValue("")]
    [Category("Behavior")]
    [Description("Callback prefix to use instead of the computed one.")]
    public string CallbackPrefix
    {
      get
      {
        object o = ViewState["CallbackPrefix"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["CallbackPrefix"] = value;
      }
    }

    /// <summary>
    /// Whether to reload server templates on every callback.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Server templates are discussed in the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Server_Templates.htm">Using Server Templates in Grid</a> tutorial.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    [Category("Data")]
    [Description("Whether to reload server templates on every callback.")]
    public bool CallbackReloadTemplates
    {
      get
      {
        object o = ViewState["CallbackReloadTemplates"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["CallbackReloadTemplates"] = value;
      }
    }

    /// <summary>
    /// Whether to reload scripts rendered with server templates on every callback.
    /// </summary>
    [DefaultValue(false)]
    [Category("Data")]
    [Description("Whether to reload scripts rendered with server templates on every callback.")]
    public bool CallbackReloadTemplateScripts
    {
      get
      {
        object o = ViewState["CallbackReloadTemplateScripts"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["CallbackReloadTemplateScripts"] = value;
      }
    }

    /// <summary>
    /// Spacing to apply between cells in the Grid.
    /// </summary>
    [DefaultValue(0)]
    [Category("Layout")]
    [Description("Spacing to apply between cells in the Grid.")]
    public int CellSpacing
    {
      get
      {
        object o = ViewState["CellSpacing"];
        return o == null? 0 : (int)o;
      }
      set
      {
        ViewState["CellSpacing"] = value;
      }
    }

    private GridClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public GridClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new GridClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call after a callback completes.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call after a callback completes.")]
    public string ClientSideOnAfterCallback
    {
      get
      {
        object o = ViewState["ClientSideOnAfterCallback"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnAfterCallback"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call before a callback is performed.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call before a callback is performed.")]
    public string ClientSideOnBeforeCallback
    {
      get
      {
        object o = ViewState["ClientSideOnBeforeCallback"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnBeforeCallback"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when a callback error occurs.
    /// The function will be called with one parameter: the error message.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when a callback error occurs.")]
    public string ClientSideOnCallbackError
    {
      get
      {
        object o = ViewState["ClientSideOnCallbackError"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnCallbackError"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when a checkbox is checked or unchecked.
    /// The function will be called with three parameters: the item in question, the index of the
    /// column, and the checkbox DOM object.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when a checkbox is checked or unchecked.")]
    public string ClientSideOnCheckChanged
    {
      get
      {
        object o = ViewState["ClientSideOnCheckChanged"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnCheckChanged"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when columns are reordered.
    /// The function will be called with two parameters: the column index and the new index in the display order
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when columns are reordered.")]
    public string ClientSideOnColumnReorder
    {
      get
      {
        object o = ViewState["ClientSideOnColumnReorder"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnColumnReorder"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when a columns is resized.
    /// The function will be called with two parameters: the GridColumn and the change in width (in pixels)
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when a columns is resized.")]
    public string ClientSideOnColumnResize
    {
      get
      {
        object o = ViewState["ClientSideOnColumnResize"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnColumnResize"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call before an item is deleted.
    /// The function will be called with one parameter: the item that is to be deleted.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call before an item is deleted.")]
    public string ClientSideOnDelete
    {
      get
      {
        object o = ViewState["ClientSideOnDelete"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnDelete"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when an item is double-clicked.
    /// The function will be called with one parameter: the item that was double-clicked.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when an item is double-clicked.")]
    public string ClientSideOnDoubleClick
    {
      get
      {
        object o = ViewState["ClientSideOnDoubleClick"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnDoubleClick"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when the data is grouped.
    /// The function will be called with two parameters: the column to group by and
    /// a boolean representing whether to sort in a descending order.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when the data is grouped.")]
    public string ClientSideOnGroup
    {
      get
      {
        object o = ViewState["ClientSideOnGroup"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnGroup"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call before an item is added.
    /// The function will be called with one parameter: the item to insert.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call before an item is added.")]
    public string ClientSideOnInsert
    {
      get
      {
        object o = ViewState["ClientSideOnInsert"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnInsert"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when the Grid is done loading.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when the Grid is done loading.")]
    public string ClientSideOnLoad
    {
      get
      {
        object o = ViewState["ClientSideOnLoad"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnLoad"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when a paging request is made.
    /// The function will be called with one parameter: the requested page.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when a paging request is made.")]
    public string ClientSideOnPage
    {
      get
      {
        object o = ViewState["ClientSideOnPage"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnPage"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when a sort request is made.
    /// The function will be called with two parameters: the column to sort by
    /// and a boolean representing whether to sort in a descending order.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when a sort request is made.")]
    public string ClientSideOnSort
    {
      get
      {
        object o = ViewState["ClientSideOnSort"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnSort"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when an item is selected.
    /// The function will be called with one parameter: the selected item.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when an item is selected.")]
    public string ClientSideOnSelect
    {
      get
      {
        object o = ViewState["ClientSideOnSelect"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnSelect"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when an item is edited.
    /// The function will be called with two parameters: the item before the update and the
    /// item after the update.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("Identifier of client-side function to call when an item is edited.")]
    public string ClientSideOnUpdate
    {
      get
      {
        object o = ViewState["ClientSideOnUpdate"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnUpdate"] = value;
      }
    }

    private ClientTemplateCollection _clientTemplates;
    /// <summary>
    /// Collection of client-templates that may be used by this Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Client templates need to be added to this collection in order to be used by the Grid. 
    /// The template is then applied by passing its ID to one of the *ClientTemplateId proprties.
    /// For more information on client templates, see the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Client_Templates.htm">Using Client Templates in Grid</a> tutorial.
    /// </para>
    /// <para>
    /// There are actually two templating techniques which are useable by ComponentArt Grid: Server and Client.
    /// For an overview of the differences, and situations where each is appropriate see the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebUI_Templates_Overview.htm">Overview of Templates in Web.UI Controls</a> 
    /// tutorial. 
    /// </para>
    /// </remarks>
    [Browsable(false)]
    [Description("Collection of client-templates that may be used by this Grid.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplateCollection ClientTemplates
    {
      get
      {
        if(_clientTemplates == null)
        {
          _clientTemplates = new ClientTemplateCollection();
        }

        return _clientTemplates;
      }
    }

    /// <summary>
    /// URL of image to use for indicating the collapsability of a Grid item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="Grid.ExpandImageUrl" /> property which indicates
    /// that an item has children and can be expanded. The height and width of the image can be specified
    /// using the <see cref="Grid.ExpandCollapseImageHeight" /> and <see cref="Grid.ExpandCollapseImageWidth" />
    /// properties respectively. 
    /// </para>
    /// <para>
    /// The <see cref="ExpandCollapseClientTemplateId" /> allows a client template to be used instead 
    /// of images.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("URL of image to use for indicating the collapsability of a Grid item.")]
    public string CollapseImageUrl
    {
      get
      {
        object o = ViewState["CollapseImageUrl"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["CollapseImageUrl"] = value;
      }
    }

    /// <summary>
    /// The duration (in milliseconds) of the collapse animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="CollapseSlide" /> property to describe
    /// the animation which is used when items are collapsed. This is applicable in both hierarchical and grouped
    /// grids. The <see cref="Grid.ExpandSlide" /> and <see cref="Grid.ExpandDuration" /> properties are used
    /// to describe the expand animation.
    /// </para>
    /// </remarks>
    /// <seealso cref="CollapseSlide" />
    /// <seealso cref="CollapseTransition" />
    /// <seealso cref="CollapseTransitionCustomFilter" />
    [Description("The duration (in milliseconds) of the collapse animation.")]
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
        ViewState["CollapseDuration"] = value;
      }
    }

    /// <summary>
    /// The slide type to use for the collapse animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used along with the <see cref="CollapseDuration" /> property to describe the animation
    /// which is used when items are collapsed. This is applicable in both hierarchical and grouped grids.
    /// The <see cref="Grid.ExpandSlide" /> and <see cref="Grid.ExpandDuration" /> properties are used
    /// to describe the expand animation.
    /// </para>
    /// </remarks>
    /// <seealso cref="CollapseDuration" />
    /// <seealso cref="CollapseTransition" />
    /// <seealso cref="CollapseTransitionCustomFilter" />
    [Description("The slide type to use for the collapse animation.")]
    [DefaultValue(SlideType.ExponentialDecelerate)]
    [Category("Animation")]
    public SlideType CollapseSlide
    {
      get 
      {
        object o = ViewState["CollapseSlide"];
        return (o == null) ? SlideType.ExponentialDecelerate : (SlideType)o; 
      }
      set 
      {
        ViewState["CollapseSlide"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for the collapse animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify a transition filter which is used to add a special effect to the 
    /// collapse animation for Internet Explorer users. This property has no effect in other browsers.
    /// If a value of <code>TransitionType.Custom</code> is set, then the <see cref="Grid.CollapseTransitionCustomFilter" />
    /// property can be used to specify a custom filter.
    /// The <see cref="ExpandTransition" /> property is used to specify the filter used for the expand animation.
    /// </para>
    /// </remarks>
    /// <seealso cref="CollapseDuration" />
    /// <seealso cref="CollapseSlide" />
    /// <seealso cref="CollapseTransitionCustomFilter" />
    [Description("The transition effect to use for the collapse animation.")]
    [DefaultValue(TransitionType.None)]
    [Category("Animation")]
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
    /// <remarks>
    /// <para>
    /// This property is used when <see cref="Grid.CollapseTransition" /> is set to <code>TransitionType.Custom</code>.
    /// Filters are only available for Internet Explorer users, and this property will have no effect in other browsers.
    /// </para>
    /// </remarks>
    /// <seealso cref="CollapseDuration" />
    /// <seealso cref="CollapseSlide" />
    /// <seealso cref="CollapseTransition" />
    [Description("The string defining a custom transition filter to use for the collapse animation.")]
    [DefaultValue("")]
    [Category("Animation")]
    public string CollapseTransitionCustomFilter
    {
      get
      {
        object o = ViewState["CollapseTransitionCustomFilter"]; 
        return (o == null) ? string.Empty : (string)o; 
      }
      set
      {
        ViewState["CollapseTransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// Whether to distribute the width difference to all columns to the right, when a column
    /// is resized.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When set to false, this property will cause the entire <see cref="Grid" /> to resize when a column is resized.
    /// The default behavior is to maintain a constant rendered Grid width when individual columns are resized.
    /// </para>
    /// </remarks>
    [Category("Behavior")]
    [DefaultValue(true)]
    [Description("Whether to distribute the width difference to all columns to the right, when a column is resized.")]
    public bool ColumnResizeDistributeWidth
    {
      get
      {
        object o = ViewState["ColumnResizeDistributeWidth"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["ColumnResizeDistributeWidth"] = value;
      }
    }

    /// <summary>
    /// The current page of data that the Grid is on.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The total number of pages is accessible through the <see cref="Grid.PageCount" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The current page of data that the Grid is on.")]
    public int CurrentPageIndex
    {
      get
      {
        object o = ViewState["CurrentPageIndex"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["CurrentPageIndex"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the data area panel of the Grid.
    /// </summary>
    [DefaultValue("")]
    [Description("The CSS class to use for the data area panel of the Grid.")]
    public string DataAreaCssClass
    {
      get
      {
        object o = ViewState["DataAreaCssClass"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["DataAreaCssClass"] = value;
      }
    }

    private object _dataSource;
    /// <summary>
    /// The DataSource to bind to.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This value is used in conjunction with the <see cref="Grid.DataBind" /> method. 
    /// </para>
    /// </remarks>
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
    /// Whether to provide some debugging feedback at runtime.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this property is set to true, it is important to remember to remove it when the control is used in
    /// a production environment.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to provide some debugging feedback at runtime.")]
    public bool Debug
    {
      get
      {
        object o = ViewState["Debug"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["Debug"] = value;
      }
    }

    /// <summary>
    /// Whether to switch a selected item into edit mode when it is clicked.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the <see cref="Grid.AllowEditing" /> property is set to true, the default behavior
    /// is to place an item into edit mode when it is double clicked, or when an already selected item is
    /// clicked again. This property allows that behavior to be overridden, allowing the double-click to be 
    /// handled for another purpose.
    /// </para>
    /// <para>
    /// The client-side 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_itemDoubleClick_event.htm">itemDoubleClick</a>
    /// event is fired when an item is double-clicked.
    /// </para>
    /// </remarks>
    [Category("Behavior")]
    [DefaultValue(true)]
    [Description("Whether to switch a selected item into edit mode when it is clicked.")]
    public bool EditOnClickSelectedItem
    {
      get
      {
        object o = ViewState["EditOnClickSelectedItem"];
        return o == null? true : (bool)o;
      }
      set
      {
        ViewState["EditOnClickSelectedItem"] = value;
      }
    }

    /// <summary>
    /// Text to render to indicate that the Grid has no data.
    /// </summary>
    [DefaultValue("")]
    [Description("Text to render to indicate that the Grid has no data.")]
    public string EmptyGridText
    {
      get
      {
        object o = ViewState["EmptyGridText"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["EmptyGridText"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for expand/collapse cells in a hierarhical Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Images can also be used for this purpose. Items which can be expanded will use the image defined
    /// by the <see cref="Grid.ExpandImageUrl" /> property, and items which can be collapsed will be defined
    /// by the <see cref="Grid.CollapseImageUrl" /> property.
    /// </para>
    /// <para>
    /// For more information on client-templates, see the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Client_Templates.htm">Using Client Templates in Grid</a> tutorial.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for expand/collapse cells in a hierarhical Grid.")]
    public string ExpandCollapseClientTemplateId
    {
      get
      {
        object o = ViewState["ExpandCollapseClientTemplateId"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["ExpandCollapseClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of the expand and collapse images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Actual images are defined using the <see cref="Grid.ExpandImageUrl" /> and <see cref="Grid.CollapseImageUrl" /> properties.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The height (in pixels) of the expand and collapse images.")]
    public int ExpandCollapseImageHeight
    {
      get
      {
        object o = ViewState["ExpandCollapseImageHeight"];
        return o == null? 0 : (int)o;
      }
      set
      {
        ViewState["ExpandCollapseImageHeight"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the expand and collapse images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Actual images are defined using the <see cref="Grid.ExpandImageUrl" /> and <see cref="Grid.CollapseImageUrl" /> properties.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The width (in pixels) of the expand and collapse images.")]
    public int ExpandCollapseImageWidth
    {
      get
      {
        object o = ViewState["ExpandCollapseImageWidth"];
        return o == null? 0 : (int)o;
      }
      set
      {
        ViewState["ExpandCollapseImageWidth"] = value;
      }
    }

    /// <summary>
    /// URL of image to use for indicating the expandability of a Grid item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="Grid.CollapseImageUrl" /> property which indicates
    /// that an item has children and can be collapsed. The height and width of the image can be specified
    /// using the <see cref="Grid.ExpandCollapseImageHeight" /> and <see cref="Grid.ExpandCollapseImageWidth" />
    /// properties respectively. If an item has no children, and therefore cannot be expanded, the <see cref="NoExpandImageUrl" />
    /// property can be used to specify an image which indicates that.
    /// </para>
    /// <para>
    /// The <see cref="ExpandCollapseClientTemplateId" /> allows a client template to be used instead 
    /// of images.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("URL of image to use for indicating the expandability of a Grid item.")]
    public string ExpandImageUrl
    {
      get
      {
        object o = ViewState["ExpandImageUrl"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ExpandImageUrl"] = value;
      }
    }

    /// <summary>
    /// The duration (in milliseconds) of the expand animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="ExpandSlide" /> property to describe
    /// the animation which is used when items are expanded. This is applicable in both hierarchical and grouped
    /// grids. The <see cref="Grid.CollapseSlide" /> and <see cref="Grid.CollapseDuration" /> properties are used
    /// to describe the collapse animation.
    /// </para>
    /// </remarks>
    /// <seealso cref="ExpandSlide" />
    /// <seealso cref="ExpandTransition" />
    /// <seealso cref="ExpandTransitionCustomFilter" />
    [Description("The duration (in milliseconds) of the expand animation.")]
    [DefaultValue(200)]
    [Category("Animation")]
    public int ExpandDuration
    {
      get
      {
        object o = ViewState["ExpandDuration"]; 
        return (o == null) ? 200 : (int) o; 
      }
      set
      {
        ViewState["ExpandDuration"] = value;
      }
    }

    /// <summary>
    /// The slide type to use for the expand animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="ExpandDuration" /> property to describe the 
    /// animation which is used when items are expanded. This is applicable in both hierarchical and grouped
    /// grids. The <see cref="Grid.CollapseSlide" /> and <see cref="Grid.CollapseDuration" /> properties are used
    /// to describe the collapse animation.
    /// </para>
    /// </remarks>
    /// <seealso cref="ExpandDuration" />
    /// <seealso cref="ExpandTransition" />
    /// <seealso cref="ExpandTransitionCustomFilter" />
    [Description("The slide type to use for the expand animation.")]
    [DefaultValue(SlideType.ExponentialDecelerate)]
    [Category("Animation")]
    public SlideType ExpandSlide
    {
      get
      {
        object o = ViewState["ExpandSlide"]; 
        return (o == null) ? SlideType.ExponentialDecelerate : (SlideType) o; 
      }
      set
      {
        ViewState["ExpandSlide"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for the expand animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify a transition filter which is used to add a special effect to the 
    /// expand animation for Internet Explorer users. This property has no effect in other browsers.
    /// If a value of <code>TransitionType.Custom</code> is set, then the <see cref="Grid.ExpandTransitionCustomFilter" />
    /// property can be used to specify a custom filter.
    /// The <see cref="Grid.CollapseTransition" /> property is used to specify the filter used for the collapse animation.
    /// </para>
    /// </remarks>
    /// <seealso cref="ExpandSlide" />
    /// <seealso cref="ExpandDuration" />
    /// <seealso cref="ExpandTransitionCustomFilter" />
    [Description("The transition effect to use for the expand animation.")]
    [DefaultValue(TransitionType.None)]
    [Category("Animation")]
    public TransitionType ExpandTransition
    {
      get
      {
        object o = ViewState["ExpandTransition"]; 
        return (o == null) ? TransitionType.None : (TransitionType) o; 
      }
      set
      {
        ViewState["ExpandTransition"] = value;
      }
    }

    /// <summary>
    /// The string defining a custom transition filter to use for the expand animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used when <see cref="Grid.ExpandTransition" /> is set to <code>TransitionType.Custom</code>.
    /// Filters are only available for Internet Explorer users, and this property will have no effect in other browsers.
    /// </para>
    /// </remarks>
    /// <seealso cref="ExpandSlide" />
    /// <seealso cref="ExpandTransition" />
    /// <seealso cref="ExpandDuration" />
    [Description("The string defining a custom transition filter to use for the expand animation.")]
    [DefaultValue("")]
    [Category("Animation")]
    public string ExpandTransitionCustomFilter
    {
      get
      {
        object o = ViewState["ExpandTransitionCustomFilter"]; 
        return (o == null) ? string.Empty : (string)o; 
      }
      set
      {
        ViewState["ExpandTransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// The comma-separated list of IDs of DOM elements and ComponentArt controls which this Grid's items can be dropped onto.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Item dragging is enabled with the <see cref="Grid.ItemDraggingEnabled" /> property.
    /// </para>
    /// <para>
    /// When an item is being dragged, it can be styled using the <see cref="Grid.ItemDraggingCssClass" /> property, 
    /// and can be further customized with a client template using the <see cref="Grid.ItemDraggingClientTemplateId" /> property.
    /// </para>
    /// </remarks>
    /// <seealso cref="ItemDraggingEnabled" />
    [Description("The comma-separated list of IDs of DOM elements and ComponentArt controls which this Grid's items can be dropped onto.")]
    [DefaultValue("")]
    [Category("Animation")]
    public string ExternalDropTargets
    {
      get
      {
        object o = ViewState["ExternalDropTargets"];
        return (o == null) ? string.Empty : (string)o;
      }
      set
      {
        ViewState["ExternalDropTargets"] = value;
      }
    }

    /// <summary>
    /// Whether to take on the dimensions of the containing element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this property is set to true, the <see cref="Grid.Width" /> and <see cref="Grid.Height" /> of the Grid
    /// will be adjusted to fit the containing element. The <see cref="AutoAdjustPageSize" /> property can then be used
    /// to dynamically set the <see cref="Grid.PageSize" /> property, loading the correct number of records
    /// to match the rendered height of the <see cref="Grid" />.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Layout")]
    [Description("Whether to take on the dimensions of the containing element.")]
    public bool FillContainer
    {
      get
      {
        object o = ViewState["FillContainer"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["FillContainer"] = value;
      }
    }

    /// <summary>
    /// The filter (SQL WHERE expression) to apply to the data.
    /// </summary>
    [DefaultValue("")]
    [Description("The filter (SQL WHERE expression) to apply to the data.")]
    public string Filter
    {
      get
      {
        object o = ViewState["Filter"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["Filter"] = value;
      }
    }

    /// <summary>
    /// The CssClass to use on the footer of the Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property styles the footer as a whole. The text contained within the Pager and PagerInfo
    /// elements can be styled using the <see cref="Grid.PagerTextCssClass" />.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to use on the footer of the Grid.")]
    public string FooterCssClass
    {
      get
      {
        object o = ViewState["FooterCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["FooterCssClass"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) to force for the Grid footer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The footer can be further styled using the <see cref="Grid.FooterCssClass" /> property.
    /// </para>
    /// </remarks> 
    [DefaultValue(0)]
    [Description("The height (in pixels) to force for the Grid footer.")]
    public int FooterHeight
    {
      get
      {
        object o = ViewState["FooterHeight"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["FooterHeight"] = value;
      }
    }

    /// <summary>
    /// The grouping (SQL GROUP BY expression) to use on the data.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The value of this property is generated by the Grid, and can be inserted directly into a SQL statement.
    /// </para>
    /// <para>
    /// For more information on grouping see the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Grouping.htm">Grouping With Grid</a>
    /// introductory tutorial.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The grouping (SQL GROUP BY expression) to use on the data.")]
    public string GroupBy
    {
      get
      {
        object o = ViewState["GroupBy"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["GroupBy"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the drop-to-group panel on the Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The drop-to-group panel is located in the header which is hidden by default. To show the panel, 
    /// use the <see cref="Grid.ShowHeader" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for the drop-to-group panel on the Grid.")]
    public string GroupByClientTemplateId
    {
      get
      {
        object o = ViewState["GroupByClientTemplateId"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["GroupByClientTemplateId"] = value;
      }
    }
    
    /// <summary>
    /// The CssClass to use for the drop-to-group panel on the Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The drop-to-group panel is located in the header which is hidden by default. To show the panel, 
    /// use the <see cref="Grid.ShowHeader" /> property. The panel text can be changed using the
    /// <see cref="Grid.GroupByText" /> property. The panel can be further customized with client templates using the 
    /// <see cref="Grid.GroupByClientTemplateId" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to use for the drop-to-group panel on the Grid.")]
    public string GroupByCssClass
    {
      get
      {
        object o = ViewState["GroupByCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["GroupByCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to each section (grouping) in the drop-to-group feedback.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the <see cref="Grid" /> is grouped by one or more columns, the column names appear in the 
    /// header. Each name has its own section which is styled by the CSS class defined by this property.
    /// The separator between sections is styled using the <see cref="Grid.GroupBySectionSeparatorCssClass" /> property.
    /// All of the text in the drop-to-group panel can be styled using the <see cref="Grid.GroupByTextCssClass" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to each section (grouping) in the drop-to-group feedback.")]
    public string GroupBySectionCssClass
    {
      get
      {
        object o = ViewState["GroupBySectionCssClass"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["GroupBySectionCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to each section (grouping) separator area in the drop-to-group feedback.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the <see cref="Grid" /> is grouped by one or more columns, the column names appear in the 
    /// header. Each name has its own section, and this property defines the CSS class which will style the separator between them.
    /// The sections themselves are styled using the <see cref="Grid.GroupBySectionCssClass" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to each section (grouping) separator area in the drop-to-group feedback.")]
    public string GroupBySectionSeparatorCssClass
    {
      get
      {
        object o = ViewState["GroupBySectionSeparatorCssClass"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["GroupBySectionSeparatorCssClass"] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use for indicating that the grouping sort order is ascending.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the <see cref="Grid" /> is grouped by one or more columns, by default the column names appear in the header
    /// (as long as <see cref="Grid.ShowHeader" /> is set to true). The groups can be sorted in ascending or descending order
    /// by heading.
    /// If the groups are sorted, an image will be placed beside the column name, indicating the order they are sorted in.
    /// The image referenced by this property is used to indicate ascending order. The <see cref="Grid.GroupBySortDescendingImageUrl" />
    /// property is used to specify an image indicating descending order.
    /// </para>
    /// <para>
    /// Note that this is different than the <see cref="GridLevel.SortAscendingImageUrl" /> and <see cref="GridLevel.SortDescendingImageUrl" />
    /// properties which specify an image indicating the sort order when the actual <see cref="GridItem">GridItems</see> are sorted
    /// by a particular column.
    /// </para>
    /// <para>
    /// For more information on grouping, see the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Grouping.htm">Grouping with Grid</a> and
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorials.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The URL of the image to use for indicating that the grouping sort order is ascending.")]
    public string GroupBySortAscendingImageUrl
    {
      get
      {
        object o = ViewState["GroupBySortAscendingImageUrl"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["GroupBySortAscendingImageUrl"] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use for indicating that the grouping sort order is descending.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the <see cref="Grid" /> is grouped by one or more columns, by default the column names appear in the header
    /// (as long as <see cref="Grid.ShowHeader" /> is set to true). The groups can be sorted in ascending or descending order
    /// by heading.
    /// If the groups are sorted, an image will be placed beside the column name, indicating the order they are sorted in.
    /// The image referenced by this property is used to indicate descending order. The <see cref="Grid.GroupBySortAscendingImageUrl" />
    /// property is used to specify an image indicating ascending order.
    /// </para>
    /// <para>
    /// Note that this is different than the <see cref="GridLevel.SortAscendingImageUrl" /> and <see cref="GridLevel.SortDescendingImageUrl" />
    /// properties which specify an image indicating the sort order when the actual <see cref="GridItem">GridItems</see> are sorted
    /// by a particular column.
    /// </para>
    /// <para>
    /// For more information on grouping, see the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Grouping.htm">Grouping with Grid</a> and
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorials.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The URL of the image to use for indicating that the grouping sort order is descending.")]
    public string GroupBySortDescendingImageUrl
    {
      get
      {
        object o = ViewState["GroupBySortDescendingImageUrl"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["GroupBySortDescendingImageUrl"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of the grouping sort order images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="GroupBySortDescendingImageUrl" /> and <see cref="GroupBySortAscendingImageUrl" /> properties
    /// specify images which are used to indicate the sort order of group headings. This property specifies the height
    /// of those images. The width can be specified using the <see cref="GroupBySortImageHeight" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The height (in pixels) of the grouping sort order images.")]
    public int GroupBySortImageHeight
    {
      get
      {
        object o = ViewState["GroupBySortImageHeight"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["GroupBySortImageHeight"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the grouping sort order images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="GroupBySortDescendingImageUrl" /> and <see cref="GroupBySortAscendingImageUrl" /> properties
    /// specify images which are used to indicate the sort order of group headings. This property specifies the width
    /// of those images. The height can be specified using the <see cref="GroupBySortImageHeight" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The width (in pixels) of the grouping sort order images.")]
    public int GroupBySortImageWidth
    {
      get
      {
        object o = ViewState["GroupBySortImageWidth"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["GroupBySortImageWidth"] = value;
      }
    }

    /// <summary>
    /// The text to display ahead of grouping sections in the header. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the <see cref="Grid" /> is grouped by one or more columns, the column names appear in the 
    /// header. This property defines the text which precedes the column names.
    /// When the header is visible, but the Grid is not grouped, the <see cref="Grid.GroupingNotificationText" />
    /// property defines the displayed text.
    /// </para>
    /// </remarks>
    [DefaultValue("Group by:")]
    [Description("The text to display ahead of grouping sections in the header. Default: \"Group by:\".")]
    public string GroupByText
    {
      get
      {
        object o = ViewState["GroupByText"];
        return o == null ? "Group by:" : (string)o;
      }
      set
      {
        ViewState["GroupByText"] = value;
      }
    }


    /// <summary>
    /// The CssClass to apply to the drop-to-group feedback text.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property styles the text defined by the <see cref="Grid.GroupByText" /> property, 
    /// as well as the column names, in the header, which the <see cref="Grid" /> is currently grouped by. 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to the drop-to-group feedback text.")]
    public string GroupByTextCssClass
    {
      get
      {
        object o = ViewState["GroupByTextCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["GroupByTextCssClass"] = value;
      }
    }

    /// <summary>
    /// The text to display in a group header when the group continues from the previous page. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is only applicable in ConstantRows <see cref="GroupingMode" />. The text which is 
    /// displayed on the previous page, to indicate that the group continues onto the next page, is specified
    /// using the <see cref="GroupContinuingText" /> property.
    /// </para>
    /// <para>
    /// For more information on grouping, see the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Grouping.htm">Grouping with Grid</a> and
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorials.
    /// </para>
    /// </remarks>
    [DefaultValue("cont'd")]
    [Description("The text to display in a group header when the group continues from the previous page.")]
    public string GroupContinuedText
    {
      get
      {
        object o = ViewState["GroupContinuedText"];
        return o == null ? "cont'd" : (string)o;
      }
      set
      {
        ViewState["GroupContinuedText"] = value;
      }
    }

    /// <summary>
    /// The text to display in a group header when the group continues on the next page. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is only applicable in ConstantRows <see cref="GroupingMode" />. The text which is 
    /// displayed on the page which the group continues on to is specified using the <see cref="GroupContinuedText" /> property.
    /// </para>
    /// <para>
    /// For more information on grouping, see the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Grouping.htm">Grouping with Grid</a> and
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorials.
    /// </para>
    /// </remarks>
    [DefaultValue("more...")]
    [Description("The text to display in a group header when the group continues on the next page.")]
    public string GroupContinuingText
    {
      get
      {
        object o = ViewState["GroupContinuingText"];
        return o == null ? "more..." : (string)o;
      }
      set
      {
        ViewState["GroupContinuingText"] = value;
      }
    }

    /// <summary>
    /// The type of grouping behaviour to employ.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property defines the basic behavior of the Grid when it is grouped. Specifically, the value determines
    /// how a page of data will be defined. In short, each of the grouping modes has the following behavior:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <b>ConstantRecords</b> - Each page contains the same number of records (specified using the <see cref="Grid.PageSize" /> property), 
    /// regardless of the number of groups they span.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>ConstantGroups</b> - Each page contains the same number of top-level groups, 
    /// regardless of the number of records each contains.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>ConstantRows</b> - Each page contains the same number of rows (specified using the <see cref="Grid.GroupingPageSize" /> property).
    /// The specified number of rows will include all group headings, as well as all records. 
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// It is worth noting that a Grid in either <code>ConstantGroups</code> or <code>ConstantRecords</code>
    /// grouping mode will alter its rendered height as groups are expanded and collapsed. <code>ConstantRows</code> grouping mode is 
    /// the only way to get around that behavior.
    /// </para>
    /// <para>
    /// The <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Grouping.htm">Grouping with Grid</a> tutorial provides
    /// an introduction to each of the grouping modes, while
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Manual_Paging.htm">Grid Grouping with Manual Paging</a> discusses
    /// how to implement manual paging in combination with grouping.
    /// </para>
    /// </remarks>
    [DefaultValue(GridGroupingMode.ConstantGroups)]
    [Description("The type of grouping behaviour to employ.")]
    public GridGroupingMode GroupingMode
    {
      get
      {
        object o = ViewState["GroupingMode"];
        return (o == null) ? GridGroupingMode.ConstantGroups : (GridGroupingMode)o;
      }
      set
      {
        ViewState["GroupingMode"] = value;
      }
    }

    /// <summary>
    /// The location in the Grid to render the drop-to-group indicator.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the location within the rendered Grid to place the grouping notification. 
    /// If the Grid is currently not grouped by any column, the grouping notification will consist of the text specified by the
    /// <see cref="Grid.GroupingNotificationText" /> property. If the Grid is grouped, it will consist of the column names which
    /// the Grid is currently grouped by.
    /// The two "top" positions
    /// are rendered within the header, and the "bottom" positions render within the footer. For that reason, depending on the 
    /// chosen position, <see cref="Grid.ShowHeader" /> or <see cref="Grid.ShowFooter" /> must be set to true in order to render
    /// the notification. The default value for this property is <code>GridElementPosition.TopLeft</code>. 
    /// </para>
    /// <para>
    /// It is worth noting that all of the following properties use the same positioning options. To prevent conflicts, 
    /// each of their positions must be taken into consideration if any of the default values are changed.
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="Grid.SearchBoxPosition" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.PagerPosition" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.PagerInfoPosition" />
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    [DefaultValue(GridElementPosition.TopLeft)]
    [Description("The location in the Grid to render the drop-to-group indicator.")]
    public GridElementPosition GroupingNotificationPosition
    {
      get
      {
        object o = ViewState["GroupingNotificationPosition"];
        return o == null? GridElementPosition.TopLeft : (GridElementPosition)o;
      }
      set
      {
        ViewState["GroupingNotificationPosition"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to the grouping notification text.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When <see cref="Grid.ShowHeader" /> is true and the <see cref="Grid" /> is not grouped, 
    /// the text defined by <see cref="Grid.GroupingNotificationText" /> will be displayed in the header.
    /// This property defines the CSS class which will be used to style that text.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to the grouping notification text.")]
    public string GroupingNotificationTextCssClass
    {
      get
      {
        object o = ViewState["GroupingNotificationTextCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["GroupingNotificationTextCssClass"] = value;
      }
    }

    /// <summary>
    /// The text to use for indicating drop-to-group functionality.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When <see cref="Grid.ShowHeader" /> is true, and the <see cref="Grid" /> is not grouped,
    /// the text defined by this property will be displayed in the header. The text can be styled
    /// using the <see cref="Grid.GroupingNotificationTextCssClass" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("Drag a column to this area to group by it.")]
    [Description("The text to use for indicating drop-to-group functionality.")]
    public string GroupingNotificationText
    {
      get
      {
        object o = ViewState["GroupingNotificationText"];
        return o == null? "Drag a column to this area to group by it." : (string)o;
      }
      set
      {
        ViewState["GroupingNotificationText"] = value;
      }
    }

    /// <summary>
    /// Whether to page based on rows rather than groups, when grouped.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is deprecated. Use the ConstantRecords <see cref="Grid.GroupingMode" /> instead.
    /// For more information on Grouping functionality, see the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Grouping.htm">Grouping with Grid</a>
    /// introductory tutorial.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Description("Whether to page based on rows rather than groups, when grouped.")]
    [Obsolete("Deprecated.  Use GridGroupingMode.ConstantRecords instead.", false)]
    public bool GroupingPageByRow
    {
      get
      {
        object o = ViewState["GroupingPageByRow"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["GroupingPageByRow"] = value;
      }
    }

    /// <summary>
    /// The page size to use for grouped data.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used when manual paging (<see cref="Grid.ManualPaging" />) is used 
    /// in conjunction with grouping and defines the total number of records and grouped headings which will
    /// be present on each page. See also the <see cref="Grid.GroupingMode" /> property.
    /// The <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Grouping.htm">Grouping with Grid</a> tutorial provides
    /// a good introduction to grouping functionality.
    /// </para>
    /// </remarks>
    [DefaultValue(20)]
    [Description("The page size to use for grouped data.")]
    public int GroupingPageSize
    {
      get
      {
        object o = ViewState["GroupingPageSize"]; 
        return (o == null) ? 20 : (int) o; 
      }
      set
      {
        ViewState["GroupingPageSize"] = value;
      }
    }

    /// <summary>
    /// Whether to count group headings as rows enforcing GroupingPageSize.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is deprecated. 
    /// For more information on grouping see the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Grouping.htm">Grouping with Grid</a>
    /// introductory tutorial.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Description("Whether to count group headings as rows enforcing GroupingPageSize.")]
    [Obsolete("Deprecated.  Use GridGroupingMode.ConstantRows instead.", false)]
    public bool GroupingCountHeadingsAsRows
    {
      get
      {
        object o = ViewState["GroupingTreatHeadingsAsRows"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["GroupingTreatHeadingsAsRows"] = value;
      }
    }

    /// <summary>
    /// The CssClass to use for the Grid header.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Grid" /> header is hidden by default. It is displayed using the <see cref="Grid.ShowHeader" />
    /// property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to use for the Grid header.")]
    public string HeaderCssClass
    {
      get
      {
        object o = ViewState["HeaderCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["HeaderCssClass"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) to force for the Grid header.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The header can be further styled using the <see cref="HeaderCssClass" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The height (in pixels) to force for the Grid header.")]
    public int HeaderHeight
    {
      get
      {
        object o = ViewState["HeaderHeight"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["HeaderHeight"] = value;
      }
    }

    /// <summary>
    /// The base to use for URLs of images used in this Grid.
    /// </summary>
    [DefaultValue("")]
    [Description("The base to use for URLs of images used in this Grid.")]
    public string ImagesBaseUrl
    {
      get
      {
        object o = ViewState["ImagesBaseUrl"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ImagesBaseUrl"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to indentation cells in hierarchical Grids.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In a hierarchical grid, each level of the hierarchy is indented. This property
    /// styles the cells which make up that indentation. Their width can be specified using the
    /// <see cref="IndentCellWidth" /> property.
    /// </para>
    /// <para>
    /// For more information on hierarchical grids, see the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Hierarchical.htm">Creating a Hierarchical Grid</a> tutorial.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to indentation cells in hierarchical Grids.")]
    public string IndentCellCssClass
    {
      get
      {
        object o = ViewState["IndentCellCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["IndentCellCssClass"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the indent cells in a hierarchical Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In a hierarchical grid, each level of the hierarchy is indented. This property
    /// specifies the width, in pixels, of that indent. The indent cells can be further styled
    /// using the <see cref="IndentCellCssClass" /> property.
    /// </para>
    /// <para>
    /// For more information on hierarchical grids, see the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Hierarchical.htm">Creating a Hierarchical Grid</a> tutorial.
    /// </para>
    /// </remarks>
    [DefaultValue(16)]
    [Description("The width (in pixels) of the indent cells in a hierarchical Grid.")]
    public int IndentCellWidth
    {
      get
      {
        object o = ViewState["IndentCellWidth"]; 
        return (o == null) ? 16 : (int) o; 
      }
      set
      {
        ViewState["IndentCellWidth"] = value;
      }
    }

    private GridItemCollection _items;
    /// <summary>
    /// The collection of GridItems in this Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property provides access to the top-level <see cref="GridItem">GridItems</see> for the <see cref="Grid" />.
    /// The data for a Grid is represented by a hierarchy of <code>GridItem</code> objects. 
    /// Each item has an <see cref="GridItem.Items" /> collection which contains all of that item's children.
    /// </para>
    /// <para>
    /// Presentation of the data is handled by <see cref="GridLevel" /> objects. The <see cref="GridItem.Level" />
    /// property contains an index number which corresponds to an index within the Grid's <see cref="Grid.Levels" />
    /// collection. Each item is associated with a specific level which can be accessed using that index number.
    /// </para>
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The collection of GridItems in this Grid.")]
    public GridItemCollection Items
    {
      get
      {
        if(_items == null)
        {
          _items = new GridItemCollection();
        }

        return _items;
      }
    }

    /// <summary>
    /// Whether to enable keyboard control of this Grid.
    /// If set, use Ctrl + left/right arrows to page,
    /// up/down arrows to move through items, and Enter
    /// to select the item currently highlighted. Highlighting
    /// is done by applying the the GridLevel hover row styles.
    /// </summary>
    /// <seealso cref="GridLevel.HoverRowCssClass" />
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to enable keyboard control of this Grid.")]
    public bool KeyboardEnabled
    {
      get
      {
        object o = ViewState["KeyboardEnabled"]; 
        return (o == null) ? true : (bool) o; 
      }
      set
      {
        ViewState["KeyboardEnabled"] = value;
      }
    }

    private GridLevelCollection _levels;
    /// <summary>
    /// The collection of GridLevels in this Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property provides access to the <see cref="GridLevelCollection" /> which contains all of the 
    /// <see cref="GridLevel">GridLevels</see> for the <see cref="Grid" /> instance. The top level always has
    /// the lowest index in the collection.
    /// </para>
    /// <para>
    /// On the server-side, Grid data is represented by a hierarchy of <see cref="GridItem" /> objects. 
    /// The top-level items are contained within the Grid's <see cref="Grid.Items">Items</see> <see cref="GridItemCollection" />. 
    /// Successive items in the hierarchy are contained within <code>GridItem</code> <see cref="GridItem.Items" /> collections.
    /// Each item, along with its children, is associated with a level, which is responsible for data presentation. 
    /// The <code>GridItem</code> <see cref="GridItem.Level" /> property contains the index, within this collection, of the level
    /// that is associated with the item.
    /// </para>
    /// </remarks>
    [Browsable(false)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("The collection of GridLevels in this Grid.")]
    public GridLevelCollection Levels
    {
      get
      {
        if(_levels == null)
        {
          _levels = new GridLevelCollection();
        }

        return _levels;
      }
    }

    /// <summary>
    /// The ID of the client template to use for feedback while waiting on a callback to complete.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the ID of the client-template to use as a loading panel. The loading panel is an 
    /// indicator used during callbacks to inform the user that the Grid is in the process of retrieving data.
    /// The general position of the loading panel is specified using the <see cref="Grid.LoadingPanelPosition" /> property
    /// and can be further customized using the <see cref="Grid.LoadingPanelOffsetX" /> and <see cref="Grid.LoadingPanelOffsetY" />
    /// properties. This functionality is enabled using the <see cref="Grid.LoadingPanelEnabled" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for feedback while waiting on a callback to complete.")]
    public string LoadingPanelClientTemplateId
    {
      get
      {
        object o = ViewState["LoadingPanelClientTemplateId"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["LoadingPanelClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// Whether to display a special feedback panel while callback data is loading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The panel is defined using the <see cref="Grid.LoadingPanelClientTemplateId" /> property. It is used
    /// to notify the user that a callback is in progress. Its location is specified using the <see cref="Grid.LoadingPanelPosition" />
    /// property in combination with the <see cref="Grid.LoadingPanelOffsetX" /> and <see cref="Grid.LoadingPanelOffsetY" /> properties.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to display a special feedback panel while callback data is loading.")]
    public bool LoadingPanelEnabled
    {
      get
      {
        object o = ViewState["LoadingPanelEnabled"]; 
        return (o == null) ? true : (bool) o; 
      }
      set
      {
        ViewState["LoadingPanelEnabled"] = value;
      }
    }

    /// <summary>
    /// The duration of the fade effect when transitioning the loading template, in milliseconds. A value of 0
    /// (default) turns off the fade effect.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The loading panel is an indicator used during callbacks to inform the user that the Grid is in the process of 
    /// retrieving data. The panel can be set to fade in and out using this property. If a value other than zero
    /// is set, then the fade will occur. The maximum opacity is set using the <see cref="LoadingPanelMaximumOpacity" /> property.
    /// </para>
    /// <para>
    /// The actual loading panel can be customized using the <see cref="LoadingPanelClientTemplateId" /> property. It
    /// is enabled using the <see cref="LoadingPanelEnabled" /> property, and its position can be specified using the
    /// <see cref="LoadingPanelPosition" /> property, along with the <see cref="LoadingPanelOffsetX" /> and 
    /// <see cref="LoadingPanelOffsetY" /> properties. 
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The duration of the fade effect when transitioning the loading template, in milliseconds.")]
    public int LoadingPanelFadeDuration
    {
      get
      {
        object o = ViewState["LoadingPanelFadeDuration"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["LoadingPanelFadeDuration"] = value;
      }
    }

    /// <summary>
    /// The maximum opacity percentage to fade to. Between 0 and 100. Default: 100.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The loading panel is an indicator used during callbacks to inform the user that the Grid is in the process of retrieving
    /// data. The panel can be set to fade in and out using the <see cref="LoadingPanelFadeDuration" /> property. This property
    /// specifies the opacity of the loading panel when it is fully faded in. 
    /// The default value, 100, means that the panel will be fully opaque.
    /// </para>
    /// <para>
    /// The actual loading panel can be customized using the <see cref="LoadingPanelClientTemplateId" /> property. It
    /// is enabled using the <see cref="LoadingPanelEnabled" /> property, and its position can be specified using the
    /// <see cref="LoadingPanelPosition" /> property, along with the <see cref="LoadingPanelOffsetX" /> and 
    /// <see cref="LoadingPanelOffsetY" /> properties. 
    /// </para>
    /// </remarks>
    [DefaultValue(100)]
    [Description("The maximum opacity percentage to fade to.")]
    public int LoadingPanelFadeMaximumOpacity
    {
      get
      {
        object o = ViewState["LoadingPanelFadeMaximumOpacity"];
        return (o == null) ? 100 : (int)o;
      }
      set
      {
        ViewState["LoadingPanelFadeMaximumOpacity"] = value;
      }
    }

    /// <summary>
    /// The position of the loading panel relative to the Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the location, relative to the frame of the rendered grid, to display the Loading panel. The loading
    /// panel is an indicator which is used during callbacks to indicate to the user that the <see cref="Grid" /> is in the process of 
    /// loading data. It is defined
    /// using a client-template, specified by the <see cref="Grid.LoadingPanelClientTemplateId" /> property. Its position can be 
    /// further customized using the <see cref="Grid.LoadingPanelOffsetX" /> and <see cref="Grid.LoadingPanelOffsetY" /> properties.
    /// This functionality is enabled using the <see cref="Grid.LoadingPanelEnabled" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(GridRelativePosition.TopCenter)]
    [Category("Layout")]
    [Description("The position of the loading panel relative to the Grid.")]
    public GridRelativePosition LoadingPanelPosition 
    {
      get
      {
        object o = ViewState["LoadingPanelPosition"]; 
        return (o == null) ? GridRelativePosition.TopCenter : (GridRelativePosition) o; 
      }
      set
      {
        ViewState["LoadingPanelPosition"] = value;
      }
    }

    /// <summary>
    /// The X offset (in pixels) of the loading panel from its relative position.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="Grid.LoadingPanelOffsetY" /> property to fine-tune the
    /// position of the loading panel relative to the position specified by the <see cref="Grid.LoadingPanelPosition" /> property.
    /// The loading panel is an indicator which is used during callbacks to indicate to the user that the <see cref="Grid" /> is in the process
    /// of loading data. It is defined using a client-template, specified by the <see cref="Grid.LoadingPanelClientTemplateId" /> property. 
    /// This functionality is enabled using the <see cref="Grid.LoadingPanelEnabled" /> property.
    /// </para>
    /// </remarks>
    /// <seealso cref="LoadingPanelPosition" />
    [DefaultValue(0)]
    [Description("The X offset (in pixels) of the loading panel from its relative position.")]
    public int LoadingPanelOffsetX 
    {
      get
      {
        object o = ViewState["LoadingPanelOffsetX"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["LoadingPanelOffsetX"] = value;
      }
    }

    /// <summary>
    /// The Y offset (in pixels) of the loading panel from its relative position.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="Grid.LoadingPanelOffsetX" /> property to fine-tune the
    /// position of the loading panel relative to the position specified by the <see cref="Grid.LoadingPanelPosition" /> property.
    /// The loading panel is an indicator which is used during callbacks to indicate to the user that the <see cref="Grid" /> is in the process
    /// of loading data. It is defined using a client-template, specified by the <see cref="Grid.LoadingPanelClientTemplateId" /> property.
    /// This functionality is enabled using the <see cref="Grid.LoadingPanelEnabled" /> property.
    /// </para>
    /// </remarks>
    /// <seealso cref="LoadingPanelPosition" />
    [DefaultValue(0)]
    [Description("The Y offset (in pixels) of the loading panel from its relative position.")]
    public int LoadingPanelOffsetY
    {
      get
      {
        object o = ViewState["LoadingPanelOffsetY"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["LoadingPanelOffsetY"] = value;
      }
    }

    /// <summary>
    /// Whether to enable manual paging in this Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Manual paging is accomplished differently depending on the running mode, and whether or not the Grid is grouped.
    /// If this property is set to false, the Grid will assume that the data assigned to <see cref="Grid.DataSource" />
    /// is complete, and will perform paging internally. If it is true, paging must be handled by the developer.
    /// </para>
    /// <para>
    /// A grid which is in <code>ConstantRows</code> <see cref="Grid.GroupingMode" /> <b>must</b> utilize manual paging.
    /// When a Grid is grouped, and this property is set to true, 
    /// manual paging is accomplished using the <see cref="Grid.NeedGroups" /> and <see cref="Grid.NeedGroupData" /> events.
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to enable manual paging in this Grid.")]
    public bool ManualPaging
    {
      get
      {
        object o = ViewState["ManualPaging"]; 
        return (o == null) ? false : (bool) o; 
      }
      set
      {
        ViewState["ManualPaging"] = value;
      }
    }

    /// <summary>
    /// URL of image to use for indicating the non-expandability of a Grid item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="Grid.ExpandImageUrl" /> and <see cref="Grid.CollapseImageUrl" /> 
    /// properties which indicate that an item has children and can be expanded and collapsed. 
    /// The height and width of the image can be specified
    /// using the <see cref="Grid.ExpandCollapseImageHeight" /> and <see cref="Grid.ExpandCollapseImageWidth" />
    /// properties respectively. 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("URL of image to use for indicating the non-expandability of a Grid item.")]
    public string NoExpandImageUrl
    {
      get
      {
        object o = ViewState["NoExpandImageUrl"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["NoExpandImageUrl"] = value;
      }
    }

    /// <summary>
    /// The ID of the client-side handler for the OnContextMenu (right-click) event.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The function specified by this property should call one of the <code>showContextMenu*</code> methods
    /// of the <see cref="Menu" /> which will be displayed.
    /// </para>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Adding_a_Context_Menu_to_a_Grid.htm">Adding a Context Menu to a Grid</a>
    /// tutorial for more information on implementing context menus.
    /// </para>
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Description("The ID of the client-side handler for the OnContextMenu (right-click) event.")]
    public string OnContextMenu
    {
      get
      {
        object o = ViewState["OnContextMenu"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["OnContextMenu"] = value;
      }
    }

    /// <summary>
    /// The number of pages in this Grid. Read-only.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The size of each page is contained in the <see cref="Grid.PageSize" /> property or the <see cref="Grid.GroupingPageSize" />
    /// property, depending on whether the <see cref="Grid" /> is grouped.
    /// </para>
    /// </remarks>
    [Description("The number of pages in this Grid. Read-only.")]
    public int PageCount
    {
      get
      {
        if(this.GroupBy != "")
        {
          if (this.GroupingMode == GridGroupingMode.ConstantRecords || this.GroupingMode == GridGroupingMode.ConstantRows || GroupingPageByRow)
          {
            return (int)Math.Ceiling((double)this.RecordCount / this.GroupingPageSize);
          }
          else
          {
            return (int)Math.Ceiling((double)this.NumGroupings / this.GroupingPageSize);
          }
        }
        else
        {
          return (int)Math.Ceiling((double)this.RecordCount / this.PageSize);
        }
      }
    }

    /// <summary>
    /// Whether to pad incomplete pages by rendering empty rows.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default behavior is to simply render empty space if the number of rows doesn't fill the space on a page.
    /// If this property is set to true, the space will be filled with empty rows. 
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Description("Whether to pad incomplete pages by rendering empty rows.")]
    public bool PagePaddingEnabled
    {
      get
      {
        object o = ViewState["PagePaddingEnabled"]; 
        return (o == null) ? false : (bool) o; 
      }
      set
      {
        ViewState["PagePaddingEnabled"] = value;
      }
    }

    /// <summary>
    /// Whether to look for and use active images for the pager (on mouse down).
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to look for and use active images for the pager (on mouse down).")]
    public bool PagerButtonActiveEnabled
    {
      get
      {
        object o = ViewState["PagerButtonActiveEnabled"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["PagerButtonActiveEnabled"] = value;
      }
    }

    /// <summary>
    /// Whether to look for and use hover images for the pager (on mouse over).
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to look for and use hover images for the pager (on mouse over).")]
    public bool PagerButtonHoverEnabled
    {
      get
      {
        object o = ViewState["PagerButtonHoverEnabled"]; 
        return (o == null) ? false : (bool) o; 
      }
      set
      {
        ViewState["PagerButtonHoverEnabled"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of pager buttons.
    /// </summary>
    [DefaultValue(0)]
    [Description("The height (in pixels) of pager buttons.")]
    public int PagerButtonHeight
    {
      get
      {
        object o = ViewState["PagerButtonHeight"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["PagerButtonHeight"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of pager buttons.
    /// </summary>
    [DefaultValue(0)]
    [Description("The width (in pixels) of pager buttons.")]
    public int PagerButtonWidth
    {
      get
      {
        object o = ViewState["PagerButtonWidth"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["PagerButtonWidth"] = value;
      }
    }

    /// <summary>
    /// The padding (in pixels) to apply to the pager buttons. Default: 5.
    /// </summary>
    [DefaultValue(5)]
    [Description("The padding (in pixels) to apply to the pager buttons. Default: 5.")]
    public int PagerButtonPadding
    {
      get
      {
        object o = ViewState["PagerButtonPadding"]; 
        return (o == null) ? 5 : (int) o; 
      }
      set
      {
        ViewState["PagerButtonPadding"] = value;
      }
    }

    /// <summary>
    /// The URL of the folder which contains pager images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is applicable when the <see cref="Grid.PagerStyle" /> property is set to <code>GridPagerStyle.Slider</code>
    /// or <code>GridPagerStyle.Buttons</code>. The images need to have specific names within this folder,
    /// which can be discovered by looking in the corresponding folder from one of the sample <see cref="Grid">Grids</see> which are
    /// installed with Web.UI.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The URL of the folder which contains pager images.")]
    public string PagerImagesFolderUrl
    {
      get
      {
        object o = ViewState["PagerImagesFolderUrl"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["PagerImagesFolderUrl"] = value;
      }
    }

    /// <summary>
    /// The relative position within the Grid of the pager info panel.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the location within the rendered Grid to place the pager info panel. The panel
    /// contains information about the current page, as well as the total number of pages, and rows of data. 
    /// The two "top" positions
    /// are rendered within the header, and the "bottom" positions render within the footer. For that reason, depending on the 
    /// chosen position, <see cref="Grid.ShowHeader" /> or <see cref="Grid.ShowFooter" /> must be set to true in order to render
    /// the notification. The default value for this property is <code>GridElementPosition.BottomLeft</code>. 
    /// </para>
    /// <para>
    /// It is worth noting that all of the following properties use the same positioning options. To prevent conflicts, 
    /// each of their positions must be taken into consideration if any of the default values are changed.
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="Grid.SearchBoxPosition" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.GroupingNotificationPosition" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.PagerPosition" />
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    [DefaultValue(GridElementPosition.BottomRight)]
    [Description("The relative position within the Grid of the pager info panel.")]
    public GridElementPosition PagerInfoPosition
    {
      get
      {
        object o = ViewState["PagerInfoPosition"];
        return o == null? GridElementPosition.BottomRight : (GridElementPosition)o;
      }
      set
      {
        ViewState["PagerInfoPosition"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the pager info panel.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property allows a custom client template to be specified, providing paging information for the <see cref="Grid" />.
    /// This property is not required, as there is always a default pager info panel present in a Grid, 
    /// containing the current page, total number of pages, and total number of records.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for the pager info panel.")]
    public string PagerInfoClientTemplateId
    {
      get
      {
        object o = ViewState["PagerInfoClientTemplateId"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["PagerInfoClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The relative position within the Grid of the pager.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the location within the rendered Grid to place the pager. The type of pager is
    /// specified by the <see cref="Grid.PagerStyle" /> property.
    /// The two "top" positions
    /// are rendered within the header, and the "bottom" positions render within the footer. For that reason, depending on the 
    /// chosen position, <see cref="Grid.ShowHeader" /> or <see cref="Grid.ShowFooter" /> must be set to true in order to render
    /// the notification. The default value for this property is <code>GridElementPosition.BottomLeft</code>. 
    /// </para>
    /// <para>
    /// It is worth noting that all of the following properties use the same positioning options. To prevent conflicts, 
    /// each of their positions must be taken into consideration if any of the default values are changed.
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="Grid.SearchBoxPosition" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.GroupingNotificationPosition" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.PagerInfoPosition" />
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    [DefaultValue(GridElementPosition.BottomLeft)]
    [Description("The relative position within the Grid of the pager.")]
    public GridElementPosition PagerPosition
    {
      get
      {
        object o = ViewState["PagerPosition"];
        return o == null? GridElementPosition.BottomLeft : (GridElementPosition)o;
      }
      set
      {
        ViewState["PagerPosition"] = value;
      }
    }

    /// <summary>
    /// The type of pager to render.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the style of pager to use. The pager provides an interface to the user for 
    /// navigating through pages of data.
    /// There are three options, the default being <code>GridPagerStyle.Numbered</code>.
    /// The other two options use images, which must be placed in the folder specified by the <see cref="Grid.PagerImagesFolderUrl" />
    /// property. The rendered position of the pager can be specified using the <see cref="Grid.PagerPosition" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(GridPagerStyle.Numbered)]
    [Description("The type of pager to render.")]
    public GridPagerStyle PagerStyle
    {
      get
      {
        object o = ViewState["PagerStyle"]; 
        return (o == null) ? GridPagerStyle.Numbered : (GridPagerStyle) o; 
      }
      set
      {
        ViewState["PagerStyle"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to the pager text.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The CSS class defined by this property is applied to both the pager, if <see cref="Grid.PagerStyle" /> is set to 
    /// "text", and to the pager info. It will also be applied to any text which is inside the <see cref="Grid.PagerInfoTemplate" />
    /// if one is defineds.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to the pager text.")]
    public string PagerTextCssClass
    {
      get
      {
        object o = ViewState["PagerTextCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["PagerTextCssClass"] = value;
      }
    }

    /// <summary>
    /// The number of items to render per page of the Grid. Default: 20.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="GroupingPageSize" /> property is used instead of this property when the 
    /// Grid is grouped.
    /// </para>
    /// </remarks>
    [DefaultValue(20)]
    [Description("The number of items to render per page of the Grid. Default: 20.")]
    public int PageSize
    {
      get
      {
        object o = ViewState["PageSize"]; 
        return (o == null) ? 20 : (int) o; 
      }
      set
      {
        ViewState["PageSize"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the area at the bottom of the Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The content defined by this template is rendered below the footer. It is styled by the CSS class
    /// defined by the <see cref="Grid.PostFooterCssClass" /> property. Note that this content is still
    /// displayed if <see cref="Grid.ShowFooter" /> is false.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for the area at the bottom of the Grid.")]
    public string PostFooterClientTemplateId
    {
      get
      {
        object o = ViewState["PostFooterClientTemplateId"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["PostFooterClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the area at the bottom of the Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property defines the CSS class to apply to the content contained within the template
    /// defined by the <see cref="Grid.PostFooterClientTemplateId" /> property. It is rendered below
    /// the footer.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CSS class to use for the area at the bottom of the Grid.")]
    public string PostFooterCssClass
    {
      get
      {
        object o = ViewState["PostFooterCssClass"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["PostFooterCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to pre-expand all groups.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to pre-expand all groups.")]
    public bool PreExpandOnGroup
    {
      get
      {
        object o = ViewState["PreExpandOnGroup"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["PreExpandOnGroup"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the area at the top of the Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property defines a client template which is displayed above the header. Its content
    /// is styled using the CSS class defined by the <see cref="Grid.PreHeaderCssClass" /> property.
    /// Note that this template is displayed even if the <see cref="Grid.ShowHeader" /> property 
    /// is set to false.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for the area at the top of the Grid.")]
    public string PreHeaderClientTemplateId
    {
      get
      {
        object o = ViewState["PreHeaderClientTemplateId"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["PreHeaderClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the area at the top of the Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The CSS class defined by this property is used to style the content of the client template
    /// specified by the <see cref="Grid.PreHeaderClientTemplateId" /> property. 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CSS class to use for the area at the top of the Grid.")]
    public string PreHeaderCssClass
    {
      get
      {
        object o = ViewState["PreHeaderCssClass"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["PreHeaderCssClass"] = value;
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
        if(_preloadImages == null)
        {
          _preloadImages = new StringCollection();
        }

        return _preloadImages;
      }
    }

    /// <summary>
    /// Whether to pre-load all levels. Default: true.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property should be set to false for hierarchical Grids which intend to load all of their data
    /// incrementally, when items are expanded by the user. 
    /// </para>
    /// <para>
    /// The <see cref="Grid.NeedChildDataSource" /> event is fired when an item is expanded.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    [Category("Behavior")]
    [Description("Whether to pre-load all levels. Default: true.")]
    public bool PreloadLevels
    {
      get
      {
        object o = ViewState["PreloadLevels"];
        return o == null? true : (bool)o;
      }
      set
      {
        ViewState["PreloadLevels"] = value;
      }
    }

    /// <summary>
    /// The number of items (records) that this Grid contains.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this property is set manually, it must be set <b>after</b> <see cref="Grid.DataBind" />, or it will
    /// be overwritten.
    /// </para>
    /// </remarks>
    [Description("The number of items (records) that this Grid contains.")]
    public int RecordCount
    {
      get
      {
        object o = ViewState["RecordCount"];
        return o == null? 0 : (int)o;
      }
      set
      {
        ViewState["RecordCount"] = value;
      }
    }

    /// <summary>
    /// The offset to render items from. Setting this will override the paging mechanism.
    /// </summary>
    [DefaultValue(0)]
    [Description("The offset to render items from. Setting this will override the paging mechanism.")]
    public int RecordOffset
    {
      get
      {
        object o = ViewState["RecordOffset"];
        return o == null? 0 : (int)o;
      }
      set
      {
        ViewState["RecordOffset"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the item being dragged.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is not required, as the Grid will rendered items by default in the same way when they are being dragged
    /// as when they are not. Item dragging is enabled using the <see cref="ItemDraggingEnabled" /> property. A CSS class
    /// can be applied to the item using the <see cref="ItemDraggingCssClass" /> property.
    /// </para>
    /// </remarks>
    /// <seealso cref="ItemDraggingEnabled" />
    [DefaultValue("")]
    [Description("The ID of the client template to use for the item being dragged.")]
    public string ItemDraggingClientTemplateId
    {
      get
      {
        object o = ViewState["ItemDraggingClientTemplateId"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ItemDraggingClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to the item being dragged.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Item dragging is enabled using the <see cref="ItemDraggingEnabled" /> property. The appearance of the
    /// item can be further customized using the <see cref="ItemDraggingClientTemplateId" /> property.
    /// </para>
    /// </remarks>
    /// <seealso cref="ItemDraggingEnabled" />
    [DefaultValue("")]
    [Description("The CSS class to apply to the item being dragged.")]
    public string ItemDraggingCssClass
    {
      get
      {
        object o = ViewState["ItemDraggingCssClass"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ItemDraggingCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to permit items to be dragged.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Items which are being dragged can be styled using the <see cref="ItemDraggingCssClass" /> property, 
    /// and further customized using the <see cref="ItemDraggingClientTemplateId" /> property.
    /// </para>
    /// </remarks>
    /// <seealso cref="ExternalDropTargets" />
    [DefaultValue(false)]
    [Description("Whether to permit items to be dragged.")]
    public bool ItemDraggingEnabled
    {
      get
      {
        object o = ViewState["ItemDraggingEnabled"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["ItemDraggingEnabled"] = value;
      }
    }

    /// <summary>
    /// Whether to data bind server templates at the moment they are instantiated. Default: false.
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to data bind server templates at the moment they are instantiated.")]
    public bool PreBindServerTemplates
    {
      get
      {
        object o = ViewState["PreBindServerTemplates"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["PreBindServerTemplates"] = value;
      }
    }

    /// <summary>
    /// The type of client-side rendering to perform.
    /// </summary>
    /// <seealso cref="GridRenderingMode" />
    [DefaultValue(GridRenderingMode.Default)]
    [Description("The type of pager to render.")]
    public GridRenderingMode RenderingMode
    {
      get
      {
        object o = ViewState["RenderingMode"];
        return (o == null) ? GridRenderingMode.Default : (GridRenderingMode)o;
      }
      set
      {
        ViewState["RenderingMode"] = value;
      }
    }

    /// <summary>
    /// The running mode to use for this Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is one of the key configuration properties for the <see cref="Grid" />. Its value determines
    /// how the Grid performs various data-related actions such as paging, sorting, filtering, and grouping. 
    /// The following tutorials contain information on using the different running modes:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description><a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Server_Running_Mode.htm">Using Server Running Mode</a></description>
    /// </item>
    /// <item>
    /// <description><a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Client_Running_Mode.htm">Using Client Running Mode</a></description>
    /// </item>
    /// <item>
    /// <description><a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Implementing_a_Grid_Using_Callback_AJAX_Running_Mode.htm">Using CallBack Running Mode</a></description>
    /// </item>
    /// <item>
    /// <description><a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Grid_WebService_RunningMode.htm">Using WebService Running Mode</a></description>
    /// </item>
    /// </list>
    /// <para>
    /// </remarks>
    [DefaultValue(GridRunningMode.Client)]
    [Description("The running mode to use for this Grid.")]
    public GridRunningMode RunningMode
    {
      get
      {
        object o = ViewState["RunningMode"]; 
        return (o == null) ? GridRunningMode.Client : (GridRunningMode) o; 
      }
      set
      {
        ViewState["RunningMode"] = value;
      }
    }

    /// <summary>
    /// The search term to apply to the data.
    /// </summary>
    [DefaultValue("")]
    [Description("The search term to apply to the data.")]
    public string Search
    {
      get
      {
        object o = ViewState["Search"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["Search"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to the search box.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The search box is rendered by default in the header of the Grid, although its location can be altered
    /// using the <see cref="SearchBoxPosition" /> property. The header is hidden by default, and its visibility is controlled 
    /// with the <see cref="ShowHeader" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to the search box.")]
    public string SearchBoxCssClass
    {
      get
      {
        object o = ViewState["SearchBoxCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["SearchBoxCssClass"] = value;
      }
    }

    /// <summary>
    /// The position within the Grid to use for the search box.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the location within the rendered Grid to place the Search box. The two "top" positions
    /// are rendered within the header, and the "bottom" positions render within the footer. For that reason, depending on the 
    /// chosen position, <see cref="Grid.ShowHeader" /> or <see cref="Grid.ShowFooter" /> must be set to true in order to render
    /// the box. The default value for this property is <code>GridElementPosition.TopRight</code>. 
    /// </para>
    /// <para>
    /// It is worth noting that all of the following properties use the same positioning options. To prevent conflicts, 
    /// each of their positions must be taken into consideration if any of the default values are changed.
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="Grid.GroupingNotificationPosition" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.PagerPosition" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.PagerInfoPosition" />
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    [DefaultValue(GridElementPosition.TopRight)]
    [Description("The position within the Grid to use for the search box.")]
    public GridElementPosition SearchBoxPosition
    {
      get
      {
        object o = ViewState["SearchBoxPosition"];
        return o == null? GridElementPosition.TopRight : (GridElementPosition)o;
      }
      set
      {
        ViewState["SearchBoxPosition"] = value;
      }
    }

    /// <summary>
    /// Whether to perform searches every time a key is pressed inside the search box.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The efficiency of this method of searching is dependant on the current running mode (<see cref="RunningMode" />).
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Description("Whether to perform searches every time a key is pressed inside the search box.")]
    public bool SearchOnKeyPress
    {
      get
      {
        object o = ViewState["SearchOnKeyPress"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["SearchOnKeyPress"] = value;
      }
    }

    /// <summary>
    /// The minimum delay in milliseconds between searches as the user types in the search box. Default: 250.
    /// </summary>
    /// <seealso cref="SearchOnKeyPress" />
    [DefaultValue(250)]
    [Description("The minimum delay in milliseconds between searches as the user types in the search box.")]
    public int SearchOnKeyPressDelay
    {
      get
      {
        object o = ViewState["SearchOnKeyPressDelay"];
        return o == null ? 250 : (int)o;
      }
      set
      {
        ViewState["SearchOnKeyPressDelay"] = value;
      }
    }

    /// <summary>
    /// The text to render next to the search box.
    /// </summary>
    [DefaultValue("Search:")]
    [Description("The text to render next to the search box.")]
    public string SearchText
    {
      get
      {
        object o = ViewState["SearchText"];
        return o == null? "Search:" : (string)o;
      }
      set
      {
        ViewState["SearchText"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to text in the search area.
    /// </summary>
    [DefaultValue("")]
    [Description("The CssClass to apply to text in the search area.")]
    public string SearchTextCssClass
    {
      get
      {
        object o = ViewState["SearchTextCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["SearchTextCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to display the vertical scroll bar.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    /// <seealso cref="ScrollImagesFolderUrl" />
    [DefaultValue(GridScrollBarMode.Off)]
    [Category("Behavior")]
    [Description("Whether to display the vertical scroll bar.")]
    public GridScrollBarMode ScrollBar
    {
      get
      {
        object o = ViewState["ScrollBar"];
        return o == null? GridScrollBarMode.Off : (GridScrollBarMode)o;
      }
      set
      {
        ViewState["ScrollBar"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to the scroll bar.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to the scroll bar.")]
    public string ScrollBarCssClass
    {
      get
      {
        object o = ViewState["ScrollBarCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ScrollBarCssClass"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the vertical scroll bar. Default: 19.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue(19)]
    [Category("Appearance")]
    [Description("The width (in pixels) of the vertical scroll bar. Default: 19.")]
    public int ScrollBarWidth
    {
      get
      {
        object o = ViewState["ScrollBarWidth"];
        return o == null? 19 : (int)o;
      }
      set
      {
        ViewState["ScrollBarWidth"] = value;
      }
    }

    /// <summary>
    /// Whether to look for and use the scroll button active (on mouse down) images.
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to look for and use the scroll button active (on mouse down) images.")]
    public bool ScrollButtonActiveEnabled
    {
      get
      {
        object o = ViewState["ScrollButtonActiveEnabled"]; 
        return (o == null) ? false : (bool) o; 
      }
      set
      {
        ViewState["ScrollButtonActiveEnabled"] = value;
      }
    }

    /// <summary>
    /// Whether to look for and use the scroll button active (on mouse over) images.
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to look for and use the scroll button active (on mouse over) images.")]
    public bool ScrollButtonHoverEnabled
    {
      get
      {
        object o = ViewState["ScrollButtonHoverEnabled"]; 
        return (o == null) ? false : (bool) o; 
      }
      set
      {
        ViewState["ScrollButtonHoverEnabled"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of the scroll buttons.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The height (in pixels) of the scroll buttons.")]
    public int ScrollButtonHeight
    {
      get
      {
        object o = ViewState["ScrollButtonHeight"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["ScrollButtonHeight"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the scroll buttons.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The width (in pixels) of the scroll buttons.")]
    public int ScrollButtonWidth
    {
      get
      {
        object o = ViewState["ScrollButtonWidth"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["ScrollButtonWidth"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to the scroll grip.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to the scroll grip.")]
    public string ScrollGripCssClass
    {
      get
      {
        object o = ViewState["ScrollGripCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ScrollGripCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to the scroll header cell.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The CssClass to apply to the scroll header cell.")]
    public string ScrollHeaderCssClass
    {
      get
      {
        object o = ViewState["ScrollHeaderCssClass"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ScrollHeaderCssClass"] = value;
      }
    }

    /// <summary>
    /// The folder which contains images for the scroll bar.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The folder which contains images for the scroll bar.")]
    public string ScrollImagesFolderUrl
    {
      get
      {
        object o = ViewState["ScrollImagesFolderUrl"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ScrollImagesFolderUrl"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the scroll popup.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The client template specified by this property is shown when the <see cref="Grid" /> is being scrolled.
    /// The <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Client_Templates.htm">Using Client Templates in Grid</a> tutorial
    /// contains information on creating client templates. The DataItem identifier in this template will refer
    /// to either the top or bottom visible item, depending on whether the Grid is being scrolled up or down.
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information on scrolling.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for the scroll popup.")]
    public string ScrollPopupClientTemplateId
    {
      get
      {
        object o = ViewState["ScrollPopupClientTemplateId"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ScrollPopupClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of the scrollbar top and bottom images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The height (in pixels) of the scrollbar top and bottom images.")]
    public int ScrollTopBottomImageHeight
    {
      get
      {
        object o = ViewState["ScrollTopBottomImageHeight"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["ScrollTopBottomImageHeight"] = value;
      }
    }

    /// <summary>
    /// Whether to render scrollbar top and bottom images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Description("Whether to render scrollbar top and bottom images.")]
    public bool ScrollTopBottomImagesEnabled
    {
      get
      {
        object o = ViewState["ScrollTopBottomImagesEnabled"]; 
        return (o == null) ? false : (bool) o; 
      }
      set
      {
        ViewState["ScrollTopBottomImagesEnabled"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the scrollbar top and bottom images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Creating_a_Scrolling_Grid.htm">Creating a Scrolling Grid</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The width (in pixels) of the scrollbar top and bottom images.")]
    public int ScrollTopBottomImageWidth
    {
      get
      {
        object o = ViewState["ScrollTopBottomImageWidth"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set
      {
        ViewState["ScrollTopBottomImageWidth"] = value;
      }
    }

    /// <summary>
    /// The collection of selected GridItems. Read-only.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Items can be selected by the user, or programatically using one of the following methods: <see cref="Grid.Select" />,
    /// <see cref="Grid.SelectAll" />, <see cref="Grid.SelectAllKeys" />, or <see cref="Grid.SelectKey" />.
    /// </para>
    /// </remarks>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The collection of selected GridItems. Read-only.")]
    [Browsable(false)]
    public GridItemCollection SelectedItems
    {
      get
      {
        return this.GetSelectedItems();
      }
    }

    /// <summary>
    /// The collection of unique data keys of selected items. Read-only.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The unique data key for each item is the value contained in the field specified by the
    /// <see cref="GridLevel.DataKeyField" /> property.
    /// </para>
    /// </remarks>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The collection of unique data keys of selected items. Read-only.")]
    [Browsable(false)]
    public string [] SelectedKeys
    {
      get
      {
        return this.GetSelectedKeys();
      }
    }

    /// <summary>
    /// Whether the data source is self-referencing. Default: false.
    /// </summary>
    /// <remarks>
    /// Note that PreloadLevels must be set to false in order to use this functionality.
    /// </remarks>
    /// <seealso cref="PreloadLevels" />
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether the data source is self-referencing. Default: false.")]
    public bool SelfReferencing
    {
      get
      {
        object o = ViewState["SelfReferencing"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["SelfReferencing"] = value;
      }
    }

    private GridServerTemplateCollection _serverTemplates;
    /// <summary>
    /// Server templates.
    /// </summary>
    [Browsable(false)]
    [Description("Collection of GridServerTemplate controls. ")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public GridServerTemplateCollection ServerTemplates
    {
      get
      {
        if(_serverTemplates == null)
        {
          _serverTemplates = new GridServerTemplateCollection();
        }

        return _serverTemplates;
      }
    }

    /// <summary>
    /// Whether to render the Grid footer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The footer is shown by default as it is the default location of the pager, and the pager info.
    /// The location of both of those elements can be altered with the <see cref="Grid.PagerPosition" /> and <see cref="Grid.PagerInfoPosition" />
    /// properties. The pager type can be changed using the <see cref="Grid.Pager" /> property, and a new pager info template can be defined
    /// using the <see cref="Grid.PagerInfoClientTemplateId" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    [Description("Whether to render the Grid footer.")]
    public bool ShowFooter
    {
      get
      {
        object o = ViewState["ShowFooter"];
        return o == null? true : (bool)o;
      }
      set
      {
        ViewState["ShowFooter"] = value;
      }
    }

    /// <summary>
    /// Whether to render the search box.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The position of the search box is determined using the <see cref="SearchBoxPosition" /> property.
    /// The default position results in the search box being rendered in the header of the Grid, so if the default position
    /// is used, the <see cref="Grid.ShowHeader" /> property must also be set to true if the search box is to be shown.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Description("Whether to render the search box.")]
    public bool ShowSearchBox
    {
      get
      {
        object o = ViewState["ShowSearchBox"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["ShowSearchBox"] = value;
      }
    }

    /// <summary>
    /// Whether to render the Grid header.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default location of the search box and grouping notification area is in the header. In order
    /// to display either of those elements in their default location, this property must be set to true.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Description("Whether to render the Grid header.")]
    public bool ShowHeader
    {
      get
      {
        object o = ViewState["ShowHeader"];
        return o == null? false : (bool)o;
      }
      set
      {
        ViewState["ShowHeader"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the slider popup, when hovering on a cached page.
    /// </summary>
    /// <remarks>
    /// This property is used in conjunction with <see cref="CallbackCachingEnabled" />.
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for the slider popup, when hovering on a cached page.")]
    public string SliderPopupCachedClientTemplateId
    {
      get
      {
        object o = ViewState["SliderPopupCachedClientTemplateId"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["SliderPopupCachedClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the slider popup.
    /// </summary>
    /// <remarks>
    /// The DataItem provided for this client template is the first GridItem which appears on the page.
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for the slider popup.")]
    public string SliderPopupClientTemplateId
    {
      get
      {
        object o = ViewState["SliderPopupClientTemplateId"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["SliderPopupClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the slider popup when the Grid is grouped.
    /// </summary>
    /// <remarks>
    /// The DataItem provided for this client template is the first GridItem which appears on the page.
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Client_Templates.htm">Using Client Templates in Grid</a>
    /// tutorial for more information on client templates.
    /// </remarks>
    [DefaultValue("")]
    [Description("The ID of the client template to use for the slider popup when the Grid is grouped.")]
    public string SliderPopupGroupedClientTemplateId
    {
      get
      {
        object o = ViewState["SliderPopupGroupedClientTemplateId"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["SliderPopupGroupedClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The X offset (in pixels) to use when displaying the slider popup.
    /// </summary>
    [DefaultValue(0)]
    [Description("The X offset (in pixels) to use when displaying the slider popup.")]
    public int SliderPopupOffsetX
    {
      get
      {
        object o = ViewState["SliderPopupOffsetX"];
        return o == null? 0 : (int)o;
      }
      set
      {
        ViewState["SliderPopupOffsetX"] = value;
      }
    }

    /// <summary>
    /// The Y offset (in pixels) to use when displaying the slider popup.
    /// </summary>
    [DefaultValue(0)]
    [Description("The Y offset (in pixels) to use when displaying the slider popup.")]
    public int SliderPopupOffsetY
    {
      get
      {
        object o = ViewState["SliderPopupOffsetY"];
        return o == null ? 0 : (int)o;
      }
      set
      {
        ViewState["SliderPopupOffsetY"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the (often rounded) edges of the slider image.
    /// </summary>
    /// <remarks>
    /// This is the width of the areas at the beginning and end of the slider image which
    /// are not to be overlapped with the "data loaded" images when caching is enabled in
    /// callback mode. In other situations, this property is not used.
    /// </remarks>
    [DefaultValue(1)]
    [Description("The width (in pixels) of the slider grip.")]
    public int SliderEdgeWidth
    {
      get
      {
        object o = ViewState["SliderEdgeWidth"];
        return o == null ? 1 : (int)o;
      }
      set
      {
        ViewState["SliderEdgeWidth"] = value;
      }
    }

    /// <summary>
    /// The delay (in milliseconds) after which the hovered page is fetched to cache, if
    /// caching is enabled.
    /// </summary>
    /// <remarks>
    /// To disable this feature, set the delay to zero.
    /// </remarks>
    /// <seealso cref="CallbackCachingEnabled" />
    [DefaultValue(600)]
    [Description("The delay (in milliseconds) after which the hovered page is fetched to cache, if caching is enabled.")]
    public int SliderFetchDelay
    {
      get
      {
        object o = ViewState["SliderFetchDelay"];
        return o == null ? 600 : (int)o;
      }
      set
      {
        ViewState["SliderFetchDelay"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the slider grip.
    /// </summary>
    [DefaultValue(0)]
    [Description("The width (in pixels) of the slider grip.")]
    public int SliderGripWidth
    {
      get
      {
        object o = ViewState["SliderGripWidth"];
        return o == null? 0 : (int)o;
      }
      set
      {
        ViewState["SliderGripWidth"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of the slider. Default: 20.
    /// </summary>
    [DefaultValue(20)]
    [Description("The height (in pixels) of the slider. Default: 20.")]
    public int SliderHeight
    {
      get
      {
        object o = ViewState["SliderHeight"];
        return o == null? 20 : (int)o;
      }
      set
      {
        ViewState["SliderHeight"] = value;
      }
    }
   
    /// <summary>
    /// The width (in pixels) of the slider. Default: 300.
    /// </summary>
    [DefaultValue(300)]
    [Description("The width (in pixels) of the slider. Default: 300.")]
    public int SliderWidth
    {
      get
      {
        object o = ViewState["SliderWidth"];
        return o == null? 300 : (int)o;
      }
      set
      {
        ViewState["SliderWidth"] = value;
      }
    }

    /// <summary>
    /// The name of the standard SOA.UI service to use in web service mode (to be used instead of WebService/WebServiceMethod).
    /// </summary>
    /// <seealso cref="RunningMode" />
    [Category("Data")]
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
    /// The sort order (SQL ORDER BY expression) to use on the data.
    /// </summary>
    [Category("Data")]
    [DefaultValue("")]
    [Description("The sort order (SQL ORDER BY expression) to use on the data.")]
    public string Sort
    {
      get
      {
        object o = ViewState["Sort"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["Sort"] = value;
      }
    }

    /// <summary>
    /// The folder which contains the tree line images for hierarchical displays.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In a hierarchical grid, the relationships are reflected by a treeview-style structure.
    /// The images which make up the treeview should be placed into a folder at the location 
    /// specified by this property. A slight performance increase can be gained by specifying the height and width of the
    /// images using the <see cref="TreeLineImageHeight" /> and <see cref="TreeLineImageWidth" /> property respectively.
    /// </para>
    /// <para>
    /// The control looks for images with specific names. The following list contains the names of the images.
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <b>dash.gif</b> - Horizontal line image.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>dashminus.gif</b> - Horizontal line with a minus (indicating that the item can be collapsed).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>dashplus.gif</b> - Horizontal line with a plus (indicating that the item can be expanded).
    /// </description>
    /// </item>
    /// <item>

    /// <description>
    /// <b>i.gif</b> - Vertical line image.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>l.gif</b> - Vertical line image with a horizontal line intersecting at the bottom.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>lminus.gif</b> - Minus image (indicating that the item can be collapsed) with lines extending out the top and to the right.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>lplus.gif</b> - Plus image (indicating that the item can be expanded) with lines extending out the top and to the right.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>minus.gif</b> - Minus image (indicating that the item can be collapsed).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>noexpand.gif</b> - Image which indicates that the item cannot be expanded (eg. it has no children).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>plus.gif</b> - Plus image (indicating that the item has children and can be expanded).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>r.gif</b> - Vertical line image with a horizontal line intersecting at the top.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>rminus.gif</b> - Minus image (indicating that the item can be collapsed) with lines extending out the bottom and to the right.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>rplus.gif</b> - Plus image (indicating that the item has children and can be expanded) with lines extending out the bottom and to the right.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>t.gif</b> - Vertical line image with a horizontal line intersecting at the middle.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>tminus.gif</b> - Minus image (indicating that the item can be collapsed) with horizontal lines extending out the top and bottom
    /// and a vertical line extending out the right.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>tplus.gif</b> - Plus image (indicating that the item has children and can be expanded) 
    /// with horizontal lines extending out the top and bottom and a vertical line extending out the right.
    /// </description>
    /// </item>
    /// </list> 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    [Description("The folder which contains the tree line images for hierarchical displays.")]
    public string TreeLineImagesFolderUrl
    {
      get
      {
        object o = ViewState["TreeLineImagesUrl"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["TreeLineImagesUrl"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of the tree line images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In a hierarchical grid, the relationships are reflected by a treeview-style structure.
    /// The images which make up the structure should be placed into a folder which is referenced by <see cref="TreeLineImagesFolderUrl" />
    /// property. This property is used to specify the height of the images. The <see cref="TreeLineImageWidth" /> property
    /// can be used to specify the width of the images. Setting these properties can result in a small 
    /// performance gain.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The height (in pixels) of the tree line images.")]
    public int TreeLineImageHeight
    {
      get
      {
        object o = ViewState["TreeLineImageHeight"];
        return o == null? 0 : (int)o;
      }
      set
      {
        ViewState["TreeLineImageHeight"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the tree line images.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In a hierarchical grid, the relationships are reflected by a treeview-style structure.
    /// The images which make up the structure should be placed into a folder which is referenced by <see cref="TreeLineImagesFolderUrl" />
    /// property. This property is used to specify the width of the images. The <see cref="TreeLineImageHeight" /> property
    /// can be used to specify the height of the images. Setting these properties can result in a small 
    /// performance gain.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    [Description("The width (in pixels) of the tree line images.")]
    public int TreeLineImageWidth
    {
      get
      {
        object o = ViewState["TreeLineImageWidth"];
        return o == null? 0 : (int)o;
      }
      set
      {
        ViewState["TreeLineImageWidth"] = value;
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

    /// <summary>
    /// Whether to only bind to properties defined explicitly in the type of the objects being provided.
    /// </summary>
    [DefaultValue(false)]
    [Category("Data")]
    [Description("Whether to only bind to properties defined explicitly in the type of the objects being provided.")]
    public bool UseShallowObjectBinding
    {
      get
      {
        object o = ViewState["UseShallowObjectBinding"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["UseShallowObjectBinding"] = value;
      }
    }

    /// <summary>
    /// The name of the ASP.NET AJAX web service to use for WebService running mode.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The web service specified by this property must be registered with the script manager on the page. The following
    /// tutorials contain information on web service integration with Web.UI controls:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Introduction.htm">Web Services with Web.UI Controls</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_WebServiceCustomParameter.htm">Using the WebServiceCustomParameter</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Grid_WebService_RunningMode.htm">An Introduction to WebService Running Mode in Grid</a>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="RunningMode" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service to use for WebService running mode.")]
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
    /// Whether to enable caching of data in webservice mode.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in WebService <see cref="RunningMode" />. In callback running mode, 
    /// the <see cref="CallbackCachingEnabled" /> property accomplishes the same task. See the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Grid_WebService_RunningMode.htm">Using WebService Running Mode</a>
    /// tutorial for information on creating a basic Grid in WebService running mode.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    [Category("Data")]
    [Description("Whether to enable caching of data in webservice mode.")]
    public bool WebServiceCachingEnabled
    {
      get
      {
        object o = ViewState["WebServiceCachingEnabled"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["WebServiceCachingEnabled"] = value;
      }
    }

    /// <summary>
    /// The name of the method to use for fetching configuration info in WebService running mode.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The method specified by this property can be used to configure various properties of the Grid through a web service call.
    /// At this point, the method can only be used to set basic properties, although in a future release, it will be
    /// able to perform more advanced Grid configuration including <see cref="GridLevel">Level</see> definitions.
    /// </para>
    /// </remarks>
    /// <seealso cref="RunningMode" />
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the method to use for fetching configuration info in WebService running mode.")]
    public string WebServiceConfigMethod
    {
      get
      {
        object o = ViewState["WebServiceConfigMethod"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["WebServiceConfigMethod"] = value;
      }
    }

    /// <summary>
    /// The (optional) custom parameter to send with each web service request.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The value of this property is included in the request object for every web service call. 
    /// This property is accessible from both the client and the server. 
    /// </para>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_WebServiceCustomParameter.htm">Using the WebServiceCustomParameter</a>
    /// tutorial for more information. 
    /// </para>
    /// </remarks>
    /// <seealso cref="RunningMode" />
    /// <seealso cref="WebServiceSelectMethod" />
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
    /// The name of the method to use for deleting records in WebService running mode.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The method specified by this property is used to delete a record from the underlying data source.
    /// The method must be contained within the web service specified by the <see cref="Grid.WebService" /> property.
    /// This method must be called explicitly from the client-side by passing a <see cref="GridItem" /> object
    /// into the <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_class.htm">Grid</a> 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_webServiceDelete_method.htm">webServiceDelete</a>
    /// method.
    /// </para>
    /// <para>
    /// The following tutorials provide more information on using a Grid in web service running mode:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Introduction.htm">Web Services with Web.UI Controls</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_WebServiceCustomParameter.htm">Using the WebServiceCustomParameter</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Grid_WebService_RunningMode.htm">An Introduction to WebService Running Mode in Grid</a>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// The following properties are used to specify methods to use for the other CRUD operations:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceInsertMethod" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceSelectMethod" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceUpdateMethod" />
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="RunningMode" />
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the method to use for deleting records in WebService running mode.")]
    public string WebServiceDeleteMethod
    {
      get
      {
        object o = ViewState["WebServiceDeleteMethod"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["WebServiceDeleteMethod"] = value;
      }
    }

    /// <summary>
    /// The name of the method to use for retrieving groups in WebService running mode.
    /// </summary>
    /// <seealso cref="RunningMode" />
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the method to use for retrieving groups in WebService running mode.")]
    public string WebServiceGroupMethod
    {
      get
      {
        object o = ViewState["WebServiceGroupMethod"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["WebServiceGroupMethod"] = value;
      }
    }

    /// <summary>
    /// The name of the method to use for inserting records in WebService running mode.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The method specified by this property is used to insert a new record into the underlying data source.
    /// The method must be contained within the web service defined by the <see cref="Grid.WebService" /> property.
    /// This method must be called explicitly by passing a <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~GridItem_class.htm">GridItem</a>
    /// object into the <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_class.htm">Grid</a>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_webServiceInsert_method.htm">webServiceInsert</a>
    /// method.
    /// </para>
    /// <para>
    /// The following tutorials provide more information on using a Grid in web service running mode:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Introduction.htm">Web Services with Web.UI Controls</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_WebServiceCustomParameter.htm">Using the WebServiceCustomParameter</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Grid_WebService_RunningMode.htm">An Introduction to WebService Running Mode in Grid</a>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// The following properties are used to specify methods to use for the other CRUD operations:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceSelectMethod" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceUpdateMethod" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceDeleteMethod" />
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="RunningMode" />
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the method to use for inserting records in WebService running mode.")]
    public string WebServiceInsertMethod
    {
      get
      {
        object o = ViewState["WebServiceInsertMethod"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["WebServiceInsertMethod"] = value;
      }
    }

    /// <summary>
    /// The name of the method to use for fetching records in WebService running mode.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The method specified by this property is called when the Grid is in web service running mode and requires
    /// data. This is the method which is responsible for paging, sorting, and filtering. The method must be
    /// contained within the web service specified by the <see cref="Grid.WebService" /> property.
    /// </para>
    /// <para>
    /// This method is called automatically when in situations where it is needed, or it can be called directly
    /// using the
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_webServiceSelect_method.htm">webServiceSelect</a>
    /// method. Note that when this method is called, the current value of the
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_webServiceCustomParameter_property.htm">webServiceCustomParameter</a>
    /// is passed to the web service, allowing the returned data to be further customized.
    /// </para>
    /// <para>
    /// The following tutorials provide more information on using a Grid in web service running mode:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Introduction.htm">Web Services with Web.UI Controls</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_WebServiceCustomParameter.htm">Using the WebServiceCustomParameter</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Grid_WebService_RunningMode.htm">An Introduction to WebService Running Mode in Grid</a>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <para> 
    /// The following properties are used to specify methods to use for the other CRUD operations:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceInsertMethod" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceUpdateMethod" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceDeleteMethod" />
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="RunningMode" />
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the method to use for fetching records in WebService running mode.")]
    public string WebServiceSelectMethod
    {
      get
      {
        object o = ViewState["WebServiceSelectMethod"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["WebServiceSelectMethod"] = value;
      }
    }

    /// <summary>
    /// The name of the method to use for updating records in WebService running mode.
    /// </summary>
     /// <remarks>
    /// <para>
    /// The method specified by this property is used to update the underlying data source when a <see cref="GridItem" />
    /// has been edited. As with the other WebService* methods, this method is used while the <see cref="Grid" /> is in WebService
    /// <see cref="Grid.RunningMode" />. This method is called automatically by the Grid when an item has been edited, or it can
    /// be called directly from code by passing a 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~GridItem_class.htm">GridItem</a> object
    /// into the client-side 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~Grid_webServiceUpdate_method.htm">webServiceUpdate</a>
    /// method.
    /// </para>
    /// <para>
    /// The following tutorials provide more information on using a Grid in web service running mode:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Introduction.htm">Web Services with Web.UI Controls</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_WebServiceCustomParameter.htm">Using the WebServiceCustomParameter</a>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_Grid_WebService_RunningMode.htm">An Introduction to WebService Running Mode in Grid</a>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <para> 
    /// The following properties are used to specify methods to use for the other CRUD operations:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceInsertMethod" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceSelectMethod" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Grid.WebServiceDeleteMethod" />
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="RunningMode" />
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the method to use for updating records in WebService running mode.")]
    public string WebServiceUpdateMethod
    {
      get
      {
        object o = ViewState["WebServiceUpdateMethod"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["WebServiceUpdateMethod"] = value;
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
        this.CssClass = prefix + "datagrid";
      }
      if ((this.GroupByCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.GroupByCssClass = prefix + "datagrid-groupby";
      }
      if ((this.GroupByTextCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.GroupByTextCssClass = prefix + "datagrid-text";
      }
      if ((this.GroupBySectionCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.GroupBySectionCssClass = prefix + "datagrid-groupby-section";
      }
      if ((this.GroupBySectionSeparatorCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.GroupBySectionSeparatorCssClass = prefix + "datagrid-groupby-separator";
      }
      if ((this.GroupingNotificationTextCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.GroupingNotificationTextCssClass = prefix + "datagrid-grouping-text";
      }
      if ((this.IndentCellCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.IndentCellCssClass = prefix + "datagrid-indent";
      }
      if ((this.DataAreaCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.DataAreaCssClass = prefix + "datagrid-content";
      }
      if ((this.HeaderCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.HeaderCssClass = prefix + "datagrid-header";
      }
      if ((this.FooterCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.FooterCssClass = prefix + "datagrid-footer";
      }
      if ((this.PagerTextCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.PagerTextCssClass = prefix + "datagrid-pager-text";
      }
      if (ViewState["ScrollButtonHeight"] == null || overwrite)
      {
        this.ScrollButtonHeight = 17;
      }
      if ((this.ScrollBarCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.ScrollBarCssClass = prefix + "datagrid-scroll";
      }
      if ((this.ScrollHeaderCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.ScrollHeaderCssClass = prefix + "datagrid-scroll-header";
      }
      if ((this.ScrollGripCssClass ?? string.Empty) == string.Empty || overwrite)
      {
        this.ScrollGripCssClass = prefix + "datagrid-scroll-grip-middle";
      }
      if (ViewState["ScrollTopBottomImageHeight"] == null || overwrite)
      {
        this.ScrollTopBottomImageHeight = 4;
      }      
      if (ViewState["PagerStyle"] == null || overwrite)
      {
        this.PagerStyle = GridPagerStyle.Slider;
      }
      if (ViewState["PagerButtonHoverEnabled"] == null || overwrite)
      {
        this.PagerButtonHoverEnabled = true;
      }
      if (ViewState["PagerButtonWidth"] == null || overwrite)
      {
        this.PagerButtonWidth = 30;
      }
      if (ViewState["PagerButtonHeight"] == null || overwrite)
      {
        this.PagerButtonHeight = 23;
      }
      if (ViewState["SliderHeight"] == null || overwrite)
      {
        this.SliderHeight = 30;
      }
      if (ViewState["SliderWidth"] == null || overwrite)
      {
        this.SliderWidth = 160;
      }
      if (ViewState["SliderGripWidth"] == null || overwrite)
      {
        this.SliderGripWidth = 12;
      }
      if (ViewState["SliderPopupOffsetX"] == null || overwrite)
      {
        this.SliderPopupOffsetX = 140;
      }
      if (ViewState["SliderPopupOffsetY"] == null || overwrite)
      {
        this.SliderPopupOffsetY = 12;
      }
      if (ViewState["LoadingPanelFadeDuration"] == null || overwrite)
      {
        this.LoadingPanelFadeDuration = 1000;
      }
      if (ViewState["LoadingPanelFadeMaximumOpacity"] == null || overwrite)
      {
        this.LoadingPanelFadeMaximumOpacity = 60;
      }
      if (ViewState["LoadingPanelClientTemplateId"] == null || overwrite)
      {
        this.LoadingPanelClientTemplateId = "LoadingFeedbackTemplate";
      }
      if (ViewState["LoadingPanelPosition"] == null || overwrite)
      {
        this.LoadingPanelPosition = GridRelativePosition.MiddleCenter;
      }
      if (ViewState["TreeLineImageWidth"] == null || overwrite)
      {
        this.TreeLineImageWidth = 11;
      }
      if (ViewState["TreeLineImageHeight"] == null || overwrite)
      {
        this.TreeLineImageHeight = 11;
      }
      if (ViewState["IndentCellWidth"] == null || overwrite)
      {
        this.IndentCellWidth = 16;
      }
      if (ViewState["ScrollPopupClientTemplateId"] == null || overwrite)
      {
        this.ScrollPopupClientTemplateId = "ScrollerPopupTemplate";
      }
      if (ViewState["SliderPopupClientTemplateId"] == null || overwrite)
      {
        this.SliderPopupClientTemplateId = "SliderPopupTemplate";
      }
      if (ViewState["SliderPopupCachedClientTemplateId"] == null || overwrite)
      {
        this.SliderPopupCachedClientTemplateId = "SliderPopupCachedTemplate";
      }
      if (ViewState["SliderPopupGroupedClientTemplateId"] == null || overwrite)
      {
        this.SliderPopupGroupedClientTemplateId = "SliderPopupGroupedTemplate";
      }

      // GridLevel
      foreach (GridLevel gridLevel in this.Levels)
      {
        if ((gridLevel.RowCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.RowCssClass = prefix + "datagrid-row";
        }
        if ((gridLevel.HoverRowCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.HoverRowCssClass = prefix + "datagrid-row-hover";
        }
        if ((gridLevel.SelectedRowCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.SelectedRowCssClass = prefix + "datagrid-row " + prefix + "datagrid-row-selected";
        }
        if ((gridLevel.DataCellCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.DataCellCssClass = prefix + "datagrid-cell";
        }
        if ((gridLevel.SelectorCellCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.SelectorCellCssClass = prefix + "datagrid-selector-cell";
        }
        if ((gridLevel.HeadingCellCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.HeadingCellCssClass = prefix + "datagrid-heading-cell";
        }
        if ((gridLevel.HeadingCellHoverCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.HeadingCellHoverCssClass = prefix + "datagrid-heading-cell-hover";
        }
        if ((gridLevel.HeadingRowCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.HeadingRowCssClass = prefix + "datagrid-heading-row";
        }
        if ((gridLevel.HeadingSelectorCellCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.HeadingSelectorCellCssClass = prefix + "datagrid-heading-cell";
        }
        if (gridLevel.SelectorCellWidth == 0 || overwrite)
        {
          gridLevel.SelectorCellWidth = 16;
        }   
        if ((gridLevel.HeadingTextCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.HeadingTextCssClass = prefix + "datagrid-heading-text";
        }
        if ((gridLevel.SortedHeadingCellCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.SortedHeadingCellCssClass = prefix + "sort";
        }        
        if ((gridLevel.GroupHeadingCssClass ?? string.Empty) == string.Empty || overwrite)
        {
          gridLevel.GroupHeadingCssClass = prefix + "datagrid-group-heading";
        }
        
        // GridColumns
        foreach (GridColumn gridColumn in gridLevel.Columns)
        {
          // Hwan 2010-01-07: doesn't seem to be a need to style these in the theme
        }
      }

      // Client Templates
      StringBuilder templateText = new StringBuilder();
      templateText.Append(@"<table height=""## document.getElementById('");
      templateText.Append(this.ClientObjectId);
      templateText.Append(@"').offsetHeight ##"" width=""## document.getElementById('");
      templateText.Append(this.ClientObjectId);
      templateText.Append(@"').offsetWidth ##"" bgcolor=""#e0e0e0"">
        <tr><td valign=""center"" align=""center"">
          <table cellspacing=""0"" cellpadding=""0"" border=""0"">
            <tr>
              <td>&nbsp;</td>
           </tr>
          </table>
        </td></tr>
      </table>
");
      AddClientTemplate(overwrite, "LoadingFeedbackTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append(@"<div class=""" + this.AutoThemingCssClassPrefix + @"datagrid-slider-popup"">
      	<div class=""" + this.AutoThemingCssClassPrefix + @"datagrid-slider-popup-content"">
		      <span>Page <strong>## DataItem.PageIndex + 1 ##</strong> of <strong>## " + this.ClientObjectId + @".PageCount ##</strong></span>
	      </div>
      </div>
");
      AddClientTemplate(overwrite, "SliderPopupTemplate", templateText.ToString());
      AddClientTemplate(overwrite, "SliderPopupCachedTemplate", templateText.ToString());
      AddClientTemplate(overwrite, "SliderPopupGroupedTemplate", templateText.ToString());

      templateText = new StringBuilder();
      templateText.Append(@"<div class=""" + this.AutoThemingCssClassPrefix + @"datagrid-scroll-popup"">
	      <div class=""" + this.AutoThemingCssClassPrefix + @"datagrid-scroll-popup-content"">
		      <span>Page <strong>## DataItem.PageIndex + 1 ##</strong> of <strong>## " + this.ClientObjectId + @".PageCount ##</strong></span>
	      </div>
      </div>      	
");
      AddClientTemplate(overwrite, "ScrollerPopupTemplate", templateText.ToString());
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
    /// Collapse the given item on the given level (if it is expanded).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to collapse an item in a hierarchical grid. If a grid possesses only a single level of expandable items, the 
    /// other version of this method can be used (
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~Collapse(GridItem).html">Collapse</a>).
    /// To collapse all expanded items, use the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~CollapseAll.html">CollapseAll</a> method.
    /// Expanding and collapsing of a GridItem is obviously dependent on whether the GridItem's <see cref="GridItem.Items" /> property has been populated.
    /// If the item has no children, or the item could not be collapsed for some other reason, this method will return false.
    /// </para>
    /// <para>
    /// To programatically expand an item, use the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~Expand.html">Expand</a> 
    /// method. 
    /// </para>
    /// </remarks>
    /// <param name="oItem">The GridItem to collapse.</param>
    /// <param name="iLevel">The index of the level on which the GridItem is.</param>
    /// <returns>Whether the item could be collapsed.</returns>
    public bool Collapse(GridItem oItem, int iLevel)
    {
      if(oItem.Items.Count > 0 && Levels.Count > iLevel && Levels[iLevel].DataKeyField != string.Empty)
      {
        object oKey = oItem[Levels[iLevel].DataKeyField];

        string sListEntry = string.Format("{0} {1}", iLevel, oKey);

        if(this.ExpandedList.Contains(sListEntry))
        {
          this.ExpandedList.Remove(sListEntry);
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Collapse the given item on the top level (if it is expanded).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to collapse a single item which is on the top level (Grid.Levels[0]). 
    /// It is used for non-hierarchical grids, or when there is only a single level of inheritance. There is also an overloaded version of this method
    /// (<a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~Collapse(GridItem,Int32).html">Collapse</a>)
    /// which accepts the level number of the item, as well. To collapse all expanded items, use the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~CollapseAll.html">CollapseAll</a> method.
    /// Expanding and collapsing of a GridItem is obviously dependent on whether the GridItem's <see cref="GridItem.Items" /> property has been populated.
    /// If the item has no children, or the item could not be collapsed for some other reason, this method will return false.
    /// </para>
    /// <para>
    /// To programatically expand an item, use the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~Expand.html">Expand</a> 
    /// method. 
    /// </para>
    /// </remarks>
    /// <param name="oItem">The GridItem to collapse.</param>
    /// <returns>Whether the item could be collapse.</returns>
    public bool Collapse(GridItem oItem)
    {
      return this.Collapse(oItem, 0);
    }

    /// <summary>
    /// Collapse all expanded items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method will collapse all expanded items, regardless of the level they are on. To collapse a single expanded GridItem, use the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~Collapse.html">Collapse</a> method.
    /// To expand all items, use one of the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~ExpandAll.html">ExpandAll</a> methods.
    /// </para>
    /// </remarks>
    public void CollapseAll()
    {
      this.ExpandedList.Clear();
    }

    /// <summary>
    /// Collapse the group at the given path in the current grouping.
    /// </summary>
    /// <param name="sGroupPath">The path string of the group to expand.</param>
    public void CollapseGroup(string sGroupPath)
    {
      ExpandedGroupInfo.Remove(sGroupPath);
    }

    /// <summary>
    /// Bind to the set DataSource.
    /// </summary>
    /// <remarks>
    /// This method is used to bind the grid to a datasource. The datasource is specified using the <see cref="DataSource" /> property, 
    /// and must be set before this method is called. 
    /// </remarks>
    /// <seealso cref="DataSource" />
    public override void DataBind()
    {
      string sGridVar = this.GetSaneId();
      
      // Mark whether this is a callback request
      if(Context.Request.QueryString[string.Format("Cart_{0}_Callback", sGridVar)] != null)
      {
        this.RunningMode = GridRunningMode.Callback;
      }
      
      // Clear loaded data.
      Items.Clear();

      // do we need server-side group loading?
      if (this.NeedServerGroups)
      {
        // in that case, we won't be doing direct data loading

        if (this.NeedGroups != null && this.NeedGroupData != null)
        {
          // get groupings
          string[] arGroupings = this.GroupBy.Split(',');
          for (int i = 0; i < arGroupings.Length; i++)
          {
            // fix up each grouping to make sure it's in a good format
            arGroupings[i] = arGroupings[i].Trim();
          }

          // default offset (all groups collapsed)
          int iOffset = this.RecordOffset > 0? this.RecordOffset : this.CurrentPageIndex * this.GroupingPageSize;
          int iCount = this.GroupingPageSize;
          int iSkip = 0;

          int iRecordCountAdjustment = 0;

          // we need to adjust for rendered rows?
          if (this.GroupingMode == GridGroupingMode.ConstantRows)
          {
            // get list of sorted top-level keys
            ArrayList oSortedKeys = new ArrayList();
            foreach (string sKey in this.ExpandedGroupInfo.Keys)
            {
              if (sKey.IndexOf("_") < 0)
              {
                // top-level key, add it to numerically sorted list
                oSortedKeys.Add(Convert.ToInt32(sKey));
              }
            }
            oSortedKeys.Sort();

            // Go through groups on previous pages
            foreach(object oSortedKey in oSortedKeys)
            {
              ServerGroup oExpandedGroup = (ServerGroup)this.ExpandedGroupInfo[oSortedKey.ToString()];

              // before current page?
              if (oExpandedGroup.Index < iOffset)
              {
                // does this group push into the visible page
                int endOffset = oExpandedGroup.Index + oExpandedGroup.RenderCount + 1;
                if (endOffset > iOffset)
                {
                  // we'll have to start rendering mid-group
                  iSkip = oExpandedGroup.RenderCount - (endOffset - iOffset);
                  iOffset = oExpandedGroup.Index;
                }
                else if (endOffset == iOffset)
                {
                  iSkip = 0;
                  iOffset = oExpandedGroup.Index + 1;
                }
                else
                {
                  iOffset -= oExpandedGroup.RenderCount;
                }
              }

              iRecordCountAdjustment += oExpandedGroup.RenderCount;
            }
          }

          // Get all potential groups for the current page
          GridNeedGroupsEventArgs oGroupRequestArgs = new GridNeedGroupsEventArgs();
          oGroupRequestArgs.Offset = iOffset;
          oGroupRequestArgs.Count = iCount;
          oGroupRequestArgs.GroupColumn = SplitFieldAndDirection(arGroupings[0], "")[0];
          oGroupRequestArgs.GroupDirection = SplitFieldAndDirection(arGroupings[0], "ASC")[1]; 
          this.OnNeedGroups(oGroupRequestArgs);

          // set total count
          this.RecordCount = oGroupRequestArgs.TotalCount + iRecordCountAdjustment;

          this.ServerGroups.Clear();

          // now we have all the top-level groups we need, load them up
          foreach (object oGroupValue in oGroupRequestArgs.Groups)
          {
            // if this is an expanded group, store its value, we might need it for loading
            int iServerGroupIndex = this.ServerGroups.Count;

            ServerGroup oGroup = new ServerGroup(oGroupValue);
            oGroup.Index = iServerGroupIndex + iOffset;
            oGroup.Path = oGroup.Index.ToString(); // top-level paths

            this.ServerGroups.Add(oGroup);
          }

          // Go through groups on the current page, loading data where needed
          int iGroups = this.ServerGroups.Count;
          for(int i = 0; i < iGroups; i++)
          {
            ServerGroup oGroup = (ServerGroup)this.ServerGroups[i];
            if (this.ExpandedGroupInfo.ContainsKey(oGroup.Path))
            {
              if (oGroup.Index >= iOffset && oGroup.Index < iOffset + iCount)
              {
                this.LoadServerGroupContents(oGroup, iSkip, arGroupings, 0);

                if (iSkip > 0)
                {
                  // Mark group list as continued
                  _serverGroupsContinued = true;

                // No more skipping
                iSkip = 0;
                }

                // Adjust count
                iCount -= oGroup.RenderCount;
              }
              // beyond current page - not interested anymore
              else
              {
                break;
              }
            }
          }

          // we'll possibly need to continue in order to count all the groups/records
        }
        else
        {
          throw new InvalidOperationException("NeedGroups and NeedGroupData events must be hooked if server-side grouping if manual paging is enabled with the current RunningMode and GroupingMode settings.");
        }
      }
      else
      {
        // regular binding
        this.RecordCount = 0;


        // Convert the data source control into disconnected data we can bind with
        if (_dataSource == null && DataSourceID != "")
        {
          Control oDS = Utils.FindControl(this, this.DataSourceID);

          if (oDS == null)
          {
            throw new Exception("Data source control '" + this.DataSourceID + "' not found.");
          }

          if (oDS is IDataSource)
          {
            IDataSource oDSC = (IDataSource)oDS;
            DataSourceView oView = oDSC.GetView("");

            oView.Select(DataSourceSelectArguments.Empty, new DataSourceViewSelectCallback(this.DataBindToEnumerable));

            // do no more
            return;
          }
          else if (oDS is IListSource)
          {
            IListSource oDSC = (IListSource)oDS;
            _dataSource = oDSC.GetList();
          }
          else
          {
            throw new Exception("Data source control must implement IDataSource or IListSource.");
          }
        }


        // DataSource isn't set? Fire an event
        if (_dataSource == null)
        {
          this.OnNeedDataSource(EventArgs.Empty);
        }

        // Set initial sort indicators, even if we have no data
        if (this.Sort != string.Empty && this.Levels.Count > 0)
        {
          string[] arSortParams = SplitFieldAndDirection(this.Sort, "ASC");
          this.Levels[0].IndicatedSortColumn = arSortParams[0];
          this.Levels[0].IndicatedSortDirection = arSortParams[1];
        }

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
              if (this.Levels.Count > 0 && this.Levels[0].DataMember != string.Empty)
              {
                _dataView = ((DataSet)_dataSource).Tables[this.Levels[0].DataMember].DefaultView;
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

            this.DataBindToDataView(_dataView);
          }
          else if (_dataSource is IEnumerable)
          {
            this.DataBindToEnumerable((IEnumerable)_dataSource);
          }
          else
          {
            throw new Exception("Cannot bind to data source of type " + _dataSource.GetType().ToString());
          }
        }
      }

      this.InstantiateServerTemplates();

      base.DataBind();

      _dataBound = true;
    }

    /// <summary>
    /// Expand the given item on the given level (if it has child items).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to expand a GridItem in a hierarchical grid. In a non-hierarchical grid (only one 
    /// <see cref="GridLevel">level</see>), the other version
    /// of this method can be used (
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~Expand(GridItem).html">Expand</a>).
    /// To collapse a single item, use the 
    /// <see cref="Grid.Collapse">Collapse</see> method.
    /// Expanding and collapsing of a GridItem is obviously dependent on whether the GridItem's <see cref="GridItem.Items" /> property has been populated.
    /// If the item has no children, or the item could not be expanded for some other reason, this method will return false.
    /// </para>
    /// <para>
    /// To expand all items in a grid, or on a given level, one of the 
    /// <see cref="Grid.ExpandAll">ExpandAll</see> 
    /// methods can be used.
    /// </para>
    /// </remarks>
    /// <param name="oItem">The GridItem to expand.</param>
    /// <param name="iLevel">The level on which the GridItem can be found.</param>
    /// <returns>Whether the item could be expanded.</returns>
    public bool Expand(GridItem oItem, int iLevel)
    {
      if(oItem.Items.Count > 0 && Levels.Count > iLevel && Levels[iLevel].DataKeyField != string.Empty)
      {
        object oKey = oItem[Levels[iLevel].DataKeyField];

        this.ExpandedList.Add(string.Format("{0} {1}", iLevel, oKey));

        return true;
      }

      return false;
    }

    /// <summary>
    /// Expand the given item on the top level (if it has child items).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to expand a <see cref="GridItem" /> on the top <see cref="GridLevel">level</see> of a grid (Grid.Levels[0]).
    /// To expand an item on a lower level in a hierarchical grid, the other version of this method must be used (
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~Expand(GridItem,Int32).html">Expand</a>).
    /// Expanding and collapsing of a GridItem is obviously dependent on whether the GridItem's <see cref="GridItem.Items" /> property has been populated.
    /// If the item has no children, or the item could not be expanded for some other reason, this method will return false.
    /// </para>
    /// <para>
    /// To collapse an item which has been expanded, use the <see cref="Grid.Collapse" /> method.
    /// </para>
    /// </remarks>
    /// <param name="oItem">The GridItem to expand.</param>
    /// <returns>Whether the item could be expanded.</returns>
    public bool Expand(GridItem oItem)
    {
      return this.Expand(oItem, 0);
    }

    /// <summary>
    /// Expand all items in the hierarchical Grid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to expand all <see cref="GridItem">GridItems</see> in a grid, regardless of the level they are on. The other version
    /// of this method allows you to specify a level, limiting the depth to which the grid will be expanded (
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~ExpandAll(Int32).html">ExpandAll</a>).
    /// To expand a single GridItem, use the <see cref="Grid.Expand" /> method.
    /// </para>
    /// <para>
    /// To collapse all items in a Grid, use the <see cref="Grid.CollapseAll" /> method.
    /// </para>
    /// </remarks>
    /// <returns></returns>
    public void ExpandAll()
    {
      this.ExpandAll(int.MaxValue);
    }

    /// <summary>
    /// Expand all items in the hierarchical Grid down to the given depth.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method allows all items in a grid to be expanded down to a specific level. This behavior differs from the other version of this method which
    /// expands all items in the grid (
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.Server/ComponentArt.Web.UI~ComponentArt.Web.UI.Grid~ExpandAll().html">ExpandAll</a>).
    /// To expand a single GridItem, use the <see cref="Grid.Expand" /> method.
    /// </para>
    /// <para>
    /// To collapse all items in a Grid, use the <see cref="Grid.CollapseAll" /> method.
    /// </para>
    /// </remarks>
    /// <param name="iDepth">How many levels deep to pre-expand.</param>
    /// <returns></returns>
    public void ExpandAll(int iDepth)
    {
      this.ExpandAllRecursive(this.Items, 0, iDepth);
    }

    /// <summary>
    /// Expand all items on all levels of this Grid.
    /// </summary>
    /// <returns></returns>
    private void ExpandAllRecursive(GridItemCollection arItems, int iLevel, int iMaxLevel)
    {
      if(iLevel < iMaxLevel)
      {
        foreach(GridItem oItem in arItems)
        {
          if(oItem.Items.Count > 0)
          {
            this.Expand(oItem, iLevel);

            ExpandAllRecursive(oItem.Items, iLevel + 1, iMaxLevel);
          }
        }
      }
    }

    /// <summary>
    /// Expand the group at the given path in the current grouping.
    /// </summary>
    /// <param name="sGroupPath">The path string of the group to expand.</param>
    public void ExpandGroup(string sGroupPath)
    {
      ServerGroup oGroup = new ServerGroup();
      oGroup.Path = sGroupPath;
      oGroup.Index = int.Parse(sGroupPath.Split('_')[0]);

      if (!ExpandedGroupInfo.ContainsKey(sGroupPath))
      {
        ExpandedGroupInfo.Add(sGroupPath, oGroup);
      }
    }

    /// <summary>
    /// Look for a control within the server-side template instance for the given item,
    /// under the given column at the given level.
    /// </summary>
    /// <param name="iLevelIndex">Index of the level.</param>
    /// <param name="iColumnIndex">Index of the column.</param>
    /// <param name="oItem">The GridItem to search in.</param>
    /// <param name="sControlId">ID of control to find.</param>
    /// <returns></returns>
    public Control FindControl(int iLevelIndex, int iColumnIndex, GridItem oItem, string sControlId)
    {
      int iIndex = 0;

      // data key field on top level?
      if(iLevelIndex == 0 && this.Levels.Count > 0)
      {
        if(this.Levels[0].DataKeyField != "")
        {
          int iKeyColumn = this.Levels[0].Columns.IndexOf(this.Levels[0].DataKeyField);

          object oKey = oItem[iKeyColumn];

          int iIndexCount = 0;

          foreach(GridItem item in this.Items)
          {
            if(item[iKeyColumn] == oKey)
            {
              iIndex = iIndexCount;
              break;
            }

            iIndexCount++;
          }
        }
        else
        {
          iIndex = this.Items.IndexOf(oItem);
        }
      }

      string sTemplateId = GetTemplateId(iLevelIndex, iColumnIndex, iIndex, oItem);

      //Context.Response.Write("Searching: " + sTemplateId + "<BR>");

      Control oContainer = this.FindControl(sTemplateId);
      if(oContainer != null)
      {
        //Context.Response.Write("Looking inside " + sTemplateId + " for " + sControlId + "<BR>");

        return oContainer.FindControl(sControlId);
      }

      return null;
    }

    /// <summary>
    /// Look for a control within the server-side template instance for the given item,
    /// under the given column at the given level.
    /// </summary>
    /// <param name="iLevelIndex">Index of the level.</param>
    /// <param name="iColumnIndex">Index of the column.</param>
    /// <param name="iItemIndex">Index of the item.</param>
    /// <param name="sControlId">ID of control to find.</param>
    /// <returns></returns>
    public Control FindControl(int iLevelIndex, int iColumnIndex, int iItemIndex, string sControlId)
    {
      string sTemplateId = GetTemplateId(iLevelIndex, iColumnIndex, iItemIndex, this.Items[iItemIndex]);

      Control oContainer = this.FindControl(sTemplateId);
      if(oContainer != null)
      {
        return oContainer.FindControl(sControlId);
      }

      return null;
    }
    
    public override Control FindControl(string sId)
    {
      Control c = base.FindControl(sId);

      if(c == null)
      {
        foreach(Control oControl in Controls)
        {
          Control control = oControl.FindControl(sId);
          if(control != null)
          {
            return control;
          }
        }
      }

      return c;
    }

    /// <summary>
    /// Look for a control within the server-side template instance for the given item,
    /// under the given column.
    /// </summary>
    /// <param name="iColumnIndex">Index of the column.</param>
    /// <param name="iItemIndex">Index of the item.</param>
    /// <param name="sControlId">ID of control to find.</param>
    /// <returns></returns>
    public Control FindControl(int iColumnIndex, int iItemIndex, string sControlId)
    {
      string sTemplateId = GetTemplateId(0, iColumnIndex, iItemIndex, this.Items[iItemIndex]);

      Control oContainer = this.FindControl(sTemplateId);
      if(oContainer != null)
      {
        return oContainer.FindControl(sControlId);
      }

      return null;
    }

    private void GetCheckedItems(GridItemCollection arInItems, GridItemCollection arOutItems, int iColumnIndex)
    {
      foreach(GridItem oItem in arInItems)
      {
        object oValue = oItem[iColumnIndex];

        if( Object.Equals(oValue, true) ||
          (oValue != null && oValue.ToString().ToLower() == Boolean.TrueString.ToLower() ))
        {
          arOutItems.Add(oItem);
        }
      }
    }

    private void GetCheckedItems(int iLevel, int iInLevel, GridItemCollection arInItems, GridItemCollection arOutItems, int iColumnIndex)
    {
      if(iLevel > iInLevel)
      {
        foreach(GridItem oItem in arInItems)
        {
          GetCheckedItems(iLevel, iInLevel + 1, oItem.Items, arOutItems, iColumnIndex);
        }
      }
      else
      {
        GetCheckedItems(arInItems, arOutItems, iColumnIndex);
      }
    }

    public GridItemCollection GetCheckedItems(GridLevel oLevel, GridColumn oColumn)
    {
      if(oColumn.ColumnType != GridColumnType.CheckBox)
      {
        return null;
      }

      GridItemCollection arItems = new GridItemCollection();

      int iLevel = Levels.IndexOf(oLevel);

      if(oColumn.DataField != "")
      {
        int iColumnIndex = oColumn.ColumnIndex;

        GetCheckedItems(iLevel, 0, this.Items, arItems, iColumnIndex);
      }
      else
      {
        foreach(string sCheckedItem in this.CheckedList)
        {
          string [] arElements = sCheckedItem.Split(' ');

          if(arElements[2] == oColumn.ColumnIndex.ToString())
          {
            int iCheckLevel = int.Parse(arElements[0]);
            string sCheckRowXml = HttpUtility.UrlDecode(arElements[3], Encoding.UTF8);

            GridItem oItem = new GridItem(this, iCheckLevel,
              this.GetXmlValues(this.Levels[iCheckLevel].Columns, sCheckRowXml));

            if(iCheckLevel == 0 && Levels[0].DataKeyField != "")
            {
              int iIndex = Items.IndexOf(Levels[0].DataKeyField, oItem[Levels[0].DataKeyField]);
              if(iIndex >= 0)
              {
                // pass in the actual item in the collection, not a copy
                oItem = Items[iIndex];
              }
            }

            arItems.Add(oItem);
          }
        }
      }

      return arItems;
    }

    /// <summary>
    /// Returns a collection of GridItems which are checked under the given column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used in conjunction with the <see cref="GridColumn" /> <see cref="GridColumn.ColumnType" /> property. If a column
    /// is bound to a boolean data type, and has its ColumnType set to <code>CheckBox</code>, this method can be used to retrieve
    /// all of the items which are currently checked in a column.
    /// </para>
    /// <para>
    /// For more information on implementing a checkbox column, see the following tutorial: 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Checkboxes.htm">Using Checkboxes in Grid</a>.
    /// </para>
    /// </remarks>
    /// <param name="oColumn">The column under which to look.</param>
    /// <returns>A collection of GridItems which are checked under the given column.</returns>
    public GridItemCollection GetCheckedItems(GridColumn oColumn)
    {
      if(oColumn.ColumnType != GridColumnType.CheckBox)
      {
        return null;
      }

      GridItemCollection arItems = new GridItemCollection();

      if(oColumn.DataField != "")
      {
        int iColumnIndex = oColumn.ColumnIndex;

        GetCheckedItems(this.Items, arItems, iColumnIndex);
      }
      else
      {
        foreach(string sCheckedItem in this.CheckedList)
        {
          string [] arElements = sCheckedItem.Split(' ');

          if(arElements[2] == oColumn.ColumnIndex.ToString())
          {
            int iCheckLevel = int.Parse(arElements[0]);
            string sCheckRowXml = HttpUtility.UrlDecode(arElements[3], Encoding.UTF8);

            GridItem oItem = new GridItem(this, iCheckLevel,
              this.GetXmlValues(this.Levels[iCheckLevel].Columns, sCheckRowXml));

            if(iCheckLevel == 0 && Levels[0].DataKeyField != "")
            {
              int iIndex = Items.IndexOf(Levels[0].DataKeyField, oItem[Levels[0].DataKeyField]);
              if(iIndex >= 0)
              {
                // pass in the actual item in the collection, not a copy
                oItem = Items[iIndex];
              }
            }

            arItems.Add(oItem);
          }
        }
      }

      return arItems;
    }

    /// <summary>
    /// Whether the given item is expanded.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to determine whether a <see cref="GridItem" /> is currently expanded.  
    /// </para>
    /// <para>
    /// Items can be expanded using the <see cref="Grid.Expand" /> method, and collapsed using <see cref="Grid.Collapse" />.
    /// </para>
    /// </remarks>
    /// <param name="oItem">The GridItem to check.</param>
    /// <returns>Whether the given item is expanded.</returns>
    public bool IsExpanded(GridItem oItem)
    {
      int iLevel = oItem.Level;

      if(Levels.Count > iLevel && Levels[iLevel].DataKeyField != string.Empty)
      {
        object oKey = oItem[Levels[iLevel].DataKeyField];

        string sListEntry = string.Format("{0} {1}", iLevel, oKey);

        if(this.ExpandedList.Contains(sListEntry))
        {
          this.ExpandedList.Remove(sListEntry);
          return true;
        }
      }

      return false;
    }
    /*
    /// <summary>
    /// Adjust the page index or record offset to display the record with the given ID.
    /// </summary>
    /// <param name="dataKey">The unique data key of the item to find.</param>
    public void MoveToItem(object dataKey)
    {
      
    }
    */
    /// <summary>
    /// Raise a postback event.
    /// </summary>
    /// <param name="eventArgument">Postback argument</param>
    public void RaisePostBackEvent(string eventArgument)
    {
      this.HandlePostback(eventArgument);
    }

    /// <summary>
    /// Select the given item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to programmatically select a <see cref="GridItem" />. The list of currently selected items is accessible through
    /// the <see cref="Grid" /> <see cref="Grid.SelectedItems" /> property.
    /// </para>
    /// <para>
    /// All items can be selected using the <see cref="Grid.SelectAll" /> method and all selected items can be de-selected using the 
    /// <see cref="Grid.UnSelectAll" /> method. Items can be selected by key using the <see cref="Grid.SelectAllKeys" /> and <see cref="Grid.SelectKey" />
    /// methods.
    /// </para>
    /// </remarks>
    /// <param name="oItem">The GridItem to select.</param>
    /// <returns>Whether the item could be selected.</returns>
    public bool Select(GridItem oItem)
    {
      if(Levels.Count > 0 && Levels[0].DataKeyField != string.Empty)
      {
        int iIndex = this.Items.IndexOf(oItem);
        if(iIndex >= 0)
        {
          object oKey = oItem[Levels[0].DataKeyField];

          this.SelectedList.Add(string.Format("0 {0} {1}", oKey, ArrayToXml(oItem.ToArray(), true)));

          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Select all loaded items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method causes all <see cref="GridItem">GridItems</see> which are loaded into the grid to be selected. The list of currently selected items 
    /// is accessible through the <see cref="Grid" /> <see cref="Grid.SelectedItems" /> property. All selected items can be de-selected using the
    /// <see cref="Grid.UnSelectAll" /> method. 
    /// </para>
    /// <para>
    /// A single item can be selected using the <see cref="Grid.Select" /> method. Items can be selected by key using the 
    /// <see cref="Grid.SelectAllKeys" /> and <see cref="SelectKey" />
    /// methods.
    /// </para>
    /// </remarks>
    public void SelectAll()
    {
      this.UnSelectAll();

      foreach (GridItem oItem in this.Items)
      {
        this.Select(oItem);
      }
    }

    /// <summary>
    /// Select all items with keys in the given list, on the top level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts an array of key values and selects all corresponding <see cref="GridItem">GridItems</see> on the first level (Grid.Levels[0]). 
    /// The overload for this method is used on hierarchical grids, and allows an item to be selected by key from a different level.
    /// Each <see cref="GridLevel" /> has a 
    /// <see cref="GridLevel.DataKeyField" /> property which allows a column to be defined as the "key" for that level. The column
    /// should have unique values for each item.  
    /// </para>
    /// <para>
    /// A single item can be selected by key using the <see cref="Grid.SelectKey" /> method. The list of currently selected items 
    /// is accessible through the <see cref="Grid" /> <see cref="Grid.SelectedItems" /> property. All selected items can be de-selected using the
    /// <see cref="Grid.UnSelectAll" /> method.
    /// </para>
    /// </remarks>
    /// <param name="arKeys">The array of keys to select.</param>
    public void SelectAllKeys(Array arKeys)
    {
      SelectAllKeys(0, arKeys);
    }

    /// <summary>
    /// Select all items with keys in the given list, on the given level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts an array of key values and selects all corresponding <see cref="GridItem">GridItems</see> on the specified
    /// <see cref="GridLevel" />.
    /// Each level has a <see cref="GridLevel.DataKeyField" /> property which allows a column to be defined as the "key" for that level. 
    /// The column is required to have unique values for each item.  
    /// </para>
    /// <para>
    /// A single item can be selected by key using the <see cref="Grid.SelectKey" /> method. The list of currently selected items 
    /// is accessible through the <see cref="Grid" /> <see cref="Grid.SelectedItems" /> property. All selected items can be de-selected using the
    /// <see cref="Grid.UnSelectAll" /> method.
    /// </para>
    /// </remarks>
    /// <param name="arKeys">The array of keys to select.</param>
    public void SelectAllKeys(int iLevel, Array arKeys)
    {
      foreach (object oKey in arKeys)
      {
        this.SelectedList.Add(iLevel + " " + oKey);
      }
    }

    /// <summary>
    /// Select the item with the given unique key, on the top level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a key value and selects the corresponding <see cref="GridItem" /> on the top <see cref="GridLevel" />.
    /// Each level has a <see cref="GridLevel.DataKeyField" /> property which allows a column to be defined as the "key" for that level. 
    /// The column is required to have unique values for each item.  
    /// </para>
    /// <para>
    /// Multiple items can be selected by key using the <see cref="Grid.SelectAllKeys" /> method. The list of currently selected items 
    /// is accessible through the <see cref="Grid" /> <see cref="Grid.SelectedItems" /> property. All selected items can be de-selected using the
    /// <see cref="Grid.UnSelectAll" /> method.
    /// </para>
    /// </remarks>
    /// <param name="oKey">The key to select.</param>
    public void SelectKey(object oKey)
    {
      this.SelectKey(0, oKey);
    }

    /// <summary>
    /// Select the item with the given unique key, on the given level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a key value and selects the corresponding <see cref="GridItem" /> on the given <see cref="GridLevel" />.
    /// Each level has a <see cref="GridLevel.DataKeyField" /> property which allows a column to be defined as the "key" for that level. 
    /// The column is required to have unique values for each item.  
    /// </para>
    /// <para>
    /// Multiple items can be selected by key using the <see cref="Grid.SelectAllKeys" /> method. The list of currently selected items 
    /// is accessible through the <see cref="Grid" /> <see cref="Grid.SelectedItems" /> property. All selected items can be de-selected using the
    /// <see cref="Grid.UnSelectAll" /> method.
    /// </para>
    /// </remarks>
    /// <param name="oKey">The key to select.</param>
    public void SelectKey(int iLevel, object oKey)
    {
      this.SelectedList.Add(iLevel + " " + oKey);
    }

    /// <summary>
    /// Uncheck all checked items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to uncheck all <see cref="GridItem">GridItems</see> which are checked. 
    /// This method requires that a checkbox column be defined on at least one <see cref="GridLevel" />.
    /// A boolean column can be defined as a checkbox column using the <see cref="GridColumn" /> <see cref="GridColumn.ColumnType" /> property.
    /// The <see cref="Grid.GetCheckedItems" /> method can be used to retrieve the collection of checked items.
    /// </para>
    /// <para>
    /// For more information on implementing a checkbox column, see the following tutorial: 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Checkboxes.htm">Using Checkboxes in Grid</a>.
    /// </para>
    /// </remarks>
    public void UnCheckAll()
    {
      this.CheckedList.Clear();
    }

    /// <summary>
    /// Unselect all selected items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method de-selects all currently selected <see cref="GridItem">GridItems</see>. Items can be selected manually by the user,
    /// or programatically using one of the following methods: <see cref="Grid.Select" />, <see cref="Grid.SelectAll" />, <see cref="Grid.SelectKey" />,
    /// <see cref="Grid.SelectAllKeys" />. The list of currently selected items is accessible through
    /// the <see cref="Grid" /> <see cref="Grid.SelectedItems" /> property.  
    /// </para>
    /// </remarks>
    public void UnSelectAll()
    {
      this.SelectedList.Clear();
    }

    #endregion

    #region Protected Methods

    protected override void ComponentArtPreRender(EventArgs oArgs)
    {
      // make sure we're data-bound
      if (!_dataBound)
      {
        if (this.NeedRebind != null)
        {
          this.OnNeedRebind(EventArgs.Empty);
        }
        else
        {
          this.DataBind();
        }
      }

      if(!this.IsDownLevel())
      {
        // Tack this to the bottom of the page, to know when we've loaded.
        RegisterStartupScript("ComponentArt_Page_Loaded", this.DemarcateClientScript("window.ComponentArt_Page_Loaded = true;"));
      }
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      string sGridVar = this.GetSaneId();

      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        RenderAccessibleContent(output);
      }

      else
      {
        // are we down-level?
        if (this.IsDownLevel())
        {
          this.DownLevelRender(output);
        }
        else
        {
          // Is this a callback? Handle it now
          if (Context.Request.QueryString[string.Format("Cart_{0}_Callback", sGridVar)] != null)
          {
            this.HandleCallback();
            return;
          }

          output = new HtmlTextWriter(output, string.Empty);

          StringBuilder oSB = new StringBuilder();

              // Write script
              if (!this.IsDownLevel() && Page != null)
              {
                // Add core code
                if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
                {
                  Page.RegisterClientScriptBlock("A573G988.js", "");
                  WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
                }
                if (!Page.IsClientScriptBlockRegistered("A573P291.js"))
                {
                  Page.RegisterClientScriptBlock("A573P291.js", "");
                  WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573P291.js");
                }
                if (!Page.IsClientScriptBlockRegistered("A573Z388.js"))
                {
                  Page.RegisterClientScriptBlock("A573Z388.js", "");
                  WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573Z388.js");
                }
                if (!Page.IsClientScriptBlockRegistered("A573R178.js"))
                {
                  Page.RegisterClientScriptBlock("A573R178.js", "");
                  WriteGlobalClientScript(output, "ComponentArt.Web.UI.Grid.client_scripts", "A573R178.js");
                }
                if (!Page.IsClientScriptBlockRegistered("A573J198.js"))
                {
                  Page.RegisterClientScriptBlock("A573J198.js", "");
                  WriteGlobalClientScript(output, "ComponentArt.Web.UI.Grid.client_scripts", "A573J198.js");
                }
                if (!Page.IsClientScriptBlockRegistered("A573R378.js"))
                {
                  Page.RegisterClientScriptBlock("A573R378.js", "");
                  WriteGlobalClientScript(output, "ComponentArt.Web.UI.Grid.client_scripts", "A573R378.js");
                }
                if (!Page.IsClientScriptBlockRegistered("A573G188.js"))
                {
                  Page.RegisterClientScriptBlock("A573G188.js", "");
                  WriteGlobalClientScript(output, "ComponentArt.Web.UI.Grid.client_scripts", "A573G188.js");
                }
                if (!Page.IsClientScriptBlockRegistered("A573L238.js"))
                {
                  Page.RegisterClientScriptBlock("A573L238.js", "");
                  WriteGlobalClientScript(output, "ComponentArt.Web.UI.Grid.client_scripts", "A573L238.js");
                }
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

          // render server templates
          this.RenderServerTemplates(output);

          if (this.AutoTheming)
          {
            this.ApplyTheming(false);
          }

          bool bRenderScrollBar = (ScrollBar == GridScrollBarMode.On || (ScrollBar == GridScrollBarMode.Auto && RecordCount > PageSize));

          // Render frame

          output.Write("<table"); // begin <table>
          output.WriteAttribute("id", sGridVar);
          output.WriteAttribute("cellpadding", "0");
          output.WriteAttribute("cellspacing", "0");
          output.WriteAttribute("border", "0");

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
          output.Write("\">"); // end <table>

          /** begin render pre-header **/
          if (this.PreHeaderClientTemplateId != "")
          {
            output.RenderBeginTag(HtmlTextWriterTag.Tr);

            if (this.PreHeaderCssClass != string.Empty)
            {
              output.AddAttribute("class", this.PreHeaderCssClass);
            }
            output.AddAttribute("id", sGridVar + "_preheader");

            if (bRenderScrollBar)
            {
              output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
            }
            output.RenderBeginTag(HtmlTextWriterTag.Td);

            output.RenderEndTag(); // </td>
            output.RenderEndTag(); // </tr>
          }
          /** end render pre-header **/

          /** begin render header **/
          if (this.ShowHeader)
          {
            output.RenderBeginTag(HtmlTextWriterTag.Tr);

            if (this.HeaderCssClass != string.Empty)
            {
              output.AddAttribute("class", this.HeaderCssClass);
            }
            output.AddAttribute("id", sGridVar + "_header");

            if (bRenderScrollBar)
            {
              output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
            }
            output.RenderBeginTag(HtmlTextWriterTag.Td);

            output.RenderEndTag(); // </td>
            output.RenderEndTag(); // </tr>
          }
          /** end render header **/

          /** begin render grid cell **/
          output.RenderBeginTag(HtmlTextWriterTag.Tr);

          if (!AllowHorizontalScrolling)
          {
            output.AddAttribute("id", sGridVar + "_dom");
          }
          output.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
          output.AddStyleAttribute("vertical-align", "top");
          if (this.DataAreaCssClass != "")
          {
            output.AddAttribute(HtmlTextWriterAttribute.Class, this.DataAreaCssClass);
          }
          output.RenderBeginTag(HtmlTextWriterTag.Td);
          if (AllowHorizontalScrolling)
          {
            output.AddAttribute("id", sGridVar + "_dom");
            output.AddStyleAttribute("overflow-x", "auto");
            output.AddStyleAttribute("overflow-y", "hidden");
            output.AddStyleAttribute("vertical-align", "top");
            output.AddStyleAttribute("height", "100%");
            if (Width.Type == UnitType.Pixel)
            {
              output.AddStyleAttribute("width", this.Width.ToString());
            }
            output.RenderBeginTag(HtmlTextWriterTag.Div);
          }

          output.Write("&nbsp;");

          if (AllowHorizontalScrolling)
          {
            output.RenderEndTag(); // </div>
          }
          output.RenderEndTag(); // </td>

          if (bRenderScrollBar)
          {
            bool bScrollBarEnabled = this.RecordCount > this.PageSize;
            output.AddAttribute("id", sGridVar + "_scroll");
            output.AddAttribute("width", this.ScrollBarWidth.ToString());
            //output.AddStyleAttribute("height", "100%"); causes problems for IE in standards-compliant mode
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.Write("&nbsp;");
            output.RenderEndTag(); // </td>
          }

          output.RenderEndTag(); // </tr>
          /** end render grid cell **/

          /** begin render footer **/
          if (this.ShowFooter)
          {
            output.RenderBeginTag(HtmlTextWriterTag.Tr);

            if (this.FooterCssClass != string.Empty)
            {
              output.AddAttribute("class", this.FooterCssClass);
            }
            output.AddAttribute("id", sGridVar + "_footer");
            if (bRenderScrollBar)
            {
              output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
            }
            output.RenderBeginTag(HtmlTextWriterTag.Td);

            output.RenderEndTag(); // </td>
            output.RenderEndTag(); // </tr>
          }
          /** end render footer **/

          /** begin render post-footer **/
          if (this.PostFooterClientTemplateId != "")
          {
            output.RenderBeginTag(HtmlTextWriterTag.Tr);

            if (this.PostFooterCssClass != string.Empty)
            {
              output.AddAttribute("class", this.PostFooterCssClass);
            }
            output.AddAttribute("id", sGridVar + "_postfooter");

            if (bRenderScrollBar)
            {
              output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
            }
            output.RenderBeginTag(HtmlTextWriterTag.Td);

            output.RenderEndTag(); // </td>
            output.RenderEndTag(); // </tr>
          }
          /** end render post-footer **/

          output.Write("</table>"); // </table>

          // Render event list hidden field
          output.AddAttribute("id", sGridVar + "_EventList");
          output.AddAttribute("name", sGridVar + "_EventList");
          if (this.RunningMode == GridRunningMode.Callback)
          {
            output.AddAttribute("value", GenerateStateEventList());
          }
          else
          {
            output.AddAttribute("value", string.Empty);
          }
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render data hidden field
          output.AddAttribute("id", sGridVar + "_Data");
          output.AddAttribute("name", sGridVar + "_Data");
          output.AddAttribute("value", string.Empty);
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render the hidden expanded-server-groups field?
          if (ExpandedGroupInfo.Count > 0)
          {
            output.AddAttribute("id", sGridVar + "_ExpandedGroups");
            output.AddAttribute("name", sGridVar + "_ExpandedGroups");
            output.AddAttribute("value", GenerateExpandedGroupList(ExpandedGroupInfo));
            output.AddAttribute("type", "hidden");
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();
          }
          
          // Render tab-focus-getting form element.
          if (this.KeyboardEnabled)
          {
            output.AddAttribute("href", "#");
            output.AddAttribute("onfocus", "ComponentArt_SetKeyboardFocusedGrid(document.getElementById('" + sGridVar + "'), " + sGridVar + ");");
            output.AddStyleAttribute("position", "absolute");
            output.AddStyleAttribute("z-index", "99");
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.RenderEndTag();
          }

          // Write init + data
          oSB.Append("/*** ComponentArt.Web.UI.Grid ").Append(this.VersionString()).Append(" ").Append(sGridVar).Append(" ***/\n");

          // Add grid initialization
          oSB.Append("window.ComponentArt_Init_" + sGridVar + " = function() {\n");

          // Include check for whether everything we need is loaded,
          // and a retry after a delay in case it isn't.
          oSB.Append("if(!window.ComponentArt_Page_Loaded ||!window.ComponentArt_Grid_Kernel_Loaded || !window.ComponentArt_Grid_Support_Loaded || !window.ComponentArt_Grid_Render_Loaded || !window.ComponentArt_Grid_Callback_Loaded || !window.ComponentArt_Grid_Keyboard_Loaded || !window.ComponentArt_DragDrop_Loaded || !window.ComponentArt_Keyboard_Loaded || !window.ComponentArt_Utils_Loaded || !document.getElementById('" + this.ClientID + "'))\n");
          oSB.Append("\t{window.setTimeout('window.ComponentArt_Init_" + sGridVar + "()', 50); return; }\n\n");

          // Write data
          oSB.AppendFormat("window.{0} = new ComponentArt_Grid('{0}');\n", sGridVar);
          oSB.Append(sGridVar + ".Data = ");
          oSB.Append(GenerateStorage(Items, 0));
          oSB.Append(";\n");

          // Write column info
          oSB.Append(GenerateLevelData(sGridVar));

          // Write client-side templates
          if (this.ClientTemplates.Count > 0)
          {
            oSB.Append(sGridVar + ".ClientTemplates = [");

            int iNumTemplates = this.ClientTemplates.Count;
            foreach (ClientTemplate oClientTemplate in this.ClientTemplates)
            {
              string sClientTemplateText = oClientTemplate.Text.Replace("\n", "").Replace("\r", "").Replace("'", "\\'");
              oSB.Append("['" + oClientTemplate.ID + "','" + sClientTemplateText + "'],");
              if (iNumTemplates < this.ClientTemplates.Count - 1)
              {
                oSB.Append(",");
              }
              iNumTemplates++;
            }

            oSB.Append("];\n");
          }

          // Write postback function reference
          if (Page != null)
          {
            oSB.Append(sGridVar + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
          }

          // Hook the actual ID if available and different from effective client ID
          if (this.ID != sGridVar)
          {
            oSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sGridVar + "; " + sGridVar + ".GlobalAlias = '" + ID + "'; }\n");
          }

          // Write properties
          oSB.Append("var properties = [\n");
          if (_addRow) oSB.Append("['AddingRow',1],");
          if (AllowColumnResizing) oSB.Append("['AllowColumnResizing',1],");
          if (AllowEditing) oSB.Append("['AllowEditing',1],");
          if (AllowHorizontalScrolling) oSB.Append("['AllowHorizontalScrolling',1],");
          if (AllowMultipleSelect) oSB.Append("['AllowMultipleSelect',1],");
          if (AllowPaging) oSB.Append("['AllowPaging',1],");
          if (AllowTextSelection) oSB.Append("['AllowTextSelection',1],");
          if (AllowVerticalScrolling) oSB.Append("['AllowVerticalScrolling',1],");
          if (AutoAdjustPageSize) oSB.Append("['AutoAdjustPageSize',1],");
          if (AutoCallBackOnCheckChanged) oSB.Append("['AutoCallBackOnCheckChanged',1],");
          if (AutoCallBackOnColumnReorder) oSB.Append("['AutoCallBackOnColumnReorder',1],");
          if (AutoCallBackOnDelete) oSB.Append("['AutoCallBackOnDelete',1],");
          if (AutoCallBackOnInsert) oSB.Append("['AutoCallBackOnInsert',1],");
          if (AutoCallBackOnUpdate) oSB.Append("['AutoCallBackOnUpdate',1],");
          if (AutoFocusSearchBox) oSB.Append("['AutoFocusSearchBox',1],");
          if (AutoPostBackOnCheckChanged) oSB.Append("['AutoPostBackOnCheckChanged',1],");
          if (AutoPostBackOnColumnReorder) oSB.Append("['AutoPostBackOnColumnReorder',1],");
          if (AutoPostBackOnDelete) oSB.Append("['AutoPostBackOnDelete',1],");
          if (AutoPostBackOnInsert) oSB.Append("['AutoPostBackOnInsert',1],");
          if (AutoPostBackOnUpdate) oSB.Append("['AutoPostBackOnUpdate',1],");
          if (AutoPostBackOnSelect) oSB.Append("['AutoPostBackOnSelect',1],");
          if (AutoTheming)
          {
            oSB.Append("['AutoTheming',1],");
            oSB.Append("['AutoThemingCssClassPrefix','" + AutoThemingCssClassPrefix + "'],");
          }
          if (CallbackCachingEnabled)
          {
            oSB.Append("['CallbackCachingEnabled',1],");
            oSB.Append("['CallbackCacheLookAhead'," + CallbackCacheLookAhead + "],");
            oSB.Append("['CallbackCacheSize'," + CallbackCacheSize + "],");
          }
          oSB.Append("['CallbackParameter'," + Utils.ConvertStringToJSString(this.CallbackParameter) + "],");
          oSB.Append("['CallbackPrefix',unescape('" + HttpUtility.UrlEncode(this.CallbackPrefix != "" ? this.CallbackPrefix : Utils.GetResponseUrl(Context) + (Context.Request.QueryString.Count > 0 ? "&" : "?") + "Cart_" + sGridVar + "_Callback=yes").Replace("'", "\\'") + "')],");
          if (CallbackReloadTemplates) oSB.Append("['CallbackReloadTemplates',1],");
          if (CallbackReloadTemplateScripts) oSB.Append("['CallbackReloadTemplateScripts',1],");
          oSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "],");
          if (ClientSideOnAfterCallback != string.Empty) oSB.Append("['ClientSideOnAfterCallback'," + ClientSideOnAfterCallback + "],");
          if (ClientSideOnBeforeCallback != string.Empty) oSB.Append("['ClientSideOnBeforeCallback'," + ClientSideOnBeforeCallback + "],");
          if (ClientSideOnCallbackError != string.Empty) oSB.Append("['ClientSideOnCallbackError'," + ClientSideOnCallbackError + "],");
          if (ClientSideOnCheckChanged != string.Empty) oSB.Append("['ClientSideOnCheckChanged'," + ClientSideOnCheckChanged + "],");
          if (ClientSideOnColumnReorder != string.Empty) oSB.Append("['ClientSideOnColumnReorder'," + ClientSideOnColumnReorder + "],");
          if (ClientSideOnColumnResize != string.Empty) oSB.Append("['ClientSideOnColumnResize'," + ClientSideOnColumnResize + "],");
          if (ClientSideOnDelete != string.Empty) oSB.Append("['ClientSideOnDelete'," + ClientSideOnDelete + "],");
          if (ClientSideOnDoubleClick != string.Empty) oSB.Append("['ClientSideOnDoubleClick'," + ClientSideOnDoubleClick + "],");
          if (ClientSideOnGroup != string.Empty) oSB.Append("['ClientSideOnGroup'," + ClientSideOnGroup + "],");
          if (ClientSideOnInsert != string.Empty) oSB.Append("['ClientSideOnInsert'," + ClientSideOnInsert + "],");
          if (ClientSideOnLoad != string.Empty) oSB.Append("['ClientSideOnLoad'," + ClientSideOnLoad + "],");
          if (ClientSideOnPage != string.Empty) oSB.Append("['ClientSideOnPage'," + ClientSideOnPage + "],");
          if (ClientSideOnSelect != string.Empty) oSB.Append("['ClientSideOnSelect'," + ClientSideOnSelect + "],");
          if (ClientSideOnSort != string.Empty) oSB.Append("['ClientSideOnSort'," + ClientSideOnSort + "],");
          if (ClientSideOnUpdate != string.Empty) oSB.Append("['ClientSideOnUpdate'," + ClientSideOnUpdate + "],");
          if (CollapseImageUrl != string.Empty) oSB.Append("['CollapseImageUrl','" + Utils.ConvertUrl(Context, ImagesBaseUrl, CollapseImageUrl) + "'],");
          oSB.Append("['CellSpacing'," + this.CellSpacing + "],");
          oSB.Append("['CheckedList'," + GenerateList(CheckedList, 3) + "],");
          oSB.Append("['CollapseDuration'," + this.CollapseDuration + "],");
          oSB.Append("['CollapseSlide'," + ((int)this.CollapseSlide).ToString() + "],");
          oSB.Append("['CollapseTransition'," + ((int)this.CollapseTransition).ToString() + "],");
          if (this.CollapseTransitionCustomFilter != string.Empty) oSB.Append("['CollapseTransitionCustomFilter','" + this.CollapseTransitionCustomFilter + "'],");
          if (ColumnResizeDistributeWidth) oSB.Append("['ColumnResizeDistributeWidth',1],");
          oSB.Append("['CurrentPageIndex'," + this.CurrentPageIndex + "],");
          if (Debug) oSB.Append("['Debug',1],");
          if (_editingId != null)
          {
            oSB.Append("['EditingId','" + _editingId + "'],");
            oSB.Append("['EditingDirty',1],");
          }
          if (EditOnClickSelectedItem) oSB.Append("['EditOnClickSelectedItem',1],");
          if (EmptyGridText != string.Empty) oSB.Append("['EmptyGridText','" + EmptyGridText + "'],");
          if (EnableViewState) oSB.Append("['EnableViewState',1],");
          if (ExpandCollapseClientTemplateId != string.Empty) oSB.Append("['ExpandCollapseClientTemplateId','" + ExpandCollapseClientTemplateId + "'],");
          if (CollapseImageUrl != string.Empty || ExpandImageUrl != string.Empty)
          {
            oSB.Append("['ExpandCollapseImageHeight'," + ExpandCollapseImageHeight + "],");
            oSB.Append("['ExpandCollapseImageWidth'," + ExpandCollapseImageWidth + "],");
          }
          oSB.Append("['ExpandDuration'," + this.ExpandDuration + "],");
          oSB.Append("['ExpandSlide'," + ((int)this.ExpandSlide).ToString() + "],");
          oSB.Append("['ExpandTransition'," + ((int)this.ExpandTransition).ToString() + "],");
          if (this.ExpandTransitionCustomFilter != string.Empty) oSB.Append("['ExpandTransitionCustomFilter','" + this.ExpandTransitionCustomFilter + "'],");
          oSB.Append("['ExpandedList'," + GenerateList(ExpandedList, 0) + "],");
          if (ExpandImageUrl != string.Empty) oSB.Append("['ExpandImageUrl','" + Utils.ConvertUrl(Context, ImagesBaseUrl, ExpandImageUrl) + "'],");
          if (ExternalDropTargets != string.Empty) oSB.Append("['ExternalDropTargets','" + this.ExternalDropTargets + "'],");
          if (FillContainer) oSB.Append("['FillContainer',1],");
          oSB.Append("['FooterHeight'," + this.FooterHeight + "],");
          oSB.Append("['GroupContinuedText'," + Utils.ConvertStringToJSString(GroupContinuedText) + "],");
          oSB.Append("['GroupContinuingText'," + Utils.ConvertStringToJSString(GroupContinuingText) + "],");
          if (this.ShowHeader)
          {
            if (GroupByClientTemplateId != string.Empty) oSB.Append("['GroupByClientTemplateId','" + GroupByClientTemplateId + "'],");
            if (GroupByCssClass != string.Empty) oSB.Append("['GroupByCssClass','" + GroupByCssClass + "'],");
            if (GroupBySectionCssClass != string.Empty) oSB.Append("['GroupBySectionCssClass','" + GroupBySectionCssClass + "'],");
            if (GroupBySectionSeparatorCssClass != string.Empty) oSB.Append("['GroupBySectionSeparatorCssClass','" + GroupBySectionSeparatorCssClass + "'],");
            oSB.Append("['GroupByText','" + GroupByText + "'],");
            if (GroupByTextCssClass != string.Empty) oSB.Append("['GroupByTextCssClass','" + GroupByTextCssClass + "'],");
            if (GroupBySortAscendingImageUrl != string.Empty || GroupBySortDescendingImageUrl != string.Empty)
            {
              if (GroupBySortAscendingImageUrl != string.Empty) oSB.Append("['GroupBySortAscendingImageUrl','" + Utils.ConvertUrl(Context, ImagesBaseUrl, GroupBySortAscendingImageUrl) + "'],");
              if (GroupBySortDescendingImageUrl != string.Empty) oSB.Append("['GroupBySortDescendingImageUrl','" + Utils.ConvertUrl(Context, ImagesBaseUrl, GroupBySortDescendingImageUrl) + "'],");
              oSB.Append("['GroupBySortImageHeight'," + GroupBySortImageHeight + "],");
              oSB.Append("['GroupBySortImageWidth'," + GroupBySortImageWidth + "],");
            }
            oSB.Append("['GroupingNotificationText','" + GroupingNotificationText + "'],");
            if (GroupingNotificationTextCssClass != string.Empty) oSB.Append("['GroupingNotificationTextCssClass','" + GroupingNotificationTextCssClass + "'],");
            oSB.Append("['ShowHeader',1],");
          }
          if (this.ServerGroups.Count > 0)
          {
            oSB.Append("['ServerGroups'," + GenerateServerGroups(this.ServerGroups) + "],");
          }
          if (this.NeedServerGroups)
          {
            if (_serverGroupsContinued) oSB.Append("['ServerGroupsContinued',1],");
            if (_serverGroupsContinuing) oSB.Append("['ServerGroupsContinuing',1],");
            oSB.Append("['ServerGrouping',1],");
            oSB.Append("['ExpandedGroups','" + GenerateExpandedGroupList(this.ExpandedGroupInfo) + "'],");
          }
          oSB.Append("['GroupingPageSize'," + this.GroupingPageSize + "],");
          if (GroupingCountHeadingsAsRows) oSB.Append("['GroupingCountHeadingsAsRows',1],");
          oSB.Append("['GroupingMode'," + (int)this.GroupingMode + "],");
          if (GroupingPageByRow) oSB.Append("['GroupingPageByRow',1],");
          oSB.Append("['Groupings'," + GenerateGroupList(GroupBy) + "],");
          oSB.Append("['HeaderHeight'," + this.HeaderHeight + "],");
          oSB.Append("['Id','" + sGridVar + "'],");
          if (IndentCellCssClass != string.Empty) oSB.Append("['IndentCellCssClass','" + IndentCellCssClass + "'],");
          if (KeyboardEnabled) oSB.Append("['KeyboardEnabled',1],");
          oSB.Append("['IndentCellWidth'," + IndentCellWidth + "],");
          if (ItemDraggingClientTemplateId != string.Empty) oSB.Append("['ItemDraggingClientTemplateId','" + ItemDraggingClientTemplateId + "'],");
          if (ItemDraggingCssClass != string.Empty) oSB.Append("['ItemDraggingCssClass','" + ItemDraggingCssClass + "'],");
          if (ItemDraggingEnabled) oSB.Append("['ItemDraggingEnabled',1],");
          if (LoadingPanelEnabled) oSB.Append("['LoadingPanelEnabled',1],");
          if (LoadingPanelClientTemplateId != string.Empty)
          {
            oSB.Append("['LoadingPanelClientTemplateId','" + LoadingPanelClientTemplateId + "'],");
            oSB.Append("['LoadingPanelPosition','" + LoadingPanelPosition + "'],");
            oSB.Append("['LoadingPanelOffsetX'," + LoadingPanelOffsetX + "],");
            oSB.Append("['LoadingPanelOffsetY'," + LoadingPanelOffsetY + "],");
            oSB.Append("['LoadingPanelFadeDuration'," + LoadingPanelFadeDuration + "],");
            oSB.Append("['LoadingPanelFadeMaximumOpacity'," + LoadingPanelFadeMaximumOpacity + "],");
          }
          if (ManualPaging) oSB.Append("['ManualPaging',1],");
          if (NoExpandImageUrl != string.Empty) oSB.Append("['NoExpandImageUrl','" + Utils.ConvertUrl(Context, ImagesBaseUrl, NoExpandImageUrl) + "'],");
          if (OnContextMenu != string.Empty) oSB.Append("['OnContextMenu'," + OnContextMenu + "],");
          if (this.ShowFooter)
          {
            if (PagerButtonActiveEnabled) oSB.Append("['PagerButtonActiveEnabled',1],");
            if (PagerButtonHoverEnabled) oSB.Append("['PagerButtonHoverEnabled',1],");
            oSB.Append("['PagerButtonHeight'," + PagerButtonHeight + "],");
            oSB.Append("['PagerButtonWidth'," + PagerButtonWidth + "],");
            oSB.Append("['PagerButtonPadding'," + PagerButtonPadding + "],");
            if (PagerImagesFolderUrl != string.Empty) oSB.Append("['PagerImagesFolderUrl','" + Utils.ConvertUrl(Context, "", PagerImagesFolderUrl) + (PagerImagesFolderUrl.EndsWith("/") ? "" : "/") + "'],");
            if (PagerInfoClientTemplateId != string.Empty) oSB.Append("['PagerInfoClientTemplateId','" + PagerInfoClientTemplateId + "'],");
            if (PagerTextCssClass != string.Empty) oSB.Append("['PagerTextCssClass','" + PagerTextCssClass + "'],");
            oSB.Append("['PagerStyle'," + (int)this.PagerStyle + "],");
            oSB.Append("['ShowFooter',1],");
          }
          if (PagePaddingEnabled) oSB.Append("['PagePaddingEnabled',1],");
          oSB.Append("['PageSize'," + this.PageSize + "],");
          if (PostFooterClientTemplateId != "") oSB.Append("['PostFooterClientTemplateId','" + PostFooterClientTemplateId + "'],");
          if (PreHeaderClientTemplateId != "") oSB.Append("['PreHeaderClientTemplateId','" + PreHeaderClientTemplateId + "'],");
          if (PreExpandOnGroup) oSB.Append("['PreExpandOnGroup',1],");
          if (PreloadLevels) oSB.Append("['PreloadLevels',1],");
          oSB.Append("['GroupingNotificationPosition','" + this.GroupingNotificationPosition.ToString().ToLower() + "'],");
          oSB.Append("['PagerPosition','" + this.PagerPosition.ToString().ToLower() + "'],");
          oSB.Append("['PagerInfoPosition','" + this.PagerInfoPosition.ToString().ToLower() + "'],");
          oSB.Append("['SearchBoxPosition','" + this.SearchBoxPosition.ToString().ToLower() + "'],");
          oSB.Append("['ScrollBarWidth'," + (bRenderScrollBar ? this.ScrollBarWidth : 0) + "],");
          if (bRenderScrollBar)
          {
            oSB.Append("['ScrollBar',1],");
            oSB.Append("['ScrollButtonHeight'," + ScrollButtonHeight + "],");
            oSB.Append("['ScrollButtonWidth'," + ScrollButtonWidth + "],");
            if (ScrollBarCssClass != string.Empty) oSB.Append("['ScrollBarCssClass','" + this.ScrollBarCssClass + "'],");
            if (ScrollGripCssClass != string.Empty) oSB.Append("['ScrollGripCssClass','" + this.ScrollGripCssClass + "'],");
            if (ScrollHeaderCssClass != string.Empty) oSB.Append("['ScrollHeaderCssClass','" + this.ScrollHeaderCssClass + "'],");
            if (ScrollImagesFolderUrl != string.Empty) oSB.Append("['ScrollImagesFolderUrl','" + Utils.ConvertUrl(Context, "", ScrollImagesFolderUrl) + (ScrollImagesFolderUrl.EndsWith("/") ? "" : "/") + "'],");
            if (ScrollPopupClientTemplateId != string.Empty) oSB.Append("['ScrollPopupClientTemplateId','" + ScrollPopupClientTemplateId + "'],");
            if (ScrollTopBottomImagesEnabled) oSB.Append("['ScrollTopBottomImagesEnabled',1],");
            oSB.Append("['ScrollTopBottomImageHeight'," + ScrollTopBottomImageHeight + "],");
            oSB.Append("['ScrollTopBottomImageWidth'," + ScrollTopBottomImageWidth + "],");
          }
          oSB.Append("['RenderingMode'," + (int)this.RenderingMode + "],");
          oSB.Append("['RunningMode'," + (int)this.RunningMode + "],");
          oSB.Append("['SelectedList'," + GenerateList(SelectedList, 2) + "],");
          if (SelfReferencing) oSB.Append("['SelfReferencing',1],");
          if (ShowSearchBox)
          {
            if (SearchBoxCssClass != string.Empty) oSB.Append("['SearchBoxCssClass','" + SearchBoxCssClass + "'],");
            if (SearchOnKeyPress)
            {
              oSB.Append("['SearchOnKeyPress',1],");
              oSB.Append("['SearchOnKeyPressDelay'," + SearchOnKeyPressDelay + "],");
            }
            if (ClientSearchString != null) oSB.Append("['SearchString', decodeURIComponent('" + HttpUtility.UrlEncode(ClientSearchString) + "')],");
            oSB.Append("['SearchText','" + this.SearchText + "'],");
            if (SearchTextCssClass != string.Empty) oSB.Append("['SearchTextCssClass','" + SearchTextCssClass + "'],");
            oSB.Append("['ShowSearchBox',1],");
          }
          if (PagerStyle == GridPagerStyle.Slider)
          {
            oSB.Append("['SliderPopupOffsetX'," + SliderPopupOffsetX + "],");
            oSB.Append("['SliderPopupOffsetY'," + SliderPopupOffsetY + "],");
            oSB.Append("['SliderEdgeWidth'," + SliderEdgeWidth + "],");
            oSB.Append("['SliderFetchDelay'," + SliderFetchDelay + "],");
            oSB.Append("['SliderGripWidth'," + SliderGripWidth + "],");
            oSB.Append("['SliderHeight'," + SliderHeight + "],");
            oSB.Append("['SliderWidth'," + SliderWidth + "],");
            if (SliderPopupClientTemplateId != string.Empty) oSB.Append("['SliderPopupClientTemplateId','" + SliderPopupClientTemplateId + "'],");
            if (SliderPopupCachedClientTemplateId != string.Empty) oSB.Append("['SliderPopupCachedClientTemplateId','" + SliderPopupCachedClientTemplateId + "'],");
            if (SliderPopupGroupedClientTemplateId != string.Empty) oSB.Append("['SliderPopupGroupedClientTemplateId','" + SliderPopupGroupedClientTemplateId + "'],");
          }
          if (this.Width.Type == UnitType.Percentage)
          {
            // percentage width
            oSB.Append("['PercentageWidth'," + this.Width.Value + "],");
          }
          if (TreeLineImagesFolderUrl != string.Empty)
          {
            oSB.Append("['TreeLineImagesFolderUrl','" + Utils.ConvertUrl(Context, "", TreeLineImagesFolderUrl) + (TreeLineImagesFolderUrl.EndsWith("/") ? "" : "/") + "'],");
            oSB.Append("['TreeLineImageHeight'," + +TreeLineImageHeight + "],");
            oSB.Append("['TreeLineImageWidth'," + +TreeLineImageWidth + "],");
          }

          if (SoaService != string.Empty) oSB.Append("['SoaService','" + SoaService + "'],");
          if (UseClientUrlAsPrefix) oSB.Append("['UseClientUrlAsPrefix',1],");
          if (WebService != "" || SoaService != "")
          {
            // common web service stuff
            oSB.Append("['WebService','" + WebService + "'],");
            if (WebServiceCachingEnabled) oSB.Append("['WebServiceCachingEnabled',1],");
            oSB.Append("['WebServiceCustomParameter','" + WebServiceCustomParameter + "'],");

            // custom web service? (non-soa)
            if (WebService != "")
            {
              oSB.Append("['WebService','" + WebService + "'],");
              oSB.Append("['WebServiceSelectMethod','" + WebServiceSelectMethod + "'],");
              if (WebServiceConfigMethod != "") oSB.Append("['WebServiceConfigMethod','" + WebServiceConfigMethod + "'],");
              if (WebServiceDeleteMethod != "") oSB.Append("['WebServiceDeleteMethod','" + WebServiceDeleteMethod + "'],");
              if (WebServiceGroupMethod != "") oSB.Append("['WebServiceGroupMethod','" + WebServiceGroupMethod + "'],");
              if (WebServiceUpdateMethod != "") oSB.Append("['WebServiceUpdateMethod','" + WebServiceUpdateMethod + "'],");
              if (WebServiceInsertMethod != "") oSB.Append("['WebServiceInsertMethod','" + WebServiceInsertMethod + "'],");
            }
          }

          oSB.Append("['PageCount'," + this.PageCount + "],");
          oSB.Append("['RecordCount'," + this.RecordCount + "],");
          oSB.Append("['RecordOffset'," + this.RecordOffset + "]");

          // End properties
          oSB.Append("];\n");

          // Set properties
          oSB.AppendFormat("ComponentArt_SetProperties({0}, properties);\n", sGridVar);

          // Initialize
          oSB.Append(sGridVar + ".Initialize();\n");
          if (KeyboardEnabled)
          {
            // Initialize keyboard
            oSB.Append(sGridVar + ".InitKeyboard();\n");
          }

          oSB.Append("}\n");

          if (this.Width.Type == UnitType.Percentage) oSB.Append("window.ComponentArt_" + sGridVar + "_ResizeHandler = function() { " + sGridVar + ".ResizeHandler(); };\n");

          // Complete init function
          oSB.Append("\nComponentArt_Init_" + sGridVar + "();\n");

          WriteStartupScript(output, this.DemarcateClientScript(oSB.ToString()));
        }
      }
    }

    private void HandleCallback()
    {
      GridItemCollection arItems = null;

      string sVarPrefix = "Cart_" + this.GetSaneId();

      // get callback level
      int iLevel = Convert.ToInt32(Context.Request.Params[sVarPrefix + "_Callback_Level"]);
      
      // get callback path
      string sPath = Context.Request.Params[sVarPrefix + "_Callback_Path"];

      // is this an out-of-band request?
      string sOutOfBand = Context.Request.Params[sVarPrefix + "_Callback_OutOfBand"];

      if(sPath != null)
      {
        GridItem oItem = this.GetItemFromPath(sPath);
        
        if(oItem != null)
        {
          arItems = oItem.Items;
        }
        else
        {
          // error, item not found, throw exception
        }
      }
      else
      {
        arItems = Items; 
      }

      // don't output child tables
      Context.Response.Clear();
      Context.Response.ContentType = "text/xml";
      Context.Response.Write("<GridResponse>");
      Context.Response.Write(this.GenerateXmlParams(sOutOfBand != null));
      Context.Response.Write(this.GenerateXmlTemplates(arItems, iLevel));
      Context.Response.Write(this.GenerateXmlData(arItems, iLevel));
      Context.Response.Write(this.GenerateXmlGroups());
      Context.Response.Write("</GridResponse>");

      try
      {
        Context.Response.End();
      }
      catch { }
    }

    private void HandleCallbackError(Exception ex)
    {
      // don't output child tables
      Context.Response.Clear();
      Context.Response.ContentType = "text/xml";
      Context.Response.Write("<GridResponse><Error><![CDATA[");
      Context.Response.Write(ex.Message);
      Context.Response.Write("]]></Error></GridResponse>");

      try
      {
        Context.Response.End();
      }
      catch { }
    }

    private void HandlePostback(string stringArgument)
    {
      return;
    }

    protected override bool IsDownLevel()
    {
      if(this.ClientTarget != ClientTargetLevel.Auto)
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
      catch {}

      if( // We are good if:

        // 0. We have the W3C Validator
        (sUserAgent.IndexOf("Validator") >= 0) ||

        // 1. We have IE 5 or greater on a non-Mac
        (sUserAgent.IndexOf("MSIE") >= 0 && iMajorVersion >= 5 && !Context.Request.Browser.Platform.ToUpper().StartsWith("MAC")) ||

        // 2. We have Gecko-based browser (Mozilla, Firefox, Netscape 6+)
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

    protected override void LoadViewState(object state)
    {
      base.LoadViewState(state);
      
      string sGridVar = this.GetSaneId();

      // Load level data
      if(ViewState["GridLevels"] != null)
      {
        this.Levels.LoadXml((string)ViewState["GridLevels"]);
      }

      // Load client storage data
      string sViewState = Context.Request.Form[sGridVar + "_Data"];
      if(sViewState != null)
      {
        this.LoadClientData(sViewState);

        // in client mode, the number of records might have changed, without a need to re-bind
        if (this.RunningMode == GridRunningMode.Client)
        {
          // update the count
          this.RecordCount = this.Items.Count;

          // it's also possible that the page we're on no longer exists
          if (this.CurrentPageIndex >= this.PageCount)
          {
            // go to the last page.
            this.CurrentPageIndex = Math.Max(0, this.PageCount - 1);
          }
        }
      }

      // set checked, selected, etc, in time for OnLoad
      if(this.EventList != null && this.EventList.Count > 0)
      {
        ExecuteScriptStateChanges(this.EventList);
      }

      this.InstantiateServerTemplates();

      _dataBound = true;
    }
    
    protected override void OnInit(EventArgs oArgs)
    {
      base.OnInit(oArgs);

      if(Context != null && Page != null && Context.Request != null)
      {
        string sGridVar = this.GetSaneId();

        // set callback parameter?
        string sCallbackParameter = Context.Request.Params[string.Format("Cart_{0}_CallbackParameter", this.GetSaneId())];
        if (sCallbackParameter != null)
        {
          this.CallbackParameter = sCallbackParameter;
        }
        
        // Mark whether this is a callback request
        if( Context.Request.QueryString[string.Format("Cart_{0}_Callback", sGridVar)] != null
          && !IsAjaxDisabled())
        {
          string sVarPrefix = "Cart_" + sGridVar;

          this.RunningMode = GridRunningMode.Callback;

          // Store event script.
          string sScript = Context.Request.Params[sVarPrefix + "_Callback_Script"];
          
          if(sScript != null)
          {
            this.EventList = new ArrayList(sScript.Split(';'));
          }

          // Load expanded server-groups.
          string sExpandedGroups = Context.Request.Form[sVarPrefix + "_ExpandedGroups"];
          if (sExpandedGroups != null)
          {
            string[] arGroups = sExpandedGroups.Split(',');
            foreach (string sGroup in arGroups)
            {
              ServerGroup oGroup = new ServerGroup();
              oGroup.RenderCount = Convert.ToInt32(sGroup.Split(':')[1]);
              oGroup.Path = sGroup.Split(':')[0];
              oGroup.Index = int.Parse(oGroup.Path.Split('_')[0]);

              this.ExpandedGroupInfo.Add(oGroup.Path, oGroup);
            }
          }
        }
        else if(Page.IsPostBack)
        {
          // Store event script.
          string sScript = Context.Request.Form[sGridVar + "_EventList"];
          if (sScript != null)
          {
            this.EventList = new ArrayList(sScript.Split(';'));
          }
        }
      }
    }

    protected override void OnLoad(EventArgs oArgs)
    {
      base.OnLoad(oArgs);

      string sGridVar = this.GetSaneId();

      // Mark whether this is a callback request
      if (Context.Request.QueryString[string.Format("Cart_{0}_Callback", sGridVar)] != null)
      {
        this.RunningMode = GridRunningMode.Callback;
      }

      // do we revert to server mode?
      if (this.RunningMode == GridRunningMode.Callback && IsAjaxDisabled())
      {
        this.RunningMode = GridRunningMode.Server;
      }

      // Execute events that occured on the client.
      if (this.EventList != null && this.EventList.Count > 0)
      {
        if (this.BeforeCallback != null && this.CausedCallback)
        {
          this.OnBeforeCallback(EventArgs.Empty);
        }

        this.ExecuteScript(this.EventList);

        if (this.AfterCallback != null && this.CausedCallback)
        {
          this.OnAfterCallback(EventArgs.Empty);
        }
      }

      if (this.ItemCommand != null && Page.IsPostBack)
      {
        // was the postback launched from one of our child controls?
        string sPostBackControlId = Page.Request.Params["__EVENTTARGET"];

        if (sPostBackControlId != null && sPostBackControlId != "")
        {
          Control oPostBackControl = Utils.FindControl(this, sPostBackControlId);

          if (oPostBackControl != null)
          {
            Control oControl = oPostBackControl;
            while (oControl.Parent != null && oControl.Parent != Page)
            {
              if (oControl.Parent.Parent == this)
              {
                // found!
                string sParentId = oControl.Parent.ID;
                string[] arParams = sParentId.Split('_');
                string sIndex = arParams[arParams.Length - 1];
                int iIndex = int.Parse(sIndex);
                GridItem oItem = this.Items[iIndex];
                this.OnItemCommand(new GridItemCommandEventArgs(oItem, oPostBackControl));
                break;
              }

              oControl = oControl.Parent;
            }
          }
        }
      }

    }

    protected override object SaveViewState()
    {
      if(EnableViewState)
      {
        ViewState["GridLevels"] = this.Levels.GetXml();
      }

      return base.SaveViewState();     
    }

    #endregion

    #region Private Methods

    private string ArrayToXml(object [] arData, bool bEncode)
    {
      StringBuilder oSB = new StringBuilder();
      oSB.Append("<r>");

      foreach(object o in arData)
      {
        oSB.Append("<c>" + (o == null? "" : o.ToString()) + "</c>");
      }

      oSB.Append("</r>");
      string sXml = oSB.ToString();

      if(bEncode)
      {
        sXml = HttpUtility.UrlEncode(sXml);
      }
      
      return sXml;
    }

    private void BuildImageList()
    {
      // do top-level images
      if(this.CollapseImageUrl != string.Empty)
      {
        string sImage = Utils.ConvertUrl(Context, this.ImagesBaseUrl, this.CollapseImageUrl);
        if(!this.PreloadImages.Contains(sImage))
        {
          this.PreloadImages.Add(sImage);
        }
      }
      if(this.ExpandImageUrl != string.Empty)
      {
        string sImage = Utils.ConvertUrl(Context, this.ImagesBaseUrl, this.ExpandImageUrl);
        if(!this.PreloadImages.Contains(sImage))
        {
          this.PreloadImages.Add(sImage);
        }
      }
      if(this.GroupBySortAscendingImageUrl != string.Empty)
      {
        string sImage = Utils.ConvertUrl(Context, this.ImagesBaseUrl, this.GroupBySortAscendingImageUrl);
        if(!this.PreloadImages.Contains(sImage))
        {
          this.PreloadImages.Add(sImage);
        }
      }
      if(this.GroupBySortDescendingImageUrl != string.Empty)
      {
        string sImage = Utils.ConvertUrl(Context, this.ImagesBaseUrl, this.GroupBySortDescendingImageUrl);
        if(!this.PreloadImages.Contains(sImage))
        {
          this.PreloadImages.Add(sImage);
        }
      }

      // do tree images
      if(this.TreeLineImagesFolderUrl != string.Empty)
      {
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "dash.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "dashplus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "dashminus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "l.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "lplus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "lminus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "r.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "rplus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "rminus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "t.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "tplus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "tminus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "plus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "minus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "i.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.TreeLineImagesFolderUrl, "noexpand.gif"));
      }

      // do pager images
      if(this.ShowFooter && this.PagerImagesFolderUrl != string.Empty)
      {
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "first.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "last.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "next.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "prev.gif"));

        if(this.PagerButtonHoverEnabled)
        {
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "first_hover.gif"));
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "last_hover.gif"));
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "next_hover.gif"));
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "prev_hover.gif"));
        }

        if(this.PagerButtonActiveEnabled)
        {
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "first_active.gif"));
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "last_active.gif"));
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "next_active.gif"));
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "prev_active.gif"));
        }

        if(this.PagerStyle == GridPagerStyle.Slider)
        {
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "slider_bg.gif"));
          this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "slider_grip.gif"));
          if(this.PagerButtonHoverEnabled)
          {
            this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "slider_grip_hover.gif"));
          }
          if(this.PagerButtonActiveEnabled)
          {
            this.PreloadImages.Add(Utils.ConvertUrl(Context, this.PagerImagesFolderUrl, "slider_grip_active.gif"));
          }
        }
      }

      // do level-specific images
      foreach(GridLevel oLevel in this.Levels)
      {
        if(oLevel.SortAscendingImageUrl != string.Empty)
        {
          string sImage = Utils.ConvertUrl(Context, this.ImagesBaseUrl, oLevel.SortAscendingImageUrl);
          if(!this.PreloadImages.Contains(sImage))
          {
            this.PreloadImages.Add(sImage);
          }
        }
        if(oLevel.SortDescendingImageUrl != string.Empty)
        {
          string sImage = Utils.ConvertUrl(Context, this.ImagesBaseUrl, oLevel.SortDescendingImageUrl);
          if(!this.PreloadImages.Contains(sImage))
          {
            this.PreloadImages.Add(sImage);
          }
        }
        if(oLevel.SelectorImageUrl != string.Empty)
        {
          string sImage = Utils.ConvertUrl(Context, this.ImagesBaseUrl, oLevel.SelectorImageUrl);
          if(!this.PreloadImages.Contains(sImage))
          {
            this.PreloadImages.Add(sImage);
          }
        }
        if(oLevel.ColumnReorderIndicatorImageUrl != string.Empty)
        {
          string sImage = Utils.ConvertUrl(Context, this.ImagesBaseUrl, oLevel.ColumnReorderIndicatorImageUrl);
          if(!this.PreloadImages.Contains(sImage))
          {
            this.PreloadImages.Add(sImage);
          }
        }
      }
    }

    private void DataBindToDataView(DataView oDataView)
    {
      this.LoadGridLevels(oDataView);

      // Apply group
      if(this.GroupBy != string.Empty && !(ManualPaging && GroupingPageByRow) && this.AutoSortOnGroup)
      {
        oDataView.Sort = this.GroupBy;
      }

      // Apply sort
      if(this.Sort != string.Empty)
      {
        oDataView.Sort = (oDataView.Sort == string.Empty? this.Sort : oDataView.Sort + ", " + this.Sort);
      }

      // Apply filter
      if(this.Search != string.Empty)
      {
        // Apply search
        oDataView.RowFilter = this.GetFilterForSearch(this.Search);
      }
      else if(this.Filter != string.Empty)
      {
        // Apply manual filter
        oDataView.RowFilter = this.Filter;
      }

      // Get some info about the dataset
      this.RecordCount = oDataView.Count;

      this.LoadGridItems(Items, GetPagedRows(oDataView), 0);
    }

    private void DataBindToEnumerable(IEnumerable arList)
    {
      // were were given a dataview? we have a different method for that.
      if (arList is DataView)
      {
        DataBindToDataView((DataView)arList);
        return;
      }

      if(this.Levels.Count == 0)
      {
        throw new Exception("At least one GridLevel must be defined when binding to object list.");
      }
      
      IEnumerator oEnumerator = arList.GetEnumerator();

      // determine how many items in the list
      int iCount = 0;
      if(arList is IList)
      {
        iCount = ((IList)arList).Count;
      }
      else
      {
        while(oEnumerator.MoveNext())
        {
          iCount++;
        }

        oEnumerator.Reset();
        oEnumerator.MoveNext();
      }

      // Set column types and indices
      if (iCount > 0 && Levels.Count > 0)
      {
        int iColumnIndex = 0;
        foreach(GridColumn oColumn in Levels[0].Columns)
        {
          object o = arList is IList ? ((IList)arList)[0] : oEnumerator.Current;
          BindingFlags oFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | (UseShallowObjectBinding ? BindingFlags.DeclaredOnly : BindingFlags.FlattenHierarchy);
              
          if(oColumn.DataField.IndexOf(".") > 0)
          {
            string [] arProperties = oColumn.DataField.Split('.');
            
            for(int i = 0; i < arProperties.Length; i++)
            {
              PropertyInfo oProperty = o.GetType().GetProperty(arProperties[i], oFlags);
              if(oProperty != null)
              {
                if(i == arProperties.Length - 1)
                {
                  oColumn.DataType = oProperty.PropertyType;
                }
                else
                {
                  o = oProperty.GetValue(o, null);
                }
              }
            }
          }
          else
          {
            PropertyInfo oProperty = o.GetType().GetProperty(oColumn.DataField, oFlags);
            if(oProperty != null)
            {
              oColumn.DataType = oProperty.PropertyType;
            }
          }

          oColumn.ColumnIndex = iColumnIndex;
          iColumnIndex++;
        }
      }

      // Get some info about the dataset
      this.RecordCount = iCount;

      this.LoadGridItems(Items, GetPagedObjects(oEnumerator, iCount), 0);
    }

    private void ExecuteScriptStateChanges(ArrayList arCommands)
    {
      foreach(string sCommand in arCommands)
      {
        string [] arArgs = sCommand.Split(' ');
        switch(arArgs[0])
        {
          case "ADDROW":
            _addRow = true;
            break;
          case "CHECK":
            if(arArgs.Length == 5)
            {
              this.CheckedList.Add(arArgs[1] + " " + arArgs[2] + " " + arArgs[3] + " " + arArgs[4]);
            }
            break;
          case "EDIT":
            if(arArgs.Length == 4)
            {
              int iEditLevel = int.Parse(arArgs[1]);
              string sEditKey = arArgs[2];
                
              _editingId = iEditLevel + " " + sEditKey;

              string sEditRowXml = HttpUtility.UrlDecode(arArgs[3], Encoding.UTF8);
                
              GridItem oItem = new GridItem(this, iEditLevel,
                GetXmlValues(Levels[iEditLevel].Columns, sEditRowXml));

              _editingItem = oItem;
            }
            break;
          case "INSERT":
            _haveUpdate = true;
            break;
          case "MSELECT":
            if(arArgs.Length == 4)
            {
              this.SelectedList.Add(arArgs[1] + " " + arArgs[2] + " " + arArgs[3]);
            }
            break;
          case "SEARCH":
            ClientSearchString = HttpUtility.UrlDecode(arArgs[1]);
            break;
          case "SELECT":
            this.SelectedList.Clear();
            if (arArgs.Length == 4)
            {
              this.SelectedList.Add(arArgs[1] + " " + arArgs[2] + " " + arArgs[3]);
            }
            else
            {
              this.SelectedList.Add(arArgs[1] + " " + arArgs[2]);
            }
            break;
          case "UNCHECK":
            string sCheckedIdStartsWith = arArgs[1] + " " + arArgs[2] + " " + arArgs[3];
            for(int i = 0; i < this.CheckedList.Count; i++)
            {
              string sItem = (string)this.CheckedList[i];
              if(sItem.StartsWith(sCheckedIdStartsWith))
              {
                this.CheckedList.RemoveAt(i);
                i--;
              }
            }
            break;
          case "UNSELECT":
            string sSelectedIdStartsWith = arArgs[1] + " " + arArgs[2];
            for(int i = 0; i < this.SelectedList.Count; i++)
            {
              string sItem = (string)this.SelectedList[i];
              if(sItem.StartsWith(sSelectedIdStartsWith))
              {
                this.SelectedList.RemoveAt(i);
                i--;
              }
            }
            break;
          case "UPDATE":
            _haveUpdate = true;
            break;
          default:
            break;
        }
      }
    }

    private void ExecuteScript(ArrayList arCommands)
    {
      try
      {
        bool bNeedRebind = false;

        foreach(string sCommand in arCommands)
        {
          // execute command
          string [] arArgs = sCommand.Split(' ');

          switch(arArgs[0])
          {
            case "ADDROW":
              int iAddLevel = int.Parse(arArgs[1]);
              this.RecordCount++;
              this.CurrentPageIndex = this.PageCount - 1;
              bNeedRebind = true;
              break;
            case "CHECK":
              if(arArgs.Length == 5)
              {
                if(this.ItemCheckChanged != null)
                {
                  int iCheckLevel = int.Parse(arArgs[1]);
                  int iCheckCol = int.Parse(arArgs[3]);
                  string sCheckRowXml = HttpUtility.UrlDecode(arArgs[4], Encoding.UTF8);
              
                  GridItem oItem = new GridItem(this, iCheckLevel,
                    GetXmlValues(Levels[iCheckLevel].Columns, sCheckRowXml));

                  if(iCheckLevel == 0 && Levels[0].DataKeyField != "")
                  {
                    int iIndex = Items.IndexOf(Levels[0].DataKeyField, oItem[Levels[0].DataKeyField]);
                    if(iIndex >= 0)
                    {
                      // pass in the actual item in the collection, not a copy
                      oItem = Items[iIndex];
                    } 
                  }

                  GridItemCheckChangedEventArgs oArgs = new GridItemCheckChangedEventArgs(oItem, this.Levels[iCheckLevel].Columns[iCheckCol], true);
                  this.OnItemCheckChanged(oArgs);
                }
              }
              break;
            case "COLLAPSE":
              if(arArgs.Length == 3)
              {
                this.ExpandedList.Remove(arArgs[1] + " " + arArgs[2]);
              }
              break;
            case "COLGRP":
              if (arArgs.Length == 3)
              {
                int iGroupLevel = int.Parse(arArgs[1]);
                string sGroupPath = arArgs[2];

                this.ExpandedGroupInfo.Remove(sGroupPath);
              }
              break;
            case "DELETE":
              if(this.DeleteCommand != null)
              {
                int iDeleteLevel = int.Parse(arArgs[1]);
                string sDeleteRowXml = HttpUtility.UrlDecode(arArgs[2], Encoding.UTF8);
              
                GridItem oItem = new GridItem(this, iDeleteLevel,
                  GetXmlValues(Levels[iDeleteLevel].Columns, sDeleteRowXml));

                if(iDeleteLevel == 0 && Levels[0].DataKeyField != "")
                {
                  int iIndex = Items.IndexOf(Levels[0].DataKeyField, oItem[Levels[0].DataKeyField]);
                  if(iIndex >= 0)
                  {
                    // pass in the actual item in the collection, not a copy
                    oItem = Items[iIndex];
                  } 
                }

                GridItemEventArgs oArgs = new GridItemEventArgs(oItem);
                this.OnDeleteCommand(oArgs);
                bNeedRebind = true;
              }
              break;
            case "EDIT":
              break;
            case "EXPAND":
              if(arArgs.Length == 4)
              {
                GridItem oItem = this.GetItemFromPath(arArgs[3]);

                // only do anything if it isn't already expanded
                if(oItem != null && !this.IsExpanded(oItem))
                {
                  // do we need to load on demand?
                  if(!this.PreloadLevels && this.NeedChildDataSource != null && oItem.Items.Count == 0)
                  {
                    int iExpandLevel = int.Parse(arArgs[1]);

                    // do we not have this level created yet?
                    if(this.SelfReferencing && this.Levels.Count <= iExpandLevel + 1)
                    {
                      // clone the first one
                      this.Levels.Add(this.Levels[0].Clone());
                    }

                    GridNeedChildDataSourceEventArgs args = new GridNeedChildDataSourceEventArgs(oItem);
                    this.OnNeedChildDataSource(args);
                   
                    // bind to datasource?
                    if(args.DataSource != null)
                    {
                      // load rows from datasource
                      this.LoadGridItemsFromDataSource(oItem.Items, args.DataSource, iExpandLevel + 1);
                    }

                    // instantiate templates for new items
                    this.InstantiateServerTemplates(oItem.Items, iExpandLevel + 1);
                  }

                  this.ExpandedList.Add(arArgs[1] + " " + arArgs[2]);
                }
              }
              break;
            case "EXPGRP":
              if (arArgs.Length == 4)
              {
                int iGroupLevel = int.Parse(arArgs[1]);
                string sGroupPath = arArgs[2];
                string sGroupValue = HttpUtility.UrlDecode(arArgs[3], Encoding.UTF8);

                ServerGroup oGroup = new ServerGroup(sGroupValue);
                oGroup.Path = sGroupPath;
                oGroup.Index = int.Parse(sGroupPath.Split('_')[0]);

                if (!ExpandedGroupInfo.ContainsKey(sGroupPath))
                {
                  ExpandedGroupInfo.Add(sGroupPath, oGroup);
                }
              }
              break;
            case "FILTER":
              Levels[0].FilterExpression = HttpUtility.UrlDecode(arArgs[1], Encoding.UTF8);
              if(this.RunningMode != GridRunningMode.Client)
              {
                string sFilterExpression = Levels[0].FilterExpression;

                if(sFilterExpression != this.Filter)
                {
                  this.OnFilterCommand(new GridFilterCommandEventArgs(sFilterExpression));

                  bNeedRebind = true;
                }
              
                this.CurrentPageIndex = 0;
              }
              break;
            case "GROUP":
              int iGroupColumn = int.Parse(arArgs[1]);
              
              // is it an ungroup?
              if (iGroupColumn < 0)
              {
                this.GroupBy = string.Empty;

                if (GroupCommand != null)
                {
                  this.OnGroupCommand(new GridGroupCommandEventArgs(""));
                }

                // clear group expanded info
                this.ExpandedGroupInfo.Clear();
              }
              else
              {
                string sGroupExpression = "";
                string sGroupingInfo = sCommand.Substring(sCommand.IndexOf(' ') + 1);
                string [] arGroupingInfo = sGroupingInfo.Split(',');

                foreach (string sGrouping in arGroupingInfo)
                {
                  string[] arGrouping = sGrouping.Split(' ');

                  iGroupColumn = int.Parse(arGrouping[0]);
                  int iGroupDirection = int.Parse(arGrouping[1]);

                  string sGroupColumn = Levels[0].Columns[iGroupColumn].DataField;
                  if (sGroupColumn.IndexOf(" ") >= 0)
                  {
                    sGroupColumn = "[" + sGroupColumn + "]";
                  }

                  string sGroupDirection = (iGroupDirection == 1 ? "DESC" : "ASC");

                  if (sGroupExpression.Length > 0)
                  {
                    sGroupExpression += ", ";
                  }
                  sGroupExpression += sGroupColumn + " " + sGroupDirection;
                }
                
                if (this.GroupBy != sGroupExpression)
                {
                  this.OnGroupCommand(new GridGroupCommandEventArgs(sGroupExpression));
                  bNeedRebind = true;
                }
              }

              this.CurrentPageIndex = 0;
              break;
            case "INSERT":
              if(this.InsertCommand != null)
              {
                int iInsertLevel = int.Parse(arArgs[1]);
                string sInsertXml = HttpUtility.UrlDecode(arArgs[2], Encoding.UTF8);
                object [] arInsertValues = this.GetXmlValues(Levels[iInsertLevel].Columns, sInsertXml);

                GridItem oInsertItem = new GridItem(this, iInsertLevel, arInsertValues);
                GridItemEventArgs oArgs = new GridItemEventArgs(oInsertItem);

                this.OnInsertCommand(oArgs);

                bNeedRebind = true;
              }
              break;
            case "MOVECOL":
              int iLevel = int.Parse(arArgs[1]);
              int iCol = int.Parse(arArgs[2]);
              int iDestIndex = int.Parse(arArgs[3]);

              ArrayList arColumnOrder = new ArrayList(Levels[iLevel].ColumnDisplayOrder.Split(','));

              int iOldIndex = arColumnOrder.IndexOf(iCol.ToString());
              arColumnOrder.RemoveAt(iOldIndex);

              if(iOldIndex < iDestIndex)
              {
                iDestIndex--;
              }

              if(iDestIndex < arColumnOrder.Count)
              {
                arColumnOrder.Insert(iDestIndex, iCol.ToString());
              }
              else
              {
                arColumnOrder.Add(iCol.ToString());
              }

              Levels[iLevel].ColumnDisplayOrder = string.Join(",", (string [])arColumnOrder.ToArray(typeof(string)));

              this.OnColumnReorder(new GridColumnReorderEventArgs(iOldIndex, iDestIndex));
              break;
            case "MSELECT":
              if(arArgs.Length == 4)
              {
                if(this.SelectCommand != null && arArgs.Length >= 4)
                {
                  int iSelectLevel = int.Parse(arArgs[1]);
                  string sSelectRowXml = HttpUtility.UrlDecode(arArgs[3], Encoding.UTF8);

                  GridItem oItem = new GridItem(this, iSelectLevel,
                    GetXmlValues(Levels[iSelectLevel].Columns, sSelectRowXml));

                  if(iSelectLevel == 0 && Levels[0].DataKeyField != "")
                  {
                    int iIndex = Items.IndexOf(Levels[0].DataKeyField, oItem[Levels[0].DataKeyField]);
                    if(iIndex >= 0)
                    {
                      // pass in the actual item in the collection, not a copy
                      oItem = Items[iIndex];
                    } 
                  }

                  GridItemEventArgs oArgs = new GridItemEventArgs(oItem);
                  this.OnSelectCommand(oArgs);
                }
              }
              break;
            case "PAGE":
              int iNewPage = int.Parse(arArgs[1]);
              if( this.RunningMode != GridRunningMode.Client &&
                this.PageIndexChanged != null )
              {
                this.OnPageIndexChanged(new GridPageIndexChangedEventArgs(iNewPage));
                bNeedRebind = true;
              }
              else
              {
                this.CurrentPageIndex = iNewPage;
              }

              // cancel offset
              this.RecordOffset = 0;
              break;
            case "PGSIZE":
              this.PageSize = int.Parse(arArgs[1]);
              break;
            case "RESIZE":
              if(!this.IsCallback)
              {
                try
                {
                  // don't allow this to break the grid.
                  int iColumn = int.Parse(arArgs[1]);
                  int iWidth = int.Parse(arArgs[2]);
                  this.Levels[0].Columns[iColumn].Width = iWidth;
                }
                catch {}
              }
              break;
            case "SCROLL":
              int iOffset = int.Parse(arArgs[1]);
              if (this.Scroll != null)
              {
                GridScrollEventArgs oArgs = new GridScrollEventArgs(iOffset);
                this.OnScroll(oArgs);
              }
              else
              {
                this.RecordOffset = iOffset;
              }
              bNeedRebind = true;
              break;
            case "SEARCH":
              break;
            case "SELECT":
              if(arArgs.Length == 4)
              {
                if (this.SelectCommand != null && arArgs.Length >= 4)
                {
                  int iSelectLevel = int.Parse(arArgs[1]);
                  string sSelectRowXml = HttpUtility.UrlDecode(arArgs[3], Encoding.UTF8);

                  GridItem oItem = new GridItem(this, iSelectLevel,
                    GetXmlValues(Levels[iSelectLevel].Columns, sSelectRowXml));

                  if(iSelectLevel == 0 && Levels[0].DataKeyField != "")
                  {
                    int iIndex = Items.IndexOf(Levels[0].DataKeyField, oItem[Levels[0].DataKeyField]);
                    if(iIndex >= 0)
                    {
                      // pass in the actual item in the collection, not a copy
                      oItem = Items[iIndex];
                    } 
                  }

                  GridItemEventArgs oArgs = new GridItemEventArgs(oItem);
                  this.OnSelectCommand(oArgs);
                }
              }
              break;
            case "SORT":
              int iSortColumn = int.Parse(arArgs[1]);
              int iSortDescending = int.Parse(arArgs[2]);
              Levels[0].IndicatedSortColumn = Levels[0].Columns[iSortColumn].DataField;
              Levels[0].IndicatedSortDirection = (iSortDescending == 1? "DESC" : "ASC");
              if(this.RunningMode != GridRunningMode.Client)
              {
                string sSortColumn = Levels[0].IndicatedSortColumn;
                if(sSortColumn.IndexOf(" ") >= 0)
                {
                  sSortColumn = "[" + sSortColumn + "]";
                }

                string sSortExpression = sSortColumn + " " + Levels[0].IndicatedSortDirection;

                if(this.SortCommand != null && sSortExpression != this.Sort)
                {
                  this.OnSortCommand(new GridSortCommandEventArgs(sSortExpression));
                  bNeedRebind = true;
                }

                if(this.SelectedList.Count > 0)
                {
                  string sLastSelectedId = (string)this.SelectedList[this.SelectedList.Count - 1];
                  string [] arLastSelectedData = sLastSelectedId.Split(' ');

                  if(arLastSelectedData.Length == 3 && arLastSelectedData[0] == "0")
                  {
                    string sKey = arLastSelectedData[1];
                    string sXml = HttpUtility.UrlDecode(arLastSelectedData[2], Encoding.UTF8);

                    // we're gonna need to bind before we find the new page index.
                    // are we missing a data source?
                    if(_dataView == null)
                    {
                      this.OnNeedDataSource(EventArgs.Empty);
                    }

                    this.OnNeedRebind(EventArgs.Empty);
                    bNeedRebind = false;
                  
                    // do we have it now?
                    if(_dataView != null)
                    {
                      int iIndex = this.GetRowIndexFromKey(_dataView, sKey, sXml, 0);

                      if(iIndex >= 0)
                      {
                        // rebind
                        this.CurrentPageIndex = (int)Math.Floor((double)iIndex / this.PageSize);
                      
                        this.OnNeedRebind(EventArgs.Empty);

                        break;
                      }
                    }
                  }
                }

                this.CurrentPageIndex = 0;
              }
              break;
            case "UNCHECK":
              if(arArgs.Length == 5 && this.ItemCheckChanged != null)
              {
                string sUnCheckXml = arArgs[4];
              
                int iUnCheckLevel = int.Parse(arArgs[1]);
                int iUnCheckCol = int.Parse(arArgs[3]);
                string sUnCheckRowXml = HttpUtility.UrlDecode(sUnCheckXml, Encoding.UTF8);
              
                GridItem oItem = new GridItem(this, iUnCheckLevel,
                  GetXmlValues(Levels[iUnCheckLevel].Columns, sUnCheckRowXml));

                if(iUnCheckLevel == 0 && Levels[0].DataKeyField != "")
                {
                  int iIndex = Items.IndexOf(Levels[0].DataKeyField, oItem[Levels[0].DataKeyField]);
                  if(iIndex >= 0)
                  {
                    // pass in the actual item in the collection, not a copy
                    oItem = Items[iIndex];
                  } 
                }

                GridItemCheckChangedEventArgs oArgs = new GridItemCheckChangedEventArgs(oItem, this.Levels[iUnCheckLevel].Columns[iUnCheckCol], false);
                this.OnItemCheckChanged(oArgs);
              }
              break;
            case "UNSELECT":
              break;
            case "UPDATE":
              if(this.UpdateCommand != null)
              {
                int iEditLevel = int.Parse(arArgs[1]);
                string sEditRowXml = HttpUtility.UrlDecode(arArgs[2], Encoding.UTF8);
                
                GridItem oItem = new GridItem(this, iEditLevel,
                  GetXmlValues(Levels[iEditLevel].Columns, sEditRowXml));

                // only look for the actual item for updates if it persisted in viewstate
                //if(iEditLevel == 0 && Levels[0].DataKeyField != "" && !this.IsCallback && EnableViewState)
                //{
                //  int iIndex = Items.IndexOf(Levels[0].DataKeyField, oItem[Levels[0].DataKeyField]);
                //  if(iIndex >= 0)
                //  {
                //    // pass in the actual item in the collection, not a copy
                //    oItem = Items[iIndex];
                //  } 
                //}

                GridItemEventArgs oArgs = new GridItemEventArgs(oItem);

                this.OnUpdateCommand(oArgs);
                bNeedRebind = true;
              }
              break;
            case "":
              // end of script
              // call for a rebind
              if(bNeedRebind)
              {
                this.OnNeedDataSource(EventArgs.Empty);
                this.OnNeedRebind(EventArgs.Empty);
              }
              break;
            default:
              throw new Exception("Error processing event script: Unknown event: " + HttpUtility.HtmlEncode(arArgs[0]));
          }
        }
      }
      catch(Exception ex)
      {
        if(this.CausedCallback)
        {
          this.HandleCallbackError(ex);
        }
        else
        {
          throw;
        }
      }
    }

    private string GenerateColumnData(GridLevel oLevel)
    {
      StringBuilder oSB = new StringBuilder();
      
      int i = 0;
      foreach(GridColumn oColumn in oLevel.Columns)
      {
        bool bAllowEditing = oColumn.AllowEditing == InheritBool.Inherit? this.AllowEditing : oColumn.AllowEditing == InheritBool.True;
        
        oSB.Append("[");
        oSB.Append(ObjectToJavaScriptString(oColumn.DataField));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.HeadingText));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(GetTypeNum(oColumn.DataType)));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.Width));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.Visible));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.Align.ToString().ToLower()));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.ColumnType.ToString().ToLower()));
        oSB.Append(",");
        if(oColumn.EditControlType != GridColumnEditControlType.Default)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.EditControlType.ToString()));
        }
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString((int)(oColumn.DefaultSortDirection)));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.SortImageJustify));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(
          bAllowEditing &&
          (oColumn.EditCellServerTemplateId != "" ||
            (oColumn.DataCellClientTemplateId == "" && 
            oColumn.DataCellServerTemplateId == "" &&
            (oColumn.DataField != "" || oColumn.ColumnType == GridColumnType.CheckBox))
          )
        ));
        oSB.Append(",");
        if(this.Levels.IndexOf(oLevel) == 0 || this.RunningMode == GridRunningMode.Client || this.RunningMode == GridRunningMode.WebService)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.AllowGrouping == InheritBool.Inherit? oLevel.AllowGrouping : oColumn.AllowGrouping == InheritBool.True));
          oSB.Append(",");
          oSB.Append(ObjectToJavaScriptString(oColumn.AllowSorting == InheritBool.Inherit? oLevel.AllowSorting : oColumn.AllowSorting == InheritBool.True));
          oSB.Append(",");
        }
        else // disallow sorting and grouping by sub-levels for non-client mode
        {
          oSB.Append(",,");
        }
        oSB.Append(ObjectToJavaScriptString(oColumn.AllowReordering == InheritBool.Inherit? oLevel.AllowReordering : oColumn.AllowReordering == InheritBool.True));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.AllowHtmlContent == InheritBool.Inherit? this.AllowHtmlContent : oColumn.AllowReordering == InheritBool.True));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.IsSearchable));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.DataCellServerTemplateId != string.Empty));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.EditCellServerTemplateId != string.Empty));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oColumn.TextWrap));
        oSB.Append(",");
        if(oColumn.HeadingGripImageUrl != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(Utils.ConvertUrl(Context, ImagesBaseUrl, oColumn.HeadingGripImageUrl)));
          oSB.Append(",");
          oSB.Append(ObjectToJavaScriptString(oColumn.HeadingGripImageHeight));
          oSB.Append(",");
          oSB.Append(ObjectToJavaScriptString(oColumn.HeadingGripImageWidth));
          oSB.Append(",");
        }
        else
        {
          oSB.Append(",,,");
        }

        if(oColumn.HeadingImageUrl != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(Utils.ConvertUrl(Context, ImagesBaseUrl, oColumn.HeadingImageUrl)));
          oSB.Append(",");
          oSB.Append(ObjectToJavaScriptString(oColumn.HeadingImageHeight));
          oSB.Append(",");
          oSB.Append(ObjectToJavaScriptString(oColumn.HeadingImageWidth));
          oSB.Append(",");
        }
        else
        {
          oSB.Append(",,,");
        }
        
        if(oColumn.DataCellCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.DataCellCssClass));
        }
        oSB.Append(",");
        if(oColumn.HeadingCellCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.HeadingCellCssClass));
        }
        oSB.Append(",");
        if(oColumn.HeadingTextCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.HeadingTextCssClass));
        }
        oSB.Append(",");
        if(oColumn.DataCellClientTemplateId != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.DataCellClientTemplateId));
        }
        oSB.Append(",");
        if(oColumn.HeadingCellClientTemplateId != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.HeadingCellClientTemplateId));
        }
        oSB.Append(",");
        if(oColumn.SortedDataCellCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.SortedDataCellCssClass));
        }
        else if(oLevel.SortedDataCellCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oLevel.SortedDataCellCssClass));
        }
        oSB.Append(",");
        if(oColumn.SortedHeadingCellCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.SortedHeadingCellCssClass));
        }
        else if(oLevel.SortedHeadingCellCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oLevel.SortedHeadingCellCssClass));
        }
        oSB.Append(",");
        if(oColumn.EditCellCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.EditCellCssClass));
        }
        else if(oLevel.EditCellCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oLevel.EditCellCssClass));
        }
        oSB.Append(",");
        if(oColumn.EditFieldCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.EditFieldCssClass));
        }
        else if(oLevel.EditFieldCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oLevel.EditFieldCssClass));
        }
        oSB.Append(",");
        if(oColumn.ForeignTable != string.Empty)
        {
          oSB.Append(GenerateEditOptions(oColumn));
        }
        oSB.Append(",");
        if(oColumn.CustomEditGetExpression != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.CustomEditGetExpression));
        }
        oSB.Append(",");
        if(oColumn.CustomEditSetExpression != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.CustomEditSetExpression));
        }
        oSB.Append(",");
        if (oColumn.FooterCellClientTemplateId != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.FooterCellClientTemplateId));
        }
        oSB.Append(",");
        if (oColumn.ContextMenuId != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.ContextMenuId));
        }
        oSB.Append(",");
        if (oColumn.ContextMenuHotSpotCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.ContextMenuHotSpotCssClass));
        }
        oSB.Append(",");
        if (oColumn.ContextMenuHotSpotHoverCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.ContextMenuHotSpotHoverCssClass));
        }
        oSB.Append(",");
        if (oColumn.ContextMenuHotSpotActiveCssClass != string.Empty)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.ContextMenuHotSpotActiveCssClass));
        }
        oSB.Append(",");
        if (oColumn.Tag != null)
        {
          oSB.Append(ObjectToJavaScriptString(oColumn.Tag));
        }
        if (oColumn.FixedWidth)
        {
          oSB.Append(",1");
        }
        oSB.Append("]");
          
        if(i < oLevel.Columns.Count - 1)
        {
          oSB.Append(",");
        }

        i++;
      }
      
      return oSB.ToString();
    }

    private string GenerateConditionalFormats(GridConditionalFormatCollection arFormats)
    {
      StringBuilder oSB = new StringBuilder();
      
      int i = 0;
      foreach(GridConditionalFormat oFormat in arFormats)
      {
        oSB.Append("[");
        oSB.Append(ObjectToJavaScriptString(oFormat.ClientFilter));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oFormat.RowCssClass));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oFormat.HoverRowCssClass));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oFormat.SelectedRowCssClass));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oFormat.SelectedHoverRowCssClass));
        oSB.Append("]");

        if(i < arFormats.Count - 1)
        {
          oSB.Append(",");
        }

        i++;
      }
      
      return oSB.ToString();
    }

    private string GenerateDisplayOrder(GridLevel oLevel)
    {
      StringBuilder oSB = new StringBuilder();
      
      string sDisplayOrder = oLevel.ColumnDisplayOrder;

      if(sDisplayOrder != "")
      {
        string [] arIndices = sDisplayOrder.Split(',');

        bool bFirst = true;
        foreach(string sIndex in arIndices)
        {
          int iIndex = int.Parse(sIndex);
          if(oLevel.Columns[iIndex].Visible)
          {
            if(!bFirst)
            {
              oSB.Append(",");
            }
            oSB.Append(sIndex);
            bFirst = false;
          }
        }
      }

      return oSB.ToString();
    }

    private string GenerateEditOptions(GridColumn oColumn)
    {
      StringBuilder oSB = new StringBuilder();
      oSB.Append("[");

      if(oColumn.ForeignData != "")
      {
        string [] arValues = oColumn.ForeignData.Split('\n');
              
        for(int i = 0; i < arValues.Length; i+=2)
        {
          oSB.AppendFormat("[{1},{0}]", ObjectToJavaScriptString(arValues[i]), ObjectToJavaScriptString(arValues[i+1]));

          if(i < arValues.Length - 2)
          {
            oSB.Append(",");
          }
        }
      }

      oSB.Append("]");
      return oSB.ToString();
    }

    private string GenerateExpandedGroupList(Hashtable expandedGroupInfo)
    {
      StringBuilder oSB = new StringBuilder();

      foreach (object o in expandedGroupInfo.Keys)
      {
        if (oSB.Length > 0)
        {
          oSB.Append(",");
        }
        oSB.Append(o + ":" + ((ServerGroup)expandedGroupInfo[o]).RenderCount);
      }

      return oSB.ToString();
    }

    private string GenerateLevelData(string sDataGridVarName)
    {
      StringBuilder oSB = new StringBuilder();
      ArrayList arProcessedRelations = new ArrayList();

      oSB.Append(sDataGridVarName + ".Levels = new Array();\n");
      
      for(int iLevel = 0; iLevel < Levels.Count; iLevel++)
      {
        GridLevel oLevel = Levels[iLevel];

        string sPropVar = sDataGridVarName + "_level_" + iLevel;
        oSB.Append(sPropVar + " = [\n");

        // Declare properties
        oSB.Append("['TableName','" + oLevel.TableName + "'],");
        oSB.Append("['Columns',[" + this.GenerateColumnData(oLevel) + "]],");
        oSB.Append("['ColumnDisplayOrder',[" + this.GenerateDisplayOrder(oLevel) + "]],");
        if(oLevel.ConditionalFormats.Count > 0)
        {
          oSB.Append("['ConditionalFormats',[" + this.GenerateConditionalFormats(oLevel.ConditionalFormats) + "]],");
        }
        if (oLevel.ColumnGroupIndicatorImageUrl != string.Empty) oSB.Append("['ColumnGroupIndicatorImageUrl','" + Utils.ConvertUrl(Context, this.ImagesBaseUrl, oLevel.ColumnGroupIndicatorImageUrl) + "'],");
        if (oLevel.ColumnReorderIndicatorImageUrl != string.Empty) oSB.Append("['ColumnReorderIndicatorImageUrl','" + Utils.ConvertUrl(Context, this.ImagesBaseUrl, oLevel.ColumnReorderIndicatorImageUrl) + "'],");
        if(oLevel.DataKeyField != string.Empty) oSB.Append("['DataKeyField'," + oLevel.Columns.IndexOf(oLevel.Columns[oLevel.DataKeyField]) + "],");
        if (oLevel.DataMember != string.Empty) oSB.Append("['DataMember'," + Utils.ConvertStringToJSString(oLevel.DataMember) + "],");
        if (oLevel.IndicatedSortColumn != string.Empty)
        {
          oSB.Append("['IndicatedSortColumn'," + oLevel.Columns.IndexOf(oLevel.IndicatedSortColumn) + "],");
          oSB.Append("['IndicatedSortDirection'," + (string.Equals(oLevel.IndicatedSortDirection, "DESC", StringComparison.InvariantCultureIgnoreCase)? 1 : 0) + "],");
        }
        if(oLevel.FilterExpression != string.Empty && this.RunningMode == GridRunningMode.Client)
        {
          oSB.Append("['FilterExpression','" + oLevel.FilterExpression.Replace("'", "\\'") + "'],");
        }

        // output level properties
        if(oLevel.AlternatingRowCssClass != string.Empty) oSB.Append("['AlternatingRowCssClass','" + oLevel.AlternatingRowCssClass + "'],");
        if(oLevel.AlternatingHoverRowCssClass != string.Empty) oSB.Append("['AlternatingHoverRowCssClass','" + oLevel.AlternatingHoverRowCssClass + "'],");
        if(oLevel.DataCellCssClass != string.Empty) oSB.Append("['DataCellCssClass','" + oLevel.DataCellCssClass + "'],");
        if(oLevel.EditCommandClientTemplateId != string.Empty) oSB.Append("['EditCommandClientTemplateId','" + oLevel.EditCommandClientTemplateId + "'],");
        if (oLevel.ShowFooterRow)
        {
          oSB.Append("['ShowFooterRow',1],");
          if (oLevel.FooterRowClientTemplateId != string.Empty) oSB.Append("['FooterRowClientTemplateId','" + oLevel.FooterRowClientTemplateId + "'],");
          if (oLevel.FooterRowCssClass != string.Empty) oSB.Append("['FooterRowCssClass','" + oLevel.FooterRowCssClass + "'],");
        }
        if(oLevel.GroupHeadingClientTemplateId != string.Empty) oSB.Append("['GroupHeadingClientTemplateId','" + oLevel.GroupHeadingClientTemplateId + "'],");
        if(oLevel.GroupHeadingCssClass != string.Empty) oSB.Append("['GroupHeadingCssClass','" + oLevel.GroupHeadingCssClass + "'],");
        if(oLevel.HeadingCellCssClass != string.Empty) oSB.Append("['HeadingCellCssClass','" + oLevel.HeadingCellCssClass + "'],");
        if(oLevel.HeadingCellActiveCssClass != string.Empty) oSB.Append("['HeadingCellActiveCssClass','" + oLevel.HeadingCellActiveCssClass + "'],");
        if(oLevel.HeadingCellHoverCssClass != string.Empty) oSB.Append("['HeadingCellHoverCssClass','" + oLevel.HeadingCellHoverCssClass + "'],");
        if(oLevel.HeadingRowCssClass != string.Empty) oSB.Append("['HeadingRowCssClass','" + oLevel.HeadingRowCssClass + "'],");
        if(oLevel.HeadingSelectorCellCssClass != string.Empty) oSB.Append("['HeadingSelectorCellCssClass','" + oLevel.HeadingSelectorCellCssClass + "'],");
        if(oLevel.HeadingTextCssClass != string.Empty) oSB.Append("['HeadingTextCssClass','" + oLevel.HeadingTextCssClass + "'],");
        if(oLevel.HoverRowCssClass != string.Empty) oSB.Append("['HoverRowCssClass','" + oLevel.HoverRowCssClass + "'],");
        if(oLevel.InsertCommandClientTemplateId != string.Empty) oSB.Append("['InsertCommandClientTemplateId','" + oLevel.InsertCommandClientTemplateId + "'],");
        if(oLevel.RowCssClass != string.Empty) oSB.Append("['RowCssClass','" + oLevel.RowCssClass + "'],");
        if(oLevel.SelectedRowCssClass != string.Empty) oSB.Append("['SelectedRowCssClass','" + oLevel.SelectedRowCssClass + "'],");
        if(oLevel.SelectorImageUrl != string.Empty)
        {
          oSB.Append("['SelectorImageUrl','" + Utils.ConvertUrl(Context, ImagesBaseUrl, oLevel.SelectorImageUrl) + "'],");
          oSB.Append("['SelectorImageHeight'," + oLevel.SelectorImageHeight + "],");
          oSB.Append("['SelectorImageWidth'," + oLevel.SelectorImageWidth + "],");
        }
        if(oLevel.SelectorCellCssClass != string.Empty) oSB.Append("['SelectorCellCssClass','" + oLevel.SelectorCellCssClass + "'],");
        oSB.Append("['SelectorCellWidth'," + oLevel.SelectorCellWidth + "],");
        if(oLevel.ShowHeadingCells) oSB.Append("['ShowHeadingCells',1],");
        if(oLevel.ShowSelectorCells) oSB.Append("['ShowSelectorCells',1],");
        if(oLevel.ShowSortHeadings) oSB.Append("['ShowSortHeadings',1],");
        if(oLevel.ShowTableHeading) oSB.Append("['ShowTableHeading',1],");
        if(oLevel.SortAscendingImageUrl != string.Empty) oSB.Append("['SortAscendingImageUrl','" + Utils.ConvertUrl(Context, ImagesBaseUrl, oLevel.SortAscendingImageUrl) + "'],");
        if(oLevel.SortDescendingImageUrl != string.Empty) oSB.Append("['SortDescendingImageUrl','" + Utils.ConvertUrl(Context, ImagesBaseUrl, oLevel.SortDescendingImageUrl) + "'],");
        if(oLevel.SortAscendingImageUrl != string.Empty || oLevel.SortDescendingImageUrl != string.Empty)
        {
          oSB.Append("['SortImageHeight'," + oLevel.SortImageHeight + "],");
          oSB.Append("['SortImageWidth'," + oLevel.SortImageWidth + "],");
        }
        if(oLevel.SortedDataCellCssClass != string.Empty) oSB.Append("['SortedDataCellCssClass','" + oLevel.SortedDataCellCssClass + "'],");
        if(oLevel.SortedHeadingCellCssClass != string.Empty) oSB.Append("['SortedHeadingCellCssClass','" + oLevel.SortedHeadingCellCssClass + "'],");
        if (oLevel.SortHeadingCssClass != string.Empty) oSB.Append("['SortHeadingCssClass','" + oLevel.SortHeadingCssClass + "'],");
        if (oLevel.SortHeadingClientTemplateId != string.Empty) oSB.Append("['SortHeadingClientTemplateId','" + oLevel.SortHeadingClientTemplateId + "'],");
        if (oLevel.TableHeadingCssClass != string.Empty) oSB.Append("['TableHeadingCssClass','" + oLevel.TableHeadingCssClass + "'],");
        if(oLevel.TableHeadingClientTemplateId != string.Empty) oSB.Append("['TableHeadingClientTemplateId','" + oLevel.TableHeadingClientTemplateId + "'],");
      
        oSB.Append("];\n");

        // Set properties
        oSB.Append(sDataGridVarName + ".Levels[" + iLevel + "] = new ComponentArt_GridLevel();\n");
        oSB.AppendFormat("ComponentArt_SetProperties({0}, {1});\n", sDataGridVarName + ".Levels[" + iLevel + "]", sPropVar);
      }

      return oSB.ToString();
    }

    private string[] SplitFieldAndDirection(string sFieldAndDirection, string sDefaultDirection)
    {
      // are there multiple fields? ignore all but the first
      if (sFieldAndDirection.IndexOf(",") > 0)
      {
        sFieldAndDirection = sFieldAndDirection.Substring(0, sFieldAndDirection.IndexOf(","));
      }

      string[] arSplitValues = new string[2];

      if (sFieldAndDirection.IndexOf(" ") > 0)
      {
        arSplitValues[0] = sFieldAndDirection.Substring(0, sFieldAndDirection.LastIndexOf(' '));
        arSplitValues[1] = sFieldAndDirection.Substring(sFieldAndDirection.LastIndexOf(' ') + 1);
      }
      else
      {
        arSplitValues[0] = sFieldAndDirection;
        arSplitValues[1] = sDefaultDirection;
      }

      return arSplitValues;
    }

    private string GenerateStateEventList()
    {
      string[] arStateEvents = { "SORT", "PAGE", "FILTER", "GROUP", "SEARCH" };

      StringBuilder oSB = new StringBuilder();

      if (this.EventList != null)
      {
        foreach (string sEvent in this.EventList)
        {
          foreach (string sPrefix in arStateEvents)
          {
            if (sEvent.StartsWith(sPrefix))
            {
              oSB.Append(sEvent + ";");
            }
          }
        }
      }

      return oSB.ToString();
    }

    private string GenerateGroupList(string sGroupBy)
    {
      ArrayList arConvertedList = new ArrayList();

      if (sGroupBy != "")
      {
        string[] arGroupings = sGroupBy.Split(',');

        foreach (string sGrouping in arGroupings)
        {
          string[] arSections = sGrouping.Trim().Split(' ');

          string sColumnName = GetGroupColumn(arSections);

          int iGroupColumn = this.Levels[0].Columns.IndexOf(sColumnName);
          int iGroupDirection = (arSections.Length > 1 && arSections[arSections.Length - 1].ToUpper() == "DESC") ? 1 : 0;

          arConvertedList.Add(new ArrayList(new object[] { iGroupColumn, iGroupDirection }));
        }
      }

      return (new JavaScriptArray(arConvertedList)).ToString();
    }

    private string GenerateList(ArrayList arList, int iHowManyElements)
    {
      ArrayList arRealList;

      if(iHowManyElements > 0)
      {
        arRealList = new ArrayList();

        foreach(string sItem in arList)
        {
          string [] arElements = sItem.Split(' ');
          arRealList.Add(string.Join(" ", arElements, 0, iHowManyElements));
        }
      }
      else
      {
        arRealList = arList;
      }

      return arRealList.Count > 0 ? "['" + string.Join("','", (string [])(arRealList.ToArray(typeof(string)))) + "']" : "[]";
    }

    private string GenerateItems(GridItemCollection arItems, int iLevel)
    {
      StringBuilder oSB = new StringBuilder();
      
      int iItem = 0;
      foreach(GridItem oItem in arItems)
      {
        oSB.Append("[");

        int iCol = 0;
        foreach(GridColumn oColumn in Levels[iLevel].Columns)
        {
          if(oColumn.DataField != "")
          {
            object valueObject = oItem[oColumn.DataField];
            object displayObject = null;

            if(oColumn.ForeignData != "")
            {
              string sValueString = valueObject.ToString();
            
              string [] arValues = oColumn.ForeignData.Split('\n');
              
              for(int i = 0; i < arValues.Length; i+=2)
              {
                if(arValues[i] == sValueString)
                {
                  displayObject = arValues[i+1];
                  break;
                }
              }
            }

            oSB.Append(GetClientValue(valueObject, oColumn, displayObject));
          }
          
          if(iCol < Levels[iLevel].Columns.Count - 1)
          {
            oSB.Append(",");
          }

          iCol++;
        }
        
        // The last item is always a child table (could be null or empty).
        if(oItem.Items.Count > 0)
        {
          oSB.Append(",[");
          oSB.Append(GenerateItems(oItem.Items, iLevel + 1));
          oSB.Append("]");
        }

        oSB.Append("]");

        if(iItem < arItems.Count - 1)
        {
          oSB.Append(",");
        }

        iItem++;
      }

      return oSB.ToString();
    }

    private string GenerateServerGroups(ArrayList groups)
    {
      StringBuilder oSB = new StringBuilder();

      // send out groups, as well as lists of item indices in each. go through items to figure out group matches for this
      foreach (ServerGroup oGroup in groups)
      {
        if (oSB.Length > 0)
        {
          oSB.Append(",");
        }
        oSB.Append("['" + oGroup.Path + "'," + Utils.ConvertStringToJSString(oGroup.Value.ToString()) + "," + (new JavaScriptArray(oGroup.SubGroup)).ToString() + "]");
      }
      return "[" + oSB.ToString() + "]";
    }

    // Return a JS array containing all the data the client needs to work with.
    private string GenerateStorage(GridItemCollection arItems, int iLevel)
    {
      StringBuilder oSB = new StringBuilder();
      oSB.Append("[");

      oSB.Append(GenerateItems(arItems, iLevel));
      
      oSB.Append("]");
      return oSB.ToString();
    }

    private string GenerateXmlData(GridItemCollection arItems, int iLevel)
    {
      StringBuilder oSB = new StringBuilder();
      
      oSB.Append("<Data><![CDATA[");
      oSB.Append(GenerateStorage(arItems, iLevel));
      oSB.Append("]]></Data>");

      string sXml = oSB.ToString();

      return sXml;
    }

    private string GenerateXmlGroups()
    {
      StringBuilder oSB = new StringBuilder();

      oSB.Append("<Groups><![CDATA[");
      oSB.Append(GenerateServerGroups(this.ServerGroups));
      oSB.Append("]]></Groups>");

      string sXml = oSB.ToString();

      return sXml;
    }

    private string GenerateXmlParams(bool bOutOfBand)
    {
      StringBuilder oSB = new StringBuilder();

      oSB.Append("<Params>");

      if (!bOutOfBand)
      {
        oSB.AppendFormat("<CurrentPageIndex>{0}</CurrentPageIndex>", this.CurrentPageIndex);
        oSB.AppendFormat("<PageCount>{0}</PageCount>", this.PageCount);
        oSB.AppendFormat("<RecordCount>{0}</RecordCount>", this.RecordCount);
        oSB.AppendFormat("<RecordOffset>{0}</RecordOffset>", this.RecordOffset);
        oSB.AppendFormat("<NewSelectedKeys>{0}</NewSelectedKeys>", new JavaScriptArray(this.SelectedKeys));
        oSB.AppendFormat("<CallbackParameter>{0}</CallbackParameter>", this.CallbackParameter.Replace("&", "&amp;"));
        oSB.AppendFormat("<ExpandedGroups>{0}</ExpandedGroups>", GenerateExpandedGroupList(this.ExpandedGroupInfo));
        if (this.NeedServerGroups) oSB.AppendFormat("<ServerGrouping>true</ServerGrouping>");
        if (_serverGroupsContinued) oSB.AppendFormat("<ServerGroupsContinued>true</ServerGroupsContinued>");
        if (_serverGroupsContinuing) oSB.AppendFormat("<ServerGroupsContinuing>true</ServerGroupsContinuing>");
      }
      else
      {
        // in an out-of-band situation, report which page the data is for, to prevent async problems
        oSB.AppendFormat("<OutOfBandPageIndex>{0}</OutOfBandPageIndex>", this.CurrentPageIndex);
        oSB.AppendFormat("<OutOfBandRecordCount>{0}</OutOfBandRecordCount>", this.Items.Count);
      }

      oSB.Append("</Params>");

      return oSB.ToString();
    }

    private string GenerateXmlTemplates(GridItemCollection arItems, int iLevel)
    {
      StringBuilder oSB = new StringBuilder();
      
      oSB.Append("<Templates>");
      
      foreach(Control oControl in this.Controls)
      {
        string sName = oControl.ID;

        oSB.AppendFormat("<{0}><![CDATA[", sName);
              
        StringBuilder oControlSB = new StringBuilder();
        StringWriter oStringWriter = new StringWriter(oControlSB);
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter, string.Empty);
        oControl.RenderControl(oWriter);
        oWriter.Flush();
        oStringWriter.Flush();

        oSB.Append(oControlSB.ToString().Replace("]]>", "$$$CART_CDATA_CLOSE$$$"));
        oSB.AppendFormat("]]></{0}>", sName);
      }

      oSB.Append("</Templates>");

      string sXml = oSB.ToString(); 

      return sXml;
    }

    private DataRow [] GetAllRows(DataView oDataView)
    {
      DataRow [] arrRows = new DataRow[oDataView.Count];

      for(int iCount = 0; iCount < oDataView.Count; iCount++)
      {
        arrRows[iCount] = oDataView[iCount].Row;
      }
    
      return arrRows;
    }

    private string GetClientValue(object o, GridColumn oColumn, object oDisplayValue)
    {
      if(oDisplayValue != null)
      {
        // Convert display object to safe string
        oDisplayValue = oDisplayValue.ToString().Replace("<", "#%cLt#%");

        // Output a double (value, display value)
        StringBuilder oSB = new StringBuilder();

        oSB.Append("[");
        oSB.Append(ObjectToJavaScriptString(o));
        oSB.Append(",");
        oSB.Append(ObjectToJavaScriptString(oDisplayValue));
        oSB.Append("]");

        return oSB.ToString();
      }
      else if(oColumn.DataType == typeof(DateTime) || oColumn.FormatString != string.Empty)
      {
        if(o != null)
        {
          if(oColumn.DataType == typeof(DateTime) && !(o is DateTime))
          {
            string sDateTime = o.ToString();

            if(sDateTime != "")
            {
              try
              {
                o = DateTime.Parse(o.ToString());
              }
              catch {}
            }
            else
            {
              // null (empty) date
              o = null;
            }
          }

          // Output a double (client, server)
          StringBuilder oSB = new StringBuilder();

          oSB.Append("[");
          oSB.Append(ObjectToJavaScriptString(o));
          oSB.Append(",");
          oSB.Append(ObjectToJavaScriptString(string.Format("{0:" + oColumn.FormatString + "}", o)));
          oSB.Append("]");

          return oSB.ToString();
        }
        else
        {
          return "";
        }
      }
      else if(oColumn.DataType == typeof(string) && o is string && o != null)
      {
        return ObjectToJavaScriptString(((string)o).Replace("<", "#%cLt#%"));
      }

      // last-minute type conversions for strings
      if(o != null && o is string && oColumn.DataType != typeof(string))
      {
        if(oColumn.DataType == typeof(bool))
        {
          o = bool.Parse((string)o);
        }
        else if(oColumn.DataType == typeof(int))
        {
          o = int.Parse((string)o, CultureInfo.InvariantCulture);
        }
        else if(oColumn.DataType == typeof(double))
        {
          o = double.Parse((string)o, CultureInfo.InvariantCulture);
        }
        else if(oColumn.DataType == typeof(decimal))
        {
          o = decimal.Parse((string)o, CultureInfo.InvariantCulture);
        }
        else if (oColumn.DataType == typeof(float))
        {
          o = float.Parse((string)o, CultureInfo.InvariantCulture);
        }
      }

      return ObjectToJavaScriptString(o);
    }

    private string GetGroupColumn(string[] arSections)
    {
      string sColumnName = arSections[0];

      // spaces in column names?
      if (sColumnName[0] == '\"' || sColumnName[0] == '[')
      {
        sColumnName = sColumnName.Substring(1);

        // is this not a pre-split multi-part column name?
        if (sColumnName.EndsWith("\"") || sColumnName.EndsWith("]"))
        {
          sColumnName = sColumnName.Substring(0, sColumnName.Length - 1);
        }
        else
        {
          // append other parts
          int i = 1;
          while (i < arSections.Length && !arSections[i].EndsWith("\"") && !arSections[i].EndsWith("]"))
          {
            sColumnName += " " + arSections[i];
            i++;
          }

          if (i < arSections.Length && arSections[i].ToUpper() != "DESC" && arSections[i].ToUpper() != "ASC")
          {
            sColumnName += " " + arSections[i].Substring(0, arSections[i].Length - 1);
          }
        }
      }

      return sColumnName;
    }

    private string GetFilterForSearch(string sSearch)
    {
      if(Levels.Count > 0)
      {
        StringBuilder oSB = new StringBuilder();

        bool bIsFirst = true;

        for(int i = 0; i < Levels[0].Columns.Count; i++)
        {
          if(Levels[0].Columns[i].DataType == typeof(string))
          {
            if(!bIsFirst)
            {
              oSB.Append(" OR ");
            }
            
            bIsFirst = false;
            oSB.AppendFormat("({0} LIKE '%{1}%')", Levels[0].Columns[i].DataField, sSearch);
          }
        }

        return oSB.ToString();
      }

      return "";
    }

    private object [] GetObjectsFromEnumerable(IEnumerator oEnumerator)
    {
      ArrayList arList = new ArrayList();

      oEnumerator.Reset();
      
      while(oEnumerator.MoveNext())
      {
        arList.Add(oEnumerator.Current);
      }
      
      return arList.ToArray();
    }

    private object [] GetPagedObjects(IEnumerator oEnumerator, int iNumRecords)
    {
      int iStartRow; 
      int iEndRow; 

      // do we not need paging?
      if(ManualPaging || this.RunningMode == GridRunningMode.Client)
      {
        // output everything
        iStartRow = 0;
        iEndRow = iNumRecords;
      }
      else
      {
        if(this.ScrollBar != GridScrollBarMode.Off)
        {
          iStartRow = this.RecordOffset;
        }
        else if (this.GroupBy != "")
        {
          iStartRow = this.CurrentPageIndex * this.GroupingPageSize;
        }
        else
        {
          iStartRow = this.CurrentPageIndex * this.PageSize;
        }

        if (this.RunningMode == GridRunningMode.Callback && CallbackCachingEnabled)
        {
          iEndRow = Math.Min(iStartRow + (this.PageSize * CallbackCacheLookAhead), iNumRecords);
        }
        else
        {
          // we only need regular paging from the server for this sort of grouping
          if (this.GroupBy != "" && this.GroupingMode == GridGroupingMode.ConstantRecords)
          {
            iEndRow = Math.Min(iStartRow + this.GroupingPageSize, iNumRecords);
          }
          else
          {
            iEndRow = Math.Min(iStartRow + this.PageSize, iNumRecords);
          }
        }
      }
 
      if(iStartRow > iEndRow)
      {
        iStartRow = 0;
      }

      oEnumerator.Reset();
      oEnumerator.MoveNext();

      object[] arrObjects;
      
      // do we need to do server-side grouping prep?
      if (this.RunningMode != GridRunningMode.Client && this.GroupBy != "" && this.GroupingMode != GridGroupingMode.ConstantRecords)
      {
        ArrayList objectList = new ArrayList();

        string[] arGrouping = SplitFieldAndDirection(this.GroupBy, "ASC");

        string sColumn = arGrouping[0];

        if (GroupingPageByRow || this.GroupingMode == GridGroupingMode.ConstantRows)
        {
          int iRow = 0;
          int iGroupCount = 0;
          int iGroupEndRow = iStartRow + this.GroupingPageSize;
          object lastValue = Guid.NewGuid();
          object nullValue = lastValue;

          do
          {
            if(oEnumerator.Current != null)
            {
              PropertyInfo oProperty = oEnumerator.Current.GetType().GetProperty(sColumn);

              if (oProperty != null)
              {
                object thisValue = oProperty.GetValue(oEnumerator.Current, null);

                // count groups
                if ((thisValue == null && lastValue != null) ||
                    (thisValue != null && !thisValue.Equals(lastValue)))
                {
                  lastValue = thisValue;
                  iGroupCount++;
                }

                if (iRow >= iStartRow && iRow < iGroupEndRow)
                {
                  objectList.Add(oEnumerator.Current);
                }
              }
            }

            iRow++;
          } while (oEnumerator.MoveNext());

          this.NumGroupings = iGroupCount;
        }
        else
        {
          int iGroupCount = 0;
          int iGroupEndRow = iStartRow + this.GroupingPageSize;
          object lastValue = Guid.NewGuid();
          object nullValue = lastValue;

          do
          {
            if (oEnumerator.Current != null)
            {
              PropertyInfo oProperty = oEnumerator.Current.GetType().GetProperty(sColumn);

              if (oProperty != null)
              {
                object thisValue = oProperty.GetValue(oEnumerator.Current, null);

                // compensate for formatstring
                if (this.Levels.Count > 0)
                {
                  GridColumn oColumn = this.Levels[0].Columns[sColumn];
                  if (oColumn != null && oColumn.FormatString != "")
                  {
                    thisValue = string.Format("{0:" + oColumn.FormatString + "}", thisValue);
                  }
                }
                
                if ((thisValue == null && lastValue != null) ||
                    (thisValue != null && !thisValue.Equals(lastValue)))
                {
                  lastValue = thisValue;
                  iGroupCount++;
                }

                if (iGroupCount - 1 >= iStartRow && iGroupCount - 1 < iGroupEndRow)
                {
                  objectList.Add(oEnumerator.Current);
                }
              }
            }
          } while (oEnumerator.MoveNext());

          this.NumGroupings = iGroupCount;
        }

        arrObjects = (object[])objectList.ToArray(typeof(object));
      }
      else
      {
        arrObjects = new object[iEndRow - iStartRow];

        int iCount = 0;

        while (iCount < iStartRow)
        {
          oEnumerator.MoveNext();
          iCount++;
        }

        // do regular paging
        while (iCount < iEndRow)
        {
          arrObjects[iCount - iStartRow] = oEnumerator.Current;

          oEnumerator.MoveNext();
          iCount++;
        }
      }

      return arrObjects;
    }

    private DataRow [] GetPagedRows(DataView oDataView)
    {
      int iStartRow; 
      int iEndRow; 
      
      // do we not need paging?
      if(ManualPaging || this.RunningMode == GridRunningMode.Client)
      {
        // output everything
        iStartRow = 0;
        iEndRow = oDataView.Count;
      }
      else
      {
        if(this.ScrollBar != GridScrollBarMode.Off)
        {
          iStartRow = this.RecordOffset;
        }
        else if(this.GroupBy != "")
        {
          iStartRow = this.CurrentPageIndex * this.GroupingPageSize;
        }
        else
        {
          iStartRow = this.CurrentPageIndex * this.PageSize;
        }

        if (this.RunningMode == GridRunningMode.Callback && CallbackCachingEnabled)
        {
          iEndRow = Math.Min(iStartRow + (this.PageSize * CallbackCacheLookAhead), oDataView.Count);
        }
        else
        {
          // we only need regular paging from the server for this sort of grouping
          if (this.GroupBy != "" && this.GroupingMode == GridGroupingMode.ConstantRecords)
          {
            iEndRow = Math.Min(iStartRow + this.GroupingPageSize, oDataView.Count);
          }
          else
          {
            iEndRow = Math.Min(iStartRow + this.PageSize, oDataView.Count);
          }
        }
      }
 
      if(iStartRow > iEndRow)
      {
        iStartRow = 0;
      }

      DataRow [] arrRows; 
      
      // do we need to do server-side grouping prep?
      if(this.RunningMode != GridRunningMode.Client && this.GroupBy != "" && this.GroupingMode != GridGroupingMode.ConstantRecords)
      {
        ArrayList rowList = new ArrayList();
        string[] arGrouping = SplitFieldAndDirection(this.GroupBy, "ASC");

        string sColumn = GetGroupColumn(arGrouping);

        if (this.GroupingMode == GridGroupingMode.ConstantRows)
        {
          int iGroupCount = 0;
          int iRenderCount = 0;
          int iGroupEndRow = iStartRow + this.GroupingPageSize;
          object lastValue = Guid.NewGuid();
          object nullValue = lastValue;

          // TODO: consider expanded groups!

          for (int iRow = 0; iRow < oDataView.Count; iRow++)
          {
            object thisValue = oDataView[iRow].Row[sColumn];

            // count groups
            if (thisValue.ToString() != lastValue.ToString())
            {
              lastValue = thisValue;
              iGroupCount++;
              iRenderCount++;
            }

            if (iRenderCount >= iStartRow && iRenderCount < iGroupEndRow)
            {
              rowList.Add(oDataView[iRow].Row);
            }

            iRenderCount++;
          }

          this.NumGroupings = iGroupCount;
        }
        else
        {
          int iGroupCount = 0;
          int iGroupEndRow = iStartRow + this.GroupingPageSize;
          object lastValue = Guid.NewGuid();
          object nullValue = lastValue;

          for (int iRow = 0; iRow < oDataView.Count; iRow++)
          {
            object thisValue = oDataView[iRow].Row[sColumn];

            // compensate for formatstring
            if (this.Levels.Count > 0)
            {
              GridColumn oColumn = this.Levels[0].Columns[sColumn];
              if (oColumn != null && oColumn.FormatString != "")
              {
                thisValue = string.Format("{0:" + oColumn.FormatString + "}", thisValue);
              }
            }

            if (thisValue.ToString() != lastValue.ToString())
            {
              lastValue = thisValue;
              iGroupCount++;
            }

            if (iGroupCount - 1 >= iStartRow && iGroupCount - 1 < iGroupEndRow)
            {
              rowList.Add(oDataView[iRow].Row);
            }
          }

          this.NumGroupings = iGroupCount;
        }

        arrRows = (DataRow [])rowList.ToArray(typeof(DataRow));
      }
      else
      {
        arrRows = new DataRow[iEndRow - iStartRow];

        // do regular paging
        for(int iCount = iStartRow; iCount < iEndRow; iCount++)
        {
          arrRows[iCount - iStartRow] = oDataView[iCount].Row;
        }
      }

      return arrRows;
    }

    private GridItem GetItemFromPath(string sPath)
    {
      GridItem oItem = null;

      string [] arPath = sPath.Split('_');

      int index = int.Parse(arPath[0]);

      if (index < Items.Count)
      {
        oItem = Items[index];

        for (int i = 1; i < arPath.Length; i++)
        {
          index = int.Parse(arPath[i]);

          if (index < oItem.Items.Count)
          {
            oItem = oItem.Items[index];
          }
          else
          {
            return null;
          }
        }
      }
  
      return oItem;
    }

    private int GetRowIndexFromKey(DataView oView, string sKey, string sXml, int iLevel)
    {
      try
      {
        if(oView.Sort != string.Empty)
        {
          string sFirstSortColumn = SplitFieldAndDirection(oView.Sort, "")[0];
          int iFirstSortColumn = Levels[iLevel].Columns.IndexOf(sFirstSortColumn);

          string sSortValue = this.GetXmlValue(sXml, iFirstSortColumn);
          
          int iIndex = oView.Find(sSortValue);

          if(iIndex >= 0)
          {
            // Go to the beginning of sort-matching records
            while(oView[iIndex][sFirstSortColumn].ToString() == sSortValue)
            {
              iIndex--;
            }

            // Go forward through sort matches until we hit a key match
            while(oView[iIndex][Levels[iLevel].DataKeyField].ToString() != sKey)
            {
              iIndex++;
            }

            return iIndex;
          }
        }
        else
        {
          // find index of first match
          oView.ApplyDefaultSort = true;
          return oView.Find(sKey);
        }
      }
      catch
      {
        return -1;
      }
      
      return -1;
    }

    private GridItemCollection GetSelectedItems()
    {
      GridItemCollection arItems = new GridItemCollection();

      foreach(string sId in this.SelectedList)
      {
        string [] arSelectedData = sId.Split(' ');
                
        if(arSelectedData.Length == 3)
        {
          int iLevel = int.Parse(arSelectedData[0]);
          string sKey = arSelectedData[1];
          string sXml = HttpUtility.UrlDecode(arSelectedData[2], Encoding.UTF8);

          object [] arValues = this.GetXmlValues(this.Levels[iLevel].Columns, sXml);

          GridItem oItem = new GridItem(this, iLevel, arValues);

          if(iLevel == 0 && Levels[0].DataKeyField != "")
          {
            int iIndex = Items.IndexOf(Levels[0].DataKeyField, oItem[Levels[0].DataKeyField]);
            if(iIndex >= 0)
            {
              // pass in the actual item in the collection, not a copy
              oItem = Items[iIndex];
            }
          }

          arItems.Add(oItem);
        }
      }

      return arItems;
    }

    private string [] GetSelectedKeys()
    {
      ArrayList arKeys = new ArrayList();

      foreach (string sId in this.SelectedList)
      {
        string[] arSelectedData = sId.Split(' ');

        if (arSelectedData.Length >= 2)
        {
          int iLevel = int.Parse(arSelectedData[0]);
          string sKey = arSelectedData[1];

          arKeys.Add(sKey);
        }
      }

      return (string [])arKeys.ToArray(typeof(string));
    }

    private string GetTemplateId(int iLevel, int iColumn, int iIndex, GridItem oItem)
    {
      string sId = iLevel > 0 && this.Levels[iLevel].DataKeyField != "" ? oItem[this.Levels[iLevel].DataKeyField].ToString() : iIndex.ToString();
      return GetSaneId() + "_" + iLevel + "_" + iColumn + "_" + sId;
    }

    private GridServerTemplate GetTemplateById(string sTemplateId)
    {
      foreach(GridServerTemplate oTemplate in ServerTemplates)
      {
        if(oTemplate.ID == sTemplateId)
        {
          return oTemplate;
        }
      }
      
      return null;
    }

    private int GetTypeNum(Type oType)
    {
      if(oType == typeof(Int16) || oType == typeof(Int32) || oType == typeof(Int64))
      {
        // whole numbers
        return 0;
      }
      else if(oType == null || oType == typeof(String))
      {
        // strings
        return 1;
      }
      else if(oType == typeof(Single) || oType == typeof(Decimal) || oType == typeof(Double))
      {
        // real numbers
        return 2;
      }
      else if(oType == typeof(Boolean))
      {
        // bool
        return 3;
      }
      else if(oType == typeof(DateTime))
      {
        // datetime
        return 4;
      }
      else
      {
        return 99;
      }
    }

    private string GetXmlValue(string sXml, int iCol)
    {
      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.LoadXml(sXml);

      XmlNode oValueNode = oXmlDoc.DocumentElement.ChildNodes[iCol];

      string sValue = oValueNode.InnerXml;
      
      // do we have a value,display pair?
      if(oValueNode.ChildNodes.Count > 0 && oValueNode.FirstChild.ChildNodes.Count == 2)
      {
        sValue = oValueNode.FirstChild.FirstChild.InnerXml;
      }
      
      return sValue;
    }

    private object [] GetXmlValues(GridColumnCollection arColumns, XmlNodeList arNodes)
    {
      int iNumColumns = arColumns.Count;
      object [] arValues = new Object[iNumColumns];

      for(int i = 0; i < iNumColumns; i++)
      {
        XmlNode oValueNode = arNodes.Count > i? arNodes[i] : null;

        string sValue;
        
        if(oValueNode == null)
        {
          sValue = null;
        }
        else if(oValueNode.ChildNodes.Count > 0 && oValueNode.FirstChild.ChildNodes.Count == 2)
        {
          // for dates, we use the display value (due to parsing), not the real value
          if(arColumns[i].DataType == typeof(DateTime))
          {
            sValue = oValueNode.FirstChild.LastChild.InnerXml;
          }
          else
          {
            sValue = oValueNode.FirstChild.FirstChild.InnerXml;
          }
        }
        else
        {
          sValue = oValueNode.InnerXml;
        }

        // what to do with the string value?
        if (arColumns[i].DataType == null || arColumns[i].DataType == typeof(string))
        {
          // undo xml-safety replacements
          if(sValue != null)
          {
            sValue = sValue.Replace("#$cAmp@*", "&");
            sValue = sValue.Replace("#%cLt#%", "<");
          }
          
          arValues[i] = sValue;
        }
        else
        {
          // we have a non-string non-empty value?
          if(sValue != "" && sValue != null)
          {
            // try to convert it to its appropriate type
            GridColumn oColumn = arColumns[i];
            if(oColumn.DataType == typeof(bool))
            {
              arValues[i] = bool.Parse(sValue);
            }
            else if(oColumn.DataType == typeof(int))
            {
              arValues[i] = int.Parse(sValue, CultureInfo.InvariantCulture);
            }
            else if(oColumn.DataType == typeof(double))
            {
              arValues[i] = double.Parse(sValue, CultureInfo.InvariantCulture);
            }
            else if(oColumn.DataType == typeof(decimal))
            {
              arValues[i] = decimal.Parse(sValue, CultureInfo.InvariantCulture);
            }
            else if (oColumn.DataType == typeof(float))
            {
              arValues[i] = float.Parse(sValue, CultureInfo.InvariantCulture);
            }
            else if (oColumn.DataType == typeof(DateTime))
            {
              if(oColumn.FormatString != "")
              {
                arValues[i] = DateTime.ParseExact(sValue, oColumn.FormatString, null);
              }
              else
              {
                arValues[i] = DateTime.Parse(sValue);
              }
            }
            else
            {
              arValues[i] = sValue;
            }
          }
          else
          {
            arValues[i] = null;
          }
        }
      }

      return arValues;
    }

    internal object [] GetXmlValues(GridColumnCollection arColumns, string sXml)
    {
      // make xml-safe.
      sXml = sXml.Replace("&", "#$cAmp@*");

      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.LoadXml(sXml);

      return GetXmlValues(arColumns, oXmlDoc.DocumentElement.ChildNodes);
    }

    private void InstantiateCustomEditTemplates(int iLevel)
    {
      int iColumnIndex = 0;
      foreach(GridColumn oColumn in this.Levels[iLevel].Columns)
      {
        if(oColumn.EditControlType == GridColumnEditControlType.Custom && oColumn.EditCellServerTemplateId != "")
        {
          GridServerTemplate oTemplate = this.GetTemplateById(oColumn.EditCellServerTemplateId);

          if(oTemplate != null)
          {
            GridServerTemplateContainer oContainer = new GridServerTemplateContainer(new GridItem(this, iLevel));
            
            oTemplate.Template.InstantiateIn(oContainer);            
            oContainer.ID = this.ClientObjectId + "_EditTemplate_" + iLevel + "_" + iColumnIndex;

            if (PreBindServerTemplates) oContainer.DataBind();

            this.Controls.Add(oContainer);
          }
        }

        iColumnIndex++;
      }

      if(this.Levels.Count > iLevel + 1)
      {
        this.InstantiateCustomEditTemplates(iLevel + 1);
      }
    }

    private void InstantiateServerTemplates()
    {
      if(Levels.Count == 0)
      {
        return;
      }

      // make sure old templates are not present.
      this.Controls.Clear();

      // render item templates
      InstantiateServerTemplates(Items, 0);

      // render edit templates if we're doing currently doing templated editing
      if(_editingId != null || _addRow || _haveUpdate)
      {
        int iLevel = _editingId != null? int.Parse(_editingId.Split(' ')[0]) : 0;

        int iColumnIndex = 0;
        foreach(GridColumn oColumn in this.Levels[iLevel].Columns)
        {
          if(oColumn.EditCellServerTemplateId != "" && oColumn.EditControlType != GridColumnEditControlType.Custom)
          {
            GridServerTemplate oTemplate = this.GetTemplateById(oColumn.EditCellServerTemplateId);
            
            if(oTemplate != null)
            {
              GridServerTemplateContainer oContainer = _editingItem != null?
                new GridServerTemplateContainer(_editingItem) :
                new GridServerTemplateContainer(new GridItem(this, iLevel));
            
              oTemplate.Template.InstantiateIn(oContainer);
              oContainer.ID = this.ClientObjectId + "_EditTemplate_" + iLevel + "_" + iColumnIndex;
              
              if(PreBindServerTemplates) oContainer.DataBind();

              this.Controls.Add(oContainer);
            }
          }

          iColumnIndex++;
        }
      }

      // render any custom edit controls for the client
      InstantiateCustomEditTemplates(0);
    }

    private void InstantiateServerTemplates(GridItemCollection arItems, int iLevel)
    {
      string sSaneId = this.GetSaneId();
      
      for(int i = 0; i < arItems.Count; i++)
      {
        GridItem oItem = arItems[i];
        
        int iColumnIndex = 0;
        foreach(GridColumn oColumn in Levels[iLevel].Columns)
        {
          if(oColumn.DataCellServerTemplateId != string.Empty)
          {
            GridServerTemplate oTemplate = this.GetTemplateById(oColumn.DataCellServerTemplateId);
          
            if(oTemplate != null)
            {
              GridServerTemplateContainer oContainer = new GridServerTemplateContainer(oItem);
            
              oTemplate.Template.InstantiateIn(oContainer);
              oContainer.ID = GetTemplateId(iLevel, iColumnIndex, i, oItem);

              if (PreBindServerTemplates) oContainer.DataBind();

              this.Controls.Add(oContainer);

              this.OnItemContentCreated(new GridItemContentCreatedEventArgs(oItem, oColumn, (Control)oContainer));
            }
          }

          iColumnIndex++;
        }

        if(oItem.Items.Count > 0)
        {
          InstantiateServerTemplates(oItem.Items, iLevel + 1);
        }
      }
    }

    private bool IsAjaxDisabled()
    {
      if(this.ClientTarget != ClientTargetLevel.Auto)
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
      catch {}

      // is this opera?
      if(sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion < 8)
      {
        return true;
      }
      else if( // We are good if:

        // 1. We have IE 5 or greater on a non-Mac
        (sUserAgent.IndexOf("MSIE") >= 0 && iMajorVersion >= 5 && !Context.Request.Browser.Platform.ToUpper().StartsWith("MAC")) ||

        // 2. We have Gecko-based browser (Mozilla, Firefox, Netscape 6+)
        (sUserAgent.IndexOf("Gecko") >= 0) ||

        // 3. We have recent Opera
        (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion >= 8) ||

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

    private void LoadClientData(string sData)
    {
      try
      {
        if(sData != string.Empty)
        {
          sData = HttpUtility.UrlDecode(sData, Encoding.UTF8);
          
          // make it xml-safe
          sData = sData.Replace("&", "#$cAmp@*");
          
          XmlDocument oXmlDoc = new XmlDocument();
          oXmlDoc.LoadXml(sData);

          XmlNode oRootNode = oXmlDoc.DocumentElement;

          if(oRootNode != null && oRootNode.ChildNodes.Count > 0)
          {
            this.LoadClientXmlRows(oRootNode.ChildNodes, Items, 0);
          }
        }
      }
      catch(Exception ex)
      {
        throw new Exception("Error loading postback data: " + ex);
      }
    }

    private void LoadClientXmlRow(XmlNode oRowNode, GridItemCollection arItems, int iLevel)
    {
      // do we not have this level (self-referencing)? create it
      if (this.SelfReferencing && this.Levels.Count <= iLevel + 1)
      {
        this.Levels.Add(this.Levels[0].Clone());
      }

      GridColumnCollection arColumns = this.Levels[iLevel].Columns;

      object [] arValues = this.GetXmlValues(arColumns, oRowNode.ChildNodes);

      GridItem oItem = new GridItem(this, iLevel, arValues);
      
      arItems.Add(oItem);

      // are there child rows?
      if(oRowNode.ChildNodes.Count > arColumns.Count)
      {
        this.LoadClientXmlRows(oRowNode.ChildNodes[arColumns.Count].FirstChild.ChildNodes, oItem.Items, iLevel + 1);
      }
    }

    private void LoadClientXmlRows(XmlNodeList arNodes, GridItemCollection arItems, int iLevel)
    {
      foreach(XmlNode oRow in arNodes)
      {
        if(oRow.FirstChild != null)
        {
          this.LoadClientXmlRow(oRow.FirstChild, arItems, iLevel);
        }
      }
    }

    private void LoadGridItems(GridItemCollection arItems, object [] arObjects, int iLevel)
    {
      foreach(object oObject in arObjects)
      {
        GridItem oItem = new GridItem(this, iLevel, oObject);
        arItems.Add(oItem);

        if(this.ItemDataBound != null)
        {
          this.OnItemDataBound(new GridItemDataBoundEventArgs(oItem, oObject));
        }

        // add sub-items?
        if(this.Levels.Count - 1 > iLevel)
        {
          if(this.PreloadLevels || this.IsExpanded(oItem))
          {
            GridLevel oNextLevel = this.Levels[iLevel + 1];
            object oNextLevelObject = oObject;
            PropertyInfo oNextLevelProperty = null;
          
            // resolve dotted properties?
            if(oNextLevel.DataMember.IndexOf(".") > 0)
            {
              string [] arProperties = oNextLevel.DataMember.Split('.');

              for(int i = 0; i < arProperties.Length; i++)
              {
                oNextLevelProperty = oNextLevelObject.GetType().GetProperty(arProperties[i]);
                if(oNextLevelProperty != null)
                {
                  oNextLevelObject = oNextLevelProperty.GetValue(oNextLevelObject, null);
                }
              }
            }
            else
            {
              oNextLevelProperty = oObject.GetType().GetProperty(oNextLevel.DataMember);
            }

            if(oNextLevelProperty == null)
            {
              throw new Exception("Property " + oNextLevel.DataMember + " not found in object of type " + oObject.GetType() + ".");
            }

            // Load next level from property
            oNextLevelObject = oNextLevelProperty.GetValue(oNextLevelObject, null);

            if(oNextLevelObject is IEnumerable)
            {
              IEnumerable oEnumerable = (IEnumerable)oNextLevelObject;
              LoadGridItems(oItem.Items, this.GetObjectsFromEnumerable(oEnumerable.GetEnumerator()), iLevel + 1);
            }
            else
            {
              throw new Exception("The type of property " + oNextLevel.DataMember + " (" + oNextLevelObject.GetType() + ") must implement IEnumerable.");
            }
          }
        }
      }
    }

    private void LoadGridItems(GridItemCollection arItems, DataRow [] arRows, int iLevel)
    {
      foreach(DataRow oRow in arRows)
      {
        GridItem oItem = new GridItem(this, iLevel, oRow);
        arItems.Add(oItem);

        if(this.ItemDataBound != null)
        {
          this.OnItemDataBound(new GridItemDataBoundEventArgs(oItem, oRow));
        }

        // add sub-items?
        if(this.Levels.Count - 1 > iLevel)
        {
          if(this.PreloadLevels || this.IsExpanded(oItem))
          {
            GridLevel oNextLevel = this.Levels[iLevel + 1];

            DataRelation oRelation = null;
          
            // do we fire an event to load sub-data?
            if(this.NeedChildDataSource != null)
            {
              GridNeedChildDataSourceEventArgs args = new GridNeedChildDataSourceEventArgs(oItem);
              
              this.OnNeedChildDataSource(args);

              // bind to datasource?
              if(args.DataSource != null)
              {
                // load rows from datasource
                this.LoadGridItemsFromDataSource(oItem.Items, args.DataSource, iLevel + 1);
              }
            }
            else // try to be smart, looking at the data source
            {
              if(oRow.Table.ChildRelations.Count == 0)
              {
                //throw new Exception("Could not find table to use for level " + (iLevel + 1));
                continue;
              }
              else if(oRow.Table.ChildRelations.Count == 1)
              {
                oRelation = oRow.Table.ChildRelations[0];
              }
              else if(oNextLevel.DataMember != string.Empty)
              {
                foreach(DataRelation rel in oRow.Table.ChildRelations)
                {
                  if(rel.ParentTable == oRow.Table && rel.ChildTable.TableName == oNextLevel.DataMember)
                  {
                    oRelation = rel;
                    break;
                  }
                }

                if(oRelation == null)
                {
                  throw new Exception("Could not find DataRelation between " + oRow.Table.TableName + " and " + oNextLevel.DataMember + " for level " + (iLevel + 1));
                }
              }
              else
              {
                //throw new Exception("Must specify DataMember for level " + (iLevel + 1));
                continue;
              }

              DataRow [] arChildRows = oRow.GetChildRows(oRelation);
              if(arChildRows.Length > 0)
              {
                LoadGridItems(oItem.Items, arChildRows, iLevel + 1);
              }
            }
          }
        }
      }
    }

    private void LoadGridItemsFromDataSource(GridItemCollection arItems, object dataSource, int iLevel)
    {
      DataView oView = null;

      // Load the data
      if(dataSource != null)
      {
        if (dataSource is DataView || dataSource is DataSet || dataSource is DataTable)
        {
          if(dataSource is DataView)
          {
            oView = (DataView)dataSource;
          }
          else if(dataSource is DataSet)
          {
            if(this.Levels.Count > 0 && this.Levels[iLevel].DataMember != string.Empty)
            {
              oView = ((DataSet)dataSource).Tables[this.Levels[iLevel].DataMember].DefaultView;
            }
            else
            {
              oView = ((DataSet)dataSource).Tables[0].DefaultView;
            }
          }
          else // DataTable
          {
            oView = ((DataTable)dataSource).DefaultView;
          }

          this.LoadGridItems(arItems, GetAllRows(oView), iLevel);
        }
        else if (dataSource is IEnumerable)
        {
          this.LoadGridItems(arItems, GetObjectsFromEnumerable(((IEnumerable)dataSource).GetEnumerator()), iLevel);
        }
        else
        {
          throw new Exception("Cannot bind to data source of type " + dataSource.GetType().ToString());
        }
      }
    }

    private void LoadGridLevels(DataView oDataView)
    {
      DataTable oTable = oDataView.Table;

      // Load levels/columns
      int iLevel = 0;
      ArrayList arProcessedRelations = new ArrayList();

      while(oTable != null)
      {
        if(iLevel >= Levels.Count)
        {
          // Create level if it hasn't been defined.
          Levels.Add(new GridLevel());
        }

        GridLevel oLevel = Levels[iLevel];

        oLevel.TableName = oTable.TableName;
        
        // Do we need to load columns?
        if(oLevel.Columns.Count == 0)
        {
          foreach(DataColumn oColumn in oTable.Columns)
          {
            GridColumn oGridColumn = new GridColumn();
            oGridColumn.DataField = oColumn.ColumnName;
            oGridColumn.HeadingText = oColumn.Caption;
            oGridColumn.DataType = oColumn.DataType;
            oLevel.Columns.Add(oGridColumn);
          }
        }

        // if datakeyfield is defined, make sure its legit.
        if(oLevel.DataKeyField != string.Empty)
        {
          if(oTable.Columns[oLevel.DataKeyField] == null)
          {
            throw new Exception("DataKeyField '" + oLevel.DataKeyField + "' not found in data source.");
          }
          else if(oLevel.Columns[oLevel.DataKeyField] == null)
          {
            throw new Exception("No GridColumn defined for DataKeyField '" + oLevel.DataKeyField + "' on level " + iLevel + ".");
          }
        }

        // load column types (if first time) and other stuff
        int iColumnIndex = 0;
        foreach(GridColumn oColumn in oLevel.Columns)
        {
          oColumn.ColumnIndex = iColumnIndex;

          if(oColumn.DataField != "")
          {
            if(oTable.Columns[oColumn.DataField] == null)
            {
              throw new Exception("Column '" + oColumn.DataField + "' not found in data source.");
            }
            else if(oColumn.DataType == null)
            {
              oColumn.DataType = oTable.Columns[oColumn.DataField].DataType;
            }
          }

          if(oColumn.ForeignTable != "")
          {
            // load key/value pairs
            DataTable oForeignTable = oDataView.Table.DataSet.Tables[oColumn.ForeignTable];

            if(oForeignTable == null)
            {
              throw new Exception("Foreign table \"" + oColumn.ForeignTable + "\" not found in DataSet.");
            }

            DataView oForeignView = oForeignTable.DefaultView;
               
            ArrayList arForeignData = new ArrayList();

            foreach(DataRowView oRowView in oForeignView)
            {
              arForeignData.Add(oRowView[oColumn.ForeignDataKeyField].ToString());
              arForeignData.Add(oRowView[oColumn.ForeignDisplayField].ToString());
            }
            
            oColumn.ForeignData = string.Join("\n", (string [])arForeignData.ToArray(typeof(string)));
          }

          iColumnIndex++;
        }
        
        if(oTable.ChildRelations.Count > 0 && !arProcessedRelations.Contains(oTable.ChildRelations[0]))
        {
          DataRelation oRelation = oTable.ChildRelations[0];
          arProcessedRelations.Add(oRelation);
          oTable = oRelation.ChildTable;
          
          iLevel++;
        }
        else
        {
          oTable = null;
        }
      }
    }

    private void LoadServerGroupContents(ServerGroup oExpandedGroup, int iSkip, string[] arGroupings, int iGroupLevel)
    {
      // are there subgroups?
      if (arGroupings.Length > iGroupLevel + 1)
      {
        // find expanded sub-groups

        // we need to: 
        // - get all groups under here (problem?), but we have offset (skip) and count (groupingpagesize) to help us
        // - look check which of them are expanded (search for their path in ExpandedGroupInfo

        // Get all potentially visible subgroups for this group
        GridNeedGroupsEventArgs oSubGroupRequestArgs = new GridNeedGroupsEventArgs();
        oSubGroupRequestArgs.Offset = iSkip;
        oSubGroupRequestArgs.Count = this.GroupingPageSize;
        oSubGroupRequestArgs.GroupColumn = SplitFieldAndDirection(arGroupings[iGroupLevel + 1], "")[0];
        oSubGroupRequestArgs.GroupDirection = SplitFieldAndDirection(arGroupings[iGroupLevel + 1], "ASC")[1];
        oSubGroupRequestArgs.Where.Add(new GridDataCondition(SplitFieldAndDirection(arGroupings[iGroupLevel], "")[0], oExpandedGroup.Value));
        this.OnNeedGroups(oSubGroupRequestArgs);

        // update in the expanded list
        ((ServerGroup)this.ExpandedGroupInfo[oExpandedGroup.Path]).RenderCount = oSubGroupRequestArgs.TotalCount;

        // set total count
        oExpandedGroup.RenderCount = oSubGroupRequestArgs.TotalCount;

        int iCount = this.GroupingPageSize;

        // load up groups
        for (int i = 0; i < Math.Min(oSubGroupRequestArgs.Groups.Count, iCount); i++)
        {
          string sGroupValue = oSubGroupRequestArgs.Groups[i].ToString();

          ServerGroup oSubGroup = new ServerGroup(sGroupValue);
          oSubGroup.Index = iSkip + i;
          oSubGroup.Path = oExpandedGroup.Path + "_" + oSubGroup.Index.ToString();
          oSubGroup.ParentGroup = oExpandedGroup;

          // check whether each is expanded
          if (this.ExpandedGroupInfo.ContainsKey(oSubGroup.Path))
          {
            // expand below
            LoadServerGroupContents(oSubGroup, 0, arGroupings, iGroupLevel + 1);

            ((ServerGroup)this.ExpandedGroupInfo[oSubGroup.Path]).RenderCount = oSubGroup.RenderCount;

            iCount -= oSubGroup.RenderCount;
          }

          oExpandedGroup.SubGroup.Add(this.ServerGroups.Count); // index of subgroup
          this.ServerGroups.Add(oSubGroup);
        }
      }
      else
      {
        // fetch records of groups showing up here
        // but only until iCount is filled
        GridNeedGroupDataEventArgs oGroupDataArgs = new GridNeedGroupDataEventArgs();
        oGroupDataArgs.SortExpression = this.GroupBy;

        // put together where clause
        oGroupDataArgs.Where.Add(new GridDataCondition(SplitFieldAndDirection(arGroupings[iGroupLevel], "")[0], oExpandedGroup.Value));
        if (iGroupLevel > 0)
        {
          ServerGroup oAncestorGroup = oExpandedGroup.ParentGroup;
          int iAncestorLevel = iGroupLevel - 1;
          while (oAncestorGroup != null && iAncestorLevel >= 0)
          {
            oGroupDataArgs.Where.Add(new GridDataCondition(SplitFieldAndDirection(arGroupings[iAncestorLevel], "")[0], oAncestorGroup.Value));

            oAncestorGroup = oAncestorGroup.ParentGroup;

            iAncestorLevel--;
          }
        }
        // end where

        // apply sort
        oGroupDataArgs.SortExpression = this.Sort;

        this.OnNeedGroupData(oGroupDataArgs);

        if (oGroupDataArgs.DataSource != null)
        {
          GridItemCollection arItems = new GridItemCollection();
          this.LoadGridItemsFromDataSource(arItems, oGroupDataArgs.DataSource, 0);

          // update in the expanded list
          ((ServerGroup)this.ExpandedGroupInfo[oExpandedGroup.Path]).RenderCount = arItems.Count;

          // Do we need to skip some?
          for (int i = 0; i < iSkip && arItems.Count > 0; i++)
          {
            arItems.RemoveAt(0);
          }

          // Copy to the main items collection (never a need to add more than pagesize worth)
          for (int i = 0; i < Math.Min(arItems.Count, this.GroupingPageSize); i++)
          {
            oExpandedGroup.SubGroup.Add(this.Items.Count);
            this.Items.Add(arItems[i]);
          }

          oExpandedGroup.RenderCount = arItems.Count;
        }
      }
    }

    private string ObjectToJavaScriptString(object item)
    {
      if (item is string)
      {
        return "'" + ((string)item).Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\r", "") + "'";
      }
      else if(item is bool)
      {
        return item.ToString().ToLower();
      }
      else if(item is Enum)
      {
        return ((int)item).ToString();
      }
      else if(item is DateTime)
      {
        return Utils.ConvertDateTimeToJsDate((DateTime)item);
      }
      else if(item is Decimal || item is Double)
      {
        return string.Format(CultureInfo.InvariantCulture, "{0}", item); 
      }
      else if(item is Int16 || item is Int32 || item is Int64)
      {
        return item.ToString();
      }
      else if(item != null)
      {
        return ObjectToJavaScriptString(item.ToString());
      }
      else
      {
        return "";
      }
    }

    private void RenderPreloadImages(HtmlTextWriter output)
    {
      output.Write("<div style=\"position:absolute;top:0px;left:0px;visibility:hidden;\">"); 
      foreach(string sImage in this.PreloadImages)
      {
        output.Write("<img src=\"" + sImage + "\" width=\"0\" height=\"0\" alt=\"\" />\n");
      }
      output.Write("</div>"); 
    }

    private void RenderServerTemplates(HtmlTextWriter output)
    {
      if(this.Controls.Count > 0 || this.RunningMode == GridRunningMode.Callback)
      {
        string sGridId = this.GetSaneId();

        output.AddAttribute(HtmlTextWriterAttribute.Id, sGridId + "_ServerTemplates");
        output.AddStyleAttribute("display", "none");
        output.RenderBeginTag(HtmlTextWriterTag.Div); // <div>

        foreach(Control oControl in this.Controls)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Id, oControl.ID);
          output.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
          oControl.RenderControl(output);
          output.RenderEndTag(); // </div>
        }

        output.RenderEndTag(); // </div>
      }
    }

    #endregion

    #region Accessible Rendering

    private void RenderAccessibleContent(HtmlTextWriter output)
    {
      if (this.Levels.Count > 0)
      {
        // TODO: Implement accessible rendering for multi-level
        GridLevel level = this.Levels[0];

        RenderAccessibleLevel(output, level);
        RenderAccessibleFooter(output);
      }
    }

    private void RenderAccessibleLevel(HtmlTextWriter output, GridLevel level)
    {
      output.Write("<table>");
      RenderAccessibleHeader(output, level);
      RenderAccessibleItems(output, level);
      output.Write("</table>");
    }

    private void RenderAccessibleHeader(HtmlTextWriter output, GridLevel level)
    {
      output.Write("<thead>");
      foreach (GridColumn column in level.Columns)
      {
        output.Write("<th");

        output.Write(" scope=\"col\"");

        string[] cssClassArray = { level.HeadingCellCssClass, column.HeadingCellCssClass, level.HeadingTextCssClass, column.HeadingTextCssClass };
        string cssClassString = String.Join(" ", cssClassArray).Trim();
        if (cssClassString != String.Empty)
        {
          output.Write(" class=\"");
          output.Write(cssClassString);
          output.Write("\"");
        }

        if (column.Width > 0)
        {
          output.Write(" width=\"");
          output.Write(column.Width);
          output.Write("\"");
        }

        output.Write(">");

        output.Write(column.HeadingText != null && column.HeadingText != "" ? column.HeadingText : column.DataField);

        output.Write("</th>");
      }
      output.Write("</thead>");
    }

    private void RenderAccessibleItems(HtmlTextWriter output, GridLevel level)
    {
      foreach (GridItem item in this.Items)
      {
        output.Write("<tr>");
        foreach (GridColumn column in level.Columns)
        {
          output.Write("<td>");
          if (column.DataField != "")
          {
            output.Write(item[column.DataField]);
          }
          output.Write("</td>");
        }
        output.Write("</tr>");
      }
    }

    private void RenderAccessibleFooter(HtmlTextWriter output)
    {
      // UNDONE: Accessible footer not yet implemented
    }

    #endregion Accessible Rendering

    #region Downlevel Rendering

    internal void RenderDesignTime(HtmlTextWriter output)
    {
      // TODO: optionally add some columns and data

      DownLevelRender(output);
    }

    private void DownLevelRender(HtmlTextWriter output)
    {
      output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
      output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
      output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
      output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
      output.RenderBeginTag(HtmlTextWriterTag.Table); // <table>

      if(Levels.Count > 0)
      {
        GridLevel oLevel = Levels[0];

        this.DownLevelRenderHeadings(output, oLevel);

        foreach(GridItem oItem in this.Items)
        {
          if(oLevel.RowCssClass != string.Empty)
          {
            output.AddAttribute(HtmlTextWriterAttribute.Class, oLevel.RowCssClass);
          }
          output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>

          foreach(GridColumn oColumn in oLevel.Columns)
          {
            string sCssClass = oLevel.DataCellCssClass;
            if(oColumn.DataCellCssClass != "") sCssClass += " " + oColumn.DataCellCssClass;
            
            if(sCssClass != string.Empty)
            {
              output.AddAttribute(HtmlTextWriterAttribute.Class, sCssClass);
            }
            output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

            if(oColumn.DataField != "")
            {
              output.Write(oItem[oColumn.DataField]);
            }

            output.RenderEndTag(); // </td>
          }

          output.RenderEndTag(); // </tr>
        }
      }
        
      output.RenderEndTag(); // </table>
    }

    private void DownLevelRenderFooter(HtmlTextWriter output, string sGridVar)
    {
      int iStartPage = Math.Max(0, this.CurrentPageIndex - 5);
      int iEndPage = Math.Min(iStartPage + 10, this.PageCount);

      for(int iPage = iStartPage; iPage < iEndPage; iPage++)
      {
        string sRef = string.Format("javascript:{0}.Page({1});", sGridVar, iPage);
        output.AddAttribute(HtmlTextWriterAttribute.Href, sRef);
        output.RenderBeginTag(HtmlTextWriterTag.A);
        output.Write(iPage.ToString());
        output.RenderEndTag();
        output.Write("&nbsp;");
      }

      output.Write("...");
    }

    private void DownLevelRenderHeadingMargin(HtmlTextWriter output, GridLevel oLevel)
    {
      int iLevel = Levels.IndexOf(oLevel);

      // Render indent cells for each level of ancestry
      for(int i = 0; i < iLevel + 1; i++)
      {
        this.DownLevelRenderIndentCell(output, false);
      }
  
      // grouped? 
      if(this.GroupBy != "")
      {
        this.DownLevelRenderIndentCell(output, false);
      }

      // are there more levels below us?
      if(iLevel < this.Levels.Count - 1)
      {
        // exp/col margin cell
        output.Write("<td class=\"" + oLevel.HeadingSelectorCellCssClass + "\" width=\"" + this.IndentCellWidth + "\"><div style=\"width:" + this.IndentCellWidth + "px;\">&nbsp;</div></td>");
      }

      // selector margin cell
      if(oLevel.ShowSelectorCells)
      {
        output.Write("<td class=\"" + oLevel.HeadingSelectorCellCssClass + "\" width=\"" + oLevel.SelectorCellWidth + "\"><div style=\"width:" + oLevel.SelectorCellWidth + "px;\">&nbsp;</div></td>");
      }
    }

    private void DownLevelRenderHeadings(HtmlTextWriter output, GridLevel oLevel)
    {
      if(oLevel.HeadingRowCssClass != string.Empty)
      {
        output.AddAttribute(HtmlTextWriterAttribute.Class, oLevel.HeadingRowCssClass);
      }
      output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>

      foreach(GridColumn oColumn in oLevel.Columns)
      {
        string sCssClass = oLevel.HeadingCellCssClass;
        if(oColumn.HeadingCellCssClass != "") sCssClass += " " + oColumn.HeadingCellCssClass;
        if(oLevel.HeadingTextCssClass != "") sCssClass += " " + oLevel.HeadingTextCssClass;
        if(oColumn.HeadingTextCssClass != "") sCssClass += " " + oColumn.HeadingTextCssClass;
        
        if(sCssClass != string.Empty)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Class, sCssClass);
        }
        if(oColumn.Width > 0)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Width, oColumn.Width.ToString());
        }
        output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

        output.Write(oColumn.HeadingText);

        output.RenderEndTag(); // </td>
      }

      output.RenderEndTag(); // </tr>
    }

    private void DownLevelRenderIndentCell(HtmlTextWriter output, bool bConsiderLines)
    {
      if(this.IndentCellCssClass != string.Empty)
      {
        output.AddAttribute(HtmlTextWriterAttribute.Class, this.IndentCellCssClass);
      }
      if(this.IndentCellWidth > 0)
      {
        output.AddAttribute(HtmlTextWriterAttribute.Width, this.Width.ToString());
      }
      output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

      if(this.IndentCellWidth > 0)
      {
        output.AddStyleAttribute("width", this.IndentCellWidth.ToString());
      }
      output.RenderBeginTag(HtmlTextWriterTag.Div); // <div>

      if(bConsiderLines && this.TreeLineImagesFolderUrl != string.Empty)
      {
        output.Write("<img border=\"0\" alt=\"\"");
        if(this.TreeLineImageHeight > 0)
        {
          output.Write(" height=\"" + this.TreeLineImageHeight + "\"");
        }
        if(this.TreeLineImageWidth > 0)
        {
          output.Write(" width=\"" + this.TreeLineImageWidth + "\"");
        }
        output.Write(" src=\"" + this.TreeLineImagesFolderUrl + "/i.gif\" />");
      }
      else
      {
        output.Write("<div style=\"height:1px;width:" + this.IndentCellWidth + "px;\"></div>");
      }

      output.RenderEndTag(); // </div>
      output.RenderEndTag(); // </td>
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate for <see cref="InsertCommand"/>, <see cref="SelectCommand"/>, <see cref="UpdateCommand"/>, 
    /// and <see cref="DeleteCommand"/> events of <see cref="Grid"/> class.
    /// </summary>
    public delegate void GridItemEventHandler(object sender, GridItemEventArgs e);

    /// <summary>
    /// Fires after a new item is created.
    /// </summary>
    [ Description("Fires after a new item is created."), Category("Grid Events") ]
    public event GridItemEventHandler InsertCommand;

    private void OnInsertCommand(GridItemEventArgs e) 
    {         
      if (InsertCommand != null) 
      {
        InsertCommand(this, e);
      }   
    }

    /// <summary>
    /// Fires after an update is made.
    /// </summary>
    [ Description("Fires after an update is made."), Category("Grid Events") ]
    public event GridItemEventHandler SelectCommand;

    private void OnSelectCommand(GridItemEventArgs e) 
    {
      if (SelectCommand != null) 
      {
        SelectCommand(this, e);
      }   
    }

    /// <summary>
    /// Fires after an update is made.
    /// </summary>
    [ Description("Fires after an update is made."), Category("Grid Events") ]
    public event GridItemEventHandler UpdateCommand;

    private void OnUpdateCommand(GridItemEventArgs e) 
    {
      if (UpdateCommand != null) 
      {
        UpdateCommand(this, e);
      }   
    }

    /// <summary>
    /// Fires after a delete request is made.
    /// </summary>
    [ Description("Fires after a delete request is made."), Category("Grid Events") ]
    public event GridItemEventHandler DeleteCommand;

    private void OnDeleteCommand(GridItemEventArgs e) 
    {
      if (DeleteCommand != null) 
      {
        DeleteCommand(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="ItemCheckChanged"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void ItemCheckChangedEventHandler(object sender, GridItemCheckChangedEventArgs e);

    /// <summary>
    /// Fires after an item is checked or unchecked.
    /// </summary>
    [ Description("Fires after an item is checked or unchecked."), Category("Grid Events") ]
    public event ItemCheckChangedEventHandler ItemCheckChanged;

    private void OnItemCheckChanged(GridItemCheckChangedEventArgs e) 
    {
      if (ItemCheckChanged != null) 
      {
        ItemCheckChanged(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="ItemCommand"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void ItemCommandEventHandler(object sender, GridItemCommandEventArgs e);

    /// <summary>
    /// Fires after a control belonging to this item causes a postback.
    /// </summary>
    [ Description("Fires after a control belonging to this item causes a postback."), Category("Grid Events") ]
    public event ItemCommandEventHandler ItemCommand;

    private void OnItemCommand(GridItemCommandEventArgs e) 
    {
      if (ItemCommand != null) 
      {
        ItemCommand(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="ItemDataBound"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void ItemDataBoundEventHandler(object sender, GridItemDataBoundEventArgs e);

    /// <summary>
    /// Fires after a data item is bound to a GridItem.
    /// </summary>
    [ Description("Fires after a data item is bound to a GridItem."), Category("Grid Events") ]
    public event ItemDataBoundEventHandler ItemDataBound;

    private void OnItemDataBound(GridItemDataBoundEventArgs e) 
    {
      if (ItemDataBound != null) 
      {
        ItemDataBound(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="ItemContentCreated"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void ItemContentCreatedEventHandler(object sender, GridItemContentCreatedEventArgs e);

    /// <summary>
    /// Fires when a server template is instantiated for a cell in a GridItem.
    /// </summary>
    [ Description("Fires when a server template is instantiated for a cell in a GridItem."), Category("Grid Events") ]
    public event ItemContentCreatedEventHandler ItemContentCreated;

    private void OnItemContentCreated(GridItemContentCreatedEventArgs e) 
    {
      if (ItemContentCreated != null) 
      {
        ItemContentCreated(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="PageIndexChanged"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void PageIndexChangedEventHandler(object sender, GridPageIndexChangedEventArgs e);

    /// <summary>
    /// Fires after the page is changed.
    /// </summary>
    [ Description("Fires after the page is changed."), Category("Grid Events") ]
    public event PageIndexChangedEventHandler PageIndexChanged;

    private void OnPageIndexChanged(GridPageIndexChangedEventArgs e) 
    {         
      if (PageIndexChanged != null) 
      {
        PageIndexChanged(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="Scroll"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void ScrollEventHandler(object sender, GridScrollEventArgs e);

    /// <summary>
    /// Fires after the grid is scrolled.
    /// </summary>
    [ Description("Fires after the grid is scrolled."), Category("Grid Events") ]
    public event ScrollEventHandler Scroll;

    private void OnScroll(GridScrollEventArgs e) 
    {         
      if (Scroll != null) 
      {
        Scroll(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="ColumnReorder"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void ColumnReorderEventHandler(object sender, GridColumnReorderEventArgs e);

    /// <summary>
    /// Fires after the column order is changed.
    /// </summary>
    [ Description("Fires after the column order is changed."), Category("Grid Events") ]
    public event ColumnReorderEventHandler ColumnReorder;

    private void OnColumnReorder(GridColumnReorderEventArgs e) 
    {         
      if (ColumnReorder != null) 
      {
        ColumnReorder(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="FilterCommand"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void FilterCommandEventHandler(object sender, GridFilterCommandEventArgs e);

    /// <summary>
    /// Fires after a filter request is made.
    /// </summary>
    [ Description("Fires after a filter request is made."), Category("Grid Events") ]
    public event FilterCommandEventHandler FilterCommand;

    private void OnFilterCommand(GridFilterCommandEventArgs e) 
    {         
      if (FilterCommand != null) 
      {
        FilterCommand(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="GroupCommand"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void GroupCommandEventHandler(object sender, GridGroupCommandEventArgs e);

    /// <summary>
    /// Fires after a group request is made.
    /// </summary>
    [ Description("Fires after a group request is made."), Category("Grid Events") ]
    public event GroupCommandEventHandler GroupCommand;

    private void OnGroupCommand(GridGroupCommandEventArgs e) 
    {         
      if (GroupCommand != null) 
      {
        GroupCommand(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="SortCommand"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void SortCommandEventHandler(object sender, GridSortCommandEventArgs e);

    /// <summary>
    /// Fires after a sort request is made.
    /// </summary>
    [ Description("Fires after a sort request is made."), Category("Grid Events") ]
    public event SortCommandEventHandler SortCommand;

    private void OnSortCommand(GridSortCommandEventArgs e) 
    {         
      if (SortCommand != null) 
      {
        SortCommand(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NeedGroupData"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void NeedGroupDataEventHandler(object sender, GridNeedGroupDataEventArgs e);

    /// <summary>
    /// Fires when the records of a group are needed in manual paging mode.
    /// </summary>
    [Description("Fires when the records of a group are needed in manual paging mode."), Category("Grid Events")]
    public event NeedGroupDataEventHandler NeedGroupData;

    private void OnNeedGroupData(GridNeedGroupDataEventArgs e)
    {
      if (NeedGroupData != null)
      {
        NeedGroupData(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="NeedGroups"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void NeedGroupsEventHandler(object sender, GridNeedGroupsEventArgs e);

    /// <summary>
    /// Fires when the groups need to be retrieved in ManualPaging mode.
    /// </summary>
    [Description("Fires when the groups need to be retrieved in ManualPaging mode."), Category("Grid Events")]
    public event NeedGroupsEventHandler NeedGroups;

    private void OnNeedGroups(GridNeedGroupsEventArgs e)
    {
      if (NeedGroups != null)
      {
        NeedGroups(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="NeedRebind"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void NeedRebindEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Fires after a series of events which require a rebinding.
    /// </summary>
    [ Description("Fires after a series of events which require a rebinding."), Category("Grid Events") ]
    public event NeedRebindEventHandler NeedRebind;

    private void OnNeedRebind(EventArgs e) 
    {         
      if (NeedRebind != null) 
      {
        NeedRebind(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NeedDataSource"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void NeedDataSourceEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Fires when the DataSource needs to be set.
    /// </summary>
    [ Description("Fires when the DataSource needs to be set."), Category("Grid Events") ]
    public event NeedDataSourceEventHandler NeedDataSource;

    private void OnNeedDataSource(EventArgs e) 
    {         
      if (NeedDataSource != null) 
      {
        NeedDataSource(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NeedChildDataSource"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void NeedChildDataSourceEventHandler(object sender, GridNeedChildDataSourceEventArgs e);

    /// <summary>
    /// Fires when the child DataSource of a GridItem needs to be set.
    /// </summary>
    [ Description("Fires when the child DataSource of a GridItem needs to be set."), Category("Grid Events") ]
    public event NeedChildDataSourceEventHandler NeedChildDataSource;

    private void OnNeedChildDataSource(GridNeedChildDataSourceEventArgs e) 
    {         
      if (NeedChildDataSource != null) 
      {
        NeedChildDataSource(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="AfterCallback"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void AfterCallbackEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Fires during a Grid callback after all the other events have fired.
    /// </summary>
    [Description("Fires during a Grid callback after all the other events have fired."), Category("Grid Events")]
    public event AfterCallbackEventHandler AfterCallback;

    private void OnAfterCallback(EventArgs e)
    {
      if (AfterCallback != null)
      {
        AfterCallback(this, e);
      }
    }

    /// <summary>
    /// Delegate for <see cref="BeforeCallback"/> event of <see cref="Grid"/> class.
    /// </summary>
    public delegate void BeforeCallbackEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Fires during a Grid callback before all the other events are fired.
    /// </summary>
    [Description("Fires during a Grid callback before all the other events are fired."), Category("Grid Events")]
    public event BeforeCallbackEventHandler BeforeCallback;

    private void OnBeforeCallback(EventArgs e)
    {
      if (BeforeCallback != null)
      {
        BeforeCallback(this, e);
      }
    }

    #endregion
	}

  #region Enum types

  /// <summary>
  /// Specifies the position of <see cref="Grid"/> control's loading panel within the Grid.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the <see cref="Grid.LoadingPanelPosition" /> property, and is used to specify the location, relative to the 
  /// frame of the rendered grid, that the loading indicator will be displayed. Unlike the <see cref="GridElementPosition"/> enumeration, the
  /// element controlled by this enumeration (the loading panel) is rendered on top of the underlying Grid.
  /// </para>
  /// <para>
  /// Compare to <see cref="GridElementPosition"/>.
  /// </para>
  /// </remarks>
  public enum GridRelativePosition
  {
    /// <summary>Top left corner.</summary>
    TopLeft,

    /// <summary>Middle of the top edge.</summary>
    TopCenter,

    /// <summary>Top right corner.</summary>
    TopRight,

    /// <summary>Middle of the left edge.</summary>
    MiddleLeft,

    /// <summary>Center of the Grid.</summary>
    MiddleCenter,

    /// <summary>Middle of the right edge.</summary>
    MiddleRight,

    /// <summary>Bottom left corner.</summary>
    BottomLeft,

    /// <summary>Middle of the bottom edge.</summary>
    BottomCenter,

    /// <summary>Bottom right corner.</summary>
    BottomRight
  }

  /// <summary>
  /// Specifies whether a <see cref="GridColumn"/> contains a checkbox.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the <see cref="GridColumn.ColumnType" /> property. It is used to specify whether a column
  /// which contains boolean data will be implemented as user-selectable checkboxes.
  /// </para>
  /// </remarks>
  public enum GridColumnType
  {
    /// <summary>A non-checkbox column.</summary>
    Default,

    /// <summary>A checkbox column.</summary>
    CheckBox
  }

  /// <summary>
  /// Specifies the type of edit interface a <see cref="GridColumn"/> implements.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the <see cref="GridColumn.EditControlType" /> property. It specifies the type of 
  /// control to use for editing the column. 
  /// </para>
  /// </remarks>
  public enum GridColumnEditControlType
  {
    /// <summary>Use the default editing interface for this column.</summary>
    Default,

    /// <summary>Editing interface is implemented with a custom template.</summary>
    Custom,

    /// <summary>This is an editing confirmation column and is not edited directly.</summary>
    EditCommand,

    /// <summary>Editing interface is a text box.</summary>
    TextBox,

    /// <summary>Editing interface is a text area.</summary>
    TextArea
  }

  /// <summary>
  /// Specifies in which of the four corners of the <see cref="Grid"/> to position an element.
  /// </summary>
  /// <remarks>
  /// <para>
  /// It is used to position an element relative to the rendered <see cref="Grid" />. Elements positioned using this
  /// enumeration are rendered within either the header of the footer, on the right or left side.
  /// The <see cref="Grid.ShowHeader" /> and <see cref="Grid.ShowFooter" /> properties determine whether the header and footer are displayed.
  /// The footer is shown by default, and the header is hidden.
  /// </para>
  /// <para>
  /// This enumeration is similar to <see cref="GridRelativePosition"/>. 
  /// </para>
  /// </remarks>
  public enum GridElementPosition
  {
    /// <summary>Top left corner.</summary>
    TopLeft,

    /// <summary>Top right corner.</summary>
    TopRight,

    /// <summary>Bottom left corner.</summary>
    BottomLeft,

    /// <summary>Bottom right corner.</summary>
    BottomRight
  }

  /// <summary>
  /// Specifies the behaviour of grouping, when combined with paging.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the <see cref="Grid.GroupingMode" /> property. It is used to specify the behavior 
  /// of the Grid when it is grouped. Specifically, it determines how each page of data is defined. The 
  /// <see cref="Grid.ManualPaging" /> property also affects the grouping behavior. 
  /// </para>
  /// </remarks>
  public enum GridGroupingMode
  {
    /// <summary>
    /// Each page has a constant number of groups, defined by the <see cref="Grid.GroupingPageSize" /> property.
    /// </summary>
    ConstantGroups,

    /// <summary>
    /// Each page has a constant number of records. This grouping mode is compatible with web service running mode.
    /// Records are retrieved from the server and grouped on the client.
    /// </summary>
    ConstantRecords,

    /// <summary>
    /// Each page has a constant number of rendered rows (records and group headings), defined by the 
    /// <see cref="Grid.GroupingPageSize" /> property.
    /// This grouping mode results in a consistent rendered height.
    /// Manual paging must be implemented in this grouping mode. 
    /// (See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorial)
    /// </summary>
    ConstantRows
  }

  /// <summary>
  /// Specifies the type of interface to use for paging within the <see cref="Grid"/>.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the <see cref="Grid">Grid</see> <see cref="Grid.PagerStyle" /> property, and
  /// is used to specify the type of pager to render. The rendered position of the pager can be specified using the 
  /// <see cref="Grid.PagerPosition" /> property.
  /// </para>
  /// </remarks>
  public enum GridPagerStyle
  {
    /// <summary>Numbered pager.</summary>
    Numbered,

    /// <summary>Sliding pager. Images must be placed in the location specified by the <see cref="Grid.PagerImagesFolderUrl" /> property.</summary>
    Slider,

    /// <summary>
    /// Button pager (first, previous, next, last). 
    /// Images must be placed in the location specified by the <see cref="Grid.PagerImagesFolderUrl" /> property.
    /// </summary>
    Buttons
  }

  /// <summary>
  /// Specifies how to perform client-side rendering.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the <see cref="Grid.RenderingMode" /> property.
  /// </para>
  /// </remarks>
  public enum GridRenderingMode
  {
    /// <summary>
    /// Default, fully-featured rendering.
    /// </summary>
    Default,

    /// <summary>
    /// Lightweight, stripped down rendering.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This rendering mode specifies lightweight rendering in which stripped down HTML markup is generated, at the cost of some features.
    /// The features which are unavailable in this rendering mode are:
    /// - Data cell content clipping
    /// - Data cell ellipsis
    /// </para>
    /// </remarks>
    UltraLight
  }

  /// <summary>
  /// Specifies how to transfer the contents of the <see cref="Grid"/> between the server and the client.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the <see cref="Grid.RunningMode" /> property, which is one of the <see cref="Grid">Grid's</see>
  /// key configuration properties.
  /// </para>
  /// </remarks>
  public enum GridRunningMode
  {
    /// <summary>Perform all actions on the client without callbacks or postbacks.</summary>
    Client,

    /// <summary>Use postbacks to access data on the server.</summary>
    Server,

    /// <summary>Use callbacks to access data on the server.</summary>
    Callback,

    /// <summary>Use ASP.NET AJAX web service calls to access data on the server</summary>
    WebService
  }

  /// <summary>
  /// Specifies whether to scroll the <see cref="Grid"/> contents when they exceed their specified height.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the <see cref="Grid.ScrollBar" /> property, which determines whether to display
  /// a vertical scrollbar, allowing the user to scroll through items.
  /// </para>
  /// </remarks>
  public enum GridScrollBarMode
  {
    /// <summary>Turn off scroll bar.</summary>
    Off,

    /// <summary>Turn on scroll bar.</summary>
    On,

    /// <summary>Display scroll bar if the number of records exceeds the page size.</summary>
    Auto
  }

  /// <summary>
  /// Specifies whether <see cref="Grid"/> contents are sorted in ascending or descending order.
  /// </summary>
  /// <seealso cref="GridColumn.DefaultSortDirection"/>
  public enum GridSortDirection
  {
    /// <summary>Sort in ascending order.</summary>
    Ascending,

    /// <summary>Sort in descending order.</summary>
    Descending
  }

  #endregion
}
