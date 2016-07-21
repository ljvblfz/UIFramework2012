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
using System.Globalization;

namespace ComponentArt.Web.UI
{
  #region Enumerations

  /// <summary> 
  /// Specifies the orientation to use for a <see cref="Slider"/> control.
  /// </summary>
  public enum OrientationType
  {
    /// <summary>Horizontal Slider.</summary> 
    Horizontal,
    /// <summary>Vertical Slider.</summary> 
    Vertical,
  }

  #endregion

  /// <summary>
  /// Displays a slider control on a web page for sliding numeric user input.
  /// </summary>
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:Slider runat=server></{0}:Slider>")]
  [Designer(typeof(ComponentArt.Web.UI.SliderDesigner))]
  [GuidAttribute("53f586d7-6911-4cb0-82bd-564a9f882220")]
  public class Slider : WebControl
  {

    #region Public Properties

    private SliderClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public SliderClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new SliderClientEvents();
        }
        return _clientEvents;
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
            return (ClientTemplateCollection)_clientTemplates;
        }
    }

    /// <summary>
    /// The ID of the client template to use for the Slider popup.
    /// </summary>
    [Category("Appearance")]
    [Description("The ID of the client template to use for the Slider popup.")]
    [DefaultValue("")]
    public string PopUpClientTemplateId
    {
        get
        {
            object o = ViewState["PopUpClientTemplateId"];
            return (o == null) ? "" : (string)o;
        }
        set
        {
            ViewState["PopUpClientTemplateId"] = value;
        }
    }


    /// <summary>
    /// The ID of the client template to use for the TrackToolTip.
    /// </summary>
    [Category("Appearance")]
    [Description("The ID of the client template to use for the TrackToolTip.")]
    [DefaultValue("")]
    public string TrackToolTipClientTemplateId
    {
        get
        {
            object o = ViewState["TrackToolTipClientTemplateId"];
            return (o == null) ? "" : (string)o;
        }
        set
        {
            ViewState["TrackToolTipClientTemplateId"] = value;
        }
    }
    /// <summary>
    /// The Y offset of the Slider popup.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int PopUpOffsetY
    {
        get
        {
            object o = ViewState["PopUpOffsetY"];
            return (o == null) ? 0 : (int)o;
        }
        set
        {
            ViewState["PopUpOffsetY"] = value;
        }
    }

    /// <summary>
    /// The X offset of the Slider popup.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int PopUpOffsetX
    {
        get
        {
            object o = ViewState["PopUpOffsetX"];
            return (o == null) ? 0 : (int)o;
        }
        set
        {
            ViewState["PopUpOffsetX"] = value;
        }
    }

    /// <summary>
    /// The orientation of the Slider. Default: Horizontal.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(OrientationType.Horizontal)]
    public OrientationType Orientation
    {
      get
      {
        object o = ViewState["Orientation"];
        return (o == null) ? OrientationType.Horizontal : (OrientationType)o;
      }
      set
      {
        ViewState["Orientation"] = value;
      }
    }

    /// <summary>
    /// The current value of the Slider.
    /// </summary>
    [Category("Content")]
    [DefaultValue(0)]
    public decimal Value
    {
      get
      {
        object o = ViewState["Value"];
        return (o == null) ? 0 : (decimal)o;
      }
      set
      {
        ViewState["Value"] = value;
      }
    }

    /// <summary>
    /// The current inverted value of the Slider.
    /// </summary>
    [Category("Content")]
    [DefaultValue(0)]
    public decimal InvertedValue
    {
      get
      {
        object o = ViewState["InvertedValue"];
        return (o == null) ? 0 : (decimal)o;
      }
      set
      {
        ViewState["InvertedValue"] = value;
      }
    }

    /// <summary>
    /// The text for the Grip.
    /// </summary>
    [Category("Content")]
    [DefaultValue("")]
    public string GripText
    {
      get
      {
        object o = ViewState["GripText"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["GripText"] = value;
      }
    }

    /// <summary>
    /// The tool tip for the Grip.
    /// </summary>
    [Category("Content")]
    [DefaultValue("")]
    public string GripToolTip
    {
      get
      {
        object o = ViewState["GripToolTip"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["GripToolTip"] = value;
      }
    }


    /// <summary>
    /// The text for the Decrease Button.
    /// </summary>
    [Category("Content")]
    [DefaultValue("")]
    public string DecreaseText
    {
      get
      {
        object o = ViewState["DecreaseText"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["DecreaseText"] = value;
      }
    }

    /// <summary>
    /// The tool tip for the Decrease Button.
    /// </summary>
    [Category("Content")]
    [DefaultValue("")]
    public string DecreaseToolTip
    {
      get
      {
        object o = ViewState["DecreaseToolTip"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["DecreaseToolTip"] = value;
      }
    }

    /// <summary>
    /// The text for the Increase Button.
    /// </summary>
    [Category("Content")]
    [DefaultValue("")]
    public string IncreaseText
    {
      get
      {
        object o = ViewState["IncreaseText"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IncreaseText"] = value;
      }
    }

    /// <summary>
    /// The tool tip for the Increase Button.
    /// </summary>
    [Category("Content")]
    [DefaultValue("")]
    public string IncreaseToolTip
    {
      get
      {
        object o = ViewState["IncreaseToolTip"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IncreaseToolTip"] = value;
      }
    }

    /// <summary>
    /// The maximum value for the Slider, default 100.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(100)]
    public int MaxValue
    {
      get
      {
        object o = ViewState["MaxValue"];
        return (o == null) ? 100 : (int)o;
      }
      set
      {
        ViewState["MaxValue"] = value;
      }
    }

    /// <summary>
    /// The minimum value for the Slider, default 0.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int MinValue
    {
      get
      {
        object o = ViewState["MinValue"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["MinValue"] = value;
      }
    }

    /// <summary>
    /// The minimum value to increment/decrement when dragging the Slider, default 1.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(1)]
    public decimal Increment
    {
      get
      {
        object o = ViewState["Increment"];
        return (o == null) ? 1 : (decimal)o;
      }
      set
      {
        ViewState["Increment"] = value;
      }
    }

    /// <summary>
    /// Whether or not to add tick value as text to ticks. Default false.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool ShowTickValue
    {
      get
      {
        object o = ViewState["ShowTickValue"];
        return (o == null) ? false : (bool)o;
      }
      set
      {
        ViewState["ShowTickValue"] = value;
      }
    }

    /// <summary>
    /// Whether or not to show the current mouse position value as a ToolTip of the Slider track. Default true.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool ShowTrackToolTip
    {
      get
      {
        object o = ViewState["ShowTrackToolTip"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["ShowTrackToolTip"] = value;
      }
    }

    /// <summary>
    /// Whether or not mouse wheel support is enabled. Default true.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool MouseWheelEnabled
    {
      get
      {
        object o = ViewState["MouseWheelEnabled"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["MouseWheelEnabled"] = value;
      }
    }

    /// <summary>
    /// The multiplier to apply to increments for mouse wheel scrolling.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(1)]
    public int MouseWheelFactor
    {
      get
      {
        object o = ViewState["MouseWheelFactor"];
        return (o == null) ? 1 : (int)o;
      }
      set
      {
        ViewState["MouseWheelFactor"] = value;
      }
    }

    /// <summary>
    /// The interval at which to place ticks along the Slider track.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(0)]
    public int TickInterval
    {
      get
      {
        object o = ViewState["TickInterval"];
        return (o == null) ? 0 : (int)o;
      }
      set
      {
        ViewState["TickInterval"] = value;
      }
    }

    /// <summary>
    /// Whether or not keyboard support is enabled. Default true.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool KeyboardEnabled
    {
      get
      {
        object o = ViewState["KeyboardEnabled"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["KeyboardEnabled"] = value;
      }
    }

    /// <summary>
    /// Whether or not to snap the grip to the increment multiple.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool SnapToIncrement
    {
      get
      {
        object o = ViewState["SnapToIncrement"];
        return (o == null) ? true : (bool)o;
      }
      set
      {
        ViewState["SnapToIncrement"] = value;
      }
    }


    /// <summary>
    /// The type of the grip animation.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(SlideType.Linear)]
    public SlideType AnimationType
    {
      get
      {
        object o = ViewState["AnimationType"];
        return (o == null) ? SlideType.Linear : (SlideType)o;
      }
      set
      {
        ViewState["AnimationType"] = value;
      }
    }

    /// <summary>
    /// The duration of the grip animation in milliseconds.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(500)]
    public int AnimationDuration
    {
      get
      {
        object o = ViewState["AnimationDuration"];
        return (o == null) ? 200 : (int)o;
      }
      set
      {
        ViewState["AnimationDuration"] = value;
      }
    }

    /// <summary>
    /// The height or width in pixels of the track depending on the orientation.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(-1)]
    public int TrackLength
    {
        get
        {
            object o = ViewState["TrackLength"];
            return (o == null) ? -1 : (int)o;
        }
        set
        {
            ViewState["TrackLength"] = value;
        }
    }

    /// <summary>
    /// The height or width in pixels of the increase button depending on the orientation.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(null)]
    public int IncreaseLength
    {
        get
        {
            object o = ViewState["IncreaseLength"];
            return (o == null) ? -1 : (int)o;
        }
        set
        {
            ViewState["IncreaseLength"] = value;
        }
    }

    /// <summary>
    /// The height or width in pixels of the decrease button depending on the orientation.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(-1)]
    public int DecreaseLength
    {
        get
        {
            object o = ViewState["DecreaseLength"];
            return (o == null) ? -1 : (int)o;
        }
        set
        {
            ViewState["DecreaseLength"] = value;
        }
    }

    /// <summary>
    /// The css class to apply to grip of the slider.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string GripCssClass
    {
      get
      {
        object o = ViewState["GripCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["GripCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to grip of the slider when disabled.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string GripDisabledCssClass
    {
        get
        {
            object o = ViewState["GripDisabledCssClass"];
            return (o == null) ? String.Empty : (string)o;
        }
        set
        {
            ViewState["GripDisabledCssClass"] = value;
        }
    }

    /// <summary>
    /// The css class to apply to grip of the slider when hovering.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string GripHoverCssClass
    {
      get
      {
        object o = ViewState["GripHoverCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["GripHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to grip of the slider when active.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string GripActiveCssClass
    {
      get
      {
        object o = ViewState["GripActiveCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["GripActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to track of the slider.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string TrackCssClass
    {
      get
      {
        object o = ViewState["TrackCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["TrackCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to track of the slider when disabled.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string TrackDisabledCssClass
    {
        get
        {
            object o = ViewState["TrackDisabledCssClass"];
            return (o == null) ? String.Empty : (string)o;
        }
        set
        {
            ViewState["TrackDisabledCssClass"] = value;
        }
    }

    /// <summary>
    /// The css class to apply to the increase track of the slider.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string IncreaseTrackCssClass
    {
      get
      {
        object o = ViewState["IncreaseTrackCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IncreaseTrackCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to the increase track of the slider when disabled.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string IncreaseTrackDisabledCssClass
    {
        get
        {
            object o = ViewState["IncreaseTrackDisabledCssClass"];
            return (o == null) ? String.Empty : (string)o;
        }
        set
        {
            ViewState["IncreaseTrackDisabledCssClass"] = value;
        }
    }

    /// <summary>
    /// The css class to apply to the increase track of the slider when hovering.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string IncreaseTrackHoverCssClass
    {
      get
      {
        object o = ViewState["IncreaseTrackHoverCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IncreaseTrackHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to the increase track of the slider when active.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string IncreaseTrackActiveCssClass
    {
      get
      {
        object o = ViewState["IncreaseTrackActiveCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IncreaseTrackActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to the decrease track of the slider.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string DecreaseTrackCssClass
    {
      get
      {
        object o = ViewState["DecreaseTrackCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["DecreaseTrackCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to the decrease track of the slider when disabled.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string DecreaseTrackDisabledCssClass
    {
        get
        {
            object o = ViewState["DecreaseTrackDisabledCssClass"];
            return (o == null) ? String.Empty : (string)o;
        }
        set
        {
            ViewState["DecreaseTrackDisabledCssClass"] = value;
        }
    }

    /// <summary>
    /// The css class to apply to the decrease track of the slider when hovering.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string DecreaseTrackHoverCssClass
    {
      get
      {
        object o = ViewState["DecreaseTrackHoverCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["DecreaseTrackHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to the decrease track of the slider when active.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string DecreaseTrackActiveCssClass
    {
      get
      {
        object o = ViewState["DecreaseTrackActiveCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["DecreaseTrackActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to the increase button of the slider.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string IncreaseCssClass
    {
      get
      {
        object o = ViewState["IncreaseCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IncreaseCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to increase button of the slider when disabled.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string IncreaseDisabledCssClass
    {
        get
        {
            object o = ViewState["IncreaseDisabledCssClass"];
            return (o == null) ? String.Empty : (string)o;
        }
        set
        {
            ViewState["IncreaseDisabledCssClass"] = value;
        }
    }

    /// <summary>
    /// The css class to apply to the increase button of the slider when hovering.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string IncreaseHoverCssClass
    {
      get
      {
        object o = ViewState["IncreaseHoverCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IncreaseHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to the increase button of the slider when active.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string IncreaseActiveCssClass
    {
      get
      {
        object o = ViewState["IncreaseActiveCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["IncreaseActiveCssClass"] = value;
      }
    }


    /// <summary>
    /// The css class to apply to the decrease button of the slider.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string DecreaseCssClass
    {
      get
      {
        object o = ViewState["DecreaseCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["DecreaseCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to decrease button of the slider when disabled.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string DecreaseDisabledCssClass
    {
        get
        {
            object o = ViewState["DecreaseDisabledCssClass"];
            return (o == null) ? String.Empty : (string)o;
        }
        set
        {
            ViewState["DecreaseDisabledCssClass"] = value;
        }
    }

    /// <summary>
    /// The css class to apply to the decrease button of the slider when hovering.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string DecreaseHoverCssClass
    {
      get
      {
        object o = ViewState["DecreaseHoverCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["DecreaseHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to the decrease button of the slider when active.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string DecreaseActiveCssClass
    {
      get
      {
        object o = ViewState["DecreaseActiveCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["DecreaseActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// The css class to apply to the ticks along the Slider track.
    /// </summary>
    [Category("Style")]
    [DefaultValue("")]
    public string TickCssClass
    {
      get
      {
        object o = ViewState["TickCssClass"];
        return (o == null) ? String.Empty : (string)o;
      }
      set
      {
        ViewState["TickCssClass"] = value;
      }
    }
    #endregion

    #region Public Methods

    public Slider()
    {
    }

    #endregion

    #region Protected Methods

    protected override void RenderContents(HtmlTextWriter output)
    {
      output.Write("<div");
      output.WriteAttribute("id", this.ClientID);
      output.WriteAttribute("class", this.CssClass);
      output.WriteAttribute("style", "position:relative;display:block;visibility:hidden;height:" + this.Height + ";width:" + this.Width + ";");
      output.Write(">");
      output.Write("</div>");

      foreach (Control oControl in Controls)
      {
        oControl.RenderControl(output);
      }
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
        if (this.Height.Value == 0)
            this.Height = (this.Orientation == OrientationType.Horizontal) ? Unit.Pixel(25) : Unit.Pixel(200);

        if (this.Width.Value == 0)
            this.Width = (this.Orientation == OrientationType.Horizontal) ? Unit.Pixel(200) : Unit.Pixel(25);

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
        // do we need to render scripts for non-Atlas?
        ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
        if (oScriptManager == null)
        {
          // Add core code
          if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
          {
            Page.RegisterClientScriptBlock("A573G988.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A573HR343.js"))
          {
            Page.RegisterClientScriptBlock("A573HR343.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Slider.client_scripts", "A573HR343.js");
          }
        }
      }

      string sSliderVarName = this.GetSaneId();

      // Render data hidden fields
      output.AddAttribute("id", sSliderVarName + "_Value");
      output.AddAttribute("name", sSliderVarName + "_Value");
      output.AddAttribute("value", Convert.ToString(this.Value));
      output.AddAttribute("type", "hidden");
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      output.AddAttribute("id", sSliderVarName + "_InvertedValue");
      output.AddAttribute("name", sSliderVarName + "_InvertedValue");
      output.AddAttribute("value", Convert.ToString(this.InvertedValue));
      output.AddAttribute("type", "hidden");
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      // Content Output
      RenderContents(output);

      // Render client-side object initiation. 
      StringBuilder oStartupSB = new StringBuilder();

      oStartupSB.Append("/*** ComponentArt.Web.UI.Slider ").Append(this.VersionString()).Append(" ").Append(sSliderVarName).Append(" ***/\n");

      oStartupSB.Append("function ComponentArt_Init_" + sSliderVarName + "() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      oStartupSB.Append("if(!window.ComponentArt_Slider_Loaded || !window.ComponentArt_Utils_Loaded)\n");
      oStartupSB.Append("\t{setTimeout('ComponentArt_Init_" + sSliderVarName + "()', 100); return; }\n\n");

      // Instantiate object
      oStartupSB.Append("window." + sSliderVarName + " = new ComponentArt_Slider('" + sSliderVarName + "');\n");

      // Hook the actual ID if available and different from effective client ID
      if (this.ID != sSliderVarName)
      {
        oStartupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sSliderVarName + "; " + sSliderVarName + ".GlobalAlias = '" + ID + "'; }\n");
      }

      oStartupSB.Append(sSliderVarName + ".ControlId = '" + this.UniqueID + "';\n");


      oStartupSB.Append("var properties = [\n");
      oStartupSB.Append("['ApplicationPath'," + Utils.ConvertStringToJSString(Context.Request.ApplicationPath) + "],");
      oStartupSB.Append("['Height','" + this.Height.ToString() + "'],");
      oStartupSB.Append("['Width','" + this.Width.ToString() + "'],");
      oStartupSB.Append("['PopUpClientTemplateId','" + this.PopUpClientTemplateId + "'],");
      oStartupSB.Append("['TrackToolTipClientTemplateId','" + this.TrackToolTipClientTemplateId + "'],");
      oStartupSB.Append("['PopUpOffsetX'," + this.PopUpOffsetX.ToString() + "],");
      oStartupSB.Append("['PopUpOffsetY'," + this.PopUpOffsetY.ToString() + "],");
      oStartupSB.Append("['GripCssClass','" + this.GripCssClass.ToString() + "'],");
      oStartupSB.Append("['GripDisabledCssClass','" + this.GripDisabledCssClass.ToString() + "'],");
      oStartupSB.Append("['GripHoverCssClass','" + this.GripHoverCssClass.ToString() + "'],");
      oStartupSB.Append("['GripActiveCssClass','" + this.GripActiveCssClass.ToString() + "'],");
      oStartupSB.Append("['TrackCssClass','" + this.TrackCssClass.ToString() + "'],");
      oStartupSB.Append("['TrackDisabledCssClass','" + this.TrackCssClass.ToString() + "'],");
      oStartupSB.Append("['IncreaseCssClass','" + this.IncreaseCssClass.ToString() + "'],");
      oStartupSB.Append("['IncreaseDisabledCssClass','" + this.IncreaseCssClass.ToString() + "'],");
      oStartupSB.Append("['IncreaseHoverCssClass','" + this.IncreaseHoverCssClass.ToString() + "'],");
      oStartupSB.Append("['IncreaseActiveCssClass','" + this.IncreaseActiveCssClass.ToString() + "'],");
      oStartupSB.Append("['DecreaseCssClass','" + this.DecreaseCssClass.ToString() + "'],");
      oStartupSB.Append("['DecreaseDisabledCssClass','" + this.DecreaseCssClass.ToString() + "'],");
      oStartupSB.Append("['DecreaseHoverCssClass','" + this.DecreaseHoverCssClass.ToString() + "'],");
      oStartupSB.Append("['DecreaseActiveCssClass','" + this.DecreaseActiveCssClass.ToString() + "'],");
      oStartupSB.Append("['IncreaseTrackCssClass','" + this.IncreaseTrackCssClass.ToString() + "'],");
      oStartupSB.Append("['IncreaseTrackDisabledCssClass','" + this.IncreaseTrackDisabledCssClass.ToString() + "'],");
      oStartupSB.Append("['IncreaseTrackHoverCssClass','" + this.IncreaseTrackHoverCssClass.ToString() + "'],");
      oStartupSB.Append("['IncreaseTrackActiveCssClass','" + this.IncreaseTrackActiveCssClass.ToString() + "'],");
      oStartupSB.Append("['DecreaseTrackCssClass','" + this.DecreaseTrackCssClass.ToString() + "'],");
      oStartupSB.Append("['DecreaseTrackDisabledCssClass','" + this.DecreaseTrackCssClass.ToString() + "'],");
      oStartupSB.Append("['DecreaseTrackHoverCssClass','" + this.DecreaseTrackHoverCssClass.ToString() + "'],");
      oStartupSB.Append("['DecreaseTrackActiveCssClass','" + this.DecreaseTrackActiveCssClass.ToString() + "'],");
      oStartupSB.Append("['TickCssClass','" + this.TickCssClass.ToString() + "'],");
      oStartupSB.Append("['TickInterval'," + this.TickInterval.ToString() + "],");
      oStartupSB.Append("['TrackLength'," + this.TrackLength.ToString() + "],");
      oStartupSB.Append("['IncreaseLength'," + this.IncreaseLength.ToString() + "],");
      oStartupSB.Append("['DecreaseLength'," + this.DecreaseLength.ToString() + "],");
      oStartupSB.Append("['Value'," + this.Value.ToString() + "],");
      oStartupSB.Append("['InvertedValue'," + this.InvertedValue.ToString() + "],");
      oStartupSB.Append("['GripText','" + this.GripText.ToString() + "'],");
      oStartupSB.Append("['GripToolTip','" + this.GripToolTip.ToString() + "'],");
      oStartupSB.Append("['DecreaseText','" + this.DecreaseText.ToString() + "'],");
      oStartupSB.Append("['DecreaseToolTip','" + this.DecreaseToolTip.ToString() + "'],");
      oStartupSB.Append("['IncreaseText','" + this.IncreaseText.ToString() + "'],");
      oStartupSB.Append("['IncreaseToolTip','" + this.IncreaseToolTip.ToString() + "'],");
      oStartupSB.Append("['MinValue'," + this.MinValue.ToString() + "],");
      oStartupSB.Append("['MaxValue'," + this.MaxValue.ToString() + "],");
      oStartupSB.Append("['Increment'," + this.Increment.ToString() + "],");
      oStartupSB.Append("['TabIndex'," + this.TabIndex.ToString() + "],");
      oStartupSB.Append("['Enabled'," + this.Enabled.ToString().ToLower() + "],");
      oStartupSB.Append("['ShowTickValue'," + this.ShowTickValue.ToString().ToLower() + "],");
      oStartupSB.Append("['ShowTrackToolTip'," + this.ShowTrackToolTip.ToString().ToLower() + "],");
      oStartupSB.Append("['MouseWheelFactor'," + this.MouseWheelFactor.ToString() + "],");
      oStartupSB.Append("['MouseWheelEnabled'," + this.MouseWheelEnabled.ToString().ToLower() + "],");
      oStartupSB.Append("['KeyboardEnabled'," + this.KeyboardEnabled.ToString().ToLower() + "],");
      oStartupSB.Append("['SnapToIncrement'," + this.SnapToIncrement.ToString().ToLower() + "],");
      oStartupSB.Append("['Orientation','" + this.Orientation.ToString().ToLower() + "'],");
      oStartupSB.Append("['AnimationType'," + ((int)this.AnimationType).ToString() + "],");
      oStartupSB.Append("['AnimationDuration'," + this.AnimationDuration.ToString() + "],");
      oStartupSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "]");
      oStartupSB.Append("];\n");

      oStartupSB.Append(sSliderVarName + ".ClientTemplates = " + this._clientTemplates.ToString() + ";\n");
      oStartupSB.AppendFormat("ComponentArt_SetProperties({0}, properties);\n", sSliderVarName);

      oStartupSB.Append(sSliderVarName + ".Initialize();\n}\n");

      oStartupSB.Append("ComponentArt_Init_" + sSliderVarName + "();");

      WriteStartupScript(output, this.DemarcateClientScript(oStartupSB.ToString()));
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      EnsureChildControls();
      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Slider.client_scripts.A573HR343.js");
      }
    }

    protected override object SaveViewState()
    {
      ViewState["Slider"] = "ForceViewState";
      return base.SaveViewState();
    }

    protected override void LoadViewState(object state)
    {
      base.LoadViewState(state);

      string sSliderVarName = this.GetSaneId();

      // Load client data
      string value = Context.Request.Form[sSliderVarName + "_Value"];

      if (value != null)
      {
        this.Value = Convert.ToDecimal(HttpUtility.UrlDecode(value));
      }

      string invertedvalue = Context.Request.Form[sSliderVarName + "_InvertedValue"];
      if (invertedvalue != null)
      {
        this.InvertedValue = Convert.ToDecimal(HttpUtility.UrlDecode(invertedvalue));
      }
    }

    private void RenderAccessible(HtmlTextWriter output)
    {
      output.Write("<select id=\"");
      output.Write(this.ID);
      output.Write("\">");
      for (decimal i = this.MinValue; i <= this.MaxValue; i += this.Increment)
      {
        output.Write("<option value=\"");
        output.Write(i.ToString(CultureInfo.InvariantCulture));
        output.Write("\">");
        output.Write(i.ToString(CultureInfo.InvariantCulture));
        output.Write("</option>");
      }
      output.Write("</select>");
    }

    internal void RenderDownLevel(HtmlTextWriter output)
    {
        if (this.Height.Value == 0)
            this.Height = (this.Orientation == OrientationType.Horizontal) ? Unit.Pixel(25) : Unit.Pixel(200);

        if (this.Width.Value == 0)
            this.Width = (this.Orientation == OrientationType.Horizontal) ? Unit.Pixel(200) : Unit.Pixel(25);

        output.Write("<div style='position:relative;height:" + this.Height.ToString() + ";width:" + this.Width.ToString() + ";'>");

        string trackstyle = "background-color:#dddddd;border:1px;border-style:solid;border-color:#888888;";
        string gripstyle = "background-color:#dddddd;border:1px;border-style:solid;border-color:#888888;";
        
        if(this.Orientation == OrientationType.Horizontal)
        {
            output.Write("<div style='top:" + (Convert.ToInt16(this.Height.Value) / 2 - 2) + "px;"+trackstyle+"position:absolute;height:3px;width:" + (Convert.ToInt16(this.Width.Value) - 2) + ";font-size:1px;'>&nbsp;</div>");

            double factor = Convert.ToDouble(this.MaxValue - this.MinValue) / (Convert.ToDouble(this.Width.Value) - 7);
            int target = Convert.ToInt16((Convert.ToDouble(this.Value - this.MinValue) / factor));

            output.Write("<div style='top:" + (Convert.ToInt16(this.Height.Value) / 2 - 7) + "px;left:" + target + "px;" + gripstyle + "position:absolute;height:13px;width:5px;font-size:1px;'>&nbsp;</div>");
        }else{
            output.Write("<div style='left:" + (Convert.ToInt16(this.Width.Value) / 2 - 2) + "px;" + trackstyle + "position:absolute;height:" + (Convert.ToInt16(this.Height.Value) - 2) + ";width:3px;font-size:1px;'>&nbsp;</div>");

            double factor = Convert.ToDouble(this.MaxValue - this.MinValue) / (Convert.ToDouble(this.Height.Value) - 7);
            int target = Convert.ToInt16((Convert.ToDouble(this.Value - this.MinValue) / factor));

            output.Write("<div style='left:" + (Convert.ToInt16(this.Width.Value) / 2 - 7) + "px;top:" + target + "px;" + gripstyle + "position:absolute;width:13px;height:5px;font-size:1px;'>&nbsp;</div>");
        }
        output.Write("</div>");
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
    #endregion

  }

}
