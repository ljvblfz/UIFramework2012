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
using ComponentArt.Silverlight.UI.Layout;

namespace ComponentArt.Silverlight.Demos
{
    public partial class WindowCoreFeatures : UserControl
    {
        public WindowCoreFeatures()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1.IsOpen = true;
        }
    }
}
