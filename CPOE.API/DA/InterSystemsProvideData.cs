using InterSystems.Data.CacheClient;
using CPOE.API.Common;
using CPOE.API.Models;
using System;
using DoctorOrder.Api;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CPOE.API.Repository;

namespace CPOE.API.DA
{
    public static class InterSystemsProvideData
    {
        private static string conString = GlobalVariables.Cache89;
        private static ICPOERepository _ICPOERepository = new CPOERepository();

        public static Patient GetPatient(string epiRowId)
        {
            Patient pt = new Patient();

            using (var con = new CacheConnection(conString))
            {
                con.Open();
                using (var cmd = new CacheCommand(QueryString.GetPatient(epiRowId), con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hn = reader["PAPMI_No"].ToString();
                            pt.HN = hn;
                            pt.Name = string.IsNullOrEmpty(reader["TTL_Desc"].ToString()) ? reader["PAPMI_Name"].ToString() + " " + reader["PAPMI_Name2"].ToString() : reader["TTL_Desc"].ToString() + reader["PAPMI_Name"].ToString() + " " + reader["PAPMI_Name2"].ToString();
                            pt.Gender = reader["CTSEX_Desc"].ToString();
                            DateTime dt = Convert.ToDateTime(reader["PAPMI_DOB"].ToString());
                            pt.DOB = dt.ToString("dd/MM/") + dt.Year.ToString();
                            pt.EpisodeNo = reader["PAADM_ADMNo"].ToString();
                            pt.Age = reader["PAPER_AgeYr"].ToString();
                            pt.Allergys = _ICPOERepository.GetAllergys(hn);
                        }
                    }
                }
            }

            return pt;
        }

        public static List<QuestionAnswer> GetQuestionAnswer(string OEORI_RowId)
        {
            List<QuestionAnswer> listQuestionAnswer = new List<QuestionAnswer>();

            using (var con = new CacheConnection(conString))
            {
                con.Open();
                using (var cmd = new CacheCommand(QueryString.GetQuesAns(OEORI_RowId), con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            QuestionAnswer qa = new QuestionAnswer();
                            string item = reader["QUES_Code"].ToString();
                            string[] items = item.Split('_');
                            qa.OrdItemType = items.Length > 1 ? items[1].ToString() : "";
                            qa.QUES = items.Length > 2 ? items[2].ToString() : "";
                            qa.QUES_Desc = reader["QUES_Desc"].ToString();
                            qa.QA_Answer = reader["QA_Answer"].ToString();

                            listQuestionAnswer.Add(qa);
                        }
                    }
                }
            }

            return listQuestionAnswer;
        }

        public static List<QAModel> GetQAModel(string OEORI_RowId)
        {
            List<QAModel> results = new List<QAModel>();

            using (var con = new CacheConnection(conString))
            {
                con.Open();
                using (var cmd = new CacheCommand(QueryString.GetQuesAns(OEORI_RowId), con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        string type = string.Empty;
                        while (reader.Read())
                        {
                            QAModel qam = new QAModel();
                            string[] spilts = reader["QUES_Code"].ToString().Split('_');

                            if (Regex.IsMatch(reader["QUES_Desc"].ToString(), @"<[^>]*>"))
                            {
                                type = spilts.Length > 2 ? spilts[2] : "";

                                qam.QUES_Code = reader["QUES_Code"].ToString();
                                qam.QUES_Desc = reader["QUES_Desc"].ToString();
                                qam.QA_Answer = reader["QA_Answer"].ToString();
                                qam.QUES_ControlType = reader["QUES_ControlType"].ToString();
                                qam.QUES_Type = type;

                                results.Add(qam);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(reader["QA_Answer"].ToString()))
                                {
                                    qam.QUES_Code = reader["QUES_Code"].ToString();
                                    qam.QUES_Desc = reader["QUES_Desc"].ToString();
                                    qam.QA_Answer = reader["QA_Answer"].ToString();
                                    qam.QUES_ControlType = reader["QUES_ControlType"].ToString();
                                    qam.QUES_Type = type;

                                    results.Add(qam);
                                }
                            }

                            
                        }
                    }
                }
            }

            return results;
        }

        public static string GetAllergyCategory(string algName)
        {
            string result = string.Empty;

            var cmdString = QueryString.GetAllergyCategory(algName);
            result = InterSystemsDA.BindDataCommand(cmdString, GlobalVariables.Cache89);

            return result;
        }
    }
}