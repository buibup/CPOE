using System;
using System.Collections.Generic;
using System.Linq;
using static DoctorOrder.Web.Models.PatientDrugModels;

namespace DoctorOrder.Web.Models
{
    public class PatientDrugGroupViewModel
    {
        public Patient Patient { get; set; }
        public List<DrugDate> DrugDateList { get; set; } = new List<DrugDate>();

        public static PatientDrugGroupViewModel patientDrugGroupViewModel { get; set; } = new PatientDrugGroupViewModel();



        //public void ToPatientDrugGroupViewModel(PatientDrugViewModels patientDrugViewModels)
        //{
        //    List<DrugDate> drgDteList = new List<DrugDate>();
        //    DrugDate drugDate;

        //    DrugTime drugTime;

        //    patientDrugGroupViewModel.Patient = patientDrugViewModels.Patient;

        //    string date = string.Empty;
        //    string time = string.Empty;

        //    //var disDates = patientDrugViewModels.DrugViewModel.Select(m => m.StartDate).Distinct();
        //    //var disTimes = patientDrugViewModels.DrugViewModel.Select(m => new { m.StartDate, m.StartTime }).Distinct();

        //    var disDates = patientDrugViewModels.DrugViewModel.Select(m => m.TimeLineDate).Distinct();
        //    var disTimes = patientDrugViewModels.DrugViewModel.Select(m => new { m.TimeLineDate, m.TimeLineDateTime }).Distinct();

        //    foreach (var disDate in disDates)
        //    {
        //        drugDate = new DrugDate();
        //        List<DrugTime> drgTmeList = new List<DrugTime>();
        //        drgDteList.Clear();
        //        drgTmeList.Clear();
        //        foreach (var disTime in disTimes)
        //        {
        //            if(disDate == disTime.TimeLineDate)
        //            {
        //                drugTime = new DrugTime();
        //                drugTime.StartTime = disTime.TimeLineDateTime;
        //                drgTmeList.Add(drugTime);

                        
        //                List<Drug> drugOneDayList = new List<Drug>();
        //                List<Drug> drugContinueList = new List<Drug>();
        //                drugOneDayList.Clear();
        //                drugContinueList.Clear();

        //                foreach (var item in patientDrugViewModels.DrugViewModel)
        //                {
        //                    if(disDate == item.TimeLineDate & disTime.TimeLineDateTime == item.TimeLineDateTime)
        //                    {
        //                        Drug drug = new Drug();

        //                        if(item.Type.ToUpper() == "ONEDAY")
        //                        {
        //                            drug.OEORI_Date = item.OEORI_Date;
        //                            drug.Service = item.Service;
        //                            drug.QuestionAnswerModel = item.QuestionAnswerModel;
        //                            drug.Qty = item.Qty;
        //                            drug.Dose = item.Dose;
        //                            drug.StartDate = item.StartDate;
        //                            drug.StartTime = item.StartTime;
        //                            drug.OrderingClinician = item.OrderingClinician;
        //                            drug.AuthorisingClinician = item.AuthorisingClinician;
        //                            drug.DCUserCode = item.DCUserCode;
        //                            drug.DCUserName = item.DCUserName;
        //                            drug.DCDate = item.DCDate;
        //                            drug.DCTime = item.DCTime;
        //                            drug.AddUserCode = item.AddUserCode;
        //                            drug.AddUserName = item.AddUserName;

        //                            drug.OSTAT_Code = item.OSTAT_Code;
        //                            drug.OSTAT_Desc = item.OSTAT_Desc;
                                    
        //                            drugOneDayList.Add(drug);
        //                        }
        //                        if (item.Type.ToUpper() == "CONTINUE")
        //                        {
        //                            drug.OEORI_Date = item.OEORI_Date;
        //                            drug.Service = item.Service;
        //                            drug.QuestionAnswerModel = item.QuestionAnswerModel;
        //                            drug.Qty = item.Qty;
        //                            drug.Dose = item.Dose;
        //                            drug.StartDate = item.StartDate;
        //                            drug.StartTime = item.StartTime;
        //                            drug.OrderingClinician = item.OrderingClinician;
        //                            drug.AuthorisingClinician = item.AuthorisingClinician;
        //                            drug.DCUserCode = item.DCUserCode;
        //                            drug.DCUserName = item.DCUserName;
        //                            drug.DCDate = item.DCDate;
        //                            drug.DCTime = item.DCTime;
        //                            drug.AddUserCode = item.AddUserCode;
        //                            drug.AddUserName = item.AddUserName;

