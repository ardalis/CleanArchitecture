namespace Clean.Architecture.AspireTests.Tests;

public class IntegrationTest1
{
    [Fact]
    public async Task GetContributorsReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Clean_Architecture_AspireHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
        // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
    
        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("web");
        await resourceNotificationService.WaitForResourceAsync("web", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        var response = await httpClient.GetAsync("/Contributors");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
