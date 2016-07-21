using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class to define MenuItem objects for collections.
  /// </summary>
  public class MenuItemFactory
  {
    private MenuItemCollection items;
    /// <summary>
    /// Factory to define MenuItem objects for collections.
    /// </summary>
    /// <param name="items"></param>
    public MenuItemFactory(MenuItemCollection items)
    {
      this.items = items;
    }
    /// <summary>
    /// Add a MenuItem to a collection.
    /// </summary>
    /// <returns></returns>
    public virtual MenuItemBuilder Add()
    {
      MenuItem item = new MenuItem();

      items.Add(item);

      return new MenuItemBuilder(item);
    }
  }
}
