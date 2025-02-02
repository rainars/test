
namespace tests.support
{
    public class TestContext
    {
        public string BaseUrl { get; private set; }
        public ApiClient ApiClient { get; private set; }

        public void SetBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl;
            ApiClient = new ApiClient(baseUrl);
        }

        public static implicit operator TestContext(NUnit.Framework.TestContext v)
        {
            throw new NotImplementedException();
        }
    }
}
