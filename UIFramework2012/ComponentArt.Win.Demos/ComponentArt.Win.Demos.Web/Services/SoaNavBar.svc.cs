using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

using ComponentArt.SOA.UI;
using System.Web;
namespace ComponentArt.Win.Demos.Web
{
    //[ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SoaNavBar : SoaNavBarService
    {
        public override SoaNavBarGetItemsResponse GetItems(SoaNavBarGetItemsRequest request)
        {
            SoaNavBarGetItemsResponse response = new SoaNavBarGetItemsResponse();

            Dictionary<string, SoaNavBarItem> data = getData();

            // load from root
            if (request.ParentGroup == null)
            {
                foreach (string group in data.Keys)
                    response.Items.Add(data[group]);
                // show the first group
                response.Items[0].IsSelected = true;
            }
            else
            {
                if (data.ContainsKey(request.ParentGroup.Text))
                {
                    response.Items = data[request.ParentGroup.Text].Items;
                }
            }
            response.ParentGroup = request.ParentGroup;

            return response;
        }

        private string _serverPath
        {
            get
            {
                return new Uri(HttpContext.Current.Request.Url, IMG_PATH).AbsoluteUri;
            }
        }
        private const string IMG_PATH = "../ClientBin/controls/navbar/small/";

        private Dictionary<string, SoaNavBarItem> getData()
        {
            Dictionary<string, SoaNavBarItem> result = new Dictionary<string, SoaNavBarItem>();

            SoaNavBarItem curr_group = new SoaNavBarItem() { Text = "Mail", IconSource = _serverPath + "24x24-mail.png", Items = new List<SoaNavBarItem>() };
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Mailbox", IconSource = _serverPath + "mailbox.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Inbox", IconSource = _serverPath + "inbox.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Drafts", IconSource = _serverPath + "drafts.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Outbox", IconSource = _serverPath + "outbox.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Junk E-mail", IconSource = _serverPath + "junk.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Deleted Items", IconSource = _serverPath + "deleted.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Search Folders", IconSource = _serverPath + "search.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Sent Items", IconSource = _serverPath + "sent.png" });
            result.Add("Mail", curr_group);

            curr_group = new SoaNavBarItem() { Text = "Notes", IconSource = _serverPath + "24x24-notes.png", Items = new List<SoaNavBarItem>() };
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Icons", IconSource = _serverPath + "notes.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Note List", IconSource = _serverPath + "notes.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Last Seven Days", IconSource = _serverPath + "notes.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "By Category", IconSource = _serverPath + "notes.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "By Color", IconSource = _serverPath + "notes.png" });
            result.Add("Notes", curr_group);

            curr_group = new SoaNavBarItem() { Text = "Contacts", IconSource = _serverPath + "24x24-contacts.png", Items = new List<SoaNavBarItem>() };
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Address Cards", IconSource = _serverPath + "contacts.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "Details Address List", IconSource = _serverPath + "contacts.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "By Category", IconSource = _serverPath + "contacts.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "By Company", IconSource = _serverPath + "contacts.png" });
            curr_group.Items.Add(new SoaNavBarItem() { Text = "By Follow-up Flag", IconSource = _serverPath + "contacts.png" });
            result.Add("Contacts", curr_group);

            return result;
        }

    }
}
