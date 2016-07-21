namespace Demos.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	public class Css : System.Web.UI.UserControl {
		public Literal MetaTags;
		public Literal Favicons;
		public Literal CommonCss;
		public Literal Ie6Css;

		public String MajorVersion = "20121";

		private void Page_Load(object sender, System.EventArgs e) {
			Favicons.Text = GetFavicons();
			MetaTags.Text = GetMetaTags();

			string CssPath = FixUrl("~/common/css/");
			string CssPrefix = "<link href=\"" + CssPath;
			string CssSuffix = "?" + MajorVersion + "\" type=\"text/css\" rel=\"stylesheet\" />";

			CommonCss.Text = CssPrefix + "demos.css" + CssSuffix;

			if (Request.ServerVariables["HTTP_USER_AGENT"].ToString().IndexOf("MSIE 6") != -1) Ie6Css.Text = CssPrefix + "ie6.css" + CssSuffix;
		}

		public String GetMetaTags() {
			StringBuilder Meta = new StringBuilder();

			Meta.AppendFormat("\n\t");
			Meta.Append("<meta http-equiv=\"imagetoolbar\" content=\"false\" />");
			Meta.AppendFormat("\n\t");
			Meta.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=EmulateIE7\" />");
			Meta.AppendFormat("\n");

			return Meta.ToString();
		}

		public String GetFavicons() {
			String Root = FixUrl("~/");
			StringBuilder Fav = new StringBuilder();

			Fav.AppendFormat("\n\t");
			Fav.Append("<link rel=\"shortcut icon\" type=\"image/x-icon\" href=\"" + Root + "favicon.ico\" />");
			Fav.AppendFormat("\n\t");
			Fav.AppendFormat("<link rel=\"icon\" type=\"image/x-icon\" href=\"" + Root + "favicon.ico\" />");
			Fav.AppendFormat("\n");

			return Fav.ToString();
		}

		//	Returns a fully-resolved path
		public static string FixUrl(string url) {
			string path = url;
			if (url.StartsWith("~")) path = (HttpContext.Current.Request.ApplicationPath + url.Substring(1)).Replace("//","/");
			if (path == "/") path = "";
			return path;
		}

		//////////////////////////////////
		override protected void OnInit(EventArgs e) { InitializeComponent();base.OnInit(e); }
		private void InitializeComponent() { this.Load += new System.EventHandler(this.Page_Load); }
	}
}
