using AutoFixture;
using Ninject;
using Sightseeing.DI;

namespace SightSeeing.Tests
{
    public abstract class TestBase
    {
        protected IKernel Kernel;
        protected Fixture Fixture;

        [SetUp]
        public void SetUp()
        {
            Kernel = new StandardKernel();
            Kernel.Load(new ServiceModule());
            Fixture = new Fixture();
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [TearDown]
        public void TearDown()
        {
            Kernel.Dispose();
        }
    }
}