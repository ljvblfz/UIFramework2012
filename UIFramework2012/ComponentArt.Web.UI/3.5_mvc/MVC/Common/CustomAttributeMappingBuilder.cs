using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentArt.Web.UI
{
  public class CustomAttributeMappingBuilder
  {
    private readonly CustomAttributeMapping item;

    public CustomAttributeMappingBuilder(CustomAttributeMapping item)
    {
      this.item = item;
    }

    public CustomAttributeMappingBuilder To(string value)
    {
      item.To = value;
      return this;
    }
    public CustomAttributeMappingBuilder From(string value)
    {
      item.From = value;
      return this;
    }
  }
}
