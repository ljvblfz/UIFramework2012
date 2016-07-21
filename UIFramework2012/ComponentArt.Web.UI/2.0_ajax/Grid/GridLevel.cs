using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Web.UI;


namespace ComponentArt.Web.UI
{
  #region Conditional Format Classes

  /// <summary>
  /// Defines conditional formatting based on client filters for the <see cref="Grid"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class GridConditionalFormat 
  {
    private string _filter;
    /// <summary>
    /// Client filter to use.
    /// </summary>
    public string ClientFilter
    {
      get
      {
        return _filter == null? "" : _filter;
      } 
      set
      {
        _filter = value;
      }
    } 

    private string _hoverRowClass;
    /// <summary>
    /// HoverRowCssClass to use.
    /// </summary>
    public string HoverRowCssClass
    {
      get
      {
        return _hoverRowClass == null? "" : _hoverRowClass;
      } 
      set
      {
        _hoverRowClass = value;
      }
    } 

    private string _rowClass;
    /// <summary>
    /// RowCssClass to use.
    /// </summary>
    public string RowCssClass
    {
      get
      {
        return _rowClass == null? "" : _rowClass;
      } 
      set
      {
        _rowClass = value;
      }
    }

    private string _selectedHoverRowClass;
    /// <summary>
    /// SelectedHoverRowCssClass to use.
    /// </summary>
    public string SelectedHoverRowCssClass
    {
      get
      {
        return _selectedHoverRowClass == null? "" : _selectedHoverRowClass;
      } 
      set
      {
        _selectedHoverRowClass = value;
      }
    } 

    private string _selectedRowClass;
    /// <summary>
    /// SelectedRowCssClass to use.
    /// </summary>
    public string SelectedRowCssClass
    {
      get
      {
        return _selectedRowClass == null? "" : _selectedRowClass;
      } 
      set
      {
        _selectedRowClass = value;
      }
    }
  }

