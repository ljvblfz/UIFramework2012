using System;
using System.Text;  
using System.Drawing;    
using System.Web; 
using System.Web.UI; 
using System.Web.UI.HtmlControls;   
using System.Web.UI.WebControls;       
using System.ComponentModel;   
using System.Collections; 
using System.IO;     
using System.Runtime.InteropServices;

namespace ComponentArt.Web.UI
{
  #region Enumerations
  /// <summary> 
  /// Specifies the type of animation to use with a <see cref="Dialog"/> control.
  /// </summary>
  public enum DialogAnimationType
  {
    /// <summary>Do not animate.</summary> 
    None,
    /// <summary>Animate an outline.</summary> 
    Outline,
    /// <summary>Animate the entire instance, IE only.</summary> 
    Live,
  }

  /// <summary> 
  /// Specifies the path for animation to use with a <see cref="Dialog"/> control.
  /// </summary>
  public enum SlidePath
  {
    /// <summary>Move directly to the target position.</summary> 
    Direct,
    /// <summary>Move with directional acceleration to the target position.</summary> 
    Boomerang,
  }

  /// <summary> 
  /// Specifies which point of the browser window <see cref="Dialog"/> control should be aligning with.
  /// </summary>
  public enum DialogAlignType
  {
    /// <summary>Default alignment.</summary> 
    Default,

    /// <summary>Align to top left corner of the window.</summary>
    TopLeft,

    /// <summary>Align to middle of the top edge of the window.</summary>
    TopCentre,

    /// <summary>Align to top right corner of the window.</summary>
    TopRight,

    /// <summary>Align to bottom left corner of the window.</summary>
    BottomLeft,

    /// <summary>Align to middle of the bottom edge of the window.</summary>
    BottomCentre,

    /// <summary>Align to bottom right corner of the window.</summary>
    BottomRight,

    /// <summary>Align to middle of the left edge of the window.</summary>
    MiddleLeft,

    /// <summary>Align to centre of the window.</summary>
    MiddleCentre,

    /// <summary>Align to middle of the right edge of the window.</summary>
    MiddleRight
  }
  #endregion

  #region DialogTemplateContainer

  /// <summary>
  /// Naming container used for housing templated contents of the <see cref="Dialog"/> control.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The following are instantiated in <b>DialogTemplateContainer</b> instances: 
  /// <see cref="Dialog.HeaderTemplate"/>, <see cref="Dialog.ContentTemplate"/> and
  /// <see cref="Dialog.FooterTemplate"/>.
  /// </para> 
  /// <para>
  /// Not intended for direct use by developers.
  /// </para>
  /// </remarks>
  [ToolboxItem(false)]
  public class DialogTemplateContainer : Control, INamingContainer
  {
    private Dialog parent;
    public DialogTemplateContainer(Dialog parent)
    {
      this.parent = parent;
    }
  }

  #endregion

  #region DialogContent

  /// <summary>
  /// Class for content sections of the <see cref="Dialog"/> control.
  /// </summary>
  /// <remarks>
  /// Following <b>DialogContent</b> instances are implemented by the <b>Dialog</b> control:
  /// <see cref="Dialog.HeaderTemplate"/>, <see cref="Dialog.ContentTemplate"/>, and
  /// <see cref="Dialog.FooterTemplate"/>.
  /// </remarks>
  [ToolboxItem(false)]
  public class DialogContent : Control
  {

  }

  #endregion

  /// <summary>
  /// Displays a dialog box on a web page, offering functionality to show and close as well as track results.
  /// </summary>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:Dialog runat=server></{0}:Dialog>")]
  [ParseChildren(true)]
  [PersistChildren(false)]
  [Designer(typeof(ComponentArt.Web.UI.DialogDesigner))]
  public class Dialog : WebControl
  {
    #region Private Properties

    private DialogContent m_oHeader;
    private ITemplate m_oHeaderTemplate;
    private DialogTemplateContainer m_oHeaderTemplateContainer;

    private DialogContent m_oContent;
    private ITemplate m_oContentTemplate;
    private DialogTemplateContainer m_oContentTemplateContainer;

    private DialogContent m_oFooter;
    private ITemplate m_oFooterTemplate;
    private DialogTemplateContainer m_oFooterTemplateContainer;

    #endregion

    #region Public Properties

