using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();


var logger = app.Services.GetService<ILogger<Program>>();
// Configure the HTTP request pipeline.

app.MapGet("/weatherforecast", () =>
{
    logger.LogInformation("/weatherforecast");
    return "TestGet";
});

app.MapPost("/PostTest", ([FromBody] JsonDocument todo) =>
{
    logger.LogInformation("/PostTest");
    var some = JsonSerializer.Deserialize<object>(todo);
});

app.Run();