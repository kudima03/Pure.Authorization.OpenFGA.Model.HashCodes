using System.Buffers;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Internals;

/// <summary>
/// The canonical UTF-8 JSON text of a node: object properties sorted by ordinal key at
/// every depth, arrays in their original order, primitives as their JSON text, and a JSON
/// <c>null</c> value kept (so it differs from an absent property). Strings are re-escaped
/// uniformly, so equal strings written with different escapes give the same text.
/// </summary>
internal sealed record CanonicalJson : IEnumerable<byte>
{
    private readonly JsonNode? _node;

    public CanonicalJson(JsonNode? node)
    {
        _node = node;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        using JsonDocument document = JsonDocument.Parse(_node?.ToJsonString() ?? "null");
        ArrayBufferWriter<byte> buffer = new ArrayBufferWriter<byte>();
        using (Utf8JsonWriter writer = new Utf8JsonWriter(buffer))
        {
            Write(document.RootElement, writer);
        }
        return buffer.WrittenMemory.ToArray().AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static void Write(JsonElement element, Utf8JsonWriter writer)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            writer.WriteStartObject();
            foreach (
                JsonProperty property in element
                    .EnumerateObject()
                    .OrderBy(x => x.Name, StringComparer.Ordinal)
            )
            {
                writer.WritePropertyName(property.Name);
                Write(property.Value, writer);
            }
            writer.WriteEndObject();
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            writer.WriteStartArray();
            foreach (JsonElement item in element.EnumerateArray())
            {
                Write(item, writer);
            }
            writer.WriteEndArray();
        }
        else
        {
            element.WriteTo(writer);
        }
    }
}
