using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Data;
using ComponentArt.Web.UI;
using ComponentArt.Mvc.Demos.Models;

namespace ComponentArt.Mvc.Demos.Controllers
{
  public partial class CalendarController : Controller
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

    private CalendarActionResponse GetPostResponse(CalendarActionRequest actionRequest)
    {
      Post post = new Post();

      // Note that this demo uses a pre-chosen Post object
      string sql = "SELECT * FROM Posts WHERE PostId = 1030";
      DataSet ds = GetDemoData(sql);

      if (ds.Tables[0].Rows.Count > 0)
      {
        DataRow dataRow = ds.Tables[0].Rows[0];

        post.LastPostDate = DateTime.Parse(dataRow["LastPostDate"].ToString());
      }

      CalendarActionResponse actionResponse = new CalendarActionResponse();
      if (actionRequest != null)
      {
        actionResponse.SelectedDate = (actionRequest.SelectedDate != DateTime.MinValue) ? actionRequest.SelectedDate : post.LastPostDate;
        actionResponse.VisibleDate = (actionRequest.VisibleDate != DateTime.MinValue) ? actionRequest.VisibleDate : post.LastPostDate;
      }

      return actionResponse;
    }

    #region Handle the simple demos
    public ActionResult DateRangePicker()
    {
      return View();
    }
    public ActionResult MultiMonthCalendar()
    {
      return View();
    }
    public ActionResult ThemeSwitching()
    {
      return View();
    }
    #endregion
  }
}
