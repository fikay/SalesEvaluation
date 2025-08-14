using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using SalesEvaluation.ConfigVariables;
using SalesEvaluation.Model;
using SalesEvaluation.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddDownstreamApi("SalesEvalAPI", builder.Configuration.GetSection("DownstreamApi"))
    .AddInMemoryTokenCaches();
builder.Services.AddRazorPages(opt =>
{
    opt.Conventions.AuthorizeFolder("/");
});

///Store login token for 1 hour 
builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
    opt.ExpireTimeSpan = TimeSpan.FromHours(1);
});

//Environment variables for file upload
builder.Services.Configure<FilesEnv>(builder.Configuration.GetSection("FileValues"));

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CallingService>();
builder.Services.AddSingleton<DWUserInfo>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
