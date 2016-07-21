using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

using ComponentArt.SOA.UI;
using System.IO;
using System.Web;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class TreeViewFileBrowserService : SoaTreeViewService
{
  public override SoaTreeViewGetNodesResponse GetNodes(SoaTreeViewGetNodesRequest request)
  {
    // first, wait a bit to show the loading spinner
    
    SoaTreeViewGetNodesResponse response = new SoaTreeViewGetNodesResponse();

    // initial load?
    if (request.ParentNode == null)
    {
      // Start with the website root folder
      SoaTreeViewNode rootNode = new SoaTreeViewNode();
      rootNode.Text = "Sample Files";
      rootNode.Id = "\\";
      rootNode.IconSource = IMG_PATH + "folder.png";
      rootNode.IsExpanded = true;

      response.Nodes = new List<SoaTreeViewNode>();
      response.Nodes.Add(rootNode);

      // Populate one level down
      List<SoaTreeViewNode> arNodes = GetFileNodes(_root, (string)(request.Tag) == "FoldersOnly");

      rootNode.Items = new List<SoaTreeViewNode>();
      foreach (SoaTreeViewNode node in arNodes)
      {
        rootNode.Items.Add(node);
      }
    }
    else
    {
      string dirPath = new DirectoryInfo((string)(request.ParentNode.Tag)).FullName;

      // Don't allow browsing  the file system outside of the app root
      if (dirPath.StartsWith(_root))
      {
        List<SoaTreeViewNode> arNodes = GetFileNodes(dirPath, (string)(request.Tag) == "FoldersOnly");

        response.Nodes = new List<SoaTreeViewNode>();
        foreach (SoaTreeViewNode node in arNodes)
        {
          response.Nodes.Add(node);
        }
      }
    }

    return response;

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

  private const string IMG_PATH = "";


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
      node.IconSource = IMG_PATH + "folder.png";
      node.Tag = directory;

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
        //node.Tag = file; // do not set tags for files

        FileInfo fi = new FileInfo(file);
        switch (fi.Extension.ToLower())
        {
          case ".dll": node.IconSource = IMG_PATH + "dll.png"; break;
          case ".aspx": node.IconSource = IMG_PATH + "aspx.png"; break;
          case ".ascx": node.IconSource = IMG_PATH + "ascx.png"; break;
          case ".cs": node.IconSource = IMG_PATH + "cs.png"; break;
          case ".vb": node.IconSource = IMG_PATH + "vb.png"; break;
          case ".config": node.IconSource = IMG_PATH + "config.png"; break;
          case ".png": node.IconSource = IMG_PATH + "image.png"; break;
          case ".jpg": node.IconSource = IMG_PATH + "image.png"; break;
          case ".xml": node.IconSource = IMG_PATH + "xml.png"; break;
          case ".js": node.IconSource = IMG_PATH + "js.png"; break;
          case ".css": node.IconSource = IMG_PATH + "css.png"; break;
          default: node.IconSource = IMG_PATH + "file.png"; break;
        }

        arList.Add(node);
      }
    }

    return arList;
  }
}

