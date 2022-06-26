using System.Linq.Expressions;

namespace TGC.JobServer.Abstractions.Jobs;

public interface IJobEngine
{
    public string FireAndForget(Expression<Action> method);
    public string Recurring(string recurringName, Expression<Action> method, string cronString);
    public string Delayed(Expression<Action> method, TimeSpan timeSpan);
    public bool Delete(string jobId);
}
