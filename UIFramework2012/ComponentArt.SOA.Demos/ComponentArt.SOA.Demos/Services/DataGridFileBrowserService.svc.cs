using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using ComponentArt.SOA.UI;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class DataGridFileBrowserService : SoaDataGridService
{
  public override SoaDataGridSelectResponse Select(SoaDataGridSelectRequest request)
  {
    string root = Directory.Exists(HttpContext.Current.Server.MapPath("~/webui/demos")) ? "~/webui/demos" : "~";

    string dirPath = request.Tag != null && (string)request.Tag != "" ? (string)request.Tag : HttpContext.Current.Server.MapPath(root);


    List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

    if (dirPath.StartsWith(HttpContext.Current.Server.MapPath(root)))
    {
      string[] subDirectories = Directory.GetDirectories(dirPath);
      foreach (string directory in subDirectories)
      {
        string[] parts = directory.Split('\\');
        string name = parts[parts.Length - 1];

        Dictionary<string, object> info = new Dictionary<string, object>();

        info["Name"] = name;
        info["Icon"] = "folder.png";
        info["Size"] = 0;
        info["Type"] = "File Folder";
        info["DateModified"] = System.DateTime.Now.ToString("M/d/yyyy hh:mm tt");
        info["IsFolder"] = true;
        info["Value"] = directory;
        info["Extension"] = "";
        info["SizeString"] = "";

        list.Add(info);
      }
      string[] files = Directory.GetFiles(dirPath);
      foreach (string file in files)
      {
        string[] parts = file.Split('\\');
        string name = parts[parts.Length - 1];
        
        FileInfo fi = new FileInfo(file);
        ItemAttributes attribs = GetItemAttributes(fi.Extension);

        Dictionary<string, object> info = new Dictionary<string, object>();

        info["Name"] = name;
        info["Icon"] = attribs.Icon;
        info["Size"] = fi.Length;
        info["Type"] = attribs.Type;
        info["DateModified"] = fi.LastWriteTime.ToString("M/d/yyyy hh:mm tt");
        info["IsFolder"] = false;
        info["Value"] = "";
        info["Extension"] = fi.Extension;
        info["SizeString"] = GetSizeString(fi.Length);

        list.Add(info);
      }
    }

    SoaDataGridSelectResponse response = new SoaDataGridSelectResponse();

    // convert dictionaries to raw data
    List<List<object>> arData = new List<List<object>>();

    foreach(Dictionary<string,object> fileEntry in list)
    {
      // load data row
      List<object> fileRecord = new List<object>();
      foreach (SoaDataGridColumn oColumn in request.Columns)
      {
        fileRecord.Add(fileEntry[oColumn.Name]);
      }

      arData.Add(fileRecord);
    }

    response.Data = arData;
    response.ItemCount = arData.Count;

    return response;
  }

  // Returns item attributes based on its extension 
  private ItemAttributes GetItemAttributes(string Extension)
  {
    ItemAttributes attribs = new ItemAttributes();
    switch (Extension.ToLower())
    {
      case "":
        attribs.Icon = "folder.png";
        attribs.Type = "File Folder";
        break;
      case ".txt":
        attribs.Icon = "file.png";
        attribs.Type = "Text File";
        break;
      case ".dll":
        attribs.Icon = "dll.png";
        attribs.Type = "Application Extension";
        break;
      case ".aspx":
        attribs.Icon = "aspx.png";
        attribs.Type = "ASP.NET Server Page";
        break;
      case ".ascx":
        attribs.Icon = "ascx.png";
        attribs.Type = "ASP.NET User Control";
        break;
      case ".cs":
        attribs.Icon = "cs.png";
        attribs.Type = "C# File";
        break;
      case ".vb":
        attribs.Icon = "vb.png";
        attribs.Type = "VB.NET File";
        break;
      case ".config":
        attribs.Icon = "config.png";
        attribs.Type = "Web Config File";
        break;
      case ".gif":
        attribs.Icon = "image.png";
        attribs.Type = "GIF Image File";
        break;
      case ".jpg":
        attribs.Icon = "image.png";
        attribs.Type = "JPEG Image File";
        break;
      case ".png":
        attribs.Icon = "image.png";
        attribs.Type = "PNG Image File";
        break;
      case ".xml":
        attribs.Icon = "xml.png";
        attribs.Type = "XML File";
        break;
      case ".js":
        attribs.Icon = "js.png";
        attribs.Type = "JavaScript File";
        break;
      case ".css":
        attribs.Icon = "css.png";
        attribs.Type = "CSS Document";
        break;
      default:
        attribs.Icon = "file.png";
        attribs.Type = Extension.Replace(".", "").ToUpper() + " File";
        break;
    }
    return attribs;
  }

  // Formats the file size string 
  private string GetSizeString(long length)
  {
    return (Convert.ToInt32(length / 1000) + 1).ToString() + " KB";
  }

  public class ItemAttributes
  {
    private string _icon;
    public string Icon
    {
      get
      {
        return _icon;
      }
      set
      {
        _icon = value;
      }
    }

    private string _type;
    public string Type
    {
      get
      {
        return _type;
      }
      set
      {
        _type = value;
      }
    }
  }
}

