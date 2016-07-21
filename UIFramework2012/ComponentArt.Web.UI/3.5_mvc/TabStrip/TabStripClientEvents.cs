using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="TabStrip"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class TabStripClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires when the TabStrip is done loading on the client.
    /// </summary>
    [Description("This event fires when the TabStrip is done loading on the client.")]
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
    /// This event fires before a tab is selected and can cancel the action.
    /// </summary>
    [Description("This event fires before a tab is selected and can cancel the action.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent TabBeforeSelect
    {
      get
      {
        return this.GetValue("TabBeforeSelect");
      }
      set
      {
        this.SetValue("TabBeforeSelect", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse pointer moves away from a tab.
    /// </summary>
    [Description("This event fires when the mouse pointer moves away from a tab.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent TabMouseOut
    {
      get
      {
        return this.GetValue("TabMouseOut");
      }
      set
      {
        this.SetValue("TabMouseOut", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse pointer moves over a tab.
    /// </summary>
    [Description("This event fires when the mouse pointer moves over a tab.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent TabMouseOver
    {
      get
      {
        return this.GetValue("TabMouseOver");
      }
      set
      {
        this.SetValue("TabMouseOver", value);
      }
    }

    /// <summary>
    /// This event fires when a tab is selected.
    /// </summary>
    [Description("This event fires when a tab is selected.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent TabSelect
    {
      get
      {
        return this.GetValue("TabSelect");
      }
      set
      {
        this.SetValue("TabSelect", value);
      }
    }

    /// <summary>
    /// This event fires when a web service call completes successfully.
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
    /// This event fires when a web service call results in an error.
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
