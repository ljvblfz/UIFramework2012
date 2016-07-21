using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Types of ItemLooks.
  /// </summary>
  internal enum ItemLookType
  {
    /// <summary>
    /// Default look.
    /// </summary>
    Normal,

    /// <summary>
    /// Look to be used when the associated item is selected.
    /// </summary>
    Selected,

    /// <summary>
    /// Look to be used when one of associated item's descendants is selected.
    /// </summary>
    ChildSelected,

    /// <summary>
    /// Look to be used when the associated item is disabled.
    /// </summary>
    Disabled
  }

	/// <summary>
  /// Provides members which along with CSS definitions define look and feel of <see cref="BaseMenuItem"/> instances. 
	/// </summary>
  [ToolboxItem(false)]
  public class ItemLook : ICloneable
	{
    #region Properties

    private Hashtable m_oProperties;

    internal bool FlatObject;
    internal bool ForDefaultSubItem;
    internal BaseMenuItem Item;
    internal ItemLookType LookType;

    
    private string _lookId;
    /// <summary>
    /// The ID of the look (or, in the case of a look translator, the last loaded look)
    /// </summary>
    [DefaultValue(null)]
    public string LookId
    {
      get
      {
        if(FlatObject)
        {
          return _lookId;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("LookId");
        }
        else
        {
          return (string)m_oProperties["LookId"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _lookId = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("LookId", value);
        }
        else
        {
          m_oProperties["LookId"] = value;
        }
      }
    }

    private Unit _labelPaddingBottom;
    /// <summary>
    /// The padding to apply to the bottom of the label.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit LabelPaddingBottom
    {
      get
      {
        if(FlatObject)
        {
          return _labelPaddingBottom;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("LabelPaddingBottom"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["LabelPaddingBottom"]);
        }
      }
      set
      {
        if(FlatObject)
        {
          _labelPaddingBottom = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("LabelPaddingBottom", value.ToString());
        }
        else
        {
          m_oProperties["LabelPaddingBottom"] = value.ToString();
        }
      }
    }

    private Unit _labelPaddingLeft;
    /// <summary>
    /// The padding to apply to the left of the label.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit LabelPaddingLeft
    {
      get
      {
        if(FlatObject)
        {
          return _labelPaddingLeft;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("LabelPaddingLeft"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["LabelPaddingLeft"]);
        }
      }
      set
      {
        if(FlatObject)
        {
          _labelPaddingLeft = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("LabelPaddingLeft", value.ToString());
        }
        else
        {
          m_oProperties["LabelPaddingLeft"] = value.ToString();
        }
      }
    }

    private Unit _labelPaddingRight;
    /// <summary>
    /// The padding to apply to the right of the label.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit LabelPaddingRight
    {
      get
      {
        if(FlatObject)
        {
          return _labelPaddingRight;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("LabelPaddingRight"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["LabelPaddingRight"]);
        }
      }
      set
      {
        if(FlatObject)
        {
          _labelPaddingRight = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("LabelPaddingRight", value.ToString());
        }
        else
        {
          m_oProperties["LabelPaddingRight"] = value.ToString();
        }
      }
    }

    private Unit _labelPaddingTop;
    /// <summary>
    /// The padding to apply to the top of the label.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit LabelPaddingTop
    {
      get
      {
        if(FlatObject)
        {
          return _labelPaddingTop;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("LabelPaddingTop"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["LabelPaddingTop"]);
        }
      }
      set
      {
        if(FlatObject)
        {
          _labelPaddingTop = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("LabelPaddingTop", value.ToString());
        }
        else
        {
          m_oProperties["LabelPaddingTop"] = value.ToString();
        }
      }
    }

    private string _cssClass;
    /// <summary>
    /// The CSS class to use.
    /// </summary>
    [DefaultValue(null)]
    public string CssClass
    {
      get
      {
        if(FlatObject)
        {
          return _cssClass;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("CssClass");
        }
        else
        {
          return (string)m_oProperties["CssClass"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _cssClass = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("CssClass", value);
        }
        else
        {
          m_oProperties["CssClass"] = value;
        }
      }
    }

    private Unit _leftIconHeight;
    /// <summary>
    /// The height of the left icon.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit LeftIconHeight
    {
      get
      {
        if(FlatObject)
        {
          return _leftIconHeight;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("LeftIconHeight"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["LeftIconHeight"]);
        }
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          if(FlatObject)
          {
            _leftIconHeight = value;
          }
          else if (this.Item != null)
          {
            this.SetProperty("LeftIconHeight", value.ToString());
          }
          else
          {
            m_oProperties["LeftIconHeight"] = value.ToString();
          }
        }
        else
        {
          throw new Exception("Icon dimensions may only be specified in pixels.");
        }
      }
    }

    private string _leftIconUrl;
    /// <summary>
    /// The URL of the left icon to use in this look.
    /// </summary>
    [DefaultValue(null)]
    public string LeftIconUrl
    {
      get
      {
        if(FlatObject)
        {
          return _leftIconUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("LeftIconUrl");
        }
        else
        {
          return (string)m_oProperties["LeftIconUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _leftIconUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("LeftIconUrl", value);
        }
        else
        {
          m_oProperties["LeftIconUrl"] = value;
        }
      }
    }

    private ItemIconVisibility _leftIconVisibility;
    /// <summary>
    /// When to show the left icon.
    /// </summary>
    [Description("When to show the left icon.")]
    [DefaultValue(ItemIconVisibility.Always)]
    public ItemIconVisibility LeftIconVisibility
    {
      get
      {
        if (FlatObject)
        {
          return _leftIconVisibility;
        }
        else if (this.Item != null)
        {
          return Utils.ParseItemIconVisibility(this.GetProperty("LeftIconVisibility"));
        }
        else
        {
          return Utils.ParseItemIconVisibility(m_oProperties["LeftIconVisibility"]);
        }
      }
      set
      {
        if (FlatObject)
        {
          _leftIconVisibility = value;
        }
        else if (this.Item != null)
        {
          this.SetProperty("LeftIconVisibility", value.ToString());
        }
        else
        {
          m_oProperties["LeftIconVisibility"] = value.ToString();
        }
      }
    }

    private Unit _leftIconWidth;
    /// <summary>
    /// The width of the left icon.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit LeftIconWidth
    {
      get
      {
        if(FlatObject)
        {
          return _leftIconWidth;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("LeftIconWidth"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["LeftIconWidth"]);
        }
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          if(FlatObject)
          {
            _leftIconWidth = value;
          }
          else if (this.Item != null)
          {
            this.SetProperty("LeftIconWidth", value.ToString());
          }
          else
          {
            m_oProperties["LeftIconWidth"] = value.ToString();
          }
        }
        else
        {
          throw new Exception("Icon dimensions may only be specified in pixels.");
        }
      }
    }

    private Unit _rightIconHeight;
    /// <summary>
    /// The height of the right icon.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit RightIconHeight
    {
      get
      {
        if(FlatObject)
        {
          return _rightIconHeight;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("RightIconHeight"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["RightIconHeight"]);
        }
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          if(FlatObject)
          {
            _rightIconHeight = value;
          }
          else if (this.Item != null)
          {
            this.SetProperty("RightIconHeight", value.ToString());
          }
          else
          {
            m_oProperties["RightIconHeight"] = value.ToString();
          }
        }
        else
        {
          throw new Exception("Icon dimensions may only be specified in pixels.");
        }
      }
    }

    private string _rightIconUrl;
    /// <summary>
    /// The URL of the right icon to use in this look.
    /// </summary>
    [DefaultValue(null)]
    public string RightIconUrl
    {
      get
      {
        if(FlatObject)
        {
          return _rightIconUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("RightIconUrl");
        }
        else
        {
          return (string)m_oProperties["RightIconUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _rightIconUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("RightIconUrl", value);
        }
        else
        {
          m_oProperties["RightIconUrl"] = value;
        }
      }
    }

    private ItemIconVisibility _rightIconVisibility;
    /// <summary>
    /// When to show the right icon.
    /// </summary>
    [Description("When to show the right icon.")]
    [DefaultValue(ItemIconVisibility.Always)]
    public ItemIconVisibility RightIconVisibility
    {
      get
      {
        if (FlatObject)
        {
          return _rightIconVisibility;
        }
        else if (this.Item != null)
        {
          return Utils.ParseItemIconVisibility(this.GetProperty("RightIconVisibility"));
        }
        else
        {
          return Utils.ParseItemIconVisibility(m_oProperties["RightIconVisibility"]);
        }
      }
      set
      {
        if (FlatObject)
        {
          _rightIconVisibility = value;
        }
        else if (this.Item != null)
        {
          this.SetProperty("RightIconVisibility", value.ToString());
        }
        else
        {
          m_oProperties["RightIconVisibility"] = value.ToString();
        }
      }
    }
    
    private Unit _rightIconWidth;
    /// <summary>
    /// The width of the right icon.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit RightIconWidth
    {
      get
      {
        if(FlatObject)
        {
          return _rightIconWidth;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("RightIconWidth"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["RightIconWidth"]);
        }
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          if(FlatObject)
          {
            _rightIconWidth = value;
          }
          else if (this.Item != null)
          {
            this.SetProperty("RightIconWidth", value.ToString());
          }
          else
          {
            m_oProperties["RightIconWidth"] = value.ToString();
          }
        }
        else
        {
          throw new Exception("Icon dimensions may only be specified in pixels.");
        }
      }
    }

    private Unit _imageHeight;
    /// <summary>
    /// The height of the image.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit ImageHeight
    {
      get
      {
        if(FlatObject)
        {
          return _imageHeight;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("ImageHeight"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["ImageHeight"]);
        }
      }
      set
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          if(FlatObject)
          {
            _imageHeight = value;
          }
          else if (this.Item != null)
          {
            this.SetProperty("ImageHeight", value.ToString());
          }
          else
          {
            m_oProperties["ImageHeight"] = value.ToString();
          }
        }
        else
        {
          throw new Exception("Image dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    private Unit _imageWidth;
    /// <summary>
    /// The width of the image.
    /// </summary>
    [DefaultValue(typeof(Unit),"")]
    public Unit ImageWidth
    {
      get
      {
        if(FlatObject)
        {
          return _imageWidth;
        }
        else if (this.Item != null)
        {
          return Utils.ParseUnit(this.GetProperty("ImageWidth"));
        }
        else
        {
          return Utils.ParseUnit(m_oProperties["ImageWidth"]);
        }
      }
      set
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          if(FlatObject)
          {
            _imageWidth = value;
          }
          else if (this.Item != null)
          {
            this.SetProperty("ImageWidth", value.ToString());
          }
          else
          {
            m_oProperties["ImageWidth"] = value.ToString();
          }
        }
        else
        {
          throw new Exception("Image dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    private string _imageUrl;
    /// <summary>
    /// The URL of the image to use in this look.
    /// </summary>
    [DefaultValue(null)]
    public string ImageUrl
    {
      get
      {
        if(FlatObject)
        {
          return _imageUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("ImageUrl");
        }
        else
        {
          return (string)m_oProperties["ImageUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _imageUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("ImageUrl", value);
        }
        else
        {
          m_oProperties["ImageUrl"] = value;
        }
      }
    }

    private string _hoverCssClass;
    /// <summary>
    /// The CSS class to use for this look on hover.
    /// </summary>
    [DefaultValue(null)]
    public string HoverCssClass
    {
      get
      {
        if(FlatObject)
        {
          return _hoverCssClass;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("HoverCssClass");
        }
        else
        {
          return (string)m_oProperties["HoverCssClass"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _hoverCssClass = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("HoverCssClass", value);
        }
        else
        {
          m_oProperties["HoverCssClass"] = value;
        }
      }
    }

    private string _hoverLeftIconUrl;
    /// <summary>
    /// The left icon to use for this look on hover.
    /// </summary>
    [DefaultValue(null)]
    public string HoverLeftIconUrl
    {
      get
      {
        if(FlatObject)
        {
          return _hoverLeftIconUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("HoverLeftIconUrl");
        }
        else
        {
          return (string)m_oProperties["HoverLeftIconUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _hoverLeftIconUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("HoverLeftIconUrl", value);
        }
        else
        {
          m_oProperties["HoverLeftIconUrl"] = value;
        }
      }
    }

    private string _hoverRightIconUrl;
    /// <summary>
    /// The right icon to use for this look on hover.
    /// </summary>
    [DefaultValue(null)]
    public string HoverRightIconUrl
    {
      get
      {
        if(FlatObject)
        {
          return _hoverRightIconUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("HoverRightIconUrl");
        }
        else
        {
          return (string)m_oProperties["HoverRightIconUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _hoverRightIconUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("HoverRightIconUrl", value);
        }
        else
        {
          m_oProperties["HoverRightIconUrl"] = value;
        }
      }
    }
    
    private string _hoverImageUrl;
    /// <summary>
    /// The image to use in this look on hover.
    /// </summary>
    [DefaultValue(null)]
    public string HoverImageUrl
    {
      get
      {
        if(FlatObject)
        {
          return _hoverImageUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("HoverImageUrl");
        }
        else
        {
          return (string)m_oProperties["HoverImageUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _hoverImageUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("HoverImageUrl", value);
        }
        else
        {
          m_oProperties["HoverImageUrl"] = value;
        }
      }
    }

    private string _activeCssClass;
    /// <summary>
    /// The CSS class to use in this look, when active/highlighted (ie. on mouse over, or keyboard highlight).
    /// </summary>
    [DefaultValue(null)]
    public string ActiveCssClass
    {
      get
      {
        if(FlatObject)
        {
          return _activeCssClass;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("ActiveCssClass");
        }
        else
        {
          return (string)m_oProperties["ActiveCssClass"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _activeCssClass = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("ActiveCssClass", value);
        }
        else
        {
          m_oProperties["ActiveCssClass"] = value;
        }
      }
    }

    private string _activeLeftIconUrl;
    /// <summary>
    /// The left icon to use in this look, when active.
    /// </summary>
    [DefaultValue(null)]
    public string ActiveLeftIconUrl
    {
      get
      {
        if(FlatObject)
        {
          return _activeLeftIconUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("ActiveLeftIconUrl");
        }
        else
        {
          return (string)m_oProperties["ActiveLeftIconUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _activeLeftIconUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("ActiveLeftIconUrl", value);
        }
        else
        {
          m_oProperties["ActiveLeftIconUrl"] = value;
        }
      }
    }

    private string _activeRightIconUrl;
    /// <summary>
    /// The right icon to use in this look, when active.
    /// </summary>
    [DefaultValue(null)]
    public string ActiveRightIconUrl
    {
      get
      {
        if(FlatObject)
        {
          return _activeRightIconUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("ActiveRightIconUrl");
        }
        else
        {
          return (string)m_oProperties["ActiveRightIconUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _activeRightIconUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("ActiveRightIconUrl", value);
        }
        else
        {
          m_oProperties["ActiveRightIconUrl"] = value;
        }
      }
    }

    private string _activeImageUrl;
    /// <summary>
    /// The URL of the image to use in this look, when active.
    /// </summary>
    [DefaultValue(null)]
    public string ActiveImageUrl
    {
      get
      {
        if(FlatObject)
        {
          return _activeImageUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("ActiveImageUrl");
        }
        else
        {
          return (string)m_oProperties["ActiveImageUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _activeImageUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("ActiveImageUrl", value);
        }
        else
        {
          m_oProperties["ActiveImageUrl"] = value;
        }
      }
    }

    private string _expandedCssClass;
    /// <summary>
    /// The CSS class to use in this look, when the item in question is expanded.
    /// </summary>
    [DefaultValue(null)]
    public string ExpandedCssClass
    {
      get
      {
        if(FlatObject)
        {
          return _expandedCssClass;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("ExpandedCssClass");
        }
        else
        {
          return (string)m_oProperties["ExpandedCssClass"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _expandedCssClass = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("ExpandedCssClass", value);
        }
        else
        {
          m_oProperties["ExpandedCssClass"] = value;
        }
      }
    }

    private string _expandedLeftIconUrl;
    /// <summary>
    /// The left icon to use in this look, when the item is expanded.
    /// </summary>
    [DefaultValue(null)]
    public string ExpandedLeftIconUrl
    {
      get
      {
        if(FlatObject)
        {
          return _expandedLeftIconUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("ExpandedLeftIconUrl");
        }
        else
        {
          return (string)m_oProperties["ExpandedLeftIconUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _expandedLeftIconUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("ExpandedLeftIconUrl", value);
        }
        else
        {
          m_oProperties["ExpandedLeftIconUrl"] = value;
        }
      }
    }

    private string _expandedRightIconUrl;
    /// <summary>
    /// The right icon to use when expanded.
    /// </summary>
    [DefaultValue(null)]
    public string ExpandedRightIconUrl
    {
      get
      {
        if(FlatObject)
        {
          return _expandedRightIconUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("ExpandedRightIconUrl");
        }
        else
        {
          return (string)m_oProperties["ExpandedRightIconUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _expandedRightIconUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("ExpandedRightIconUrl", value);
        }
        else
        {
          m_oProperties["ExpandedRightIconUrl"] = value;
        }
      }
    }
    
    private string _expandedImageUrl;
    /// <summary>
    /// The image to use when expanded.
    /// </summary>
    [DefaultValue(null)]
    public string ExpandedImageUrl
    {
      get
      {
        if(FlatObject)
        {
          return _expandedImageUrl;
        }
        else if(this.Item != null)
        {
          return this.GetProperty("ExpandedImageUrl");
        }
        else
        {
          return (string)m_oProperties["ExpandedImageUrl"];
        }
      }
      set
      {
        if(FlatObject)
        {
          _expandedImageUrl = value;
        }
        else if(this.Item != null)
        {
          this.SetProperty("ExpandedImageUrl", value);
        }
        else
        {
          m_oProperties["ExpandedImageUrl"] = value;
        }
      }
    }

    /// <summary>
    /// Whether nothing at all has been set in this look. Read-only.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsEmpty
    {
      get
      {
        return (
          this.CssClass == null && this.ImageUrl == null && this.LeftIconUrl == null && this.RightIconUrl == null &&
          this.ActiveCssClass == null && this.ActiveImageUrl == null && this.ActiveLeftIconUrl == null && this.ActiveRightIconUrl == null &&
          this.HoverCssClass == null && this.HoverImageUrl == null && this.HoverLeftIconUrl == null && this.HoverRightIconUrl == null &&
          this.ExpandedCssClass == null && this.ExpandedImageUrl == null && this.ExpandedLeftIconUrl == null && this.ExpandedRightIconUrl == null
          );
      }
    }

    #endregion

    #region Methods

    public ItemLook()
    {
      m_oProperties = new Hashtable();
    }

    internal ItemLook(bool bTranslator, bool bFlat)
    {
      FlatObject = bFlat;
    }

    internal ItemLook(bool bTranslator)
    {
    }

    /// <summary>
    /// Clear the contents of this look.
    /// </summary>
    public void Clear()
    {
      ItemLook oClearLook = new ItemLook();
      oClearLook.CopyTo(this);
    }

    /// <summary>
    /// Creates an exact duplicate of the this look object.
    /// </summary>
    /// <returns>The cloned ItemLook</returns>
    public object Clone()
    {
      ItemLook oClone = new ItemLook();

      this.CopyTo(oClone);      

      return oClone;
    }

    /// <summary>
    /// Copy the settings of this ItemLook to the specified ItemLook object.
    /// </summary>
    /// <param name="otherLook">The ItemLook to copy settings to</param>
    public void CopyTo(ItemLook otherLook)
    {
      CopyTo(otherLook, false);
    }

    /// <summary>
    /// Copy the settings of this ItemLook to the specified ItemLook object.
    /// </summary>
    /// <param name="otherLook">The ItemLook to copy settings to</param>
    /// <param name="bToNullsOnly">Whether to only copy to properties which are undefined, leaving the rest</param>
    public void CopyTo(ItemLook otherLook, bool bToNullsOnly)
    {
      CopyTo(otherLook, bToNullsOnly, false);
    }

    /// <summary>
    /// Copy the settings of this ItemLook to the specified ItemLook object.
    /// </summary>
    /// <param name="otherLook">The ItemLook to copy settings to</param>
    /// <param name="bToNullsOnly">Whether to only copy to properties which are undefined, leaving the rest</param>
    /// <param name="bFromNonNullsOnly">Whether to only copy from properties which are undefined, leaving the rest</param>
    public void CopyTo(ItemLook otherLook, bool bToNullsOnly, bool bFromNonNullsOnly)
    {
      if((!bToNullsOnly || otherLook.LookId == null) && (!bFromNonNullsOnly || this.LookId != null)) otherLook.LookId = this.LookId; // for internal use

      if((!bToNullsOnly || otherLook.LeftIconHeight == Unit.Empty) && (!bFromNonNullsOnly || this.LeftIconHeight != Unit.Empty)) otherLook.LeftIconHeight = this.LeftIconHeight;
      if((!bToNullsOnly || otherLook.LeftIconWidth == Unit.Empty) && (!bFromNonNullsOnly || this.LeftIconWidth != Unit.Empty)) otherLook.LeftIconWidth = this.LeftIconWidth;
      if((!bToNullsOnly || otherLook.RightIconHeight == Unit.Empty) && (!bFromNonNullsOnly || this.RightIconHeight != Unit.Empty)) otherLook.RightIconHeight = this.RightIconHeight;
      if((!bToNullsOnly || otherLook.RightIconWidth == Unit.Empty) && (!bFromNonNullsOnly || this.RightIconWidth != Unit.Empty)) otherLook.RightIconWidth = this.RightIconWidth;

      if ((!bToNullsOnly || otherLook.LeftIconVisibility == ItemIconVisibility.Always) && (!bFromNonNullsOnly || this.LeftIconVisibility != ItemIconVisibility.Always)) otherLook.LeftIconVisibility = this.LeftIconVisibility;
      if ((!bToNullsOnly || otherLook.RightIconVisibility == ItemIconVisibility.Always) && (!bFromNonNullsOnly || this.RightIconVisibility != ItemIconVisibility.Always)) otherLook.RightIconVisibility = this.RightIconVisibility;
      
      if((!bToNullsOnly || otherLook.ImageHeight == Unit.Empty) && (!bFromNonNullsOnly || this.ImageHeight != Unit.Empty)) otherLook.ImageHeight = this.ImageHeight;
      if((!bToNullsOnly || otherLook.ImageWidth == Unit.Empty) && (!bFromNonNullsOnly || this.ImageWidth != Unit.Empty)) otherLook.ImageWidth = this.ImageWidth;
      
      if((!bToNullsOnly || otherLook.LabelPaddingBottom == Unit.Empty) && (!bFromNonNullsOnly || this.LabelPaddingBottom != Unit.Empty)) otherLook.LabelPaddingBottom = this.LabelPaddingBottom;
      if((!bToNullsOnly || otherLook.LabelPaddingLeft == Unit.Empty) && (!bFromNonNullsOnly || this.LabelPaddingLeft != Unit.Empty)) otherLook.LabelPaddingLeft = this.LabelPaddingLeft;
      if((!bToNullsOnly || otherLook.LabelPaddingRight == Unit.Empty) && (!bFromNonNullsOnly || this.LabelPaddingRight != Unit.Empty)) otherLook.LabelPaddingRight = this.LabelPaddingRight;
      if((!bToNullsOnly || otherLook.LabelPaddingTop == Unit.Empty) && (!bFromNonNullsOnly || this.LabelPaddingTop != Unit.Empty)) otherLook.LabelPaddingTop = this.LabelPaddingTop;

      if((!bToNullsOnly || otherLook.CssClass == null) && (!bFromNonNullsOnly || this.CssClass != null)) otherLook.CssClass = this.CssClass;
      if((!bToNullsOnly || otherLook.ImageUrl == null) && (!bFromNonNullsOnly || this.ImageUrl != null)) otherLook.ImageUrl = this.ImageUrl;
      if((!bToNullsOnly || otherLook.LeftIconUrl == null) && (!bFromNonNullsOnly || this.LeftIconUrl != null)) otherLook.LeftIconUrl = this.LeftIconUrl;
      if((!bToNullsOnly || otherLook.RightIconUrl == null) && (!bFromNonNullsOnly || this.RightIconUrl != null)) otherLook.RightIconUrl = this.RightIconUrl;

      if((!bToNullsOnly || otherLook.HoverCssClass == null) && (!bFromNonNullsOnly || this.HoverCssClass != null)) otherLook.HoverCssClass = this.HoverCssClass;
      if((!bToNullsOnly || otherLook.HoverImageUrl == null) && (!bFromNonNullsOnly || this.HoverImageUrl != null)) otherLook.HoverImageUrl = this.HoverImageUrl;
      if((!bToNullsOnly || otherLook.HoverLeftIconUrl == null) && (!bFromNonNullsOnly || this.HoverLeftIconUrl != null)) otherLook.HoverLeftIconUrl = this.HoverLeftIconUrl;
      if((!bToNullsOnly || otherLook.HoverRightIconUrl == null) && (!bFromNonNullsOnly || this.HoverRightIconUrl != null)) otherLook.HoverRightIconUrl = this.HoverRightIconUrl;

      if((!bToNullsOnly || otherLook.ActiveCssClass == null) && (!bFromNonNullsOnly || this.ActiveCssClass != null)) otherLook.ActiveCssClass = this.ActiveCssClass;
      if((!bToNullsOnly || otherLook.ActiveImageUrl == null) && (!bFromNonNullsOnly || this.ActiveImageUrl != null)) otherLook.ActiveImageUrl = this.ActiveImageUrl;
      if((!bToNullsOnly || otherLook.ActiveLeftIconUrl == null) && (!bFromNonNullsOnly || this.ActiveLeftIconUrl != null)) otherLook.ActiveLeftIconUrl = this.ActiveLeftIconUrl;
      if((!bToNullsOnly || otherLook.ActiveRightIconUrl == null) && (!bFromNonNullsOnly || this.ActiveRightIconUrl != null)) otherLook.ActiveRightIconUrl = this.ActiveRightIconUrl;

      if((!bToNullsOnly || otherLook.ExpandedCssClass == null) && (!bFromNonNullsOnly || this.ExpandedCssClass != null)) otherLook.ExpandedCssClass = this.ExpandedCssClass;
      if((!bToNullsOnly || otherLook.ExpandedImageUrl == null) && (!bFromNonNullsOnly || this.ExpandedImageUrl != null)) otherLook.ExpandedImageUrl = this.ExpandedImageUrl;
      if((!bToNullsOnly || otherLook.ExpandedLeftIconUrl == null) && (!bFromNonNullsOnly || this.ExpandedLeftIconUrl != null)) otherLook.ExpandedLeftIconUrl = this.ExpandedLeftIconUrl;
      if((!bToNullsOnly || otherLook.ExpandedRightIconUrl == null) && (!bFromNonNullsOnly || this.ExpandedRightIconUrl != null)) otherLook.ExpandedRightIconUrl = this.ExpandedRightIconUrl;
    }

    private string GetProperty(string sProperty)
    {
      string sPrefix = this.ForDefaultSubItem? "DefaultSubItem" : "";
      string sLookType = this.LookType == ItemLookType.Normal ? "" : this.LookType.ToString();

      return Item.Properties[Item.GetAttributeVarName(sPrefix + sLookType + "Look-" + sProperty)]; 
    }

    private void SetProperty(string sProperty, string sValue)
    {
      string sPrefix = this.ForDefaultSubItem? "DefaultSubItem" : "";
      string sLookType = this.LookType == ItemLookType.Normal ? "" : this.LookType.ToString();

      string sKey = Item.GetAttributeVarName(sPrefix + sLookType + "Look-" + sProperty);

      Item.Properties.Remove(sKey);

      if(sValue != null)
      {
        Item.Properties.Add(sKey, sValue);
      }
    }

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();
      
      oSB.Append("<Look");

      foreach(string sKey in m_oProperties.Keys)
      {
        oSB.AppendFormat(" {0}=\"{1}\"", sKey, m_oProperties[sKey]);
      }

      oSB.Append(" />");

      return oSB.ToString();
    }

    internal void LoadXml(string sXml)
    {
      XmlDocument oXmlDoc = new XmlDocument();
      oXmlDoc.LoadXml(sXml);

      foreach(XmlAttribute oAttr in oXmlDoc.DocumentElement.Attributes)
      {
        m_oProperties[oAttr.Name] = oAttr.Value;
      }
    }

    #endregion
	}
}
