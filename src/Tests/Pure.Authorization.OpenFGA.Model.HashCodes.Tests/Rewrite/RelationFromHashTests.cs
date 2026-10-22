using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Rewrite;

public sealed record RelationFromHashTests
{
    private static readonly byte[] TypePrefix =
    [
        170,
        74,
        229,
        34,
        155,
        70,
        128,
        141,
        7,
        44,
        0,
        35,
        17,
        86,
        130,
        121,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("viewer")),
                .. new DeterminedHash(new String("parent")),
            ]),
            new RelationFromHash(
                new FakeRelationFrom(new String("viewer"), new String("parent"))
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeRelationFrom relation = new FakeRelationFrom(
            new String("viewer"),
            new String("parent")
        );

        Assert.Equal(
            new RelationFromHash(relation),
            new UntypedBytes(new RelationFromHash(relation))
        );
    }

    [Fact]
    public void EqualForEqualRelations()
    {
        Assert.Equal<byte>(
            new RelationFromHash(
                new FakeRelationFrom(new String("viewer"), new String("parent"))
            ),
            new RelationFromHash(
                new FakeRelationFrom(new String("viewer"), new String("parent"))
            )
        );
    }

    [Fact]
    public void DiffersByRelation()
    {
        Assert.NotEqual<byte>(
            new RelationFromHash(
                new FakeRelationFrom(new String("viewer"), new String("parent"))
            ),
            new RelationFromHash(
                new FakeRelationFrom(new String("editor"), new String("parent"))
            )
        );
    }

    [Fact]
    public void DiffersByTupleset()
    {
        Assert.NotEqual<byte>(
            new RelationFromHash(
                new FakeRelationFrom(new String("viewer"), new String("parent"))
            ),
            new RelationFromHash(
                new FakeRelationFrom(new String("viewer"), new String("owner"))
            )
        );
    }

    [Fact]
    public void DiffersWhenRelationAndTuplesetSwap()
    {
        Assert.NotEqual<byte>(
            new RelationFromHash(
                new FakeRelationFrom(new String("viewer"), new String("parent"))
            ),
            new RelationFromHash(
                new FakeRelationFrom(new String("parent"), new String("viewer"))
            )
        );
    }
}
