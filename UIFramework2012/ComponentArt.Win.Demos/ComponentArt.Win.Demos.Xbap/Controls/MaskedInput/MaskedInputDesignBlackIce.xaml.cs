using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Win.Demos
{
    public partial class MaskedInputDesignBlackIce : UserControl, IDisposable
    {
        public MaskedInputDesignBlackIce()
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
