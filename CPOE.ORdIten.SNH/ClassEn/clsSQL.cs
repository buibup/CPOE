using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CPOEORdIten.ModelEn;

namespace CPOEORdItem.ClassEn
{
  public  class clsSQL
    {
        public static string SQLCon = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();

        public OrderItemModel Model = new OrderItemModel();
        public DataTable data_TableSQL(string strSQL)
        {
            DataTable dataTable = new DataTable();


            try
            {

                DataSet set = new DataSet();
                SqlConnection selectConnection = new SqlConnection();
                SqlDataAdapter adapter = new SqlDataAdapter();
                selectConnection = new SqlConnection(SQLCon);
                if (selectConnection.State != ConnectionState.Open)
                {
                    selectConnection.Open();
                }
                new SqlDataAdapter(strSQL, selectConnection).Fill(dataTable);
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

        public DataTable GetDataSQL(string sql)
        {

            DataTable DT = new DataTable();

            using (SqlConnection conn = new SqlConnection(SQLCon))
            {
                try
                {



                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        command.CommandTimeout = 1000;

                        SqlDataAdapter Oda1 = new SqlDataAdapter(command);
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


        public Boolean InsertOrderItem(OrderItemModel Model)
        {
            Boolean StatusInsert = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQLCon))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 300;
                        command.CommandText = "spOrderItem";
                        command.Parameters.Add("@State", SqlDbType.VarChar).Value = "Insert";
                        command.Parameters.Add("@Epi", SqlDbType.Decimal).Value = Model.Epi;
                        command.Parameters.Add("@OEORI_PhQtyOrd", SqlDbType.Int).Value = Model.OEORI_PhQtyOrd;
                        command.Parameters.Add("@OEORI_PrescNo", SqlDbType.VarChar).Value = Model.OEORI_PrescNo;
                        command.Parameters.Add("@OEORI_PrescSeqNo", SqlDbType.VarChar).Value = Model.OEORI_PrescSeqNo;
                        command.Parameters.Add("@OEORI_RowId", SqlDbType.VarChar).Value = Model.OEORI_RowId;
                        command.Parameters.Add("@OEORI_SttDat", SqlDbType.SmallDateTime).Value = Model.OEORI_SttDat;
                        command.Parameters.Add("@OEORI_SttTim", SqlDbType.Time).Value = Model.OEORI_SttTim;
                        command.Parameters.Add("@PHCFR_Desc1", SqlDbType.VarChar).Value = Model.PHCFR_Desc1;
                        command.Parameters.Add("@PHCIN_Desc1", SqlDbType.VarChar).Value = Model.PHCIN_Desc1;
                        command.Parameters.Add("@StatusConfirm", SqlDbType.VarChar).Value = '0';
                        command.Parameters.Add("@ARCIM_Desc", SqlDbType.VarChar).Value = Model.ARCIM_Desc;
                        command.Parameters.Add("@CTPCP_Desc", SqlDbType.VarChar).Value = Model.CTPCP_Desc;
                        command.Parameters.Add("@CTUOM_Code", SqlDbType.VarChar).Value = Model.CTUOM_Code;
                        command.Parameters.Add("@ORCAT_Desc", SqlDbType.VarChar).Value = Model.ORCAT_Desc;
                        command.Parameters.Add("@OSTAT_Desc", SqlDbType.VarChar).Value = Model.OSTAT_Desc;
                        command.Parameters.Add("@BED_Code", SqlDbType.VarChar).Value = Model.BED_Code;
                        command.Parameters.Add("@WARD_Code", SqlDbType.VarChar).Value = Model.WARD_Code;
                        command.Parameters.Add("@HOSP_Code", SqlDbType.VarChar).Value = Model.HOSP_Code;
                        command.Parameters.Add("@OECPR_Desc", SqlDbType.VarChar).Value = Model.OECPR_Desc;
                        command.Parameters.Add("@PAADM_VisitStatus", SqlDbType.VarChar).Value = Model.PAADM_VisitStatus;
                        command.Parameters.Add("@DCByCode", SqlDbType.VarChar).Value = Model.DCBy_Code;
                        command.Parameters.Add("@DCByName", SqlDbType.VarChar).Value = Model.DCBy_Name;
                        command.Parameters.Add("@AddByCode", SqlDbType.VarChar).Value = Model.AddBy_Code;
                        command.Parameters.Add("@AddByName", SqlDbType.VarChar).Value = Model.AddBy_Name;
                        command.Parameters.Add("@EN", SqlDbType.VarChar).Value = Model.EN;
                        command.Parameters.Add("@HN", SqlDbType.VarChar).Value = Model.HN;
                        command.Parameters.Add("@ORCAT_Code", SqlDbType.VarChar).Value = Model.ORCAT_Code;
                    

                        try
                        {
                            command.ExecuteNonQuery();
                            if (conn.State == ConnectionState.Open)
                                conn.Close();
                            StatusInsert= true;
              
                        }
                        catch(System.Exception ex){
                            if (conn.State == ConnectionState.Open)
                                conn.Close();
                            StatusInsert= false;
                        }


                    }

                    }
                }
            catch
            {

            }

            return StatusInsert;
        }


    }
}
