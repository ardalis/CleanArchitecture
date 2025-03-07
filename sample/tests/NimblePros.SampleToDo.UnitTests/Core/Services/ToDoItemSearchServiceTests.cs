﻿using NimblePros.SampleToDo.Core.Interfaces;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.Core.Services;

namespace NimblePros.SampleToDo.UnitTests.Core.Services;

public class ToDoItemSearchServiceTests
{
  private readonly IToDoItemSearchService _service;
  private readonly IRepository<Project> _repo = Substitute.For<IRepository<Project>>();
  
  public ToDoItemSearchServiceTests()
  {
    _service = new ToDoItemSearchService(_repo);
    
  }

  [Fact]
  public async Task ReturnsValidationErrors()
  {
    var projects = await _service.GetAllIncompleteItemsAsync(ProjectId.From(0), string.Empty);
    
    Assert.NotEmpty(projects.ValidationErrors);
  }
  
  [Fact]
  public async Task ReturnsProjectNotFound()
  {
    var projects = await _service.GetAllIncompleteItemsAsync(ProjectId.From(100), "Hello");

    Assert.Equal(ResultStatus.NotFound, projects.Status);
  }
  
  [Fact]
  public async Task ReturnsAllIncompleteItems()
  {
    var title = "Some Title";
    Project project = new Project(ProjectName.From("Cool Project"));
    
    project.AddItem(new ToDoItem
    {
      Title = title,
      Description = "Some Description"
    });

    _repo.FirstOrDefaultAsync(Arg.Any<ISpecification<Project>>(), Arg.Any<CancellationToken>())
      .Returns(project);

    var projects = await _service.GetAllIncompleteItemsAsync(ProjectId.From(1), title);

    Assert.Empty(projects.ValidationErrors);
    Assert.Equal(projects.Value.First().Title, title);
    Assert.Equal(project.Items.Count(), projects.Value.Count);
  }
}
