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
  public class WCFMessageService : IComponentArtMessageService
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

    private const string IMG_PATH = "../ClientBin/controls/datagrid/";
    private string ServerPath
    {
      get
      {
        return new Uri(HttpContext.Current.Request.Url, IMG_PATH).AbsoluteUri; 
      }
    }

    [OperationContract]
    public List<Message> GetRecords(int pageSize, int pageIndex, string sort)
    {
      DataSet oDS = LoadData();
      DataView oView = oDS.Tables[0].DefaultView;

      oView.Sort = sort;

      List<Message> list = new List<Message>();

      int iStart = pageSize * pageIndex;
      int iEnd = Math.Min(oView.Count, (pageIndex + 1) * pageSize);

      for (int i = iStart; i < iEnd; i++)
      {
        Message msg = new Message();

        msg.Subject = (string)oView[i]["Subject"];
        msg.LastPostDate = (DateTime)oView[i]["LastPostDate"];
        msg.Replies = (int)oView[i]["Replies"];
        msg.TotalViews = (int)oView[i]["TotalViews"];
        msg.StartedBy = (string)oView[i]["StartedBy"];
        msg.EmailIcon = ServerPath + oView[i]["EmailIcon"];
        msg.LargeEmailIcon = ServerPath + oView[i]["EmailIconLarge"];
        msg.FlagIcon = ServerPath + oView[i]["FlagIcon"];
        msg.PriorityIcon = ServerPath + oView[i]["PriorityIcon"];
        msg.AttachmentIcon = ServerPath + oView[i]["AttachmentIcon"];

        list.Add(msg);
      }

      return list;
    }
  }
}
