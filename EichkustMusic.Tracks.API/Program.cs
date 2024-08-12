using static EichkustMusic.Tracks.Infrastructure.Persistence.DependencyInjection;
using static EichkustMusic.S3.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddApiVersioning(
    o => o.DefaultApiVersion = new Asp.Versioning.ApiVersion(1,0))
    .AddApiExplorer();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddS3(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
