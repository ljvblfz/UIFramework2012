using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{

  /// <summary>
  /// Client-side events of <see cref="Scheduler"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class SchedulerClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires before an appointment is modified, including recurring appointment instances/exceptions.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent AppointmentBeforeModify
    {
      get
      {
        return this.GetValue("AppointmentBeforeModify");
      }
      set
      {
        this.SetValue("AppointmentBeforeModify", value);
      }
    }

    /// <summary>
    /// This event fires before an appointment is removed, including recurring appointment instances/exceptions.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent AppointmentBeforeRemove
    {
      get
      {
        return this.GetValue("AppointmentBeforeRemove");
      }
      set
      {
        this.SetValue("AppointmentBeforeRemove", value);
      }
    }

    /// <summary>
    /// This event fires after an appointment is modified, including recurring appointment instances/exceptions.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent AppointmentModify
    {
      get
      {
        return this.GetValue("AppointmentModify");
      }
      set
      {
        this.SetValue("AppointmentModify", value);
      }
    }

    /// <summary>
    /// This event fires after an appointment is removed, including recurring appointment instances/exceptions.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent AppointmentRemove
    {
      get
      {
        return this.GetValue("AppointmentRemove");
      }
      set
      {
        this.SetValue("AppointmentRemove", value);
      }
    }

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
    /// This event fires before a webservice request is initiated.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent BeforeWebService
    {
      get
      {
        return this.GetValue("BeforeWebService");
      }
      set
      {
        this.SetValue("BeforeWebService", value);
      }
    }

    /// <summary>
    /// This event fires after a successful callback response is handled.
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
    /// This event fires when the Scheduler control is done loading on the client.
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
    /// This event fires after a successful webservice response is handled.
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
    /// This event fires when a webservice request results in an error.
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
