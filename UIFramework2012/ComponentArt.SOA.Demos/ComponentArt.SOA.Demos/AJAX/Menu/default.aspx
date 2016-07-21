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

	<title>Menu: SOA.UI Integration</title>

	<Demos:Css RunAt="server" Id="DemoCss" Index="true" />

	<link href="styles.css" type="text/css" rel="stylesheet" >

  <script type="text/javascript">

    function Menu1_webServiceComplete(sender, eventArgs) {
      // set top-level item styles
      sender.beginUpdate();
      for (var i = 0; i < sender.get_items().get_length(); i++) {
        sender.get_items().getItem(i).setProperty('LookId', 'TopItemLook');
      }
      sender.endUpdate();
    }
  
  </script>

</head>
<body>
<form id="form1" runat="server">
	<Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="Menu: SOA.UI Integration" />

    <div class="demo-area">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
      <Services>
        <asp:ServiceReference
           path="~/Services/SoaMenuDemoService.svc/json" />
      </Services>
    </asp:ScriptManager>

        <ComponentArt:Menu id="Menu1" runat="server" Width="80" 
        SoaService="ComponentArt.SOA.UI.ISoaMenuService" 
        CssClass="TopMenuGroup"
        DefaultGroupCssClass="MenuGroup"
        DefaultGroupItemSpacing="1"
        TopGroupItemSpacing="1"
        ExpandDelay="100"
        ImagesBaseUrl="images/"
        DefaultItemLookId="DefaultItemLook">          
          <ItemLooks>
            <ComponentArt:ItemLook LookID="TopItemLook" CssClass="TopMenuItem" HoverCssClass="TopMenuItemHover" ExpandedCssClass="TopMenuItemExpanded" LabelPaddingLeft="15" LabelPaddingRight="15" LabelPaddingTop="2" LabelPaddingBottom="2" />
            <ComponentArt:ItemLook LookID="DefaultItemLook" CssClass="MenuItem" HoverCssClass="MenuItemHover" ExpandedCssClass="MenuItemHover" LeftIconWidth="16" LeftIconHeight="16" LabelPaddingLeft="10" LabelPaddingRight="10" LabelPaddingTop="3" LabelPaddingBottom="4" />
            <ComponentArt:ItemLook LookID="BreakItem" CssClass="MenuBreak" />
          </ItemLooks>
          <ClientEvents>
            <WebServiceComplete EventHandler="Menu1_webServiceComplete" />
          </ClientEvents>
        </ComponentArt:Menu>

    </div>

  <Demos:About RunAt="server" Id="DemoAbout" />
  <Demos:Footer RunAt="server" Id="DemoFooter" />
</form>
</body>
</html>