using Hangfire;
using TGC.JobServer.Abstractions.Services;
using TGC.JobServer.WebAPI;
using TGC.WebAPI.RateLimiting;

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

app.UseRequestThrottling();


using (var initializeScope = app.Services.CreateScope())
{
    var jobInitializeService = initializeScope.ServiceProvider.GetService<IJobInitializeService>();
    //jobInitializeService.InitialzeJobsOnStartup().Wait();
}

app.Run();

