using CleanArchitecture.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Tests.Integration.Web
{
    public class HomeControllerIndexShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public HttpClient Client { get; }

        public HomeControllerIndexShould(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnViewWithCorrectMessage()
        {
            HttpResponseMessage response = await Client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            string stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("CleanArchitecture.Web", stringResponse);
        }
    }
}
