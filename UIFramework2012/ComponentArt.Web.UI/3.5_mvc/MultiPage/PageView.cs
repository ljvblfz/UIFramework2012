using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ComponentArt.Web.UI
{
	/// <summary>
  /// One of the page fragments managed by <see cref="MultiPage"/> control.
	/// </summary>
  /// <remarks>
  /// The PageView control contains ASP.NET content to be used for one content section inside the MultiPage control.
  /// </remarks>
  /// <seealso cref="MultiPage.PageViews"/>
  [ParseChildren(false)]
  [PersistChildren(true)]
  [ToolboxItem(false)]
  public class PageView : System.Web.UI.WebControls.WebControl
	{
    private MultiPage _Parent;

    public PageView() : base()
    {
    }

    [DefaultValue(true)]
    public override bool Enabled
    {
      get
      {
        object o = ViewState["Enabled"];
        if(o == null)
        {
          if(_Parent != null)
          {
            return _Parent.Enabled;
          }
          else
          {
            return true;
          }
        }
        else
        {
          return (bool)o;
        }
      }
      set
      {
        ViewState["Enabled"] = value;
      }
    }

    /// <summary>
    /// The parent MultiPage control.
    /// </summary>
    protected internal MultiPage ParentMultiPage
    {
      get { return _Parent; }
      set { _Parent = value; }
    }

    protected override void Render(HtmlTextWriter output)
    {
      // render table tag
      output.Write("<table"); // begin <table>
      output.WriteAttribute("id", this.ClientID);
      output.WriteAttribute("cellpadding", "0");
      output.WriteAttribute("cellspacing", "0");
      output.WriteAttribute("border", "0");

      if (this.CssClass != string.Empty)
      {
        output.WriteAttribute("class", this.CssClass);
      }
      if (!this.Enabled)
      {
        output.WriteAttribute("disabled", "disabled");
      }

      // Output style
      output.Write(" style=\"");
      if (!this.Height.IsEmpty)
      {
        output.WriteStyleAttribute("height", this.Height.ToString());
      }
      if (!this.Width.IsEmpty)
      {
        output.WriteStyleAttribute("width", this.Width.ToString());
      }
      foreach (string sKey in this.Style.Keys)
      {
        output.WriteStyleAttribute(sKey, this.Style[sKey]);
      }
      if (!this.BackColor.IsEmpty)
      {
        output.WriteStyleAttribute("background-color", System.Drawing.ColorTranslator.ToHtml(this.BackColor));
      }
      if (!this.BorderWidth.IsEmpty)
      {
        output.WriteStyleAttribute("border-width", this.BorderWidth.ToString());
      }
      if (this.BorderStyle != BorderStyle.NotSet)
      {
        output.WriteStyleAttribute("border-style", this.BorderStyle.ToString());
      }
      if (!this.BorderColor.IsEmpty)
      {
        output.WriteStyleAttribute("border-color", System.Drawing.ColorTranslator.ToHtml(this.BorderColor));
      }
      output.Write("\">"); // end <table>

      output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
      
      output.AddStyleAttribute("vertical-align", "top");
      output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

      // Render contents unless we're only rendering selected and this pageview isn't selected
      if( !(  ParentMultiPage != null &&
        ParentMultiPage.RenderSelectedPageOnly &&
        ParentMultiPage.SelectedIndex >= 0 &&
        ParentMultiPage.PageViews[ParentMultiPage.SelectedIndex] != this)
        )
      {
        RenderContents(output);
      }

      output.RenderEndTag(); // </td>
      output.RenderEndTag(); // </tr>

      output.Write("</table>"); // </table>
    }
	}
}
