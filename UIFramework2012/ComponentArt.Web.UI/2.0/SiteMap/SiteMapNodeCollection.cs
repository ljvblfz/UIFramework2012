using System;
using System.ComponentModel;
using System.Collections;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="SiteMapNode"/> objects. 
  /// </summary>
  [ToolboxItem(false)]
  public class SiteMapNodeCollection : NavigationNodeCollection, IList
  {
    public SiteMapNodeCollection(SiteMap oSiteMap, SiteMapNode oParent) : base(oSiteMap, oParent)
    {
    }

    public new SiteMapNode this[int index] 
    {
      get 
      {
        return (SiteMapNode)nodeList[index];
      }
    }

    object IList.this[int index] 
    {
      get 
      {
        return (SiteMapNode)nodeList[index];
      }
      set
      {
        nodeList[index] = (SiteMapNode)value;
      }
    }

    /// <summary>
    /// Add method.
    /// </summary>
    /// <param name="item">The SiteMapNode to be added</param>
    /// <returns>Index of added node in the collection</returns>
    public int Add(SiteMapNode item) 
    {
      return base.Add(item);
    }
    
    /// <summary>
    /// Contains method.
    /// </summary>
    /// <param name="item">A SiteMapNode</param>
    /// <returns>Whether this collection contains the given item</returns>
    public bool Contains(SiteMapNode item) 
    {
      return base.Contains(item);
    }

    /// <summary>
    /// IndexOf method.
    /// </summary>
    /// <param name="item">A SiteMapNode</param>
    /// <returns>Index of the given node in this collection, or a negative value.</returns>
    public int IndexOf(SiteMapNode item) 
    {
      return base.IndexOf(item);
    }

    /// <summary>
    /// Insert method.
    /// </summary>
    /// <param name="index">The index at which to insert the given SiteMapNode.</param>
    /// <param name="item">A SiteMapNode to be inserted into this collection.</param>
    public void Insert(int index, SiteMapNode item) 
    {
      base.Insert(index, item);
    }
    
    /// <summary>
    /// Remove method.
    /// </summary>
    /// <param name="item">The SiteMapNode to be removed from this collection.</param>
    public void Remove(SiteMapNode item) 
    {
      base.Remove(item);
    }
  }
}