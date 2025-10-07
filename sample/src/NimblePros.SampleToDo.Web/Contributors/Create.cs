using System.ComponentModel.DataAnnotations;
using FluentValidation;
using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.UseCases.Contributors.Commands.Create;
using NimblePros.SampleToDo.Web.Extensions;

namespace NimblePros.SampleToDo.Web.Contributors;

// This shows an example of having all related types in one file for simplicity.
// Fast-Endpoints generally uses one file per class for larger projects, which
// is the recommended approach. More files, but fewer merge conflicts and easier to 
// see what changed in a given commit or PR.

public class Create(IMediator mediator)
  : Endpoint<CreateContributorRequest, 
          Results<Created<CreateContributorResponse>,
                          ValidationProblem,
                          ProblemHttpResult>>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post(CreateContributorRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Create a new contributor";
      s.Description = "Creates a new contributor with the provided name. The contributor name must be between 2 and 100 characters long.";
      s.ExampleRequest = new CreateContributorRequest { Name = "John Doe" };
      s.ResponseExamples[201] = new CreateContributorResponse(1, "John Doe");
      
      // Document possible responses
      s.Responses[201] = "Contributor created successfully";
      s.Responses[400] = "Invalid input data - validation errors";
      s.Responses[500] = "Internal server error";
    });
    
    // Add tags for API grouping
    Tags("Contributors");
    
    // Add additional metadata
    Description(builder => builder
      .Accepts<CreateContributorRequest>("application/json")
      .Produces<CreateContributorResponse>(201, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(500));
  }

  public override async Task<Results<Created<CreateContributorResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateContributorRequest request, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new CreateContributorCommand(ContributorName.From(request.Name!)));

    return result.ToCreatedResult(
      id => $"/Contributors/{id}", 
      id => new CreateContributorResponse(id, request.Name!));
  }
}

public class CreateContributorRequest
{
  public const string Route = "/Contributors";

  [Required]
  public string Name { get; set; } = String.Empty;
}

public class CreateContributorValidator : Validator<CreateContributorRequest>
{
  public CreateContributorValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Name is required.")
      .MinimumLength(2)
      .MaximumLength(ContributorName.MaxLength);
  }
}

public class CreateContributorResponse(int id, string name)
{
  public int Id { get; set; } = id;
  public string Name { get; set; } = name;
}