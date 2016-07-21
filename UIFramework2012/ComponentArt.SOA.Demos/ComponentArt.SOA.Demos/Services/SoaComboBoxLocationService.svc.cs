using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using ComponentArt.SOA.UI;
using System.Web;
using System.Data;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class SoaComboBoxLocationService : SoaComboBoxService
{
  private DataSet LoadData()
  {
    string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
    conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
    System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
    dbCon.Open();

    string sql = "SELECT * FROM Locations";
    System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
    DataSet ds = new DataSet();
    dbAdapter.Fill(ds);

    dbCon.Close();

    return ds;
  }

  public override SoaComboBoxSelectResponse Select(SoaComboBoxSelectRequest request)
  {
    DataSet oDS = LoadData();
    DataView oView = oDS.Tables[0].DefaultView;

    oView.RowFilter = request.Filters.ToSqlString();

    SoaComboBoxSelectResponse response = new SoaComboBoxSelectResponse();

    List<Dictionary<string, object>> arMessages = new List<Dictionary<string, object>>();

    for (int i = request.Offset; i < request.Offset + request.Count && i < oView.Count; i++)
    {
      // load data row
      Dictionary<string, object> msg = new Dictionary<string, object>();
      foreach (SoaComboBoxColumn oColumn in request.Columns)
      {
        msg.Add(oColumn.Name, oView[i][oColumn.Name]);
      }

      arMessages.Add(msg);
    }

    response.ItemCount = oView.Count;
    response.Data = arMessages;

    return response;
  }

  public override SoaComboBoxInsertResponse Insert(SoaComboBoxInsertRequest request)
  {
    return null;
  }
}

