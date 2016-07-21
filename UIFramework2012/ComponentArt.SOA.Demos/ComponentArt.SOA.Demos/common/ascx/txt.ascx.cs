namespace Demos.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for demo_header.
	/// </summary>
	public class About : System.Web.UI.UserControl {
		public string AboutText;
		public string AboutFile;
		public string DemoFolder;

		private void Page_Load(object sender, System.EventArgs e) {
			try {
				AboutFile = Request.FilePath.Substring(0,(Request.FilePath.LastIndexOf("/") + 1)) + "about.inc";

				//charting samples use differently named about files
				if (!File.Exists(MapPath(AboutFile)))
					AboutFile = Request.FilePath.Substring(0, (Request.FilePath.LastIndexOf("/") + 1)) + "AboutThisSample.html";

				FileStream StreamAbout = new FileStream(MapPath(AboutFile),FileMode.Open,FileAccess.Read);
				StreamReader ReaderAbout = new StreamReader(StreamAbout);
				AboutText = ReaderAbout.ReadToEnd();

				//again for charting samples
				if (AboutFile.EndsWith("AboutThisSample.html"))
					AboutText = "<p>" + AboutText + "</p>";

				StreamAbout.Close();
				if (!OnSite()) DemoFolder = @"<p>Full source code for this demo is located in the following folder:<br /><span class=""fld"">" + Server.MapPath(AboutFile).Replace("about.inc","").Replace("AboutThisSample.html","") + "</span></p>";
			}


			catch (Exception ex) { // grabs the page title in case the about file cannot be found
				//AboutText = "<p>" + (Page.FindControl("DemoHeader") as Header).DemoTitle + "</p>";
				AboutText = "<script type=\"text/javascript\">document.write(\"<p>\" + document.title + \"</p>\");</script>";
			}
		}

		private bool OnSite() {
			try {
				HttpContext context = HttpContext.Current;

				if (context.Request.Url.Host.IndexOf("aspnetajax.componentart.com") >= 0 || context.Request.Url.Host.IndexOf("aspnetajax-stage.componentart.com") >= 0 || context.Request.Url.Host.IndexOf("www.componentart.com") >= 0) return true;
				else return false;
			}

			catch (Exception ex) { return true; }
		}

		//////////////////////////////////
		override protected void OnInit(EventArgs e) { InitializeComponent();base.OnInit(e); }
		private void InitializeComponent() { this.Load += new System.EventHandler(this.Page_Load); }
	}
}
