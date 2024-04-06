using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); // This line ensures that your app can use controllers

// Configure logging
builder.Logging.AddConsole(); // Adds logging output to the console
builder.Logging.AddDebug();

// Add Quartz
// QuartzConfiguration.ConfigureQuartz(builder);
builder.Services.AddQuartz(q =>
{
        // Register your job as follows
    q.AddJob<SampleTask>(opts => opts.WithIdentity("SampleJob"));
    
    // Create a trigger for the job
    q.AddTrigger(opts => opts
        .ForJob("SampleJob") // reference to the SampleJob
        .WithIdentity("SampleJob-trigger") // unique name for the trigger
        .WithCronSchedule("0/5 * * * * ?")); // e.g., run every 5 seconds
});

// builder.Services.AddQuartz(q =>
// {
//     var jobName = "SampleTask";
//     var jobKey = new JobKey(jobName);
//     q.AddJob<SampleTask>(opts => opts.WithIdentity(jobKey));
//     q.AddTrigger(opts => opts
//         .ForJob(jobKey)
//         .WithIdentity(jobName + "-Trigger")
//         .WithCronSchedule("0/5 * * * * ?"));
// });

// Add the Quartz hosted service which manages the job lifecycle
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

KafkaConfiguration.ConfigureKafka(builder);

var app = builder.Build();
app.UseAuthorization();

app.MapControllers(); // This line ensures that routing to controllers is enabled


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}




