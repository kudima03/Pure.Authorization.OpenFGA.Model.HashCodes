using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Rewrite;

public sealed record IntersectionHashTests
{
    private static readonly byte[] TypePrefix =
    [
        98,
        201,
        240,
        131,
        229,
        232,
        229,
        91,
        131,
        88,
        117,
        200,
        166,
        161,
        27,
        53,
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
            new IntersectionHash(new FakeIntersection([Editor, Owner]))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeIntersection rewrite = new FakeIntersection([Editor, Owner]);

        Assert.Equal(
            new IntersectionHash(rewrite),
            new UntypedBytes(new IntersectionHash(rewrite))
        );
    }

    [Fact]
    public void EqualForEqualChildren()
    {
        Assert.Equal<byte>(
            new IntersectionHash(new FakeIntersection([Editor, Owner])),
            new IntersectionHash(
                new FakeIntersection([
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
            new IntersectionHash(new FakeIntersection([Editor, Owner, ParentViewer])),
            new IntersectionHash(new FakeIntersection([ParentViewer, Editor, Owner]))
        );
    }

    [Fact]
    public void DiffersByChildren()
    {
        Assert.NotEqual<byte>(
            new IntersectionHash(new FakeIntersection([Editor, Owner])),
            new IntersectionHash(new FakeIntersection([Editor, ParentViewer]))
        );
    }
}
