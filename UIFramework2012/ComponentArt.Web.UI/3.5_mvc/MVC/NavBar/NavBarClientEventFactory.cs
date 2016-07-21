using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class for defining client event handlers.
  /// </summary>
  public class NavBarClientEventFactory
  {
    private ComponentArt.Web.UI.NavBarClientEvents navBarClientEvents;
    /// <summary>
    /// Factory to define client event handlers.
    /// </summary>
    /// <param name="navBarClientEvents"></param>
    public NavBarClientEventFactory(ComponentArt.Web.UI.NavBarClientEvents navBarClientEvents)
    {
      this.navBarClientEvents = navBarClientEvents;
    }
    /// <summary>
    /// Fires before an item collapses.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeCollapse(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      navBarClientEvents.ItemBeforeCollapse = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires before an item expands.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemBeforeExpand(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      navBarClientEvents.ItemBeforeExpand = clientEvent;

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

      navBarClientEvents.ItemBeforeSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item collapses.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemCollapse(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      navBarClientEvents.ItemCollapse = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when an item expands.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemExpand(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      navBarClientEvents.ItemExpand = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the mouse pointer moves away from an item.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemMouseOut(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      navBarClientEvents.ItemMouseOut = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the mouse pointer hovers over an item.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder ItemMouseOver(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      navBarClientEvents.ItemMouseOver = clientEvent;

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

      navBarClientEvents.ItemSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the NavBar loads.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Load(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      navBarClientEvents.Load = clientEvent;

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

      navBarClientEvents.WebServiceComplete = clientEvent;

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

      navBarClientEvents.WebServiceError = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
  }
}
