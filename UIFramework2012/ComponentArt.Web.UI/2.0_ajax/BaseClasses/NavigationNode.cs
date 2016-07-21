using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Xml;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Serves as the base class for nodes of ComponentArt navigation controls.
  /// </summary>
  /// <remarks>
  /// Base navigation node class representing a node in the hierarchical structure of a <see cref="BaseNavigator"/>. 
  /// NavigationNode provides core navigation node properties such as Text, NavigateUrl, ToolTip. 
  /// All ComponentArt navigation node classes inherit from this class. 
  /// </remarks>
  /// <seealso cref="BaseNavigator"/>
  [ToolboxItem(false)]
  public abstract class NavigationNode : System.Web.UI.WebControls.WebControl
	{
    internal bool _defaultStyle = false;

	#region Public Properties 

    #region Unused hidden inheritance

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string AccessKey
		{
			get { return base.AccessKey; }
		}
		
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override FontInfo Font
    {
      get { return base.Font; }
    }

    [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override System.Drawing.Color BackColor
    {
      get { return base.BackColor; }
    }

    [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override System.Drawing.Color BorderColor
    {
      get { return base.BorderColor; }
    }

    [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override BorderStyle BorderStyle
    {
      get { return base.BorderStyle; }
    }

    [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Unit BorderWidth
    {
      get { return base.BorderWidth; }
    }

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool EnableViewState
		{
			get { return base.EnableViewState; }
		}
		
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
    public override System.Drawing.Color ForeColor
    {
      get { return base.ForeColor; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override short TabIndex
    {
      get
      {
        return base.TabIndex;
      }
    }

    #endregion

    /// <summary>
    /// Whether to perform a postback when this node is selected. Default: false.
    /// </summary>
    [Description("Whether to perform a postback when this node is selected. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnSelect
    {
      get 
      {
        string o = Properties[GetAttributeVarName("AutoPostBackOnSelect")]; 
        return (o == null) ? (navigator == null? false : navigator.AutoPostBackOnSelect) : Utils.ParseBool(o, false); 
      }
      set 
      {
        Properties[GetAttributeVarName("AutoPostBackOnSelect")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to perform validation when this node causes a postback. Default: Inherit.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether to perform validation when this node causes a postback. Default: Inherit.")]
    [DefaultValue(InheritBool.Inherit)]
    public InheritBool CausesValidation
    {
      get 
      {
				return Utils.ParseInheritBool(Properties[GetAttributeVarName("CausesValidation")]);
      }
      set 
      {
        Properties[GetAttributeVarName("CausesValidation")] = value.ToString();
      }
    }

    /// <summary>
    /// Client-side command to execute on selection. This can be any valid client script.
    /// </summary>
    [Category("Behavior")]
    [Description("Client-side command to execute on selection.")]
    [DefaultValue("")]
		public string ClientSideCommand
		{
			get 
			{
				string o = Properties[GetAttributeVarName("ClientSideCommand")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("ClientSideCommand")] = value;
			}
		}
		
    /// <summary>
    /// ID of the client template to use for this node.
    /// </summary>
    [Category("Appearance")]
    [Description("ID of the client template to use for this node.")]
    [DefaultValue("")]
    public string ClientTemplateId
    {
      get 
      {
        string o = Properties[GetAttributeVarName("ClientTemplateId")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("ClientTemplateId")] = value;
      }
    }

    /// <summary>
    /// Default CSS class to use for this node.
    /// </summary>
		[Category("Appearance")]
		[Description("Default CSS class to use for this node.")]
    [DefaultValue("")]
		public new string CssClass
		{
			get 
			{
				string o = Properties[GetAttributeVarName("CssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("CssClass")] = value;
			}
		}

		/// <summary>
		/// Depth within the structure.
		/// </summary>
		/// <remarks>
		/// This is a read-only property.
    /// </remarks>
		[Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Depth
		{
			get
			{
				int depth = 0;

				for(NavigationNode oNode = this; oNode.parentNode != null; oNode = oNode.parentNode)
				{
					depth++;
				}

				return depth;
			}
		}

    /// <summary>
    /// Whether this navigation node is enabled.
    /// </summary>
		[Category("Behavior")]
		[Description("Whether this navigation node is enabled.")]
    [DefaultValue(true)]
    public override bool Enabled
    {
      get 
      {
        string o = Properties[GetAttributeVarName("Enabled")]; 
        return (o == null) ? true : Utils.ParseBool(o, true); 
      }
      set 
      {
        Properties[GetAttributeVarName("Enabled")] = value.ToString();
      }
    }

    /// <summary>
    /// ID of this node.
    /// </summary>
    [Description("ID of this node.")]
    [DefaultValue("")]
		public new string ID
		{
			get 
			{
				string o = Properties[GetAttributeVarName("ID")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("ID")] = value;
			}
		}

    /// <summary>
    /// Whether the selected node is one of this node's descendants.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsChildSelected
		{
			get
			{
        if( this.navigator == null ||
          this.navigator.selectedNode == null)
        {
          return false;
        }
        else
        {
          NavigationNode oNode = this.navigator.selectedNode.parentNode;
          while(oNode != null)
          {
            if(oNode == this)
            {
              return true;
            }

            oNode = oNode.parentNode;
          }
          return false;
        }
			}
		}
		
    /// <summary>
    /// Whether this is the selected node.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsSelected
		{
			get
			{
				return this.navigator != null? (this.navigator.selectedNode == this) : false;
			}
		}

    /// <summary>
    /// String representing keyboard shortcut for selecting this node.
    /// </summary>
    /// <remarks>
    /// Examples of the format are:
    /// Shift+Ctrl+P, Alt+A, Shift+Alt+F7, etc. "Shift", "Ctrl" and "Alt" must appear in that order.
    /// </remarks>
    [Category("Behavior")]
    [Description("String representing keyboard shortcut for selecting this node.")]
    [DefaultValue("")]
    public string KeyboardShortcut
    {
      get 
      {
        string o = Properties[GetAttributeVarName("KeyboardShortcut")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("KeyboardShortcut")] = value;
      }
    }

    /// <summary>
    /// URL to navigate to when this node is selected.
    /// </summary>
    [Category("Navigation")]
    [Description("URL to navigate to when this node is selected.")]
    [DefaultValue("")]
		public string NavigateUrl
		{
			get 
			{
				string o = Properties[GetAttributeVarName("NavigateUrl")]; 
				return (o == null) ? string.Empty : (navigator == null? o : navigator.ConvertUrl(o)); 
			}
			set 
			{
				Properties[GetAttributeVarName("NavigateUrl")] = value;
			}
		}
		
    /// <summary>
    /// ID of PageView to switch the target ComponentArt MultiPage control to
    /// when this node is selected.
    /// </summary>
    /// <seealso cref="BaseNavigator.MultiPageId" />
    [Category("Behavior")]
    [Description("ID of PageView to switch the target ComponentArt MultiPage control to when this node is selected.")]
    [DefaultValue("")]
    public string PageViewId
    {
      get 
      {
        string o = Properties[GetAttributeVarName("PageViewId")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("PageViewId")] = value;
      }
    }

    /// <summary>
    /// ID of NavigationCustomTemplate to use for this node.
    /// </summary>
    [Category("Appearance")]
    [Description("ID of NavigationCustomTemplate to use for this node.")]
    [DefaultValue("")]
    public string ServerTemplateId
    {
      get 
      {
        string o = Properties[GetAttributeVarName("ServerTemplateId")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("ServerTemplateId")] = value;
      }
    }

    /// <summary>
    /// XML file to load the substructure of this node from.
    /// </summary>
		[Category("Data")]
		[Description("XML file to load the substructure of this node from.")]
    [DefaultValue("")]
    public string SiteMapXmlFile
    {
      get 
      {
        string o = Properties[GetAttributeVarName("SiteMapXmlFile")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("SiteMapXmlFile")] = value;
      }
    }

    /// <summary>
    /// Target frame for this node's navigation.
    /// </summary>
    [Category("Navigation")]
    [Description("Target frame for this node's navigation.")]
    [DefaultValue("")]
		public string Target
		{
			get 
			{
				string o = Properties[GetAttributeVarName("Target")]; 
				return (o == null) ? (navigator == null? string.Empty : navigator.DefaultTarget) : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("Target")] = value;
			}
		}

    /// <summary>
    /// Deprecated.  Use <see cref="ServerTemplateId"/> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Description("Deprecated.  Use ServerTemplateId instead.")]
    [Category("Appearance")]
    [DefaultValue("")]
    [Obsolete("Deprecated.  Use ServerTemplateId instead.", false)]
    public string TemplateId
    {
      get 
      {
        return ServerTemplateId;
      }
      set 
      {
        ServerTemplateId = value;
      }
    }

    /// <summary>
    /// Text label of this node.
    /// </summary>
		[Category("Appearance")]
		[Description("Text label of this node.")]
    [DefaultValue("")]
    public string Text
		{
			get 
			{
				string o = Properties[GetAttributeVarName("Text")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("Text")] = value;
			}
		}

    /// <summary>
    /// ToolTip to display for this item.
    /// </summary>
    [Description("ToolTip to display for this item.")]
    [DefaultValue("")]
    public override string ToolTip
    {
      get 
      {
        string o = Properties[GetAttributeVarName("ToolTip")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("ToolTip")] = value;
      }
    }

    /// <summary>
    /// Optional internal string value of this node.
    /// </summary>
    [Description("Optional internal string value of this node.")]
    [Category("Data")]
    [DefaultValue("")]
    public string Value
    {
      get 
      {
        string o = Properties[GetAttributeVarName("Value")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("Value")] = value;
      }
    }

    /// <summary>
    /// Whether this node should be displayed.
    /// </summary>
    [Description("Whether this node should be displayed.")]
    [Category("Data")]
    [DefaultValue(true)]
    public override bool Visible
    {
      get 
      {
        string o = Properties[GetAttributeVarName("Visible")]; 
        return (o == null) ? true : Utils.ParseBool(o, true); 
      }
      set 
      {
        Properties[GetAttributeVarName("Visible")] = value.ToString();
      }
    }

	#endregion

	#region Public Methods 
		
    public NavigationNode()
    {
      // If we're not in design-time, use Attributes instead of Properties
      if(Context != null)
      {
        // First, copy Properties to Attributes
        foreach(string sKey in Properties.Keys)
        {
          Attributes[sKey] = Properties[sKey];
        }

        // Make Properties point to Attributes
        Properties = Attributes;
      }
    }

    /// <summary>
    /// Get the zero-based index of this NavigationNode within its group. A negative value is
    /// returned if the index could not be computed, for example if the node doesn't belong to
    /// a group at all.
    /// </summary>
    /// <returns>The zero-based index of this NavigationNode within its group.</returns>
    public int GetCurrentIndex()
    {
      NavigationNodeCollection arNodes;

      if(this.parentNode != null)
      {
        arNodes = this.parentNode.nodes;
      }
      else if(this.navigator != null)
      {
        arNodes = this.navigator.nodes;
      }
      else
      {
        return -1;
      }
      
      return arNodes.IndexOf(this);
    }

    /// <summary>
    /// Find a control in this node's template instance.
    /// </summary>
    /// <param name="sID">ID of the sought control</param>
    /// <returns>
    /// The control with the given ID contained inside the template instance for this node,
    /// if the node is templated, or null if it isn't or the control is not found.
    /// </returns>
    /// <seealso cref="ServerTemplateId" />
    public override Control FindControl(string sID)
    {
      if(this.ServerTemplateId != string.Empty && this.navigator != null)
      {
        foreach(Control oControl in this.navigator.Controls)
        {
          if(oControl.ID.EndsWith("_" + this.PostBackID))
          {
            Control result = oControl.FindControl(sID);

            // look inside user control?
            if(result == null && oControl.Controls.Count == 1 && oControl.Controls[0] is UserControl)
            {
              return oControl.Controls[0].FindControl(sID);
            }
            else
            {
              return result;
            }
          }
        }
      }
      
      return null;
    }

    /// <summary>
    /// IsDescendantOf method.
    /// </summary>
    /// <param name="oAncestor">Node to check</param>
    /// <returns>Whether the given node is an ancestor of this one</returns>
		public bool IsDescendantOf(NavigationNode oAncestor)
		{
			NavigationNode oNode = this.parentNode;

			while(oNode != null)
			{
				if(oNode == oAncestor)
				{
					return true;
				}
				oNode = oNode.parentNode;
			}

			return false;
		}

    /// <summary>
    /// Navigates to the set NavigateUrl for this node.
    /// </summary>
    /// <remarks>
    /// Server-side navigation redirects the browser to the new URL without taking Target into account.
    /// </remarks>
    /// <seealso cref="NavigateUrl" />
		public void Navigate()
		{
      string sUrl = this.NavigateUrl;

      if(sUrl.StartsWith("/"))
      {
        Context.Response.Redirect(sUrl, false);
      }
      else if(sUrl.StartsWith("?"))
      {
        Context.Response.Redirect(Context.Request.Path + sUrl, false);
      }
      else
      {
        Context.Response.Redirect(sUrl, false);
      }
		}

	#endregion

	#region Protected Properties 

    private System.Web.UI.AttributeCollection _properties;
    internal System.Web.UI.AttributeCollection Properties
    {
      get
      {
        if(_properties == null)
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

		protected string commandJavaScript;

		internal NavigationNodeCollection nodes;

		internal BaseNavigator navigator;

		internal NavigationNode parentNode;
		internal NavigationNode previousSibling;
		internal string previousUrl;
		internal string nextUrl;
		internal NavigationNode nextSibling;

		internal string PostBackID;

	#endregion

	#region Internal Methods

		internal abstract NavigationNode AddNode();

    /// <summary>
    /// Generate the client-side command that should be executed when this node is selected.
    /// </summary>
    /// <returns>The client-side script in a string.</returns>
		internal string GenerateClientCommand()
		{
			if(!this.AutoPostBackOnSelect && this.NavigateUrl != string.Empty)
			{
				if(this.Target == string.Empty)
				{
					this.commandJavaScript = "document.location.href='" + this.NavigateUrl + "'";
				}
				else
				{
          this.commandJavaScript = "window.open('" + this.NavigateUrl + "','" + this.Target + "')";
				}
			}
			else 
			{
				string sPostback = (this.navigator == null || Context == null || Page == null)? "__doPostBack(null,null)" :
					Page.GetPostBackEventReference(this.navigator, this.PostBackID);

				if(this.ClientSideCommand != string.Empty)
				{
          string sClientSideCommand = this.ClientSideCommand;
          if(sClientSideCommand.EndsWith(";"))
          {
            sClientSideCommand = sClientSideCommand.Substring(0, sClientSideCommand.Length - 1);
          }
					this.commandJavaScript = string.Format("if({0}){{{1};}}", sClientSideCommand, sPostback);
				}
				else if(this.AutoPostBackOnSelect)
				{
					this.commandJavaScript = sPostback + ";";
				}
				else
				{
					this.commandJavaScript = string.Empty;
				}
			}

			return commandJavaScript;
		}

    /// <summary>
    /// Get the name to be used for the given attribute, taking into
    /// consideration any attribute mappings.
    /// </summary>
    /// <param name="sAttribute">The default name of the attribute.</param>
    /// <returns>The actual name of the attribute.</returns>
    internal string GetAttributeVarName(string sAttribute)
    {
      if(this.navigator != null && navigator.CustomAttributeMappings.Count > 0)
      {
        for(int i = 0; i < navigator.CustomAttributeMappings.Count; i++)
        {
          CustomAttributeMapping oMapping = (CustomAttributeMapping)navigator.CustomAttributeMappings[i];

          if(oMapping.To.ToLower() == sAttribute.ToLower())
          {
            return oMapping.From;
          }
        }
      }
      
      return sAttribute;
    }

    internal string GetVarAttributeName(string sAttribute)
    {
      if(this.navigator != null && navigator.CustomAttributeMappings.Count > 0)
      {
        for(int i = 0; i < navigator.CustomAttributeMappings.Count; i++)
        {
          CustomAttributeMapping oMapping = (CustomAttributeMapping)navigator.CustomAttributeMappings[i];

          if(oMapping.From.ToLower() == sAttribute.ToLower())
          {
            return oMapping.To;
          }
        }
      }
      
      return sAttribute;
    }

		internal void ReadXmlAttributes(XmlAttributeCollection arAttributes)
		{
			// This takes over all attributes specified in Xml
			foreach(XmlAttribute oAttribute in arAttributes)
			{
				this.Properties[oAttribute.Name] = oAttribute.Value;
			}
		}

    internal bool IsPropertySet(string sPropertyName)
    {
      return (Properties[GetAttributeVarName(sPropertyName)] != null);
    }

	#endregion

	}

	#region Supporting types

  /// <summary>
  /// Extends the boolean type to include a third value indicating that neither true nor false is set.
  /// </summary>
	public enum InheritBool
	{
    /// <summary>Value is not set.</summary>
		Inherit,

    /// <summary>Boolean value of <b>false</b>.</summary>
		False,

    /// <summary>Boolean value of <b>true</b>.</summary>
		True
	}

	#endregion

}
