using System;
using System.Windows.Controls;

namespace ComponentArt.SOA.Demos
{
  public partial class ComboBoxFiltering : UserControl, IDisposable
  {
      public ComboBoxFiltering()
    {
      InitializeComponent();
    }


    #region IDisposable Members

    public void Dispose()
    {
      MyComboBox.Dispose();
    }

    #endregion
  }
}
