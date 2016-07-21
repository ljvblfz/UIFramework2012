<%@ Page Language="C#" %>
<%@ Register TagPrefix="Demos" TagName="Css" Src="~/common/ascx/css.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Header" Src="~/common/ascx/hdr.ascx"%>
<%@ Register TagPrefix="Demos" TagName="About" Src="~/common/ascx/txt.ascx"%>
<%@ Register TagPrefix="Demos" TagName="Footer" Src="~/common/ascx/ftr.ascx"%>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

	<title>SOA.UI Live Demos: Overview</title>

	<Demos:Css RunAt="server" Id="DemoCss" Override="true" />

<style type="text/css">
#mn #txt { display:none; }

#idx { padding:15px 0 0 20px;width:auto;height:620px; }

	#idx .soa { width:332px;height:520px;margin:0 10px 0 0;float:left; }
		#idx .soa h1 { margin:0;padding:0;text-indent:-10000px;width:332px;height:293px;background:transparent url("common/img/20121/idx/static.png") no-repeat; }
		#idx .soa h2 { margin:0;padding:0;text-indent:-10000px;width:332px;height:55px;background:transparent url("common/img/20121/idx/static.png") no-repeat 0 -293px; }
		#idx .soa p { margin:0;padding:10px 0 0 30px;line-height:16px;color:#333;width:270px; }
		#idx .soa a { display:block;width:219;height:56px;margin:21px 0 0 84px;text-indent:-10000px;background:transparent url("common/img/20121/idx/static.png") no-repeat 0 -348px; }
		#idx .soa a:hover { background-position:0 -404px }

	#idx .dem { margin:0 0 0 352px;width:696px; }
		#idx .dem .int { width:696px;height:147px;background:transparent url("common/img/20121/idx/horizontal.png") repeat-x 0 31px; }
			#idx .dem .int h2 { width:100%;height:31px;margin:0;padding:0;line-height:31px;font-size:20px;font-weight:bold;background:transparent url("common/img/20121/idx/static.png") no-repeat -332px 0;text-indent:-10000px; }
				#idx .dem .int div.img { float:right;width:356px;height:113px;position:relative; }
					#idx .dem .int div.img img { width:346px;height:100px;background:#eee;position:absolute;top:-14px; }
					#idx .dem .int div.img a { display:block;width:100px;height:24px;line-height:23px;text-transform:uppercase;text-align:center;position:absolute;bottom:14px;font-size:10px;right:0;font-weight:bold;color:#fff;background:transparent url("common/img/20121/idx/static.png") no-repeat; }
					#idx .dem .int div.img a.ajax { right:100px;background-position:-332px -31px;border-right:1px solid #fff; }
					#idx .dem .int div.img a.sl { background-position:-432px -31px; }

					#idx .dem .int div.img a:hover { text-decoration:none; }
					#idx .dem .int div.img a.ajax:hover { background-position:-332px -55px; }
					#idx .dem .int div.img a.sl:hover { background-position:-432px -55px; }

				#idx .dem .int p { margin:0;padding:10px 10px 0;width:300px;line-height:16px;color:#333;font-size:11px; }
					#idx .dem .int p strong { display:block;font-size:13px;font-weight:bold; }

		#idx .dem ul { margin:0;padding:0;list-style:none; }
			#idx .dem ul li { line-height:13px;color:#333;margin:0;cursor:default; }
			#idx .dem ul li strong { display:block;font-size:13px;font-weight:bold;line-height:18px; }
			#idx .dem ul li p { margin:0;padding:0 0 0 2px;line-height:13px;font-size:10px; }

			#idx .dem ul li a { font-size:10px;font-weight:bold;color:#fff;text-transform:uppercase;padding:0;background:#dd3409;line-height:14px; }
			#idx .dem ul li a.sl { background:#00aef0; }

			#idx .dem ul li a:hover { text-decoration:none;background:#ff5d06; }
			#idx .dem ul li a.sl:hover { background:#52cef2; }


		#idx .dem .smp { width:275px;height:389px;float:left;background:transparent url("common/img/20121/idx/horizontal.png") repeat-x 0 -194px; }
			#idx .dem .smp h2 { width:100%;height:31px;margin:0;padding:0;line-height:31px;font-size:20px;font-weight:bold;background:transparent url("common/img/20121/idx/static.png") no-repeat -332px -79px;text-indent:-10000px; }
			#idx .dem .smp li { height:40px;padding:0 0 0 45px;margin:10px 0 6px;background:transparent url("common/img/20121/idx/static.png") no-repeat; }
				#idx .dem .smp li.dg { background-position:-986px -128px; }
				#idx .dem .smp li.tv { background-position:-986px -176px; }
				#idx .dem .smp li.mnu { background-position:-986px -226px; }
				#idx .dem .smp li.cb { background-position:-986px -275px; }
				#idx .dem .smp li.nb { background-position:-986px -325px; }
				#idx .dem .smp li.ts { background-position:-986px -372px; }
				#idx .dem .smp li.tb { background-position:-986px -422px; }

				#idx .dem .smp li a { float:left;border:1px solid #fff;width:95px;text-align:center;height:15px; }
				#idx .dem .smp li a.sl { border-left:none; }

		#idx .dem .adv { width:401px;height:389px;margin:0 0 0 20px;background:transparent url("common/img/20121/idx/horizontal.png") repeat-x 0 -194px;float:left; }
			#idx .dem .adv h2 { width:100%;height:31px;margin:0;padding:0;line-height:31px;font-size:20px;font-weight:bold;background:transparent url("common/img/20121/idx/static.png") no-repeat -607px -79px;text-indent:-10000px; }
				#idx .dem .adv li { height:48px;padding:0;margin:10px 0 0 10px; }
				#idx .dem .adv li strong { display:block;font-size:13px;font-weight:bold;line-height:14px; }
				#idx .dem .adv li p { line-height:14px;padding:0 0 2px 2px; }
				#idx .dem .adv li a { float:left;border:1px solid #fff;width:95px;text-align:center;height:15px;margin:0px 0 0; }
				#idx .dem .adv li a.sl { border-left:none; }
