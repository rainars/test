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
            _context.SetBaseUrl("https://api.example.com");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Cleanup actions if necessary
        }
    }
}
