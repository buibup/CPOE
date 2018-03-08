using CPOE.API.Models;
using CPOE.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CPOE.API.Controllers
{
    public class PatientsController : ApiController
    {
        private ICPOERepository _ICPOERepository = new CPOERepository();
        public List<Allergy> GetAllergys(string hn)
        {
            var model = _ICPOERepository.GetAllergys(hn);
            if(model == null)
            {
                return new List<Allergy>();
            }
            return model;
        }
    }
}
