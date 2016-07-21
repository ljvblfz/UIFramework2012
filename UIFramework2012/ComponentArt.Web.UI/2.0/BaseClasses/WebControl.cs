using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Globalization;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Serves as the base class that defines the features common to all ComponentArt web controls. 
  /// </summary>
  /// <remarks>
  /// Provides licensing, client-script rendering, and search engine stamp rendering services. 
  /// All ComponentArt server controls inherit from this class. 
  /// </remarks>
  public abstract class WebControl : System.Web.UI.WebControls.WebControl
	{
    internal static string _scriptHandlerControls = null;
    
    #region Public Properties 

    /// <summary>
    /// Whether we are currently in a callback request that this control caused. Read-only.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is similar to the <see cref="WebControl.IsCallback" /> property, except that this property
    /// is only true if the control itself caused the callback.
    /// </para>
    /// </remarks>
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
    /// ID of the client-side object corresponding to this control.
    /// </summary>
    /// <value>The name of the JavaScript global variable representing this control's client-side object.</value>
    /// <remarks>ClientObjectId is often the same as ClientID. However this cannot always be the case, 
    /// because not all ClientID values are valid JavaScript variable names.</remarks>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [Description("ID of the client-side object corresponding to this control.")]
    public string ClientObjectId
    {
      get
      {
        return this.GetSaneId();
      }
    }

    /// <summary>
    /// Relative or absolute path to the folder containing the client-side script file(s).
    /// </summary>
    /// <remarks>
    /// The actual JS files are placed in a folder named to correspond to the version of Web.UI being used, inside a folder named
    /// <b>componentart_webui_client</b>, which is placed in the folder specified by this property.
    /// </remarks>
    [Category("Support")]
    [DefaultValue("")]
    [Description("Relative or absolute path to the folder containing the client-side script file(s).")]
    public string ClientScriptLocation
    {
      get
      {
        object o = ViewState["ClientScriptLocation"]; 
        return (o == null) ? String.Empty : Utils.ConvertUrl(Context, string.Empty, (string)o); 
      }

      set
      {
        ViewState["ClientScriptLocation"] = value; 
      }
    }
    
    /// <summary>
    /// Specifies the level of client-side content that the control renders.
    /// </summary>
    /// <value>
    /// Gets or sets a value that allows you to override automatic detection of browser capabilities and to specify 
    /// how the control renders.  Default is Auto, indicating that the control will decide based on the client's 
    /// browser whether it is appropriate to serve it uplevel or downlevel content.
    /// </value>
    [DefaultValue(ClientTargetLevel.Auto)]
    [Description("Specifies the level of client-side content that the control renders.")]
    public ClientTargetLevel ClientTarget
    {
      get
      {
        return Utils.ParseClientTargetLevel(ViewState["ClientTarget"], ClientTargetLevel.Auto);
      }
      set
      {
        ViewState["ClientTarget"] = value;
      }
    }

    /// <summary>
    /// Whether this control considers the current browser accessible.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public bool IsAccessibleBrowser
    {
      get
      {
        if (Context != null)
        {
          return this.IsAccessible();
        }
        else
          return false;
      }
    }

    private bool _isCallbackProcessed = false;
    private bool _isCallback = false;
    /// <summary>
    /// Whether this page request is a callback request by a ComponentArt control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is similar to the <see cref="WebControl.CausedCallback" /> property, except that this property
    /// is true for any callback, regardless of which control initiated it.
    /// </para>
    /// </remarks>
    [Description("Whether we are currently in a callback request.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public bool IsCallback
    {
      get
      {
        if (!_isCallbackProcessed)
        {
          if (Context != null && Context.Request != null)
          {
            foreach (string key in Context.Request.Params.AllKeys)
            {
              if (key != null && key.StartsWith("Cart_") && key.IndexOf("_Callback") > 0)
              {
                _isCallback = true;
                _isCallbackProcessed = true;
                break;
              }
            }
          }

          _isCallbackProcessed = true;
        }

        return _isCallback;
      }
    }

    /// <summary>
    /// Whether this control considers the current browser down-level.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public bool IsDownLevelBrowser
    {
      get
      {
        if(Context != null)
        {
          return this.IsDownLevel();
        }
        else
          return false;
      }
    }

    /// <summary>
    /// Whether to render the search engine stamp.
    /// </summary>
    [Category("Support")]
    [DefaultValue(true)]
    [Description("Whether to render the search engine stamp.")]
    public bool RenderSearchEngineStamp
    {
      get 
      {
        object o = ViewState["RenderSearchEngineStamp"]; 
        return (o == null) ? true : (bool) o; 
      }
      set 
      {
        ViewState["RenderSearchEngineStamp"] = value;
      }
    }

    #endregion 

    #region Public Methods 

    /// <summary>
    /// IsLicensed method.
    /// </summary>
    /// <returns>Whether this control is licensed.</returns>
    public virtual bool IsLicensed()
    {
      if (License != null) return true; 
      try
      {
        License = LicenseManager.Validate(this.GetType(), this);
        return true; 
      }
      catch
      {
        return false; 
      }
    }

    #endregion 

    #region Internal Properties 

    internal System.ComponentModel.License License = null;

    #endregion 

    #region Protected Methods

    internal string GetSaneId()
    {
      if (this.UniqueID == null || this.UniqueID == "")
      {
        throw new Exception("An ID must be defined on the control.");
      }
      else
      {
        return this.UniqueID.Replace("$", "_").Replace("{", "_").Replace("}", "_").Replace(":", "_");
      }
    }

    protected bool IsBrowserSearchEngine()
    {
      if(Context == null || Context.Request == null)
        return false;

      string sUserAgent = Context.Request.UserAgent;
      if(sUserAgent == null)
      {
        return false;
      }

      return (
        sUserAgent.IndexOf("Googlebot") >= 0 ||
        sUserAgent.IndexOf("SideWinder") >= 0 ||
        sUserAgent.IndexOf("inktomi") >= 0 ||
        sUserAgent.IndexOf("ZyBorg") >= 0 ||
        sUserAgent.IndexOf("FAST-WebCrawler") >= 0 ||
        sUserAgent.IndexOf("Lycos") >= 0 ||
        sUserAgent.IndexOf("Scooter") >= 0 ||
        sUserAgent.IndexOf("NPBot") >= 0 ||
        sUserAgent.IndexOf("Gigabot") >= 0 ||
        sUserAgent.IndexOf("MSNBOT") >= 0 ||
        sUserAgent.IndexOf("SearchSpider") >= 0 ||
        sUserAgent.IndexOf("Vagabondo") >= 0 ||
        sUserAgent.IndexOf("almaden") >= 0 ||
        sUserAgent.IndexOf("CipinetBot") >= 0 ||
        sUserAgent.IndexOf("QuepasaCreep") >= 0 ||
        sUserAgent.IndexOf("Gaisbot") >= 0 ||
        sUserAgent.IndexOf("DoCoMo") >= 0 ||
        sUserAgent.IndexOf("grub-client") >= 0 ||
        sUserAgent.IndexOf("Openbot") >= 0 ||
        sUserAgent.IndexOf("Ask Jeeves") >= 0 ||
        sUserAgent.IndexOf("Girafabot") >= 0
        );
    }

    // Whether the control is running in design mode 
    protected bool IsRunningInDesignMode()
    { 
      return HttpContext.Current == null;
    }

    protected virtual bool IsAccessible()
    {
      return (Context != null && 
              Context.Request != null && 
              Context.Request.UserAgent != null && 
              Context.Request.UserAgent.ToLower(CultureInfo.InvariantCulture).IndexOf("lynx") >= 0);
    }

    protected abstract bool IsDownLevel();

    protected virtual void ComponentArtPreRender(EventArgs e)
    {
    }
    
    protected abstract void ComponentArtRender(HtmlTextWriter output); 
    
    protected string GetResourceContent(string sFileName)
    {
      return Utils.GetResourceContent(sFileName);
    }

    protected override void OnInit(EventArgs oArgs)
    {
      base.OnInit(oArgs);

      string sScriptControlsKey = "ComponentArtScriptControls";

      // Should the ScriptHandler be used? If so, call off any subsequent script registrations.
      _scriptHandlerControls = ConfigurationManager.AppSettings[sScriptControlsKey];
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);




      // register scripts with our own handler, if needed
      if (_scriptHandlerControls != null)
      {
        string sPath = "";

        if (Context != null)
        {
          sPath = Context.Request.ApplicationPath;
          if(!sPath.EndsWith("/")) sPath += "/";
        }

        if (_scriptHandlerControls.ToLower() == "percontrol")
        {
          string sControlName = this.GetType().ToString().Replace("ComponentArt.Web.UI.", "");

          if (ScriptGenerator.IsScriptControl(sControlName))
          {
            string sUtilScriptReference = sPath + "ComponentArtScript.axd?f=" + ScriptGenerator.GetUtilsScriptReference() + "&amp;v=" + this.VersionString(true);
            string sControlScriptReference = sPath + "ComponentArtScript.axd?f=" + ScriptGenerator.GetScriptReference(sControlName, true) + "&amp;v=" + this.VersionString(true);

              if (!Page.IsClientScriptBlockRegistered("ComponentArtScripts"))
              {
                Page.RegisterClientScriptBlock("ComponentArtScript_Utils", "<script src=\"" + sUtilScriptReference + "\" type=\"text/javascript\"></script>");
                Page.RegisterClientScriptBlock("ComponentArtScript_" + sControlName, "<script src=\"" + sControlScriptReference + "\" type=\"text/javascript\"></script>");
              }
          }
        }
        else
        {
          string sScriptReference = sPath + "ComponentArtScript.axd?f=" +
            (_scriptHandlerControls.ToLower() == "all" ? ScriptGenerator.GetFullScriptReference() :
              ScriptGenerator.GetScriptReference(_scriptHandlerControls.Split(',')));

          // add version-dependent stamp
          sScriptReference += "&amp;v=" + this.VersionString(true);

            if (!Page.IsClientScriptBlockRegistered("ComponentArtScripts"))
            {
              Page.RegisterClientScriptBlock("ComponentArtScripts", "<script src=\"" + sScriptReference + "\" type=\"text/javascript\"></script>");
            }
        }
      }
    }

    

    protected void RenderCrawlerStamp(HtmlTextWriter output)
    {
      Assembly thisAssembly = Assembly.GetExecutingAssembly();
      string thisAssemblyVersion = thisAssembly.GetName().Version.ToString();
      string thisProductName = this.GetType().ToString().Replace("ComponentArt.Web.UI.", ""); 
      
      output.RenderBeginTag(HtmlTextWriterTag.Noscript);

      output.Write("ComponentArtSCStamp " + thisProductName + " " + thisAssemblyVersion);

      output.RenderEndTag(); // </noscript>
    }
    
    

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
      if (IsLicensed()) ComponentArtPreRender(e); 
    }
    
    protected override void Render(HtmlTextWriter output)
    {
      // Render demo warning if required 
      if (!IsLicensed())
      {
        RenderRedistributableWarning(output); 
        return; 
      }
      
      // Render control 
      ComponentArtRender(output); 

      if(this.IsBrowserSearchEngine() && this.RenderSearchEngineStamp)
      {
        RenderCrawlerStamp(output);
      }
    }
    
    protected void RenderRedistributableWarning(HtmlTextWriter output)
    {
      output.Write("<div style=\"background-color:#3F3F3F;border:1px;border-style:solid;border-bottom-color:black;border-right-color:black;border-left-color:lightslategray;border-top-color:lightslategray;color:cornsilk;padding:2px;font-family:verdana;font-size:11px;\">");
      string productName = this.GetType().ToString().Replace("ComponentArt.Web.UI.", ""); 

      output.Write("<b>ComponentArt " + productName + "</b> :: ");
      output.Write("Unlicensed version. <br><br>"); 
      output.Write("This version is licensed for single application use only. <br><br>"); 
      output.Write("You can download the free trial version <a href=http://www.ComponentArt.com/ target=_blank><font color=silver>here</font></a>."); 
      output.Write("</div>"); 
    }
    
       
    protected void WriteGlobalClientScript(HtmlTextWriter output, string sDefaultPath, string sScriptFile)
    {
      if (_scriptHandlerControls != null)
      {
        // do nothing, we're using our own script handler
        return;
      }

      string sScript = GenerateClientScriptBlock(sDefaultPath, sScriptFile);
      output.Write(sScript);
    }

    #endregion 

    #region Private Members

    


		private string _versionString = null;
    private string _versionStringWithRevision = null;
    internal string VersionString()
    {
      return VersionString(true);
    }
		internal string VersionString(bool includeRevisionNumber)
		{
		  if (_versionString == null || _versionStringWithRevision == null)
		  {
        Version version = Assembly.GetExecutingAssembly().GetName().Version;
			  _versionString =  version.Major.ToString() + "_" +
				                  version.Minor.ToString() + "_" +
				                  version.Build.ToString();
        _versionStringWithRevision = _versionString + "_" + 
                                     version.Revision.ToString();
		  }
		  return includeRevisionNumber ? _versionStringWithRevision : _versionString;
		}

    private string GenerateClientScriptBlock(string sDefaultPath, string sScriptFile)
    {
      string sScript = string.Empty;
      string sScriptLocation = string.Empty;

      if (this.ClientScriptLocation != string.Empty)
      {
        sScriptLocation = Path.Combine(Path.Combine(this.ClientScriptLocation, this.VersionString(false)), sScriptFile).Replace("\\", "/");
			}

      if(sScriptLocation != string.Empty)
      {
        // Do we have a tilde?
        if(sScriptLocation.StartsWith("~") && Context != null && Context.Request != null)
        {
          string sAppPath = Context.Request.ApplicationPath;
          if(sAppPath.EndsWith("/"))
          {
            sAppPath = sAppPath.Substring(0, sAppPath.Length - 1);
          }

          sScriptLocation = sScriptLocation.Replace("~", sAppPath);
        }

        if(File.Exists(Context.Server.MapPath(sScriptLocation)))
        {
          sScript = "<script src=\"" + sScriptLocation + "\" type=\"text/javascript\"></script>";
        }
        else
        {
          throw new Exception(sScriptLocation + " not found");
        }
      }
      else 
      {
        // If everything failed, emit our internal script
        string sResourceUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), sDefaultPath + "." + sScriptFile);
        sResourceUrl = sResourceUrl.Replace("&", "&amp;");
        sScript = "<script src=\"" + sResourceUrl + "\" type=\"text/javascript\"></script>";

      }

      return sScript;
    }


    protected void RegisterStartupScript(string sKey, string sScript)
    {

      if (!Page.IsClientScriptBlockRegistered(sKey))
      {
        Page.RegisterStartupScript(sKey, sScript);
      }
    }

    internal string DemarcateClientScript(string script)
    {
      return DemarcateClientScript(script, null);
    }
    internal string DemarcateClientScript(string script, string title)
    {
      StringBuilder result = new StringBuilder();
      bool includeCDATA = true;
      result.Append("<script type=\"text/javascript\">\n");
      result.Append(includeCDATA ? "//<![CDATA[\n" : null);
      result.Append(title != null ? "/*** " + title + " ***/\n" : null);
      result.Append(script);
      result.Append("\n");
      result.Append(includeCDATA ? "//]]>\n" : null);
      result.Append("</script>\n");
      return result.ToString();
    }

    protected void WriteStartupScript(HtmlTextWriter output, string sScript)
    {
        output.Write(sScript);
    }

    #endregion 
	}
	
  /// <summary>
  /// Specifies the level of client-side content that the control renders.
  /// </summary>
  public enum ClientTargetLevel
  {
    /// <summary>Automatically detect whether the browser should be served uplevel or downlevel content.</summary>
    Auto,

    /// <summary>Serve downlevel content, typically appropriate for older and less advanced browsers.</summary>
    Downlevel,
    
    /// <summary>Serve uplevel content, typically appropriate for newer and more advanced browsers.</summary>
    Uplevel,

    /// <summary>Generate output compliant with Section 508, accessible for users with disabilities.</summary>
    Accessible
  }

}
