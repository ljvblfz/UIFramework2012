using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Xml;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="SplitterLayout"/> controls.
  /// </summary>
  public class SplitterLayoutCollection  : System.Collections.CollectionBase
  { 
    public int Add(SplitterLayout obj) { return List.Add(obj); } 
    public void Insert(int index, SplitterLayout obj) { List.Insert(index, obj); } 
    public void Remove(SplitterLayout obj) { List.Remove(obj); } 
    public bool Contains(SplitterLayout obj) { return List.Contains(obj); } 
    public void CopyTo(SplitterLayout[] array, int index) { List.CopyTo(array, index); } 

    /// <summary>
    /// Returns the index of the given SplitterLayout within the collection.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int IndexOf(object obj) 
    {
      if (obj is int)
        return (int)obj;

      if(obj == null)
      {
        return -1;
      }

      if (obj is SplitterLayout)
      {
        for (int i = 0; i < List.Count; i++)
          if (Object.Equals(List[i], obj)) 
            return i;

        return -1; 
      }
      else 
      {
        throw new ArgumentException("Only a SplitterLayout or an integer is permitted for the indexer.");
      }
    } 

    public SplitterLayout this[object obj]
    {
      get 
      {
        int iIndex = IndexOf(obj);
        
        if(iIndex >= 0)
          return (SplitterLayout)List[iIndex];
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

    /// <summary>
    /// Remove the given SplitterLayout from the collection.
    /// </summary>
    /// <param name="obj">SplitterLayout to remove</param>
    public void Remove(object obj)
    {
      if (obj is SplitterLayout)
      {
        for (int i = 0; i < List.Count; i++)
        {
          if (Object.Equals(List[i], obj))
          {
            base.RemoveAt(i);
            return;
          }
        }
      }
      else 
      {
        throw new ArgumentException("Only a SplitterLayout parameter is permitted.");
      }
    }

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();
      oSB.Append("<Layouts>");
      
      foreach(SplitterLayout oLayout in List)
      {
        oSB.Append(oLayout.GetXml());
      }
      
      oSB.Append("</Layouts>");

      return oSB.ToString();
    }

    internal void LoadXml(XmlNode oNode)
    {
      this.Clear();

      foreach(XmlNode oChildNode in oNode.ChildNodes)
      {
        SplitterLayout oLayout = new SplitterLayout();
        oLayout.LoadXml(oChildNode);
        this.Add(oLayout);
      }
    }

    internal void LoadXml(string sXml)
    {
      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.LoadXml(sXml);

      this.LoadXml(oXmlDoc.DocumentElement);
    }
  }
}
