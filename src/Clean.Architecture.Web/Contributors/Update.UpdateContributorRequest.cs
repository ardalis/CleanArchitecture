using System.ComponentModel.DataAnnotations;

namespace Clean.Architecture.Web.Endpoints.ContributorEndpoints;

public class UpdateContributorRequest
{
  public const string Route = "/Contributors/{ContributorId:int}";
  public static string BuildRoute(int contributorId) => Route.Replace("{ContributorId:int}", contributorId.ToString());

  public int ContributorId { get; set; }

  [Required]
  public int Id { get; set; }
  [Required]
  public string? Name { get; set; }
}
