using QuestionaireAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPOE.API.Models
{
    public class Order
    {
        public string OEORI_RowId { get; set; }
        public string OEORI_Date { get; set; }
        public string ARCIM_Code { get; set; }
        public string Service { get; set; }
        public List<Questionaire> QuestionAnswerModel { get; set; }
        public string Qty { get; set; }
        public string Dose { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string OrderingClinician { get; set; }
        public string AuthorisingClinician { get; set; }
        public string DCUserCode { get; set; }
        public string DCUserName { get; set; }
        public string DCDate { get; set; }
        public string DCTime { get; set; }
        public string AddUserCode { get; set; }
        public string AddUserName { get; set; }
        public string OSTAT_Code { get; set; }
        public string OSTAT_Desc { get; set; }
        public string OEORI_UpdateDate { get; set; }
        public string OEORI_UpdateTime { get; set; }
    }
}