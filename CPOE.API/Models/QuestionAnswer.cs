using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPOE.API.Models
{
    public class QuestionAnswer
    {
        public string OrdItemType { get; set; }
        public string QUES { get; set; }
        public string QUES_Desc { get; set; }
        public string QA_Answer { get; set; }
    }
}