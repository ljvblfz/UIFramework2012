using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="TreeView"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class TreeViewClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires when a callback request completes.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent CallbackComplete
    {
      get
      {
        return this.GetValue("CallbackComplete");
      }
      set
      {
        this.SetValue("CallbackComplete", value);
      }
    }

    /// <summary>
    /// This event fires when the user right-clicks a node.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ContextMenu
    {
      get
      {
        return this.GetValue("ContextMenu");
      }
      set
      {
        this.SetValue("ContextMenu", value);
      }
    }

    /// <summary>
    /// This event fires when the TreeView is done loading on the client.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Load
    {
      get
      {
        return this.GetValue("Load");
      }
      set
      {
        this.SetValue("Load", value);
      }
    }

    /// <summary>
    /// This event fires before a checkable node is checked or unchecked.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeBeforeCheckChange
    {
      get
      {
        return this.GetValue("NodeBeforeCheckChange");
      }
      set
      {
        this.SetValue("NodeBeforeCheckChange", value);
      }
    }

    /// <summary>
    /// This event fires before an expanded node is collapsed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeBeforeCollapse
    {
      get
      {
        return this.GetValue("NodeBeforeCollapse");
      }
      set
      {
        this.SetValue("NodeBeforeCollapse", value);
      }
    }

    /// <summary>
    /// This event fires before an expandable node is expanded.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeBeforeExpand
    {
      get
      {
        return this.GetValue("NodeBeforeExpand");
      }
      set
      {
        this.SetValue("NodeBeforeExpand", value);
      }
    }

    /// <summary>
    /// This event fires before a node is moved due to drag and drop.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeBeforeMove
    {
      get
      {
        return this.GetValue("NodeBeforeMove");
      }
      set
      {
        this.SetValue("NodeBeforeMove", value);
      }
    }

    /// <summary>
    /// This event fires before a node is renamed (edited).
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeBeforeRename
    {
      get
      {
        return this.GetValue("NodeBeforeRename");
      }
      set
      {
        this.SetValue("NodeBeforeRename", value);
      }
    }

    /// <summary>
    /// This event fires before a node is selected.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeBeforeSelect
    {
      get
      {
        return this.GetValue("NodeBeforeSelect");
      }
      set
      {
        this.SetValue("NodeBeforeSelect", value);
      }
    }

    /// <summary>
    /// This event fires when a checkable node is checked or unchecked.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeCheckChange
    {
      get
      {
        return this.GetValue("NodeCheckChange");
      }
      set
      {
        this.SetValue("NodeCheckChange", value);
      }
    }

    /// <summary>
    /// This event fires when an expanded node is collapsed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeCollapse
    {
      get
      {
        return this.GetValue("NodeCollapse");
      }
      set
      {
        this.SetValue("NodeCollapse", value);
      }
    }

    /// <summary>
    /// This event fires when a node is copied (Ctrl+C).
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeCopy
    {
      get
      {
        return this.GetValue("NodeCopy");
      }
      set
      {
        this.SetValue("NodeCopy", value);
      }
    }

    /// <summary>
    /// This event fires when an expandable node is expanded.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeExpand
    {
      get
      {
        return this.GetValue("NodeExpand");
      }
      set
      {
        this.SetValue("NodeExpand", value);
      }
    }

    /// <summary>
    /// This event fires when a node is dragged and dropped on an external container.
    /// </summary>
    /// <seealso cref="TreeView.ExternalDropTargets" />
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeExternalDrop
    {
      get
      {
        return this.GetValue("NodeExternalDrop");
      }
      set
      {
        this.SetValue("NodeExternalDrop", value);
      }
    }

    /// <summary>
    /// This event fires when the user highlights the node using the keyboard.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeKeyboardNavigate
    {
      get
      {
        return this.GetValue("NodeKeyboardNavigate");
      }
      set
      {
        this.SetValue("NodeKeyboardNavigate", value);
      }
    }

    /// <summary>
    /// This event fires when the user double-clicks a node.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeMouseDoubleClick
    {
      get
      {
        return this.GetValue("NodeMouseDoubleClick");
      }
      set
      {
        this.SetValue("NodeMouseDoubleClick", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse pointer leaves a node.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeMouseOut
    {
      get
      {
        return this.GetValue("NodeMouseOut");
      }
      set
      {
        this.SetValue("NodeMouseOut", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse pointer hovers over a node.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeMouseOver
    {
      get
      {
        return this.GetValue("NodeMouseOver");
      }
      set
      {
        this.SetValue("NodeMouseOver", value);
      }
    }

    /// <summary>
    /// This event fires when a node is moved.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeMove
    {
      get
      {
        return this.GetValue("NodeMove");
      }
      set
      {
        this.SetValue("NodeMove", value);
      }
    }

    /// <summary>
    /// This event fires when a node is renamed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeRename
    {
      get
      {
        return this.GetValue("NodeRename");
      }
      set
      {
        this.SetValue("NodeRename", value);
      }
    }

    /// <summary>
    /// This event fires when a node is selected.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent NodeSelect
    {
      get
      {
        return this.GetValue("NodeSelect");
      }
      set
      {
        this.SetValue("NodeSelect", value);
      }
    }

    /// <summary>
    /// This event fires when a web service call completes successfully.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent WebServiceComplete
    {
      get
      {
        return this.GetValue("WebServiceComplete");
      }
      set
      {
        this.SetValue("WebServiceComplete", value);
      }
    }
    /// <summary>
    /// This event fires when a web service call results in an error.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent WebServiceError
    {
      get
      {
        return this.GetValue("WebServiceError");
      }
      set
      {
        this.SetValue("WebServiceError", value);
      }
    }
  }
}
