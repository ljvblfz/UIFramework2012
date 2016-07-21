using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Houses the content of a <see cref="SplitterPane"/> control.
  /// </summary>
  [ParseChildren(false)]
  [PersistChildren(true)]
  [ToolboxItem(false)]
  public class SplitterPaneContent : System.Web.UI.WebControls.WebControl
  {
    protected override void Render(HtmlTextWriter output)
    {
      this.RenderContents(output);
    }
  }
}
