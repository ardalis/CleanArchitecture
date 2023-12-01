﻿namespace Clean.Architecture.Web.Contributors;

public record DeleteContributorRequest
{
  public const string Route = "/Contributors/{ContributorId:int}";
  public static string BuildRoute(int contributorId)
  {
    return Route.Replace("{ContributorId:int}", contributorId.ToString());
  }

  public int ContributorId { get; set; }
}
