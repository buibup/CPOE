using CPOE.API.DA;
using CPOE.API.Models;
using DoctorOrder.Api;
using Newtonsoft.Json;
using QuestionaireAPI.Controllers;
using QuestionaireAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CPOE.API.Common
{
    public class Helper
    {
        public static PatientOrder DataTableToPatientOrder(string epiRowId, DataTable dtOneDay, DataTable dtContinue)
        {
            Models.PatientOrder ptOrder = new Models.PatientOrder();

            List<Order> OneDayList = new List<Order>();
            List<Order> ContinueList = new List<Order>();
            bool isCheck = false;

            string[] listARCIM_Code = GlobalVariables.ARCIM_Code.Split('|');
            string[] listARCIM_Code_Oneday = GlobalVariables.ARCIM_Code_Oneday.Split('|');
            listARCIM_Code_Oneday = listARCIM_Code_Oneday.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] listARCIM_Code_Continue = GlobalVariables.ARCIM_Code_Continue.Split('|');
            listARCIM_Code_Continue = listARCIM_Code_Continue.Where(x => !string.IsNullOrEmpty(x)).ToArray();


            foreach (DataRow row in dtOneDay.Rows)
            {
                bool isOne = listARCIM_Code_Oneday.Any(s => row["ARCIM_Code"].ToString().Contains(s));
                bool isCon = listARCIM_Code_Continue.Any(s => row["ARCIM_Code"].ToString().Contains(s));


                // ignore (standing || continue) order
                if (isCon || !(isOne || row["OECPR_Desc"].ToString() != "Standing"))
                    continue;

                Order OneDay = new Order();
                isCheck = listARCIM_Code.Any(s => row["ARCIM_Code"].ToString().Contains(s));
                if (isCheck)
                {
                    var data = GetQuestionAnswerModelFromAPI(row["OEORI_RowId"].ToString());
                    OneDay.QuestionAnswerModel = data;
                }
                else
                {
                    OneDay.QuestionAnswerModel = new List<Questionaire>();
                }

                OneDay.OEORI_RowId = String.IsNullOrEmpty(row["OEORI_RowId"].ToString())?"": row["OEORI_RowId"].ToString();
                OneDay.OEORI_Date = String.IsNullOrEmpty(row["OEORI_Date"].ToString()) ? "" : Convert.ToDateTime(row["OEORI_Date"].ToString()).ToString("dd/MM/") + Convert.ToDateTime(row["OEORI_Date"].ToString()).Year.ToString();
                OneDay.ARCIM_Code = String.IsNullOrEmpty(row["ARCIM_Code"].ToString())?"": row["ARCIM_Code"].ToString();
                OneDay.Service = String.IsNullOrEmpty(row["ARCIM_Desc"].ToString())?"": row["ARCIM_Desc"].ToString();
                OneDay.Qty = String.IsNullOrEmpty(row["OEORI_PhQtyOrd"].ToString()) ?"" : row["OEORI_PhQtyOrd"].ToString();
                OneDay.Dose = String.IsNullOrEmpty(row["PHCFR_Desc2"].ToString()) ? "" : row["PHCFR_Desc2"].ToString();
                DateTime dt = Convert.ToDateTime(row["OEORI_SttDat"].ToString());
                OneDay.StartDate = String.IsNullOrEmpty(row["OEORI_SttDat"].ToString()) ? "" : dt.ToString("dd/MM/") + dt.Year.ToString();
                OneDay.StartTime = String.IsNullOrEmpty(row["OEORI_SttTim"].ToString()) ? "" : Convert.ToDateTime(row["OEORI_SttTim"].ToString()).ToString("HH:mm");
                OneDay.OrderingClinician = String.IsNullOrEmpty(row["OrderingClinician"].ToString()) ? "" : row["OrderingClinician"].ToString();
                OneDay.AuthorisingClinician = String.IsNullOrEmpty(row["AuthorisingClinician"].ToString()) ? "" : row["AuthorisingClinician"].ToString();
                OneDay.DCUserCode = String.IsNullOrEmpty(row["DCUserCode"].ToString()) ? "" : row["DCUserCode"].ToString();
                OneDay.DCUserName = String.IsNullOrEmpty(row["DCUserName"].ToString())? "" : row["DCUserName"].ToString();
                OneDay.DCDate = String.IsNullOrEmpty(row["DCDate"].ToString()) ? "" : Convert.ToDateTime(row["DCDate"].ToString()).ToString("dd/MM/") + Convert.ToDateTime(row["DCDate"].ToString()).Year.ToString();
                OneDay.DCTime = String.IsNullOrEmpty(row["DCTime"].ToString()) ? "" : Convert.ToDateTime(row["DCTime"].ToString()).ToString("HH:mm");
                OneDay.AddUserCode = String.IsNullOrEmpty(row["AddUserCode"].ToString()) ? "" : row["AddUserCode"].ToString();
                OneDay.AddUserName = String.IsNullOrEmpty(row["AddUserName"].ToString()) ? "" : row["AddUserName"].ToString();
                OneDay.OSTAT_Code = String.IsNullOrEmpty(row["OSTAT_Code"].ToString()) ? "" : row["OSTAT_Code"].ToString();
                OneDay.OSTAT_Desc = String.IsNullOrEmpty(row["OSTAT_Desc"].ToString()) ? "" : row["OSTAT_Desc"].ToString();
                OneDay.OEORI_UpdateDate = String.IsNullOrEmpty(row["OEORI_UpdateDate"].ToString()) ? "" : Convert.ToDateTime(row["OEORI_UpdateDate"].ToString()).ToString("dd/MM/") + Convert.ToDateTime(row["OEORI_UpdateDate"].ToString()).Year.ToString();
                OneDay.OEORI_UpdateTime = String.IsNullOrEmpty(row["OEORI_UpdateTime"].ToString()) ? "" : Convert.ToDateTime(row["OEORI_UpdateTime"].ToString()).ToString("HH:mm");

                OneDayList.Add(OneDay);
            }

            foreach (DataRow row in dtContinue.Rows)
            {
                bool isOne = listARCIM_Code_Oneday.Any(s => row["ARCIM_Code"].ToString().Contains(s));
                bool isCon = listARCIM_Code_Continue.Any(s => row["ARCIM_Code"].ToString().Contains(s));

                // ignore (oneday or not Standing) order
                if (isOne || !( isCon || row["OECPR_Desc"].ToString() == "Standing"))
                    continue;

                Order Continue = new Order();
                isCheck = listARCIM_Code.Any(s => row["ARCIM_Code"].ToString().Contains(s));
                if (isCheck)
                {
                    var data = GetQuestionAnswerModelFromAPI(row["OEORI_RowId"].ToString());
                    Continue.QuestionAnswerModel = data;

                    var test = InterSystemsProvideData.GetQuestionAnswer(row["OEORI_RowId"].ToString());
                }
                else
                {
                    Continue.QuestionAnswerModel = new List<Questionaire>();
                }

                Continue.OEORI_RowId = String.IsNullOrEmpty(row["OEORI_RowId"].ToString()) ? "" : row["OEORI_RowId"].ToString();
                Continue.OEORI_Date = String.IsNullOrEmpty(row["OEORI_Date"].ToString()) ? "" : Convert.ToDateTime(row["OEORI_Date"].ToString()).ToString("dd/MM/") + Convert.ToDateTime(row["OEORI_Date"].ToString()).Year.ToString();
                Continue.ARCIM_Code = String.IsNullOrEmpty(row["ARCIM_Code"].ToString()) ? "" : row["ARCIM_Code"].ToString();
                Continue.Service = String.IsNullOrEmpty(row["ARCIM_Desc"].ToString()) ? "" : row["ARCIM_Desc"].ToString();
                Continue.Qty = String.IsNullOrEmpty(row["OEORI_PhQtyOrd"].ToString()) ? "" : row["OEORI_PhQtyOrd"].ToString();
                Continue.Dose = String.IsNullOrEmpty(row["PHCFR_Desc2"].ToString()) ? "" : row["PHCFR_Desc2"].ToString();
                DateTime dt = Convert.ToDateTime(row["OEORI_SttDat"].ToString());
                Continue.StartDate = String.IsNullOrEmpty(row["OEORI_SttDat"].ToString()) ? "" : dt.ToString("dd/MM/") + dt.Year.ToString();
                Continue.StartTime = String.IsNullOrEmpty(row["OEORI_SttTim"].ToString()) ? "" : Convert.ToDateTime(row["OEORI_SttTim"].ToString()).ToString("HH:mm");
                Continue.OrderingClinician = String.IsNullOrEmpty(row["OrderingClinician"].ToString()) ? "" : row["OrderingClinician"].ToString();
                Continue.AuthorisingClinician = String.IsNullOrEmpty(row["AuthorisingClinician"].ToString()) ? "" : row["AuthorisingClinician"].ToString();
                Continue.DCUserCode = String.IsNullOrEmpty(row["DCUserCode"].ToString()) ? "" : row["DCUserCode"].ToString();
                Continue.DCUserName = String.IsNullOrEmpty(row["DCUserName"].ToString()) ? "" : row["DCUserName"].ToString();
                Continue.DCDate = String.IsNullOrEmpty(row["DCDate"].ToString()) ? "" : Convert.ToDateTime(row["DCDate"].ToString()).ToString("dd/MM/") + Convert.ToDateTime(row["DCDate"].ToString()).Year.ToString();
                Continue.DCTime = String.IsNullOrEmpty(row["DCTime"].ToString()) ? "" : Convert.ToDateTime(row["DCTime"].ToString()).ToString("HH:mm");
                Continue.AddUserCode = String.IsNullOrEmpty(row["AddUserCode"].ToString()) ? "" : row["AddUserCode"].ToString();
                Continue.AddUserName = String.IsNullOrEmpty(row["AddUserName"].ToString()) ? "" : row["AddUserName"].ToString();
                Continue.OSTAT_Code = String.IsNullOrEmpty(row["OSTAT_Code"].ToString()) ? "" : row["OSTAT_Code"].ToString();
                Continue.OSTAT_Desc = String.IsNullOrEmpty(row["OSTAT_Desc"].ToString()) ? "" : row["OSTAT_Desc"].ToString();
                Continue.OEORI_UpdateDate = String.IsNullOrEmpty(row["OEORI_UpdateDate"].ToString()) ? "" : Convert.ToDateTime(row["OEORI_UpdateDate"].ToString()).ToString("dd/MM/") + Convert.ToDateTime(row["OEORI_UpdateDate"].ToString()).Year.ToString();
                Continue.OEORI_UpdateTime = String.IsNullOrEmpty(row["OEORI_UpdateTime"].ToString()) ? "" : Convert.ToDateTime(row["OEORI_UpdateTime"].ToString()).ToString("HH:mm");

                ContinueList.Add(Continue);
            }

            ptOrder.Patient = InterSystemsProvideData.GetPatient(epiRowId);
            ptOrder.OneDay = OneDayList;
            ptOrder.Continue = ContinueList;

            return ptOrder;
        }

        public static List<Questionaire> GetQuestionAnswerModelFromAPI(string OEORI_RowId)
        {
            QuestionController quest = new QuestionController();
            return quest.GetQuestionJson(OEORI_RowId);
        }

        public static List<QuestionAnswer> GetCompleteQuestionAnswer(List<QuestionAnswer> _listQuestionAnswer)
        {
            List<QuestionAnswer> listQuestionAnswer = new List<QuestionAnswer>();

            foreach (var item in _listQuestionAnswer)
            {
                QuestionAnswer qa;
                string noHtmlTag = string.Empty;
                if (Regex.IsMatch(item.QUES_Desc, @"<[^>]*>"))
                {
                    qa = new QuestionAnswer();
                    noHtmlTag = Regex.Replace(item.QUES_Desc, @"<[^>]*>", string.Empty);

                    qa.OrdItemType = item.OrdItemType;
                    qa.QUES = item.QUES;
                    qa.QUES_Desc = noHtmlTag;
                    qa.QA_Answer = item.QA_Answer;

                    listQuestionAnswer.Add(qa);
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.QA_Answer))
                    {
                        qa = new QuestionAnswer();

                        qa.OrdItemType = item.OrdItemType;
                        qa.QUES = item.QUES;
                        qa.QUES_Desc = item.QUES_Desc;
                        qa.QA_Answer = item.QA_Answer;

                        listQuestionAnswer.Add(qa);
                    }
                }
            }

            return listQuestionAnswer;
        }

        public static List<QuestionAnswerViewModel> GetQuestionAnswerViewModel(List<QAModel> lsitQAModel)
        {
            List<QuestionAnswerViewModel> results = new List<QuestionAnswerViewModel>();

            foreach(var item in lsitQAModel)
            {
                QuestionAnswerViewModel qavm = new QuestionAnswerViewModel();

                if (!string.IsNullOrEmpty(item.QUES_Type))
                {
                    qavm.Title = item.QUES_Desc;
                    
                }

                results.Add(qavm);
            }

            return results;
        }

        public static List<Allergy> DataTableToAllergy(DataTable dt)
        {
            List<Allergy> algs = new List<Allergy>();

            foreach (DataRow row in dt.Rows)
            {
                Allergy alg = new Allergy();
                string algName = string.Empty;
                if (!string.IsNullOrEmpty(row["ALG_Comments"].ToString()))
                {
                    algName = row["ALG_Comments"].ToString();
                    alg.Name = algName;
                    alg.Category = InterSystemsProvideData.GetAllergyCategory(algName);
                    algs.Add(alg);
                }
                else if (!string.IsNullOrEmpty(row["PHCGE_Name"].ToString()))
                {
                    algName = row["PHCGE_Name"].ToString();
                    alg.Name = algName;
                    alg.Category = InterSystemsProvideData.GetAllergyCategory(algName);
                    algs.Add(alg);
                }
                else if (!string.IsNullOrEmpty(row["ALG_Desc"].ToString()))
                {
                    algName = row["ALG_Desc"].ToString();
                    alg.Name = algName;
                    alg.Category = InterSystemsProvideData.GetAllergyCategory(algName);
                    algs.Add(alg);
                }
                else if (!string.IsNullOrEmpty(row["ALGR_Desc"].ToString()))
                {
                    algName = row["ALGR_Desc"].ToString();
                    alg.Name = algName;
                    alg.Category = InterSystemsProvideData.GetAllergyCategory(algName);
                    algs.Add(alg);
                }
                else if (!string.IsNullOrEmpty(row["PHCD_Name"].ToString()))
                {
                    algName = row["PHCD_Name"].ToString();
                    alg.Name = algName;
                    alg.Category = InterSystemsProvideData.GetAllergyCategory(algName);
                    algs.Add(alg);
                }
                else if (!string.IsNullOrEmpty(row["INGR_Desc"].ToString()))
                {
                    algName = row["INGR_Desc"].ToString();
                    alg.Name = algName;
                    alg.Category = InterSystemsProvideData.GetAllergyCategory(algName);
                    algs.Add(alg);
                }

            }

            return algs;
        }
    }
}