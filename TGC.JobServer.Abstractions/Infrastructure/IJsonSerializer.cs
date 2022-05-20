using System.Text.Json;

namespace TGC.JobServer.Abstractions.Infrastructure;
public interface IJsonSerializer
{
    T Deserialize<T>(string json) where T : class;
    public T Deserialize<T>(JsonDocument json) where T : class;
}
