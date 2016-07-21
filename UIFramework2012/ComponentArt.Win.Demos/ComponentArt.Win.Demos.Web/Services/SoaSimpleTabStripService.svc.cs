using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace ComponentArt.Win.Demos.Web
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SoaSimpleTabStripService : ComponentArt.SOA.UI.SoaTabStripService
    {
        public override ComponentArt.SOA.UI.SoaTabStripGetTabsResponse GetTabs(ComponentArt.SOA.UI.SoaTabStripGetTabsRequest request)
        {
            ComponentArt.SOA.UI.SoaTabStripGetTabsResponse response = new ComponentArt.SOA.UI.SoaTabStripGetTabsResponse();

            response.Tabs = new List<ComponentArt.SOA.UI.SoaTabStripTab>();

            for (int i = 0; i < 5; i++)
            {
                ComponentArt.SOA.UI.SoaTabStripTab myTab = new ComponentArt.SOA.UI.SoaTabStripTab();
                myTab.Text = "Tab #"+i;
                response.Tabs.Add(myTab);
            }

            return response;
        }
    }
}
