using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Text;
using System.Globalization;
using System.Collections;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Control providing validation and formatting for strictly numeric input.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Like the <see cref="MaskedInput" /> class, <code>NumberInput</code> is under the Web.UI Input umbrella. Both
  /// are responsible for accepting, validating, and formatting user Input, but they do so in entirely different ways.
  /// The <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_Masked_Number_Intro.htm">MaskedInput vs. NumberInput</a> tutorial
  /// discusses the differences and basic functionality of both.
  /// </para>
  /// <para>
  /// As the name implies, NumberInput is responsible for formatting purely numeric input. One powerful aspect of the
  /// control is its integration with .NET's <see cref="CultureInfo" /> class, and the corresponding <see cref="NumberFormatInfo" />.
  /// This provides powerful yet easy to implement localization capabilities. The <see cref="NumberInput.Culture" /> and
  /// <see cref="NumberInput.NumberFormat" /> properties are used to take advantage of localization for an instance of this class.
  /// In addition, all of the default settings can be further customized with properties like <see cref="NumberInput.NegativePattern" />
  /// <see cref="NumberInput.PositivePattern" />.
  /// </para>
  /// <para>
  /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_Number_Intro.htm">Implementing a NumberInput</a> tutorial for information
  /// on getting started with the control. The <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_How_to_Style.htm">Input Style Properties</a>
  /// tutorial contains a brief introduction to styling both <code>NumberInput</code> and <code>MaskedInput</code> instances.
  /// </para>
  /// </remarks>
  [PersistChildren(false)]
  [ParseChildren(true)]
  public sealed class NumberInput : BaseInput
  {

    /// <summary>
    /// Constructor for the NumberInput control.
    /// </summary>
    public NumberInput()
    {
      this.NumberFormat = NumberFormatInfo.CurrentInfo;
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {

      if (!this.IsDownLevel() && Page != null)
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
          if (!Page.IsClientScriptBlockRegistered("A579I433.js"))
          {
            Page.RegisterClientScriptBlock("A579I433.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Input.client_scripts", "A579I433.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A579I432.js"))
          {
            Page.RegisterClientScriptBlock("A579I432.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Input.client_scripts", "A579I432.js");
          }
        }
      }


      string clientControlId = this.GetSaneId();

      output.Write("<span");
      output.Write(" id=\"" + this.ContainerId + "\"");
      output.Write(" class=\"" + ((!this.Enabled && this.DisabledCssClass != null) ? this.DisabledCssClass : this.CssClass) + "\"");
      if (!this.Visible)
      {
        output.Write(" style=\"visibility:hidden;\"");
      }
      output.Write(">");

      output.Write("<input type=\"hidden\"");
      output.Write(" id=\"" + this.ValueInputId + "\"");
      output.Write(" name=\"" + this.ValueInputId + "\"");
      if (this.ValueDefined)
      {
        output.Write(" value=\"" + System.Web.HttpUtility.HtmlEncode(this.Value.Value.ToString(NumberFormatInfo.InvariantInfo)) + "\"");
      }
      output.Write(" />");

      output.Write("<input type=\"text\"");
      output.Write(" id=\"" + this.ValidationInputId + "\"");
      output.Write(" name=\"" + this.ValidationInputId + "\"");
      output.Write(" style=\"display:none;\"");
      if (this.ValidationText != null)
      {
        output.Write(" value=\"" + this.ValidationText + "\"");
      }
      output.Write(" />");

      output.Write("<input type=\"text\"");
      output.Write(" id=\"" + this.DisplayInputId + "\"");
      output.Write(" name=\"" + this.DisplayInputId + "\"");
      // Prevent FF from showing input text suggestions
      output.Write(" autocomplete=\"off\"");

      this.RenderLayoutSettings(output);

      output.Write(" onblur=\"return ComponentArt_NumberInput_Blur(window." + clientControlId + ", event);\"");
      output.Write(" onclick=\"return ComponentArt_NumberInput_Click(window." + clientControlId + ", event);\"");
      output.Write(" oncut=\"return ComponentArt_NumberInput_Cut(window." + clientControlId + ", event);\"");
      output.Write(" onfocus=\"return ComponentArt_NumberInput_Focus(window." + clientControlId + ", event);\"");
      output.Write(" onkeydown=\"return ComponentArt_NumberInput_KeyDown(window." + clientControlId + ", event);\"");
      output.Write(" onkeypress=\"return ComponentArt_NumberInput_KeyPress(window." + clientControlId + ", event);\"");
      output.Write(" onkeyup=\"return ComponentArt_NumberInput_KeyUp(window." + clientControlId + ", event);\"");
      output.Write(" onpaste=\"return ComponentArt_NumberInput_Paste(window." + clientControlId + ", event);\"");
      output.Write(" />");

      output.Write("</span>");

      this.WriteStartupScript(output, this.GenerateClientSideIntializationScript(clientControlId));
    }

    private string GenerateClientSideIntializationScript(string clientControlId)
    {
      StringBuilder scriptSB = new StringBuilder();
      scriptSB.Append("window.ComponentArt_Init_" + clientControlId + " = function() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      int retryDelay = 100; // 100 ms retry time sounds about right
      string areScriptsLoaded = "(window.cart_numberinput_kernel_loaded && window.cart_numberinput_support_loaded)";
      scriptSB.Append("if (!" + areScriptsLoaded + ")\n");
      scriptSB.Append("{\n\tsetTimeout('ComponentArt_Init_" + clientControlId + "()', " + retryDelay.ToString() + ");\n\treturn;\n}\n");

      // Instantiate the client-side object
      scriptSB.Append("window." + clientControlId + " = new ComponentArt_NumberInput('" + clientControlId + "');\n");

      // Write postback function reference
      if (this.Page != null)
      {
        scriptSB.Append(clientControlId + ".Postback = function() { " + this.Page.GetPostBackEventReference(this) + " };\n");
      }

      // Hook the actual ID if available and different from effective client ID
      if (this.ID != clientControlId)
      {
        scriptSB.Append("if(!window['" + this.ID + "']) { window['" + this.ID + "'] = window." + clientControlId + "; " + clientControlId + ".GlobalAlias = '" + this.ID + "'; }\n");
      }

      // Output client property settings
      this.PopulateClientProperties();
      scriptSB.Append(clientControlId + ".LoadProperties(" + this.GeneratePropertyStorage() + ");\n");

      // Add us to the client viewstate-saving mechanism
      if (this.EnableViewState)
      {
        scriptSB.Append("ComponentArt_ClientStateControls[ComponentArt_ClientStateControls.length] = " + clientControlId + ";\n");
      }

      // Initialize the client-side object
      scriptSB.Append(clientControlId + ".Initialize();\n");

      // Render the Input
      scriptSB.Append(clientControlId + ".Render();\n");

      // Set the flag that the control has been initialized.  This is the last action in the initialization.
      scriptSB.Append("window." + clientControlId + "_loaded = true;\n}\n");

      // Call this initialization function.  Remember that it will be repeated after a delay if it's not all ready.
      scriptSB.Append("ComponentArt_Init_" + clientControlId + "();");

      return this.DemarcateClientScript(scriptSB.ToString(), "ComponentArt_NumberInput_Startup_" + clientControlId + " " + this.VersionString());
    }

    protected override void OnLoad(System.EventArgs e)
    {
      base.OnLoad(e);

      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Input.client_scripts.A579I433.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Input.client_scripts.A579I432.js");
      }

      //TODO: If we introduce this piece of code, we must modify it to allow for Page.Header being null (when <head> is not runat="server")
      //if (this.NeedsDefaultStyle())
      //{
      //  HtmlLink link = new HtmlLink();
      //  link.Attributes.Add("type", "text/css");
      //  link.Attributes.Add("rel", "stylesheet");
      //  link.Attributes.Add("href", this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ComponentArt.Web.UI.Input.defaultStyle.css"));
      //  this.Page.Header.Controls.Add(link);
      //}

    }

    protected override void PopulateClientProperties()
    {
      base.PopulateClientProperties();
      this.ClientProperties.Add("ClientEvents", Utils.ConvertClientEventsToJsObject(this.ClientEvents));
      this.ClientProperties.Add("DecimalDigits", this.DecimalDigits.ToString());
      this.ClientProperties.Add("DecimalSeparator", Utils.ConvertStringToJSString(this.DecimalSeparator));
      this.ClientProperties.Add("GroupSeparator", Utils.ConvertStringToJSString(this.GroupSeparator));
      this.ClientProperties.Add("GroupSize", this.GroupSize.ToString());
      this.ClientProperties.Add("Increment", this.Increment.ToString(CultureInfo.InvariantCulture));
      this.ClientProperties.Add("MaxValue", this.MaxValue.ToString(CultureInfo.InvariantCulture));
      this.ClientProperties.Add("MinValue", this.MinValue.ToString(CultureInfo.InvariantCulture));
      this.ClientProperties.Add("MinWholeDigits", this.MinWholeDigits.ToString());
      this.ClientProperties.Add("NegativePattern", Utils.ConvertStringToJSString(this.NegativePattern));
      this.ClientProperties.Add("PositivePattern", Utils.ConvertStringToJSString(this.PositivePattern));
      this.ClientProperties.Add("Step", this.Step.ToString(CultureInfo.InvariantCulture));
      this.ClientProperties.Add("Value", Utils.ConvertNullableDoubleToJSString(this.Value));
      this.ClientProperties.Add("ValueDefined", this.ValueDefined.ToString().ToLower());
    }


    private NumberFormatInfo _numberFormat;
    /// <summary>
    /// Gets or sets a <see cref="NumberFormatInfo"/> that defines the format of displayed numbers.
    /// </summary>
    /// <value>
    /// A <see cref="NumberFormatInfo"/> that defines the format of displayed numbers.
    /// </value>
    /// <remarks>
    /// <para>
    /// When setting this property, a number of input's other properties will be set to the corresponding value from the
    /// given <see cref="NumberFormatInfo"/>.  These include: <see cref="DecimalDigits"/>, <see cref="DecimalSeparator"/>, 
    /// <see cref="GroupSeparator"/>, <see cref="GroupSize"/>, <see cref="NegativePattern"/>, and <see cref="PositivePattern"/>.
    /// </para>
    /// <para>
    /// The <see cref="NumberInput.Culture" /> property can also be used to set this properly indirectly, based
    /// on the appropriate culture. 
    /// </para>
    /// </remarks>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public NumberFormatInfo NumberFormat
    {
      get
      {
        return _numberFormat;
      }
      set
      {
        this._numberFormat = value;
        switch (this.NumberType)
        {
          case NumberType.Number:
            this.DecimalDigits = value.NumberDecimalDigits;
            this.DecimalSeparator = value.NumberDecimalSeparator;
            this.GroupSeparator = value.NumberGroupSeparator;
            this.GroupSize = value.NumberGroupSizes[0];
            this.NegativePattern = GetNegativePattern(value, NumberType.Number);
            this.PositivePattern = GetPositivePattern(value, NumberType.Number);
            break;

          case NumberType.Currency:
            this.DecimalDigits = value.CurrencyDecimalDigits;
            this.DecimalSeparator = value.CurrencyDecimalSeparator;
            this.GroupSeparator = value.CurrencyGroupSeparator;
            this.GroupSize = value.CurrencyGroupSizes[0];
            this.NegativePattern = GetNegativePattern(value, NumberType.Currency);
            this.PositivePattern = GetPositivePattern(value, NumberType.Currency);
            break;

          case NumberType.Percent:
            this.DecimalDigits = value.PercentDecimalDigits;
            this.DecimalSeparator = value.PercentDecimalSeparator;
            this.GroupSeparator = value.PercentGroupSeparator;
            this.GroupSize = value.PercentGroupSizes[0];
            this.NegativePattern = GetNegativePattern(value, NumberType.Percent);
            this.PositivePattern = GetPositivePattern(value, NumberType.Percent);
            break;
        }
      }
    }

    /// <summary>
    /// Indicates the number of decimal places to use in number values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Default value is <see cref="Int32.MaxValue"/>, indicating that the number of decimal places is not enforced.
    /// If a value is set for <see cref="NumberInput.NumberFormat" />, it can provide a different default for this property.
    /// If a value is set for this property, however, it will override the value provided by the current <code>NumberFormatInfo</code>.
    /// </para>
    /// <para>
    /// Values less than zero are not supported.
    /// </para>
    /// </remarks>
    [DefaultValue(Int32.MaxValue)]
    public int DecimalDigits
    {
      get
      {
        return Utils.ParseInt(Properties["DecimalDigits"], Int32.MaxValue);
      }
      set
      {
        Properties["DecimalDigits"] = value.ToString();
      }
    }

    /// <summary>
    /// Gets or sets the string to use as the decimal separator in number values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default for this property depends on the current value of <see cref="NumberInput.NumberFormat" />,
    /// which is responsible for localization, as well as the value of the <see cref="NumberInput.NumberType" /> property. 
    /// If a value is set for this property, however, it will override the default provided by the <see cref="NumberFormatInfo" /> class.
    /// </para>    
    /// </remarks>
    public string DecimalSeparator
    {
      get
      {
        return Properties["DecimalSeparator"];
      }
      set
      {
        Properties["DecimalSeparator"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the string that separates groups of digits to the left of the decimal in number values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default for this property depends on the current value of <see cref="NumberInput.NumberFormat" />,
    /// which is responsible for localization, as well as the value of the <see cref="NumberInput.NumberType" /> property. 
    /// If a value is set for this property, however, it will override the default provided by the <see cref="NumberFormatInfo" /> class.
    /// </para>    
    /// </remarks>
    public string GroupSeparator
    {
      get
      {
        return Properties["GroupSeparator"];
      }
      set
      {
        Properties["GroupSeparator"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the number of digits in each group to the left of the decimal in number values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default for this property depends on the current value of <see cref="NumberInput.NumberFormat" />,
    /// which is responsible for localization, as well as the value of the <see cref="NumberInput.NumberType" /> property. 
    /// If a value is set for this property, however, it will override the default provided by the <see cref="NumberFormatInfo" /> class.
    /// </para>    
    /// </remarks>
    public int GroupSize
    {
      get
      {
        return Utils.ParseInt(Properties["GroupSize"]);
      }
      set
      {
        Properties["GroupSize"] = value.ToString();
      }
    }

    /// <summary>
    /// Gets or sets the format pattern for negative number values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default for this property depends on the current value of <see cref="NumberInput.NumberFormat" />,
    /// which is responsible for localization, as well as the value of the <see cref="NumberInput.NumberType" /> property. 
    /// If a value is set for this property, however, it will override the default provided by the <see cref="NumberFormatInfo" /> class.
    /// </para>    
    /// </remarks>
    public string NegativePattern
    {
      get
      {
        return Properties["NegativePattern"];
      }
      set
      {
        Properties["NegativePattern"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the format pattern for positive number values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default for this property depends on the current value of <see cref="NumberInput.NumberFormat" />,
    /// which is responsible for localization, as well as the value of the <see cref="NumberInput.NumberType" /> property. 
    /// If a value is set for this property, however, it will override the default provided by the <see cref="NumberFormatInfo" /> class.
    /// </para>    
    /// </remarks>
    public string PositivePattern
    {
      get
      {
        return Properties["PositivePattern"];
      }
      set
      {
        Properties["PositivePattern"] = value;
      }
    }

    private string GetNegativePattern(NumberFormatInfo numberFormatInfo, NumberType numberType)
    {
      switch (numberType)
      {
        case NumberType.Number:
          switch (numberFormatInfo.NumberNegativePattern)
          {
            case 0: return "(n)";
            case 1: return "-n";
            case 2: return "- n";
            case 3: return "n-";
            case 4: return "n -";
          }
          break;

        case NumberType.Currency:
          string currencyNegativePattern = "";
          switch (numberFormatInfo.CurrencyNegativePattern)
          {
            case 0: currencyNegativePattern = "($n)"; break;
            case 1: currencyNegativePattern = "-$n"; break;
            case 2: currencyNegativePattern = "$-n"; break;
            case 3: currencyNegativePattern = "$n-"; break;
            case 4: currencyNegativePattern = "(n$)"; break;
            case 5: currencyNegativePattern = "-n$"; break;
            case 6: currencyNegativePattern = "n-$"; break;
            case 7: currencyNegativePattern = "n$-"; break;
            case 8: currencyNegativePattern = "-n $"; break;
            case 9: currencyNegativePattern = "-$ n"; break;
            case 10: currencyNegativePattern = "n $-"; break;
            case 11: currencyNegativePattern = "$ n-"; break;
            case 12: currencyNegativePattern = "$ -n"; break;
            case 13: currencyNegativePattern = "n- $"; break;
            case 14: currencyNegativePattern = "($ n)"; break;
            case 15: currencyNegativePattern = "(n $)"; break;
          }
          return currencyNegativePattern.Replace("$", numberFormatInfo.CurrencySymbol);

        case NumberType.Percent:
          string percentNegativePattern = "";
          switch (numberFormatInfo.PercentNegativePattern)
          {
            case 0: percentNegativePattern = "-n %"; break;
            case 1: percentNegativePattern = "-n%"; break;
            case 2: percentNegativePattern = "-%n"; break;
            case 3: percentNegativePattern = "%-n"; break;
            case 4: percentNegativePattern = "%n-"; break;
            case 5: percentNegativePattern = "n-%"; break;
            case 6: percentNegativePattern = "n%-"; break;
            case 7: percentNegativePattern = "-% n"; break;
            case 8: percentNegativePattern = "n %-"; break;
            case 9: percentNegativePattern = "% n-"; break;
            case 10: percentNegativePattern = "% -n"; break;
            case 11: percentNegativePattern = "n- %"; break;
          }
          return percentNegativePattern.Replace("%", numberFormatInfo.PercentSymbol);
      }

      throw new Exception("Invalid NegativePattern in NumberFormatInfo.");
    }

    private string GetPositivePattern(NumberFormatInfo numberFormatInfo, NumberType numberType)
    {
      switch (numberType)
      {
        case NumberType.Number:
          return "n";

        case NumberType.Currency:
          string currencyPositivePattern = "";
          switch (numberFormatInfo.CurrencyPositivePattern)
          {
            case 0: currencyPositivePattern = "$n"; break;
            case 1: currencyPositivePattern = "n$"; break;
            case 2: currencyPositivePattern = "$ n"; break;
            case 3: currencyPositivePattern = "n $"; break;
          }
          return currencyPositivePattern.Replace("$", numberFormatInfo.CurrencySymbol);

        case NumberType.Percent:
          string percentPositivePattern = "";
          switch (numberFormatInfo.PercentPositivePattern)
          {
            case 0: percentPositivePattern = "n %"; break;
            case 1: percentPositivePattern = "n%"; break;
            case 2: percentPositivePattern = "%n"; break;
            case 3: percentPositivePattern = "% n"; break;
          }
          return percentPositivePattern.Replace("%", numberFormatInfo.PercentSymbol);
      }

      throw new Exception("Invalid PositivePattern in NumberFormatInfo.");
    }

    private NumberType _numberType = NumberType.Number;
    /// <summary>
    /// Determines which type of number is to be displayed (Currency, Percentage, or just a Number).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Many of the default <see cref="NumberInput" /> property values depend on the value of this property. 
    /// Formatting can be further customized beyond the defaults, however, using the various properties. 
    /// Default values are provided by the .NET <see cref="NumberFormatInfo" /> class, providing built-in
    /// localization for <code>NumberInput</code> instances.
    /// </para>
    /// </remarks>
    [DefaultValue(NumberType.Number)]
    public NumberType NumberType
    {
      get
      {
        return this._numberType;
      }
      set
      {
        this._numberType = value;
        this.NumberFormat = this.NumberFormat; // Force a recalculation of the values dependent on NumberFormat
      }
    }

    private NumberInputClientEvents _clientEvents = null;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public NumberInputClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new NumberInputClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// This is a set-only property enabling you to set the Input's <see cref="NumberFormat"/> to the given culture's <see cref="NumberFormatInfo" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is simply a means of specifying the <see cref="CultureInfo" /> which will be used in conjunction with the
    /// <see cref="NumberInput.NumberType" /> property to determine the default number formatting. As the <see cref="NumberInput" /> is exclusively 
    /// responsible for formatting numeric input, the <see cref="NumberInput.NumberFormat" /> property can be used to 
    /// specify the appropriate <see cref="NumberFormatInfo" /> directly.
    /// </para>
    /// </remarks>
    /// <seealso cref="CultureId" />
    /// <seealso cref="CultureName" />
    /// <seealso cref="NumberFormat" />
    public CultureInfo Culture
    {
      set
      {
        this.NumberFormat = value.NumberFormat;
      }
    }

    /// <summary>
    /// This is a set-only property enabling you to set the Input's <see cref="NumberFormatInfo"/> to the given culture's <b>NumberFormatInfo</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property allows the <see cref="CultureInfo" /> to be set for the control, using the ID. The <code>CultureInfo</code>
    /// is used to specify the <see cref="NumberFormatInfo" /> which will be used in conjunction with the <see cref="NumberInput.NumberType" />
    /// property to determine the default number formatting. As the <see cref="NumberInput" /> is exclusively responsible for formatting
    /// numeric input, the <see cref="NumberInput.NumberFormat" /> property can be used to specify the appropriate <see cref="NumberFormatInfo" />
    /// directly.
    /// </para>
    /// </remarks>
    /// <seealso cref="Culture" />
    /// <seealso cref="CultureName" />
    /// <seealso cref="NumberFormat" />
    public int CultureId
    {
      set
      {
        this.NumberFormat = new CultureInfo(value, false).NumberFormat;
      }
    }

    /// <summary>
    /// This is a set-only property enabling you to set the Input's <see cref="NumberFormat"/> to the given culture's <b>NumberFormat</b>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property allows the <see cref="CultureInfo" /> for the control to be set by name. The <code>CultureInfo</code>
    /// is used to specify the <see cref="NumberFormatInfo" /> which will be used in conjunction with the <see cref="NumberInput.NumberType" />
    /// property to determine the default number formatting. As the <see cref="NumberInput" /> is exclusively responsible for formatting
    /// numeric input, the <see cref="NumberInput.NumberFormat" /> property can be used to specify the appropriate <code>NumberFormatInfo</code>
    /// directly. 
    /// </para>
    /// </remarks>
    /// <seealso cref="Culture" />
    /// <seealso cref="CultureId" />
    /// <seealso cref="NumberFormat" />
    public string CultureName
    {
      set
      {
        this.NumberFormat = new CultureInfo(value, false).NumberFormat;
      }
    }

    /// <summary>
    /// This value is the increment by which the number value is changed when up/down arrows are clicked.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The client-side <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~NumberInput_increaseValue_method.htm">increaseValue</a>
    /// and <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~NumberInput_decreaseValue_method.htm">decreaseValue</a>
    /// methods are used to increase or decrease the control's <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~NumberInput_value_property.htm">value</a>
    /// on the client-side. This property determines the amount which the value will change every time one of those methods is called.
    /// </para>
    /// <para>
    /// If <see cref="Step"/> is nonzero, Increment should logically be a multiple of Step.
    /// </remarks>
    [DefaultValue(1)]
    public double Increment
    {
      get
      {
        return Utils.ParseDouble(Properties["Increment"], 1);
      }
      set
      {
        Properties["Increment"] = value.ToString();
      }
    }

    private bool _loadedValueFromForm = false;

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      if (Context != null && Context.Request.Form[this.ValueInputId] != null)
      {
        this.Properties["Value"] = Context.Request.Form[this.ValueInputId];
        this._loadedValueFromForm = true;
      }
    }

    protected override object SaveViewState()
    {
      // Save State as a cumulative array of objects.
      object baseState = base.SaveViewState();
      object[] allStates = new object[2];
      allStates[0] = baseState;
      allStates[1] = Utils.ConvertNullableDoubleToJSString(this.Value);
      return allStates;
    }

    protected override void LoadViewState(object savedState)
    {
      if (savedState != null)
      {
        // Load State from the array of objects that was saved in SaveViewState
        object[] myState = (object[])savedState;
        if (myState[0] != null)
        {
          base.LoadViewState(myState[0]);
        }
        if (myState[1] != null)
        {
          if (!this._loadedValueFromForm)
          {
            this.Value = Utils.ParseNullableDouble(myState[1]);
          }
        }
      }
    }

    // JavaScript range is supposed to correspond exactly to .NET's double.MinValue to double.MaxValue, 
    // but because of rounding, things don't work out like that.  So we use these slightly modified values.
    private double BoundDoubleForJavaScript(double value)
    {
      double jsMaxValue = 1.79769313486231E+308;
      double jsMinValue = -1.79769313486231E+308;
      return Math.Min(jsMaxValue, Math.Max(jsMinValue, value));
    }

    /// <summary>
    /// Minimum that the <see cref="Value"/> can be.
    /// </summary>
    [DefaultValue(double.MinValue)]
    public double MinValue
    {
      get
      {
        return BoundDoubleForJavaScript(Utils.ParseDouble(Properties["MinValue"], double.MinValue));
      }
      set
      {
        Properties["MinValue"] = BoundDoubleForJavaScript(value).ToString();
      }
    }

    /// <summary>
    /// Indicates the minimum number of whole digits to show.  Setting this value allows for zero-padding the displayed number.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Default value is 0, indicating that the number should not be padded.
    /// If a value is set to a positive number, the displayed numbers with fewer digits will be zero-padded to the set length.
    /// </para>
    /// <para>
    /// Values of less than zero are not supported.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    public int MinWholeDigits
    {
      get
      {
        return Utils.ParseInt(Properties["MinWholeDigits"], 0);
      }
      set
      {
        Properties["MinWholeDigits"] = value.ToString();
      }
    }

    /// <summary>
    /// Maximum that the <see cref="Value"/> can be.
    /// </summary>
    [DefaultValue(double.MaxValue)]
    public double MaxValue
    {
      get
      {
        return BoundDoubleForJavaScript(Utils.ParseDouble(Properties["MaxValue"], double.MaxValue));
      }
      set
      {
        Properties["MaxValue"] = BoundDoubleForJavaScript(value).ToString();
      }
    }

    /// <summary>
    /// This value is the increment by which the number value is able to change.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If Step is nonzero, the <see cref="Value"/> is always forced to be a multiple of Step.
    /// If Step is zero, no such restrictions are placed on Value.
    /// If Step is nonzero, <see cref="Increment"/> should logically be a multiple of Step.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    public double Step
    {
      get
      {
        return Utils.ParseDouble(Properties["Step"], 0);
      }
      set
      {
        Properties["Step"] = value.ToString();
      }
    }


    /// <summary>
    /// Number value of the control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this is the unmasked value. To access the masked value, use the <see cref="BaseInput.DisplayText" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(null)]
    public double? Value
    {
      get
      {
        return Utils.ParseNullableDouble(Properties["Value"]);
      }
      set
      {
        Properties["Value"] = (Double.Parse(value.ToString(), CultureInfo.CurrentCulture)).ToString(CultureInfo.InvariantCulture);
      }
    }

    /// <summary>
    /// Read-only property, that is true if Value is set.
    /// </summary>
    public bool ValueDefined
    {
      get
      {
        return this.Value.HasValue;
      }
    }



  }

  /// <summary>
  /// Determines the type of number to display.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used for the value of the <see cref="NumberInput.NumberType" /> property. It is used
  /// in conjunction with the <see cref="NumberInput.NumberFormat" /> to determine how to format the input number.
  /// </para>
  /// </remarks>
  public enum NumberType
  {
    /// <summary>Represents a standard number</summary>
    Number,
    /// <summary>Represents currency. Will result in appropriate formatting for the culture.</summary>
    Currency,
    /// <summary>Represents a percentage.</summary>
    Percent
  }

}
