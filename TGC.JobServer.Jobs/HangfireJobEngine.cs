using Hangfire;
using System.Linq.Expressions;
using TGC.JobServer.Abstractions.Jobs;

namespace TGC.JobServer.Jobs;

public class HangfireJobEngine : IJobEngine
{
    public string Delayed(Expression<Action> method, TimeSpan timeSpan)
    {
        var jobId = BackgroundJob.Schedule(
            method,
            timeSpan);

        return jobId;
    }

    public string FireAndForget(Expression<Action> method)
    {
        //null being parsed as parameter since PerformContext is automatically set by Hangfire
        var jobId = BackgroundJob.Enqueue(method);
        return jobId;
    }

    public string Recurring(string recurringName, Expression<Action> method, string cronString)
    {
        RecurringJob.AddOrUpdate(
                recurringName,
                method,
                cronString
            );

        return recurringName;
    }

    public bool Delete(string jobId)
    {
        return BackgroundJob.Delete(jobId);
    }

}