        //                            drug.OSTAT_Code = item.OSTAT_Code;
        //                            drug.OSTAT_Desc = item.OSTAT_Desc;

        //                            if (item.OSTAT_Code == "D")
        //                            {
        //                                string[] oeoriDate = item.OEORI_Date.Split('/');
        //                                int iOeoriDate = Convert.ToInt32(oeoriDate[2] + oeoriDate[1] + oeoriDate[0]);

        //                                string[] dcDate = item.DCDate.Split('/');
        //                                int iDcDate = Convert.ToInt32(dcDate[2] + dcDate[1] + dcDate[0]);

        //                                if (iOeoriDate >= iDcDate)
        //                                    drug.OSTAT_Desc = "Cancel";
        //                            }

        //                            drugContinueList.Add(drug);
        //                        }
        //                    }
        //                }
        //                drugTime.OneDay = drugOneDayList;
        //                drugTime.Continue = drugContinueList;
        //            }


        //        }
        //        drugDate.DrugTime = drgTmeList;
        //        drugDate.StartDate = disDate;
        //        AddPatientDrugGroupViewModel(drugDate);
        //    }

        //}

        public void ToOrderGroupViewModel(PatientDrugViewModels patientDrugViewModels)
        {
            patientDrugGroupViewModel.Patient = patientDrugViewModels.Patient;
            
            var disDates = patientDrugViewModels.DrugViewModel.Where(w => !String.IsNullOrEmpty(w.TimeLineDate)).Select(m => m.TimeLineDate).Distinct();

            /************************/
            // Date Loop
            /************************/
            List<DrugDate> drgDteList = new List<DrugDate>();
            foreach (var objDate in disDates)
            {
                DrugDate drugDate = new DrugDate();

                /************************/
                // Time Loop
                /************************/
                List<DrugTime> lsDrugTime = new List<DrugTime>();
                foreach (var objTime in patientDrugViewModels.DrugViewModel
                                            .Where(w => w.TimeLineDate == objDate)
                                            .Select(s => new { s.TimeLineDate, s.TimeLineDateTime }).Distinct())
                {
                    DrugTime drugTime = new DrugTime();

                    Status OneDay = new Status();
                    Status Continue = new Status();

                    /************************/
                    // Type Loop ('ONEDAY')
                    /************************/
                    List<Drug> lsVerify = new List<Drug>();
                    List<Drug> lsCancel = new List<Drug>();
                    List<Drug> lsDiscontinue = new List<Drug>();
                    List<Drug> lsExecute = new List<Drug>();
                    foreach (var objType in patientDrugViewModels.DrugViewModel
                                            .Where(w => w.TimeLineDate == objTime.TimeLineDate 
                                                        && w.TimeLineDateTime == objTime.TimeLineDateTime
                                                        && w.Type.ToUpper() == "ONEDAY")
                                            .Select(s => new { s.TimeLineDate, s.TimeLineDateTime, s.Type }).Distinct())
                    {
                        foreach (var objStatus in patientDrugViewModels.DrugViewModel
                                                    .Where(w => w.TimeLineDate == objType.TimeLineDate
                                                                && w.TimeLineDateTime == objType.TimeLineDateTime
                                                                && w.Type.ToUpper() == objType.Type.ToUpper()
                                                                && w.OSTAT_Code == "V"))
                        {
                            Drug drug = new Drug();
                            drug.OEORI_Date = objStatus.OEORI_Date;
                            drug.Service = objStatus.Service;
                            drug.QuestionAnswerModel = objStatus.QuestionAnswerModel;
                            drug.Qty = objStatus.Qty;
                            drug.Dose = objStatus.Dose;
                            drug.StartDate = objStatus.StartDate;
                            drug.StartTime = objStatus.StartTime;
                            drug.OrderingClinician = objStatus.OrderingClinician;
                            drug.AuthorisingClinician = objStatus.AuthorisingClinician;
                            drug.DCUserCode = objStatus.DCUserCode;
                            drug.DCUserName = objStatus.DCUserName;
                            drug.DCDate = objStatus.DCDate;
                            drug.DCTime = objStatus.DCTime;
                            drug.AddUserCode = objStatus.AddUserCode;
                            drug.AddUserName = objStatus.AddUserName;

                            drug.OSTAT_Code = objStatus.OSTAT_Code;
                            drug.OSTAT_Code = objStatus.OSTAT_Desc;
                            lsVerify.Add(drug);
                        }
                        
                        foreach (var objStatus in patientDrugViewModels.DrugViewModel
                                                    .Where(w => w.TimeLineDate == objType.TimeLineDate
                                                                && w.TimeLineDateTime == objType.TimeLineDateTime
                                                                && w.Type.ToUpper() == objType.Type.ToUpper()
                                                                && w.OSTAT_Code == "D"))
                        {
                            Drug drug = new Drug();
                            drug.OEORI_Date = objStatus.OEORI_Date;
                            drug.Service = objStatus.Service;
                            drug.QuestionAnswerModel = objStatus.QuestionAnswerModel;
                            drug.Qty = objStatus.Qty;
                            drug.Dose = objStatus.Dose;
                            drug.StartDate = objStatus.StartDate;
                            drug.StartTime = objStatus.StartTime;
                            drug.OrderingClinician = objStatus.OrderingClinician;
                            drug.AuthorisingClinician = objStatus.AuthorisingClinician;
                            drug.DCUserCode = objStatus.DCUserCode;
                            drug.DCUserName = objStatus.DCUserName;
                            drug.DCDate = objStatus.DCDate;
                            drug.DCTime = objStatus.DCTime;
                            drug.AddUserCode = objStatus.AddUserCode;
                            drug.AddUserName = objStatus.AddUserName;

                            drug.OSTAT_Code = objStatus.OSTAT_Code;

                            string[] oeoriDate = objStatus.OEORI_Date.Split('/');
                            int iOeoriDate = Convert.ToInt32(oeoriDate[2] + oeoriDate[1] + oeoriDate[0]);

                            string[] dcDate = objStatus.DCDate.Split('/');
                            int iDcDate = Convert.ToInt32(dcDate[2] + dcDate[1] + dcDate[0]);

                            if (iOeoriDate >= iDcDate) {
                                // Cancel
                                drug.OSTAT_Desc = "Cancel";
                                lsCancel.Add(drug);
                            }
                            else
                            {
                                // Discontinue
                                drug.OSTAT_Desc = objStatus.OSTAT_Desc;
                                lsDiscontinue.Add(drug);
                            }
                        }
                        
                        foreach (var objStatus in patientDrugViewModels.DrugViewModel
                                                    .Where(w => w.TimeLineDate == objType.TimeLineDate
                                                                && w.TimeLineDateTime == objType.TimeLineDateTime
                                                                && w.Type.ToUpper() == objType.Type.ToUpper()
                                                                && w.OSTAT_Code == "E"))
                        {
                            Drug drug = new Drug();
                            drug.OEORI_Date = objStatus.OEORI_Date;
                            drug.Service = objStatus.Service;
                            drug.QuestionAnswerModel = objStatus.QuestionAnswerModel;
                            drug.Qty = objStatus.Qty;
                            drug.Dose = objStatus.Dose;
                            drug.StartDate = objStatus.StartDate;
                            drug.StartTime = objStatus.StartTime;
                            drug.OrderingClinician = objStatus.OrderingClinician;
                            drug.AuthorisingClinician = objStatus.AuthorisingClinician;
                            drug.DCUserCode = objStatus.DCUserCode;
                            drug.DCUserName = objStatus.DCUserName;
                            drug.DCDate = objStatus.DCDate;
                            drug.DCTime = objStatus.DCTime;
                            drug.AddUserCode = objStatus.AddUserCode;
                            drug.AddUserName = objStatus.AddUserName;

                            drug.OSTAT_Code = objStatus.OSTAT_Code;
                            drug.OSTAT_Code = objStatus.OSTAT_Desc;
                            lsExecute.Add(drug);
                        }
                    }

                    OneDay.lsVerify = lsVerify;
                    OneDay.lsCancel = lsCancel;
                    OneDay.lsDiscontinue = lsDiscontinue;
                    OneDay.lsExecute = lsExecute;

                    /************************/
                    // Type Loop ('CONTINUE')
                    /************************/
                    List<Drug> lsContinueVerify = new List<Drug>();
                    List<Drug> lsContinueCancel = new List<Drug>();
                    List<Drug> lsContinueDiscontinue = new List<Drug>();
                    List<Drug> lsContinueExecute = new List<Drug>();
                    foreach (var objType in patientDrugViewModels.DrugViewModel
                                            .Where(w => w.TimeLineDate == objTime.TimeLineDate
                                                        && w.TimeLineDateTime == objTime.TimeLineDateTime
                                                        && w.Type.ToUpper() == "CONTINUE")
                                            .Select(s => new { s.TimeLineDate, s.TimeLineDateTime, s.Type }).Distinct())
                    {
                        foreach (var objStatus in patientDrugViewModels.DrugViewModel
                                                    .Where(w => w.TimeLineDate == objType.TimeLineDate
                                                                && w.TimeLineDateTime == objType.TimeLineDateTime
                                                                && w.Type.ToUpper() == objType.Type.ToUpper()
                                                                && w.OSTAT_Code == "V"))
                        {
                            Drug drug = new Drug();
                            drug.OEORI_Date = objStatus.OEORI_Date;
                            drug.Service = objStatus.Service;
                            drug.QuestionAnswerModel = objStatus.QuestionAnswerModel;
                            drug.Qty = objStatus.Qty;
                            drug.Dose = objStatus.Dose;
                            drug.StartDate = objStatus.StartDate;
                            drug.StartTime = objStatus.StartTime;
                            drug.OrderingClinician = objStatus.OrderingClinician;
                            drug.AuthorisingClinician = objStatus.AuthorisingClinician;
                            drug.DCUserCode = objStatus.DCUserCode;
                            drug.DCUserName = objStatus.DCUserName;
                            drug.DCDate = objStatus.DCDate;
                            drug.DCTime = objStatus.DCTime;
                            drug.AddUserCode = objStatus.AddUserCode;
                            drug.AddUserName = objStatus.AddUserName;

                            drug.OSTAT_Code = objStatus.OSTAT_Code;
                            drug.OSTAT_Code = objStatus.OSTAT_Desc;
                            lsContinueVerify.Add(drug);
                        }
                        
                        foreach (var objStatus in patientDrugViewModels.DrugViewModel
                                                    .Where(w => w.TimeLineDate == objType.TimeLineDate
                                                                && w.TimeLineDateTime == objType.TimeLineDateTime
                                                                && w.Type.ToUpper() == objType.Type.ToUpper()
                                                                && w.OSTAT_Code == "D"))
                        {
                            Drug drug = new Drug();
                            drug.OEORI_Date = objStatus.OEORI_Date;
                            drug.Service = objStatus.Service;
                            drug.QuestionAnswerModel = objStatus.QuestionAnswerModel;
                            drug.Qty = objStatus.Qty;
                            drug.Dose = objStatus.Dose;
                            drug.StartDate = objStatus.StartDate;
                            drug.StartTime = objStatus.StartTime;
                            drug.OrderingClinician = objStatus.OrderingClinician;
                            drug.AuthorisingClinician = objStatus.AuthorisingClinician;
                            drug.DCUserCode = objStatus.DCUserCode;
                            drug.DCUserName = objStatus.DCUserName;
                            drug.DCDate = objStatus.DCDate;
                            drug.DCTime = objStatus.DCTime;
                            drug.AddUserCode = objStatus.AddUserCode;
                            drug.AddUserName = objStatus.AddUserName;

                            drug.OSTAT_Code = objStatus.OSTAT_Code;

                            string[] oeoriDate = objStatus.OEORI_Date.Split('/');
                            int iOeoriDate = Convert.ToInt32(oeoriDate[2] + oeoriDate[1] + oeoriDate[0]);

                            string[] dcDate = objStatus.DCDate.Split('/');
                            int iDcDate = Convert.ToInt32(dcDate[2] + dcDate[1] + dcDate[0]);

                            if (iOeoriDate >= iDcDate)
                            {
                                // Cancel
                                drug.OSTAT_Desc = "Cancel";
                                lsContinueCancel.Add(drug);
                            }
                            else
                            {
                                // Discontinue
                                drug.OSTAT_Desc = objStatus.OSTAT_Desc;
                                lsContinueDiscontinue.Add(drug);
                            }
                        }

                        foreach (var objStatus in patientDrugViewModels.DrugViewModel
                                                    .Where(w => w.TimeLineDate == objType.TimeLineDate
                                                                && w.TimeLineDateTime == objType.TimeLineDateTime
                                                                && w.Type.ToUpper() == objType.Type.ToUpper()
                                                                && w.OSTAT_Code == "E"))
                        {
                            Drug drug = new Drug();
                            drug.OEORI_Date = objStatus.OEORI_Date;
                            drug.Service = objStatus.Service;
                            drug.QuestionAnswerModel = objStatus.QuestionAnswerModel;
                            drug.Qty = objStatus.Qty;
                            drug.Dose = objStatus.Dose;
                            drug.StartDate = objStatus.StartDate;
                            drug.StartTime = objStatus.StartTime;
                            drug.OrderingClinician = objStatus.OrderingClinician;
                            drug.AuthorisingClinician = objStatus.AuthorisingClinician;
                            drug.DCUserCode = objStatus.DCUserCode;
                            drug.DCUserName = objStatus.DCUserName;
                            drug.DCDate = objStatus.DCDate;
                            drug.DCTime = objStatus.DCTime;
                            drug.AddUserCode = objStatus.AddUserCode;
                            drug.AddUserName = objStatus.AddUserName;

                            drug.OSTAT_Code = objStatus.OSTAT_Code;
                            drug.OSTAT_Code = objStatus.OSTAT_Desc;
                            lsContinueExecute.Add(drug);
                        }
                    }
                    Continue.lsVerify = lsContinueVerify;
                    Continue.lsCancel = lsContinueCancel;
                    Continue.lsDiscontinue = lsContinueDiscontinue;
                    Continue.lsExecute = lsContinueExecute;


                    // Set startDate and Time List
                    drugTime.StartTime = objTime.TimeLineDateTime;
                    drugTime.OneDay = OneDay;
                    drugTime.Continue = Continue;

                    // Add drugTime to List
                    lsDrugTime.Add(drugTime);
                }
                
                // Set startDate and Time List (** sort time)
                drugDate.StartDate = objDate;
                drugDate.DrugTime = lsDrugTime;

                // Add drugDate to List
                //drgDteList.Add(drugDate);
                AddPatientDrugGroupViewModel(drugDate);
            }

        }

        public void AddPatientDrugGroupViewModel(DrugDate drgDate)
        {
            DrugDateList.Add(drgDate);
        }

        public PatientDrugGroupViewModel GetPatientDrugGroupViewModel()
        {
            patientDrugGroupViewModel.DrugDateList = DrugDateList;
            return patientDrugGroupViewModel;
        }

    }

    public class DrugDate
    {
        public string StartDate { get; set; }
        public List<DrugTime> DrugTime { get; set; }
    }

    //public class DrugTime
    //{
    //    public string StartTime { get; set; }
    //    public List<Drug> OneDay { get; set; }
    //    public List<Drug> Continue { get; set; }

    //    public Status Verify { get; set; }
    //}

        public class DrugTime
    {
        public string StartTime { get; set; }
        public Status OneDay { get; set; }
        public Status Continue { get; set; }
    }

    public class Status
    {
        public List<Drug> lsVerify { get; set; }
        public List<Drug> lsDiscontinue { get; set; }
        public List<Drug> lsCancel { get; set; }
        public List<Drug> lsExecute { get; set; }
    }
}