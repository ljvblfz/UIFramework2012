using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentArt.Web.UI
{
  public class CustomAttributeMappingFactory
  {
    private CustomAttributeMappingCollection items;

    public CustomAttributeMappingFactory(CustomAttributeMappingCollection items)
    {
      this.items = items;
    }

    public virtual CustomAttributeMappingBuilder Add()
    {
      CustomAttributeMapping item = new CustomAttributeMapping();

      items.Add(item);

      return new CustomAttributeMappingBuilder(item);
    }
  }
}
