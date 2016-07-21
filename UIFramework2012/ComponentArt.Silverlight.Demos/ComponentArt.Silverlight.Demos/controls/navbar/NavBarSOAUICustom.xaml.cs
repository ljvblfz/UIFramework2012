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
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using ComponentArt.SOA.UI;

namespace ComponentArt.Silverlight.Demos
{
    public partial class NavBarSOAUICustom : UserControl
    {
        public NavBarSOAUICustom()
        {
            InitializeComponent();
        }

        private void MyNavBar_LoadOnDemand(object sender, NavBarItemCancelLoadOnDemandEventArgs e)
        {
            NavBarService.SoaNavBarServiceClient soaClient = new NavBarService.SoaNavBarServiceClient("BasicHttpBinding_ISoaNavBarService",
                new Uri(Application.Current.Host.Source, "../SoaNavBar.svc").AbsoluteUri);

            soaClient.GetItemsCompleted +=new EventHandler<ComponentArt.Silverlight.Demos.NavBarService.GetItemsCompletedEventArgs>(soaClient_GetItemsCompleted);
            soaClient.GetItemsAsync(e.CreateRequest(), e.Item);
        }

        void soaClient_GetItemsCompleted(object sender, ComponentArt.Silverlight.Demos.NavBarService.GetItemsCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.Items != null)
            {
                BaseNavBarItem parentNode = (e.UserState == null) ? MyNavBar as BaseNavBarItem : e.UserState as BaseNavBarItem;
                // groups
                foreach (SoaNavBarItem item in e.Result.Items)
                {
                    NavBarItem newItem = new NavBarItem() { SoaData = item };
                    parentNode.NavBarItems.Add(newItem);

                    // group items
                    if (item.Items != null && item.Items.Count > 0)
                    {
                        foreach (SoaNavBarItem subItem in item.Items)
                        {
                            newItem.NavBarItems.Add(new NavBarItem() { SoaData = subItem });
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
