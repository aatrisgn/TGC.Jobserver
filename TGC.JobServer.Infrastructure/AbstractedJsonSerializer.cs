using System.Text.Json;
using TGC.JobServer.Abstractions.Infrastructure;

namespace TGC.JobServer.Infrastructure;
public class AbstractedJsonSerializer : IJsonSerializer
{
    public T Deserialize<T>(string json) where T : class => JsonSerializer.Deserialize<T>(json);
    public T Deserialize<T>(JsonDocument json) where T : class => JsonSerializer.Deserialize<T>(json);
    public string Serialize(object classObject) => JsonSerializer.Serialize(classObject);
}