using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class for defining client event handlers.
  /// </summary>
  public class DataGridClientEventFactory
  {
    private ComponentArt.Web.UI.GridClientEvents gridClientEvents;
    /// <summary>
    /// Factory to define client event handlers for the DataGrid.
    /// </summary>
    /// <param name="gridClientEvents"></param>
    public DataGridClientEventFactory(ComponentArt.Web.UI.GridClientEvents gridClientEvents)
    {
      this.gridClientEvents = gridClientEvents;
    }
    /// <summary>
    /// Fires before a callback update is initiated.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder BeforeCallback(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.BeforeCallback = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when a callback request completes.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder CallbackComplete(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.CallbackComplete = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an error occurs in performing a callback.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder CallbackError(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.CallbackError = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when a column's order is about to be changed.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ColumnReorder(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ColumnReorder = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when a column is about to be resized.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ColumnResize(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ColumnResize = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the Grid is right-clicked.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ContextMenu(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ContextMenu = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when a group of items is collapsed.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder GroupCollapse(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.GroupCollapse = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when a group of items is expanded.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder GroupExpand(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.GroupExpand = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when data is about to be grouped.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder GroupingChange(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.GroupingChange = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when a column header is right-clicked.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder HeadingContextMenu(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.HeadingContextMenu = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when a checkbox cell is about to be checked or unchecked.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeCheckChange(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemBeforeCheckChange = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is about to be deleted.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeDelete(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemBeforeDelete = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is about to be inserted.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeInsert(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemBeforeInsert = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires before an item is selected.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeSelect(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemBeforeSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is about to be updated.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeUpdate(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemBeforeUpdate = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when a checkbox cell is checked or unchecked.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemCheckChange(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemCheckChange = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item (row) is clicked.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemClick(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemClick = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an expanded parent item is collapsed in a hierarchical structure.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemCollapse(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemCollapse = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is deleted.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemDelete(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemDelete = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is double-clicked.
    /// Note that EditOnClickSelectedItem can sometimes consume clicks meant for this event
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemDoubleClick(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemDoubleClick = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an expandable parent item is expanded in a hierarchical structure.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemExpand(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemExpand = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is dragged and dropped onto an external target.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemExternalDrop(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemExternalDrop = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is inserted.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemInsert(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemInsert = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is selected.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemSelect(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is unselected.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemUnSelect(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemUnSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item is updated.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemUpdate(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.ItemUpdate = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the Grid is done loading and is ready for interaction on the client.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Load(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.Load = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the page index is about to be changed.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder PageIndexChange(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.PageIndexChange = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the Grid is done rendering.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder RenderComplete(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.RenderComplete = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the Grid is about to be scrolled.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Scroll(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.Scroll = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the Grid is about to be sorted.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder SortChange(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.SortChange = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a web service request completes.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder WebServiceBeforeComplete(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.WebServiceBeforeComplete = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called before a web service request is sent.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder WebServiceBeforeInvoke(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.WebServiceBeforeInvoke = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a web service request completes.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder WebServiceComplete(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.WebServiceComplete = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a web service request results in an error.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder WebServiceError(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      gridClientEvents.WebServiceError = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
  }
}
