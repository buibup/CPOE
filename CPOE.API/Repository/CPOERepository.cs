using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CPOE.API.Models;
using DoctorOrder.Api;
using CPOE.API.DA;
using CPOE.API.Common;

namespace CPOE.API.Repository
{
    public class CPOERepository : ICPOERepository
    {
        private string conString = GlobalVariables.Cache89;

        public List<Allergy> GetAllergys(string hn)
        {
            List<Allergy> results = new List<Allergy>();

            var cmdString = QueryString.GetAllergy(hn);
            var dtAlg = InterSystemsDA.DTBindDataCommand(cmdString, GlobalVariables.Cache89);

            results = Helper.DataTableToAllergy(dtAlg);

            return results;
        }

        public PatientOrder GetPatientOrder(string epiRowId)
        {
            PatientOrder ptOrder = new PatientOrder();

            var dtOneDay = InterSystemsDA.DTBindDataCommand(QueryString.GetOrders(epiRowId, "OneDay"), conString);
            var dtContinue = InterSystemsDA.DTBindDataCommand(QueryString.GetOrders(epiRowId, "Continue"), conString);

            ptOrder = Helper.DataTableToPatientOrder(epiRowId, dtOneDay, dtContinue);

            return ptOrder;
        }

        public PatientOrder GetPatientOrder(string epiRowId, string dateFrom, string dateTo)
        {
            PatientOrder ptOrder = new PatientOrder();

            var dtOneDay = InterSystemsDA.DTBindDataCommand(QueryString.GetOrders(epiRowId, dateFrom, dateTo, "OneDay"), conString);
            var dtContinue = InterSystemsDA.DTBindDataCommand(QueryString.GetOrders(epiRowId, dateFrom, dateTo, "Continue"), conString);

            ptOrder = Helper.DataTableToPatientOrder(epiRowId, dtOneDay, dtContinue);

            return ptOrder;
        }
    }
}