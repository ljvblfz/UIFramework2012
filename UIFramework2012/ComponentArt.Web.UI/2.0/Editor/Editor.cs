using System;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace ComponentArt.Web.UI
{
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:Editor runat=server></{0}:Editor>")]
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [Designer(typeof(ComponentArt.Web.UI.EditorDesigner))]
  [ParseChildren(true)]
  [PersistChildren(true)]
  public sealed class Editor : WebControl, INamingContainer
  {
    public Editor() : base()
    {

    }

    /// <summary>
    /// Naming container used for housing templated contents of the <see cref="Editor"/> control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The following are instantiated in <b>EditorTemplateContainer</b> instances: 
    /// <see cref="Editor.Template"/>
    /// </para> 
    /// <para>
    /// Not intended for direct use by developers.
    /// </para>
    /// </remarks>
    [ToolboxItem(false)]
    public class EditorTemplateContainer : Control, INamingContainer
    {
      private Editor parent;
      public EditorTemplateContainer(Editor parent)
      {
        this.parent = parent;
      }
    }

    /// <summary>
    /// Class for content sections of the <see cref="Dialog"/> control.
    /// </summary>
    /// <remarks>
    /// Following <b>EditorTemplate</b> instances are implemented by the <b>Editor</b> control:
    /// </remarks>
    [ToolboxItem(false)]
    public class EditorTemplate : Control
    {

    }

    #region Private Properties

    private EditorToolBarCollection _toolbars = new EditorToolBarCollection();
    private EditorTemplate e_Template;

    #endregion

    #region Public Properties

    /// <summary>
    /// Whether we are currently in a callback request that this control caused. Read-only.
    /// </summary>
    [Description("Whether we are currently in a callback request that this control caused.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public bool CausedCallback
    {
      get
      {
        if (Context != null && Context.Request != null)
        {
          if (Context.Request.Params[string.Format("Cart_{0}_Callback", this.GetSaneId())] != null)
          {
            return true;
          }
        }

        return false;
      }
    }

    /// <summary>
    /// The initial HTML content for the Editor.
    /// </summary>
    [Category("Content")]
    [DefaultValue("")]
    public string ContentHTML
    {
      get
      {
        object o = ViewState["ContentHTML"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["ContentHTML"] = value;
      }
    }

    /// <summary>
    /// The initial text content for the Editor.
    /// </summary>
    [Category("Content")]
    [DefaultValue("")]
    public string ContentText
    {
      get
      {
        object o = ViewState["ContentText"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["ContentText"] = value;
      }
    }

    /// <summary>
    /// The root path for user uploads / browsing.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue("")]
    public string UploadPath
    {
        get
        {
            object o = ViewState["UploadPath"];
            return (o == null) ? String.Empty : (string)o;
        }
        set
        {
            ViewState["UploadPath"] = value;
            HttpContext.Current.Session["EditorUploadPath"] = value;
        }
    }

    /// <summary>
    /// The temp directory for user uploads / browsing.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue("")]
    public string UploadTempPath
    {
        get
        {
            object o = ViewState["UploadTempPath"];
            return (o == null) ? String.Empty : (string)o;
        }
        set
        {
            ViewState["UploadTempPath"] = value;
            HttpContext.Current.Session["EditorUploadTempPath"] = value;
        }
    }

    /// <summary>
    /// The allowed image extensions for image uploading.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue("jpg, jpeg, gif, png")]
      public string ImageFileFilter 
    {
        get
        {
            object o = ViewState["ImageFileFilter"];
            if (o == null) HttpContext.Current.Session["EditorImageFileFilter"] = "jpg, jpeg, gif, png";
            return (o == null) ? "jpg, jpeg, gif, png" : (string)o;
        }
        set
        {
            ViewState["ImageFileFilter"] = value;
            HttpContext.Current.Session["EditorImageFileFilter"] = value;
        }
    }

    /// <summary>
    /// The css class to apply to highlighted elements in the editor text area.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string HighlightElementCssClass
    {
      get
      {
        object o = ViewState["HighlightElementCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["HighlightElementCssClass"] = value;
      }
    }

    /// <summary>
    /// Relative or absolute path to the folder containing the skin files for the Editor.
    /// </summary>
    [Description("Relative or absolute path to the folder containing the skin files for the Editor.")]
    [Category("Style")]
    [DefaultValue("")]
    public string SkinFolderLocation
    {
      get
      {
        object o = ViewState["SkinFolderLocation"];
        return (o == null) ? String.Empty : Utils.ConvertUrl(Context, string.Empty, (string)o);
      }

      set
      {
        ViewState["SkinFolderLocation"] = value;
      }
    }

    /// <summary>
    /// The css file for the source view.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string SourceCssClass
    {
      get
      {
        object o = ViewState["SourceCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["SourceCssClass"] = value;
      }
    }

    /// <summary>
    /// The css file for the design view.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string DesignCssClass
    {
      get
      {
        object o = ViewState["DesignCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["DesignCssClass"] = value;
      }
    }

    /// <summary>
    /// The css file for the Editor.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string CssFileURL
    {
      get
      {
        object o = ViewState["CssFileURL"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["CssFileURL"] = value;
      }
    }

    /// <summary>
    /// Clone styles from parent page.
    /// </summary>
    [Category("Behavior")]
    [Description("Clone styles from parent page.")]
    [DefaultValue("true")]
    public bool CloneStyles
    {
        get
        {
            object o = ViewState["CloneStyles"];
            return (o == null) ? true : Convert.ToBoolean(o);
        }
        set
        {
            ViewState["CloneStyles"] = value;
        }
    }

    /// <summary>
    /// The frameBorder property to apply to the ContentUrl IFrame. Use "0" for none.
    /// </summary>
    [Category("Style")]
    [DefaultValue("0")]
    public string IFrameBorder
    {
      get
      {
        object o = ViewState["IFrameBorder"];
        return (o == null) ? "0" : (string)o;
      }
      set
      {
        ViewState["IFrameBorder"] = value;
      }
    }

    /// <summary>
    /// The scrolling property to apply to the ContentUrl IFrame. Use "no" for none.
    /// </summary>
    [Category("Style")]
    [DefaultValue("auto")]
    public string IFrameScrolling
    {
      get
      {
        object o = ViewState["IFrameScrolling"];
        return (o == null) ? "auto" : (string)o;
      }
      set
      {
        ViewState["IFrameScrolling"] = value;
      }
    }

    /// <summary>
    /// The maximum number of Breadcrumbs to display. Default 8.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(8)]
    public int MaxBreadcrumbs
    {
        get
        {
            object o = ViewState["MaxBreadcrumbs"];
            return (o == null) ? 8 : (int)o;
        }
        set
        {
            ViewState["MaxBreadcrumbs"] = value;
        }
    }

    /// <summary>
    /// The separator to use between Breadcrumbs.
    /// </summary>
    [Category("Appearance")]
    [Description("The separator to use between Breadcrumbs.")]
    [DefaultValue(">")]
    public string BreadcrumbSeparator
    {
      get
      {
        object o = ViewState["BreadcrumbSeparator"];
        return (o == null) ? ">" : (string)o;
      }
      set
      {
        ViewState["BreadcrumbSeparator"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for Breadcrumbs.
    /// </summary>
    [Category("Appearance")]
    [Description("The CSS class to use for Breadcrumbs")]
    [DefaultValue("")]
    public string BreadcrumbCssClass
    {
      get
      {
        object o = ViewState["BreadcrumbCssClass"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["BreadcrumbCssClass"] = value;
      }
    }

    /// <summary>
    /// Toggle debug mode.
    /// </summary>
    [Category("Behavior")]
    [Description("Toggle debug mode.")]
    [DefaultValue("false")]
    public bool Debug
    {
      get
      {
        object o = ViewState["Debug"];
        return (o == null) ? false : Convert.ToBoolean(o);
      }
      set
      {
        ViewState["Debug"] = value;
      }
    }

    /// <summary>
    /// Toggle the editor's visibility manager to remove/add the editor based on parent element's visible/display state
    /// </summary>
    [Category("Behavior")]
    [Description("Toggle the editor's visibility manager.")]
    [DefaultValue("false")]
    public bool EnableVisibilityManager
    {
        get
        {
            object o = ViewState["EnableVisibilityManager"];
            return (o == null) ? false : Convert.ToBoolean(o);
        }
        set
        {
            ViewState["EnableVisibilityManager"] = value;
        }
    }

    /// <summary>
    /// Whether to automatically focus the editor or not.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to automatically focus the editor or not.")]
    [DefaultValue("true")]
    public bool FocusOnLoad
    {
      get
      {
        object o = ViewState["FocusOnLoad"];
        return (o == null) ? true : Convert.ToBoolean(o);
      }
      set
      {
        ViewState["FocusOnLoad"] = value;
      }
    }

    /// <summary>
    /// Preserve relative paths
    /// </summary>
    [Category("Behavior")]
    [Description("Preserve relative paths.")]
    [DefaultValue("true")]
    public bool PreserveRelativePaths
    {
      get
      {
        object o = ViewState["PreserveRelativePaths"];
        return (o == null) ? true : Convert.ToBoolean(o);
      }
      set
      {
        ViewState["PreserveRelativePaths"] = value;
      }
    }

    /// <summary>
    /// Editor running mode.
    /// </summary>
    [Category("Behavior")]
    [Description("Editor running mode.")]
    [DefaultValue(EditModeType.Default)]
    public EditModeType EditMode
    {
      get
      {
        object o = ViewState["EditMode"];
        return (o == null) ? EditModeType.Default : (EditModeType)o;
      }
      set
      {
        ViewState["EditMode"] = value;
      }
    }

    /// <summary>
    /// Editor engine mode. *Default EditEngineType.Browser, Web.UI 2009.3
    /// </summary>
    [Category("Behavior")]
    [Description("Editor edit engine mode.")]
    [DefaultValue(EditEngineType.Browser)]
    public EditEngineType EditEngine
    {
      get
      {
        object o = ViewState["EditEngine"];
        return (o == null) ? EditEngineType.Browser : (EditEngineType)o;
      }
      set
      {
        ViewState["EditEngine"] = value;
      }
    }

    private EditorStyleCollection _styles = new EditorStyleCollection();
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public EditorStyleCollection Styles
    {
      get
      {
        return this._styles;
      }
      set
      {
        this._styles = value;
      }
    }

    /// <summary>
    /// Editor Template container.  Specifies the layout of the Editor instance.
    /// </summary>
    /// <seealso cref="TemplateFile"/>
    [Browsable(false)]
    [DefaultValue(null)]
    [Description("The template container.")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [NotifyParentProperty(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public EditorTemplate Template
    {
      get
      {
        return e_Template;
      }
      set
      {
        e_Template = value;
      }
    }

    /// <summary>
    /// Name of the file containing the editor layout <see cref="Template"/>.  
    /// </summary>
    /// <remarks>
    /// This property is only taken into account if <see cref="Template"/> is not set.
    /// </remarks>
    /// <remarks>
    /// The file is expected to be located within "templates" subfolder of the <see cref="SkinFolderLocation">skin folder</see>.
    /// </remarks>
    /// <seealso cref="Template" /><seealso cref="SkinFolderLocation" />
    [DefaultValue(null)]
    [Description("Path to the the file containing the editor layout Template.")]
    public string TemplateFile
    {
      get
      {
        object o = ViewState["TemplateFile"];
        return (o == null) ? "" : (string)o;
      }
      set
      {
        ViewState["TemplateFile"] = value;
      }
    }

    /// <summary>
    /// The Id of the Client Template to apply to Editor Breadcrumbs.
    /// </summary>
    [Category("Templates")]
    [DefaultValue(null)]
    public string BreadcrumbClientTemplateId
    {
      get
      {
        object o = ViewState["BreadcrumbClientTemplateId"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["BreadcrumbClientTemplateId"] = value;
      }
    }


    internal ClientTemplateCollection _clientTemplates = new ClientTemplateCollection();
    
    /// <summary>
    /// Collection of client-templates that may be used by this control.
    /// </summary>
    [Browsable(false)]
    [Description("Collection of client-templates that may be used by this control.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplateCollection ClientTemplates
    {
      get
      {
        return _clientTemplates;
      }
    }

    private EditorClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public EditorClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new EditorClientEvents();
        }
        return _clientEvents;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public EditorToolBarCollection ToolBars
    {
      get
      {
        return this._toolbars;
      }
    }

    #endregion

    #region Protected Methods

    internal void RenderDesignTime(HtmlTextWriter output)
    {
      // TODO: optionally add some columns and data

      DownLevelRender(output);
    }

    internal bool UseModernSkins()
    {
      return this.SkinFolderLocation != null && this.SkinFolderLocation != String.Empty;
    }

    private void DownLevelRender(HtmlTextWriter output)
    {
      output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
      output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
      output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
      output.AddAttribute(HtmlTextWriterAttribute.Border, "1");
      output.RenderBeginTag(HtmlTextWriterTag.Textarea); // <table>
      output.RenderEndTag(); // </table>
    }

    protected override void RenderContents(HtmlTextWriter output)
    {
        foreach (Control oControl in Controls)
        {
            oControl.RenderControl(output);
        }
    }

    protected override void LoadViewState(object state)
    {
      base.LoadViewState(state);

      string sEditorVarName = this.GetSaneId();

      // Load client data
      string content = Context.Request.Form[sEditorVarName + "_Content"];
      if(content != null)
      {
        this.ContentHTML = HttpUtility.UrlDecode(content);
      }

      string text = Context.Request.Form[sEditorVarName + "_Text"];
      if (text != null)
      {
        this.ContentText = HttpUtility.UrlDecode(text);
      }
    }

    protected override object SaveViewState()
    {
      if (EnableViewState)
      {
        ViewState["EnableViewState"] = true;
      }

      return base.SaveViewState();
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      if (!this.Debug)
      {
        Utils._BrowserCapabilities bc = Utils.BrowserCapabilities(Context.Request);
        if (bc.IsBrowserOpera)
        {
          output = new HtmlTextWriter(output, string.Empty);
          output.Write("<div style=\"background-color:#3F3F3F;border:1px;border-style:solid;border-bottom-color:black;border-right-color:black;border-left-color:lightslategray;border-top-color:lightslategray;color:cornsilk;padding:2px;font-family:verdana;font-size:11px;\">");
          output.Write("<b>ComponentArt Editor</b>");
          output.Write("<br><br>");
          output.Write("Sorry, Opera is not supported in this release.");
          output.Write("</div>");
          return;
        }
      }

      output = new HtmlTextWriter(output, string.Empty);

      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        RenderAccessible(output);
        return;
      }

      if (this.IsDownLevel())
      {
        RenderDownLevel(output);
        return;
      }

      if (Page != null)
      {
      // Add core code
      if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
      {
        Page.RegisterClientScriptBlock("A573G988.js", "");
        WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
      }
      if (!Page.IsClientScriptBlockRegistered("A573Z388.js"))
      {
        Page.RegisterClientScriptBlock("A573Z388.js", "");
        WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573Z388.js");
      }
      if (!Page.IsClientScriptBlockRegistered("A572GI44.js"))
      {
        Page.RegisterClientScriptBlock("A572GI44.js", "");
        WriteGlobalClientScript(output, "ComponentArt.Web.UI.Editor.client_scripts", "A572GI44.js");
      }
      if (!Page.IsClientScriptBlockRegistered("A572GI43.js"))
      {
        Page.RegisterClientScriptBlock("A572GI43.js", "");
        WriteGlobalClientScript(output, "ComponentArt.Web.UI.Editor.client_scripts", "A572GI43.js");
      }
      if (!Page.IsClientScriptBlockRegistered("A572GI45.js"))
      {
        Page.RegisterClientScriptBlock("A572GI45.js", "");
        WriteGlobalClientScript(output, "ComponentArt.Web.UI.Editor.client_scripts", "A572GI45.js");
      }
      if (this.UseModernSkins())
      {
        if (!Page.IsClientScriptBlockRegistered("A572GI46.js"))
        {
          Page.RegisterClientScriptBlock("A572GI46.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.Editor.client_scripts", "A572GI46.js");
        }
      }

        }

        string sEditorVarName = this.GetSaneId();

        // Render data hidden fields
        output.AddAttribute("id", sEditorVarName + "_Content");
        output.AddAttribute("name", sEditorVarName + "_Content");
        output.AddAttribute("value", string.Empty);
        output.AddAttribute("type", "hidden");
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();

        output.AddAttribute("id", sEditorVarName + "_Text");
        output.AddAttribute("name", sEditorVarName + "_Text");
        output.AddAttribute("value", string.Empty);
        output.AddAttribute("type", "hidden");
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();

        // Content Output 
        RenderContents(output);  

        // Render client-side object initiation.
        StringBuilder oStartupSB = new StringBuilder();

        oStartupSB.Append("/*** ComponentArt.Web.UI.Editor ").Append(this.VersionString()).Append(" ").Append(sEditorVarName).Append(" ***/\n");

        oStartupSB.Append("function ComponentArt_Init_" + sEditorVarName + "() {\n");

        // Include check for whether everything we need is loaded,
        // and a retry after a delay in case it isn't.
        oStartupSB.Append("if(!window.ComponentArt_Editor_Kernel_Loaded)\n");
        oStartupSB.Append("\t{setTimeout('ComponentArt_Init_" + sEditorVarName + "()', 100); return; }\n\n");

        // Instantiate object
        oStartupSB.Append("window." + sEditorVarName + " = new ComponentArt_Editor('" + sEditorVarName + "');\n");

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != sEditorVarName)
        {
            oStartupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sEditorVarName + "; }\n");
        }

        // Write postback function reference
        if (Page != null)
        {
          oStartupSB.Append(sEditorVarName + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
        }

        oStartupSB.Append(sEditorVarName + ".ControlId = '" + this.UniqueID + "';\n");

        // Toolbars
        string toolbarArray = "";
        foreach (ToolBar editorToolbar in this.ToolBars)
        {
          toolbarArray += ",['" + editorToolbar.ID + "']";
        }

        oStartupSB.Append("var properties = [\n");
        oStartupSB.Append("['Id','" + sEditorVarName + "'],");
        oStartupSB.Append("['Mode','design'],");
        if (EnableViewState) oStartupSB.Append("['EnableViewState',1],");
        oStartupSB.Append("['ClientTemplates'," + this._clientTemplates.ToString() + "],");
        oStartupSB.Append("['ControlReferences'," + "{" + String.Join(",", (string[])(this._controlReferences.ToArray(typeof(string)))) + "}" + "],");
        oStartupSB.Append("['BreadcrumbClientTemplateId','" + this.BreadcrumbClientTemplateId + "'],");
        oStartupSB.Append("['MaxBreadcrumbs'," + this.MaxBreadcrumbs + "],");
        oStartupSB.Append("['Width','" + this.Width.ToString() + "'],");
        oStartupSB.Append("['Height','" + this.Height.ToString() + "'],");
        oStartupSB.Append("['BreadcrumbCssClass','" + this.BreadcrumbCssClass + "'],");
        oStartupSB.Append("['BreadcrumbSeparator'," + Utils.ConvertStringToJSString(this.BreadcrumbSeparator) + "],");
        oStartupSB.Append("['SkinFolderLocation'," + Utils.ConvertStringToJSString(this.SkinFolderLocation) + "],");
        oStartupSB.Append("['SourceCssClass','" + this.SourceCssClass + "'],");
        oStartupSB.Append("['DesignCssClass','" + this.DesignCssClass + "'],");
        oStartupSB.Append("['HighlightElementCssClass','" + this.HighlightElementCssClass + "'],");
        oStartupSB.Append("['EditMode','" + this.EditMode.ToString() + "'],");
        oStartupSB.Append("['EditEngine','" + this.EditEngine.ToString() + "'],");
        oStartupSB.Append("['IFrameBorder','" + this.IFrameBorder.ToString() + "'],");
        oStartupSB.Append("['IFrameScrolling','" + this.IFrameScrolling.ToString() + "'],");
        oStartupSB.Append("['Debug'," + this.Debug.ToString().ToLower() + "],");
        oStartupSB.Append("['CloneStyles'," + this.CloneStyles.ToString().ToLower() + "],");
        oStartupSB.Append("['EnableVisibilityManager'," + this.EnableVisibilityManager.ToString().ToLower() + "],");
        oStartupSB.Append("['PreserveRelativePaths'," + this.PreserveRelativePaths.ToString().ToLower() + "],");
        oStartupSB.Append("['FocusOnLoad'," + this.FocusOnLoad.ToString().ToLower() + "],");
        
        if (this.CssFileURL != "") oStartupSB.Append("['CssFileURL'," + Utils.ConvertStringToJSString(this.CssFileURL) + "],");
        if (this.ContentHTML != "") oStartupSB.Append("['ContentHTML'," + Utils.ConvertStringToJSString(this.ContentHTML) + "],");
        if (this.ContentText != "") oStartupSB.Append("['ContentText'," + Utils.ConvertStringToJSString(this.ContentText) + "],");
        oStartupSB.Append("['StyleStorage'," + this.Styles.ToString() + "],");
        if (toolbarArray.Length > 0)
          oStartupSB.Append("['ToolBars',[" + toolbarArray.Substring(1) + "]],");

        string TemplateString = String.Empty;

        if (this.Template != null)
        {
          StringBuilder sb = new StringBuilder();
          StringWriter tw = new StringWriter(sb);
          HtmlTextWriter hw = new HtmlTextWriter(tw);
          this.Template.RenderControl(hw);
          TemplateString = sb.ToString();
        }
        
        if(this.Template == null && this.TemplateFile == null || (TemplateString.ToLower().IndexOf("$$editorarea$$") == -1 && this.EditMode == EditModeType.Default))       
            TemplateString = "<div style=\"height:" + this.Height.ToString() + ";width:" + this.Width.ToString() + ";\">$$editorarea$$</div><!-- default edit template -->";

        oStartupSB.Append("['Template'," + Utils.ConvertStringToJSString(TemplateString) + "],");

        oStartupSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "]");
        oStartupSB.Append("];\n");

        // KeyEvents
        oStartupSB.Append(sEditorVarName + ".keyDownEvent = function(event){ " + sEditorVarName + ".KeyDown(event);}\n");
        oStartupSB.Append(sEditorVarName + ".keyUpEvent = function(event){ " + sEditorVarName + ".KeyUp(event);}\n");
        oStartupSB.Append(sEditorVarName + ".keyPressEvent = function(event){ " + sEditorVarName + ".KeyPress(event);}\n");

        // MouseEvents
        oStartupSB.Append(sEditorVarName + ".mouseDownEvent = function(event){ " + sEditorVarName + ".MouseDown(event);}\n");
        oStartupSB.Append(sEditorVarName + ".mouseUpEvent = function(event){ " + sEditorVarName + ".MouseUp(event);}\n");
        oStartupSB.Append(sEditorVarName + ".mouseClickEvent = function(event){ " + sEditorVarName + ".MouseClick(event);}\n");
        oStartupSB.Append(sEditorVarName + ".mouseMoveEvent = function(event){ " + sEditorVarName + ".MouseMove(event);}\n");

        oStartupSB.Append(sEditorVarName + ".blurEvent = function(event){ " + sEditorVarName + ".Blur(event);}\n");

        // Set properties
        oStartupSB.AppendFormat("ComponentArt_SetProperties({0}, properties);\n", sEditorVarName);

        //oStartupSB.Append("ComponentArt_HookEvents(" + sEditorVarName + ");\n");

        oStartupSB.Append(sEditorVarName + ".Render();\n");

        oStartupSB.Append(sEditorVarName + ".Initialize();\n");

        oStartupSB.Append("\n}");

        // Initiate Editor creation
        oStartupSB.Append("ComponentArt_Init_" + sEditorVarName + "();");

        WriteStartupScript(output, this.DemarcateClientScript(oStartupSB.ToString()));
    }

    private void RenderAccessible(HtmlTextWriter output)
    {
      output.Write("<textarea id=\"");
      output.Write(this.ID);
      output.Write("\">");
      output.Write(HttpUtility.HtmlEncode(this.ContentHTML));
      output.Write("</textarea>");
    }

    private void RenderDownLevel(HtmlTextWriter output)
    {
        // Sanify writer
        output = new HtmlTextWriter(output);

        // Temporary Output
        output.Write("Editor Downlevel Output");
    }

    protected override bool IsDownLevel()
    {
        if (this.ClientTarget != ClientTargetLevel.Auto)
        {
            return this.ClientTarget == ClientTargetLevel.Downlevel;
        }

        if (Context == null)
        {
            return true;
        }

        string sUserAgent = Context.Request.UserAgent;

        if (sUserAgent == null)
        {
            return true;
        }

        int iMajorVersion = 0;

        try
        {
            iMajorVersion = Context.Request.Browser.MajorVersion;
        }
        catch { }

        if ( // We are good if:

            // 0. We have the W3C Validator
            (sUserAgent.IndexOf("Validator") >= 0) ||

            // 1. We have IE 5 or greater on a non-Mac
            (sUserAgent.IndexOf("MSIE") >= 0 && iMajorVersion >= 5) ||

            // 2. We have Gecko-based browser (Netscape 6+, Mozilla, FireFox)
            (sUserAgent.IndexOf("Gecko") >= 0) ||

            // 3. We have Opera 7 or later
            (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion >= 7) ||

            // 4. We have Safari
            (sUserAgent.IndexOf("Safari") >= 0) ||

            // 5. We have Konqueror
            (sUserAgent.IndexOf("Konqueror") >= 0)
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
              
        // Is this a callback? Handle it now
        if (this.CausedCallback)
        {
          this.HandleCallback();
          return;
        }

      }

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      this.Page.Load += new EventHandler(this.ParentPage_Load);
    }

    protected void ParentPage_Load(object sender, EventArgs e)
    {
      if (this.EditMode == EditModeType.Inline)
      {
        string sEditorVarName = this.GetSaneId();

        // main iframe
        System.Web.UI.WebControls.WebControl editorIFrame = new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Iframe);
        editorIFrame.Attributes.Add("frameborder", this.IFrameBorder);
        editorIFrame.Attributes.Add("scrolling", this.IFrameScrolling);
        editorIFrame.Attributes.Add("id", sEditorVarName + "_IFrame");
        editorIFrame.Attributes.Add("name", sEditorVarName + "_IFrame");
        editorIFrame.Style.Add("display", "none");
        editorIFrame.Style.Add("position", "absolute");
        editorIFrame.Style.Add("z-index", "500");

        if (Context.Request.UserAgent.IndexOf("MSIE") == -1)
          editorIFrame.Attributes.Add("src", "javascript:void(0);");

        // textarea
        System.Web.UI.WebControls.WebControl editorTextarea = new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Textarea);
        editorTextarea.Style.Add("display", "none");
        editorTextarea.Style.Add("position", "absolute");
        editorTextarea.Style.Add("z-index", "450");
        editorTextarea.Attributes.Add("id", sEditorVarName + "_Textarea");
        editorTextarea.Attributes.Add("name", sEditorVarName + "_Textarea");

        editorIFrame.Height = Unit.Percentage(100);
        editorIFrame.Width = Unit.Percentage(100);

        foreach (Control control in Page.Controls)
        {
          // Search for the Form to add editorIFrame and editorTextarea to it.
          // If we cared only about ASP.NET 2.0 we could just call Page.Form
          if (control is HtmlForm)
          {
            HtmlForm form = control as HtmlForm;
            form.Controls.AddAt(0, editorTextarea);
            form.Controls.AddAt(0, editorIFrame);
            break;
          }
        }
      }
    }

    protected override void CreateChildControls()
    {
      // cssfile
      if (this.CssFileURL != String.Empty && CloneStyles)
        Controls.Add(new LiteralControl("<link rel='stylesheet' type='text/css' href='" + this.CssFileURL + "' />"));

      // place holder
      System.Web.UI.WebControls.WebControl placeHolder = new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);

      string sEditorVarName = this.GetSaneId();

      placeHolder.Attributes.Add("id", sEditorVarName + "_Placeholder");
      placeHolder.Attributes.Add("name", sEditorVarName + "_Placeholder");
      placeHolder.Height = this.Height;
      placeHolder.Width = this.Width;
      Controls.Add(placeHolder);

      foreach (ToolBar childToolbar in this.ToolBars)
      {
        Controls.Add(childToolbar);
            childToolbar.ClientRenderCondition = "window." + sEditorVarName + " && " + sEditorVarName + ".Initialized";
      }

      EnsureChildControls();
      CatalogChildControls();

      if (this.EditMode == EditModeType.Default)
      {
        /* main iframe
         * handled client-side
        System.Web.UI.WebControls.WebControl editorIFrame = new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Iframe);
        editorIFrame.Attributes.Add("frameborder", this.IFrameBorder);
        editorIFrame.Attributes.Add("scrolling", this.IFrameScrolling);
        editorIFrame.Attributes.Add("id", sEditorVarName + "_IFrame");
        editorIFrame.Attributes.Add("name", sEditorVarName + "_IFrame");
 
        editorIFrame.Style.Add("position", "absolute");
        editorIFrame.Style.Add("top", "-10000px");
        editorIFrame.Style.Add("left", "-10000px");
        editorIFrame.Style.Add("z-index", "500");

        if(Context.Request.UserAgent.IndexOf("MSIE") == -1) 
          editorIFrame.Attributes.Add("src", "javascript:void(0);");

        // textarea
        System.Web.UI.WebControls.WebControl editorTextarea = new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Textarea);
        editorTextarea.Attributes.Add("style", "visibility:hidden;overflow:auto;");
        editorTextarea.Attributes.Add("id", sEditorVarName + "_Textarea");
        editorTextarea.Attributes.Add("name", sEditorVarName + "_Textarea");
              
        editorIFrame.Height = Unit.Percentage(100);
        editorIFrame.Width = Unit.Percentage(100);

        Controls.Add(editorTextarea);
        Controls.Add(editorIFrame);*/
      }
      
      // bread crumbs
      if (this.Template == null && this.TemplateFile != null && this.TemplateFile != String.Empty)
      {
        UserControl templateAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "/templates/" + this.TemplateFile);
        templateAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
        this.Template = new EditorTemplate();
        this.Template.Controls.Add(templateAscx);
      }

      if (this.Template != null)
      {
        StringBuilder sb = new StringBuilder();
        StringWriter tw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        this.Template.RenderControl(hw);
        if (sb.ToString().ToUpper().IndexOf("$$BREADCRUMBS$$") > -1)
        {
          System.Web.UI.WebControls.WebControl editorTracer = new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
          editorTracer.Attributes.Add("id", sEditorVarName + "_Breadcrumbs");
          editorTracer.Attributes.Add("name", sEditorVarName + "_Breadcrumbs");
          Controls.Add(editorTracer);
        }
      }

    }

    private JavaScriptArray _controlReferences;
    private Hashtable _referencedControls;

    private class ControlReference
    {
      public Control Control;
      public string ParentToolBarId;
      public int ParentItemIndex;
      public ControlReference(Control control, string parentToolBarId, int parentItemIndex)
      {
        this.Control = control;
        this.ParentToolBarId = parentToolBarId;
        this.ParentItemIndex = parentItemIndex;
      }
    }

    private void CatalogChildControls()
    {
      this._referencedControls = new Hashtable();
      foreach (ToolBar childToolbar in this.ToolBars)
      {
        for (int i = 0; i < childToolbar.Items.Count; i++)
        {
          ToolBarItem item = childToolbar.Items[i];
          if (this.UseModernSkins())
          {
            switch (Utils.ParseEditorCommandType(item.Attributes["EditorCommandType"], EditorCommandType.Custom))
            {
              case EditorCommandType.EditFind:
              case EditorCommandType.EditReplace:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\FindReplace.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  Dialog childDialog = (Dialog)Utils.FindControl(childAscx, "FindDialog");
                  item.ClientSideCommand = childDialog.GetSaneId() + ".show()";
                  this.FindReferencedControl(childToolbar, "FindDialog", i);
                }
                break;
              case EditorCommandType.EditUndoExtended:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\Undo.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  item.ItemType = ToolBarItemType.SplitDropDown;
                  item.DropDownId = "UndoMenu";
                  this.FindReferencedControl(childToolbar, "UndoMenu", i);
                }
                break;
              case EditorCommandType.EditRedoExtended:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\Redo.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  item.ItemType = ToolBarItemType.SplitDropDown;
                  item.DropDownId = "RedoMenu";
                  this.FindReferencedControl(childToolbar, "RedoMenu", i);
                }
                break;
              case EditorCommandType.FormatBold:
              case EditorCommandType.FormatItalic:
              case EditorCommandType.FormatUnderline:
              case EditorCommandType.FormatStrikethrough:
                {
                  item.ItemType = ToolBarItemType.ToggleCheck;
                }
                break;
              case EditorCommandType.FormatBulletsExtended:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\FormatBullets.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  item.ItemType = ToolBarItemType.SplitDropDown;
                  item.DropDownId = "BulletsMenu";
                  this.FindReferencedControl(childToolbar, "BulletsMenu", i);
                }
                break;
              case EditorCommandType.FormatNumberingExtended:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\FormatNumbering.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  item.ItemType = ToolBarItemType.SplitDropDown;
                  item.DropDownId = "NumberingMenu";
                  this.FindReferencedControl(childToolbar, "NumberingMenu", i);
                }
                break;
              case EditorCommandType.FormatFontFace:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\FormatFontFace.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  ToolBarItemContent itemContent = new ToolBarItemContent();
                  item.CustomContentId = itemContent.ID = "FormatFontFaceItem";
                  itemContent.Controls.Add(childAscx);
                  childToolbar.Content.Add(itemContent);
                  this.FindReferencedControl(childToolbar, "ComboBoxFontFace", i);
                }
                break;
              case EditorCommandType.FormatFontSize:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\FormatFontSize.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  ToolBarItemContent itemContent = new ToolBarItemContent();
                  item.CustomContentId = itemContent.ID = "FormatFontSizeItem";
                  itemContent.Controls.Add(childAscx);
                  childToolbar.Content.Add(itemContent);
                  this.FindReferencedControl(childToolbar, "ComboBoxFontSize", i);
                }
                break;
              case EditorCommandType.FormatFontColor:
              case EditorCommandType.FormatHighlight:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\FontColor.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  item.ItemType = ToolBarItemType.SplitDropDown;
                  item.DropDownId = "FontColorMenu";
                  this.FindReferencedControl(childToolbar, "FontColorMenu", i);
                }
                break;
              case EditorCommandType.FormatStyle:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\FormatStyle.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  ToolBarItemContent itemContent = new ToolBarItemContent();
                  item.CustomContentId = itemContent.ID = "FormatStyleItem";
                  itemContent.Controls.Add(childAscx);
                  childToolbar.Content.Add(itemContent);
                  this.FindReferencedControl(childToolbar, "ComboBoxStyle", i);
                }
                break;
              case EditorCommandType.InsertHyperlink:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\InsertEditHyperlink.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  Dialog childDialog = (Dialog)Utils.FindControl(childAscx, "HyperlinkDialog");
                  item.ClientSideCommand = childDialog.GetSaneId() + ".show()";
                  this.FindReferencedControl(childToolbar, "HyperlinkDialog", i);
                }
                break;
              case EditorCommandType.InsertMedia:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\InsertEditMedia.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  Dialog childDialog = (Dialog)Utils.FindControl(childAscx, "MediaDialog");
                  item.ClientSideCommand = childDialog.GetSaneId() + ".show()";
                  this.FindReferencedControl(childToolbar, "MediaDialog", i);
                }
                break;
              case EditorCommandType.InsertSymbol:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\InsertSymbol.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  item.ItemType = ToolBarItemType.DropDown;
                  item.DropDownId = "InsertSymbolMenu";
                  this.FindReferencedControl(childToolbar, "InsertSymbolMenu", i);
                }
                break;
              case EditorCommandType.InsertTable:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\InsertTable.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  item.ItemType = ToolBarItemType.DropDown;
                  item.DropDownId = "InsertTableMenu";
                  this.FindReferencedControl(childToolbar, "InsertTableMenu", i);
                }
                break;
            case EditorCommandType.ImageManager:
                {
                    UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\ImageManager.ascx");
                    childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                    childAscx.Attributes["EditorUploadPath"] = this.UploadPath;
                    this.Controls.Add(childAscx);
                    Dialog childDialog = (Dialog)Utils.FindControl(childAscx, "ImageManagerDialog");
                    item.ClientSideCommand = childDialog.GetSaneId() + ".show()";
                    this.FindReferencedControl(childToolbar, "ImageManagerDialog", i);
                }
                break;
              case EditorCommandType.ToolsSpelling:
                {
                  UserControl childAscx = (UserControl)Page.LoadControl(this.SkinFolderLocation + "\\widgets\\SpellChecker.ascx");
                  childAscx.Attributes["SkinFolderLocation"] = this.SkinFolderLocation;
                  this.Controls.Add(childAscx);
                  Dialog childDialog = (Dialog)Utils.FindControl(childAscx, "SpellCheckDialog");
                  item.ClientSideCommand = "beginSpellCheck();";
                  this.FindReferencedControl(childToolbar, "SpellCheckDialog", i);
                }
                break;
              default:
                this.FindReferencedControl(childToolbar, item.Attributes["ChildControlId"], i);
                this.FindReferencedControl(childToolbar, item.Attributes["DropDownId"], i);
                break;
            }
          }
          else
          {
            this.FindReferencedControl(childToolbar, item.Attributes["ChildControlId"], i);
            this.FindReferencedControl(childToolbar, item.Attributes["DropDownId"], i);
          }
        }
      }
      this._controlReferences = new JavaScriptArray();
      foreach (Control childControl in this.Controls)
      {
        CatalogChildControls(childControl);
      }
      foreach (DictionaryEntry referenceEntry in this._referencedControls)
      {
        ControlReference controlReference = (ControlReference)(referenceEntry.Value);
        ComponentArt.Web.UI.WebControl webuicontrol = controlReference.Control as ComponentArt.Web.UI.WebControl;
        HookUpChildWebControl(webuicontrol);
        this._controlReferences.Add(JavaScriptControlEntry(webuicontrol, controlReference));
      }
      this._referencedControls.Clear();
    }

    private void CatalogChildControls(Control control)
    {
      if (control is ComponentArt.Web.UI.WebControl)
      {
        ComponentArt.Web.UI.WebControl webuicontrol = control as ComponentArt.Web.UI.WebControl;
        HookUpChildWebControl(webuicontrol);
        this._controlReferences.Add(JavaScriptControlEntry(webuicontrol, (ControlReference)this._referencedControls[control.ClientID]));
        this._referencedControls.Remove(control.ClientID);
      }
      foreach (Control childControl in control.Controls)
      {
        CatalogChildControls(childControl);
      }
    }

    private void FindReferencedControl(WebControl parentControl, string referencedControlId, int parentIndex)
    {
      if (referencedControlId != null && referencedControlId != "")
      {
        Control referencedControl = Utils.FindControl(parentControl, referencedControlId);
        if (referencedControl == null)
        {
          throw new Exception("Control '" + referencedControlId + "' not found.");
        }
        if (!this._referencedControls.Contains(referencedControl.ClientID))
        {
          this._referencedControls.Add(referencedControl.ClientID, new ControlReference(referencedControl, parentControl.GetSaneId(), parentIndex));
        }
      }
    }

    private void HandleCallback()
    {
      bool bSuccess = false;

      string sVarPrefix = "Cart_" + this.GetSaneId();
      
      try
      {
        // get callback level
        string sContent = Context.Request.Params[sVarPrefix + "_Content"];
        string sText = Context.Request.Params[sVarPrefix + "_Text"];

        // load appropriate data
        if (this.Save != null)
        {
          EditorSaveEventArgs args = new EditorSaveEventArgs();

          // undo encoding
          args.Content = sContent.Replace("%#CLT#%", "<");
          args.Text = sText;

          this.OnSave(args);

          bSuccess = true;
        }
      }
      catch
      {
        bSuccess = false;
      }

      Context.Response.Clear();
      Context.Response.ContentType = "text/xml";
      Context.Response.Write("<EditorSaveResponse>");
      Context.Response.Write(bSuccess.ToString().ToLower());
      Context.Response.Write("</EditorSaveResponse>");
      Context.Response.End();
    }

    private void HookUpChildWebControl(ComponentArt.Web.UI.WebControl webcontrol)
    {
      webcontrol.License = new ComponentArt.Licensing.Providers.RedistributableLicense(new ComponentArt.Licensing.Providers.RedistributableLicenseProvider(), "");
      if (webcontrol is ComboBox)
      {
        ComboBox combobox = webcontrol as ComboBox;
        combobox.ClientEvents.Handlers.Add("Init", "function(sender, eventArgs) {ComponentArt_Editor_HookUpControl(" + this.GetSaneId() + ", sender);}");
        // Add a condition to make sure we're not initializing the ComboBox before the Editor client-side object is instantiated.
        // The "offsetParent" bit is there to make sure IE doesn't mistake a DOM element of the same name for the Editor variable.
        combobox.ClientInitCondition = "window." + this.GetSaneId() + " && !" + this.GetSaneId() + ".offsetParent";
      }
    }

    private string JavaScriptControlEntry(WebControl control, ControlReference controlReference)
    {
      StringBuilder result = new StringBuilder();
      result.Append("'").Append(control.GetSaneId()).Append("':");
      if (controlReference != null)
      {
        result.Append("{");
        result.Append("'ParentToolBarId':'").Append(controlReference.ParentToolBarId).Append("',");
        result.Append("'ParentItemIndex':").Append(controlReference.ParentItemIndex.ToString());
        result.Append("}");
      }
      else
      {
        result.Append("null");
      }
      return result.ToString();
    }

    #endregion

    #region Events

    /// <summary>
    /// Delegate for <see cref="Save"/> event of <see cref="Editor"/> class.
    /// </summary>
    public delegate void SaveEventHandler(object sender, EditorSaveEventArgs e);

    /// <summary>
    /// Fires when a document save is requested via callback.
    /// </summary>
    [Description("Fires when a document save is requested via callback.")]
    [Category("Editor Events")]
    public event SaveEventHandler Save;

    private void OnSave(EditorSaveEventArgs e)
    {
      if (Save != null)
      {
        Save(this, e);
      }
    }

