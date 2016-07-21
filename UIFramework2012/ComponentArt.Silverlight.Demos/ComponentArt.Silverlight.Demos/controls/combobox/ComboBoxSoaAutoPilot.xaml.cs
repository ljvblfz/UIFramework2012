using System;
using System.Windows.Controls;

namespace ComponentArt.Silverlight.Demos {
    public partial class ComboBoxSoaAutoPilot : UserControl, IDisposable
    {
        public ComboBoxSoaAutoPilot()
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
