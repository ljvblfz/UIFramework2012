using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Silverlight.Demos
{
    public partial class NumberInputDesignOffice2007 : UserControl, IDisposable
    {
        public NumberInputDesignOffice2007()
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
