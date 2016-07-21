using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
// using System.ServiceModel.Web;


namespace ComponentArt.Silverlight.Demos.Web
{
  [ServiceContract]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class WCFCarService : IComponentArtCarService
  {
    private DataSet LoadModels(int? iCategoryId)
    {
      string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
      conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
      System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
      dbCon.Open();

      string sql = "SELECT * FROM Car" + (iCategoryId.HasValue ? " WHERE CategoryId = " + iCategoryId : "");
      System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
      DataSet ds = new DataSet();
      dbAdapter.Fill(ds);
      ds.Tables[0].TableName = "Models";

      dbCon.Close();

      return ds;
    }

    private DataSet LoadCategories()
    {
      string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
      conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
      System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
      dbCon.Open();

      string sql = "SELECT * FROM CarCategory";
      System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
      DataSet ds = new DataSet();
      dbAdapter.Fill(ds);
      ds.Tables[0].TableName = "Categories";

      dbCon.Close();

      return ds;
    }

    private DataSet LoadSales(int iModelId)
    {
      string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
      conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
      System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
      dbCon.Open();

      string sql = "SELECT * FROM CarSale WHERE ModelId = " + iModelId;
      System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
      DataSet ds = new DataSet();
      dbAdapter.Fill(ds);
      ds.Tables[0].TableName = "Sales";

      dbCon.Close();

      return ds;
    }

    [OperationContract]
    public List<CarCategory> GetCategories()
    {
      DataSet oDS = LoadCategories();
      DataView oView = oDS.Tables[0].DefaultView;

      List<CarCategory> list = new List<CarCategory>();

      int iStart = 0;
      int iEnd = oView.Count;

      for (int i = iStart; i < iEnd; i++)
      {
        CarCategory o = new CarCategory();

        o.Id = (int)oView[i]["Id"];
        o.Name = (string)oView[i]["Name"];
        o.Description = (string)oView[i]["Description"];

        list.Add(o);
      }

      return list;
    }

    [OperationContract]
    public List<CarModel> GetModels(int? categoryId)
    {
      DataSet oDS = LoadModels(categoryId);
      DataView oView = oDS.Tables[0].DefaultView;

      List<CarModel> list = new List<CarModel>();

      int iStart = 0;
      int iEnd = oView.Count;

      for (int i = iStart; i < iEnd; i++)
      {
        CarModel o = new CarModel();

        o.Id = Convert.ToInt32(oView[i]["Id"]);
        o.Model = (string)oView[i]["Model"];
        o.Year = Convert.ToInt32(oView[i]["Year"]);
        o.CategoryId = Convert.ToInt32(oView[i]["CategoryId"]);
        o.ListPrice = Convert.ToDouble(oView[i]["ListPrice"]);
        o.UnitsInStock = Convert.ToInt32(oView[i]["UnitsInStock"]);
        o.UnitsOnOrder = Convert.ToInt32(oView[i]["UnitsOnOrder"]);
        o.LastOrderedOn = Convert.ToDateTime(oView[i]["LastOrdered"]);

        list.Add(o);
      }

      return list;
    }

    [OperationContract]
    public List<CarSale> GetSales(int productId)
    {
      DataSet oDS = LoadSales(productId);
      DataView oView = oDS.Tables[0].DefaultView;

      List<CarSale> list = new List<CarSale>();

      int iStart = 0;
      int iEnd = oView.Count;

      for (int i = iStart; i < iEnd; i++)
      {
        CarSale o = new CarSale();

        o.Id = Convert.ToInt32(oView[i]["Id"]);
        o.Price = Convert.ToDouble(oView[i]["Price"]);
        o.SalesPerson = (string)oView[i]["SalesPerson"];
        o.SerialNumber = (string)oView[i]["SerialNumber"];
        o.Color = (string)oView[i]["Color"];
        o.DateOfSale = Convert.ToDateTime(oView[i]["DateOfSale"]);

        list.Add(o);
      }

      return list;
    }
  }
}
