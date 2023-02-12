using Clean.Architecture.Web;
using Xunit;

namespace Clean.Architecture.FunctionalTests.ControllerViews;

[Collection("Sequential")]
public class HomeControllerIndex : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public HomeControllerIndex(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsViewWithCorrectMessage()
  {
    HttpResponseMessage response = await _client.GetAsync("/");
    response.EnsureSuccessStatusCode();
    string stringResponse = await response.Content.ReadAsStringAsync();

    Assert.Contains("Clean.Architecture.Web", stringResponse);
  }
}
