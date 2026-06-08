using System.Globalization;

namespace Clean.Architecture.Web.Contributors;

public class GetContributorByIdRequest
{
  public const string Route = "/Contributors/{ContributorId:int}";
  public static string BuildRoute(int contributorId) => Route.Replace("{ContributorId:int}", contributorId.ToString(CultureInfo.InvariantCulture));

  public int ContributorId { get; set; }
}
