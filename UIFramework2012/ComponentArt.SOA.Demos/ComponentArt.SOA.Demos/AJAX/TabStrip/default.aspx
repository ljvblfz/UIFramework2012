<%@ Page Language="C#" %>
<%@ Register TagPrefix="Demos" TagName="Css" Src="~/common/ascx/css.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Header" Src="~/common/ascx/hdr.ascx"%>
<%@ Register TagPrefix="Demos" TagName="About" Src="~/common/ascx/txt.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Footer" Src="~/common/ascx/ftr.ascx"%>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head id="head1" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<meta http-equiv="Content-language" content="en" />
	<meta http-equiv="imagetoolbar" content="false"/>
	<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />

	<link rel="shortcut icon" type="image/x-icon" href="http://www.componentart.com/favicon.ico" />
	<link rel="icon" type="image/x-icon" href="http://www.componentart.com/favicon.ico" />

	<title>TabStrip: SOA.UI Integration</title>

	<Demos:Css RunAt="server" Id="DemoCss" Index="true" />

	<link href="styles.css" type="text/css" rel="stylesheet" >

</head>
<body>
<form id="form1" runat="server">
	<Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="TabStrip: SOA.UI Integration" />

    <div class="demo-area">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
      <Services>
        <asp:ServiceReference
           path="~/Services/SoaSimpleTabStripService.svc/json" />
      </Services>
    </asp:ScriptManager>

      <ComponentArt:TabStrip id="TabStrip1"
            SoaService="ComponentArt.SOA.UI.ISoaTabStripService"
            CssClass="TopGroup"
            DefaultItemLookId="DefaultTabLook"
            DefaultSelectedItemLookId="SelectedTabLook"
            DefaultDisabledItemLookId="DisabledTabLook"
            DefaultGroupTabSpacing="1"
            ImagesBaseUrl="tabstrip_images/"
            runat="server">
      <ItemLooks>
        <ComponentArt:ItemLook LookId="DefaultTabLook" CssClass="DefaultTab" HoverCssClass="DefaultTabHover" LabelPaddingLeft="10" LabelPaddingRight="10" LabelPaddingTop="5" LabelPaddingBottom="4" LeftIconUrl="tab_left_icon.gif" RightIconUrl="tab_right_icon.gif" HoverLeftIconUrl="hover_tab_left_icon.gif" HoverRightIconUrl="hover_tab_right_icon.gif" LeftIconWidth="3" LeftIconHeight="21" RightIconWidth="3" RightIconHeight="21" />
        <ComponentArt:ItemLook LookId="SelectedTabLook" CssClass="SelectedTab" LabelPaddingLeft="10" LabelPaddingRight="10" LabelPaddingTop="4" LabelPaddingBottom="4" LeftIconUrl="selected_tab_left_icon.gif" RightIconUrl="selected_tab_right_icon.gif" LeftIconWidth="3" LeftIconHeight="21" RightIconWidth="3" RightIconHeight="21" />
      </ItemLooks>
      </ComponentArt:TabStrip>


    </div>

  <Demos:About RunAt="server" Id="DemoAbout" />
  <Demos:Footer RunAt="server" Id="DemoFooter" />
</form>
</body>
</html>