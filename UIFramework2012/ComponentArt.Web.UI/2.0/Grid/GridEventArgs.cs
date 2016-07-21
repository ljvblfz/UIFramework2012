using System;
using System.Collections;
using System.Data;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Arguments for <see cref="GridItem">item</see>-centric server-side events of the <see cref="Grid"/> control.
  /// </summary>
  /// <remarks>
  /// Arguments of this type are used by the following events: <see cref="Grid.InsertCommand"/>, <see cref="Grid.SelectCommand"/>,
  /// <see cref="Grid.UpdateCommand"/>, and <see cref="Grid.DeleteCommand"/>.
  /// </remarks>
  public class GridItemEventArgs : EventArgs
  {
    /// <summary>
    /// The GridItem which the event relates to.
    /// </summary>
    public GridItem Item;

    public GridItemEventArgs(GridItem oItem)
    {
      Item = oItem;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.ItemCommand"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridItemCommandEventArgs : EventArgs
  {
    /// <summary>
    /// The GridItem which the event relates to.
    /// </summary>
    public GridItem Item;

    /// <summary>
    /// The control which causes the postback.
    /// </summary>
    public Control Control;

    public GridItemCommandEventArgs(GridItem oItem, Control oControl)
    {
      Item = oItem;
      Control = oControl;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.ItemContentCreated"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridItemContentCreatedEventArgs : EventArgs
  {
    /// <summary>
    /// The GridItem which the event relates to.
    /// </summary>
    public GridItem Item;

    /// <summary>
    /// The column under which the cell was created.
    /// </summary>
    public GridColumn Column;

    /// <summary>
    /// The ASP.NET content which was created for the cell.
    /// </summary>
    public Control Content;

    public GridItemContentCreatedEventArgs(GridItem oItem, GridColumn oColumn, Control oContent)
    {
      Item = oItem;
      Column = oColumn;
      Content = oContent;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.ItemDataBound"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridItemDataBoundEventArgs : EventArgs
  {
    /// <summary>
    /// The GridItem which the event relates to.
    /// </summary>
    public GridItem Item;

    /// <summary>
    /// The data item which the item was loaded from.
    /// </summary>
    public object DataItem;

    public GridItemDataBoundEventArgs(GridItem oItem, object oDataItem)
    {
      Item = oItem;
      DataItem = oDataItem;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.ItemCheckChanged"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridItemCheckChangedEventArgs : EventArgs
  {
    /// <summary>
    /// The GridItem which the event relates to.
    /// </summary>
    public GridItem Item;

    /// <summary>
    /// The column under which this event occured.
    /// </summary>
    public GridColumn Column;

    /// <summary>
    /// Whether the checkbox is now checked.
    /// </summary>
    public bool Checked;

    public GridItemCheckChangedEventArgs(GridItem oItem, GridColumn oColumn, bool bChecked)
    {
      Item = oItem;
      Column = oColumn;
      Checked = bChecked;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.FilterCommand"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridFilterCommandEventArgs : EventArgs
  {
    /// <summary>
    /// The requested filter expression.
    /// </summary>
    public string FilterExpression;

    public GridFilterCommandEventArgs(string sExpression)
    {
      FilterExpression = sExpression;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.GroupCommand"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridGroupCommandEventArgs : EventArgs
  {
    /// <summary>
    /// The requested grouping expression.
    /// </summary>
    public string GroupExpression;

    public GridGroupCommandEventArgs(string sExpression)
    {
      GroupExpression = sExpression;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.PageIndexChanged"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridPageIndexChangedEventArgs : EventArgs
  {
    /// <summary>
    /// The new page index.
    /// </summary>
    public int NewIndex;

    public GridPageIndexChangedEventArgs(int iNewIndex)
    {
      NewIndex = iNewIndex;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.Scroll"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridScrollEventArgs : EventArgs
  {
    /// <summary>
    /// The zero-based offset within the record set from which to start loading items.
    /// </summary>
    public int RecordOffset;

    public GridScrollEventArgs(int iRecordOffset)
    {
      RecordOffset = iRecordOffset;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.ColumnReorder"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridColumnReorderEventArgs : EventArgs
  {
    /// <summary>
    /// The index of the column being moved.
    /// </summary>
    public int OldIndex;

    /// <summary>
    /// The new index which the column is to take on.
    /// </summary>
    public int NewIndex;

    public GridColumnReorderEventArgs(int iOldIndex, int iNewIndex)
    {
      NewIndex = iNewIndex;
      OldIndex = iOldIndex;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.SortCommand"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridSortCommandEventArgs : EventArgs
  {
    /// <summary>
    /// The expression representing the requested sort order.
    /// </summary>
    public string SortExpression;

    public GridSortCommandEventArgs(string sExpression)
    {
      SortExpression = sExpression;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.NeedChildDataSource"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridNeedChildDataSourceEventArgs : EventArgs
  {
    /// <summary>
    /// The GridItem which the event relates to.
    /// </summary>
    public GridItem Item;

    /// <summary>
    /// The data source from which to load child data for the given item. This should be
    /// set by the developer inside the event handler.
    /// </summary>
    public object DataSource = null;

    public GridNeedChildDataSourceEventArgs(GridItem oItem)
    {
      Item = oItem;
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.NeedGroups"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridNeedGroupsEventArgs : EventArgs
  {
    /// <summary>
    /// The name of the column to group by.
    /// </summary>
    public string GroupColumn;

    /// <summary>
    /// The direction of the grouping ("ASC" or "DESC").
    /// </summary>
    public string GroupDirection;

    /// <summary>
    /// Constraints to include in the query.
    /// </summary>
    public GridDataConditionCollection Where;

    /// <summary>
    /// How many groups to skip.
    /// </summary>
    public int Offset;

    /// <summary>
    /// How many groups to include.
    /// </summary>
    public int Count;

    /// <summary>
    /// The collection of loaded group names. This should be
    /// populated by the developer inside the event handler.
    /// </summary>
    public ArrayList Groups;

    /// <summary>
    /// The total number of distinct values for this grouping. This should be
    /// set by the developer inside the event handler.
    /// </summary>
    public int TotalCount;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public GridNeedGroupsEventArgs()
    {
      Groups = new ArrayList();
      Where = new GridDataConditionCollection();
    }
  }

  /// <summary>
  /// Arguments for <see cref="Grid.NeedGroupData"/> server-side event of <see cref="Grid"/> control.
  /// </summary>
  public class GridNeedGroupDataEventArgs : EventArgs
  {
    /// <summary>
    /// The constraints that define the required data set.
    /// </summary>
    public GridDataConditionCollection Where;

    /// <summary>
    /// The sort expression to apply.
    /// </summary>
    public string SortExpression;

    /// <summary>
    /// The data source from which to load data rows for the given group. This should be
    /// set by the developer inside the event handler.
    /// </summary>
    public object DataSource = null;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public GridNeedGroupDataEventArgs()
    {
      Where = new GridDataConditionCollection();
    }
  }


  /// <summary>
  /// This class describes a simple data condition to be satisfied in a data query.
  /// </summary>
  public class GridDataCondition
  {
    /// <summary>
    /// The name of the data field to restrict by.
    /// </summary>
    public string DataFieldName;

    /// <summary>
    /// The value of the data field to match.
    /// </summary>
    public object DataFieldValue;

    public GridDataCondition(string sName, object oValue)
    {
      DataFieldName = sName;
      DataFieldValue = oValue;
    }

    public string ToSqlString()
    {
      if (DataFieldValue != null)
      {
        return "[" + DataFieldName + "] LIKE '" + DataFieldValue + "'";
      }
      else
      {
        return "[" + DataFieldName + "] IS NULL";
      }
    }
  }

  /// <summary>
  /// This class describes a collection of data conditions to be satisfied in a data query.
  /// </summary>
  /// <seealso cref="GridDataCondition" />
  public class GridDataConditionCollection : System.Collections.CollectionBase
  {
    /// <summary>
    /// Returns whether this collection is empty of any conditions.
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        return (List.Count == 0);
      }
    }

    /// <summary>
    /// Adds the given GridDataCondition to this collection.
    /// </summary>
    /// <param name="obj">The GridDataCondition to add</param>
    /// <returns></returns>
    public int Add(GridDataCondition obj) { return List.Add(obj); }

    /// <summary>
    /// Inserts the given GridDataCondition into GridDataCondition collection, at the given index.
    /// </summary>
    /// <param name="index">Index at which to insert</param>
    /// <param name="obj">GridDataCondition to insert</param>
    public void Insert(int index, GridDataCondition obj) { List.Insert(index, obj); }

    /// <summary>
    /// Removes the given GridDataCondition from this collection.
    /// </summary>
    /// <param name="obj">The GridDataCondition to remove</param>
    public void Remove(GridDataCondition obj) { List.Remove(obj); }

    /// <summary>
    /// Returns whether this collection contains the given GridColumn.
    /// </summary>
    /// <param name="obj">The GridDataCondition to look for</param>
    /// <returns>Whether this collection contains the given GridDataCondition</returns>
    public bool Contains(GridDataCondition obj) { return List.Contains(obj); }

    /// <summary>
    /// Copies this collection to the given array, starting at the given index
    /// </summary>
    /// <param name="array">Array to copy to</param>
    /// <param name="index">Index to copy to</param>
    public void CopyTo(GridDataCondition[] array, int index) { List.CopyTo(array, index); }

    /// <summary>
    /// Returns the index of the given GridDataCondition within the collection.
    /// </summary>
    /// <param name="obj">The GridDataCondition to look for</param>
    /// <returns>The index within the collection</returns>
    public int IndexOf(object obj)
    {
      if (obj is int)
        return (int)obj;

      if (obj == null)
      {
        return -1;
      }

      if (obj is GridDataCondition)
      {
        for (int i = 0; i < List.Count; i++)
          if (List[i] == obj)
            return i;

        return -1;
      }
      else if (obj is string)
      {
        for (int i = 0; i < List.Count; i++)
          if (string.Compare(((GridDataCondition)List[i]).DataFieldName, (string)obj, true) == 0)
            return i;

        return -1;
      }
      else
      {
        throw new ArgumentException("Only a GridDataCondition, string or an int are permitted for the indexer.");
      }
    }

    public GridDataCondition this[object obj]
    {
      get
      {
        int iIndex = IndexOf(obj);

        if (iIndex >= 0)
          return (GridDataCondition)List[iIndex];
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

    /// <summary>
    /// Removes the given GridDataCondition from this collection.
    /// </summary>
    /// <param name="obj">The GridColumn to remove</param>
    public void Remove(object obj)
    {
      if (obj is GridDataCondition)
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
      else if (obj is int)
      {
        base.RemoveAt((int)obj);
      }
      else
      {
        throw new ArgumentException("Only a GridDataCondition or an integer (index) parameter are permitted.");
      }
    }

    public string ToSqlString()
    {
      if (!IsEmpty)
      {
        string sSql = "";

        for (int i = 0; i < List.Count; i++)
        {
          if (i > 0)
          {
            sSql += " AND ";
          }

          sSql += ((GridDataCondition)List[i]).ToSqlString();
        }

        return sSql;
      }
      else
      {
        return "1";
      }
    }
  }
}
