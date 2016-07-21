using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class SoaSimpleTabStripService : ComponentArt.SOA.UI.SoaTabStripService
{
    public string[] tabs = new string[7] { "System Restore", "Automatic Updates", "Remote", "General", "Computer Name", "Hardware", "Advanced" };

    public override ComponentArt.SOA.UI.SoaTabStripGetTabsResponse GetTabs(ComponentArt.SOA.UI.SoaTabStripGetTabsRequest request)
    {
        ComponentArt.SOA.UI.SoaTabStripGetTabsResponse response = new ComponentArt.SOA.UI.SoaTabStripGetTabsResponse();

        response.Tabs = new List<ComponentArt.SOA.UI.SoaTabStripTab>();

        foreach(string mystring in tabs)
        {
            ComponentArt.SOA.UI.SoaTabStripTab myTab = new ComponentArt.SOA.UI.SoaTabStripTab();
            myTab.Text = mystring;
            if (mystring == "System Restore") myTab.IsSelected = true;
            myTab.Tag = mystring + " Content";
            myTab.Id = "tab_" + mystring;
            response.Tabs.Add(myTab);
        }

        return response;
    }
}
