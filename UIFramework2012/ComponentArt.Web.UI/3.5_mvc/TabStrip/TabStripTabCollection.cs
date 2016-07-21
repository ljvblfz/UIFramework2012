using System;
using System.ComponentModel;
using System.Collections;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="TabStripTab"/> objects. 
  /// </summary>
  [ToolboxItem(false)]
  public class TabStripTabCollection : BaseMenuItemCollection, IList
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="oTabStrip">The parent <see cref="TabStrip"/> control.</param>
    /// <param name="oParent">The parent item of the collection, or null if this is the top-level collection.</param>
    public TabStripTabCollection(TabStrip oTabStrip, TabStripTab oParent)
      : base(oTabStrip, oParent)
    {
    }

    /// <summary>
    /// Gets the tabstrip tab at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the tabstrip tab to get.</param>
    /// <value>The <see cref="TabStripTab"/> at the specified index.</value>
    public new TabStripTab this[int index] 
    {
      get 
      {
        return (TabStripTab)nodeList[index];
      }
    }

    /// <summary>
    /// Gets or sets the tabstrip tab at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the tabstrip tab to get or set.</param>
    /// <value>The <see cref="TabStripTab"/> at the specified index.</value>
    object IList.this[int index] 
    {
      get 
      {
        return (TabStripTab)nodeList[index];
      }
      set
      {
        nodeList[index] = (TabStripTab)value;
      }
    }

    /// <summary>
    /// Adds a tabstrip tab to the end of the <b>TabStripTabCollection</b>.
    /// </summary>
    /// <param name="tab">The <see cref="TabStripTab"/> to be added to the end of the <see cref="TabStripTabCollection"/>.</param>
    /// <returns>The <b>TabStripTabCollection</b> index at which the <paramref name="tab"/> has been added.</returns>
    public int Add(TabStripTab tab) 
    {
      return base.Add(tab);
    }

    /// <summary>
    /// Determines whether a tabstrip tab is in the <b>TabStripTabCollection</b>.
    /// </summary>
    /// <param name="tab">The <see cref="TabStripTab"/> to locate in the <see cref="TabStripTabCollection"/>.</param>
    /// <returns><b>true</b> if <paramref name="tab"/> is found in the <b>TabStripTabCollection</b>; otherwise, <b>false</b>.</returns>
    public bool Contains(TabStripTab tab) 
    {
      return base.Contains(tab);
    }

    /// <summary>
    /// Searches for the specified <b>TabStripTab</b> and returns the zero-based index of it in the <b>TabStripTabCollection</b>.
    /// </summary>
    /// <param name="tab">The <see cref="TabStripTab"/> to locate in the <see cref="TabStripTabCollection"/>.</param>
    /// <value>The zero-based index of the <paramref name="tab"/> within the <b>TabStripTabCollection</b>, if found; otherwise, -1.</value>
    public int IndexOf(TabStripTab tab) 
    {
      return base.IndexOf(tab);
    }

    /// <summary>
    /// Inserts a tab into the <b>TabStripTabCollection</b> at the specified index. 
    /// </summary>
    /// <param name="index">The zero-based index at which the <paramref name="tab"/> should be inserted.</param>
    /// <param name="tab">The <see cref="TabStripTab"/> to be inserted into this <b>TabStripTabCollection</b>.</param>
    public void Insert(int index, TabStripTab tab) 
    {
      base.Insert(index, tab);
    }

    /// <summary>
    /// Removes the occurrence of a specific tab from the <b>TabStripTabCollection</b>.
    /// </summary>
    /// <param name="tab">The <see cref="TabStripTab"/> to be removed from the <see cref="TabStripTabCollection"/>.</param>
    public void Remove(TabStripTab tab) 
    {
      base.Remove(tab);
    }
  }
}