using System;
using System.Text.RegularExpressions;

namespace CPOE.API.Common
{
    public class QueryString
    {
        public static string GetOrders(string epiRowId, string type)
        {
            string OECPR_Desc = string.Empty;
            string OSTAT_Code = string.Empty;
            string query = string.Empty;
            switch (type.ToString().ToUpper())
            {
                case "ONEDAY":
                    //OECPR_Desc = "Normal";
                    OSTAT_Code = "'V','E'";
                    query = "";
                    break;
                case "CONTINUE":
                    //OECPR_Desc = "Standing";
                    OSTAT_Code = "'V','D','E'";
                    query = "and ((OEORI_ItemStat_DR->OSTAT_Code = 'D' and  OEORI_ReturnUser_DR->SSUSR_Name <> '') or (OEORI_ItemStat_DR->OSTAT_Code <> 'D' ))";
                    break;
                default:
                    break;
            }

            string querySring = @"

                select 
                OEORI_RowId
                ,OEORI_Date
                ,OEORI_ItmMast_DR->ARCIM_Code
                ,OEORI_ItmMast_DR->ARCIM_Desc 
                ,OEORI_AuthoriseClinician_DR->CTPCP_Desc AuthorisingClinician
                ,OEORI_Doctor_DR->CTPCP_Desc OrderingClinician
                ,OEORI_PhQtyOrd 
                ,OEORI_PHFreq_DR->PHCFR_Desc1 
                ,OEORI_PHFreq_DR->PHCFR_Desc2 
                ,OEORI_SttDat 
                ,OEORI_SttTim 
                ,OEORI_ItemStat_DR->OSTAT_Code	
                ,OEORI_ItemStat_DR->OSTAT_Desc
                ,OEORI_ReturnUser_DR->SSUSR_Initials DCUserCode
                ,OEORI_ReturnUser_DR->SSUSR_Name DCUserName
                ,OEORI_ReturnDate DCDate
                ,OEORI_ReturnTime DCTime
                ,OEORI_UserAdd->SSUSR_Initials AddUserCode
                ,OEORI_UserAdd->SSUSR_Name AddUserName
                ,OEORI_ItmMast_DR->ARCIM_BillSub_DR->ARCSG_ARCBG_ParRef->ARCBG_Code
                ,OEORI_UpdateDate
                ,OEORI_UpdateTime
                ,OEORI_Priority_DR->OECPR_Code
                ,OEORI_Priority_DR->OECPR_Desc
                from OE_OrdItem
                where OEORI_OEORD_ParRef->OEORD_Adm_DR->PAADM_RowID = '{epiRowId}'
                and OEORI_ItemStat_DR->OSTAT_Code in ({OSTAT_Code})
                and SUBSTRING(OEORI_Doctor_DR->CTPCP_Code,1,3) = '119'
                {query}
                order by OEORI_SttDat,OEORI_SttTim,OEORI_RowId

                ";
            // Drug -> ARCBG_Code = '00100000'
            // OEORI_Priority_DR->OECPR_Desc = '{OECPR_Desc}'

            querySring = querySring.Replace("{epiRowId}", epiRowId);
            querySring = querySring.Replace("{OECPR_Desc}", OECPR_Desc);
            querySring = querySring.Replace("{OSTAT_Code}", OSTAT_Code);
            querySring = querySring.Replace("{query}", query);

            return querySring;
        }

        public static string GetAllergy(string hn)
        {
            string alg = @"

                SELECT ALG_Comments
                ,ALG_PHCGE_DR->PHCGE_Name
                ,ALG_Type_DR->ALG_Desc
                ,ALG_AllergyGrp_DR->ALGR_Desc
                ,ALG_PHCDRGForm_DR->PHCDF_PHCD_ParRef->PHCD_Name
                ,ALG_Ingred_DR->INGR_Desc
                FROM PA_Allergy
                WHERE ALG_PAPMI_ParRef->PAPMI_No = '{hn}'
                and ALG_Status = 'A'
            ";

            alg = alg.Replace("{hn}", hn);

            return alg;
        }

