using Microsoft.Extensions.Primitives;

namespace gateway;

public class HeadersHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public HeadersHandler(IHttpContextAccessor? httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor?.HttpContext is not null)
        {
            AddToHeaders(request, "x-real-ip");
            AddToHeaders(request, "x-forwarded-for");
            AddToHeaders(request, "x-forwarded-host");
            AddToHeaders(request, "x-forwarded-port");
            AddToHeaders(request, "x-forwarded-proto");
            AddToHeaders(request, "x-original-uri");
            AddToHeaders(request, "x-scheme");
            AddToHeaders(request, "KanaloaDevice");
            AddToHeaders(request, "Authorization");
            AddToHeaders(request, "Accept-Language");
            AddToHeaders(request, "TestHeader");
        }
        else
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Need and incoming request")
            };
        }
        return await base.SendAsync(request, cancellationToken);
    }

    private void AddToHeaders(HttpRequestMessage request, string property)
    {
        if (_httpContextAccessor?.HttpContext is not null)
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(property, out StringValues value))
                request.Headers.Add(property, value.AsEnumerable());
        }
    }
}