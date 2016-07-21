using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Win.Demos
{
    public partial class NumberInputDesignArcticWhite : UserControl, IDisposable
    {
        public NumberInputDesignArcticWhite()
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
