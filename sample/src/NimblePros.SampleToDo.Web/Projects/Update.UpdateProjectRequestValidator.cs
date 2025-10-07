using FluentValidation;
using NimblePros.SampleToDo.Infrastructure.Data.Config;

namespace NimblePros.SampleToDo.Web.Projects;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateProjectRequestValidator : Validator<UpdateProjectRequest>
{
  public UpdateProjectRequestValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Name is required.")
      .MinimumLength(2)
      .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
    RuleFor(x => x.ProjectId)
      .Must((args, projectId) => args.Id == projectId)
      .WithMessage("Route and body Ids must match; cannot update Id of an existing resource.");
  }
}
