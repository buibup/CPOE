using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DoctorOrder.Api
{
    public static class GlobalVariables
    {
        public static string Cache89 = ConfigurationManager.ConnectionStrings["Cache89"].ToString();
        public static string Cache47 = ConfigurationManager.ConnectionStrings["Cache47"].ToString();
        public static string Cache49 = ConfigurationManager.ConnectionStrings["Cache49"].ToString();
        public static string ARCIM_Code = ConfigurationManager.AppSettings["ARCIM_Code"];
        public static string ARCIM_Code_Oneday = ConfigurationManager.AppSettings["ARCIM_Code_Oneday"];
        public static string ARCIM_Code_Continue = ConfigurationManager.AppSettings["ARCIM_Code_Continue"];
    }
}