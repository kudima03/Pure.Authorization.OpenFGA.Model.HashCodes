using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Rewrite;

public sealed record ComputedRelationHashTests
{
    private static readonly byte[] TypePrefix =
    [
        20,
        147,
        206,
        2,
        41,
        130,
        155,
        31,
        202,
        83,
        86,
        50,
        107,
        155,
        238,
        159,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([.. TypePrefix, .. new DeterminedHash(new String("editor"))]),
            new ComputedRelationHash(new FakeComputedRelation(new String("editor")))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeComputedRelation relation = new FakeComputedRelation(new String("editor"));

        Assert.Equal(
            new ComputedRelationHash(relation),
            new UntypedBytes(new ComputedRelationHash(relation))
        );
    }

    [Fact]
    public void EqualForEqualRelations()
    {
        Assert.Equal<byte>(
            new ComputedRelationHash(new FakeComputedRelation(new String("editor"))),
            new ComputedRelationHash(new FakeComputedRelation(new String("editor")))
        );
    }

    [Fact]
    public void DiffersByRelation()
    {
        Assert.NotEqual<byte>(
            new ComputedRelationHash(new FakeComputedRelation(new String("editor"))),
            new ComputedRelationHash(new FakeComputedRelation(new String("owner")))
        );
    }
}
