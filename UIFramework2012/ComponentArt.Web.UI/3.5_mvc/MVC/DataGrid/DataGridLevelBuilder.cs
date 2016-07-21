using System;
using System.Collections.Generic;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define DataGridLevel objects.
  /// </summary>
  public class DataGridLevelBuilder
  {
    GridLevel level = new GridLevel();
    /// <summary>
    /// Builder to define GridLevel objects, each of which represents a level (table) of data in a DataGrid control. 
    /// </summary>
    /// <param name="level"></param>
    public DataGridLevelBuilder(GridLevel level)
    {
      this.level = level;
    }
    /// <summary>
    /// The collection of DataGridColumns for this level. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public DataGridLevelBuilder Columns(Action<DataGridColumnFactory> addAction)
    {
      var factory = new DataGridColumnFactory(level.Columns);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Collection of conditional formats, providing the ability to assign custom styles to rows that match a particular client filter. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public DataGridLevelBuilder ConditionalFormats(Action<DataGridConditionalFormatFactory> addAction)
    {
      var factory = new DataGridConditionalFormatFactory(level.ConditionalFormats);

      addAction(factory);
      return this;
    }
    /// <summary>
    /// Whether to allow grouping on this level. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder AllowGrouping(bool value)
    {
      level.AllowGrouping = value;

      return this;
    }
    /// <summary>
    /// Whether to allow column reordering on this level. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder AllowReordering(bool value)
    {
      level.AllowReordering = value;

      return this;
    }
    /// <summary>
    /// Whether to allow sorting on this level. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder AllowSorting(bool value)
    {
      level.AllowSorting = value;

      return this;
    }
    /// <summary>
    /// The CSS class to use on alternating rows, on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder AlternatingHoverRowCssClass(string value)
    {
      level.AlternatingHoverRowCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to use on alternating rows. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder AlternatingRowCssClass(string value)
    {
      level.AlternatingRowCssClass = value;

      return this;
    }
    /// <summary>
    /// Order of indices in which to draw columns.  eg. "[0, 1, 3, 2]"
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder ColumnDisplayOrder(string value)
    {
      level.ColumnDisplayOrder = value;

      return this;
    }
    /// <summary>
    /// The indicator image to use while dragging columns to the header to group.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder ColumnGroupIndicatorImageUrl(string value)
    {
      level.ColumnGroupIndicatorImageUrl = value;

      return this;
    }
    /// <summary>
    /// The indicator image to use while dragging to reorder columns. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder ColumnReorderIndicatorImageUrl(string value)
    {
      level.ColumnReorderIndicatorImageUrl = value;

      return this;
    }
    /// <summary>
    /// The CSS class to use on data cells on this level. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder DataCellCssClass(string value)
    {
      level.DataCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The name of the field which contains unique keys for items on this level. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder DataKeyField(string value)
    {
      level.DataKeyField = value;

      return this;
    }
    /// <summary>
    /// The name of the data member (ie. table) in the DataSource which corresponds to this level. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder DataMember(string value)
    {
      level.DataMember = value;

      return this;
    }
    /// <summary>
    /// The CSS class to use on cells when they are in edit mode. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder EditCellCssClass(string value)
    {
      level.EditCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The ID of the client template to use for the EditCommand column. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder EditCommandClientTemplateId(string value)
    {
      level.EditCommandClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to edit fields. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder EditFieldCssClass(string value)
    {
      level.EditFieldCssClass = value;

      return this;
    }
    /// <summary>
    /// The client template to use for the optional footer row. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder FooterRowClientTemplateId(string value)
    {
      level.FooterRowClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to the optional footer row. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder FooterRowCssClass(string value)
    {
      level.FooterRowCssClass = value;

      return this;
    }
    /// <summary>
    /// The client template to use for group headings. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder GroupHeadingClientTemplateId(string value)
    {
      level.GroupHeadingClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to group headings. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder GroupHeadingCssClass(string value)
    {
      level.GroupHeadingCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to column headings on mouse down. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder HeadingCellActiveCssClass(string value)
    {
      level.HeadingCellActiveCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to column headings. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder HeadingCellCssClass(string value)
    {
      level.HeadingCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to column headings on hover.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder HeadingCellHoverCssClass(string value)
    {
      level.HeadingCellHoverCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to the column heading row. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder HeadingRowCssClass(string value)
    {
      level.HeadingRowCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to the selector cell in the heading row. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder HeadingSelectorCellCssClass(string value)
    {
      level.HeadingSelectorCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to text in the column heading cells. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder HeadingTextCssClass(string value)
    {
      level.HeadingTextCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to rows on hover. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder HoverRowCssClass(string value)
    {
      level.HoverRowCssClass = value;

      return this;
    }
    /// <summary>
    /// The name of the column to indicate as the sort column by rendering a sort image in the heading.
    /// To be used only for overriding default behaviour. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder IndicatedSortColumn(string value)
    {
      level.IndicatedSortColumn = value;

      return this;
    }
    /// <summary>
    /// The direction ("ASC" or "DESC") to indicate as the sort direction by rendering the appropriate sort image in the column heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder IndicatedSortDirection(string value)
    {
      level.IndicatedSortDirection = value;

      return this;
    }
    /// <summary>
    /// The ID of the client template to use for the EditCommand column when inserting a new item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder InsertCommandClientTemplateId(string value)
    {
      level.InsertCommandClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to rows. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder RowCssClass(string value)
    {
      level.RowCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to selected rows. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SelectedRowCssClass(string value)
    {
      level.SelectedRowCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to the selector cells. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SelectorCellCssClass(string value)
    {
      level.SelectorCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The width (in pixels) of the selector cells. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SelectorCellWidth(int value)
    {
      level.SelectorCellWidth = value;

      return this;
    }
    /// <summary>
    /// The height (in pixels) of the selector image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SelectorImageHeight(int value)
    {
      level.SelectorImageHeight = value;

      return this;
    }
    /// <summary>
    /// The image to display in the selector cell when the row is selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SelectorImageUrl(string value)
    {
      level.SelectorImageUrl = value;

      return this;
    }
    /// <summary>
    /// The width (in pixels) of the selector image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SelectorImageWidth(int value)
    {
      level.SelectorImageWidth = value;

      return this;
    }
    /// <summary>
    /// Whether to display a footer row.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder ShowFooterRow(bool value)
    {
      level.ShowFooterRow = value;

      return this;
    }
    /// <summary>
    /// Whether to display heading cells of columns.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder ShowHeadingCells(bool value)
    {
      level.ShowHeadingCells = value;

      return this;
    }
    /// <summary>
    /// Whether to display the selector cells. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder ShowSelectorCells(bool value)
    {
      level.ShowSelectorCells = value;

      return this;
    }
    /// <summary>
    /// Whether to display the headings above groups with the same sort value when the data is sorted. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder ShowSortHeadings(bool value)
    {
      level.ShowSortHeadings = value;

      return this;
    }
    /// <summary>
    /// Whether to display the table heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder ShowTableHeading(bool value)
    {
      level.ShowTableHeading = value;

      return this;
    }

    /// <summary>
    /// The image to use for indicating the ascending sort direction in a column heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SortAscendingImageUrl(string value)
    {
      level.SortAscendingImageUrl = value;

      return this;
    }
    /// <summary>
    /// The image to use for indicating the descending sort direction in a column heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SortDescendingImageUrl(string value)
    {
      level.SortDescendingImageUrl = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to data cells in the column that is sorted by. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SortedDataCellCssClass(string value)
    {
      level.SortedDataCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to the heading of the column that is sorted by. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SortedHeadingCellCssClass(string value)
    {
      level.SortedHeadingCellCssClass = value;

      return this;
    }
    /// <summary>
    /// The ID of the client template to use for sort headings. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SortHeadingClientTemplateId(string value)
    {
      level.SortHeadingClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The CSS class to apply to sort headings. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SortHeadingCssClass(string value)
    {
      level.SortHeadingCssClass = value;

      return this;
    }
    /// <summary>
    /// The height (in pixels) of sort images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SortImageHeight(int value)
    {
      level.SortImageHeight = value;

      return this;
    }
    /// <summary>
    /// The width (in pixels) of sort images. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder SortImageWidth(int value)
    {
      level.SortImageWidth = value;

      return this;
    }
    /// <summary>
    /// The ID of the client tamplate to use for the table heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder TableHeadingClientTemplateId(string value)
    {
      level.TableHeadingClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The CSS class to use for the table heading. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder TableHeadingCssClass(string value)
    {
      level.TableHeadingCssClass = value;

      return this;
    }
    /// <summary>
    /// Name of the table in the associated datasource for this DataGridLevel.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridLevelBuilder TableName(string value)
    {
      level.TableName = value;

      return this;
    }

  }
}