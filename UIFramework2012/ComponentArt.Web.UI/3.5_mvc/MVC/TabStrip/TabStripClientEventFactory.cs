using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class for defining client event handlers.
  /// </summary>
  public class TabStripClientEventFactory
  {
    private ComponentArt.Web.UI.TabStripClientEvents tabStripClientEvents;
    /// <summary>
    /// Factory to define client event handlers.
    /// </summary>
    /// <param name="tabStripClientEvents"></param>
    public TabStripClientEventFactory(ComponentArt.Web.UI.TabStripClientEvents tabStripClientEvents)
    {
        this.tabStripClientEvents = tabStripClientEvents;
    }

/// <summary>
/// Fires before a tab is selected.
/// </summary>
/// <param name="eventHandler"></param>
/// <returns></returns>
    public virtual ClientEventBuilder TabBeforeSelect(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      tabStripClientEvents.TabBeforeSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the mouse pointer enters a tab.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder TabMouseOut(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      tabStripClientEvents.TabMouseOut = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the mouse pointer leaves a tab.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder TabMouseOver(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      tabStripClientEvents.TabMouseOver = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// This event fires when a tab is selected.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder TabSelect(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      tabStripClientEvents.TabSelect = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the TabStrip loads.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Load(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      tabStripClientEvents.Load = clientEvent;

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

      tabStripClientEvents.WebServiceComplete = clientEvent;

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

      tabStripClientEvents.WebServiceError = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
  }
}