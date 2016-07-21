using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using System.Data;
using ComponentArt.Web.UI;

namespace ComponentArt.Mvc.Demos.Controllers
{
  public class DesignController : Controller
  {
    public ActionResult Themes()
    {
      return View(GetPostItems());
    }

    #region DataGrid binding

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

    private DataGridActionResponse GetPostItems()
    {
      string sql = "SELECT * FROM Posts";
      DataSet ds = GetDemoData(sql);

      DataGridActionResponse response = new DataGridActionResponse();
      response.Data = ds;
      response.RecordCount = ds.Tables[0].Rows.Count;

      return response;
    }

    #endregion
  }
}