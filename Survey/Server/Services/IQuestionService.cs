using Survey.Shared.Models;

namespace Survey.Server.Services
{
    public interface IQuestionDataService
    {
        Task<IEnumerable<QuestionDto>> ListQuestions(string id);
        Task<QuestionDto> GetQuestion(string id);
        Task<QuestionDto> SaveQuestion(QuestionDto item);
        Task<bool> DeleteQuestion(string id);

        Task<IEnumerable<AlternativeDto>> ListAlternatives(string id);
        Task<bool> SaveAlternative(AlternativeDto item);
        Task<bool> DeleteAlternative(string id);
    }
}
