using System.Net.Http.Json;

using WoodenWorkshop.Common.Core.Exceptions;
using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Common.Utils.Extensions;

namespace WoodenWorkshop.Common.Utils.Http;

public class HttpClientFacade
{
    private readonly HttpClient _httpClient;

    public Uri? BaseAddress
    {
        get => _httpClient.BaseAddress;
        set => _httpClient.BaseAddress = value;
    }

    public HttpClientFacade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TContent> GetAsync<TContent>(Uri? requestUri)
    {
        return await SendAsync<TContent>(HttpMethod.Get, requestUri);
    }

    public async Task<TContent> PostAsync<TContent>(Uri? requestUri, object? body = null)
    {
        return await SendAsync<TContent>(HttpMethod.Post, requestUri, body);
    }

    public async Task PostAsync(Uri? requestUri, object? body = null)
    {
        await SendAsync(HttpMethod.Post, requestUri, body);
    }
    
    public async Task<TContent> PutAsync<TContent>(Uri? requestUri, object? body = null)
    {
        return await SendAsync<TContent>(HttpMethod.Put, requestUri, body);
    }
    
    public async Task PutAsync(Uri? requestUri, object? body = null)
    {
        await SendAsync(HttpMethod.Put, requestUri, body);
    }

    public async Task<TContent> PatchAsync<TContent>(Uri? requestUri, object? body = null)
    {
        return await SendAsync<TContent>(HttpMethod.Patch, requestUri, body);
    }
    
    public async Task PatchAsync(Uri? requestUri, object? body = null)
    {
        await SendAsync(HttpMethod.Patch, requestUri, body);
    }

    public async Task DeleteAsync(Uri? requestUri)
    {
        await SendAsync(HttpMethod.Get, requestUri);
    }
    
    public async Task<TContent> SendAsync<TContent>(
        HttpMethod method,
        Uri? requestUri = null,
        object? body = null
    )
    {
        var request = new HttpRequestMessage
        {
            Method = method,
            Content = body is null ? null : JsonContent.Create(body),
            RequestUri = requestUri,
        };
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsJsonAsync<TContent>();
        }

        var error = await response.Content.ReadAsJsonAsync<BaseError>();
        throw new CoreLogicException(error.Message, error.ErrorCode);
    }
    
    public async Task SendAsync(
        HttpMethod method,
        Uri? requestUri = null,
        object? body = null
    )
    {
        var request = new HttpRequestMessage
        {
            Method = method,
            Content = body is null ? null : JsonContent.Create(body),
            RequestUri = requestUri,
        };
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsJsonAsync<BaseError>();
            throw new CoreLogicException(error.Message, error.ErrorCode);
        }
    }
}