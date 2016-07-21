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
using ComponentArt.Silverlight.UI.Utils;

namespace ComponentArt.Silverlight.Demos {
	public partial class ContextMenuDesignOffice2007 : UserControl, IDisposable {
		public ContextMenuDesignOffice2007() {
			InitializeComponent();
		}

		#region IDisposable Members
		void IDisposable.Dispose() {
			Office2007Menu.Dispose();
		}

		#endregion
	}
}
