using System.Net.Http.Headers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var backendApiUrl = builder.Configuration["BackendApiUrl"] ?? 
                    throw new InvalidOperationException("BackendApiUrl is not configured.");

var token = builder.Configuration["JwtToken"];

builder.Services.AddHttpClient<HRCoreSuite.Frontend.Services.ApiClient>(client =>
{
    client.BaseAddress = new Uri(backendApiUrl);
    if (!string.IsNullOrEmpty(token))
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
});

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
