using ServerJarsAPI.Converter;
using ServerJarsAPI.Events;
using ServerJarsAPI.Extensions;
using ServerJarsAPI.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ServerJarsAPI;

public abstract class ClientApi : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _disposeClient = true;
    private readonly JsonSerializerOptions _jsonOptions = new();

    protected ClientApi(string baseUri, HttpClient httpClient, bool disposeClient = false)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(baseUri);
        _disposeClient = disposeClient;

        _jsonOptions.Converters.Add(new UnixEpochDateTimeConverter());
    }

    protected async Task<T> GetAsync<T>(
        string uri,
        CancellationToken cancellationToken = default)
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

    protected async Task<Stream> StreamAsync(
        string uri,
        CancellationToken cancellationToken = default)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        requestMessage.Headers.Add("Accept", "application/json");

        var httpResponse = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            return await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        }

        string errorMsg = await GetErrorRepsonse(httpResponse, cancellationToken);
        throw new HttpRequestException($"{httpResponse.ReasonPhrase}: {errorMsg}", null, httpResponse.StatusCode);
    }

    protected async Task DownloadAsync(
        Stream stream,
        string uri,
        IProgress<ProgressEventArgs>? progress,
        CancellationToken cancellationToken = default)
    {
        progress?.Report(new ProgressEventArgs());

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        requestMessage.Headers.Add("Accept", "application/json");

        using var httpResponse = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (!httpResponse.IsSuccessStatusCode)
        {
            string errorMsg = await GetErrorRepsonse(httpResponse, cancellationToken);
            throw new HttpRequestException($"{httpResponse.ReasonPhrase}: {errorMsg}", null, httpResponse.StatusCode);
        }

        var contentLength = httpResponse.Content.Headers.ContentLength;
        progress?.Report(new ProgressEventArgs() { TotalBytes = contentLength });

        using var httpStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

        if (progress is null || !contentLength.HasValue)
        {
            await httpStream.CopyToAsync(stream, cancellationToken);
            return;
        }

        await httpStream.CopyToAsync(stream, 1024, contentLength, progress, cancellationToken);
        return;
    }

    private static async Task<string> GetErrorRepsonse(
        HttpResponseMessage response,
        CancellationToken cancellationToken = default)
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
