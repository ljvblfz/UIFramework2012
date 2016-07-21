namespace Demos.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	public class Header : System.Web.UI.UserControl {
		public string DemoType;
		public string DemoTitle;
		public string SelectedNode;
		public bool ShowCode;

		public String ProductListClass;

		public ComponentArt.Web.UI.Menu DemoMenu;
		public Literal RootPath;
		public Literal ImagePreloader;
		public String MajorVersion = "20121";

		public Literal CodeButton;

		private void Page_Load(object sender, System.EventArgs e) {
			ProductListClass = (OnSite()) ? "" : "local";

			if (Request.ServerVariables["HTTP_USER_AGENT"].ToString().IndexOf("MSIE 6") != -1) {
				// Preload the GIFs in place of the 32-bit PNGs
				InitImagePreloader(false); // true = 32-bit PNGs, false = GIFs

				// Shadow, since it doesn't support 32-bit PNGs
				DemoMenu.ShadowEnabled = true;
				DemoMenu.ShadowOffset = 2;
				DemoMenu.ShadowColor = Color.FromArgb(128,128,128);
			} else {
				// Initialise the preloader for CSS-referenced images
				InitImagePreloader(true); // true = 32-bit PNGs, false = GIFs
			}

			// Get the working root
			//RootPath.Text = "root = \"" + FixUrl("~") + "\";";
		}

		// Returns a fully-resolved path
		public static string FixUrl(string url) {
			string path = url;
			if (url.StartsWith("~")) path = (HttpContext.Current.Request.ApplicationPath + url.Substring(1)).Replace("//","/");
			if (path == "/") path = "";
			return path;
		}

		// Populate the image preloader DIV
		public void InitImagePreloader(bool Pngs) {
			string ImageRoot = FixUrl("~/common/img/" + MajorVersion + "/");
			StringBuilder PreloaderOutput = new StringBuilder();

			//	8-bit / 24-bit PNGs
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "spc.png\" width=\"1\" height=\"1\" />");
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "dem/bg.png\" width=\"1\" height=\"1\" />");
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "dem/full.png\" width=\"1\" height=\"1\" />");
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "hdr/static.png\" width=\"1\" height=\"1\" />");
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "nav/bg.png\" width=\"1\" height=\"1\" />");
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "nav/col.png\" width=\"1\" height=\"1\" />");
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "nav/exp.png\" width=\"1\" height=\"1\" />");
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "override/bg.png\" width=\"1\" height=\"1\" />");
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "txt/bg.png\" width=\"1\" height=\"1\" />");
			PreloaderOutput.AppendFormat("\n\t");
			PreloaderOutput.Append("<img src=\"" + ImageRoot + "txt/full.png\" width=\"1\" height=\"1\" />");

			if (!Pngs) { // Preload GIFS rather than PNGs for IE6
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "horizontal.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "static.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "vertical.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/bob/static.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/bob/vertical.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/ctrl/static.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/ctrl/vertical.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/des/static.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/des/vertical.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/int/static.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/int/vertical.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/tech/static.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/tech/vertical.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "hdr/logo.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "nav/static.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "override/horizontal.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "override/static.gif\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n");
			} else { // 32-bit PNGs
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "horizontal.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "static.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "vertical.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/bob/static.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/bob/vertical.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/ctrl/static.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/ctrl/vertical.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/des/static.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/des/vertical.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/int/static.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/int/vertical.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/tech/static.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "ddn/tech/vertical.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "hdr/logo.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "nav/static.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "override/horizontal.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n\t");
				PreloaderOutput.Append("<img src=\"" + ImageRoot + "override/static.png\" width=\"1\" height=\"1\" />");
				PreloaderOutput.AppendFormat("\n");
			}

			ImagePreloader.Text = PreloaderOutput.ToString();
		}

		private bool OnSite() {
			HttpContext context = HttpContext.Current;
			if ((context.Request.Url.Host != null) && (context.Request.Url.Host.IndexOf("componentart.com") >= 0 || context.Request.Url.Host.IndexOf("www.componentart.com") >= 0)) return true;
			// returns false if local
			return false;
		}

		//////////////////////////////////
		override protected void OnInit(EventArgs e) { InitializeComponent();base.OnInit(e); }
		private void InitializeComponent() { this.Load += new System.EventHandler(this.Page_Load); }
	}
}
