using Hangfire;
using TGC.JobServer.WebAPI;

var builder = CustomApplicationBuilder.Build(args);

var app = builder.Build();

GlobalConfiguration.Configuration
        .UseActivator(new HangfireActivator(app.Services));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.UseHangfireDashboard();
app.UseHangfireServer();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();