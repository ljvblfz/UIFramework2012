using System;
using System.Windows.Controls;


namespace ComponentArt.Win.Demos
{

    public partial class ComboBoxCodingXml : UserControl, IDisposable
    {
        public ComboBoxCodingXml()
        {
            InitializeComponent();
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            MyComboBox.Dispose();
        }
        #endregion
    }
}
