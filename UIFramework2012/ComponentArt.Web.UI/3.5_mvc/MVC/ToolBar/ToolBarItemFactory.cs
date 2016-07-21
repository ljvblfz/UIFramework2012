using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class to define ToolBarItem objects for collections.
  /// </summary>
  public class ToolBarItemFactory
  {
    private ToolBarItemCollection items;
    /// <summary>
    /// Factory to define ToolBarItem objects for collections.
    /// </summary>
    /// <param name="items"></param>
    public ToolBarItemFactory(ToolBarItemCollection items)
    {
      this.items = items;
    }
    /// <summary>
    /// Add a ToolBarItem to a collection.
    /// </summary>
    /// <returns></returns>
    public virtual ToolBarItemBuilder Add()
    {
      ToolBarItem item = new ToolBarItem();

      items.Add(item);

      return new ToolBarItemBuilder(item);
    }
  }
}
