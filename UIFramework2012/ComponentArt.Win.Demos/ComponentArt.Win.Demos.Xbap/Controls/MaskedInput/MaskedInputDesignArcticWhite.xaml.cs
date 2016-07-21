using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Win.Demos
{
    public partial class MaskedInputDesignArcticWhite : UserControl, IDisposable
    {
        public MaskedInputDesignArcticWhite()
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
