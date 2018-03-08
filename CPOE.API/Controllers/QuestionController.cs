using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Odbc;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace QuestionaireAPI.Controllers
{
    public class QuestionController : ApiController
    {
        [HttpGet]
        public String GetQuestion(String OEORI_rowid)
        {
            String param_return = "";
            try
            {
                DataTable dt = new DataTable();
                dt = LoadData(OEORI_rowid);
                //  ex.OEORI_rowid  17089598||24    17116023||3     17116023||7     17089598||67        17116023||5

                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var dataSplit = dt.Rows[i]["QUES_Code"].ToString().Split('_');      //split code

                        if (dataSplit.Length <= 3 && dt.Rows[i]["QUES_ControlType"].ToString() == "Label")  // Head subject
                        {
                            var param_data = dt.Rows[i]["QUES_Desc"].ToString();
                            param_data = Regex.Replace(param_data, @"<[^>]*>", string.Empty);
                            if (i != 0)
                            {
                                param_return += "<br/>";
                            }
                            param_return += param_data + " ";

                        }
                        else if (dataSplit.Length > 3 && dt.Rows[i]["QUES_ControlType"].ToString() == "Label") //sub detail ex.Enddate
                        {
                            var param_desc = " , " + dt.Rows[i]["QUES_Desc"].ToString() + ": ";
                            if (i + 2 != dt.Rows.Count)     // Ans in Checkbox and not last item
                            {
                                if (dt.Rows[i + 2]["QUES_ControlType"].ToString() == "Label" || dt.Rows[i + 2]["QUES_ControlType"].ToString() == "CheckBox")
                                {
                                    if (!dt.Rows[i + 1]["QUES_Desc"].ToString().Contains("<font"))
                                    {
                                        param_desc += dt.Rows[i + 1]["QUES_Desc"];
                                        i += 1;
                                    }
                                    
                                }
                                else
                                {
                                    param_desc += dt.Rows[i + 1]["QUES_Desc"].ToString();
                                    param_desc += " " + dt.Rows[i + 2]["QA_Answer"].ToString();
                                    i += 2;
                                }
                            }
                            else   // Ans in Checkbox and last item
                            {
                                param_desc += dt.Rows[i + 1]["QUES_Desc"].ToString();
                                i += 1;
                            }

                            param_return += param_desc.Replace("&nbsp", "").Replace("date & time ให้เลือก", "").Replace("Date & Time ให้เลือก", "").Replace("Date & Time", "");
                        }
                        else
                        {
                            if (i != dt.Rows.Count - 1 && dt.Rows[i + 1]["QUES_ControlType"].ToString() != "CheckBox" && dt.Rows[i + 1]["QUES_ControlType"].ToString() != "Label")
                            //Detail next type = (checkbox or Label) and not end of loop
                            {
                                var param_desc = dt.Rows[i]["QUES_Desc"].ToString() + " " + dt.Rows[i + 1]["QUES_Desc"].ToString();

                                if (param_desc.ToUpper().IndexOf("X:") != -1)  //replace value X
                                {
                                    param_desc = param_desc.Replace("X:", dt.Rows[i + 1]["QA_Answer"].ToString() + ":").Replace("x:", dt.Rows[i + 1]["QA_Answer"].ToString() + ":");
                                }
                                else if (param_desc.ToUpper().IndexOf("(X ") != -1) //replace value X
                                {
                                    param_desc = param_desc.Replace("(X ", "(" + dt.Rows[i + 1]["QA_Answer"].ToString() + " ").Replace("(x ", "(" + dt.Rows[i + 1]["QA_Answer"].ToString() + " ");
                                }
                                else
                                {
                                    param_desc += dt.Rows[i + 1]["QA_Answer"].ToString().Replace("on", "");
                                }
                                param_return += param_desc.Replace("&nbsp", "").Replace("date & time ให้เลือก", "").Replace("Date & Time ให้เลือก", "").Replace("Date & Time", "");
                                i += 1;
                            }
                            else    //Detail Normal
                            {
                                param_return += " " + dt.Rows[i]["QUES_Desc"].ToString() + dt.Rows[i]["QA_Answer"].ToString().Replace("on","");
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return param_return;
        }
        [HttpGet]
        public List<QuestionaireAPI.Models.Questionaire> GetQuestionJson(String OEORI_rowid)
        {
            List<QuestionaireAPI.Models.Questionaire> Questions = new List<Models.Questionaire>();
            QuestionaireAPI.Models.Questionaire Ques = null;
            int rec_no = 0;
            try
            {
                DataTable dt = new DataTable();
                dt = LoadData(OEORI_rowid); 
                //  ex.OEORI_rowid  17089598||24    17116023||3     17116023||7     17089598||67        17116023||5

                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var dataSplit = dt.Rows[i]["QUES_Code"].ToString().Split('_');      //split code

                        if (dataSplit.Length <= 3 && dt.Rows[i]["QUES_ControlType"].ToString() == "Label")  // Head subject
                        {
                            var param_data = dt.Rows[i]["QUES_Desc"].ToString();
                            param_data = Regex.Replace(param_data, @"<[^>]*>", string.Empty);

                            Ques = new Models.Questionaire();
                            Ques.Head = param_data;
                        }else if (dataSplit.Length > 3 && dt.Rows[i]["QUES_ControlType"].ToString() == "Label") //sub detail ex.Enddate
                        {
                            Ques.Head = Questions[rec_no - 1].Head;
                            //Ques.Head = dataSplit[2];
                            Ques.Title = dt.Rows[i]["QUES_Desc"].ToString() + " ";
                            if (i + 2 != dt.Rows.Count)     // Ans in Checkbox and not last item
                            {
                                if (dt.Rows[i + 2]["QUES_ControlType"].ToString() == "Label" || dt.Rows[i + 2]["QUES_ControlType"].ToString() == "CheckBox")
                                {
                                    if (dt.Rows[i + 1]["QUES_Desc"].ToString().Contains("<font"))
                                    {
                                        Ques.Ans = "";
                                    }
                                    else
                                    {
                                        Ques.Ans = dt.Rows[i + 1]["QUES_Desc"].ToString().Replace("&nbsp", "").Replace("date & time ให้เลือก", "").Replace("Date & Time ให้เลือก", "");
                                        i += 1;
                                    }
                                    
                                }
                                else
                                {
                                    Ques.Title += dt.Rows[i + 1]["QUES_Desc"].ToString().Replace("&nbsp", "").Replace("date & time ให้เลือก", "").Replace("Date & Time ให้เลือก", "");
                                    Ques.Ans = dt.Rows[i + 2]["QA_Answer"].ToString();
                                    i += 2;
                                }
                            }
                            else   // Ans in Checkbox and last item
                            {
                                Ques.Ans = dt.Rows[i + 1]["QUES_Desc"].ToString().Replace("&nbsp", "").Replace("date & time ให้เลือก", "").Replace("Date & Time ให้เลือก", "");
                                i += 1;
                            }

                            Questions.Add(Ques);
                            rec_no += 1;
                            Ques = new Models.Questionaire();
                        } else {
                            if (i != dt.Rows.Count - 1 && dt.Rows[i + 1]["QUES_ControlType"].ToString() != "CheckBox" && dt.Rows[i + 1]["QUES_ControlType"].ToString() != "Label")
                            //Detail next type = (checkbox or Label) and not end of loop
                            {
                                var param_desc = dt.Rows[i]["QUES_Desc"].ToString() + " " + dt.Rows[i + 1]["QUES_Desc"].ToString();

                                if (param_desc.ToUpper().IndexOf("X:") != -1)  //replace value X
                                {
                                    param_desc = param_desc.Replace("X:", dt.Rows[i + 1]["QA_Answer"].ToString() + ":").Replace("x:", dt.Rows[i + 1]["QA_Answer"].ToString() + ":");
                                }
                                else if (param_desc.ToUpper().IndexOf("(X ") != -1) //replace value X
                                {
                                    param_desc = param_desc.Replace("(X ", "(" + dt.Rows[i + 1]["QA_Answer"].ToString() + " ").Replace("(x ", "(" + dt.Rows[i + 1]["QA_Answer"].ToString() + " ");
                                }
                                else
                                {
                                    Ques.Ans = dt.Rows[i + 1]["QA_Answer"].ToString();
                                }
                                Ques.Title = param_desc.Replace("&nbsp", "").Replace("date & time ให้เลือก", "").Replace("Date & Time ให้เลือก", "");
                                i += 1;
                            }
                            else    //Detail Normal
                            {
                                Ques.Title = dt.Rows[i]["QUES_Desc"].ToString();
                                Ques.Ans = dt.Rows[i]["QA_Answer"].ToString();
                            }
                            if (Ques.Ans == null || Ques.Ans == "on")
                            {
                                Ques.Ans = "";
                            }
                            Questions.Add(Ques);
                            rec_no += 1;
                            Ques = new Models.Questionaire();
                        }
                        
                    }
                }
            }
            catch
            {

            }

            return Questions;
        }
        private DataTable LoadData(String OEORI_rowid)
        {
            DataSet ds = new DataSet();
            try
            {
                String Sql = "select QUES_code,QUES_Desc,QA_Answer,QUES_ControlType,QUES_Sequence,OEORI_ItmMast_DR->ARCIM_code " +
                             "from (OE_OrdItem inner join OE_OrdQuestion   on OE_OrdQuestion.QA_ParRef =OE_OrdItem.OEORI_RowId) " +
                             "inner join PAC_Question on PAC_Question.QUES_RowId = OE_OrdQuestion.QA_Question_DR " +
                             "where OEORI_ItmMast_DR->ARCIM_code in ('18342','18343','18344','18345','18346') " +
                             "and OEORI_ItemStat_DR->OSTAT_Code in ('V','D') " +
                             "and case when QUES_ControlType <> 'Label' and isnull(QA_Answer,'') = '' then '0' else '1' end = '1' " +
                             "and QA_ParRef in ('" + OEORI_rowid + "') order by OEORI_ItmMast_DR->ARCIM_code,QUES_Sequence";
                using (OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings["prodtrak"].ConnectionString.ToString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    using (OdbcCommand comm = new OdbcCommand(Sql, conn))
                    {

                        OdbcDataAdapter Oda = new OdbcDataAdapter(comm);
                        Oda.Fill(ds);
                        conn.Close();
                    }
                }
            }
            catch
            {

            }
            return ds.Tables[0];
        }
    }
}
