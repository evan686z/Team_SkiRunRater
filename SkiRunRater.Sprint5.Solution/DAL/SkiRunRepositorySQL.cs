using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Configuration.Assemblies;


namespace SkiRunRater
{
    public class SkiRunRepositorySQL : ISkiRunRepository
    {
        public SkiRunRepositorySQL()
        {

        }

        /// <summary>
        /// read all data from the ski run table into a DataSet
        /// </summary>
        /// <returns>DataSet of ski run info</returns>
        private DataSet GetDataSet()
        {
            DataSet skiRuns_ds = new DataSet();

            string connString = GetConnectionString();
            string sqlCommandString = "SELECT * from SkiRuns";

            SqlConnection sqlConn = new SqlConnection(connString);
            SqlCommand sqlCommand = new SqlCommand(sqlCommandString, sqlConn);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);

            using (sqlConn)
            {
                using (sqlCommand)
                {
                    try
                    {
                        sqlConn.Open();
                        sqlAdapter.Fill(skiRuns_ds, "SkiRuns");
                    }
                    catch (SqlException sqlEx)
                    {
                        Console.WriteLine("SQL Exception: {0}", sqlEx.Message);
                    }
                }
            }

            return skiRuns_ds;
        }

        /// <summary>
        /// method to return a ski run given the ID
        /// uses a DataSet to hold ski run info
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <returns>ski run object</returns>
        public SkiRun SelectSkiRun(int ID)
        {
            DataSet skiRuns_ds = new DataSet();
            DataTable skiRuns_dt = new DataTable();
            DataRow[] skiRunRows;

            SkiRun skiRun = new SkiRun();
            string selectExpression;

            // read all ski run info into a DataSet
            skiRuns_ds = GetDataSet();

            // build select expression
            selectExpression = "ID = " + ID.ToString();

            // fill row array with matching ski runs
            skiRunRows = skiRuns_ds.Tables["SkiRuns"].Select(selectExpression);

            // check for unique ID value
            if (skiRunRows.Count() == 1)
            {
                skiRun.ID = int.Parse(skiRunRows[0]["ID"].ToString());
                skiRun.Name = skiRunRows[0]["Name"].ToString();
                skiRun.Vertical = int.Parse(skiRunRows[0]["Vertical"].ToString());
            }
            else
            {
                // ski run ID not unique
            }

            return skiRun;
        }

        /// <summary>
        /// method to return a ski run given the name
        /// uses a DataSet to hold ski run info
        /// NOTE: not implemented or tested
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <returns>ski run object</returns>
        public SkiRun SelectSkiRun(string name)
        {
            DataSet skiRuns_ds = new DataSet();
            DataTable skiRuns_dt = new DataTable();
            DataRow[] skiRunRows;

            SkiRun skiRun = new SkiRun();
            string selectExpression;

            // read all ski run info into a DataSet
            skiRuns_ds = GetDataSet();

            // build select expression
            selectExpression = "ID = " + name;

            // add all matching ski runs to an array of DataRow
            skiRunRows = skiRuns_ds.Tables["SkiRuns"].Select(selectExpression);

            // check for unique ID value
            if (skiRunRows.Count() == 1)
            {
                skiRun.ID = int.Parse(skiRunRows[0]["ID"].ToString());
                skiRun.Name = skiRunRows[0]["Name"].ToString();
                skiRun.Vertical = int.Parse(skiRunRows[0]["Vertical"].ToString());
            }
            else
            {
                // ski run ID not unique
            }

            return skiRun;
        }

        /// <summary>
        /// method to return a list of ski runs
        /// uses a DataSet to hold ski run info
        /// </summary>
        /// <returns>list of ski run objects</returns>
        public List<SkiRun> SelectAllSkiRuns()
        {
            DataSet skiRuns_ds = new DataSet();
            DataTable skiRuns_dt = new DataTable();

            List<SkiRun> skiRuns = new List<SkiRun>();

            // load in DataSet and DataTable
            skiRuns_ds = GetDataSet();

            // add all ski runs to an array of DataRow
            DataRow[] skiRunRows = skiRuns_ds.Tables["SkiRuns"].Select();

            // add all DataRow info to the list of ski runs
            foreach (DataRow skiRun in skiRunRows)
            {
                skiRuns.Add(new SkiRun
                {
                    ID = int.Parse(skiRun["ID"].ToString()),
                    Name = skiRun["Name"].ToString(),
                    Vertical = int.Parse(skiRun["Vertical"].ToString()),
                });
            }

            return skiRuns;
        }

        /// <summary>
        /// method to return a list of ski runs based on a select expression
        /// uses a DataSet to hold ski run info
        /// NOTE: not implemented or tested
        /// </summary>
        /// <returns>list of ski run objects</returns>
        public List<SkiRun> SelectSkiRuns(string expressoinString)
        {
            DataSet skiRuns_ds = new DataSet();
            DataTable skiRuns_dt = new DataTable();

            List<SkiRun> skiRuns = new List<SkiRun>();

            // load in DataSet and DataTable
            skiRuns_ds = GetDataSet();

            // add all matching ski runs to an array of DataRow
            DataRow[] skiRunRows = skiRuns_ds.Tables["SkiRuns"].Select(expressoinString);

            // add all DataRow info to the list of ski runs
            foreach (DataRow skiRun in skiRunRows)
            {
                skiRuns.Add(new SkiRun
                {
                    ID = int.Parse(skiRun["ID"].ToString()),
                    Name = skiRun["Name"].ToString(),
                    Vertical = int.Parse(skiRun["Vertical"].ToString()),
                });
            }

            return skiRuns;
        }

