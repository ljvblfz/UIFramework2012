using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class to define NavBarItem objects for collections.
  /// </summary>
  public class NavBarItemFactory
  {
    private NavBarItemCollection items;
    /// <summary>
    /// Factory to define NavBarItem objects for collections.
    /// </summary>
    /// <param name="items"></param>
    public NavBarItemFactory(NavBarItemCollection items)
    {
      this.items = items;
    }
    /// <summary>
    /// Add a NavBarItem to a collection.
    /// </summary>
    /// <returns></returns>
    public virtual NavBarItemBuilder Add()
    {
      NavBarItem item = new NavBarItem();

      items.Add(item);

      return new NavBarItemBuilder(item);
    }
  }
}
