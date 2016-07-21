using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Silverlight.Demos
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
