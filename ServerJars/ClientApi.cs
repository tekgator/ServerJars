using ServerJarsAPI.Converter;
using ServerJarsAPI.Events;
using ServerJarsAPI.Models;
using System.Net.Http.Handlers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ServerJarsAPI;

public abstract class ClientApi : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ProgressMessageHandler _progressMessageHandler;

    private readonly JsonSerializerOptions _jsonOptions = new();

    protected ClientApi(string baseUri)
    {
        _progressMessageHandler = new ProgressMessageHandler(new HttpClientHandler()
        {
            AllowAutoRedirect = true
        });

        _httpClient = new HttpClient(_progressMessageHandler)
        {
            BaseAddress = new Uri(baseUri)
        };

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
        void processEventHandler(object? _, HttpProgressEventArgs args)
        {
            progress?.Report(new ProgressEventArgs(args.ProgressPercentage, args.BytesTransferred, args.TotalBytes));
        }

        _progressMessageHandler.HttpReceiveProgress += processEventHandler;
        progress?.Report(new ProgressEventArgs(0, 0));

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        requestMessage.Headers.Add("Accept", "application/json");

        using var httpResponse = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (!httpResponse.IsSuccessStatusCode)
        {
            string errorMsg = await GetErrorRepsonse(httpResponse, cancellationToken);
            throw new HttpRequestException($"{httpResponse.ReasonPhrase}: {errorMsg}", null, httpResponse.StatusCode);
        }

        progress?.Report(new ProgressEventArgs(0, 0, httpResponse.Content.Headers.ContentLength));

        using var httpStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        await httpStream.CopyToAsync(stream, cancellationToken);

        _progressMessageHandler.HttpReceiveProgress -= processEventHandler;
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
            _progressMessageHandler?.Dispose();
            _httpClient?.Dispose();
        }
    }
}
