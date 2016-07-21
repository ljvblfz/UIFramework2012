using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

namespace ComponentArt.Silverlight.Demos.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ManualNavBarService
    {
        [OperationContract]
        public List<MyNavBarData> GetItems(string parentGroup)
        {

            List<MyNavBarData> response = new List<MyNavBarData>();

            Dictionary<string, List<MyNavBarData>> data = getData();

            // load from root
            if (string.IsNullOrEmpty(parentGroup))
            {
                foreach (string group in data.Keys)
                    response.Add(new MyNavBarData() { Header = group, Items = data[group] });
                // show the first group
            }
            else
            {
                if (data.ContainsKey(parentGroup))
                {
                    response.Add(new MyNavBarData() { Header = parentGroup, Items = data[parentGroup] });
                }
            }

            return response;

        }

        private Dictionary<string, List<MyNavBarData>> getData()
        {
            Dictionary<string, List<MyNavBarData>> result = new Dictionary<string, List<MyNavBarData>>();

            result.Add("Mail", new List<MyNavBarData>());
            result["Mail"].Add(new MyNavBarData() { Header = "Mailbox", IconSource = "mailbox.png" });
            result["Mail"].Add(new MyNavBarData() { Header = "Inbox", IconSource = "inbox.png" });
            result["Mail"].Add(new MyNavBarData() { Header = "Drafts", IconSource = "drafts.png" });
            result["Mail"].Add(new MyNavBarData() { Header = "Outbox", IconSource = "outbox.png" });
            result["Mail"].Add(new MyNavBarData() { Header = "Junk E-mail", IconSource = "junk.png" });
            result["Mail"].Add(new MyNavBarData() { Header = "Deleted Items", IconSource = "deleted.png" });
            result["Mail"].Add(new MyNavBarData() { Header = "Search Folders", IconSource = "search.png" });
            result["Mail"].Add(new MyNavBarData() { Header = "Sent Items", IconSource = "sent.png" });

            result.Add("Notes", new List<MyNavBarData>());
            result["Notes"].Add(new MyNavBarData() { Header = "Icons", IconSource = "notes.png" });
            result["Notes"].Add(new MyNavBarData() { Header = "Note List", IconSource = "notes.png" });
            result["Notes"].Add(new MyNavBarData() { Header = "Last Seven Days", IconSource = "notes.png" });
            result["Notes"].Add(new MyNavBarData() { Header = "By Category", IconSource = "notes.png" });
            result["Notes"].Add(new MyNavBarData() { Header = "By Color", IconSource = "notes.png" });

            result.Add("Contacts", new List<MyNavBarData>());
            result["Contacts"].Add(new MyNavBarData() { Header = "Address Cards", IconSource = "contacts.png" });
            result["Contacts"].Add(new MyNavBarData() { Header = "Details Address List", IconSource = "contacts.png" });
            result["Contacts"].Add(new MyNavBarData() { Header = "By Category", IconSource = "contacts.png" });
            result["Contacts"].Add(new MyNavBarData() { Header = "By Company", IconSource = "contacts.png" });
            result["Contacts"].Add(new MyNavBarData() { Header = "By Follow-up Flag", IconSource = "contacts.png" });

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
