using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.Collections;

namespace ComponentArt.Silverlight.Demos.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ManualComboBoxLocationsService
    {
        protected DataTable LocationsTable;

        [OperationContract]
        public List<string> GetMatchesForArgument(string toMatch, string matchMode, int iStartIndex, int iNumItems)
        {
            List<string> matches = new List<string>();

            // Safety check
            if (toMatch.Length > 12 || toMatch.IndexOf(";") >= 0) return null;

            LoadLocations();

            ArrayList arRows = toMatch.Length > 0 ? new ArrayList(LocationsTable.Select("LocationName LIKE '" + toMatch + "%'")) : new ArrayList(LocationsTable.Rows);

            int iEndIndex = Math.Min(iStartIndex + iNumItems, arRows.Count);

            for (int i = iStartIndex; i < iEndIndex && i < arRows.Count; i++)
            {
                DataRow oRow = (DataRow)arRows[i];
                matches.Add(oRow["LocationName"].ToString());
            }

            return matches;
        }

        private void LoadLocations()
        {
            if (LocationsTable == null)
            {
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
                conStr += HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);

                System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
                dbCon.Open();

                string sql = "SELECT LocationName FROM Locations ORDER BY LocationName";

                System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);

                DataTable dt = new DataTable();
                dbAdapter.Fill(dt);

                LocationsTable = dt;
            }
        }
    }
}
