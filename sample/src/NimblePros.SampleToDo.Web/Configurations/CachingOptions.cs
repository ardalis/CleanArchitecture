namespace NimblePros.SampleToDo.Web.Configurations;

public class CachingOptions
{
  public const string SectionName = "Caching";
  public int DefaultDurationSeconds { get; set; } = 10;
  public List<CachingProfile> Profiles { get; set; } = [];
}
