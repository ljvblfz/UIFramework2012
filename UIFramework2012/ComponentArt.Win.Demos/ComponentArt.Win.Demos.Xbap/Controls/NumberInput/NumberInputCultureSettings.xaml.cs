using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Win.Demos
{
    public partial class NumberInputCultureSettings : UserControl, IDisposable
    {
        public NumberInputCultureSettings()
        {
            InitializeComponent();
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            MyNumberInput.Dispose();
            MyNumberInput2.Dispose();
            MyNumberInput3.Dispose();

            GC.Collect();
        }
        #endregion

        private void CultureCombo_SelectionFinalized(object sender, ComponentArt.Win.UI.Input.ComboBoxEventArgs cbea)
        {
            if (cbea.ItemSource == null) { return; }
            MyNumberInput.CultureName = cbea.ItemSource.Id;
            MyNumberInput2.CultureName = cbea.ItemSource.Id;
            MyNumberInput3.CultureName = cbea.ItemSource.Id;
        }
    }
 }
