using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

using ComponentArt.SOA.UI;
using System.IO;
using System.Web;

namespace ComponentArt.Silverlight.Demos.Web
{
    //[ServiceContract(Namespace = "")]  // already declared in ISoaTreeViewService
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TreeViewFileBrowserService: SoaTreeViewService
    {
        public override SoaTreeViewGetNodesResponse GetNodes(SoaTreeViewGetNodesRequest request)
        {
            SoaTreeViewGetNodesResponse response = new SoaTreeViewGetNodesResponse();

            // initial load?
            if (request.ParentNode == null)
            {
                // Start with the website root folder
                SoaTreeViewNode rootNode = new SoaTreeViewNode();
                rootNode.Text = "Sample Files";
                rootNode.Id = "\\";
                rootNode.IsLoadOnDemandEnabled = true;
                rootNode.IconSource = _serverPath + "folder.png";
                //rootNode.ExpandedIconSource = _serverPath + "folder_open.png";
                rootNode.IsExpanded = true;

                response.Nodes = new List<SoaTreeViewNode>();
                response.Nodes.Add(rootNode);

                // Populate one level down
                rootNode.Items = GetFileNodes(_root, request.Tag is string && ((string)request.Tag).Contains("FoldersOnly"));
            }
            else
            {
                string dirPath = new DirectoryInfo(_root + request.ParentNode.Id).FullName;

                // Don't allow browsing  the file system outside of the app root
                if (dirPath.StartsWith(_root))
                {
                    response.Nodes = GetFileNodes(dirPath, request.Tag is string && ((string)request.Tag).Contains("FoldersOnly"));
                }
            }

            return response;

        }

        private string _serverPath
        {
            get
            {
              return new Uri(HttpContext.Current.Request.Url, IMG_PATH).AbsoluteUri;
            }
        }

        private string _root
        {
            get
            {
                string _root = HttpContext.Current.Server.MapPath("~");
                _root = Directory.Exists(_root) ? _root : Directory.GetCurrentDirectory();
                return _root.TrimEnd(new char[] { '\\', ' ' });
            }
        }

        private const string IMG_PATH = "ClientBin/controls/treeview/icons/FileBrowser/";


        private List<SoaTreeViewNode> GetFileNodes(string dirPath, bool bFoldersOnly)
        {
            List<SoaTreeViewNode> arList = new List<SoaTreeViewNode>();

            string[] subDirectories = Directory.GetDirectories(dirPath);
            foreach (string directory in subDirectories)
            {
                string[] parts = directory.Split('\\');
                string name = parts[parts.Length - 1];

                SoaTreeViewNode node = new SoaTreeViewNode();
                node.Text = name;
                node.Id = directory.Substring(_root.Length);
                node.IsLoadOnDemandEnabled = true;
                node.IconSource = _serverPath + "folder.png";

                arList.Add(node);
            }

            if (!bFoldersOnly)
            {
                string[] files = Directory.GetFiles(dirPath);
                foreach (string file in files)
                {
                    string[] parts = file.Split('\\');
                    string name = parts[parts.Length - 1];

                    SoaTreeViewNode node = new SoaTreeViewNode();
                    node.Text = name;

                    FileInfo fi = new FileInfo(file);
                    switch (fi.Extension.ToLower())
                    {
                        case ".dll": node.IconSource = _serverPath + "dll.png"; break;
                        case ".aspx": node.IconSource = _serverPath + "aspx.png"; break;
                        case ".ascx": node.IconSource = _serverPath + "ascx.png"; break;
                        case ".cs": node.IconSource = _serverPath + "cs.png"; break;
                        case ".vb": node.IconSource = _serverPath + "vb.png"; break;
                        case ".config": node.IconSource = _serverPath + "config.png"; break;
                        case ".png": node.IconSource = _serverPath + "image.png"; break;
                        case ".jpg": node.IconSource = _serverPath + "image.png"; break;
                        case ".xml": node.IconSource = _serverPath + "xml.png"; break;
                        case ".js": node.IconSource = _serverPath + "js.png"; break;
                        case ".css": node.IconSource = _serverPath + "css.png"; break;
                        default: node.IconSource = _serverPath + "file.png"; break;
                    }

                    arList.Add(node);
                }
            }

            return arList;
        }
    }
}
