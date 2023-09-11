using System.Reflection;
using System.Text.Json.Serialization;
using SolarRelog.Application.Exceptions;
using SolarRelog.Application.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppLogging();

builder.Services.AddMediatR(cfg 
    => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add (new JsonStringEnumConverter ());
    });  

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Logger.Log(LogLevel.Information, 0, "Starting App");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
