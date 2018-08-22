using System;
using System.Data;
using System.Text.RegularExpressions;
using MongoDB.Bson.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tntp.CodingExercise.Classes
{
    public class deserialization_TNTP
    {
        public static DataSet deserial_process(MongoCursor cursor)
        {

            try
            {
                #region Parse MongoCursor to Json
                var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
                string JSON_String = cursor.ToJson(jsonWriterSettings);
                var objects = JArray.Parse(JSON_String);
                dynamic obj = JsonConvert.DeserializeObject(JSON_String);
                DataSet Dset = new DataSet();
                #endregion

                #region Create DataSet tables and their columns
                Dset.Tables.Add("Developers");
                Dset.Tables["Developers"].Columns.Add("ID");
                Dset.Tables["Developers"].Columns.Add("FirstName");
                Dset.Tables["Developers"].Columns.Add("aka");
                Dset.Tables["Developers"].Columns.Add("Title");
                Dset.Tables["Developers"].Columns.Add("LastName");
                Dset.Tables["Developers"].Columns.Add("BirthDateTime");
                Dset.Tables["Developers"].Columns["BirthDateTime"].DataType = typeof(DateTime);
                Dset.Tables["Developers"].Columns.Add("DeathDateTime");
                Dset.Tables["Developers"].Columns["DeathDateTime"].DataType = typeof(DateTime);

                Dset.Tables.Add("Contribs");
                Dset.Tables["Contribs"].Columns.Add("ID");
                Dset.Tables["Contribs"].Columns.Add("FirstName");
                Dset.Tables["Contribs"].Columns.Add("LastName");
                Dset.Tables["Contribs"].Columns.Add("BirthDateTime");
                Dset.Tables["Contribs"].Columns["BirthDateTime"].DataType = typeof(DateTime);
                Dset.Tables["Contribs"].Columns.Add("ContribName");


                Dset.Tables.Add("Awards");
                Dset.Tables["Awards"].Columns.Add("ID");
                Dset.Tables["Awards"].Columns.Add("FirstName");
                Dset.Tables["Awards"].Columns.Add("LastName");
                Dset.Tables["Awards"].Columns.Add("BirthDateTime");
                Dset.Tables["Awards"].Columns["BirthDateTime"].DataType = typeof(DateTime);
                Dset.Tables["Awards"].Columns.Add("AwardName");
                Dset.Tables["Awards"].Columns.Add("Year");
                Dset.Tables["Awards"].Columns["Year"].DataType = typeof(Int32);
                Dset.Tables["Awards"].Columns.Add("GivenBy");
                #endregion

                #region Deserializing data, reading and deploying DataSet
                foreach (var item in obj)
                {
                    #region creating rows
                    DataRow newDeveloperRow = Dset.Tables["Developers"].NewRow();
                    #endregion

                    #region handling ObjectID, IsoDate and the common columns (id, FirstName, LastName, BirthDate)
                    if (item._id.ToString().StartsWith("{"))
                    {
                        string id_str = item._id.ToString();
                        id_str = id_str.Remove(0, 14);
                        id_str = id_str.Remove(id_str.Length - 4);
                        newDeveloperRow["ID"] = id_str;
                    }
                    else
                    {
                        newDeveloperRow["ID"] = item._id.ToString();
                    }

                    DateTime epoch_birth = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                    string str_birth = Regex.Replace(item.birth.ToString(), "[^-0-9]", "");

                    if (str_birth != "")
                    {
                        epoch_birth = epoch_birth.AddMilliseconds(Int64.Parse(str_birth));
                        newDeveloperRow["BirthDateTime"] = epoch_birth;
                    }
                    else
                    {
                        newDeveloperRow["BirthDateTime"] = DBNull.Value;
                    }

                    newDeveloperRow["FirstName"] = item.name.first.ToString();
                    if (item.name.aka != null)
                    {
                        if (!item.name.aka.ToString().StartsWith("{"))
                        {
                            newDeveloperRow["aka"] = item.name.aka.ToString();
                        }
                        else
                        {
                            newDeveloperRow["aka"] = DBNull.Value;
                        }
                    }
                    newDeveloperRow["LastName"] = item.name.last.ToString();
                    if (!item.title.ToString().StartsWith("{"))
                    {
                        newDeveloperRow["Title"] = item.title.ToString();
                    }
                    else
                    {
                        newDeveloperRow["Title"] = DBNull.Value;
                    }




                    #endregion

                    #region put add'l data inside "Developers" table
                    var epoch_death = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                    string str_death = Regex.Replace(item.death.ToString(), "[^-0-9]", "");

                    if (str_death != "")
                    {
                        epoch_death = epoch_death.AddMilliseconds(Int64.Parse(str_death));
                        newDeveloperRow["DeathDateTime"] = epoch_death;
                    }
                    else
                    {
                        newDeveloperRow["DeathDateTime"] = DBNull.Value;
                    }

                    Dset.Tables["Developers"].Rows.Add(newDeveloperRow);
                    #endregion

                    #region put data inside "Contribs" table

                    for (int i = 0; i < item.contribs.Count; i++)
                    {
                        DataRow newContribRow = Dset.Tables["Contribs"].NewRow();
                        newContribRow["ID"] = newDeveloperRow["ID"];
                        newContribRow["FirstName"] = item.name.first.ToString();
                        newContribRow["LastName"] = item.name.last.ToString();
                        newContribRow["BirthDateTime"] = newDeveloperRow["BirthDateTime"];
                        newContribRow["ContribName"] = item.contribs[i].ToString();

                        Dset.Tables["Contribs"].Rows.Add(newContribRow);
                    }

                    #endregion

                    #region put data inside "Awards" table

                    for (int i = 0; i < item.awards.Count; i++)
                    {
                        DataRow newAwardRow = Dset.Tables["Awards"].NewRow();
                        newAwardRow["ID"] = newDeveloperRow["ID"];
                        newAwardRow["FirstName"] = item.name.first.ToString();
                        newAwardRow["LastName"] = item.name.last.ToString();
                        newAwardRow["BirthDateTime"] = newDeveloperRow["BirthDateTime"];

                        JObject aItem = (JObject)item.awards[i];
                        newAwardRow["AwardName"] = aItem["award"].ToString();
                        newAwardRow["Year"] = Convert.ToInt32(aItem["year"].ToString());
                        newAwardRow["GivenBy"] = aItem["by"].ToString();
                        Dset.Tables["Awards"].Rows.Add(newAwardRow);
                    }

                    #endregion

                }
                #endregion

                return Dset;
            }
            catch (Exception ee)
            {
                string error = "Something went wrong " + ee.Message;
                return null;
            }
        }
    }
}