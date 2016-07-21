using System;
using System.Collections; 
using System.Collections.Specialized; 
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;


namespace ComponentArt.Web.UI
{
  #region WebService classes

  public class BaseNavigatorWebServiceResponse
  {
    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;


    protected ArrayList NodesToArray(NavigationNodeCollection arNodes)
    {
      ArrayList arList = new ArrayList();

      for (int i = 0; i < arNodes.Count; i++)
      {
        ArrayList props = new ArrayList();

        NavigationNode oNode = arNodes[i];

        foreach (string sKey in oNode.Properties.Keys)
        {
          props.Add(new object[] { sKey, oNode.Properties[sKey] });
        }

        if (oNode.nodes != null && oNode.nodes.Count > 0)
        {
          props.Add(new object[] { "Nodes", NodesToArray(oNode.nodes) });
        }

        arList.Add(props);
      }

      return arList;
    }
  }

  #endregion

  #region Template Classes

  /// <summary>
  /// Template class used for specifying customized rendering for <see cref="NavigationNode"/> instances.
  /// </summary>
  [DefaultProperty("Template")]
  [ParseChildren(true)]
  [PersistChildren(false)]
  [ToolboxItem(false)]
  public class NavigationCustomTemplate : System.Web.UI.WebControls.WebControl
  {
    private ITemplate m_oTemplate;

