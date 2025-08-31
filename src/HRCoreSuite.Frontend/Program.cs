using HRCoreSuite.Frontend.Handlers;
using Microsoft.AspNetCore.Authentication.Cookies;
 
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<TokenHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizePage("/Index");
        options.Conventions.AuthorizePage("/Upload");
    });

var backendApiUrl = builder.Configuration["BackendApiUrl"] ?? 
                    throw new InvalidOperationException("BackendApiUrl is not configured.");

var token = builder.Configuration["JwtToken"];

builder.Services.AddHttpClient<HRCoreSuite.Frontend.Services.ApiClient>(client =>
{
    client.BaseAddress = new Uri(backendApiUrl);
})
.AddHttpMessageHandler<TokenHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
