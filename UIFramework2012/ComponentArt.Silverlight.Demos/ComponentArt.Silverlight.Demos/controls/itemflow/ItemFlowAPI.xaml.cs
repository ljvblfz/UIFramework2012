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

namespace ComponentArt.Silverlight.Demos
{
    public partial class ItemFlowAPI : UserControl
    {
        private System.Windows.Threading.DispatcherTimer st;
        private bool forward = true;

        public ItemFlowAPI()
        {
            InitializeComponent();

            // autoscroll timer
            st = new System.Windows.Threading.DispatcherTimer();
            st.Interval = new TimeSpan(0, 0, 0, 2, 0);
            st.Tick += new EventHandler(st_Tick);
        }

        private void st_Tick(object sender, EventArgs e)
        {
            AutoScrollStep();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            st.Stop();
            myCheckBox.IsChecked = false;
            myItemFlow.Previous();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            st.Stop();
            myCheckBox.IsChecked = false;
            myItemFlow.Next();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myItemFlow != null)
            {
                st.Stop();
                myCheckBox.IsChecked = false;
                myItemFlow.MoveTo(((ComboBox)sender).SelectedIndex);
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (myItemFlow != null)
            {
                st.Stop();
                myCheckBox.IsChecked = false;
                myItemFlow.SelectedIndex = (((ComboBox)sender).SelectedIndex);
                myItemFlow.Initialize();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            forward = true;
            AutoScrollStep();
            st.Start();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            st.Stop();
        }

        private void AutoScrollStep()
        {
            if (forward && myItemFlow.SelectedIndex < myItemFlow.Items.Count - 1 || !forward && myItemFlow.SelectedIndex == 0)
            {
                forward = true;
                myItemFlow.Next();
            }
            else
            {
                forward = false;
                myItemFlow.Previous();
            }
        }

        private void myItemFlow_ItemClick(object sender, ComponentArt.Silverlight.UI.Navigation.ItemFlowItemEventArgs e)
        {
            st.Stop();
            myCheckBox.IsChecked = false;
        }
    }
}
