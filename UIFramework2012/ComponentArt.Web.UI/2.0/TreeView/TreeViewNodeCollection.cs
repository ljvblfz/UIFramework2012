using System;
using System.ComponentModel;
using System.Collections;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="TreeViewNode"/> objects. 
  /// </summary>
  /// <remarks>
  /// <para>
  /// This is the collection class used by <see cref="TreeView" /> and <see cref="TreeViewNode" /> objects for their Nodes property: 
  /// TreeView <see cref="TreeView.Nodes" />, TreeViewNode <see cref="TreeViewNode.Nodes" />.
  /// The index of a node within its containing collection can be discovered using the <see cref="IndexOf" /> method. 
  /// Nodes are added to the collection using the <see cref="Add" /> or <see cref="Insert" /> methods and can be removed with the 
  /// <see cref="Remove" /> method. 
  /// </para>
  /// </remarks>
	[ToolboxItem(false)]
  public class TreeViewNodeCollection : NavigationNodeCollection, IList
	{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="oTreeView">The parent <see cref="TreeView"/> control.</param>
    /// <param name="oParent">The parent node of the collection, or null if this is the top-level collection.</param>
    public TreeViewNodeCollection(TreeView oTreeView, TreeViewNode oParent)
      : base(oTreeView, oParent)
		{
		}

    /// <summary>
    /// Gets the treeview node at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the treeview node to get.</param>
    /// <value>The <see cref="TreeViewNode"/> at the specified index.</value>
    public new TreeViewNode this[int index] 
		{
			get 
			{
				return (TreeViewNode)nodeList[index];
			}
		}

    /// <summary>
    /// Gets or sets the treeview node at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the treeview node to get or set.</param>
    /// <value>The <see cref="TreeViewNode"/> at the specified index.</value>
    object IList.this[int index] 
		{
			get 
			{
				return (TreeViewNode)nodeList[index];
			}
			set
			{
				nodeList[index] = (TreeViewNode)value;
			}
		}

    /// <summary>
    /// Adds a treeview node to the end of the <b>TreeViewNodeCollection</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="TreeViewNode" /> as an argument, adding it to the collection. The <see cref="TreeViewNodeCollection.Insert" />
    /// method is also used to add a node to a collection, but instead of simply adding it to the end of the collection (as this method does),
    /// it allows a node to be added at a specific index.  
    /// </para>
    /// <para>
    /// To remove an item from the collection, use the <see cref="TreeViewNodeCollection.Remove" /> method.
    /// </para>
    /// </remarks>
    /// <param name="node">The <see cref="TreeViewNode"/> to be added to the end of the <see cref="TreeViewNodeCollection"/>.</param>
    /// <returns>The <b>TreeViewNodeCollection</b> index at which the <paramref name="node"/> has been added.</returns>
    public int Add(TreeViewNode node) 
		{
			return base.Add(node);
		}

    /// <summary>
    /// Determines whether a treeview node is in the <b>TreeViewNodeCollection</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="TreeViewNode" /> as an argument, returning a boolean value indicating whether the node is contained
    /// within the collection.
    /// </para>
    /// </remarks>
    /// <param name="node">The <see cref="TreeViewNode"/> to locate in the <see cref="TreeViewNodeCollection"/>.</param>
    /// <returns><b>true</b> if <paramref name="node"/> is found in the <b>TreeViewNodeCollection</b>; otherwise, <b>false</b>.</returns>
    public bool Contains(TreeViewNode node) 
		{
			return base.Contains(node);
		}

    /// <summary>
    /// Searches for the specified <b>TreeViewNode</b> and returns the zero-based index of it in the <b>TreeViewNodeCollection</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="TreeViewNode" /> as an argument, returning that node's index within the collection.
    /// If the item is not contained within the collection, this method will return -1.
    /// </para>
    /// </remarks>
    /// <param name="node">The <see cref="TreeViewNode"/> to locate in the <see cref="TreeViewNodeCollection"/>.</param>
    /// <value>The zero-based index of the <paramref name="node"/> within the <b>TreeViewNodeCollection</b>, if found; otherwise, -1.</value>
    public int IndexOf(TreeViewNode node) 
		{
			return base.IndexOf(node);
		}

    /// <summary>
    /// Inserts a node into the <b>TreeViewNodeCollection</b> at the specified index. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to add a <see cref="TreeViewNode" /> to the collection. Unlike the <see cref="TreeViewNodeCollection.Add" /> method, 
    /// this method also accepts an index as an argument, inserting the item at a specific position within the collection.
    /// </para>
    /// <para>
    /// To remove an item from the collection, use the <see cref="TreeViewNodeCollection.Remove" /> method.
    /// </para>
    /// </remarks>
    /// <param name="index">The zero-based index at which the <paramref name="node"/> should be inserted.</param>
    /// <param name="node">The <see cref="TreeViewNode"/> to be inserted into this <b>TreeViewNodeCollection</b>.</param>
    public void Insert(int index, TreeViewNode node) 
		{
			base.Insert(index, node);
		}

    /// <summary>
    /// Removes the occurrence of a specific node from the <b>TreeViewNodeCollection</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method accepts a <see cref="TreeViewNode" /> as an argument, removing that node from the collection.
    /// Items are added to the collection using either the <see cref="TreeViewNodeCollection.Add" /> method or the 
    /// <see cref="TreeViewNodeCollection.Insert" /> method.
    /// </para>
    /// </remarks>
    /// <param name="node">The <see cref="TreeViewNode"/> to be removed from the <see cref="TreeViewNodeCollection"/>.</param>
    public void Remove(TreeViewNode node) 
		{
			base.Remove(node);
		}
	}
}