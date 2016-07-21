<%@ Page language="c#" %>
<%@ Register TagPrefix="Demos" TagName="Css" Src="~/common/ascx/css.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Header" Src="~/common/ascx/hdr.ascx"%>
<%@ Register TagPrefix="Demos" TagName="About" Src="~/common/ascx/txt.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Footer" Src="~/common/ascx/ftr.ascx"%>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <title>ComboBox SOA.UI Operation</title>

	  <Demos:Css RunAt="server" Id="DemoCss" Index="true" />
	
    <link href="combobox.css" type="text/css" rel="stylesheet" />
  </head>

  <body>
    <form id="Form1" method="post" runat="server">

	  <Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="ComboBox SOA.UI Operation" />

    <div class="DemoArea">

      <asp:ScriptManager ID="ScriptManager1" runat="server">
          <Services>
            <asp:ServiceReference path="~/Services/SoaComboBoxLocationService.svc/json" />
          </Services>
      </asp:ScriptManager>    

      <ComponentArt:ComboBox id="ComboBox1" runat="server" RunningMode="Callback"
        AutoHighlight="false"
        AutoComplete="true"
        AutoFilter="true"
        SoaService="ComponentArt.SOA.UI.ISoaComboBoxService"
        DataTextField="LocationName"
        DataValueField="CountryCode"
        CssClass="comboBox"
        HoverCssClass="comboBoxHover"
        FocusedCssClass="comboBoxHover"
        TextBoxCssClass="comboTextBox"
        TextBoxHoverCssClass="comboBoxHover"
        DropDownCssClass="comboDropDown"
        ItemCssClass="comboItem"
        ItemHoverCssClass="comboItemHover"
        SelectedItemCssClass="comboItemHover"
        DropHoverImageUrl="images/drop_hover.gif"
        DropImageUrl="images/drop.gif"
        DropDownPageSize="10"
        Width="200">
      </ComponentArt:ComboBox>

    </div>

  <Demos:About RunAt="server" Id="DemoAbout" />
  <Demos:Footer RunAt="server" Id="DemoFooter" />

    </form>
  </body>
</html>




