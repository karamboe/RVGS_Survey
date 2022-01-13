using Survey.Server.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Survey.Server.Infrastructure
{
    public static class ServerExtensions
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services) =>
            services
                .AddSingleton<ISurveyDataService, DbSurveyDataService>();
    }
}
