using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPOE.API.Models
{
    public class Patient
    {
        public string HN { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string EpisodeNo { get; set; }
        public string Age { get; set; }
        public List<Allergy> Allergys { get; set; }
    }
}