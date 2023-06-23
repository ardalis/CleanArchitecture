using Ardalis.Result;
using Ardalis.Specification;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.Services;
using Clean.Architecture.SharedKernel.Interfaces;
using Moq;
using Xunit;

namespace Clean.Architecture.UnitTests.Core.Services;

public class ToDoItemSearchServiceTests
{
  private readonly IToDoItemSearchService _service;
  private readonly Mock<IRepository<Project>> _mockRepo = new();
  
  public ToDoItemSearchServiceTests()
  {
    _service = new ToDoItemSearchService(_mockRepo.Object);
    
  }

  [Fact]
  public async Task ReturnsValidationErrors()
  {
    var projects = await _service.GetAllIncompleteItemsAsync(0, string.Empty);
    
    Assert.NotEmpty(projects.ValidationErrors);
  }
  
  [Fact]
  public async Task ReturnsProjectNotFound()
  {
    var projects = await _service.GetAllIncompleteItemsAsync(100, "Hello");

    Assert.Equal(ResultStatus.NotFound, projects.Status);
  }
  
  [Fact]
  public async Task ReturnsAllIncompleteItems()
  {
    var title = "Some Title";
    var project = new Project("Cool Project", PriorityStatus.Backlog);
    
    project.AddItem(new ToDoItem
    {
      Title = title,
      Description = "Some Description"
    });
    
    _mockRepo.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<Project>>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(project);

    var projects = await _service.GetAllIncompleteItemsAsync(1, title);

    Assert.Empty(projects.ValidationErrors);
    Assert.Equal(projects.Value.First().Title, title);
    Assert.Equal(project.Items.Count(), projects.Value.Count);
  }
}
