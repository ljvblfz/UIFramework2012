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

	<title>ToolBar: SOA.UI Integration</title>

	<Demos:Css RunAt="server" Id="DemoCss" Index="true" />

	<link href="styles.css" type="text/css" rel="stylesheet" >

</head>
<body>
<form id="form1" runat="server">
	<Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="ToolBar: SOA.UI Integration" />

    <div class="demo-area">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
      <Services>
        <asp:ServiceReference
           path="~/Services/SoaSimpleToolBarService.svc/json" />
      </Services>
    </asp:ScriptManager>

        <ComponentArt:ToolBar ID="ToolBar1" 
            CssClass="toolbar"
            ImagesBaseUrl="./"
            DefaultItemCssClass="itm"
            DefaultItemHoverCssClass="itm-h"
            DefaultItemActiveCssClass="itm-a"
            DefaultItemCheckedCssClass="itm-a"
            DefaultItemCheckedHoverCssClass="itm-a"
            DefaultItemCheckedActiveCssClass="itm-a"
            DefaultItemTextImageSpacing="0"
            DefaultItemTextImageRelation="ImageBeforeText"
            DefaultImageHeight="16"
            DefaultImageWidth="16"
            ItemSpacing="2"
            Orientation="Horizontal"
            SoaService="ComponentArt.SOA.UI.ISoaToolBarService"
            runat="server"
            >
        </ComponentArt:ToolBar>
        
    </div>

  <Demos:About RunAt="server" Id="DemoAbout" />
  <Demos:Footer RunAt="server" Id="DemoFooter" />
</form>
</body>
</html>