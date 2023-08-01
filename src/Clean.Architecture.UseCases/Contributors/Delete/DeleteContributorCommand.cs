using Ardalis.Result;
using MediatR;

namespace Clean.Architecture.UseCases.Contributors.Delete;

public record DeleteContributorCommand(int ContributorId) : IRequest<Result>;
