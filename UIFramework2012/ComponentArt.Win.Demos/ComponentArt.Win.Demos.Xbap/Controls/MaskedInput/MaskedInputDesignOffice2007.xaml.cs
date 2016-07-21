using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Win.Demos
{
    public partial class MaskedInputDesignOffice2007 : UserControl, IDisposable
    {
        public MaskedInputDesignOffice2007()
        {
            InitializeComponent();
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            MyMaskedInput.Dispose();
            GC.Collect();
        }
        #endregion
    }
 }
