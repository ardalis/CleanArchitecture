using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.UseCases.Contributors;
public record ContributorDto(ContributorId Id, ContributorName Name, PhoneNumber PhoneNumber);
