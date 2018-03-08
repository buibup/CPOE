using CPOE.API.Models;
using CPOE.API.Repository;
using System.Web.Http;

namespace CPOE.API.Controllers
{
    public class OrdersController : ApiController
    {
        private ICPOERepository _ICPOERepository = new CPOERepository();

        [HttpGet]
        public PatientOrder GetPatientOrder(string epiRowId)
        {

            var model = _ICPOERepository.GetPatientOrder(epiRowId);

            if (model == null)
            {
                return new PatientOrder();
            }

            return model;
        }

        [HttpGet]
        public PatientOrder GetPatientOrder(string epiRowId, string dateFrom, string dateTo)
        {

            var model = _ICPOERepository.GetPatientOrder(epiRowId, dateFrom, dateTo);

            if (model == null)
            {
                return new PatientOrder();
            }

            return model;
        }
    }
}
