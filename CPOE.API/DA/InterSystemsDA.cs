using InterSystems.Data.CacheClient;
using System;
using System.Data;

namespace CPOE.API.DA
{
    public class InterSystemsDA
    {
        public static DataTable DTBindDataCommand(string cmdString, string conString)
        {
            DataTable dt = new DataTable();

            using (var con = new CacheConnection(conString))
            {
                con.Open();
                using (var adt = new CacheDataAdapter(cmdString, conString))
                {
                    adt.Fill(dt);
                }
            }

            return dt;
        }

        public static DataSet DSBindDataCommand(string cmdString, string conString)
        {
            DataSet ds = new DataSet();

            using (var con = new CacheConnection(conString))
            {
                con.Open();
                using (var adt = new CacheDataAdapter(cmdString, con))
                {
                    adt.Fill(ds);
                }
            }

            return ds;
        }

        public static string BindDataCommand(string cmdString, string conString)
        {
            string result = string.Empty;

            using (var con = new CacheConnection(conString))
            {
                con.Open();
                using (var cmd = new CacheCommand(cmdString, con))
                {
                    try
                    {
                        result = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception)
                    {

                        return result;
                    }

                }
            }

            return result;
        }
    }
}