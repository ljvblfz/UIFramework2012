using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using ComponentArt.Mvc.Demos.Models;
using System.Data;
using ComponentArt.Web.UI;

namespace ComponentArt.Mvc.Demos.Controllers
{
  public partial class ComboBoxController : Controller
  {
    private DataSet GetDemoData(string sql)
    {
      string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
      conStr += Server.MapPath("~/App_Data/common/db/demo.mdb");
      System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
      dbCon.Open();

      System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
      DataSet ds = new DataSet();
      dbAdapter.Fill(ds);

      dbCon.Close();

      return ds;
    }

    private int GetDemoRecordCount(string sql)
    {
      string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
      conStr += Server.MapPath("~/App_Data/common/db/demo.mdb");
      System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr); dbCon.Open();
      System.Data.OleDb.OleDbCommand dbCmd = new System.Data.OleDb.OleDbCommand(sql, dbCon);

      int iCount = (int)dbCmd.ExecuteScalar();

      return iCount;
    }

    private DataSet GetCountries()
    {
      string sql = "SELECT * FROM Countries";
      DataSet ds = GetDemoData(sql);

      return ds;
    }

    private ComboBoxActionResponse GetCountryItems()
    {
      string sql = "SELECT * FROM Countries";
      DataSet ds = GetDemoData(sql);

      ComboBoxActionResponse response = new ComboBoxActionResponse();
      response.Data = ds;
      response.ItemCount = ds.Tables[0].Rows.Count;

      return response;
    }

    private ComboBoxActionResponse GetCountryItems(ComboBoxActionRequest actionRequest)
    {
      string orderBy = "ORDER BY CountryName ASC";
      string orderByReverse = "ORDER BY CountryName DESC";

      int skipReverse = actionRequest.Skip + actionRequest.Take;
      
      string sqlWhere = "WHERE 1=1 ";
      if (actionRequest.Filter != null && actionRequest.Filter != "")
      {
        sqlWhere += "AND CountryName LIKE '" + actionRequest.Filter + "%' ";
      }

      string sql = "SELECT * FROM ( ";
      sql += " SELECT TOP " + actionRequest.Take + " * FROM (";
      sql += " SELECT TOP " + skipReverse + " * FROM Countries " + sqlWhere + orderBy;
      sql += ") AS innerresults " + orderByReverse;
      sql += ") AS outerresults " + orderBy;

      DataSet ds = GetDemoData(sql);

      List<Country> countries = new List<Country>();
      Country country;
      foreach (DataRow dataRow in ds.Tables[0].Rows)
      {
        country = new Country();
        country.CountryCode = dataRow["CountryCode"].ToString();
        country.CountryName = dataRow["CountryName"].ToString();

        countries.Add(country);
      }

      int recordCount = GetDemoRecordCount("SELECT COUNT(*) FROM Countries " + sqlWhere);
      
      ComboBoxActionResponse response = new ComboBoxActionResponse();
      response.Data = countries;
      response.ItemCount = recordCount;

      return response;
    }


    #region Handle the simple demos

    public ActionResult CoreFeatures()
    {
      return View(GetCountryItems());
    }

    public ActionResult ThemeSwitching()
    {
      return View(GetCountryItems());
    }
    #endregion
  }

}
