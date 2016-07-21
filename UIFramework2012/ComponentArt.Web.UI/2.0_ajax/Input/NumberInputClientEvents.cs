using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Client-side events of <see cref="BaseInput"/> controls.
  /// </summary>
  [TypeConverter(typeof(ClientEventsConverter))]
  public class NumberInputClientEvents : BaseInputClientEvents
  {
    /// <summary>
    /// This event fires when the <see cref="NumberInput.Value"/> property changes.
    /// </summary>
    [Description("This event fires when the NumberInput's Value property changes.")]
    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DefaultValue(null)]
    public ClientEvent ValueChanged
    {
      get
      {
        return this.GetValue("ValueChanged");
      }
      set
      {
        this.SetValue("ValueChanged", value);
      }
    }
  }
}
