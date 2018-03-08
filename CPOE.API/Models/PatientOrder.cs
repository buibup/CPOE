using System.Collections.Generic;

namespace CPOE.API.Models
{
    public class PatientOrder
    {
        public Patient Patient { get; set; }
        public List<Order> OneDay { get; set; }
        public List<Order> Continue { get; set; }
        
    }
}