<%@ Page language="c#" AutoEventWireup="false" %>
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

	<title>AJAX File Explorer</title>

	<Demos:Css RunAt="server" Id="DemoCss" Index="true" />

	<link href="styles.css" type="text/css" rel="stylesheet" />

<script type="text/javascript">

function TreeView1_nodeSelect(sender, eventArgs)
{
  Grid1.set_webServiceCustomParameter(eventArgs.get_node().get_value());
  Grid1.webServiceSelect();
}

// Handles the Grid double-click event
function Grid1_onItemDoubleClick(sender, eventArgs) {
  var item = eventArgs.get_item();
  TreeView1.selectNodeById(item.Cells[6].get_value());
}

// Load initial grid set
function Grid1_onLoad(sender, eventArgs) {
  Grid1.webServiceSelect();
}

// Overrides the default Grid client-side sort mechanism,
// ensuring that folders are grouped together
function Grid1_onSortChange(sender, eventArgs) {
  var grid = sender;
  var isDesc = eventArgs.get_descending();
  var column = eventArgs.get_column();

  // multiple sort, giving the top priority to IsFolder
  grid.sortMulti([5, !isDesc, column.ColumnNumber, isDesc]);

  // cancel default sort
  eventArgs.set_cancel(true);
}

</script>

</head>
<body>
<form id="form1" runat="server">
	<Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="AJAX File Explorer" />