        public static string GetOrders(string epiRowId, string dateFrom, string dateTo, string type)
        {

            dateFrom = Regex.Replace(dateFrom, @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");
            dateTo = Regex.Replace(dateTo, @"^(.{4})(.{2})(.{2})$", "$1-$2-$3");

            string OECPR_Desc = string.Empty;
            string OSTAT_Code = string.Empty;
            string query = string.Empty;
            switch (type.ToString().ToUpper())
            {
                case "ONEDAY":
                    OECPR_Desc = "Normal";
                    OSTAT_Code = "'V','E'";
                    query = "";
                    break;
                case "CONTINUE":
                    OECPR_Desc = "Standing";
                    OSTAT_Code = "'V','D','E'";
                    query = "or ( OEORI_ReturnUser_DR->SSUSR_Name <> '' and OEORI_ItemStat_DR->OSTAT_Code = 'D' )";
                    break;
                default:
                    break;
            }

            string querySring = @"

                select 
                OEORI_RowId
                ,OEORI_Date
                ,OEORI_ItmMast_DR->ARCIM_Code
                ,OEORI_ItmMast_DR->ARCIM_Desc 
                ,OEORI_AuthoriseClinician_DR->CTPCP_Desc AuthorisingClinician
                ,OEORI_Doctor_DR->CTPCP_Desc OrderingClinician
                ,OEORI_PhQtyOrd 
                ,OEORI_PHFreq_DR->PHCFR_Desc1 
                ,OEORI_PHFreq_DR->PHCFR_Desc2 
                ,OEORI_SttDat 
                ,OEORI_SttTim 
                ,OEORI_ItemStat_DR->OSTAT_Code	
                ,OEORI_ItemStat_DR->OSTAT_Desc
                ,OEORI_ReturnUser_DR->SSUSR_Initials DCUserCode
                ,OEORI_ReturnUser_DR->SSUSR_Name DCUserName
                ,OEORI_ReturnDate DCDate
                ,OEORI_ReturnTime DCTime
                ,OEORI_UserAdd->SSUSR_Initials AddUserCode
                ,OEORI_UserAdd->SSUSR_Name AddUserName
                ,OEORI_ItmMast_DR->ARCIM_BillSub_DR->ARCSG_ARCBG_ParRef->ARCBG_Code
                ,OEORI_UpdateDate
                ,OEORI_UpdateTime
                from OE_OrdItem
                where OEORI_OEORD_ParRef->OEORD_Adm_DR->PAADM_RowID = '{epiRowId}'
                and OEORI_Priority_DR->OECPR_Desc = '{OECPR_Desc}'
                and OEORI_ItemStat_DR->OSTAT_Code in ({OSTAT_Code})
                and SUBSTRING(OEORI_Doctor_DR->CTPCP_Code,1,3) = '119'
                {query}
                and OEORI_SttDat >= '{dateFrom}' and OEORI_SttDat <= '{dateTo}'
                order by OEORI_SttDat,OEORI_SttTim,OEORI_RowId
            ";

            querySring = querySring.Replace("{epiRowId}", epiRowId);
            querySring = querySring.Replace("{OECPR_Desc}", OECPR_Desc);
            querySring = querySring.Replace("{dateFrom}", dateFrom);
            querySring = querySring.Replace("{dateTo}", dateTo);
            querySring = querySring.Replace("{OSTAT_Code}", OSTAT_Code);
            querySring = querySring.Replace("{query}", query);

            return querySring;
        }

        public static string GetPatient(string epiRowId)
        {
            string queryString = @"

                select PAADM_PAPMI_DR->PAPMI_No
                ,PAADM_PAPMI_DR->PAPMI_Title_DR->TTL_Desc
                ,PAADM_PAPMI_DR->PAPMI_Name
                ,PAADM_PAPMI_DR->PAPMI_Name2
                ,PAADM_PAPMI_DR->PAPMI_Sex_DR->CTSEX_Desc
                ,PAADM_PAPMI_DR->PAPMI_DOB
                ,PAADM_ADMNo
                ,PAADM_PAPMI_DR->PAPMI_PAPER_DR->PAPER_AgeYr
                from pa_adm
                where PAADM_RowID = '{epiRowId}'

            ";

            queryString = queryString.Replace("{epiRowId}", epiRowId);

            return queryString;
        }

        public static string GetQuesAns(string OEORI_RowId)
        {
            string query = @"
            select 
            OE_OrdQuestion->QA_Answer
            ,OE_OrdQuestion->QA_Question_DR->QUES_Code
            ,OE_OrdQuestion->QA_Question_DR->QUES_Desc
            ,OE_OrdQuestion->QA_Question_DR->QUES_ControlType
            ,OE_OrdQuestion->QA_Question_DR->QUES_Sequence
            from OE_OrdItem 
            where OEORI_RowId = '{OEORI_RowId}'
            order by OE_OrdQuestion->QA_Question_DR->QUES_Sequence
            ";

            query = query.Replace("{OEORI_RowId}", OEORI_RowId);

            return query;
        }

        public static string GetAllergyCategory(string algName)
        {
            string algCat = @"

                SELECT ALG_Type_DR->MRCAT_Desc
                FROM PAC_Allergy
                Where ALG_Desc like '{algName}%'

            ";

            algCat = algCat.Replace("{algName}", algName);

            return algCat;
        }

    }
}