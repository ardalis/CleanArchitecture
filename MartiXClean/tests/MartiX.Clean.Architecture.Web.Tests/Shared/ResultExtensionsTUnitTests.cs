using MartiX.Clean.Architecture.Web.Extensions;
using MartiX.Clean.Architecture.Web.Feature.Product;
using MartiX.WebApi.Results;
using MartiX.WebApi.SharedKernel;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MartiX.Clean.Architecture.Web.Tests.Shared;

public class ResultExtensionsTUnitTests
{
  [Test]
  public async Task ToCreatedAndDeleteResult_WhenSuccessOrNotFound_MapsTypedResults()
  {
    var ok = Result.Success(7);
    var okResult = ok.ToCreatedResult(v => $"/Products/{v}", v => new ProductRecord(v, "P", 1m));
    var notFound = Result.NotFound("missing");
    var deleteResult = notFound.ToDeleteResult();

    await Assert.That(okResult.Result is not Created<ProductRecord>).IsFalse();
    await Assert.That(deleteResult.Result is not NotFound).IsFalse();
  }

  [Test]
  public async Task ResultExtensionMappings_WhenInvalidAndFailure_MapsProblemBranches()
  {
    Result<int> invalid = Result.Invalid(new ValidationError("Invalid field"));
    Result<int> notFound = Result.NotFound("Missing");
    var createdInvalid = invalid.ToCreatedResult(v => $"/Products/{v}", v => new ProductRecord(v, "P", 1m));
    var createdProblem = notFound.ToCreatedResult(v => $"/Products/{v}", v => new ProductRecord(v, "P", 1m));
    var getByIdInvalid = invalid.ToGetByIdResult(v => new ProductRecord(v, "P", 1m));
    var updateNotFound = notFound.ToUpdateResult(v => new ProductRecord(v, "P", 1m));
    var deleteProblem = Result.Invalid(new ValidationError("Delete failed")).ToDeleteResult();
    var okOnly = Result.Success(13).ToOkOnlyResult(v => new ProductRecord(v, "OK", 2m));

    await Assert.That(createdInvalid.Result is not ValidationProblem).IsFalse();
    await Assert.That(createdProblem.Result is not ProblemHttpResult).IsFalse();
    await Assert.That(getByIdInvalid.Result is not ProblemHttpResult).IsFalse();
    await Assert.That(updateNotFound.Result is not NotFound).IsFalse();
    await Assert.That(deleteProblem.Result is not ProblemHttpResult).IsFalse();
    await Assert.That(okOnly.Value is null || okOnly.Value.Id != 13).IsFalse();
  }
}

