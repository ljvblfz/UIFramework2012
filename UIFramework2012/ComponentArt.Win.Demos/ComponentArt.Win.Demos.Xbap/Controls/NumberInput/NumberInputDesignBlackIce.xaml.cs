using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Win.Demos
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
