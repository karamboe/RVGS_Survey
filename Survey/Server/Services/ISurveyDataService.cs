using Survey.Shared;

namespace Survey.Server.Services
{
    public interface ISurveyDataService
    {
        Task<IEnumerable<SurveyDto>> ListSurveys();
        Task<SurveyDto> GetSurvey(string id);
        Task<bool> SaveSurvey(SurveyDto survey);
        Task<bool> DeleteSurvey(string id);
    }
}
