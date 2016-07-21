using System;
using System.Collections;
using System.Data;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="GridItem"/> objects. 
  /// </summary>
  /// <remarks>
  /// <para>
  /// This is a collection class used by <see cref="Grid"/> and <see cref="GridItem" /> objects for their Items property: 
  /// Grid <see cref="Grid.Items">Items</see>, 
  /// GridItem <see cref="GridItem.Items">Items</see>. Items within a GridItemCollection are accessed through the Items property of their parent.
  /// </para>
  /// <para>Since items are accessed using their index within the collection, it is sometimes necessary to discover the index of an item
  /// using the value of a property. This is accomplished using the <see cref="IndexOf" /> method. The 
  /// <see cref="ComponentArt.Web.UI.chm::/Grid_Server_Tips.htm">Common Server-side Programming Tips</see> tutorial covers this, and other 
  /// server-side programming techniques.
  /// </para>
  /// </remarks>
  public class GridItemCollection  : System.Collections.CollectionBase
  { 
    /// <summary>
    /// Adds the given GridItem to this collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridItem" /> as an argument, adding it to the collection. The <see cref="GridItemCollection.Insert" />
    /// method is also used to add an item to a collection, but instead of simply adding it to the end of the collection, it allows an item to be added
    /// at a specific index. 
    /// </para>
    /// <para>
    /// To remove an item from the collection, use the <see cref="GridItemCollection.Remove" /> method. 
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridItem to add</param>
    /// <returns></returns>
    public int Add(GridItem obj) { return List.Add(obj); }

    /// <summary>
    /// Inserts the given GridItem into this collection, at the given index.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to insert a <see cref="GridItem" /> into the collection. 
    /// Unlike the <see cref="GridItemCollection.Add" /> method, this method also accepts an index as an argument, 
    /// inserting the item at a specific position within the collection.
    /// </para>
    /// <para>
    /// To remove an item from the collection, use the <see cref="GridItemCollection.Remove" /> method. 
    /// </para>
    /// </remarks>
    /// <param name="index">Index at which to insert</param>
    /// <param name="obj">GridItem to insert</param>
    public void Insert(int index, GridItem obj) { List.Insert(index, obj); }

    /// <summary>
    /// Removes the given GridItem from this collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridItem" /> as an argument, removing that item from the collection. 
    /// Items are added to the collection using either the <see cref="GridItemCollection.Add" /> method
    ///  or the <see cref="GridItemCollection.Insert" /> method.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridItem to remove</param>
    public void Remove(GridItem obj) { List.Remove(obj); } 

    /// <summary>
    /// Returns whether this collection contains the given GridItem.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridItem" /> as an argument, returning a boolean value indicating whether the item is 
    /// contained within the collection.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridItem to look for</param>
    /// <returns>Whether this collection contains the given GridItem</returns>
    public bool Contains(GridItem obj) { return List.Contains(obj); } 

    /// <summary>
    /// Copies this collection to the given array, starting at the given index
    /// </summary>
    /// <param name="array">Array to copy to</param>
    /// <param name="index">Index to copy to</param>
    public void CopyTo(GridItem[] array, int index) { List.CopyTo(array, index); } 

    /// <summary>
    /// Returns the index of the GridItem in this collection with the given value under the given column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method returns the index of a <see cref="GridItem" /> within the collection using the value of a column.
    /// The method accepts the column name as a string, and the desired value. If the collection contains more than one item with the
    /// provided value, this method will return the index of the first one. If there are no matching items, the method will return -1.
    /// </para>
    /// <para>
    /// The <see cref="GridItemCollection.IndexOf(Object)">other version</see> of this method accepts a <code>GridItem</code> as an argument. 
    /// </para>
    /// </remarks>
    /// <param name="sName">Column name</param>
    /// <param name="oValue">Value</param>
    /// <returns>The index of the GridItem in this collection with the given value under the given column</returns>
    public int IndexOf(string sName, object oValue)
    {
      for (int i = 0; i < List.Count; i++)
      {
        object oThisValue = ((GridItem)List[i])[sName];

        if(oValue is string && oThisValue != null && (string)oValue == oThisValue.ToString())
        {
          return i;
        }
        else if (Object.Equals(oThisValue, oValue))
        {
          return i;
        }
      }

      return -1;
    }

    /// <summary>
    /// Returns the index of the given GridItem within the collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridItem" /> as an argument, returning that item's index within the collection. 
    /// If the item is not contained within the collection, this method will return -1.
    /// </para>
    /// <para>
    /// The <see cref="GridItemCollection.IndexOf(String,Object)">other version</see> of this method allows an item's index to be 
    /// retrieved using a column-value pair.
    /// </para>
    /// </remarks>
    /// <param name="obj">The GridItem to look for</param>
    /// <returns>The index within the collection</returns>
    public int IndexOf(object obj) 
    {
      if (obj is int)
        return (int)obj;

      if(obj == null)
      {
        return -1;
      }

      if (obj is GridItem)
      {
        for (int i = 0; i < List.Count; i++)
          if (Object.Equals(List[i], obj)) 
            return i;

        return -1; 
      }
      else 
      {
        throw new ArgumentException("Only a GridItem or an integer is permitted for the indexer.");
      }
    } 

    public GridItem this[object obj]
    {
      get 
      {
        int iIndex = IndexOf(obj);
        
        if(iIndex >= 0)
          return (GridItem)List[iIndex];
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
    /// Sort this item collection according to the given column and direction.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method allows the collection to be sorted based on the values in a single column. The column is passed into the method as
    /// an argument, along with a boolean value which is used to determine whether or not the column will be sorted in descending order.
    /// </para>
    /// </remarks>
    /// <param name="oColumn">The column to sort by</param>
    /// <param name="bDesc">Whether to sort in a descending order</param>
    public void Sort(GridColumn oColumn, bool bDesc)
    {
      if(this.Count > 0)
      {
        if(oColumn != null)
        {
          GridItemSortComparer oComparer = new GridItemSortComparer();
          oComparer.Column = oColumn;
          oComparer.Descending = bDesc;

          this.InnerList.Sort(oComparer);
        }
      }
    }
  }

  #region GridItemSortComparer
  internal class GridItemSortComparer : IComparer  
  {
    private IComparer comparer = null;

    public GridColumn Column;
    public bool Descending;

    int IComparer.Compare( Object x, Object y )  
    {
      object a = ((GridItem)x)[Column.ColumnIndex];
      object b = ((GridItem)y)[Column.ColumnIndex];

      if(Descending)
      {
        object c = a;
        a = b;
        b = c;
      }

      if(Column.DataType == typeof(string))
      {
        if(comparer == null)
        {
          comparer = new CaseInsensitiveComparer();
        }

        return comparer.Compare( a, b );
      }
      else if(Column.DataType == typeof(int))
      {
        return (int)a == (int)b? 0 : ((int)a > (int)b? 1 : -1);
      }
      else if(Column.DataType == typeof(decimal))
      {        
        return (decimal)a == (decimal)b? 0 : ((decimal)a > (decimal)b? 1 : -1);
      }
      else if(Column.DataType == typeof(double))
      {
        return (double)a == (double)b? 0 : ((double)a > (double)b? 1 : -1);
      }
      else if(Column.DataType == typeof(DateTime))
      {
        return (DateTime)a == (DateTime)b? 0 : ((DateTime)a > (DateTime)b? 1 : -1);
      }
      else
      {
        return 0;
      }
    }
  }
  #endregion
}
