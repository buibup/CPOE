using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Configuration;
using System.Drawing;
using System.Net;
using System.Text;
using System.Json;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

public partial class frmData : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    String Sql = "";
    String param_Sql = "";
    Cclass Class = new Cclass();
    String param_loc = "";
    String param_ward = "";
    String param_bed = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["bu"] != null)
            {
                param_loc = Request.QueryString["bu"];
            }
            if (Request.QueryString["ward"] != null)
            {
                param_ward = Request.QueryString["ward"];
            }
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    protected string GetDiag(String param_Rowid)
    {
        String param_Diag = "";
        DataTable dt1 = new DataTable();
        try
        {
            param_Sql = "select PAADM_MainMRADM_DR->MR_Diagnos->MRDIA_ICDCode_DR->MRCID_Desc from pa_adm where PAADM_RowID=" + param_Rowid;
            dt1 = Class.GetDataOdbc(param_Sql);
            if (dt1.Rows.Count > 0)
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (j != 0)
                    {
                        param_Diag += " , ";
                    }
                    param_Diag += dt1.Rows[j]["MRCID_Desc"];
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return param_Diag;
    }
    protected string GetOrderSQL(String param_OEORI_RowId)
    {
        String param_Diag = "";
        DataTable dt1 = new DataTable();
        try
        {
            param_Sql = "select distinct ARCIM_Desc As  Orderdetail from [CPOE_ORDERITM].[dbo].[OrderItem] where OEORI_RowId ='" + param_OEORI_RowId + "'";
            dt1 = Class.GetDataSQL(param_Sql);
            if (dt1.Rows.Count > 0)
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (j != 0)
                    {
                        param_Diag += " , ";
                    }
                    param_Diag += dt1.Rows[j]["Orderdetail"];
                    //param_Diag += dt1.Rows[j]["Date"];
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return param_Diag;
    }
    protected string GetOrderDateSQL(String param_OEORI_RowId)
    {
        String param_Diag = "";
        DataTable dt1 = new DataTable();
        try
        {
            param_Sql = "select distinct [OEORI_SttDat]As  OrderDate from [CPOE_ORDERITM].[dbo].[OrderItem] where OEORI_RowId ='" + param_OEORI_RowId + "'";
            dt1 = Class.GetDataSQL(param_Sql);
            if (dt1.Rows.Count > 0)
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (j != 0)
                    {
                        param_Diag += " , ";
                    }
                    param_Diag += dt1.Rows[j]["OrderDate"];

                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return param_Diag;
    }
    protected string GetOrderTimeSQL(String param_OEORI_RowId)
    {
        String param_Diag = "";
        DataTable dt1 = new DataTable();
        try
        {
            param_Sql = "select distinct  [OEORI_SttTim] As  OrderTime from [CPOE_ORDERITM].[dbo].[OrderItem] where OEORI_RowId ='" + param_OEORI_RowId + "'";
            dt1 = Class.GetDataSQL(param_Sql);
            if (dt1.Rows.Count > 0)
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {

                    param_Diag += dt1.Rows[j]["OrderTime"];

                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return param_Diag;
    }
    protected DataTable LoadData()
    {
        //string OEORI_RowId = "17089598||67";
        //getApiOrder(OEORI_RowId);

       

        try
        {


            //Sql = "SELECT PAADM_AdmDate,convert(varchar,PAADM_AdmDate,103)as AdmDate,PAADM_AdmTime,PAADM_PAPMI_DR,PAADM_ADMNo,PAADM_RowID,PAADM_PAPMI_DR->PAPMI_No, " +
            //     "PAADM_PAPMI_DR->PAPMI_Title_DR->TTL_Desc,PAADM_PAPMI_DR->PAPMI_Name,PAADM_PAPMI_DR->PAPMI_Name2, " +
            //     "(PAADM_PAPMI_DR->PAPMI_Title_DR->TTL_Desc || ' ' || PAADM_PAPMI_DR->PAPMI_Name || ' ' ||PAADM_PAPMI_DR->PAPMI_Name2)as Name, " +
            //     "PAADM_PAPMI_DR->PAPMI_Sex_DR->CTSEX_Code, " +
            //     "(case when PAADM_PAPMI_DR->PAPMI_Sex_DR->CTSEX_Code = 'M' then 'ชาย(Male)' " +
            //     "when PAADM_PAPMI_DR->PAPMI_Sex_DR->CTSEX_Code = 'F' then 'หญิง(Female)' else '' end ) as Sex, " +
            //     "PAADM_Type,PAADM_AdmDocCodeDR->CTPCP_Desc,PAADM_VisitStatus, " +
            //     "PAADM_Oper_DR->OPER_Desc,PAADM_CurrentRoom_DR->ROOM_Code,PAADM_CurrentWard_DR->WARD_Code, " +
            //     "PAADM_Hospital_DR->HOSP_Code,PAADM_PAPMI_DR->PAPMI_RowId->PAPER_AgeYr,PAADM_PAPMI_DR->PAPMI_RowId->PAPER_AgeMth, " +
            //     "PAADM_CurrentBed_DR->BED_Code,'' as Diag " +
            //     "FROM pa_adm " +
            //     "where PAADM_Type like 'I%' and  PAADM_VisitStatus like 'A%' and PAADM_CurrentRoom_DR->ROOM_Code not in ('12FN','11FN') ";



            //Sql = @"SELECT  PAADM_AdmDate, convert(varchar, PAADM_AdmDate,103)as AdmDate,PAADM_AdmTime,PAADM_PAPMI_DR,PAADM_ADMNo,PAADM_RowID,PAADM_PAPMI_DR->PAPMI_No,
            //                PAADM_PAPMI_DR->PAPMI_Title_DR->TTL_Desc,PAADM_PAPMI_DR->PAPMI_Name,PAADM_PAPMI_DR->PAPMI_Name2,
            //                (PAADM_PAPMI_DR->PAPMI_Title_DR->TTL_Desc || ' ' || PAADM_PAPMI_DR->PAPMI_Name || ' ' || PAADM_PAPMI_DR->PAPMI_Name2) as Name,
            //                PAADM_PAPMI_DR->PAPMI_Sex_DR->CTSEX_Code, (case when PAADM_PAPMI_DR->PAPMI_Sex_DR->CTSEX_Code = 'M' then 'ชาย(Male)' when PAADM_PAPMI_DR->PAPMI_Sex_DR->CTSEX_Code = 'F' then 'หญิง(Female)' else '' end ) as Sex,
            //                PAADM_Type,PAADM_AdmDocCodeDR->CTPCP_Desc,PAADM_VisitStatus, PAADM_Oper_DR->OPER_Desc,PAADM_CurrentRoom_DR->ROOM_Code,PAADM_CurrentWard_DR->WARD_Code,
            //                PAADM_Hospital_DR->HOSP_Code,PAADM_PAPMI_DR->PAPMI_RowId->PAPER_AgeYr,PAADM_PAPMI_DR->PAPMI_RowId->PAPER_AgeMth, PAADM_CurrentBed_DR->BED_Code,'' as Diag,
            //                OE_OrdItem.OEORI_Priority_DR->OECPR_Desc,
            //                OE_OrdItem.OEORI_OEORD_ParRef->OEORD_Adm_DR->PAADM_RowID,OE_OrdItem.OEORI_RowId,'' as OrderDetail,''as OrderDate,''as OrderTime
            //     FROM pa_adm " + (char)34 +"pa_adm" + (char)34 + ",SQLUser.OE_OrdItem " + (char)34 +"OE_OrdItem" + (char)34 +
            //    " where PAADM_Type like 'I%' and PAADM_VisitStatus like 'A%' and PAADM_CurrentRoom_DR->ROOM_Code not in ('12FN','11FN')"+
            //     "and PAADM_RowID = OE_OrdItem.OEORI_OEORD_ParRef->OEORD_Adm_DR->PAADM_RowID ";


            Sql = "select ARCIM_Desc,OEORI_PhQtyOrd,CTUOM_Code,PHCFR_Code,CONVERT(VARCHAR(10),[OEORI_SttDat],103) as OEORI_SttDat ,OEORI_SttTim,CTPCP_Desc,StatusConfirm,BED_Code,OEORI_RowId,OECPR_Desc,HOSP_Code,WARD_Code,'' as APIOrder FROM[CPOE_ORDERITM].[dbo].[OrderItem] where AddByCode like'0119%' and  StatusConfirm = 0 ";

            if (param_loc != "")
            {
                Sql += "and HOSP_Code = '0" + param_loc + "' ";
            }
            if (param_ward != "")
            {
                Sql += "and WARD_Code='" + param_ward + "' ";
            }

            if (param_bed != "")
            {
                Sql += "and BED_Code='" + param_bed + "' ";
            }

            Sql += "And  ORCAT_Code not in( '19','24','17' ) ";
            Sql += "group by ARCIM_Desc,OEORI_PhQtyOrd,CTUOM_Code,PHCFR_Code,[OEORI_RowId],OEORI_SttDat,OEORI_SttTim,CTPCP_Desc,StatusConfirm,BED_Code,OECPR_Desc,HOSP_Code,WARD_Code ";
            Sql += "Order By OEORI_SttDat desc ,OEORI_SttTim desc ,BED_Code asc ";

             dt = Class.GetDataSQL(Sql);

            DataTable dtOrderRowID = new DataTable();
            dtOrderRowID.Columns.Add("OrderAPIRowID", typeof(string));

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   
                    string OrderAPI =  getApiOrder(dt.Rows[i]["OEORI_RowId"].ToString());
                    dt.Rows[i]["APIOrder"] = OrderAPI;
                    dt.Rows[i]["ARCIM_Desc"] = dt.Rows[i]["ARCIM_Desc"].ToString() + "    Quantity : " + dt.Rows[i]["OEORI_PhQtyOrd"].ToString() + " " + dt.Rows[i]["CTUOM_Code"].ToString() + " " + dt.Rows[i]["PHCFR_Code"].ToString();

                }
            }
            grid.DataSource = dt;
            grid.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return dt;
    }
    protected string getApiOrder(string OEORI_RowId)
    {
        string URL = "http://10.104.10.45/QuestionAPI/api/Question/GetQuestion?OEORI_rowid=" + OEORI_RowId;
        WebClient wc = new WebClient();
        wc.Encoding = Encoding.UTF8;
        string json = wc.DownloadString(URL);

        if ((json == "\"\"") || (json == null))
        {

            json = null;
        }
        else
        {
            json = json.Replace(Convert.ToChar(034).ToString(), ""); //char(034) is "
             json = json.Replace("<br/>",Environment.NewLine );
            //json = Regex.Replace(json, @"<[^>]*>", String.Empty);
            json = Regex.Replace(json, @"<[^>]*>", Environment.NewLine);


        }

        // Regex.Replace(json, @"<[^>]*>", String.Empty);
        return json;


        #region json model
        //string URL = " http://10.104.10.45/CRM_API/api/crm/GetListCRM?hn=11-09-013746";
        //WebClient wc = new WebClient();
        //wc.Encoding = Encoding.UTF8;
        //string json = wc.DownloadString(URL);
        //JsonArray jsonObj = (JsonArray)JsonObject.Load(wc.OpenRead(URL));
        //List<APIOrder> listOrder = JsonConvert.DeserializeObject<List<APIOrder>>(jsonObj.ToString());


        //return json;
        #endregion


    }

    // Gridview Event
    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        
        try
        {
            

            //*** CustomerID ***//
            Label lblCustomerID = (Label)(e.Row.FindControl("lblOEORI_RowId"));
            if (lblCustomerID != null)
            {
                lblCustomerID.Text = (string)DataBinder.Eval(e.Row.DataItem, "OEORI_RowId");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lblAPIOrder = (Label)e.Row.Cells[6].FindControl("lblAPIOrder");
                //e.Row.Cells[2].ToolTip = HttpUtility.HtmlDecode(lblAPIOrder.Text.Replace("<br/>", "\r\n"));

                ImageButton btnImgNew = (ImageButton)e.Row.FindControl("imgNew");
                ImageButton btnImgView = (ImageButton)e.Row.FindControl("imgView");
                ImageButton btnImgReport = (ImageButton)e.Row.FindControl("imgReport");
                Label lblHnRowid = (Label)e.Row.FindControl("lblPAADM_RowID");
                Label lblEpiRowId = (Label)e.Row.FindControl("lblPAADM_PAPMI_DR");
                

                int param_ = Class.GetOrderNew(lblHnRowid.Text).Rows.Count;
                if (param_ > 0)
                {
                    string OpenPopup = "var Mleft = (screen.width/2)-(800/2);var Mtop = (screen.height/2)-(500/2);window.open( 'frmPopup.aspx?rowid=" + lblHnRowid.Text + "', null, 'height=500,width=800,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' ); return false;";
                    btnImgNew.Attributes.Add("onclick", OpenPopup);
                    btnImgNew.Visible = true;
                }
                else
                {
                    btnImgNew.Visible = false;
                }
                btnImgView.Attributes.Add("onclick", "window.open('" + ConfigurationManager.AppSettings["MedDay"].ToString() + lblEpiRowId.Text + "'); return false;");
                btnImgReport.Attributes.Add("onclick", "alert('" + lblEpiRowId.Text + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }


        
    }
    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid.PageIndex = e.NewPageIndex;
        LoadData();
    }
    protected void grid_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i <= grid.Rows.Count - 1; i++)
        {
            string checkStstus = grid.Rows[i].Cells[5].Text;

            if (checkStstus == "STAT")
            {
                //grid.Rows[i].BackColor = Color.FromName("#ffd394");
               // grid.Rows[i].BackColor = Color.FromName("#ff7c75");
                grid.Rows[i].BackColor = Color.FromName("#ff564b");
               // grid.Rows[i].BackColor = Color.FromName("#FFD1DC"); 
                grid.Rows[i].ForeColor = Color.White;
   
                   
            }

        }


    }
    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CheckAllRoomSelect")
        {
            string Bedcode = e.CommandArgument.ToString();
            
            for(var i=0;i< grid.Rows.Count;i++)
            {
                CheckBox chk = (CheckBox)grid.Rows[i].Cells[0].FindControl("CheckID");
                Label lblBED_Code = (Label)grid.Rows[i].Cells[0].FindControl("BED_Code");
                if (Bedcode == lblBED_Code.Text)
                {
                    chk.Checked = true;
                }
                else
                {
                    chk.Checked = false;
                }
            }
            grid_DataBound(null,null);
            btCheckOrder.Focus();
        }

    }
    protected void grid_RowCreated(object sender, GridViewRowEventArgs e)
    {
      
    }

    // Button Event
    protected void btCheckOrder_Click(object sender, EventArgs e)
    {
        CheckBox chkCusID;
        Label lblID;

        string strname = string.Empty;
        // int count = 1;
        int i;
        lblText.Text = "";

        for (i = 0; i <= grid.Rows.Count - 1; i++)
        {

            chkCusID = (CheckBox)grid.Rows[i].FindControl("CheckID");
            lblID = (Label)grid.Rows[i].FindControl("lblOEORI_RowId");


            if (chkCusID != null & chkCusID.Checked)
            {

                //for (i = 0; i < count; i++)
                //{
                //   // this.lblText.Text = this.lblText.Text + "<br>" + lblID.Text + "<br>" + count;
                Class.UpdateStatusConfirmOrder(lblID.Text);

                //}
                //   count++; 
            }


        }

        LoadData();
    }
}