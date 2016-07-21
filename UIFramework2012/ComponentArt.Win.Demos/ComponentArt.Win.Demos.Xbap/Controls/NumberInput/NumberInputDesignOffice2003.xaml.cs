using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Win.Demos
{
    public partial class NumberInputDesignOffice2003 : UserControl, IDisposable
    {
        public NumberInputDesignOffice2003()
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
