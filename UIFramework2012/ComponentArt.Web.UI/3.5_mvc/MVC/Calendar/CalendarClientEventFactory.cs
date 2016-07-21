using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  public class CalendarClientEventFactory
  {
    private ComponentArt.Web.UI.CalendarClientEvents calendarClientEvents;
    /// <summary>
    /// Factory to define client event handlers for the Calendar.
    /// </summary>
    /// <param name="calendarClientEvents"></param>
    public CalendarClientEventFactory(ComponentArt.Web.UI.CalendarClientEvents calendarClientEvents)
    {
      this.calendarClientEvents = calendarClientEvents;
    }
    /// <summary>
    /// Called after the Calendar control finishes a VisibleDate month swap.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder AfterVisibleDateChanged(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      calendarClientEvents.AfterVisibleDateChanged = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called before the Calendar control starts a VisibleDate month swap.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder BeforeVisibleDateChanged(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      calendarClientEvents.BeforeVisibleDateChanged = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the Calendar loads.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Load(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      calendarClientEvents.Load = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the SelectedDates collection of the calendar changes.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder SelectionChanged(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      calendarClientEvents.SelectionChanged = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the VisibleDate of the calendar changes.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder VisibleDateChanged(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      calendarClientEvents.VisibleDateChanged = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Called when the Calendar detects a mouse double-click on itself.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder DblClick(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      calendarClientEvents.DblClick = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
  }
}
