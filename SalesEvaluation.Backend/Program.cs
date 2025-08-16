using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SalesEvaluation.Backend.Services.Storage;
using SalesEvaluation.Backend.StartUp;
using SalesEvaluation.Backend.Utils;
using SalesEvaluation.Backend.Utils.DatabaseInitializer;
using SalesEvaluation.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter());
});

//Decide if to use sql or connect to azure blob storage 
 var storageConnection = builder.Configuration.GetSection("Storage");
 var storageType = StorageProviderFactory.Getprovider(storageConnection);

 //Services Lifecycle
 builder.Services.AddSingleton(storageType);
 builder.Services.AddSingleton<StorageService>();



builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( opt =>
{
    var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);

    opt.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "SalesEvaluation",
        Version = "v2"
    });
});
builder.Services.AddHttpClient();
builder.Services.AddHealthChecks()
    .AddTypeActivatedCheck<IntegrationHealthCheck>("IntegrationHealthCheck", args: new object[] 
    {
        builder.Configuration.GetValue<string>("Storage:connectionString")!
    });


var app = builder.Build();

var dbConnectionString = builder.Configuration.GetValue<string>("Storage:connectionString");

if (!string.IsNullOrEmpty(dbConnectionString))
{
    var dbStorageType = ConnectonStringUtil.GetStorageTypeFromConnectionString(dbConnectionString);

    if (dbStorageType == StorageEnums.StorageType.Sql)
    {
       await  DbInitializer.RunScriptsAsync(dbConnectionString);
    }
}


app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonConvert.SerializeObject(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new { name = e.Key, status = e.Value.Status.ToString() })
        });
        await context.Response.WriteAsync(result);
    }
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opt =>
    {
        opt.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
    });
    app.UseSwaggerUI( x =>
    {
        x.SwaggerEndpoint("/swagger/v2/swagger.json", "SalesEvaluation");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
