using System;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace ComponentArt.Web.UI
{
	/// <summary>
  /// Provides common logic such as <see cref="ItemLook"/> handling to its descendent controls.
	/// </summary>
  /// <remarks>
  /// All menu-based ComponentArt navigation controls inherit from this class.
  /// </remarks>
	public abstract class BaseMenu : BaseNavigator
	{
    #region Properties

		#region Unused hidden inheritance

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override System.Drawing.Color BackColor
		{
			get { return base.BackColor; }
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override System.Drawing.Color ForeColor
		{
			get { return base.ForeColor; }
		}

    #endregion

    internal BaseMenuItem m_oForceHighlightedItem = null;

    /// <summary>
    /// Constructor
    /// </summary>
    public BaseMenu() : base()
    {
      // Set some defaults.
      this.CollapseDuration = 200;
      this.CollapseSlide = SlideType.ExponentialDecelerate;
      this.ExpandDuration = 200;
      this.ExpandSlide = SlideType.ExponentialDecelerate;
    }

    /// <summary>
    /// The duration of the collapse animation, in milliseconds.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="BaseMenu.CollapseSlide" /> property, providing full control
    /// over the collapse animation of item groups.
    /// </para>
    /// </remarks>
    /// <seealso cref="CollapseSlide" />
    /// <seealso cref="CollapseTransition" />
		/// <seealso cref="CollapseTransitionCustomFilter" />
    [Category("Animation")]
		[Description("The duration of the collapse animation, in milliseconds.")]
    [DefaultValue(200)]
    public int CollapseDuration
    {
      get 
      {
        return Utils.ParseInt(ViewState["CollapseDuration"]);
      }
      set 
      {
        ViewState["CollapseDuration"] = value;
      }
    }

    /// <summary>
    /// The slide type to use for the collapse animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the behavior of the animation which is used when collapsing groups of <see cref="BaseMenuItem">items</see>.
    /// It is used in conjunction with the <see cref="BaseMenu.CollapseDuration" /> property to fully describe the behavior of the animation.
    /// </para>
    /// <para>
    /// The <see cref="ExpandSlide" /> property is used for the expand animation. 
    /// </para>
    /// </remarks>
    /// <seealso cref="CollapseDuration" />
    /// <seealso cref="CollapseTransition" />
    /// <seealso cref="CollapseTransitionCustomFilter" />
    [Category("Animation")]
		[Description("The slide type to use for the collapse animation.")]
    [DefaultValue(SlideType.ExponentialDecelerate)]
    public SlideType CollapseSlide
    {
      get 
      {
        return Utils.ParseSlideType(ViewState["CollapseSlide"]);
      }
      set 
      {
        ViewState["CollapseSlide"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for the collapse animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to select an IE filter to use when animating the collapse of item groups. 
    /// This is an Internet Explorer only feature, and will not have any effect in other browsers.
    /// </para>
    /// <para>
    /// If <code>TransitionType.custom</code> is selected, the custom filter must be defined using the 
    /// <see cref="BaseMenu.CollapseTransitionCustomFilter" /> property. 
    /// </para>
    /// </remarks>
    /// <seealso cref="CollapseDuration" />
    /// <seealso cref="CollapseSlide" />
    /// <seealso cref="CollapseTransitionCustomFilter" />
    [Category("Animation")]
		[Description("The transition effect to use for the collapse animation.")]
    [DefaultValue(TransitionType.None)]
    public TransitionType CollapseTransition
    {
      get 
      {
        return Utils.ParseTransitionType(ViewState["CollapseTransition"]);
      }
      set 
      {
        ViewState["CollapseTransition"] = value;
      }
    }

    /// <summary>
    /// The custom transition filter to use for the collapse animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="BaseMenu.CollapseTransition" /> property. It is used to specify
    /// a custom IE filter to use for collapse animations. This is an Internet Explorer only feature, and will not work with other browsers.
    /// </para>
    /// </remarks>
    /// <seealso cref="CollapseDuration" />
    /// <seealso cref="CollapseSlide" />
    /// <seealso cref="CollapseTransition" />
    [Category("Animation")]
		[Description("The custom transition filter to use for the collapse animation.")]
    [DefaultValue(null)]
    public string CollapseTransitionCustomFilter
    {
      get
      {
        return (string)ViewState["CollapseTransitionCustomFilter"];
      }
      set
      {
        ViewState["CollapseTransitionCustomFilter"] = value;
      }
    }

    private ItemLook _defaultChildSelectedItemLook;
    /// <summary>
    /// The default look to apply to ancestors of the selected item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property sets a default <see cref="ItemLook" /> to use for styling items which are
    /// ancestors of the selected item. Because it is a default value, it will be used for any items which have 
    /// not specifically overridden the property at the item-level 
    /// (<see cref="BaseMenuItem.ChildSelectedLook" /> or <see cref="BaseMenuItem.DefaultSubItemChildSelectedLook" />).
    /// </para>
    /// </remarks>
    /// <seealso cref="BaseMenuItem.DefaultChildSelectedItemLookId" /> 
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook DefaultChildSelectedItemLook
    {
      get
      {
        if(_defaultChildSelectedItemLook == null)
        {
          _defaultChildSelectedItemLook = new ItemLook(false, true);
        }
        return _defaultChildSelectedItemLook;
      }
      set
      {
        if(value != null)
        {
          _defaultChildSelectedItemLook = (ItemLook)value.Clone();
        }
      }
    }
    
    /// <summary>
    /// The ID of the default look to apply to ancestors of the selected item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property sets the ID of an <see cref="ItemLook" /> to use as the default for styling items which are
    /// ancestors of the selected item. Because it is a default value, it will be used for any items which have 
    /// not specifically overridden the property at the item-level 
    /// (<see cref="BaseMenuItem.ChildSelectedLookId" /> or <see cref="BaseMenuItem.DefaultSubItemChildSelectedLookId" />).
    /// </para>
    /// </remarks>
    /// <seealso cref="BaseMenuItem.DefaultChildSelectedItemLook" /> 
    [Description("The ID of the default look to apply to ancestors of the selected item.")]
    [DefaultValue("")]
    [Category("ItemLook")]
    public string DefaultChildSelectedItemLookId
    {
      get 
      {
        object o = ViewState["DefaultChildSelectedItemLookId"]; 
        return (o == null) ? string.Empty : (string)o; 
      }
      set 
      {
        ViewState["DefaultChildSelectedItemLookId"] = value;
      }
    }

    private ItemLook _defaultDisabledItemLook;
    /// <summary>
    /// The default look to apply to disabled items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.DefaultSubItemDisabledLook" /> or <see cref="BaseMenuItem.DisabledLook" />).
    /// </para>
    /// </remarks>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook DefaultDisabledItemLook
    {
      get
      {
        if(_defaultDisabledItemLook == null)
        {
          _defaultDisabledItemLook = new ItemLook(false, true);
        }
        return _defaultDisabledItemLook;
      }
      set
      {
        if(value != null)
        {
          _defaultDisabledItemLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the default look to apply to disabled items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.DefaultSubItemDisabledLookId" /> or <see cref="BaseMenuItem.DisabledLookId" />).
    /// </para>
    /// </remarks>
    [Description("The ID of the default look to apply to disabled items.")]
    [DefaultValue("")]
    [Category("ItemLook")]
    public string DefaultDisabledItemLookId
    {
      get 
      {
        object o = ViewState["DefaultDisabledItemLookId"]; 
        return (o == null) ? string.Empty : (string)o; 
      }
      set 
      {
        ViewState["DefaultDisabledItemLookId"] = value;
      }
    }

    /// <summary>
    /// The default CSS class to apply to groups
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.DefaultSubGroupCssClass" />).
    /// </para>
    /// </remarks>
    [Description("The default CSS class to apply to groups")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultGroupCssClass
    {
      get 
      {
        return (string)ViewState["DefaultGroupCssClass"];
      }
      set 
      {
        ViewState["DefaultGroupCssClass"] = value;
      }
    }

    private ItemLook _defaultItemLook;
    /// <summary>
    /// The default look to apply to items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.DefaultSubItemLook" /> or <see cref="BaseMenuItem.Look" />).
    /// </para>
    /// </remarks>
    /// <seealso cref="BaseMenuItem.Look" />
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook DefaultItemLook
    {
      get
      {
        if(_defaultItemLook == null)
        {
          _defaultItemLook = new ItemLook(false, true);
        }
        return _defaultItemLook;
      }
      set
      {
        if(value != null)
        {
          _defaultItemLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the default look to apply to items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.DefaultSubItemLookId" /> or <see cref="BaseMenuItem.LookId" />).
    /// </para>
    /// </remarks>
    [Description("The ID of the default look to apply to items.")]
    [DefaultValue("")]
    [Category("ItemLook")]
    public string DefaultItemLookId
    {
      get 
      {
        object o = ViewState["DefaultItemLookId"]; 
        return (o == null) ? string.Empty : (string)o; 
      }
      set 
      {
        ViewState["DefaultItemLookId"] = value;
      }
    }

    /// <summary>
    /// The default text alignment to apply to labels.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.SubItemTextAlign" /> or <see cref="BaseMenuItem.TextAlign" />).
    /// </para>
    /// </remarks>
    /// <seealso cref="BaseMenuItem.TextAlign" />
    [Category("Appearance")]
    [DefaultValue(TextAlign.Left)]
    [Description("The default text alignment to apply to labels.")]
    public TextAlign DefaultItemTextAlign
    {
      get
      {
        return Utils.ParseTextAlign(ViewState["DefaultItemTextAlign"]);
      }
      set
      {
        ViewState["DefaultItemTextAlign"] = value;
      }
    }

    /// <summary>
    /// Whether to permit text wrapping in labels by default.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.SubItemTextWrap" /> or <see cref="BaseMenuItem.TextWrap" />).
    /// </para>
    /// </remarks>
    /// <seealso cref="BaseMenuItem.TextWrap" />
    [Category("Layout")]
    [DefaultValue(false)]
    [Description("Whether to permit text wrapping in labels by default.")]
    public bool DefaultItemTextWrap
    {
      get
      {
        return Utils.ParseBool(ViewState["DefaultItemTextWrap"], false);
      }
      set
      {
        ViewState["DefaultItemTextWrap"] = value;
      }
    }

    private ItemLook _defaultSelectedItemLook;
    /// <summary>
    /// The default look to apply to the selected item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.DefaultSubItemSelectedLook" /> or <see cref="BaseMenuItem.SelectedLook" />).
    /// </para>
    /// </remarks>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook DefaultSelectedItemLook
    {
      get
      {
        if(_defaultSelectedItemLook == null)
        {
          _defaultSelectedItemLook = new ItemLook(false, true);
        }
        return _defaultSelectedItemLook;
      }
      set
      {
        if(value != null)
        {
          _defaultSelectedItemLook = (ItemLook)value.Clone();
        }
      }
    }

    /// <summary>
    /// The ID of the default look to apply to the selected items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since this is a default value, it will be used for any items which have not specifically overridden
    /// the property at the item-level(<see cref="BaseMenuItem.DefaultSubItemSelectedLookId" /> or <see cref="BaseMenuItem.SelectedLookId" />).
    /// </para>
    /// </remarks>
    [Description("The ID of the default look to apply to the selected items.")]
    [DefaultValue("")]
    [Category("ItemLook")]
    public string DefaultSelectedItemLookId
    {
      get 
      {
        object o = ViewState["DefaultSelectedItemLookId"]; 
        return (o == null) ? string.Empty : (string)o; 
      }
      set 
      {
        ViewState["DefaultSelectedItemLookId"] = value;
      }
    }
    
    /// <summary>
    /// The duration of the expand animation, in milliseconds.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="BaseMenu.ExpandSlide" /> property, providing
    /// full control over the expand animation of item groups.
    /// </para>
    /// </remarks>
    /// <seealso cref="ExpandSlide" />
    /// <seealso cref="ExpandTransition" />
    /// <seealso cref="ExpandTransitionCustomFilter" />
    [Category("Animation")]
		[Description("The duration of the expand animation, in milliseconds.")]
    [DefaultValue(200)]
    public int ExpandDuration
    {
      get 
      {
        return Utils.ParseInt(ViewState["ExpandDuration"]);
      }
      set 
      {
        ViewState["ExpandDuration"] = value;
      }
    }

    /// <summary>
    /// The slide type to use for the expand animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the behavior of the animation which is used when expanding groups of <see cref="BaseMenuItem">items</see>.
    /// It is used in conjunction with the <see cref="BaseMenu.ExpandDuration" /> property to fully describe the behavior of the animation.
    /// </para>
    /// <para>
    /// The <see cref="CollapseSlide" /> property is used for the collapse animation. 
    /// </para>
    /// </remarks>
    /// <seealso cref="ExpandDuration" />
    /// <seealso cref="ExpandTransition" />
    /// <seealso cref="ExpandTransitionCustomFilter" />
    [Category("Animation")]
		[Description("The slide type to use for the expand animation.")]
    [DefaultValue(SlideType.ExponentialDecelerate)]
    public SlideType ExpandSlide
    {
      get
      {
        return Utils.ParseSlideType(ViewState["ExpandSlide"]);
      }
      set
      {
        ViewState["ExpandSlide"] = value;
      }
    }

    /// <summary>
    /// The transition effect to use for the expand animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to select an IE filter to use when animating the expansion of item groups. 
    /// This is an Internet Explorer only feature, and will not have any effect in other browsers.
    /// </para>
    /// <para>
    /// If <code>TransitionType.custom</code> is selected, the custom filter must be defined using the 
    /// <see cref="BaseMenu.ExpandTransitionCustomFilter" /> property. 
    /// </para>
    /// </remarks>
    /// <seealso cref="ExpandSlide" />
    /// <seealso cref="ExpandDuration" />
    /// <seealso cref="ExpandTransitionCustomFilter" />
    [Category("Animation")]
		[Description("The transition effect to use for the expand animation.")]
    [DefaultValue(TransitionType.None)]
    public TransitionType ExpandTransition
    {
      get
      {
        return Utils.ParseTransitionType(ViewState["ExpandTransition"]);
      }
      set
      {
        ViewState["ExpandTransition"] = value;
      }
    }

    /// <summary>
    /// The custom transition filter to use for the expand animation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used in conjunction with the <see cref="BaseMenu.ExpandTransition" /> property. It is used to specify
    /// a custom IE filter to use for when animating the expansion of item groups. 
    /// This is an Internet Explorer only feature, and will not work with other browsers.
    /// </para>
    /// </remarks>    
    /// <seealso cref="ExpandSlide" />
    /// <seealso cref="ExpandDuration" />
    /// <seealso cref="ExpandTransition" />
    [Category("Animation")]
		[Description("The custom transition filter to use for the expand animation.")]
    [DefaultValue(null)]
    public string ExpandTransitionCustomFilter
    {
      get
      {
        return (string)ViewState["ExpandTransitionCustomFilter"];
      }
      set
      {
        ViewState["ExpandTransitionCustomFilter"] = value;
      }
    }

    /// <summary>
    /// ID of item to forcefully highlight. This will make it appear as it would when selected.
    /// </summary>
		[Category("Appearance")]
		[Description("ID of item to forcefully highlight.")]
    [DefaultValue("")]
    public string ForceHighlightedItemID
    {
      get
      {
        return base.ForceHighlightedNodeID;
      }

      set
      {
        base.ForceHighlightedNodeID = value; 
      }
    }

    private ItemLookCollection _looks;
    /// <summary>
    /// The collection of looks defined for this control.
    /// </summary>
    /// <remarks>
    /// The ItemLooks defined in this collection can be referenced by Default look properties on the control using the corresponding LookId
    /// property. For example, setting DefaultItemLookId to the LookId of one of the pre-defined ItemLooks will load it into DefaultItemLook.
    /// Similarly, Items can reference ItemLooks defined in their parent control.
    /// </remarks>
    /// <seealso cref="BaseMenuItem.Look" />
    [Description("The collection of looks defined for this control.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Category("ItemLook")]
    public ItemLookCollection ItemLooks
    {
      get
      {
        if(_looks == null)
        {
          _looks = new ItemLookCollection();
        }

        return _looks;
      }
    }

    /// <summary>
    /// ID of item to begin rendering down from.
    /// </summary>
    [Description("ID of item to begin rendering down from.")]
    [DefaultValue("")]
    [Category("Data")]
    public string RenderRootItemId
    {
      get 
      {
        return base.RenderRootNodeId;
      }
      set 
      {
        base.RenderRootNodeId = value;
      }
    }

    /// <summary>
    /// Whether to include the RenderRootItem when rendering, instead of only its children. Default: false.
    /// </summary>
    [Category("Data")]
    [Description("Whether to include the RenderRootItem when rendering, instead of only its children. Default: false.")]
    [DefaultValue(false)]
    public bool RenderRootItemInclude
    {
      get 
      {
        return base.RenderRootNodeInclude;
      }
      set 
      {
        base.RenderRootNodeInclude = value;
      }
    }

    #endregion

    #region Methods

    protected override NavigationNode AddNode()
    {
      return null;
    }
    
    /// <summary>
    /// Apply looks to the data: Load specified looks by ID, and apply them.
    /// If called explicitly, this method will overwrite some look settings which were set on individual nodes.
    /// </summary>
    public virtual void ApplyLooks()
    {
      if(this.DefaultItemLookId != string.Empty)
      {
        this.DefaultItemLook = this.ItemLooks[this.DefaultItemLookId];
      }
      if(this.DefaultSelectedItemLookId != string.Empty)
      {
        this.DefaultSelectedItemLook = this.ItemLooks[this.DefaultSelectedItemLookId];
      }
      if(this.DefaultChildSelectedItemLookId != string.Empty)
      {
        this.DefaultChildSelectedItemLook = this.ItemLooks[this.DefaultChildSelectedItemLookId];
      }
      if(this.DefaultDisabledItemLookId != string.Empty)
      {
        this.DefaultDisabledItemLook = this.ItemLooks[this.DefaultDisabledItemLookId];
      }

      if(this.nodes != null && this.nodes.Count > 0)
      {
        foreach(BaseMenuItem oItem in this.nodes)
        {
          oItem.navigator = this;
          oItem.ApplyLooks();
        }
      }
    }

    internal BaseMenuItem FindItemById(string sNodeID)
    {
      return (BaseMenuItem)base.FindNodeById(sNodeID);
    }

    private void LoadPreloadImagesRecursive(NavigationNodeCollection arItems)
    {
      foreach(BaseMenuItem oItem in arItems)
      {
        string [] arProperties = new string [] {
          oItem.EffectiveLook.ImageUrl, 
          oItem.EffectiveLook.LeftIconUrl,
          oItem.EffectiveLook.RightIconUrl,
          oItem.EffectiveLook.ActiveImageUrl,
          oItem.EffectiveLook.ActiveLeftIconUrl,
          oItem.EffectiveLook.ActiveRightIconUrl,
          oItem.EffectiveLook.ExpandedImageUrl,
          oItem.EffectiveLook.ExpandedLeftIconUrl,
          oItem.EffectiveLook.ExpandedRightIconUrl,
          oItem.EffectiveLook.HoverImageUrl,
          oItem.EffectiveLook.HoverLeftIconUrl,
          oItem.EffectiveLook.HoverRightIconUrl};

        // if its an image, add to preloadimages
        foreach(string sValue in arProperties)
        {
          if(sValue != null && sValue != string.Empty)
          {
            string sPreloadImage = ConvertImageUrl(sValue);

            // add sValue to menu.preloadimages if not already there
            if(!this.PreloadImages.Contains(sPreloadImage))
            {
              this.PreloadImages.Add(sPreloadImage);
            } 
          }
        }

        if(oItem.nodes != null)
        {
          LoadPreloadImagesRecursive(oItem.nodes);
        }
      }
    }

    protected virtual void LoadPreloadImages()
    {
      if(this.nodes != null)
      {
        this.LoadPreloadImagesRecursive(this.nodes);
      }
    }

    protected override bool IsDownLevel()
    {
      if (this.ClientTarget == ClientTargetLevel.Downlevel) return true;
      if (this.ClientTarget == ClientTargetLevel.Uplevel) return false;
      
      return false;
    }

    /// <summary>
    /// Generates an array of ItemLook properties that are to be propagated to the client side.
    /// Intended to be used with control-level looks (the ones in ItemLook collection).
    /// </summary>
    /// <param name="look">The look to process.</param>
    /// <returns>An ArrayList of relevant look properties. All properties are processed.</returns>
    internal JavaScriptArray ProcessLook(ItemLook look)
    {
      /* NOTE: The order of the ArrayList elements must match the order of their client-side 
       * equivalents in ComponentArt_ItemLook.PermanentProperties array in A573S188.js. */
      JavaScriptArray lookProperties = new JavaScriptArray(JavaScriptArrayType.Sparse);
      lookProperties.Add(look.LookId); // 'LookId'
      lookProperties.Add(look.CssClass); // 'CssClass'
      lookProperties.Add(look.HoverCssClass); // 'HoverCssClass'
      lookProperties.Add(look.ImageHeight); // 'ImageHeight'
      lookProperties.Add(look.ImageWidth); // 'ImageWidth'
      lookProperties.Add(look.LabelPaddingBottom); // 'LabelPaddingBottom'
      lookProperties.Add(look.LabelPaddingLeft); // 'LabelPaddingLeft'
      lookProperties.Add(look.LabelPaddingRight); // 'LabelPaddingRight'
      lookProperties.Add(look.LabelPaddingTop); // 'LabelPaddingTop'
      lookProperties.Add(look.ActiveCssClass); // 'ActiveCssClass'
      lookProperties.Add(look.ExpandedCssClass); // 'ExpandedCssClass'
      lookProperties.Add(look.LeftIconUrl); // 'LeftIconUrl'
      lookProperties.Add(look.HoverLeftIconUrl); // 'HoverLeftIconUrl'
      lookProperties.Add(look.LeftIconWidth); // 'LeftIconWidth'
      lookProperties.Add(look.LeftIconHeight); // 'LeftIconHeight'
      lookProperties.Add(look.ActiveLeftIconUrl); // 'ActiveLeftIconUrl'
      lookProperties.Add(look.ExpandedLeftIconUrl); // 'ExpandedLeftIconUrl'
      lookProperties.Add(look.RightIconUrl); // 'RightIconUrl'
      lookProperties.Add(look.HoverRightIconUrl); // 'HoverRightIconUrl'
      lookProperties.Add(look.RightIconWidth); // 'RightIconWidth'
      lookProperties.Add(look.RightIconHeight); // 'RightIconHeight'
      lookProperties.Add(look.ActiveRightIconUrl); // 'ActiveRightIconUrl'
      lookProperties.Add(look.ExpandedRightIconUrl); // 'ExpandedRightIconUrl'
      lookProperties.Add(look.ImageUrl); // 'ImageUrl'
      lookProperties.Add(look.HoverImageUrl); // 'HoverImageUrl'
      lookProperties.Add(look.ActiveImageUrl); // 'ActiveImageUrl'
      lookProperties.Add(look.ExpandedImageUrl); // 'ExpandedImageUrl'
      lookProperties.Add(look.RightIconVisibility); // 'RightIconVisibility'
      lookProperties.Add(look.LeftIconVisibility); // 'LeftIconVisibility'
      return lookProperties;
    }

    /// <summary>
    /// Given two looks, returns the difference between them.
    /// </summary>
    /// <param name="effectiveLook">Left-hand side of the subtraction.</param>
    /// <param name="originalLook">Right-hand side of the subtraction.</param>
    /// <returns>An ItemLook containing only those properties of the effectiveLook which 
    /// are different from the corresponding originalLook property.</returns>
    internal ItemLook LookDifference(ItemLook effectiveLook, ItemLook originalLook)
    {
      ItemLook difference;
      if (originalLook == null)
      {
        /* Copy all the values from effectiveLook.  Ensure that LookId is null. */
        difference = (ItemLook) effectiveLook.Clone();
        difference.LookId = null;
      }
      else
      {
        /* Make sure the LookId matches originalLook's.  Include only the properties which are 
         * different in effectiveLook from the ones in originalLook. */
        difference = new ItemLook();
        difference.LookId = originalLook.LookId;
        difference.ActiveCssClass = effectiveLook.ActiveCssClass != originalLook.ActiveCssClass ? effectiveLook.ActiveCssClass : null;
        difference.ActiveImageUrl = effectiveLook.ActiveImageUrl != originalLook.ActiveImageUrl ? effectiveLook.ActiveImageUrl : null;
        difference.ActiveLeftIconUrl = effectiveLook.ActiveLeftIconUrl != originalLook.ActiveLeftIconUrl ? effectiveLook.ActiveLeftIconUrl : null;
        difference.ActiveRightIconUrl = effectiveLook.ActiveRightIconUrl != originalLook.ActiveRightIconUrl ? effectiveLook.ActiveRightIconUrl : null;
        difference.CssClass = effectiveLook.CssClass != originalLook.CssClass ? effectiveLook.CssClass : null;
        difference.ExpandedCssClass = effectiveLook.ExpandedCssClass != originalLook.ExpandedCssClass ? effectiveLook.ExpandedCssClass : null;
        difference.ExpandedImageUrl = effectiveLook.ExpandedImageUrl != originalLook.ExpandedImageUrl ? effectiveLook.ExpandedImageUrl : null;
        difference.ExpandedLeftIconUrl = effectiveLook.ExpandedLeftIconUrl != originalLook.ExpandedLeftIconUrl ? effectiveLook.ExpandedLeftIconUrl : null;
        difference.ExpandedRightIconUrl = effectiveLook.ExpandedRightIconUrl != originalLook.ExpandedRightIconUrl ? effectiveLook.ExpandedRightIconUrl : null;
        difference.HoverCssClass = effectiveLook.HoverCssClass != originalLook.HoverCssClass ? effectiveLook.HoverCssClass : null;
        difference.HoverImageUrl = effectiveLook.HoverImageUrl != originalLook.HoverImageUrl ? effectiveLook.HoverImageUrl : null;
        difference.HoverLeftIconUrl = effectiveLook.HoverLeftIconUrl != originalLook.HoverLeftIconUrl ? effectiveLook.HoverLeftIconUrl : null;
        difference.HoverRightIconUrl = effectiveLook.HoverRightIconUrl != originalLook.HoverRightIconUrl ? effectiveLook.HoverRightIconUrl : null;
        difference.ImageHeight = effectiveLook.ImageHeight != originalLook.ImageHeight ? effectiveLook.ImageHeight : Unit.Empty;
        difference.ImageUrl = effectiveLook.ImageUrl != originalLook.ImageUrl ? effectiveLook.ImageUrl : null;
        difference.ImageWidth = effectiveLook.ImageWidth != originalLook.ImageWidth ? effectiveLook.ImageWidth : Unit.Empty;
        difference.LabelPaddingBottom = effectiveLook.LabelPaddingBottom != originalLook.LabelPaddingBottom ? effectiveLook.LabelPaddingBottom : Unit.Empty;
        difference.LabelPaddingLeft = effectiveLook.LabelPaddingLeft != originalLook.LabelPaddingLeft ? effectiveLook.LabelPaddingLeft : Unit.Empty;
        difference.LabelPaddingRight = effectiveLook.LabelPaddingRight != originalLook.LabelPaddingRight ? effectiveLook.LabelPaddingRight : Unit.Empty;
        difference.LabelPaddingTop = effectiveLook.LabelPaddingTop != originalLook.LabelPaddingTop ? effectiveLook.LabelPaddingTop : Unit.Empty;
        difference.LeftIconHeight = effectiveLook.LeftIconHeight != originalLook.LeftIconHeight ? effectiveLook.LeftIconHeight : Unit.Empty;
        difference.LeftIconUrl = effectiveLook.LeftIconUrl != originalLook.LeftIconUrl ? effectiveLook.LeftIconUrl : null;
        difference.LeftIconVisibility = effectiveLook.LeftIconVisibility != originalLook.LeftIconVisibility ? effectiveLook.LeftIconVisibility : ItemIconVisibility.Always;
        difference.LeftIconWidth = effectiveLook.LeftIconWidth != originalLook.LeftIconWidth ? effectiveLook.LeftIconWidth : Unit.Empty;
        difference.RightIconHeight = effectiveLook.RightIconHeight != originalLook.RightIconHeight ? effectiveLook.RightIconHeight : Unit.Empty;
        difference.RightIconUrl = effectiveLook.RightIconUrl != originalLook.RightIconUrl ? effectiveLook.RightIconUrl : null;
        difference.RightIconVisibility = effectiveLook.RightIconVisibility != originalLook.RightIconVisibility ? effectiveLook.RightIconVisibility : ItemIconVisibility.Always;
        difference.RightIconWidth = effectiveLook.RightIconWidth != originalLook.RightIconWidth ? effectiveLook.RightIconWidth : Unit.Empty;
      }
      return difference;
    }

    #endregion
  }

  #region enums

  /// <summary>
  /// Specifies the movement pattern for various moving and resizing animations.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration describes the behavior of various movement animations associated with Web.UI Controls. It is generally used in
  /// conjunction with a <code>Duration</code> property, providing fine control over the resulting animation.
  /// </para>
  /// </remarks>
  public enum SlideType
  {
    /// <summary>No slide.</summary>
    None,

    /// <summary>Exponential speed-up slide.</summary>
    ExponentialAccelerate,

    /// <summary>Exponential slow-down slide.</summary>
    ExponentialDecelerate,

    /// <summary>Linear slide - constant speed.</summary>
    Linear,

    /// <summary>Quadratic speed-up slide.</summary>
    QuadraticAccelerate,

    /// <summary>Quadratic slow-down slide.</summary>
    QuadraticDecelerate
}

  /// <summary>
  /// Specifies the Internet Explorer visual transition to use in an animation.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used to describe animation transitions using IE Filters. This is a feature which is implemented
  /// only in the Internet Explorer browser, and these values will have no effect in other browsers.
  /// </para>
  /// </remarks>
  public enum TransitionType
  {
    /// <summary>No transition.</summary>
    None,         /* no transition */

    /// <summary>Custom transition manually specified by its string declaration.</summary>
    Custom,       /* uses string specified in *TransitionCustomFilter */

    /// <summary>Exposure of pixels in random order.</summary>
    Dissolve,     /* progid:DXImageTransform.Microsoft.RandomDissolve() */

    /// <summary>Fading from the old to the new content.</summary>
    Fade,         /* progid:DXImageTransform.Microsoft.Fade() */

    /// <summary>A circular camera aperture revealing outwards.</summary>
    IrisCircle,   /* progid:DXImageTransform.Microsoft.Iris(irisStyle=CIRCLE) */

    /// <summary>A circular camera aperture revealing inwards.</summary>
    IrisCircleIn, /* progid:DXImageTransform.Microsoft.Iris(irisStyle=CIRCLE,motion=in) */

    /// <summary>An X-shaped camera aperture revealing outwards.</summary>
    IrisCross,    /* progid:DXImageTransform.Microsoft.Iris(irisStyle=CROSS) */

    /// <summary>An X-shaped camera aperture revealing inwards.</summary>
    IrisCrossIn,  /* progid:DXImageTransform.Microsoft.Iris(irisStyle=CROSS,motion=in) */

    /// <summary>A diamond-shaped camera aperture revealing outwards.</summary>
    IrisDiamond,  /* progid:DXImageTransform.Microsoft.Iris(irisStyle=DIAMOND) */

    /// <summary>A diamond-shaped camera aperture revealing inwards.</summary>
    IrisDiamondIn,/* progid:DXImageTransform.Microsoft.Iris(irisStyle=DIAMOND,motion=in) */

    /// <summary>A plus sign-shaped camera aperture revealing outwards.</summary>
    IrisPlus,     /* progid:DXImageTransform.Microsoft.Iris(irisStyle=PLUS) */

    /// <summary>A plus sign-shaped camera aperture revealing inwards.</summary>
    IrisPlusIn,   /* progid:DXImageTransform.Microsoft.Iris(irisStyle=PLUS,motion=in) */

    /// <summary>A square camera aperture revealing outwards.</summary>
    IrisSquare,   /* progid:DXImageTransform.Microsoft.Iris(irisStyle=SQUARE) */

    /// <summary>A square camera aperture revealing inwards.</summary>
    IrisSquareIn, /* progid:DXImageTransform.Microsoft.Iris(irisStyle=SQUARE,motion=in) */

    /// <summary>A star-shaped camera aperture revealing outwards.</summary>
    IrisStar,     /* progid:DXImageTransform.Microsoft.Iris(irisStyle=STAR) */

    /// <summary>A star-shaped camera aperture revealing inwards.</summary>
    IrisStarIn,   /* progid:DXImageTransform.Microsoft.Iris(irisStyle=STAR,motion=in) */

    /// <summary>An animation of colored squares that take the average color value of the pixels they replace.</summary>
    Pixelate,     /* progid:DXImageTransform.Microsoft.Pixelate(MaxSquare=20) */

    /// <summary>Animates a rotating wipe transition from the old content to the new, like a propeller.</summary>
    Wheel2,       /* progid:DXImageTransform.Microsoft.Wheel(spokes=2) */

    /// <summary>Animates a rotating wipe transition from the old content to the new, like eight spokes of a wheel.</summary>
    Wheel8,       /* progid:DXImageTransform.Microsoft.Wheel(spokes=8) */

    /// <summary>Reveals new content by passing a gradient band over the old content in downward direction.</summary>
    WipeDown,     /* progid:DXImageTransform.Microsoft.Wipe(GradientSize=1.0,wipeStyle=1) */

    /// <summary>Reveals new content by passing a gradient band over the old content towards left.</summary>
    WipeLeft,     /* progid:DXImageTransform.Microsoft.Wipe(GradientSize=1.0,wipeStyle=0,motion=reverse) */

    /// <summary>Reveals new content by passing a gradient band over the old content towards right.</summary>
    WipeRight,    /* progid:DXImageTransform.Microsoft.Wipe(GradientSize=1.0,wipeStyle=0) */

    /// <summary>Reveals new content by passing a gradient band over the old content in upward direction.</summary>
    WipeUp        /* progid:DXImageTransform.Microsoft.Wipe(GradientSize=1.0,wipeStyle=1,motion=reverse) */
  }

  /// <summary>
  /// Describes the visual orientation of item groups.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration is used to describe whether groups of items are positioned vertically (one item on top of another) or
  /// horizontally (side by side).
  /// </para>
  /// </remarks>
  public enum GroupOrientation
  {
    /// <summary>Group is oriented vertically - like a column.</summary>
    Vertical,

    /// <summary>Group is oriented horizontally - like a row.</summary>
    Horizontal,

    /// <summary>Group flows like textual content.  Currently supported by <see cref="ToolBar"/> only.</summary>
    Flow
  }

  /// <summary>
  /// Specifies horizontal alignment of contents within their container.
  /// </summary>
  public enum TextAlign
  {
    /// <summary>Contents are aligned to the left.</summary>
    Left,
    
    /// <summary>Contents are aligned to the right.</summary>
    Right,

    /// <summary>Contents are centered.</summary>
    Center
  }

  /// <summary>
  /// Specifies whether the item is supposed to function as a checkbox.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration describes the whether an item behaves as a checkbox, and if so, the specific behavior it will
  /// display. The difference between <code>RadioButton</code> and <code>RadioButtonCheckBox</code> is subtle. <code>RadioButton</code> 
  /// will result in a group where one item, and no more, is always checked. This differs from the behavior of <code>RadioCheckBox</code>
  /// which results in a group where either one or zero items can be checked.
  /// </para>
  /// </remarks>
  public enum ItemToggleType
  {
    /// <summary>This is a regular item which won't act as a checkbox or a radio button.</summary>
    None,

    /// <summary>This item acts as a checkbox, and can be checked or unchecked.</summary>
    CheckBox,

    /// <summary>This item acts as a radio button in a group where exactly one item is checked at any time.</summary>
    RadioButton,

    /// <summary>This item acts as a radio button in a group where no more than one item is checked at any time.</summary>
    RadioCheckBox
  }

  /// <summary>
  /// Specifies when the icon is visible.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This enumeration describes the visibility of an item's icon. It is generally used in cases when the item's icon
  /// is used to provide feedback about the state of the item. For example, a checkbox icon can be used to indicate that an item is
  /// checked, or an arrow can be used to indicate that an item can be expanded.
  /// </para>
  /// </remarks>
  public enum ItemIconVisibility
  {
    /// <summary>Icon is always visible.</summary>
    Always,

    /// <summary>Icon is visible when the item is checked.</summary>
    WhenChecked,

    /// <summary>Icon is visible when the item has children.</summary>
    WhenExpandable
  }

  #endregion

}
