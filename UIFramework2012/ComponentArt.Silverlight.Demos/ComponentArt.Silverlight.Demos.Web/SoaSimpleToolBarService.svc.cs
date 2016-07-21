using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace ComponentArt.Silverlight.Demos.Web
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SoaSimpleToolBarService : ComponentArt.SOA.UI.SoaToolBarService
    {
        public override ComponentArt.SOA.UI.SoaToolBarGetItemsResponse GetItems(ComponentArt.SOA.UI.SoaToolBarGetItemsRequest request)
        {
            ComponentArt.SOA.UI.SoaToolBarGetItemsResponse response = new ComponentArt.SOA.UI.SoaToolBarGetItemsResponse();

            response.Items = new List<ComponentArt.SOA.UI.SoaToolBarItem>();

            for(int i=1;i<4;i++)
            {
                ComponentArt.SOA.UI.SoaToolBarItem myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
                myItem.Text = "Button "+i;
                myItem.Height = 28;
                myItem.Width = 65;
                response.Items.Add(myItem);
            }

            ComponentArt.SOA.UI.SoaToolBarItem mySep = new ComponentArt.SOA.UI.SoaToolBarItem();
            mySep.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Separator;
            mySep.Height = 28;
            mySep.Width = 5;
            response.Items.Add(mySep);
            
            for (int i = 1; i < 4; i++)
            {
                ComponentArt.SOA.UI.SoaToolBarItem myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
                myItem.Text = "Check " + i;
                myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Check;
                myItem.Height = 28;
                myItem.Width = 65;
                response.Items.Add(myItem);
            }
            
            mySep = new ComponentArt.SOA.UI.SoaToolBarItem();
            mySep.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Separator;
            mySep.Height = 28;
            mySep.Width = 5;
            response.Items.Add(mySep);

            for (int i = 1; i < 4; i++)
            {
                ComponentArt.SOA.UI.SoaToolBarItem myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
                myItem.Text = "Radio " + i;
                myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Radio;
                myItem.GroupName = "Radio";
                myItem.Height = 28;
                myItem.Width = 60;
                response.Items.Add(myItem);
            }

            return response;
        }
    }
}
