using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Models;
using PlatformService.Services;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddScoped<IPlatformRepo, PlatformRepo>()
    .AddScoped<IPlatformService, PlatformService.Services.PlatformService>()
    .AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"));
});

builder.Services
    .AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

var app = builder.Build();


DbInit.Initialize(app);

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/env", () => { return app.Environment.EnvironmentName; });

app.Run();
