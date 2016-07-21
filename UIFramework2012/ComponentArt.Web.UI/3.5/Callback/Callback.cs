using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using ComponentArt.Licensing.Providers;


namespace ComponentArt.Web.UI
{
  #region EventArgs

  /// <summary>
  /// Arguments for <see cref="CallBack.Callback"/> server-side event of <see cref="CallBack"/> control.
  /// </summary>
  public class CallBackEventArgs : EventArgs
  {
    public string Parameter;
    public string [] Parameters;

    public HtmlTextWriter Output;
    
    public CallBackEventArgs(HtmlTextWriter _writer)
    {
      Output = _writer;
    }

    public CallBackEventArgs(HtmlTextWriter _writer, string sParam)
    {
      Output = _writer;
      Parameter = sParam;
    }
  }

  #endregion

  #region CallBackContent

  /// <summary>
  /// Content to render in asynchronised callback of a <see cref="CallBack"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class CallBackContent : Control
  {
  }

  #endregion

  /// <summary>
  /// Provides asynchronised callback functionality and partial rendering of page fragments without the need for full page postback.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The CallBack control consists of a container DOM element and the ability to refresh its content via AJAX callbacks. A client-side
  /// object with an ID corresponding to the server-side control is exposed, with a simple client-side API.
  /// </para>
  /// <para>
  /// The main part of the client-side API is the Callback method, which is used to initiate an AJAX request, send
  /// parameters to a server-side event handler and allow it to render new content to the output stream.
  /// </para>
  /// <para>
  /// In addition to the server-side <see cref="Callback" /> event, there is a number of client-side events
  /// which can be used to react to callback actions, such as <see cref="ClientSideOnCallbackComplete" /> and 
  /// <see cref="ClientSideOnCallbackError" />.
  /// </para>
  /// </remarks>
  [Designer(typeof(ComponentArt.Web.UI.CallBackDesigner))]
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:Callback Width=350 Height=250 runat=server></{0}:Callback>")]
  [ParseChildren(true)]
  [PersistChildren(false)]
  public sealed class CallBack : WebControl
  {
    #region Public Properties

