using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using ComponentArt.SOA.UI;


namespace ComponentArt.Win.Demos.Web
{
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class SoaDataGridFeedService : SoaDataGridService
  {
    private DataSet LoadData()
    {
      if (HttpContext.Current.Application.AllKeys.Contains("SoaDataGridFeedService_FeedItem"))
      {
        DataSet cachedSet = HttpContext.Current.Application["SoaDataGridFeedService_FeedItem"] as DataSet;

        if (cachedSet != null)
        {
          return cachedSet;
        }
      }

      string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
      conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
      System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
      dbCon.Open();

      string sql = "SELECT * FROM FeedItem";
      System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
      DataSet ds = new DataSet();
      dbAdapter.Fill(ds);

      dbCon.Close();

      HttpContext.Current.Application["SoaDataGridFeedService_FeedItem"] = ds;

      return ds;
    }

    public override SoaDataGridSelectResponse Select(SoaDataGridSelectRequest request)
    {
      DataSet oDS = LoadData();
      DataView oView = new DataView(oDS.Tables[0]);

      oView.Sort = request.Sortings.ToSqlString();
      oView.RowFilter = request.Filters.ToSqlString();

      SoaDataGridSelectResponse response = new SoaDataGridSelectResponse();

      List<List<object>> arItems = new List<List<object>>();

      int iOffset = (string)request.Tag == "LoadEverything" ? 0 : request.Offset;
      int iCount = (string)request.Tag == "LoadEverything" ? int.MaxValue : request.Count;

      for (int i = iOffset; i < iOffset + iCount && i < oView.Count; i++)
      {
        // load data row
        List<object> msg = new List<object>();
        foreach (SoaDataGridColumn oColumn in request.Columns)
        {
          msg.Add(oView[i][oColumn.Name]);
        }

        arItems.Add(msg);
      }

      response.ItemCount = oView.Count;
      response.Data = arItems;

      return response;
    }
  }
}
