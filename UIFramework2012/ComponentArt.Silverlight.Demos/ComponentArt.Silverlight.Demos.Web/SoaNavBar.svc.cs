using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

using ComponentArt.SOA.UI;

namespace ComponentArt.Silverlight.Demos.Web
{
    //[ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SoaNavBar: SoaNavBarService
    {
        public override SoaNavBarGetItemsResponse GetItems(SoaNavBarGetItemsRequest request)
        {
            SoaNavBarGetItemsResponse response = new SoaNavBarGetItemsResponse();

            Dictionary<string, List<SoaNavBarItem>> data = getData();

            // load from root
            if (request.ParentGroup == null)
            {
                foreach (string group in data.Keys)
                    response.Items.Add(new SoaNavBarItem() { Text = group, Items = data[group] });
                // show the first group
                response.Items[0].IsSelected = true;
            }
            else
            {
                if (data.ContainsKey(request.ParentGroup.Text))
                {
                    response.Items = data[request.ParentGroup.Text];
                }
            }
            response.ParentGroup = request.ParentGroup;

            return response;
        }

        private Dictionary<string, List<SoaNavBarItem>> getData()
        {
            Dictionary<string, List<SoaNavBarItem>> result = new Dictionary<string, List<SoaNavBarItem>>();

            result.Add("Mail", new List<SoaNavBarItem>());
            result["Mail"].Add(new SoaNavBarItem() { Text = "Mailbox", IconSource = "mailbox.png" });
            result["Mail"].Add(new SoaNavBarItem() { Text = "Inbox", IconSource = "inbox.png" });
            result["Mail"].Add(new SoaNavBarItem() { Text = "Drafts", IconSource = "drafts.png" });
            result["Mail"].Add(new SoaNavBarItem() { Text = "Outbox", IconSource = "outbox.png" });
            result["Mail"].Add(new SoaNavBarItem() { Text = "Junk E-mail", IconSource = "junk.png" });
            result["Mail"].Add(new SoaNavBarItem() { Text = "Deleted Items", IconSource = "deleted.png" });
            result["Mail"].Add(new SoaNavBarItem() { Text = "Search Folders", IconSource = "search.png" });
            result["Mail"].Add(new SoaNavBarItem() { Text = "Sent Items", IconSource = "sent.png" });

            result.Add("Notes", new List<SoaNavBarItem>());
            result["Notes"].Add(new SoaNavBarItem() { Text = "Icons", IconSource = "notes.png" });
            result["Notes"].Add(new SoaNavBarItem() { Text = "Note List", IconSource = "notes.png" });
            result["Notes"].Add(new SoaNavBarItem() { Text = "Last Seven Days", IconSource = "notes.png" });
            result["Notes"].Add(new SoaNavBarItem() { Text = "By Category", IconSource = "notes.png" });
            result["Notes"].Add(new SoaNavBarItem() { Text = "By Color", IconSource = "notes.png" });

            result.Add("Contacts", new List<SoaNavBarItem>());
            result["Contacts"].Add(new SoaNavBarItem() { Text = "Address Cards", IconSource = "contacts.png" });
            result["Contacts"].Add(new SoaNavBarItem() { Text = "Details Address List", IconSource = "contacts.png" });
            result["Contacts"].Add(new SoaNavBarItem() { Text = "By Category", IconSource = "contacts.png" });
            result["Contacts"].Add(new SoaNavBarItem() { Text = "By Company", IconSource = "contacts.png" });
            result["Contacts"].Add(new SoaNavBarItem() { Text = "By Follow-up Flag", IconSource = "contacts.png" });

            return result;
        }

    }
}
