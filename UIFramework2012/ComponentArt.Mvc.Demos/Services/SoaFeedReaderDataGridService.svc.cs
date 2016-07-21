using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text;
using System.Web;
using ComponentArt.SOA.UI;
using System.Diagnostics;

  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class SoaFeedReaderDataGridService : SoaDataGridService
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
      conStr += HttpContext.Current.Server.MapPath("~/App_Data/common/db/demo.mdb");
      
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

      for (int i = request.Offset; i < request.Offset + request.Count && i < oView.Count; i++)
      {
        // load data row
        List<object> msg = new List<object>();
        foreach (SoaDataGridColumn oColumn in request.Columns)
        {
					if(oColumn.Name != "")
					{
					  msg.Add(oView[i][oColumn.Name]);
					}
					else
					{
					  msg.Add(null);
					}
        }

        arItems.Add(msg);
      }

      // Thread.Sleep simulates the experience of accessing a remote server 
      // Remove before going to production 
      System.Threading.Thread.Sleep(500);

      response.ItemCount = oView.Count;
      response.Data = arItems;

      return response;
    }
  }
