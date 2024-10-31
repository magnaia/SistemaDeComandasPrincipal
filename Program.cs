using Radzen;
using SistemaBushidoSushiWok.Components;
using Microsoft.EntityFrameworkCore;
using SistemaBushidoSushiWok.Controllers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "SistemaBushidoSushiWokTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddHttpClient();
builder.Services.AddScoped<SistemaBushidoSushiWok.BUSHIDOSUSHIWOKService>();
builder.Services.AddDbContext<SistemaBushidoSushiWok.Data.BUSHIDOSUSHIWOKContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BUSHIDOSUSHIWOKConnection"));
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();