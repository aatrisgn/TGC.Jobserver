namespace TGC.JobServer.Abstractions.Services
{
    public interface IJobInitializeService
    {
        Task InitialzeJobsOnStartup();
    }
}
