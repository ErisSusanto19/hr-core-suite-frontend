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

app.MapRazorPages();

app.Run();