// Here you could define global logic that would affect all tests

namespace MartiX.Clean.Architecture.Web.Tests;

public class GlobalHooks
{
  [Before(TestSession)]
  public static void SetUp()
  { }

  [After(TestSession)]
  public static void CleanUp()
  { }
}
