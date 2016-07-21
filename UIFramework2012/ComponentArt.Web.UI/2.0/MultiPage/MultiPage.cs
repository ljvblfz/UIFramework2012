using System;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Runtime.InteropServices;



namespace ComponentArt.Web.UI
{
  #region EventArgs classes
  
  /// <summary>
  /// Arguments for <see cref="MultiPage.PageSelected"/> server-side event of <see cref="MultiPage"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class MultiPageSelectEventArgs : EventArgs
  {
    /// <summary>
    /// The previous SelectedIndex.
    /// </summary>
    public int PreviousIndex;
  }

#endregion

	/// <summary>
  /// Manages a collection of page fragments, all occupying the same section of the page, only one of them visible at a time.
	/// </summary>
  /// <remarks>
  /// <para>
  /// The MultiPage control manages <see cref="PageViews">sections</see> of content layered on top of each other, with only one section visible
  /// at any one time. A MultiPage contains a number of <see cref="PageView" /> controls, each containing ASP.NET
  /// content to be rendered for one visible section.
  /// </para>
  /// <para>
  /// By default, the PageViews are rendered all at once, and their visibility is manipulated on the client. Alternatively
  /// by setting <see cref="AutoPostBack" /> and <see cref="RenderSelectedPageOnly" /> to true, content will be changed
  /// via postback.
  /// </para>
  /// </remarks>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [Designer(typeof(ComponentArt.Web.UI.MultiPageDesigner))]
  [ParseChildren(true, "PageViews")]
  [ToolboxData("<{0}:MultiPage runat=server></{0}:MultiPage>")]
	public class MultiPage : WebControl, IPostBackEventHandler
	{
    #region Public Properties

