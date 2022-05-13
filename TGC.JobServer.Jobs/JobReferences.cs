namespace TGC.JobServer.Jobs;

public class JobReferences
{
    public static JobTypeReferences JobTypes { get; set; }
}

public class JobTypeReferences
{
    public static string IS_ALIVE_JOB = "TGC:Jobserver:JobType:IsAliveJob:1.0.0";
    public static string ORDER_BIO_BAGS = "TGC:Jobserver:JobType:OrderBioBags:1.0.0";
    public static string HTTP_JOB = "TGC:Jobserver:JobType:HttpJob:1.0.0";
}

public class JobExecutionReferences
{
    public static string FIRE_AND_FORGET = "TGC:Jobserver:ExecutionType:FIRE_AND_FORGET:1.0.0";
    public static string BATCH = "TGC:Jobserver:ExecutionType:BATCH:1.0.0";
    public static string RECURRING = "TGC:Jobserver:ExecutionType:RECURRING:1.0.0";
    public static string DELAYED = "TGC:Jobserver:ExecutionType:Delayed:1.0.0";
}
