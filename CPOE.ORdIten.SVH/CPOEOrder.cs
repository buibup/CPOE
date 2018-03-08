using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CPOEORdItem.ClassEn;
using CPOEORdIten.ModelEn;

namespace CPOEORdItem
{
    public partial class CPOE : Form
    {
        public CPOE()
        {
            InitializeComponent();
        }
        private string SiteStr = ConfigurationManager.ConnectionStrings["Site"].ToString();
        private DataTable dtEpi = new DataTable();
        private DataTable dtOrder = new DataTable();
        private DataTable dtOrderSQL = new DataTable();
        private clsTrakCare TrakC = new clsTrakCare();
        private clsSQL SQLC = new clsSQL();
        private OrderItemModel model = new OrderItemModel();
        private void CPOE_Load(object sender, EventArgs e)
        {
            String SqlCommand = "";

            if (SiteStr != null)
            {
                if (SiteStr.Trim().ToUpper() == "SVH")
                {
                    SqlCommand = @"SELECT PAADM_RowID
                                FROM pa_adm where  PAADM_CurrentWard_DR->WARD_Code
                                like '11%' and PAADM_Type like 'I%'  and  PAADM_VisitStatus like 'A%'  and  PAADM_CurrentRoom_DR->ROOM_Code <> '11FN' ";
                }
                else if (SiteStr.Trim().ToUpper() == "SNH")
                {
                    SqlCommand = @"SELECT PAADM_RowID  FROM pa_adm where PAADM_Type like 'I%' and  PAADM_CurrentWard_DR->WARD_Code
                                like '12%' and  PAADM_VisitStatus like 'A%'     and  PAADM_CurrentRoom_DR->ROOM_Code <> '12FN'";
                }
                string QueryOrder = "";
                // select Episode Rowid in Current Ward
                dtEpi = TrakC.GetDataTrak(SqlCommand);
                if (dtEpi.Rows.Count > 0)
                {
                    foreach (DataRow row in dtEpi.Rows)
                    {

                        if (!string.IsNullOrEmpty(row["PAADM_RowID"].ToString()))
                        {

                            QueryOrder = "";

                            //Check Max Data In sql
                            QueryOrder = @"SELECT top 1 OEORI_SttDat,OEORI_SttTim,OEORI_PrescSeqNo,OEORI_PrescNo,
                            OEORI_RowId from OrderItem where  Epi ='" + row["PAADM_RowID"].ToString().Trim() +
                            "'   and    ((OEORI_SttDat >= CONVERT(DATETIME, '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "', 102)) and    (OEORI_SttDat <= CONVERT(DATETIME, '" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "', 102)))     order by OEORI_SttDat desc ,OEORI_SttTim desc,OEORI_RowId desc,OEORI_PrescSeqNo desc";
                            dtOrderSQL = new DataTable();
                            dtOrderSQL = SQLC.GetDataSQL(QueryOrder);

                            // ถ้ามีข้อมูล Episode นี้..ใน  Select จาก Trackcare แค่ ข้อมูลที่อัพเดท
                            QueryOrder = "";
                            if (dtOrderSQL.Rows.Count > 0)
                            {
                                //                                SELECT distinct OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_Desc,OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc,OEORD_Adm_DR->PAADM_CurrentBed_DR->BED_Code
                                //,OEORD_Adm_DR->PAADM_Rowid,OE_OrdItem->OEORI_SttDat,OE_OrdItem->OEORI_SttTim,OE_OrdItem->OEORI_PrescSeqNo,OE_OrdItem->OEORI_PrescNo,OE_OrdItem->OEORI_RowId,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_Desc, OE_OrdItem->OEORI_PhQtyOrd,OE_OrdItem->OEORI_Unit_DR->CTUOM_Code,OE_OrdItem->OEORI_Instr_DR->PHCIN_Desc1,OE_OrdItem->OEORI_PHFreq_DR->PHCFR_Desc1,OE_OrdItem->OEORI_Doctor_DR->CTPCP_Desc  FROM OE_Order where OEORD_Adm_DR->PAADM_Rowid = 16967351 and OE_OrdItem->OEORI_SttDat >= { d'2017-04-21'}
                                //                                and OE_OrdItem->OEORI_SttDat <= { d'2017-04-21'}
                                //                                and OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc <> 'D/C (Discontinued)'     order by OE_OrdItem->OEORI_SttDat  ,OE_OrdItem->OEORI_SttTim ,OE_OrdItem->OEORI_RowId ,OE_OrdItem->OEORI_PrescNo

                                if (!string.IsNullOrEmpty(dtOrderSQL.Rows[0]["OEORI_SttTim"].ToString()))
                                {
                                    //    QueryOrder = @"SELECT distinct OEORD_Adm_DR->PAADM_Rowid,OE_OrdItem->OEORI_SttDat,OE_OrdItem->OEORI_SttTim,OE_OrdItem->OEORI_PrescSeqNo,OE_OrdItem->OEORI_PrescNo,OE_OrdItem->OEORI_RowId,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_Desc, OE_OrdItem->OEORI_PhQtyOrd,OE_OrdItem->OEORI_Unit_DR->CTUOM_Code,OE_OrdItem->OEORI_Instr_DR->PHCIN_Desc1,OE_OrdItem->OEORI_PHFreq_DR->PHCFR_Desc1,OE_OrdItem->OEORI_Doctor_DR->CTPCP_Desc  FROM OE_Order " +
                                    //" where OEORD_Adm_DR->PAADM_Rowid='" + row["PAADM_RowID"].ToString().Trim() + "' and    OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc <>'D/C (Discontinued)' and    OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Code ='V' and OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_Desc='Medicine' and  OE_OrdItem->OEORI_SttTim >= '" + dtOrderSQL.Rows[0]["OEORI_SttTim"].ToString().Trim() + "'  and OE_OrdItem->OEORI_SttDat  >= {d'" + Convert.ToDateTime(dtOrderSQL.Rows[0]["OEORI_SttDat"]).ToString("yyyy-MM-dd") + "'}     and OE_OrdItem->OEORI_RowId >'" + dtOrderSQL.Rows[0]["OEORI_RowId"].ToString().Trim() + "'   order by OE_OrdItem->OEORI_SttDat  ,OE_OrdItem->OEORI_SttTim ,OE_OrdItem->OEORI_RowId ";

                                    QueryOrder = @"SELECT distinct OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_code,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_Desc,OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc,OEORD_Adm_DR->PAADM_CurrentBed_DR->BED_Code
                                    ,OEORD_Adm_DR->PAADM_Rowid,OE_OrdItem->OEORI_SttDat,OE_OrdItem->OEORI_SttTim,OE_OrdItem->OEORI_PrescSeqNo,OE_OrdItem->OEORI_PrescNo,OE_OrdItem->OEORI_RowId,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_Desc, OE_OrdItem->OEORI_PhQtyOrd,OE_OrdItem->OEORI_Unit_DR->CTUOM_Code,OE_OrdItem->OEORI_Instr_DR->PHCIN_Desc1,OE_OrdItem->OEORI_PHFreq_DR->PHCFR_Desc1,OE_OrdItem->OEORI_Doctor_DR->CTPCP_Desc,
                                     OEORD_Adm_DR->PAADM_CurrentWard_DR->WARD_Code,OEORD_Adm_DR->PAADM_Hospital_DR->HOSP_Code,OE_OrdItem->OEORI_Priority_DR->OECPR_Desc 
,OEORD_Adm_DR->PAADM_VisitStatus,OE_OrdItem->OEORI_ReturnDate as DCDate,OE_OrdItem->OEORI_ReturnTime as DCTime,OE_OrdItem->OEORI_UserAdd->SSUSR_Initials as AddBy_Code ,OE_OrdItem->OEORI_UserAdd->SSUSR_Name as AddBy_Name
,OE_OrdItem->OEORI_ReturnUser_DR->SSUSR_Initials as DCBy_Code
,OE_OrdItem->OEORI_ReturnUser_DR->SSUSR_Name as DCBy_Name ,OEORD_Adm_DR->PAADM_ADMNo as EN ,OEORD_Adm_DR->PAADM_PAPMI_DR->PAPMI_No as HN FROM OE_Order " +
                                   " where OEORD_Adm_DR->PAADM_Rowid='" + row["PAADM_RowID"].ToString().Trim() + "' and    OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc <>'D/C (Discontinued)'   and  OE_OrdItem->OEORI_SttTim >= '" + dtOrderSQL.Rows[0]["OEORI_SttTim"].ToString().Trim() + "'  and OE_OrdItem->OEORI_SttDat  >= {d'" + Convert.ToDateTime(dtOrderSQL.Rows[0]["OEORI_SttDat"]).ToString("yyyy-MM-dd") + "'}     and OE_OrdItem->OEORI_RowId >'" + dtOrderSQL.Rows[0]["OEORI_RowId"].ToString().Trim() + "'   order by OE_OrdItem->OEORI_SttDat  ,OE_OrdItem->OEORI_SttTim ,OE_OrdItem->OEORI_RowId ";
                                }
                                else
                                {
                                    //    QueryOrder = @"SELECT distinct OEORD_Adm_DR->PAADM_Rowid,OE_OrdItem->OEORI_SttDat,OE_OrdItem->OEORI_SttTim,OE_OrdItem->OEORI_PrescSeqNo,OE_OrdItem->OEORI_PrescNo,OE_OrdItem->OEORI_RowId,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_Desc, OE_OrdItem->OEORI_PhQtyOrd,OE_OrdItem->OEORI_Unit_DR->CTUOM_Code,OE_OrdItem->OEORI_Instr_DR->PHCIN_Desc1,OE_OrdItem->OEORI_PHFreq_DR->PHCFR_Desc1,OE_OrdItem->OEORI_Doctor_DR->CTPCP_Desc  FROM OE_Order " +
                                    //" where OEORD_Adm_DR->PAADM_Rowid='" + row["PAADM_RowID"].ToString().Trim() + "' and    OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc <>'D/C (Discontinued)' and    OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Code ='V' and OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_Desc='Medicine'   and OE_OrdItem->OEORI_SttDat  >= {d'" + Convert.ToDateTime(dtOrderSQL.Rows[0]["OEORI_SttDat"]).ToString("yyyy-MM-dd") + "'}  and    OE_OrdItem->OEORI_RowId >'" + dtOrderSQL.Rows[0]["OEORI_RowId"].ToString().Trim() + "'   order by OE_OrdItem->OEORI_SttDat  ,OE_OrdItem->OEORI_SttTim ,OE_OrdItem->OEORI_RowId ";
                                    QueryOrder = @"SELECT distinct OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_code,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_Desc,OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc,OEORD_Adm_DR->PAADM_CurrentBed_DR->BED_Code
                                ,OEORD_Adm_DR->PAADM_Rowid,OE_OrdItem->OEORI_SttDat,OE_OrdItem->OEORI_SttTim,OE_OrdItem->OEORI_PrescSeqNo,OE_OrdItem->OEORI_PrescNo,OE_OrdItem->OEORI_RowId,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_Desc, OE_OrdItem->OEORI_PhQtyOrd,OE_OrdItem->OEORI_Unit_DR->CTUOM_Code,OE_OrdItem->OEORI_Instr_DR->PHCIN_Desc1,OE_OrdItem->OEORI_PHFreq_DR->PHCFR_Desc1,OE_OrdItem->OEORI_Doctor_DR->CTPCP_Desc,
                                 OEORD_Adm_DR->PAADM_CurrentWard_DR->WARD_Code,OEORD_Adm_DR->PAADM_Hospital_DR->HOSP_Code,OE_OrdItem->OEORI_Priority_DR->OECPR_Desc 
,OEORD_Adm_DR->PAADM_VisitStatus,OE_OrdItem->OEORI_ReturnDate as DCDate,OE_OrdItem->OEORI_ReturnTime as DCTime,OE_OrdItem->OEORI_UserAdd->SSUSR_Initials as AddBy_Code ,OE_OrdItem->OEORI_UserAdd->SSUSR_Name as AddBy_Name
,OE_OrdItem->OEORI_ReturnUser_DR->SSUSR_Initials as DCBy_Code
,OE_OrdItem->OEORI_ReturnUser_DR->SSUSR_Name as DCBy_Name ,OEORD_Adm_DR->PAADM_ADMNo as EN ,OEORD_Adm_DR->PAADM_PAPMI_DR->PAPMI_No as HN  FROM OE_Order " +
                                 " where OEORD_Adm_DR->PAADM_Rowid='" + row["PAADM_RowID"].ToString().Trim() + "' and    OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc <>'D/C (Discontinued)'     and OE_OrdItem->OEORI_SttDat  >= {d'" + Convert.ToDateTime(dtOrderSQL.Rows[0]["OEORI_SttDat"]).ToString("yyyy-MM-dd") + "'}  and    OE_OrdItem->OEORI_RowId >'" + dtOrderSQL.Rows[0]["OEORI_RowId"].ToString().Trim() + "'   order by OE_OrdItem->OEORI_SttDat  ,OE_OrdItem->OEORI_SttTim ,OE_OrdItem->OEORI_RowId ";

                                }

                            }
                            else
                            {
                                // Get Order Item In trakcare
                                //QueryOrder = @"SELECT distinct OEORD_Adm_DR->PAADM_Rowid,OE_OrdItem->OEORI_SttDat,OE_OrdItem->OEORI_SttTim,OE_OrdItem->OEORI_PrescSeqNo,OE_OrdItem->OEORI_PrescNo,OE_OrdItem->OEORI_RowId,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_Desc, OE_OrdItem->OEORI_PhQtyOrd,OE_OrdItem->OEORI_Unit_DR->CTUOM_Code,OE_OrdItem->OEORI_Instr_DR->PHCIN_Desc1,OE_OrdItem->OEORI_PHFreq_DR->PHCFR_Desc1,OE_OrdItem->OEORI_Doctor_DR->CTPCP_Desc  FROM OE_Order where OEORD_Adm_DR->PAADM_Rowid=" + row["PAADM_RowID"].ToString().Trim() + " and    OE_OrdItem->OEORI_SttDat >= {d'"+ DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)+ "'}  and    OE_OrdItem->OEORI_SttDat <= {d'" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'} and   OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc <>'D/C (Discontinued)'  and OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_Desc='Medicine' and    OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Code ='V'  order by OE_OrdItem->OEORI_SttDat  ,OE_OrdItem->OEORI_SttTim ,OE_OrdItem->OEORI_RowId ,OE_OrdItem->OEORI_PrescNo ";
                                QueryOrder = @"SELECT distinct OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_code,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_Desc,OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc,OEORD_Adm_DR->PAADM_CurrentBed_DR->BED_Code
                                ,OEORD_Adm_DR->PAADM_Rowid,OE_OrdItem->OEORI_SttDat,OE_OrdItem->OEORI_SttTim,OE_OrdItem->OEORI_PrescSeqNo,OE_OrdItem->OEORI_PrescNo,OE_OrdItem->OEORI_RowId,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_Desc, OE_OrdItem->OEORI_PhQtyOrd,OE_OrdItem->OEORI_Unit_DR->CTUOM_Code,OE_OrdItem->OEORI_Instr_DR->PHCIN_Desc1,OE_OrdItem->OEORI_PHFreq_DR->PHCFR_Desc1,OE_OrdItem->OEORI_Doctor_DR->CTPCP_Desc,
                                 OEORD_Adm_DR->PAADM_CurrentWard_DR->WARD_Code,OEORD_Adm_DR->PAADM_Hospital_DR->HOSP_Code,OE_OrdItem->OEORI_Priority_DR->OECPR_Desc  
,OEORD_Adm_DR->PAADM_VisitStatus,OE_OrdItem->OEORI_ReturnDate as DCDate,OE_OrdItem->OEORI_ReturnTime as DCTime,OE_OrdItem->OEORI_UserAdd->SSUSR_Initials as AddBy_Code ,OE_OrdItem->OEORI_UserAdd->SSUSR_Name as AddBy_Name
,OE_OrdItem->OEORI_ReturnUser_DR->SSUSR_Initials as DCBy_Code
,OE_OrdItem->OEORI_ReturnUser_DR->SSUSR_Name as DCBy_Name,OEORD_Adm_DR->PAADM_ADMNo as EN ,OEORD_Adm_DR->PAADM_PAPMI_DR->PAPMI_No as HN
FROM OE_Order where OEORD_Adm_DR->PAADM_Rowid=" + row["PAADM_RowID"].ToString().Trim() + " and    OE_OrdItem->OEORI_SttDat >= {d'" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'}  and    OE_OrdItem->OEORI_SttDat <= {d'" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'} and   OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc <>'D/C (Discontinued)'     order by OE_OrdItem->OEORI_SttDat  ,OE_OrdItem->OEORI_SttTim ,OE_OrdItem->OEORI_RowId ,OE_OrdItem->OEORI_PrescNo ";
                            }



                            dtOrder = new DataTable();
                            dtOrder = TrakC.GetDataTrak(QueryOrder);
                            //Loop Insert Data to SQL
                            if (dtOrder.Rows.Count > 0)
                            {
                                foreach (DataRow rowOrderItm in dtOrder.Rows)
                                {
                                    QueryOrder = @"SELECT * from OrderItem where  Epi ='" + row["PAADM_RowID"].ToString().Trim() + "' and  OEORI_RowId ='" + rowOrderItm["OEORI_RowId"].ToString().Trim() + "'   order by OEORI_SttDat desc,OEORI_PrescNo desc,OEORI_PrescSeqNo desc";
                                    dtOrderSQL = new DataTable();
                                    dtOrderSQL = TrakC.GetDataTrak(QueryOrder);

                                    // ถ้าไม่เจอข้อมูลให้ ทำการ Save Data to SQL
                                    if (dtOrderSQL.Rows.Count <= 0)
                                    {
                                        model = new OrderItemModel();
                                        if (rowOrderItm["PAADM_RowID"] != null) { model.Epi = rowOrderItm["PAADM_RowID"].ToString().Trim(); }
                                        if (rowOrderItm["ARCIM_Desc"] != null) { model.ARCIM_Desc = rowOrderItm["ARCIM_Desc"].ToString().Trim(); }
                                        
                                        if (rowOrderItm["CTPCP_Desc"] != null) { model.CTPCP_Desc = rowOrderItm["CTPCP_Desc"].ToString().Trim(); }
                                        if (rowOrderItm["CTUOM_Code"] != null) { model.CTUOM_Code = rowOrderItm["CTUOM_Code"].ToString().Trim(); }
                                        if (rowOrderItm["OEORI_PhQtyOrd"] != null) { model.OEORI_PhQtyOrd = rowOrderItm["OEORI_PhQtyOrd"].ToString().Trim(); }
                                        if (rowOrderItm["OEORI_PrescNo"] != null) { model.OEORI_PrescNo = rowOrderItm["OEORI_PrescNo"].ToString().Trim(); }
                                        if (rowOrderItm["OEORI_PrescSeqNo"] != null) { model.OEORI_PrescSeqNo = rowOrderItm["OEORI_PrescSeqNo"].ToString().Trim(); }
                                        if (rowOrderItm["OEORI_RowId"] != null) { model.OEORI_RowId = rowOrderItm["OEORI_RowId"].ToString().Trim(); }
                                        if (rowOrderItm["OEORI_SttDat"] != null) { model.OEORI_SttDat = Convert.ToDateTime(rowOrderItm["OEORI_SttDat"]); }
                                        if (!String.IsNullOrEmpty(rowOrderItm["OEORI_SttTim"].ToString())) { model.OEORI_SttTim = rowOrderItm["OEORI_SttTim"].ToString().Trim(); }
                                        if (rowOrderItm["PHCFR_Desc1"] != null) { model.PHCFR_Desc1 = rowOrderItm["PHCFR_Desc1"].ToString().Trim(); }
                                        if (rowOrderItm["PHCIN_Desc1"] != null) { model.PHCIN_Desc1 = rowOrderItm["PHCIN_Desc1"].ToString().Trim(); }
                                        if (rowOrderItm["ORCAT_Code"] != null) { model.ORCAT_Code = rowOrderItm["ORCAT_Code"].ToString().Trim(); }
                                        if (rowOrderItm["ORCAT_Desc"] != null) { model.ORCAT_Desc = rowOrderItm["ORCAT_Desc"].ToString().Trim(); }
                                        if (rowOrderItm["OSTAT_Desc"] != null) { model.OSTAT_Desc = rowOrderItm["OSTAT_Desc"].ToString().Trim(); }
                                        if (rowOrderItm["BED_Code"] != null) { model.BED_Code = rowOrderItm["BED_Code"].ToString().Trim(); }

                                        if (rowOrderItm["PAADM_VisitStatus"] != null) { model.PAADM_VisitStatus = rowOrderItm["PAADM_VisitStatus"].ToString().Trim(); }
                                        if (rowOrderItm["AddBy_Code"] != null) { model.AddBy_Code = rowOrderItm["AddBy_Code"].ToString().Trim(); }
                                        if (rowOrderItm["AddBy_Name"] != null) { model.AddBy_Name = rowOrderItm["AddBy_Name"].ToString().Trim(); }
                                        if (rowOrderItm["DCBy_Code"] != null) { model.DCBy_Code = rowOrderItm["DCBy_Code"].ToString().Trim(); }
                                        if (rowOrderItm["DCBy_Name"] != null) { model.DCBy_Name = rowOrderItm["DCBy_Name"].ToString().Trim(); }
                                        if (rowOrderItm["EN"] != null) { model.EN = rowOrderItm["EN"].ToString().Trim(); }
                                        if (rowOrderItm["HN"] != null) { model.HN = rowOrderItm["HN"].ToString().Trim(); }
                                       

                                        if (rowOrderItm["WARD_Code"] != null) { model.WARD_Code = rowOrderItm["WARD_Code"].ToString().Trim(); }
                                        if (SiteStr.Trim().ToUpper() == "SVH")
                                        {
                                            model.HOSP_Code = "011";
                                        }
                                        else
                                        {
                                            model.HOSP_Code = "012";
                                        }

                                        if (rowOrderItm["OECPR_Desc"] != null) { model.OECPR_Desc = rowOrderItm["OECPR_Desc"].ToString().Trim(); }
                                        SQLC.InsertOrderItem(model);

                                    }


                                }
                            }

                            //Application.Exit();
                            // Check Data In Sql 
                            // QueryOrder = @"SELECT distinct OEORD_Adm_DR->PAADM_Rowid,OE_OrdItem->OEORI_SttDat,OE_OrdItem->OEORI_SttTim,OE_OrdItem->OEORI_PrescSeqNo,OE_OrdItem->OEORI_PrescNo,OE_OrdItem->OEORI_RowId,OE_OrdItem->OEORI_ItmMast_DR->ARCIM_Desc, OE_OrdItem->OEORI_PhQtyOrd,OE_OrdItem->OEORI_Unit_DR->CTUOM_Code,OE_OrdItem->OEORI_Instr_DR->PHCIN_Desc1,OE_OrdItem->OEORI_PHFreq_DR->PHCFR_Desc1,OE_OrdItem->OEORI_Doctor_DR->CTPCP_Desc  FROM OE_Order where OEORD_Adm_DR->PAADM_Rowid=" + row["PAADM_RowID"].ToString().Trim() + " and    OE_OrdItem->OEORI_ItemStat_DR->OSTAT_Desc <>'D/C (Discontinued)' and   OE_OrdItem->OEORI_Priority_DR->OECPR_Desc='Normal' and OE_OrdItem->OEORI_ItmMast_DR->ARCIM_ItemCat_DR->ARCIC_OrdCat_DR->ORCAT_Desc='Medicine' order by OE_OrdItem->OEORI_SttTim desc,OE_OrdItem->OEORI_SttDat desc ,OE_OrdItem->OEORI_PrescNo desc,OE_OrdItem->OEORI_SttTim,OE_OrdItem->OEORI_PrescSeqNo desc";





                        }


                    }

                    Application.Exit();
                }

                Application.Exit();

            }
        }
    }
}
