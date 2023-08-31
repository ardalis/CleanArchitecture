using FastEndpoints;
using FluentValidation;
using NimblePros.SampleToDo.Infrastructure.Data.Config;

namespace NimblePros.SampleToDo.Web.Projects;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateProjectValidator : Validator<CreateProjectRequest>
{
  public CreateProjectValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Name is required.")
      .MinimumLength(2)
      .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
  }
}
