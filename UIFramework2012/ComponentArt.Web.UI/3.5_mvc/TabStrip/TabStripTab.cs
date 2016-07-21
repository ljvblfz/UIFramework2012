using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Navigation node class for the <see cref="TabStrip"/> control.
  /// </summary>
  [ToolboxItem(false)]
  [ParseChildren(true, "Tabs")]
  public class TabStripTab : BaseMenuItem
  {
    #region Public Properties


    /// <summary>
    /// How tabs in subgroups are aligned.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(TabStripAlign.Left)]
    [Description("How tabs in subgroups are aligned.")]
    public TabStripAlign DefaultSubGroupAlign
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupAlign")];
        return (o != null) ? Utils.ParseTabStripAlign(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupAlign :
          this.ParentTabStrip.DefaultGroupAlign;
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubGroupAlign")] = value.ToString();
      }
    }

    /// <summary>
    /// Direction in which the subgroups expand.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(GroupExpandDirection.Auto)]
    [Description("Direction in which the subgroups expand.")]
    public GroupExpandDirection DefaultSubGroupExpandDirection
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupExpandDirection")];
        return (o != null) ? Utils.ParseGroupExpandDirection(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupExpandDirection :
          this.ParentTabStrip.DefaultGroupExpandDirection;
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubGroupExpandDirection")] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along x-axis from subgroups' normal expand positions.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along x-axis from subgroups' normal expand positions.")]
    public int DefaultSubGroupExpandOffsetX
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupExpandOffsetX")];
        return (o != null) ? Utils.ParseInt(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupExpandOffsetX :
          this.ParentTabStrip.DefaultGroupExpandOffsetX;
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubGroupExpandOffsetX")] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along y-axis from subgroups' normal expand positions.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along y-axis from subgroups' normal expand positions.")]
    public int DefaultSubGroupExpandOffsetY
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupExpandOffsetY")];
        return (o != null) ? Utils.ParseInt(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupExpandOffsetY :
          this.ParentTabStrip.DefaultGroupExpandOffsetY;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupExpandOffsetY")] = value.ToString();
      }
    }

    /// <summary>
    /// Default first separator height.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default first separator height.  Default is Unit.Empty.")]
    public Unit DefaultSubGroupFirstSeparatorHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupFirstSeparatorHeight")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupFirstSeparatorHeight :
          this.ParentTabStrip.DefaultGroupFirstSeparatorHeight;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DefaultSubGroupFirstSeparatorHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Default first separator width.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default first separator width.  Default is Unit.Empty.")]
    public Unit DefaultSubGroupFirstSeparatorWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupFirstSeparatorWidth")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupFirstSeparatorWidth :
          this.ParentTabStrip.DefaultGroupFirstSeparatorWidth;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DefaultSubGroupFirstSeparatorWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Whether to expand the subgroups so they fill exactly the dimensions of the TabStrip.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(true)]
    [Description("Whether to expand the subgroups so they fill exactly the dimensions of the TabStrip.")]
    public bool DefaultSubGroupFullExpand
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupFullExpand")];
        return (o != null) ? Utils.ParseBool(o, true) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupFullExpand :
          this.ParentTabStrip.DefaultGroupFullExpand;
      }
      set
      {
        Properties[GetAttributeVarName("DefaultSubGroupFullExpand")] = value.ToString();
      }
    }

    /// <summary>
    /// Height of subgroups.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Height of subgroups.")]
    public Unit DefaultSubGroupHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupHeight")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupHeight :
          this.ParentTabStrip.DefaultGroupHeight;
      }
      set
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("DefaultSubGroupHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Default last separator height.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default last separator height.  Default is Unit.Empty.")]
    public Unit DefaultSubGroupLastSeparatorHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupLastSeparatorHeight")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupLastSeparatorHeight :
          this.ParentTabStrip.DefaultGroupLastSeparatorHeight;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DefaultSubGroupLastSeparatorHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Default last separator width.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default last separator width.  Default is Unit.Empty.")]
    public Unit DefaultSubGroupLastSeparatorWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupLastSeparatorWidth")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupLastSeparatorWidth :
          this.ParentTabStrip.DefaultGroupLastSeparatorWidth;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DefaultSubGroupLastSeparatorWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Default  separator height.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default  separator height.  Default is Unit.Empty.")]
    public Unit DefaultSubGroupSeparatorHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupSeparatorHeight")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupSeparatorHeight :
          this.ParentTabStrip.DefaultGroupSeparatorHeight;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DefaultSubGroupSeparatorHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Folder with default group separator images.
    /// </summary>
    [Description("Folder with default group separator images.")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string DefaultSubGroupSeparatorImagesFolderUrl
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupSeparatorImagesFolderUrl")];
        return (o != null) ? o :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupSeparatorImagesFolderUrl :
          this.ParentTabStrip.DefaultGroupSeparatorImagesFolderUrl;
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubGroupSeparatorImagesFolderUrl")] = value;
      }
    }

    /// <summary>
    /// Default  separator width.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Default  separator width.  Default is Unit.Empty.")]
    public Unit DefaultSubGroupSeparatorWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupSeparatorWidth")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupSeparatorWidth :
          this.ParentTabStrip.DefaultGroupSeparatorWidth;
      }
      set
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DefaultSubGroupSeparatorWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Whether to show separator images for the subgroups.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(false)]
    [Description("Whether to show separator images for the subgroups.")]
    public bool DefaultSubGroupShowSeparators
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupShowSeparators")];
        return (o != null) ? Utils.ParseBool(o, false) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupShowSeparators :
          this.ParentTabStrip.DefaultGroupShowSeparators;
      }
      set
      {
        Properties[GetAttributeVarName("DefaultSubGroupShowSeparators")] = value.ToString();
      }
    }

    /// <summary>
    /// Spacing between subgroups' tabs.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Spacing between subgroups' tabs.")]
    public Unit DefaultSubGroupTabSpacing
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupTabSpacing")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupTabSpacing :
          this.ParentTabStrip.DefaultGroupTabSpacing;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DefaultSubGroupTabSpacing")] = value.ToString();
        }
        else
        {
          throw new Exception("Group tab spacing may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Width of subgroups.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Width of subgroups.")]
    public Unit DefaultSubGroupWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupWidth")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupWidth :
          this.ParentTabStrip.DefaultGroupWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("DefaultSubGroupWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Tab's height.
    /// </summary>
    [Category("Layout")]
    [Description("Tab's height.")]
    public override Unit Height
    {
      get 
      {
        return Unit.Parse(this.Properties[GetAttributeVarName("Height")]); 
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("Height")] = value.ToString();
        }
        else
        {
          throw new Exception("Tab dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Collection of immediate child TabStripTabs.
    /// </summary>
    [Description("Collection of immediate child TabStripTabs.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    [Browsable(false)]
    public TabStripTabCollection Tabs
    {
      get
      {
        if (this.nodes == null)
        {
          nodes = new TabStripTabCollection(ParentTabStrip, this);
        }
        return (TabStripTabCollection)nodes;
      }
    }

    /// <summary>
    /// The TabStrip that this tab belongs to.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TabStrip ParentTabStrip
    {
      get
      {
        return (TabStrip)this.navigator;
      }
    }

    /// <summary>
    /// This tab's parent tab.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TabStripTab ParentTab
    {
      get
      {
        return (TabStripTab) this.ParentItem;
      }
    }

    /// <summary>
    /// How tabs in the subgroup are aligned.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(TabStripAlign.Left)]
    [Description("How tabs in the subgroup are aligned.")]
    public TabStripAlign SubGroupAlign
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupAlign")];
        return (o != null) ? Utils.ParseTabStripAlign(o) : this.DefaultSubGroupAlign;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupAlign")] = value.ToString();
      }
    }

    /// <summary>
    /// Subgroup's CSS class.
    /// </summary>
    [Category("Appearance")]
    [Description("Subgroup's CSS class.")]
    public string SubGroupCssClass
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupCssClass")];
        return (o != null) ? o : this.DefaultSubGroupCssClass;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupCssClass")] = value;
      }
    }

    /// <summary>
    /// Direction in which the subgroup expands.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(GroupExpandDirection.Auto)]
    [Description("Direction in which the subgroup expands.")]
    public GroupExpandDirection SubGroupExpandDirection
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupExpandDirection")];
        return (o != null) ? Utils.ParseGroupExpandDirection(o) : this.DefaultSubGroupExpandDirection;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupExpandDirection")] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along x-axis from subgroup's normal expand position.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along x-axis from subgroup's normal expand position.")]
    public int SubGroupExpandOffsetX
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupExpandOffsetX")];
        return (o != null) ? Utils.ParseInt(o) : this.DefaultSubGroupExpandOffsetX;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupExpandOffsetX")] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along y-axis from subgroup's normal expand position.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(0)]
    [Description("Offset along y-axis from subgroup's normal expand position.")]
    public int SubGroupExpandOffsetY
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupExpandOffsetY")];
        return (o != null) ? Utils.ParseInt(o) : this.DefaultSubGroupExpandOffsetY;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupExpandOffsetY")] = value.ToString();
      }
    }

    /// <summary>
    /// Height of the subgroup's first separator.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Height of the subgroup's first separator.  Default is Unit.Empty.")]
    public Unit SubGroupFirstSeparatorHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupFirstSeparatorHeight")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupFirstSeparatorHeight;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("SubGroupFirstSeparatorHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Width of the subgroup's first separator.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Width of the subgroup's first separator.  Default is Unit.Empty.")]
    public Unit SubGroupFirstSeparatorWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupFirstSeparatorWidth")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupFirstSeparatorWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("SubGroupFirstSeparatorWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Whether to expand the immediate subgroup so it fills exactly the dimensions of the TabStrip.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(true)]
    [Description("Whether to expand the immediate subgroup so it fills exactly the dimensions of the TabStrip.")]
    public bool SubGroupFullExpand
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("SubGroupFullExpand")];
        return (o != null) ? Utils.ParseBool(o, true) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupFullExpand :
          this.ParentTabStrip.DefaultGroupFullExpand;
      }
      set
      {
        Properties[GetAttributeVarName("SubGroupFullExpand")] = value.ToString();
      }
    }

    /// <summary>
    /// Height of subgroup.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Height of subgroup.")]
    public Unit SubGroupHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupHeight")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupHeight;
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("SubGroupHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Height of the subgroup's last separator.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Height of the subgroup's last separator.  Default is Unit.Empty.")]
    public Unit SubGroupLastSeparatorHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupLastSeparatorHeight")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupLastSeparatorHeight;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("SubGroupLastSeparatorHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Width of the subgroup's last separator.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Width of the subgroup's last separator.  Default is Unit.Empty.")]
    public Unit SubGroupLastSeparatorWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupLastSeparatorWidth")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupLastSeparatorWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("SubGroupLastSeparatorWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Height of the subgroup's separators.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Height of the subgroup's separators.  Default is Unit.Empty.")]
    public Unit SubGroupSeparatorHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupSeparatorHeight")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupSeparatorHeight;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("SubGroupSeparatorHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Folder with the subgroup's separator images.
    /// </summary>
    [Description("Folder with the subgroup's separator images.")]
    [DefaultValue(null)]
    [Category("Appearance")]
    public string SubGroupSeparatorImagesFolderUrl
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupSeparatorImagesFolderUrl")];
        return (o != null) ? o : this.DefaultSubGroupSeparatorImagesFolderUrl;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupSeparatorImagesFolderUrl")] = value;
      }
    }

    /// <summary>
    /// Width of the subgroup's separators.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Width of the subgroup's separators.  Default is Unit.Empty.")]
    public Unit SubGroupSeparatorWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupSeparatorWidth")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupSeparatorWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("SubGroupSeparatorWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Separator dimensions may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Whether to show separator images for the immediate subgroup.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(false)]
    [Description("Whether to show separator images for the immediate subgroup.")]
    public bool SubGroupShowSeparators
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("SubGroupShowSeparators")];
        return (o != null) ? Utils.ParseBool(o, false) :
          (this.ParentTab != null) ? this.ParentTab.DefaultSubGroupShowSeparators :
          this.ParentTabStrip.DefaultGroupShowSeparators;
      }
      set
      {
        Properties[GetAttributeVarName("SubGroupShowSeparators")] = value.ToString();
      }
    }

    /// <summary>
    /// Spacing between subgroup's tabs.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Spacing between subgroup's tabs.")]
    public Unit SubGroupTabSpacing
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupTabSpacing")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupTabSpacing;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("SubGroupTabSpacing")] = value.ToString();
        }
        else
        {
          throw new Exception("Group tab spacing may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Width of subgroup.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
    [Description("Width of subgroup.")]
    public Unit SubGroupWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupWidth")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("SubGroupWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Tab's width.
    /// </summary>
    [Category("Layout")]
    [Description("Tab's width.")]
    public override Unit Width
    {
      get 
      {
        return Unit.Parse(this.Properties[GetAttributeVarName("Width")]); 
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("Width")] = value.ToString();
        }
        else
        {
          throw new Exception("Tab dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    #endregion

    #region Protected Properties
    #endregion

    #region Internal Properties

    /// <summary>
    /// Creates a new TabStripTab and adds it to this one's subgroup.
    /// </summary>
    /// <returns>The newly created child tab.</returns>
    internal override NavigationNode AddNode()
    {
      TabStripTab newTab = new TabStripTab();
      this.Tabs.Add(newTab);
      return newTab;
    }

    #endregion

    #region Private Properties
    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Internal Methods
    #endregion

    #region Private Methods
    #endregion
  }
}
