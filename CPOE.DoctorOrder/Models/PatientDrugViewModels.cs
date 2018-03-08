using System.Collections.Generic;
using System.Linq;
using static DoctorOrder.Web.Models.PatientDrugModels;

namespace DoctorOrder.Web.Models
{
    public class PatientDrugViewModels
    {
        public PatientDrugModels.Patient Patient { get; set; }
        public List<DrugViewModel> DrugViewModel { get; set; }


        public static PatientDrugViewModels GetPatientDrugViewModel(PatientDrugModel PatientDrug)
        {
            PatientDrugViewModels ptDrugVM = new PatientDrugViewModels();
            List<DrugViewModel> drugVMList = new List<DrugViewModel>();

            ptDrugVM.Patient = PatientDrug.Patient;

            foreach (var item in PatientDrug.OneDay)
            {
                DrugViewModel drugVM = new DrugViewModel();
                drugVM.OEORI_Date = item.OEORI_Date;
                drugVM.Service = item.Service;
                drugVM.QuestionAnswerModel = item.QuestionAnswerModel;
                drugVM.Qty = item.Qty;
                drugVM.Dose = item.Dose;
                drugVM.StartDate = item.StartDate;
                drugVM.StartTime = item.StartTime;
                drugVM.OrderingClinician = item.OrderingClinician;
                drugVM.AuthorisingClinician = item.AuthorisingClinician;
                drugVM.DCUserCode = item.DCUserCode;
                drugVM.DCUserName = item.DCUserName;
                drugVM.DCDate = item.DCDate;
                drugVM.DCTime = item.DCTime;
                drugVM.AddUserCode = item.AddUserCode;
                drugVM.AddUserName = item.AddUserName;
                drugVM.OSTAT_Code = item.OSTAT_Code;
                drugVM.OSTAT_Desc = item.OSTAT_Desc;
                drugVM.Type = "OneDay";

                if (item.OSTAT_Code == "D")
                {
                    drugVM.TimeLineDate = item.DCDate;
                    drugVM.TimeLineDateTime = item.DCTime;
                }
                else if (item.OSTAT_Code == "V")
                {
                    drugVM.TimeLineDate = item.StartDate;
                    drugVM.TimeLineDateTime = item.StartTime;
                }
                else if (item.OSTAT_Code == "E")
                {
                    drugVM.TimeLineDate = item.OEORI_UpdateDate;
                    drugVM.TimeLineDateTime = item.OEORI_UpdateTime;
                }

                drugVMList.Add(drugVM);
            }

            foreach (var item in PatientDrug.Continue)
            {
                DrugViewModel drugVM = new DrugViewModel();
                drugVM.OEORI_Date = item.OEORI_Date;
                drugVM.Service = item.Service;
                drugVM.QuestionAnswerModel = item.QuestionAnswerModel;
                drugVM.Qty = item.Qty;
                drugVM.Dose = item.Dose;
                drugVM.StartDate = item.StartDate;
                drugVM.StartTime = item.StartTime;
                drugVM.OrderingClinician = item.OrderingClinician;
                drugVM.AuthorisingClinician = item.AuthorisingClinician;
                drugVM.DCUserCode = item.DCUserCode;
                drugVM.DCUserName = item.DCUserName;
                drugVM.DCDate = item.DCDate;
                drugVM.DCTime = item.DCTime;
                drugVM.AddUserCode = item.AddUserCode;
                drugVM.AddUserName = item.AddUserName;
                drugVM.OSTAT_Code = item.OSTAT_Code;
                drugVM.OSTAT_Desc = item.OSTAT_Desc;
                drugVM.Type = "Continue";

                if (item.OSTAT_Code == "D")
                {
                    drugVM.TimeLineDate = item.DCDate;
                    drugVM.TimeLineDateTime = item.DCTime;
                }
                else if(item.OSTAT_Code == "V")
                {
                    drugVM.TimeLineDate = item.StartDate;
                    drugVM.TimeLineDateTime = item.StartTime;
                }
                else if (item.OSTAT_Code == "E")
                {
                    drugVM.TimeLineDate = item.OEORI_UpdateDate;
                    drugVM.TimeLineDateTime = item.OEORI_UpdateTime;
                }

                drugVMList.Add(drugVM);
            }
            
            var drugVMListSortDate = drugVMList.OrderBy(d => d.TimeLineDate).ThenBy(d => d.TimeLineDateTime).ToList();

            ptDrugVM.DrugViewModel = drugVMListSortDate;


            return ptDrugVM;
        }

        public static PatientDrugViewModels GetPatientDrugViewModel(PatientDrugViewModels ptDrugVM)
        {
            PatientDrugViewModels result = new PatientDrugViewModels();

            result.Patient = ptDrugVM.Patient;

            foreach (var item in ptDrugVM.DrugViewModel)
            {
                DrugViewModel drugVM = new DrugViewModel();
            }

            return result;
        }
    }

    public class DrugViewModel
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
        public string Type { get; set; }
        public string TimeLineDate { get; set; }
        public string TimeLineDateTime { get; set; }
    }

    public class QuestionAnswerModel
    {
        public string Head { get; set; }
        public string Title { get; set; }
        public string Ans { get; set; }
    }


}