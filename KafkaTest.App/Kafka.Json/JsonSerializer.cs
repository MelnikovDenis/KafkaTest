using Confluent.Kafka;

namespace KafkaTest.App.Kafka.Json;

internal class JsonSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data);
    }
}