using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;


namespace ComponentArt.Web.Visualization.Charting
{
  /// <summary>
  /// Client-side events of <see cref="Chart"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class ChartClientEvents : ClientEvents
  {

      /// <summary>
      /// This event fires when the range is changed on the range picker control, but before any charts are updated. 
      /// Can be used only on charts which are scroll controls. The update can be cancelled before it updates the charts.
      /// </summary>
      [Description("This event fires when the range is changed on the range picker control, but before any charts are updated.")]
      [NotifyParentProperty(true)]
      [PersistenceMode(PersistenceMode.InnerProperty)]
      [DefaultValue(null)]
      public ClientEvent BeforeRangeChange
      {
          get
          {
              return this.GetValue("BeforeRangeChange");
          }
          set
          {
              this.SetValue("BeforeRangeChange", value);
          }
      }

    /// <summary>
    /// This event fires when the viewable range of the chart changes (i.e. zoom or a scroll).
    /// </summary>
    [Description("This event fires when the viewable range of the chart changes")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent RangeChange
    {
      get
      {
          return this.GetValue("RangeChange");
      }
      set
      {
          this.SetValue("RangeChange", value);
      }
    }

    /// <summary>
    /// This event fires when the chart image is updated.
    /// </summary>
      [Description("This event fires when the chart image is updated")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ChartUpdated
    {
        get
        {
            return this.GetValue("ChartUpdated");
        }
        set
        {
            this.SetValue("ChartUpdated", value);
        }
    }

    /// <summary>
    /// This event fires when the View Angle Chooser control is finished.
    /// </summary>
    [Description("This event fires when the View Angle Chooser control is finished")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ViewAngleChooserFinished
    {
        get
        {
            return this.GetValue("ViewAngleChooserFinished");
        }
        set
        {
            this.SetValue("ViewAngleChooserFinished", value);
        }
    }

    /// <summary>
    /// This event fires when the angle is changed in a View Angle Chooser control.
    /// </summary>
      [Description("This event fires when the angle is changed in a View Angle Chooser control")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ViewAngleChooserChanged
    {
        get
        {
            return this.GetValue("ViewAngleChooserChanged");
        }
        set
        {
            this.SetValue("ViewAngleChooserChanged", value);
        }
    }


    /// <summary>
    /// This event fires when a data point is moused over on the chart image.
    /// Must be used together with Chart's RenderAreaMap and Clientside.SerializeDataPoints properties (set to true).
    /// </summary>
      [Description("Fires when a data point is moused over on the chart image")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent DataPointHover
    {
        get
        {
            return this.GetValue("DataPointHover");
        }
        set
        {
            this.SetValue("DataPointHover", value);
        }
    }


    /// <summary>
    /// This event fires when a series is moused over on the chart image.
    /// Must be used together with Chart's RenderAreaMap property (set to true).
    /// </summary>
    [Description("Fires when a series is moused over on the chart image")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent SeriesHover
    {
        get
        {
            return this.GetValue("SeriesHover");
        }
        set
        {
            this.SetValue("SeriesHover", value);
        }
    }

    /// <summary>
    /// This event fires when the mouse exits a data point on the chart image.
    /// Must be used together with Chart's RenderAreaMap and Clientside.SerializeDataPoints properties (set to true).
    /// </summary>
      [Description("Fires when the mouse exits a data point on the chart image")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent DataPointExit
    {
        get
        {
            return this.GetValue("DataPointExit");
        }
        set
        {
            this.SetValue("DataPointExit", value);
        }
    }


    /// <summary>
    /// This event fires when the mouse exits the series on the chart image.
    /// Must be used together with Chart's RenderAreaMap property (set to true).
    /// </summary>
      [Description("Fires the mouse exits the series on the chart image")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent SeriesExit
    {
        get
        {
            return this.GetValue("SeriesExit");
        }
        set
        {
            this.SetValue("SeriesExit", value);
        }
    }

  }
}
