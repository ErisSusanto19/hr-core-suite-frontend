using HRCoreSuite.Frontend.Handlers;
using HRCoreSuite.Frontend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
 
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<TokenHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/AccessDenied";
    });

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeFolder("/");
        options.Conventions.AllowAnonymousToPage("/Account/Login");
        options.Conventions.AllowAnonymousToPage("/AccessDenied");
    });

var backendApiUrl = builder.Configuration["BackendApiUrl"] ?? throw new InvalidOperationException("BackendApiUrl is not configured.");

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(backendApiUrl);
});

Action<HttpClient> configureClient = client =>
{
    client.BaseAddress = new Uri(backendApiUrl);
};

builder.Services.AddHttpClient<IEmployeeService, EmployeeService>(configureClient)
    .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddHttpClient<IBranchService, BranchService>(configureClient)
    .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddHttpClient<IPositionService, PositionService>(configureClient)
    .AddHttpMessageHandler<TokenHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Map("/api/{**path}", async (HttpContext context, IHttpClientFactory clientFactory, ILoggerFactory loggerFactory) =>
{
    var client = clientFactory.CreateClient(nameof(IEmployeeService));

    var path = context.Request.Path.ToString();
    if (context.Request.QueryString.HasValue)
    {
        path += context.Request.QueryString.Value;
    }

    var backendRequest = new HttpRequestMessage(new HttpMethod(context.Request.Method), path);

    foreach (var header in context.Request.Headers)
    {
        if (!header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase))
        {
            backendRequest.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        }
    }
    
    var backendResponse = await client.SendAsync(backendRequest, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
    
    context.Response.StatusCode = (int)backendResponse.StatusCode;
    
    foreach (var header in backendResponse.Headers)
    {
        if (!header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase) && !header.Key.Equals("Cookie", StringComparison.OrdinalIgnoreCase))
        {
            backendRequest.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        }
    }

    if (context.Request.Headers.TryGetValue("Cookie", out var cookieValues))
    {
        backendRequest.Headers.Add("Cookie", cookieValues.ToArray());
    }

    foreach (var header in backendResponse.Content.Headers)
    {
        context.Response.Headers[header.Key] = header.Value.ToArray();
    }
    
    await backendResponse.Content.CopyToAsync(context.Response.Body);
});

app.MapRazorPages();

app.Run();