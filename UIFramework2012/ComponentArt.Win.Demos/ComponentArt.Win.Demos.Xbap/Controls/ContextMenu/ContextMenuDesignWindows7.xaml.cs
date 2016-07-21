﻿using System;
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
using ComponentArt.Win.UI.Navigation;
using ComponentArt.Win.UI.Utils;

namespace ComponentArt.Win.Demos {
	public partial class ContextMenuDesignWindows7 : UserControl, IDisposable {
		public ContextMenuDesignWindows7() {
			InitializeComponent();
		}

		#region IDisposable Members

		void IDisposable.Dispose() {
			Windows7ContextMenu.Dispose();
		}

		#endregion
	}
}
