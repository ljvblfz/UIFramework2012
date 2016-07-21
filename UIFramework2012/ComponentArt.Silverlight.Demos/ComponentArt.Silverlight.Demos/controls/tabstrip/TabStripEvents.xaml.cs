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
    public partial class TabStripEvents : UserControl
    {
        public TabStripEvents()
        {
            InitializeComponent();
        }

        private void TabStrip_TabClick(object sender, ComponentArt.Silverlight.UI.Navigation.TabStripTabEventArgs e)
        {
            output.Text += "TabStrip '" + e.Tab.Text + "' Click\n";
        }

        private void TabStrip_TabMouseEnter(object sender, ComponentArt.Silverlight.UI.Navigation.TabStripTabEventArgs e)
        {
            overoutput.Text += "TabStrip '" + e.Tab.Text + "' MouseEnter\n";
        }

        private void TabStrip_TabMouseLeave(object sender, ComponentArt.Silverlight.UI.Navigation.TabStripTabEventArgs e)
        {
            overoutput.Text += "TabStrip '" + e.Tab.Text + "' MouseLeave\n";
        }

        private void TabStrip_TabMoved(object sender, ComponentArt.Silverlight.UI.Navigation.TabStripTabMovedEventArgs e)
        {
            output.Text += "TabStrip '" + e.Tab.Text + "' Moved " + e.OldIndex + " > " + e.NewIndex + "\n";
        }

        private void TabStripTab_Click(object sender, EventArgs e)
        {
            output.Text += "Tab '"+((TabStripTab)sender).Text + "' Click\n";
        }

        private void TabStripTab_MouseEnter(object sender, EventArgs e)
        {
            overoutput.Text += "Tab '" + ((TabStripTab)sender).Text + "' MouseEnter\n";
        }

        private void TabStripTab_MouseLeave(object sender, EventArgs e)
        {
            overoutput.Text += "Tab '" + ((TabStripTab)sender).Text + "' MouseLeave\n";
        }
    }
}
