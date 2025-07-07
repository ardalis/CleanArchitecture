using System.Diagnostics;
using System.Reflection;
using Clean.Architecture.Core.ContributorAggregate;
using FastEndpoints;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clean.Architecture.UseCases.Contributors.Create;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record CreateContributorCommand(string Name, string? PhoneNumber) : Ardalis.SharedKernel.ICommand<Result<int>>;

public record CreateContributorCommand2(string Name) : FastEndpoints.ICommand<Result<int>>;

public class CreateContributorCommandHandler2 : CommandHandler<CreateContributorCommand2, Result<int>>
{
    private readonly IRepository<Contributor> _repository;
    public CreateContributorCommandHandler2(IRepository<Contributor> repository)
    {
        _repository = repository;
    }
    public override async Task<Result<int>> ExecuteAsync(CreateContributorCommand2 request, CancellationToken cancellationToken)
    {
        var newContributor = new Contributor(request.Name);
        var createdItem = await _repository.AddAsync(newContributor, cancellationToken);

    Console.WriteLine($"<<<<<<<Created contributor with ID: {createdItem.Id}");
    return createdItem.Id;
    }
}

public sealed class CommandLogger<TCommand, TResult>(ILogger<TCommand> logger)
    : ICommandMiddleware<TCommand, TResult> where TCommand : FastEndpoints.ICommand<TResult>
{
  private readonly ILogger<TCommand> _logger = logger;

  public async Task<TResult> ExecuteAsync(TCommand command,
                                          CommandDelegate<TResult> next,
                                          CancellationToken ct)
  {
    string commandName = command.GetType().Name;
    if (_logger.IsEnabled(LogLevel.Information))
    {
      _logger.LogInformation("Handling {RequestName}", commandName);

      // Reflection! Could be a performance concern
      Type myType = command.GetType();
      IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
      foreach (PropertyInfo prop in props)
      {
        object? propValue = prop?.GetValue(command, null);
        _logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
      }
    }

    var sw = Stopwatch.StartNew();

    var result = await next();

    _logger.LogInformation("Handled {CommandName} with {Result} in {ms} ms", commandName, result, sw.ElapsedMilliseconds);
    sw.Stop();

    return result;
  }
}
