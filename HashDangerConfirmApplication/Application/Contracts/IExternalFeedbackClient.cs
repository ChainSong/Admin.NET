namespace TaskPlaApplication.Application.Contracts;

public interface IExternalFeedbackClient
{
  string BuildUrl(string baseUrl, Dictionary<string, string?> parameters);
  Task<T> SendRequestAsync<T>(string url) where T : class;
    Task<TResponse> PostFormUrlEncodedAsync<TResponse>(
        string urlWithQuery,
        IDictionary<string, string>? headers = null,
        string? bearerToken = null,
        CancellationToken ct = default)
        where TResponse : class;

    Task<TResponse> SendPostAsync<TRequest, TResponse>(
         string url,
         TRequest body,
         IDictionary<string, string>? headers = null,
         string? bearerToken = null,
         CancellationToken ct = default)
         where TResponse : class;
}