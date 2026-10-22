using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Rewrite;

public sealed record UnionHashTests
{
    private static readonly byte[] TypePrefix =
    [
        220,
        254,
        198,
        166,
        3,
        247,
        101,
        151,
        43,
        205,
        11,
        63,
        156,
        217,
        67,
        55,
    ];

    private static readonly IRewrite Editor = new FakeComputedRelation(
        new String("editor")
    );

    private static readonly IRewrite Owner = new FakeComputedRelation(
        new String("owner")
    );

    private static readonly IRewrite ParentViewer = new FakeRelationFrom(
        new String("viewer"),
        new String("parent")
    );

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash([new RewriteHash(Editor), new RewriteHash(Owner)]),
            ]),
            new UnionHash(new FakeUnion([Editor, Owner]))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeUnion rewrite = new FakeUnion([Editor, Owner]);

        Assert.Equal(new UnionHash(rewrite), new UntypedBytes(new UnionHash(rewrite)));
    }

    [Fact]
    public void EqualForEqualChildren()
    {
        Assert.Equal<byte>(
            new UnionHash(new FakeUnion([Editor, Owner])),
            new UnionHash(
                new FakeUnion([
                    new FakeComputedRelation(new String("editor")),
                    new FakeComputedRelation(new String("owner")),
                ])
            )
        );
    }

    [Fact]
    public void ChildrenOrderDoesNotMatter()
    {
        Assert.Equal<byte>(
            new UnionHash(new FakeUnion([Editor, Owner, ParentViewer])),
            new UnionHash(new FakeUnion([ParentViewer, Editor, Owner]))
        );
    }

    [Fact]
    public void DiffersByChildren()
    {
        Assert.NotEqual<byte>(
            new UnionHash(new FakeUnion([Editor, Owner])),
            new UnionHash(new FakeUnion([Editor, ParentViewer]))
        );
    }
}
