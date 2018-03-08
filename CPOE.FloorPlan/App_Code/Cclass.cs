using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
/// <summary>
/// Summary description for Cclass
/// </summary>
public class Cclass
{
    public DataTable GetDataOdbc(string param_Sql)
    {
        DataTable dt_A = new DataTable();
        try
        {
            using (OdbcConnection Oconn = new OdbcConnection(ConfigurationManager.AppSettings["Trakcare"].ToString()))
            {
                using (OdbcCommand Ocomm = new OdbcCommand(param_Sql, Oconn))
                {
                    Ocomm.Connection.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter(Ocomm);
                    DataSet ds_A = new DataSet();
                    da.Fill(ds_A);
                    dt_A = ds_A.Tables[0];
                    Ocomm.Connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return dt_A;
    }

    public DataTable GetDataSQL(string param_Sql)
    {
        DataTable dt_A = new DataTable();
        try
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Conn"].ToString()))
            {
                using (SqlCommand comm = new SqlCommand(param_Sql, conn))
                {
                    comm.Connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(comm);
                    DataSet ds_A = new DataSet();
                    da.Fill(ds_A);
                    dt_A = ds_A.Tables[0];
                    comm.Connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return dt_A;
    }

    public DataTable GetOrderNew(String param_epiRowID)
    {
        DataTable dt_A = new DataTable();
        try
        {
            String Sql = "select * from OrderItem where isnull(StatusConfirm,'0') = '0' and CONVERT(varchar,OEORI_SttDat,103) = CONVERT(varchar,GETDATE(),103) and epi = " +
                         param_epiRowID + " order by cast(REPLACE(OEORI_PrescSeqNo,'/','')as integer) ";
            dt_A = GetDataSQL(Sql);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return dt_A;
    }
    public Boolean UpdateStatusOrder(String param_data)
    {
        Boolean param_return = false;
        try
        {
            String Sql = "update OrderItem set  [StatusConfirm] = '1' where re_cno in ('" + param_data + "')";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Conn"].ToString()))
            {
                using (SqlCommand comm = new SqlCommand(Sql, conn))
                {
                    comm.Connection.Open();
                    comm.ExecuteNonQuery();
                    param_return = true;
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return param_return;
    }

    public Boolean UpdateStatusConfirmOrder(String param_OEORI_RowId)
    {
        DateTime datetime = DateTime.Now;

        Boolean param_return = false;
        try
        {
            String Sql = "update OrderItem set [StatusConfirm] = '1',[ComnameUpdate] = '"+Environment.MachineName + "',[TimeUpdate] = '" + datetime  + "' where OEORI_RowId in ('" + param_OEORI_RowId + "')";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Conn"].ToString()))
            {
                using (SqlCommand comm = new SqlCommand(Sql, conn))
                {
                    
                    comm.Connection.Open();
                    comm.ExecuteNonQuery();
                    param_return = true;
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return param_return;
    }

}