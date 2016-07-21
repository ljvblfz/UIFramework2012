using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using ComponentArt.SOA.UI;
using System.Web;
using System.Data;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class SoaMenuDemoService : SoaMenuService
{
  private List<SoaMenuItem> LoadItems()
  {
    List<SoaMenuItem> lsmi = new List<SoaMenuItem>();

    SoaMenuItem smi_file = GenerateMenuItem("File", "file", "", "", false);
    smi_file.Items = new List<SoaMenuItem>();
    smi_file.Items.Add(GenerateMenuItem("New", "file_new", "", "MenuIcons/New.png", false));
    smi_file.Items.Add(GenerateMenuItem("Open", "file_open", "", "MenuIcons/Open.png", false));
    smi_file.Items.Add(GenerateMenuItem("Save", "file_save", "", "MenuIcons/Save.png", false));
    smi_file.Items.Add(GenerateMenuItem("Save As...", "file_saveas", "", "", false));
    smi_file.Items.Add(GenerateMenuItem("Save As Web Page", "file_saveaswebpage", "", "MenuIcons/SaveAsWebPage.png", false));
    smi_file.Items.Add(GenerateMenuItem("File Search...", "file_search", "", "MenuIcons/Find.png", false));
    smi_file.Items.Add(GenerateMenuItem("Exit", "file_exit", "http://componentart.com", "", false));

    SoaMenuItem smi_edit = GenerateMenuItem("Edit", "edit", "", "", false);
    smi_edit.Items = new List<SoaMenuItem>();
    smi_edit.Items.Add(GenerateMenuItem("Cut", "edit_cut", "", "MenuIcons/Cut.png", false));
    smi_edit.Items.Add(GenerateMenuItem("Copy", "edit_copy", "", "MenuIcons/Copy.png", false));
    smi_edit.Items.Add(GenerateMenuItem("Office Clipboard", "", "edit_officeclipboard", "", false));
    smi_edit.Items.Add(GenerateMenuItem("Paste", "edit_paste", "", "MenuIcons/Paste.png", false));

    SoaMenuItem smi_view = GenerateMenuItem("View", "view", "", "", false);
    smi_view.Items = new List<SoaMenuItem>();
    smi_view.Items.Add(GenerateMenuItem("Normal", "view_normal", "", "MenuIcons/ViewNormal.png", false));
    smi_view.Items.Add(GenerateMenuItem("Web Layout", "view_weblayout", "", "MenuIcons/ViewWebLayout.png", false));
    smi_view.Items.Add(GenerateMenuItem("Outline", "view_outline", "", "MenuIcons/ViewOutline.png", false));
    smi_view.Items.Add(GenerateMenuItem("Navigate", "view_navigate", "", "", false));
    smi_view.Items.Add(GenerateMenuItem("Back", "view_back", "", "", false));
    smi_view.Items.Add(GenerateMenuItem("Forward", "view_forward", "", "", false));

    lsmi.Add(smi_file);
    lsmi.Add(smi_edit);
    lsmi.Add(smi_view);
    return lsmi;
  }

  private SoaMenuItem GenerateMenuItem(string text, string id, string navUrl, string sIconUrl, bool isSeparator)
  {
    SoaMenuItem smi = new SoaMenuItem(text);
    smi.Id = id;
    smi.NavigateUrl = navUrl;
    smi.IsSeparator = isSeparator;
    smi.IconSource = sIconUrl;
    return smi;
  }

  public override SoaMenuGetItemsResponse GetItems(SoaMenuGetItemsRequest request)
  {
    SoaMenuGetItemsResponse response = new SoaMenuGetItemsResponse();

    List<SoaMenuItem> liItems = new List<SoaMenuItem>();
    response.Items = LoadItems();
    response.Tag = request.Tag;
    return response;
  }
}
