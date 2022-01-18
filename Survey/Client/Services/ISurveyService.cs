
using Survey.Shared.Models;

namespace Survey.Client.Services
{
    public interface ISurveyService
    {
        Task<IEnumerable<SurveyDto>> List();

        Task<SurveyDto> GetById(string id);

        Task<bool> Delete(string id);

        Task<bool> Save(SurveyDto survey);
    }
}
