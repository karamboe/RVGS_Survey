using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Shared
{
    public class QuestionDto
    {
        public string Id { get; set; }
        public string SurveyId { get; set; }
        public string Text { get; set; }
        public bool IsMultipleChoise { get; set; }
        public bool Deleted { get; set; }
        public int UpdateCount { get; set; }
    }
}
