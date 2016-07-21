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
  /// Control providing validation and formatting for input.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Like the <see cref="NumberInput" /> class, <code>MaskedInput</code> is under the Web.UI Input umbrella. Both
  /// are responsible for accepting, validating, and formatting user input, but they do so in entirely different ways.
  /// The <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_Masked_Number_Intro.htm">MaskedInput vs. NumberInput</a> tutorial
  /// discusses the differences and basic functionality of both.
  /// </para>
  /// <para>
  /// Unlike <code>NumberInput</code>, <code>MaskedInput</code> can validate and format all types of input. 
  /// The <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_Intro_Tutorial.htm">Introduction to MaskedInput</a> tutorial
  /// discusses the creation of a simple MaskedInput.
  /// </para>
  /// <para>
  /// The <see cref="MaskedInput.Transform" /> property is responsible for defining the type of input that each
  /// instance will accept. The control has a number of built-in transforms for common scenarios, and new transforms
  /// can be added, or existing transforms customized very easily. See the 
  /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_Customize_Transforms.htm">Customizing MaskedInput Transforms</a>
  /// tutorial for more information.
  /// </para>
  /// </remarks>
  [PersistChildren(false)]
  [ParseChildren(true)]
  public sealed class MaskedInput : BaseInput
  {

    /// <summary>
    /// The string of characters that the input box will accept.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property limits the characters which can actually be typed into the input box, while the
    /// <see cref="MaskedInput.Transform" /> property performs validation on the input after it has been entered.
    /// </para>
    /// <para>
    /// When AcceptedCharacters is an empty string, the input box does not accept any typing.
    /// When AcceptedCharacters is null, the input box accepts all typing.
    /// </para>
    /// </remarks>
    public string AcceptedCharacters
    {
      get
      {
        return Properties["AcceptedCharacters"];
      }
      set
      {
        Properties["AcceptedCharacters"] = value;
      }
    }

    private MaskedInputClientEvents _clientEvents = null;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public new MaskedInputClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new MaskedInputClientEvents();
        }
        return _clientEvents;
      }
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
          if (!Page.IsClientScriptBlockRegistered("A570I433.js"))
          {
            Page.RegisterClientScriptBlock("A570I433.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Input.client_scripts", "A570I433.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A570I431.js"))
          {
            Page.RegisterClientScriptBlock("A570I431.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Input.client_scripts", "A570I431.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A570I432.js"))
          {
            Page.RegisterClientScriptBlock("A570I432.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Input.client_scripts", "A570I432.js");
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
      if (this.Text != null)
      {
        output.Write(" value=\"" + this.Text + "\"");
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

      output.Write("<input type=");
      output.Write(this.TextMode == MaskedInputTextMode.Password ? "\"password\"" : "\"text\"");
      output.Write(" id=\"" + this.DisplayInputId + "\"");
      output.Write(" name=\"" + this.DisplayInputId + "\"");

      this.RenderLayoutSettings(output);

      output.Write(" onblur=\"return ComponentArt_MaskedInput_Blur(window." + clientControlId + ", event);\"");
      output.Write(" onclick=\"return ComponentArt_MaskedInput_Click(window." + clientControlId + ", event);\"");
      output.Write(" oncut=\"return ComponentArt_MaskedInput_Cut(window." + clientControlId + ", event);\"");
      output.Write(" onfocus=\"return ComponentArt_MaskedInput_Focus(window." + clientControlId + ", event);\"");
      output.Write(" onkeydown=\"return ComponentArt_MaskedInput_KeyDown(window." + clientControlId + ", event);\"");
      output.Write(" onkeypress=\"return ComponentArt_MaskedInput_KeyPress(window." + clientControlId + ", event);\"");
      output.Write(" onkeyup=\"return ComponentArt_MaskedInput_KeyUp(window." + clientControlId + ", event);\"");
      output.Write(" onpaste=\"return ComponentArt_MaskedInput_Paste(window." + clientControlId + ", event);\"");
      output.Write(" />");

      output.Write("</span>");

      this.WriteStartupScript(output, this.GenerateClientSideIntializationScript(clientControlId));
    }

    /// <summary>
    /// Whether <see cref="AcceptedCharacters"/> should be filtered onkeydown. Default is true.
    /// </summary>
    [DefaultValue(true)]
    [Description("Whether AcceptedCharacters should be filtered onkeydown. Default is true.")]
    public bool FilterCharactersOnKeyDown
    {
      get
      {
        return Utils.ParseBool(Properties["FilterCharactersOnKeyDown"], true);
      }
      set
      {
        Properties["FilterCharactersOnKeyDown"] = value.ToString();
      }
    }

    private string GenerateClientSideIntializationScript(string clientControlId)
    {
      StringBuilder scriptSB = new StringBuilder();
      scriptSB.Append("window.ComponentArt_Init_" + clientControlId + " = function() {\n");

      // Include check for whether everything we need is loaded,
      // and a retry after a delay in case it isn't.
      int retryDelay = 100; // 100 ms retry time sounds about right
      string areScriptsLoaded = "(window.cart_maskedinput_kernel_loaded && window.cart_maskedinput_transforms_loaded && window.cart_maskedinput_support_loaded)";
      scriptSB.Append("if (!" + areScriptsLoaded + ")\n");
      scriptSB.Append("{\n\tsetTimeout('ComponentArt_Init_" + clientControlId + "()', " + retryDelay.ToString() + ");\n\treturn;\n}\n");

      // Instantiate the client-side object
      scriptSB.Append("window." + clientControlId + " = new ComponentArt_MaskedInput('" + clientControlId + "');\n");

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

      if (this.EnableViewState)
      {
        // add us to the client viewstate-saving mechanism
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

      return this.DemarcateClientScript(scriptSB.ToString(), "ComponentArt_MaskedInput_Startup_" + clientControlId + " " + this.VersionString());
    }

    private bool _loadedTextFromForm = false;

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      if (Context != null && Context.Request.Form[this.ValueInputId] != null)
      {
        this.Properties["Text"] = Context.Request.Form[this.ValueInputId];
        this._loadedTextFromForm = true;
      }
    }

    protected override object SaveViewState()
    {
      // Save State as a cumulative array of objects.
      object baseState = base.SaveViewState();
      object[] allStates = new object[2];
      allStates[0] = baseState;
      allStates[1] = this.Text;
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
          if (!this._loadedTextFromForm)
          {
            this.Text = Utils.ParseJSString(myState[1]);
          }
        }
      }
    }

    protected override void OnLoad(System.EventArgs e)
    {
      base.OnLoad(e);

      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Input.client_scripts.A570I433.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Input.client_scripts.A570I431.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Input.client_scripts.A570I432.js");
      }

      //TODO: If we reintroduce this piece of code, we must modify it to allow for Page.Header being null (when <head> is not runat="server")
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
      this.ClientProperties.Add("AcceptedCharacters", Utils.ConvertStringToJSString(this.AcceptedCharacters));
      this.ClientProperties.Add("ClientEvents", Utils.ConvertClientEventsToJsObject(this.ClientEvents));
      this.ClientProperties.Add("FilterCharactersOnKeyDown", this.FilterCharactersOnKeyDown.ToString().ToLower());
      this.ClientProperties.Add("Text", Utils.ConvertStringToJSString(this.Text));
      this.ClientProperties.Add("TextMode", ((int)this.TextMode).ToString());
      this.ClientProperties.Add("Transform", Utils.ConvertStringToJSString(this.Transform));
      this.ClientProperties.Add("TransformMask", this.TransformMask);
      this.ClientProperties.Add("TransformUnmask", this.TransformUnmask);
      this.ClientProperties.Add("TransformValidate", this.TransformValidate);
      this.ClientProperties.Add("ValidateMasked", this.ValidateMasked.ToString().ToLower());
    }

    /// <summary>
    /// Value of the input textbox.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property contains the text which was entered into the input box. Note that this is the unmasked
    /// text. If the masked text needs to be accessed from the server, it is contained in the <see cref="BaseInput.DisplayText" />
    /// property for both the <see cref="NumberInput" /> and <see cref="MaskedInput" />.
    /// </para>
    /// </remarks>
    public string Text
    {
      get
      {
        string value = (string)this.Properties["Text"];
        return (value != null) ? value : String.Empty;
      }
      set
      {
        this.Properties["Text"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the behavior mode (single-line, or password) of the <b>MaskedInput</b> control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use the <b>TextMode</b> property to specify whether a <see cref="MaskedInput"/> control is displayed as a single-line, or password text box.
    /// </para>
    /// <para>
    /// If the <see cref="MaskedInput"/> control is in password mode, all characters entered in the control are masked.
    /// </para>
    /// </remarks>
    public MaskedInputTextMode TextMode
    {
      get
      {
        return Utils.ParseMaskedInputTextMode(Properties["MaskedInputTextMode"]);
      }
      set
      {
        Properties["MaskedInputTextMode"] = value.ToString();
      }
    }

    /// <summary>
    /// The type of masking this control uses.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The transform is responsible for validation, as well as 
    /// masking and unmasking the input text. A number of default transforms are included with the control, 
    /// and new ones can be easily added. Existing transforms can also be customized using the <see cref="TransformMask" />, 
    /// <see cref="TransformUnmask" />, and <see cref="TransformValidate" /> properties. See the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_Customize_Transforms.htm">Customizing MaskedInput Transforms</a> 
    /// tutorial for more information on customizing and adding transforms.
    /// </para>
    /// <para>
    /// The following list contains all of the transforms which are currently included with the control: 
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <b>CreditCard_VisaMasterCard</b> - Visa or MasterCard credit card number.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>CreditCard_AmEx</b> - American Express credit card number.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>Telephone_NorthAmerica</b> - North American telephone number.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>ZipCode</b> - United States zip code.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>PostalCode</b> - Canadian postal code.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>PostalCode_Australia</b> - Australian postal code.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>EmailAddress</b> - E-Mail address.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>empty</b> - accepts all input. This is the default value.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [TypeConverter(typeof(MaskedInputTransformConverter))]
    public string Transform
    {
      get
      {
        return Properties["Transform"];
      }
      set
      {
        Properties["Transform"] = value;
      }
    }

    /// <summary>
    /// JavaScript function that is called to mask the input contents.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property overrides the <code>mask</code> method of the selected <see cref="MaskedInput.Transform" />. This property
    /// is set automatically by the <code>Transform</code> and only needs to be set manually if the existing <code>mask</code> method
    /// is insufficient.
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_Customize_Transforms.htm">Customizing MaskedInput Transforms</a>
    /// tutorial for more information on customizing transforms.
    /// </para>
    /// </remarks>
    public string TransformMask
    {
      get
      {
        return Properties["TransformMask"];
      }
      set
      {
        Properties["TransformMask"] = value;
      }
    }

    /// <summary>
    /// JavaScript function that is called to unmask the input contents.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property overrides the <code>unmask</code> method of the selected <see cref="MaskedInput.Transform" />. This property
    /// is set automatically by the <code>Transform</code> and only needs to be set manually if the existing <code>unmask</code> method
    /// is insufficient.
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_Customize_Transforms.htm">Customizing MaskedInput Transforms</a>
    /// tutorial for more information on customizing transforms.
    /// </para>
    /// </remarks>
    public string TransformUnmask
    {
      get
      {
        return Properties["TransformUnmask"];
      }
      set
      {
        Properties["TransformUnmask"] = value;
      }
    }

    /// <summary>
    /// JavaScript function that is called to validate the input contents.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property overrides the <code>validate</code> method of the selected <see cref="MaskedInput.Transform" />. This property
    /// is set automatically by the <code>Transform</code> and only needs to be set manually if the existing <code>validate</code> method
    /// is insufficient.
    /// See the <a href="ms-help:/../ComponentArt.Web.UI.AJAX/Input_Customize_Transforms.htm">Customizing MaskedInput Transforms</a>
    /// tutorial for more information on customizing transforms.
    /// </para>
    /// </remarks>
    public string TransformValidate
    {
      get
      {
        return Properties["TransformValidate"];
      }
      set
      {
        Properties["TransformValidate"] = value;
      }
    }

    /// <summary>
    /// Whether external ASP.NET validators validate the masked or unmasked content of this control. Default is true (validate masked).
    /// </summary>
    /// <remarks>
    /// <para>
    /// If an ASP.NET validator is used with this control, this property determines which content it validates.
    /// If it is <code>true</code>, the <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~MaskedInput_masked_property.htm">masked</a> 
    /// property will be used, if false, the 
    /// <a href="ms-help:/../ComponentArt.Web.UI.AJAX.client/ComponentArt.Web.UI~MaskedInput_unmasked_property.htm">unmasked</a>
    /// property will be used.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    public bool ValidateMasked
    {
      get
      {
        return Utils.ParseBool(Properties["ValidateMasked"], true);
      }
      set
      {
        Properties["ValidateMasked"] = value.ToString();
      }
    }

  }

  /// <summary>
  /// Provides Visual Studio intellisense for Transform property.
  /// The list of StandardValues here corresponds to the properties of client-side ComponentArt_MaskedInput_Transforms object.
  /// </summary>
  public class MaskedInputTransformConverter : TypeConverter
  {
    public string[] StandardValues =
      {
        "empty", 
        "CreditCard_VisaMasterCard",
        "CreditCard_AmEx",
        "Telephone_NorthAmerica",
        "ZipCode",
        "PostalCode",
        "PostalCode_Australia",
        "EmailAddress"
      };

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      return new StandardValuesCollection(StandardValues);
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }
  }

  /// <summary>
  /// Specifies the behavior mode of the MaskedInput.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The <b>MaskedInputTextMode</b> enumeration represents the different display options for <see cref="MaskedInput"/> controls.
  /// </para>
  /// <para>
  /// <b>SingleLine</b> mode displays the <see cref="MaskedInput"/> control as a single row. If the user enters text that exceeds 
  /// the physical size of the <see cref="MaskedInput"/> control, the text will scroll horizontally. 
  /// </para>
  /// <para>
  /// The behavior of <b>Password</b> mode is similar to <b>SingleLine</b> mode except that all characters entered in the 
  /// <see cref="MaskedInput"/> control are masked and are not saved in view state.
  /// </para>
  /// </remarks>
  public enum MaskedInputTextMode
  {
    /// <summary>Represents single-line entry mode.</summary>
    SingleLine,
    /// <summary>Represents password entry mode.</summary>
    Password
  }

}
