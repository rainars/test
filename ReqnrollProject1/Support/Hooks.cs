using Reqnroll;

namespace tests.support
{
    [Binding]
    public class Hooks
    {
        private readonly TestContext _context;

        public Hooks(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _context.SetBaseUrl("http://card-validation-api:8080");  // Updated to match Docker Compose service name and port
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Cleanup actions if necessary
        }
    }
}
