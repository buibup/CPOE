using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class frmPopup : System.Web.UI.Page
{
    Cclass Class = new Cclass();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            String param_ = Request.QueryString["rowid"];
            DataTable dt1 = new DataTable();
            dt1 = Class.GetOrderNew(param_);
            grid.DataSource = dt1;
            grid.DataBind();
            if (dt1.Rows.Count > 5)
            {
                divDetail.Attributes.Add("style", "height:300px;overflow-y:scroll;width: 100%;");
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            String param_id = "";

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                Label lblId = (Label)grid.Rows[i].FindControl("lblOrderID");
                if (i != 0)
                {
                    param_id += "','";
                }
                param_id += lblId.Text;
            }
            if (Class.UpdateStatusOrder(param_id) == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "alert", "alert('Confirm Success');window.close();window.opener.location.reload();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "alert", "alert('Error Please contact IT.');", true);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
}