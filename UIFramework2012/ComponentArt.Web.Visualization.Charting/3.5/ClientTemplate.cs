using System;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;


namespace ComponentArt.Web.Visualization.Charting
{
  /// <summary>
  /// Allows rendering of templated content that is data-bound and generated on the client instead of on the server.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Client templates differ from server templates in that they are bound to data on the client.
  /// They consist of markup and client-side binding expressions and are
  /// the suggested way of templating for situations where ASP.NET controls are not required. 
  /// </para>
  /// <para>
  /// For more information on templates, see 
  /// <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Overview of Templates in Web.UI</see>.
  /// </para>
  /// </remarks>
  [ToolboxItem(false)]
  public class ClientTemplate : Control
  {
    /// <summary>
    /// The content of this client template.
    /// </summary>
    [DefaultValue("")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Text
    {
      get
      {
        if (this.Controls.Count > 0 && this.Controls[0] is LiteralControl)
        {
          return ((LiteralControl)this.Controls[0]).Text;
        }

        return "";
      }
      set
      {
        this.Controls.Clear();
        this.Controls.Add(new LiteralControl(value));
      }
    }

    public override string ToString()
    {
      return "['" + this.ID + "','" + this.Text.Replace("\n", "").Replace("\r", "").Replace("'", "\\'") + "']";
    }
  }

  /// <summary>
  /// A collection of <see cref="ClientTemplate"/> objects.
  /// </summary>
  public class ClientTemplateCollection : CollectionBase
  {
    public new ClientTemplate this[int index]
    {
      get
      {
        return (ClientTemplate)this.List[index];
      }
    }

    public int Add(ClientTemplate template)
    {
      return this.List.Add(template);
    }

    public override string ToString()
    {
      string[] templateArray = new string[this.List.Count];
      for (int i = 0; i < templateArray.Length; i++)
      {
        templateArray[i] = this.List[i].ToString();
      }
      return "[" + string.Join(",", templateArray) + "]";
    }
  }
}
