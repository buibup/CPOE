using System.Collections.Generic;
using static DoctorOrder.Web.Models.PatientDrugModels;

namespace DoctorOrder.Web.Models
{
    public class DataViewModels
    {
        public Patient Patient { get; set; }

    }

    public class Data
    {
        public string StartDate { get; set; }
        List<DataTime> DataTimeList { get; set; }
    }

    public class DataTime
    {
        public string StartTime { get; set; }
        public DrugOneDay DrugOneDay { get; set; }
        public DrugContinue DrugContinue { get; set; }
    }

    public class DrugOneDay
    {
        public Drug Drug { get; set; }
    }

    public class DrugContinue
    {
        public Drug Drug { get; set; }
    }
}