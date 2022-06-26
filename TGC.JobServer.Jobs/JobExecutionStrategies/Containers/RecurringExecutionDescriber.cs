namespace TGC.JobServer.Jobs.JobExecutionStrategies.Containers;
public class RecurringExecutionDescriber
{
    public string RecurringJobName { get; set; }
    public bool ShouldUpdate { get; set; }
    public string CronExpression { get; set; }
}