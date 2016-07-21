using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class for defining client event handlers.
  /// </summary>
  public class ComboBoxClientEventFactory
  {
    private ComponentArt.Web.UI.ComboBoxClientEvents comboBoxClientEvents;
    /// <summary>
    /// Factory to define client event handlers.
    /// </summary>
    /// <param name="comboBoxClientEvents"></param>
    public ComboBoxClientEventFactory(ComponentArt.Web.UI.ComboBoxClientEvents comboBoxClientEvents)
    {
      this.comboBoxClientEvents = comboBoxClientEvents;
    }
    /// <summary>
    /// Fires before the selected index changes.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder BeforeChange(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      comboBoxClientEvents.BeforeChange = clientEvent;

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

      comboBoxClientEvents.CallbackComplete = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when a callback request results in an error.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder CallbackError(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      comboBoxClientEvents.CallbackError = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the selected index changes.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Change(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      comboBoxClientEvents.Change = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the dropdown is collapsed.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Collapse(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      comboBoxClientEvents.Collapse = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the dropdown is expanded.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Expand(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      comboBoxClientEvents.Expand = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the ComboBox is initialized.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Init(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      comboBoxClientEvents.Init = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
    /// <summary>
    /// Fires when the ComboBox is loaded.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <returns></returns>
    public virtual ClientEventBuilder Load(string eventHandler)
    {
      ClientEvent clientEvent = new ClientEvent();
      clientEvent.EventHandler = eventHandler;

      comboBoxClientEvents.Load = clientEvent;

      return new ClientEventBuilder(clientEvent);
    }
  }
}