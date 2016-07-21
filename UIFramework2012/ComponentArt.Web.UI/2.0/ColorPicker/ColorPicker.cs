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
    /// Specifies the type of ColorPicker to use for a <see cref="ColorPicker"/> control.
    /// </summary>
    public enum ColorPickerMode
    {
        /// <summary>Color Grid.</summary> 
        Grid,
        /// <summary>Color Slider.</summary> 
        Slider,
        /// <summary>Color Wheel.</summary> 
        Wheel,
        /// <summary>Color Bloom.</summary> 
        Bloom
    }

    #endregion

    /// <summary>
    /// Displays a ColorPicker control on a web page allowing various color selection user input.
    /// </summary>
    [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
    [ToolboxData("<{0}:ColorPicker runat=server></{0}:ColorPicker>")]
    [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
    [Designer(typeof(ComponentArt.Web.UI.ColorPickerDesigner))]
    [Serializable]
    public class ColorPicker : WebControl
    {
        # region Private Properties

        private String ColorsArrayString;
        private String CustomColorsArrayString;
        private Slider _ColorSlider = new ComponentArt.Web.UI.Slider();

        #endregion

        #region Public Properties

        private ColorPickerColorCollection _colors;
        /// <summary>
        /// The collection of ColorPicker colors.
        /// </summary>
        [Category("Data")]
        [Description("The collection of ColorPicker colors.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ColorPickerColorCollection Colors
        {
            get
            {
                if (_colors == null)
                {
                    _colors = new ColorPickerColorCollection();
                }

                return _colors;
            }
        }

        private ColorPickerColorCollection _customcolors;
        /// <summary>
        /// The collection of ColorPicker colors.
        /// </summary>
        [Category("Data")]
        [Description("The collection of custom ColorPicker colors.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ColorPickerColorCollection CustomColors
        {
            get
            {
                if (_customcolors == null)
                {
                    _customcolors = new ColorPickerColorCollection();
                }

                return _customcolors;
            }
        }

        /// <summary>
        /// The currently selected Color.
        /// </summary>
        [Category("Data")]
        [DefaultValue(null)]
        public ColorPickerColor SelectedColor
        {
            get
            {
                object o = ViewState["SelectedColor"];
                return (o == null) ? null : (ColorPickerColor)o;
            }
            set
            {
                ViewState["SelectedColor"] = value;
            }
        }

        /// <summary>
        /// The X offset of the crosshair to account for styles applied to the ColorPlane.
        /// </summary>
        [Category("Layout")]
        [DefaultValue(0)]
        public int CrosshairOffsetX
        {
            get
            {
                object o = ViewState["CrosshairOffsetX"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                ViewState["CrosshairOffsetX"] = value;
            }
        }

        /// <summary>
        /// The Y offset of the crosshair to account for styles applied to the ColorPlane.
        /// </summary>
        [Category("Layout")]
        [DefaultValue(0)]
        public int CrosshairOffsetY
        {
            get
            {
                object o = ViewState["CrosshairOffsetY"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                ViewState["CrosshairOffsetY"] = value;
            }
        }

        /// <summary>
        /// The currently selected ColorSwatch. -1 for null.
        /// </summary>
        [Category("Data")]
        [DefaultValue(-1)]
        public int SelectedColorSwatch
        {
            get
            {
                object o = ViewState["SelectedColorSwatch"];
                return (o == null) ? -1 : (int)o;
            }
            set
            {
                ViewState["SelectedColorSwatch"] = value;
            }
        }

        /// <summary>
        /// The currently selected CustomColorSwatch. -1 for null.
        /// </summary>
        [Category("Data")]
        [DefaultValue(-1)]
        public int SelectedCustomColorSwatch
        {
            get
            {
                object o = ViewState["SelectedCustomColorSwatch"];
                return (o == null) ? -1 : (int)o;
            }
            set
            {
                ViewState["SelectedCustomColorSwatch"] = value;
            }
        }

        private ColorPickerClientEvents _clientEvents = null;
        /// <summary>
        /// Client event handler definitions.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("Client event handler definitions.")]
        [Category("Client events")]
        public ColorPickerClientEvents ClientEvents
        {
            get
            {
                if (_clientEvents == null)
                {
                    _clientEvents = new ColorPickerClientEvents();
                }
                return _clientEvents;
            }
        }

        /// <summary>
        /// The ColorSlider Object when in Slider Mode.
        /// </summary>
        [Category("Layout")]
        [DefaultValue(null)]
        public Slider ColorSlider
        {
            get
            {
                return _ColorSlider;
            }
        }

        /// <summary>
        /// The mode of the ColorPicker. Default: Bloom.
        /// </summary>
        [Category("Layout")]
        [DefaultValue(ColorPickerMode.Bloom)]
        public ColorPickerMode Mode
        {
            get
            {
                object o = ViewState["Mode"];
                return (o == null) ? ColorPickerMode.Bloom : (ColorPickerMode)o;
            }
            set
            {
                ViewState["Mode"] = value;
            }
        }

        /// <summary>
        /// The number of Columns in the ColorGrid and CustomColorGrid. Default 8.
        /// </summary>
        [Category("Layout")]
        [DefaultValue(8)]
        public int GridColumns
        {
            get
            {
                object o = ViewState["GridColumns"];
                return (o == null) ? 8 : (int)o;
            }
            set
            {
                ViewState["GridColumns"] = value;
            }
        }

        /// <summary>
        /// The cell spacing for the ColorGrid and CustomColorGrid. Default 0.
        /// </summary>
        [Category("Layout")]
        [DefaultValue(0)]
        public int GridCellSpacing
        {
            get
            {
                object o = ViewState["GridCellSpacing"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                ViewState["GridCellSpacing"] = value;
            }
        }

        /// <summary>
        /// The cell padding for the ColorGrid and CustomColorGrid. Default 0.
        /// </summary>
        [Category("Layout")]
        [DefaultValue(0)]
        public int GridCellPadding
        {
            get
            {
                object o = ViewState["GridCellPadding"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                ViewState["GridCellPadding"] = value;
            }
        }

        /// <summary>
        /// The URL of the image directory, required for ASP.NET 1.0.
        /// </summary>
        [Category("Layout")]
        [DefaultValue("")]
        public string BaseImageUrl
        {
            get
            {
                object o = ViewState["BaseImageUrl"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["BaseImageUrl"] = value;
            }
        }

        /// <summary>
        /// The label to place above the ColorGrid.
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        public string ColorGridLabel
        {
            get
            {
                object o = ViewState["ColorGridLabel"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["ColorGridLabel"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to the ColorPlane.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string ColorPlaneCssClass
        {
            get
            {
                object o = ViewState["ColorPlaneCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["ColorPlaneCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to the ColorGrid.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string ColorGridCssClass
        {
            get
            {
                object o = ViewState["ColorGridCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["ColorGridCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to the ColorSlider in Slider Mode.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string SliderCssClass
        {
            get
            {
                object o = ViewState["SliderCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["SliderCssClass"] = value;
            }
        }

        /// <summary>
        /// The label to place above the CustomColorGrid.
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        public string CustomColorGridLabel
        {
            get
            {
                object o = ViewState["CustomColorGridLabel"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["CustomColorGridLabel"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to the CustomColorGrid.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string CustomColorGridCssClass
        {
            get
            {
                object o = ViewState["CustomColorGridCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["CustomColorGridCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to ColorGrid Color Swatches.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string ColorCssClass
        {
            get
            {
                object o = ViewState["ColorCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["ColorCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to ColorGrid Color Swatches when hovering.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string ColorHoverCssClass
        {
            get
            {
                object o = ViewState["ColorHoverCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["ColorHoverCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to ColorGrid Color Swatches when selected.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string ColorActiveCssClass
        {
            get
            {
                object o = ViewState["ColorActiveCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["ColorActiveCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to Custom ColorGrid Color Swatches.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string CustomColorCssClass
        {
            get
            {
                object o = ViewState["CustomColorCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["CustomColorCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to Custom ColorGrid Color Swatches when hovering.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string CustomColorHoverCssClass
        {
            get
            {
                object o = ViewState["CustomColorHoverCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["CustomColorHoverCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class to apply to Custom ColorGrid Color Swatches when selected.
        /// </summary>
        [Category("Style")]
        [DefaultValue("")]
        public string CustomColorActiveCssClass
        {
            get
            {
                object o = ViewState["CustomColorActiveCssClass"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["CustomColorActiveCssClass"] = value;
            }
        }

        #endregion

        #region Protected Methods

        protected override void RenderContents(HtmlTextWriter output)
        {
          string saneid = this.GetSaneId();

          output.Write("<div");
          output.WriteAttribute("id", saneid);
          output.WriteAttribute("Class", this.CssClass);
          output.Write(">");

            string blank_gif = String.Empty;
            string crosshair_gif = String.Empty;
            string grip_gif = String.Empty;
            string hsvbloom_png = String.Empty;
            string hsvwheel_png = String.Empty;
            string hue_png = String.Empty;
            string saturation_png = String.Empty;

          if (this.BaseImageUrl == String.Empty)
          {
              blank_gif = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ComponentArt.Web.UI.ColorPicker.images.blank.gif");
              crosshair_gif = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ComponentArt.Web.UI.ColorPicker.images.crosshair.gif");
              grip_gif = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ComponentArt.Web.UI.ColorPicker.images.grip.gif");
              hsvbloom_png = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ComponentArt.Web.UI.ColorPicker.images.hsvbloom.png");
              hsvwheel_png = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ComponentArt.Web.UI.ColorPicker.images.hsvwheel.png");
              hue_png = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ComponentArt.Web.UI.ColorPicker.images.hue.png");
              saturation_png = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ComponentArt.Web.UI.ColorPicker.images.saturation.png");
          }
          else
          {
              blank_gif = this.BaseImageUrl + "blank.gif";
              crosshair_gif = this.BaseImageUrl + "crosshair.gif";
              grip_gif = this.BaseImageUrl + "grip.gif";
              hsvbloom_png = this.BaseImageUrl + "hsvbloom.png";
              hsvwheel_png = this.BaseImageUrl + "hsvwheel.png";
              hue_png = this.BaseImageUrl + "hue.png";
              saturation_png = this.BaseImageUrl + "saturation.png";
          }


          switch (this.Mode)
          {
              case ColorPickerMode.Bloom:
                  output.Write("<div style='position:relative;'>");
                  output.Write("<img unselectable='on' id='" + saneid + "_Crosshair' src='" + crosshair_gif + "' />");
                  output.Write("<img class='" + this.ColorPlaneCssClass + "' unselectable='on' id='" + saneid + "_Plane' src='" + hsvbloom_png + "' />");
                  output.Write("</div>");
                  break;
              case ColorPickerMode.Wheel:
                  output.Write("<div style='position:relative;'>");
                  output.Write("<img unselectable='on' id='" + saneid + "_Crosshair' src='" + crosshair_gif + "' />");
                  output.Write("<img class='" + this.ColorPlaneCssClass + "' unselectable='on' id='" + saneid + "_Plane' src='" + hsvwheel_png + "' />");
                  output.Write("</div>");
                  break;
              case ColorPickerMode.Slider:
                  // needs adjust for selected color
                  output.Write("<div style='position:relative;float:left;display:block;padding-right:0px;'>");
                  output.Write("<img unselectable='on' id='" + saneid + "_Crosshair' src='" + crosshair_gif + "' />");
                  try
                  {
                      // IE PNG Fix
                      if (Context.Request.UserAgent.IndexOf("MSIE") >= 0 && Context.Request.Browser.MajorVersion < 7)
                      {
                          output.Write("<img class='" + this.ColorPlaneCssClass + "' style=\"height:256px;width:256px;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + saturation_png + "', sizingMethod='scale')\" style='background-color:#FFFFFF;' unselectable='on' id='" + saneid + "_Plane' src='" + blank_gif + "' />");
                      }
                      else
                      {
                          output.Write("<img class='" + this.ColorPlaneCssClass + "' style='background-color:#FF0000;' unselectable='on' id='" + saneid + "_Plane' src='" + saturation_png + "' />");
                      }
                  }
                  catch
                  {
                      output.Write("<img class='" + this.ColorPlaneCssClass + "' style='background-color:#FF0000;' unselectable='on' id='" + saneid + "_Plane' src='" + saturation_png + "' />");
                  }
  
                  output.Write("</div>");
                  
                  // needs background, grip, etc
                  output.Write("<style>");
                  output.Write(".trackCssClass { cursor: hand; cursor: pointer; height:256px; width:22px; background-image:url('" + hue_png + "'); }");
                  output.Write(".gripCssClass { cursor: hand; cursor: pointer; height:5px; width:28px; background-image:url('" + grip_gif + "'); }");
                  output.Write("</style>");
                  output.Write("<div style='float:left;clear:right;display:block;padding-right:10px;'>");
                  _ColorSlider.ID = saneid + "_Slider";
                  _ColorSlider.CssClass = this.SliderCssClass;
                  _ColorSlider.Height = Unit.Pixel(256);
                  _ColorSlider.Width = Unit.Pixel(38);
                  _ColorSlider.TrackLength = 256;
                  _ColorSlider.GripCssClass = "gripCssClass";
                  _ColorSlider.TrackCssClass = "trackCssClass";
                  _ColorSlider.MaxValue = 359;
                  _ColorSlider.MouseWheelFactor = 5;
                  _ColorSlider.ShowTrackToolTip = false;
                  if(this.SelectedColor != null) _ColorSlider.Value = Convert.ToDecimal(this.SelectedColor.SystemDrawingColor.GetHue());
                  _ColorSlider.DecreaseLength = 0;
                  _ColorSlider.IncreaseLength = 0;
                  _ColorSlider.Orientation = OrientationType.Vertical;
                  _ColorSlider.ClientEvents.Handlers.Add("ValueChanged", "function(sender, eventArgs) { " + this.GetSaneId() + ".ColorPicker_Slider_Handler(sender, eventArgs); }");
                  _ColorSlider.RenderControl(output);
                  output.Write("</div>");
                  break;
          }

          if (this.Colors.Count > 0)
          {
              output.Write("<div style='clear:left;'><table class=\"" + this.ColorGridCssClass + "\" cellpadding='" + this.GridCellPadding.ToString() + "' cellspacing='" + this.GridCellSpacing.ToString() + "' width='256'><tr>");

              if (this.ColorGridLabel != String.Empty) output.Write("<th colspan='" + this.GridColumns + "'>" + this.ColorGridLabel + "</th></tr><tr>");

              for (int i = 0; i < this.Colors.Count; i++)
              {
                  if (i % this.GridColumns == 0 && i != 0) output.Write("</tr><tr>");
                  string cssclass = (i == this.SelectedColorSwatch) ? this.ColorActiveCssClass : this.ColorCssClass;
                  output.Write("<td Class=\"" + cssclass + "\" id=\"" + this.GetSaneId() + "_Color_" + i.ToString() + "\" onclick=\"" + this.GetSaneId() + ".Color_Select_Handler(this," + i.ToString() + ");\" onmouseover=\"" + this.GetSaneId() + ".Color_Over_Handler(this," + i.ToString() + ");\" onmouseout=\"" + this.GetSaneId() + ".Color_Out_Handler(this," + i.ToString() + ");\" style='" + ((this.Colors[i].SystemDrawingColor == Color.Empty) ? "" : "background-color:#"+this.Colors[i].Hex) + "'>&nbsp;</td>");
              }

              output.Write("</tr></table>");
          }
          if(this.CustomColors.Count > 0)
          {
              output.Write("<table class=\"" + this.CustomColorGridCssClass + "\" cellpadding='" + this.GridCellPadding.ToString() + "' cellspacing='" + this.GridCellSpacing.ToString() + "' width='256'><tr>");

              if (this.CustomColorGridLabel != String.Empty) output.Write("<th colspan='" + this.GridColumns + "'>" + this.CustomColorGridLabel + "</th></tr><tr>");

              for (int i = 0; i < this.CustomColors.Count; i++)
              {
                  if (i % this.GridColumns == 0 && i != 0) output.Write("</tr><tr>");
                  string cssclass = (i == this.SelectedCustomColorSwatch) ? this.CustomColorActiveCssClass : this.CustomColorCssClass;
                  output.Write("<td Class=\"" + cssclass + "\" id=\"" + this.GetSaneId() + "_CustomColor_" + i.ToString() + "\" onclick=\"" + this.GetSaneId() + ".CustomColor_Select_Handler(this," + i.ToString() + ");\" onmouseover=\"" + this.GetSaneId() + ".CustomColor_Over_Handler(this," + i.ToString() + ");\" onmouseout=\"" + this.GetSaneId() + ".CustomColor_Out_Handler(this," + i.ToString() + ");\" style='" + ((this.CustomColors[i].SystemDrawingColor == Color.Empty) ? "" : "background-color:#" + this.CustomColors[i].Hex) + "'>&nbsp;</td>");
              }

              output.Write("</tr></table>");
          }

          output.Write("</div></div>");

          foreach (Control oControl in Controls)
          {
            oControl.RenderControl(output);
          }
        }

        protected override void ComponentArtRender(HtmlTextWriter output)
        {
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
              if (!Page.IsClientScriptBlockRegistered("A573C779.js"))
              {
                Page.RegisterClientScriptBlock("A573C779.js", "");
                WriteGlobalClientScript(output, "ComponentArt.Web.UI.ColorPicker.client_scripts", "A573C779.js");
              }
              if (this.Mode == ColorPickerMode.Slider && !Page.IsClientScriptBlockRegistered("A573HR343.js"))
              {
                Page.RegisterClientScriptBlock("A573HR343.js", "");
                WriteGlobalClientScript(output, "ComponentArt.Web.UI.Slider.client_scripts", "A573HR343.js");
              }
              }

          string sColorPickerVarName = this.GetSaneId();

          String ColorsArrayString = "[";
          String ColorsArrayStorage = String.Empty;

          for (int i = 0; i < this.Colors.Count; i++)
          {
              ColorsArrayString += "{Hex:'" + ((this.Colors[i].SystemDrawingColor == Color.Empty) ? "null" : this.Colors[i].Hex) + "', Name:'" + this.Colors[i].Name + "'}";
              ColorsArrayStorage += ((this.Colors[i].SystemDrawingColor == Color.Empty) ? "null" : this.Colors[i].Hex) + "|" + this.Colors[i].Name + ",";

              if (i < this.Colors.Count) ColorsArrayString += ",";
          }
          ColorsArrayString += "]";

          String CustomColorsArrayString = "[";
          String CustomColorsArrayStorage = String.Empty;

          for (int i = 0; i < this.CustomColors.Count; i++)
          {
              CustomColorsArrayString += "{Hex:'" + ((this.CustomColors[i].SystemDrawingColor == Color.Empty) ? "null" : this.CustomColors[i].Hex) + "', Name:'" + this.CustomColors[i].Name + "'}";
              CustomColorsArrayStorage += ((this.CustomColors[i].SystemDrawingColor == Color.Empty) ? "'null'" : this.CustomColors[i].Hex) + "|" + this.CustomColors[i].Name + ",";

              if (i < this.CustomColors.Count) CustomColorsArrayString += ",";
          }
          CustomColorsArrayString += "]";

          // Render data hidden fields
          output.AddAttribute("id", sColorPickerVarName + "_SelectedColor");
          output.AddAttribute("name", sColorPickerVarName + "_SelectedColor");
          output.AddAttribute("value", (this.SelectedColor == null) ? "" : this.SelectedColor.Hex);
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          output.AddAttribute("id", sColorPickerVarName + "_SelectedColorSwatch");
          output.AddAttribute("name", sColorPickerVarName + "_SelectedColorSwatch");
          output.AddAttribute("value", Convert.ToString(this.SelectedColorSwatch));
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          output.AddAttribute("id", sColorPickerVarName + "_SelectedCustomColorSwatch");
          output.AddAttribute("name", sColorPickerVarName + "_SelectedCustomColorSwatch");
          output.AddAttribute("value", Convert.ToString(this.SelectedCustomColorSwatch));
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          output.AddAttribute("id", sColorPickerVarName + "_CustomColors");
          output.AddAttribute("name", sColorPickerVarName + "_CustomColors");
          output.AddAttribute("value", CustomColorsArrayStorage);
          output.AddAttribute("type", "hidden");
          output.RenderBeginTag(HtmlTextWriterTag.Input);
          output.RenderEndTag();

          // Content Output
          RenderContents(output);

          // Render client-side object initiation. 
          StringBuilder oStartupSB = new StringBuilder();

          oStartupSB.Append("/*** ComponentArt.Web.UI.ColorPicker ").Append(this.VersionString()).Append(" ").Append(sColorPickerVarName).Append(" ***/\n");

          oStartupSB.Append("function ComponentArt_Init_" + sColorPickerVarName + "() {\n");

          // Include check for whether everything we need is loaded,
          // and a retry after a delay in case it isn't.
          if (this.Mode == ColorPickerMode.Slider)
          {
              oStartupSB.Append("if(!window.ComponentArt_ColorPicker_Loaded || !window.ComponentArt_Utils_Loaded || !window.ComponentArt_Slider_Loaded)\n");
          }
          else
          {
              oStartupSB.Append("if(!window.ComponentArt_ColorPicker_Loaded || !window.ComponentArt_Utils_Loaded)\n");
          }
          oStartupSB.Append("\t{setTimeout('ComponentArt_Init_" + sColorPickerVarName + "()', 100); return; }\n\n");

          // Instantiate object
          oStartupSB.Append("window." + sColorPickerVarName + " = new ComponentArt_ColorPicker('" + sColorPickerVarName + "');\n");

          // Hook the actual ID if available and different from effective client ID
          if (this.ID != sColorPickerVarName)
          {
            oStartupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sColorPickerVarName + "; " + sColorPickerVarName + ".GlobalAlias = '" + ID + "'; }\n");
          }

          oStartupSB.Append(sColorPickerVarName + ".ControlId = '" + this.UniqueID + "';\n");

          oStartupSB.Append("var properties = [\n");
          oStartupSB.Append("['ApplicationPath'," + Utils.ConvertStringToJSString(Context.Request.ApplicationPath) + "],");
          oStartupSB.Append("['Height','" + this.Height.ToString() + "'],");
          oStartupSB.Append("['Width','" + this.Width.ToString() + "'],");
          oStartupSB.Append("['Mode','" + this.Mode.ToString() + "'],");
          oStartupSB.Append("['Colors'," + ColorsArrayString + "],");
          oStartupSB.Append("['ColorCssClass','" + this.ColorCssClass + "'],");
          oStartupSB.Append("['ColorHoverCssClass','" + this.ColorHoverCssClass + "'],");
          oStartupSB.Append("['ColorActiveCssClass','" + this.ColorActiveCssClass + "'],");
          oStartupSB.Append("['CustomColorCssClass','" + this.CustomColorCssClass + "'],");
          oStartupSB.Append("['CustomColorHoverCssClass','" + this.CustomColorHoverCssClass + "'],");
          oStartupSB.Append("['CustomColorActiveCssClass','" + this.CustomColorActiveCssClass + "'],");
          oStartupSB.Append("['CrosshairOffsetX'," + this.CrosshairOffsetX.ToString() + "],");
          oStartupSB.Append("['CrosshairOffsetY'," + this.CrosshairOffsetY.ToString() + "],");
          oStartupSB.Append("['SelectedColor'," + ((this.SelectedColor == null) ? "null" : "new ComponentArt.Web.UI.ColorPickerColor('" + this.SelectedColor.Hex + "')") + "],");
          oStartupSB.Append("['SelectedColorSwatch',"+this.SelectedColorSwatch.ToString()+"],");
          oStartupSB.Append("['SelectedCustomColorSwatch'," + this.SelectedCustomColorSwatch.ToString() + "],");
          oStartupSB.Append("['CustomColors'," + CustomColorsArrayString + "],");
          oStartupSB.Append("['ClientEvents'," + Utils.ConvertClientEventsToJsObject(this._clientEvents) + "]");
          oStartupSB.Append("];\n");

          oStartupSB.AppendFormat("ComponentArt_SetProperties({0}, properties);\n", sColorPickerVarName);

          oStartupSB.Append(sColorPickerVarName + ".Initialize();\n}\n");

          oStartupSB.Append("ComponentArt_Init_" + sColorPickerVarName + "();");

          WriteStartupScript(output, this.DemarcateClientScript(oStartupSB.ToString()));
        }

        protected override void OnLoad(EventArgs e)
        {
          base.OnLoad(e);
          EnsureChildControls();
            }

        protected override object SaveViewState()
        {
          ViewState["ColorPicker"] = "ForceViewState";
          return base.SaveViewState();
        }

        protected override void LoadViewState(object state)
        {
          base.LoadViewState(state);

          string sColorPickerVarName = this.GetSaneId();

          // Load client data
          string input_customcolors = Context.Request.Form[sColorPickerVarName + "_CustomColors"];
            
          if (input_customcolors != null)
          {
              this.CustomColors.Clear();

              string[] input_customcolors_array = input_customcolors.Split(',');

              foreach (string input in input_customcolors_array)
              {
                  if (input.Split('|')[0].Length >= 2)
                  {
                      ColorPickerColor colorObj = new ColorPickerColor();
                      if (input.Split('|')[0].Replace("'", "").Length >= 6)
                      {
                          colorObj.Hex = input.Split('|')[0];
                      }
                      else
                      {
                          colorObj.SystemDrawingColor = System.Drawing.Color.Empty;
                      }

                      this.CustomColors.Add(colorObj);
                  }
              }
          }

          string input_selectedcolorswatch = Context.Request.Form[sColorPickerVarName + "_SelectedColorSwatch"];
          if (input_selectedcolorswatch != null)
              this.SelectedColorSwatch = Convert.ToInt16(input_selectedcolorswatch);

          string input_selectedcustomcolorswatch = Context.Request.Form[sColorPickerVarName + "_SelectedCustomColorSwatch"];
          if (input_selectedcustomcolorswatch != null)
              this.SelectedCustomColorSwatch = Convert.ToInt16(input_selectedcustomcolorswatch);


          string input_selectedcolor = Context.Request.Form[sColorPickerVarName + "_SelectedColor"];
          if (input_selectedcolor != null && input_selectedcolor.Length >= 6)
          {
              ColorPickerColor colorObj = new ColorPickerColor();
              colorObj.Hex = input_selectedcolor;
              this.SelectedColor = colorObj;
          }
        }

        private void RenderAccessible(HtmlTextWriter output)
        {
            // accessibility output
        }

        internal void RenderDownLevel(HtmlTextWriter output)
        {
            // downlevel output
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
