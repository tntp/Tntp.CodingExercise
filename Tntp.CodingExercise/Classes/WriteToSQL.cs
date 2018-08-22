using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Tntp.CodingExercise.Classes
{
    public class WriteToSQL
    {
        public static string _ConnectionString;

        public static int WriteToSql(DataSet dSet)
        {
            _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TNTP"].ConnectionString;

            //Using "Upsert Method" to insert only new entries reference: http://www.databasejournal.com/features/mssql/article.php/3739131/UPSERT-Functionality-in-SQL-Server-2008.htm

            #region temp tables string
            //Make temp tables in sql server that matches with production tables
            string tmp = " CREATE TABLE #developers ([ID] [nvarchar](256) NOT NULL, " +
                              " [FirstName] [nvarchar](100) NULL, " +
                              " [aka] [nvarchar](100) NULL, " +
                              " [Title] [nvarchar](100) NULL, " +
                              " [LastName] [nvarchar](100) NULL, " +
                              " [BirthDateTime] [datetime] NULL, " +
                              " [DeathDateTime] [datetime] NULL); " +

                              " CREATE TABLE #awards ([ID] [nvarchar](256) NOT NULL, " +
                              " [FirstName] [nvarchar](100) NULL, " +
                              " [LastName] [nvarchar](100) NULL, " +
                              " [BirthDateTime] [datetime] NULL, " +
                              " [AwardName] [nvarchar](512) NULL, " +
                              " [Year] [int] NULL, " +
                              " [GivenBy] [nvarchar](512) NULL); " +

                              " CREATE TABLE #contribs ([ID] [nvarchar](256) NOT NULL, " +
                              " [FirstName] [nvarchar](100) NULL, " +
                              " [LastName] [nvarchar](100) NULL, " +
                              " [BirthDateTime] [datetime] NULL, " +
                              " [ContribName] [nvarchar](100) NULL) ";
            #endregion

            #region inserting into SQL
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();

                //Execute the command to make a temp table
                SqlCommand cmd = new SqlCommand(tmp, conn);
                try
                {
                    #region creating temp tables and SqlBulkCopy DataTable into temp tables
                    cmd.ExecuteNonQuery();

                    //BulkCopy the data in the DataTable to the temp table
                    using (SqlBulkCopy bulkdev = new SqlBulkCopy(conn))
                    {
                        bulkdev.DestinationTableName = "#developers";
                        bulkdev.WriteToServer(dSet.Tables["Developers"]);
                    }

                    using (SqlBulkCopy bulkawa = new SqlBulkCopy(conn))
                    {
                        bulkawa.DestinationTableName = "#awards";
                        bulkawa.WriteToServer(dSet.Tables["Awards"]);
                    }

                    using (SqlBulkCopy bulkcon = new SqlBulkCopy(conn))
                    {
                        bulkcon.DestinationTableName = "#contribs";
                        bulkcon.WriteToServer(dSet.Tables["Contribs"]);
                    }
                    #endregion

                    #region comparing temp tables with production table and insert if not exists and scope new row count
                    string mergeSql = " MERGE INTO Developers AS Target " +
                                      " USING #developers AS Source " +
                                      " ON " +
                                      " Target.ID=Source.ID " +
                                      " WHEN NOT MATCHED THEN " +
                                      " INSERT (ID, FirstName, aka, Title, LastName, BirthDateTime, DeathDateTime) " +
                                      " VALUES (Source.ID, Source.FirstName, Source.aka, Source.Title, Source.LastName, Source.BirthDateTime, " +
                                      " Source.DeathDateTime);" +
                                      " SELECT @Rows_dev=@@ROWCOUNT; " +

                                      " MERGE INTO Awards AS Target " +
                                      " USING #awards AS Source " +
                                      " ON " +
                                      " Target.ID=Source.ID " +
                                      " WHEN NOT MATCHED THEN " +
                                      " INSERT (ID, FirstName, LastName, BirthDateTime, AwardName, Year, GivenBy) " +
                                      " VALUES (Source.ID, Source.FirstName, Source.LastName, Source.BirthDateTime, " +
                                      " Source.AwardName, Source.Year, Source.GivenBy);" +
                                      " SELECT @Rows_award=@@ROWCOUNT; " +

                                      " MERGE INTO Contribs AS Target " +
                                      " USING #contribs AS Source " +
                                      " ON " +
                                      " Target.ID=Source.ID " +
                                      " WHEN NOT MATCHED THEN " +
                                      " INSERT (ID, FirstName, LastName, BirthDateTime, ContribName) " +
                                      " VALUES (Source.ID, Source.FirstName, Source.LastName, Source.BirthDateTime, " +
                                      " Source.ContribName); " +
                                      " SELECT @Rows_con=@@ROWCOUNT; ";

                    cmd.CommandText = mergeSql;
                    SqlParameter Rows_dev = cmd.Parameters.Add("@Rows_dev", SqlDbType.Int);
                    SqlParameter Rows_award = cmd.Parameters.Add("@Rows_award", SqlDbType.Int);
                    SqlParameter Rows_con = cmd.Parameters.Add("@Rows_con", SqlDbType.Int);
                    Rows_dev.Direction = ParameterDirection.Output;
                    Rows_award.Direction = ParameterDirection.Output;
                    Rows_con.Direction = ParameterDirection.Output;



                    cmd.ExecuteNonQuery();
                    #endregion
                    HttpContext.Current.Session["Dev_inserted"] = (int)Rows_dev.Value;
                    HttpContext.Current.Session["Awa_inserted"] = (int)Rows_award.Value;
                    HttpContext.Current.Session["Con_inserted"] = (int)Rows_con.Value;

                    int a = (int)HttpContext.Current.Session["Dev_inserted"];
                    int b = (int)HttpContext.Current.Session["Awa_inserted"];
                    int c = (int)HttpContext.Current.Session["Con_inserted"];

                    #region dropping temp tables
                    cmd.CommandText = "DROP TABLE #developers; DROP TABLE #awards; DROP TABLE #contribs;";
                    cmd.ExecuteNonQuery();
                    #endregion

                }
                catch
                {
                    return 0;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                    cmd.Dispose();
                }
            }
            #endregion

            #region assign counts to session variable
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();

                SqlDataReader SqlReader = null;
                string sqlstr = " SELECT " +
                                " (SELECT COUNT (*) FROM Developers) AS Developer_Count, " +
                                " (SELECT COUNT (*) FROM Contribs) AS Contrib_Count, " +
                                " (SELECT COUNT (*) FROM Awards) AS Award_Count " +
                                " FROM Developers ";



                SqlCommand SqlCommand = new SqlCommand(sqlstr, conn);
                try
                {
                    SqlReader = SqlCommand.ExecuteReader();
                    while (SqlReader.Read())
                    {
                        HttpContext.Current.Session["Developer_Count"] = Convert.ToInt32(SqlReader["Developer_Count"].ToString());
                        HttpContext.Current.Session["Contrib_Count"] = Convert.ToInt32(SqlReader["Contrib_Count"].ToString());
                        HttpContext.Current.Session["Award_Count"] = Convert.ToInt32(SqlReader["Award_Count"].ToString());

                    }
                }
                catch
                {
                    return 1;
                }
                finally
                {
                    conn.Close();
                }
            }
            #endregion

            return 0;
        }
    }
}