using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class to define GridConditionalFormat objects for collections.
  /// </summary>
  public class DataGridConditionalFormatFactory
  {
    private GridConditionalFormatCollection formats;
    /// <summary>
    /// Factory to define GridConditionalFormat objects for GridConditionalFormatCollections.
    /// </summary>
    /// <param name="formats"></param>
    public DataGridConditionalFormatFactory(GridConditionalFormatCollection formats)
    {
      this.formats = formats;
    }
    /// <summary>
    /// Add a GridConditionalFormat to the collection.
    /// </summary>
    /// <returns></returns>
    public virtual DataGridConditionalFormatBuilder Add()
    {
      GridConditionalFormat format = new GridConditionalFormat();

      formats.Add(format);

      return new DataGridConditionalFormatBuilder(format);
    }
  }
}

