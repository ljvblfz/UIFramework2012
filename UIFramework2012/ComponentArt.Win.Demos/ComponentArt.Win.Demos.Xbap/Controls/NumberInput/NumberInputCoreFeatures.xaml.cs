using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Win.Demos
{
    public partial class NumberInputCoreFeatures : UserControl, IDisposable
    {
        public NumberInputCoreFeatures()
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
    }
 }
