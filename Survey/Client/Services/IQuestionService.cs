using Survey.Shared.Models;

namespace Survey.Client.Services
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDto>> ListQuestions(string surveyId);

        Task<QuestionDto> GetQuestionById(string id);

        Task<bool> DeleteQuestion(string id);

        Task<QuestionDto> SaveQuestion(QuestionDto item);

        Task<IEnumerable<AlternativeDto>> ListAlternatives(string questionId);

        Task<bool> SaveAlternative(AlternativeDto item);

        Task<bool> DeleteAlternative(string id);

        Task<AnswerDto> GetAnswer(string questionId);
        Task<AnswerDto> SaveAnswer(AnswerDto answer);
    }
}
