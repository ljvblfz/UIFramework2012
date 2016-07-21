using System;
using System.Collections.Generic;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define DataGridColumn objects.
  /// </summary>
  public class DataGridColumnBuilder
  {
    GridColumn column = new GridColumn();

    /// <summary>
    /// Builder to define DataGridColumn objects.
    /// </summary>
    /// <param name="column"></param>
    public DataGridColumnBuilder(GridColumn column)
    {
      this.column = column;
    }
    /// <summary>
    /// The alignment of content in the column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder Align(TextAlign value)
    {
      column.Align = value;

      return this;
    }
    /// <summary>
    /// Whether to allow editing of data in this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder AllowEditing(InheritBool value)
    {
      column.AllowEditing = value;

      return this;
    }
    /// <summary>
    /// Whether to allow grouping by this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder AllowGrouping(InheritBool value)
    {
      column.AllowGrouping = value;

      return this;
    }
    /// <summary>
    /// Whether to allow HTML in the content for this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder AllowHtmlContent(InheritBool value)
    {
      column.AllowHtmlContent = value;

      return this;
    }
    /// <summary>
    /// Whether to allow reordering of this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder AllowReordering(InheritBool value)
    {
      column.AllowReordering = value;

      return this;
    }
    /// <summary>
    /// Whether to allow sorting by this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder AllowSorting(InheritBool value)
    {
      column.AllowSorting = value;

      return this;
    }
    /// <summary>
    /// The type of this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder ColumnType(GridColumnType value)
    {
      column.ColumnType = value;

      return this;
    }
    /// <summary>
    /// The CSS class to use for the area of the column heading which is used to displaying the context menu, on mouse down. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder ContextMenuHotSpotActiveCssClass(string value)
    {
      column.ContextMenuHotSpotActiveCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to use for the area of the column heading which is used to displaying the context menu. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder ContextMenuHotSpotCssClass(string value)
    {
      column.ContextMenuHotSpotCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to use for the area of the column heading which is used to displaying the context menu, on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder ContextMenuHotSpotHoverCssClass(string value)
    {
      column.ContextMenuHotSpotHoverCssClass = value;

      return this;
    }
    /// <summary>
    /// The ID of the context menu to use for the heading of this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder ContextMenuId(string value)
    {
      column.ContextMenuId = value;

      return this;
    }
    /// <summary>
    /// The ID of the client template to use for the data cells in this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder DataCellClientTemplateId(string value)
    {
      column.DataCellClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The CssClass to use for the data cells in this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder DataCellCssClass(string value)
    {
      column.DataCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The name of the field in the data source to use for this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder DataField(string value)
    {
      column.DataField = value;

      return this;
    }
    /// <summary>
    /// The type of the data in this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder DataType(Type value)
    {
      column.DataType = value;

      return this;
    }
    /// <summary>
    /// The sort direction to use for the first sort request on this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder DefaultSortDirection(GridSortDirection value)
    {
      column.DefaultSortDirection = value;

      return this;
    }
    /// <summary>
    /// The CssClass to use on cells in this column when in edit mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder EditCellCssClass(string value)
    {
      column.EditCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The type of control to use for editing cells in this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder EditControlType(GridColumnEditControlType value)
    {
      column.EditControlType = value;

      return this;
    }
    /// <summary>
    /// The CssClass to use on edit fields in this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder EditFieldCssClass(string value)
    {
      column.EditFieldCssClass = value;

      return this;
    }
    /// <summary>
    /// Whether to disallow resizing of this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder FixedWidth(bool value)
    {
      column.FixedWidth = value;

      return this;
    }
    /// <summary>
    /// The client template to use for the footer cell. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder FooterCellClientTemplateId(string value)
    {
      column.FooterCellClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The standard .NET format string to use for data in this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder FormatString(string value)
    {
      column.FormatString = value;

      return this;
    }
    /// <summary>
    /// The ID of the client template to use for the heading cell of this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingCellClientTemplateId(string value)
    {
      column.HeadingCellClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The CssClass to use for the heading cell of this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingCellCssClass(string value)
    {
      column.HeadingCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The height (in pixels) of this column's heading grip image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingGripImageHeight(int value)
    {
      column.HeadingGripImageHeight = value;

      return this;
    }
    /// <summary>
    /// The grip image to use for this column's heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingGripImageUrl(string value)
    {
      column.HeadingGripImageUrl = value;

      return this;
    }
    /// <summary>
    /// The width (in pixels) of this column's heading grip image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingGripImageWidth(int value)
    {
      column.HeadingGripImageWidth = value;

      return this;
    }
    /// <summary>
    /// The height (in pixels) of the column heading image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingImageHeight(int value)
    {
      column.HeadingImageHeight = value;

      return this;
    }
    /// <summary>
    /// The image to use for the column heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingImageUrl(string value)
    {
      column.HeadingImageUrl = value;

      return this;
    }
    /// <summary>
    /// The width (in pixels) of the column heading image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingImageWidth(int value)
    {
      column.HeadingImageWidth = value;

      return this;
    }
    /// <summary>
    /// The text to display in this column's heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingText(string value)
    {
      column.HeadingText = value;

      return this;
    }
    /// <summary>
    /// The CssClass to apply to this column's heading text. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder HeadingTextCssClass(string value)
    {
      column.HeadingTextCssClass = value;

      return this;
    }
    /// <summary>
    /// Whether to search within this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder IsSearchable(bool value)
    {
      column.IsSearchable = value;

      return this;
    }
    /// <summary>
    /// The CssClass to apply to data cells when the grid is sorted by this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder SortedDataCellCssClass(string value)
    {
      column.SortedDataCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The CssClass to apply to the heading cell when the grid is sorted by this column.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder SortedHeadingCellCssClass(string value)
    {
      column.SortedHeadingCellCssClass = value;

      return this;
    }
    /// <summary>
    /// Whether to justify the sort (ascending or descending) image in the heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder SortImageJustify(bool value)
    {
      column.SortImageJustify = value;

      return this;
    }
    /// <summary>
    /// Whether to wrap text in this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder TextWrap(bool value)
    {
      column.TextWrap = value;

      return this;
    }
    /// <summary>
    /// Whether to display this column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder Visible(bool value)
    {
      column.Visible = value;

      return this;
    }
    /// <summary>
    /// Assigned width of the DataGridColumn.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridColumnBuilder Width(int value)
    {
      column.Width = value;

      return this;
    }
  }
}