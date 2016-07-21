using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="Snap"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class SnapClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires when the Snap is collapsed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent SnapCollapse
    {
      get
      {
        return this.GetValue("SnapCollapse");
      }
      set
      {
        this.SetValue("SnapCollapse", value);
      }
    }

    /// <summary>
    /// This event fires when the Snap is docked.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent SnapDock
    {
      get
      {
        return this.GetValue("SnapDock");
      }
      set
      {
        this.SetValue("SnapDock", value);
      }
    }

    /// <summary>
    /// This event fires when the Snap is expanded.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent SnapExpand
    {
      get
      {
        return this.GetValue("SnapExpand");
      }
      set
      {
        this.SetValue("SnapExpand", value);
      }
    }
  }
}
