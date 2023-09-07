using Quartz;
using SolarRelog.DevDevice.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SolarLogRecordService>();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("UpdateJob");
    q.AddJob<SolarLogUpdateJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("update-job-trigger")
        .WithCronSchedule("0/5 * * * * ?"));

});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
