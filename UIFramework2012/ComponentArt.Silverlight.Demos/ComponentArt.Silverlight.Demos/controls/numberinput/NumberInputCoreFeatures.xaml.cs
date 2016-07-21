using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Silverlight.Demos
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