    /// <summary>
    /// Whether to cache content. If set, callbacks will only be made the first time for
    /// each unique parameter used. Default: false.
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to cache content.")]
    public bool CacheContent
    {
      get 
      {
        object o = ViewState["CacheContent"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["CacheContent"] = value;
      }
    }

    /// <summary>
    /// Callback prefix to use instead of the computed one.
    /// </summary>
    [DefaultValue("")]
    [Category("Behavior")]
    [Description("Callback prefix to use instead of the computed one.")]
    public string CallbackPrefix
    {
      get
      {
        object o = ViewState["CallbackPrefix"];
        return o == null ? string.Empty : (string)o;
      }
      set
      {
        ViewState["CallbackPrefix"] = value;
      }
    }

    /// <summary>
    /// Whether to catch server-side errors. If set, exceptions occuring during callback are caught and sent to the
    /// client where the callbackError event is raised. Default: true.
    /// </summary>
    [DefaultValue(true)]
    [Description("Whether to catch server-side errors.")]
    public bool CatchServerErrors
    {
      get
      {
        object o = ViewState["CatchServerErrors"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["CatchServerErrors"] = value;
      }
    }

    /// <summary>
    /// Whether we are currently in a callback request that this control caused. Read-only.
    /// </summary>
    [Description("Whether we are currently in a callback request that this control caused.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    new public bool CausedCallback
    {
      get 
      {
        string [] arParams = GetCallbackParameters();

        return (arParams != null && arParams.Length > 0);
      }
    }

    private CallBackClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public CallBackClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new CallBackClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// Identifier of client-side function to handle the callback event.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Category("Behavior")]
    [Description("Identifier of client-side function to handle the callback event.")]
    public string ClientSideOnCallback
    {
      get
      {
        object o = ViewState["ClientSideOnCallback"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnCallback"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to call when a callback is complete.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Category("Behavior")]
    [Description("Identifier of client-side function to call when a callback is complete.")]
    public string ClientSideOnCallbackComplete
    {
      get
      {
        object o = ViewState["ClientSideOnCallbackComplete"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnCallbackComplete"] = value;
      }
    }

    /// <summary>
    /// Identifier of client-side function to handle a callback error.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    [Category("Behavior")]
    [Description("Identifier of client-side function to handle a callback error.")]
    public string ClientSideOnCallbackError
    {
      get
      {
        object o = ViewState["ClientSideOnCallbackError"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["ClientSideOnCallbackError"] = value;
      }
    }

    private ClientTemplate _loadingTemplate;
    /// <summary>
    /// Client template to use for feedback while waiting for a callback to complete.
    /// </summary>
    [Description("ID of client-template to use for feedback while waiting for a callback to complete.")]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ClientTemplate LoadingPanelClientTemplate
    {
      get
      {
        return _loadingTemplate;
      }
      set
      {
        _loadingTemplate = value;
      }
    }

    /// <summary>
    /// The duration of the fade effect when transitioning the loading template, in milliseconds. A value of 0
    /// (default) turns off the fade effect.
    /// </summary>
    [DefaultValue(0)]
    [Description("The duration of the fade effect when transitioning the loading template, in milliseconds.")]
    public int LoadingPanelFadeDuration
    {
      get
      {
        object o = ViewState["LoadingPanelFadeDuration"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["LoadingPanelFadeDuration"] = value;
      }
    }

    /// <summary>
    /// The maximum opacity percentage to fade to. Between 0 and 100. Default: 100.
    /// </summary>
    [DefaultValue(100)]
    [Description("The maximum opacity percentage to fade to.")]
    public int LoadingPanelFadeMaximumOpacity
    {
      get
      {
        object o = ViewState["LoadingPanelFadeMaximumOpacity"];
        return (o == null) ? 100 : (int)o;
      }
      set
      {
        ViewState["LoadingPanelFadeMaximumOpacity"] = value;
      }
    }

    [Browsable(false)]
    public override ControlCollection Controls
    {
      get
      {
        EnsureChildControls();
        return base.Controls;
      }
    }

    private CallBackContent _content;
    /// <summary>
    /// Initial content to render.
    /// </summary>
    [Description("Initial content to render.")]
    [DefaultValue(null)]
    [Browsable(false)]
    [NotifyParentProperty(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public CallBackContent Content
    {
      get
      {
        return _content;
      }
      set
      {
        _content = value;
      }
    }

    /// <summary>
    /// Whether to provide debug information at runtime.
    /// </summary>
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether to provide debug information at runtime.")]
    public bool Debug
    {
      get 
      {
        object o = ViewState["Debug"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["Debug"] = value;
      }
    }

    /// <summary>
    /// The last set parameter for this CallBack.
    /// </summary>
    [Category("Data")]
    [DefaultValue("")]
    [Description("The last set parameter for this CallBack.")]
    public string Parameter
    {
      get
      {
        object o = ViewState["Parameter"];
        return o == null? string.Empty : (string)o;
      }
      set
      {
        ViewState["Parameter"] = value;
      }
    }

    /// <summary>
    /// Whether to post the state of the form with callback requests.
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to post the state of the form with callback requests.")]
    public bool PostState
    {
      get 
      {
        object o = ViewState["PostState"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["PostState"] = value;
      }
    }

    /// <summary>
    /// The interval (in milliseconds) on which to refresh callback contents.
    /// </summary>
    [DefaultValue(0)]
    [Description("The interval (in milliseconds) on which to refresh callback contents.")]
    public int RefreshInterval
    {
      get 
      {
        object o = ViewState["RefreshInterval"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set 
      {
        ViewState["RefreshInterval"] = value;
      }
    }

    /// <summary>
    /// Whether to use the client-side page HREF as the prefix URL for callback requests.
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to use the client-side page HREF as the prefix URL for callback requests.")]
    public bool UseClientUrlAsPrefix
    {
      get
      {
        object o = ViewState["UseClientUrlAsPrefix"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["UseClientUrlAsPrefix"] = value;
      }
    }

    #endregion

    #region Methods

    protected override void OnInit(EventArgs args)
    {
      base.OnInit(args);

      if(Context == null || Page == null)
      {
        return;
      }

      if(Content != null)
      {
        this.Controls.Add(Content);
      }
    }
    
    protected override void OnLoad(EventArgs args)
    {
      base.OnLoad(args);

      if(Context.Request != null)
      {
        // get the param(s)
        string [] arParams = GetCallbackParameters();
        
        if(arParams != null && arParams.Length > 0)
        {
          this.Parameter = arParams[0];
        }
      }

      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Callback.client_scripts.A573P191.js");
      }

      if (!this.IsDownLevel())
      {
        // Tack this to the bottom of the page, to know when we've loaded.
        RegisterStartupScript("ComponentArt_Page_Loaded",
          "<script type=\"text/javascript\">\n//<![CDATA[\nwindow.ComponentArt_Page_Loaded = true;\n//]]>\n</script>");
      }
    }

    protected override void CreateChildControls()
    {
      if(Content != null)
      {
        this.Controls.Add(Content);
      }
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      string sControlId = this.GetSaneId();

      string [] arParams = GetCallbackParameters();
      
      // are we in design time?
      if (Context == null)
      {
        RenderDesignTime(output);
        return;
      }

      // is this a callback request?
      if(arParams != null && arParams.Length > 0)
      {
        this.HandleCallback(arParams);
        return;
      }

      // override writer
      output = new HtmlTextWriter(output, string.Empty);

      // write script
      if(Page != null)
      {
        // do we need to render scripts for non-Atlas?
        ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
        if (oScriptManager == null)
        {
          if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
          {
            Page.RegisterClientScriptBlock("A573G988.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
          } 
          if (!Page.IsClientScriptBlockRegistered("A573P191.js"))
          {
            Page.RegisterClientScriptBlock("A573P191.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Callback.client_scripts", "A573P191.js");
          }
        }
      }

      // output hidden param field
      output.AddAttribute("id", sControlId + "_ParamField");
      output.AddAttribute("name", sControlId + "_ParamField");
      output.AddAttribute("type", "hidden");
      output.AddAttribute("value", this.Parameter);
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      // Render frame

      output.Write("<div"); // begin <div>
      output.WriteAttribute("id", sControlId);

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
      output.Write("\">"); // end <div>
      
      // do we have a postback?
      if( this.IsDownLevel() &&
        Page != null &&
        Context != null &&
        Context.Request != null &&
        Context.Request.Form[sControlId + "_ParamField"] != null &&
        Context.Request.Form[sControlId + "_ParamField"] != "")
      {
        this.OnCallback(new CallBackEventArgs(output, Context.Request.Form[sControlId + "_ParamField"]));
      }
      else
      {
        if(Content != null)
        {
          Content.RenderControl(output);
        }
      }

      output.Write("</div>"); // </div>
 
      // write init
      StringBuilder oSB = new StringBuilder();
      oSB.Append("/*** ComponentArt.Web.UI.CallBack ").Append(this.VersionString()).Append(" ").Append(sControlId).Append(" ***/\n");

      oSB.Append("window.ComponentArt_Init_" + sControlId + " = function() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      oSB.Append("if(!window.ComponentArt_CallBack_Loaded)\n");
      oSB.Append("\t{setTimeout('ComponentArt_Init_" + sControlId + "()', 50); return; }\n\n");

      oSB.AppendFormat("window.{0} = new ComponentArt_CallBack('{0}');\n", sControlId);

      // Hook the actual ID if available and different from effective client ID
      if (this.ID != sControlId)
      {
        oSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sControlId + "; " + sControlId + ".GlobalAlias = '" + ID + "'; }\n");
      }

      // write properties
      oSB.Append(sControlId + ".CallbackPrefix = unescape('" + HttpUtility.UrlEncode(this.CallbackPrefix != "" ? this.CallbackPrefix : Utils.GetResponseUrl(Context)) + "');\n");
      oSB.Append(sControlId + ".CallbackParamDelimiter = '" + (Context.Request.QueryString.Count > 0? "&" : "?") + "';\n");
      if(this.CacheContent) oSB.Append(sControlId + ".Cache = new Object();\n");
      oSB.Append(sControlId + ".ClientEvents = " + Utils.ConvertClientEventsToJsObject(this._clientEvents) + ";\n");
      if (this.ClientSideOnCallback != string.Empty) oSB.Append(sControlId + ".ClientSideOnCallback = " + ClientSideOnCallback + ";\n");
      if(this.ClientSideOnCallbackComplete != string.Empty) oSB.Append(sControlId + ".ClientSideOnCallbackComplete = " + ClientSideOnCallbackComplete + ";\n");
      if(this.ClientSideOnCallbackError != string.Empty) oSB.Append(sControlId + ".ClientSideOnCallbackError = " + ClientSideOnCallbackError + ";\n");
      if(this.Debug) oSB.Append(sControlId + ".Debug = 1;\n");
      if(this.LoadingPanelClientTemplate != null) oSB.Append(sControlId + ".LoadingPanelClientTemplate = '" + LoadingPanelClientTemplate.Text.Replace("\n", "").Replace("\r", "").Replace("'", "\\'") + "';\n");
      oSB.Append(sControlId + ".LoadingPanelFadeDuration = " + LoadingPanelFadeDuration + ";\n");
      oSB.Append(sControlId + ".LoadingPanelFadeMaximumOpacity = " + LoadingPanelFadeMaximumOpacity + ";\n");
      if (this.IsDownLevel()) oSB.Append(sControlId + ".IsDownLevel = 1;\n");
      oSB.Append(sControlId + ".Parameter = '" + this.Parameter + "';\n");
      oSB.Append(sControlId + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
      if(this.PostState) oSB.Append(sControlId + ".PostState = 1;\n");
      if (this.UseClientUrlAsPrefix) oSB.Append(sControlId + ".UseClientUrlAsPrefix = 1;\n");
      oSB.Append(sControlId + ".Initialize();\n");
      oSB.Append("\n}\n");

      // Initiate Callback creation
      oSB.Append("ComponentArt_Init_" + sControlId + "();\n");
      
      // Do we have a refresh interval?
      if(RefreshInterval > 0)
      {
        oSB.Append(string.Format("setInterval('{0}.Callback({0}.Parameter)', {1});\n", sControlId, RefreshInterval));
      }

      output.Write(this.DemarcateClientScript(oSB.ToString()));
    }
      
    protected override bool IsDownLevel()
    {
      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        return true;
      }

      if(this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }

      if(Context == null || Page == null)
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
      catch {}

      if(sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion < 8)
      {
        return true;
      }
      else if( // We are good if:

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

    internal void RenderDesignTime(HtmlTextWriter output)
    {
      // Sanify writer
      output = new HtmlTextWriter(output);
      
      // Render the outer frame
      AddAttributesToRender(output);
      output.RenderBeginTag(HtmlTextWriterTag.Div);

      this.RenderContents(output);
      
      output.RenderEndTag();
    }

    private string [] GetCallbackParameters()
    {
      string sCallbackParamVar = string.Format("Cart_{0}_Callback_Param", this.GetSaneId());
      string [] arParams = null;

      if(Context != null)
      {
        arParams = Context.Request.Params.GetValues(sCallbackParamVar);
      }
      
      return arParams;
    }

    private void HandleCallback(string[] arParams)
    {
      try
      {
        StringWriter oStringWriter = new StringWriter();
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter, string.Empty);

        CallBackEventArgs oArgs = new CallBackEventArgs(oWriter);
        oArgs.Parameter = arParams[0];
        oArgs.Parameters = arParams;

        this.OnCallback(oArgs);

        oWriter.Close();

        Context.Response.Clear();
        Context.Response.ContentType = "text/xml";
        Context.Response.Write("<CallbackContent><![CDATA[");
        Context.Response.Write(oStringWriter.ToString().Replace("]]>", "$$$CART_CDATA_CLOSE$$$"));
        Context.Response.Write("]]></CallbackContent>");
      }
      catch (Exception ex)
      {
        if (this.CatchServerErrors)
        {
          this.HandleCallbackError(ex);
        }
        else
        {
          throw ex;
        }
      }

      try
      {
        Context.Response.End();
      }
      catch { }
    }

    private void HandleCallbackError(Exception ex)
    {
      // don't output child tables
      Context.Response.Clear();
      Context.Response.ContentType = "text/xml";
      Context.Response.Write("<CallbackError><![CDATA[");
      Context.Response.Write(ex.Message);
      Context.Response.Write("]]></CallbackError>");
      Context.Response.End();
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate for <see cref="Callback"/> event of <see cref="CallBack"/> class.
    /// </summary>
    public delegate void CallbackEventHandler(object sender, CallBackEventArgs e);

    /// <summary>
    /// CallbackCallbackEvent event.
    /// </summary>
    [ Description("Fires when a callback is made."), 
    Category("Callback Events") ]
    public event CallbackEventHandler Callback;

    private void OnCallback(CallBackEventArgs e) 
    {         
      if (Callback != null) 
      {
        Callback(this, e);
      }   
    }

    #endregion
  }
}
