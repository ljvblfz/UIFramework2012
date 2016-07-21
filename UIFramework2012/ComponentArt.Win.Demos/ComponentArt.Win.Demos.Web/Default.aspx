<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head id="Head1" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<meta http-equiv="Content-language" content="en" />
	<meta http-equiv="imagetoolbar" content="false"/>

	<title>ComponentArt Win.UI for WPF &ndash; Live Demos</title>

<style type="text/css">
a:active,a:focus { outline:none; }
body { background:#000;font-family:Tahoma,"Lucida Grande",Verdana,Arial,Helvetica,sans-serif;font-size:12px;line-height:20px;padding:0;margin:0;color:#fff; }
#main { width:100%;height:400px;background:#3d025a url("images/horizontal.png") repeat-x 0 bottom;padding:10% 0 0; }
	#main .con { width:720px;height:100%;margin:0 auto;background:none; }
		#main .con h1 { width:393px;height:87px;background:transparent url("images/branding-2012.png") no-repeat;text-indent:-10000px;margin:0 0 10px 42px;padding:0; }

		#main .con .sep { width:716px;height:2px;line-height:0;font-size:0;overflow:hidden;background:#f0f;background:transparent url("images/static.png") no-repeat 50% bottom }

		#main .con p { margin:15px 0 0 90px;width:410px; }

		#main .con ul { margin:0 0 14px 90px;padding:0;list-style:none; }
			#main .con ul li { background:transparent url("images/static.png") no-repeat;height:20px;line-height:20px;padding:0 0 0 18px; }

		#main .con a { display:block;height:34px;line-height:34px;font-size:16px;font-weight:bold;background:transparent url("images/static.png") no-repeat;color:#fff;text-indent:-10000px;margin:0 0 0 90px; }
		#main .con a.dem { width:268px;background-position:0 -30px;margin-bottom:25px; }
		#main .con a.dem:hover { background-position:-268px -30px; }
		#main .con a.dl { width:286px;background-position:0 -64px;margin-top:16px; }
		#main .con a.dl:hover { background-position:-286px -64px; }
</style>
<!--[if lt IE 7]>
<style type="text/css">
#main .con h1 { background-image:url("images/branding-2012.gif"); }
#main .con .sep { background:#cc9cf6;height:1px;margin:0 0 1px; }
#main .con ul li { background-image:url("images/static.gif"); }
#main .con a { background-image:url("images/static.gif"); }
</style>
<![endif]-->
</head>

<body id="wpf-launch">
<form id="form1" runat="server">
	<div id="main">
		<div class="con">
			<h1>Win.UI for WPF &ndash; Live Demos</h1>

			<div class="sep"></div>

			<p>You are about to launch an in-browser WPF application. You will require:</p>
			<ul>
				<li>Microsoft Internet Explorer 7 or higher</li>
				<li>Microsoft WPF runtime</li>
			</ul>

			<a href="xbap/ComponentArt.Win.Demos.xbap" onclick="this.blur();" class="dem">Launch Live Demos Now</a>

			<div class="sep"></div>

			<p>Alternatively, you may download and install the UI Framework distribution package, which includes a desktop version of the WPF demos.</p>

			<a href="http://www.componentart.com/download/" onclick="this.blur();" class="dl">Download UI Framework</a>
		</div>
	</div>
</form>
</body>
</html>
