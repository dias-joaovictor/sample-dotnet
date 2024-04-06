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
QuartzConfiguration.ConfigureQuartz(builder);

// Add kafka configuration
KafkaConfiguration.ConfigureKafka(builder);

builder.Services.AddSingleton<ProductService>();


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

app.Run();