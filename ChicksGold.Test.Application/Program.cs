using ChicksGold.Test.Domain.AutoMapper;
using ChicksGold.Test.Presentation.ActionFilters;
using ChicksGold.Test.Service;
using ChicksGold.Test.Service.Abstractions;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IBucketChallengeService, BucketChallengeService>();
builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilterAttribute>();
}).AddApplicationPart(typeof(ChicksGold.Test.Presentation.AssemblyReference).Assembly);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TwoBucket API", Version = "v1" });
    c.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TwoBucket API v1"));
}

app.UseHttpsRedirection();

app.UseResponseCompression();
app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{

}