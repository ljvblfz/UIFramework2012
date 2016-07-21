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
    public partial class DataGridController : Controller
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

      private DataSet GetFeedItems()
      {
        string sql = "SELECT * FROM FeedItem";
        DataSet ds = GetDemoData(sql);

        return ds;
      }

      private DataSet GetFeedItems(int numRecords)
      {
        string sql = "SELECT TOP " + numRecords + " * FROM FeedItem ORDER BY DatePosted DESC, Title ASC";
        DataSet ds = GetDemoData(sql);

        return ds;
      }

      private DataGridActionResponse GetFeedItems(DataGridActionRequest actionRequest)
      {
        string orderBy = "ORDER BY DatePosted ASC"; // Default  

        // Grouping order has precedence
        if (actionRequest.GroupOrder != null && actionRequest.GroupOrder != "")
        {
          orderBy = "ORDER BY " + actionRequest.GroupOrder.ToUpper();
          if (actionRequest.Order != null && actionRequest.Order != "")
          {
            orderBy += ", " + actionRequest.Order.ToUpper();
          }
        }
        else
        {
          if (actionRequest.Order != null && actionRequest.Order != "")
          {
            orderBy = "ORDER BY " + actionRequest.Order.ToUpper();
          }
        }
        if (!orderBy.Contains("DatePosted"))
        {
          orderBy += ", DatePosted ASC";
        }

        string orderByReverse = orderBy.Replace(" DESC", " ___ASC").Replace(" ASC", " ___DESC").Replace("___", "");
        int skipReverse = actionRequest.Skip + actionRequest.Take;

        string sql = "SELECT * FROM ( ";
        sql += " SELECT TOP " + actionRequest.Take + " * FROM (";
        sql += " SELECT TOP " + skipReverse + " * FROM FeedItem " + orderBy;
        sql += ") AS innerresults " + orderByReverse;
        sql += ") AS outerresults " + orderBy;

        DataSet ds = GetDemoData(sql);

        List<Feed> Feeds = new List<Feed>();
        Feed Feed;
        foreach (DataRow dataRow in ds.Tables[0].Rows)
        {
          Feed = new Feed();
          Feed.DatePosted = DateTime.Parse(dataRow["DatePosted"].ToString());
          Feed.FeedName = dataRow["FeedName"].ToString();
          Feed.ItemURL = dataRow["ItemURL"].ToString();
          Feed.Rating = Int32.Parse(dataRow["Rating"].ToString());
          Feed.Title = dataRow["Title"].ToString();
          Feed.Visits = Int32.Parse(dataRow["Visits"].ToString());

          Feeds.Add(Feed);
        }

        int recordCount = GetDemoRecordCount("SELECT COUNT(*) FROM FeedItem");

        DataGridActionResponse response = new DataGridActionResponse();
        response.Data = Feeds;
        response.RecordCount = recordCount;
        response.PageSize = actionRequest.Take;
        response.CurrentPageIndex = actionRequest.Skip / (actionRequest.Take == 0 ? 1 : actionRequest.Take);
        response.Sort = actionRequest.Order;
        response.GroupBy = actionRequest.GroupOrder;

        return response;
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

      private DataGridActionResponse GetPostItems(DataGridActionRequest actionRequest)
      {
        string orderBy = "ORDER BY PostId ASC";
        // Grouping order has precedence
        if (actionRequest.GroupOrder != null && actionRequest.GroupOrder != "")
        {
          orderBy = "ORDER BY " + actionRequest.GroupOrder.ToUpper();
          if (actionRequest.Order != null && actionRequest.Order != "")
          {
            orderBy += ", " + actionRequest.Order.ToUpper();
          }
        }
        else
        {
          if (actionRequest.Order != null && actionRequest.Order != "")
          {
            orderBy = "ORDER BY " + actionRequest.Order.ToUpper();
          }
        }
        if (!orderBy.Contains("PostId"))
        {
          orderBy += ", PostId ASC";
        }

        string orderByReverse = orderBy.Replace(" DESC", " ___ASC").Replace(" ASC", " ___DESC").Replace("___", "");
        int skipReverse = actionRequest.Skip + actionRequest.Take;

        string sql = "SELECT * FROM ( ";
        sql += " SELECT TOP " + actionRequest.Take + " * FROM (";
        sql += " SELECT TOP " + skipReverse + " * FROM Posts " + orderBy;
        sql += ") AS innerresults " + orderByReverse;
        sql += ") AS outerresults " + orderBy;

         DataSet ds = GetDemoData(sql);

        List<Post> posts = new List<Post>();
        Post post;
        foreach (DataRow dataRow in ds.Tables[0].Rows)
        {
          post = new Post();
          post.PostId = Int32.Parse(dataRow["PostId"].ToString());
          post.Subject = dataRow["Subject"].ToString();
          post.StartedBy = dataRow["StartedBy"].ToString();
          post.AttachmentIcon = dataRow["AttachmentIcon"].ToString();
          post.EmailIcon = dataRow["EmailIcon"].ToString();
          post.FlagIcon = dataRow["FlagIcon"].ToString();
          post.Icon = dataRow["Icon"].ToString();
          post.LastPostDate = Convert.ToDateTime(dataRow["LastPostDate"]);
          post.PriorityIcon = dataRow["PriorityIcon"].ToString();
          post.TotalViews = int.Parse(dataRow["TotalViews"].ToString());

          posts.Add(post);
        }

        int recordCount = GetDemoRecordCount("SELECT COUNT(*) FROM Posts");

        DataGridActionResponse response = new DataGridActionResponse();
        response.Data = posts;
        response.RecordCount = recordCount;
        response.PageSize = actionRequest.Take;
        response.CurrentPageIndex = actionRequest.Skip / (actionRequest.Take == 0 ? 1 : actionRequest.Take);
        response.Sort = actionRequest.Order;
        response.GroupBy = actionRequest.GroupOrder;

        return response;
      }


      #region Handle the simple demos

      public ActionResult ClientRunningMode()
      {
        return View(GetFeedItems());
      }

      public ActionResult CoreFeatures()
      {
        return View(GetFeedItems(350));
      }

      public ActionResult ServerRunningMode()
      {
        return View(GetFeedItems());
      }

      public ActionResult SliderClient()
      {
        return View(GetPostItems());
      }

      public ActionResult ScrollerClient()
      {
        return View(GetPostItems());
      }

      public ActionResult ScrollerLive()
      {
        return View(GetPostItems());
      }

      public ActionResult BasicPager()
      {
        return View(GetPostItems());
      }

      public ActionResult PagerButtons()
      {
        return View(GetPostItems());
      }

      #endregion
    }
}
