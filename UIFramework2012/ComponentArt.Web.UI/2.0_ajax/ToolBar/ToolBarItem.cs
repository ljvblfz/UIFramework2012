using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Xml;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// A single cell in a <see cref="ToolBar"/> control. 
  /// </summary>
  [ToolboxItem(false)]
  public class ToolBarItem : System.Web.UI.WebControls.WebControl
  {

    public ToolBarItem()
    {
      // If we're not in design-time, use Attributes instead of Properties
      if (Context != null)
      {
        // First, copy Properties to Attributes
        foreach (string sKey in Properties.Keys)
        {
          Attributes[sKey] = Properties[sKey];
        }

        // Make Properties point to Attributes
        Properties = Attributes;
      }
    }

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
    /// CSS class to use for this item when active (on mouse down).
    /// </summary>
    [Description("CSS class to use for this item when active (on mouse down).")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string ActiveCssClass
    {
      get
      {
        return Properties[GetAttributeVarName("ActiveCssClass")];
      }
      set
      {
        Properties[GetAttributeVarName("ActiveCssClass")] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use in this item when active (on mouse down).
    /// </summary>
    [Description("The URL of the image to use in this item when active (on mouse down).")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string ActiveImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("ActiveImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("ActiveImageUrl")] = value;
      }
    }

    /// <summary>
    /// The URL of the dropdown image to use in this item when active (on mouse down).
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Description("The URL of the dropdown image to use in this item when active (on mouse down).")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string ActiveDropDownImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("ActiveDropDownImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("ActiveDropDownImageUrl")] = value;
      }
    }

    /// <summary>
    /// Whether to allow HTML content in this item's <see cref="Text"/> field.  Default: true.
    /// </summary>
    [DefaultValue(true)]
    [Category("Appearance")]
    [Description("Whether to allow HTML content in this item's Text field.")]
    public bool AllowHtmlContent
    {
      get
      {
        return Utils.ParseBool(Properties[GetAttributeVarName("AllowHtmlContent")], true);
      }
      set
      {
        Properties[GetAttributeVarName("AllowHtmlContent")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to perform a postback when this item is selected. Default: false.
    /// </summary>
    [Description("Whether to perform a postback when this item is selected. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnSelect
    {
      get
      {
        string o = Properties[GetAttributeVarName("AutoPostBackOnSelect")];
        return (o == null) ? (this.ParentToolBar == null ? false : this.ParentToolBar.AutoPostBackOnSelect) : Utils.ParseBool(o, false);
      }
      set
      {
        Properties[GetAttributeVarName("AutoPostBackOnSelect")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to perform validation when this item causes a postback. Default: Inherit.
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
    /// Whether this item is checked.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(false)]
    [Description("Whether this item is checked.")]
    public bool Checked
    {
      get
      {
        return Utils.ParseBool(Properties[GetAttributeVarName("Checked")], false);
      }
      set
      {
        Properties[GetAttributeVarName("Checked")] = value.ToString();
      }
    }

    /// <summary>
    /// CSS class to use for this item when active (on mouse down) when it is <see cref="Checked"/>.
    /// </summary>
    [Description("CSS class to use for this item when active (on mouse down) when it is Checked.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string CheckedActiveCssClass
    {
      get
      {
        return Properties[GetAttributeVarName("CheckedActiveCssClass")];
      }
      set
      {
        Properties[GetAttributeVarName("CheckedActiveCssClass")] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use in this item when active (on mouse down) when it is <see cref="Checked"/>.
    /// </summary>
    [Description("The URL of the image to use in this item when active (on mouse down) when it is Checked.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string CheckedActiveImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("CheckedActiveImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("CheckedActiveImageUrl")] = value;
      }
    }

    /// <summary>
    /// CSS class of this item when it is <see cref="Checked"/>.
    /// </summary>
    [Description("CSS class of this item when it is Checked.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string CheckedCssClass
    {
      get
      {
        return Properties[GetAttributeVarName("CheckedCssClass")];
      }
      set
      {
        Properties[GetAttributeVarName("CheckedCssClass")] = value;
      }
    }

    /// <summary>
    /// CSS class to use for this item on hover (on mouse over) when it is <see cref="Checked"/>.
    /// </summary>
    [Description("CSS class to use for this item on hover (on mouse over) when it is Checked.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string CheckedHoverCssClass
    {
      get
      {
        return Properties[GetAttributeVarName("CheckedHoverCssClass")];
      }
      set
      {
        Properties[GetAttributeVarName("CheckedHoverCssClass")] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use in this item on hover (on mouse over) when it is <see cref="Checked"/>.
    /// </summary>
    [Description("The URL of the image to use in this item on hover (on mouse over) when it is Checked.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string CheckedHoverImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("CheckedHoverImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("CheckedHoverImageUrl")] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use in this item when it is <see cref="Checked"/>.
    /// </summary>
    [Description("The URL of the image to use in this item when it is Checked.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string CheckedImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("CheckedImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("CheckedImageUrl")] = value;
      }
    }

    /// <summary>
    /// Client-side command to execute on click. This can be any valid client script.
    /// </summary>
    [Category("Behavior")]
    [Description("Client-side command to execute on click.")]
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
    /// ID of the client template to use for this item.
    /// </summary>
    [Category("Appearance")]
    [Description("ID of the client template to use for this item.")]
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
    /// CSS class of this item.
    /// </summary>
    [Description("CSS class of this item.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public override string CssClass
    {
      get
      {
        return Properties[GetAttributeVarName("CssClass")];
      }
      set
      {
        Properties[GetAttributeVarName("CssClass")] = value;
      }
    }

    /// <summary>
    /// ID of the child control to use for content of this item.
    /// </summary>
    [Category("Appearance")]
    [Description("ID of the child control to use for content of this item.")]
    [DefaultValue("")]
    public string CustomContentId
    {
      get
      {
        string o = Properties[GetAttributeVarName("CustomContentId")];
        return (o == null) ? string.Empty : o;
      }
      set
      {
        Properties[GetAttributeVarName("CustomContentId")] = value;
      }
    }

    /// <summary>
    /// CSS class of this item when it is not <see cref="Enabled"/> and it is <see cref="Checked"/>.
    /// </summary>
    [Description("CSS class of this item when it is not Enabled and it is Checked.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string DisabledCheckedCssClass
    {
      get
      {
        return Properties[GetAttributeVarName("DisabledCheckedCssClass")];
      }
      set
      {
        Properties[GetAttributeVarName("DisabledCheckedCssClass")] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use in this item when it is not <see cref="Enabled"/> and it is <see cref="Checked"/>.
    /// </summary>
    [Description("The URL of the image to use in this item when it is not Enabled and it is Checked.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string DisabledCheckedImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("DisabledCheckedImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("DisabledCheckedImageUrl")] = value;
      }
    }

    /// <summary>
    /// CSS class of this item when it is not <see cref="Enabled"/>.
    /// </summary>
    [Description("CSS class of this item when it is not Enabled.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string DisabledCssClass
    {
      get
      {
        return Properties[GetAttributeVarName("DisabledCssClass")];
      }
      set
      {
        Properties[GetAttributeVarName("DisabledCssClass")] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use in this item when it is not <see cref="Enabled"/>.
    /// </summary>
    [Description("The URL of the image to use in this item when it is not Enabled.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string DisabledImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("DisabledImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("DisabledImageUrl")] = value;
      }
    }

    /// <summary>
    /// The URL of the dropdown image to use in this item when it is not <see cref="Enabled"/>.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Description("The URL of the dropdown image to use in this item when it is not Enabled.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string DisabledDropDownImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("DisabledDropDownImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("DisabledDropDownImageUrl")] = value;
      }
    }

    /// <summary>
    /// ID of ComponentArt Menu to act as a dropdown for this item.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Category("Appearance")]
    [Description("ID of ComponentArt Menu to act as a dropdown for this item.")]
    [DefaultValue("")]
    public string DropDownId
    {
      get
      {
        string o = Properties[GetAttributeVarName("DropDownId")];
        return (o == null) ? string.Empty : o;
      }
      set
      {
        Properties[GetAttributeVarName("DropDownId")] = value;
      }
    }

    /// <summary>
    /// Item's dropdown image height.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Category("Layout")]
    [DefaultValue(typeof(Unit), "")]
    [Description("Item's dropdown image height.")]
    public Unit DropDownImageHeight
    {
      get
      {
        string o = Properties[GetAttributeVarName("DropDownImageHeight")];
        return (o != null) ? Utils.ParseUnit(o) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemImageHeight : Unit.Empty;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DropDownImageHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Item dropdown image dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// The position of the dropdown image relative to rest of the item.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Category("Appearance")]
    [DefaultValue(ToolBarDropDownImagePosition.Right)]
    [Description("The position of the dropdown image relative to rest of the item.")]
    public ToolBarDropDownImagePosition DropDownImagePosition
    {
      get
      {
        string o = Properties[GetAttributeVarName("DropDownImagePosition")];
        return (o != null) ? Utils.ParseToolBarDropDownImagePosition(o, ToolBarDropDownImagePosition.Right) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemDropDownImagePosition :
          ToolBarDropDownImagePosition.Right;
      }
      set
      {
        Properties[GetAttributeVarName("DropDownImagePosition")] = value.ToString();
      }
    }

    /// <summary>
    /// The URL of the dropdown image to use in this item.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Description("The URL of the dropdown image to use in this item.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string DropDownImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("DropDownImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("DropDownImageUrl")] = value;
      }
    }

    /// <summary>
    /// Item's dropdown image width.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Category("Layout")]
    [DefaultValue(typeof(Unit), "")]
    [Description("Item's dropdown image width.")]
    public Unit DropDownImageWidth
    {
      get
      {
        string o = Properties[GetAttributeVarName("DropDownImageWidth")];
        return (o != null) ? Utils.ParseUnit(o) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemImageWidth : Unit.Empty;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DropDownImageWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Item dropdown image dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Whether this item is enabled.
    /// </summary>
    [Category("Behavior")]
    [Description("Whether this item is enabled.")]
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
    /// CSS class to use for this item when its dropdown menu is expanded.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Description("CSS class to use for this item when its dropdown menu is expanded.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string ExpandedCssClass
    {
      get
      {
        return Properties[GetAttributeVarName("ExpandedCssClass")];
      }
      set
      {
        Properties[GetAttributeVarName("ExpandedCssClass")] = value;
      }
    }

    /// <summary>
    /// The URL of the dropdown image to use in this item when its dropdown menu is expanded.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Description("The URL of the dropdown image to use in this item when its dropdown menu is expanded.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string ExpandedDropDownImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("ExpandedDropDownImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("ExpandedDropDownImageUrl")] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use in this item when its dropdown menu is expanded.
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Description("The URL of the image to use in this item when its dropdown menu is expanded.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string ExpandedImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("ExpandedImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("ExpandedImageUrl")] = value;
      }
    }

    /// <summary>
    /// Item's height.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit), "")]
    [Description("Item's height.")]
    public override Unit Height
    {
      get
      {
        string o = Properties[GetAttributeVarName("Height")];
        return (o != null) ? Utils.ParseUnit(o) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemHeight : Unit.Empty;
      }
      set
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("Height")] = value.ToString();
        }
        else
        {
          throw new Exception("Item dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// CSS class to use for this item on hover (on mouse over).
    /// </summary>
    [Description("CSS class to use for this item on hover (on mouse over).")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string HoverCssClass
    {
      get
      {
        return Properties[GetAttributeVarName("HoverCssClass")];
      }
      set
      {
        Properties[GetAttributeVarName("HoverCssClass")] = value;
      }
    }

    /// <summary>
    /// The URL of the image to use in this item on hover (on mouse over).
    /// </summary>
    [Description("The URL of the image to use in this item on hover (on mouse over).")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string HoverImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("HoverImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("HoverImageUrl")] = value;
      }
    }

    /// <summary>
    /// The URL of the dropdown image to use in this item on hover (on mouse over).
    /// </summary>
    /// <remarks>
    /// This property only applies when the <see cref="ItemType"/> property is set to <b>ToolBarItemType.DropDown</b>
    /// or <b>ToolBarItemType.SplitDropDown</b>.
    /// </remarks>
    [Description("The URL of the dropdown image to use in this item when its dropdown menu is expanded.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string HoverDropDownImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("HoverDropDownImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("HoverDropDownImageUrl")] = value;
      }
    }

    /// <summary>
    /// ID of this item.
    /// </summary>
    [Description("ID of this item.")]
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
    /// Item's image height.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit), "")]
    [Description("Item's image height.")]
    public Unit ImageHeight
    {
      get
      {
        string o = Properties[GetAttributeVarName("ImageHeight")];
        return (o != null) ? Utils.ParseUnit(o) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemImageHeight : Unit.Empty;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("ImageHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Item image dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// The URL of the image to use in this item.
    /// </summary>
    [Description("The URL of the image to use in this item.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public string ImageUrl
    {
      get
      {
        return Properties[GetAttributeVarName("ImageUrl")];
      }
      set
      {
        Properties[GetAttributeVarName("ImageUrl")] = value;
      }
    }

    /// <summary>
    /// Item's image width.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit), "")]
    [Description("Item's image width.")]
    public Unit ImageWidth
    {
      get
      {
        string o = Properties[GetAttributeVarName("ImageWidth")];
        return (o != null) ? Utils.ParseUnit(o) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemImageWidth : Unit.Empty;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("ImageWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Item image dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Type of this item.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(ToolBarItemType.Command)]
    [Description("Type of this item.")]
    public ToolBarItemType ItemType
    {
      get
      {
        return Utils.ParseToolBarItemType(Properties[GetAttributeVarName("ItemType")]);
      }
      set
      {
        Properties[GetAttributeVarName("ItemType")] = value.ToString();
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

    internal ToolBar _parentToolBar;
    public ToolBar ParentToolBar
    {
      get
      {
        return _parentToolBar;
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
    /// Text label of this item.
    /// </summary>
    [Category("Appearance")]
    [Description("Text label of this item.")]
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
    /// The text alignment to apply to this item.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(TextAlign.Left)]
    [Description("The text alignment to apply to this item.")]
    public TextAlign TextAlign
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("TextAlign")];
        return (o != null) ? Utils.ParseTextAlign(o) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemTextAlign :
          TextAlign.Left;
      }
      set
      {
        Properties[GetAttributeVarName("TextAlign")] = value.ToString();
      }
    }

    /// <summary>
    /// The position of item text and image relative to each other.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(ToolBarTextImageRelation.ImageBeforeText)]
    [Description("The position of item text and image relative to each other.")]
    public ToolBarTextImageRelation TextImageRelation
    {
      get
      {
        string o = Properties[GetAttributeVarName("TextImageRelation")];
        return (o != null) ? Utils.ParseToolBarTextImageRelation(o, ToolBarTextImageRelation.ImageBeforeText) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemTextImageRelation :
          ToolBarTextImageRelation.ImageBeforeText;
      }
      set
      {
        Properties[GetAttributeVarName("TextImageRelation")] = value.ToString();
      }
    }

    /// <summary>
    /// The gap in pixels between item text and image.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(0)]
    [Description("The gap in pixels between item text and image.")]
    public int TextImageSpacing
    {
      get
      {
        string o = Properties[GetAttributeVarName("TextImageSpacing")];
        return (o != null) ? Utils.ParseInt(o) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemTextImageSpacing : 0;
      }
      set
      {
        Properties[GetAttributeVarName("TextImageSpacing")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to wrap text in this item.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(false)]
    [Description("Whether to wrap text in this item.")]
    public bool TextWrap
    {
      get
      {
        string o = Properties[GetAttributeVarName("TextWrap")];
        return (o != null) ? Utils.ParseBool(o, false) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemTextWrap : false;
      }
      set
      {
        Properties[GetAttributeVarName("TextWrap")] = value.ToString();
      }
    }

    /// <summary>
    /// Identifier of the toggle group this item belongs to.
    /// </summary>
    /// <remarks>
    /// This property only has effect in items with <see cref="ItemType"/> of 
    /// <b>ToolBarItemType.ToggleRadio</b> and <b>ToolBarItemType.ToggleRadioCheck</b>.
    /// </remarks>
    [Category("Behavior")]
    [Description("Identifier of the toggle group this item belongs to.")]
    [DefaultValue(null)]
    public string ToggleGroupId
    {
      get
      {
        return this.Properties[GetAttributeVarName("ToggleGroupId")];
      }
      set
      {
        Properties[GetAttributeVarName("ToggleGroupId")] = value;
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
    /// Optional internal string value of this item.
    /// </summary>
    [Description("Optional internal string value of this item.")]
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
    /// Whether this item should be displayed.
    /// </summary>
    [Description("Whether this item should be displayed.")]
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

    /// <summary>
    /// Item's width.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit), "")]
    [Description("Item's width.")]
    public override Unit Width
    {
      get
      {
        string o = Properties[GetAttributeVarName("Width")];
        return (o != null) ? Utils.ParseUnit(o) :
          (this.ParentToolBar != null) ? this.ParentToolBar.DefaultItemWidth : Unit.Empty;
      }
      set
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("Width")] = value.ToString();
        }
        else
        {
          throw new Exception("Item dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    internal string PostBackID;

    internal void ReadXmlAttributes(XmlAttributeCollection attributeCollection)
    {
      // This takes over all attributes specified in Xml
      foreach (XmlAttribute attribute in attributeCollection)
      {
        this.Properties[attribute.Name] = attribute.Value;
      }
    }

    private System.Web.UI.AttributeCollection _properties;
    internal System.Web.UI.AttributeCollection Properties
    {
      get
      {
        if (_properties == null)
        {
          _properties = new System.Web.UI.AttributeCollection(new StateBag(true));
        }
        return _properties;
      }
      set
      {
        _properties = value;
      }
    }

    /// <summary>
    /// Get the name to be used for the given attribute, taking into consideration any attribute mappings.
    /// </summary>
    /// <param name="attributeName">The default name of the attribute.</param>
    /// <returns>The actual name of the attribute.</returns>
    internal string GetAttributeVarName(string attributeName)
    {
      if (this.ParentToolBar != null)
      {
        foreach (CustomAttributeMapping mapping in this.ParentToolBar.CustomAttributeMappings)
        {
          if (mapping.To.ToLower() == attributeName.ToLower())
          {
            return mapping.From;
          }
        }
      }
      return attributeName;
    }

    internal string GetVarAttributeName(string varName)
    {
      if (this.ParentToolBar != null)
      {
        foreach (CustomAttributeMapping mapping in this.ParentToolBar.CustomAttributeMappings)
        {
          if (mapping.From.ToLower() == varName.ToLower())
          {
            return mapping.To;
          }
        }
      }
      return varName;
    }

  }
}
