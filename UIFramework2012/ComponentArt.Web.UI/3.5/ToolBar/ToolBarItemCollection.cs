using System;
using System.ComponentModel;
using System.Collections;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="ToolBarItem"/> objects. 
  /// </summary>
  [ToolboxItem(false)]
  public class ToolBarItemCollection : IEnumerable, ICollection, IList
  {
    private ToolBar _toolbar;
    private ArrayList _items;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="toolbar">The parent <see cref="ToolBar"/> control.</param>
    public ToolBarItemCollection(ToolBar toolbar)
    {
      this._toolbar = toolbar;
      this._items = new ArrayList();
    }

    /// <summary>
    /// Gets the toolbar item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the toolbar item to get.</param>
    /// <value>The <see cref="ToolBarItem"/> at the specified index.</value>
    public ToolBarItem this[int index] 
    {
      get 
      {
        return (ToolBarItem)this._items[index];
      }
    }

    /// <summary>
    /// Adds a menu item to the end of the <b>ToolBarItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="ToolBarItem"/> to be added to the end of the <see cref="ToolBarItemCollection"/>.</param>
    /// <returns>The <b>ToolBarItemCollection</b> index at which the <paramref name="item"/> has been added.</returns>
    public int Add(ToolBarItem item) 
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      item._parentToolBar = this._toolbar;
      this._items.Add(item);
      return this._items.Count - 1;
    }

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    public void Clear()
    {
      this._items.Clear();
    }

    /// <summary>
    /// Determines whether a toolbar item is in this <b>ToolBarItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="ToolBarItem"/> to locate in the <see cref="ToolBarItemCollection"/>.</param>
    /// <returns><b>true</b> if <paramref name="item"/> is found in the <b>ToolBarItemCollection</b>; otherwise, <b>false</b>.</returns>
    public bool Contains(ToolBarItem item)
    {
      if (item == null)
      {
        return false;
      }
      return this._items.Contains(item);
    }

    /// <summary>
    /// Searches for the specified <b>ToolBarItem</b> and returns the zero-based index of it in the <b>ToolBarItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="ToolBarItem"/> to locate in the <see cref="ToolBarItemCollection"/>.</param>
    /// <value>The zero-based index of the <paramref name="item"/> within the <b>ToolBarItemCollection</b>, if found; otherwise, -1.</value>
    public int IndexOf(ToolBarItem item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      return this._items.IndexOf(item);
    }

    /// <summary>
    /// Inserts an item into the <b>ToolBarItemCollection</b> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which the <paramref name="item"/> should be inserted.</param>
    /// <param name="item">The <see cref="ToolBarItem"/> to be inserted into this <b>ToolBarItemCollection</b>.</param>
    public void Insert(int index, ToolBarItem item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      item._parentToolBar = this._toolbar;
      this._items.Insert(index, item);
    }

    /// <summary>
    /// Removes the occurrence of a specific item from the <b>MenuItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="ToolBarItem"/> to be removed from the <see cref="ToolBarItemCollection"/>.</param>
    public void Remove(ToolBarItem item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      int index = this.IndexOf(item);
      if (index >= 0)
      {
        this.RemoveAt(index);
      }
    }

    /// <summary>
    /// Removes the item at the specified index of the collection.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    public void RemoveAt(int index)
    {
      this._items.RemoveAt(index);
    }

    #region IEnumerable Implementation

    public IEnumerator GetEnumerator()
    {
      return this._items.GetEnumerator();
    }

    #endregion IEnumerable Implementation

    #region ICollection Implementation

    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    /// <value>
    /// The number of elements contained in the collection.
    /// </value>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Count
    {
      get
      {
        return this._items.Count;
      }
    }

    /// <summary>
    /// Copies the entire collection to a compatible one-dimensional Array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from 
    /// this collection. The <b>Array</b> must have zero-based indexing.</param>
    /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    public void CopyTo(Array array, int index)
    {
      this._items.CopyTo(array, index);
    }

    /// <summary>
    /// Gets a value indicating whether access to the collection is synchronized (thread safe).
    /// </summary>
    /// <value>
    /// <b>true</b> if access to the collection is synchronized (thread safe); otherwise, <b>false</b>. The default is <b>false</b>.
    /// </value>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsSynchronized
    {
      get
      {
        return true;
      }
    }

    /// <summary>
    /// Gets an object that can be used to synchronize access to the collection.
    /// </summary>
    /// <value>
    /// An object that can be used to synchronize access to the collection.
    /// </value>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object SyncRoot
    {
      get
      {
        return this._items.SyncRoot;
      }
    }

    #endregion ICollection Implementation

    #region IList Implementation

    object IList.this[int index]
    {
      get
      {
        return this._items[index];
      }
      set
      {
        this._items[index] = (ToolBarItem)value;
      }
    }

    bool IList.IsFixedSize
    {
      get
      {
        return false;
      }
    }

    bool IList.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    int IList.Add(object item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      if (!(item is ToolBarItem))
      {
        throw new ArgumentException("item must be a ToolBarItem");
      }
      return this.Add((ToolBarItem)item);
    }

    void IList.Clear()
    {
      this.Clear();
    }

    bool IList.Contains(object item)
    {
      return this.Contains(item as ToolBarItem);
    }

    int IList.IndexOf(object item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      if (!(item is ToolBarItem))
      {
        throw new ArgumentException("item must be a ToolBarItem");
      }
      return this.IndexOf((ToolBarItem)item);
    }

    void IList.Insert(int index, object item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      if (!(item is ToolBarItem))
      {
        throw new ArgumentException("item must be a ToolBarItem");
      }
      this.Insert(index, (ToolBarItem)item);
    }

    void IList.Remove(object item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      if (!(item is ToolBarItem))
      {
        throw new ArgumentException("item must be a ToolBarItem");
      }
      this.Remove((ToolBarItem)item);
    }

    void IList.RemoveAt(int index)
    {
      this.RemoveAt(index);
    }

    #endregion IList Implementation

  }
}