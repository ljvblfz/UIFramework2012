using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="SplitterPane"/> class.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class SplitterPaneClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires before the pane is collapsed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PaneBeforeCollapse
    {
      get
      {
        return this.GetValue("PaneBeforeCollapse");
      }
      set
      {
        this.SetValue("PaneBeforeCollapse", value);
      }
    }

    /// <summary>
    /// This event fires before the pane is expanded.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PaneBeforeExpand
    {
      get
      {
        return this.GetValue("PaneBeforeExpand");
      }
      set
      {
        this.SetValue("PaneBeforeExpand", value);
      }
    }

    /// <summary>
    /// This event fires before the pane is resized.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PaneBeforeResize
    {
      get
      {
        return this.GetValue("PaneBeforeResize");
      }
      set
      {
        this.SetValue("PaneBeforeResize", value);
      }
    }

    /// <summary>
    /// This event fires when the pane is collapsed.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PaneCollapse
    {
      get
      {
        return this.GetValue("PaneCollapse");
      }
      set
      {
        this.SetValue("PaneCollapse", value);
      }
    }

    /// <summary>
    /// This event fires when the pane is expanded.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PaneExpand
    {
      get
      {
        return this.GetValue("PaneExpand");
      }
      set
      {
        this.SetValue("PaneExpand", value);
      }
    }

    /// <summary>
    /// This event fires when the pane is resized.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PaneResize
    {
      get
      {
        return this.GetValue("PaneResize");
      }
      set
      {
        this.SetValue("PaneResize", value);
      }
    }

    /// <summary>
    /// This event fires when a pane resize step occurs.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent PaneResizeStep
    {
      get
      {
        return this.GetValue("PaneResizeStep");
      }
      set
      {
        this.SetValue("PaneResizeStep", value);
      }
    }
  }
}
