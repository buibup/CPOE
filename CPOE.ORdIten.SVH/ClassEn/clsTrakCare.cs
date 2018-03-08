using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace CPOEORdItem.ClassEn
{
   public class clsTrakCare
    {
        public static string ODBCCon = ConfigurationManager.ConnectionStrings["MEDSD"].ToString();

        public DataTable data_Table(string strSQL)
        {
            DataTable dataTable = new DataTable();


            try
            {

                DataSet set = new DataSet();
                OdbcConnection selectConnection = new OdbcConnection();
                OdbcDataAdapter adapter = new OdbcDataAdapter();
                selectConnection = new OdbcConnection(ODBCCon);
                if (selectConnection.State != ConnectionState.Open)
                {
                    selectConnection.Open();
                }
                new OdbcDataAdapter(strSQL, selectConnection).Fill(dataTable);
                return dataTable;
            }
            catch (Exception exception)
            {
                exception.ToString();
            }
            finally
            {
                //this.Conn.Close();
            }
            return dataTable;
        }

        public DataTable GetDataTrak(string sql)
        {

            DataTable DT = new DataTable();

            using (OdbcConnection conn = new OdbcConnection(ODBCCon))
            {
                try
                {



                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    using (OdbcCommand command = new OdbcCommand())
                    {
                        command.Connection = conn;
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        command.CommandTimeout = 1000;

                        OdbcDataAdapter Oda1 = new OdbcDataAdapter(command);
                        Oda1.Fill(DT);
                        conn.Close();
                    }
                }
                catch (System.Exception ex)
                {
                    conn.Close();
                }
            }


            return DT;

        }




    }
}