#endregion

  }

  /// <summary>
  /// Arguments for <see cref="Editor.Save"/> server-side event of <see cref="Editor"/> control.
  /// </summary>
  public class EditorSaveEventArgs : EventArgs
  {
    public string Content;
    public string Text;
  }

  /// <summary>
  /// Font style for the <see cref="Editor"/> control.
  /// </summary>
  public class EditorStyle
  {
    public EditorStyle()
    {
    }

    private string _name;
    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }

    private string _element;
    public string Element
    {
      get
      {
        return this._element;
      }
      set
      {
        this._element = value;
      }
    }

    private bool _replaceParagraphTag = false;
    public bool ReplaceParagraphTag
    {
      get
      {
        return this._replaceParagraphTag;
      }
      set
      {
        this._replaceParagraphTag = value;
      }
    }

    private string _cssClass;
    public string CssClass
    {
      get
      {
        return this._cssClass;
      }
      set
      {
        this._cssClass = value;
      }
    }

    private string _styleString;
    public string StyleString
    {
      get
      {
        return this._styleString;
      }
      set
      {
        this._styleString = value;
      }
    }

    private string _dropDownCssClass;
    public string DropDownCssClass
    {
      get
      {
        return this._dropDownCssClass;
      }
      set
      {
        this._dropDownCssClass = value;
      }
    }

    private string _dropDownStyleString;
    public string DropDownStyleString
    {
      get
      {
        return this._dropDownStyleString;
      }
      set
      {
        this._dropDownStyleString = value;
      }
    }

    public override string ToString()
    {
      return "[" + string.Join(",", new string[] {
          Utils.ConvertStringToJSString(this.Name),
          Utils.ConvertStringToJSString(this.Element),
          this.ReplaceParagraphTag.ToString().ToLower(),
          Utils.ConvertStringToJSString(this.CssClass),
          Utils.ConvertStringToJSString(this.StyleString),
          Utils.ConvertStringToJSString(this.DropDownCssClass),
          Utils.ConvertStringToJSString(this.DropDownStyleString)
        }) + "]";
    }
  }

  /// <summary>
  /// Collection of <see cref="EditorStyle"/> objects.
  /// </summary>
  public class EditorStyleCollection : CollectionBase
  {
    public new EditorStyle this[int index]
    {
      get
      {
        return (EditorStyle)this.List[index];
      }
    }

    public int Add(EditorStyle style)
    {
      return this.List.Add(style);
    }

    public override string ToString()
    {
      string[] styleStringArray = new string[this.List.Count];
      for (int i = 0; i < styleStringArray.Length; i++)
      {
        styleStringArray[i] = this.List[i].ToString();
      }
      return "[" + string.Join(",", styleStringArray) + "]";
    }
  }

  #region Enumerations
  /// <summary> 
  /// Specifies the mode in which the editor should run<see cref="Editor"/> control.
  /// </summary>
  public enum EditModeType
  {
    /// <summary>Normal editor textarea</summary> 
    Default,
    /// <summary>Edit dom elements in place.</summary> 
    Inline
  }

  /// <summary> 
  /// Specifies which editing engine the editor should use<see cref="Editor"/> control.
  /// </summary>
  public enum EditEngineType
  {
    /// <summary>The default browser editing engine.</summary> 
    Browser,
    /// <summary>The ComponentArt editoring engine.</summary> 
    ComponentArt
  }

  public enum EditorCommandType
  // NOTE: Entries in this enum must match entries in client-side enum cart_editor_command_type
  {
    Save,
    New,
    EditUndo,
    EditRedo,
    EditCut,
    EditCopy,
    EditPaste,
    ViewRevealFormatting, // Switch between source and design views.
    InsertHyperlink,
    FormatFont/*DropDown*/,
    FormatFontFace, // or FontName?
    FormatFontSize,
    FormatBold,
    FormatItalic,
    FormatUnderline,
    FormatFontColor/*ForeColor*/,
    FormatHighlight/*BackColor*/,
    FormatNumbering,
    FormatBullets,
    FormatAlignLeft,
    FormatAlignCenter,
    FormatAlignRight,
    FormatAlignJustify,
    ToolsSpelling,
    ToolsSetLanguage,
    InsertHorizontalRule,
    FormatIncreaseIndent,
    FormatDecreaseIndent,
    FormatStyle,
    EditUndoExtended,
    EditRedoExtended,
    FormatStrikethrough,
    Print,
    TableInsertRowAbove,
    TableInsertRowBelow,
    TableInsertColumnRight,
    TableInsertColumnLeft,
    TableDeleteRow,
    TableDeleteColumn,
    TableDeleteCell,
    TableMergeCellRight,
    TableMergeCellDown,
    TableSplitCellHorizontal,
    TableSplitCellVertical,
    InsertForm,
    InsertButton,
    InsertCheckbox,
    InsertHidden,
    InsertRadio,
    InsertPassword,
    InsertReset,
    InsertSelect,
    InsertSubmit,
    InsertTextbox,
    InsertTextarea,
    RemoveHyperlink,
    EditFind,
    EditReplace,
    InsertMedia,
    InsertSymbol,
    InsertTable,
    FormatNumberingExtended,
    FormatBulletsExtended,
    ImageManager,
    //FormatCustom?
    //InsertCustom?
    Custom
  }

  #endregion
}
