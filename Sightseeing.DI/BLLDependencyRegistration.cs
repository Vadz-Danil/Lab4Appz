using Microsoft.Extensions.DependencyInjection;
using SightSeeing.BLL.Interfaces;
using SightSeeing.BLL.Mapping;
using SightSeeing.BLL.Services;

namespace Sightseeing.DI
{
    public static class BllDependencyRegistration
    {
        public static void AddSightSeeingBll(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}