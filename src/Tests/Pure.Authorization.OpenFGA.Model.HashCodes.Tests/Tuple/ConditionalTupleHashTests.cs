using System.Security.Cryptography;
using System.Text.Json.Nodes;
using Pure.Authorization.OpenFGA.Model.Abstractions.Object;
using Pure.Authorization.OpenFGA.Model.Abstractions.Tuple;
using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.Authorization.OpenFGA.Model.HashCodes.Subject;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tuple;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Tuple;

public sealed record ConditionalTupleHashTests
{
    private static readonly byte[] TypePrefix =
    [
        222,
        62,
        103,
        201,
        69,
        237,
        101,
        33,
        15,
        47,
        168,
        222,
        171,
        235,
        132,
        39,
    ];

    private static readonly IObject Anne = new FakeObject(
        new String("user"),
        new String("anne")
    );

    private static readonly IObject Bob = new FakeObject(
        new String("user"),
        new String("bob")
    );

    private static readonly IObject Document = new FakeObject(
        new String("document"),
        new String("1")
    );

    private static readonly IRelationshipCondition NonExpired =
        new FakeRelationshipCondition(
            new String("non_expired"),
            JsonNode.Parse("""{"duration":"1h"}""")!
        );

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new SubjectHash(Anne),
                .. new DeterminedHash(new String("viewer")),
                .. new ObjectHash(Document),
                .. new RelationshipConditionHash(NonExpired),
            ]),
            new ConditionalTupleHash(
                new FakeConditionalTuple(Anne, new String("viewer"), Document, NonExpired)
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeConditionalTuple tuple = new FakeConditionalTuple(
            Anne,
            new String("viewer"),
            Document,
            NonExpired
        );

        Assert.Equal(
            new ConditionalTupleHash(tuple),
            new UntypedBytes(new ConditionalTupleHash(tuple))
        );
    }

    [Fact]
    public void EqualForEqualTuples()
    {
        Assert.Equal<byte>(
            new ConditionalTupleHash(
                new FakeConditionalTuple(Anne, new String("viewer"), Document, NonExpired)
            ),
            new ConditionalTupleHash(
                new FakeConditionalTuple(
                    new FakeObject(new String("user"), new String("anne")),
                    new String("viewer"),
                    new FakeObject(new String("document"), new String("1")),
                    new FakeRelationshipCondition(
                        new String("non_expired"),
                        new JsonObject { ["duration"] = "1h" }
                    )
                )
            )
        );
    }

    [Fact]
    public void DiffersBySubject()
    {
        Assert.NotEqual<byte>(
            new ConditionalTupleHash(
                new FakeConditionalTuple(Anne, new String("viewer"), Document, NonExpired)
            ),
            new ConditionalTupleHash(
                new FakeConditionalTuple(Bob, new String("viewer"), Document, NonExpired)
            )
        );
    }

    [Fact]
    public void DiffersByRelation()
    {
        Assert.NotEqual<byte>(
            new ConditionalTupleHash(
                new FakeConditionalTuple(Anne, new String("viewer"), Document, NonExpired)
            ),
            new ConditionalTupleHash(
                new FakeConditionalTuple(Anne, new String("editor"), Document, NonExpired)
            )
        );
    }

    [Fact]
    public void DiffersByObject()
    {
        Assert.NotEqual<byte>(
            new ConditionalTupleHash(
                new FakeConditionalTuple(Anne, new String("viewer"), Document, NonExpired)
            ),
            new ConditionalTupleHash(
                new FakeConditionalTuple(Anne, new String("viewer"), Bob, NonExpired)
            )
        );
    }

    [Fact]
    public void DiffersByCondition()
    {
        Assert.NotEqual<byte>(
            new ConditionalTupleHash(
                new FakeConditionalTuple(Anne, new String("viewer"), Document, NonExpired)
            ),
            new ConditionalTupleHash(
                new FakeConditionalTuple(
                    Anne,
                    new String("viewer"),
                    Document,
                    new FakeRelationshipCondition(
                        new String("non_expired"),
                        JsonNode.Parse("""{"duration":"2h"}""")!
                    )
                )
            )
        );
    }
}
