using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Tests.Integration.Web
{
    public class HomeControllerContactShould : BaseWebTest
    {
        [Fact]
        public async Task ReturnViewWithCorrectMessage()
        {
            var response = await _client.GetAsync("/home/contact");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("Your contact page", stringResponse);

        }
    }
}