using System;
using System.Web.UI;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace ComponentArt.Web.UI
{
  [GuidAttribute("53f586d7-6911-4cb0-82bd-564a9f882220")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [PersistChildren(false)]
  [ParseChildren(true)]
  [ValidationProperty("ValidationText")]
  public abstract class BaseInput : ComponentArt.Web.UI.WebControl
  {

    protected Hashtable ClientProperties = new Hashtable();

    protected virtual void PopulateClientProperties()
    {
      this.ClientProperties.Clear();
      this.ClientProperties.Add("ContainerId", Utils.ConvertStringToJSString(this.ContainerId));
      this.ClientProperties.Add("CssClass", Utils.ConvertStringToJSString(this.CssClass));
      this.ClientProperties.Add("DisabledCssClass", Utils.ConvertStringToJSString(this.DisabledCssClass));
      this.ClientProperties.Add("DisplayInputId", Utils.ConvertStringToJSString(this.DisplayInputId));
      this.ClientProperties.Add("EmptyCssClass", Utils.ConvertStringToJSString(this.EmptyCssClass));
      this.ClientProperties.Add("EmptyText", Utils.ConvertStringToJSString(this.EmptyText));
      this.ClientProperties.Add("Enabled", this.Enabled.ToString().ToLower());
      this.ClientProperties.Add("FocusedCssClass", Utils.ConvertStringToJSString(this.FocusedCssClass));
      this.ClientProperties.Add("FocusedValidCssClass", Utils.ConvertStringToJSString(this.FocusedValidCssClass));
      this.ClientProperties.Add("InvalidCssClass", Utils.ConvertStringToJSString(this.InvalidCssClass));
      this.ClientProperties.Add("MaxLength", this.MaxLength.ToString());
      this.ClientProperties.Add("Size", this.Size.ToString());
      this.ClientProperties.Add("ToolTip", Utils.ConvertStringToJSString(this.ToolTip));
      this.ClientProperties.Add("ValidationInputId", Utils.ConvertStringToJSString(this.ValidationInputId));
      this.ClientProperties.Add("ValueInputId", Utils.ConvertStringToJSString(this.ValueInputId));
    }

    protected string GeneratePropertyStorage()
    {
      String[] propertyArray = new String[this.ClientProperties.Count];
      int i = 0;
      foreach (string propertyName in this.ClientProperties.Keys)
      {
        propertyArray[i] = "['" + propertyName + "', " + this.ClientProperties[propertyName] + "]";
        i++;
      }
      return "[\n" + String.Join(",\n", propertyArray) + "\n]";
    }

    internal void RenderLayoutSettings(HtmlTextWriter output)
    {
      if (this.DisplayText != null)
      {
        output.Write(" value=\"" + this.DisplayText + "\"");
      }
      if (!this.Enabled)
      {
        output.Write(" disabled=\"disabled\"");
      }

      if (this.MaxLength > 0)
      {
        output.Write(" maxlength=\"" + this.MaxLength.ToString() + "\"");
      }
      if (this.Size > 0)
      {
        output.Write(" size=\"" + this.Size.ToString() + "\"");
      }
      if (this.TabIndex != 0)
      {
        output.Write(" tabindex=\"" + this.TabIndex.ToString() + "\"");
      }

      output.Write(" style=\"");
      if (!this.BackColor.IsEmpty)
      {
        output.WriteStyleAttribute("background-color", System.Drawing.ColorTranslator.ToHtml(this.BackColor));
      }
      if (!this.BorderColor.IsEmpty)
      {
        output.WriteStyleAttribute("border-color", System.Drawing.ColorTranslator.ToHtml(this.BorderColor));
      }
      if (this.BorderStyle != BorderStyle.NotSet)
      {
        output.WriteStyleAttribute("border-style", this.BorderStyle.ToString());
      }
      if (!this.BorderWidth.IsEmpty)
      {
        output.WriteStyleAttribute("border-width", this.BorderWidth.ToString());
      }
      if (this.Font.Bold)
      {
        output.WriteStyleAttribute("font-weight", "bold");
      }
      if (this.Font.Italic)
      {
        output.WriteStyleAttribute("font-style", "italic");
      }
      if (this.Font.Names.Length > 0)
      {
        output.WriteStyleAttribute("font-family", String.Join(",", this.Font.Names));
      }
      if (!this.Font.Size.IsEmpty)
      {
        output.WriteStyleAttribute("font-size", this.Font.Size.ToString());
      }
      if (this.Font.Underline)
      {
        output.WriteStyleAttribute("text-decoration", "underline");
      }
      if (!this.Height.IsEmpty)
      {
        output.WriteStyleAttribute("height", this.Height.ToString());
      }
      if (!this.ForeColor.IsEmpty)
      {
        output.WriteStyleAttribute("color", System.Drawing.ColorTranslator.ToHtml(this.ForeColor));
      }
      if (!this.Width.IsEmpty)
      {
        output.WriteStyleAttribute("width", this.Width.ToString());
      }
      output.Write("\"");
    }

    internal System.Web.UI.AttributeCollection _properties;
    internal System.Web.UI.AttributeCollection Properties
    {
      get
      {
        if (_properties == null)
        {
          StateBag oBag = new StateBag(true);
          _properties = new System.Web.UI.AttributeCollection(oBag);
        }
        return _properties;
      }
      set
      {
        _properties = value;
      }
    }

    protected override object SaveViewState()
    {
      if (this.EnableViewState)
      {
        ViewState["EnableViewState"] = true; // dummy just to ensure we have a ViewState.
      }
      return base.SaveViewState();
    }

    protected String ContainerId
    { 
      get
      {
        return this.GetSaneId() + "_container";
      }
    }

    /// <summary>
    /// The masked text as displayed on the client.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If it is necessary to access the masked text, it is contained in this property. The unmasked text is accessed through the
    /// <see cref="MaskedInput.Text" /> and <see cref="NumberInput.Value" /> for the <see cref="MaskedInput" /> and
    /// <see cref="NumberInput" /> respectively.
    /// </para>
    /// </remarks>
    [Browsable(false)]
    public String DisplayText
    {
      get
      {
        return Context == null ? null : Context.Request.Form[this.DisplayInputId];
      }
    }

    protected String DisplayInputId
    {
      get
      {
        return this.GetSaneId() + "_masked";
      }
    }

    /// <summary>
    /// Gets or sets the maximum number of characters allowed in the input box.  Default is zero, indicating there is no limitation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this property limits the number of characters which can be entered by the user. This is different than the
    /// behavior of the <see cref="BaseInput.Size" /> property which limits the rendered size of the associated input element.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    public int MaxLength
    {
      get
      {
        return Utils.ParseInt(Properties["MaxLength"], 0);
      }
      set
      {
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException("value", "MaxLength cannot be less than zero.");
        }
        else
        {
          Properties["MaxLength"] = value.ToString();
        }
      }
    }

    /// <summary>
    /// Gets or sets the size of the input box in characters.  Default is zero, indicating the size is not set.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this property limits the actual rendered size of the associated input element. This is different
    /// than the <see cref="BaseInput.MaxLength" /> property which limits the number of characters which can be entered
    /// by the user.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    public int Size
    {
      get
      {
        return Utils.ParseInt(Properties["Size"], 0);
      }
      set
      {
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException("value", "Size cannot be less than zero.");
        }
        else
        {
          Properties["Size"] = value.ToString();
        }
      }
    }

    /// <summary>
    /// Gets or sets the text displayed when the mouse pointer hovers over the input box.
    /// </summary>
    [DefaultValue("")]
    public override string ToolTip
    {
      get
      {
        object o = Properties["ToolTip"];
        return (o == null) ? "" : o.ToString();
      }
      set
      {
        Properties["ToolTip"] = value;
      }
    }

    /// <summary>
    /// Used by ASP.NET validators.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is accessed by ASP.NET validators, and contains the text which will be validated. 
    /// When the <see cref="MaskedInput" /> is used, the value of the <see cref="MaskedInput.ValidateMasked" />
    /// property determines whether the masked or unmasked input is validated. The default is to validate
    /// the masked input.
    /// </para>
    /// </remarks>
    [Browsable(false)]
    public String ValidationText
    {
      get
      {
        return Context == null ? null : Context.Request.Form[this.ValidationInputId];
      }
    }

    protected String ValidationInputId
    {
      get
      {
        return this.GetSaneId();
      }
    }

    protected String ValueInputId
    {
      get
      {
        return this.GetSaneId() + "_unmasked";
      }
    }

    /// <summary>
    /// CSS class of the control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this property styles a span element which surrounds the actual input box. 
    /// This is not often an issue, but it should be remembered that if the actual input box needs to be
    /// styled, a selector such as <code>.myInputClass input</code> should be used.
    /// </para>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_How_to_Style.htm">Input Style Properties</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    public override string CssClass
    {
      get
      {
        return Properties["CssClass"];
      }
      set
      {
        Properties["CssClass"] = value;
      }
    }

    /// <summary>
    /// CSS class to apply to the Input control when the control is disabled.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this property styles a span element which surrounds the actual input box. 
    /// This is not often an issue, but it should be remembered that if the actual input box needs to be
    /// styled, a selector such as <code>.myInputClass input</code> should be used.
    /// </para>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_How_to_Style.htm">Input Style Properties</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    public string DisabledCssClass
    {
      get
      {
        return Properties["DisabledCssClass"];
      }
      set
      {
        Properties["DisabledCssClass"] = value;
      }
    }

    /// <summary>
    /// CSS class to apply to the Input control when it doesn't have focus and its value is empty.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this property styles a span element which surrounds the actual input box. 
    /// This is not often an issue, but it should be remembered that if the actual input box needs to be
    /// styled, a selector such as <code>.myInputClass input</code> should be used.
    /// </para>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_How_to_Style.htm">Input Style Properties</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    public string EmptyCssClass
    {
      get
      {
        return Properties["EmptyCssClass"];
      }
      set
      {
        Properties["EmptyCssClass"] = value;
      }
    }

    /// <summary>
    /// The text to display in the input box when nothing has been entered.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to provide a "watermark" for the input. It can be used
    /// as a suggestion to the user for what should be entered into the box. The text specified
    /// by this property will disappear when the input receives focus. To style this text, use the
    /// <see cref="BaseInput.EmptyCssClass" /> property.
    /// </para>
    /// </remarks>
    public string EmptyText
    {
      get
      {
        return Properties["EmptyText"];
      }
      set
      {
        Properties["EmptyText"] = value;
      }
    }

    /// <summary>
    /// Whether the control is enabled or disabled.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="DisabledCssClass" /> is responsible for styling the control when it is in a disabled state.
    /// </para>
    /// </remarks>
    public override bool Enabled
    {
      get
      {
        return Utils.ParseBool(Properties["Enabled"], true);
      }
      set
      {
        Properties["Enabled"] = value.ToString();
      }
    }

    /// <summary>
    /// CSS class to apply to the Input control when it has focus and its value is not valid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this property styles a span element which surrounds the actual input box. 
    /// This is not often an issue, but it should be remembered that if the actual input box needs to be
    /// styled, a selector such as <code>.myInputClass input</code> should be used.
    /// </para>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_How_to_Style.htm">Input Style Properties</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    public string FocusedCssClass
    {
      get
      {
        return Properties["FocusedCssClass"];
      }
      set
      {
        Properties["FocusedCssClass"] = value;
      }
    }

    /// <summary>
    /// CSS Class to apply to the Input control when it has focus and its value is valid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this property styles a span element which surrounds the actual input box. 
    /// This is not often an issue, but it should be remembered that if the actual input box needs to be
    /// styled, a selector such as <code>.myInputClass input</code> should be used.
    /// </para>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_How_to_Style.htm">Input Style Properties</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    public string FocusedValidCssClass
    {
      get
      {
        return Properties["FocusedValidCssClass"];
      }
      set
      {
        Properties["FocusedValidCssClass"] = value;
      }
    }

    /// <summary>
    /// CSS Class to apply to the Input control when its value is not valid.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this property styles a span element which surrounds the actual input box. 
    /// This is not often an issue, but it should be remembered that if the actual input box needs to be
    /// styled, a selector such as <code>.myInputClass input</code> should be used.
    /// </para>
    /// <para>
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_How_to_Style.htm">Input Style Properties</a> tutorial
    /// for more information.
    /// </para>
    /// </remarks>
    public string InvalidCssClass
    {
      get
      {
        return Properties["InvalidCssClass"];
      }
      set
      {
        Properties["InvalidCssClass"] = value;
      }
    }

    protected override bool IsDownLevel()
    {
      return false;
    }

  }
}
