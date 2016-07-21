using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ServiceModel.Web;

using ComponentArt.SOA.UI;
using System.Web;

namespace ComponentArt.Win.Demos.Web
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SoaFeedReaderTreeViewService : SoaTreeViewService
    {
        public override SoaTreeViewGetNodesResponse GetNodes(SoaTreeViewGetNodesRequest request)
        {
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
            System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
            dbCon.Open();

            const string conIconBase = "/ComponentArt.Win.Demos;component/Assets/Pages/FeedBrowser/TreeView/";

            System.Data.OleDb.OleDbDataAdapter dbAdapter;
            string sql;


            SoaTreeViewGetNodesResponse response = new SoaTreeViewGetNodesResponse();
            response.Nodes = new List<SoaTreeViewNode>();

            // feed query
            sql = "SELECT FeedName FROM Feed WHERE IsIncludedInBrowser = True";
            if (request.ParentNode != null && request.ParentNode.Tag != null && request.ParentNode.Tag.ToString() != string.Empty)
            {
                sql += " And CategoryId = " + request.ParentNode.Tag;
            }
            else
            {
                sql += " And CategoryId IS NULL";
            }
            sql += " ORDER BY Priority DESC";

            dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
            DataTable oFeedTable = new DataTable();
            dbAdapter.Fill(oFeedTable);

            // populate response with feeds
            for (int i = 0; i < oFeedTable.DefaultView.Count; i++)
            {
                SoaTreeViewNode oNode = new SoaTreeViewNode();

                oNode.Text = Convert.ToString(oFeedTable.DefaultView[i][0]);
                oNode.IconSource = conIconBase + "leaf.png";
                oNode.IsLoadOnDemandEnabled = false;
                oNode.Id = "Feed";

                response.Nodes.Add(oNode);
            }



            // category query
            sql = "SELECT CategoryName, CategoryId FROM FeedCategory WHERE ";
            if (request.ParentNode != null && request.ParentNode.Tag != null && request.ParentNode.Tag.ToString() != string.Empty)
            {
                sql += "ParentCategoryId = " + request.ParentNode.Tag.ToString();
            }
            else
            {
                sql += "ParentCategoryId IS NULL";
            }
            sql += " ORDER BY CategoryId ASC";

            dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
            DataTable oCategoryTable = new DataTable();
            dbAdapter.Fill(oCategoryTable);

            // populate response with categories
            for (int i = 0; i < oCategoryTable.DefaultView.Count; i++)
            {
                SoaTreeViewNode oNode = new SoaTreeViewNode();

                oNode.Text = Convert.ToString(oCategoryTable.DefaultView[i][0]);
                oNode.Tag = Convert.ToInt32(oCategoryTable.DefaultView[i][1]);
                oNode.IconSource = conIconBase + "folder-closed.png";
                oNode.ExpandedIconSource = conIconBase + "folder-open.png";
                oNode.IsLoadOnDemandEnabled = true;
                oNode.Id = "Category";

                response.Nodes.Add(oNode);
            }
            
            dbCon.Close();
            return response;
        }
    }
}
