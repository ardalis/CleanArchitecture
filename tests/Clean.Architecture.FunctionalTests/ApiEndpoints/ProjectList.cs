using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using Clean.Architecture.Web;
using Clean.Architecture.Web.ApiModels;
using Clean.Architecture.Web.Endpoints.ProjectEndpoints;
using Newtonsoft.Json;
using Xunit;

namespace Clean.Architecture.FunctionalTests.ApiEndpoints
{
    [Collection("Sequential")]
    public class ProjectList : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ProjectList(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsOneProject()
        {
            var result = await _client.GetAndDeserialize<ProjectListResponse>("/Projects");

            Assert.Equal(1, result.Projects.Count());
            Assert.Contains(result.Projects, i => i.Name == SeedData.TestProject1.Name);
        }
    }
}
