using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class to define TabStripTab objects for collections.
  /// </summary>
    public class TabStripTabFactory
    {
        private TabStripTabCollection tabs;
      /// <summary>
      /// Factory to define TabStripTab objects for collections.
      /// </summary>
      /// <param name="tabs"></param>
        public TabStripTabFactory(TabStripTabCollection tabs)
        {
            this.tabs = tabs;
        }
      /// <summary>
      /// Add a TabstripTab to a collection.
      /// </summary>
      /// <returns></returns>
        public virtual TabStripTabBuilder Add()
        {
            TabStripTab tab = new TabStripTab();

            tabs.Add(tab);

            return new TabStripTabBuilder(tab);
        }
    }
}
