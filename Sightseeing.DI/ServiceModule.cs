using Ninject.Modules;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.BLL.Interfaces;
using SightSeeing.BLL.Services;
using SightSeeing.DAL.UnitOfWork;

namespace Sightseeing.DI
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IUserService>().To<UserService>();
            Bind<IPlaceService>().To<PlaceService>();
            Bind<IReviewService>().To<ReviewService>();
            Bind<IQuestionService>().To<QuestionService>();
            Bind<IAnswerService>().To<AnswerService>();
        }
    }
}