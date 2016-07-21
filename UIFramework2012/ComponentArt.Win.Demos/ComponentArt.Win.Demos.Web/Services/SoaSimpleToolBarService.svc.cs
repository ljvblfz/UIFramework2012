using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using ComponentArt.SOA.UI;

namespace ComponentArt.Win.Demos.Web
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SoaSimpleToolBarService : SoaToolBarService
    {
        public override SoaToolBarGetItemsResponse GetItems(SoaToolBarGetItemsRequest request)
        {
            SoaToolBarGetItemsResponse response = new SoaToolBarGetItemsResponse();

            response.Items = new List<ComponentArt.SOA.UI.SoaToolBarItem>();

            for (int i = 1; i < 4; i++)
            {
                SoaToolBarItem myItem = new SoaToolBarItem();
                myItem.Text = "Button " + i;
                myItem.Height = 28;
                myItem.Width = 65;
                response.Items.Add(myItem);
            }

            SoaToolBarItem mySep = new SoaToolBarItem();
            mySep.ItemType = SoaToolBarItemType.Separator;
            mySep.Height = 28;
            mySep.Width = 5;
            response.Items.Add(mySep);

            for (int i = 1; i < 4; i++)
            {
                SoaToolBarItem myItem = new SoaToolBarItem();
                myItem.Text = "Check " + i;
                myItem.ItemType = SoaToolBarItemType.Check;
                myItem.Height = 28;
                myItem.Width = 65;
                response.Items.Add(myItem);
            }

            mySep = new SoaToolBarItem();
            mySep.ItemType = SoaToolBarItemType.Separator;
            mySep.Height = 28;
            mySep.Width = 5;
            response.Items.Add(mySep);

            for (int i = 1; i < 4; i++)
            {
                SoaToolBarItem myItem = new SoaToolBarItem();
                myItem.Text = "Radio " + i;
                myItem.ItemType = SoaToolBarItemType.Radio;
                myItem.GroupName = "Radio";
                myItem.Height = 28;
                myItem.Width = 60;
                response.Items.Add(myItem);
            }

            return response;
        }
    }
}
