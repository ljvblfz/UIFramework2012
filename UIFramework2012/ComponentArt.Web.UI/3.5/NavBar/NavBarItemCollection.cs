using System;
using System.ComponentModel;
using System.Collections;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="NavBarItem"/> objects. 
  /// </summary>
  [ToolboxItem(false)]
  public class NavBarItemCollection : BaseMenuItemCollection, IList
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="oNavBar">The parent <see cref="NavBar"/> control.</param>
    /// <param name="oParent">The parent tab of the collection, or null if this is the top-level collection.</param>
    public NavBarItemCollection(NavBar oNavBar, NavBarItem oParent)
      : base(oNavBar, oParent)
    {
    }

    /// <summary>
    /// Gets the navbar item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the navbar item to get.</param>
    /// <value>The <see cref="NavBarItem"/> at the specified index.</value>
    public new NavBarItem this[int index]
    {
      get 
      {
        return (NavBarItem)nodeList[index];
      }
    }

    /// <summary>
    /// Gets or sets the navbar item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the navbar item to get or set.</param>
    /// <value>The <see cref="NavBarItem"/> at the specified index.</value>
    object IList.this[int index]
    {
      get 
      {
        return (NavBarItem)nodeList[index];
      }
      set
      {
        nodeList[index] = (NavBarItem)value;
      }
    }

    /// <summary>
    /// Adds a navbar item to the end of the <b>NavBarItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="NavBarItem"/> to be added to the end of the <see cref="NavBarItemCollection"/>.</param>
    /// <returns>The <b>NavBarItemCollection</b> index at which the <paramref name="item"/> has been added.</returns>
    public int Add(NavBarItem item)
    {
      return base.Add(item);
    }

    /// <summary>
    /// Determines whether a navbar item is in the <b>NavBarItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="NavBarItem"/> to locate in the <see cref="NavBarItemCollection"/>.</param>
    /// <returns><b>true</b> if <paramref name="item"/> is found in the <b>NavBarItemCollection</b>; otherwise, <b>false</b>.</returns>
    public bool Contains(NavBarItem item)
    {
      return base.Contains(item);
    }

    /// <summary>
    /// Searches for the specified <b>NavBarItem</b> and returns the zero-based index of it in the <b>NavBarItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="NavBarItem"/> to locate in the <see cref="NavBarItemCollection"/>.</param>
    /// <value>The zero-based index of the <paramref name="item"/> within the <b>NavBarItemCollection</b>, if found; otherwise, -1.</value>
    public int IndexOf(NavBarItem item) 
    {
      return base.IndexOf(item);
    }

    /// <summary>
    /// Inserts an item into the <b>NavBarItemCollection</b> at the specified index. 
    /// </summary>
    /// <param name="index">The zero-based index at which the <paramref name="item"/> should be inserted.</param>
    /// <param name="item">The <see cref="NavBarItem"/> to be inserted into this <b>NavBarItemCollection</b>.</param>
    public void Insert(int index, NavBarItem item) 
    {
      base.Insert(index, item);
    }

    /// <summary>
    /// Removes the occurrence of a specific item from the <b>NavBarItemCollection</b>.
    /// </summary>
    /// <param name="item">The <see cref="NavBarItem"/> to be removed from the <see cref="NavBarItemCollection"/>.</param>
    public void Remove(NavBarItem item) 
    {
      base.Remove(item);
    }
  }
}