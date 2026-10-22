using System.Security.Cryptography;
using System.Text.Json.Nodes;
using Pure.Authorization.OpenFGA.Model.Abstractions.Object;
using Pure.Authorization.OpenFGA.Model.Abstractions.Subject;
using Pure.Authorization.OpenFGA.Model.Abstractions.Tuple;
using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.Authorization.OpenFGA.Model.HashCodes.Subject;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tuple;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Tuple;

public sealed record TupleHashTests
{
    private static readonly byte[] TypePrefix =
    [
        227,
        24,
        78,
        93,
        93,
        126,
        98,
        174,
        13,
        246,
        15,
        247,
        176,
        64,
        103,
        72,
    ];

    private static readonly IObject Anne = new FakeObject(
        new String("user"),
        new String("anne")
    );

    private static readonly ISubject GroupMembers = new FakeSubjectSet(
        new FakeObject(new String("group"), new String("1")),
        new String("member")
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
    public void CorrectComputingStepsForPlainTuple()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new SubjectHash(GroupMembers),
                .. new DeterminedHash(new String("viewer")),
                .. new ObjectHash(Document),
            ]),
            new TupleHash(new FakeTuple(GroupMembers, new String("viewer"), Document))
        );
    }

    [Fact]
    public void DispatchesConditionalTuple()
    {
        FakeConditionalTuple tuple = new FakeConditionalTuple(
            Anne,
            new String("viewer"),
            Document,
            NonExpired
        );

        Assert.Equal(new ConditionalTupleHash(tuple), new TupleHash(tuple));
    }

    [Fact]
    public void ConditionalDiffersFromPlain()
    {
        Assert.NotEqual<byte>(
            new TupleHash(new FakeTuple(Anne, new String("viewer"), Document)),
            new TupleHash(
                new FakeConditionalTuple(Anne, new String("viewer"), Document, NonExpired)
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeTuple tuple = new FakeTuple(Anne, new String("viewer"), Document);

        Assert.Equal(new TupleHash(tuple), new UntypedBytes(new TupleHash(tuple)));
    }

    [Fact]
    public void EqualForEqualTuples()
    {
        Assert.Equal<byte>(
            new TupleHash(new FakeTuple(Anne, new String("viewer"), Document)),
            new TupleHash(
                new FakeTuple(
                    new FakeObject(new String("user"), new String("anne")),
                    new String("viewer"),
                    new FakeObject(new String("document"), new String("1"))
                )
            )
        );
    }

    [Fact]
    public void DiffersBySubject()
    {
        Assert.NotEqual<byte>(
            new TupleHash(new FakeTuple(Anne, new String("viewer"), Document)),
            new TupleHash(new FakeTuple(GroupMembers, new String("viewer"), Document))
        );
    }

    [Fact]
    public void DiffersByRelation()
    {
        Assert.NotEqual<byte>(
            new TupleHash(new FakeTuple(Anne, new String("viewer"), Document)),
            new TupleHash(new FakeTuple(Anne, new String("editor"), Document))
        );
    }

    [Fact]
    public void DiffersByObject()
    {
        Assert.NotEqual<byte>(
            new TupleHash(new FakeTuple(Anne, new String("viewer"), Document)),
            new TupleHash(
                new FakeTuple(
                    Anne,
                    new String("viewer"),
                    new FakeObject(new String("document"), new String("2"))
                )
            )
        );
    }

    [Fact]
    public void ThrowsForUnknownSubject()
    {
        _ = Assert.Throws<ArgumentException>(() =>
            new TupleHash(
                new FakeTuple(new UnknownSubject(), new String("viewer"), Document)
            ).ToArray()
        );
    }
}
