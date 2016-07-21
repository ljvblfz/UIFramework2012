using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class for defining client event handlers.
  /// </summary>
  public class ToolBarClientEventFactory
  {
    private ComponentArt.Web.UI.ToolBarClientEvents toolBarClientEvents;
    /// <summary>
    /// Factory to define client event handlers.
    /// </summary>
    /// <param name="toolBarClientEvents"></param>
    public ToolBarClientEventFactory(ComponentArt.Web.UI.ToolBarClientEvents toolBarClientEvents)
    {
      this.toolBarClientEvents = toolBarClientEvents;
    }
    /// <summary>
    /// Fires when the mouse button is released over an item.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemMouseUp(string eventHandler)
    {
        ClientEvent clientEvent = new ClientEvent();
        clientEvent.EventHandler = eventHandler;

        toolBarClientEvents.ItemMouseUp = clientEvent;

        return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the mouse button is pressed over an item.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemMouseDown(string eventHandler)
    {
        ClientEvent clientEvent = new ClientEvent();
        clientEvent.EventHandler = eventHandler;

        toolBarClientEvents.ItemMouseDown = clientEvent;

        return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fired when a toolbar item is checked/unchecked.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemCheckChange(string eventHandler)
    {
        ClientEvent clientEvent = new ClientEvent();
        clientEvent.EventHandler = eventHandler;

        toolBarClientEvents.ItemCheckChange = clientEvent;

        return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires before an item command has been executed and can cancel the action.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeSelect(string eventHandler)
    {
        ClientEvent clientEvent = new ClientEvent();
        clientEvent.EventHandler = eventHandler;

        toolBarClientEvents.ItemBeforeSelect = clientEvent;

        return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fired before a toolbar item is checked/unchecked. 
    /// This event fires before the item's check action, and can cancel the action.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeCheckChange(string eventHandler)
    {
        ClientEvent clientEvent = new ClientEvent();
        clientEvent.EventHandler = eventHandler;

        toolBarClientEvents.ItemBeforeCheckChange = clientEvent;

        return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a dropdown of this toolbar shows.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder DropDownShow(string eventHandler)
    {
        ClientEvent clientEvent = new ClientEvent();
        clientEvent.EventHandler = eventHandler;

        toolBarClientEvents.DropDownShow = clientEvent;

        return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a dropdown of this toolbar hides.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder DropDownHide(string eventHandler)
    {
        ClientEvent clientEvent = new ClientEvent();
        clientEvent.EventHandler = eventHandler;

        toolBarClientEvents.DropDownHide = clientEvent;

        return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the mouse pointer leaves a toolbar item.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemMouseOut(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      toolBarClientEvents.ItemMouseOut = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the mouse pointer enters a toolbar item.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemMouseOver(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      toolBarClientEvents.ItemMouseOver = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires after an item command has been executed.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemSelect(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      toolBarClientEvents.ItemSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the control loads.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Load(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      toolBarClientEvents.Load = clientEvent;

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

      toolBarClientEvents.WebServiceComplete = clientEvent;

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

      toolBarClientEvents.WebServiceError = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
  }
}
