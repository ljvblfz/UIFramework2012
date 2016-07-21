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
using System.Windows.Media.Imaging;
using System.Windows.Data;

namespace ComponentArt.Win.Demos
{
    public partial class NavBarLoadOnDemandFeatures : UserControl
    {
        public NavBarLoadOnDemandFeatures()
        {
            InitializeComponent();
        }

        private void MyNavBar_LoadOnDemand(object sender, ComponentArt.Win.UI.Navigation.NavBarItemCancelLoadOnDemandEventArgs e)
        {
            Uri service = new Uri(DemoWebHelper.BaseUri, "../Services/ManualNavBarService.svc");
            ManualNavBarService.ManualNavBarServiceClient wc = new ManualNavBarService.ManualNavBarServiceClient
                ("BasicHttpBinding_ManualNavBarService", service.AbsoluteUri);
            wc.GetItemsCompleted += new EventHandler<ComponentArt.Win.Demos.ManualNavBarService.GetItemsCompletedEventArgs>(wc_GetItemsCompleted);
            string path = "";
            if (e.Item != null) path = e.Item.Text;
            wc.GetItemsAsync(path, e.Item);

        }

        void wc_GetItemsCompleted(object sender, ComponentArt.Win.Demos.ManualNavBarService.GetItemsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                BaseNavBarItem parentNode = (e.UserState == null) ? MyNavBar as BaseNavBarItem : e.UserState as BaseNavBarItem;
                // groups
                foreach (ManualNavBarService.MyNavBarData item in e.Result)
                {
                    NavBarItem newItem = new NavBarItem() { Header = item.Header, IsLoadOnDemandEnabled = item.IsLoadOnDemandEnabled, Id = item.Id };
                    if (!(string.IsNullOrEmpty(item.IconSource)))
                        newItem.SetBinding(NavBarItem.IconSourceProperty, new Binding() { Source = item.IconSource });

                    parentNode.NavBarItems.Add(newItem);

                    // group items
                    if (item.Items != null && item.Items.Length > 0)
                    {
                        foreach (ManualNavBarService.MyNavBarData subItem in item.Items)
                        {
                            NavBarItem newSubItem = new NavBarItem() { Header = subItem.Header, Id = subItem.Id };
                            if (!(string.IsNullOrEmpty(subItem.IconSource)))
                                newSubItem.SetBinding(NavBarItem.IconSourceProperty, new Binding() { Source = subItem.IconSource });
                            newItem.NavBarItems.Add(newSubItem);
                        }
                    }
                    parentNode.IsLoadOnDemandCompleted = true;
                    if (parentNode is NavBarItem)
                    {
                        (parentNode as NavBarItem).IsSelected = true;
                    }
                    else if (parentNode.NavBarItems != null && parentNode.NavBarItems.Count > 0)
                    {
                        parentNode.NavBarItems[0].IsSelected = true;
                    }
                }
            }
        }
    }
}
