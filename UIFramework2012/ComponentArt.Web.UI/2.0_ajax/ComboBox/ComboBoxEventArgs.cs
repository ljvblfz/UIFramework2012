using System;
using System.Text;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Arguments for <see cref="ComboBox.DataRequested"/> server-side event of <see cref="ComboBox"/> control.
  /// </summary>
  public class ComboBoxDataRequestedEventArgs : EventArgs
  {
    public string Filter;

    public int StartIndex;
    public int NumItems;

    public ComboBoxDataRequestedEventArgs()
    {
    }
  }
}
