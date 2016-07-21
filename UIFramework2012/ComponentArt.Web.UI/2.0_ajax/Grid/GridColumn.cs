using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Globalization;


namespace ComponentArt.Web.UI
{
  internal class TypeTypeConverter : TypeConverter
  {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      if(sourceType == typeof(string))
      {
        return true;
      }

      return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      if(value == null)
      {
        return null;
      }
      else
      {
        return Type.GetType((string)value);
      }
    }
  }

	/// <summary>
  /// Defines a data field of the <see cref="Grid"/> control and its associated visual aspects.
	/// </summary>
  /// <remarks>
  /// <para>
  /// The GridColumn class represents a data field in the underlying datasource, and is responsible for the style and behavior
  /// of the rendered column and all associated cells. Each <see cref="GridLevel" /> has a <see cref="GridLevel.Columns">Columns</see>
  /// collection which contains all of the columns associated with that level.
  /// </para>
  /// <para>
  /// The <see cref="DataField" /> property defines which field a column is bound to in the underlying datasource. Although it is 
  /// necessary for databinding, that property can be left undefined, allowing the column to be populated programmatically.
  /// </para>
  /// <para>
  /// Templates provide a means of customizing the rendered cells beyond what simple CSS styles allow. For information on implementing templates
  /// on the server-side, see the following tutorials: <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see>
  /// and <see cref="ComponentArt.Web.UI.chm::/Grid_Server_Templates.htm">Using Server Templates in Grid</see>.
  /// </para>
  /// <para>
  /// See the <see cref="ComponentArt.Web.UI.chm::/Grid_Checkboxes.htm">Using Checkboxes in Grid</see> tutorial for information on implementing
  /// checkboxes for columns which display boolean data. 
  /// </para>
  /// </remarks>
	public sealed class GridColumn
	{
    internal int ColumnIndex
    {
      get
      {
        string s = (string)Properties["ColumnIndex"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["ColumnIndex"] = value.ToString();
      }
    }

    internal string ForeignData
    {
      get
      {
        string s = (string)Properties["ForeignData"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ForeignData"] = value;
      }
    }

    private Hashtable Properties;

    #region Public Properties

    /// <summary>
    /// The alignment of content in the column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The alignment is applied to both the header of the column, as well as the content cells. 
    /// Note that the images specified by <see cref="GridLevel.SortAscendingImageUrl" /> and <see cref="GridLevel.SortDescendingImageUrl" />
    /// which are usually placed on the right-side of the header cell, will be moved over to the left if this property is set to TextAlign.Right.
    /// For the TextAlign.Left (the default), and TextAlign.Center alignment, the image position is not altered.
    /// </para>
    /// </remarks>
    [Category("Appearance")]
    [DefaultValue(TextAlign.Left)]
    [Description("The content alignment to apply to the header of this column.")]
    public TextAlign Align
    {
      get
      {
        string o = (string)Properties["Align"];
        return (o != null) ? Utils.ParseTextAlign(o) : TextAlign.Left;
      }
      set
      {
        Properties["Align"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to allow editing of data in this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this property is set, it will override the Grid's default setting, which can be set using the <see cref="Grid.AllowEditing" /> property.
    /// The way that editing is handled depends on the value of the <see cref="Grid.RunningMode" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(InheritBool.Inherit)]
    public InheritBool AllowEditing
    {
      get
      {
        string s = (string)Properties["AllowEditing"];
        return s == null ? InheritBool.Inherit : (InheritBool)Enum.Parse(typeof(InheritBool), s);
      }
      set
      {
        Properties["AllowEditing"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to allow grouping by this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If set, this property will override the default setting for the <see cref="GridLevel" />, specified using the 
    /// <see cref="GridLevel.AllowGrouping" /> property.
    /// When grouping is enabled, it is important to consider both the <see cref="Grid.RunningMode" /> of the current <see cref="Grid" />,
    /// and the <see cref="Grid.GroupingMode" />, as both of those properties determine how the grouping functionality will be implemented.
    /// </para>
    /// <para>
    /// See the <a href="ms-its:ComponentArt.Web.UI.chm::/Grid_Grouping.htm">Grouping with Grid</a> and 
    /// <a href="ms-its:ComponentArt.Web.UI.chm::/Grid_Manual_Paging.htm">Grouping with Manual Paging</a> tutorials for more information
    /// on grouping.
    /// </para>
    /// </remarks>
    [DefaultValue(InheritBool.Inherit)]
    public InheritBool AllowGrouping
    {
      get
      {
        string s = (string)Properties["AllowGrouping"];
        return s == null ? InheritBool.Inherit : (InheritBool)Enum.Parse(typeof(InheritBool), s);
      }
      set
      {
        Properties["AllowGrouping"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to allow HTML in the content for this column.
    /// </summary>
    [DefaultValue(InheritBool.Inherit)]
    public InheritBool AllowHtmlContent
    {
      get
      {
        string s = (string)Properties["AllowHtmlContent"];
        return s == null ? InheritBool.Inherit : (InheritBool)Enum.Parse(typeof(InheritBool), s);
      }
      set
      {
        Properties["AllowHtmlContent"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to allow reordering of this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default, columns can be dragged to a new position by the user. This property can be set to false to prevent that behavior.
    /// If this property is true, the <see cref="Grid.ColumnReorder" /> event will fire on the server when the column order has changed.
    /// If set, this property will override the default setting for the <see cref="GridLevel" />, which is set using the 
    /// <see cref="GridLevel.AllowReordering" /> property.
    /// </para>
    /// <para>
    /// The <see cref="GridLevel.ColumnReorderIndicatorImageUrl" /> property allows an image to be referenced which will visually indicate
    /// the new position of the column when it is being dragged.
    /// </para>
    /// </remarks>
    [DefaultValue(InheritBool.Inherit)]
    public InheritBool AllowReordering
    {
      get
      {
        string s = (string)Properties["AllowReordering"];
        return s == null ? InheritBool.Inherit : (InheritBool)Enum.Parse(typeof(InheritBool), s);
      }
      set
      {
        Properties["AllowReordering"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to allow sorting by this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default behavior of the <see cref="Grid" /> is to allow sorting based on any column. If set, this property
    /// will override the default <see cref="GridLevel" /> setting, which is specified using the <see cref="GridLevel.AllowSorting" />
    /// property.
    /// </para>
    /// </remarks>
    [DefaultValue(InheritBool.Inherit)]
    public InheritBool AllowSorting
    {
      get
      {
        string s = (string)Properties["AllowSorting"];
        return s == null ? InheritBool.Inherit : (InheritBool)Enum.Parse(typeof(InheritBool), s);
      }
      set
      {
        Properties["AllowSorting"] = value.ToString();
      }
    }

    /// <summary>
    /// The type of this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify whether a column which represents boolean data will be implemented as user-selectable checkboxes. 
    /// </para>
    /// <para>
    /// For more information on implementing a checkbox column, see the following tutorial:
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Checkboxes.htm">CheckBox Columns in ComponentArt Grid</see>.
    /// </para>
    /// </remarks>
    [DefaultValue(GridColumnType.Default)]
    public GridColumnType ColumnType
    {
      get
      {
        string s = (string)Properties["ColumnType"];
        return s == null ? GridColumnType.Default : (GridColumnType)Enum.Parse(typeof(GridColumnType), s);
      }
      set
      {
        Properties["ColumnType"] = value.ToString();
      }
    }

    /// <summary>
    /// The ID of the context menu to use for the heading of this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This should be set to the client ID of a ComponentArt Menu control. 
    /// See the <see cref="ComponentArt.Web.UI.chm::/Grid_Column_Context_Menus.htm">Column Context Menus</see> tutorial for more information.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string ContextMenuId
    {
      get
      {
        string s = (string)Properties["ContextMenuId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ContextMenuId"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the area of the column heading which is used to displaying the context menu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property applies to column-specific context menus.
    /// See the <see cref="ComponentArt.Web.UI.chm::/Grid_Column_Context_Menus.htm">Column Context Menus</see> tutorial for more information.
    /// </para>
    /// <para>
    /// The <see cref="ContextMenuHotSpotActiveCssClass" /> and <see cref="ContextMenuHotSpotHoverCssClass" /> properties are used to style the same
    /// area, when it is in different states.
    /// </para>
    /// </remarks>
    /// <seealso cref="ContextMenuId" />
    [DefaultValue("")]
    public string ContextMenuHotSpotCssClass
    {
      get
      {
        string s = (string)Properties["ContextMenuHotSpotCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ContextMenuHotSpotCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the area of the column heading which is used to displaying the context menu, on mouse down.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property applies to column-specific context menus.
    /// See the <see cref="ComponentArt.Web.UI.chm::/Grid_Column_Context_Menus.htm">Column Context Menus</see> tutorial for more information.
    /// </para>
    /// <para>
    /// The <see cref="ContextMenuHotSpotHoverCssClass" /> and <see cref="ContextMenuHotSpotCssClass" /> properties are used to style the same
    /// area, when it is in different states.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string ContextMenuHotSpotActiveCssClass
    {
      get
      {
        string s = (string)Properties["ContextMenuHotSpotActiveCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ContextMenuHotSpotActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// The CSS class to use for the area of the column heading which is used to displaying the context menu, on hover.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property applies to column-specific context menus.
    /// See the <see cref="ComponentArt.Web.UI.chm::/Grid_Column_Context_Menus.htm">Column Context Menus</see> tutorial for more information.
    /// </para>
    /// <para>
    /// The <see cref="ContextMenuHotSpotActiveCssClass" /> and <see cref="ContextMenuHotSpotCssClass" /> properties are used to style the same
    /// area, when it is in different states.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string ContextMenuHotSpotHoverCssClass
    {
      get
      {
        string s = (string)Properties["ContextMenuHotSpotHoverCssClass"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ContextMenuHotSpotHoverCssClass"] = value;
      }
    }

    /// <summary>
    /// The client-side expression to use for getting the value of a custom edit control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify a JavaScript expression which will be invoked when a row (<see cref="GridItem" />) leaves edit mode.
    /// The purpose of this expression is generally to retrieve the new value from a custom edit control(
    /// <see cref="GridColumn.EditCellServerTemplateId" />).
    /// </para>
    /// <para>
    /// When the expression is evaluated, the result should simply be the new value which will be placed into the field.
    /// </para>
    /// <para>
    /// This property is used in conjunction with the <see cref="GridColumn.CustomEditSetExpression" /> which is used to initialize and populate the
    /// custom edit control when editing is initiated.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string CustomEditGetExpression
    {
      get
      {
        string s = (string)Properties["CustomEditGetExpression"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["CustomEditGetExpression"] = value;
      }
    }

    /// <summary>
    /// The client-side expression to use for setting the value of a custom edit control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify a JavaScript expression which will be invoked when a row (<see cref="GridItem" />) is placed into edit mode.
    /// The purpose of this expression is generally to initialize and populate a custom edit control (
    /// <see cref="GridColumn.EditCellServerTemplateId" />) with the current value of the cell.
    /// </para>
    /// <para>
    /// Within the JavaScript expression, the <code>GridItem</code> object which is currently being edited is accessed through the <code>DataItem</code>
    /// identifier. This is similar to the expressions used in client templates. 
    /// (See the <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates in Grid</see>
    ///  tutorial for more information on that topic.)  
    /// </para>
    /// <para>
    /// The <see cref="GridColumn.CustomEditGetExpression" /> is then invoked when the item leaves edit mode, and is used to retrieve the new value 
    /// which was set by the custom control.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string CustomEditSetExpression
    {
      get
      {
        string s = (string)Properties["CustomEditSetExpression"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["CustomEditSetExpression"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the data cells in this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Data cells can be styled using the CSS class specified by the <see cref="GridColumn.DataCellCssClass" /> property.
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see> and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates in Grid</see> tutorials
    /// for more information on client templates.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string DataCellClientTemplateId
    {
      get
      {
        string s = (string)Properties["DataCellClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["DataCellClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CssClass to use for the data cells in this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Styles can also be applied to all data cells in a <see cref="GridLevel" /> using the <see cref="GridLevel.DataCellCssClass" /> property.
    /// </para>
    /// <para>
    /// Client templates offer another solution for further customization of cells. A template can be specified for all cells in a column using the 
    /// <see cref="DataCellClientTemplateId" /> property. 
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see> and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates in Grid</see> tutorials
    /// for more information on client templates.
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
    /// The ID of the server template to use for the data cells in this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cells can be customized using client or server templates. See the 
    /// <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see> tutorial for a discussion
    /// on the advantages of each templating method. 
    /// The <see cref="ComponentArt.Web.UI.chm::/Grid_Server_Templates.htm">Using Server Templates in Grid</see> tutorial
    /// contains information specific to server templates.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string DataCellServerTemplateId
    {
      get
      {
        string s = (string)Properties["DataCellServerTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["DataCellServerTemplateId"] = value;
      }
    }

    /// <summary>
    /// The name of the field in the data source to use for this column.
    /// </summary>
    [DefaultValue("")]
    public string DataField
    {
      get
      {
        string s = (string)Properties["DataField"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["DataField"] = value;
      }
    }

    private Type _dataType = null;
    /// <summary>
    /// The type of the data in this column.
    /// </summary>
    [DefaultValue(null)]
    [TypeConverter(typeof(TypeTypeConverter))]
    public Type DataType
    {
      get
      {
        if(_dataType == null)
        {
          string s = (string)Properties["DataType"];
          _dataType = (s == null ? null : Type.GetType(s));
        }

        return _dataType;
      }
      set
      {
        Properties["DataType"] = value.ToString();
        _dataType = value;
      }
    }

    /// <summary>
    /// The sort direction to use for the first sort request on this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the default order that a column will be sorted by when the heading is clicked.
    /// </para>
    /// <para>
    /// Note that this property only affects sorting when the user clicks the column heading and not when the column is sorted programmatically. 
    /// When sorting with the sort method, the default value is always ascending order. The second argument to the method allows the order 
    /// to be specified explicitly when the method is called.
    /// </para>
    /// </remarks>
    [DefaultValue(GridSortDirection.Ascending)]
    public GridSortDirection DefaultSortDirection
    {
      get
      {
        string s = (string)Properties["DefaultSortDirection"];
        return s == null ? GridSortDirection.Ascending : (GridSortDirection)Enum.Parse(typeof(GridSortDirection), s);
      }
      set
      {
        Properties["DefaultSortDirection"] = value.ToString();
      }
    }

    /// <summary>
    /// The CssClass to use on cells in this column when in edit mode.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If set, this property will override the default setting for the <see cref="GridLevel" />, specified using the 
    /// <see cref="GridLevel.EditCellCssClass" /> property.
    /// </para>
    /// <para>
    /// See also the  <see cref="EditFieldCssClass" /> property.
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
    /// The type of control to use for editing cells in this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify the type of control which will be used for editing the column. The <code>GridColumnEditControlType.Default</code>
    /// value will attempt to use an appropriate control for the <see cref="GridColumn.DataType" />. To implement a custom edit control,
    /// a value of <code>GridColumnEditControlType.Custom</code> must be set, and the control must be defined using the 
    /// <see cref="GridColumn.EditCellServerTemplateId" /> property. In order to populate the custom control and retrieve
    /// the edited value from the control, the JavaScript expressions specified by the 
    /// <see cref="GridColumn.CustomEditGetExpression" /> and <see cref="GridColumn.CustomEditSetExpression" /> properties are used.
    /// </para>
    /// <para>
    /// A value of <code>GridColumnEditControlType.EditCommand</code> is used to specify a column which contains functional commands relating
    /// to the edit process (eg: edit, delete, submit, or cancel). The actual content of the cells in the column is customized using templates. 
    /// When editing is initiated, the template specified by the <see cref="GridLevel" /> 
    /// <see cref="GridLevel.EditCommandClientTemplateId" /> property is used for the cell. For more information on templates, see the following 
    /// tutorials: <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see>, 
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates with Grid</see>,
    /// and <see cref="ComponentArt.Web.UI.chm::/Grid_Server_Templates.htm">Using Server Templates with Grid</see>.
    /// </para>
    /// </remarks>
    [Category("Appearance")]
    [DefaultValue(GridColumnEditControlType.Default)]
    [Description("The type of control to use when editing this column.")]
    public GridColumnEditControlType EditControlType
    {
      get
      {
        string s = (string)Properties["EditControlType"];
        return s == null? GridColumnEditControlType.Default : (GridColumnEditControlType)Enum.Parse(typeof(GridColumnEditControlType), s);
      }
      set
      {
        Properties["EditControlType"] = value.ToString();
      }
    }

    /// <summary>
    /// The ID of the server template to use for editing cells in this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The server template specified by this property will be used for cells which are being edited.
    /// To enable this template, the <see cref="GridColumn.EditControlType" /> property must be set to <code>GridColumnEditControlType.Custom</code>.
    /// In order to initialize the control, a JavaScript expression can be defined using the <see cref="GridColumn.CustomEditSetExpression" /> property.
    /// The <see cref="GridColumn.CustomEditGetExpression" /> property is then used to retrieve the value from the control when editing is complete.
    /// For more information on templates, see the following 
    /// tutorials: <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see>, 
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates with Grid</see>,
    /// and <see cref="ComponentArt.Web.UI.chm::/Grid_Server_Templates.htm">Using Server Templates with Grid</see>. 
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string EditCellServerTemplateId
    {
      get
      {
        string s = (string)Properties["EditCellServerTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["EditCellServerTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CssClass to use on edit fields in this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the CSS class to use for styling the actual edit field within a cell which is being edited.
    /// If set, this property will override the default setting for the <see cref="GridLevel" />, specified using the 
    /// <see cref="GridLevel.EditFieldCssClass" /> property.
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
    /// Whether to disallow resizing of this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether or not the column width can by altered by the user. The default value is false, 
    /// meaning that the user can drag the edge of the column, altering its width.
    /// </para>
    /// <para>
    /// Note that the width can still be altered programmatically, using the <see cref="Width" /> property, regardless of the value
    /// of this property.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    public bool FixedWidth
    {
      get
      {
        string s = (string)Properties["FixedWidth"];
        return s == null ? false : bool.Parse(s);
      }
      set
      {
        Properties["FixedWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// The client template to use for the footer cell.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is used to specify a client template to be used as a footer cell. It is generally used to provide a summary or 
    /// total for the column. The footer row is defined separately for individual <see cref="GridLevel">GridLevels</see>, 
    /// and the client template can be defined either for individual columns, using this property, or the level as a whole. 
    /// To define a client template
    /// for the entire footer row, use the <see cref="GridLevel.FooterRowClientTemplateId" /> property. 
    /// Note that if a footer row client template is defined for the GridLevel, it will override individual column
    /// footer cells.
    /// The footer row must be enabled explicitly using the <see cref="GridLevel.ShowFooterRow" /> property.
    /// </para>
    /// <para>
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see>, and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates with Grid</see> tutorials for more information on 
    /// client templates.
    /// </para>
    /// </remarks>
    /// <seealso cref="GridLevel.ShowFooterRow" />
    [DefaultValue("")]
    public string FooterCellClientTemplateId
    {
      get
      {
        string s = (string)Properties["FooterCellClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["FooterCellClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The foreign key to use for looking up data for this column.
    /// </summary>
    [DefaultValue("")]
    public string ForeignDataKeyField
    {
      get
      {
        string s = (string)Properties["ForeignDataKeyField"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ForeignDataKeyField"] = value;
      }
    }
    
    /// <summary>
    /// The foreign table to look for this column's data in.
    /// </summary>
    [DefaultValue("")]
    public string ForeignTable
    {
      get
      {
        string s = (string)Properties["ForeignTable"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ForeignTable"] = value;
      }
    }

    /// <summary>
    /// The field in the foreign table that contains the display value to use in this column.
    /// </summary>
    [DefaultValue("")]
    public string ForeignDisplayField
    {
      get
      {
        string s = (string)Properties["ForeignDisplayField"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["ForeignDisplayField"] = value;
      }
    }

    /// <summary>
    /// The standard .NET format string to use for data in this column.
    /// </summary>
    /// <example>00.00</example>
    /// <example>(###) ###-####</example>
    /// <example>C2</example>
    [DefaultValue("")]
    public string FormatString
    {
      get
      {
        string s = (string)Properties["FormatString"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["FormatString"] = value;
      }
    }

    /// <summary>
    /// The ID of the client template to use for the heading cell of this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See the <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Templates in Web.UI</see>, and
    /// <see cref="ComponentArt.Web.UI.chm::/Grid_Client_Templates.htm">Using Client Templates with Grid</see> tutorials for more information on 
    /// client templates.
    /// </para>
    /// <para>
    /// The heading cells can be styled using the <see cref="HeadingCellCssClass" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HeadingCellClientTemplateId
    {
      get
      {
        string s = (string)Properties["HeadingCellClientTemplateId"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingCellClientTemplateId"] = value;
      }
    }

    /// <summary>
    /// The CssClass to use for the heading cell of this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If set, this property will override the default setting for the <see cref="GridLevel" />, specified using the 
    /// <see cref="GridLevel.HeadingCellCssClass" /> property. The heading cell can be further customized using a client
    /// template using the <see cref="HeadingCellClientTemplateId" /> property.
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
    /// The height (in pixels) of this column's heading grip image.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the height of the image used as a grip for the column heading. The image is placed at the 
    /// edge of the heading cell, and has a custom cursor associated with it, indicating that the column can be dragged. 
    /// </para>
    /// <para>
    /// Note that the side on which the image is placed depends on the value of the <see cref="Align" /> property. 
    /// For left and center alignment the grip image will be placed at the left side of the cell. 
    /// For right alignment, the image will be placed on the right. 
    /// </para>
    /// <para>
    /// See also the <see cref="HeadingGripImageWidth" /> and <see cref="HeadingGripImageUrl" /> properties.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    public int HeadingGripImageHeight
    {
      get
      {
        string s = (string)Properties["HeadingGripImageHeight"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["HeadingGripImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// The grip image to use for this column's heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the URL of an image to use as a grip for the column heading. 
    /// The image is placed at the edge of the heading cell, and has a custom cursor associated with it, indicating that the column can be dragged. 
    /// </para>
    /// <para>
    /// Note that the side on which the image is placed depends on the value of the <see cref="Align" /> property.
    /// For left and center alignment the grip image will be placed at the left side of the cell. 
    /// For right alignment, the image will be placed on the right. 
    /// </para>
    /// <para>
    /// See also the <see cref="HeadingGripImageWidth" /> and <see cref="HeadingGripImageHeight" /> properties.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HeadingGripImageUrl
    {
      get
      {
        string s = (string)Properties["HeadingGripImageUrl"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingGripImageUrl"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of this column's heading grip image.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the width of the image used as a grip for the column heading. The image is placed at the 
    /// edge of the heading cell, and has a custom cursor associated with it, indicating that the column can be dragged. 
    /// </para>
    /// <para>
    /// Note that the side on which the image is placed depends on the value of the <see cref="Align" /> property. 
    /// For left and center alignment the grip image will be placed at the left side of the cell. 
    /// For right alignment, the image will be placed on the right. 
    /// </para>
    /// <para>
    /// See also the <see cref="HeadingGripImageWidth" /> and <see cref="HeadingGripImageUrl" /> properties.
    /// </para>
    /// </remarks>    
    [DefaultValue(0)]
    public int HeadingGripImageWidth
    {
      get
      {
        string s = (string)Properties["HeadingGripImageWidth"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["HeadingGripImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// The height (in pixels) of the column heading image.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the height of the image which is used as a heading for the column. 
    /// The URL of the image is specified using the <see cref="HeadingImageUrl" /> property. The width of the image is specified using the 
    /// <see cref="HeadingImageWidth" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    public int HeadingImageHeight
    {
      get
      {
        string s = (string)Properties["HeadingImageHeight"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["HeadingImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// The image to use for the column heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the URL of the image which is used as a heading for the column. 
    /// The height and width of the image can be specified using the <see cref="HeadingImageHeight" /> and 
    /// <see cref="HeadingImageWidth " /> properties respectively.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HeadingImageUrl
    {
      get
      {
        string s = (string)Properties["HeadingImageUrl"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingImageUrl"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of the column heading image.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the width of the image which is used as a heading for the column. The URL of the image is specified 
    /// using the <see cref="HeadingImageUrl" /> property. The height of the image is specified using the 
    /// <see cref="HeadingImageHeight" /> property.
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    public int HeadingImageWidth
    {
      get
      {
        string s = (string)Properties["HeadingImageWidth"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["HeadingImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// The text to display in this column's heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this property is not defined, the text displayed in the heading will be the value of the <see cref="DataField" /> property.
    /// Styling of the heading cell is done using the <see cref="HeadingCellCssClass" /> property, and styles related 
    /// specifically to the text can be specified using the <see cref="HeadingTextCssClass" /> property.
    /// </para>
    /// <para>
    /// Another option is to define an image using the <see cref="HeadingImageUrl" /> property, which will be displayed in the heading
    /// cell instead of text.
    /// </para>
    /// </remarks>
    [DefaultValue("")]
    public string HeadingText
    {
      get
      {
        string s = (string)Properties["HeadingText"];
        return s == null ? "" : s;
      }
      set
      {
        Properties["HeadingText"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to this column's heading text.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the CSS class used to style the text which is contained within the column's heading cell. 
    /// The text itself can be retrieved or altered using the <see cref="HeadingText" /> property. The containing cell 
    /// can be styled using the <see cref="HeadingCellCssClass" /> property. 
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
    /// Whether to search within this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether or not data from the column will be included in searches. 
    /// The default behavior of the <see cref="Grid" /> <see cref="Grid.Search" /> method is to search data in all columns for matching values. 
    /// This property allows the search to be limited to certain columns.
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    public bool IsSearchable
    {
      get
      {
        string s = (string)Properties["IsSearchable"];
        return s == null ? (this.DataType == typeof(string)? true : false) : bool.Parse(s);
      }
      set
      {
        Properties["IsSearchable"] = value.ToString();
      }
    }

    /// <summary>
    /// The CssClass to apply to data cells when the grid is sorted by this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If set, this property will override the default setting for the <see cref="GridLevel" />, specified using the 
    /// <see cref="GridLevel.SortedDataCellCssClass" /> property.
    /// </para>
    /// <para>
    /// The heading cell for the column is styled by the <see cref="SortedHeadingCellCssClass" /> property.
    /// </para>
    /// </remarks>
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
    /// The CssClass to apply to the heading cell when the grid is sorted by this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If set, this property will override the default setting for the <see cref="GridLevel" />, specified using the 
    /// <see cref="GridLevel.SortedHeadingCellCssClass" /> property.
    /// </para>
    /// <para>
    /// The data cells for the column are styled by the <see cref="SortedDataCellCssclass" /> property.
    /// </para>
    /// </remarks>
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
    /// Whether to justify the sort (ascending or descending) image in the heading.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies the location of the sort image within the header cell of the column. 
    /// The default value for this property is true, which places the image at the edge of the cell. 
    /// </para>
    /// <para>
    /// If this property is set to false, the image will be displayed inline with the heading text, meaning the location will 
    /// vary according to the width of the text and the style properties defined using the <see cref="HeadingTextCssClass" /> property. 
    /// </para>
    /// <para>
    /// Note that the side on which the image is placed depends on the value of the <see cref="GridColumn" /> <see cref="Align" /> property. 
    /// For left and center alignment the sort image will be placed at the right side of the cell. 
    /// For right alignment, the image will be placed on the left.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    public bool SortImageJustify
    {
      get
      {
        string s = (string)Properties["SortImageJustify"];
        return s == null ? true : bool.Parse(s);
      }
      set
      {
        Properties["SortImageJustify"] = value.ToString();
      }
    }

    /// <summary>
    /// Free-form custom data associated with this column.
    /// </summary>
    [DefaultValue(null)]
    public string Tag
    {
      get
      {
        return (string)Properties["Tag"];
      }
      set
      {
        Properties["Tag"] = (string)value;
      }
    }

    /// <summary>
    /// Whether to wrap text in this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property specifies whether the text content of cells in this column will wrap or not. 
    /// The default value for this property is false. As a result, if there are no templates changing the behavior, 
    /// the text is displayed on a single line and will be cut off if the column width is insufficient. 
    /// </para>
    /// </remarks>
    [DefaultValue(false)]
    public bool TextWrap
    {
      get
      {
        string s = (string)Properties["TextWrap"];
        return s == null ? false : bool.Parse(s);
      }
      set
      {
        Properties["TextWrap"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to display this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Data for the column will still be passed to the client, regardless of the value of this property, and can still be manipulated 
    /// programmatically. The value of this property only affects whether or not the column is rendered.
    /// </para>
    /// </remarks>
    [DefaultValue(true)]
    public bool Visible
    {
      get
      {
        string s = (string)Properties["Visible"];
        return s == null ? true : bool.Parse(s);
      }
      set
      {
        Properties["Visible"] = value.ToString();
      }
    }

    /// <summary>
    /// The width (in pixels) of this column.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that by default the width of a column can be adjusted by the user. To prevent that behavior, use the 
    /// <see cref="GridColumn.FixedWidth" /> property. The width can always be adjusted using this property, regardless of the value of FixedWidth. 
    /// </para>
    /// </remarks>
    [DefaultValue(0)]
    public int Width
    {
      get
      {
        string s = (string)Properties["Width"];
        return s == null ? 0 : int.Parse(s);
      }
      set
      {
        Properties["Width"] = value.ToString();
      }
    }

    #endregion

    #region Methods

    public GridColumn()
    {
      Properties = new Hashtable();
    }
    
    public GridColumn Clone()
    {
      GridColumn oClone = new GridColumn();

      foreach(string sKey in Properties.Keys)
      {
        oClone.Properties[sKey] = Properties[sKey];
      }

      return oClone;
    }

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();
      oSB.Append("<Column");

      foreach(string sKey in Properties.Keys)
      {
        oSB.Append(" " + sKey + "=\"" + Properties[sKey].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace("\"", "&quot;") + "\"");
      }
      
      oSB.Append(" />");

      return oSB.ToString();
    }

    internal void LoadXml(XmlNode oNode)
    {
      foreach(XmlAttribute oAttr in oNode.Attributes)
      {
        Properties[oAttr.Name] = oAttr.Value;
      }
    }
    
    #endregion
	}
}
