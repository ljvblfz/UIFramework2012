using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.ComponentModel;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="GridColumn"/> objects.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This is a collection class used by <see cref="GridLevel"/> objects for their <see cref="GridLevel.Columns">Columns</see> property.
  /// Columns are accessed by index directly through the columns property of their parent GridLevel. The index of a column within its 
  /// containing collection can be discovered using the <see cref="IndexOf"/> method. 
  /// Columns are added to the collection using the <see cref="Add"/> or <see cref="Insert"/> methods and can be removed with the
  /// <see cref="Remove"/> method.
  /// </para>
  /// </remarks>
  [Editor("System.Windows.Forms.Design.CollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
  public class GridColumnCollection : System.Collections.CollectionBase
  {
    /// <summary>
    /// Adds the given GridColumn to this collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridColumn" /> as an argument, adding it to the collection. The <see cref="GridColumnCollection.Insert" />
    /// method is also used to add a column to a collection, but instead of simply adding it to the end of the collection, it allows a column to be added
    /// at a specific index. 
    /// </para>
    /// <para>
    /// To remove a column from the collection, use the <see cref="GridColumnCollection.Remove" /> method. 
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridColumn to add</param>
    /// <returns></returns>
    public int Add(GridColumn obj) { return List.Add(obj); }

    /// <summary>
    /// Inserts the given GridColumn into this collection, at the given index.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to insert a <see cref="GridColumn" /> into the collection. 
    /// Unlike the <see cref="GridColumnCollection.Add" /> method, this method also accepts an index as an argument, 
    /// inserting the column at a specific position within the collection.
    /// </para>
    /// <para>
    /// To remove a column from the collection, use the <see cref="GridColumnCollection.Remove" /> method. 
    /// </para>
    /// </remarks>
    /// <param name="index">Index at which to insert</param>
    /// <param name="obj">GridColumn to insert</param>
    public void Insert(int index, GridColumn obj) { List.Insert(index, obj); }

    /// <summary>
    /// Removes the given GridColumn from this collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridColumn" /> as an argument, removing that column from the collection. 
    /// Columns are added to the collection using either the <see cref="GridColumnCollection.Add" /> method or the 
    /// <see cref="GridColumnCollection.Insert" /> method.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridColumn to remove</param>
    public void Remove(GridColumn obj) { List.Remove(obj); }

    /// <summary>
    /// Returns whether this collection contains the given GridColumn.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridColumn" /> as an argument, returning a boolean value indicating whether the column is 
    /// contained within the collection.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridColumn to look for</param>
    /// <returns>Whether this collection contains the given GridColumn</returns>
    public bool Contains(GridColumn obj) { return List.Contains(obj); }

    /// <summary>
    /// Copies this collection to the given array, starting at the given index
    /// </summary>
    /// <param name="array">Array to copy to</param>
    /// <param name="index">Index to copy to</param>
    public void CopyTo(GridColumn[] array, int index) { List.CopyTo(array, index); }

    /// <summary>
    /// Returns the index of the given GridColumn within the collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method returns the index of a <see cref="GridColumn" /> from within the collection. 
    /// It accepts either a <code>GridColumn</code> object or a string as an argument. If a string is provided, it must correspond to the 
    /// <see cref="GridColumn.DataField" /> property of one of the columns in the collection.
    /// If the provided column is not contained within the collection, the method will return -1.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridColumn to look for</param>
    /// <returns>The index within the collection</returns>
    public int IndexOf(object obj) 
    {
      if (obj is int)
        return (int)obj;

      if(obj == null)
      {
        return -1;
      }

      if (obj is GridColumn)
      {
        for (int i = 0; i < List.Count; i++)
          if (List[i] == obj) 
            return i;

        return -1; 
      }
      else if(obj is string)
      {
        for (int i = 0; i < List.Count; i++)
          if (string.Compare(((GridColumn)List[i]).DataField, (string)obj, true) == 0) 
            return i;

        return -1; 
      }
      else 
      {
        throw new ArgumentException("Only a GridColumn, string or an int are permitted for the indexer.");
      }
    } 

    public GridColumn this[object obj]
    {
      get 
      {
        int iIndex = IndexOf(obj);
        
        if(iIndex >= 0)
          return (GridColumn)List[iIndex];
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
    /// Removes the given GridColumn from this collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts either a <see cref="GridColumn" /> object or an integer index as an argument, removing the 
    /// corresponding column from the collection. 
    /// Columns are added to the collection using either the <see cref="GridColumnCollection.Add" /> method or the 
    /// <see cref="GridColumnCollection.Insert" /> method.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridColumn to remove</param>
    public void Remove(object obj)
    {
      if (obj is GridColumn)
      {
        for (int i = 0; i < List.Count; i++)
        {
          if (List[i] == obj)
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
        throw new ArgumentException("Only a GridColumn or an integer (index) parameter are permitted.");
      }
    }

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();

      oSB.Append("<Columns>");

      foreach(GridColumn oColumn in List)
      {
        oSB.Append(oColumn.GetXml());
      }

      oSB.Append("</Columns>");

      return oSB.ToString();
    }

    internal void LoadXml(string sXml)
    {
      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.LoadXml(sXml);

      for(int i = 0; i < oXmlDoc.DocumentElement.ChildNodes.Count; i++)
      {
        XmlNode oNode = oXmlDoc.DocumentElement.ChildNodes[i];

        GridColumn oColumn;

        if(i >= List.Count)
        {
          oColumn = new GridColumn();
          List.Add(oColumn);
        }
        else
        {
          oColumn = (GridColumn)List[i];
        }

        oColumn.LoadXml(oNode);
      }
    }
  }
}
