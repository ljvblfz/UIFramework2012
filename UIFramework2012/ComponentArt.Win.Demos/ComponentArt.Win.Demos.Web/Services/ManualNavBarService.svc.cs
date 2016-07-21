using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ComponentArt.Win.Demos.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ManualNavBarService
    {
        [OperationContract]
        public List<MyNavBarData> GetItems(string parentGroup)
        {

            List<MyNavBarData> response = new List<MyNavBarData>();

            Dictionary<string, MyNavBarData> data = getData();

            // load from root
            if (string.IsNullOrEmpty(parentGroup))
            {
                foreach (string group in data.Keys)
                    response.Add(data[group]);
            }
            else
            {
                if (data.ContainsKey(parentGroup))
                {
                    response.Add(data[parentGroup]);
                }
            }

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


        private Dictionary<string, MyNavBarData> getData()
        {
            Dictionary<string, MyNavBarData> result = new Dictionary<string, MyNavBarData>();

            MyNavBarData curr_group = new MyNavBarData() { Header = "Mail", IconSource = _serverPath + "24x24-mail.png", Items = new List<MyNavBarData>() };
            curr_group.Items.Add(new MyNavBarData() { Header = "Mailbox", IconSource = _serverPath + "mailbox.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Inbox", IconSource = _serverPath + "inbox.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Drafts", IconSource = _serverPath + "drafts.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Outbox", IconSource = _serverPath + "outbox.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Junk E-mail", IconSource = _serverPath + "junk.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Deleted Items", IconSource = _serverPath + "deleted.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Search Folders", IconSource = _serverPath + "search.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Sent Items", IconSource = _serverPath + "sent.png" });
            result.Add("Mail", curr_group);

            curr_group = new MyNavBarData() { Header = "Notes", IconSource = _serverPath + "24x24-notes.png", Items = new List<MyNavBarData>() };
            curr_group.Items.Add(new MyNavBarData() { Header = "Icons", IconSource = _serverPath + "notes.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Note List", IconSource = _serverPath + "notes.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Last Seven Days", IconSource = _serverPath + "notes.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "By Category", IconSource = _serverPath + "notes.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "By Color", IconSource = _serverPath + "notes.png" });
            result.Add("Notes", curr_group);

            curr_group = new MyNavBarData() { Header = "Contacts", IconSource = _serverPath + "24x24-contacts.png", Items = new List<MyNavBarData>() };
            curr_group.Items.Add(new MyNavBarData() { Header = "Address Cards", IconSource = _serverPath + "contacts.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "Details Address List", IconSource = _serverPath + "contacts.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "By Category", IconSource = _serverPath + "contacts.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "By Company", IconSource = _serverPath + "contacts.png" });
            curr_group.Items.Add(new MyNavBarData() { Header = "By Follow-up Flag", IconSource = _serverPath + "contacts.png" });
            result.Add("Contacts", curr_group);

            return result;
        }

    }
    // class for data exchange
    [DataContract]
    public class MyNavBarData
    {
        [DataMember]
        public string IconSource;
        [DataMember]
        public string Header;
        [DataMember]
        public bool IsLoadOnDemandEnabled;
        [DataMember]
        public string Id;
        [DataMember]
        public List<MyNavBarData> Items;
        [DataMember]
        public bool IsSelected;
    }
}
