using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;

namespace ComponentArt.Win.Demos.Web
{

    public static class DemoExtensions
    {
        public static string ToOleDbString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }

    [DataContract]
    public enum TimePeriod
    {
        [EnumMember]
        Decade,
        [EnumMember]
        Year,
        [EnumMember]
        Month,
        [EnumMember]
        Day
    }

    [DataContract]
    public class DateRange
    {
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }

    [DataContract]
    public class SalesRecord
    {
        public SalesRecord(DateTime date, double sales, double downloads, double inquiries, double accounts)
        {
            Date = date;
            Sales = sales;
            Downloads = downloads;
            Inquiries = inquiries;
            Accounts = accounts;
        }

        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public double Sales { get; set; }
        [DataMember]
        public double Downloads { get; set; }
        [DataMember]
        public double Inquiries { get; set; }
        [DataMember]
        public double Accounts { get; set; }
    }

    [DataContract]
    public class StringSalesRecord
    {
        public StringSalesRecord(String date, double sales, double downloads, double inquiries, double accounts)
        {
            Date = date;
            Sales = sales;
            Downloads = downloads;
            Inquiries = inquiries;
            Accounts = accounts;
        }

        [DataMember]
        public String Date { get; set; }
        [DataMember]
        public double Sales { get; set; }
        [DataMember]
        public double Downloads { get; set; }
        [DataMember]
        public double Inquiries { get; set; }
        [DataMember]
        public double Accounts { get; set; }
    }

    [DataContract]
    public class ProductSummary
    {
        public ProductSummary(string productID, double downloads, double inquiries, double sales)
        {
            ProductID = productID;
            Downloads = downloads;
            Inquiries = inquiries;
            Sales = sales;
        }

        [DataMember]
        public string ProductID { get; set; }
        [DataMember]
        double Inquiries { get; set; }
        [DataMember]
        double Downloads { get; set; }
        [DataMember]
        double Sales { get; set; }
    }

    [DataContract]
    public class DashboardData
    {
        [DataMember]
        public List<SalesRecord> SalesData { get; set; }

        [DataMember]
        public List<ProductSummary> ProductSummaryData { get; set; }

        [DataMember]
        public double Accounts { get; set; }

