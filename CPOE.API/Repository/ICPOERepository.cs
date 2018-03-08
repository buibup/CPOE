using CPOE.API.Models;
using System.Collections.Generic;

namespace CPOE.API.Repository
{
    interface ICPOERepository
    {
        PatientOrder GetPatientOrder(string epiRowId);
        PatientOrder GetPatientOrder(string epiRowId, string dateFrom, string dateTo);
        List<Allergy> GetAllergys(string hn);
    }
}
