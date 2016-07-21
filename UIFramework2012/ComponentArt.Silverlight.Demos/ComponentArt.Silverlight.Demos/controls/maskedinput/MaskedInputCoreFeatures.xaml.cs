using System;
using System.Windows;
using System.Windows.Controls;

namespace ComponentArt.Silverlight.Demos
{
    public partial class MaskedInputCoreFeatures : UserControl, IDisposable
    {
        public MaskedInputCoreFeatures()
        {
            InitializeComponent();
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            MyMaskedInput.Dispose();
            MyMaskedInput2.Dispose();
            MyMaskedInput3.Dispose();
            MyMaskedInput4.Dispose();
            MyMaskedInput5.Dispose();

            GC.Collect();
        }
        #endregion
    }
 }
