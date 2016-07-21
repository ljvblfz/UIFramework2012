using System;
using System.Windows.Controls;

namespace ComponentArt.SOA.Demos
{
  public partial class ComboBox : UserControl, IDisposable
  {
    public ComboBox()
    {
      InitializeComponent();
      Loaded += new System.Windows.RoutedEventHandler(ComboBox_Loaded);
    }

    void ComboBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        MyComboBox.Initialized += new System.Windows.RoutedEventHandler(MyComboBox_Initialized);
    }

    void MyComboBox_Initialized(object sender, System.Windows.RoutedEventArgs e)
    {
        MyComboBox.SoaResponseProcessed += new System.Windows.RoutedEventHandler(MyComboBox_SoaResponseProcessed);
    }

    void MyComboBox_SoaResponseProcessed(object sender, System.Windows.RoutedEventArgs e)
    {
        MyComboBox.OperationType = ComponentArt.Silverlight.UI.Input.ComboBoxOperationTypeMode.Items;
    }

    #region IDisposable Members

    public void Dispose()
    {
      MyComboBox.Dispose();
    }

    #endregion
  }
}
