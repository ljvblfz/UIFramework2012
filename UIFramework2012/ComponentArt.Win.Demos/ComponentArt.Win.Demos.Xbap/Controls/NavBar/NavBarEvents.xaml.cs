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

namespace ComponentArt.Win.Demos
{
    public partial class NavBarEvents : UserControl
    {
        public NavBarEvents()
        {
            InitializeComponent();
            MyNavBar.BeforeItemSelect += new EventHandler<NavBarItemCancelEventArgs>(MyNavBar_BeforeItemSelect);
            MyNavBar.ItemSelected += new EventHandler<NavBarItemEventArgs>(MyNavBar_ItemSelected);
            MyNavBar.BeforeItemUnselect += new EventHandler<NavBarItemCancelEventArgs>(MyNavBar_BeforeItemUnselect);
            MyNavBar.ItemUnselected += new EventHandler<NavBarItemEventArgs>(MyNavBar_ItemUnselected);
            MyNavBar.BeforeGroupCollapse += new EventHandler<NavBarItemCancelEventArgs>(MyNavBar_BeforeGroupCollapse);
            MyNavBar.BeforeGroupExpand += new EventHandler<NavBarItemCancelEventArgs>(MyNavBar_BeforeGroupExpand);
            MyNavBar.GroupCollapsed+=new EventHandler<NavBarItemEventArgs>(MyNavBar_GroupCollapsed);
            MyNavBar.GroupExpanded += new EventHandler<NavBarItemEventArgs>(MyNavBar_GroupExpanded);
            MyNavBar.ItemMouseEnter += new EventHandler<NavBarItemMouseEventArgs>(MyNavBar_ItemMouseEnter);
            MyNavBar.ItemMouseLeave += new EventHandler<NavBarItemMouseEventArgs>(MyNavBar_ItemMouseLeave);
            MyNavBar.ItemMouseLeftButtonDown += new EventHandler<NavBarItemMouseEventArgs>(MyNavBar_ItemMouseLeftButtonDown);
            MyNavBar.ItemMouseLeftButtonUp += new EventHandler<NavBarItemMouseEventArgs>(MyNavBar_ItemMouseLeftButtonUp);
            MyNavBar.ItemMouseMove += new EventHandler<NavBarItemMouseEventArgs>(MyNavBar_ItemMouseMove);
        }

        private void addToList(NavBarItem item, string state)
        {
            eventList.Items.Add(string.Format("{0}: {1}", item, state));
            // select & scroll to the bottom
            eventList.Dispatcher.BeginInvoke((Action) delegate 
            {
              eventList.SelectedIndex = eventList.Items.Count - 1;
              eventList.ScrollIntoView(eventList.SelectedItem);
            });
      }

      private void Clear_Click(object sender, RoutedEventArgs e)
      {
          eventList.Items.Clear();
      }


        private string mouseMoveText;
        void MyNavBar_ItemMouseMove(object sender, NavBarItemMouseEventArgs e)
        {
            if (mouseMoveText != e.Item.ToString())
            {
                mouseMoveText = e.Item.ToString();
                mouseMove.Text = "Mouse move on " + mouseMoveText;
            }
        }

        void MyNavBar_ItemUnselected(object sender, NavBarItemEventArgs e)
        {
            addToList(e.Item, " un-selected");
        }

        void MyNavBar_BeforeItemUnselect(object sender, NavBarItemCancelEventArgs e)
        {
            addToList(e.Item, " Before un-select");
        }

        void MyNavBar_ItemSelected(object sender, NavBarItemEventArgs e)
        {
            addToList(e.Item, " selected");
        }

        void MyNavBar_BeforeItemSelect(object sender, NavBarItemCancelEventArgs e)
        {
            addToList(e.Item, " Before selected");
        }


        void MyNavBar_ItemMouseLeftButtonUp(object sender, NavBarItemMouseEventArgs e)
        {
            if (showMouseEvents.IsChecked == true)
                addToList(e.Item, "LeftButtonUp");
        }

        void MyNavBar_ItemMouseLeftButtonDown(object sender, NavBarItemMouseEventArgs e)
        {
            if (showMouseEvents.IsChecked == true)
                addToList(e.Item, "LeftButtonDown");
        }

        void MyNavBar_ItemMouseLeave(object sender, NavBarItemMouseEventArgs e)
        {
            if (showMouseEvents.IsChecked == true)
                addToList(e.Item, "MouseLeave");
        }

        void MyNavBar_ItemMouseEnter(object sender, NavBarItemMouseEventArgs e)
        {
            if (showMouseEvents.IsChecked == true)
                addToList(e.Item, "MouseEnter");
        }

        void MyNavBar_GroupExpanded(object sender, NavBarItemEventArgs e)
        {
            addToList(e.Item, "GroupExpanded");
        }

        void  MyNavBar_GroupCollapsed(object sender, NavBarItemEventArgs e)
        {
            addToList(e.Item, "GroupCollapsed");
        }

        void MyNavBar_BeforeGroupExpand(object sender, NavBarItemCancelEventArgs e)
        {
            addToList(e.Item, "BeforeGroupExpand");
        }

        void MyNavBar_BeforeGroupCollapse(object sender, NavBarItemCancelEventArgs e)
        {
            addToList(e.Item, "BeforeGroupCollapse");
        }
    }
}
