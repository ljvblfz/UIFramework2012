<%@ Control Language="C#" AutoEventWireup="false" Src="hdr.ascx.cs" Inherits="Demos.Controls.Header" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<div id="bd"><%-- body --%>

<%-- Preload the images referenced by CSS --%><div id="image-preloader" style="position:absolute;z-index:98765;top:-10000px;left:-10000px;overflow:hidden;"><Asp:Literal Id="ImagePreloader" RunAt="server" /></div><%-- /Preload the images referenced by CSS --%>

	<div id="hd"><%-- header --%>

		<ul id="prod" class="<%= ProductListClass %>"><%-- product links --%>
			<li class="uif"><a href="http://www.componentart.com/products/uiframework/" target="_blank">UI Framework for .NET</a></li>
			<li class="sl"><a href="http://silverlight.componentart.com/" target="_blank">Silverlight</a></li>
			<li class="ajax"><a href="http://aspnetajax.componentart.com/" target="_blank">ASP.NET AJAX</a></li>
			<li class="soa">SOA.UI</li>
			<li class="dl"><a href="http://www.componentart.com/download/" target="_blank">Download</a></li>
			<li class="lnk"><a href="http://www.componentart.com/" target="_blank">www.componentart.com</a></li>
		</ul><%-- /product links --%>

		<Asp:Hyperlink RunAt="server" Id="Logo" NavigateUrl="~/" CssClass="logo">SOA.UI</Asp:Hyperlink><%-- /demo index --%>

		<div id="mnu"><%-- main menu --%>
			<div class="l"></div>
			<div class="m">

				<ComponentArt:Menu
					Id="DemoMenu"
					RunAt="server"
					SiteMapXmlFile="~/common/xml/demos.xml"
					CssClass="nil"
					ExpandOnClick="false"
					ShadowEnabled="false"
					ExpandSlide="ExponentialDecelerate"
					CollapseSlide="ExponentialAccelerate"
					DefaultGroupExpandOffsetY="0"
					KeyboardEnabled="false"
				>
					<ItemLooks>
						<ComponentArt:ItemLook LookId="ItemSeparator" CssClass="mnu-sep" />
						<ComponentArt:ItemLook LookId="ItemAjax" CssClass="mnu-ajax" HoverCssClass="mnu-ajax" ActiveCssClass="mnu-ajax" ExpandedCssClass="mnu-ajax-h" />
						<ComponentArt:ItemLook LookId="ItemSilverlight" CssClass="mnu-sl" HoverCssClass="mnu-sl" ActiveCssClass="mnu-sl" ExpandedCssClass="mnu-sl-h" />
					</ItemLooks>

					<ClientTemplates>
						<ComponentArt:ClientTemplate ID="TopLevelItemTemplate">
							<a href="javascript:void(0);" onclick="this.blur();">## DataItem.getProperty('Text'); ##</a>
						</ComponentArt:ClientTemplate>

						<ComponentArt:ClientTemplate ID="SpacerTemplate">
							<div class="mnu-spc"></div>
						</ComponentArt:ClientTemplate>

						<ComponentArt:ClientTemplate Id="AjaxTitleTemplate">
							<div class="ttl-ajax">
								<h3 class="## DataItem.getProperty('Class'); ##">## DataItem.getProperty('Text'); ##</h3>
							</div>
						</ComponentArt:ClientTemplate>

						<ComponentArt:ClientTemplate Id="AjaxItemTemplate">
							<div class="itm-ajax">
								<a href="## DataItem.getProperty('NavigateUrl'); ##" onclick="this.blur();return false;">## DataItem.getProperty('Text'); ##</a>
							</div>
						</ComponentArt:ClientTemplate>

						<ComponentArt:ClientTemplate Id="SilverlightTitleTemplate">
							<div class="ttl-sl">
								<h3 class="## DataItem.getProperty('Class'); ##">## DataItem.getProperty('Text'); ##</h3>
							</div>
						</ComponentArt:ClientTemplate>

						<ComponentArt:ClientTemplate Id="SilverlightItemTemplate">
							<div class="itm-sl">
								<a href="## DataItem.getProperty('NavigateUrl'); ##" onclick="this.blur();return false;">## DataItem.getProperty('Text'); ##</a>
							</div>
						</ComponentArt:ClientTemplate>

					</ClientTemplates>
				</ComponentArt:Menu>

			</div>
			<div class="r"></div>
		</div><%-- /main menu --%>
	</div><%-- /header --%>

	<div id="ttl"><%-- demo title --%>
		<div class="l">
			<h2>SOA.UI Live Demos</h2>
		</div>
		<div class="m">
			<h1><%= DemoTitle %></h1>
		</div>
		<div class="r"></div>
	</div><%-- /demo title --%>


	<div id="con"><%-- page content --%>
		<div id="mn"><%-- main content --%>
			<div id="dem"><%-- demo wrapper --%>
				<div class="DemoContainer" id="DemoArea"><%-- /demo proper --%>