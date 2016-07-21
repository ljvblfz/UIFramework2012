<%@ Page language="c#" %>
<%@ Register TagPrefix="Demos" TagName="Css" Src="~/common/ascx/css.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Header" Src="~/common/ascx/hdr.ascx"%>
<%@ Register TagPrefix="Demos" TagName="About" Src="~/common/ascx/txt.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Footer" Src="~/common/ascx/ftr.ascx"%>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="EN">
  <head>
    <title>Grid SOA.UI Editing</title>
    
    <Demos:Css RunAt="server" Id="DemoCss" Index="true" />
    
    <link href="gridStyle.css" type="text/css" rel="stylesheet" />
    <link href="combobox.css" type="text/css" rel="stylesheet" />
    <link href="calendarStyle.css" type="text/css" rel="stylesheet" />
  </head>
  <script type="text/javascript">

  function Grid1_webServiceBeforeInvoke(sender, eventArgs)
  {
    document.getElementById('soa_feedback').innerHTML += "Issuing SOA.UI " + eventArgs.get_action() + " request...<br/>";
  }

  function Grid1_webServiceComplete(sender, eventArgs)
  {
    var response = eventArgs.get_response();
    
    document.getElementById('soa_feedback').innerHTML += "Completed SOA.UI " + eventArgs.get_action() + " request" + (response.Message? ": " + response.Message : ".") + "<br/>";
  }

  function getValue()
  {
    return Picker1.getSelectedDate();
  }

  function setValue(DataItem)
  {
    var selectedDate = DataItem.getMember('LastOrderedOn').get_object();
    Picker1.setSelectedDate(selectedDate);
  }

  function editGrid(rowId)
  {
    Grid1.edit(Grid1.getItemFromClientId(rowId));
  }

  function editRow()
  {
    Grid1.editComplete();
  }

  function insertRow()
  {
    Grid1.editComplete();
  }

  function deleteRow(rowId)
  {
    Grid1.deleteItem(Grid1.getItemFromClientId(rowId));
  }

  function Picker1_OnDateChange()
  {
    var fromDate = Picker1.getSelectedDate();
    Calendar1.setSelectedDate(fromDate);
  }

  function Calendar1_OnChange()
  {
    var fromDate = Calendar1.getSelectedDate();
    Picker1.setSelectedDate(fromDate);
  }

  function Button_OnClick(button)
  {
    if (Calendar1.get_popUpShowing())
    {
      Calendar1.hide();
    }
    else
    {
      var date = Picker1.getSelectedDate();
      Calendar1.setSelectedDate(date);
      Calendar1.show(button);
    }
  }

  function Button_OnMouseUp()
  {
    if (Calendar1.get_popUpShowing())
    {
      event.cancelBubble=true;
      event.returnValue=false;
      return false;
    }
    else
    {
      return true;
    }
  }

  </script>

  <body>
    <form id="Form1" method="post" runat="server">

    <Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="Grid SOA.UI Editing" />
    
    <div class="DemoArea">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
          <asp:ServiceReference
             path="~/Services/SoaDataGridProductService.svc/json" />
        </Services>
        </asp:ScriptManager>

      <ComponentArt:Grid id="Grid1"
        SoaService="ComponentArt.SOA.UI.ISoaDataGridService"
        EditOnClickSelectedItem="false"
        CallbackReloadTemplates="false"
        AllowEditing="true"
        AllowTextSelection="true"
        ShowHeader="False"
        CssClass="Grid"
        KeyboardEnabled="false"
        FooterCssClass="GridFooter"
        RunningMode="WebService"
        PagerStyle="Numbered"
        PagerTextCssClass="PagerText"
        PageSize="15"
        ImagesBaseUrl="images/"
        width="730" Height="350"
        LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
        LoadingPanelPosition="MiddleCenter"
        runat="server">
        <ClientEvents>
          <WebServiceBeforeInvoke EventHandler="Grid1_webServiceBeforeInvoke" />
          <WebServiceComplete EventHandler="Grid1_webServiceComplete" />
        </ClientEvents>
        <Levels>
          <ComponentArt:GridLevel
            DataKeyField="ProductId"
            ShowTableHeading="false"
            ShowSelectorCells="true"
            SelectorCellCssClass="SelectorCell"
            SelectorCellWidth="18"
            SelectorImageUrl="selector.gif"
            SelectorImageWidth="17"
            SelectorImageHeight="15"
            HeadingSelectorCellCssClass="SelectorCell"
            HeadingCellCssClass="HeadingCell"
            HeadingRowCssClass="HeadingRow"
            HeadingTextCssClass="HeadingCellText"
            DataCellCssClass="DataCell"
            RowCssClass="Row"
            SelectedRowCssClass="SelectedRow"
            SortAscendingImageUrl="asc.gif"
            SortDescendingImageUrl="desc.gif"
            SortImageWidth="10"
            SortImageHeight="10"
            EditCellCssClass="EditDataCell"
            EditFieldCssClass="EditDataField"
            EditCommandClientTemplateId="EditCommandTemplate"
            InsertCommandClientTemplateId="InsertCommandTemplate"
            >
            <Columns>
              <ComponentArt:GridColumn AllowEditing="false" DataField="ProductId" Visible="false"/>
              <ComponentArt:GridColumn DataField="CategoryId" Width="140" />
              <ComponentArt:GridColumn DataField="ProductName"/>
              <ComponentArt:GridColumn DataField="LastOrderedOn" Width="200" FormatString="MM/dd/yyyy" EditControlType="Custom" EditCellServerTemplateId="PickerTemplate" CustomEditSetExpression="setValue(DataItem)" CustomEditGetExpression="getValue()" />
              <ComponentArt:GridColumn DataField="UnitPrice" />
              <ComponentArt:GridColumn DataField="UnitsInStock" Width="50" />
              <ComponentArt:GridColumn AllowSorting="false" HeadingText="Edit Command" DataCellClientTemplateId="EditTemplate" EditControlType="EditCommand" Width="100" Align="Center" />
            </Columns>
          </ComponentArt:GridLevel>
        </Levels>
        <ClientTemplates>
          <ComponentArt:ClientTemplate Id="EditTemplate">
            <a href="javascript:editGrid('## DataItem.ClientId ##');">Edit</a> | <a href="javascript:deleteRow('## DataItem.ClientId ##')">Delete</a>
          </ComponentArt:ClientTemplate>
          <ComponentArt:ClientTemplate Id="EditCommandTemplate">
            <a href="javascript:editRow();">Update</a> | <a href="javascript:Grid1.EditCancel();">Cancel</a>
          </ComponentArt:ClientTemplate>
          <ComponentArt:ClientTemplate Id="InsertCommandTemplate">
            <a href="javascript:insertRow();">Insert</a> | <a href="javascript:Grid1.EditCancel();">Cancel</a>
          </ComponentArt:ClientTemplate>
          <ComponentArt:ClientTemplate Id="LoadingFeedbackTemplate">
          <table cellspacing="0" cellpadding="0" border="0">
          <tr>
            <td style="font-size:10px;">Loading...&nbsp;</td>
            <td><img src="images/spinner.gif" width="16" height="16" border="0"></td>
          </tr>
          </table>
          </ComponentArt:ClientTemplate>
        </ClientTemplates>
        <ServerTemplates>          
          <ComponentArt:GridServerTemplate Id="PickerTemplate">
            <Template>

 	   <table cellspacing="0" cellpadding="0" border="0">
	    <tr>
	      <td onmouseup="Button_OnMouseUp()"><ComponentArt:Calendar id="Picker1"
	          runat="server"
	          PickerFormat="Custom"
	          PickerCustomFormat="MMMM d yyyy"
	          ControlType="Picker"
	          SelectedDate="2005-9-13"
	          ClientSideOnSelectionChanged="Picker1_OnDateChange"
	          PickerCssClass="picker" /></td>
	      <td style="font-size:10px;">&nbsp;</td>
	      <td><img id="calendar_from_button" alt="" onclick="Button_OnClick(this)" onmouseup="Button_OnMouseUp()" class="calendar_button" src="images/btn_calendar.gif" /></td>
	    </tr>
	    </table>

		<ComponentArt:Calendar runat="server"
		      id="Calendar1"
		      AllowMultipleSelection="false"
		      AllowWeekSelection="false"
		      AllowMonthSelection="false"
		      ControlType="Calendar"
		      PopUp="Custom"
		      PopUpExpandControlId="calendar_from_button"
		      CalendarTitleCssClass="title"
		      SelectedDate="2005-9-13"
		      VisibleDate="2005-9-13"
		      ClientSideOnSelectionChanged="Calendar1_OnChange"
		      DayHeaderCssClass="dayheader"
		      DayCssClass="day"
		      DayHoverCssClass="dayhover"
		      OtherMonthDayCssClass="othermonthday"
		      SelectedDayCssClass="selectedday"
		      CalendarCssClass="calendar"
		      NextPrevCssClass="nextprev"
		      MonthCssClass="month"
		      SwapSlide="Linear"
		      SwapDuration="300"
		      DayNameFormat="FirstTwoLetters"
		      PrevImageUrl="images/cal_prevMonth.gif"
  		      NextImageUrl="images/cal_nextMonth.gif"
    			/>

            </Template>
          </ComponentArt:GridServerTemplate>
        </ServerTemplates>
      </ComponentArt:Grid>

      <br />
      <table width="730" cellpadding="0" cellspacing="0" border="0">
      <tr>
        <td valign="top">
          SOA.UI actions:<br />
          <div id="soa_feedback" style="overflow:scroll; height:160px; width:350px; background-color:White; border: 1px solid black;">
          </div>
        </td>
        <td width="270" valign="top" style="padding:10px;">
          <span class="hint">Insert, update, and delete any number of records. All changes are automatically communicated to the SOA.UI web service. </span><br><br>          
        </td>        
        <td align="right" valign="top"><input type="button" onclick="Grid1.get_table().addRow()" value="Add row" /></td>
      </tr>
      </table>

    </div>

    
<Demos:About RunAt="server" Id="DemoAbout" />
<Demos:Footer RunAt="server" Id="DemoFooter" />

    </form>

  </body>
</html>




