using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Web;

namespace ComponentArt.Silverlight.Demos.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ManualTreeViewFileExplorerService
    {
        [OperationContract]
        public List<MyTreeViewNodeData> GetNodes(string parentNodePath)
        {

            List<MyTreeViewNodeData> response = new List<MyTreeViewNodeData>();

            // initial load?
            if (string.IsNullOrEmpty(parentNodePath))
            {
                // Start with the website root folder
                MyTreeViewNodeData rootNode = new MyTreeViewNodeData();
                rootNode.Header = "Sample Files";
                rootNode.Id = "\\";
                rootNode.IconSource = _serverPath + "folder.png";
                rootNode.IsExpanded = true;

                response.Add(rootNode);

                // Populate one level down
                List<MyTreeViewNodeData> arNodes = GetFileNodes(_root, false);

                rootNode.Nodes = new List<MyTreeViewNodeData>();
                foreach (MyTreeViewNodeData node in arNodes)
                {
                    rootNode.Nodes.Add(node);
                }
            }
            else
            {
                string dirPath = new DirectoryInfo(_root + parentNodePath).FullName;

                // Don't allow browsing  the file system outside of the app root
                if (dirPath.StartsWith(_root))
                {
                    List<MyTreeViewNodeData> arNodes = GetFileNodes(dirPath, false);

                    foreach (MyTreeViewNodeData node in arNodes)
                    {
                        response.Add(node);
                    }
                }
            }

            return response;

        }

        private string _serverPath
        {
            get
            {
                Uri url = HttpContext.Current.Request.Url;
                return string.Format("{0}://{1}:{2}/{3}", url.Scheme, url.Host, url.Port, IMG_PATH);
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


        private List<MyTreeViewNodeData> GetFileNodes(string dirPath, bool bFoldersOnly)
        {
            List<MyTreeViewNodeData> arList = new List<MyTreeViewNodeData>();

            string[] subDirectories = Directory.GetDirectories(dirPath);
            foreach (string directory in subDirectories)
            {
                string[] parts = directory.Split('\\');
                string name = parts[parts.Length - 1];

                MyTreeViewNodeData node = new MyTreeViewNodeData();
                node.Header = name;
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

                    MyTreeViewNodeData node = new MyTreeViewNodeData();
                    node.Header = name;

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


    // class for data exchange
    [DataContract]
    public class MyTreeViewNodeData
    {
        [DataMember]
        public string IconSource;
        [DataMember]
        public string Header;
        [DataMember]
        public bool IsLoadOnDemandEnabled;
        [DataMember]
        public string Id;
        [DataMember]
        public List<MyTreeViewNodeData> Nodes;
        [DataMember]
        public bool IsExpanded;
    }
}
