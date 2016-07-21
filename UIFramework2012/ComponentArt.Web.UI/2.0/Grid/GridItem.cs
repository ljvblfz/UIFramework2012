using System;
using System.Data;
using System.Reflection;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Represents an item (row) in a <see cref="Grid"/> control.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The GridItem class represents one row of data and corresponds to a row or object from the underlying data source. 
  /// Each GridItem is associated with a <see cref="GridLevel" /> which represents a table of data.
  /// A GridItem contains a field for each column in the GridLevel it is associated with. 
  /// Individual values can be accessed directly from the GridItem object using a string (field name) or integer index.
  /// </para>
  /// <para>
  /// A GridItem is represented visually by a row in the rendered Grid. Style properties which are applied to rows are defined
  /// in the GridLevel object that is associated with the item. 
  /// </para>
  /// <para>
  /// The <see cref="Level" /> property contains the index of the item's associated GridLevel within the Grid's <see cref="Grid.Levels">Levels</see>
  /// collection.
  /// </para>
  /// </remarks>
  public class GridItem
	{
    private Grid _grid;
    private GridColumnCollection _columns;
    private object [] _values;

    private object _dataItem = null;
    /// <summary>
    /// The data item, from the data source that the Grid was bound to, which
    /// corresponds to this loaded GridItem.
    /// </summary>
    public object DataItem
    {
      get
      {
        return _dataItem;
      }
    }

    private int _level = 0;
    /// <summary>
    /// The level (depth) on which this item is found.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every <see cref="Grid" /> object has a <see cref="Levels" /> collection, containing all of its <see cref="GridLevel" />
    /// objects. Each GridLevel corresponds to a level of hierarchy in the Grid. For that reason, each <see cref="GridItem" /> 
    /// is associated with a level, even if it is not a hierarchical grid. This property contains an integer which 
    /// corresponds to the index of the associated GridLevel within the Grid's Levels collection.
    /// </para>
    /// </remarks>
    public int Level
    {
      get
      {
        return _level;
      }
    }

    public GridItem(Grid oGrid, int iLevel, object [] arValues)
    {
      _grid = oGrid;
      _columns = oGrid.Levels[iLevel].Columns;
      _values = arValues;
      _level = iLevel;
      _dataItem = arValues;
    }

    public GridItem(Grid oGrid, int iLevel)
    {
      _grid = oGrid;
      _columns = oGrid.Levels[iLevel].Columns;
      _values = new Object[_columns.Count];
      _level = iLevel;
    }

    public GridItem(Grid oGrid, int iLevel, object oObject)
    {
      _grid = oGrid;
      _columns = oGrid.Levels[iLevel].Columns;
      _values = new object[_columns.Count];
      _level = iLevel;
      _dataItem = oObject;

      this.FillValuesFromObject(oObject, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | (oGrid.UseShallowObjectBinding ? BindingFlags.DeclaredOnly : BindingFlags.FlattenHierarchy));
    }

    public GridItem(Grid oGrid, int iLevel, DataRow oRow)
    {
      _grid = oGrid;
      _columns = oGrid.Levels[iLevel].Columns;
      _values = new object[_columns.Count];
      _level = iLevel;
      _dataItem = oRow;
     
      this.FillValuesFromDataRow(oRow);
    }

    /// <summary>
    /// Returns an array of objects, representing cell values in this GridItem.
    /// </summary>
    /// <returns></returns>
    public object [] ToArray()
    {
      return _values;
    }

    private GridItemCollection _items;
    /// <summary>
    /// The collection of items below this item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In a hierarchical <see cref="Grid" />, each successive level of data is contained within 
    /// <see cref="GridItemCollection">GridItemCollections</see>, which are accessed through this property.
    /// The Grid itself has an <see cref="Grid.Items" /> collection which contains the top-level <see cref="GridItem">items</see>.
    /// Each item in that collection can contain items of its own, creating a hierarchy.  
    /// </para>
    /// <para>
    /// It is important to note that each level of data
    /// must have a corresponding <see cref="GridLevel" /> defined. The <see cref="GridItem.Level" /> property represents the index number
    /// of the corresponding level, within the Grid's <see cref="Grid.Levels" /> collection.
    /// </para>
    /// </remarks>
    public GridItemCollection Items
    {
      get
      {
        if(_items == null)
        {
          _items = new GridItemCollection();
        }

        return _items;
      }
    }

    /// <summary>
    /// Returns a value from the item indexed by the field name or index.
    /// </summary>
    /// <param name="obj">Field name or numeric index.</param>
    /// <returns>Cell value</returns>
    public object this[object obj]
    {
      get 
      {
        if(obj is string)
        {
          if(_columns != null && _values != null)
          {
            int iColumnIndex = _columns.IndexOf((string)obj);
            if(iColumnIndex >= 0)
            {
              return _values[iColumnIndex];
            }
            else
            {
              return null;
            }
          }
          else
          {
            return null;
          }
        }
        else if(obj is int)
        {
          return _values[(int)obj];
        }
        else
        {
          throw new ArgumentException("Only a string (field name) or integer index is permitted.");
        }
      }
      set
      {
        if(obj is string)
        {
          if(_columns != null && _values != null)
          {
            _values[_columns.IndexOf((string)obj)] = value;
          }
        }
        else if(obj is int)
        {
          _values[(int)obj] = value;
        }
        else
        {
          throw new ArgumentException("Only a string (column name) or int parameter are permitted.");
        }
      }
    }

    /// <summary>
    /// Returns the hash code for this item.
    /// </summary>
    /// <returns>The hash code for this item.</returns>
    public override int GetHashCode()
    {
      int iHashCode = 0;

      for(int i = 0; i < _values.Length; i++)
      {
        if(_values[i] != null)
        {
          iHashCode += _values[i].GetHashCode();
        }
      }

      return iHashCode;
    }

    /// <summary>
    /// Returns whether this item equals the passed-in item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="GridItem" /> object as an argument, returning a boolean value indicating whether the item it is called on
    /// is identical to the passed in item. In order for this method to return true both items must have identical values defined for each field.
    /// </para>
    /// </remarks>
    /// <param name="o">A GridItem.</param>
    /// <returns>Whether this item equals the passed-in item.</returns>
    public override bool Equals(object o)
    {
      if(o is GridItem && o != null)
      {
        GridItem other = (GridItem)o;

        for(int i = 0; i < _values.Length; i++)
        {
          if(!Object.Equals(this[i],other[i]))
          {
            return false;
          }
        }

        return true;
      }
      
      return false;
    }

    #region Private Methods

    private void FillValuesFromObject(object oObject, BindingFlags oFlags)
    {
      for(int i = 0; i < _columns.Count; i++)
      {
        if(_columns[i].DataField.IndexOf(".") > 0)
        {
          string [] arProperties = _columns[i].DataField.Split('.');
          object o = oObject;
          for(int prop = 0; prop < arProperties.Length; prop++)
          {
            PropertyInfo oProperty = o.GetType().GetProperty(arProperties[prop], oFlags);
            if(oProperty != null)
            {
              o = oProperty.GetValue(o, null);
            }
          }

          _values[i] = o;
        }
        else
        {
          PropertyInfo oProperty = oObject.GetType().GetProperty(_columns[i].DataField, oFlags);
          if(oProperty != null)
          {
            _values[i] = oProperty.GetValue(oObject, null);
          }
        }
      }
    }

    private void FillValuesFromDataRow(DataRow oRow)
    {
      for(int i = 0; i < _columns.Count; i++)
      {
        if(_columns[i].DataField != "")
        {
          _values[i] = oRow[_columns[i].DataField];
        }
        else
        {
          _values[i] = null;
        }
      }
    }

    #endregion
	}
}
