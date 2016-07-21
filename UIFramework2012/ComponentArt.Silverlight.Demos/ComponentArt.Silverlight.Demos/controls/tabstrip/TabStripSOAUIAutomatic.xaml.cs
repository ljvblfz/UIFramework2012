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

namespace ComponentArt.Silverlight.Demos
{
    public partial class TabStripSOAUIAutomatic : UserControl
    {
        public TabStripSOAUIAutomatic()
        {
            InitializeComponent();
        }

        private void TabStrip1_SoaResponseProcessed(object sender, TabStripSoaResponseEventArgs e)
        {
            foreach (TabStripTab myTab in e.Tabs)
                myTab.Padding = new Thickness(5, 3, 5, 3);
        }

        private void TabStrip1_TabClick(object sender, TabStripTabEventArgs e)
        {
            Content.Text = e.Tab.Tag.ToString();
        }
    }
}
