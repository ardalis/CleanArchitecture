using FluentValidation;

namespace NimblePros.SampleToDo.Web.Projects;

public class UpdateProjectRequestValidator : Validator<UpdateProjectRequest>
{
  public UpdateProjectRequestValidator()
  {
    RuleFor(x => x.Id)
        .GreaterThan(0).WithMessage("Id must be a positive integer.");

    RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .Must(name => !string.IsNullOrWhiteSpace(name))
        .WithMessage("Name cannot be empty or whitespace.");
  }
}
