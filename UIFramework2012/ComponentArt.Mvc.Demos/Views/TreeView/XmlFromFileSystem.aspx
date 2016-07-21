<%@ Page Language="C#" AutoEventWireUp="true" %>
<%@ import Namespace="System.Threading" %>
<%@ import Namespace="System.IO" %>
<%@ import Namespace="ComponentArt.Web.UI" %>
<% Response.ContentType = "text/xml"; %>
<script language="C#" runat="server">
void Page_Load(Object sender,EventArgs e)
{
  ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();
  string _root = Directory.Exists(Server.MapPath("~/demos")) ? "~/demos" : "~";

  string dirPath = Request.QueryString["dir"];

  // Don't allow browsing  the file system outside of the app root
  if (dirPath.StartsWith(Request.MapPath(_root)))
  {
    string[] subDirectories = Directory.GetDirectories(dirPath);
    foreach (string directory in subDirectories)
    {
      string[] parts = directory.Split('\\');
      string name = parts[parts.Length-1];
      ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode ();
      node.Text = name;
      node.ContentCallbackUrl = "../XmlFromFileSystem/?dir=" + Server.UrlEncode(directory);
      TreeView1.Nodes.Add(node);
    }

    string[] files = Directory.GetFiles(dirPath);
    foreach (string file in files)
    {
      string[] parts = file.Split('\\');
      string name = parts[parts.Length-1];
      ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
      node.Text = name;
      FileInfo fi = new FileInfo(file);
      switch (fi.Extension.ToLower())
      {
        case ".dll" : node.ImageUrl = "dll.gif"; break;
        case ".aspx" : node.ImageUrl = "aspx.gif"; break;
        case ".ascx" : node.ImageUrl = "ascx.gif"; break;
        case ".cs" : node.ImageUrl = "cs.gif"; break;
        case ".vb" : node.ImageUrl = "vb.gif"; break;
        case ".config" : node.ImageUrl = "config.gif"; break;
        case ".gif" : node.ImageUrl = "image.gif"; break;
        case ".jpg" : node.ImageUrl = "image.gif"; break;
        case ".xml" : node.ImageUrl = "xml.gif"; break;
        case ".js" : node.ImageUrl = "js.gif"; break;
        case ".css" : node.ImageUrl = "css.gif"; break;
      }
      TreeView1.Nodes.Add(node);
    }
    Response.Write(TreeView1.GetXml());
  }
}
</script>
