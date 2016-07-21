using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="NavBar"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class NavBarClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires before an expanded item is collapsed.
    /// </summary>
    [Description("This event fires before an expanded item is collapsed.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemBeforeCollapse
    {
      get
      {
        return this.GetValue("ItemBeforeCollapse");
      }
      set
      {
        this.SetValue("ItemBeforeCollapse", value);
      }
    }

    /// <summary>
    /// This event fires before an expandable item is expanded.
    /// </summary>
    [Description("This event fires before an expandable item is expanded.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemBeforeExpand
    {
      get
      {
        return this.GetValue("ItemBeforeExpand");
      }
      set
      {
        this.SetValue("ItemBeforeExpand", value);
      }
    }

    /// <summary>
    /// This event fires before an item is selected.
    /// </summary>
    [Description("This event fires before an item is selected.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemBeforeSelect
    {
      get
      {
        return this.GetValue("ItemBeforeSelect");
      }
      set
      {
        this.SetValue("ItemBeforeSelect", value);
      }
    }

    /// <summary>
    /// This event fires when an expanded item is collapsed.
    /// </summary>
    [Description("This event fires when an expanded item is collapsed.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemCollapse
    {
      get
      {
        return this.GetValue("ItemCollapse");
      }
      set
      {
        this.SetValue("ItemCollapse", value);
      }
    }

    /// <summary>
    /// This event fires when an expandable item is expanded.
    /// </summary>
    [Description("This event fires when an expandable item is expanded.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemExpand
    {
      get
      {
        return this.GetValue("ItemExpand");
      }
      set
      {
        this.SetValue("ItemExpand", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse pointer moves away from an item.
    /// </summary>
    [Description("This event fires when the mouse pointer moves away from an item.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemMouseOut
    {
      get
      {
        return this.GetValue("ItemMouseOut");
      }
      set
      {
        this.SetValue("ItemMouseOut", value);
      }
    }

    /// <summary>
    /// This event fires when the mouse pointer moves over an item.
    /// </summary>
    [Description("This event fires when the mouse pointer moves over an item.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemMouseOver
    {
      get
      {
        return this.GetValue("ItemMouseOver");
      }
      set
      {
        this.SetValue("ItemMouseOver", value);
      }
    }

    /// <summary>
    /// This event fires when an item is selected.
    /// </summary>
    [Description("This event fires when an item is selected.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ItemSelect
    {
      get
      {
        return this.GetValue("ItemSelect");
      }
      set
      {
        this.SetValue("ItemSelect", value);
      }
    }

    /// <summary>
    /// This event fires when the NavBar is done loading on the client.
    /// </summary>
    [Description("This event fires when the NavBar is done loading on the client.")]
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
