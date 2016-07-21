using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.ComponentModel;


namespace ComponentArt.Web.UI
{
	/// <summary>
  /// Collection of <see cref="ItemLook"/> objects. 
	/// </summary>
	[Editor("System.Windows.Forms.Design.CollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
  public class ItemLookCollection : System.Collections.CollectionBase
  { 
    public int Add(ItemLook obj) { return List.Add(obj); } 
    public void Insert(int index, ItemLook obj) { List.Insert(index, obj); } 
    public void Remove(ItemLook obj) { List.Remove(obj); } 
    public bool Contains(ItemLook obj) { return List.Contains(obj); } 
    public void CopyTo(ItemLook[] array, int index) { List.CopyTo(array, index); } 

    public int IndexOf(object obj) 
    {
      if (obj is int)
        return (int)obj;

      if(obj == null)
      {
        return -1;
      }

      if (obj is string)
      {
        for (int i = 0; i < List.Count; i++)
          if (((ItemLook)List[i]).LookId == (string)obj) 
            return i;

        return -1; 
      }
      else 
      {
        throw new ArgumentException("Only a string or an integer is permitted for the indexer.");
      }
    } 

    public ItemLook this[object obj]
    {
      get 
      {
        int iIndex = IndexOf(obj);
        
        if(iIndex >= 0)
          return (ItemLook)List[iIndex];
        else
          return null;
      } 
      set
      {
        int iIndex = IndexOf(obj);
        
        if(iIndex >= 0)
          List[iIndex] = value;
        else
          this.Add(value);
      } 
    }

    public void Remove(object obj)
    {
      if (obj is string)
      {
        for (int i = 0; i < List.Count; i++)
        {
          if (((ItemLook)List[i]).LookId == (string)obj)
          {
            base.RemoveAt(i);
            return;
          }
        }
      }
      else if(obj is int)
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

      foreach(ItemLook oLook in List)
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

      foreach(XmlNode oNode in oXmlDoc.DocumentElement.ChildNodes)
      {
        ItemLook oLook = new ItemLook();
        oLook.LoadXml(oNode.OuterXml);
        List.Add(oLook);
      }
    }
	}
}
