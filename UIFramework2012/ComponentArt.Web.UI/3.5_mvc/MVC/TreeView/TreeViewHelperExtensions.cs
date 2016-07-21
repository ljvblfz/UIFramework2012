using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.Mvc;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Provides a method to access some basic state properties.
  /// </summary>
  public static class TreeViewHelperExtensions
  {
    /// <summary>
    /// Extracts a TreeView's selected node's text from the page's FormCollection.
    /// </summary>
    /// <param name="treeViewId"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
    public static string GetSelectedText(string treeViewId, FormCollection formData)
    {
      return formData.Get(treeViewId + "_SelectedText");
    }
    /// <summary>
    /// Extracts a TreeView's selected node's value from the page's FormCollection.
    /// </summary>
    /// <param name="treeViewId"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
    public static string GetSelectedValue(string treeViewId, FormCollection formData)
    {
      return formData.Get(treeViewId + "_SelectedValue");
    }
  }
}
