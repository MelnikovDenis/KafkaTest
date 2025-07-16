using Confluent.Kafka;

namespace KafkaTest.App.Kafka.Json;

internal class JsonDeserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if(data.IsEmpty || isNull)
        {
            throw new InvalidOperationException("Невозможно десериализовать сообщение");
        }

        return System.Text.Json.JsonSerializer.Deserialize<T>(data) 
            ?? throw new InvalidOperationException("Невозможно десериализовать сообщение");
    }
}
