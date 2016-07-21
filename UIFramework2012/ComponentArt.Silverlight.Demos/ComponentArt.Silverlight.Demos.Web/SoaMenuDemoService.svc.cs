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

namespace ComponentArt.Silverlight.Demos.Web
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SoaMenuDemoService : SoaMenuService
    {
        private List<SoaMenuItem> GetItems()
        {
            List<SoaMenuItem> lsmi = new List<SoaMenuItem>();

            SoaMenuItem smi_file = GenerateMenuItem("File", "file", "", false, null);
            smi_file.Items = new List<SoaMenuItem>();
            smi_file.Items.Add(GenerateMenuItem("New", "file_new", "", false, "New.png"));
            smi_file.Items.Add(GenerateMenuItem(null, "file_open", "", false, "Open.png"));
            smi_file.Items.Add(GenerateMenuItem("", "", "", true, null));
            smi_file.Items.Add(GenerateMenuItem("Save", "file_save", "", false, "Save.png"));
            smi_file.Items.Add(GenerateMenuItem("Save As...", "file_saveas", "", false, null));
            smi_file.Items.Add(GenerateMenuItem("Save As Web Page", "file_saveaswebpage", "", false, "SaveAsWebPage.png"));
            smi_file.Items.Add(GenerateMenuItem("File Search...", "file_search", "", false, "Find.png"));
            smi_file.Items.Add(GenerateMenuItem("", "", "", true, null));
            smi_file.Items.Add(GenerateMenuItem("Exit", "file_exit", "http://componentart.com", false, null));

            SoaMenuItem smi_edit = GenerateMenuItem("Edit", "edit", "", false, null);
            smi_edit.Items = new List<SoaMenuItem>();
            smi_edit.Items.Add(GenerateMenuItem("Cut", "edit_cut", "", false, "Cut.png"));
            smi_edit.Items.Add(GenerateMenuItem("Copy", "edit_copy", "", false, "Copy.png"));
            smi_edit.Items.Add(GenerateMenuItem("Office Clipboard", "", "edit_officeclipboard", false, null));
            smi_edit.Items.Add(GenerateMenuItem("Paste", "edit_paste", "", false, "Paste.png"));

            SoaMenuItem smi_view = GenerateMenuItem("View", "view", "", false, null);
            smi_view.Items = new List<SoaMenuItem>();
            smi_view.Items.Add(GenerateMenuItem("Normal", "view_normal", "", false, "ViewNormal.png"));
            smi_view.Items.Add(GenerateMenuItem("Web Layout", "view_weblayout", "", false, "ViewWebLayout.png"));
            smi_view.Items.Add(GenerateMenuItem("Outline", "view_outline", "", false, "ViewOutline.png"));
            smi_view.Items.Add(GenerateMenuItem("", "", "", true, null));
            smi_view.Items.Add(GenerateMenuItem("Navigate", "view_navigate", "", false, null));
            smi_view.Items.Add(GenerateMenuItem("Back", "view_back", "", false, null));
            smi_view.Items.Add(GenerateMenuItem("Forward", "view_forward", "", false, null));

            lsmi.Add(smi_file);
            lsmi.Add(smi_edit);
            lsmi.Add(smi_view);
            return lsmi;
        }

        private SoaMenuItem GenerateMenuItem(string text, string id, string navUrl, bool isSeparator, string iconSource)
        {
            SoaMenuItem smi = new SoaMenuItem(text);
            smi.Id = id;
            smi.NavigateUrl = navUrl;
            smi.IsSeparator = isSeparator;
            if (iconSource == null)
            {
                smi.IconSource = null;
            }
            else
            {
                smi.IconSource = "/controls/menu/menuicons/" + iconSource;
            }
            return smi;
        }

        public override SoaMenuGetItemsResponse GetItems(SoaMenuGetItemsRequest request)
        {
            SoaMenuGetItemsResponse response = new SoaMenuGetItemsResponse();

            List<SoaMenuItem> liItems = new List<SoaMenuItem>();
            response.Items = GetItems();
            response.Tag = request.Tag;
            return response;
        }
    }
}
