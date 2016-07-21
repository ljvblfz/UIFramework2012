using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices; 
using ComponentArt.Licensing.Providers;
using System.Web.UI.HtmlControls;

namespace ComponentArt.Web.UI
{
  #region WebService classes

  /// <summary>
  /// Contains information describing a web service node expand request.
  /// </summary>
  /// <remarks>
  /// This is the type passed to the web service method set to handle such requests.
  /// </remarks>
  /// <seealso cref="TreeView.WebService" />
  /// <seealso cref="TreeView.WebServiceMethod" />
  public class TreeViewWebServiceRequest
  {
    /// <summary>
    /// The node that needs expanding.
    /// </summary>
    public TreeViewNode Node;

    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains information to be included in a web service node expand response.
  /// </summary>
  /// <remarks>
  /// This is the type returned from the web service method set to handle such requests.
  /// </remarks>
  /// <seealso cref="TreeView.WebService" />
  /// <seealso cref="TreeView.WebServiceMethod" />
  public class TreeViewWebServiceResponse : BaseNavigatorWebServiceResponse
  {
    TreeViewNodeCollection _nodes;

    /// <summary>
    /// Node data to be sent back to the client. Read-only.
    /// </summary>
    public ArrayList Nodes
    {
      get
      {
        return NodesToArray(_nodes);        
      }
    }

    public TreeViewWebServiceResponse()
    {
      _nodes = new TreeViewNodeCollection(null, null);
    }

    public void AddNode(TreeViewNode oNode)
    {
      _nodes.Add(oNode);
    }
  }

  #endregion

  #region EventArgs classes

  /// <summary>
  /// Arguments for <see cref="TreeView.NodeDataBound"/> server-side event of <see cref="TreeView"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class TreeViewNodeDataBoundEventArgs : EventArgs
  {
    /// <summary>
    /// The TreeView node.
    /// </summary>
    public TreeViewNode Node;

    /// <summary>
    /// The data item bound to.
    /// </summary>
    public object DataItem;
  }

  /// <summary>
  /// Arguments for some <see cref="TreeViewNode">node</see>-centric server-side events of the <see cref="TreeView"/> control.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Arguments of this type are used by the following events: <see cref="TreeView.NodeSelected"/>, <see cref="TreeView.NodeCheckChanged"/>,
  /// <see cref="TreeView.NodeExpanded"/>, and <see cref="TreeView.NodeCollapsed"/>.
  /// </para>
  /// <para>
  /// Following classes inherit from <b>TreeViewNodeEventArgs</b> class: <see cref="TreeViewNodeCopiedEventArgs"/> used by 
  /// <see cref="TreeView.NodeCopied"/> event, <see cref="TreeViewNodeMovedEventArgs"/> used by <see cref="TreeView.NodeMoved"/> event, 
  /// and <see cref="TreeViewNodeRenamedEventArgs"/> used by <see cref="TreeView.NodeRenamed"/> event.
  /// </para>
  /// </remarks>
	[ToolboxItem(false)]
	public class TreeViewNodeEventArgs : EventArgs
	{
    /// <summary>
    /// The command name.
    /// </summary>
		public string Command;

    /// <summary>
    /// The node in question.
    /// </summary>
		public TreeViewNode Node;
	}

  /// <summary>
  /// Arguments for <see cref="TreeView.NodeCopied"/> server-side event of <see cref="TreeView"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class TreeViewNodeCopiedEventArgs : TreeViewNodeEventArgs
  {
    /// <summary>
    /// Node that was copied.
    /// </summary>
    public TreeViewNode CopiedFrom;
  }

  /// <summary>
  /// Arguments for <see cref="TreeView.NodeMoved"/> server-side event of <see cref="TreeView"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class TreeViewNodeMovedEventArgs : TreeViewNodeEventArgs
  {
    /// <summary>
    /// Previous parent.
    /// </summary>
    public TreeViewNode OldParent;
    
    /// <summary>
    /// Index within the previous group.
    /// </summary>
    public int OldIndex;

    /// <summary>
    /// Previous parent TreeView.
    /// </summary>
    public TreeView OldTreeView;
  }

  /// <summary>
  /// Arguments for <see cref="TreeView.NodeRenamed"/> server-side event of <see cref="TreeView"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class TreeViewNodeRenamedEventArgs : TreeViewNodeEventArgs
  {
    /// <summary>
    /// Previous label.
    /// </summary>
    public string OldText;
  }

  #endregion

  /// <summary>
  /// Main class for the TreeView control. 
  /// </summary>
  /// <remarks>
  /// <para>
  /// The TreeView control is used to display hierarchical data, represented visually in an interactive tree structure.
  /// Every TreeView instance is made up of nodes, encapsulated by <see cref="TreeViewNode" /> objects. 
  /// The root nodes for a TreeView are contained within a <see cref="TreeViewNodeCollection" /> which is accessed through the 
  /// TreeView's <see cref="TreeView.Nodes">Nodes</see> property. Each TreeViewNode object also contains a <see cref="TreeViewNode.Nodes">Nodes</see>
  /// property and corresponding TreeViewNodeCollection, creating the hierarchical structure of the control.
  /// </para>
  /// <para>
  /// The TreeView can be populated in a number of different ways. The following tutorials contain information on each of the methods:
  /// <list type="bullet">
  /// <item>
  /// <description>
  /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/TreeView_Using_the_TreeView_Designer.htm">Using the TreeView Designer</a>
  /// </description>
  /// </item>
  /// <item>
  /// <description>
  /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/TreeView_Binding_with_an_XML_File.htm">Binding with an XML File</a> 
  /// (and the corresponding <a href="ms-help:/../ComponentArt.Web.UI.AJAX/TreeView_Binding_with_an_XML_File__Code_Walkthrough.htm">Code Walkthrough</a>)
  /// </description>
  /// </item>
  /// <item>
  /// <description>
  /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/TreeView_Using_Load_on_Demand.htm">Using Load on Demand</a> 
  /// </description>
  /// </item>
  /// <item>
  /// <description>
  /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/WebServices_TreeView_Load_On_Demand.htm">Using Web Service Load on Demand</a> 
  /// </description>
  /// </item>
  /// </list>
  /// <para>
  /// The TreeView control is styled using CSS in combination with the look and feel properties, which are discussed in the
  /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/TreeView_Look_and_Feel_Properties.htm">Look and Feel Properties</a> tutorial.
  /// </para>
  /// <para>
  /// In addition to styling the control using CSS, individual nodes can be further customized using templates.
  /// Either <see cref="BaseNavigator.ServerTemplates" /> or <see cref="BaseNavigator.ClientTemplates" /> can be used, depending on the
  /// specific needs of the application. The 
  /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/TreeView_Look_and_Feel_Properties.htm">Overview of Templates in Web.UI</a> 
  /// tutorial discusses this topic in detail.
  /// </para> 
  /// </remarks>
  [GuidAttribute("53f586d7-6911-4cb0-82bd-564a9f882220")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:TreeView Width=\"200\" Height=\"300\" runat=\"server\"></{0}:TreeView>")]
  [PersistChildren(false)]
  [ParseChildren(true)]
  [Designer(typeof(ComponentArt.Web.UI.TreeViewNodesDesigner))]
  public sealed class TreeView : BaseNavigator
  {
    #region Private Properties	

