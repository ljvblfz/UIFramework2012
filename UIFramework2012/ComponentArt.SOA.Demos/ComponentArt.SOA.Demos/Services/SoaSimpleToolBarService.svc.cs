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
public class SoaSimpleToolBarService : ComponentArt.SOA.UI.SoaToolBarService
{
    public override ComponentArt.SOA.UI.SoaToolBarGetItemsResponse GetItems(ComponentArt.SOA.UI.SoaToolBarGetItemsRequest request)
    {
        ComponentArt.SOA.UI.SoaToolBarGetItemsResponse response = new ComponentArt.SOA.UI.SoaToolBarGetItemsResponse();

        response.Items = new List<ComponentArt.SOA.UI.SoaToolBarItem>();

        ComponentArt.SOA.UI.SoaToolBarItem myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Button;
        myItem.IconSource = "ToolBar_Images/EditCutDark.png";
        response.Items.Add(myItem);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Button;
        myItem.IconSource = "ToolBar_Images/EditCopyDark.png";
        response.Items.Add(myItem);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Button;
        myItem.IconSource = "ToolBar_Images/EditPaste.png";
        response.Items.Add(myItem);

        ComponentArt.SOA.UI.SoaToolBarItem mySep = new ComponentArt.SOA.UI.SoaToolBarItem();
        mySep.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Separator;
        mySep.Text = "|";
        response.Items.Add(mySep);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Check;
        myItem.IconSource = "ToolBar_Images/FormatBoldDark.png";
        response.Items.Add(myItem);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Check;
        myItem.IconSource = "ToolBar_Images/FormatItalicDark.png";
        response.Items.Add(myItem);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Check;
        myItem.IconSource = "ToolBar_Images/FormatUnderlineDark.png";
        response.Items.Add(myItem);

        mySep = new ComponentArt.SOA.UI.SoaToolBarItem();
        mySep.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Separator;
        mySep.Text = "|";
        response.Items.Add(mySep);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Radio;
        myItem.GroupName = "Align";
        myItem.IconSource = "ToolBar_Images/AlignLeftDark.png";
        response.Items.Add(myItem);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Radio;
        myItem.GroupName = "Align";
        myItem.IconSource = "ToolBar_Images/AlignCenterDark.png";
        response.Items.Add(myItem);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Radio;
        myItem.GroupName = "Align";
        myItem.IconSource = "ToolBar_Images/AlignRightDark.png";
        response.Items.Add(myItem);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Radio;
        myItem.GroupName = "Align";
        myItem.IconSource = "ToolBar_Images/AlignJustifyDark.png";
        response.Items.Add(myItem);

        mySep = new ComponentArt.SOA.UI.SoaToolBarItem();
        mySep.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Separator;
        mySep.Text = "|";
        response.Items.Add(mySep);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Radio;
        myItem.GroupName = "Bullets";
        myItem.IconSource = "ToolBar_Images/BulletsNumberingDark.png";
        response.Items.Add(myItem);

        myItem = new ComponentArt.SOA.UI.SoaToolBarItem();
        myItem.ItemType = ComponentArt.SOA.UI.SoaToolBarItemType.Radio;
        myItem.GroupName = "Bullets";
        myItem.IconSource = "ToolBar_Images/BulletsSymbolsDark.png";
        response.Items.Add(myItem);

        return response;
    }
}