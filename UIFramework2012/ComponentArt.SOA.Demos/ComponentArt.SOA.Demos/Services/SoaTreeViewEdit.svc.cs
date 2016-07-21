using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

using ComponentArt.SOA.UI;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class SoaTreeViewEdit : SoaTreeViewService
{
    public override SoaTreeViewAddResponse AddNode(SoaTreeViewAddRequest request)
    {
        sleep();
        return new SoaTreeViewAddResponse() { Cancel = (request.Tag != null && (bool)request.Tag == true) };
    }

    public override SoaTreeViewDeleteResponse DeleteNode(SoaTreeViewDeleteRequest request)
    {
        sleep();
        return new SoaTreeViewDeleteResponse() { Cancel = (request.Tag != null && (bool)request.Tag == true) };
    }

    public override SoaTreeViewEditResponse EditNode(SoaTreeViewEditRequest request)
    {
        sleep();
        return new SoaTreeViewEditResponse() { Cancel = (request.Tag != null && (bool)request.Tag == true) };
    }

    public override SoaTreeViewMoveResponse MoveNode(SoaTreeViewMoveRequest request)
    {
        sleep();
        return new SoaTreeViewMoveResponse() { Cancel = (request.Tag != null && (bool)request.Tag == true) };
    }

    public override SoaTreeViewGetNodesResponse GetNodes(SoaTreeViewGetNodesRequest request)
    {
        return new SoaTreeViewGetNodesResponse() { Nodes = getTreeNodes() };
    }

    // slow down service response to showcase busy indicators
    private void sleep()
    {
        System.Threading.Thread.Sleep(500);
    }

    private List<SoaTreeViewNode> getTreeNodes()
    {
        SoaTreeViewNode newNode;
        SoaTreeViewNode curParentNode;
        List<SoaTreeViewNode> result = new List<SoaTreeViewNode>();

        newNode = new SoaTreeViewNode()
        {
          Text = "Mailbox",
          IconSource = "root.png",
          AllowDrag = true,
          AllowDrop = true,
          AllowNodeEditing = true,
          IsExpanded = true
        };
        result.Add(newNode);
        curParentNode = newNode;

        newNode = new SoaTreeViewNode()
        {
            Text = "Calendar",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "calendar.png"
        };
        curParentNode.Items.Add(newNode);

        newNode = new SoaTreeViewNode()
        {
            Text = "Deleted Items",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "deleted.png",
            IsExpanded = true
        };
        curParentNode.Items.Add(newNode);

        newNode = new SoaTreeViewNode()
        {
            Text = "Drafts",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "drafts.png"
        };
        curParentNode.Items.Add(newNode);

        newNode = new SoaTreeViewNode()
        {
            Text = "Inbox",
            AllowDrag = true,
            AllowDrop = true,
            IconSource = "inbox.png"
        };
        curParentNode.Items.Add(newNode);

        newNode = new SoaTreeViewNode()
        {
            Text = "Junk E-mail",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "junk.png"
        };
        curParentNode.Items.Add(newNode);

        curParentNode = result[0].Items[1];

        newNode = new SoaTreeViewNode()
        {
            Text = "Folder 1",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "folder.png"
        };
        curParentNode.Items.Add(newNode);

        newNode = new SoaTreeViewNode()
        {
            Text = "Folder 2",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "folder.png"
        };
        curParentNode.Items.Add(newNode);

        newNode = new SoaTreeViewNode()
        {
            Text = "Folder 3",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "folder.png"
        };
        curParentNode.Items.Add(newNode);

        newNode = new SoaTreeViewNode()
        {
            Text = "Public Folders",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "folders.png",
            IsExpanded = true
        };
        result.Add(newNode);

        curParentNode = newNode;

        newNode = new SoaTreeViewNode()
        {
            Text = "Folder 1",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "folder.png"
        };
        curParentNode.Items.Add(newNode);

        newNode = new SoaTreeViewNode()
        {
            Text = "Folder 2",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "folder.png"
        };
        curParentNode.Items.Add(newNode);

        newNode = new SoaTreeViewNode()
        {
            Text = "Folder 3",
            AllowDrag = true,
            AllowDrop = true,
            AllowNodeEditing = true,
            IconSource = "folder.png"
        };
        curParentNode.Items.Add(newNode);

        return result;
    }

}
