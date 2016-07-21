using System;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;

namespace ComponentArt.Web.UI
{
  public class EditorToolBarCollection : CollectionBase
  {
    public ToolBar this[int index]
    {
      get
      {
        return (ToolBar)base.List[index];
      }
      set
      {
        base.List[index] = value;
      }
    }

    public int Add(ToolBar toolbar)
    {
      return this.List.Add(toolbar);
    }
  }
}
