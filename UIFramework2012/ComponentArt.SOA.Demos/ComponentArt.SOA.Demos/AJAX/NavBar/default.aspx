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

	<title>NavBar: SOA.UI Integration</title>

	<Demos:Css RunAt="server" Id="DemoCss" Index="true" />

	<link href="styles.css" type="text/css" rel="stylesheet" >

</head>
<body>
<form id="form1" runat="server">
	<Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="NavBar: SOA.UI Integration" />

    <div class="demo-area">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
      <Services>
        <asp:ServiceReference
           path="~/Services/SimpleNavBarService.svc/json" />
      </Services>
    </asp:ScriptManager>

      <ComponentArt:NavBar id="NavBar1" Width="230" Height="400"
              CssClass="NavBar"              
              DefaultItemSpacing="3"
              DefaultItemLookId="TopItemLook"
              DefaultItemTextAlign="Left"
              DefaultSubItemTextAlign="Center"
              DefaultSubItemLookId="Level2ItemLook"
              ExpandSinglePath="true"
              FullExpand="true"
              ImagesBaseUrl="images/"
              ExpandTransition="Fade"
              ExpandDuration="500"
              CollapseTransition="Fade"
              CollapseDuration="500" 
              SoaService="ComponentArt.SOA.UI.ISoaNavBarService"
              ScrollUpImageUrl="scrollup.gif"
              ScrollDownImageUrl="scrolldown.gif"
              ScrollUpHoverImageUrl="scrollup_hover.gif"
              ScrollDownHoverImageUrl="scrolldown_hover.gif"
              ScrollUpActiveImageUrl="scrollup_active.gif"
              ScrollDownActiveImageUrl="scrolldown_active.gif"
              ScrollUpImageWidth="16"
              ScrollUpImageHeight="16"
              ScrollDownImageWidth="16"
              ScrollDownImageHeight="16"
              runat="server">
            <ItemLooks>
              <ComponentArt:ItemLook LookID="TopItemLook" CssClass="TopItem" HoverCssClass="TopItemHover" LabelPaddingLeft="15" />
              <ComponentArt:ItemLook LookID="Level2ItemLook" CssClass="Level2Item" HoverCssClass="Level2ItemHover"  />
              <ComponentArt:ItemLook LookID="EmptyLook" CssClass="Empty" />
            </ItemLooks>
      </ComponentArt:NavBar>

    </div>

  <Demos:About RunAt="server" Id="DemoAbout" />
  <Demos:Footer RunAt="server" Id="DemoFooter" />
</form>
</body>
</html>