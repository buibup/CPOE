using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CPOEORdIten.ModelEn
{
    public class OrderItemModel
    {
        public String Epi { get; set; }
        public int re_cno { get; set; }
        public DateTime? OEORI_SttDat { get; set; }
        public String OEORI_SttTim { get; set; }
        public String OEORI_PrescSeqNo { get; set; }
        public String OEORI_PrescNo { get; set; }
        public String OEORI_RowId { get; set; }
        public String ARCIM_Desc { get; set; }
        public String OEORI_PhQtyOrd { get; set; }
        public String CTUOM_Code { get; set; }
        public String PHCIN_Desc1 { get; set; }
        public String PHCFR_Desc1 { get; set; }
        public String CTPCP_Desc { get; set; }
        public String StatusConfirm { get; set; }
        public String ORCAT_Code { get; set; }
        public String ORCAT_Desc { get; set; }
        public String OSTAT_Desc { get; set; }
        public String BED_Code { get; set; }


        public String WARD_Code { get; set; }
        public String HOSP_Code { get; set; }
        public String OECPR_Desc { get; set; }

   

        public String PAADM_VisitStatus { get; set; }

        public String DCBy_Code { get; set; }
        public String DCBy_Name { get; set; }

        public String AddBy_Code { get; set; }
        public String AddBy_Name { get; set; }
        public String HN { get; set; }
        public String EN { get; set; }


        public String OEORI_SttDatString
        {
            get
            {
                if (OEORI_SttDat != null)
                {
                    return OEORI_SttDat.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                return "";
            }
        }

    }
}
