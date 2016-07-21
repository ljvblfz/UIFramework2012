using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.ComponentModel;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="ComboBoxItem"/> objects. 
  /// </summary>
  [Editor("System.Windows.Forms.Design.CollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
  public class ComboBoxItemCollection : System.Collections.CollectionBase
  {
    public int Add(ComboBoxItem obj) { return List.Add(obj); }
    public void Insert(int index, ComboBoxItem obj) { List.Insert(index, obj); }
    public void Remove(ComboBoxItem obj) { List.Remove(obj); }
    public bool Contains(ComboBoxItem obj) { return List.Contains(obj); }
    public void CopyTo(ComboBoxItem[] array, int index) { List.CopyTo(array, index); }

    public int IndexOf(object obj)
    {
      if (obj is int)
        return (int)obj;

      if (obj == null)
      {
        return -1;
      }

      if (obj is string)
      {
        for (int i = 0; i < List.Count; i++)
          if (((ComboBoxItem)List[i]).Id == (string)obj)
            return i;

        return -1;
      }
      else if (obj is ComboBoxItem)
      {
        for (int i = 0; i < List.Count; i++)
          if ((ComboBoxItem)List[i] == (ComboBoxItem)obj)
            return i;

        return -1;
      }
      else
      {
        throw new ArgumentException("Only a string, an integer or a ComboBoxItem are permitted for the indexer.");
      }
    }

    public ComboBoxItem this[object obj]
    {
      get
      {
        int iIndex = IndexOf(obj);

        if (iIndex >= 0)
          return (ComboBoxItem)List[iIndex];
        else
          return null;
      }
      set
      {
        int iIndex = IndexOf(obj);

        if (iIndex >= 0)
          List[iIndex] = value;
        else
          this.Add(value);
      }
    }

    public ComboBoxItem FindByText(string sText)
    {
      for (int i = 0; i < List.Count; i++)
      {
        if (((ComboBoxItem)List[i]).Text == sText)
        {
          return ((ComboBoxItem)List[i]);
        }
      }

      return null;
    }

    public ComboBoxItem FindByValue(string sValue)
    {
      for (int i = 0; i < List.Count; i++)
      {
        if (((ComboBoxItem)List[i]).Value == sValue)
        {
          return ((ComboBoxItem)List[i]);
        }
      }

      return null;
    }

    public void Remove(object obj)
    {
      if (obj is string)
      {
        for (int i = 0; i < List.Count; i++)
        {
          if (((ComboBoxItem)List[i]).Id == (string)obj)
          {
            base.RemoveAt(i);
            return;
          }
        }
      }
      else if (obj is int)
      {
        base.RemoveAt((int)obj);
      }
      else
      {
        throw new ArgumentException("Only a string (ID) or an integer (index) parameter are permitted.");
      }
    }

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();

      oSB.Append("<Looks>");

      foreach (ItemLook oLook in List)
      {
        oSB.Append(oLook.GetXml());
      }

      oSB.Append("</Looks>");

      return oSB.ToString();
    }

    internal void LoadXml(string sXml)
    {
      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.LoadXml(sXml);

      foreach (XmlNode oNode in oXmlDoc.DocumentElement.ChildNodes)
      {
        ComboBoxItem oItem = new ComboBoxItem();
        //oItem.LoadXml(oNode.OuterXml);
        List.Add(oItem);
      }
    }
  }
}
