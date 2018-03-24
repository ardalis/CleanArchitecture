using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Tests.Integration.Web
{
    public class HomeControllerIndexShould : BaseWebTest
    {
        [Fact]
        public async Task ReturnViewWithCorrectMessage()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("CleanArchitecture.Web", stringResponse);

        }
    }
}