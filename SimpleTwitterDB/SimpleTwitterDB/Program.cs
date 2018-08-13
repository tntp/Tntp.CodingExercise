using System;
using System.Data.SqlClient;   // System.Data.dll 
//using System.Data;           // For:  SqlDbType , ParameterDirection

namespace simple_twitter_db_create
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var cb = new SqlConnectionStringBuilder
                {
                    DataSource = "simpletwitterrjh.database.windows.net",
                    UserID = "simpletwitteradmin",
                    Password = "SimpleTwitter1389#",
                    InitialCatalog = "SimpleTwitter"
                };

                using (var connection = new SqlConnection(cb.ConnectionString))
                {
                    connection.Open();

                    Submit_Tsql_NonQuery(connection, "Create Tables", CreateTable());
                    Submit_Tsql_NonQuery(connection, "Insert Data1", InsertData1());
                    Submit_Tsql_NonQuery(connection, "Insert Data2", InsertData2());
                    Submit_Tsql_NonQuery(connection, "Insert Data3", InsertData3());
                    Submit_Tsql_NonQuery(connection, "Insert Key", InsertKey());

                    DisplaySelectComments(connection);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("View the report output here, then press any key to end the program...");
            Console.ReadKey();
        }

        static string CreateTable()
        {
            return @"DROP TABLE IF EXISTS commentTable;
                     DROP TABLE IF EXISTS commentKey;
                     CREATE TABLE commentTable(
                        Comment_Id      int                 not null PRIMARY KEY,
                        Comment_Name    nvarchar(128)       not null,
                        Comment_Comment nvarchar(140)       not null,
                        Comment_Date    datetime            not null default SYSUTCDATETIME());
                     CREATE TABLE commentKey(
                        Next_Id         int                 not null PRIMARY KEY);
                     ";
        }

        static string InsertData1()
        {
            return @"
                    INSERT INTO commentTable
                       (Comment_Id, Comment_Name, Comment_Comment, Comment_Date) VALUES
                       (1, 'Shazam', 'Comment test 1', SYSUTCDATETIME());";
        }

        static string InsertData2()
        {
            return @"
                  INSERT INTO commentTable
                       (Comment_Id, Comment_Name, Comment_Comment, Comment_Date) VALUES
                       (2, 'Isis', 'Comment test 2', DATEADD(minute, 1, SYSDATETIME()));";
        }

        static string InsertData3()
        {
            return @"
                    INSERT INTO commentTable
                       (Comment_Id, Comment_Name, Comment_Comment, Comment_Date) VALUES
                       (3, 'Capt. Marvel', 'Comment test 3', DATEADD(minute, 2, SYSDATETIME()));";
        }

        static string InsertKey()
        {
            return @"INSERT INTO commentKey (Next_Id) VALUES (4);";
        }

        static string SelectComments()
        {
            return @"
                    SELECT cmt.Comment_Id,
                           cmt.Comment_Name,
                           cmt.Comment_Comment,
                           cmt.Comment_Date
                        FROM commentTable as cmt
                        ORDER BY Comment_Date desc;";
        }

        static void DisplaySelectComments(SqlConnection connection)
        {
            Console.WriteLine();
            Console.WriteLine("=================================");
            Console.WriteLine("Select comments...");

            string tsql = SelectComments();

            using (var command = new SqlCommand(tsql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0} , {1} , {2} , {3}",
                                          reader.GetInt32(0),
                                          reader.GetString(1),
                                          reader.GetString(2),
                                          reader.GetDateTime(3));
                    }
                }
            }
        }

        static void Submit_Tsql_NonQuery(SqlConnection connection, string tsqlPurpose, string tsqlSourceCode,
                                         string parameterName = null, string parameterValue = null)
        {
            Console.WriteLine();
            Console.WriteLine("=================================");
            Console.WriteLine(tsqlPurpose);

            using (var command = new SqlCommand(tsqlSourceCode, connection))
            {
                if (parameterName != null)
                {
                    command.Parameters.AddWithValue(parameterName, parameterValue);
                }
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " = rows affected.");
            }
        }
    } // end Program
}