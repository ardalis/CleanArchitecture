using FastEndpoints;
using FluentValidation;
using NimblePros.SampleToDo.Web.Endpoints.Projects;

namespace NimblePros.SampleToDo.Web.Projects;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetProjectByIdValidator : Validator<GetProjectByIdRequest>
{
  public GetProjectByIdValidator()
  {
    RuleFor(x => x.ProjectId)
      .GreaterThan(0);
  }
}