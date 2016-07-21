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
	public partial class BreadCrumbDesignOffice2003 : UserControl, IDisposable
    {
        public BreadCrumbDesignOffice2003()
        {
			InitializeComponent();
		}

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
		}

        #region IDisposable Members

        public void Dispose()
        {
            bcDemo.Dispose();
        }

        #endregion
    }
}