    /// <summary>
    /// The template.
    /// </summary>
    [
    Browsable(false),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
    PersistenceMode(PersistenceMode.InnerProperty),
    TemplateContainer(typeof(ComponentArt.Web.UI.NavigationTemplateContainer)),
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
  /// Collection of <see cref="NavigationCustomTemplate"/> objects.
  /// </summary>
  public class NavigationCustomTemplateCollection : CollectionBase
  {
    public new NavigationCustomTemplate this[int index]
    {
      get
      {
        return (NavigationCustomTemplate)base.List[index];
      }
      set
      {
        base.List[index] = value;
      }
    }

    public new int Add(NavigationCustomTemplate template)
    {
      return this.List.Add(template);
    }
  }

  /// <summary>
  /// Naming container for a customized <see cref="NavigationNode"/> instance.
  /// </summary>
  [ToolboxItem(false)]
  public class NavigationTemplateContainer : Control, INamingContainer
  {
    private NavigationNode _dataItem;
    private System.Web.UI.AttributeCollection _attributes;

    /// <summary>
    /// NavigationTemplateContainer constructor.
    /// </summary>
    /// <param name="oNode">Node to look to for data.</param>
    public NavigationTemplateContainer(NavigationNode oNode)
    {
      _dataItem = oNode;

      if(oNode != null)
      {
        _attributes = oNode.Attributes;
      }
    }

    /// <summary>
    /// Item containing data to bind to (a NavigationNode).
    /// </summary>
    public virtual NavigationNode DataItem
    {
      get
      {
        return _dataItem;
      }
    }

    /// <summary>
    /// Attributes of the given data item.
    /// </summary>
    public virtual System.Web.UI.AttributeCollection Attributes
    {
      get
      {
        return _attributes;
      }
    }
  }

  #endregion

  #region Custom Attribute Classes

  /// <summary>
  /// Used for extending the data model of ComponentArt navigation controls.
  /// </summary>
  /// <remarks>
  /// Any number of custom XML attributes can be defined within the XML structure.
  /// These custom attributes can be mapped to navigation node properties via CustomAttributeMappings. 
  /// </remarks>
  [ToolboxItem(false)]
  public class CustomAttributeMapping 
  {
    private string _from;
    private string _to;

    /// <summary>
    /// Name to map from.
    /// </summary>
    public string From
    {
      get
      {
        return _from;
      } 
      set
      {
        _from = value;
      }
    } 

    /// <summary>
    /// Name to map to.
    /// </summary>
    public string To
    {
      get
      {
        return _to;
      } 
      set
      {
        _to = value;
      }
    } 
  }

  /// <summary>
  /// Collection of <see cref="CustomAttributeMapping"/> objects. 
  /// </summary>
  [ToolboxItem(false)]
  [Editor("System.Windows.Forms.Design.CollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
  public class CustomAttributeMappingCollection : ICollection, IEnumerable, IList
  {
    private ArrayList _mappings;

    public CustomAttributeMappingCollection()
    {
      _mappings = new ArrayList();
    }

    object IList.this[int index] 
    {
      get 
      {
        return _mappings[index];
      }
      set 
      {
        _mappings[index] = (CustomAttributeMapping)value;
      }
    }

    public int Count
    {
      get
      {
        return _mappings.Count;
      }
    }

    public bool IsSynchronized
    {
      get
      {
        return true;
      }
    }

    public object SyncRoot
    {
      get
      {
        return _mappings.SyncRoot;
      }
    }

    public void CopyTo (Array ar, int index)
    {
    }

    public virtual CustomAttributeMapping this[int index]
    {
      get
      {
        return (CustomAttributeMapping) _mappings[index];
      }
      set
      {
        _mappings[index] = value;
      }
    }

    public void Remove (object item)
    {
      _mappings.Remove (item);
    }
		
    public void Insert (int index, object item)
    {
      _mappings[index] = item;
    }

    public int Add (object item)
    {
      return _mappings.Add(item);
    }

    public void Clear()
    {
      _mappings.Clear();
    }

    public bool Contains(object item)
    {
      return _mappings.Contains(item);
    }

    public int IndexOf (object item)
    {
      return _mappings.IndexOf(item);
    }

    public bool IsFixedSize
    {
      get
      {
        return false;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public void RemoveAt(int index)
    {
      _mappings.RemoveAt(index);
    }

    public virtual IEnumerator GetEnumerator()
    {
      return _mappings.GetEnumerator();
    }
  }

  #endregion

  /// <summary>
  /// Serves as the base class for ComponentArt navigation controls which organize their data in hierarchical form.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Provides base navigation object model, data binding, template collections, selected node functionality, URL redirects, 
  /// postbacks, navigation methods, and search engine structure rendering to its descendents.
  /// All ComponentArt navigation controls inherit from this class.  
  /// </para>
  /// <para>
  /// <see cref="NavigationNode" /> is the base class for each node in the hierarchical <code>BaseNavigator</code> structure.
  /// </para>
  /// </remarks>
  public abstract class BaseNavigator : WebControl, INamingContainer, IPostBackEventHandler
  {
    protected bool bNewDataLoaded = false;
    protected string selectedNodePostbackId;

    #region Public Properties 

    #region Unused hidden inheritance

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override FontInfo Font
    {
      get { return base.Font; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string AccessKey
    {
      get { return base.AccessKey; }
    }

    #endregion
    
    /// <summary>
    /// Whether to perform a postback when a node is selected. Default: false.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to perform a postback when a node is selected. Default: false.")]
    [DefaultValue(false)]
    public bool AutoPostBackOnSelect
    {
      get 
      {
        object o = ViewState["AutoPostBackOnSelect"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoPostBackOnSelect"] = value;
      }
    }

    /// <summary>
    /// Prefix to use for all non-image URL paths. For images, use ImagesBaseUrl.
    /// </summary>
    [Category("Navigation")]
    [Description("Used as a prefix for all node URLs. ")]
    [DefaultValue("")]
    public string BaseUrl
    {
      get
      {
        object o = ViewState["BaseUrl"]; 
        return (o == null) ? String.Empty : Utils.ConvertUrl(Context, string.Empty, (string)o); 
      }

      set
      {
        ViewState["BaseUrl"] = value; 
      }
    }

    /// <summary>
    /// Whether to trigger ASP.NET page validation when a node is selected. Default: true.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to trigger ASP.NET page validation when a node is selected. Default: true. ")]
    [DefaultValue(true)]
    public bool CausesValidation
    {
      get 
      {
        object o = ViewState["CausesValidation"]; 
        return (o == null) ? true : (bool) o; 
      }
      set 
      {
        ViewState["CausesValidation"] = value;
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
        return _clientTemplates;
      }
    }

    public override ControlCollection Controls
    {
      get
      {
        EnsureChildControls();
        return base.Controls;
      }
    }
    
    private CustomAttributeMappingCollection _customAttributeMappings;
    /// <summary>
    /// Custom attribute mappings. Provides the ability to re-map property names when they are looked up in XML.
    /// </summary>
    /// <example>
    /// Suppose the Menu structure is being loaded from an XML file named MenuData.xml with contents like this:
    /// <code>
    /// <![CDATA[
    /// &lt;node>
    ///   &lt;node txt="circle" pic="images/circle.gif" alt="Roundish Circle" />
    ///   &lt;node txt="square" pic="images/square.gif" alt="Squarish Square" />
    /// &lt;/node>
    /// ]]>
    /// </code>
    /// The names of the XML attributes in this file do not correspond to the names of the relevant MenuItem properties.
    /// An easy way to work around this problem is to use <b>CustomAttributeMappings</b>.  Here is how this can be done:
    /// <code>
    /// <![CDATA[
    /// &lt;ComponentArt:Menu ID="Menu1" SiteMapXmlFile="MenuData.xml" runat="server">
    ///   &lt;CustomAttributeMappings>
    ///     &lt;ComponentArt:CustomAttributeMapping From="alt" To="ToolTip" />
    ///     &lt;ComponentArt:CustomAttributeMapping From="pic" To="Look-LeftIconUrl" />
    ///     &lt;ComponentArt:CustomAttributeMapping From="txt" To="Text" />
    ///   &lt;/CustomAttributeMappings>
    /// &lt;/ComponentArt:Menu>
    /// ]]>
    /// </code>
    /// </example>
    [Category("Data")]
    [Description("Custom attribute mappings. Provides the ability to re-map property names when they are looked up in XML.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [NotifyParentProperty(true)]
    public CustomAttributeMappingCollection CustomAttributeMappings
    {
      get
      {
        if(_customAttributeMappings == null)
        {
          _customAttributeMappings = new CustomAttributeMappingCollection();
        }
        return _customAttributeMappings;
      }
    }

    private NavigationCustomTemplateCollection _serverTemplates;
    /// <summary>
    /// Custom server templates which are referenced by nodes with special needs.
    /// </summary>
    /// <seealso cref="NavigationNode.ServerTemplateId" />
    [Browsable(false)]
    [Description("Collection of CustomTemplate controls.")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public NavigationCustomTemplateCollection ServerTemplates
    {
      get
      {
        if (_serverTemplates == null)
        {
          _serverTemplates = new NavigationCustomTemplateCollection();
        }
        return _serverTemplates;
      }
    }

    /// <summary>
    /// Deprecated.  Use <see cref="ServerTemplates"/> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use ServerTemplates instead.")]
    [Obsolete("Deprecated.  Use ServerTemplates instead.", false)]
    public NavigationCustomTemplateCollection Templates
    {
      get
      {
        return ServerTemplates;
      }
    }

    private object _dataSource; 
    /// <summary>
    /// DataSource to bind to. This can be an XmlDocument or a DataSet.
    /// </summary>
    /// <seealso cref="SiteMapXmlFile"/>
    /// <seealso cref="LoadXml(string)" /><seealso cref="LoadXml(System.Xml.XmlDocument)" />
    [ 
    Browsable(false), 
    Description("Data source for ASP.NET data binding. "),
    DefaultValue(null),
    Category("Data") 
    ]
    public object DataSource
    {
      get 
      { 
        return _dataSource; 
      }
		      
      set 
      { 
        _dataSource = value; 
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
        object o = ViewState["DataSourceID"];
        return (o == null) ? String.Empty : (string)o;
      }

      set
      {
        ViewState["DataSourceID"] = value;
      }
    }

 

    /// <summary>
    /// Default target (frame or window) to use when navigating.
    /// </summary>
    /// <seealso cref="NavigationNode.NavigateUrl" />
    [Category("Navigation")]
    [Description("Default target frame. ")]
    [DefaultValue("")]
    public string DefaultTarget
    {
      get
      {
        object o = ViewState["DefaultTarget"]; 
        return (o == null) ? String.Empty : (string)o; 
      }

      set
      {
        ViewState["DefaultTarget"] = value; 
      }
    }

    /// <summary>
    /// The NavigateUrl of the first node in the group.
    /// </summary>
    [Description("The NavigateUrl of the first node in the group. ")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FirstInGroupUrl
    {
      get
      {
        if(this.selectedNode != null) 
        {
          if(this.selectedNode.parentNode != null)
          {
            return this.selectedNode.parentNode.nodes[0].NavigateUrl;
          }
          else
          {
            return this.FirstUrl;
          }
        }
        else
        {
          return this.FirstUrl;
        }
      }
    }
    
    /// <summary>
    /// The NavigateUrl of the first node in the tree.
    /// </summary>
    [Description("The NavigateUrl of the first node in the tree. ")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FirstUrl
    {
      get
      {
        if(this.nodes != null && this.nodes.Count > 0)
        {
          return this.nodes[0].NavigateUrl;
        }
        else
        {
          return null;
        }
      }
    }

    /// <summary>
    /// Whether to force the rendering of the search engine structure. Default: false.
    /// </summary>
    [Category("Support")]
    [Description("Whether to force the rendering of the search engine structure. Default: false.")]
    [DefaultValue(false)]
    public bool ForceSearchEngineStructure
    {
      get 
      {
        object o = ViewState["ForceSearchEngineStructure"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["ForceSearchEngineStructure"] = value;
      }
    }

    /// <summary>
    /// Whether to visually distinguish Selected node and its parent nodes from other non-Selected nodes.
    /// </summary>
    /// <remarks>
    /// For this to work, Selected and/or ChildSelected styles must be defined.
    /// </remarks>
    [Category("Behavior")]
    [Description("Whether to visually distinguish Selected node and its parent nodes from other non-Selected nodes.")]
    [DefaultValue(true)]
    public bool HighlightSelectedPath
    {
      get 
      {
        return Utils.ParseBool(ViewState["HighlightSelectedPath"], true);
      }
      set 
      {
        ViewState["HighlightSelectedPath"] = value;
      }
    }

    /// <summary>
    /// Prefix to use for all image URL paths. For non-image URLs, use BaseUrl.
    /// </summary>
    [Category("Support")]
    [Description("Used as a prefix for all image URLs. ")]
    [DefaultValue("")]
    public virtual string ImagesBaseUrl
    {
      get
      {
        object o = ViewState["ImagesBaseUrl"]; 
        return (o == null) ? String.Empty : Utils.ConvertUrl(Context, string.Empty, (string)o); 
      }

      set
      {
        ViewState["ImagesBaseUrl"] = value; 
      }
    }

    /// <summary>
    /// The NavigateUrl of the first node in the group.
    /// </summary>
    [Description("The NavigateUrl of the first node in the group. ")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string LastInGroupUrl
    {
      get
      {
        if(this.selectedNode != null) 
        {
          if(this.selectedNode.parentNode != null)
          {
            return this.selectedNode.parentNode.nodes[this.selectedNode.parentNode.nodes.Count - 1].NavigateUrl;
          }
          else
          {
            return this.LastUrl;
          }
        }
        else
        {
          return null;
        }
      }
    }


    /// <summary>
    /// The NavigateUrl of the last node in the tree.
    /// </summary>
    [Description("The NavigateUrl of the last node in the tree. ")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string LastUrl
    {
      get
      {
        if(this.nodes != null && this.nodes.Count > 0)
        {
          NavigationNode oLastNode = this.nodes[this.nodes.Count - 1];

          while(oLastNode.nodes != null && oLastNode.nodes.Count > 0)
          {
            oLastNode = oLastNode.nodes[oLastNode.nodes.Count - 1];
          }

          return oLastNode.NavigateUrl;
        }
        else
        {
          return null;
        }
      }
    }

    /// <summary>
    /// The NavigateUrl of the next node in the group.
    /// </summary>
    [Description("The NavigateUrl of the next node in the group. ")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string NextInGroupUrl
    {
      get
      {
        if(this.selectedNode != null)
        {
          if(this.selectedNode.nextSibling != null)
          {
            return this.selectedNode.nextSibling.NavigateUrl;
          }
          else
          {
            return this.selectedNode.NavigateUrl;
          }
        }
        else
        {
          return null;
        }
      }
    }

    /// <summary>
    /// The NavigateUrl of the next node in the tree.
    /// </summary>
    [Description("The NavigateUrl of the next node in the tree. ")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string NextUrl
    {
      get
      {
        if (this.selectedNode == null)
        {
          return null;
        }
        else
        {
          this.PopulateNodeNextUrls();
          return this.selectedNode.nextUrl;
        }
      }
    }

    /// <summary>
    /// Whether to persist custom attributes of nodes to JavaScript. Default: true.
    /// </summary>
    [Description("Whether to persist custom attributes of nodes to JavaScript. Default: true.")]
    [DefaultValue(true)]
    public bool OutputCustomAttributes
    {
      get 
      {
        object o = ViewState["OutputCustomAttributes"]; 
        return (o == null) ? true : Utils.ParseBool(o,true); 
      }
      set 
      {
        ViewState["OutputCustomAttributes"] = value;
      }
    }

    /// <summary>
    /// Whether to pre-render the entire structure on the client, instead of only the initially visible parts. Default: false.
    /// </summary>
    /// <remarks>
    /// Setting this property to <b>true</b> can improve the reliability of the control in rare situations where
    /// caching is disabled in the browser, and not forceable by IIS, when images are used extensively. Using
    /// this feature degrades the performance of the control in all situations, however.
    /// </remarks>
    [Description("Whether to pre-render the entire structure, instead of only the initially visible parts. Default: false.")]
    [DefaultValue(false)]
    public virtual bool PreRenderAllLevels
    {
      get 
      {
        object o = ViewState["PreRenderAllLevels"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["PreRenderAllLevels"] = value;
      }
    }

    /// <summary>
    /// The NavigateUrl of the previous node in the group.
    /// </summary>
    [Description("The NavigateUrl of the previous node in the group. ")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string PreviousInGroupUrl
    {
      get
      {
        if(this.selectedNode != null)
        {
          if(this.selectedNode.previousSibling != null)
          {
            return this.selectedNode.previousSibling.NavigateUrl;
          }
          else
          {
            return this.selectedNode.NavigateUrl;
          }
        }
        else
        {
          return null;
        }
      }
    }

    /// <summary>
    /// The NavigateUrl of the previous node in the tree.
    /// </summary>
    [Description("The NavigateUrl of the previous node in the tree. ")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string PreviousUrl
    {
      get
      {
        if (this.selectedNode == null)
        {
          return null;
        }
        else
        {
          this.PopulateNodePreviousUrls();
          return this.selectedNode.previousUrl;
        }
      }
    }

    /// <summary>
    /// Whether to render the search engine structure for detected crawlers. Default: true.
    /// </summary>
    [Category("Support")]
    [Description("Whether to render the search engine structure for detected crawlers. Default: true.")]
    [DefaultValue(true)]
    public bool RenderSearchEngineStructure
    {
      get 
      {
        object o = ViewState["RenderSearchEngineStructure"]; 
        return (o == null) ? true : (bool) o; 
      }
      set 
      {
        ViewState["RenderSearchEngineStructure"] = value;
      }
    }

    /// <summary>
    /// Depth from RenderRoot(Node|Item) to render to.
    /// </summary>
    [Category("Data")]
    [Description("Depth from RenderRootNode to render to. Default: 0.")]
    [DefaultValue(0)]
    public int RenderDrillDownDepth
    {
      get 
      {
        object o = ViewState["RenderDrillDownDepth"]; 
        return (o == null) ? 0 : (int)o; 
      }
      set 
      {
        ViewState["RenderDrillDownDepth"] = value;
      }
    }
		
    /// <summary>
    /// ID of node to begin rendering down from.
    /// </summary>
    [Description("ID of node to begin rendering down from.")]
    [DefaultValue("")]
    protected string RenderRootNodeId
    {
      get 
      {
        object o = ViewState["RenderRootNodeId"]; 
        return (o == null) ? string.Empty : (string)o; 
      }
      set 
      {
        ViewState["RenderRootNodeId"] = value;
      }
    }

    /// <summary>
    /// Whether to include the RenderRootNode when rendering, instead of only its children. Default: false.
    /// </summary>
    [DefaultValue(false)]
    protected bool RenderRootNodeInclude
    {
      get 
      {
        object o = ViewState["RenderRootNodeInclude"]; 
        return (o == null) ? false : (bool)o; 
      }
      set 
      {
        ViewState["RenderRootNodeInclude"] = value;
      }
    }

    /// <summary>
    /// Path to the site map XML file.
    /// </summary>
    /// <seealso cref="DataSource" />
    /// <seealso cref="LoadXml(string)" /><seealso cref="LoadXml(System.Xml.XmlDocument)" />
    [Category("Data")]
    [DefaultValue("")]
    [Description("Path to the site map XML file. ")]
    [Editor(typeof(System.Web.UI.Design.XmlUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public string SiteMapXmlFile
    {
      get
      {
        object o = ViewState["SiteMapXmlFile"]; 
        return (o == null) ? String.Empty : (string)o; 
      }
      set
      {
        ViewState["SiteMapXmlFile"] = value;

        if(Context != null)
        {
          string sServerPath = Context.Server.MapPath(value);

          if(!File.Exists(sServerPath))
          {
            throw new FileNotFoundException("Specified SiteMapXmlFile (" + value + ") does not exist.");
          }

          // Load XML from SiteMapXmlFile
          XmlDocument oXmlDoc = new XmlDocument();
          oXmlDoc.Load(sServerPath);
          this.LoadXml(oXmlDoc);

          ComponentArtFixStructure();

          this.UpdateSelectedNode();

          bNewDataLoaded = true;
        }
      }
    }

    /// <summary>
    /// ID of ComponentArt MultiPage to control from this navigator.
    /// </summary>
    /// <seealso cref="NavigationNode.PageViewId" />
    [Description("ID of ComponentArt MultiPage to control from this navigator.")]
    [Category("Behavior")]
    [DefaultValue("")]
    public string MultiPageId
    {
      get 
      {
        object o = ViewState["MultiPageId"]; 
        return (o == null) ? string.Empty : (string)o; 
      }
      set 
      {
        ViewState["MultiPageId"] = value;
      }
    }

		#endregion

	  #region Public Methods 

    /// <summary>
    /// Bind to the current DataSource.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method binds the <code>BaseNavigator</code> control to the datasource specified by the 
    /// <see cref="BaseNavigator.DataSource" /> property. This property is not usually called explicitly. 
    /// <code>BaseNavigator</code> data is generally loaded from an XML file automatically 
    /// using the <see cref="BaseNavigator.SiteMapXmlFile" /> property, automatically using the designer, 
    /// or by manually creating nodes and adding them to the hierarchy using the various 
    /// <see cref="NavigationNodeCollection">NavigationNodeCollections</see>. 
    /// </para>
    /// </remarks>
    /// <seealso cref="DataSource" />
    public override void DataBind() 
    {
      if (this.DataSourceID != "")
      {
        Control oControl = Utils.FindControl(this, this.DataSourceID);

        if (oControl != null)
        {
          this.DataSource = oControl;
        }
        else
        {
          throw new Exception("Data source control '" + DataSourceID + "' not found.");
        }
      }

      if(this.DataSource == null)
      {
        return;
      }

      if(this.DataSource is DataSet)
      {
        DataSet oDS = (DataSet)(this.DataSource);

        this.LoadXml(oDS.GetXml());
      }
      else if (this.DataSource is SiteMapDataSource)
      {
        SiteMapDataSource oDS = (SiteMapDataSource)(this.DataSource);

        this.LoadFromSiteMap(oDS.Provider.RootNode, oDS.ShowStartingNode);
      }
      else if (this.DataSource is XmlDataSource)
      {
        XmlDataSource oDS = (XmlDataSource)(this.DataSource);

        XmlDocument oXD = oDS.GetXmlDocument();

        this.LoadXml(oXD.OuterXml);
      }
      else if (this.DataSource is IHierarchicalDataSource)
      {
        IHierarchicalDataSource oDS = (IHierarchicalDataSource)(this.DataSource);
        
        this.LoadFromHierarchy(oDS.GetHierarchicalView("").Select(), null);
      }
      else if(this.DataSource is XmlDocument)
      {
        XmlDocument oXD = (XmlDocument)(this.DataSource);

        this.LoadXml(oXD.OuterXml);
      }
      else
      {
        throw new ArgumentException("Cannot bind to DataSource of type " + this.DataSource.GetType().ToString() + ".");
      }

      this.ComponentArtFixStructure();
      base.DataBind();
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

      if(this.nodes != null)
      {
        GetXml(oXmlDoc, oTopNode, this.nodes);
      }

      return oTopNode.OuterXml;
    }
		
    /// <summary>
    /// Select the first selectable node.
    /// </summary>
    public void GoFirst()
    {
      if(!_globalNavigationPossible)
      {
        return;
      }

      // Go first in top group.
      this.selectedNode = nodes[0];
      
      if(this.selectedNode.NavigateUrl != string.Empty)
      {
        this.selectedNode.Navigate();
      }
      else if(this.selectedNode.AutoPostBackOnSelect)
      {
        this.selectedNodePostbackId = this.selectedNode.PostBackID;
      }
      else
      {
        GoNext();
      }
    }

    /// <summary>
    /// Select the first selectable node in the current group.
    /// </summary>
    public void GoFirstInGroup()
    {
      // None are selected or we are in the top group?
      if(this.selectedNode == null || this.selectedNode.parentNode == null)
      {
        GoFirst();
      }
      else
      {
        // Go first in current group.
        this.selectedNode = this.selectedNode.parentNode.nodes[0];
      }

      if(this.selectedNode.NavigateUrl != string.Empty)
      {
        this.selectedNode.Navigate();
      }
      else if(this.selectedNode.AutoPostBackOnSelect)
      {
        this.selectedNodePostbackId = this.selectedNode.PostBackID;
      }
      else
      {
        GoNextInGroup(false);
      }
    }

    /// <summary>
    /// Select the last selectable node.
    /// </summary>
    public void GoLast()
    {
      if(!_globalNavigationPossible)
      {
        return;
      }

      // Go last in top group.
      NavigationNode oNode = nodes[nodes.Count - 1];

      // Go to last descendant.
      while(oNode.nodes != null && oNode.nodes.Count > 0)
      {
        // Go to last child.
        oNode = oNode.nodes[oNode.nodes.Count - 1];
      }

      this.selectedNode = oNode;

      if(oNode.NavigateUrl != string.Empty)
      {
        oNode.Navigate();
      }
      else if(oNode.AutoPostBackOnSelect)
      {
        this.selectedNodePostbackId = this.selectedNode.PostBackID;
      }
      else
      {
        GoPrevious();
      }
    }

    /// <summary>
    /// Select the last selectable node in the current group.
    /// </summary>
    public void GoLastInGroup()
    {
      // None are selected or we are in the top group?
      if(this.selectedNode == null || this.selectedNode.parentNode == null)
      {
        if(nodes != null && nodes.Count > 0)
        {
          this.selectedNode = nodes[nodes.Count - 1];
        }
        else
        {
          return;
        }
      }
      else
      {
        // Go last in current group.
        this.selectedNode = selectedNode.parentNode.nodes[selectedNode.parentNode.nodes.Count - 1];
      }

      if(this.selectedNode.NavigateUrl != string.Empty)
      {
        this.selectedNode.Navigate();
      }
      else if(this.selectedNode.AutoPostBackOnSelect)
      {
        this.selectedNodePostbackId = this.selectedNode.PostBackID;
      }
      else
      {
        GoPreviousInGroup(false);
      }
    }

    /// <summary>
    /// Select the next selectable node.
    /// </summary>
    public void GoNext()
    {
      GoNext(false);
    }
    
    /// <summary>
    /// Select the next selectable node.
    /// </summary>
    /// <param name="bWrap">Whether to wrap to the beginning after reaching the end.</param>
    public void GoNext(bool bWrap)
    {
      NavigationNode oNode = FindNext(this.selectedNode, bWrap, false);

      if(oNode != null)
      {
        this.selectedNode = oNode;

        if(oNode.NavigateUrl != string.Empty)
        {
          oNode.Navigate();
        }
        else
        {
          this.selectedNodePostbackId = this.selectedNode.PostBackID;
        }
      }
    }

    /// <summary>
    /// Select the next selectable node in the current group.
    /// </summary>
    public void GoNextInGroup()
    {
      GoNextInGroup(false);
    }

    /// <summary>
    /// Select the next selectable node in the current group.
    /// </summary>
    /// <param name="bWrap">Whether to wrap to the beginning when we reach the end.</param>
    public void GoNextInGroup(bool bWrap)
    {
      if(this.selectedNode == null)
      {
        return;
      }

      NavigationNode oNode = this.selectedNode.nextSibling;

      while(oNode != null && oNode != this.selectedNode)
      {
        if(oNode.NavigateUrl != string.Empty || oNode.AutoPostBackOnSelect)
        {
          break;
        }

        // Move next
        oNode = oNode.nextSibling;
        
        // Wrap around potentially
        if(oNode == null && bWrap)
        {
          if(selectedNode.parentNode == null)
          {
            oNode = this.nodes[0];
          }
          else
          {
            oNode = selectedNode.parentNode.nodes[0];
          }
        }
      }

      if(oNode != null)
      {
        this.selectedNode = oNode;

        if(oNode.NavigateUrl != string.Empty)
        {
          oNode.Navigate();
        }
        else
        {
          this.selectedNodePostbackId = this.selectedNode.PostBackID;
        }
      }
    }

    /// <summary>
    /// Select the previous selectable node.
    /// </summary>
    public void GoPrevious()
    {
      GoPrevious(false);
    }

    /// <summary>
    /// Select the previous selectable node.
    /// </summary>
    /// <param name="bWrap">Whether to wrap around to the end after we pass the beginning.</param>
    public void GoPrevious(bool bWrap)
    {
      NavigationNode oNode = FindPrevious(this.selectedNode, bWrap);

      if(oNode != null)
      {
        this.selectedNode = oNode;

        if(oNode.NavigateUrl != string.Empty)
        {
          oNode.Navigate();
        }
        else
        {
          this.selectedNodePostbackId = this.selectedNode.PostBackID;
        }
      }
    }

    /// <summary>
    /// Select the previous selectable node in the current group.
    /// </summary>
    public void GoPreviousInGroup()
    {
      GoPreviousInGroup(false);
    }

    /// <summary>
    /// Select the previous selectable node in the current group.
    /// </summary>
    /// <param name="bWrap">Whether to wrap around to the end after we pass the beginning.</param>
    public void GoPreviousInGroup(bool bWrap)
    {
      if(this.selectedNode == null)
      {
        return;
      }

      NavigationNode oNode = this.selectedNode.previousSibling;

      while(oNode != null && oNode != this.selectedNode)
      {
        if(oNode.NavigateUrl != string.Empty || oNode.AutoPostBackOnSelect)
        {
          break;
        }

        // Move previous
        oNode = oNode.previousSibling;
        
        // Wrap around potentially
        if(oNode == null && bWrap)
        {
          if(selectedNode.parentNode == null)
          {
            oNode = this.nodes[this.nodes.Count - 1];
          }
          else
          {
            oNode = selectedNode.parentNode.nodes[selectedNode.parentNode.nodes.Count - 1];
          }
        }
      }

      if(oNode != null)
      {
        this.selectedNode = oNode;

        if(oNode.NavigateUrl != string.Empty)
        {
          oNode.Navigate();
        }
        else
        {
          this.selectedNodePostbackId = this.selectedNode.PostBackID;
        }
      }
    }

    /// <summary>
    /// Load structure from given XmlDocument.
    /// </summary>
    /// <param name="oXmlDoc">XmlDocument to load from</param>
    public void LoadXml(XmlDocument oXmlDoc)
    {
      if(nodes != null)
      {
        nodes.Clear();
      }

      XmlNodeList arRoots = oXmlDoc.DocumentElement.ChildNodes;
					
      // Add roots and process their children recursively
      foreach(XmlNode oNode in arRoots)
      {
        // Only process Xml elements (ignore comments, etc)
        if(oNode.NodeType == XmlNodeType.Element)
        {
          NavigationNode oNewNode = this.AddNode();
          this.LoadXmlNode(oNewNode, oNode);

          this.OnNodeDataBound(oNewNode, oNode);
        }
      }
    }

    /// <summary>
    /// Load structure from given XML string.
    /// </summary>
    /// <param name="sXml">XML string to load from</param>
    public void LoadXml(string sXml)
    {
      if(nodes != null)
      {
        nodes.Clear();
      }

      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.LoadXml(sXml);

      this.LoadXml(oXmlDoc);
    }

    /// <summary>
    /// Raise a postback event.
    /// </summary>
    /// <param name="eventArgument">Postback argument</param>
    public void RaisePostBackEvent(string eventArgument)
    {     
      this.HandlePostback(eventArgument);
    }

    /// <summary>
    /// Force the re-loading and re-binding of custom node templates.
    /// </summary>
    /// <seealso cref="NavigationNode.ServerTemplateId" />
    /// <seealso cref="ServerTemplates" />
    public void ReloadTemplates()
    {
      this.Controls.Clear();
      this.ComponentArtFixStructure();
      this.InstantiateTemplatedNodes(this.nodes);
    }

#endregion

    #region Protected Properties 

    /// <summary>
    /// ID of node to forcefully highlight. This will make it appear as it would when selected.
    /// </summary>
    [Description("ID of node to forcefully highlight.")]
    [Category("Behavior")]
    [DefaultValue("")]
    protected string ForceHighlightedNodeID
    {
      get
      {
        object o = ViewState["ForceHighlightedNodeID"]; 
        return (o == null) ? String.Empty : (string)o; 
      }

      set
      {
        ViewState["ForceHighlightedNodeID"] = value; 
      }
    }

    /// <summary>
    /// The collection of root nodes.
    /// </summary>
    internal NavigationNodeCollection nodes;
		
    /// <summary>
    /// A list of images to preload.
    /// </summary>
    private StringCollection _preloadImages;
    protected StringCollection PreloadImages
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
    /// The selected node.
    /// </summary>
    private NavigationNode _selectedNode;
    internal NavigationNode selectedNode
    {
      get 
      {
        if(_selectedNode == null)
        {
          object o = ViewState["SelectedNodeId"]; 
          if(o != null && this.nodes != null)
          {
            _selectedNode = (NavigationNode) this.FindNodeByPostBackId((string)o, this.nodes);
          }
        }
        
        return _selectedNode;
      }
      set 
      {
        _selectedNode = value;
        ViewState["SelectedNodeId"] = (value == null? null : value.PostBackID);
      }
    }

    private bool _globalNavigationPossible = false;

    internal string _multiPageId;

#endregion

    #region Protected Methods 

    protected abstract NavigationNode AddNode();
    
    protected abstract NavigationNode NewNode();

    protected void LoadClientData(string sData)
    {
      try
      {
        if (sData != string.Empty)
        {
          this.nodes.Clear();
          
          sData = HttpUtility.UrlDecode(sData, Encoding.UTF8);

          // make it xml-safe
          sData = sData.Replace("&", "#$cAmp@*");

          XmlDocument oXmlDoc = new XmlDocument();
          oXmlDoc.LoadXml(sData);

          XmlNode oRootNode = oXmlDoc.DocumentElement;

          if (oRootNode != null && oRootNode.ChildNodes.Count > 0)
          {
            this.LoadClientXmlNodes(oRootNode.ChildNodes, -1, this.nodes);

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
      catch(Exception ex)
      {
        throw new Exception("Error loading client properties: " + ex);
      }
    }

    protected NavigationNode LoadClientXmlNode(XmlNodeList arClientNodes, XmlNodeList arXmlMembers)
    {
      NavigationNode oNode = NewNode();

      oNode.navigator = this;

      // postback ID
      if (arXmlMembers[0].FirstChild != null)
      {
        oNode.PostBackID = arXmlMembers[0].FirstChild.InnerText;
      }

      // are there properties on this node?
      if (arXmlMembers[3].FirstChild.ChildNodes.Count > 0)
      {
        XmlNodeList arProperties = arXmlMembers[3].FirstChild.ChildNodes;

        this.LoadClientXmlNodeProperties(arProperties, oNode);
      }

      if (arXmlMembers[2].FirstChild.ChildNodes.Count > 0)
      {
        // load children!
        LoadClientXmlNodes(arClientNodes, arXmlMembers[2].FirstChild.ChildNodes, oNode.nodes);
      }

      return oNode;
    }

    protected void LoadClientXmlNodes(XmlNodeList arClientNodes, int iParentIndex, NavigationNodeCollection arNodes)
    {
      string sParentIndex = iParentIndex.ToString();

      foreach(XmlNode oNode in arClientNodes)
      {
        if(oNode.ChildNodes.Count > 0)
        {
          XmlNodeList arElements = oNode.FirstChild.ChildNodes;

          if(arElements[1].InnerText == sParentIndex)
          {
            NavigationNode oNavNode = LoadClientXmlNode(arClientNodes, arElements);
            arNodes.Add(oNavNode);
          }
        }
      }
    }

    protected void LoadClientXmlNodes(XmlNodeList arClientNodes, XmlNodeList arChildIndices, NavigationNodeCollection arNodes)
    {
      foreach(XmlNode oIndex in arChildIndices)
      {
        if(oIndex.InnerText.Length > 0)
        {
          int iIndex = int.Parse(oIndex.InnerText);

          NavigationNode oChildNode = LoadClientXmlNode(arClientNodes, arClientNodes[iIndex].FirstChild.ChildNodes);
          arNodes.Add(oChildNode);
        }
      }
    }

    protected void LoadClientXmlNodeProperties(XmlNodeList arXmlProperties, NavigationNode oNode)
    {
      foreach(XmlNode arProperty in arXmlProperties)
      {
        string sPropertyName = arProperty.FirstChild.FirstChild.InnerText;
        if (Utils.CanParseAsInt(sPropertyName))
        {
          sPropertyName = this.PropertyIndex[sPropertyName].ToString();
        }

        string sPropertyValue = (arProperty.FirstChild.ChildNodes.Count > 1) ? arProperty.FirstChild.LastChild.InnerText : ""; // handle undefined values

        sPropertyValue = sPropertyValue.Replace("#$cAmp@*", "&");
        
        oNode.Properties[oNode.GetAttributeVarName(sPropertyName)] = sPropertyValue;
      }
    }

    protected void LoadClientXmlProperties(XmlNodeList arXmlProperties, System.Web.UI.AttributeCollection arProperties)
    {
      foreach(XmlNode arProperty in arXmlProperties)
      {
        string sPropertyName = arProperty.FirstChild.FirstChild.InnerText;
        string sPropertyValue = (arProperty.FirstChild.ChildNodes.Count > 1) ? arProperty.FirstChild.LastChild.InnerText : ""; // handle undefined values

        sPropertyValue = sPropertyValue.Replace("#$cAmp@*", "&");

        arProperties[sPropertyName] = sPropertyValue;
      }
    }
    
    private Hashtable _propertyIndex;
    internal virtual Hashtable PropertyIndex
    {
      get
      {
        if (_propertyIndex == null)
        {
          _propertyIndex = new Hashtable();
        }
        return _propertyIndex;
      }
    }

    private System.Web.UI.AttributeCollection _properties;
    internal System.Web.UI.AttributeCollection Properties
    {
      get
      {
        if(_properties == null)
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

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      // last chance to fix structure before render
      ComponentArtFixStructure();

      // figure out which node is current, if any
      if(this.selectedNode == null)
      {
        this.UpdateSelectedNode();
      }

      if (this.MultiPageId != null && this.MultiPageId != "")
      {
        // First try to find the corresponding MultiPage nearby (maybe within a user control we're both in)
        Control multiPage = Utils.FindControl(this, this.MultiPageId);
        
        if (multiPage is MultiPage)
          this._multiPageId = multiPage.ClientID; // resolve the MultiPageId
        else
          throw new Exception("Target MultiPage control (" + this.MultiPageId + ") not found on the page.");
      }
    }

    internal virtual bool ConsiderDefaultStyles()
    {
      return false;
    }

    internal string ConvertUrl(string sUrl)
    {
      return Utils.ConvertUrl(Context, this.BaseUrl, sUrl);	
    }

    internal string ConvertImageUrl(string sImageUrl)
    {
      return Utils.ConvertUrl(Context, this.ImagesBaseUrl, sImageUrl);	
    }

    protected override void CreateChildControls()
    {
      if(this.ServerTemplates.Count > 0 && this.nodes != null)
      {
        this.Controls.Clear();
        this.ComponentArtFixStructure();
        this.InstantiateTemplatedNodes(this.nodes);
      }
    }

    /// <summary>
    /// Find the selectable node following the given one in the structure.
    /// </summary>
    /// <param name="oStart">Node to start from.</param>
    /// <param name="bWrap">Whether to wrap when the end is reached.</param>
    /// <param name="bNoDeeper">Whether to not go deeper in the structure when looking.</param>
    /// <returns>The found node.</returns>
    private NavigationNode FindNext(NavigationNode oStart, bool bWrap, bool bNoDeeper)
    {
      if(!_globalNavigationPossible)
      {
        return null;
      }

      // None are selected?
      if(oStart == null)
      {
        return FindNext(this.nodes[0], bWrap, false);
      }
      else if(oStart.nodes != null && oStart.nodes.Count > 0 && !bNoDeeper) // Do we have children?
      {
        oStart = oStart.nodes[0];
      }
      else if(oStart.nextSibling != null) // Do we have siblings?
      {
        oStart = oStart.nextSibling;
      }
      else if(oStart.parentNode != null) // Do we have a parent?
      {
        oStart = oStart.parentNode;
        return FindNext(oStart, bWrap, true);
      }
      else if(bWrap) // Do we wrap around?
      {
        oStart = nodes[0];
      }
      else
      {
        return null;
      }

      // does the found node qualify?
      if(oStart.NavigateUrl != string.Empty || oStart.AutoPostBackOnSelect)
      {
        return oStart;
      }
      else if(oStart != this.selectedNode)
      {
        return FindNext(oStart, bWrap, false);
      }
      else
      {
        return null;
      }
    }

    private NavigationNode FindNodeById(string sNodeID, NavigationNodeCollection arNodes)
    {
      foreach(NavigationNode oNode in arNodes)
      {
        if(oNode.ID == sNodeID)
        {
          return oNode;
        }
				
        if(oNode.nodes != null)
        {
          NavigationNode oFoundBelow = FindNodeById(sNodeID, oNode.nodes);

          if(oFoundBelow != null)
          {
            return oFoundBelow;
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Find the node with the given ID.
    /// </summary>
    /// <param name="sNodeID">The ID to search for.</param>
    /// <returns>The found node or null.</returns>
    protected NavigationNode FindNodeById(string sNodeID)
    {
      if(this.nodes != null)
      {
        return FindNodeById(sNodeID, this.nodes);
      }
      else
      {
        return null;
      }
    }
	 
    protected NavigationNode FindNodeByPostBackId(string sPostBackID, NavigationNodeCollection arNodes)
    {
      if(arNodes == null)
      {
        return null;
      }

      foreach(NavigationNode oNode in arNodes)
      {
        if(oNode.PostBackID == sPostBackID)
        {
          return oNode;
        }
				
        if(oNode.nodes != null)
        {
          NavigationNode oFoundBelow = FindNodeByPostBackId(sPostBackID, oNode.nodes);

          if(oFoundBelow != null)
          {
            return oFoundBelow;
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Find the selectable node preceding the given one in the structure.
    /// </summary>
    /// <param name="oStart">Node to start from.</param>
    /// <param name="bWrap">Whether to wrap when the beginning is reached.</param>
    /// <returns>The found node.</returns>
    private NavigationNode FindPrevious(NavigationNode oStart, bool bWrap)
    {
      if(!_globalNavigationPossible)
      {
        return null;
      }

      // None are selected?
      if(oStart == null)
      {
        return FindNext(this.nodes[0], bWrap, false);
      }
      else if(oStart.previousSibling != null) // Do we have siblings?
      {
        oStart = oStart.previousSibling;

        // Does the new selected node have children?
        while(oStart.nodes != null && oStart.nodes.Count > 0)
        {
          oStart = oStart.nodes[oStart.nodes.Count - 1];
        }
      }
      else if(oStart.parentNode != null) // Do we have a parent?
      {
        oStart = oStart.parentNode;
      }
      else if(bWrap) // Do we wrap around?
      {
        oStart = nodes[nodes.Count - 1];
        while(oStart.nodes != null && oStart.nodes.Count > 0)
        {
          oStart = oStart.nodes[oStart.nodes.Count - 1];
        }
      }
      else
      {
        return null;
      }

      // does the found node qualify?
      if(oStart.NavigateUrl != string.Empty || oStart.AutoPostBackOnSelect)
      {
        return oStart;
      }
      else
      {
        return FindPrevious(oStart, bWrap);
      }
    }

    internal NavigationCustomTemplate FindTemplateById(string sTemplateId)
    {
      foreach(NavigationCustomTemplate oTemplate in ServerTemplates)
      {
        if(oTemplate.ID == sTemplateId)
        {
          return oTemplate;
        }
      }
      
      return null;
    }

    /// <summary>
    /// This function is used to resolve the various references and unique IDs that ensure
    /// a sound tree structure.
    /// </summary>
    protected virtual void ComponentArtFixStructure()
    {
      FixStructureStep(null, this.nodes, 0);
    }

    private int FixStructureStep (
      NavigationNode oParent,
      NavigationNodeCollection arNodes,
      int counter )
    {
      if(arNodes == null)
      {
        return counter;
      }

      NavigationNode oPreviousNode = null;

      foreach(NavigationNode oNode in arNodes)
      {
        if(oNode.ID == null || oNode.ID == string.Empty)
        {
          oNode.PostBackID = String.Format("p{0:X}", counter);
        }
        else
        {
          oNode.PostBackID = "p_" + oNode.ID;
        }
			
        // set parent pointer
        oNode.parentNode = oParent;

        // set navigator
        oNode.navigator = this;

        // do we have a navigable?
        if(!_globalNavigationPossible &&
          (oNode.NavigateUrl != string.Empty || oNode.AutoPostBackOnSelect))
        {
          _globalNavigationPossible = true;
        }
        
        // Deprecate TemplateId, and use ServerTemplateId instead
        if (oNode.Properties["TemplateId"] != null)
        {
          if (oNode.Properties["ServerTemplateId"] == null)
          {
            oNode.Properties["ServerTemplateId"] = oNode.Properties["TemplateId"];
          }
          oNode.Properties.Remove("TemplateId");
        }

        // TODO: fix up prev/next pointers
        oNode.previousSibling = oPreviousNode;
        if(oPreviousNode != null)
        {
          oPreviousNode.nextSibling = oNode;
        }
        oNode.nextSibling = null;

        counter++;

        // maybe we need to read some additional data?
        if(oNode.nodes == null && oNode.SiteMapXmlFile != string.Empty)
        {
          LoadXmlNode(oNode, oNode.SiteMapXmlFile);
        }

        if(oNode.nodes != null)
        {
          counter = FixStructureStep(oNode, oNode.nodes, counter);
        }

        oPreviousNode = oNode;
      }

      return counter;
    }

    private void GetXml(XmlDocument oXmlDoc, XmlNode oXmlNode, NavigationNodeCollection arNodes)
    {
      foreach(NavigationNode oNode in arNodes)
      {
        XmlElement oNewElement = oXmlDoc.CreateElement("Node");
				
        foreach(string sKey in oNode.Properties.Keys)
        {
          oNewElement.SetAttribute(sKey, oNode.Properties[sKey]);
        }

        oXmlNode.AppendChild(oNewElement);
        if(oNode.nodes != null)
        {
          GetXml(oXmlDoc, oNewElement, oNode.nodes);
        }
      }
    }

    protected virtual void HandlePostback(string stringArgument)
    {
      return;
    }

    /// <summary>
    /// Go through nodes, finding ones that reference templates, and 
    /// instantiate those templates using the nodes.
    /// </summary>
    /// <param name="arNodes">The nodes to begin searching from.</param>
    private void InstantiateTemplatedNodes(NavigationNodeCollection arNodes)
    {
      foreach(NavigationNode oNode in arNodes)
      {
        if(oNode.ServerTemplateId != string.Empty)
        {
          NavigationCustomTemplate oTemplate = this.FindTemplateById(oNode.ServerTemplateId);

          if(oTemplate == null)
          {
            throw new Exception("Template not found: " + oNode.ServerTemplateId);
          }

          NavigationTemplateContainer oContainer = new NavigationTemplateContainer(oNode);
          oTemplate.Template.InstantiateIn(oContainer);
          oContainer.ID = this.ClientID + "_" + oNode.PostBackID;
          oContainer.DataBind();

          this.Controls.Add(oContainer);
        }

        if(oNode.nodes != null)
        {
          InstantiateTemplatedNodes(oNode.nodes);
        }
      }
    }
    

    protected void LoadFromHierarchy(IHierarchicalEnumerable oEnum, NavigationNode oParent)
    {
      IEnumerator oEnumerator = oEnum.GetEnumerator();

      oEnumerator.Reset();
      oEnumerator.MoveNext();

      while (oEnumerator.Current != null)
      {
        IHierarchyData oData = oEnum.GetHierarchyData(oEnumerator.Current);

        NavigationNode oNavigationNode = oParent == null? this.AddNode() : oParent.AddNode();
        oNavigationNode.Text = oData.Item != null ? oData.Item.ToString() : "";

        if(oData.HasChildren)
        {
          IHierarchicalEnumerable arChildren = oData.GetChildren();

          LoadFromHierarchy(arChildren, oNavigationNode);
        }

        oEnumerator.MoveNext();
      }
    }

    protected void LoadFromSiteMap(System.Web.SiteMapNode oRoot, bool bIncludeRoot)
    {
      if (bIncludeRoot)
      {
        NavigationNode oRootNode = this.AddNode();

        this.LoadFromSiteMapNode(oRootNode, oRoot);

        this.OnNodeDataBound(oRootNode, oRoot);
      }
      else
      {
        foreach (System.Web.SiteMapNode oNode in oRoot.ChildNodes)
        {
          NavigationNode oNavigationNode = this.AddNode();
          this.LoadFromSiteMapNode(oNavigationNode, oNode);

          this.OnNodeDataBound(oNavigationNode, oNode);
        }
      }
    }

    protected void LoadFromSiteMapNode(NavigationNode oNavigationNode, System.Web.SiteMapNode oNode)
    {
      oNavigationNode.NavigateUrl = oNode.Url;
      oNavigationNode.Text = oNode.Title;
      oNavigationNode.ToolTip = oNode.Description;
      oNavigationNode.ID = oNode.Key;
      
      // use attribute mappings to access additional properties
      foreach (CustomAttributeMapping oMapping in this.CustomAttributeMappings)
      {
        string sValue = oNode[oMapping.From];

        if (sValue != null)
        {
          oNavigationNode.Properties[oMapping.From] = sValue;
        }
      }

      // load children
      foreach (System.Web.SiteMapNode oChildNode in oNode.ChildNodes)
      {
        NavigationNode oChildNavigationNode = oNavigationNode.AddNode();
        this.LoadFromSiteMapNode(oChildNavigationNode, oChildNode);

        this.OnNodeDataBound(oChildNavigationNode, oChildNode);
      }
    }


    protected void LoadXmlNode(NavigationNode oNavigationNode, string sXmlFile)
    {
      if(Context == null)
      {
        return;
      }

      string sServerPath = Context.Server.MapPath(sXmlFile);

      if(!File.Exists(sServerPath))
      {
        throw new FileNotFoundException("File " + sServerPath + " does not exist.");
      }

      // Load XML from SiteMapXmlFile
      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.Load(sServerPath);

      XmlNodeList arNodeList = oXmlDoc.DocumentElement.ChildNodes;

      // Add roots and process their children recursively
      foreach(XmlNode oNode in arNodeList)
      {
        // Only process Xml elements (ignore comments, etc)
        if(oNode.NodeType == XmlNodeType.Element)
        {
          NavigationNode oNewNode = oNavigationNode.AddNode();
          this.LoadXmlNode(oNewNode, oNode);

          this.OnNodeDataBound(oNewNode, oNode);
        }
      } 
    }

    protected void LoadXmlNode(NavigationNode oNavigationNode, XmlNode oXmlNode)
    {
      oNavigationNode.ReadXmlAttributes(oXmlNode.Attributes);

      if(oNavigationNode.SiteMapXmlFile != string.Empty)
      {
        this.LoadXmlNode(oNavigationNode, oNavigationNode.SiteMapXmlFile);
      }
      else
      {
        // Add roots and process their children recursively
        foreach(XmlNode oNode in oXmlNode.ChildNodes)
        {
          // Only process Xml elements (ignore comments, etc)
          if(oNode.NodeType == XmlNodeType.Element)
          {
            NavigationNode oNewNode = oNavigationNode.AddNode();
            this.LoadXmlNode(oNewNode, oNode);

            this.OnNodeDataBound(oNewNode, oNode);
          }
        }
      }
    }
    
    private void PopulateNodePreviousUrls()
    {
      if (this.nodes.Count > 0)
      {
        this._curPreviousUrl = null;
        for (int i=0; i<this.nodes.Count; i++)
        {
          this.PopulateNodePreviousUrls(this.nodes[i]);
        }
      }
    }
    private string _curPreviousUrl;
    private void PopulateNodePreviousUrls(NavigationNode curNode)
    {
      if (curNode.nodes != null)
      {
        for (int i=0; i<curNode.nodes.Count; i++)
        {
          NavigationNode childNode = curNode.nodes[i];
          this.PopulateNodePreviousUrls(childNode);
        }
      }
      curNode.previousUrl = this._curPreviousUrl;
      if (curNode.NavigateUrl != null && curNode.NavigateUrl != String.Empty)
      {
        this._curPreviousUrl = curNode.NavigateUrl;
      }
    }
    private void PopulateNodeNextUrls()
    {
      if (this.nodes.Count > 0)
      {
        this._curNextUrl = null;
        for (int i=this.nodes.Count-1; i>=0; i--)
        {
          this.PopulateNodeNextUrls(this.nodes[i]);
        }
      }
    }
    private string _curNextUrl;
    private void PopulateNodeNextUrls(NavigationNode curNode)
    {
      if (curNode.nodes != null)
      {
        for (int i=curNode.nodes.Count-1; i>=0; i--)
        {
          NavigationNode childNode = curNode.nodes[i];
          this.PopulateNodeNextUrls(childNode);
        }
      }
      curNode.nextUrl = this._curNextUrl;
      if (curNode.NavigateUrl != null && curNode.NavigateUrl != String.Empty)
      {
        this._curNextUrl = curNode.NavigateUrl;
      }
    }


    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      if(Context != null && Page != null)
      {
        string dummy = Page.GetPostBackEventReference(this); // Ensure that __doPostBack is output to client side
      }
      
      // Do nothing if this isn't an initial run.
      if(EnableViewState && Context != null && Page.IsPostBack)
        return;


      if (this.DataSourceID != "")
      {
        Control oControl = Utils.FindControl(this, this.DataSourceID);

        if (oControl != null)
        {
          if (oControl is SiteMapDataSource)
          {
            SiteMapDataSource oDS = (SiteMapDataSource)oControl;

            this.LoadFromSiteMap(oDS.Provider.RootNode, oDS.ShowStartingNode);
          }
          else if (oControl is XmlDataSource)
          {
            XmlDataSource oDS = (XmlDataSource)oControl;
            XmlDocument oXmlDoc = oDS.GetXmlDocument();

            this.LoadXml(oXmlDoc);
          }
          else if (oControl is IHierarchicalDataSource)
          {
            IHierarchicalDataSource oDS = (IHierarchicalDataSource)oControl;

            this.LoadFromHierarchy(oDS.GetHierarchicalView("").Select(), null);
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

      this.UpdateSelectedNode();
    }

    protected override void LoadViewState(object savedState)
    {
      base.LoadViewState(savedState);

      if(ViewState["SiteMapXml"] != null)
      {
        this.LoadXml((string)ViewState["SiteMapXml"]);
        bNewDataLoaded = true;
        this.ComponentArtFixStructure();
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      // do we have no content?
      if((this.nodes == null || this.nodes.Count == 0) && this.SiteMapXmlFile != string.Empty && Context != null) // if we have an xml file, load it up
      {
        string sServerPath = Context.Server.MapPath(this.SiteMapXmlFile);

        if(!File.Exists(sServerPath))
        {
          throw new FileNotFoundException("Specified SiteMapXmlFile does not exist.");
        }

        // Load XML from SiteMapXmlFile
        XmlDocument oXmlDoc = new XmlDocument();
        oXmlDoc.Load(sServerPath);

        this.LoadXml(oXmlDoc);
        
        if(this.nodes != null && this.nodes.Count > 0)
        {
          bNewDataLoaded = true;
        }
      }

      // at this point, we have the structure loaded.
      ComponentArtFixStructure();

      this.UpdateSelectedNode();
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      // Persist tree structure
      if(this.EnableViewState && !(this is BaseMenu) && !(this is TreeView))
      {
        ViewState["SiteMapXml"] = this.GetXml();
      }

      // ConsiderDefaultStyles
      if (this.ConsiderDefaultStyles())
      {
        this.RenderDefaultStyles = true;
      }
    }

    private void RenderCrawlerNodes(NavigationNodeCollection arNodes, HtmlTextWriter output)
    {
      foreach(NavigationNode oNode in arNodes)
      {
        if(oNode.NavigateUrl != string.Empty)
        {
          output.AddAttribute("href", oNode.NavigateUrl);
          output.RenderBeginTag(HtmlTextWriterTag.A);
          output.Write(oNode.Text);
          output.RenderEndTag(); // </a>
        }

        if(oNode.nodes != null)
        {
          RenderCrawlerNodes(oNode.nodes, output);
        }
      }
    }

    protected void RenderCrawlerStructure(HtmlTextWriter output)
    {
      output.RenderBeginTag(HtmlTextWriterTag.Noscript);

      if(this.nodes != null && this.nodes.Count > 0)
      {
        RenderCrawlerNodes(this.nodes, output);
      }

      output.RenderEndTag();
    }

    protected void RenderPreloadImages(HtmlTextWriter output)
    {
      output.Write("<div style=\"position:absolute;top:0px;left:0px;visibility:hidden;\">"); 
      foreach(string sImage in this.PreloadImages)
      {
        output.Write("<img src=\"" + sImage + "\" width=\"0\" height=\"0\" alt=\"\" />\n");
      }
      output.Write("</div>"); 
    }

    // This method determines which NavNode should be designated as the SelectedNode 
    protected void UpdateSelectedNode()
    {
      if(Context == null || Page == null)
      {
        return;
      }

      if (Page != null && Page.Request != null && Page.Request.HttpMethod == "POST" && Page.Request.Params != null)
      {
        string sEventTarget = Page.Request.Params["__EVENTTARGET"];
        if(sEventTarget != null && sEventTarget != string.Empty && this.ClientID == sEventTarget)
        {
          if(this.selectedNode == null)
          {
            string postbackNodeId = Page.Request.Params["__EVENTARGUMENT"];
            
            if(postbackNodeId != null && postbackNodeId != string.Empty)
            {
              NavigationNode itemThatCausedPostback = FindNodeByPostBackId(postbackNodeId, this.nodes);
              this.selectedNode = itemThatCausedPostback;
            }
          }
        }
      }

      // try the regular means of finding the selected node
      if(selectedNode == null)
      {
        NavigationNode oFoundNode = FindSelectedNode();
          
        if(oFoundNode != null)
        {
          this.selectedNode = oFoundNode;
        }
      }    
    }

    #region Logic for finding the current node
    /* Algorithm for picking the SelectedNode when page is not a postback (when it's an HTTP GET).
    * (Looking for the item with the URL that best corresponds to the URL of the current request.)
    * 
    * URL match quality, from best to worst:
    * 
    * a) Node's URL points to the same file as the current request and has an identical query string
    * b) Node's URL points to the same file as the current request, its query params are a subset of
    *    the query params of the current request, and no menu item has a bigger matching subset of params.
    *    (This means that the node's URL can include all the query params, some of the params, or none of
    *    the params, and the ordering of the params in the query string never matters.)
    * c) All else is not a match. (Node doesn't have a URL, or the URL doesn't point to the same file 
    *    as the current request, or it contains some query parameters not found in the request.)
    * 
    * Note: paths are case-insensitive, query strings are case-sensitive. */

    private NavigationNode bestMatchItemNode = null;
    private bool bestMatchQueryIdentical = false;
    private int bestMatchQueryParamsMatched = -1;

    protected NavigationNode FindSelectedNode()
    {
      // Reset the bestMatch data for the brand-new search:
      bestMatchItemNode = null;
      bestMatchQueryIdentical = false;
      bestMatchQueryParamsMatched = -1; // not applicable
      
      // Let's search:
      if(this.RenderRootNodeId != string.Empty)
      {
        NavigationNode oRootNode = this.FindNodeById(this.RenderRootNodeId);
        if(oRootNode != null && oRootNode.nodes != null)
        {
          SearchForSelectedNode(oRootNode.nodes);
        }

        if(bestMatchItemNode == null)
        {
          SearchForSelectedNode(nodes);
        }
      }
      else
      {
        SearchForSelectedNode(nodes);
      }
      
      return bestMatchItemNode;
    }

    // Recursively search through the given group looking for the item whose
    // URL best corresponds to the URL of the page that we are rendering.
    // Class variables bestMatch* are being modified as we search
    // (usually not the best programming practice, but this case seems like an exception).
    private void SearchForSelectedNode(NavigationNodeCollection arNodes)
    {
      if(arNodes != null)
      {
        foreach (NavigationNode oNode in arNodes)
        {
          UpdateBestMatch(oNode);
          if (bestMatchQueryIdentical)
            return; // might as well cut the search early since we have found a perfect match
				
          if(oNode.nodes != null)
          {
            SearchForSelectedNode(oNode.nodes); // recurse into subgroup
            if (bestMatchQueryIdentical)
              return; // might as well cut the search early since we have found a perfect match
          }
        }
      }
    }   

    // Takes a look at the URL of the given menu item and compares it to the URL of the current request.
    // If is the best match that we have seen so far, it updates bestMatch* class variables.
    // This routine has many exits, but each one is explained with a comment.
    private void UpdateBestMatch(NavigationNode givenNode)
    {
      if (bestMatchQueryIdentical)
        return; // a perfect match has already been found, and we sure can't do any better than that
      string givenUrlRaw = givenNode.NavigateUrl;
      if (givenUrlRaw == string.Empty)
        return; // no URL, so this is not a match
      Uri idealUrl = Context.Request.Url;
      Uri givenUrl = new Uri(idealUrl, givenUrlRaw);
      bool pathsEqual = (idealUrl.AbsolutePath.ToLower() == givenUrl.AbsolutePath.ToLower()
        || givenUrlRaw.StartsWith("?"));
      if (!pathsEqual)
        return; // points to a different file, so this is not a match
      bool queriesIdentical = (givenUrl.Query.TrimStart('?') == idealUrl.Query.TrimStart('?'));
      if (queriesIdentical)
      {
        bestMatchItemNode = givenNode;
        bestMatchQueryIdentical = true;
        bestMatchQueryParamsMatched = -1; // not applicable, they surely have all been matched
        return; // new best match (and a perfect one, too)
      }
      NameValueCollection idealQueryParams = Context.Request.QueryString;
      if (bestMatchQueryParamsMatched == idealQueryParams.Count)
        return; // we have no more chance of beating the best match
      NameValueCollection givenQueryParams = TokenizeQuery(givenUrl.Query.TrimStart('?'));
      if (idealQueryParams.Count < givenQueryParams.Count)
        return; // too many query params to be a subset - hence not a match
      if (givenQueryParams.Count <= bestMatchQueryParamsMatched)
        return; // we have no more chance of beating the best match
      if (IsSubset(givenQueryParams, idealQueryParams))
      {
        bestMatchItemNode = givenNode;
        bestMatchQueryIdentical = false;
        bestMatchQueryParamsMatched = givenQueryParams.Count;
        return; // new best match
      }
      // not a match - item has some query params not found in the URL of the current request
    }

    // Given a URL, checks to see whether it is equivalent to the URL of the current request.
    // It will be considered equivalent if it points to the same file and if there is a 
    // one-to-one matching between the two URLs' query parameters (regardless of order).
    internal bool IsEquivalentToCurrentURL(string givenUrlRaw)
    {
      if (givenUrlRaw == null || givenUrlRaw == "")
        return false; // no URL, so this is not a match
      Uri idealUrl = Context.Request.Url;
      Uri givenUrl = new Uri(idealUrl, givenUrlRaw);
      bool pathsEqual = (idealUrl.AbsolutePath.ToLower() == givenUrl.AbsolutePath.ToLower()
        || givenUrlRaw.StartsWith("?"));
      if (!pathsEqual)
        return false; // points to a different file, so this is not a match
      bool queriesIdentical = (givenUrl.Query.TrimStart('?') == idealUrl.Query.TrimStart('?'));
      if (queriesIdentical)
        return true; // perfect match
      NameValueCollection idealQueryParams = Context.Request.QueryString;
      NameValueCollection givenQueryParams = TokenizeQuery(givenUrl.Query.TrimStart('?'));
      if (idealQueryParams.Count != givenQueryParams.Count)
        return false; // can't be a good enough match with a different number of query params
      // Finally: if query param collections have the same number of elements and one is the subset of 
      // the other then they must contain the same exact elements (only maybe in different order):
      return (IsSubset(givenQueryParams, idealQueryParams));
    }

    // Tokenizes the query string.  Given "pa=va&pb=vb" returns [("pa","va") , ("pb","vb")]
    // This routine is not intended for work with unusual query strings like:
    // "pa=va&ParamWithNoValue&pb=vb" or "pa=va&EndsWithAnAmpersand=vb&"
    private NameValueCollection TokenizeQuery(string queryString)
    {
      NameValueCollection queryCollection = new NameValueCollection();
      string [] keysAndValues = queryString.Split(new char[] {'&','='});
      for (int i=0; i+2<=keysAndValues.Length; i+=2)
        queryCollection.Add(keysAndValues[i], keysAndValues[i+1]);
      return queryCollection;
    }

    // Given two NameValueCollections checks whether the first is a subset of the second
    private bool IsSubset(NameValueCollection subset, NameValueCollection superset)
    {
      foreach (string key in subset.AllKeys)
        if (subset[key] != superset[key])
          return false;
      return true;
    }

#endregion // Logic for finding the current menu item

#endregion

		#region Private Properties 

		internal bool RenderDefaultStyles
		{
			get
			{
				return Utils.ParseBool(ViewState["RenderDefaultStyles"],false);
			}
			set
			{
				ViewState["RenderDefaultStyles"] = value;
			}
		}

		#endregion

    protected abstract void OnNodeDataBound(NavigationNode oNode, object oDataItem);
  }
}



