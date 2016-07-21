using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.Visualization.Gauges
{
  /// <summary>
  /// Represents a set of client-side events for a <see cref="WebGauge"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class GaugeClientEvents : ClientEvents
  { 
    /// <summary>
    /// This event fires when the WebGauge control has been initialized on the ASP.NET page. 
    /// </summary>
    [Description("Fires when the WebGauge control has been initialized on the ASP.NET page")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent Initialized
    {
      get
      {
        return this.GetValue("Initialized");
      }
      set
      {
        this.SetValue("Initialized", value);
      }
    }

    /// <summary>
    /// This event fires before the WebGauge control is updated from the server. This event can be cancelled. 
    /// </summary>
    [Description("Fires before the WebGauge control is updated from the server. This event can be cancelled.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent BeforeGaugeUpdate
    {
      get
      {
        return this.GetValue("BeforeGaugeUpdate");
      }
      set
      {
        this.SetValue("BeforeGaugeUpdate", value);
      }
    }

    /// <summary>
    /// This event fires after the WebGauge control has been updated from the server. 
    /// </summary>
    [Description("Fires after the WebGauge control has been updated from the server")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GaugeUpdated
    {
      get
      {
        return this.GetValue("GaugeUpdated");
      }
      set
      {
        this.SetValue("GaugeUpdated", value);
      }
    }

    /// <summary>
    /// This event fires if an error occurs when the WebGauge control is updated from the server. 
    /// </summary>
    [Description("Fires if an error occurs when the WebGauge control is updated from the server. ")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GaugeUpdateError
    {
      get
      {
        return this.GetValue("GaugeUpdateError");
      }
      set
      {
        this.SetValue("GaugeUpdateError", value);
      }
    }

    /// <summary>
    /// This event fires when a gauge, or sub-gauge, is modified.
    /// </summary>
    [Description("Fires when a gauge, or sub-gauge, is modified.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GaugeChanged
    {
      get
      {
        return this.GetValue("GaugeChanged");
      }
      set
      {
        this.SetValue("GaugeChanged", value);
      }
    }

    /// <summary>
    /// This event fires when mouse is clicked on a gauge, or sub-gauge, in the gauge's image when WebGauge's RenderAreaMap is set to true. 
    /// </summary>
    [Description("Fires when mouse is clicked on a gauge, or sub-gauge")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GaugeClick
    {
      get
      {
        return this.GetValue("GaugeClick");
      }
      set
      {
        this.SetValue("GaugeClick", value);
      }
    }

    /// <summary>
    /// This event fires when mouse exits a guage, or sub-gauge, in the gauge's image when WebGauge's RenderAreaMap is set to true. 
    /// </summary>
    [Description("Fires when mouse exits a guage, or sub-gauge")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GaugeExit
    {
      get
      {
        return this.GetValue("GaugeExit");
      }
      set
      {
        this.SetValue("GaugeExit", value);
      }
    }

    /// <summary>
    /// This event fires on mouse over a gauge, or sub-gauge, in the gauge's image when WebGauge's RenderAreaMap is set to true. 
    /// </summary>
    [Description("Fires on mouse over a gauge, or sub-gauge")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent GaugeHover
    {
      get
      {
        return this.GetValue("GaugeHover");
      }
      set
      {
        this.SetValue("GaugeHover", value);
      }
    }
    
    /// <summary>
    /// This event fires when a Scale within the WebGauge control is modified. 
    /// </summary>
    [Description("Fires when a Scale within the WebGauge control is modified")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ScaleChanged
    {
      get
      {
        return this.GetValue("ScaleChanged");
      }
      set
      {
        this.SetValue("ScaleChanged", value);
      }
    }

    /// <summary>
    /// This event fires when a Range within the WebGauge control is modified.
    /// </summary>
    [Description("Fires when a Range within the WebGauge control is modified")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent RangeChanged
    {
      get
      {
        return this.GetValue("RangeChanged");
      }
      set
      {
        this.SetValue("RangeChanged", value);
      }
    }

    /// <summary>
    /// This event fires when an Indicator within the WebGauge control is modified.
    /// </summary>
    [Description("Fires when an Indicator within the WebGauge control is modified")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent IndicatorChanged
    {
      get
      {
        return this.GetValue("IndicatorChanged");
      }
      set
      {
        this.SetValue("IndicatorChanged", value);
      }
    }

    /// <summary>
    /// This event fires when mouse is clicked on an indicator in the gauge's image when WebGauge's RenderAreaMap is set to true.
    /// </summary>
    [Description("Fires when mouse is clicked on an indicator")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent IndicatorClick
    {
      get
      {
        return this.GetValue("IndicatorClick");
      }
      set
      {
        this.SetValue("IndicatorClick", value);
      }
    }

    /// <summary>
    /// This event fires when mouse exits an indicator in the gauge's image when WebGauge's RenderAreaMap is set to true.
    /// </summary>
    [Description("Fires when mouse exits an indicator")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent IndicatorExit
    {
      get
      {
        return this.GetValue("IndicatorExit");
      }
      set
      {
        this.SetValue("IndicatorExit", value);
      }
    }

    /// <summary>
    /// This event fires on mouse over an indicator in the gauge's image when WebGauge's RenderAreaMap is set to true.
    /// </summary>
    [Description("Fires on mouse over an indicator")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent IndicatorHover
    {
      get
      {
        return this.GetValue("IndicatorHover");
      }
      set
      {
        this.SetValue("IndicatorHover", value);
      }
    }
    
    /// <summary>
    /// This event fires when a Pointer within the WebGauge control is modified.
    /// </summary>
    [Description("Fires when a Pointer within the WebGauge control is modified")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PointerChanged
    {
      get
      {
        return this.GetValue("PointerChanged");
      }
      set
      {
        this.SetValue("PointerChanged", value);
      }
    }

    /// <summary>
    /// This event fires when mouse is clicked on a pointer in the gauge's image when WebGauge's RenderAreaMap is set to true.
    /// </summary>
    [Description("Fires when mouse is clicked on a pointer")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PointerClick
    {
      get
      {
        return this.GetValue("PointerClick");
      }
      set
      {
        this.SetValue("PointerClick", value);
      }
    }

    /// <summary>
    /// This event fires when mouse exits a pointer in the gauge's image when WebGauge's RenderAreaMap is set to true.
    /// </summary>
    [Description("Fires when mouse exits a pointer")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PointerExit
    {
      get
      {
        return this.GetValue("PointerExit");
      }
      set
      {
        this.SetValue("PointerExit", value);
      }
    }

    /// <summary>
    /// This event fires on mouse over a pointer in the gauge's image when WebGauge's RenderAreaMap is set to true.
    /// </summary>
    [Description("Fires on mouse over a pointer")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PointerHover
    {
      get
      {
        return this.GetValue("PointerHover");
      }
      set
      {
        this.SetValue("PointerHover", value);
      }
    }
    
  }
  
}
