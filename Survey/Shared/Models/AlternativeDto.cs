using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Shared.Models
{
    public class AlternativeDto
    {
        public string Id { get; set; }
        public string SurveyId { get; set; }
        public string QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }        
        public int UpdateCount { get; set; }
    }
}
