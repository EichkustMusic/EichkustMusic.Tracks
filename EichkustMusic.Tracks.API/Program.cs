using EichkustMusic.Tracks.Application.S3;
using EichkustMusic.Tracks.Infrastructure.S3;
using static EichkustMusic.Tracks.Infrastructure.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddApiVersioning(
    o => o.DefaultApiVersion = new Asp.Versioning.ApiVersion(1,0))
    .AddApiExplorer();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddSingleton<IS3Storage>(ci =>
    new S3Storage(builder.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
