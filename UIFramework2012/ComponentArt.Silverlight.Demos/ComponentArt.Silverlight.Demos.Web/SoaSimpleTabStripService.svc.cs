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
    public class SoaSimpleTabStripService : ComponentArt.SOA.UI.SoaTabStripService
    {
        public override ComponentArt.SOA.UI.SoaTabStripGetTabsResponse GetTabs(ComponentArt.SOA.UI.SoaTabStripGetTabsRequest request)
        {
            ComponentArt.SOA.UI.SoaTabStripGetTabsResponse response = new ComponentArt.SOA.UI.SoaTabStripGetTabsResponse();

            response.Tabs = new List<ComponentArt.SOA.UI.SoaTabStripTab>();

            DirectoryInfo myDirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/source/controls/tabstrip"));

            foreach (FileInfo myFile in myDirInfo.GetFiles())
            {
                ComponentArt.SOA.UI.SoaTabStripTab myTab = new ComponentArt.SOA.UI.SoaTabStripTab();
                myTab.Text = myFile.Name;
                myTab.Tag = readFile(HttpContext.Current.Server.MapPath("~/source/controls/tabstrip/" + myFile.Name));
                response.Tabs.Add(myTab);
            }

            return response;
        }

        protected string readFile(string file)
        {
            HttpContext context = HttpContext.Current;
            string fileText = "";

            StreamReader srRead = File.OpenText(file);
            srRead.BaseStream.Seek(0, SeekOrigin.Begin);

            while (srRead.Peek() > -1)
            {
                fileText += srRead.ReadLine() + "\n";
            }
            srRead.Close();
          

            return fileText;
        }
    }
}
