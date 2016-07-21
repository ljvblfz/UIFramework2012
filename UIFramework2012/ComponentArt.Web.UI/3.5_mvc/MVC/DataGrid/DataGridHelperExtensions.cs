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
  /// Provides methods to access some basic state properties.
  /// </summary>
  public static class DataGridHelperExtensions
  {
    #region Private Methods

    private static List<string> GetAllEvents(string dataGridId, FormCollection formData)
    {
      string sScript = formData.Get(dataGridId + "_EventList");
      if (sScript != null)
      {
        return new List<string>(sScript.Split(';'));
      }

      return null;
    }

    private static List<string> GetEvents(string dataGridId, FormCollection formData, string eventType)
    {
      string sScript = formData.Get(dataGridId + "_EventList");
      if (sScript != null)
      {
        return new List<string>(sScript.Split(';')).Where(s => s.StartsWith(eventType + " ")).ToList();
      }

      return null;
    }

    #endregion
    /// <summary>
    /// Extracts a DataGrid's selected items from the page's FormCollection.
    /// </summary>
    /// <param name="dataGridId"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
    public static List<object> GetSelectedItems(string dataGridId, FormCollection formData)
    {
      List<object> selectedItems = new List<object>();
      List<string> selectEvents = GetEvents(dataGridId, formData, "SELECT");

      foreach (string selectEvent in selectEvents)
      {
        string[] eventArgs = selectEvent.Split(' ');
        string sSelectedKey = HttpUtility.UrlDecode(eventArgs[2], Encoding.UTF8);

        selectedItems.Add(sSelectedKey);
      }

      return selectedItems;
    }
  }
}
