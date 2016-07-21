using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Silverlight.Demos
{
    public partial class NumberInputDesignBlackIce : UserControl, IDisposable
    {
        public NumberInputDesignBlackIce()
        {
            InitializeComponent();
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            MyNumberInput.Dispose();
            GC.Collect();
        }
        #endregion
    }
 }
