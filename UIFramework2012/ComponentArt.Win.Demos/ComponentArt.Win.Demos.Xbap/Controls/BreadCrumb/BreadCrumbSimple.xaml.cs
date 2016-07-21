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
    public partial class BreadCrumbSimple : UserControl
    {

        public BreadCrumbSimple()
        {
            InitializeComponent();
        }
        
        void bcDemo_ItemClick(object sender, BreadCrumbCommandEventArgs e)
        {
            click.Text = e.ItemSource.Text;
        }

        private void AppendAndSelectItem(object sender, RoutedEventArgs e)
        {
            // Create new BreadCrumbItem and set it as the SelectedItem.
            BreadCrumbItem bci = new BreadCrumbItem();
            bci.Text = "Item:"+bci.GetHashCode().ToString();
            bci.Id = bci.GetHashCode().ToString();

            // If there is no SelectedItem add the new Item to the RootItem Items Collection,
            if (bcDemo.SelectedItem == null)
            {
                bcDemo.RootItem.Items.Add(bci);
            }
            else // Otherwise add the new item to the SelectedItem Items Collection.
            {
                bcDemo.SelectedItem.Items.Add(bci);
            }
            // Select the new Item.
            bcDemo.SelectedItem = bci;
        }

        private void RemoveSelectedItem(object sender, RoutedEventArgs e)
        {
            if( bcDemo.SelectedItem != null)
            {
                // Get the Parent of the SelectedItem.
                BreadCrumbItem bci = bcDemo.SelectedItem.GetParentItem();
                // Remove the current SelectedItem from the Parent.
                bci.Items.Remove(bcDemo.SelectedItem);
                // Select the Parent Item.
                bcDemo.SelectedItem = bci;
            }
        }

        private void bcDemo_SelectionChanged(object sender, SelectionChangedEventArgs evargs)
        {
            if (bcDemo.SelectedItem != null)
            {
                path.Text = bcDemo.GetPathForItem(bcDemo.SelectedItem, " / ");
            }
        }

    }
}
