using System;
using System.ComponentModel;
using System.Collections;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="MenuItem"/> objects. 
  /// </summary>
  /// <remarks>
  /// <para>
  /// This is the collection class used by <see cref="Menu" /> and <see cref="MenuItem" /> objects for their Items property: 
  /// Menu <see cref="Menu.Items">Items</see>, MenuItem <see cref="MenuItem.Items">Items</see>.
  /// The index of an item within its containing collection can be discovered using the <see cref="IndexOf" /> method. 
  /// Items are added to the collection using the <see cref="Add" /> or <see cref="Insert" /> methods and can be removed with the 
  /// <see cref="Remove" /> method. 
  /// </para>
  /// </remarks>
  [ToolboxItem(false)]
  public class MenuItemCollection : BaseMenuItemCollection, IList
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="oMenu">The parent <see cref="Menu"/> control.</param>
    /// <param name="oParent">The parent item of the collection, or null if this is the top-level collection.</param>
    public MenuItemCollection(Menu oMenu, MenuItem oParent) : base(oMenu, oParent)
    {
    }

    /// <summary>
    /// Gets the menu item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the menu item to get.</param>
    /// <value>The <see cref="MenuItem"/> at the specified index.</value>
    public new MenuItem this[int index] 
    {
      get 
      {
        return (MenuItem)nodeList[index];
      }
    }

    /// <summary>
    /// Gets or sets the menu item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the menu item to get or set.</param>
    /// <value>The menu item at the specified index.</value>
    object IList.this[int index] 
    {
      get 
      {
        return (MenuItem)nodeList[index];
      }
      set
      {
        nodeList[index] = (MenuItem)value;
      }
    }

    /// <summary>
    /// Adds a menu item to the end of the <b>MenuItemCollection</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="MenuItem" /> as an argument, adding it to the collection. The <see cref="MenuItemCollection.Insert" /> 
    /// method is also used to add a node to a collection, but instead of simply adding it to the end of the collection (as this method does), it allows
    /// a node to be added at a specific index.
    /// </para>
    /// <para>
    /// To remove an item from the collection, use the <see cref="MenuItemCollection.Remove" /> method.
    /// </para>
    /// </remarks>
    /// <param name="item">The <see cref="MenuItem"/> to be added to the end of the <see cref="MenuItemCollection"/>.</param>
    /// <returns>The <b>MenuItemCollection</b> index at which the <paramref name="item"/> has been added.</returns>
    public int Add(MenuItem item) 
    {
      return base.Add(item);
    }
    
    /// <summary>
    /// Determines whether a menu item is in the <b>MenuItemCollection</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="MenuItem" /> as an argument, returning a boolean value indicating whether the node is contained within
    /// the collection.
    /// </para>
    /// </remarks>
    /// <param name="item">The <see cref="MenuItem"/> to locate in the <see cref="MenuItemCollection"/>.</param>
    /// <returns><b>true</b> if <paramref name="item"/> is found in the <b>MenuItemCollection</b>; otherwise, <b>false</b>.</returns>
    public bool Contains(MenuItem item) 
    {
      return base.Contains(item);
    }

    /// <summary>
    /// Searches for the specified <b>MenuItem</b> and returns the zero-based index of it in the <b>MenuItemCollection</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="MenuItem" /> as an argument, returning that node's index within the collection. If the item is not contained within
    /// the collection, this method will return -1.
    /// </para>
    /// </remarks> 
    /// <param name="item">The <see cref="MenuItem"/> to locate in the <see cref="MenuItemCollection"/>.</param>
    /// <value>The zero-based index of the <paramref name="item"/> within the <b>MenuItemCollection</b>, if found; otherwise, -1.</value>
    public int IndexOf(MenuItem item) 
    {
      return base.IndexOf(item);
    }

    /// <summary>
    /// Inserts an item into the <b>MenuItemCollection</b> at the specified index. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to add a <see cref="MenuItem" /> to the collection. Unlike the <see cref="MenuItemCollection.Add" /> method, this method 
    /// also accepts an index as an argument, inserting the item at a specific position within the collection.
    /// </para>
    /// <para>
    /// To remove an item from the collection, use the <see cref="MenuItemCollection.Remove" /> method.
    /// </para>
    /// </remarks>
    /// <param name="index">The zero-based index at which the <paramref name="item"/> should be inserted.</param>
    /// <param name="item">The <see cref="MenuItem"/> to be inserted into this <b>MenuItemCollection</b>.</param>
    public void Insert(int index, MenuItem item) 
    {
      base.Insert(index, item);
    }
    
    /// <summary>
    /// Removes the occurrence of a specific item from the <b>MenuItemCollection</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="MenuItem" /> as an argument, removing that item from the collection. Items are added to the collection
    /// using either the <see cref="MenuItemCollection.Add" /> method or the <see cref="MenuItemCollection.Insert" /> method.
    /// </para>
    /// </remarks>
    /// <param name="item">The <see cref="MenuItem"/> to be removed from the <see cref="MenuItemCollection"/>.</param>
    public void Remove(MenuItem item) 
    {
      base.Remove(item);
    }
  }
}