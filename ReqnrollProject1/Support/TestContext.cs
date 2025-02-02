using System.Net.Http;

namespace tests.support
{
    public class TestContext
    {
        public HttpClient ApiClient { get; private set; } = default!;

        public void SetBaseUrl(string baseUrl)
        {
            ApiClient = new HttpClient { BaseAddress = new System.Uri(baseUrl) };
        }

        public static implicit operator NUnit.Framework.TestContext(TestContext v)
        {
            throw new NotImplementedException();
        }
    }
}
