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
  public class MaskedInputClientEvents : BaseInputClientEvents
  {
  }
}