</style>

<!--[if lt IE 7]>
<style type="text/css">
#idx .soa h1 { background-image:url("common/img/20121/idx/static.gif"); }
#idx .soa h2 { background-image:url("common/img/20121/idx/static.gif"); }
#idx .soa a { background-image:url("common/img/20121/idx/static.gif"); }
#idx .dem { margin:0 0 0 347px; }
#idx .dem .int h2 { background-image:url("common/img/20121/idx/static.gif"); }
#idx .dem .int div.img a { background-aimge:url("common/img/20121/idx/static.gif"); }
#idx .dem .smp h2 { background-image:url("common/img/20121/idx/static.gif"); }
#idx .dem .smp li { background-image:url("common/img/20121/idx/static.gif"); }
#idx .dem .adv h2 { background-image:url("common/img/20121/idx/static.gif"); }
</style>
<![endif]-->
</head>

<body id="soa-componentart-com">
<form id="form1" runat="server">
	<Demos:Header RunAt="server" Id="DemoHeader" DemoTitle="Overview" />

<div id="idx">
	<div class="soa">
		<h1>ComponentArt SOA.UI for .NET</h1>
		<h2>Introducing Service Oriented Architecture User Interfaces.</h2>

		<p>ComponentArt SOA.UI for .NET is a framework for building web services specialized to serve the user interface tier. It's the industry's first framework to enable full server-side code reuse between ASP.NET AJAX and Silverlight front ends.</p>

		<a href="http://www.componentart.com" title="UI Framework for .NET" target="_blank">Part of ComponentArt UI Framework for .NET</a>
	</div>

	<div class="dem">
		<div class="int">
			<h2>Control Integration</h2>
			<div class="img">
				<img src="common/img/20121/idx/int.png" width="346" height="100" />
				<a href="ajax/filebrowser" onclick="this.blur();" class="ajax" title="ASP.NET AJAX">ASP.NET AJAX</a>
				<a href="silverlight/#FileExplorer" onclick="this.blur();" class="sl" title="Silverlight">Silverlight</a>
			</div>
			<p><strong>File Explorer</strong>This sample demonstrates integration between TreeView and Grid controls as they communicate with each other and SOA.UI services in a "file explorer" application.</p>
		</div>

		<div class="smp">
			<h2>Simple SOA.UI Loading</h2>
			<ul>
				<li class="dg">
					<p><strong>DataGrid</strong></p>
					<a href="ajax/datagridsimpleloading/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#DataGridSimpleLoading" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li class="tv">
					<p><strong>TreeView</strong></p>
					<a href="ajax/treeview/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#TreeViewSimpleLoading" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li class="mnu">
					<p><strong>Menu</strong></p>
					<a href="ajax/menu/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#Menu" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li class="cb">
					<p><strong>ComboBox</strong></p>
					<a href="ajax/combobox/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#ComboBox" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li class="nb">
					<p><strong>NavBar</strong></p>
					<a href="ajax/navbar/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#NavBar" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li class="ts">
					<p><strong>TabStrip</strong></p>
					<a href="ajax/tabstrip/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#TabStrip" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li class="tb">
					<p><strong>ToolBar</strong></p>
					<a href="ajax/toolbar/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#ToolBar" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>
			</ul>
		</div>

		<div class="adv">
			<h2>Advanced SOA.UI Features</h2>
			<ul>
				<li>
					<p><strong>DataGrid: Automatic Paging</strong>Automatic paging, sorting and grouping using SOA.UI.</p>
					<a href="ajax/datagridmessagebrowser/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#DataGridMessageBrowser" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li>
					<p><strong>DataGrid: Automatic Scrolling</strong>Automatic scrolling, sorting and grouping using SOA.UI.</p>
					<a href="ajax/datagridsoascrolling" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#DataGridSoaScrolling" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li>
					<p><strong>DataGrid: Data Editing</strong>Automatic update, insert and delete using SOA.UI.</p>
					<a href="ajax/datagridsoaediting/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#DataGridSoaEditing" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li>
					<p><strong>TreeView: Load-On-Demand</strong>TreeView Load on Demand, using SOA.UI.</p>
					<a href="ajax/treeviewfilebrowser/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#TreeViewFileBrowser" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li>
					<p><strong>TreeView: Structure Manipulation</strong>Automatic tree manipulation using SOA.UI.</p>
					<a href="ajax/treeviewstructuremanipulation" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#TreeViewStructureEdit" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>

				<li>
					<p><strong>ComboBox: Automatic Filtering</strong>Automatic on-keypress filtering using SOA.UI.</p>
					<a href="ajax/comboboxfiltering/" onclick="this.blur();" title="ASP.NET AJAX" class="ajax">ASP.NET AJAX</a>
					<a href="silverlight/#ComboBoxFiltering" onclick="this.blur();" title="Silverlight" class="sl">Silverlight</a>
				</li>
			</ul>
		</div>
	</div>
</div><!-- /index -->

	<Demos:About RunAt="server" Id="DemoAbout" />
	<Demos:Footer RunAt="server" Id="DemoFooter" />
</form>
</body>
</html>
