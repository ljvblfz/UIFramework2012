using System;
using System.Collections;
using System.Text;

namespace ComponentArt.Web.UI
{
  #region WebService classes

  /// <summary>
  /// Contains information describing a web service select request.
  /// </summary>
  /// <seealso cref="Grid.RunningMode" />
  /// <seealso cref="Grid.WebService" />
  public class GridWebServiceSelectRequest
  {
    /// <summary>
    /// The fields which are requested by the client.
    /// </summary>
    public string[] Columns;

    /// <summary>
    /// Current page size.
    /// </summary>
    public int PageSize;

    /// <summary>
    /// Current page index.
    /// </summary>
    public int CurrentPageIndex;

    /// <summary>
    /// Record offset (used for scrolling).
    /// </summary>
    public int RecordOffset;

    /// <summary>
    /// The name of the field to sort by, if any.
    /// </summary>
    public string SortField;

    /// <summary>
    /// The sort order, if any (DESC or ASC).
    /// </summary>
    public string SortOrder;

    /// <summary>
    /// The filter string to apply, if any.
    /// </summary>
    public string Filter;

    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains information describing a web service update request.
  /// </summary>
  /// <seealso cref="Grid.RunningMode" />
  /// <seealso cref="Grid.WebService" />
  public class GridWebServiceUpdateRequest
  {
    /// <summary>
    /// Data key value.
    /// </summary>
    public object Key;

    /// <summary>
    /// Updated data row values.
    /// </summary>
    public object[] Values;

    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains information describing a web service config request.
  /// </summary>
  /// <seealso cref="Grid.RunningMode" />
  /// <seealso cref="Grid.WebService" />
  public class GridWebServiceConfigRequest
  {
    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains information describing a web service group request.
  /// </summary>
  /// <seealso cref="Grid.RunningMode" />
  /// <seealso cref="Grid.WebService" />
  public class GridWebServiceGroupRequest
  {
    /// <summary>
    /// The name of the column (data field) to group by.
    /// </summary>
    public string Column;

    /// <summary>
    /// The index of the page of groups to return.
    /// </summary>
    public int CurrentPageIndex;

    /// <summary>
    /// Whether to group in descending order.
    /// </summary>
    public bool Descending;

    /// <summary>
    /// The number of groups to return.
    /// </summary>
    public int PageSize;

    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains information describing a web service insert request.
  /// </summary>
  /// <seealso cref="Grid.RunningMode" />
  /// <seealso cref="Grid.WebService" />
  public class GridWebServiceInsertRequest
  {
    public object[] Values;

    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains information describing a web service delete request.
  /// </summary>
  /// <seealso cref="Grid.RunningMode" />
  /// <seealso cref="Grid.WebService" />
  public class GridWebServiceDeleteRequest
  {
    public object[] Values;

    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains data to be sent back in response to a web service select request.
  /// </summary>
  public class GridWebServiceSelectResponse
  {
    /// <summary>
    /// The total number of records in the data set.
    /// </summary>
    public int RecordCount;

    /// <summary>
    /// The items matching the web service select request.
    /// </summary>
    /// <remarks>
    /// The items contained here should only be the ones matching the requested page index,
    /// sort order, filter string, etc.
    /// </remarks>
    public IList Items;

    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains information describing a web service config response.
  /// </summary>
  /// <seealso cref="Grid.RunningMode" />
  /// <seealso cref="Grid.WebService" />
  public class GridWebServiceConfigResponse
  {
    public GridWebServiceConfigResponse()
    {
      Properties = new Hashtable();
      Levels = new ArrayList();
    }

    /// <summary>
    /// Top-level property settings.
    /// </summary>
    public Hashtable Properties;

    /// <summary>
    /// Levels definitions.
    /// </summary>
    public ArrayList Levels;

    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  /// <summary>
  /// Contains information describing a level returned as part of a GridWebServiceConfigResponse.
  /// </summary>
  /// <seealso cref="GridWebServiceConfigResponse" />
  public class GridWebServiceLevelInfo
  {
    public GridWebServiceLevelInfo()
    {
      Properties = new Hashtable();
      Columns = new ArrayList();
    }

    /// <summary>
    /// Level property settings.
    /// </summary>
    public Hashtable Properties;

    /// <summary>
    /// The items to be loaded into this group.
    /// </summary>
    public ArrayList Columns;
  }

  /// <summary>
  /// Contains information describing a column returned as part of a GridWebServiceConfigResponse.
  /// </summary>
  /// <seealso cref="GridWebServiceConfigResponse" />
  /// <seealso cref="GridWebServiceLevelInfo" />
  public class GridWebServiceColumnInfo
  {
    public GridWebServiceColumnInfo()
    {
      Properties = new Hashtable();
    }

    /// <summary>
    /// Column property settings.
    /// </summary>
    public Hashtable Properties;
  }

  /// <summary>
  /// Contains information describing a group returned as part of a GridWebServiceGroupResponse.
  /// </summary>
  /// <seealso cref="GridWebServiceGroupResponse" />
  public class GridWebServiceGroupInfo
  {
    public GridWebServiceGroupInfo()
    {
      Items = new ArrayList();
      Groups = new ArrayList();
    }

    /// <summary>
    /// The name of the group-by column (data field) for this group.
    /// </summary>
    public string Column;

    /// <summary>
    /// The value under the group-by column for this group.
    /// </summary>
    public object GroupValue;

    /// <summary>
    /// The sub-groups to be loaded into this group.
    /// </summary>
    /// <remarks>
    /// If this is non-empty, Items will be ignored.
    /// </remarks>
    public ArrayList Groups;

    /// <summary>
    /// The items to be loaded into this group.
    /// </summary>
    /// <remarks>
    /// This will only be considered if Groups is left empty.
    /// </remarks>
    public ArrayList Items;

    /// <summary>
    /// Whether to display this group as pre-expanded.
    /// </summary>
    public bool Expanded;
  }

  /// <summary>
  /// Contains information describing a web service group response.
  /// </summary>
  /// <seealso cref="Grid.RunningMode" />
  /// <seealso cref="Grid.WebService" />
  /// <seealso cref="GridWebServiceGroupInfo" />
  public class GridWebServiceGroupResponse
  {
    public GridWebServiceGroupResponse()
    {
      Groups = new ArrayList();
    }

    /// <summary>
    /// Groups.
    /// </summary>
    public ArrayList Groups;

    /// <summary>
    /// The total number of groups available.
    /// </summary>
    public int GroupCount;

    /// <summary>
    /// Optional custom parameter.
    /// </summary>
    public string CustomParameter;
  }

  #endregion

}
