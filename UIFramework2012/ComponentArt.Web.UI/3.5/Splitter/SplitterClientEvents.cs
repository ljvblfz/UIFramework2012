using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="Splitter"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class SplitterClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires when the Splitter is done loading on the client.
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
    /// This event fires when the user is done resizing a pane in this Splitter.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ResizeEnd
    {
      get
      {
        return this.GetValue("ResizeEnd");
      }
      set
      {
        this.SetValue("ResizeEnd", value);
      }
    }

    /// <summary>
    /// This event fires when the user starts resizing a pane in this Splitter.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ResizeStart
    {
      get
      {
        return this.GetValue("ResizeStart");
      }
      set
      {
        this.SetValue("ResizeStart", value);
      }
    }
  }
}