        /// <summary>
        /// method to add a new ski run
        /// </summary>
        /// <param name="skiRun"></param>
        public void InsertSkiRun(SkiRun skiRun)
        {
            string connString = GetConnectionString();

            // build out SQL command
            var sb = new StringBuilder("INSERT INTO SkiRuns");
            sb.Append(" ([ID],[Name],[Vertical])");
            sb.Append(" Values (");
            sb.Append("'").Append(skiRun.ID).Append("',");
            sb.Append("'").Append(skiRun.Name).Append("',");
            sb.Append("'").Append(skiRun.Vertical).Append("')");
            string sqlCommandString = sb.ToString();

            SqlConnection sqlConn = new SqlConnection(connString);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();

            using (sqlConn)
            {
                try
                {
                    sqlConn.Open();
                    sqlAdapter.InsertCommand = new SqlCommand(sqlCommandString, sqlConn);
                    sqlAdapter.InsertCommand.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("SQL Exception: {0}", sqlEx.Message);
                    Console.WriteLine(sqlCommandString);
                }
            }
        }

        /// <summary>
        /// method to delete a ski run by ski run ID
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteSkiRun(int ID)
        {
            string connString = GetConnectionString();

            // build out SQL command
            var sb = new StringBuilder("DELETE FROM SkiRuns");
            sb.Append(" WHERE ID = ").Append(ID);
            string sqlCommandString = sb.ToString();

            SqlConnection sqlConn = new SqlConnection(connString);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();

            using (sqlConn)
            {
                try
                {
                    sqlConn.Open();
                    sqlAdapter.DeleteCommand = new SqlCommand(sqlCommandString, sqlConn);
                    sqlAdapter.DeleteCommand.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("SQL Exception: {0}", sqlEx.Message);
                    Console.WriteLine(sqlCommandString);
                }
            }
        }

        /// <summary>
        /// method to update an existing ski run
        /// </summary>
        /// <param name="skiRun">ski run object</param>
        public void UpdateSkiRun(SkiRun skiRun)
        {
            string connString = GetConnectionString();

            // build out SQL command
            var sb = new StringBuilder("UPDATE SkiRuns SET ");
            sb.Append("Name = '").Append(skiRun.Name).Append("', ");
            sb.Append("Vertical = ").Append(skiRun.Vertical).Append(" ");
            sb.Append("WHERE ");
            sb.Append("ID = ").Append(skiRun.ID);
            string sqlCommandString = sb.ToString();

            SqlConnection sqlConn = new SqlConnection(connString);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();

            using (sqlConn)
            {
                try
                {
                    sqlConn.Open();
                    sqlAdapter.UpdateCommand = new SqlCommand(sqlCommandString, sqlConn);
                    sqlAdapter.UpdateCommand.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("SQL Exception: {0}", sqlEx.Message);
                    Console.WriteLine(sqlCommandString);
                }
            }
        }

        /// <summary>
        /// method to query the data by the vertical of each ski run in feet
        /// </summary>
        /// <param name="minimumVertical">int minimum vertical</param>
        /// <param name="maximumVertical">int maximum vertical</param>
        /// <returns></returns>
        public List<SkiRun> QueryByVertical(int minimumVertical, int maximumVertical)
        {
            DataSet skiRuns_ds = new DataSet();
            DataTable skiRuns_dt = new DataTable();

            List<SkiRun> matchingSkiRuns = new List<SkiRun>();

            // load in DataSet and DataTable
            skiRuns_ds = GetDataSet();
            skiRuns_dt = skiRuns_ds.Tables["SkiRuns"];

            // create an array of DataRows to hold the matching ski runs
            DataRow[] skiRuns;

            // generate the a query string based on the values of the arguments sent
            string queryExpression = String.Format("Vertical > {0} AND Vertical < {1}", minimumVertical, maximumVertical);

            // use the Select method to fill the array of ski runs
            skiRuns = skiRuns_dt.Select(queryExpression);

            // for each row in the array, create a new SkiRun object and add it to the list
            foreach (DataRow skiRun in skiRuns)
            {
                matchingSkiRuns.Add(new SkiRun()
                {
                    ID = int.Parse(skiRun["ID"].ToString()),
                    Name = skiRun["Name"].ToString(),
                    Vertical = int.Parse(skiRun["Vertical"].ToString()),
                });
            }

            return matchingSkiRuns;
        }

        /// <summary>
        /// get the connection string by name
        /// </summary>
        /// <returns>string connection string</returns>
        private static string GetConnectionString()
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            var settings = ConfigurationManager.ConnectionStrings["SkiRunRater_Local"];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        /// <summary>
        /// method to handle the IDisposable interface contract
        /// </summary>
        public void Dispose()
        {

        }
    }
}
