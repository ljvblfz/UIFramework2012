using System;
using System.Windows.Controls;

namespace ComponentArt.Win.Demos {
    public partial class ComboBoxSoaAutoPilot : UserControl, IDisposable
    {
        public ComboBoxSoaAutoPilot()
        {
            InitializeComponent();
            MyComboBox.SoaServiceUrl = new Uri(DemoWebHelper.BaseUri, "../Services/SoaComboBoxLocationService.svc").ToString();
        }


        #region IDisposable Members

        public void Dispose()
        {
            MyComboBox.Dispose();
        }

        #endregion
    }
}