<div class="demo-area">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
          <asp:ServiceReference path="~/Services/DataGridFileBrowserService.svc/json" />
          <asp:ServiceReference path="~/Services/TreeViewFileBrowserService.svc/json" />
        </Services>
        </asp:ScriptManager>
    <div>


    <table cellspacing="5"><tr><td style="width:270px;background:white;">
      <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height:25px;">
        <tr><td class="Heading">Folder View</td></tr>
      </table>
      <div class="HeadingCell" style="font-family: MS Sans Serif; font-size: 10px; height: 13px;cursor:default; border: 1px solid #ACACAC;border-bottom-width:1px;border-bottom-color:#D5D5D5;border-top-width:0px;">Folders</div>
      <ComponentArt:TreeView id="TreeView1" Height="500" Width="270"
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
        WebServiceCustomParameter="FoldersOnly"
        runat="server" >
        <ClientEvents>
          <NodeSelect EventHandler="TreeView1_nodeSelect" />
        </ClientEvents>
      </ComponentArt:TreeView>
      </td>
      <td style="background:white;">

      <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr><td class="Heading">Folder Contents</td></tr>
      </table>
      <ComponentArt:Grid id="Grid1"
        SoaService="ComponentArt.SOA.UI.ISoaDataGridService"
        PageSize="25"
        RunningMode="Client"
        CssClass="Grid"
        GroupByTextCssClass="GroupByText"
        GroupingNotificationTextCssClass="GridHeaderText"
        ShowFooter="false"
        ImagesBaseUrl="images/"
        Sort="IsFolder DESC, Type"
        ScrollBar="On"
        ScrollTopBottomImagesEnabled="true"
        ScrollTopBottomImageHeight="2"
        ScrollTopBottomImageWidth="16"
        ScrollImagesFolderUrl="images/scroller/"
        ScrollButtonWidth="16"
        ScrollButtonHeight="17"
        ScrollBarCssClass="ScrollBar"
        ScrollGripCssClass="ScrollGrip"
        ScrollBarWidth="16"
        ScrollPopupClientTemplateId="ScrollPopupTemplate"
        Width="500" Height="525" runat="server">
        <ClientEvents>
          <ItemDoubleClick EventHandler="Grid1_onItemDoubleClick" />
          <SortChange EventHandler="Grid1_onSortChange" />
          <Load EventHandler="Grid1_onLoad" />
        </ClientEvents>
        <Levels>
          <ComponentArt:GridLevel
            DataKeyField="Value"
            ShowTableHeading="false"
            ShowSelectorCells="false"
            HeadingCellCssClass="HeadingCell"
            HeadingCellHoverCssClass="HeadingCellHover"
            HeadingCellActiveCssClass="HeadingCellActive"
            HeadingTextCssClass="HeadingCellText"
            DataCellCssClass="DataCell"
            RowCssClass="Row"
            SelectedRowCssClass="SelectedRow"
            SortedDataCellCssClass="SortedDataCell"
            ColumnReorderIndicatorImageUrl="reorder.gif"
            SortAscendingImageUrl="asc.gif"
            SortDescendingImageUrl="desc.gif"
            SortImageWidth="14"
            SortImageHeight="14">
            <Columns>
              <ComponentArt:GridColumn DataField="Icon" Visible="false" />
              <ComponentArt:GridColumn DataField="Name" DataCellClientTemplateId="FirstColumnTemplate" HeadingText="Name" SortImageJustify="false" Width="200" />
              <ComponentArt:GridColumn DataField="Size" DataCellClientTemplateId="SizeColumnTemplate" Align="Right" SortImageJustify="true" />
              <ComponentArt:GridColumn DataField="Type" SortImageJustify="false" />
              <ComponentArt:GridColumn DataField="DateModified" HeadingText="Date Modified" FormatString="M/d/yyyy hh:mm tt" SortImageJustify="false" />
              <ComponentArt:GridColumn DataField="IsFolder" Visible="false" />
              <ComponentArt:GridColumn DataField="Value" Visible="false" />
              <ComponentArt:GridColumn DataField="Extension" Visible="false" />
              <ComponentArt:GridColumn DataField="SizeString" Visible="false" />
            </Columns>
          </ComponentArt:GridLevel>
        </Levels>
        <ClientTemplates>
          <ComponentArt:ClientTemplate Id="FirstColumnTemplate">
          <table cellspacing="0" cellpadding="0" border="0">
          <tr>
            <td><img src="images/## DataItem.GetMember("Icon").Value ##" alt="" width="16" height="16" border="0" ></td>
            <td style="padding-left:2px;"><div style="font-size:10px;font-family: MS Sans Serif;text-overflow:ellipsis;overflow:hidden;"><nobr>## DataItem.GetMember("Name").Value ##</nobr></div></td>
          </tr>
          </table>
          </ComponentArt:ClientTemplate>
          <ComponentArt:ClientTemplate Id="SizeColumnTemplate">
            ## DataItem.GetMember("SizeString").Value ##
          </ComponentArt:ClientTemplate>
          <ComponentArt:ClientTemplate Id="ScrollPopupTemplate">
          <table cellspacing="0" cellpadding="2" border="0" class="ScrollPopup">
          <tr>
            <td style="width:20px;"><img src="images/## DataItem.GetMember("Icon").Value ##" width="16" height="16" border="0" /></td>
            <td style="width:130px;"><div style="font-size:10px;font-family: MS Sans Serif;text-overflow:ellipsis;overflow:hidden;width:130px;"><nobr>## DataItem.GetMember("Name").Value ##</nobr></div></td>
            <td style="width:50px;"><div style="font-size:10px;font-family: MS Sans Serif;text-overflow:ellipsis;overflow:hidden;width:50px;"><nobr>## DataItem.GetMember("SizeString").Value ##</nobr></div></td>
            <td  style="width:120px;" align="right"><div style="font-size:10px;font-family: MS Sans Serif;text-overflow:ellipsis;overflow:hidden;width:120px;"><nobr>## DataItem.GetMember("DateModified").Text ##</nobr></div></td>
          </tr>
          </table>
          </ComponentArt:ClientTemplate>
        </ClientTemplates>
      </ComponentArt:Grid>

      </td>
      </tr>
      </table>

      <br /><br />
    </div>
</div><!-- /demo -->

<Demos:About RunAt="server" Id="DemoAbout" />
<Demos:Footer RunAt="server" Id="DemoFooter" />
</form>
</body>
</html>