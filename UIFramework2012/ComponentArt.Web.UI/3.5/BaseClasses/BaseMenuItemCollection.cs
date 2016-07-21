using System;
using System.ComponentModel;
using System.Collections;



namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="BaseMenuItem"/> objects. 
  /// </summary>
  [ToolboxItem(false)]
  public abstract class BaseMenuItemCollection : NavigationNodeCollection, IList
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="oControl">The parent <see cref="BaseMenu"/> control.</param>
    /// <param name="oParent">The parent item of the collection, or null if this is the top-level collection.</param>
    public BaseMenuItemCollection(BaseMenu oControl, BaseMenuItem oParent) : base(oControl, oParent)
    {
    }

    /// <summary>
    /// Gets the base menu item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the base menu item to get.</param>
    /// <value>The <see cref="BaseMenuItem"/> at the specified index.</value>
    internal new BaseMenuItem this[int index] 
    {
      get 
      {
        return (BaseMenuItem)base[index];
      }
    }

    /// <summary>
    /// Gets or sets the base menu item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the base menu item to get or set.</param>
    /// <value>The base menu item at the specified index.</value>
    object IList.this[int index] 
    {
      get 
      {
        return (BaseMenuItem)base[index];
      }
      set
      {
        nodeList[index] = (BaseMenuItem)value;
      }
    }

    /// <summary>
    /// Adds a base menu item to the end of the <b>BaseMenuItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="BaseMenuItem"/> to be added to the end of the <see cref="BaseMenuItemCollection"/>.</param>
    /// <returns>The <b>BaseMenuItemCollection</b> index at which the <paramref name="item"/> has been added.</returns>
    internal int Add(BaseMenuItem item) 
    {
      int iRetValue = base.Add(item);

      if(item.ParentItem != null && item.ParentBaseMenu != null)
      {
        if(item.ParentItem.m_bLooksApplied)
        {
          item.ApplyLooks();
        }
      }

      return iRetValue;
    }

    /// <summary>
    /// Determines whether a base menu item is in the <b>BaseMenuItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="BaseMenuItem"/> to locate in the <see cref="BaseMenuItemCollection"/>.</param>
    /// <returns><b>true</b> if <paramref name="item"/> is found in the <b>BaseMenuItemCollection</b>; otherwise, <b>false</b>.</returns>
    internal bool Contains(BaseMenuItem item) 
    {
      return base.Contains(item);
    }

    /// <summary>
    /// Searches for the specified <b>BaseMenuItem</b> and returns the zero-based index of it in the <b>BaseMenuItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="BaseMenuItem"/> to locate in the <see cref="BaseMenuItemCollection"/>.</param>
    /// <value>The zero-based index of the <paramref name="item"/> within the <b>BaseMenuItemCollection</b>, if found; otherwise, -1.</value>
    internal int IndexOf(BaseMenuItem item) 
    {
      return base.IndexOf(item);
    }

    /// <summary>
    /// Inserts an item into the <b>BaseMenuItemCollection</b> at the specified index. 
    /// </summary>
    /// <param name="index">The zero-based index at which the <paramref name="item"/> should be inserted.</param>
    /// <param name="item">The <see cref="BaseMenuItem"/> to be inserted into this <b>BaseMenuItemCollection</b>.</param>
    internal void Insert(int index, BaseMenuItem item) 
    {
      base.Insert(index, item);
      
      if(item.ParentItem != null && item.ParentBaseMenu != null)
      {
        if(item.ParentItem.m_bLooksApplied)
        {
          item.ApplyLooks();
        }
      }
    }

    /// <summary>
    /// Removes the occurrence of a specific item from the <b>BaseMenuItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="BaseMenuItem"/> to be removed from the <see cref="BaseMenuItemCollection"/>.</param>
    internal void Remove(BaseMenuItem item) 
    {
      base.Remove(item);
    }
  }
}