    /// <summary>
    /// Whether to perform a postback when a page is selected on client. Default: false.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to perform a postback when a page is selected on client. Default: false.")]
    [DefaultValue(false)]
    public bool AutoPostBack
    {
      get 
      {
        object o = ViewState["AutoPostBack"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["AutoPostBack"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for page transitions.
    /// </summary>
    /// <seealso cref="TransitionCustomFilter" />
    [Description("The transition effect to use for page transitions.")]
    [DefaultValue(TransitionType.None)]
    [Category("Animation")]
    public TransitionType Transition
    {
      get
      {
        return Utils.ParseTransitionType(ViewState["Transition"]);
      }
      set
      {
        ViewState["Transition"] = value.ToString();
      }
    }

    /// <summary>
    /// The string defining a custom transition filter to use for page transitions.
    /// </summary>
    /// <seealso cref="Transition" />
    [Description("The string defining a custom transition filter to use for page transitions.")]
    [DefaultValue("")]
    public string TransitionCustomFilter
    {
      get
      {
        string s = (string)ViewState["TransitionCustomFilter"];
        return s == null ? string.Empty : s;
      }
      set
      {
        ViewState["TransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// The duration of the page transitions.
    /// </summary>
    /// <seealso cref="Transition" />
    [Description("The duration of the page transitions.")]
    [DefaultValue(0)]
    public int TransitionDuration
    {
      get
      {
        object o = ViewState["TransitionDuration"];
        return o == null ? 0 : (int)o;
      }
      set
      {
        ViewState["TransitionDuration"] = value;
      }
    }

    /// <summary>
    /// Creates a new collection of child controls for the current control.
    /// </summary>
    /// <returns>A PageViewCollection object that contains the currents control's children.</returns>
    protected override ControlCollection CreateControlCollection()
    {
      return new PageViewCollection(this);
    }

    /// <summary>
    /// The collection of PageView objects which belong to this MultiPage.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public PageViewCollection PageViews
    {
      get
      {
        return (PageViewCollection)this.Controls;
      }
    }

    private int _cachedIndex = -1;
    /// <summary>
    /// Index of the currently selected page.
    /// </summary>
    public int SelectedIndex
    {
      get
      {
        // If there are no controls, return -1.
        if(Controls.Count == 0)
        {
          return -1;
        }

        // Default index is 0.
        object obj = ViewState["SelectedIndex"];
        if(obj == null)
        {
          if(_cachedIndex >= 0)
          {
            return _cachedIndex;
          }
          else
          {
            return 0;
          }
        }
        else if((int)obj >= Controls.Count)
        {
          return 0;
        }

        return (int)obj;
      }

      set
      {
        if(this.Controls.Count == 0)
        {
          _cachedIndex = value;
        }
        else if ((value >= 0) && (value < Controls.Count))
        {
          ViewState["SelectedIndex"] = value;
        }
        else
        {
          throw new ArgumentOutOfRangeException();
        }
      }
    }

    /// <summary>
    /// Whether to only output the currently selected page. In conjunction with AutoPostBack, this is used to
    /// enable load-on-demand functionality. Default: false.
    /// </summary>
    /// <seealso cref="AutoPostBack" />
    [Category("Behavior")]
    [Description("Whether to only output the currently selected page. Default: false.")]
    [DefaultValue(false)]
    public bool RenderSelectedPageOnly
    {
      get 
      {
        object o = ViewState["RenderSelectedPageOnly"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["RenderSelectedPageOnly"] = value;
      }
    }

    #endregion

    #region Public Methods
    
		public MultiPage()
		{
		}

    /// <summary>
    /// Select the first PageView in this MultiPage.
    /// </summary>
    public void GoFirst()
    {
      this.SelectedIndex = 0;
    }

    /// <summary>
    /// Select the last PageView in this MultiPage.
    /// </summary>
    public void GoLast()
    {
      this.SelectedIndex = this.Controls.Count - 1;
    }

    /// <summary>
    /// Select the next PageView in this MultiPage, if there is one.
    /// </summary>
    public void GoNext()
    {
      GoNext(false);
    }

    /// <summary>
    /// Select the next PageView in this MultiPage.
    /// </summary>
    /// <param name="bWrap">Whether to wrap back to the first page after the last one.</param>
    public void GoNext(bool bWrap)
    {
      if(SelectedIndex >= this.Controls.Count - 1)
      {
        if(bWrap)
        {
          GoFirst();
        }
      }
      else
      {
        SelectedIndex++;
      }
    }

    /// <summary>
    /// Select the previous PageView in this MultiPage, if there is one.
    /// </summary>
    public void GoPrevious()
    {
      GoPrevious(false);
    }

    /// <summary>
    /// Select the previous PageView in this MultiPage.
    /// </summary>
    /// <param name="bWrap">Whether to wrap back to the last page after the first one.</param>
    public void GoPrevious(bool bWrap)
    {
      if(SelectedIndex <= 0)
      {
        if(bWrap)
        {
          GoLast();
        }
      }
      else
      {
        SelectedIndex--;
      }
    }

  /// <summary>
  /// Handle a postback raised by the MultiPage.
  /// </summary>
  /// <param name="stringArgument">Postback argument</param>
    public void RaisePostBackEvent(string stringArgument)
    {
      int iIndex = int.Parse(stringArgument);

      MultiPageSelectEventArgs oArgs = new MultiPageSelectEventArgs();
      oArgs.PreviousIndex = this.SelectedIndex;

      this.SelectedIndex = iIndex;

      this.OnPageSelected(oArgs);
    }

    /// <summary>
    /// Select the PageView with the given ID.
    /// </summary>
    /// <param name="sId">ID of desired PageView</param>
    public void SelectPageById(string sId)
    {
      int iIndex = 0;
      foreach(PageView oPageView in this.Controls)
      {
        if(oPageView.ID == sId)
        {
          this.SelectedIndex = iIndex;
          return;
        }

        iIndex++;
      }
    }

    #endregion
    
    #region Protected Methods

    /// <summary>
    /// Overridden. Filters out all objects except PageView objects.
    /// </summary>
    /// <param name="obj">The parsed element.</param>
    protected override void AddParsedSubObject(object obj)
    {
      if (obj is PageView)
      {
        base.AddParsedSubObject(obj);
      }
    }

    protected override void LoadViewState(object obj)
    {
      base.LoadViewState(obj);

      string sMultiPageVarName = this.GetSaneId();

      if(Context != null)
      {
        // Maintain any changes to SelectedIndex
        string sIndex = Context.Request.Form[sMultiPageVarName + "_SelectedIndex"];
        if(sIndex != null && sIndex != string.Empty)
        {
          this.SelectedIndex = int.Parse(sIndex);
          //Context.Response.Write("Assigned index: " + sIndex);
        }
      }
    }

    protected override object SaveViewState()
    {
      // dummy
      if(EnableViewState)
      {
        ViewState["EnsureViewState"] = true;
      }

      return base.SaveViewState();
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      if(this.IsDownLevel())
      {
        RenderDownLevel(output);
        return;
      }

      if(Page != null)
      {
        // Add core code
        if(!Page.IsClientScriptBlockRegistered("A573G988.js"))
        {
          Page.RegisterClientScriptBlock("A573G988.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
        }
        if(!Page.IsClientScriptBlockRegistered("A573A488.js"))
        {
          Page.RegisterClientScriptBlock("A573A488.js", "");
          WriteGlobalClientScript(output, "ComponentArt.Web.UI.MultiPage.client_scripts", "A573A488.js");
        }
      }

      output = new HtmlTextWriter(output, string.Empty);

      string sMultiPageVarName = this.GetSaneId();

      if(EnableViewState)
      {
        // Render selected index maintaining hidden field.
        output.AddAttribute("id", sMultiPageVarName + "_SelectedIndex");
        output.AddAttribute("name", sMultiPageVarName + "_SelectedIndex");
        output.AddAttribute("type", "hidden");
        output.AddAttribute("value", this.SelectedIndex.ToString());
        output.RenderBeginTag(HtmlTextWriterTag.Input);
        output.RenderEndTag();
      }

      // Render the content
      // Render frame

      output.Write("<table"); // begin <table>
      output.WriteAttribute("id", sMultiPageVarName);
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

      // begin other stuff

      output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>

      output.AddStyleAttribute("height", "100%");
      output.AddStyleAttribute("width", "100%");
      output.AddStyleAttribute("vertical-align", "top");
      output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

      foreach (PageView oPageView in this.Controls)
      {
        oPageView.Style["height"] = "100%";
        oPageView.Style["width"] = "100%";
        if (this.ClientTarget != ClientTargetLevel.Accessible)
        {
          oPageView.Style["display"] = "none";
        }
        oPageView.RenderControl(output);
      }

      output.RenderEndTag(); // </td>
      output.RenderEndTag(); // </tr>

      output.Write("</table>"); // </table>

      // End content render

      // Render client-side object initiation.
      StringBuilder oStartupSB = new StringBuilder();
      oStartupSB.Append("/*** ComponentArt.Web.UI.MultiPage ").Append(this.VersionString()).Append(" ").Append(sMultiPageVarName).Append(" ***/\n");
      oStartupSB.Append("function ComponentArt_Init_" + sMultiPageVarName + "() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      oStartupSB.Append("if(!window.ComponentArt_MultiPage_Kernel_Loaded)\n");
      oStartupSB.Append("\t{setTimeout('ComponentArt_Init_" + sMultiPageVarName + "()', 100); return; }\n\n");

      // Instantiate object
      oStartupSB.Append("window." + sMultiPageVarName + " = new ComponentArt_MultiPage('" + sMultiPageVarName + "');\n");

      // Hook the actual ID if available and different from effective client ID
      if (this.ID != sMultiPageVarName)
      {
        oStartupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sMultiPageVarName + "; " + sMultiPageVarName + ".GlobalAlias = '" + ID + "'; }\n");
      }

      oStartupSB.Append(sMultiPageVarName + ".ControlId = '" + this.UniqueID + "';\n");
      if(AutoPostBack) oStartupSB.Append(sMultiPageVarName + ".AutoPostBack = true;\n");
      oStartupSB.Append(sMultiPageVarName + ".Transition = " + ((int)this.Transition).ToString() + ";\n");
      if (TransitionCustomFilter != "") oStartupSB.Append(sMultiPageVarName + ".TransitionCustomFilter = '" + TransitionCustomFilter + "';\n");
      oStartupSB.Append(sMultiPageVarName + ".TransitionDuration = " + TransitionDuration + ";\n");
        
      // Add PageViews to the client-side MultiPage
      foreach(PageView oPageView in this.Controls)
      {
        oStartupSB.Append(sMultiPageVarName + ".AddPage(new ComponentArt_PageView('" + oPageView.ClientID + "','" + oPageView.ID + "'));\n");
        if(!oPageView.Enabled) oStartupSB.Append(sMultiPageVarName + ".DisablePage('" + oPageView.ClientID + "');\n");
      }

      if(this.SelectedIndex >= 0)
      {
        oStartupSB.Append(sMultiPageVarName + ".SetPageIndex(" + this.SelectedIndex + ",true);\n");
      }

      oStartupSB.Append("\nwindow." + sMultiPageVarName + "_loaded = true;\n}\n");
        
      // Initiate MultiPage creation
      oStartupSB.Append("ComponentArt_Init_" + sMultiPageVarName + "();");
          
      WriteStartupScript(output, this.DemarcateClientScript(oStartupSB.ToString()));
    }

    internal void RenderDesignTime(HtmlTextWriter output)
    {
      // Sanify writer
      output = new HtmlTextWriter(output);
      
      // Render the outer frame
      AddAttributesToRender(output);
      output.RenderBeginTag(HtmlTextWriterTag.Div);

      // Render the selected inner frame, if any
      if(this.SelectedIndex >= 0)
      {
        PageView oPageView = (PageView)Controls[this.SelectedIndex];
        
        Utils.WriteOpenDiv(oPageView, output);
        oPageView.RenderControl(output);
        output.Write("</div>");
      }

      output.RenderEndTag();
    }

    private void RenderDownLevel(HtmlTextWriter output)
    {
      // Sanify writer
      output = new HtmlTextWriter(output);
      
      // Render the outer frame
      AddAttributesToRender(output);
      output.RenderBeginTag(HtmlTextWriterTag.Div);

      // Render the selected inner frame, if any
      if(this.SelectedIndex >= 0)
      {
        PageView oPageView = (PageView)Controls[this.SelectedIndex];
        
        Utils.WriteOpenDiv(oPageView, output);
        oPageView.RenderControl(output);
        output.Write("</div>");
      }

      output.RenderEndTag();
    }

    protected override bool IsDownLevel()
    {
      if(this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }
      
      if(Context == null)
      {
        return true;
      }

      string sUserAgent = Context.Request.UserAgent;

      if(sUserAgent == null)
      {
        return true;
      }

      int iMajorVersion = 0;
      
      try
      {
        iMajorVersion = Context.Request.Browser.MajorVersion;
      }
      catch {}

      if( // We are good if:

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
      base.OnLoad (e);
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate for <see cref="PageSelected"/> event of <see cref="MultiPage"/> class.
    /// </summary>
    public delegate void PageSelectedEventHandler(object sender, MultiPageSelectEventArgs e);

    /// <summary>
    /// Fires after a PageView is selected.
    /// </summary>
    [ Description("Fires after a PageView is selected."),
    Category("MultiPage Events") ]
    public event PageSelectedEventHandler PageSelected;

    private void OnPageSelected(MultiPageSelectEventArgs e) 
    {         
      if (PageSelected != null) 
      {
        PageSelected(this, e);
      }   
    }

    #endregion
	}
}
