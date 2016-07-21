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
using ComponentArt.Silverlight.UI.Utils;
using ComponentArt.Silverlight.UI.Navigation;

namespace ComponentArt.Silverlight.Demos
{
    public partial class ItemFlowEvents : UserControl
    {
        public ItemFlowEvents()
        {
            InitializeComponent();
            ComponentArt.Silverlight.UI.Utils.MouseWheel.Enable(this);
        }

        private void Animate(ItemFlowItem myItem)
        {
            if (Title0.Text == myItem.Text) return;

            Title0.Text = myItem.Text;

            Storyboard oAnimation = (Storyboard)this.Resources["title_animation"];

            if (oAnimation != null 
                && oAnimation.GetCurrentState() != ClockState.Active
                )
            {
                oAnimation.Begin();
                oAnimation.Completed += new EventHandler(Animation_Completed);
            }
        }

        void Animation_Completed(object sender, EventArgs e)
        {
            Title.Text = Title0.Text;

            Storyboard oAnimation = (Storyboard)this.Resources["title_animation_reverse"];
            oAnimation.Begin();
        }

        private void myItemFlow_Flow(object sender, ItemFlowItemEventArgs e)
        {
            Animate(e.Item);
        }

        private void myItemFlow_ItemMouseEnter(object sender, ItemFlowItemEventArgs e)
        {
            Animate(e.Item);
        }

        private void myItemFlow_ItemMouseLeave(object sender, ItemFlowItemEventArgs e)
        {
            Animate(((ItemFlow)sender).Items[((ItemFlow)sender).SelectedIndex]);
        }

        private void myItemFlow_Loaded(object sender, RoutedEventArgs e)
        {
            Animate(((ItemFlow)sender).Items[((ItemFlow)sender).SelectedIndex]);
        }
    }
}
