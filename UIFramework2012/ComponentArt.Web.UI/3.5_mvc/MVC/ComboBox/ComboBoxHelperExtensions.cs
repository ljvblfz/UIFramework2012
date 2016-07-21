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
  public static class ComboBoxHelperExtensions
  {
    /// <summary>
    /// Extracts a ComboBox's selected index from the page's FormCollection.
    /// </summary>
    /// <param name="comboBoxId"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
    public static int GetSelectedIndex(string comboBoxId, FormCollection formData)
    {
      if (formData.AllKeys.Contains(comboBoxId + "_SelectedIndex"))
      {
        return int.Parse(formData.Get(comboBoxId + "_SelectedIndex"));
      }

      return -1;
    }
    /// <summary>
    /// Extracts a ComboBox's selected item from the page's FormCollection.
    /// </summary>
    /// <param name="comboBoxId"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
    public static ComboBoxItem GetSelectedItem(string comboBoxId, FormCollection formData)
    {
      if (formData.AllKeys.Contains(comboBoxId + "_SelectedText"))
      {
        ComboBoxItem oSelectedItem = new ComboBoxItem();

        oSelectedItem.Text = formData.Get(comboBoxId + "_SelectedText");
        oSelectedItem.Value = formData.Get(comboBoxId + "_SelectedValue");

        return oSelectedItem;
      }

      return null;
    }
  }
}
