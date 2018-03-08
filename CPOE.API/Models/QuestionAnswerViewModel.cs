using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPOE.API.Models
{
    public class QuestionAnswerViewModel
    {
        public string Title { get; set; }
        public List<string> Items { get; set; }
    }

    public class QAModel
    {
        public string QUES_Code { get; set; }
        public string QUES_Desc { get; set; }
        public string QA_Answer { get; set; }
        public string QUES_ControlType { get; set; }
        public string QUES_Type { get; set; }
    }
}