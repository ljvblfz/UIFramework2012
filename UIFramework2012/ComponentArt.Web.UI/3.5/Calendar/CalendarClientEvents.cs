using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="Calendar"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class CalendarClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires after the Calendar control finishes a VisibleDate month swap.
    /// </summary>
    [Description("This event fires after the Calendar control finishes a VisibleDate month swap.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent AfterVisibleDateChanged
    {
      get
      {
        return this.GetValue("AfterVisibleDateChanged");
      }
      set
      {
        this.SetValue("AfterVisibleDateChanged", value);
      }
    }

    /// <summary>
    /// This event fires before the Calendar control finishes a VisibleDate month swap.
    /// </summary>
    [Description("This event fires before the Calendar control finishes a VisibleDate month swap.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent BeforeVisibleDateChanged
    {
      get
      {
        return this.GetValue("BeforeVisibleDateChanged");
      }
      set
      {
        this.SetValue("BeforeVisibleDateChanged", value);
      }
    }

    /// <summary>
    /// This event fires when the control is double-clicked.
    /// </summary>
    [Description("This event fires when the control is double-clicked.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent DblClick
    {
      get
      {
        return this.GetValue("DblClick");
      }
      set
      {
        this.SetValue("DblClick", value);
      }
    }

    /// <summary>
    /// This event fires when the Calendar control is done loading on the client.
    /// </summary>
    [Description("This event fires when the Calendar control is done loading on the client.")]
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
    /// This event fires when the SelectedDates collection of the Calendar changes.
    /// </summary>
    [Description("This event fires when the SelectedDates collection of the Calendar changes.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent SelectionChanged
    {
      get
      {
        return this.GetValue("SelectionChanged");
      }
      set
      {
        this.SetValue("SelectionChanged", value);
      }
    }

    /// <summary>
    /// This event fires when the VisibleDate of the Calendar changes.
    /// </summary>
    [Description("This event fires when the VisibleDate of the Calendar changes.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent VisibleDateChanged
    {
      get
      {
        return this.GetValue("VisibleDateChanged");
      }
      set
      {
        this.SetValue("VisibleDateChanged", value);
      }
    }
  }
}
