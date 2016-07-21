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
using ComponentArt.Win.UI.Navigation;
using System.ComponentModel;

namespace ComponentArt.Win.Demos 
{
    public partial class ToolBarEvents : UserControl, IDisposable
    {
        public ToolBarEvents() 
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }


        private void ToolBarItem_Click(object sender, EventArgs e)
        {
            output.Text = (((ToolBarItem)sender).Name) + " Click\n" + output.Text;
            output_viewer.ScrollToVerticalOffset(0);
        }

        private void Item_Checked(object sender, EventArgs e)
        {
            output.Text = (((ToolBarItem)sender).Name) + " Checked\n" + output.Text;
            output_viewer.ScrollToVerticalOffset(0);
        }

        private void Item_Unchecked(object sender, EventArgs e)
        {
            output.Text = (((ToolBarItem)sender).Name) + " Unchecked\n" + output.Text;
            output_viewer.ScrollToVerticalOffset(0);
        }

        private void New_DropDownClick(object sender, EventArgs e)
        {
            output.Text = (((ToolBarItem)sender).Name) + " DropDown Click\n" + output.Text;
            output_viewer.ScrollToVerticalOffset(0);
        }

        private void New_MenuClick(object sender, MenuCommandEventArgs e)
        {
            output.Text = (((ToolBarItem)sender).Name) + " MenuItem \"" + e.ItemSource.Text.ToString() + "\" Click\n" + output.Text;
            output_viewer.ScrollToVerticalOffset(0);
        }

        private void ToolBar1_Click(object sender, ToolBarItemEventArgs e)
        {
            output.Text = e.Item.Name + " ToolBar Click\n" + output.Text;
        }

        private void Item_MouseEnter(object sender, EventArgs e)
        {
            overoutput.Text = (((ToolBarItem)sender).Name) + " MouseEnter\n" + overoutput.Text;
            overoutput_viewer.ScrollToVerticalOffset(0);
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
