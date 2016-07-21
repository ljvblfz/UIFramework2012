using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.ComponentModel;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="GridLevel"/> objects. 
  /// </summary>
  /// <remarks>
  /// <para>
  /// This is a collection class used by <see cref="Grid" /> objects for their <see cref="Grid.Levels">Levels</see> property. Items within the collection
  /// are accessed by index directly through the Levels property of their containing Grid. The index of a Level within its 
  /// containing collection can be discovered using the <see cref="IndexOf"/> method.  
  /// Levels are added to the collection using the <see cref="Add"/> or <see cref="Insert"/> methods and can be removed with the
  /// <see cref="Remove"/> method.
  /// </para>
  /// </remarks>
  [Editor("System.Windows.Forms.Design.CollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
  public class GridLevelCollection : System.Collections.CollectionBase
  {
    /// <summary>
    /// Adds the given GridLevel to this collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridLevel" /> as an argument, adding it to the collection. The <see cref="GridLevelCollection.Insert" />
    /// method is also used to add a level to a collection, but instead of simply adding it to the end of the collection, it allows a level to be added
    /// at a specific index. 
    /// </para>
    /// <para>
    /// To remove a level from the collection, use the <see cref="GridLevelCollection.Remove" /> method. 
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridLevel to add</param>
    /// <returns></returns>
    public int Add(GridLevel obj) { return List.Add(obj); }

    /// <summary>
    /// Inserts the given GridLevel into this collection, at the given index.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to insert a <see cref="GridLevel" /> into the collection. 
    /// Unlike the <see cref="GridLevelCollection.Add" /> method, this method also accepts an index as an argument, 
    /// inserting the level at a specific position within the collection.
    /// </para>
    /// <para>
    /// To remove a level from the collection, use the <see cref="GridLevelCollection.Remove" /> method. 
    /// </para>
    /// </remarks>
    /// <param name="index">Index at which to insert</param>
    /// <param name="obj">GridLevel to insert</param>
    public void Insert(int index, GridLevel obj) { List.Insert(index, obj); }

    /// <summary>
    /// Removes the given GridLevel from this collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridLevel" /> as an argument, removing that level from the collection. 
    /// Levels are added to the collection using either the <see cref="GridLevelCollection.Add" /> method or the 
    /// <see cref="GridLevelCollection.Insert" /> method.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridLevel to remove</param>
    public void Remove(GridLevel obj) { List.Remove(obj); }

    /// <summary>
    /// Returns whether this collection contains the given GridLevel.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridLevel" /> as an argument, returning a boolean value indicating whether the level is 
    /// contained within the collection.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridLevel to look for</param>
    /// <returns>Whether this collection contains the given GridLevel</returns>
    public bool Contains(GridLevel obj) { return List.Contains(obj); }

    /// <summary>
    /// Copies this collection to the given array, starting at the given index
    /// </summary>
    /// <param name="array">Array to copy to</param>
    /// <param name="index">Index to copy to</param>
    public void CopyTo(GridLevel[] array, int index) { List.CopyTo(array, index); }

    /// <summary>
    /// Returns the index of the given GridLevel within the collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridLevel" /> as an argument, returning its index within the collection.
    /// If the provided level is not contained within the collection, the method will return -1.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridLevel to look for</param>
    /// <returns>The index within the collection</returns>
    public int IndexOf(object obj) 
    {
      if (obj is int)
        return (int)obj;

      if(obj == null)
      {
        return -1;
      }

      if (obj is GridLevel)
      {
        for (int i = 0; i < List.Count; i++)
          if (List[i] == obj) 
            return i;

        return -1; 
      }
      else 
      {
        throw new ArgumentException("Only a GridLevel or an integer is permitted for the indexer.");
      }
    } 

    public GridLevel this[object obj]
    {
      get 
      {
        int iIndex = IndexOf(obj);
        
        if(iIndex >= 0)
          return (GridLevel)List[iIndex];
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
    /// Removes the given GridLevel from this collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts either a <see cref="GridLevel" /> object or an integer index as an argument, removing the 
    /// corresponding level from the collection. 
    /// Levels are added to the collection using either the <see cref="GridLevelCollection.Add" /> method or the 
    /// <see cref="GridLevelCollection.Insert" /> method.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridLevel to remove</param>
    public void Remove(object obj)
    {
      if (obj is GridLevel)
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
        throw new ArgumentException("Only a GridLevel or an integer (index) parameter are permitted.");
      }
    }

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();

      oSB.Append("<Levels>");

      foreach(GridLevel oLevel in List)
      {
        oSB.Append(oLevel.GetXml());
      }

      oSB.Append("</Levels>");

      return oSB.ToString();
    }

    internal void LoadXml(string sXml)
    {
      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.LoadXml(sXml);

      for(int i = 0; i < oXmlDoc.DocumentElement.ChildNodes.Count; i++)
      {
        XmlNode oNode = oXmlDoc.DocumentElement.ChildNodes[i];

        GridLevel oLevel;

        if(i >= List.Count)
        {
          oLevel = new GridLevel();
          List.Add(oLevel);
        }
        else
        {
          oLevel = (GridLevel)List[i];
        }

        oLevel.LoadXml(oNode);  
      }
    }
  }
}
