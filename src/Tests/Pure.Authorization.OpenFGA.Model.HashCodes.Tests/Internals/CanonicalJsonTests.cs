using System.Text;
using System.Text.Json.Nodes;
using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Internals;

public sealed record CanonicalJsonTests
{
    [Fact]
    public void SortsPropertiesByOrdinalKey()
    {
        Assert.Equal(
            /*lang=json,strict*/
            """{"B":3,"a":2,"b":1}""",
            Text(JsonNode.Parse("""{"b":1,"a":2,"B":3}"""))
        );
    }

    [Fact]
    public void SortsNestedProperties()
    {
        Assert.Equal(
            /*lang=json,strict*/
            """{"a":[{"c":2,"d":1}],"z":{"x":2,"y":{"m":0,"n":1}}}""",
            Text(
                JsonNode.Parse("""{"z":{"y":{"n":1,"m":0},"x":2},"a":[{"d":1,"c":2}]}""")
            )
        );
    }

    [Fact]
    public void KeepsArrayOrder()
    {
        Assert.Equal("""[3,1,[2,0]]""", Text(JsonNode.Parse("""[3, 1, [2, 0]]""")));
    }

    [Fact]
    public void WritesPrimitivesAsJsonText()
    {
        Assert.Equal(
            /*lang=json,strict*/
            """{"e":1e3,"f":false,"i":-7,"n":1.50,"s":"text","t":true,"z":null}""",
            Text(
                JsonNode.Parse(
                    """{"s":"text","n":1.50,"i":-7,"e":1e3,"t":true,"f":false,"z":null}"""
                )
            )
        );
    }

    [Fact]
    public void KeepsNullProperties()
    {
        Assert.Equal( /*lang=json,strict*/
            """{"a":null}""",
            Text(JsonNode.Parse("""{"a":null}"""))
        );
    }

    [Fact]
    public void WritesNullNodeAsNull()
    {
        Assert.Equal("null", Text(null));
    }

    [Fact]
    public void RemovesWhitespace()
    {
        Assert.Equal(
            /*lang=json,strict*/
            """{"a":[1,2],"b":{"c":true}}""",
            Text(JsonNode.Parse("{ \"b\" : { \"c\" : true } ,\n \"a\" : [ 1 , 2 ] }"))
        );
    }

    [Fact]
    public void NormalizesStringEscapes()
    {
        Assert.Equal(
            Text(JsonNode.Parse("""{"a":"Aé"}""")),
            Text(JsonNode.Parse("{\"a\":\"Aé\"}"))
        );
    }

    [Fact]
    public void BuiltNodesMatchParsedNodes()
    {
        Assert.Equal(
            Text(JsonNode.Parse("""{"a":"x","b":[1,true]}""")),
            Text(new JsonObject { ["b"] = new JsonArray(1, true), ["a"] = "x" })
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        JsonNode? node = JsonNode.Parse("""{"b":1,"a":2}""");

        Assert.Equal(new CanonicalJson(node), new UntypedBytes(new CanonicalJson(node)));
    }

    private static string Text(JsonNode? node)
    {
        return Encoding.UTF8.GetString([.. new CanonicalJson(node)]);
    }
}
