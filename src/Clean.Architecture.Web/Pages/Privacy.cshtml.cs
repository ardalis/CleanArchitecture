using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages;
public class PrivacyModel : PageModel
{
  private readonly ILogger<PrivacyModel> _logger;

  public PrivacyModel(ILogger<PrivacyModel> logger)
  {
    _logger = logger;
  }

  public void OnGet()
  {
  }
}

