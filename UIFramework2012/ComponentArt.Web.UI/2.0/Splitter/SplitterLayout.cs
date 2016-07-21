using System;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace ComponentArt.Web.UI
{
	/// <summary>
  /// Defines the pane layout for the <see cref="Splitter"/> control.
	/// </summary>
  [ParseChildren(true)]
  [ToolboxItem(false)]
	public class SplitterLayout : System.Web.UI.WebControls.WebControl
	{
    internal bool Fixed = false;

    #region Public Properties

    private SplitterPaneGroup _panes;
    /// <summary>
    /// The top SplitterPaneGroup in this layout.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public SplitterPaneGroup Panes
    {
      get
      {
        return _panes;
      }
      set
      {
        _panes = value;
      }
    }

    #endregion

    #region Methods

		public SplitterLayout()
		{
		}

    /// <summary>
    /// Overridden. Filters out all objects except SplitterPaneCollection objects.
    /// </summary>
    /// <param name="obj">The parsed element.</param>
    protected override void AddParsedSubObject(object obj)
    {
      if (obj is SplitterPaneGroup)
      {
        base.AddParsedSubObject(obj);
      }
    }

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();
      oSB.Append("<Layout ID=\"" + this.ID + "\">");
      
      if(Panes != null && Panes.Count > 0)
      {
        oSB.Append(Panes.GetXml());
      }
      
      oSB.Append("</Layout>");

      return oSB.ToString();
    }

    internal void LoadXml(XmlNode oNode)
    {
      if(oNode.Attributes["ID"] != null)
      {
        this.ID = oNode.Attributes["ID"].Value;
      }

      if(oNode.ChildNodes.Count > 0)
      {
        this.Panes = new SplitterPaneGroup();
        this.Panes.LoadXml(oNode.FirstChild);
      }
    }

    #endregion
	}
}
