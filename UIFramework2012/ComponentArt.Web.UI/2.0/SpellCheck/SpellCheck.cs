using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using ComponentArt.Licensing.Providers;


namespace ComponentArt.Web.UI
{
  #region ISpellCheckCustomDictionaryManager

  public interface ISpellCheckCustomDictionaryManager
  {
    string [] GetCustomDictionary(HttpContext oContext);
    bool RemoveFromCustomDictionary(HttpContext oContext, string sWord);
    bool AddToCustomDictionary(HttpContext oContext, string sWord);
  }

  #endregion

  /// <summary>
  /// Provides spell-checking functionality.
  /// </summary>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:SpellCheck runat=server></{0}:SpellCheck>")]
  [ParseChildren(true)]
  [PersistChildren(false)]
  [Designer(typeof(ComponentArt.Web.UI.SpellCheckDesigner))]
  public sealed class SpellCheck : WebControl
  {
    #region Properties

    private SpellCheckClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public SpellCheckClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new SpellCheckClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// The ID of the context menu to use for suggestions.
    /// </summary>
    [DefaultValue("")]
    public string ContextMenuId
    {
      get
      {
        object o = ViewState["ContextMenuId"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ContextMenuId"] = value;
      }
    }

    /// <summary>
    /// The ID of the control to check.
    /// </summary>
    [DefaultValue("")]
    public string ControlToCheck
    {
      get
      {
        object o = ViewState["ControlToCheck"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ControlToCheck"] = value;
      }
    }

    /// <summary>
    /// The default culture (language) to use for spell checking.
    /// </summary>
    [DefaultValue("en-US")]
    public string DefaultLanguage
    {
      get
      {
        object o = ViewState["DefaultLanguage"];
        return o == null ? "en-US" : (string)o;
      }
      set
      {
        ViewState["DefaultLanguage"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for highlighting misspelled words inline.
    /// </summary>
    [DefaultValue("")]
    public string ErrorCssClass
    {
      get
      {
        object o = ViewState["ErrorCssClass"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ErrorCssClass"] = value;
      }
    }

    /// <summary>
    /// Whether to ignore email addresses when checking. Default: true.
    /// </summary>
    [DefaultValue(true)]
    public bool IgnoreEmailAddresses
    {
      get
      {
        object o = ViewState["IgnoreEmailAddresses"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["IgnoreEmailAddresses"] = value;
      }
    }

    /// <summary>
    /// Whether to ignore words with all capitals when checking. Default: true.
    /// </summary>
    [DefaultValue(true)]
    public bool IgnoreAcronyms
    {
      get
      {
        object o = ViewState["IgnoreAcronyms"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["IgnoreAcronyms"] = value;
      }
    }

    /// <summary>
    /// Whether to ignore words with non-letters in them when checking. Default: true.
    /// </summary>
    [DefaultValue(true)]
    public bool IgnoreNonAlphabetic
    {
      get
      {
        object o = ViewState["IgnoreNonAlphabetic"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["IgnoreNonAlphabetic"] = value;
      }
    }

    /// <summary>
    /// Whether to ignore URLs when checking. Default: true.
    /// </summary>
    [DefaultValue(true)]
    public bool IgnoreURLs
    {
      get
      {
        object o = ViewState["IgnoreURLs"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["IgnoreURLs"] = value;
      }
    }

    /// <summary>
    /// The maximum number of suggestions to return for each spelling error.
    /// </summary>
    [DefaultValue(10)]
    public int MaxSuggestions
    {
      get
      {
        object o = ViewState["MaxSuggestions"];
        return o == null ? 10 : (int)o;
      }
      set
      {
        ViewState["MaxSuggestions"] = value;
      }
    }
    /*
    /// <summary>
    /// Whether to display a dialog window when spell checking.
    /// </summary>
    [DefaultValue(true)]
    public bool ShowDialog
    {
      get
      {
        object o = ViewState["ShowDialog"];
        return o == null ? true : (bool)o;
      }
      set
      {
        ViewState["ShowDialog"] = value;
      }
    }
*/
    /// <summary>
    /// How hard the engine should work to find suggestions for misspelled words.
    /// </summary>
    [DefaultValue(SpellSuggestionDepthType.Medium)]
    public SpellSuggestionDepthType SpellSuggestionDepth
    {
      get
      {
        object o = ViewState["SpellSuggestionDepth"];
        return o == null ? SpellSuggestionDepthType.Medium : (SpellSuggestionDepthType)o;
      }
      set
      {
        ViewState["SpellSuggestionDepth"] = value;
      }
    }

    #endregion

    #region Protected Methods

    protected override void ComponentArtPreRender(EventArgs args)
    {
      /*
      if (this.ShowDialog && this.ContextMenuId == "")
      {
        this.BuildDialog();
      }*/
    }
     
    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      string sControlId = this.GetSaneId();

      // override writer
      output = new HtmlTextWriter(output, string.Empty);

      // write script
      if (Page != null)
      {
        if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
        {
          Page.RegisterClientScriptBlock("A573G988.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
        }
        if (!Page.IsClientScriptBlockRegistered("A573O912.js"))
        {
          Page.RegisterClientScriptBlock("A573O912.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.SpellCheck.client_scripts", "A573O912.js");
        }
      }

      // write markup
      AddAttributesToRender(output);
      // write content
      output.RenderBeginTag(HtmlTextWriterTag.Div);
      this.RenderContents(output);
      output.RenderEndTag();

      // write init
      StringBuilder oSB = new StringBuilder();
      oSB.Append("/*** ComponentArt.Web.UI.SpellCheck ").Append(this.VersionString()).Append(" ").Append(sControlId).Append(" ***/\n");

      oSB.Append("window.ComponentArt_Init_" + sControlId + " = function() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      oSB.Append("if(!window.ComponentArt_SpellCheck_Loaded)\n");
      oSB.Append("\t{setTimeout('ComponentArt_Init_" + sControlId + "()', 50); return; }\n\n");

      oSB.AppendFormat("window.{0} = new ComponentArt_SpellCheck('{0}');\n", sControlId);

      // Hook the actual ID if available and different from effective client ID
      if (this.ID != sControlId)
      {
        oSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sControlId + "; " + sControlId + ".GlobalAlias = '" + ID + "'; }\n");
      }

      // write properties
      oSB.Append(sControlId + ".CallbackUrl = '" + this.GetCallbackUrl() + "';\n");
      oSB.Append(sControlId + ".ClientEvents = " + Utils.ConvertClientEventsToJsObject(this._clientEvents) + ";\n");
      oSB.Append(sControlId + ".ControlToCheck = '" + this.GetClientId(this.ControlToCheck) + "';\n");
      oSB.Append(sControlId + ".ContextMenuId = '" + this.ContextMenuId + "';\n");
      oSB.Append(sControlId + ".DefaultLanguage = '" + this.DefaultLanguage + "';\n");
      oSB.Append(sControlId + ".ErrorCssClass = '" + this.ErrorCssClass + "';\n");
      if (IgnoreAcronyms) oSB.Append(sControlId + ".IgnoreAcronyms = 1;\n");
      if (IgnoreEmailAddresses) oSB.Append(sControlId + ".IgnoreEmailAddresses = 1;\n");
      if (IgnoreNonAlphabetic) oSB.Append(sControlId + ".IgnoreNonAlphabetic = 1;\n");
      if (IgnoreURLs) oSB.Append(sControlId + ".IgnoreURLs = 1;\n");
      oSB.Append(sControlId + ".MaxSuggestions = " + this.MaxSuggestions + ";\n");
      //if (ShowDialog) oSB.Append(sControlId + ".ShowDialog = true;\n");
      oSB.Append(sControlId + ".SpellSuggestionDepth = '" + this.SpellSuggestionDepth.ToString() + "';\n");
      oSB.Append(sControlId + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
      oSB.Append(sControlId + ".Initialize();\n");
      oSB.Append("\n}\n");

      // Initiate Callback creation
      oSB.Append("ComponentArt_Init_" + sControlId + "();\n");

      output.Write(this.DemarcateClientScript(oSB.ToString()));
    }

    protected override bool IsDownLevel()
    {
      if (this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }

      if (Context == null || Page == null)
      {
        return true;
      }

      string sUserAgent = Context.Request.UserAgent;

      if (sUserAgent == null || sUserAgent == "") return true;

      int iMajorVersion = 0;

      try
      {
        iMajorVersion = Context.Request.Browser.MajorVersion;
      }
      catch { }

      if (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion < 8)
      {
        return true;
      }
      else if ( // We are good if:

        // 1. We have IE 5 or greater on a non-Mac
        (sUserAgent.IndexOf("MSIE") >= 0 && iMajorVersion >= 5 && !Context.Request.Browser.Platform.ToUpper().StartsWith("MAC")) ||

        // 2. We have Gecko-based browser (Mozilla, Firefox, Netscape 6+)
        (sUserAgent.IndexOf("Gecko") >= 0) ||

        // 3. We have Opera 8 or later
        (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion >= 8)
        )
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

    }

    #endregion

    #region Private Methods

    /*
    private void BuildDialog()
    {
      string sId = GetSaneId();

      Table oTable = new Table();
      oTable.BorderWidth = 0;
      oTable.CellPadding = 5;
      oTable.CellSpacing = 5;
      
      TableCell oCell;

      oTable.Rows.Add(new TableRow());

      oCell = new TableCell();
      oCell.Width = 300;
      oCell.Style["font-family"] = "verdana";
      oCell.Style["font-size"] = "12px;";
      oCell.Text = "Not in dictionary:";
      oTable.Rows[0].Cells.Add(oCell);

      oCell = new TableCell();
      oCell.Width = 80;
      oTable.Rows[0].Cells.Add(oCell);
      
      oTable.Rows.Add(new TableRow());

      oCell = new TableCell();
      System.Web.UI.HtmlControls.HtmlGenericControl oTextArea = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
      oTextArea.ID = sId + "_Dialog_TextArea";
      oTextArea.Style["font-family"] = "verdana";
      oTextArea.Style["font-size"] = "12px";
      oTextArea.Style["height"] = "150px";
      oTextArea.Style["overflow"] = "auto";
      oTextArea.Style["background-color"] = "white";
      oTextArea.Style["border"] = "1px solid black";
      oCell.Controls.Add(oTextArea);
      oTable.Rows[1].Cells.Add(oCell);

      oCell = new TableCell();
      oCell.VerticalAlign = VerticalAlign.Top;
      System.Web.UI.HtmlControls.HtmlInputButton oIgnoreButton = new System.Web.UI.HtmlControls.HtmlInputButton();
      oIgnoreButton.Style["width"] = "100%";
      oIgnoreButton.Value = "Ignore";
      oIgnoreButton.Attributes["onclick"] = sId + ".DialogIgnore()"; 
      oCell.Controls.Add(oIgnoreButton);
      oCell.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("br"));
      System.Web.UI.HtmlControls.HtmlInputButton oIgnoreAllButton = new System.Web.UI.HtmlControls.HtmlInputButton();
      oIgnoreAllButton.Style["width"] = "100%";
      oIgnoreAllButton.Value = "Ignore All";
      oIgnoreAllButton.Attributes["onclick"] = sId + ".DialogIgnoreAll()";
      oCell.Controls.Add(oIgnoreAllButton);
      oCell.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("br"));
      oTable.Rows[1].Cells.Add(oCell);

      oTable.Rows.Add(new TableRow());

      oCell = new TableCell();
      oCell.Style["font-family"] = "verdana";
      oCell.Style["font-size"] = "12px;";
      oCell.Controls.Add(new LiteralControl("Change to:"));
      oCell.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("br"));
      System.Web.UI.HtmlControls.HtmlInputText oChangeTo = new System.Web.UI.HtmlControls.HtmlInputText();
      oChangeTo.Style["width"] = "100%";
      oChangeTo.ID = sId + "_Dialog_Replacement";
      oCell.Controls.Add(oChangeTo);
      oTable.Rows[2].Cells.Add(oCell);

      oCell = new TableCell();
      oCell.VerticalAlign = VerticalAlign.Top;
      System.Web.UI.HtmlControls.HtmlInputButton oChangeButton = new System.Web.UI.HtmlControls.HtmlInputButton();
      oChangeButton.Style["width"] = "100%";
      oChangeButton.Value = "Change";
      oChangeButton.Attributes["onclick"] = sId + ".DialogChange()";
      oCell.Controls.Add(oChangeButton);
      oCell.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("br"));
      System.Web.UI.HtmlControls.HtmlInputButton oChangeAllButton = new System.Web.UI.HtmlControls.HtmlInputButton();
      oChangeAllButton.Style["width"] = "100%";
      oChangeAllButton.Value = "Change All";
      oChangeAllButton.Attributes["onclick"] = sId + ".DialogChangeAll()";
      oCell.Controls.Add(oChangeAllButton);
      oCell.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("br"));
      oTable.Rows[2].Cells.Add(oCell);

      oTable.Rows.Add(new TableRow());

      oCell = new TableCell();
      oCell.Style["font-family"] = "verdana";
      oCell.Style["font-size"] = "12px;";
      oCell.Controls.Add(new LiteralControl("Suggestions:"));
      oCell.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("br"));
      System.Web.UI.HtmlControls.HtmlSelect oSuggestions = new System.Web.UI.HtmlControls.HtmlSelect();
      oSuggestions.Attributes["onchange"] = string.Format("{0}.DialogSuggestionChange('{0}_Dialog_Suggestions','{0}_Dialog_Replacement')", sId);
      oSuggestions.Style["width"] = "100%";
      oSuggestions.Size = 6;
      oSuggestions.ID = sId + "_Dialog_Suggestions";
      oCell.Controls.Add(oSuggestions);
      oTable.Rows[3].Cells.Add(oCell);

      oTable.Rows[3].Cells.Add(new TableCell());

      oTable.Rows.Add(new TableRow());

      oTable.Rows[4].Cells.Add(new TableCell());

      oCell = new TableCell();
      System.Web.UI.HtmlControls.HtmlInputButton oOKButton = new System.Web.UI.HtmlControls.HtmlInputButton();
      oOKButton.Attributes["value"] = "OK";
      oOKButton.Attributes["onclick"] = sId + "_Dialog.Close()";
      oCell.Controls.Add(oOKButton);

      oTable.Rows[4].Cells.Add(oCell);

      Dialog oDialog = new Dialog();
      oDialog.ID = sId + "_Dialog";
      oDialog.Header = new DialogContent(); 
      oDialog.Header.Controls.Add(new LiteralControl("Spell Check"));
      oDialog.Content = new DialogContent();
      oDialog.Content.Controls.Add(oTable);
      
      this.Controls.Add(oDialog);
    }
    */
    private string GetCallbackUrl()
    {
      string sUrl = Context.Request.Url.AbsoluteUri;
      return sUrl.Substring(0, sUrl.LastIndexOf('/')) + "/ComponentArtSpellCheckHandler.axd";
    }

    private string GetClientId(string sId)
    {
      if (Page != null)
      {
        Control oControl = Page.FindControl(sId);

        if (oControl != null)
        {
          return oControl.ClientID;
        }
      }

      return sId;
    }

    #endregion
  }

  public enum SpellSuggestionDepthType
  {
    Low,
    Medium,
    High
  }
}
