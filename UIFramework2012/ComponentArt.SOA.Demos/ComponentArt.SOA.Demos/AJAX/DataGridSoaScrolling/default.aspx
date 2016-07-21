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

	<title>Grid: Automatic Scrolling using SOA.UI</title>

	<Demos:Css RunAt="server" Id="DemoCss" Index="true" />
	<link href="gridStyle.css" type="text/css" rel="stylesheet" />

</head>
<body>
<form id="form1" runat="server">
	<Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="Grid: Automatic Scrolling using SOA.UI" />

<div class="demo-area">
<asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
          <asp:ServiceReference
             path="~/Services/SoaDataGridMessageService.svc/json" />
        </Services>
        </asp:ScriptManager>
    <div>
    
    <div style="padding:20px;">
    
    <ComponentArt:Grid id="Grid1"
        RunningMode="WebService"
        CssClass="Grid"
        SoaService="ComponentArt.SOA.UI.ISoaDataGridService"
        DataAreaCssClass="GridData"
        ShowHeader="true"
        ShowFooter="false"
        HeaderCssClass="GridHeader"
        FooterCssClass="GridFooter"
        PageSize="18"
        PagerStyle="Slider"
        PagerTextCssClass="GridFooterText"
        PagerButtonWidth="44"
        PagerButtonHeight="26"
        PagerButtonHoverEnabled="true"
        SliderHeight="26"
        SliderWidth="150"
        SliderGripWidth="9"
        SliderPopupOffsetX="80"
        SliderPopupClientTemplateId="SliderTemplate"
        ImagesBaseUrl="images/"
        LoadingPanelFadeDuration="1000"
        LoadingPanelFadeMaximumOpacity="60"
        LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
        LoadingPanelPosition="MiddleCenter"
        AllowGrouping="true"
        GroupingMode="ConstantRows"
        PreExpandOnGroup="true"
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
        Width="700" runat="server">
        <Levels>
          <ComponentArt:GridLevel
            AllowGrouping="true"
            DataKeyField="PostId"
            ShowTableHeading="false"
            TableHeadingCssClass="GridHeader"
            RowCssClass="Row"
            GroupHeadingCssClass="GroupHeading"
            ColumnReorderIndicatorImageUrl="reorder.gif"
            DataCellCssClass="DataCell"
            HeadingCellCssClass="HeadingCell"
            HeadingCellHoverCssClass="HeadingCellHover"
            HeadingCellActiveCssClass="HeadingCellActive"
            HeadingRowCssClass="HeadingRow"
            HeadingTextCssClass="HeadingCellText"
            SelectedRowCssClass="SelectedRow"
            SortedDataCellCssClass="SortedDataCell"
            SortAscendingImageUrl="asc.gif"
            SortDescendingImageUrl="desc.gif"
            SortImageWidth="9"
            SortImageHeight="5"
            TableHeadingClientTemplateId="TableHeadingTemplate"
            >
            <Columns>
              <ComponentArt:GridColumn DataField="PriorityIcon" Align="Center" DataCellClientTemplateId="PriorityIconTemplate" HeadingCellCssClass="FirstHeadingCell" DataCellCssClass="FirstDataCell" HeadingImageUrl="icon_priority.png" HeadingImageWidth="8" HeadingImageHeight="14" AllowGrouping="false" Width="12" FixedWidth="True" />
              <ComponentArt:GridColumn DataField="EmailIcon" Align="Center" DataCellClientTemplateId="EmailIconIconTemplate" HeadingImageUrl="icon_icon.png" HeadingImageWidth="12" HeadingImageHeight="14" AllowGrouping="false" Width="20" FixedWidth="True" />
              <ComponentArt:GridColumn DataField="AttachmentIcon" Align="Center" DataCellClientTemplateId="AttachmentIconTemplate" HeadingImageUrl="icon_attachment.png" HeadingImageWidth="10" HeadingImageHeight="16" AllowGrouping="false" Width="12" FixedWidth="True" />
              <ComponentArt:GridColumn DataField="Subject" />
              <ComponentArt:GridColumn DataField="LastPostDate" HeadingText="Received" FormatString="MMM dd yyyy, hh:mm tt" />
              <ComponentArt:GridColumn DataField="StartedBy" Width="80" />
              <ComponentArt:GridColumn DataField="TotalViews" DefaultSortDirection="Descending" Width="80" />
              <ComponentArt:GridColumn DataField="FlagIcon" Align="Center" DataCellClientTemplateId="FlagIconTemplate" DataCellCssClass="LastDataCell" HeadingImageUrl="icon_flag.png" HeadingImageWidth="16" HeadingImageHeight="14" AllowGrouping="false" Width="20" FixedWidth="True" />
              <ComponentArt:GridColumn DataField="PostId" Visible="false" />
            </Columns>
          </ComponentArt:GridLevel>
        </Levels>
        <ClientTemplates>
    <ComponentArt:ClientTemplate Id="PreHeaderTemplate">
            <img alt="" src="images/grid_preheader.gif" style="display:block;" />
          </ComponentArt:ClientTemplate>

    <ComponentArt:ClientTemplate Id="PostFooterTemplate">
            <img alt="" src="images/grid_postfooter.gif" style="display:block;" />
          </ComponentArt:ClientTemplate>


          <ComponentArt:ClientTemplate Id="TableHeadingTemplate">
            Try paging, sorting, column resizing, and column reordering.
          </ComponentArt:ClientTemplate>

          <ComponentArt:ClientTemplate Id="PriorityIconTemplate">
            <img src="images/## DataItem.GetMember('PriorityIcon').Value ##" width="8" height="10" border="0" >
          </ComponentArt:ClientTemplate>

          <ComponentArt:ClientTemplate Id="EmailIconIconTemplate">
            <img src="images/## DataItem.GetMember('EmailIcon').Value ##" width="20" height="15" border="0" >
          </ComponentArt:ClientTemplate>

          <ComponentArt:ClientTemplate Id="AttachmentIconTemplate">
            <img src="images/## DataItem.GetMember('AttachmentIcon').Value ##" width="8" height="10" border="0" >
          </ComponentArt:ClientTemplate>

          <ComponentArt:ClientTemplate Id="FlagIconTemplate">
            <img src="images/## DataItem.GetMember('FlagIcon').Value ##" width="12" height="12" border="0" >
          </ComponentArt:ClientTemplate>

          <ComponentArt:ClientTemplate Id="LoadingFeedbackTemplate">
          <table height="340" width="692" bgcolor="#e0e0e0"><tr><td valign="center" align="center">
          <table cellspacing="0" cellpadding="0" border="0">
          <tr>
            <td style="font-size:10px;font-family:Verdana;">Loading...&nbsp;</td>
            <td><img src="images/spinner.gif" width="16" height="16" border="0"></td>
          </tr>
          </table>
          </td></tr></table>
          </ComponentArt:ClientTemplate>

          <ComponentArt:ClientTemplate Id="SliderTemplate">
            <table class="SliderPopup" width="200" style="background-color:#ffffff" cellspacing="0" cellpadding="0" border="0">
            <tr>
              <td style="padding:20px;" valign="center" align="center">
Page <b>## DataItem.PageIndex + 1 ##</b> of <b>## Grid1.PageCount ##</b>
              </td>
            </tr>
            </table>
          </ComponentArt:ClientTemplate>
        </ClientTemplates>
      </ComponentArt:Grid>
    
    </div>
      
      
      <br /><br />
    </div>
</div><!-- /demo -->

<Demos:About RunAt="server" Id="DemoAbout" />
<Demos:Footer RunAt="server" Id="DemoFooter" />
</form>
</body>
</html>