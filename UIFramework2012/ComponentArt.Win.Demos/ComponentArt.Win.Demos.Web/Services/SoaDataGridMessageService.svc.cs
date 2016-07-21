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
      DataSet oDS = LoadData();

      // we use the built-in helper for this typical case
      return GroupFromDataTable(request, oDS.Tables[0]);
    }

    public override SoaDataGridSelectResponse Select(SoaDataGridSelectRequest request)
    {
      DataSet oDS = LoadData();

      // we use the built-in helper for this typical case
      return SelectFromDataTable(request, oDS.Tables[0]);
    }

    public override SoaDataGridUpdateResponse Update(SoaDataGridUpdateRequest request)
    {
      SoaDataGridUpdateResponse response = new SoaDataGridUpdateResponse();

      response.Message = "No updating done - this is just a demo.";
      response.Cancel = true;

      return response;
    }

    public override SoaDataGridDeleteResponse Delete(SoaDataGridDeleteRequest request)
    {
      SoaDataGridDeleteResponse response = new SoaDataGridDeleteResponse();

      response.Message = "No deleting done - this is just a demo.";
      response.Cancel = true;

      return response;
    }
  }
}
