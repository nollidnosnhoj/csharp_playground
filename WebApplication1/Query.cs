using HotChocolate.Language;

namespace WebApplication1;

public class Query
{
    public string Hello(HttpContext httpContext)
    {
        return "Hello from " + httpContext.Request.Headers["TestHeader"];
    }
}