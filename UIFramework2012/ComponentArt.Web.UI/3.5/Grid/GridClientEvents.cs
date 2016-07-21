using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="Grid"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class GridClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires before a callback request is made.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent BeforeCallback
    {
      get
      {
        return this.GetValue("BeforeCallback");
      }
      set
      {
        this.SetValue("BeforeCallback", value);
      }
    }

    /// <summary>
    /// This event fires when a callback request completes.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
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
    /// This event fires when a callback request results in an error on the server.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent CallbackError
    {
      get
      {
        return this.GetValue("CallbackError");
      }
      set
      {
        this.SetValue("CallbackError", value);
      }
    }

    /// <summary>
    /// This event fires when a the display order of columns is altered by the user.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ColumnReorder
    {
      get
      {
        return this.GetValue("ColumnReorder");
      }
      set
      {
        this.SetValue("ColumnReorder", value);
      }
    }

    /// <summary>
    /// This event fires when a column is resized by the user.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ColumnResize
    {
      get
      {
        return this.GetValue("ColumnResize");
      }
      set
      {
        this.SetValue("ColumnResize", value);
      }
    }

    /// <summary>
    /// This event fires when the user right-clicks a grid item.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
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
    /// This event fires when a group is collapsed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent GroupCollapse
    {
      get
      {
        return this.GetValue("GroupCollapse");
      }
      set
      {
        this.SetValue("GroupCollapse", value);
      }
    }

    /// <summary>
    /// This event fires when a group is expanded.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent GroupExpand
    {
      get
      {
        return this.GetValue("GroupExpand");
      }
      set
      {
        this.SetValue("GroupExpand", value);
      }
    }

    /// <summary>
    /// This event fires when the user causes item grouping to be changed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent GroupingChange
    {
      get
      {
        return this.GetValue("GroupingChange");
      }
      set
      {
        this.SetValue("GroupingChange", value);
      }
    }

    /// <summary>
    /// This event fires when the user clicks on a column heading cell's context menu hot spot area.
    /// </summary>
    /// <seealso cref="GridColumn.ContextMenuId"/>
    /// <seealso cref="GridColumn.ContextMenuHotSpotCssClass"/>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent HeadingContextMenu
    {
      get
      {
        return this.GetValue("HeadingContextMenu");
      }
      set
      {
        this.SetValue("HeadingContextMenu", value);
      }
    }


    /// <summary>
    /// This event fires before a checkbox cell is checked or unchecked in a grid item.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemBeforeCheckChange
    {
      get
      {
        return this.GetValue("ItemBeforeCheckChange");
      }
      set
      {
        this.SetValue("ItemBeforeCheckChange", value);
      }
    }

    /// <summary>
    /// This event fires before the user checks or unchecks a checkbox cell.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemBeforeDelete
    {
      get
      {
        return this.GetValue("ItemBeforeDelete");
      }
      set
      {
        this.SetValue("ItemBeforeDelete", value);
      }
    }

    /// <summary>
    /// This event fires before a new item is inserted.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemBeforeInsert
    {
      get
      {
        return this.GetValue("ItemBeforeInsert");
      }
      set
      {
        this.SetValue("ItemBeforeInsert", value);
      }
    }

    /// <summary>
    /// This event fires before an item is updated due to editing.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemBeforeUpdate
    {
      get
      {
        return this.GetValue("ItemBeforeUpdate");
      }
      set
      {
        this.SetValue("ItemBeforeUpdate", value);
      }
    }

    /// <summary>
    /// This event fires before an item is selected.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemBeforeSelect
    {
      get
      {
        return this.GetValue("ItemBeforeSelect");
      }
      set
      {
        this.SetValue("ItemBeforeSelect", value);
      }
    }

    /// <summary>
    /// This event fires when a checkbox cell is checked or unchecked.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemCheckChange
    {
      get
      {
        return this.GetValue("ItemCheckChange");
      }
      set
      {
        this.SetValue("ItemCheckChange", value);
      }
    }

    /// <summary>
    /// This event fires when an item is clicked.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemClick
    {
      get
      {
        return this.GetValue("ItemClick");
      }
      set
      {
        this.SetValue("ItemClick", value);
      }
    }

    /// <summary>
    /// This event fires when an expanded item in a hierarchical grid cell is collapsed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemCollapse
    {
      get
      {
        return this.GetValue("ItemCollapse");
      }
      set
      {
        this.SetValue("ItemCollapse", value);
      }
    }

    /// <summary>
    /// This event fires when an item is deleted.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemDelete
    {
      get
      {
        return this.GetValue("ItemDelete");
      }
      set
      {
        this.SetValue("ItemDelete", value);
      }
    }

    /// <summary>
    /// This event fires when an item is double-clicked.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemDoubleClick
    {
      get
      {
        return this.GetValue("ItemDoubleClick");
      }
      set
      {
        this.SetValue("ItemDoubleClick", value);
      }
    }

    /// <summary>
    /// This event fires when an expandable item in a hierarchical grid cell is expanded.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemExpand
    {
      get
      {
        return this.GetValue("ItemExpand");
      }
      set
      {
        this.SetValue("ItemExpand", value);
      }
    }

    /// <summary>
    /// This event fires when an item is dragged and dropped on an external container.
    /// </summary>
    /// <seealso cref="Grid.ItemDraggingEnabled" />
    /// <seealso cref="Grid.ExternalDropTargets" />
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemExternalDrop
    {
      get
      {
        return this.GetValue("ItemExternalDrop");
      }
      set
      {
        this.SetValue("ItemExternalDrop", value);
      }
    }

    /// <summary>
    /// This event fires when a new item is inserted.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemInsert
    {
      get
      {
        return this.GetValue("ItemInsert");
      }
      set
      {
        this.SetValue("ItemInsert", value);
      }
    }

    /// <summary>
    /// This event fires when an item is selected.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemSelect
    {
      get
      {
        return this.GetValue("ItemSelect");
      }
      set
      {
        this.SetValue("ItemSelect", value);
      }
    }

    /// <summary>
    /// This event fires when an item is unselected.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemUnSelect
    {
      get
      {
        return this.GetValue("ItemUnSelect");
      }
      set
      {
        this.SetValue("ItemUnSelect", value);
      }
    }

    /// <summary>
    /// This event fires when an item is updated.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent ItemUpdate
    {
      get
      {
        return this.GetValue("ItemUpdate");
      }
      set
      {
        this.SetValue("ItemUpdate", value);
      }
    }

    /// <summary>
    /// This event fires when the grid is done loading on the client.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
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
    /// This event fires when the page index changes.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent PageIndexChange
    {
      get
      {
        return this.GetValue("PageIndexChange");
      }
      set
      {
        this.SetValue("PageIndexChange", value);
      }
    }

    /// <summary>
    /// This event fires when the Grid is done drawing itself on the client.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent RenderComplete
    {
      get
      {
        return this.GetValue("RenderComplete");
      }
      set
      {
        this.SetValue("RenderComplete", value);
      }
    }

    /// <summary>
    /// This event fires when the Grid is scrolled.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent Scroll
    {
      get
      {
        return this.GetValue("Scroll");
      }
      set
      {
        this.SetValue("Scroll", value);
      }
    }
    
    /// <summary>
    /// This event fires when the sort order changes.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent SortChange
    {
      get
      {
        return this.GetValue("SortChange");
      }
      set
      {
        this.SetValue("SortChange", value);
      }
    }

    /// <summary>
    /// This event fires before a web service call is handled.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent WebServiceBeforeComplete
    {
      get
      {
        return this.GetValue("WebServiceBeforeComplete");
      }
      set
      {
        this.SetValue("WebServiceBeforeComplete", value);
      }
    }

    /// <summary>
    /// This event fires before a web service call is invoked.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
    public ClientEvent WebServiceBeforeInvoke
    {
      get
      {
        return this.GetValue("WebServiceBeforeInvoke");
      }
      set
      {
        this.SetValue("WebServiceBeforeInvoke", value);
      }
    }

    /// <summary>
    /// This event fires after a web service call is handled.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
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
    /// This event fires when a web service call produces an error.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue("")]
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
