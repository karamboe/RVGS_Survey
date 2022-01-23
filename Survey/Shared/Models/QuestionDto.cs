namespace Survey.Shared.Models
{
    public class QuestionDto
    {
        public string Id { get; set; }
        public string SurveyId { get; set; }
        public string Text { get; set; }
        public bool IsMultipleChoise { get; set; }
        public bool Deleted { get; set; }
        public int UpdateCount { get; set; }

        //public IEnumerable<AlternativeDto> Alternatives { get; set; }
    }
}