        public DashboardData(List<SalesRecord> salesData, List<ProductSummary> productSummaryData, double accounts)
        {
            this.SalesData = salesData;
            this.ProductSummaryData = productSummaryData;
            this.Accounts = accounts;
        }
    }


    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SalesDataService
    {
        [OperationContract]
        public List<SalesRecord> GetData(DateRange range, TimePeriod dataPointPeriod)
        {
            List<SalesRecord> result = null;

            if (dataPointPeriod == TimePeriod.Decade)
                result = LoadDecadeSalesData(range.StartDate, range.EndDate);
            else if (dataPointPeriod == TimePeriod.Year)
                result = LoadYearlySalesData(range.StartDate, range.EndDate);
            else if (dataPointPeriod == TimePeriod.Month)
                result = LoadMonthlySalesData(range.StartDate, range.EndDate);
            else
                result = LoadDailySalesData(range.StartDate, range.EndDate);
            return result;
        }

        [OperationContract]
        public List<StringSalesRecord> GetAllData()
        {
            List<StringSalesRecord> a = LoadDecadeStringSalesData(new DateTime(1951, 1, 1), new DateTime(2000, 12, 31));
            return a;
        }

        [OperationContract]
        public List<SalesRecord> GetYearlyData(DateTime year)
        {
            return LoadYearlySalesData(new DateTime(year.Year, 1, 1), new DateTime(year.Year + 9, 12, 31));
        }

        [OperationContract]
        public List<SalesRecord> GetYearlyDataFromDecade(String decade)
        {
            int year = Convert.ToInt32(decade.Substring(0, 4));
            return LoadYearlySalesData(new DateTime(year + 1, 1, 1), new DateTime(year + 10, 12, 31));
        }

        [OperationContract]
        public List<SalesRecord> GetMonthlyData(DateTime year)
        {
            return LoadMonthlySalesData(new DateTime(year.Year, 1, 1), new DateTime(year.Year, 12, 31));
        }

        [OperationContract]
        public List<SalesRecord> GetDailyData(DateTime month)
        {
            return LoadDailySalesData(FirstDayOfMonth(month), LastDayOfMonth(month));
        }

        [OperationContract]
        public List<ProductSummary> GetProductSummaryData(DateRange range)
        {
            return LoadProductSummaryData(range.StartDate, range.EndDate);
        }

        [OperationContract]
        public DashboardData GetDashboardData(DateRange range, TimePeriod dataPointPeriod)
        {
            List<SalesRecord> SalesData = GetData(range, dataPointPeriod);
            List<ProductSummary> ProductSummaryData = LoadProductSummaryData(range.StartDate, range.EndDate);
            double AccountsData = LoadAccountsData(range.StartDate, range.EndDate);

            return new DashboardData(SalesData, ProductSummaryData, AccountsData);
        }

		/**************************************************************************/

		[OperationContract]
		public List<SalesRecord> GetAllAuxiliaryData()
		{
			return LoadYearlySalesData(new DateTime(1951, 1, 1), new DateTime(2000, 12, 31));
		}

		[OperationContract]
		public List<SalesRecord> GetYearlyAuxiliaryData(DateTime year)
		{
			//return LoadYearlySalesData(new DateTime(year.Year, 1, 1), new DateTime(year.Year + 9, 12, 31));
			return LoadMonthlySalesData(new DateTime(year.Year, 1, 1), new DateTime(year.Year + 9, 12, 31));
		}

		[OperationContract]
		public List<SalesRecord> GetYearlyAuxiliaryDataFromDecade(String decade)
		{
			int year = Convert.ToInt32(decade.Substring(0, 4));
			//return LoadYearlySalesData(new DateTime(year + 1, 1, 1), new DateTime(year + 10, 12, 31));
			return LoadMonthlySalesData(new DateTime(year + 1, 1, 1), new DateTime(year + 10, 12, 31));
		}

		[OperationContract]
		public List<SalesRecord> GetMonthlyAuxiliaryData(DateTime year)
		{
			//return LoadMonthlySalesData(new DateTime(year.Year, 1, 1), new DateTime(year.Year, 12, 31));
			return LoadDailySalesData(new DateTime(year.Year, 1, 1), new DateTime(year.Year, 12, 31));
		}

		[OperationContract]
		public List<SalesRecord> GetDailyAuxiliaryData(DateTime month)
		{
			return LoadDailySalesData(FirstDayOfMonth(month), LastDayOfMonth(month));
		}

		/**************************************************************************/

        private List<SalesRecord> LoadDecadeSalesData(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT (Int((Year(DateField)-1)/10)*10) AS DecadeC, "
                       + "(Sum(Sales_A)+Sum(Sales_B)+Sum(Sales_C)+Sum(Sales_D)) AS SumSales, "
                       + "(Sum(Downloads_A)+Sum(Downloads_B)+Sum(Downloads_C)+Sum(Downloads_D)) AS SumDownloads, "
                       + "(Sum(Inquiries_A)+Sum(Inquiries_B)+Sum(Inquiries_C)+Sum(Inquiries_D)) AS SumInquiries, "
                       + "Sum(Accounts) AS SumAccounts "
                       + "FROM SalesData "
                       + "WHERE DateField BETWEEN #" + startDate.ToOleDbString() + "# AND #" + (endDate - TimeSpan.FromDays(1)).ToOleDbString() + "# "
                       + "GROUP BY (Int((Year(DateField)-1)/10)*10)";
            DataTable dt = LoadData(sql);
            return GetDecadeSalesRecordList(dt);
        }

        private List<StringSalesRecord> LoadDecadeStringSalesData(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT (Int((Year(DateField)-1)/10)*10) AS DecadeC, "
                       + "(Sum(Sales_A)+Sum(Sales_B)+Sum(Sales_C)+Sum(Sales_D)) AS SumSales, "
                       + "(Sum(Downloads_A)+Sum(Downloads_B)+Sum(Downloads_C)+Sum(Downloads_D)) AS SumDownloads, "
                       + "(Sum(Inquiries_A)+Sum(Inquiries_B)+Sum(Inquiries_C)+Sum(Inquiries_D)) AS SumInquiries, "
                       + "Sum(Accounts) AS SumAccounts "
                       + "FROM SalesData "
                       + "WHERE DateField BETWEEN #" + startDate.ToOleDbString() + "# AND #" + (endDate - TimeSpan.FromDays(1)).ToOleDbString() + "# "
                       + "GROUP BY (Int((Year(DateField)-1)/10)*10)";
            DataTable dt = LoadData(sql);
            return GetDecadeSalesStringRecordList(dt);
        }

        private List<SalesRecord> LoadYearlySalesData(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT Year(DateField) AS YearC, "
                       + "(Sum(Sales_A)+Sum(Sales_B)+Sum(Sales_C)+Sum(Sales_D)) AS SumSales, "
                       + "(Sum(Downloads_A)+Sum(Downloads_B)+Sum(Downloads_C)+Sum(Downloads_D)) AS SumDownloads, "
                       + "(Sum(Inquiries_A)+Sum(Inquiries_B)+Sum(Inquiries_C)+Sum(Inquiries_D)) AS SumInquiries, "
                       + "Sum(Accounts) AS SumAccounts "
                       + "FROM SalesData "
                       + "WHERE DateField BETWEEN #" + startDate.ToOleDbString() + "# AND #" + (endDate - TimeSpan.FromDays(1)).ToOleDbString() + "# "
                       + "GROUP BY Year(DateField)";
            DataTable dt = LoadData(sql);
            return GetYearlySalesRecordList(dt);
        }

        private List<SalesRecord> LoadMonthlySalesData(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT Year(DateField) as YearC, Month(DateField) AS MonthC, "
                       + "(Sum(Sales_A)+Sum(Sales_B)+Sum(Sales_C)+Sum(Sales_D)) AS SumSales, "
                       + "(Sum(Downloads_A)+Sum(Downloads_B)+Sum(Downloads_C)+Sum(Downloads_D)) AS SumDownloads, "
                       + "(Sum(Inquiries_A)+Sum(Inquiries_B)+Sum(Inquiries_C)+Sum(Inquiries_D)) AS SumInquiries, "
                       + "Sum(Accounts) AS SumAccounts "
                       + "FROM SalesData "
                       + "WHERE DateField BETWEEN #" + startDate.ToOleDbString() + "# AND #" + (endDate - TimeSpan.FromDays(1)).ToOleDbString() + "# "
                       + "GROUP BY Year(DateField), Month(DateField)";
            DataTable dt = LoadData(sql);
            return GetMonthlySalesRecordList(dt);
        }

        private List<SalesRecord> LoadDailySalesData(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT Year(DateField) as YearC, Month(DateField) AS MonthC, Day(DateField) AS DayC, "
                       + "(Sum(Sales_A)+Sum(Sales_B)+Sum(Sales_C)+Sum(Sales_D)) AS SumSales, "
                       + "(Sum(Downloads_A)+Sum(Downloads_B)+Sum(Downloads_C)+Sum(Downloads_D)) AS SumDownloads, "
                       + "(Sum(Inquiries_A)+Sum(Inquiries_B)+Sum(Inquiries_C)+Sum(Inquiries_D)) AS SumInquiries, "
                       + "Sum(Accounts) AS SumAccounts "
                       + "FROM SalesData "
                       + "WHERE DateField BETWEEN #" + startDate.ToOleDbString() + "# AND #" + (endDate - TimeSpan.FromDays(1)).ToOleDbString() + "# "
                       + "GROUP BY Year(DateField), Month(DateField), Day(DateField)";
            DataTable dt = LoadData(sql);
            return GetDailySalesRecordList(dt);
        }

        private List<ProductSummary> LoadProductSummaryData(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT Sum(Downloads_A) AS SumDownloads_A, Sum(Inquiries_A) AS SumInquiries_A, Sum(Sales_A) AS SumSales_A, "
                       + "Sum(Downloads_B) AS SumDownloads_B, Sum(Inquiries_B) AS SumInquiries_B, Sum(Sales_B) AS SumSales_B, "
                       + "Sum(Downloads_C) AS SumDownloads_C, Sum(Inquiries_C) AS SumInquiries_C, Sum(Sales_C) AS SumSales_C, "
                       + "Sum(Downloads_D) AS SumDownloads_D, Sum(Inquiries_D) AS SumInquiries_D, Sum(Sales_D) AS SumSales_D "
                       + "FROM SalesData WHERE DateField BETWEEN #" + startDate.ToOleDbString() + "# AND #" + (endDate - TimeSpan.FromDays(1)).ToOleDbString() + "#";
            DataTable dt = LoadData(sql);
            return GetProductSummaryList(dt);
        }

        private double LoadAccountsData(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT Sum(Accounts) AS SumAccounts "
                       + "FROM SalesData WHERE DateField BETWEEN #" + startDate.ToOleDbString() + "# AND #" + (endDate - TimeSpan.FromDays(1)).ToOleDbString() + "#";
            DataTable dt = LoadData(sql);
            return (double)(dt.Rows[0][0] as double?);
        }

        private List<StringSalesRecord> GetDecadeSalesStringRecordList(DataTable dt)
        {
            List<StringSalesRecord> result = new List<StringSalesRecord>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new StringSalesRecord(Convert.ToString(dr["DecadeC"]) + "s", (double)dr["SumSales"], (double)dr["SumDownloads"], (double)dr["SumInquiries"], (double)dr["SumAccounts"]));
            }
            return result;
        }

        private List<SalesRecord> GetDecadeSalesRecordList(DataTable dt)
        {
            List<SalesRecord> result = new List<SalesRecord>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new SalesRecord(new DateTime((int)Math.Round((double)dr["DecadeC"]) + 1, 1, 1), (double)dr["SumSales"], (double)dr["SumDownloads"], (double)dr["SumInquiries"], (double)dr["SumAccounts"]));
            }
            return result;
        }

        private List<SalesRecord> GetYearlySalesRecordList(DataTable dt)
        {
            List<SalesRecord> result = new List<SalesRecord>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new SalesRecord(new DateTime((short)dr["YearC"], 1, 1), (double)dr["SumSales"], (double)dr["SumDownloads"], (double)dr["SumInquiries"], (double)dr["SumAccounts"]));
            }
            return result;
        }

        private List<SalesRecord> GetMonthlySalesRecordList(DataTable dt)
        {
            List<SalesRecord> result = new List<SalesRecord>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new SalesRecord(new DateTime((short)dr["YearC"], (short)dr["MonthC"], 1), (double)dr["SumSales"], (double)dr["SumDownloads"], (double)dr["SumInquiries"], (double)dr["SumAccounts"]));
            }
            return result;
        }

        private List<SalesRecord> GetDailySalesRecordList(DataTable dt)
        {
            List<SalesRecord> result = new List<SalesRecord>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new SalesRecord(new DateTime((short)dr["YearC"], (short)dr["MonthC"], (short)dr["DayC"]), (double)dr["SumSales"], (double)dr["SumDownloads"], (double)dr["SumInquiries"], (double)dr["SumAccounts"]));
            }
            return result;
        }

        private List<ProductSummary> GetProductSummaryList(DataTable dt)
        {
            List<ProductSummary> result = new List<ProductSummary>();

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                result.Add(new ProductSummary("A", (double)dr["SumDownloads_A"], (double)dr["SumInquiries_A"], (double)dr["SumSales_A"]));
                result.Add(new ProductSummary("B", (double)dr["SumDownloads_B"], (double)dr["SumInquiries_B"], (double)dr["SumSales_B"]));
                result.Add(new ProductSummary("C", (double)dr["SumDownloads_C"], (double)dr["SumInquiries_C"], (double)dr["SumSales_C"]));
                result.Add(new ProductSummary("D", (double)dr["SumDownloads_D"], (double)dr["SumInquiries_D"], (double)dr["SumSales_D"]));
            }

            return result;
        }

        private DataTable LoadData(string sql)
        {
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            conStr += HttpContext.Current.Server.MapPath("~/App_Data/demodata.mdb");
            System.Data.OleDb.OleDbConnection dbCon = new System.Data.OleDb.OleDbConnection(conStr);
            dbCon.Open();

            System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, dbCon);
            DataTable dt = new DataTable();
            dbAdapter.Fill(dt);

            dbCon.Close();

            return dt;
        }

        private DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        private DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
    }
}