  /// <summary>
  /// Collection of <see cref="GridConditionalFormat"/> objects.
  /// </summary>
  [ToolboxItem(false)]
  [Editor("System.Windows.Forms.Design.CollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
  public class GridConditionalFormatCollection : ICollection, IEnumerable, IList
  {
    private ArrayList _formats;

    public GridConditionalFormatCollection()
    {
      _formats = new ArrayList();
    }

    object IList.this[int index] 
    {
      get 
      {
        return _formats[index];
      }
      set 
      {
        _formats[index] = (GridConditionalFormat)value;
      }
    }

    /// <summary>
    /// The number of items in this collection.
    /// </summary>
    public int Count
    {
      get
      {
        return _formats.Count;
      }
    }

    public bool IsSynchronized
    {
      get
      {
        return true;
      }
    }

    public object SyncRoot
    {
      get
      {
        return _formats.SyncRoot;
      }
    }

    public void CopyTo (Array ar, int index)
    {
    }

    public virtual GridConditionalFormat this[int index]
    {
      get
      {
        return (GridConditionalFormat) _formats[index];
      }
      set
      {
        _formats[index] = value;
      }
    }

    /// <summary>
    /// Removes the given item from the collection.
    /// </summary>
    /// <param name="item"></param>
    public void Remove (object item)
    {
      _formats.Remove (item);
    }
		
    /// <summary>
    /// Inserts an item into this collection at the given index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    public void Insert (int index, object item)
    {
      _formats[index] = item;
    }

    /// <summary>
    /// Adds an item to this collection.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Add (object item)
    {
      return _formats.Add(item);
    }

    /// <summary>
    /// Removes all items from this collection.
    /// </summary>
    public void Clear()
    {
      _formats.Clear();
    }

    /// <summary>
    /// Returns whether this collection contains the given item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(object item)
    {
      return _formats.Contains(item);
    }

    /// <summary>
    /// Returns the index of the given item in this collection.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int IndexOf (object item)
    {
      return _formats.IndexOf(item);
    }

    public bool IsFixedSize
    {
      get
      {
        return false;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public void RemoveAt(int index)
    {
      _formats.RemoveAt(index);
    }

    public virtual IEnumerator GetEnumerator()
    {
      return _formats.GetEnumerator();
    }
  }

  #endregion

  /// <summary>
  /// Represents a level (table) of data in a <see cref="Grid"/> control.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The GridLevel class is responsible for the presentation and structure of the rendered data. It is used to define
  /// the <see cref="Columns" /> which will be displayed, as well as the styles for a particular table of data.
  /// A hierarchical grid will define multiple GridLevel objects, allowing each successive table of data to be presented
  /// in a different way. 
  /// </para>
  /// <para>
  /// All of the GridLevel objects defined for a Grid instance are contained within its <see cref="Grid.Levels">Levels</see> collection.
  /// Each <see cref="GridItem" /> has a <see cref="GridItem.Level">Level</see> property containing the index, within the Levels collection,
  /// of the GridLevel object which is associated with the item.
  /// </para>
  /// <para>
  /// The minimum required configuration for a GridLevel definition depends on the underlying datasource to which the grid will be bound.
  /// For example, a DataSet already contains structural information, meaning that if it is used as a datasource, the GridLevel
  /// does not need to explicitly define the columns. If the datasource is an IList, however, the Columns must be defined explicitly, 
  /// and must correspond to the properties of the objects contained within the collection.
  /// </para>
  /// <para> 
  /// At least one of the defined <see cref="GridColumn">GridColumns</see> must contain unique values for each item. 
  /// This column can be set as the <see cref="DataKeyField" />, a property which is required for the Grid to 
  /// function properly.
  /// </para>
  /// <para>
  /// While the level is responsible for the structure and appearance, the actual data for the Grid is contained within its 
  /// <see cref="Grid.Items">Items</see> collection. Each <see cref="GridItem" /> also contains
  /// an <see cref="GridItem.Items">Items</see> collection, allowing for the presentation of hierarchical data.
  /// </para>
  /// </remarks>
	public sealed class GridLevel
	{
    private Hashtable Properties;

    #region Internal Properties

    internal string FilterExpression
    {
      get
      {
        string s = (string)Properties["FilterExpression"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["FilterExpression"] = value;
      }
    }

    internal string TableName
    {
      get
      {
        string s = (string)Properties["TableName"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["TableName"] = value;
      }
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Whether to allow grouping on this level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Grouping can also be enabled/disabled for individual columns using the <see cref="GridColumn.AllowGrouping" /> property.
    /// When grouping is enabled, it is important to consider both the <see cref="Grid.RunningMode" /> of the current <see cref="Grid" />,
    /// and the <see cref="Grid.GroupingMode" />, as both of those properties determine how the grouping functionality will be implemented.
    /// </para>
    /// <para>
    /// See the <a href="ms-its:ComponentArt.Web.UI.chm::/Grid_Grouping.htm">Grouping with Grid</a> and <a href="ms-its:ComponentArt.Web.UI.chm::/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorials for more information
    /// on grouping.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    public bool AllowGrouping
    {
      get
      {
        string s = (string)Properties["AllowGrouping"];
        return s == null ? true : bool.Parse(s);
      }
      set
      {
        Properties["AllowGrouping"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to allow column reordering on this level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default, columns can be dragged to a new position by the user. This property can be set to false to prevent that behavior.
    /// If this property is true, the <see cref="Grid.ColumnReorder" /> event will fire on the server when the column order has changed.
    /// Specific columns can also set this behavior using the <see cref="GridColumn" /> <see cref="GridColumn.AllowReordering" /> property.
    /// </para>
    /// <para>
    /// The <see cref="ColumnReorderIndicatorImageUrl" /> property allows an image to be referenced which will visually indicate
    /// the new position of the column when it is being dragged.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    public bool AllowReordering
    {
      get
      {
        string s = (string)Properties["AllowReordering"];
        return s == null ? true : bool.Parse(s);
      }
      set
      {
        Properties["AllowReordering"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to allow sorting on this level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default behavior of the <see cref="Grid" /> is to allow sorting based on any column. This property applies to the entire
    /// level, but sorting can be enabled/disabled for specific columns, as well, using the 
    /// <see cref="GridColumn" /> <see cref="GridColumn.AllowSorting" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    public bool AllowSorting
    {
      get
      {
        string s = (string)Properties["AllowSorting"];
        return s == null ? true : bool.Parse(s);
      }
      set
      {
        Properties["AllowSorting"] = value.ToString();
      }
    }

    /// <summary>
    /// The CssClass to use on alternating rows.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The main property used for styling rendered <see cref="GridItem">GridItems</see> is <see cref="RowCssClass" />. 
    /// If this property is also set, the two styles will alternate with this class applied to every second row.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string AlternatingRowCssClass
    {
      get
      {
        string s = (string)Properties["AlternatingRowCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["AlternatingRowCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to use on alternating rows, on hover.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Hover styles are applied both when the mouse is hovering over the row, and when it is highlighted using keyboard navigation.
    /// The main property which is used for styling rows which are being hovered over is <see cref="HoverRowCssClass" />.
    /// If this property is also set, the two styles will alternate with this class applied to every second row.
    /// </para>
    /// <para>
    /// The <see cref="RowCssClass" /> and <see cref="AlternatingRowCssClass" /> properties are used to style rows
    /// which are not under any special conditions. 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string AlternatingHoverRowCssClass
    {
      get
      {
        string s = (string)Properties["AlternatingHoverRowCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["AlternatingHoverRowCssClass"] = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public string ColumnDisplayOrder
    {
      get
      {
        string s = (string)Properties["ColumnDisplayOrder"];

        if (s == null)
        {
          // generate default order
          ArrayList defaultOrder = new ArrayList();
          for (int iCol = 0; iCol < this.Columns.Count; iCol++)
          {
            if (this.Columns[iCol].Visible)
            {
              defaultOrder.Add(iCol.ToString());
            }
          }

          s = string.Join(",", (string[])defaultOrder.ToArray(typeof(string)));
        }

        return s;
      }
      set
      {
        Properties["ColumnDisplayOrder"] = value;
      }
    }

    /// <summary>
    /// The indicator image to use while dragging columns to the header to group.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The image specified by this property will appear in the header, in the 'drop-to-group' panel when the user drags a column over it. 
    /// If the Grid is not currently grouped, this image will be displayed at the left-hand side of the panel. If the grid is grouped, 
    /// the image will be displayed beside any column names which the Grid is currently grouped by.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string ColumnGroupIndicatorImageUrl
    {
      get
      {
        string s = (string)Properties["ColumnGroupIndicatorImageUrl"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ColumnGroupIndicatorImageUrl"] = value;
      }
    }

    /// <summary>
    /// The indicator image to use while dragging to reorder columns.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The image specified by this property will be displayed between existing columns, demonstrating the position that a
    /// dragged column will occupy when it is dropped.
    /// </para>
    /// <para>
    /// By default, columns can be reordered by the user. To change that behavior for all columns on a <see cref="GridLevel" />,
    /// the <see cref="GridLevel.AllowReordering" /> property can be used. The behavior can also be set for individual 
    /// <see cref="GridColumn">GridColumns</see> using the <see cref="GridColumn.AllowReordering" /> property.
    /// </para>
    /// <para>
    /// The current order of the columns can always be accessed through the <see cref="GridLevel.ColumnDisplayOrder" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string ColumnReorderIndicatorImageUrl
    {
      get
      {
        string s = (string)Properties["ColumnReorderIndicatorImageUrl"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ColumnReorderIndicatorImageUrl"] = value;
      }
    }

    private GridColumnCollection _columns;
    /// <summary>
    /// The collection of <see cref="GridColumn">GridColumns</see> for this level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Columns can be accessed by index directly through this property. 
    /// </para>
    /// </remarks>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public GridColumnCollection Columns
    {
      get
      {
        if(_columns == null)
        {
          _columns = new GridColumnCollection();
        }

        return _columns;
      }
    }

    private GridConditionalFormatCollection _conditionalFormats;
    /// <summary>
    /// Conditional formats. Provides the ability to assign custom styles to rows that match a particular client filter.
    /// </summary>
    [Category("Appearance")]
    [Description("Conditional formats. Provides the ability to assign custom styles to rows that match a particular client filter.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [NotifyParentProperty(true)]
    public GridConditionalFormatCollection ConditionalFormats
    {
      get
      {
        if(_conditionalFormats == null)
        {
          _conditionalFormats = new GridConditionalFormatCollection();
        }
        return _conditionalFormats;
      }
    }

    /// <summary>
    /// The CSS class to use on data cells on this level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The value set by this property can be overridden for individual <see cref="GridColumn">GridColumns</see> using the 
    /// <see cref="GridColumn.DataCellCssClass" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string DataCellCssClass
    {
      get
      {
        string s = (string)Properties["DataCellCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["DataCellCssClass"] = value;
      }
    }

    /// <summary>
    /// The name of the field which contains unique keys for items on this level.
    /// </summary>
    [DefaultValue("")]
    public string DataKeyField
    {
      get
      {
        string s = (string)Properties["DataKeyField"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["DataKeyField"] = value;
      }
    }
 
    /// <summary>
    /// The name of the data member (ie. table) in the DataSource which corresponds to this level.
    /// </summary>
    [DefaultValue("")]
    public string DataMember
    {
      get
      {
        string s = (string)Properties["DataMember"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["DataMember"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use on cells when they are in edit mode.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The value of this property can be overridden for individual <see cref="GridColumn">GridColumns</see> using the 
    /// <see cref="GridColumn.EditCellCssClass" /> property.
    /// </para>
    /// <para>
    /// See also the <see cref="EditFieldCssClass" /> property which is responsible for styling the actual edit field
    /// which is contained within the cell. 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string EditCellCssClass
    {
      get
      {
        string s = (string)Properties["EditCellCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["EditCellCssClass"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the EditCommand column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The template specified by this property is used to provide commands which are available when an item is being edited. The
    /// template will be used for the column which has its <see cref="GridColumn.EditControlType" /> set to EditCommand. If a column
    /// has its <see cref="GridColumn.DataCellClientTemplateId" /> property set, that client template will be overridden by the one
    /// specified by this property, when an item enters edit mode.
    /// </para>
    /// <para>
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see> and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates in Grid</see> tutorials
    /// for more information on client templates. 
    /// </para>
    /// </remarks>
    /// <seealso cref="GridColumn.EditControlType" />
    [DefaultValue("")]
    public string EditCommandClientTemplateId
    {
      get
      {
        string s = (string)Properties["EditCommandClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["EditCommandClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to edit fields.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the CSS class to use for styling the actual edit field within a cell which is being edited.
    /// The value of this property can be overriden for individual columns using the 
    /// <see cref="GridColumn.EditFieldCssClass" /> property.
    /// </para>
    /// <para>
    /// See also the <see cref="EditCellCssClass" /> property which is used for styling the cell as a whole.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string EditFieldCssClass
    {
      get
      {
        string s = (string)Properties["EditFieldCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["EditFieldCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to the optional footer row.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A footer row is generally used to provide a summary or set of totals for the level. It is composed of one or more
    /// client templates which are used to display relevant data. The client templates can be defined for an entire row, using
    /// the <see cref="GridLevel" /> <see cref="FooterRowClientTemplateId" /> property, or for individual <see cref="GridColumn">GridColumns</see>
    /// using the <see cref="GridColumn.FooterCellClientTemplateId" /> property.
    /// </para>
    /// <para>
    /// The <see cref="FooterRowCssClass" /> property is used to define a CSS class which will style the row.
    /// </para>
    /// </remarks>
    /// <seealso cref="ShowFooterRow" />
    [DefaultValue("")]
    public string FooterRowCssClass
    {
      get
      {
        string s = (string)Properties["FooterRowCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["FooterRowCssClass"] = value;
      }
    }

    /// <summary>
    /// The client template to use for the optional footer row.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A footer row is generally used to provide a summary or set of totals for the level. The footer can be set as a row for the entire
    /// <see cref="GridLevel" /> using this property, or for individual <see cref="GridColumn">columns</see> using the 
    /// <see cref="GridColumn.FooterCellClientTemplateId" />. If the footer is defined using this property, it will override
    /// any footer client templates which are defined for GridColumns.
    /// The footer row must be enabled explicitly using the <see cref="ShowFooterRow" /> property.
    /// </para>
    /// <para>
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see>, and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates with Grid</see> tutorials for more information on 
    /// client templates.
    /// </para>
    /// </remarks>
    /// <seealso cref="ShowFooterRow" />
    [DefaultValue("")]
    public string FooterRowClientTemplateId
    {
      get
      {
        string s = (string)Properties["FooterRowClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["FooterRowClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The client template to use for group headings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the grid is grouped, the groups are differentiated using headings. There are defaults, but they can be overridden using
    /// a client template defined by this property. The headings are styled using the <see cref="GroupHeadingCssClass" /> property.
    /// </para>
    /// <para>
    /// See the <a href="ms-its:ComponentArt.Web.UI.chm::/Grid_Grouping.htm">Grouping with Grid</a> and 
    /// <a href="ms-its:ComponentArt.Web.UI.chm::/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorials for more information
    /// on grouping.
    /// </para>
    /// <para>
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see>, and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates with Grid</see> tutorials for more information on 
    /// client templates.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string GroupHeadingClientTemplateId
    {
      get
      {
        string s = (string)Properties["GroupHeadingClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["GroupHeadingClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to group headings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the grid is grouped, the groups are differentiated using headings. The CSS class defined by this property
    /// will style those headings. Although there are default headings defined, they can be overridden using
    /// a client template defined by the <see cref="GroupHeadingClientTemplateId" /> property. 
    /// </para>
    /// <para>
    /// See the <a href="ms-its:ComponentArt.Web.UI.chm::/Grid_Grouping.htm">Grouping with Grid</a> and 
    /// <a href="ms-its:ComponentArt.Web.UI.chm::/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorials for more information
    /// on grouping.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string GroupHeadingCssClass
    {
      get
      {
        string s = (string)Properties["GroupHeadingCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["GroupHeadingCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to column headings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property defines a default style for the entire <see cref="GridLevel" />. This style can be overridden
    /// for individual columns using the <see cref="GridColumn.HeadingCellCssClass" /> property.
    /// </para> 
    /// <para>
    /// The following properties can be used to style the column headings when they are in alternate states:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="HeadingCellActiveCssClass" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="HeadingCellHoverCssClass" />
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The entire row of cells can be 
    /// styled using the <see cref="HeadingRowCssClass" /> property, and the <see cref="HeadingTextCssClass" /> property
    /// can be used to style the actual text which is contained within the heading cells.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HeadingCellCssClass
    {
      get
      {
        string s = (string)Properties["HeadingCellCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingCellCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to column headings on mouse down.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The following properties can be used to style the column headings when they are in alternate states:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="HeadingCellCssClass" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="HeadingCellHoverCssClass" />
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The entire row of cells can be 
    /// styled using the <see cref="HeadingRowCssClass" /> property, and the <see cref="HeadingTextCssClass" /> property
    /// can be used to style the actual text which is contained within the heading cells.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HeadingCellActiveCssClass
    {
      get
      {
        string s = (string)Properties["HeadingCellActiveCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingCellActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to column headings on hover.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The following properties can be used to style the column headings when they are in alternate states:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="HeadingCellCssClass" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="HeadingCellActiveCssClass" />
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The entire row of cells can be 
    /// styled using the <see cref="HeadingRowCssClass" /> property, and the <see cref="HeadingTextCssClass" /> property
    /// can be used to style the actual text which is contained within the heading cells.
    /// </para>
    [DefaultValue("")]
    public string HeadingCellHoverCssClass
    {
      get
      {
        string s = (string)Properties["HeadingCellHoverCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingCellHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to the column heading row.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property defines a style class which is applied to the entire row of column headings. Individual heading cells
    /// can be styled for the entire level using the <see cref="HeadingCellCssClass" /> property, or customized for individual 
    /// <see cref="GridColumn">columns</see> using the <see cref="GridColumn.HeadingCellCssClass" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HeadingRowCssClass
    {
      get
      {
        string s = (string)Properties["HeadingRowCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingRowCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to the selector cell in the heading row.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The selector column can be displayed or hidden using the <see cref="ShowSelectorCells" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HeadingSelectorCellCssClass
    {
      get
      {
        string s = (string)Properties["HeadingSelectorCellCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingSelectorCellCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to text in the column heading cells.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to style the <see cref="GridColumn.HeadingText" /> which is contained within the
    /// heading cells. The cells themselves can be styled using the <see cref="HeadingCellCssClass" /> property, and the
    /// whole row can be styled using the <see cref="HeadingRowCssClass" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HeadingTextCssClass
    {
      get
      {
        string s = (string)Properties["HeadingTextCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingTextCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to rows on hover.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A row enters a hover state when the mouse is over it, as well as when it is highlighted using keyboard navigation.
    /// This property can be used in conjunction with the <see cref="AlternatingHoverRowCssClass" /> property to create
    /// a stripe effect.
    /// </para>
    /// <para>
    /// Rows which are not under any special condition are styled using the CSS class specified by <see cref="RowCssClass" />
    /// and <see cref="AlternatingRowCssClass" />
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HoverRowCssClass
    {
      get
      {
        string s = (string)Properties["HoverRowCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HoverRowCssClass"] = value;
      }
    }

    /// <summary>
    /// The name of the column to indicate as the sort column by rendering a sort image in the heading. To be used
    /// only for overriding default behaviour.
    /// </summary>
    [DefaultValue("")]
    public string IndicatedSortColumn
    {
      get
      {
        string s = (string)Properties["IndicatedSortColumn"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["IndicatedSortColumn"] = value;
      }
    }

    /// <summary>
    /// The direction ("ASC" or "DESC") to indicate as the sort direction by rendering the appropriate sort image in the column heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// As with the <see cref="IndicatedSortColumn" /> property, this property is only necessary when the default Grid behavior 
    /// is being overridden.
    /// </para>
    /// </remarks>
    /// <seealso cref="IndicatedSortColumn" />
    [DefaultValue("")]
    public string IndicatedSortDirection
    {
      get
      {
        string s = (string)Properties["IndicatedSortDirection"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["IndicatedSortDirection"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the EditCommand column when inserting a new item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The template specified by this property is used to provide commands which are available when an item is being inserted. This
    /// occurs when a new item is added to the Grid, and is still in edit mode. The
    /// template will be used for the column which has its <see cref="GridColumn.EditControlType" /> set to EditCommand. 
    /// </para>
    /// <para>
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see> and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates in Grid</see> tutorials
    /// for more information on client templates. 
    /// </para>
    /// </remarks>
    /// <seealso cref="GridColumn.EditControlType" />
    [DefaultValue("")]
    public string InsertCommandClientTemplateId
    {
      get
      {
        string s = (string)Properties["InsertCommandClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["InsertCommandClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to rows.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the main class which is used to style rendered <see cref="GridItem">GridItems</see>.
    /// If the <see cref="AlternatingRowCssClass" /> property is also set, the two styles will alternate with that
    /// class applied to every second row.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string RowCssClass
    {
      get
      {
        string s = (string)Properties["RowCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["RowCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to selected rows.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The value of the <see cref="Grid.AllowMultipleSelect" /> property determines whether or not more than one item can be selected
    /// at once. The currently selected item(s) can be accessed programatically through the <see cref="Grid.SelectedItems" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string SelectedRowCssClass
    {
      get
      {
        string s = (string)Properties["SelectedRowCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["SelectedRowCssClass"] = value;
      }
    }

    /// <summary>
    /// The image to display in the selector cell when the row is selected.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Selector cells are located in a seperate column which is hidden by default. Their purpose is to display an image which
    /// indicates when a row (<see cref="GridItem" />) is selected. To enable the selector column, use the
    /// <see cref="ShowSelectorCells" /> property. 
    /// </para>
    /// <para>
    /// Other style properties related to the selector cells are as follows:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="SelectorImageWidth" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorImageHeight" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorCellCssClass" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorCellWidth" />
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [DefaultValue("")]
    public string SelectorImageUrl
    {
      get
      {
        string s = (string)Properties["SelectorImageUrl"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["SelectorImageUrl"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of the selector image.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Selector cells are located in a seperate column which is hidden by default. Their purpose is to display an image which
    /// indicates when a row (<see cref="GridItem" />) is selected. To enable the selector column, use the
    /// <see cref="ShowSelectorCells" /> property. 
    /// </para>
    /// <para>
    /// Other style properties related to the selector cells, and selector image are as follows:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="SelectorImageUrl" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorImageWidth" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorCellCssClass" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorCellWidth" />
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [DefaultValue(0)]
    public int SelectorImageHeight
    {
      get
      {
        string s = (string)Properties["SelectorImageHeight"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["SelectorImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// The width (in pixels) of the selector image.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Selector cells are located in a seperate column which is hidden by default. Their purpose is to display an image which
    /// indicates when a row (<see cref="GridItem" />) is selected. To enable the selector column, use the
    /// <see cref="ShowSelectorCells" /> property. 
    /// </para>
    /// <para>
    /// Other style properties related to the selector cells are as follows:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="SelectorImageUrl" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorImageHeight" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorCellCssClass" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorCellWidth" />
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [DefaultValue(0)]
    public int SelectorImageWidth
    {
      get
      {
        string s = (string)Properties["SelectorImageWidth"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["SelectorImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// The CSS class to apply to the selector cells.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Selector cells are located in a seperate column which is hidden by default. Their purpose is to display an image which
    /// indicates when a row (<see cref="GridItem" />) is selected. To enable the selector column, use the
    /// <see cref="ShowSelectorCells" /> property. 
    /// </para>
    /// <para>
    /// Other style properties related to the selector cells are as follows:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="SelectorImageWidth" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorImageHeight" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorImageUrl" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorCellWidth" />
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [DefaultValue("")]
    public string SelectorCellCssClass
    {
      get
      {
        string s = (string)Properties["SelectorCellCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["SelectorCellCssClass"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the selector cells.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Selector cells are located in a seperate column which is hidden by default. Their purpose is to display an image which
    /// indicates when a row (<see cref="GridItem" />) is selected. To enable the selector column, use the
    /// <see cref="ShowSelectorCells" /> property. 
    /// </para>
    /// <para>
    /// Other style properties related to the selector cells are as follows:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="SelectorImageUrl" />
    /// </description>
    /// </item>    
    /// <item>
    /// <description>
    /// <see cref="SelectorImageWidth" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorImageHeight" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorCellCssClass" />
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [DefaultValue(0)]
    public int SelectorCellWidth
    {
      get
      {
        string s = (string)Properties["SelectorCellWidth"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["SelectorCellWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to display a footer row.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A footer row is generally used to provide a summary or set of totals for the level. It is composed of one or more
    /// client templates which are used to display relevant data. The client templates can be defined for an entire row, using
    /// the <see cref="GridLevel" /> <see cref="FooterRowClientTemplateId" /> property, or for individual <see cref="GridColumn">GridColumns</see>
    /// using the <see cref="GridColumn.FooterCellClientTemplateId" /> property.
    /// </para>
    /// <para>
    /// The <see cref="FooterRowCssClass" /> property is used to define a CSS class which will style the row.
    /// </para>
    /// </remarks>
    /// <seealso cref="FooterRowCssClass" />
    /// <seealso cref="FooterRowClientTemplateId" />
    [DefaultValue(false)]
    public bool ShowFooterRow
    {
      get
      {
        string s = (string)Properties["ShowFooterCells"];
        return s == null ? false : bool.Parse(s);
      }
      set
      {
        Properties["ShowFooterCells"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to display the column headings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each <see cref="GridColumn" /> displays a heading by default, indicating what data the column contains. This property
    /// can be set to false to hide those headings.  
    /// </para>
    /// <para>
    /// The <see cref="GridColumn.HeadingText" /> property is used to define the text which will be displayed in the heading cell for
    /// a column. If the HeadingText is not defined, the text will default to the value of the <see cref="GridColumn.DataField" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    public bool ShowHeadingCells
    {
      get
      {
        string s = (string)Properties["ShowHeadingCells"];
        return s == null ? true : bool.Parse(s);
      }
      set
      {
        Properties["ShowHeadingCells"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to display the selector cells.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Selector cells are located in a seperate column which is hidden by default, and will be displayed if this property
    /// is set to true. Their purpose is to display an image which indicates when a row (<see cref="GridItem" />) is selected.
    /// </para>    
    /// <para>
    /// The following properties can be used to style various aspects of the selector cells:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="SelectorImageUrl" />
    /// </description>
    /// </item>    
    /// <item>
    /// <description>
    /// <see cref="SelectorImageHeight" />
    /// </description>
    /// </item>        
    /// <item>
    /// <description>
    /// <see cref="SelectorImageWidth" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorImageHeight" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="SelectorCellCssClass" />
    /// </description>
    /// </item>
    /// </list>    
    /// </remarks>
    [DefaultValue(false)]
    public bool ShowSelectorCells
    {
      get
      {
        string s = (string)Properties["ShowSelectorCells"];
        return s == null ? false : bool.Parse(s);
      }
      set
      {
        Properties["ShowSelectorCells"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to display the headings above groups with the same sort value when the data is sorted.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When data is sorted by a column, each individual value can be displayed as a heading, dividing the items
    /// into groups. This occurs when the <see cref="Grid" /> is sorted by the user clicking a <see cref="GridColumn">column</see> heading,
    /// or when the client-side <a href="mk:@MSITStore:ComponentArt.Web.UI.Reference_Client.chm::/ComponentArt.Web.UI~Grid_sort_method.htm">sort</a>
    /// method is called.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    public bool ShowSortHeadings
    {
      get
      {
        string s = (string)Properties["ShowSortHeadings"];
        return s == null ? false : bool.Parse(s);
      }
      set
      {
        Properties["ShowSortHeadings"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to display the table heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The table heading is created using a client template (specified by the <see cref="TableHeadingClientTemplateId" /> property) and 
    /// can be styled using the <see cref="TableHeadingCssClass" /> property.
    /// </para>
    /// <para>
    /// Note that there is no default table heading, so a client template must be defined and referenced by
    /// <code>TableHeadingClientTemplateId</code> if this property is set to true. 
    /// </para>
    /// <para>
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see> and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates in Grid</see> tutorials
    /// for more information on client templates. 
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    public bool ShowTableHeading
    {
      get
      {
        string s = (string)Properties["ShowTableHeading"];
        return s == null ? false : bool.Parse(s);
      }
      set
      {
        Properties["ShowTableHeading"] = value.ToString();
      }
    }

    /// <summary>
    /// The image to use for indicating the ascending sort direction in a column heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="SortedHeadingCellCssClass" /> property can be used to style the heading cell which will contain
    /// this image.
    /// </para>
    /// </remarks>
    /// <seealso cref="SortDescendingImageUrl" />
    [DefaultValue("")]
    public string SortAscendingImageUrl
    {
      get
      {
        string s = (string)Properties["SortAscendingImageUrl"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["SortAscendingImageUrl"] = value;
      }
    }

    /// <summary>
    /// The image to use for indicating the descending sort direction in a column heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="SortedHeadingCellCssClass" /> property can be used to style the heading cell which will contain
    /// this image.
    /// </para>
    /// </remarks>
    /// <seealso cref="SortAscendingImageUrl" />
    [DefaultValue("")]
    public string SortDescendingImageUrl
    {
      get
      {
        string s = (string)Properties["SortDescendingImageUrl"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["SortDescendingImageUrl"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to data cells in the column that is sorted by.
    /// </summary>
    [DefaultValue("")]
    public string SortedDataCellCssClass
    {
      get
      {
        string s = (string)Properties["SortedDataCellCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["SortedDataCellCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to the heading of the column that is sorted by.
    /// </summary>
    /// <seealso cref="SortAscendingImageUrl" />
    /// <seealso cref="SortDescendingImageUrl" />
    [DefaultValue("")]
    public string SortedHeadingCellCssClass
    {
      get
      {
        string s = (string)Properties["SortedHeadingCellCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["SortedHeadingCellCssClass"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for sort headings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the id of the client template which is used to customize sort heading rows. 
    /// Sort headings can be used to provide a clearer separation between individual values when a <see cref="Grid" /> is sorted. 
    /// A new heading will appear whenever a new value occurs in the sorted column. Sort headings have a default layout 
    /// which will be used if this property is not set. 
    /// </para>
    /// <para>
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see> and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates in Grid</see> tutorials
    /// for more information on client templates. 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string SortHeadingClientTemplateId
    {
      get
      {
        string s = (string)Properties["SortHeadingClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["SortHeadingClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to apply to sort headings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the CSS class which is used to style sort heading rows. 
    /// Sort headings can be used to provide a clearer separation between individual values when a <see cref="Grid" /> is sorted. 
    /// A new heading will appear whenever a new value occurs in the sorted column. 
    /// </para>
    /// <para>
    /// Sort headings have a default layout, but can be customized with client templates using the 
    /// <see cref="SortHeadingClientTemplateId" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string SortHeadingCssClass
    {
      get
      {
        string s = (string)Properties["SortHeadingCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["SortHeadingCssClass"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of sort images.
    /// </summary>
    [DefaultValue(0)]
    public int SortImageHeight
    {
      get
      {
        string s = (string)Properties["SortImageHeight"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["SortImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// The width (in pixels) of sort images.
    /// </summary>
    [DefaultValue(0)]
    public int SortImageWidth
    {
      get
      {
        string s = (string)Properties["SortImageWidth"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["SortImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// The ID of the client tamplate to use for the table heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the id of the client template which is used to customize the table heading. 
    /// There is no default table heading, so this property must be set, and reference an existing client template before 
    /// <see cref="ShowTableHeading" /> can be set to true. 
    /// </para>
    /// <para>
    /// The table heading is styled using the CSS class referenced by the <see cref="TableHeadingCssClass" /> property. 
    /// </para>
    /// <para>
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see> and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates in Grid</see> tutorials
    /// for more information on client templates. 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string TableHeadingClientTemplateId
    {
      get
      {
        string s = (string)Properties["TableHeadingClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["TableHeadingClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the table heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the CSS class to use for styling the table heading. 
    /// The table heading is enabled using the <see cref="ShowTableHeading" /> property and is defined using a 
    /// client template referenced by the <see cref="TableHeadingClientTemplateId" /> property. 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string TableHeadingCssClass
    {
      get
      {
        string s = (string)Properties["TableHeadingCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["TableHeadingCssClass"] = value;
      }
    }
    
    #endregion

    #region Public Methods

    public GridLevel()
    {
      Properties = new Hashtable();
    }
    
    public GridLevel Clone()
    {
      GridLevel oClone = new GridLevel();

      foreach(string sKey in Properties.Keys)
      {
        oClone.Properties[sKey] = Properties[sKey];
      }

      foreach(GridColumn oColumn in this.Columns)
      {
        oClone.Columns.Add(oColumn);
      }

      return oClone;
    }

    #endregion

    #region Internal Methods

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();
      oSB.Append("<Level");

      foreach(string sKey in Properties.Keys)
      {
        string sValue = (string)Properties[sKey];
        if (sValue != null)
        {
          oSB.Append(" " + sKey + "=\"" + sValue.Replace("&", "&amp;") + "\"");
        }
      }
      
      oSB.Append(">");
      oSB.Append(Columns.GetXml());
      oSB.Append("</Level>");

      return oSB.ToString();
    }

    internal void LoadXml(XmlNode oNode)
    {
      foreach(XmlAttribute oAttr in oNode.Attributes)
      {
        Properties[oAttr.Name] = oAttr.Value;
      }

      Columns.LoadXml(oNode.InnerXml);
    }

    #endregion
	}
}
