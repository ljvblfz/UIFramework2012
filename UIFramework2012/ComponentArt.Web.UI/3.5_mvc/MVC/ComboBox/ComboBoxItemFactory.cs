using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class to define ComboBoxItem objects for collections.
  /// </summary>
  public class ComboBoxItemFactory
  {
    private ComboBoxItemCollection items;
    private ComboBox comboBox;
    /// <summary>
    /// Factory to define ComboBoxItem objects for collections.
    /// </summary>
    /// <param name="items"></param>
    /// <param name="comboBox"></param>
    public ComboBoxItemFactory(ComboBoxItemCollection items, ComboBox comboBox)
    {
      this.items = items;
      this.comboBox = comboBox;
    }
    /// <summary>
    /// Add a ComboBoxItem to a collection.
    /// </summary>
    /// <returns></returns>
    public virtual ComboBoxItemBuilder Add()
    {
      ComboBoxItem item = new ComboBoxItem();

      items.Add(item);

      return new ComboBoxItemBuilder(item);
    }
  }
}
