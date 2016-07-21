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

namespace ComponentArt.SOA.Demos
{
    public partial class TabStrip : UserControl
    {
        public TabStrip()
        {
            InitializeComponent();
        }

        private void TabStrip1_SoaResponseProcessed(object sender, TabStripSoaResponseEventArgs e)
        {
            foreach (TabStripTab myTab in e.Tabs)
            {
                myTab.Padding = new Thickness(5, 3, 5, 3);
                myTab.Margin = new Thickness(0, 0, 1, 0);
            }
        }
    }
}
