using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class for defining client event handlers for the Menu.
  /// </summary>
  public class MenuClientEventFactory
  {
    private ComponentArt.Web.UI.MenuClientEvents menuClientEvents;
    /// <summary>
    /// Factory to define client event handlers for the Menu.
    /// </summary>
    /// <param name="menuClientEvents"></param>
    public MenuClientEventFactory(ComponentArt.Web.UI.MenuClientEvents menuClientEvents)
    {
      this.menuClientEvents = menuClientEvents;
    }
    /// <summary>
    /// Called when this context menu hides.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ContextMenuHide(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.ContextMenuHide = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when this context menu shows.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ContextMenuShow(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.ContextMenuShow = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a group of items starts collapsing.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder GroupCollapseBegin(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.GroupCollapseBegin = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a group of items finishes collapsing.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder GroupCollapseEnd(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.GroupCollapseEnd = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a group of items starts expanding.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder GroupExpandBegin(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.GroupExpandBegin = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a group of items finishes expanding.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder GroupExpandEnd(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.GroupExpandEnd = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called before the checked state of an item is toggled.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeCheckChange(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.ItemBeforeCheckChange = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the mouse pointer leaves a menu item. This event fires before 
    /// the menu acts on item mouse over, and can cancel the action if the handler sets cancel to true.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeMouseOver(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.ItemBeforeMouseOver = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a menu item is selected. This event fires before the item's click action, 
    /// and can cancel the action if the handler sets cancel to true.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeSelect(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.ItemBeforeSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the check status of an item is toggled.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemCheckChange(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.ItemCheckChange = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the mouse pointer enters a menu item.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemMouseOut(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.ItemMouseOut = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the mouse pointer leaves a menu item. This event fires after the item repaint, 
    /// but before its subgroup is expanded, and can prevent the expansion if the handler sets cancel to true.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemMouseOver(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.ItemMouseOver = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when a menu item is selected. This event fires before the item's click action, 
    /// and can cancel the action if the handler sets cancel to true.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemSelect(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.ItemSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the Menu loads.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Load(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      menuClientEvents.Load = clientEvent;

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

      menuClientEvents.WebServiceComplete = clientEvent;

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

      menuClientEvents.WebServiceError = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
  }
}