    private DialogClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public DialogClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new DialogClientEvents();
        }
        return _clientEvents;
      }
    }

    #region CssClasses
    /// <summary>
    /// The CssClass to apply to Dialog Content.
    /// </summary>
    [Category("Style")]
    [DefaultValue(null)]
    public string ContentCssClass
    {
      get
      {
        object o = ViewState["ContentCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["ContentCssClass"] = value;
      }
    }


    /// <summary>
    /// The CssClass to apply to Dialog Footer.
    /// </summary>
    [Category("Style")]
    [DefaultValue(null)]
    public string FooterCssClass
    {
      get
      {
        object o = ViewState["FooterCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["FooterCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to Dialog Header.
    /// </summary>
    [Category("Style")]
    [DefaultValue(null)]
    public string HeaderCssClass
    {
      get
      {
        object o = ViewState["HeaderCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["HeaderCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to Modal Dialog Mask.
    /// </summary>
    [Category("Style")]
    [DefaultValue(null)]
    public string ModalMaskCssClass
    {
      get
      {
        object o = ViewState["ModalMaskCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["ModalMaskCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to an animated outline.
    /// </summary>
    [Category("Style")]
    [DefaultValue(null)]
    public string OutlineCssClass
    {
      get
      {
        object o = ViewState["OutlineCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["OutlineCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to the IFrame when ContentUrl is specified.
    /// </summary>
    [Category("Style")]
    [DefaultValue(null)]
    public string IFrameCssClass
    {
      get
      {
        object o = ViewState["IFrameCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IFrameCssClass"] = value;
      }
    }

    #endregion


    /// <summary>
    /// The frameBorder property to apply to the ContentUrl IFrame. Use "0" for none.
    /// </summary>
    [Category("Style")]
    [DefaultValue(null)]
    public string IFrameBorder
    {
      get
      {
        object o = ViewState["IFrameBorder"];
        return (o == null) ? String.Empty : (string)o;
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
    [DefaultValue(null)]
    public string IFrameScrolling
    {
      get
      {
        object o = ViewState["IFrameScrolling"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IFrameScrolling"] = value;
      }
    }

    /// <summary>
    /// The adjustment to offset centered content for modal dialogs to compenstate for lack of scrollbars.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    public int ModalScrollbarOffset
    {
      get
      {
        object o = ViewState["ModalScrollbarOffset"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["ModalScrollbarOffset"] = value;
      }
    }

    #region Client Templates

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

    /// <summary>
    /// The Id of the Client Template to apply to Dialog Header.
    /// </summary>
    [Category("Templates")]
    [DefaultValue(null)]
    public string HeaderClientTemplateId
    {
      get
      {
        object o = ViewState["HeaderClientTemplateId"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["HeaderClientTemplateId"] = value;
      }
    }
    /// <summary>
    /// The Id of the Client Template to apply to Dialog Content.
    /// </summary>
    [Category("Templates")]
    [DefaultValue(null)]
    public string ContentClientTemplateId
    {
      get
      {
        object o = ViewState["ContentClientTemplateId"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["ContentClientTemplateId"] = value;
      }
    }
    /// <summary>
    /// The Id of the Client Template to apply to Dialog Footer.
    /// </summary>
    [Category("Templates")]
    [DefaultValue(null)]
    public string FooterClientTemplateId
    {
      get
      {
        object o = ViewState["FooterClientTemplateId"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["FooterClientTemplateId"] = value;
      }
    }

    #endregion

    /// <summary>
    /// The Title of the Dialog.
    /// </summary>
    [Category("Content")]
    [DefaultValue(null)]
    public string Title
    {
      get
      {
        object o = ViewState["Title"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["Title"] = value;
      }
    }

    /// <summary>
    /// The Icon of the Dialog.
    /// </summary>
    [Category("Content")]
    [DefaultValue(null)]
    public string Icon
    {
      get
      {
        object o = ViewState["Icon"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["Icon"] = value;
      }
    }

    /// <summary>
    /// The Modal Mask Image of the Dialog.
    /// </summary>
    [Category("Content")]
    [DefaultValue(null)]
    public string ModalMaskImage
    {
      get
      {
        object o = ViewState["ModalMaskImage"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["ModalMaskImage"] = value;
      }
    }

    /// <summary>
    /// Custom Value of the Dialog.
    /// </summary>
    [Category("Content")]
    [DefaultValue(null)]
    public string Value
    {
      get
      {
        object o = ViewState["Value"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["Value"] = value;
      }
    }

    /// <summary>
    /// Whether to bring this Dialog to top on mouse click. Default: false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool FocusOnClick
    {
      get
      {
        object o = ViewState["FocusOnClick"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["FocusOnClick"] = value;
      }
    }

    /// <summary>
    /// Whether to create an IFrame to overlay Windowed Objects.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public Boolean RenderOverWindowedObjects
    {
      get
      {
        object o = ViewState["RenderOverWindowedObjects"];
        return (o == null) ? false : (Boolean)o;
      }
      set
      {
        ViewState["RenderOverWindowedObjects"] = value;
      }
    }

    /// <summary>
    /// Whether to show this Dialog initially.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public Boolean IsShowing
    {
      get
      {
        object o = ViewState["IsShowing"];
        return (o == null) ? false : (Boolean)o;
      }
      set
      {
        ViewState["IsShowing"] = value;
      }
    }

    /// <summary>
    /// Whether to allow resizing of this Dialog.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public Boolean AllowResize
    {
      get
      {
        object o = ViewState["AllowResize"];
        return (o == null) ? false : (Boolean)o;
      }
      set
      {
        ViewState["AllowResize"] = value;
      }
    }

    /// <summary>
    /// Whether to allow dragging of this Dialog.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(true)]
    public Boolean AllowDrag
    {
      get
      {
        object o = ViewState["AllowDrag"];
        return (o == null) ? true : (Boolean)o;
      }
      set
      {
        ViewState["AllowDrag"] = value;
      }
    }

    /// <summary>
    /// Whether to preload the ContentURL, or only load OnShow.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool PreloadContentUrl
    {
      get
      {
        object o = ViewState["PreloadContentUrl"];
        return (o == null) ? true : (Boolean)o;
      }
      set
      {
        ViewState["PreloadContentUrl"] = value;
      }
    }

    /// <summary>
    /// External Content URL for Dialog.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(null)]
    public string ContentUrl
    {
      get
      {
        object o = ViewState["ContentUrl"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["ContentUrl"] = value;
      }
    }

    /// <summary>
    /// Whether or not the Dialog is modal.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public Boolean Modal
    {
      get
      {
        object o = ViewState["Modal"];
        return (o == null) ? false : (Boolean)o;
      }
      set
      {
        ViewState["Modal"] = value;
      }
    }


    /// <summary>
    /// Starting Z-Index. Default 70000.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(70000)]
    public int ZIndex
    {
      get
      {
        object o = ViewState["ZIndex"];
        return (o == null) ? 70000 : (int)o;
      }
      set
      {
        ViewState["ZIndex"] = value;
      }
    }

    /// <summary>
    /// The type of alignment to apply to this Dialog. Default: MiddleCentre.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(DialogAlignType.Default)]
    public DialogAlignType Alignment
    {
      get
      {
        object o = ViewState["Alignment"];
        return (o == null) ? DialogAlignType.MiddleCentre : (DialogAlignType)o;
      }
      set
      {
        ViewState["Alignment"] = value;
      }
    }

    /// <summary>
    /// Minimum height. Default: 100.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(100)]
    public int MinimumHeight
    {
      get
      {
        object o = ViewState["MinimumHeight"];
        return (o == null) ? 100 : (int)o;
      }
      set
      {
        ViewState["MinimumHeight"] = value;
      }
    }

    /// <summary>
    /// Minimum width. Default: 100.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(100)]
    public int MinimumWidth
    {
      get
      {
        object o = ViewState["MinimumWidth"];
        return (o == null) ? 100 : (int)o;
      }
      set
      {
        ViewState["MinimumWidth"] = value;
      }
    }

    /// <summary>
    /// Offset along the X axis when aligning. Default: 0.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    public int OffsetX
    {
      get
      {
        //this.EnsureParamsSynchronized();

        object o = ViewState["OffsetX"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["OffsetX"] = value;
      }
    }

    /// <summary>
    /// Offset along the Y axis when aligning. Default: 0.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    public int OffsetY
    {
      get
      {
        //this.EnsureParamsSynchronized();

        object o = ViewState["OffsetY"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["OffsetY"] = value;
      }
    }

    /// <summary>
    /// Absolute X value. Default: 0.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    public int X
    {
      get
      {
        //this.EnsureParamsSynchronized();

        object o = ViewState["X"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["X"] = value;
      }
    }

    /// <summary>
    /// Absolute Y value. Default: 0.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    public int Y
    {
      get
      {
        //this.EnsureParamsSynchronized();

        object o = ViewState["Y"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["Y"] = value;
      }
    }

    /// <summary>
    /// The ID of the DOM element to align the dialog to.
    /// </summary>
    [Category("Layout")]
    public string AlignmentElement
    {
      get
      {
        string s = (string)ViewState["AlignmentElement"];
        return (s == null) ? string.Empty : s;
      }
      set
      {
        ViewState["AlignmentElement"] = value;
      }
    }

    /// <summary>
    /// The type of animation to use with this Dialog. Default: Live.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(DialogAnimationType.None)]
    public DialogAnimationType AnimationType
    {
      get
      {
        object o = ViewState["AnimationType"];
        return (o == null) ? DialogAnimationType.None : (DialogAnimationType)o;
      }
      set
      {
        ViewState["AnimationType"] = value;
      }
    }

    /// <summary> 
    /// The transition effect to use for showing Dialog.
    /// </summary>
    public TransitionType ShowTransition
    {
      get
      {
        return Utils.ParseTransitionType(ViewState["ShowTransition"]);
      }
      set
      {
        ViewState["ShowTransition"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for closing Dialog.
    /// </summary>
    public TransitionType CloseTransition
    {
      get
      {
        return Utils.ParseTransitionType(ViewState["CloseTransition"]);
      }
      set
      {
        ViewState["CloseTransition"] = value;
      }
    }


    /// <summary>
    /// The ID of the DOM element toward which the open/close animation should proceed.
    /// </summary>
    [Category("Behavior")]
    public string AnimationDirectionElement
    {
      get
      {
        string s = (string)ViewState["AnimationDirectionElement"];
        return (s == null) ? this.ID : s;
      }
      set
      {
        ViewState["AnimationDirectionElement"] = value;
      }
    }

    /// <summary>
    /// The duration of the open/close transition animation.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(100)]
    public int TransitionDuration
    {
      get
      {
        object o = ViewState["TransitionDuration"];
        return (o == null) ? 100 : (int)o;
      }
      set
      {
        ViewState["TransitionDuration"] = value;
      }
    }

    /// <summary>
    /// The duration of the open/close animation.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int AnimationDuration
    {
      get
      {
        object o = ViewState["AnimationDuration"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["AnimationDuration"] = value;
      }
    }

    /// <summary>
    /// The type of the slide animation.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(SlideType.Linear)]
    public SlideType AnimationSlide
    {
      get
      {
        object o = ViewState["AnimationSlide"];
        return (o == null) ? SlideType.Linear : (SlideType)o;
      }
      set
      {
        ViewState["AnimationSlide"] = value;
      }
    }

    /// <summary>
    /// The path of the slide animation.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(SlidePath.Direct)]
    public SlidePath AnimationPath
    {
      get
      {
        object o = ViewState["AnimationPath"];
        return (o == null) ? SlidePath.Direct : (SlidePath)o;
      }
      set
      {
        ViewState["AnimationPath"] = value;
      }
    }

    /// <summary>
    /// Dialog header container.
    /// </summary>

    [Browsable(false)]
    [DefaultValue(null)]
    [Description("The header container.")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [NotifyParentProperty(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DialogContent Header
    {
      get
      {
        return m_oHeader;
      }
      set
      {
        m_oHeader = value;
      }
    }

    /// <summary>
    /// Template for the Dialog header.
    /// </summary>

    [Browsable(false)]
    [DefaultValue(null)]
    [Description("The header template")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(ComponentArt.Web.UI.DialogTemplateContainer))]
    public ITemplate HeaderTemplate
    {
      get
      {
        return m_oHeaderTemplate;
      }
      set
      {
        m_oHeaderTemplate = value;
      }
    }

    /// <summary>
    /// Dialog content container.
    /// </summary>

    [Browsable(true)]
    [DefaultValue(null)]
    [Description("The content container.")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [NotifyParentProperty(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DialogContent Content
    {
      get
      {
        return m_oContent;
      }
      set
      {
        m_oContent = value;
      }
    }

    /// <summary>
    /// Template for the inner contents of the Dialog.
    /// </summary>
    [Browsable(false)]
    [DefaultValue(null)]
    [Description("The content template")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(ComponentArt.Web.UI.DialogTemplateContainer))]
    public ITemplate ContentTemplate
    {
      get
      {
        return m_oContentTemplate;
      }
      set
      {
        m_oContentTemplate = value;
      }
    }

    /// <summary>
    /// Dialog footer content container.
    /// </summary>
    [Browsable(false)]
    [DefaultValue(null)]
    [Description("The footer container.")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [NotifyParentProperty(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DialogContent Footer
    {
      get
      {
        return m_oFooter;
      }
      set
      {
        m_oFooter = value;
      }
    }

    /// <summary>
    /// Template for the Dialog footer.
    /// </summary>
    [Browsable(false)]
    [DefaultValue(null)]
    [Description("The footer template")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [TemplateContainer(typeof(ComponentArt.Web.UI.DialogTemplateContainer))]
    public ITemplate FooterTemplate
    {
      get
      {
        return m_oFooterTemplate;
      }
      set
      {
        m_oFooterTemplate = value;
      }
    }

    [Browsable(false)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public override ControlCollection Controls
    {
      get
      {
        EnsureChildControls();
        return base.Controls;
      }
    }
    #endregion

    #region Public Methods

    public Dialog()
    {
    }

    public bool RenderDefaultCss()
    {
      if (this.CssClass == String.Empty && this.HeaderCssClass == String.Empty && this.ContentCssClass == String.Empty && this.FooterCssClass == String.Empty)
        return true;
      return false;
    }

    #endregion

    #region Protected Methods

    protected override void RenderContents(HtmlTextWriter output)
    {
      int ie = 0;
      try
      {
        ie = Context.Request.UserAgent.IndexOf("MSIE");
      }
      catch
      {
        ie = -1;
      }

      output.Write("<div");
      output.WriteAttribute("id", this.ClientID + "_PlaceHolder");
      output.Write("></div>");

      // render each div and its contents 
      output.Write("<div");
      output.WriteAttribute("id", this.ClientID);



      if (!RenderDefaultCss())
      {
        output.WriteAttribute("style", "position:absolute;visibility:hidden;height:" + this.Height.ToString() + ";width:" + this.Width.ToString() + ";");
        output.WriteAttribute("class", this.CssClass);
      }
      else
      {
        output.WriteAttribute("style", "background-color:#EEEEEE;border:1px;border-color:black;border-top-color:gray;border-left-color:gray;border-style:solid;position:absolute;visibility:hidden;height:" + this.Height.ToString() + ";width:" + this.Width.ToString() + ";");
      }

      output.Write(">");

      if (this.RenderOverWindowedObjects && ie > -1)
      {
        output.Write("<iframe");
        output.WriteAttribute("id", this.ClientID + "_OverlayIFrame");
        output.WriteAttribute("frameborder", "0");
        output.WriteAttribute("scrolling", "no");
        output.WriteAttribute("src", "javascript:false");

        output.Write(" style=\"");
        output.WriteStyleAttribute("position", "absolute");
        output.WriteStyleAttribute("top", "0");
        output.WriteStyleAttribute("left", "0");
        output.WriteStyleAttribute("width", "100%");
        output.WriteStyleAttribute("height", "100%");
        output.WriteStyleAttribute("display", "block");
        output.WriteStyleAttribute("z-index", "-1");
        output.WriteStyleAttribute("filter", "progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)");
        output.Write("\"></iframe>");
      }

      foreach (Control oControl in Controls)
      {
        oControl.RenderControl(output);
      }

      output.Write("</div>");
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      output = new HtmlTextWriter(output, string.Empty);
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
        if (!Page.IsClientScriptBlockRegistered("A573G999.js"))
        {
          Page.RegisterClientScriptBlock("A573G999.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.Dialog.client_scripts", "A573G999.js");
        }
        if (!Page.IsClientScriptBlockRegistered("A573G130.js"))
        {
          Page.RegisterClientScriptBlock("A573G130.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.Dialog.client_scripts", "A573G130.js");
        }
      }

      string sDialogVarName = this.GetSaneId();

      // Content Output
      RenderContents(output);

      // Render client-side object initiation. 
      StringBuilder oStartupSB = new StringBuilder();

      oStartupSB.Append("/*** ComponentArt.Web.UI.Dialog ").Append(this.VersionString()).Append(" ").Append(sDialogVarName).Append(" ***/\n");

      oStartupSB.Append("function ComponentArt_Init_" + sDialogVarName + "() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      oStartupSB.Append("if(!window.ComponentArt_Dialog_Kernel_Loaded)\n");
      oStartupSB.Append("\t{setTimeout('ComponentArt_Init_" + sDialogVarName + "()', 100); return; }\n\n");

      // Instantiate object
      oStartupSB.Append("window." + sDialogVarName + " = new ComponentArt_Dialog('" + sDialogVarName + "');\n");

      // Hook the actual ID if available and different from effective client ID
      if (this.ID != sDialogVarName)
      {
        oStartupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sDialogVarName + "; " + sDialogVarName + ".GlobalAlias = '" + ID + "'; }\n");
      }

      oStartupSB.Append(sDialogVarName + ".ControlId = '" + this.UniqueID + "';\n");

      oStartupSB.Append("var properties = [\n");
      oStartupSB.Append("['ApplicationPath'," + Utils.ConvertStringToJSString(Context.Request.ApplicationPath) + "],");
      oStartupSB.Append("['Alignment','" + this.Alignment.ToString() + "'],");
      oStartupSB.Append("['OffsetX'," + this.OffsetX.ToString() + "],");
      oStartupSB.Append("['OffsetY'," + this.OffsetY.ToString() + "],");
      oStartupSB.Append("['MinimumHeight'," + this.MinimumHeight.ToString() + "],");
      oStartupSB.Append("['MinimumWidth'," + this.MinimumWidth.ToString() + "],");
      oStartupSB.Append("['Height','" + this.Height.ToString() + "'],");
      oStartupSB.Append("['Width','" + this.Width.ToString() + "'],");
      oStartupSB.Append("['X'," + this.X.ToString() + "],");
      oStartupSB.Append("['Y'," + this.Y.ToString() + "],");
      oStartupSB.Append("['Modal'," + this.Modal.ToString().ToLower() + "],");
      oStartupSB.Append("['FocusOnClick','" + this.FocusOnClick.ToString().ToLower() + "'],");
      oStartupSB.Append("['ModalMaskImage','" + this.ModalMaskImage.ToString().ToLower() + "'],");
      oStartupSB.Append("['PreloadContentUrl'," + this.PreloadContentUrl.ToString().ToLower() + "],");
      oStartupSB.Append("['AllowDrag'," + this.AllowDrag.ToString().ToLower() + "],");
      oStartupSB.Append("['AllowResize'," + this.AllowResize.ToString().ToLower() + "],");
      oStartupSB.Append("['RenderOverWindowedObjects'," + this.RenderOverWindowedObjects.ToString().ToLower() + "],");
      if (this.HeaderClientTemplateId != String.Empty) oStartupSB.Append("['HeaderClientTemplateId','" + this.HeaderClientTemplateId.ToString() + "'],");
      if (this.ContentClientTemplateId != String.Empty) oStartupSB.Append("['ContentClientTemplateId','" + this.ContentClientTemplateId.ToString() + "'],");
      if (this.FooterClientTemplateId != String.Empty) oStartupSB.Append("['FooterClientTemplateId','" + this.FooterClientTemplateId.ToString() + "'],");

      oStartupSB.Append("['Content',null],");

      oStartupSB.Append("['ShowTransition'," + ((int)this.ShowTransition).ToString() + "],");
      oStartupSB.Append("['CloseTransition'," + ((int)this.CloseTransition).ToString() + "],");
      oStartupSB.Append("['AlignmentElement','" + this.AlignmentElement + "'],");
      oStartupSB.Append("['AnimationDirectionElement','" + this.AnimationDirectionElement + "'],");
      oStartupSB.Append("['AnimationSlide'," + ((int)this.AnimationSlide).ToString() + "],");
      oStartupSB.Append("['TransitionDuration'," + this.TransitionDuration.ToString() + "],");
      oStartupSB.Append("['AnimationDuration'," + this.AnimationDuration.ToString() + "],");
      oStartupSB.Append("['AnimationType','" + this.AnimationType.ToString() + "'],");
      oStartupSB.Append("['AnimationPath','" + this.AnimationPath.ToString() + "'],");
      oStartupSB.Append("['Title','" + this.Title + "'],");
      oStartupSB.Append("['Icon','" + this.Icon + "'],");
      oStartupSB.Append("['Value','" + this.Value + "'],");
      oStartupSB.Append("['ClientTemplates'," + this._clientTemplates.ToString() + "],");
      oStartupSB.Append("['ModalMaskCssClass','" + this.ModalMaskCssClass.ToString() + "'],");
      oStartupSB.Append("['ModalScrollbarOffset'," + this.ModalScrollbarOffset.ToString() + "],");
      oStartupSB.Append("['FooterCssClass','" + this.FooterCssClass.ToString() + "'],");
      oStartupSB.Append("['HeaderCssClass','" + this.HeaderCssClass.ToString() + "'],");
      oStartupSB.Append("['ContentCssClass','" + this.ContentCssClass.ToString() + "'],");
      oStartupSB.Append("['IFrameCssClass','" + this.IFrameCssClass.ToString() + "'],");
      oStartupSB.Append("['OutlineCssClass','" + this.OutlineCssClass.ToString() + "'],");
      oStartupSB.Append("['ContentUrl','" + this.ContentUrl.ToString() + "'],");
      oStartupSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "]");
      oStartupSB.Append("];\n");

      // Set properties
      oStartupSB.AppendFormat("ComponentArt_SetProperties({0}, properties);\n", sDialogVarName);

      //oStartupSB.Append("ComponentArt_HookEvents(" + sDialogVarName + ");\n");
      //oStartupSB.Append("document.getElementById('" + this.ID + "').onmousedown = art_DialogMouseDown;");
      //oStartupSB.Append(sDialogVarName + ".Render();");

      oStartupSB.Append(sDialogVarName + ".Initialize();");

      oStartupSB.Append("\n}");
      oStartupSB.Append("\nif(!zTop || zTop > "+this.ZIndex.ToString()+") var zTop = "+this.ZIndex.ToString()+";\n");

      // Initiate Dialog creation
      oStartupSB.Append("ComponentArt_Init_" + sDialogVarName + "();");

      if (this.IsShowing) oStartupSB.Append(sDialogVarName + ".Show();");

      WriteStartupScript(output, this.DemarcateClientScript(oStartupSB.ToString()));
    }



    internal void RenderDownLevel(HtmlTextWriter output)
    {
      // Sanify writer
      output = new HtmlTextWriter(output);

      // Temporary Output
      RenderContents(output);
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
      EnsureChildControls();
    }

    protected override void CreateChildControls()
    {
      if (this.Header != null)
      {
        System.Web.UI.WebControls.WebControl oHeaderSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oHeaderSpan.ID = this.ID + "_HeaderSpan";
        if (!RenderDefaultCss())
        {
          oHeaderSpan.CssClass = this.HeaderCssClass;
        }
        else
        {
          oHeaderSpan.Attributes.Add("style", "background-color:#3F3F3F;padding:5px;font-family:verdana;font-size:12px;color:white;cursor:pointer;cursor:hand;");
          if (this.AllowDrag) oHeaderSpan.Attributes.Add("onmousedown", this.ID + ".StartDrag(event);");
        }

        oHeaderSpan.Controls.Add(Header);

        Controls.Add(oHeaderSpan);
      }
      else if (this.HeaderTemplate != null)
      {
        System.Web.UI.WebControls.WebControl oHeaderSpan =
          new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oHeaderSpan.ID = this.ID + "_HeaderSpan";
        if (!RenderDefaultCss())
        {
          oHeaderSpan.CssClass = this.HeaderCssClass;
        }
        else
        {
          oHeaderSpan.Attributes.Add("style", "background-color:#3F3F3F;padding:5px;font-family:verdana;font-size:12px;color:white;cursor:pointer;cursor:hand;");
          if (this.AllowDrag) oHeaderSpan.Attributes.Add("onmousedown", this.ID + ".StartDrag(event);");
        }

        m_oHeaderTemplateContainer = new DialogTemplateContainer(this);
        this.HeaderTemplate.InstantiateIn(m_oHeaderTemplateContainer);
        oHeaderSpan.Controls.Add(m_oHeaderTemplateContainer);

        Controls.Add(oHeaderSpan);
      }
      else
      {
        System.Web.UI.WebControls.WebControl oHeaderSpan =
         new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oHeaderSpan.ID = this.ID + "_HeaderSpan";
        if (!RenderDefaultCss())
        {
          oHeaderSpan.CssClass = this.HeaderCssClass;
        }
        else
        {
          oHeaderSpan.Attributes.Add("style", "background-color:#3F3F3F;padding:5px;font-family:verdana;font-size:12px;color:white;cursor:pointer;cursor:hand;");
          if (this.AllowDrag) oHeaderSpan.Attributes.Add("onmousedown", this.ID + ".StartDrag(event);");
        }

        Controls.Add(oHeaderSpan);
        // do default header template here  
      }

      // Create inner panel 

      System.Web.UI.WebControls.WebControl oInnerSpan =
        new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);

      if (!RenderDefaultCss())
      {
        oInnerSpan.CssClass = this.ContentCssClass;
      }
      else
      {
        oInnerSpan.Attributes.Add("style", "padding:5px;font-family:verdana;font-size:12px;color:black;background-color:#EEEEEE;");
      }

      if (this.FocusOnClick) oInnerSpan.Attributes.Add("onClick", this.ID + ".Focus();");
      oInnerSpan.ID = this.ID + "_InnerSpan";

      if (this.ContentUrl != String.Empty)
      {
        System.Web.UI.WebControls.WebControl iFrame =
        new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Iframe);
        iFrame.ID = this.ID + "_IFrame";
        iFrame.CssClass = this.IFrameCssClass;

        if (this.PreloadContentUrl)
        {
            iFrame.Attributes.Add("src", Utils.ConvertUrl(HttpContext.Current, "", this.ContentUrl));
        }
        else
        {
            iFrame.Attributes.Add("src", "javascript:false");
        }

        if (this.IFrameBorder != string.Empty) iFrame.Attributes.Add("frameborder", this.IFrameBorder);
        if (this.IFrameScrolling != string.Empty) iFrame.Attributes.Add("scrolling", this.IFrameScrolling);
        oInnerSpan.Controls.Add(iFrame);
      }
      else if (this.Content != null)
      {
        oInnerSpan.Controls.Add(this.Content);
      }
      else if (this.ContentTemplate != null)
      {
        m_oContentTemplateContainer = new DialogTemplateContainer(this);
        this.ContentTemplate.InstantiateIn(m_oContentTemplateContainer);

        oInnerSpan.Controls.Add(m_oContentTemplateContainer);
      }
      else
      {
        // do default content template here
      }

      Controls.Add(oInnerSpan);


      if (this.Footer != null)
      {
        System.Web.UI.WebControls.WebControl oFooterSpan =
            new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oFooterSpan.ID = this.ID + "_FooterSpan";

        if (!RenderDefaultCss())
        {
          oFooterSpan.CssClass = this.FooterCssClass;
        }
        else
        {
          oFooterSpan.Attributes.Add("style", "padding:5px;font-family:verdana;font-size:12px;color:black;background-color:#EEEEEE;");
        }
        oFooterSpan.Controls.Add(this.Footer);
        Controls.Add(oFooterSpan);
      }
      else if (this.FooterTemplate != null)
      {
        System.Web.UI.WebControls.WebControl oFooterSpan =
            new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oFooterSpan.ID = this.ID + "_FooterSpan";

        if (!RenderDefaultCss())
        {
          oFooterSpan.CssClass = this.FooterCssClass;
        }
        else
        {
          oFooterSpan.Attributes.Add("style", "padding:5px;font-family:verdana;font-size:12px;color:black;background-color:#EEEEEE;");
        }
        m_oFooterTemplateContainer = new DialogTemplateContainer(this);
        this.FooterTemplate.InstantiateIn(m_oFooterTemplateContainer);
        oFooterSpan.Controls.Add(m_oFooterTemplateContainer);

        Controls.Add(oFooterSpan);
      }
      else
      {
        System.Web.UI.WebControls.WebControl oFooterSpan =
            new System.Web.UI.WebControls.WebControl(HtmlTextWriterTag.Div);
        oFooterSpan.ID = this.ID + "_FooterSpan";

        if (!RenderDefaultCss())
        {
          oFooterSpan.CssClass = this.FooterCssClass;
        }
        else
        {
          oFooterSpan.Attributes.Add("style", "padding:5px;font-family:verdana;font-size:12px;color:black;background-color:#EEEEEE;");
        }
        Controls.Add(oFooterSpan);
      }


    }

    #endregion
  }
}

