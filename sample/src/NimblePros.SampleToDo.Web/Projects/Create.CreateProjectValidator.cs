using FastEndpoints;
using FluentValidation;
using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.Web.Projects;

public class CreateProjectValidator : Validator<CreateProjectRequest>
{
  public CreateProjectValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Name is required.")
      .MinimumLength(2)
      .MaximumLength(ProjectName.MaxLength);
  }
}
