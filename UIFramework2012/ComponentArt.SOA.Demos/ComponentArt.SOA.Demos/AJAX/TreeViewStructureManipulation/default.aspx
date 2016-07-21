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

	<title>TreeView: Structure Manipulation with SOA.UI</title>

	<Demos:Css RunAt="server" Id="DemoCss" Index="true" />

	<link href="styles.css" type="text/css" rel="stylesheet" />
	<link href="treeStyle.css" type="text/css" rel="stylesheet" />    
    <link href="menuStyle.css" type="text/css" rel="stylesheet" />    
    <link href="toolBarStyle.css" type="text/css" rel="stylesheet" />    
    <link href="gridStyle.css" type="text/css" rel="stylesheet" />    

</head>
<body>
<form id="form1" runat="server">
	<Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="TreeView: Structure Manipulation with SOA.UI" />

<div class="demo-area">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
          <asp:ServiceReference
             path="~/Services/SoaTreeViewEdit.svc/json" />
        </Services>
        </asp:ScriptManager>

  <script type="text/javascript" src="ClientCode.js"></script>
 
  <br/>
  <span class="hint">Use the toolbar to manipulate the TreeView structure on the client and signal the server via a SOA.UI service: </span>
      
  <table cellpadding="0" cellspacing="0" border="0">
  <tr>
    <td style="width:223px;">
      <br/><br/>
      <ComponentArt:ToolBar Width="183" Orientation="Flow"
        runat="server"
        ID="ToolBar1" 
        ImagesBaseUrl="images/"
        CssClass="ToolBar"
        ItemSpacing="1"
        DefaultItemImageWidth="18"
        DefaultItemImageHeight="16"
        DefaultItemTextImageRelation="ImageOnly"
        DefaultItemCssClass="ToolBarItem"
        DefaultItemHoverCssClass="ToolBarItemHover"
        DefaultItemExpandedCssClass="ToolBarItemExpanded"
        SiteMapXmlFile="ToolBar1.xml"
        EnableViewState="true" >
      </ComponentArt:ToolBar>
      
      <ComponentArt:Menu
        runat="server"
        ID="Menu1"
        ImagesBaseUrl="images/"
        CssClass="MenuGroup"
        DefaultItemLookID="DefaultItemLook"
        DefaultGroupItemSpacing="1"
        TopGroupExpandDirection="BelowLeft"
        Orientation="Vertical"
        OverlayWindowedElements="false"
        HideSelectElements="false"
        ContextMenu="Custom"
        SiteMapXmlFile="Menu1.xml">
        <ItemLooks>
          <ComponentArt:ItemLook LookID="DefaultItemLook" CssClass="MenuItem" HoverCssClass="MenuItemHover" ExpandedCssClass="MenuItemHover" LeftIconWidth="18" LeftIconHeight="16" LabelPaddingLeft="10" LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
          <ComponentArt:ItemLook LookID="BreakMenuItemLook" CssClass="MenuBreak" />
        </ItemLooks>
        <ClientEvents>
          <ItemSelect EventHandler="Menu1_onItemSelect" />
        </ClientEvents>
      </ComponentArt:Menu>

      <ComponentArt:TreeView id="TreeView1" Height="264" Width="180" runat="server" 
        AutoAssignNodeIDs="true"
        CssClass="TreeView" 
        SoaService="ComponentArt.SOA.UI.ISoaTreeViewService"
        NodeCssClass="TreeNode" 
        SelectedNodeCssClass="SelectedTreeNode" 
        HoverNodeCssClass="HoverTreeNode" 
        NodeEditCssClass="NodeEdit" 
        ExpandNodeOnSelect="false"
        CollapseNodeOnSelect="false"
        ImagesBaseUrl="images/"
        DefaultImageWidth="18"
        DefaultImageHeight="16"        
        ShowLines="true" 
        LineImagesFolderUrl="images/lines/"
        LineImageWidth="19"
        LineImageHeight="20"
        DragAndDropEnabled="true" 
        NodeEditingEnabled="false" 
        KeyboardEnabled="true"
        EnableViewState="true" >
       <ClientEvents>
        <Load EventHandler="TreeView1_onLoad" />
        <NodeSelect EventHandler="TreeView1_onNodeSelect" />
        <NodeRename EventHandler="TreeView1_onNodeRename" />
        <NodeBeforeMove EventHandler="TreeView1_onBeforeNodeMove" />
        <NodeMove EventHandler="TreeView1_onNodeMove" />
        <WebServiceComplete EventHandler="TreeView1_onWebServiceComplete" />
       </ClientEvents>
      </ComponentArt:TreeView>

    <br /><br />
    </td>
    <td style="width:20px;">&nbsp;</td>
    <td style="width:275" valign="top">
      <br/><br/>
      <span class="hint">SOA.UI activity: </span>
      <div id="soafeedback" style="border:1px solid black; width: 300px; height: 100px; background: white;"></div>
    </td>
  </tr>
  </table>


</div><!-- /demo -->

<Demos:About RunAt="server" Id="DemoAbout" />
<Demos:Footer RunAt="server" Id="DemoFooter" />
</form>
</body>
</html>