using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using ComponentArt.SOA.UI;


[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class SoaDataGridMessageService : SoaDataGridService
{
  private DataSet LoadData()
  {
    string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
    conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
    System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
    dbCon.Open();

    string sql = "SELECT * FROM Posts ORDER BY LastPostDate DESC";
    System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
    DataSet ds = new DataSet();
    dbAdapter.Fill(ds);

    dbCon.Close();

    return ds;
  }

  public override SoaDataGridGroupResponse Group(SoaDataGridGroupRequest request)
  {
    List<object> list = new List<object>();

    DataSet oDS = LoadData();
    DataView oDataView = new DataView(oDS.Tables[0]);

    oDataView.Sort = request.Groupings.ToSqlString();
    oDataView.RowFilter = request.Filters.ToSqlString();

    string field = request.Groupings[0].Column.Name;

    SoaDataGridGroupResponse response = new SoaDataGridGroupResponse();

    int iGroupCount = 0;
    object _lastValue = Guid.NewGuid();
    for (int i = 0; i < oDataView.Count; i++)
    {
      if ((_lastValue == null && oDataView[i][field] != null) ||
        _lastValue != null && !_lastValue.Equals(oDataView[i][field]))
      {
        _lastValue = oDataView[i][field];

        if (iGroupCount >= request.Offset && iGroupCount < request.Offset + request.Count)
        {
          SoaDataGridGroup oGroup = new SoaDataGridGroup();
          oGroup.Column = field;
          oGroup.GroupValue = _lastValue;

          response.Groups.Add(oGroup);

          list.Add(_lastValue);
        }

        iGroupCount++;
      }
    }

    response.GroupCount = iGroupCount;

    return response;
  }

  public override SoaDataGridSelectResponse Select(SoaDataGridSelectRequest request)
  {
    DataSet oDS = LoadData();
    DataView oView = oDS.Tables[0].DefaultView;

    oView.Sort = request.Sortings.ToSqlString();
    oView.RowFilter = request.Filters.ToSqlString();

    SoaDataGridSelectResponse response = new SoaDataGridSelectResponse();

    List<List<object>> arMessages = new List<List<object>>();

    for (int i = request.Offset; i < request.Offset + request.Count && i < oView.Count; i++)
    {
      // load data row
      List<object> msg = new List<object>();
      foreach (SoaDataGridColumn oColumn in request.Columns)
      {
        msg.Add(oView[i][oColumn.Name]);
      }

      arMessages.Add(msg);
    }

    response.ItemCount = oView.Count;
    response.Data = arMessages;

    return response;
  }
}

