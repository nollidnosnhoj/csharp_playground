using gateway;
using HotChocolate.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services
    .AddHttpClient("test", c =>
    {
        c.BaseAddress = new Uri("http://localhost:5056/graphql/");
    })
    .AddHttpMessageHandler(sp => new HeadersHandler(sp.GetRequiredService<IHttpContextAccessor>()));

builder.Services
    .AddGraphQLServer()
    .ModifyOptions(options => { options.StrictValidation = false; })
    .AddRemoteSchema("test")
    .AddGraphQLServer("test");
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder => corsBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetPreflightMaxAge(TimeSpan.FromMinutes(5)));
});

var app = builder.Build();
app.UseCors("CorsPolicy");
app.MapGraphQL().WithOptions(new GraphQLServerOptions
{
    EnableBatching = true
});
app.Run();