    private TreeViewNode _forceHighlightedNode;
    internal TreeViewNode ForceHighlightedNode
    {
      get
      {
        if(this.ForceHighlightedNodeID != string.Empty)
        {
          if(_forceHighlightedNode == null || _forceHighlightedNode.ID != this.ForceHighlightedNodeID)
          {
            _forceHighlightedNode = this.FindNodeById(this.ForceHighlightedNodeID);
          }

          return _forceHighlightedNode;
        }

        return null;
      }
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Whether to allow text selection in this TreeView.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to allow text selection in this TreeView.")]
    public bool AllowTextSelection
    {
      get
      {
        return Utils.ParseBool(Properties["AllowTextSelection"], false);
      }
      set
      {
        Properties["AllowTextSelection"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to automatically assign IDs to nodes programmatically created on the client. Default: false.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to automatically assign IDs to nodes programmatically created on the client. Default: false.")]
    [DefaultValue(false)]
    public bool AutoAssignNodeIDs
    {
      get
      {
        return Utils.ParseBool(Properties["AutoAssignNodeIDs"], false);
      }
      set
      {
        Properties["AutoAssignNodeIDs"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to perform a postback when a node is selected. Default: false.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to perform a postback when a node is selected. Default: false.")]
    [DefaultValue(false)]
    public new bool AutoPostBackOnSelect
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
    /// Whether to perform a postback when a node is moved by dragging and dropping. Default: false.
    /// </summary>
    [Description("Whether to perform a postback when a node is moved by dragging and dropping. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnNodeMove
    {
      get 
      {
        return Utils.ParseBool(Properties["AutoPostBackOnNodeMove"], false); 
      }
      set 
      {
        Properties["AutoPostBackOnNodeMove"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to perform a postback when a node's label is changed. Default: false.
    /// </summary>
    [Description("Whether to perform a postback when a node's label is changed. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnNodeRename 
    {
      get 
      {
        return Utils.ParseBool(Properties["AutoPostBackOnNodeRename"], false); 
      }
      set 
      {
        Properties["AutoPostBackOnNodeRename"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to perform a postback when a node is checked or unchecked. Default: false.
    /// </summary>
    [Description("Whether to perform a postback when a node is checked or unchecked. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnNodeCheckChanged  
    {
      get 
      {
        return Utils.ParseBool(Properties["AutoPostBackOnNodeCheckChanged"], false); 
      }
      set 
      {
        Properties["AutoPostBackOnNodeCheckChanged"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to perform a postback when a node is expanded. Default: false.
    /// </summary>
    [Description("Whether to perform a postback when a node is expanded. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnNodeExpand
    {
      get 
      {
        return Utils.ParseBool(Properties["AutoPostBackOnNodeExpand"], false); 
      }
      set 
      {
        Properties["AutoPostBackOnNodeExpand"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to perform a postback when a node is collapsed. Default: false.
    /// </summary>
    [Description("Whether to perform a postback when a node is collapsed. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnNodeCollapse
    {
      get 
      {
        return Utils.ParseBool(Properties["AutoPostBackOnNodeCollapse"], false); 
      }
      set 
      {
        Properties["AutoPostBackOnNodeCollapse"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to apply scrollbars to the TreeView frame as its contents grow, keepings its dimensions constant,
    /// instead of letting the frame grow with the content. Default: true.
    /// </summary>
    [Description("Whether to apply scrollbars to the TreeView frame as its contents grow, keepings its dimensions constant, instead of letting the frame grow with the content. Default: true.")]
    [Category("Appearance")]
    [DefaultValue(true)]
    public bool AutoScroll
    {
      get 
      {
        return Utils.ParseBool(Properties["AutoScroll"], true); 
      }
      set 
      {
        Properties["AutoScroll"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to auto-set style-related properties to connect the control with a pre-packaged theme. Default: false.
    /// </summary>
    [Category("Appearance")]
    [Description("Whether to auto-set style-related properties to connect the control with a pre-packaged theme. Default: false.")]
    [DefaultValue(false)]
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
        object o = ViewState["AutoThemingCssClassPrefix"];
        return o == null ? "cart-" : (string)o;
      }
      set
      {
        ViewState["AutoThemingCssClassPrefix"] = value;
      }
    }

    /// <summary>
    /// The list of nodes which are currently checked.
    /// </summary>
    /// <remarks>This is a read-only property.</remarks>
    /// <seealso cref="TreeViewNode.ShowCheckBox" />
    /// <seealso cref="TreeViewNode.Checked" />
    [Description("The list of checkable nodes which are currently checked.")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreeViewNode [] CheckedNodes
    {
      get
      {
        return GetCheckedNodes(this.Nodes);
      }
    }

    private TreeViewClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public TreeViewClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new TreeViewClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// The CSS class to apply to ancestors of the selected node, on hover.
    /// </summary>
    [Description("The CSS class to apply to ancestors of the selected node, on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ChildSelectedHoverNodeCssClass
    {
      get 
      {
        string s = Properties["ChildSelectedHoverNodeCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ChildSelectedHoverNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to the rows of ancestors of the selected node, on hover.
    /// </summary>
    [Description("The CSS class to apply to the rows of ancestors of the selected node, on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ChildSelectedHoverNodeRowCssClass
    {
      get 
      {
        string s = Properties["ChildSelectedHoverNodeRowCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ChildSelectedHoverNodeRowCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to ancestors of the selected node.
    /// </summary>
    [Description("The CSS class to apply to ancestors of the selected node.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ChildSelectedNodeCssClass
    {
      get 
      {
        string s = Properties["ChildSelectedNodeCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ChildSelectedNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to rows of ancestors of the selected node.
    /// </summary>
    [Description("The CSS class to apply to rows of ancestors of the selected node.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ChildSelectedNodeRowCssClass
    {
      get 
      {
        string s = Properties["ChildSelectedNodeRowCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ChildSelectedNodeRowCssClass"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when a content callback request is complete. The sole parameter to the handler
    /// is the TreeViewNode which initiated the callback.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Client-side command to execute when a content callback request is complete.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnCallbackComplete
    {
      get 
      {
        string s = Properties["ClientSideOnCallbackComplete"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnCallbackComplete"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when the TreeView is done loading on the client.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Client-side command to execute when a content callback request is complete.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnLoad
    {
      get
      {
        string s = Properties["ClientSideOnLoad"];
        return s == null ? string.Empty : s;
      }
      set
      {
        Properties["ClientSideOnLoad"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when a node is checked or unchecked. This should be the identifier of a client-side 
    /// function which takes as its parameter a client-side TreeViewNode object. The function should return a boolean
    /// value indicating whether the event should be canceled.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Identifier of the client-side function to call when a node is checked or unchecked. This is the identifier of a client script function which takes as its parameter a client-side TreeViewNode object.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeCheckChanged
    {
      get 
      {
        string s = Properties["ClientSideOnNodeCheckChanged"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeCheckChanged"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when a node is collapsed. This is the identifier of a client script
    /// function which takes as its parameter a client-side TreeViewNode object. The function should return a boolean
    /// value indicating whether the event should be canceled.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Identifier of the client-side function to call when a node is collapsed. This is the identifier of a client script function which takes as its parameter a client-side TreeViewNode object.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeCollapse
    {
      get 
      {
        string s = Properties["ClientSideOnNodeCollapse"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeCollapse"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when a node is copied. This is the identifier of a client script
    /// function which takes as its parameters two client-side TreeViewNode objects, the node being copied and its
    /// new parent node. The function should return a boolean value indicating whether the event should be canceled.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Identifier of the client-side function to call when a node is copied.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeCopy
    {
      get 
      {
        string s = Properties["ClientSideOnNodeCopy"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeCopy"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when a node is expanded. This is the identifier of a client script
    /// function which takes as its parameter a client-side TreeViewNode object. The function should return a boolean
    /// value indicating whether the event should be canceled.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Identifier of the client-side function to call when a node is expanded.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeExpand
    {
      get 
      {
        string s = Properties["ClientSideOnNodeExpand"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeExpand"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when a node is dropped on an external target. This is the identifier of a client
    /// script function which takes as its parameters a client-side TreeViewNode object and a DOM element on which the
    /// node was dropped.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Identifier of the client-side function to call when a node is dropped on an external target.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeExternalDrop
    {
      get 
      {
        string s = Properties["ClientSideOnNodeExternalDrop"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeExternalDrop"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when the mouse is double-clicked on a node.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Identifier of the client-side function to call when the mouse is double-clicked on a node.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeMouseDoubleClick
    {
      get 
      {
        string s = Properties["ClientSideOnNodeMouseDoubleClick"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeMouseDoubleClick"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when the mouse hovers over a node.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Identifier of the client-side function to call when the mouse hovers over a node.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeMouseOver
    {
      get 
      {
        string s = Properties["ClientSideOnNodeMouseOver"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeMouseOver"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when the mouse leaves a node.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Identifier of the client-side function to call when the mouse leaves a node.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeMouseOut
    {
      get 
      {
        string s = Properties["ClientSideOnNodeMouseOut"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeMouseOut"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when a node is moved. This is the identifier of a client script
    /// function which takes as its parameters two client-side TreeViewNode objects: the node which was moved,
    /// and its new parent node. The boolean value which the function should return determines whether the
    /// action will proceed (true) or be canceled (false).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Client-side command to execute when a node is moved.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeMove
    {
      get 
      {
        string s = Properties["ClientSideOnNodeMove"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeMove"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when a node is renamed. This is the identifier of a client script
    /// function which takes as its parameters the client-side node (TreeViewNode) which was edited, and its
    /// new label (string). The boolean value which the function should return determines whether the
    /// action will proceed (true) or be canceled (false).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Client-side command to execute when a node is renamed.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeRename
    {
      get 
      {
        string s = Properties["ClientSideOnNodeRename"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeRename"] = value;
      }
    }

    /// <summary>
    /// Client-side event handler to call when a node is selected. This is the identifier of a client script
    /// function which takes as its parameter the client-side node (TreeViewNode) which was selected. The function 
    /// should return a boolean value indicating whether the event should be canceled.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Client-side command to execute when a node is selected.")]
    [DefaultValue("")]
    [Category("Behavior")]
    public string ClientSideOnNodeSelect
    {
      get 
      {
        string s = Properties["ClientSideOnNodeSelect"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ClientSideOnNodeSelect"] = value;
      }
    }

    /// <summary>
    /// The duration (in milliseconds) of the collapse animation.
    /// </summary>
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
        return Utils.ParseInt(Properties["CollapseDuration"], 200);
      }
      set 
      {
        Properties["CollapseDuration"] = value.ToString();
      }
    }

    /// <summary>
    /// The slide type to use for the collapse animation.
    /// </summary>
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
        return Utils.ParseSlideType(Properties["CollapseSlide"]);
      }
      set 
      {
        Properties["CollapseSlide"] = value.ToString();
      }
    }

    /// <summary>
    /// The transition effect to use for the collapse animation.
    /// </summary>
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
        return Utils.ParseTransitionType(Properties["CollapseTransition"]);
      }
      set 
      {
        Properties["CollapseTransition"] = value.ToString();
      }
    }

    /// <summary>
    /// The string defining a custom transition filter to use for the collapse animation.
    /// </summary>
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
        string s = Properties["CollapseTransitionCustomFilter"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["CollapseTransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// The image (often a 'minus') indicating the collapsibility of a parent node.
    /// </summary>
    /// <seealso cref="ExpandImageUrl" />
    [Description("The image (often a 'minus') indicating the collapsibility of a parent node.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string CollapseImageUrl
    {
      get 
      {
        string s = Properties["CollapseImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["CollapseImageUrl"] = value;
      }
    }

    /// <summary>
    /// Whether to collapse a parent node when it is selected on the client. Default: true.
    /// </summary>
    /// <seealso cref="ExpandNodeOnSelect" />
    [Description("Whether to collapse a parent node when it is selected on the client. Default: true.")]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool CollapseNodeOnSelect
    {
      get 
      {
        string s = Properties["CollapseNodeOnSelect"]; 
        return Utils.ParseBool(s, true);
      }
      set 
      {
        Properties["CollapseNodeOnSelect"] = value.ToString();
      }
    }

    /// <summary>
    /// The image to show in place of the expand/collapse image to indicate
    /// that child nodes are loading (for load-on-demand nodes).
    /// </summary>
    /// <seealso cref="TreeViewNode.ContentCallbackUrl" />
    [Description("The image to show in place of the expand/collapse image to indicate that child nodes are loading (for load-on-demand nodes).")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ContentLoadingImageUrl
    {
      get 
      {
        string s = Properties["ContentLoadingImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ContentLoadingImageUrl"] = value;
      }
    }

    /// <summary>
    /// The CssClass to the frame of this TreeView.
    /// </summary>
    [Description("The CssClass to the frame of this TreeView.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public override string CssClass
    {
      get 
      {
        string s = Properties["CssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["CssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to use for nodes when they are cut (with Ctrl+X).
    /// </summary>
    /// <seealso cref="KeyboardCutCopyPasteEnabled" />
    [Description("The CssClass to use for nodes when they are cut (with Ctrl+X).")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string CutNodeCssClass
    {
      get 
      {
        string s = Properties["CutNodeCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["CutNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to use for rows of nodes when they are cut (with Ctrl+X).
    /// </summary>
    /// <seealso cref="KeyboardCutCopyPasteEnabled" />
    [Description("The CssClass to use for rows of nodes when they are cut (with Ctrl+X).")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string CutNodeRowCssClass
    {
      get 
      {
        string s = Properties["CutNodeRowCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["CutNodeRowCssClass"] = value;
      }
    }

    /// <summary>
    /// The default height to use for icon images, in pixels.
    /// </summary>
    [Description("The default height to use for icon images, in pixels.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int DefaultImageHeight
    {
      get 
      {
        return Utils.ParseInt(Properties["DefaultImageHeight"], 0); 
      }
      set 
      {
        Properties["DefaultImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// The default width to use for icon images, in pixels.
    /// </summary>
    [Description("The default width to use for icon images, in pixels.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int DefaultImageWidth
    {
      get 
      {
        return Utils.ParseInt(Properties["DefaultImageWidth"], 0); 
      }
      set 
      {
        Properties["DefaultImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// The default height to use for margin images, in pixels.
    /// </summary>
    /// <seealso cref="DisplayMargin" />
    /// <seealso cref="TreeViewNode.MarginImageUrl" />
    [Description("The default height to use for margin images, in pixels.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int DefaultMarginImageHeight
    {
      get 
      {
        return Utils.ParseInt(Properties["DefaultMarginImageHeight"], 0); 
      }
      set 
      {
        Properties["DefaultMarginImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// The default width to use for margin images, in pixels.
    /// </summary>
    /// <seealso cref="DisplayMargin" />
    /// <seealso cref="TreeViewNode.MarginImageUrl" />
    [Description("The default width to use for margin images, in pixels.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int DefaultMarginImageWidth
    {
      get 
      {
        return Utils.ParseInt(Properties["DefaultMarginImageWidth"], 0); 
      }
      set 
      {
        Properties["DefaultMarginImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to display a margin on the left-hand side of the TreeView. Default: false.
    /// </summary>
    [Description("Whether to display a margin on the left-hand side of the TreeView. Default: false.")]
    [Category("Layout")]
    [DefaultValue(false)]
    public bool DisplayMargin
    {
      get 
      {
        string s = Properties["DisplayMargin"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["DisplayMargin"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether dragging to and dropping from another TreeView is enabled by default. Default: false.
    /// </summary>
    [Description("Whether dragging to and dropping from another TreeView is enabled by default. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool DragAndDropAcrossTreesEnabled
    {
      get 
      {
        string s = Properties["DragAndDropAcrossTreesEnabled"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["DragAndDropAcrossTreesEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether dragging and dropping is enabled by default. Default: false.
    /// </summary>
    [Description("Whether dragging and dropping is enabled by default. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool DragAndDropEnabled
    {
      get 
      {
        string s = Properties["DragAndDropEnabled"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["DragAndDropEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// The delay (in milliseconds) after which to expand collapsed parent nodes when hovering over them
    /// while dragging.
    /// </summary>
    /// <seealso cref="DragAndDropEnabled" />
    [Description("The delay after which to expand collapsed parent nodes when hovering over them while dragging.")]
    [DefaultValue(700)]
    [Category("Behavior")]
    public int DragHoverExpandDelay
    {
      get 
      {
        return Utils.ParseInt(Properties["DragHoverExpandDelay"], 700); 
      }
      set 
      {
        Properties["DragHoverExpandDelay"] = value.ToString();
      }
    }

    /// <summary>
    /// The CssClass to use on items when about to drop into them (and create new children for them).
    /// </summary>
    /// <seealso cref="DropChildEnabled" />
    [Description("The image to show in place of the expand/collapse image to indicate that child nodes are loading (for load-on-demand nodes).")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string DropChildCssClass
    {
      get 
      {
        string s = Properties["DropChildCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["DropChildCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to allow dropping to create child nodes (drop into nodes). Default: true.
    /// </summary>
    /// <seealso cref="DragAndDropEnabled" />
    /// <seealso cref="DropSiblingEnabled" />
    [Description("Whether to allow dropping to create child nodes (drop into nodes). Default: true.")]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool DropChildEnabled
    {
      get 
      {
        string s = Properties["DropChildEnabled"]; 
        return Utils.ParseBool(s, true);
      }
      set 
      {
        Properties["DropChildEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to allow dropping to create new root nodes, via drag/drop. Default: true.
    /// </summary>
    [Description("Whether to allow dropping to create new root nodes, via drag/drop. Default: true.")]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool DropRootEnabled
    {
      get 
      {
        string s = Properties["DropRootEnabled"]; 
        return Utils.ParseBool(s, true);
      }
      set 
      {
        Properties["DropRootEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// The CssClass to use on the visual feedback element (a black line by default) when about to drop a sibling.
    /// </summary>
    /// <seealso cref="DropSiblingEnabled" />
    [Description("The CssClass to use on the visual feedback element when about to drop a sibling.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string DropSiblingCssClass
    {
      get 
      {
        string s = Properties["DropSiblingCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["DropSiblingCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to allow dropping to create siblings (drop between nodes). Default: false.
    /// </summary>
    /// <seealso cref="DragAndDropEnabled" />
    /// <seealso cref="DropChildEnabled" />
    [Description("Whether to allow dropping to create siblings (drop between nodes). Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool DropSiblingEnabled
    {
      get 
      {
        string s = Properties["DropSiblingEnabled"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["DropSiblingEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// The duration (in milliseconds) of the expand animation.
    /// </summary>
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
        return Utils.ParseInt(Properties["ExpandDuration"], 200); 
      }
      set 
      {
        Properties["ExpandDuration"] = value.ToString();
      }
    }

    /// <summary>
    /// The slide type to use for the expand animation.
    /// </summary>
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
        return Utils.ParseSlideType(Properties["ExpandSlide"]);
      }
      set
      {
        Properties["ExpandSlide"] = value.ToString();
      }
    }

    /// <summary>
    /// The transition effect to use for the expand animation.
    /// </summary>
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
        return Utils.ParseTransitionType(Properties["ExpandTransition"]);
      }
      set
      {
        Properties["ExpandTransition"] = value.ToString();
      }
    }

    /// <summary>
    /// The string defining a custom transition filter to use for the expand animation.
    /// </summary>
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
        string s = Properties["ExpandTransitionCustomFilter"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ExpandTransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// Default height to apply to expand/collapse images, in pixels.
    /// </summary>
    [Description("Default height to apply to expand/collapse images, in pixels.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int ExpandCollapseImageHeight
    {
      get 
      {
        return Utils.ParseInt(Properties["ExpandCollapseImageHeight"], 0); 
      }
      set 
      {
        Properties["ExpandCollapseImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// Default width to apply to expand/collapse images, in pixels.
    /// </summary>
    [Description("Default width to apply to expand/collapse images, in pixels.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int ExpandCollapseImageWidth
    {
      get 
      {
        return Utils.ParseInt(Properties["ExpandCollapseImageWidth"], 0); 
      }
      set 
      {
        Properties["ExpandCollapseImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to render expand/collapse images at the left edge of the TreeView (before the indentation)
    /// instead of just before the node. Default: false.
    /// </summary>
    [Description("Whether to render expand/collapse images at the left edge of the TreeView (before the indentation) instead of just before the node. Default: false.")]
    [Category("Layout")]
    [DefaultValue(false)]
    public bool ExpandCollapseInFront
    {
      get 
      {
        string s = Properties["ExpandCollapseInFront"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["ExpandCollapseInFront"] = value.ToString();
      }
    }

    /// <summary>
    /// Image (often a 'plus') to use to indicate expandability of parent nodes.
    /// </summary>
    /// <seealso cref="CollapseImageUrl" />
    [Description("Image (often a 'plus') to use to indicate expandability of parent nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ExpandImageUrl
    {
      get 
      {
        string s = Properties["ExpandImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ExpandImageUrl"] = value;
      }
    }

    /// <summary>
    /// Whether to expand a parent node when it is selected on the client. Default: true.
    /// </summary>
    [Description("Whether to expand a parent node when it is selected on the client. Default: true.")]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool ExpandNodeOnSelect
    {
      get 
      {
        string s = Properties["ExpandNodeOnSelect"]; 
        return Utils.ParseBool(s, true);
      }
      set 
      {
        Properties["ExpandNodeOnSelect"] = value.ToString();
      }
    }

    /// <summary>
    /// Default icon to use for expanded parent nodes.
    /// </summary>
    [Description("Default icon to use for expanded parent nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ExpandedParentNodeImageUrl
    {
      get 
      {
        string s = Properties["ExpandedParentNodeImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ExpandedParentNodeImageUrl"] = value;
      }
    }

    /// <summary>
    /// Whether to force expansion of the path leading down to the selected node. Default: true.
    /// </summary>
    [Description("Whether to force expansion of the path leading down to the selected node. Default: true.")]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool ExpandSelectedPath 
    {
      get 
      {
        string s = Properties["ExpandSelectedPath"]; 
        return Utils.ParseBool(s, true);
      }
      set 
      {
        Properties["ExpandSelectedPath"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to collapse all other paths when expanding a node, ensuring that only one path is expanded.
    /// Default: false.
    /// </summary>
    [Description("Whether to collapse all other paths when expanding a node, ensuring that only one path is expanded. Default: false.")]
    [Category("Appearance")]
    [DefaultValue(false)]
    public bool ExpandSinglePath
    {
      get 
      {
        string s = Properties["ExpandSinglePath"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["ExpandSinglePath"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to extend node cells to the right edge of the TreeView. Default: false.
    /// </summary>
    [Description("Whether to extend node cells to the right edge of the TreeView. Default: false.")]
    [Category("Layout")]
    [DefaultValue(false)]
    public bool ExtendNodeCells
    {
      get 
      {
        string s = Properties["ExtendNodeCells"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["ExtendNodeCells"] = value.ToString();
      }
    }
    
    /// <summary>
    /// A comma-delimited list of IDs of DOM elements on which dropping nodes from this TreeView is allowed.
    /// </summary>
    [Description("A comma-delimited list of IDs of DOM elements on which dropping nodes from this TreeView is allowed.")]
    [Category("Behavior")]
    [DefaultValue("")]
    public string ExternalDropTargets
    {
      get 
      {
        string s = Properties["ExternalDropTargets"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ExternalDropTargets"] = value;
      }
    }

    /// <summary>
    /// Whether to take on dimensions which fill the containing DOM element entirely. Default: false.
    /// </summary>
    [Description("Whether to take on dimensions which fill the containing DOM element entirely. Default: false.")]
    [Category("Layout")]
    [DefaultValue(false)]
    public bool FillContainer
    {
      get 
      {
        string s = Properties["FillContainer"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["FillContainer"] = value.ToString();
      }
    }

    /// <summary>
    /// The CSS class to apply to the TreeView frame when it has keyboard focus.
    /// </summary>
    [Description("The CSS class to apply to the TreeView frame when it has keyboard focus.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string FocusedCssClass
    {
      get 
      {
        string s = Properties["FocusedCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["FocusedCssClass"] = value;
      }
    }

    /// <summary>
    /// ID of node to forcefully highlight. This will make it appear as it would when selected.
    /// </summary>
    [Category("Appearance")]
    [Description("ID of node to forcefully highlight.")]
    [DefaultValue("")]
    public new string ForceHighlightedNodeID
    {
      get
      {
        return base.ForceHighlightedNodeID;
      }

      set
      {
        base.ForceHighlightedNodeID = value; 
      }
    }

    /// <summary>
    /// Default CSS class to use for nodes on hover.
    /// </summary>
    [Description("Default CSS class to use for nodes on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string HoverNodeCssClass
    {
      get 
      {
        string s = Properties["HoverNodeCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["HoverNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// Default CSS class to use for node rows on hover.
    /// </summary>
    [Description("Default CSS class to use for node rows on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string HoverNodeRowCssClass
    {
      get 
      {
        string s = Properties["HoverNodeRowCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["HoverNodeRowCssClass"] = value;
      }
    }

    /// <summary>
    /// Default CSS class to use for hover popups on obscured nodes.
    /// </summary>
    [Description("Default CSS class to use for hover popups on obscured nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string HoverPopupNodeCssClass
    {
      get 
      {
        string s = Properties["HoverPopupNodeCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["HoverPopupNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to show hover popups on obscured nodes.
    /// </summary>
    /// <remarks>This feature is only supported in Microsoft Internet Explorer.</remarks>
    [Description("Whether to show hover popups on obscured nodes.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool HoverPopupEnabled
    {
      get 
      {
        return Utils.ParseBool(Properties["HoverPopupEnabled"], false); 
      }
      set 
      {
        Properties["HoverPopupEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// Prefix to use for all image URL paths. For non-image URLs, use BaseUrl.
    /// </summary>
    [Category("Support")]
    [Description("Used as a prefix for all image URLs. ")]
    [DefaultValue("")]
    public override string ImagesBaseUrl
    {
      get
      {
        string s = Properties["ImagesBaseUrl"]; 
        return s == null? string.Empty : s;
      }
      set
      {
        Properties["ImagesBaseUrl"] = value;
      }
    }

    /// <summary>
    /// Spacing (in pixels) to render between node rows. Default: 0.
    /// </summary>
    [Description("Spacing (in pixels) to render between node rows. Default: 0.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int ItemSpacing
    {
      get 
      {
        return Utils.ParseInt(Properties["ItemSpacing"], 0); 
      }
      set 
      {
        Properties["ItemSpacing"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to enable keyboard control. Default: true.
    /// </summary>
    [Description("Whether to enable keyboard control. Default: true.")]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool KeyboardEnabled
    {
      get 
      {
        return Utils.ParseBool(Properties["KeyboardEnabled"], true); 
      }
      set 
      {
        Properties["KeyboardEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to enable keyboard cut/copy/pasting of nodes via Ctrl+X/C/V. Default: false.
    /// </summary>
    [Description("Whether to enable keyboard cut/copy/pasting of nodes via Ctrl+X/C/V. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool KeyboardCutCopyPasteEnabled
    {
      get 
      {
        return Utils.ParseBool(Properties["KeyboardCutCopyPasteEnabled"], false); 
      }
      set 
      {
        Properties["KeyboardCutCopyPasteEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// Default icon to use for leaf nodes.
    /// </summary>
    [Description("Default icon to use for leaf nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string LeafNodeImageUrl
    {
      get 
      {
        string s = Properties["LeafNodeImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["LeafNodeImageUrl"] = value;
      }
    }

    /// <summary>
    /// Width to apply on line images.
    /// </summary>
    /// <seealso cref="ShowLines" />
    [Description("Width to apply on line images. ")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int LineImageWidth
    {
      get 
      {
        return Utils.ParseInt(Properties["LineImageWidth"], 0); 
      }
      set 
      {
        Properties["LineImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// Height to apply on line images.
    /// </summary>
    /// <seealso cref="ShowLines" />
    [Description("Height to apply on line images.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int LineImageHeight
    {
      get 
      {
        return Utils.ParseInt(Properties["LineImageHeight"], 0); 
      }
      set 
      {
        Properties["LineImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// Folder to look for line images in.
    /// </summary>
    /// <seealso cref="ShowLines" />
    [Description("Folder to look for line images in.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string LineImagesFolderUrl
    {
      get 
      {
        string s = Properties["LineImagesFolderUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["LineImagesFolderUrl"] = value;
      }
    }

    /// <summary>
    /// CssClass to use for feedback while loading load-on-demand content.
    /// </summary>
    [Description("CssClass to use for feedback while loading load-on-demand content.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string LoadingFeedbackCssClass
    {
      get 
      {
        string s = Properties["LoadingFeedbackCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["LoadingFeedbackCssClass"] = value;
      }
    }

    /// <summary>
    /// Text to use for feedback while loading load-on-demand content.
    /// </summary>
    [Description("Text to use for feedback while loading load-on-demand content.")]
    [Category("Appearance")]
    [DefaultValue("Loading...")]
    public string LoadingFeedbackText
    {
      get 
      {
        string s = Properties["LoadingFeedbackText"]; 
        return s == null? "Loading..." : s;
      }
      set 
      {
        Properties["LoadingFeedbackText"] = value;
      }
    }

    /// <summary>
    /// CSS class to use for left-hand margin cells.
    /// </summary>
    /// <seealso cref="DisplayMargin" />
    [Description("CSS class to use for left-hand margin cells.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string MarginCssClass
    {
      get 
      {
        string s = Properties["MarginCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["MarginCssClass"] = value;
      }
    }

    /// <summary>
    /// Width of left-hand margin in pixels. Default: 32.
    /// </summary>
    /// <seealso cref="DisplayMargin" />
    [Description("Width of left-hand margin in pixels. Default: 32.")]
    [Category("Layout")]
    [DefaultValue(32)]
    public int MarginWidth
    {
      get 
      {
        return Utils.ParseInt(Properties["MarginWidth"], 32); 
      }
      set 
      {
        Properties["MarginWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// The default CSS class to use on a multiple-selected node.
    /// </summary>
    [Description("The default CSS class to use on a multiple-selected node.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string MultipleSelectedNodeCssClass
    {
      get 
      {
        string s = Properties["MultipleSelectedNodeCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["MultipleSelectedNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// The default CSS class to use on the row of a multiple-selected node.
    /// </summary>
    [Description("The default CSS class to use on the row of a multiple-selected node.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string MultipleSelectedNodeRowCssClass
    {
      get 
      {
        string s = Properties["MultipleSelectedNodeRowCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["MultipleSelectedNodeRowCssClass"] = value;
      }
    }

    /// <summary>
    /// The list of nodes which are currently selected.
    /// </summary>
    /// <remarks>This is a read-only property.</remarks>
    /// <seealso cref="TreeViewNode.IsMultipleSelected" />
    /// <seealso cref="TreeView.MultipleSelectEnabled" />
    [Description("The list of nodes which are currently selected.")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreeViewNode [] MultipleSelectedNodes
    {
      get
      {
        TreeViewNode [] arResults = GetMultipleSelectedNodes(this.Nodes);
        if(arResults.Length == 0 && this.SelectedNode != null)
        {
          return (new TreeViewNode [] {this.SelectedNode});
        }
        else
        {
          return arResults;
        }
      }
    }

    /// <summary>
    /// Whether to enable multiple node select (via Ctrl+Click). Default: true.
    /// </summary>
    [Description("Whether to enable multiple node select. Default: true.")]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool MultipleSelectEnabled
    {
      get 
      {
        string s = Properties["MultipleSelectEnabled"]; 
        return Utils.ParseBool(s, true);
      }
      set 
      {
        Properties["MultipleSelectEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// The ID of the client template to use on nodes.
    /// </summary>
    [Description("The ID of the client template to use on nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string NodeClientTemplateId
    {
      get 
      {
        string s = Properties["NodeClientTemplateId"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["NodeClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// Default CSS class to use on nodes.
    /// </summary>
    [Description("Default CSS class to use on nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string NodeCssClass
    {
      get 
      {
        string s = Properties["NodeCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["NodeCssClass"] = value;
      }
    }

    /// <summary>
    /// CSS class to use on the input field while editing a node.
    /// </summary>
    [Description("CSS class to use on the input field while editing a node.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string NodeEditCssClass
    {
      get 
      {
        string s = Properties["NodeEditCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["NodeEditCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to enable node editing (triggered by clicking twice on a node, as in Windows Explorer).
    /// Default: false.
    /// </summary>
    [Description("Whether to enable node editing (triggered by clicking twice on a node, as in Windows Explorer). Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool NodeEditingEnabled
    {
      get 
      {
        string s = Properties["NodeEditingEnabled"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["NodeEditingEnabled"] = value.ToString();
      }
    }

    /// <summary>
    /// Width (in pixels) to indent each level of the TreeView. Default: 16.
    /// </summary>
    [Description("Width (in pixels) to indent each level of the TreeView. Default: 16.")]
    [Category("Layout")]
    [DefaultValue(16)]
    public int NodeIndent
    {
      get 
      {
        return Utils.ParseInt(Properties["NodeIndent"], 16); 
      }
      set 
      {
        Properties["NodeIndent"] = value.ToString();
      }
    }

    /// <summary>
    /// Padding to include between a node's icon and its label (in pixels). Default: 0.
    /// </summary>
    [Description("Padding to include between a node's icon and its label (in pixels). Default: 0.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int NodeLabelPadding
    {
      get 
      {
        return Utils.ParseInt(Properties["NodeLabelPadding"], 0); 
      }
      set 
      {
        Properties["NodeLabelPadding"] = value.ToString();
      }
    }

    /// <summary>
    /// Default CSS class to use for node rows.
    /// </summary>
    [Description("Default CSS class to use for node rows.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string NodeRowCssClass
    {
      get 
      {
        string s = Properties["NodeRowCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["NodeRowCssClass"] = value;
      }
    }

    /// <summary>
    /// Collection of root TreeViewNodes.
    /// </summary>
    //[Editor(typeof(ComponentArt.Web.UI.TreeViewNodesEditor), typeof(ComponentEditor))]
    [Description("The collection of root TreeViewNodes.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Browsable(false)]
    public TreeViewNodeCollection Nodes
    {
      get
      {
        return (TreeViewNodeCollection)nodes;
      }
    }

    /// <summary>
    /// Image to use to indicate the absence of expandability. This is rendered for leaf nodes in the place of
    /// the expand/collapse images.
    /// </summary>
    [Description("Image to use to indicate the absence of expandability. This is rendered for leaf nodes in the place of the expand/collapse images.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string NoExpandImageUrl
    {
      get 
      {
        string s = Properties["NoExpandImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["NoExpandImageUrl"] = value;
      }
    }

    /// <summary>
    /// Identifier of the client script handler for the ContextMenu event (right-click on node).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Description("Identifier of the client script handler for the ContextMenu event (right-click on node).")]
    [Category("Behavior")]
    [DefaultValue("")]
    public string OnContextMenu
    {
      get 
      {
        string s = Properties["OnContextMenu"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["OnContextMenu"] = value;
      }
    }

    /// <summary>
    /// Default icon to use for parent nodes.
    /// </summary>
    [Description("Default icon to use for parent nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ParentNodeImageUrl
    {
      get 
      {
        string s = Properties["ParentNodeImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["ParentNodeImageUrl"] = value;
      }
    }

    /// <summary>
    /// Whether to load the entire structure, including all load-on-demand nodes, when searching for the
    /// current node.
    /// </summary>
    /// <remarks>
    /// Setting this to <b>true</b> will cause TreeView to load all data from node ContentCallbackUrls before looking for
    /// the <b>current</b> node based on the request URL. If found in load-on-demand substructures, only the current path will remain loaded -
    /// all other load-on-demand subtrees will be unloaded. Using this feature will likely affect server-side performance.
    /// </remarks>
    /// <seealso cref="TreeViewNode.ContentCallbackUrl" />
    [Description("Whether to load the entire structure, including all load-on-demand nodes, when searching for the current node.")]
    [DefaultValue(false)]
    public bool PreloadCurrentPath
    {
      get 
      {
        string s = Properties["PreloadCurrentPath"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["PreloadCurrentPath"] = value.ToString();
      }
    }

    /// <summary>
    /// ID of node to begin rendering down from.
    /// </summary>
    [Description("ID of node to begin rendering down from.")]
    [DefaultValue("")]
    [Category("Data")]
    public new string RenderRootNodeId
    {
      get 
      {
        return base.RenderRootNodeId;
      }
      set 
      {
        base.RenderRootNodeId = value;
      }
    }

    /// <summary>
    /// Whether to include the RenderRootNode when rendering, instead of only its children. Default: false.
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to include the RenderRootNode when rendering, instead of only its children. Default: false.")]
    [Category("Data")]
    public new bool RenderRootNodeInclude
    {
      get 
      {
        return base.RenderRootNodeInclude;
      }
      set 
      {
        base.RenderRootNodeInclude = value;
      }
    }

    /// <summary>
    /// Default icon to use for the selected node if it is an expanded parent.
    /// </summary>
    [Description("Default icon to use for the selected node if it is an expanded parent.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string SelectedExpandedParentNodeImageUrl
    {
      get 
      {
        string s = Properties["SelectedExpandedParentNodeImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["SelectedExpandedParentNodeImageUrl"] = value;
      }
    }

    /// <summary>
    /// Default CSS class to use for the selected node on hover.
    /// </summary>
    [Description("Default CSS class to use for the selected node on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string SelectedHoverNodeCssClass
    {
      get 
      {
        string s = Properties["SelectedHoverNodeCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["SelectedHoverNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// Default CSS class to use on the row of the selected node on hover.
    /// </summary>
    [Description("Default CSS class to use on the row of the selected node on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string SelectedHoverNodeRowCssClass
    {
      get 
      {
        string s = Properties["SelectedHoverNodeRowCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["SelectedHoverNodeRowCssClass"] = value;
      }
    }

    /// <summary>
    /// Default icon to use on the selected node if it is a leaf.
    /// </summary>
    [Description("Default icon to use on the selected node if it is a leaf.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string SelectedLeafNodeImageUrl
    {
      get 
      {
        string s = Properties["SelectedLeafNodeImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["SelectedLeafNodeImageUrl"] = value;
      }
    }

    /// <summary>
    /// The selected node. This can be set on the server-side to force a node selection.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Description("The currently selected TreeViewNode.")]
    public TreeViewNode SelectedNode
    {
      get
      {
        return (TreeViewNode)(base.selectedNode);
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
    /// The default CSS class to use for the selected node.
    /// </summary>
    [Description("The default CSS class to use for the selected node.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string SelectedNodeCssClass
    {
      get 
      {
        string s = Properties["SelectedNodeCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["SelectedNodeCssClass"] = value;
      }
    }
  
    /// <summary>
    /// The default CSS class to use on the row of the selected node.
    /// </summary>
    [Description("The default CSS class to use on the row of the selected node.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string SelectedNodeRowCssClass
    {
      get 
      {
        string s = Properties["SelectedNodeRowCssClass"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["SelectedNodeRowCssClass"] = value;
      }
    }

    /// <summary>
    /// Default icon to use for the selected node if it is a parent.
    /// </summary>
    [Description("Default icon to use for the selected node if it is a parent.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string SelectedParentNodeImageUrl
    {
      get 
      {
        string s = Properties["SelectedParentNodeImageUrl"]; 
        return s == null? string.Empty : s;
      }
      set 
      {
        Properties["SelectedParentNodeImageUrl"] = value;
      }
    }

    /// <summary>
    /// Whether to render a line structure for the tree. Default: false.
    /// If true, LineImagesFolderUrl must be specified.
    /// </summary>
    [Description("Whether to render a line structure for the tree. Default: false. If true, LineImagesFolderUrl must be specified.")]
    [Category("Appearance")]
    [DefaultValue(false)]
    public bool ShowLines
    {
      get 
      {
        string s = Properties["ShowLines"]; 
        return Utils.ParseBool(s, false);
      }
      set 
      {
        Properties["ShowLines"] = value.ToString();
      }
    }

    /// <summary>
    /// The name of the ASP.NET AJAX web service to use for initially populating the TreeView.
    /// </summary>
    /// <seealso cref="TreeViewNode.WebServiceMethod" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service to use for initially populating the TreeView.")]
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
    /// The name of the standard SOA.UI service to use in web service mode (to be used instead of WebService/WebServiceMethod).
    /// </summary>
    /// <seealso cref="RunningMode" />
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
    /// The (optional) custom parameter to send with each web service request.
    /// </summary>
    /// <seealso cref="RunningMode" />
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
    /// The name of the ASP.NET AJAX web service method to use for initially populating the TreeView.
    /// </summary>
    /// <seealso cref="WebService" />
    [DefaultValue("")]
    [Description("The name of the ASP.NET AJAX web service method to use for initially populating the TreeView.")]
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

    #region Public Methods

    /// <summary>
    /// Constructor
    /// </summary>
    public TreeView() : base()
    {
      // Set some defaults.
      this.ExpandSlide = SlideType.ExponentialDecelerate;
      this.ExpandDuration = 200;
      this.CollapseSlide = SlideType.ExponentialDecelerate;
      this.CollapseDuration = 200;

      nodes = new TreeViewNodeCollection(this, null);
    }

    /// <summary>
    /// Check all checkable nodes in the TreeView.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method checks the checkbox of all nodes in the <see cref="TreeView" /> hierarchy which have one.
    /// </para>
    /// <para>
    /// The <see cref="TreeViewNode">TreeViewNode's</see> <see cref="TreeViewNode.ShowCheckBox" /> property determines whether or not the node 
    /// has a checkbox, and the <see cref="TreeViewNode.Checked" /> property contains its status. 
    /// To check only the nodes which are descendants of a certain node, use the 
    /// <code>TreeViewNode</code> <see cref="TreeViewNode.CheckAll" /> method. 
    /// </para>
    /// <para>
    /// All nodes can be unchecked using the <see cref="TreeView.UnCheckAll" /> method.
    /// </para>
    /// </remarks>
    public void CheckAll()
    {
      if(nodes != null)
      {
        foreach(TreeViewNode oRoot in this.Nodes)
        {
          oRoot.CheckAll();
        }
      }
    }

    /// <summary>
    /// Collapse all expanded nodes in the TreeView.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method collapses all expanded nodes in the <see cref="TreeView" /> structure. The <see cref="TreeViewNode" /> 
    /// <see cref="TreeViewNode.Expanded" /> property contains a boolean value indicating whether the node is currently expanded.
    /// That property can also be set, allowing a single node to be expanded or collapsed programatically. There is also a 
    /// node-level <see cref="TreeViewNode.CollapseAll" /> method, which collapses only the descendants of a given node.
    /// </para>
    /// <para>
    /// All nodes in a <code>TreeView</code> can be expanded using the <see cref="TreeView.ExpandAll" /> method.
    /// </para>
    /// </remarks>
    public void CollapseAll()
    {
      if(nodes != null)
      {
        foreach(TreeViewNode oRoot in this.Nodes)
        {
          oRoot.CollapseAll();
        }
      }
    }

    /// <summary>
    /// Expand all expandable nodes in the TreeView.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method expands all nodes in a <see cref="TreeView" /> hierarchy which have children. The <see cref="TreeViewNode" /> 
    /// <see cref="TreeViewNode.Expanded" /> property contains a boolean value indicating whether the node is currently expanded.
    /// That property can also be set, allowing a single node to be expanded or collapsed programatically. There is also a node-level
    /// <see cref="TreeViewNode.ExpandAll" /> method, which expands only the descendants of a given node.
    /// </para>
    /// <para>
    /// All nodes in a <code>TreeView</code> can be collapsed using the <see cref="TreeView.CollapseAll" /> method.
    /// </para>
    /// </remarks>
    public void ExpandAll()
    {
      if(nodes != null)
      {
        foreach(TreeViewNode oRoot in this.Nodes)
        {
          oRoot.ExpandAll();
        }
      }
    }

    /// <summary>
    /// Unheck all checked nodes in the TreeView.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method unchecks all checked nodes in the <see cref="TreeView" /> hierarchy. 
    /// </para>
    /// <para>
    /// The <see cref="TreeViewNode">TreeViewNode's</see> <see cref="TreeViewNode.ShowCheckBox" /> property determines whether or not the node 
    /// has a checkbox, and the <see cref="TreeViewNode.Checked" /> property contains its status. 
    /// To un-check only the nodes which are descendants of a given node, use the 
    /// <code>TreeViewNode</code> <see cref="TreeViewNode.UnCheckAll" /> method. 
    /// </para>
    /// <para>
    /// All nodes which have checkboxes can be checked using the <see cref="TreeView.CheckAll" /> method.
    /// </para>
    /// </remarks>
    public void UnCheckAll()
    {
      if(nodes != null)
      {
        foreach(TreeViewNode oRoot in this.Nodes)
        {
          oRoot.UnCheckAll();
        }
      }
    }

    /// <summary>
    /// Find the TreeViewNode with the given ID.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a string ID, returning the <see cref="TreeViewNode" /> with that ID. The entire
    /// <see cref="TreeView" /> hierarchy is searched, enabling the retrieval of specific nodes without iterating
    /// through the various collections. 
    /// </para>
    /// </remarks>
    /// <param name="sNodeID">The ID to search for.</param>
    /// <returns>The found node or null.</returns>
    public new TreeViewNode FindNodeById(string sNodeID)
    {
      return (TreeViewNode)base.FindNodeById(sNodeID);
    }

    #endregion

		#region Protected Methods		

    /// <summary>
    /// Creates a new TreeViewNode and adds it as a root.
    /// </summary>
    /// <returns>The new node.</returns>
    protected override NavigationNode AddNode()
    {
      TreeViewNode oNewNode = new TreeViewNode();
      this.Nodes.Add(oNewNode);
      return oNewNode;
    }

    protected override NavigationNode NewNode()
    {
      TreeViewNode newNode = new TreeViewNode();
      TreeViewNodeCollection dummy = newNode.Nodes; // This is a dummy call to ensure that newNode.nodes is not null
      return newNode;
    }
    
    protected override void AddParsedSubObject(Object obj) 
    {
      if(obj is TreeViewNode) 
      {
        Controls.Add((Control)obj);
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

      TreeViewNodeCollection arGroup = this.Nodes;
      TreeViewNode oNode;

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

      oNode = (TreeViewNode)(this.FindNodeByPostBackId(sPostBackID, this.Nodes));

      if(oNode == null)
      {
        throw new Exception("Node " + sPostBackID + " not found in TreeView '" + this.ID + "'.");
      }

      // should we validate the page?
      if (Utils.ConvertInheritBoolToBool(oNode.CausesValidation, this.CausesValidation))
      {
        Page.Validate();
      }

      if(sCommand == "SELECT")
      {
        TreeViewNodeEventArgs oArgs = new TreeViewNodeEventArgs();
        oArgs.Command = sCommand;
        oArgs.Node = oNode;

        this.selectedNode = oNode;

        this.OnNodeSelected(oArgs);

        // Do we need to do server-side expand/collapse?
        if(this.CollapseNodeOnSelect && oNode.Expanded)
        {
          oNode.Expanded = false;
          this.OnNodeCollapsed(oArgs);
        }
        else if(this.ExpandNodeOnSelect && !oNode.Expanded && oNode.nodes != null && oNode.Nodes.Count > 0)
        {
          oNode.Expanded = true;
          this.OnNodeExpanded(oArgs);
        }

        // If the selected node has a navurl, redirect to it.
        if(oArgs.Node.NavigateUrl != string.Empty)
        {
          oArgs.Node.Navigate();
        }
      }
      else if(sCommand == "MOVE")
      {
        // Move postback args:
        // MOVE <moving node id> <new target treeview id> <new parent node id> <index>
        
        string sTreeViewId = arArguments[2];
        string sTargetNodeId = arArguments[3];
        int iIndex = int.Parse(arArguments[4]);

        TreeViewNodeMovedEventArgs oArgs = new TreeViewNodeMovedEventArgs();
        oArgs.Command = sCommand;
        oArgs.Node = oNode;
        oArgs.OldParent = oNode.ParentNode;
        oArgs.OldIndex = oNode.ParentNode != null ? oNode.ParentNode.Nodes.IndexOf(oNode) : this.Nodes.IndexOf(oNode);
        oArgs.OldTreeView = oNode.ParentTreeView;

        // unlink from old parent
        if(oNode.ParentNode == null)
        {
          // this node was a root
          this.Nodes.Remove(oNode);
        }
        else
        {
          oNode.ParentNode.Nodes.Remove(oNode);
        }
        
        TreeView oTreeView;
        if(sTreeViewId == this.UniqueID)
        {
          oTreeView = this;
        }
        else
        {
          oTreeView = (TreeView)Utils.FindControl(this, sTreeViewId);
        }
    
        if(sTargetNodeId != string.Empty)
        {
          TreeViewNode oNewParent = (TreeViewNode)(oTreeView.FindNodeByPostBackId(sTargetNodeId, oTreeView.Nodes));
          if(oNewParent == null)
          {
            throw new Exception("Destination node not found!");
          }
          else
          {
            if(iIndex < oNewParent.Nodes.Count)
            {
              oNewParent.Nodes.Insert(iIndex, oNode);
            }
            else
            {
              oNewParent.Nodes.Add(oNode);
            }

            oNewParent.Expanded = true;
          }
        }
        else
        {
          // no target node - meaning new root
          if(iIndex < oTreeView.Nodes.Count)
          {
            oTreeView.Nodes.Insert(iIndex, oNode);
          }
          else
          {
            oTreeView.Nodes.Add(oNode);
          }
        }

        this.OnNodeMoved(oArgs);
      }
      else if(sCommand == "COPY")
      {
        // Copy postback args:
        // COPY <target node id> <nodes to copy ids>
        string [] sNodeIds = arArguments[2].Split(',');
        
        foreach(string sNodeId in sNodeIds)
        {
          TreeViewNode oNodeToCopy = (TreeViewNode) this.FindNodeByPostBackId(sNodeId, this.Nodes);
          TreeViewNode oNodeCopy = new TreeViewNode();
          foreach(string sKey in oNodeToCopy.Properties.Keys)
          {
            oNodeCopy.Properties[sKey] = oNodeToCopy.Properties[sKey];
          }
          if(oNodeToCopy.ID != string.Empty)
          {
            oNodeCopy.ID = oNodeToCopy.ID + "_copy";
          }

          oNode.Nodes.Add(oNodeCopy);

          TreeViewNodeCopiedEventArgs oArgs = new TreeViewNodeCopiedEventArgs();
          oArgs.Command = sCommand;
          oArgs.Node = oNodeCopy;
          oArgs.CopiedFrom = oNodeToCopy;

          this.OnNodeCopied(oArgs);
        }

        oNode.Expanded = true;
      }
      else if(sCommand == "LABEL")
      {
        TreeViewNodeRenamedEventArgs oArgs = new TreeViewNodeRenamedEventArgs();
        oArgs.Command = sCommand;
        oArgs.Node = oNode;
        oArgs.OldText = oNode.Text;
        oArgs.Node.Text = Encoding.UTF8.GetString(HttpUtility.UrlDecodeToBytes(arArguments[2], Encoding.UTF8));
        this.OnNodeRenamed(oArgs);
      }
      else if(sCommand == "CHECK")
      {
        TreeViewNodeEventArgs oArgs = new TreeViewNodeEventArgs();
        oArgs.Command = sCommand;
        oArgs.Node = oNode;
        oArgs.Node.Checked = bool.Parse(arArguments[2]);
        this.OnNodeCheckChanged(oArgs);
      }
      else if(sCommand == "EXPAND")
      {
        TreeViewNodeEventArgs oArgs = new TreeViewNodeEventArgs();
        oArgs.Command = sCommand;
        oArgs.Node = oNode;

        oNode.Expanded = true;
        
        this.OnNodeExpanded(oArgs);
      }
      else if(sCommand == "COLLAPSE")
      {
        TreeViewNodeEventArgs oArgs = new TreeViewNodeEventArgs();
        oArgs.Command = sCommand;
        oArgs.Node = oNode;

        oNode.Expanded = false;
        
        this.OnNodeCollapsed(oArgs);
      }
      else
      {
        throw new Exception("Unknown postback command.");
      }
    }

    /// <summary>
    /// IsDownLevel method.
    /// </summary>
    /// <returns>Whether the browser using the control is classified as down-level.</returns>
    protected override bool IsDownLevel()
    {
      if(this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }

      if (Context == null)
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

    protected override void LoadViewState(object savedState)
    {
      base.LoadViewState(savedState);

      // Load properties
      string sViewStateProperties = Context.Request.Form[ClientObjectId + "_Properties"];
      if(sViewStateProperties != null)
      {
        this.LoadClientProperties(sViewStateProperties);
      }

      // Load client storage data
      string sViewStateData = Context.Request.Form[ClientObjectId + "_Data"];
      if(sViewStateData != null)
      {
        this.LoadClientData(sViewStateData);
      }

      // Maintain selected node if we're doing that.
      if(this.selectedNodePostbackId != null && this.nodes != null)
      {
        this.SelectedNode = (TreeViewNode)this.FindNodeByPostBackId(selectedNodePostbackId, this.nodes);
      }
    }

    /// <summary>
    /// React to being initialized.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      string sTreeVarName = this.GetSaneId();

      if(Context != null && Context.Request != null && Page != null)
      {
        this.selectedNodePostbackId = Context.Request.Form[sTreeVarName + "_SelectedNode"];
      }
    }

    /// <summary>
    /// React to being loaded.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      // do we have to preload the current path?
      if (this.PreloadCurrentPath && this.selectedNode == null)
      {
        this.LoadAllOnDemandNodes(this.Nodes);
        this.UpdateSelectedNode();
        this.UnLoadNonCurrentOnDemandNodes(this.Nodes);
      }

      ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
      if (oScriptManager != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573P291.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573Z388.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.TreeView.client_scripts.A573S388.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.TreeView.client_scripts.A573O788.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.TreeView.client_scripts.A573R288.js");
      }
    }

    protected override object SaveViewState()
    {
      if(EnableViewState)
      {
        ViewState["EnableViewState"] = true;
      }

      return base.SaveViewState();     
    }

    /// <summary>
    /// Render this control.
    /// </summary>
    /// <param name="output">The HtmlTextWriter to render to.</param>
    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      if (this.AutoTheming)
      {
        this.ApplyTheming(false);
      }

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
        if(!Page.IsClientScriptBlockRegistered("A573S388.js"))
        {
          Page.RegisterClientScriptBlock("A573S388.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.TreeView.client_scripts", "A573S388.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573O788.js"))
        {
          Page.RegisterClientScriptBlock("A573O788.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.TreeView.client_scripts", "A573O788.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573R288.js"))
        {
          Page.RegisterClientScriptBlock("A573R288.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.TreeView.client_scripts", "A573R288.js");
        }
        }
      }

      string sTreeVarName = this.GetSaneId();
			
      // do we need default styles?
      if(this.ConsiderDefaultStyles())
      {
        string sDefaultStyles = "<style>" + GetResourceContent("ComponentArt.Web.UI.TreeView.defaultStyle.css") + "</style>";
        output.Write(sDefaultStyles);
      }

      // output storage
      if (!this.IsDownLevel())
      {
        // Add data
        WriteStartupScript(output, this.DemarcateClientScript("window.ComponentArt_Storage_" + sTreeVarName + " = " + this.BuildStorage() + ";", "ComponentArt.Web.UI.TreeView " + this.VersionString() + " " + sTreeVarName));
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
        RenderDownLevelContent(output);
      }
      
      if (this.ClientTarget != ClientTargetLevel.Accessible && !this.IsDownLevel())
      {
        this.BuildImageList();
      
        // Preload images, if any
        if(this.PreloadImages.Count > 0)
        {
          this.RenderPreloadImages(output);
        }
      }

      if (this.ClientTarget != ClientTargetLevel.Accessible && !this.IsDownLevel())
      {
        // Render templates, if any
        foreach(Control oTemplate in this.Controls)
        {
          output.Write("<div id=\"" + oTemplate.ID + "\" style=\"display:none;\">");
          oTemplate.RenderControl(output);
          output.Write("</div>");
        }

        // Output div
        output.Write("<div");
        output.WriteAttribute("id", sTreeVarName);

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
        output.Write("\"");

        output.WriteAttribute("onclick", "if(window." + sTreeVarName + "_loaded) ComponentArt_SetKeyboardFocusedTree(this, " + sTreeVarName + ");"); 
        output.WriteAttribute("onmouseover", "if(window." + sTreeVarName + "_loaded) ComponentArt_SetActiveTree(" + sTreeVarName + ");"); 
        output.Write("></div>");

        // Render client-side-selection-propagating field.
        output.AddAttribute("id", sTreeVarName + "_SelectedText");
        output.AddAttribute("name", sTreeVarName + "_SelectedText");
        output.AddAttribute("type", "hidden");
        output.AddAttribute("value", (this.SelectedNode == null ? "" : this.SelectedNode.PostBackID));
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();

        // Render client-side-selection-propagating field.
        output.AddAttribute("id", sTreeVarName + "_SelectedValue");
        output.AddAttribute("name", sTreeVarName + "_SelectedValue");
        output.AddAttribute("type", "hidden");
        output.AddAttribute("value", (this.SelectedNode == null ? "" : this.SelectedNode.PostBackID));
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();
        if(this.EnableViewState)
        {
          // Render client-storage-persisting field.
          output.AddAttribute("id", sTreeVarName + "_Data");
          output.AddAttribute("name", sTreeVarName + "_Data");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render toplevel-property-persisting field.
          output.AddAttribute("id", sTreeVarName + "_Properties");
          output.AddAttribute("name", sTreeVarName + "_Properties");
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Render client-side-selection-propagating field.
          output.AddAttribute("id", sTreeVarName + "_SelectedNode");
          output.AddAttribute("name", sTreeVarName + "_SelectedNode");
          output.AddAttribute("type", "hidden");
          output.AddAttribute("value", (this.SelectedNode == null? "" : this.SelectedNode.PostBackID));
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

        }

        // Render tab-focus-getting form element.
        if(this.KeyboardEnabled)
        {
          output.AddAttribute("href", "#");
          output.AddAttribute("onfocus", "ComponentArt_SetKeyboardFocusedTree(document.getElementById('" + sTreeVarName + "'), " + sTreeVarName + ");");
          output.AddStyleAttribute("position", "absolute");
          output.AddStyleAttribute("z-index", "99");
          output.RenderBeginTag(HtmlTextWriterTag.A);
          output.RenderEndTag();
        }
      }

      // output startup script
      if(this.ClientTarget != ClientTargetLevel.Accessible && !this.IsDownLevel())
      {
        // Add treeview initialization
        StringBuilder oStartupSB = new StringBuilder();
        oStartupSB.Append("window.ComponentArt_Init_" + sTreeVarName + " = function() {\n");

        // Include check for whether everything we need is loaded,
        // and a retry after a delay in case it isn't.
        oStartupSB.Append("if(!window.ComponentArt_TreeView_Kernel_Loaded || !window.ComponentArt_TreeView_Keyboard_Loaded || !window.ComponentArt_TreeView_Support_Loaded || !window.ComponentArt_Utils_Loaded || !window.ComponentArt_Keyboard_Loaded || !window.ComponentArt_DragDrop_Loaded || !document.getElementById('" + sTreeVarName + "'))\n");
        oStartupSB.Append("\t{setTimeout('ComponentArt_Init_" + sTreeVarName + "()', 100); return; }\n\n");

        // Instantiate object
        oStartupSB.Append("window." + sTreeVarName + " = new ComponentArt_TreeView('" + sTreeVarName + "', ComponentArt_Storage_" + sTreeVarName + ");\n");

        // Write postback function reference
        if (Page != null)
        {
          oStartupSB.Append(sTreeVarName + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
        }

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != sTreeVarName)
        {
          oStartupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sTreeVarName + "; " + sTreeVarName + ".GlobalAlias = '" + ID + "'; }\n");
        }

        // Define client templates
        oStartupSB.Append(sTreeVarName + ".ClientTemplates = " + this._clientTemplates.ToString() + ";\n");

        // Define other properties (these persist back!)
        oStartupSB.Append(sTreeVarName + ".Properties = [");
        oStartupSB.Append("['ApplicationPath'," + Utils.ConvertStringToJSString(Context.Request.ApplicationPath) + "],");
        if (AllowTextSelection) oStartupSB.Append("['AllowTextSelection',1],");
        if (AutoAssignNodeIDs) oStartupSB.Append("['AutoAssignNodeIDs',1],");
        if (AutoPostBackOnSelect) oStartupSB.Append("['AutoPostBackOnSelect',1],");
        if(AutoPostBackOnNodeExpand) oStartupSB.Append("['AutoPostBackOnExpand',1],");
        if(AutoPostBackOnNodeCollapse) oStartupSB.Append("['AutoPostBackOnCollapse',1],");
        if(AutoPostBackOnNodeCheckChanged) oStartupSB.Append("['AutoPostBackOnCheckChanged',1],");
        if(AutoPostBackOnNodeMove) oStartupSB.Append("['AutoPostBackOnMove',1],");
        if(AutoPostBackOnNodeRename) oStartupSB.Append("['AutoPostBackOnRename',1],");
        if (AutoScroll) oStartupSB.Append("['AutoScroll',1],");
        if (AutoTheming)
        {
          oStartupSB.Append("['AutoTheming',1],");
          oStartupSB.Append("['AutoThemingCssClassPrefix','" + AutoThemingCssClassPrefix + "'],");
        }
        if(ChildSelectedHoverNodeCssClass != string.Empty) oStartupSB.Append("['ChildSelectedHoverNodeCssClass','" + ChildSelectedHoverNodeCssClass + "'],");
        if(ChildSelectedHoverNodeRowCssClass != string.Empty) oStartupSB.Append("['ChildSelectedHoverNodeRowCssClass','" + ChildSelectedHoverNodeRowCssClass + "'],");
        if(ChildSelectedNodeCssClass != string.Empty) oStartupSB.Append("['ChildSelectedNodeCssClass','" + ChildSelectedNodeCssClass + "'],");
        if(ChildSelectedNodeRowCssClass != string.Empty) oStartupSB.Append("['ChildSelectedNodeRowCssClass','" + ChildSelectedNodeRowCssClass + "'],");
        oStartupSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "],");
        if(ClientSideOnCallbackComplete != string.Empty) oStartupSB.Append("['ClientSideOnCallbackComplete','" + MakeStringJScriptSafe(ClientSideOnCallbackComplete) + "'],");
        if(ClientSideOnLoad != string.Empty) oStartupSB.Append("['ClientSideOnLoad','" + MakeStringJScriptSafe(ClientSideOnLoad) + "'],");
        if(ClientSideOnNodeCheckChanged != string.Empty) oStartupSB.Append("['ClientSideOnNodeCheckChanged','" + MakeStringJScriptSafe(ClientSideOnNodeCheckChanged) + "'],");
        if(ClientSideOnNodeCollapse != string.Empty) oStartupSB.Append("['ClientSideOnNodeCollapse','" + MakeStringJScriptSafe(ClientSideOnNodeCollapse) + "'],");
        if(ClientSideOnNodeCopy != string.Empty) oStartupSB.Append("['ClientSideOnNodeCopy','" + MakeStringJScriptSafe(ClientSideOnNodeCopy) + "'],");
        if(ClientSideOnNodeExpand != string.Empty) oStartupSB.Append("['ClientSideOnNodeExpand','" + MakeStringJScriptSafe(ClientSideOnNodeExpand) + "'],");
        if(ClientSideOnNodeExternalDrop != string.Empty) oStartupSB.Append("['ClientSideOnNodeExternalDrop','" + MakeStringJScriptSafe(ClientSideOnNodeExternalDrop) + "'],");
        if(ClientSideOnNodeMouseDoubleClick != string.Empty) oStartupSB.Append("['ClientSideOnNodeMouseDoubleClick','" + MakeStringJScriptSafe(ClientSideOnNodeMouseDoubleClick) + "'],");
        if(ClientSideOnNodeMouseOut != string.Empty) oStartupSB.Append("['ClientSideOnNodeMouseOut','" + MakeStringJScriptSafe(ClientSideOnNodeMouseOut) + "'],");
        if(ClientSideOnNodeMouseOver != string.Empty) oStartupSB.Append("['ClientSideOnNodeMouseOver','" + MakeStringJScriptSafe(ClientSideOnNodeMouseOver) + "'],");
        if(ClientSideOnNodeMove != string.Empty) oStartupSB.Append("['ClientSideOnNodeMove','" + MakeStringJScriptSafe(ClientSideOnNodeMove) + "'],");
        if(ClientSideOnNodeRename != string.Empty) oStartupSB.Append("['ClientSideOnNodeRename','" + MakeStringJScriptSafe(ClientSideOnNodeRename) + "'],");
        if(ClientSideOnNodeSelect != string.Empty) oStartupSB.Append("['ClientSideOnNodeSelect','" + MakeStringJScriptSafe(ClientSideOnNodeSelect) + "'],");
        oStartupSB.Append("['CollapseSlide'," + ((int)this.CollapseSlide).ToString() + "],");
        oStartupSB.Append("['CollapseDuration'," + this.CollapseDuration.ToString() + "],");
        oStartupSB.Append("['CollapseTransition'," + ((int)this.CollapseTransition).ToString() + "],");
        if(CollapseTransitionCustomFilter != string.Empty) oStartupSB.Append("['CollapseTransitionCustomFilter','" + this.CollapseTransitionCustomFilter + "'],");
        if(CollapseImageUrl != string.Empty) oStartupSB.Append("['CollapseImageUrl','" + Utils.ConvertUrl(Context, string.Empty, this.CollapseImageUrl) + "'],");
        if(CollapseNodeOnSelect) oStartupSB.Append("['CollapseNodeOnSelect',true],");
        if(ContentLoadingImageUrl != string.Empty) oStartupSB.Append("['ContentLoadingImageUrl','" + Utils.ConvertUrl(Context, string.Empty, this.ContentLoadingImageUrl) + "'],");
        oStartupSB.Append("['ControlId','" + this.UniqueID + "'],");
        if(CssClass != string.Empty) oStartupSB.Append("['CssClass','" + this.CssClass + "'],");
        if(CutNodeCssClass != string.Empty) oStartupSB.Append("['CutNodeCssClass','" + this.CutNodeCssClass + "'],");
        if(CutNodeRowCssClass != string.Empty) oStartupSB.Append("['CutNodeRowCssClass','" + this.CutNodeRowCssClass + "'],");
        oStartupSB.Append("['DefaultImageHeight'," + this.DefaultImageHeight + "],");
        oStartupSB.Append("['DefaultImageWidth'," + this.DefaultImageWidth + "],");
        if(DefaultTarget != string.Empty) oStartupSB.Append("['DefaultTarget','" + this.DefaultTarget + "'],");
        oStartupSB.Append("['MarginImageHeight'," + this.DefaultMarginImageHeight + "],");
        oStartupSB.Append("['MarginImageWidth'," + this.DefaultMarginImageWidth + "],");
        if(DisplayMargin) oStartupSB.Append("['DisplayMargin',true],");
        if(DragAndDropAcrossTreesEnabled) oStartupSB.Append("['DragAndDropAcrossTreesEnabled',true],");
        if(DragAndDropEnabled) oStartupSB.Append("['DragAndDropEnabled',true],");
        oStartupSB.Append("['DragHoverExpandDelay'," + this.DragHoverExpandDelay.ToString() + "],");
        if(DropChildCssClass != string.Empty) oStartupSB.Append("['DropChildCssClass','" + this.DropChildCssClass + "'],");
        if(DropChildEnabled) oStartupSB.Append("['DropChildEnabled',true],");
        if(DropRootEnabled) oStartupSB.Append("['DropRootEnabled',true],");
        if(DropSiblingCssClass != string.Empty) oStartupSB.Append("['DropSiblingCssClass','" + this.DropSiblingCssClass + "'],");
        if(DropSiblingEnabled) oStartupSB.Append("['DropSiblingEnabled',true],");
        if(Enabled) oStartupSB.Append("['Enabled',true],");
        if(EnableViewState) oStartupSB.Append("['EnableViewState',true],");
        oStartupSB.Append("['ExpandSlide'," + ((int)this.ExpandSlide).ToString() + "],");
        oStartupSB.Append("['ExpandDuration'," + this.ExpandDuration.ToString() + "],");
        oStartupSB.Append("['ExpandTransition'," + ((int)this.ExpandTransition).ToString() + "],");
        if(ExpandTransitionCustomFilter != string.Empty) oStartupSB.Append("['ExpandTransitionCustomFilter','" + this.ExpandTransitionCustomFilter + "'],");
        oStartupSB.Append("['ExpandCollapseImageHeight'," + this.ExpandCollapseImageHeight + "],");
        oStartupSB.Append("['ExpandCollapseImageWidth'," + this.ExpandCollapseImageWidth + "],");
        if(ExpandCollapseInFront) oStartupSB.Append("['ExpandCollapseInFront',true],");
        if(ExpandedParentNodeImageUrl != string.Empty) oStartupSB.Append("['ExpandedParentNodeImageUrl','" + Utils.ConvertUrl(Context, string.Empty, this.ExpandedParentNodeImageUrl) + "'],");
        if(ExpandImageUrl != string.Empty) oStartupSB.Append("['ExpandImageUrl','" + Utils.ConvertUrl(Context, string.Empty, this.ExpandImageUrl) + "'],");
        if(ExpandSelectedPath) oStartupSB.Append("['ExpandSelectedPath',true],");
        if(ExpandSinglePath) oStartupSB.Append("['ExpandSinglePath',true],");
        if(ExpandNodeOnSelect) oStartupSB.Append("['ExpandNodeOnSelect',true],");
        if(ExtendNodeCells) oStartupSB.Append("['ExtendNodeCells',true],");
        if(ExternalDropTargets != string.Empty) oStartupSB.Append("['ExternalDropTargets','" + this.ExternalDropTargets + "'],");
        if(FillContainer) oStartupSB.Append("['FillContainer',true],");
        if(FocusedCssClass != string.Empty) oStartupSB.Append("['FocusedCssClass','" + this.FocusedCssClass + "'],");
        if(ForceHighlightedNodeID != string.Empty) oStartupSB.Append("['ForceHighlightedNodeID','" + this.ForceHighlightedNodeID + "'],");
        if(HoverNodeCssClass != string.Empty) oStartupSB.Append("['HoverNodeCssClass','" + this.HoverNodeCssClass + "'],");
        if(HoverNodeRowCssClass != string.Empty) oStartupSB.Append("['HoverNodeRowCssClass','" + this.HoverNodeRowCssClass + "'],");
        if(HoverPopupNodeCssClass != string.Empty) oStartupSB.Append("['HoverPopupNodeCssClass','" + this.HoverPopupNodeCssClass + "'],");
        if(HoverPopupEnabled) oStartupSB.Append("['HoverPopupEnabled',true],");
        if(ImagesBaseUrl != string.Empty) oStartupSB.Append("['ImagesBaseUrl','" + Utils.ConvertUrl(Context, string.Empty, this.ImagesBaseUrl) + (this.ImagesBaseUrl.EndsWith("/")? "" : "/") + "'],");
        oStartupSB.Append("['ItemSpacing'," + this.ItemSpacing + "],");
        if(KeyboardCutCopyPasteEnabled) oStartupSB.Append("['KeyboardCutCopyPasteEnabled',true],");
        if(KeyboardEnabled) oStartupSB.Append("['KeyboardEnabled',true],");
        if(LeafNodeImageUrl != string.Empty) oStartupSB.Append("['LeafNodeImageUrl','" + Utils.ConvertUrl(Context, string.Empty, this.LeafNodeImageUrl) + "'],");
        oStartupSB.Append("['LineImageHeight'," + this.LineImageHeight + "],");
        oStartupSB.Append("['LineImageWidth'," + this.LineImageWidth + "],");
        if(LineImagesFolderUrl != string.Empty) oStartupSB.Append("['LineImagesFolderUrl','" + Utils.ConvertUrl(Context, string.Empty, this.LineImagesFolderUrl) + (this.LineImagesFolderUrl.EndsWith("/")? "" : "/") + "'],");
        if(LoadingFeedbackCssClass != string.Empty) oStartupSB.Append("['LoadingFeedbackCssClass','" + LoadingFeedbackCssClass + "'],");
        if(LoadingFeedbackText != string.Empty) oStartupSB.Append("['LoadingFeedbackText','" + LoadingFeedbackText + "'],");
        if(MarginCssClass != string.Empty) oStartupSB.Append("['MarginCssClass','" + this.MarginCssClass + "'],");
        oStartupSB.Append("['MarginWidth'," + this.MarginWidth + "],");
        if(MultiPageId != string.Empty) oStartupSB.Append("['MultiPageId','" + this._multiPageId + "'],");
        if(MultipleSelectedNodeCssClass != string.Empty) oStartupSB.Append("['MultipleSelectedNodeCssClass','" + this.MultipleSelectedNodeCssClass + "'],");
        if(MultipleSelectedNodeRowCssClass != string.Empty) oStartupSB.Append("['MultipleSelectedNodeRowCssClass','" + this.MultipleSelectedNodeRowCssClass + "'],");
        if(MultipleSelectEnabled) oStartupSB.Append("['MultipleSelectEnabled',true],");
        if(NodeClientTemplateId != string.Empty) oStartupSB.Append("['NodeClientTemplateId','" + this.NodeClientTemplateId + "'],");
        if(NodeCssClass != string.Empty) oStartupSB.Append("['NodeCssClass','" + this.NodeCssClass + "'],");
        oStartupSB.Append("['NodeLabelPadding'," + this.NodeLabelPadding + "],");
        if(NodeEditCssClass != string.Empty) oStartupSB.Append("['NodeEditCssClass','" + this.NodeEditCssClass + "'],");
        if(NodeEditingEnabled) oStartupSB.Append("['NodeEditingEnabled',true],");
        oStartupSB.Append("['NodeIndent'," + this.NodeIndent.ToString() + "],");
        if(NodeRowCssClass != string.Empty) oStartupSB.Append("['NodeRowCssClass','" + this.NodeRowCssClass + "'],");
        if(NoExpandImageUrl != string.Empty) oStartupSB.Append("['NoExpandImageUrl','" + Utils.ConvertUrl(Context, string.Empty, this.NoExpandImageUrl) + "'],");
        if(OnContextMenu != string.Empty) oStartupSB.Append("['OnContextMenu','" + MakeStringJScriptSafe(this.OnContextMenu) + "'],");
        if(ParentNodeImageUrl != string.Empty) oStartupSB.Append("['ParentNodeImageUrl','" + Utils.ConvertUrl(Context, string.Empty, this.ParentNodeImageUrl) + "'],");
        if(PreRenderAllLevels) oStartupSB.Append("['PreRenderAllLevels',true],");
        if(ShowLines) oStartupSB.Append("['ShowLines',true],");
        if(SelectedHoverNodeCssClass != string.Empty) oStartupSB.Append("['SelectedHoverNodeCssClass','" + this.SelectedHoverNodeCssClass + "'],");
        if(SelectedHoverNodeRowCssClass != string.Empty) oStartupSB.Append("['SelectedHoverNodeRowCssClass','" + this.SelectedHoverNodeRowCssClass + "'],");
        if(SelectedNodeCssClass != string.Empty) oStartupSB.Append("['SelectedNodeCssClass','" + this.SelectedNodeCssClass + "'],");
        if(SelectedNodeRowCssClass != string.Empty) oStartupSB.Append("['SelectedNodeRowCssClass','" + this.SelectedNodeRowCssClass + "'],");
        if (SoaService != string.Empty) oStartupSB.Append("['SoaService','" + this.SoaService + "'],");
        if (WebService != string.Empty) oStartupSB.Append("['WebService','" + this.WebService + "'],");
        if (WebServiceCustomParameter != string.Empty) oStartupSB.Append("['WebServiceCustomParameter','" + this.WebServiceCustomParameter + "'],");
        if (WebServiceMethod != string.Empty) oStartupSB.Append("['WebServiceMethod','" + this.WebServiceMethod + "'],");

        // End properties
        oStartupSB.Append("];\n");
        
        // Render treeview
        oStartupSB.Append(sTreeVarName + ".Initialize('" + sTreeVarName + "');\n");

        if(this.KeyboardEnabled)
        {
          // Create client script to register keyboard shortcuts
          StringBuilder oKeyboardSB = new StringBuilder();
          GenerateKeyShortcutScript(sTreeVarName, this.Nodes, oKeyboardSB);
          oStartupSB.Append(oKeyboardSB.ToString());
        }

        if(this.SelectedNode != null)
        {
          oStartupSB.Append(sTreeVarName + ".SelectNodeById('" + this.SelectedNode.PostBackID + "',true);\n");
        }

        oStartupSB.Append("\nwindow." + sTreeVarName + "_loaded = true;\n}\n");
        
        // Initiate TreeView creation
        oStartupSB.Append("ComponentArt_Init_" + sTreeVarName + "();");
        
        WriteStartupScript(output, this.DemarcateClientScript(oStartupSB.ToString()));
      }
    }

		#endregion

		#region Private Methods

    /// <summary>
    /// Put together the list of images used in the given tree structure.
    /// </summary>
    /// <param name="arNodes">The collection of nodes to start building from.</param>
    private void BuildImageList(TreeViewNodeCollection arNodes)
    {
      foreach(TreeViewNode oNode in arNodes)
      {
        if(oNode.ImageUrl != string.Empty)
        {
          string sImage = ConvertImageUrl(oNode.ImageUrl);
          if(!this.PreloadImages.Contains(sImage))
          {
            this.PreloadImages.Add(sImage);
          }
        }
        if(oNode.ExpandedImageUrl != string.Empty)
        {
          string sImage = ConvertImageUrl(oNode.ExpandedImageUrl);
          if(!this.PreloadImages.Contains(sImage))
          {
            this.PreloadImages.Add(sImage);
          }
        }
        if(oNode.MarginImageUrl != string.Empty)
        {
          string sImage = ConvertImageUrl(oNode.MarginImageUrl);
          if(!this.PreloadImages.Contains(sImage))
          {
            this.PreloadImages.Add(sImage);
          }
        }

        BuildImageList(oNode.Nodes);
      }
    }

    /// <summary>
    /// Put together the list of images that need to be pre-loaded.
    /// </summary>
    private void BuildImageList()
    {
      this.PreloadImages.Clear();

      // preload individual set images
      if(this.CollapseImageUrl != string.Empty)
      {
        this.PreloadImages.Add(ConvertImageUrl(this.CollapseImageUrl));
      }
      if(this.ExpandImageUrl != string.Empty)
      {
        this.PreloadImages.Add(ConvertImageUrl(this.ExpandImageUrl));
      }
      if(this.NoExpandImageUrl != string.Empty)
      {
        this.PreloadImages.Add(ConvertImageUrl(this.NoExpandImageUrl));
      }
      if(this.ParentNodeImageUrl != string.Empty)
      {
        this.PreloadImages.Add(ConvertImageUrl(this.ParentNodeImageUrl));
      }
      if(this.LeafNodeImageUrl != string.Empty)
      {
        this.PreloadImages.Add(ConvertImageUrl(this.LeafNodeImageUrl));
      }
      if(this.ContentLoadingImageUrl != string.Empty)
      {
        this.PreloadImages.Add(ConvertImageUrl(this.ContentLoadingImageUrl));
      }

      if(this.ShowLines)
      {
        // preload line images
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "i.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "l.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "r.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "t.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "lplus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "rplus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "tplus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "lminus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "rminus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "tminus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "minus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "plus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "dash.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "dashplus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "dashminus.gif"));
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "noexpand.gif"));
      }

      // Recursively add images used by individual nodes.
      BuildImageList(this.Nodes);
    }

    /// <summary>
    /// Put together a client-script string representation of this TreeView.
    /// </summary>
    /// <returns></returns>
    private string BuildStorage()
    {
      TreeViewNodeCollection arNodes;

      if(this.RenderRootNodeId != string.Empty)
      {
        TreeViewNode oRootNode = this.FindNodeById(this.RenderRootNodeId);

        if(oRootNode == null)
        {
          throw new Exception("No node found with ID \"" + this.RenderRootNodeId + "\".");
        }
        else
        {
          if(this.RenderRootNodeInclude)
          {
            TreeViewNodeCollection oRootNodes = new TreeViewNodeCollection(this, null);
            oRootNodes.Add(oRootNode);
            arNodes = oRootNodes;
          }
          else
          {
            arNodes = oRootNode.Nodes;	
          }
        }
      }
      else
      {
        arNodes = this.Nodes;
      }

      JavaScriptArray arNodeList = new JavaScriptArray();

      foreach(TreeViewNode oNode in arNodes)
      {
        ProcessNode(oNode, arNodeList, -1, 1);
      }

      return arNodeList.ToString();
    }

    /// <summary>
    /// Go through the TreeView nodes, determining if default styles are needed anywhere, and if so, apply them.
    /// Returns whether any default styles were applied.
    /// </summary>
    private bool ConsiderDefaultStylesRecurse(TreeViewNodeCollection arItems)
    {
      bool bNeedDefaults = false;

      foreach(TreeViewNode oItem in arItems)
      {
        // is this item in need of default styles?
        if( this.CssClass == string.Empty && this.NodeCssClass == string.Empty && this.NodeRowCssClass == string.Empty &&
          !this.ExtendNodeCells && this.LeafNodeImageUrl == string.Empty && this.ParentNodeImageUrl == string.Empty &&
          this.CollapseImageUrl == string.Empty && this.ExpandImageUrl == string.Empty &&
          !this.ShowLines && oItem.ImageUrl == string.Empty &&
          oItem.HoverCssClass == string.Empty && oItem.CssClass == string.Empty &&
          oItem.RowCssClass == string.Empty && oItem.HoverRowCssClass == string.Empty )
        {
          bNeedDefaults = true;
          oItem._defaultStyle = true;
          
          // apply styles to node
          if(!this.IsRunningInDesignMode())
          {
            // NOTHING TO BE DONE, cut this thing short.
            return bNeedDefaults;
          }
          else
          {
            oItem.Attributes.Add("style", "background-color:#3F3F3F;color:white;font-family:verdana;font-size:11px;padding:2px;padding-left:10px;border:1px;border-color:black;border-top-color:#808080;border-left-color:#808080;border-style:solid;cursor:pointer;");
          }
        }

        if(oItem.nodes != null)
        {
          bNeedDefaults = ConsiderDefaultStylesRecurse(oItem.Nodes) || bNeedDefaults;
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

      if(ConsiderDefaultStylesRecurse(this.Nodes) | this.RenderDefaultStyles) /* purposely not using short-circuit OR */ 
      {
        // apply styles to the control
        this.ExtendNodeCells = true;
        
        // register default styles
        if(!this.IsRunningInDesignMode())
        {
          this.CssClass = "ctv_TreeView";
          this.NodeCssClass = "ctv_Node";
          this.HoverNodeCssClass = "ctv_HoverNode";
          this.SelectedNodeCssClass = "ctv_SelectedNode";
        }
        else
        {
          this.Attributes.Add("style", "background-color:#F6F6F6;border:1px solid black;padding:2px;padding-left:5px;");
        }

        return true;
      }

      return false;
    }

    /// <summary>
    /// Put together client script that registers all keyboard shortcuts contained in the given tree structure.
    /// </summary>
    /// <param name="sTreeVarName">Client-side TreeView object identifier.</param>
    /// <param name="arNodeList">Root nodes to begin searching from.</param>
    /// <param name="oSB">StringBuilder to add content to.</param>
    private void GenerateKeyShortcutScript(string sTreeVarName, TreeViewNodeCollection arNodeList, StringBuilder oSB)
    {
      foreach(TreeViewNode oNode in arNodeList)
      {
        if(oNode.KeyboardShortcut != string.Empty)
        {
          oSB.Append("ComponentArt_RegisterKeyHandler(" + sTreeVarName + ",'" + oNode.KeyboardShortcut + "', '" + sTreeVarName + ".SelectNodeById(\\'" + oNode.PostBackID + "\\',true)');\n");
        }

        GenerateKeyShortcutScript(sTreeVarName, oNode.Nodes, oSB);
      }
    }

    /// <summary>
    /// Build an array of checked nodes.
    /// </summary>
    /// <param name="arNodes">Root nodes to start building from.</param>
    /// <returns>The list in a string.</returns>
    private TreeViewNode [] GetCheckedNodes(TreeViewNodeCollection arNodes)
    {
      ArrayList oNodeList = new ArrayList();
			
      foreach(TreeViewNode oNode in arNodes)
      {
        if(oNode.Checked)
        {
          oNodeList.Add(oNode);
        }

        TreeViewNode [] arSubChecked = GetCheckedNodes(oNode.Nodes);
				
        if(arSubChecked.Length > 0)
        {
          oNodeList.AddRange(arSubChecked);
        }
      }

      return (TreeViewNode [])oNodeList.ToArray(typeof(ComponentArt.Web.UI.TreeViewNode));
    }

    /// <summary>
    /// Build an array of multiple-selected nodes.
    /// </summary>
    /// <param name="arNodes">Root nodes to start building from.</param>
    /// <returns>The list in a string.</returns>
    private TreeViewNode [] GetMultipleSelectedNodes(TreeViewNodeCollection arNodes)
    {
      ArrayList oNodeList = new ArrayList();
			
      foreach(TreeViewNode oNode in arNodes)
      {
        if(oNode.IsMultipleSelected)
        {
          oNodeList.Add(oNode);
        }

        TreeViewNode [] arSubSelected = GetMultipleSelectedNodes(oNode.Nodes);
				
        if(arSubSelected.Length > 0)
        {
          oNodeList.AddRange(arSubSelected);
        }
      }

      return (TreeViewNode [])oNodeList.ToArray(typeof(ComponentArt.Web.UI.TreeViewNode));
    }

    private void UnLoadNonCurrentOnDemandNodes(TreeViewNodeCollection arNodes)
    {
      if(arNodes != null)
      {
        foreach(TreeViewNode oNode in arNodes)
        {
          if(oNode.ContentCallbackUrl != string.Empty && !oNode.IsChildSelected)
          {
            oNode.nodes = null;
          }

          if(oNode.nodes != null)
          {
            this.UnLoadNonCurrentOnDemandNodes(oNode.Nodes);
          }
        }
      }
    }

    private void LoadAllOnDemandNodes(TreeViewNodeCollection arNodes)
    {
      if(arNodes != null)
      {
        foreach(TreeViewNode oNode in arNodes)
        {
          if(oNode.ContentCallbackUrl != string.Empty)
          {
            this.LoadOnDemand(oNode, oNode.ContentCallbackUrl);
          }

          if(oNode.nodes != null)
          {
            this.LoadAllOnDemandNodes(oNode.Nodes);
          }
        }
      }
    }

    private void LoadOnDemandAssignIds(TreeViewNodeCollection arNodes)
    {
      foreach(TreeViewNode oNode in arNodes)
      {
        if(oNode.ID != string.Empty)
        {
          oNode.PostBackID = "p_" + oNode.ID;
        }
  
        if(oNode.nodes != null && oNode.Nodes.Count > 0)
        {
          LoadOnDemandAssignIds(oNode.Nodes);
        }
      }
    }

    private void LoadOnDemand(TreeViewNode oNode, string sCallbackUrl)
    {
      // is it a file?
      if(sCallbackUrl.ToLower().EndsWith(".xml") && File.Exists(Context.Server.MapPath(sCallbackUrl)))
      {
        this.LoadXmlNode(oNode, sCallbackUrl);
      }
      else
      {
        // maybe a url location
        try
        {
          XmlDocument oXmlDoc = new XmlDocument();

          XmlUrlResolver urlResolver = new XmlUrlResolver();
		 
          urlResolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
          oXmlDoc.XmlResolver = urlResolver;

          Uri oRequest = new Uri(Context.Request.Url, sCallbackUrl);

          oXmlDoc.Load(oRequest.AbsoluteUri.ToString());

          this.LoadXmlNode(oNode, oXmlDoc.DocumentElement);
        }
        catch(Exception ex)
        {
          // we can't load the on-demand path
          throw new Exception("Could not load data from " + sCallbackUrl + ": " + ex);
        }
      }

      if(oNode.nodes != null && oNode.Nodes.Count > 0)
      {
        this.LoadOnDemandAssignIds(oNode.Nodes);
      }
    }

    private string MakeStringJScriptSafe(string sString)
    {
      return sString.Replace("\\", "\\\\").Replace("'", "\\'");
    }

    /// <summary>
    /// Process a node in the process of building client-side storage.
    /// </summary>
    /// <param name="oNode">The node to process.</param>
    /// <param name="arNodeList">List to add processed nodes to, including this one.</param>
    /// <param name="iParentIndex">Index of the given node's parent in the storage list.</param>
    /// <param name="depth">The depth of this node in the tree structure.</param>
    /// <returns>The index in the array of the newly added node.</returns>
    private int ProcessNode(TreeViewNode oNode, ArrayList arNodeList, int iParentIndex, int depth)
    {
      ArrayList arNodeItems = new ArrayList();
      int iNewNodeIndex = arNodeList.Count;
      arNodeList.Add(arNodeItems);

      arNodeItems.Add(oNode.PostBackID);
			arNodeItems.Add(iParentIndex);

      ArrayList arChildIndices = new ArrayList();

      if(oNode.nodes != null && this.RenderDrillDownDepth == 0 || this.RenderDrillDownDepth > depth)
      {
        foreach(TreeViewNode oChildNode in oNode.Nodes)
        {
          arChildIndices.Add(ProcessNode(oChildNode, arNodeList, iNewNodeIndex, depth + 1));
        }
      }

      arNodeItems.Add(arChildIndices);

      // pre-processing: set expanded to true if some things hold
      if(this.ExpandSelectedPath && (oNode.IsChildSelected || oNode.IsChildForceHighlighted()))
      {
        oNode.Expanded = true;
      }

      ArrayList arProperties = new ArrayList();

      foreach (string propertyName in oNode.Properties.Keys)
      {
        switch (oNode.GetVarAttributeName(propertyName).ToLower(System.Globalization.CultureInfo.InvariantCulture))
        {
            // bools
          case "autopostbackonselect": arProperties.Add(new object [] {"AutoPostBackOnSelect", oNode.AutoPostBackOnSelect}); break;
          case "autopostbackonmove": arProperties.Add(new object [] {"AutoPostBackOnMove", oNode.AutoPostBackOnMove}); break;
          case "autopostbackonrename": arProperties.Add(new object [] {"AutoPostBackOnRename", oNode.AutoPostBackOnRename}); break;
          case "autopostbackoncheckchanged": arProperties.Add(new object [] {"AutoPostBackOnCheckChanged", oNode.AutoPostBackOnCheckChanged}); break;
          case "autopostbackonexpand": arProperties.Add(new object [] {"AutoPostBackOnExpand", oNode.AutoPostBackOnExpand}); break;
          case "autopostbackoncollapse": arProperties.Add(new object [] {"AutoPostBackOnCollapse", oNode.AutoPostBackOnCollapse}); break;
          case "checked": if(oNode.Checked) arProperties.Add(new object [] {"Checked", true}); break;
          case "draggingacrosstreesenabled": arProperties.Add(new object [] {"DraggingAcrossTreesEnabled", oNode.DraggingAcrossTreesEnabled}); break;
          case "draggingenabled": arProperties.Add(new object [] {"DraggingEnabled", oNode.DraggingEnabled}); break;
          case "droppingacrosstreesenabled": arProperties.Add(new object [] {"DroppingAcrossTreesEnabled", oNode.DroppingAcrossTreesEnabled}); break;
          case "droppingenabled": arProperties.Add(new object [] {"DroppingEnabled", oNode.DroppingEnabled}); break;
          case "editingenabled": arProperties.Add(new object [] {"EditingEnabled", oNode.EditingEnabled}); break;
          case "expanded": if(oNode.Expanded) arProperties.Add(new object [] {"Expanded", true}); break;
          case "extendnodecell": arProperties.Add(new object [] {"ExtendNodeCell", oNode.ExtendNodeCell}); break;
          case "ismultipleselected": if(oNode.IsMultipleSelected) arProperties.Add(new object [] {"IsMultipleSelected", true}); break;
          case "showcheckbox": arProperties.Add(new object [] {"ShowCheckBox", oNode.ShowCheckBox}); break;
          case "selectable": arProperties.Add(new object [] {"Selectable", oNode.Selectable}); break;
          case "visible": arProperties.Add(new object [] {"Visible", oNode.Visible}); break;
          case "usewebservice": arProperties.Add(new object [] {"UseWebService", oNode.UseWebService}); break;

            // ints
          case "imageheight": arProperties.Add(new object [] {"ImageHeight", oNode.ImageHeight}); break;
          case "imagewidth": arProperties.Add(new object [] {"ImageWidth", oNode.ImageWidth}); break;
          case "indent": arProperties.Add(new object [] {"Indent", oNode.Indent}); break;
          case "labelpadding": arProperties.Add(new object [] {"LabelPadding", oNode.LabelPadding}); break;
          
            // strings
          case "clientsidecommand": arProperties.Add(new object [] {"ClientSideCommand", oNode.ClientSideCommand}); break;
          case "clienttemplateid": arProperties.Add(new object [] {"ClientTemplateId", oNode.ClientTemplateId}); break;
          case "childselectedcssclass": arProperties.Add(new object [] {"ChildSelectedCssClass", oNode.ChildSelectedCssClass}); break;
          case "childselectedhovercssclass": arProperties.Add(new object [] {"ChildSelectedHoverCssClass", oNode.ChildSelectedHoverCssClass}); break;
          case "childselectedhoverrowcssclass": arProperties.Add(new object [] {"ChildSelectedHoverRowCssClass", oNode.ChildSelectedHoverRowCssClass}); break;
          case "childselectedrowcssclass": arProperties.Add(new object [] {"ChildSelectedRowCssClass", oNode.ChildSelectedRowCssClass}); break;
          case "contentcallbackurl": arProperties.Add(new object [] {"ContentCallbackUrl", oNode.ContentCallbackUrl}); break;
          case "cssclass": arProperties.Add(new object [] {"CssClass", oNode.CssClass}); break;
          case "cutcssclass": arProperties.Add(new object [] {"CutCssClass", oNode.CutCssClass}); break;
          case "cutrowcssclass": arProperties.Add(new object [] {"CutRowCssClass", oNode.CutRowCssClass}); break;
          case "expandedimageurl": arProperties.Add(new object [] {"ExpandedImageUrl", oNode.ExpandedImageUrl}); break;
          case "hovercssclass": arProperties.Add(new object [] {"HoverCssClass", oNode.HoverCssClass}); break;
          case "hoverrowcssclass": arProperties.Add(new object [] {"HoverRowCssClass", oNode.HoverRowCssClass}); break;
          case "id": arProperties.Add(new object [] {"ID", oNode.ID}); break;
          case "imageurl": arProperties.Add(new object [] {"ImageUrl", oNode.ImageUrl}); break;
          case "marginimageurl": arProperties.Add(new object [] {"MarginImageUrl", oNode.MarginImageUrl}); break;
          case "multipleselectedcssclass": arProperties.Add(new object [] {"MultipleSelectedCssClass", oNode.MultipleSelectedCssClass}); break;
          case "multipleselectedrowcssclass": arProperties.Add(new object [] {"MultipleSelectedRowCssClass", oNode.MultipleSelectedRowCssClass}); break;
          case "navigateurl": arProperties.Add(new object [] {"NavigateUrl", oNode.NavigateUrl}); break;
          case "pageviewid": arProperties.Add(new object [] {"PageViewId", oNode.PageViewId}); break;
          case "rowcssclass": arProperties.Add(new object [] {"RowCssClass", oNode.RowCssClass}); break;
          case "selectedcssclass": arProperties.Add(new object [] {"SelectedCssClass", oNode.SelectedCssClass}); break;
          case "selectedhovercssclass": arProperties.Add(new object [] {"SelectedHoverCssClass", oNode.SelectedHoverCssClass}); break;
          case "selectedhoverrowcssclass": arProperties.Add(new object [] {"SelectedHoverRowCssClass", oNode.SelectedHoverRowCssClass}); break;
          case "selectedimageurl": arProperties.Add(new object [] {"SelectedImageUrl", oNode.SelectedImageUrl}); break;
          case "selectedexpandedimageurl": arProperties.Add(new object [] {"SelectedExpandedImageUrl", oNode.SelectedExpandedImageUrl}); break;
          case "selectedrowcssclass": arProperties.Add(new object [] {"SelectedRowCssClass", oNode.SelectedRowCssClass}); break;
          case "target": arProperties.Add(new object [] {"Target", oNode.Target}); break;
          case "templateid": arProperties.Add(new object [] {"TemplateId", oNode.TemplateId}); break;
          case "text": arProperties.Add(new object [] {"Text", Utils.MakeStringXmlSafe(oNode.Text)}); break;
          case "tooltip": arProperties.Add(new object [] {"ToolTip", Utils.MakeStringXmlSafe(oNode.ToolTip)}); break;
          case "value": arProperties.Add(new object [] {"Value", Utils.MakeStringXmlSafe(oNode.Value)}); break;
          
            // expando properties
          default:
            if(this.OutputCustomAttributes)
            {
              arProperties.Add(new object [] {oNode.GetVarAttributeName(propertyName), Utils.MakeStringXmlSafe(oNode.Properties[propertyName])});
            }
            break;
        }
      }

      arNodeItems.Add(arProperties);

      return iNewNodeIndex;
    }

	#endregion

    #region Accessible Rendering

    internal void RenderAccessibleContent(HtmlTextWriter output)
    {
      output.Write("<span class=\"treeview");
      if (this.CssClass != String.Empty && this.CssClass != null)
      {
        output.Write(" " + this.CssClass);
      }
      output.Write("\">");
      int nodeIndex = -1;
      RenderAccessibleNodes(output, null, ref nodeIndex);
      output.Write("</span>");
    }

    private void RenderAccessibleNodes(HtmlTextWriter output, TreeViewNode parentNode, ref int nodeIndex)
    {
      output.Write("<ul");

      string childNodesId = "G" + this.GetSaneId() + "_" + nodeIndex.ToString();
      output.Write(" id=\"");
      output.Write(childNodesId);
      output.Write("\"");

      output.Write(">");

      foreach (TreeViewNode node in (parentNode == null ? this.Nodes : parentNode.Nodes))
      {
        output.Write("<li");

        nodeIndex++;

        string nodeId = this.GetSaneId() + "_" + nodeIndex.ToString();
        output.Write(" id=\"");
        output.Write(nodeId);
        output.Write("\"");

        string nodeCssClass = (node.CssClass != null && node.CssClass != string.Empty) ? node.CssClass : this.NodeCssClass;
        if (nodeCssClass != null && NodeCssClass != string.Empty)
        {
          output.Write(" class=\"");
          output.Write(nodeCssClass);
          output.Write("\"");
        }

        output.Write(">");

        output.Write("<a href=\"#\" title=\"toggle\">");
        output.Write("<img src=\"#\" alt=\"Expand/Collapse\" />");
        output.Write("</a>");

        output.Write("<a");
        output.Write(" href=\"");
        output.Write(node.NavigateUrl);
        output.Write("\"");
        output.Write(">");

        output.Write("<span>");
        output.Write(node.Text);
        output.Write("</span>");

        output.Write("</a>");

        if (node.Nodes.Count > 0)
        {
          RenderAccessibleNodes(output, node, ref nodeIndex);
        }

        output.Write("</li>");
      }

      output.Write("</ul>");
    }

    #endregion Accessible Rendering

    #region Down-level Rendering

    internal void RenderDownLevelContent(HtmlTextWriter output)
    {
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
      output.WriteStyleAttribute("vertical-align", "top");
      output.WriteStyleAttribute("height", this.Height.ToString());
      output.WriteStyleAttribute("width", this.Width.ToString());
      if(this.AutoScroll)
      {
        output.WriteStyleAttribute("overflow", "auto");
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
      output.Write("\">");
      
      TreeViewNodeCollection arNodes;

      if(this.RenderRootNodeId != string.Empty)
      {
        TreeViewNode oRootNode = this.FindNodeById(this.RenderRootNodeId);

        if(oRootNode == null)
        {
          throw new Exception("No node found with specified RenderRootNodeId.");
        }
        else
        {
          if(this.RenderRootNodeInclude)
          {
            TreeViewNodeCollection oRootNodes = new TreeViewNodeCollection(this, null);
            oRootNodes.Add(oRootNode);
            arNodes = oRootNodes;
          }
          else
          {
            arNodes = oRootNode.Nodes;
          }
        }
      }
      else
      {
        arNodes = this.Nodes;
      }

      RenderDownLevelNodes(output, arNodes, 0);
			
      output.Write("</div>");
    }

    private void RenderDownLevelExpandCollapse(HtmlTextWriter output, TreeViewNode oNode)
    {
      if(oNode.Nodes.Count > 0)
      {
        if(Page != null)
        {
          output.AddAttribute("href", "javascript:" + Page.GetPostBackEventReference(this, (oNode.Expanded? "COLLAPSE " : "EXPAND ") + oNode.PostBackID));
        }
        output.RenderBeginTag(HtmlTextWriterTag.A);

        if(oNode.Expanded)
        {
          if(this.ShowLines)
          {
            if(oNode.ParentNode == null) // is a root?
            {
              if(oNode.PreviousSibling == null) // is it the first root?
              {
                if(oNode.NextSibling == null) // is it the only?
                {
                  output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "dashminus.gif"));
                }
                else
                {
                  output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "rminus.gif"));
                }
              }
              else if(oNode.NextSibling == null) // is it the last root?
              {
                output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "lminus.gif"));
              }
              else
              {
                output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "tminus.gif"));
              }
            }
            else
            {
              if(oNode.NextSibling == null)
              {
                output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "lminus.gif"));
              }
              else
              {
                output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "tminus.gif"));
              }
            }
          }
          else
          {
            output.AddAttribute("src", ConvertImageUrl(this.CollapseImageUrl));
          }
        }
        else
        {
          if(this.ShowLines)
          {
            if(oNode.ParentNode == null) // is a root?
            {
              if(oNode.PreviousSibling == null) // is it the first root?
              {
                if(oNode.NextSibling == null)
                {
                  output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "dashplus.gif"));
                }
                else
                {
                  output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "rplus.gif"));
                }
              }
              else if(oNode.NextSibling == null)
              {
                output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "lplus.gif"));
              }
              else
              {
                output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "tplus.gif"));
              }
            }
            else
            {
              if(oNode.NextSibling == null)
              {
                output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "lplus.gif"));
              }
              else
              {
                output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "tplus.gif"));
              }
            }
          }
          else
          {
            output.AddAttribute("src", ConvertImageUrl(this.ExpandImageUrl));
          }
        }
		
        output.AddAttribute("border", "0");
        output.AddAttribute("alt", "");
        output.RenderBeginTag(HtmlTextWriterTag.Img);
        output.RenderEndTag();

        output.RenderEndTag();
      }
      else
      {
        if(this.ShowLines)
        {
          if(oNode.ParentNode == null) // is a root?
          {
            if(oNode.PreviousSibling == null) // is the first root?
            {
              if(oNode.NextSibling == null)
              {
                output.Write("<img alt=\"\" src=\"" + Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "dash.gif") + "\" />");
              }
              else
              {
                output.Write("<img alt=\"\" src=\"" + Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "r.gif") + "\" />");
              }
            }
            else if(oNode.NextSibling == null)
            {
              output.Write("<img alt=\"\" src=\"" + Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "l.gif") + "\" />");
            }
            else
            {
              output.Write("<img alt=\"\" src=\"" + Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "t.gif") + "\" />");
            }
          }
          else
          {
            if(oNode.NextSibling == null)
            {
              output.Write("<img alt=\"\" src=\"" + Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "l.gif") + "\" />");
            }
            else
            {
              output.Write("<img alt=\"\" src=\"" + Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "t.gif") + "\" />");
            }
          }
        }
        else
        {
          if(this.NoExpandImageUrl != string.Empty)
          {
            output.Write("<img alt=\"\" src=\"" + ConvertImageUrl(this.NoExpandImageUrl) + "\" />");
          }
          else
          {	
            this.RenderDownLevelIndentation(output, oNode, oNode.Depth);
          }     
        }
      }
    }

    private void RenderDownLevelIndentation(HtmlTextWriter output, TreeViewNode oNode, int iDepth)
    {
      if(this.ShowLines)
      {
        TreeViewNode oParent = oNode;
        for(int i = oNode.Depth; i > iDepth; i--)
        {
          oParent = oParent.ParentNode;
        }
		
        if(oParent.NextSibling == null) // is last in group?
        {
          output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "noexpand.gif"));
        }
        else
        {
          output.AddAttribute("src", Utils.ConvertUrl(Context, this.LineImagesFolderUrl, "i.gif"));
        }

        output.AddAttribute("alt", "");
        output.RenderBeginTag(HtmlTextWriterTag.Img);
        output.RenderEndTag();
      }
      else
      {
        output.AddStyleAttribute("height", "1px");
        output.AddStyleAttribute("width", oNode.Indent.ToString() + "px");
        output.RenderBeginTag(HtmlTextWriterTag.Div);
        output.RenderEndTag();
      }
    }

    private void RenderDownLevelMargin(HtmlTextWriter output, TreeViewNode oNode)
    {
      if(this.MarginCssClass != string.Empty)
      {
        output.AddAttribute("class", this.MarginCssClass);
      }

      output.RenderBeginTag(HtmlTextWriterTag.Td);
      output.Write("<div style=\"width:" + this.MarginWidth + "px;\">");
      if(oNode.MarginImageUrl != string.Empty)
      {
        output.Write("<img alt=\"\" src=\"" + ConvertImageUrl(oNode.MarginImageUrl) + "\" />");
      }    
      output.Write("</div>");
      
      output.RenderEndTag();
    }

    private void RenderDownLevelNodes(HtmlTextWriter output, TreeViewNodeCollection arNodes, int depth)
    {
      // Render children
      foreach(TreeViewNode oNode in arNodes)
      {
        output.AddAttribute("cellpadding", "0");
        output.AddAttribute("cellspacing", "0");
        output.AddAttribute("border", "0");
        output.RenderBeginTag(HtmlTextWriterTag.Table); // <table>

        string sRowClass = (oNode.RowCssClass != string.Empty ? oNode.RowCssClass : this.NodeRowCssClass);
        if(sRowClass != string.Empty)
        {
          output.AddAttribute("class", sRowClass);
        }

        output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>

        if(oNode.ParentTreeView.DisplayMargin)
        {
          this.RenderDownLevelMargin(output, oNode);
        }

        // Render indentation
        if(!this.ExpandCollapseInFront && (oNode.Indent > 0 || this.ShowLines))
        {
          for(int i = 0; i < oNode.Depth; i++)
          {
            output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            this.RenderDownLevelIndentation(output, oNode, i);
            output.RenderEndTag(); // </td>
          }
        }

        // Render expand/collapse cell (if we have the images)
        if((this.ShowLines && this.LineImagesFolderUrl != string.Empty) ||
          (this.ExpandImageUrl != string.Empty && this.CollapseImageUrl != string.Empty))
        {
          output.AddAttribute("align", "center");
          output.RenderBeginTag(HtmlTextWriterTag.Td);
          this.RenderDownLevelExpandCollapse(output, oNode);
          output.RenderEndTag();
        }

        // Render indentation
        if(this.ExpandCollapseInFront && oNode.Indent > 0)
        {
          for(int i = 0; i < oNode.Depth; i++)
          {
            output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            this.RenderDownLevelIndentation(output, oNode, i);
            output.RenderEndTag(); // </td>
          }
        }

        // Render icon cell
        output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
        if(oNode.ImageUrl != string.Empty)
        {
          if(oNode.Expanded && oNode.ExpandedImageUrl != string.Empty)
          {
            output.Write("<img alt=\"\" src=\"" + ConvertImageUrl(oNode.ExpandedImageUrl) + "\" />");
          }
          else
          {
            output.Write("<img alt=\"\" src=\"" + ConvertImageUrl(oNode.ImageUrl) + "\" />");
          }
        }
        else
        {
          if(oNode.Nodes.Count > 0)
          {
            if(this.ParentNodeImageUrl != string.Empty)
            {
              output.Write("<img alt=\"\" src=\"" + ConvertImageUrl(this.ParentNodeImageUrl) + "\" />");
            }
          }
          else
          {
            if(this.LeafNodeImageUrl != string.Empty)
            {
              output.Write("<img alt=\"\" src=\"" + ConvertImageUrl(this.LeafNodeImageUrl) + "\" />");
            }
          }
        }
        output.RenderEndTag(); // </td>
				
        // Render label cell
        if(this.RenderDefaultStyles && this.IsRunningInDesignMode())
        {
          output.AddAttribute("style", oNode.Attributes["style"]);
        }
        output.AddAttribute("nowrap", "true");
        string sCellClass = (oNode.CssClass != string.Empty? oNode.CssClass : this.NodeCssClass);
        if(sCellClass != string.Empty)
        {
          output.AddAttribute("class", sCellClass);
        }
        output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

        // output link
        if(oNode.NavigateUrl != string.Empty)
        {
          output.AddAttribute("href", oNode.NavigateUrl);
          if(oNode.Target != string.Empty)
          {
            output.AddAttribute("target", oNode.Target);
          }
        }
        else if(Context != null && Page != null)
        {
          output.AddAttribute("href", "javascript:" + Page.GetPostBackEventReference(this, oNode.PostBackID));
        }
        output.RenderBeginTag(HtmlTextWriterTag.A);

        // output template or label
        if(oNode.TemplateId != string.Empty)
        {
          string sTemplateId = this.ClientID + "_" + oNode.PostBackID;

          // we have a template for this item.
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
          output.Write(oNode.Text);
        }

        output.RenderEndTag(); // </a>
        output.RenderEndTag(); // </td>

        // Render pusher cell
        output.AddAttribute("width", "100%");
        output.RenderBeginTag(HtmlTextWriterTag.Td);
        output.RenderEndTag();

        output.RenderEndTag(); // </tr>
        output.RenderEndTag(); // </table>

        if(oNode.Expanded && (this.RenderDrillDownDepth == 0 || depth < this.RenderDrillDownDepth))
        {
          RenderDownLevelNodes(output, oNode.Nodes, depth + 1);
        }
      }
    }

    public void ApplyTheming(bool? overwriteSettings)
    {
      bool overwrite = overwriteSettings ?? false;
      string prefix = this.AutoThemingCssClassPrefix ?? "";

      if ((this.CssClass ?? string.Empty) == string.Empty || overwrite) CssClass = prefix + "treeview";
      if ((this.NodeCssClass ?? string.Empty) == string.Empty || overwrite) NodeCssClass = prefix + "treeview-node";
      if ((this.HoverNodeCssClass ?? string.Empty) == string.Empty || overwrite) HoverNodeCssClass = prefix + "item-hover " + prefix + "treeview-node";
      if ((this.SelectedNodeCssClass ?? string.Empty) == string.Empty || overwrite) SelectedNodeCssClass = prefix + "item-selected " + prefix + "treeview-node";
      if ((this.NodeRowCssClass ?? string.Empty) == string.Empty || overwrite) NodeRowCssClass = prefix + "treeview-row";
      if ((this.HoverNodeRowCssClass ?? string.Empty) == string.Empty || overwrite) HoverNodeRowCssClass = prefix + "item-hover " + prefix + "treeview-row";
      if ((this.SelectedNodeRowCssClass ?? string.Empty) == string.Empty || overwrite) SelectedNodeRowCssClass = prefix + "item-selected " + prefix + "treeview-row";

      if ((this.NodeClientTemplateId ?? string.Empty) == string.Empty || overwrite) this.NodeClientTemplateId = "TreeViewItemTemplate";

      AddClientTemplate(overwrite, "TreeViewItemTemplate", @"
          <div class=""" + prefix + @"treeview-item"">
						<div class=""## DataItem.getProperty('IconCssClass') == null ? '' : '" + prefix + @"icon ' + DataItem.getProperty('IconCssClass'); ##"">
							<a href=""## DataItem.getProperty('NavigateUrl') == null ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##"" onclick=""this.blur();"">
								<span>## DataItem.getProperty('Text'); ##</span>
							</a>
						</div>
					</div>"
      );

      AddClientTemplate(overwrite, "TreeViewItemLeftIconTemplate", @"
						<div class=""" + prefix + @"treeview-item " + prefix + @"treeview-item-left-icon"">
							<img src=""" + Utils.ResolveBaseUrl(this.Context, this.ImagesBaseUrl) + @"## DataItem.getProperty('IconUrl'); ##"" width=""16"" height=""16"" border=""0"" />
							<a href=""## DataItem.getProperty('NavigateUrl') == null ? 'javascript:void(0);' : DataItem.getProperty('NavigateUrl'); ##"" onclick=""this.blur();"">
								<span>## DataItem.getProperty('Text'); ##</span>
							</a>
						</div>
      ");

      ApplyThemedClientTemplatesToNodes(overwrite, this.Nodes);
    }

    private void ApplyThemedClientTemplatesToNodes(bool overwrite, TreeViewNodeCollection nodes)
    {
      foreach (TreeViewNode node in nodes)
      {
        if (node.ClientTemplateId == string.Empty || overwrite)
        {
          if (node.Attributes["IconUrl"] != null)
          {
            node.ClientTemplateId = "TreeViewItemLeftIconTemplate";
          }
        }
        ApplyThemedClientTemplatesToNodes(overwrite, node.Nodes);
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

		#region Delegates 

    /// <summary>
    /// Delegate for <see cref="NodeSelected"/> event of <see cref="TreeView"/> class.
    /// </summary>
    public delegate void NodeSelectedEventHandler(object sender, TreeViewNodeEventArgs e);

    /// <summary>
    /// Fires after a treeview node is selected.
    /// </summary>
    [ Description("Fires after a treeview node is selected."), 
    Category("TreeView Events") ]
    public event NodeSelectedEventHandler NodeSelected;

    private void OnNodeSelected(TreeViewNodeEventArgs e) 
    {         
      if (NodeSelected != null) 
      {
        NodeSelected(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NodeRenamed"/> event of <see cref="TreeView"/> class.
    /// </summary>
    public delegate void NodeRenamedEventHandler(object sender, TreeViewNodeRenamedEventArgs e);
		
    /// <summary>
    /// Fires after a treeview node is renamed.
    /// </summary>
    [ Description("Fires after a treeview node is renamed."), 
    Category("TreeView Events") ]
    public event NodeRenamedEventHandler NodeRenamed;

    private void OnNodeRenamed(TreeViewNodeRenamedEventArgs e) 
    {         
      if (NodeRenamed != null) 
      {
        NodeRenamed(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NodeCopied"/> event of <see cref="TreeView"/> class.
    /// </summary>
    public delegate void NodeCopiedEventHandler(object sender, TreeViewNodeCopiedEventArgs e);

    /// <summary>
    /// Fires after a treeview node is copied.
    /// </summary>
    [ Description("Fires after a treeview node is copied."), 
    Category("TreeView Events") ]
    public event NodeCopiedEventHandler NodeCopied;

    private void OnNodeCopied(TreeViewNodeCopiedEventArgs e) 
    {
      if (NodeCopied != null) 
      {
        NodeCopied(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NodeMoved"/> event of <see cref="TreeView"/> class.
    /// </summary>
    public delegate void NodeMovedEventHandler(object sender, TreeViewNodeMovedEventArgs e);

    /// <summary>
    /// Fires after a treeview node is moved.
    /// </summary>
    [ Description("Fires after a treeview node is moved."), 
    Category("TreeView Events") ]
    public event NodeMovedEventHandler NodeMoved;

    private void OnNodeMoved(TreeViewNodeMovedEventArgs e) 
    {
      if (NodeMoved != null) 
      {
        NodeMoved(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NodeCheckChanged"/> event of <see cref="TreeView"/> class.
    /// </summary>
    public delegate void NodeCheckChangedEventHandler(object sender, TreeViewNodeEventArgs e);
		
    /// <summary>
    /// Fires after a treeview node is checked or unchecked.
    /// </summary>
    [ Description("Fires after a treeview node is checked or unchecked."),
    Category("TreeView Events") ]
    public event NodeCheckChangedEventHandler NodeCheckChanged;

    private void OnNodeCheckChanged(TreeViewNodeEventArgs e) 
    {
      if (NodeCheckChanged != null) 
      {
        NodeCheckChanged(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NodeExpanded"/> event of <see cref="TreeView"/> class.
    /// </summary>
    public delegate void NodeExpandedEventHandler(object sender, TreeViewNodeEventArgs e);
		
    /// <summary>
    /// Fires after a treeview node is expanded.
    /// </summary>
    [ Description("Fires after a treeview node is expanded."),
    Category("TreeView Events") ]
    public event NodeExpandedEventHandler NodeExpanded;

    private void OnNodeExpanded(TreeViewNodeEventArgs e) 
    {
      if (NodeExpanded != null) 
      {
        NodeExpanded(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NodeCollapsed"/> event of <see cref="TreeView"/> class.
    /// </summary>
    public delegate void NodeCollapsedEventHandler(object sender, TreeViewNodeEventArgs e);
		
    /// <summary>
    /// Fires after a treeview node is collapsed.
    /// </summary>
    [ Description("Fires after a treeview node is collapsed."),
    Category("TreeView Events") ]
    public event NodeCollapsedEventHandler NodeCollapsed;

    private void OnNodeCollapsed(TreeViewNodeEventArgs e) 
    {
      if (NodeCollapsed != null) 
      {
        NodeCollapsed(this, e);
      }   
    }

    /// <summary>
    /// Delegate for <see cref="NodeDataBound"/> event of <see cref="TreeView"/> class.
    /// </summary>
    public delegate void NodeDataBoundEventHandler(object sender, TreeViewNodeDataBoundEventArgs e);
		
    /// <summary>
    /// Fires after a treeview node is data bound.
    /// </summary>
    [ Description("Fires after a treeview node is data bound."),
    Category("TreeView Events") ]
    public event NodeDataBoundEventHandler NodeDataBound;

    // generic trigger
    protected override void OnNodeDataBound(NavigationNode oNode, object oDataItem) 
    {
      if (NodeDataBound != null) 
      {
        TreeViewNodeDataBoundEventArgs e = new TreeViewNodeDataBoundEventArgs();
        
        e.Node = (TreeViewNode)oNode;
        e.DataItem = oDataItem;

        NodeDataBound(this, e);
      }   
    }

		#endregion
  }
}
