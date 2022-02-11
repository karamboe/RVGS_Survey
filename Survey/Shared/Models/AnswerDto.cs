using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Shared.Models
{
    public class AnswerDto
    {
        public string Id { get; set; }
        public string SurveyId { get; set; }
        public string QuestionId { get; set; }
        public List<AnswerAlternativeDto> AlternativeAnswers { get; set; }
        public string QuestionText { get; set; }
        public bool IsCorrect { get; set; }
        public int UpdateCount { get; set; }
    }

    public class AnswerAlternativeDto
    {
        public string Id { get; set; }
        public string SurveyId { get; set; }
        public string QuestionId { get; set; }
        public string AlternativeId { get; set; }
        public string AlternativeText { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCorrect { get; set; }

    }
}
