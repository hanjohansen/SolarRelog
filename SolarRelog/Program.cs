using System.Reflection;
using System.Text.Json.Serialization;
using SolarRelog;
using SolarRelog.Application.Cache;
using SolarRelog.Application.Exceptions;
using SolarRelog.Application.Jobs;
using SolarRelog.Application.Logging;
using SolarRelog.Application.Services;
using SolarRelog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureAppDatabase();
builder.ConfigureAppServices();
builder.ConfigureAppCaching();
builder.ConfigureAppLogging();
builder.ConfigureAppJobs();


builder.Services.AddMediatR(cfg 
    => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add (new JsonStringEnumConverter ());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.ConfigureSwagger();

var app = builder.Build();
app.Logger.LogInformation( "Starting App");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
