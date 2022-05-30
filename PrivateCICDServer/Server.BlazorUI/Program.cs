using Server.API.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorDialog();

builder.Services.AddScoped(services  => new TokenClient(services.GetService<IConfiguration>()!["serverApiUrl"], new HttpClient()));
builder.Services.AddBlazorContextMenu();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
