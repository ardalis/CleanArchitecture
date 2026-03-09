namespace MartiX.Clean.Architecture.Blazor.Services;

/// <summary>
/// Reads a small amount of status information from the backend API.
/// </summary>
public sealed class BackendApiClient(HttpClient httpClient)
{
  /// <summary>
  /// Gets a user-friendly summary of the backend API health endpoint.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token for the outbound request.</param>
  /// <returns>A short message describing the backend API status.</returns>
  public async Task<string> GetHealthSummaryAsync(CancellationToken cancellationToken = default)
  {
    using var response = await httpClient.GetAsync("/health", cancellationToken);

    return response.IsSuccessStatusCode
      ? "The backend API responded successfully."
      : $"The backend API returned {(int)response.StatusCode} {response.ReasonPhrase}.";
  }
}
