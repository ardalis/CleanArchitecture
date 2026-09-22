using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.UseCases.Contributors.Create;

public class CreateContributorHandler(
  IRepository<Contributor> repository)
  : ICommandHandler<CreateContributorCommand, Result<ContributorId>>
{
  public async ValueTask<Result<ContributorId>> Handle(
    CreateContributorCommand command,
    CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(command);

    var newContributor = new Contributor(command.Name);

    if (!string.IsNullOrEmpty(command.PhoneNumber))
    {
      var phoneNumber = new PhoneNumber(
        "+1",
        command.PhoneNumber,
        String.Empty);

      newContributor.UpdatePhoneNumber(phoneNumber);
    }

    var createdItem = await repository
      .AddAsync(newContributor, cancellationToken)
      .ConfigureAwait(false);

    return createdItem.Id;
  }
}
