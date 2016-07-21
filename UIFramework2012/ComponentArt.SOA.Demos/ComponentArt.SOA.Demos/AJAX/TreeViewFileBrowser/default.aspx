<%@ Page Language="C#" %>
<%@ Register TagPrefix="Demos" TagName="Css" Src="~/common/ascx/css.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Header" Src="~/common/ascx/hdr.ascx"%>
<%@ Register TagPrefix="Demos" TagName="About" Src="~/common/ascx/txt.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Footer" Src="~/common/ascx/ftr.ascx"%>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head id="head1" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<meta http-equiv="Content-language" content="en" />
	<meta http-equiv="imagetoolbar" content="false"/>
	<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />

	<link rel="shortcut icon" type="image/x-icon" href="http://www.componentart.com/favicon.ico" />
	<link rel="icon" type="image/x-icon" href="http://www.componentart.com/favicon.ico" />

	<title>TreeView: SOA.UI File Explorer</title>

	<Demos:Css RunAt="server" Id="DemoCss" Index="true" />

	<link href="styles.css" type="text/css" rel="stylesheet" >

</head>
<body>
<form id="form1" runat="server">
	<Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="TreeView: SOA.UI File Explorer" />

<div class="demo-area">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
          <asp:ServiceReference
             path="~/Services/TreeViewFileBrowserService.svc/json" />
        </Services>
        </asp:ScriptManager>

  <table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
      <td width="320">
      <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height:25px;">
        <tr><td class="Heading">TreeView File Explorer</td></tr>
      </table>
      <div class="HeadingCell" style="font-family: MS Sans Serif; font-size: 10px; height: 13px;cursor:default; border: 1px solid #ACACAC;border-bottom-width:1px;border-bottom-color:#D5D5D5;border-top-width:0px;">Folders</div>
      <ComponentArt:TreeView id="TreeView1" Height="450" Width="260"
        Autoscroll="true"
        FillContainer="false"
        HoverPopupEnabled="true"
        DragAndDropEnabled="false"
        NodeEditingEnabled="false"
        KeyboardEnabled="false"
        CssClass="TreeView"
        NodeCssClass="TreeNode"
        SelectedNodeCssClass="SelectedTreeNode"
        HoverNodeCssClass="HoverTreeNode"
        HoverPopupNodeCssClass="HoverPopupTreeNode"
        LineImageWidth="19"
        LineImageHeight="20"
        DefaultImageWidth="15"
        DefaultImageHeight="15"
        ItemSpacing="0"
        ImagesBaseUrl="images/"
        NodeLabelPadding="3"
        ParentNodeImageUrl="folder.png"
        ExpandedParentNodeImageUrl="folder_open.png"
        LeafNodeImageUrl="folder.png"
        ShowLines="true"
        LineImagesFolderUrl="images/lines/"
        CollapseNodeOnSelect="false"
        EnableViewState="true"
        ContentLoadingImageUrl="lines/iplus.gif"
        SoaService="ComponentArt.SOA.UI.ISoaTreeViewService"
        runat="server">
      </ComponentArt:TreeView>
      </td>
      <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
      <td valign="middle" width="100%" class="MainText">
        <br/>
        <span class="hint">
        This is a file system browser implemented <br/>
        entirely through automatic <b>SOA.UI</b> service calls.
        </span>
        <br/><br/><br/><br/><br/><br/><br/><br/>
      </td>
    </tr>
    </table>

    <asp:label id="Label1" runat="server" />

</div><!-- /demo -->

<Demos:About RunAt="server" Id="DemoAbout" />
<Demos:Footer RunAt="server" Id="DemoFooter" />
</form>
</body>
</html>