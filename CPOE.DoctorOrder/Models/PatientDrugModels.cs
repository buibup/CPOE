using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Text;

namespace DoctorOrder.Web.Models
{
    public class PatientDrugModels
    {

        public class PatientDrugModel
        {
            public Patient Patient { get; set; }
            public Drug[] OneDay { get; set; }
            public Drug[] Continue { get; set; }
        }

        public class Patient
        {
            public string HN { get; set; }
            public string Name { get; set; }
            public string Gender { get; set; }
            public string DOB { get; set; }
            public string EpisodeNo { get; set; }
            public string Age { get; set; }
            public Allergys[] Allergys { get; set; }
        }

        public class Drug
        {
            public string OEORI_Date { get; set; }
            public string Service { get; set; }
            public QuestionAnswerModel[] QuestionAnswerModel { get; set; }
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

        public class Allergys
        {
            public string Name;
            public string Category;
        }

        public static PatientDrugModel GetPatientDrug(string epiRowId, string dataJsonTestPath)
        {
            /** get from api
            string api = "http://localhost:49250/api/PatientOrder/GetPatientDrug?epiRowId={epiRowId}";
            api = api.Replace("{epiRowId}", epiRowId);

            

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("content-type", "application/json");
                webClient.Encoding = Encoding.UTF8;
                string json = webClient.DownloadString(api);

                ptOrder = JsonConvert.DeserializeObject<PatientOrder>(json);
            }
             */


            string api = ConfigurationManager.AppSettings["WebAPI"];
            string json = "";
            PatientDrugModel ptDrug = new PatientDrugModel();

            ////Test json
            if (dataJsonTestPath != "")
            {
                json = System.IO.File.ReadAllText(dataJsonTestPath);
            }
            else
            {
                api = api.Replace("{epiRowId}", epiRowId);
                
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add("content-type", "application/json");
                    webClient.Encoding = Encoding.UTF8;
                    json = webClient.DownloadString(api);
                }
            }

            ptDrug = JsonConvert.DeserializeObject<PatientDrugModel>(json);

            return ptDrug;
        }
    }
}