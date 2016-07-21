<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ComponentArt.Silverlight.Demos.Web._default" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Silverlight.Server" Assembly="ComponentArt.Silverlight.Server" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%;">
<head id="Head1" runat="server">
    <title>ComponentArt Web.UI for Silverlight</title>
    <script type="text/javascript">
        //<![CDATA[
        function changed(sender, args) 
            {
                var toprect = sender.findName("TopBlue");
                var bottomrect = sender.findName("BottomBlue");
                var width = args.progress * 215 - 5;
                
                if (width < 0)
                    width = 0;
                    
                if (width > 202)
                    width = 202;

                toprect.Width = Math.round(width);
                bottomrect.Width = Math.round(width);
            }
            function get_version() 
            {
                return "0.0.0";
            }
    		//]]>
    </script>
    <style>
    body
    {
    	font-family:Arial;
    	font-size:12px;
    }
    a
    {
    	color:#0090C6;
    }
    h2
    {
    	font-size:20px;
    	font-weight:bold;
    	color:#0090C6;
    }
    h3
    {
    	font-size:16px;
    	font-weight:bold;
    	color:#99999A;
    }
    </style>
</head>
<body style="height:100%;margin:0;" scroll="no">
    <form id="form1" runat="server" style="height:100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div style="position: relative; height: 0px;">
            <ComponentArt:SilverlightUpload ID="SilverlightUpload1" RunAt="server" 
                MaximumFileCount="8"
                MaximumUploadSize="0"
                TempFileFolder="~/uploadtemp" 
                DestinationFolder="~/upload" 
                OverwriteExistingFiles="true"
                SilverlightObjectID="Xaml1">
            </ComponentArt:SilverlightUpload>
        </div>
        <div  style="height:100%;">
            <object type="application/x-silverlight-2" data="data:application/x-silverlight-2," id="Xaml1" style="height:100%;width:100%;" >
	            <param name="MinRuntimeVersion" value="4.0.0.0"></param>
	            <param name="Windowless" value="True"></param>
	            <param name="onsourcedownloadprogresschanged" value="changed"></param>
	            <param name="splashscreensource" value="splash.xaml"></param>
	            <param name="Source" value="ClientBin/ComponentArt.Silverlight.Demos.xap" ></param>
	            <a href="http://go2.microsoft.com/fwlink/?LinkID=114576&amp;v=2.0">
		            <img src="http://go2.microsoft.com/fwlink/?LinkID=108181" alt="Get Microsoft Silverlight" style="border-width:0;" />
	            </a>
            </object>
        </div>
    </form>
</body>
</html>
