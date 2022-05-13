namespace TGC.JobServer.Jobs.JobTypes.Containers;

public class OrderBioBagsDescriber
{
    public Guid AdressId { get; set; }
    public Guid CauseId { get; set; }
    public Guid OutputId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}
