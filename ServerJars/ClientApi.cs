using ServerJarsAPI.Converter;
using ServerJarsAPI.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ServerJarsAPI;

public abstract class ClientApi : IDisposable
{
    protected readonly HttpClient _httpClient;
    private readonly bool _disposeClient = true;
    readonly JsonSerializerOptions _jsonOptions = new();

    protected ClientApi(string baseUri, HttpClient httpClient, bool disposeClient = false)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(baseUri);
        _disposeClient = disposeClient;

        _jsonOptions.Converters.Add(new UnixEpochDateTimeConverter());
    }

    protected async Task<T> GetAsync<T>(string uri, CancellationToken cancellationToken = default)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        requestMessage.Headers.Add("Accept", "application/json");

        using var httpResponse = await _httpClient.SendAsync(requestMessage, cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<T>>(_jsonOptions, cancellationToken);
            if (apiResponse is not null && apiResponse.Response is not null)
            {
                return apiResponse.Response;
            }

            throw new JsonException("Unable to deserialize API response");
        }

        string errorMsg = await GetErrorRepsonse(httpResponse, cancellationToken);
        throw new HttpRequestException($"{httpResponse.ReasonPhrase}: {errorMsg}", null, httpResponse.StatusCode);

    }

    protected async Task<Stream> StreamAsync(string uri, CancellationToken cancellationToken = default)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        requestMessage.Headers.Add("Accept", "application/json");

        var httpResponse = await _httpClient.SendAsync(requestMessage, cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            return await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        }

        string errorMsg = await GetErrorRepsonse(httpResponse, cancellationToken);
        throw new HttpRequestException($"{httpResponse.ReasonPhrase}: {errorMsg}", null, httpResponse.StatusCode);
    }

    private static async Task<string> GetErrorRepsonse(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        string errorMsg = string.Empty;
        try
        {
            var errResponse = await response.Content.ReadFromJsonAsync<ApiError>(cancellationToken: cancellationToken);
            errorMsg = errResponse?.Message ?? string.Empty;
        }
        catch { /* ignore */ }

        return errorMsg;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_disposeClient)
            {
                _httpClient?.Dispose();
            }
        }
    }
}
