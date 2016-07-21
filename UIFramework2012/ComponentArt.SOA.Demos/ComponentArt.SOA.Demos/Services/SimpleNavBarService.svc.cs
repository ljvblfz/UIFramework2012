using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using ComponentArt.SOA.UI;


[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class SimpleNavBarService : SoaNavBarService
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

    result.Add("Outlook", new List<SoaNavBarItem>());
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Outlook Today", IconSource = "outlook_today.png" });
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Inbox", IconSource = "inbox.png" });
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Calendar", IconSource = "calendar.png" });
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Contacts", IconSource = "contacts.png" });
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Deleted Items", IconSource = "deleted.png" });
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Drafts", IconSource = "drafts.png" });
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Notes", IconSource = "notes.png" });
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Outbox", IconSource = "outbox.png" });
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Sent Items", IconSource = "sent_items.png" });
    result["Outlook"].Add(new SoaNavBarItem() { Text = "Tasks", IconSource = "tasks.png" });

    result.Add("Second Group", new List<SoaNavBarItem>());
    result["Second Group"].Add(new SoaNavBarItem() { Text = "Outlook Today", IconSource = "outlook_today.png" });
    result["Second Group"].Add(new SoaNavBarItem() { Text = "Inbox", IconSource = "inbox.png" });
    result["Second Group"].Add(new SoaNavBarItem() { Text = "Calendar", IconSource = "calendar.png" });
    result["Second Group"].Add(new SoaNavBarItem() { Text = "Contacts", IconSource = "contacts.png" });

    return result;
  }
}

