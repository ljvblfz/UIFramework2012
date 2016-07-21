using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ComponentArt.Silverlight.UI.Navigation;
using System.ComponentModel;

namespace ComponentArt.Silverlight.Demos {
	public partial class MenuDesignWindows7 : UserControl, IDisposable
    {
		public MenuDesignWindows7() {
			InitializeComponent();
		}

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
		}

        #region IDisposable Members

        public void Dispose()
        {
            Windows7Menu.Dispose();
        }

        #endregion
    }
}
