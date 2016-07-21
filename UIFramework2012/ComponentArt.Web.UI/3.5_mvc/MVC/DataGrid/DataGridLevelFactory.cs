using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class to define DataGridLevel objects for collections.
  /// </summary>
  public class DataGridLevelFactory
  {
    private GridLevelCollection levels;
    /// <summary>
    /// Factory to define DataGridLevel objects for DataGridLevelCollections.
    /// </summary>
    /// <param name="levels"></param>
    public DataGridLevelFactory(GridLevelCollection levels)
    {
      this.levels = levels;
    }
    /// <summary>
    /// Add a DataGridLevel to a DataGridLevelCollection.
    /// </summary>
    /// <returns></returns>
    public virtual DataGridLevelBuilder Add()
    {
      GridLevel item = new GridLevel();
      levels.Add(item);

      return new DataGridLevelBuilder(item);
    }
  }
}
