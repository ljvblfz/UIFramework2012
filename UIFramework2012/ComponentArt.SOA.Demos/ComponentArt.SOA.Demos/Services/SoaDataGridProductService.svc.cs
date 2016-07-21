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


[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class SoaDataGridProductService : SoaDataGridService
{
  private DataSet LoadProducts(int? iCategoryId)
  {
    string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
    conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
    System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
    dbCon.Open();

    string sql = "SELECT Products.*,Categories.CategoryName AS CategoryName FROM Products INNER JOIN Categories ON Products.CategoryId = Categories.CategoryId" + (iCategoryId.HasValue ? " WHERE Products.CategoryId = " + iCategoryId : "");
    System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
    DataSet ds = new DataSet();
    dbAdapter.Fill(ds);
    ds.Tables[0].TableName = "Products";

    dbCon.Close();

    return ds;
  }

  public override SoaDataGridSelectResponse Select(SoaDataGridSelectRequest request)
  {
    DataSet oDS = LoadProducts(null);
    DataView oView = oDS.Tables[0].DefaultView;

    oView.Sort = request.Sortings.ToSqlString();
    oView.RowFilter = request.Filters.ToSqlString();

    SoaDataGridSelectResponse response = new SoaDataGridSelectResponse();

    List<List<object>> arProducts = new List<List<object>>();

    for (int i = request.Offset; i < request.Offset + request.Count && i < oView.Count; i++)
    {
      // load data row
      List<object> msg = new List<object>();
      foreach (SoaDataGridColumn oColumn in request.Columns)
      {
        if (oView.Table.Columns.Contains(oColumn.Name))
        {
          msg.Add(oView[i][oColumn.Name]);
        }
        else
        {
          msg.Add(null);
        }
      }

      arProducts.Add(msg);
    }

    response.ItemCount = oView.Count;
    response.Data = arProducts;

    return response;
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

  public override SoaDataGridInsertResponse Insert(SoaDataGridInsertRequest request)
  {
    SoaDataGridInsertResponse response = new SoaDataGridInsertResponse();

    response.Message = "No inserting done - this is just a demo.";
    response.Cancel = true;

    return response;
  }
}
