using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="SpellCheck"/> control.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class SpellCheckClientEvents : ClientEvents
  {
    /// <summary>
    /// This event fires when the SpellCheck dialog process requires the dialog to be updated.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent DialogUpdateNeeded
    {
      get
      {
        return this.GetValue("DialogUpdateNeeded");
      }
      set
      {
        this.SetValue("DialogUpdateNeeded", value);
      }
    }

    /// <summary>
    /// This event fires when the SpellCheck dialog process completes.
    /// </summary>
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent DialogComplete
    {
      get
      {
        return this.GetValue("DialogComplete");
      }
      set
      {
        this.SetValue("DialogComplete", value);
      }
    }

    /// <summary>
    /// This event fires when the SpellCheck control is loaded on the client.
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
