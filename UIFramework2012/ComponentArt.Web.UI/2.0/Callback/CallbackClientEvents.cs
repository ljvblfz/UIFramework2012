using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="CallBack"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class CallBackClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires before a callback request is initiated.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
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
    /// This event fires when a callback request results in an error.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
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
    /// This event fires when the CallBack control is done loading on the client.
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
  }
}
