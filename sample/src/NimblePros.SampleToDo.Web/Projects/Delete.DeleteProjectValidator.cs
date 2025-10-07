using FastEndpoints;
using FluentValidation;

namespace NimblePros.SampleToDo.Web.Projects;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteProjectValidator : Validator<DeleteProjectRequest>
{
  public DeleteProjectValidator()
  {
    RuleFor(x => x.ProjectId)
      .GreaterThan(0);
  }
}