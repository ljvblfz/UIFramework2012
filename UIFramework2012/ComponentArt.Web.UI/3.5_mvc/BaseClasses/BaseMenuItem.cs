using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace ComponentArt.Web.UI
{
	/// <summary>
  /// Navigation node class for <see cref="BaseMenu"/>.
	/// </summary>
	public abstract class BaseMenuItem : NavigationNode
	{
		internal bool m_bLooksApplied = false;

    #region Public Properties

    #region Look Translators
    
		private ItemLook translator;

		private ItemLook GetTranslator()
		{
      if(translator == null)
      {
        translator = new ItemLook(true);
        translator.Item = this;
      }

      return translator;
		}
       
    #endregion

    /// <summary>
		/// The look to use for this item when a descendant is selected.
		/// </summary>
		/// <seealso cref="Look" />
		[Description("The look to use for this item when a descendant is selected.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemLook ChildSelectedLook
		{
      get
      {
        ItemLook lookTranslator = GetTranslator();
        lookTranslator.ForDefaultSubItem = false;
        lookTranslator.LookType = ItemLookType.ChildSelected;
        return lookTranslator;
      }
			set
			{
				if(value != null)
				{
					ItemLook lookTranslator = GetTranslator();
					lookTranslator.ForDefaultSubItem = false;
					lookTranslator.LookType = ItemLookType.ChildSelected;
					value.CopyTo(lookTranslator, !m_bLooksApplied);
				}
			}
		}

		/// <summary>
		/// The ID of the pre-defined look to use for this item when a descendant is selected.
		/// </summary>
		[Category("ItemLook")]
		[DefaultValue(null)]
		[Description("The ID of the pre-defined look to use for this item when a descendant is selected.")]
		public string ChildSelectedLookId
		{
			get 
			{
				return Properties[GetAttributeVarName("ChildSelectedLookId")]; 
			}
			set 
			{
				Properties[GetAttributeVarName("ChildSelectedLookId")] = value;
			}
		}

		/// <summary>
		/// Default CSS class to apply to sub-groups below this item, including this item's subgroup.
		/// </summary>
		[Category("Appearance")]
		[Description("Default CSS class to apply to sub-groups below this item, including this item's subgroup.")]
		public string DefaultSubGroupCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("DefaultSubGroupCssClass")];
				return (o != null) ? o :
					(this.ParentItem != null) ? this.ParentItem.DefaultSubGroupCssClass :
					this.ParentBaseMenu.DefaultGroupCssClass;
			}
			set 
			{
				Properties[GetAttributeVarName("DefaultSubGroupCssClass")] = value;
			}
		}

    #region DefaultSubItem...

    private ItemLook _defaultSubItemChildSelectedLook;
		/// <summary>
		/// The default look to use for sub-items when their descendant is selected.
		/// </summary>
    /// <seealso cref="Look" />
    [Description("The default look to use for sub-items when their descendant is selected.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ItemLook DefaultSubItemChildSelectedLook
		{
			get
			{
        if(_defaultSubItemChildSelectedLook != null)
        {
          return _defaultSubItemChildSelectedLook;
        }
        else
        {
          ItemLook lookTranslator = GetTranslator();
          lookTranslator.ForDefaultSubItem = true;
          lookTranslator.LookType = ItemLookType.ChildSelected;
          return lookTranslator;
        }
			}
			set
			{
				if(value != null)
				{
					ItemLook lookTranslator = GetTranslator();
					lookTranslator.ForDefaultSubItem = true;
					lookTranslator.LookType = ItemLookType.ChildSelected;
					value.CopyTo(lookTranslator, !m_bLooksApplied);
				}
			}
		}

		/// <summary>
		/// The ID of the pre-defined default look to use for sub-items when their descendant is selected.
		/// </summary>
		[Category("ItemLook")]
		[DefaultValue(null)]
		[Description("The ID of the pre-defined default look to use for sub-items when their descendant is selected.")]
		public string DefaultSubItemChildSelectedLookId
		{
			get 
			{
				return Properties[GetAttributeVarName("DefaultSubItemChildSelectedLookId")]; 
			}
			set 
			{
				Properties[GetAttributeVarName("DefaultSubItemChildSelectedLookId")] = value;
			}
		}

    private ItemLook _defaultSubItemDisabledLook;
		/// <summary>
		/// The default look to use for sub-items when they are disabled.
		/// </summary>
    /// <seealso cref="Look" />
    [Description("The default look to use for sub-items when they are disabled.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ItemLook DefaultSubItemDisabledLook
		{
			get
			{
        if(_defaultSubItemDisabledLook != null)
        {
          return _defaultSubItemDisabledLook;
        }
        else
        {
          ItemLook lookTranslator = GetTranslator();
          lookTranslator.ForDefaultSubItem = true;
          lookTranslator.LookType = ItemLookType.Disabled;
          return lookTranslator;
        }
			}
			set
			{
				if(value != null)
				{
					ItemLook lookTranslator = GetTranslator();
					lookTranslator.ForDefaultSubItem = true;
					lookTranslator.LookType = ItemLookType.Disabled;
					value.CopyTo(lookTranslator, !m_bLooksApplied);
				}
			}
		}

		/// <summary>
		/// The ID of the pre-defined default look to use for sub-items when they are disabled.
		/// </summary>
		[Category("ItemLook")]
		[DefaultValue(null)]
		[Description("The ID of the pre-defined default look to use for sub-items when they are disabled.")]
		public string DefaultSubItemDisabledLookId
		{
			get 
			{
				return Properties[GetAttributeVarName("DefaultSubItemDisabledLookId")]; 
         
			}
			set 
			{
				Properties[GetAttributeVarName("DefaultSubItemDisabledLookId")] = value;
			}
		}

    private ItemLook _defaultSubItemLook;
		/// <summary>
		/// The default look to use for sub-items.
		/// </summary>
    /// <seealso cref="Look" />
    [Description("The default look to use for sub-items.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ItemLook DefaultSubItemLook
		{
			get
			{
        if(_defaultSubItemLook != null)
        {
          return _defaultSubItemLook;
        }
        else
        {
          ItemLook lookTranslator = GetTranslator();
          lookTranslator.ForDefaultSubItem = true;
          lookTranslator.LookType = ItemLookType.Normal;
          return lookTranslator;
        }
			}
			set
			{
				if(value != null)
				{
					ItemLook lookTranslator = GetTranslator();
					lookTranslator.ForDefaultSubItem = true;
					lookTranslator.LookType = ItemLookType.Normal;
					value.CopyTo(lookTranslator, !m_bLooksApplied);
				}
			}
		}

		/// <summary>
		/// The ID of the pre-defined default look to use for sub-items.
		/// </summary>
		[Category("ItemLook")]
		[DefaultValue(null)]
		[Description("The ID of the pre-defined default look to use for sub-items.")]
    public string DefaultSubItemLookId
    {
      get 
      {
        return Properties[GetAttributeVarName("DefaultSubItemLookId")]; 
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubItemLookId")] = value;
      }
    }

    private ItemLook _defaultSubItemSelectedLook;
    /// <summary>
    /// The default look to use for sub-items when they are selected.
    /// </summary>
    /// <seealso cref="Look" />
    [Description("The default look to use for sub-items when they are selected.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ItemLook DefaultSubItemSelectedLook
    {
      get
      {
        if(_defaultSubItemSelectedLook != null)
        {
          return _defaultSubItemSelectedLook;
        }
        else
        {
          ItemLook lookTranslator = GetTranslator();
          lookTranslator.ForDefaultSubItem = true;
          lookTranslator.LookType = ItemLookType.Selected;
          return lookTranslator;
        }
      }
      set
      {
        if(value != null)
        {
          ItemLook lookTranslator = GetTranslator();
          lookTranslator.ForDefaultSubItem = true;
          lookTranslator.LookType = ItemLookType.Selected;
          value.CopyTo(lookTranslator, !m_bLooksApplied);
        }
      }
    }

    /// <summary>
    /// The ID of the default look to use for sub-items when they are selected.
    /// </summary>
		[Category("ItemLook")]
		[DefaultValue(null)]
		[Description("The ID of the default look to use for sub-items when they are selected.")]
    public string DefaultSubItemSelectedLookId
    {
      get 
      {
        return Properties[GetAttributeVarName("DefaultSubItemSelectedLookId")]; 
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubItemSelectedLookId")] = value;
      }
    }

    #endregion

    /// <summary>
    /// The default text alignment to apply to labels of sub-items.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(TextAlign.Left)]
    [Description("The default text alignment to apply to labels of sub-items.")]
    public TextAlign DefaultSubItemTextAlign
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubItemTextAlign")];
        return (o != null) ? Utils.ParseTextAlign(o) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubItemTextAlign :
          (this.ParentBaseMenu != null) ? this.ParentBaseMenu.DefaultItemTextAlign :
          TextAlign.Left;
      }
      set
      {
        Properties[GetAttributeVarName("DefaultSubItemTextAlign")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to wrap text in sub-item labels by default.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(false)]
    [Description("Whether to wrap text in sub-item labels by default.")]
    public bool DefaultSubItemTextWrap
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubItemTextWrap")];
        return (o != null) ? Utils.ParseBool(o, false) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubItemTextWrap :
          (this.ParentBaseMenu != null) ? this.ParentBaseMenu.DefaultItemTextWrap : false;
      }
      set
      {
        Properties[GetAttributeVarName("DefaultSubItemTextWrap")] = value.ToString();
      }
    }

    /// <summary>
    /// The look to use for this item when it is disabled.
    /// </summary>
    /// <seealso cref="Look" />
    [Description("The look to use for this item when it is disabled.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ItemLook DisabledLook
    {
      get
      {
        ItemLook lookTranslator = GetTranslator();
        lookTranslator.ForDefaultSubItem = false;
        lookTranslator.LookType = ItemLookType.Disabled;
        return lookTranslator;
      }
      set
      {
        if(value != null)
        {
          ItemLook lookTranslator = GetTranslator();
          lookTranslator.ForDefaultSubItem = false;
          lookTranslator.LookType = ItemLookType.Disabled;
          value.CopyTo(lookTranslator, !m_bLooksApplied);
        }
      }
    }

    /// <summary>
    /// The ID of the pre-defined look to use for this item when it is disabled.
    /// </summary>
		[Category("ItemLook")]
		[DefaultValue(null)]
		[Description("The ID of the pre-defined look to use for this item when it is disabled.")]
    public string DisabledLookId
    {
      get 
      {
        return Properties[GetAttributeVarName("DisabledLookId")]; 
      }
      set 
      {
        Properties[GetAttributeVarName("DisabledLookId")] = value;
      }
    }

    //private ItemLook _look;
    /// <summary>
    /// The look to use for this item.
    /// </summary>
    /// <remarks>
    /// Look subproperties can be set through XML or programmatically.
    /// </remarks>
    [Description("The look to use for this item.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ItemLook Look
    {
      get
      {
        ItemLook lookTranslator = GetTranslator();
        lookTranslator.ForDefaultSubItem = false;
        lookTranslator.LookType = ItemLookType.Normal;
        return lookTranslator;
      }
      set
      {
        if(value != null)
        {
          ItemLook lookTranslator = GetTranslator();
          lookTranslator.ForDefaultSubItem = false;
          lookTranslator.LookType = ItemLookType.Normal;
          value.CopyTo(lookTranslator, !m_bLooksApplied);
        }
      }
    }

    /// <summary>
    /// The ID of the pre-defined look to use for this item.
    /// </summary>
		[Category("ItemLook")]
		[DefaultValue(null)]
		[Description("The ID of the pre-defined look to use for this item.")]
    public string LookId
    {
      get 
      {
        return Properties[GetAttributeVarName("LookId")]; 
      }
      set 
      {
        Properties[GetAttributeVarName("LookId")] = value;
      }
    }

    internal BaseMenu ParentBaseMenu
    {
      get
      {
        return (BaseMenu)(this.navigator);
      }
    }

    /// <summary>
    /// The parent of this item.
    /// </summary>
    internal BaseMenuItem ParentItem
    {
      get
      {
        return (BaseMenuItem)(this.parentNode);
      }
    }

    //private ItemLook _selectedLook;
    /// <summary>
    /// The look to use for this item when it is selected.
    /// </summary>
    /// <seealso cref="Look" />
    [Description("The look to use for this item when it is selected.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ItemLook SelectedLook
    {
      get
      {
        ItemLook lookTranslator = GetTranslator();
        lookTranslator.ForDefaultSubItem = false;
        lookTranslator.LookType = ItemLookType.Selected;
        return lookTranslator;
      }
      set
      {
        if(value != null)
        {
          ItemLook lookTranslator = GetTranslator();
          lookTranslator.ForDefaultSubItem = false;
          lookTranslator.LookType = ItemLookType.Selected;
          value.CopyTo(lookTranslator, !m_bLooksApplied);
        }
      }
    }

    /// <summary>
    /// The ID of the pre-defined look to use for this item when it is selected.
    /// </summary>
		[Category("ItemLook")]
		[DefaultValue(null)]
		[Description("The ID of the pre-defined look to use for this item when it is selected.")]
    public string SelectedLookId
    {
      get 
      {
        return Properties[GetAttributeVarName("SelectedLookId")]; 
      }
      set 
      {
        Properties[GetAttributeVarName("SelectedLookId")] = value;
      }
    }

   
    /// <summary>
    /// The text alignment to apply to the label of this node.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(TextAlign.Left)]
    [Description("The text alignment to apply to the label of this node.")]
    public TextAlign TextAlign
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("TextAlign")];
        return (o != null) ? Utils.ParseTextAlign(o) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubItemTextAlign :
          (this.ParentBaseMenu != null) ? this.ParentBaseMenu.DefaultItemTextAlign :
          TextAlign.Left;
      }
      set
      {
        Properties[GetAttributeVarName("TextAlign")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to wrap text in this node's label.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(false)]
    [Description("Whether to wrap text in this node's label.")]
    public bool TextWrap
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("TextWrap")];
        return (o != null) ? Utils.ParseBool(o, false) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubItemTextWrap :
          (this.ParentBaseMenu != null) ? this.ParentBaseMenu.DefaultItemTextWrap : false;
      }
      set
      {
        Properties[GetAttributeVarName("TextWrap")] = value.ToString();
      }
    }

    #endregion

    #region Internal Properties

    internal ItemLook EffectiveLook
    {
      get
      {
        if (this.ParentBaseMenu.ForceHighlightedItemID == string.Empty && this.ParentBaseMenu.HighlightSelectedPath && this.IsSelected && !this.SelectedLook.IsEmpty)
          return this.SelectedLook;
        else if(this.ParentBaseMenu.ForceHighlightedItemID != string.Empty && this.ParentBaseMenu.HighlightSelectedPath && this.ParentBaseMenu.ForceHighlightedItemID == this.ID && !this.SelectedLook.IsEmpty)
          return this.SelectedLook;
        else if (!this.Enabled && !this.DisabledLook.IsEmpty)
          return this.DisabledLook;
        else if (this.ParentBaseMenu.ForceHighlightedItemID == string.Empty && this.ParentBaseMenu.HighlightSelectedPath && this.IsChildSelected && !this.ChildSelectedLook.IsEmpty)
          return this.ChildSelectedLook;
        else if (this.ParentBaseMenu.ForceHighlightedItemID != string.Empty && this.ParentBaseMenu.HighlightSelectedPath && this.IsChildForceHighlighted() && !this.ChildSelectedLook.IsEmpty)
          return this.ChildSelectedLook;
        else
          return this.Look;
      }
    }

    #endregion

    #region Methods

    internal override NavigationNode AddNode()
    {
      return null;
    }

    /// <summary>
    /// Apply looks to this item - if look IDs are specified, this will load the looks, otherwise
    /// individual look properties will be inherited from the control and the parent node.
    /// </summary>
    public void ApplyLooks()
    {
      // Look
      if(ParentBaseMenu != null)
      {
        if(this.LookId != null)
        {
          //_look = new ItemLook(false, true);
          //ParentBaseMenu.ItemLooks[this.LookId].CopyTo(_look);
          this.Look = ParentBaseMenu.ItemLooks[this.LookId];
        }
      
        // SelectedLook
        if(this.SelectedLookId != null)
        {
          //_selectedLook = new ItemLook(false, true);
          //ParentBaseMenu.ItemLooks[this.SelectedLookId].CopyTo(_selectedLook);
          this.SelectedLook = ParentBaseMenu.ItemLooks[this.SelectedLookId];
        }

        // ChildSelectedLook
        if(this.ChildSelectedLookId != null)
        {
          //_childSelectedLook = new ItemLook(false, true);
          //ParentBaseMenu.ItemLooks[this.ChildSelectedLookId].CopyTo(_childSelectedLook);
          this.ChildSelectedLook = ParentBaseMenu.ItemLooks[this.ChildSelectedLookId];
        }

        // DisabledLook
        if(this.DisabledLookId != null)
        {
          //_disabledLook = new ItemLook(false, true);
          //ParentBaseMenu.ItemLooks[this.DisabledLookId].CopyTo(_disabledLook);
          this.DisabledLook = ParentBaseMenu.ItemLooks[this.DisabledLookId];
        }
      }
      
      // inherit defaults...

      // DefaultSubItemLook
      if(this.DefaultSubItemLookId != null && ParentBaseMenu != null)
      {
        _defaultSubItemLook = new ItemLook(false, true);
        ParentBaseMenu.ItemLooks[this.DefaultSubItemLookId].CopyTo(_defaultSubItemLook, !m_bLooksApplied);
      }
      else if(this.ParentItem != null && !this.ParentItem.DefaultSubItemLook.IsEmpty)
      {
        _defaultSubItemLook = new ItemLook(false, true);
        this.ParentItem.DefaultSubItemLook.CopyTo(_defaultSubItemLook, !m_bLooksApplied);
      }
      else if(this.ParentBaseMenu != null && !this.ParentBaseMenu.DefaultItemLook.IsEmpty)
      {
        _defaultSubItemLook = new ItemLook(false, true);
        this.ParentBaseMenu.DefaultItemLook.CopyTo(_defaultSubItemLook, !m_bLooksApplied);
      }

      // DefaultSubItemSelectedLook
      if(this.DefaultSubItemSelectedLookId != null && ParentBaseMenu != null)
      {
        _defaultSubItemSelectedLook = new ItemLook(false, true);
        ParentBaseMenu.ItemLooks[this.DefaultSubItemSelectedLookId].CopyTo(_defaultSubItemSelectedLook, !m_bLooksApplied);
      }
      else if(this.ParentItem != null && !this.ParentItem.DefaultSubItemSelectedLook.IsEmpty)
      {
        _defaultSubItemSelectedLook = new ItemLook(false, true);
        this.ParentItem.DefaultSubItemSelectedLook.CopyTo(_defaultSubItemSelectedLook, !m_bLooksApplied);
      }
      else if(this.ParentBaseMenu != null && !this.ParentBaseMenu.DefaultSelectedItemLook.IsEmpty)
      {
        _defaultSubItemSelectedLook = new ItemLook(false, true);
        this.ParentBaseMenu.DefaultSelectedItemLook.CopyTo(_defaultSubItemSelectedLook, !m_bLooksApplied);
      }

      // DefaultSubItemChildSelectedLook
      if(this.DefaultSubItemChildSelectedLookId != null && ParentBaseMenu != null)
      {
        _defaultSubItemChildSelectedLook = new ItemLook(false, true);
        ParentBaseMenu.ItemLooks[this.DefaultSubItemChildSelectedLookId].CopyTo(_defaultSubItemChildSelectedLook, !m_bLooksApplied);
      }
      else if(this.ParentItem != null && !this.ParentItem.DefaultSubItemChildSelectedLook.IsEmpty)
      {
        _defaultSubItemChildSelectedLook = new ItemLook(false, true);
        this.ParentItem.DefaultSubItemChildSelectedLook.CopyTo(_defaultSubItemChildSelectedLook, !m_bLooksApplied);
      }
      else if(this.ParentBaseMenu != null && !this.ParentBaseMenu.DefaultChildSelectedItemLook.IsEmpty)
      {
        _defaultSubItemChildSelectedLook = new ItemLook(false, true);
        this.ParentBaseMenu.DefaultChildSelectedItemLook.CopyTo(_defaultSubItemChildSelectedLook, !m_bLooksApplied);
      }

      // DefaultSubItemDisabledLook
      if(this.DefaultSubItemDisabledLookId != null && ParentBaseMenu != null)
      {
        _defaultSubItemDisabledLook = new ItemLook(false, true);
        ParentBaseMenu.ItemLooks[this.DefaultSubItemDisabledLookId].CopyTo(_defaultSubItemDisabledLook, !m_bLooksApplied);
      }
      else if(this.ParentItem != null && !this.ParentItem.DefaultSubItemDisabledLook.IsEmpty)
      {
        _defaultSubItemDisabledLook = new ItemLook(false, true);
        this.ParentItem.DefaultSubItemDisabledLook.CopyTo(_defaultSubItemDisabledLook, !m_bLooksApplied);
      }
      else if(this.ParentBaseMenu != null && !this.ParentBaseMenu.DefaultDisabledItemLook.IsEmpty)
      {
        _defaultSubItemDisabledLook = new ItemLook(false, true);
        this.ParentBaseMenu.DefaultDisabledItemLook.CopyTo(_defaultSubItemDisabledLook, !m_bLooksApplied);
      }
      
      // Resolve inherited values - we only have to go one level up
      // BUT - only do this if we didn't have a specific LookId specified for the situation
      if(this.ParentItem != null)
      {
        // inherit from parentitem...
        if((this.LookId == null || this.LookId == string.Empty) && !ParentItem.DefaultSubItemLook.IsEmpty) ParentItem.DefaultSubItemLook.CopyTo(Look, !m_bLooksApplied, true);
        if((this.ChildSelectedLookId == null || this.ChildSelectedLookId == string.Empty) && !ParentItem.DefaultSubItemChildSelectedLook.IsEmpty) ParentItem.DefaultSubItemChildSelectedLook.CopyTo(ChildSelectedLook, !m_bLooksApplied, true);
        if((this.SelectedLookId == null || this.SelectedLookId == string.Empty) && !ParentItem.DefaultSubItemSelectedLook.IsEmpty) ParentItem.DefaultSubItemSelectedLook.CopyTo(SelectedLook, !m_bLooksApplied, true);
        if((this.DisabledLookId == null || this.DisabledLookId == string.Empty) && !ParentItem.DefaultSubItemDisabledLook.IsEmpty) ParentItem.DefaultSubItemDisabledLook.CopyTo(DisabledLook, !m_bLooksApplied, true);
      }
      else if(this.ParentBaseMenu != null)
      {
        // inherit from parentbasemenu.default...
        if((this.LookId == null || this.LookId == string.Empty) && !ParentBaseMenu.DefaultItemLook.IsEmpty) ParentBaseMenu.DefaultItemLook.CopyTo(Look, !m_bLooksApplied);
        if((this.ChildSelectedLookId == null || this.ChildSelectedLookId == string.Empty) && !ParentBaseMenu.DefaultChildSelectedItemLook.IsEmpty) ParentBaseMenu.DefaultChildSelectedItemLook.CopyTo(ChildSelectedLook, !m_bLooksApplied, true);
        if((this.SelectedLookId == null || this.SelectedLookId == string.Empty) && !ParentBaseMenu.DefaultSelectedItemLook.IsEmpty) ParentBaseMenu.DefaultSelectedItemLook.CopyTo(SelectedLook, !m_bLooksApplied, true);
        if((this.DisabledLookId == null || this.DisabledLookId == string.Empty) && !ParentBaseMenu.DefaultDisabledItemLook.IsEmpty) ParentBaseMenu.DefaultDisabledItemLook.CopyTo(DisabledLook, !m_bLooksApplied, true);
      }

      if(this.nodes != null && this.nodes.Count > 0)
      {
        foreach(BaseMenuItem oItem in this.nodes)
        {
          oItem.navigator = this.navigator;
          oItem.parentNode = this;
          oItem.ApplyLooks();
        }
      }

      // clear cached copies.
      _defaultSubItemChildSelectedLook = null;
      _defaultSubItemDisabledLook = null;
      _defaultSubItemLook = null;
      _defaultSubItemSelectedLook = null;

      m_bLooksApplied = true;
    }

    internal bool IsChildForceHighlighted()
    {
      // If we don't have this pointer yet, forget it.
      if(this.ParentBaseMenu == null || this.ParentBaseMenu.ForceHighlightedItemID == string.Empty)
      {
        return false;
      }

      if(this.ParentBaseMenu.m_oForceHighlightedItem == null)
      {
        this.ParentBaseMenu.m_oForceHighlightedItem = this.ParentBaseMenu.FindItemById(this.ParentBaseMenu.ForceHighlightedItemID);
        if(this.ParentBaseMenu.m_oForceHighlightedItem == null)
        {
          return false;
        }
      }

      BaseMenuItem oNode = this.ParentBaseMenu.m_oForceHighlightedItem.ParentItem;

      while(oNode != null)
      {
        if(oNode == this)
        {
          return true;
        }

        oNode = oNode.ParentItem;
      }

      return false;
    }


    #endregion
	}
}
