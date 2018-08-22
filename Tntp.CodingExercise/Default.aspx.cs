using System;
using System.Web.UI;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Tntp.CodingExercise.Classes;

namespace Tntp.CodingExercise
{
    public class Blog
    {
        public dynamic _id { get; set; }
        public BsonDocument name { get; set; }
        public BsonString aka { get; set; }
        public BsonString title { get; set; }
        public BsonDateTime birth { get; set; }
        public BsonDateTime death { get; set; }
        public BsonArray contribs { get; set; }
        public BsonArray awards { get; set; }
    }

    public partial class _Default : Page
    {
        public string _ConnectionString_mongo;
        public static string _ConnectionString;
        DataSet gv_dSet = null;
        string id_finder = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region DB Connection strings for MongoDB and SQL
            // Initialize data sources. Use connection string from configuration file (web.config).
            _ConnectionString_mongo = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TNTP"].ConnectionString;
            #endregion

            id_finder = Request.QueryString["ID_FINDER"];

            if (!Page.IsPostBack)
            {
                try
                {
                    #region connect to mongoDB and read
                    var client = new MongoClient(_ConnectionString_mongo);
                    MongoServer server = client.GetServer();
                    MongoDatabase database = server.GetDatabase("developersDB");
                    MongoCollection mongo_collection = database.GetCollection("DevelopersInfo");
                    MongoCursor<Blog> cursor = mongo_collection.FindAllAs<Blog>();
                    #endregion

                    #region send information to Classes
                    DataSet Dset = deserialization_TNTP.deserial_process(cursor);


                    if (Dset != null)
                    {
                        int result = WriteToSQL.WriteToSql(Dset);

                        if (result != 0)
                        {
                            lblerror.Text = "Something went wrong";
                        }
                    }
                    else
                    {
                        lblerror.Text = "Something went wrong";
                    }
                    #endregion

                    #region bindDatas
                    BindData_developers();
                    BindData_contribs();
                    BindData_awards();
                    #endregion

                    #region Session variable reading
                    Developer_total.Text = Session["Developer_Count"].ToString();
                    Contrib_total.Text = Session["Contrib_Count"].ToString();
                    Award_total.Text = Session["Award_Count"].ToString();

                    Developer_added.Text = Session["Dev_inserted"].ToString();
                    Contrib_added.Text = Session["Con_inserted"].ToString();
                    Award_added.Text = Session["Awa_inserted"].ToString();
                    #endregion
                    
                }
                catch (Exception ee)
                {
                    lblerror.Text = "Couldn't connect to MongoDB " + ee.Message;
                }
            }
        }

        #region BindData_developers()
        private void BindData_developers()
        {

            SqlConnection conn = new SqlConnection(_ConnectionString);
            string sqlstr = " SELECT " +
                              " ID, FirstName, aka, Title, LastName, BirthDateTime, DeathDateTime " +
                              " FROM Developers ";



            SqlCommand SqlCommand = new SqlCommand(sqlstr, conn);
            SqlDataAdapter SqlAdapter = new SqlDataAdapter();


            gv_dSet = new DataSet();
            try
            {
                SqlAdapter.SelectCommand = SqlCommand;
                SqlAdapter.Fill(gv_dSet, "data_bulk_dev");
                gv_developers.DataSource = gv_dSet.Tables["data_bulk_dev"].DefaultView;
                gv_developers.DataBind();
                Session["data_bulk_dev"] = gv_dSet.Tables["data_bulk_dev"];


            }
            catch (SqlException ee)
            {
                lblerror.Text = ee.Message;
            }
            finally
            {
                gv_dSet.Dispose();
                SqlCommand.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }
        #endregion

        #region BindData_contribs()
        private void BindData_contribs()
        {

            SqlConnection conn = new SqlConnection(_ConnectionString);
            string sqlstr = " SELECT " +
                              " ID, FirstName, LastName, BirthDateTime, ContribName " +
                              " FROM Contribs ";



            SqlCommand SqlCommand = new SqlCommand(sqlstr, conn);
            SqlDataAdapter SqlAdapter = new SqlDataAdapter();


            gv_dSet = new DataSet();
            try
            {
                SqlAdapter.SelectCommand = SqlCommand;
                SqlAdapter.Fill(gv_dSet, "data_bulk_con");
                gv_contribs.DataSource = gv_dSet.Tables["data_bulk_con"].DefaultView;
                gv_contribs.DataBind();
                Session["data_bulk_con"] = gv_dSet.Tables["data_bulk_con"];


            }
            catch (SqlException ee)
            {
                lblerror.Text = ee.Message;
            }
            finally
            {
                gv_dSet.Dispose();
                SqlCommand.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }
        #endregion

        #region BindData_awards()
        private void BindData_awards()
        {

            SqlConnection conn = new SqlConnection(_ConnectionString);
            string sqlstr = " SELECT " +
                              " ID, FirstName, LastName, BirthDateTime, AwardName, Year, GivenBy " +
                              " FROM Awards ";



            SqlCommand SqlCommand = new SqlCommand(sqlstr, conn);
            SqlDataAdapter SqlAdapter = new SqlDataAdapter();


            gv_dSet = new DataSet();
            try
            {
                SqlAdapter.SelectCommand = SqlCommand;
                SqlAdapter.Fill(gv_dSet, "data_bulk_awa");
                gv_awards.DataSource = gv_dSet.Tables["data_bulk_awa"].DefaultView;
                gv_awards.DataBind();
                Session["data_bulk_awa"] = gv_dSet.Tables["data_bulk_awa"];


            }
            catch (SqlException ee)
            {
                lblerror.Text = ee.Message;
            }
            finally
            {
                gv_dSet.Dispose();
                SqlCommand.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }
        #endregion

        #region Gridview events
        protected void gv_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            GridView gv = sender as GridView;
            //Retrieve the table from the session object.


            if (gv.ID == "gv_developers")
            {
                DataTable dt = Session["data_bulk_dev"] as DataTable;
                if (dt != null)
                {
                    //Sort the data.
                    dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                    gv_developers.DataSource = Session["data_bulk_dev"];
                    gv_developers.DataBind();
                }
            }

            if (gv.ID == "gv_contribs")
            {
                DataTable dt = Session["data_bulk_con"] as DataTable;
                if (dt != null)
                {
                    //Sort the data.
                    dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                    gv_contribs.DataSource = Session["data_bulk_con"];
                    gv_contribs.DataBind();
                }
            }

            if (gv.ID == "gv_awards")
            {
                DataTable dt = Session["data_bulk_awa"] as DataTable;
                if (dt != null)
                {
                    //Sort the data.
                    dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                    gv_awards.DataSource = Session["data_bulk_awa"];
                    gv_awards.DataBind();
                }
            }
        }

        protected void gv_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes.Add("onMouseOver", "Highlight(this)");
                e.Row.Attributes.Add("onMouseOut", "UnHighlight(this)");
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";


            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";

                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }
        #endregion
    }
}