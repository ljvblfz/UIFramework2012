using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;


namespace ComponentArt.Win.Demos.Web
{
  [ServiceContract]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class WCFProductService : IComponentArtProductService
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

    private DataSet LoadCategories()
    {
      string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
      conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
      System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
      dbCon.Open();

      string sql = "SELECT * FROM Categories";
      System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
      DataSet ds = new DataSet();
      dbAdapter.Fill(ds);
      ds.Tables[0].TableName = "Categories";

      dbCon.Close();

      return ds;
    }

    private DataSet LoadOrders(int iProductId)
    {
      string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
      conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
      System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
      dbCon.Open();

      string sql = "SELECT * FROM OrderDetails WHERE ProductId = " + iProductId;
      System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
      DataSet ds = new DataSet();
      dbAdapter.Fill(ds);
      ds.Tables[0].TableName = "Orders";

      dbCon.Close();

      return ds;
    }

    [OperationContract]
    public List<Category> GetCategories()
    {
      DataSet oDS = LoadCategories();
      DataView oView = oDS.Tables[0].DefaultView;

      List<Category> list = new List<Category>();

      int iStart = 0;
      int iEnd = oView.Count;

      for (int i = iStart; i < iEnd; i++)
      {
        Category o = new Category();

        o.Id = (int)oView[i]["CategoryId"];
        o.Name = (string)oView[i]["CategoryName"];
        o.Description = (string)oView[i]["Description"];

        list.Add(o);
      }

      return list;
    }

    [OperationContract]
    public List<Product> GetProducts(int? categoryId)
    {
      DataSet oDS = LoadProducts(categoryId);
      DataView oView = oDS.Tables[0].DefaultView;

      List<Product> list = new List<Product>();

      int iStart = 0;
      int iEnd = oView.Count;

      for (int i = iStart; i < iEnd; i++)
      {
        Product o = new Product();

        o.Id = Convert.ToInt32(oView[i]["ProductId"]);
        o.Name = (string)oView[i]["ProductName"];
        o.CategoryId = Convert.ToInt32(oView[i]["CategoryId"]);
        o.CategoryName = (string)oView[i]["CategoryName"];
        o.QuantityPerUnit = (string)oView[i]["QuantityPerUnit"];
        o.UnitPrice = Convert.ToDouble(oView[i]["UnitPrice"]);
        o.UnitsInStock = Convert.ToInt32(oView[i]["UnitsInStock"]);
        o.Discontinued = Convert.ToBoolean(oView[i]["Discontinued"]);

        list.Add(o);
      }

      return list;
    }

    [OperationContract]
    public List<Order> GetOrders(int productId)
    {
      DataSet oDS = LoadOrders(productId);
      DataView oView = oDS.Tables[0].DefaultView;

      List<Order> list = new List<Order>();

      int iStart = 0;
      int iEnd = oView.Count;

      for (int i = iStart; i < iEnd; i++)
      {
        Order o = new Order();

        o.Id = Convert.ToInt32(oView[i]["OrderId"]);
        o.Price = Convert.ToDouble(oView[i]["UnitPrice"]);
        o.Quantity = Convert.ToInt32(oView[i]["UnitPrice"]);
        o.Discount = Convert.ToDouble(oView[i]["Discount"]);

        list.Add(o);
      }

      return list;
    }
  }
}
