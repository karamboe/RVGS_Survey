using Survey.Client.Contract;

namespace Survey.Client.Services
{
    public static class ClientExtensions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services) =>
            services
                .AddScoped<ISurveyService, SurveyService>()
                .AddScoped<IQuestionService, QuestionService>()
                .AddScoped<IUserContext, WebUserContext>()
                .AddScoped<IUserContextService, UserContextService>()
                .AddScoped<IUserDataService, UserDataService>();
    }
}
