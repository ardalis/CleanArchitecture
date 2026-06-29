using System.Globalization;

namespace Clean.Architecture.Web.Contributors;

public record DeleteContributorRequest
{
  public const string Route = "/Contributors/{ContributorId:int}";
  public static string BuildRoute(int contributorId) => Route.Replace("{ContributorId:int}", contributorId.ToString(CultureInfo.InvariantCulture), StringComparison.InvariantCulture);

  public int ContributorId { get; set; }
}
