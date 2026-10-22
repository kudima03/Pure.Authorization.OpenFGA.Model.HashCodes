using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Rewrite;

public sealed record RewriteHashTests
{
    [Fact]
    public void DispatchesDirect()
    {
        FakeDirect rewrite = new FakeDirect([new FakeTypeForm(new String("user"))]);

        Assert.Equal(new DirectHash(rewrite), new RewriteHash(rewrite));
    }

    [Fact]
    public void DispatchesComputedRelation()
    {
        FakeComputedRelation rewrite = new FakeComputedRelation(new String("editor"));

        Assert.Equal(new ComputedRelationHash(rewrite), new RewriteHash(rewrite));
    }

    [Fact]
    public void DispatchesRelationFrom()
    {
        FakeRelationFrom rewrite = new FakeRelationFrom(
            new String("viewer"),
            new String("parent")
        );

        Assert.Equal(new RelationFromHash(rewrite), new RewriteHash(rewrite));
    }

    [Fact]
    public void DispatchesUnion()
    {
        FakeUnion rewrite = new FakeUnion([
            new FakeComputedRelation(new String("editor")),
            new FakeComputedRelation(new String("owner")),
        ]);

        Assert.Equal(new UnionHash(rewrite), new RewriteHash(rewrite));
    }

    [Fact]
    public void DispatchesIntersection()
    {
        FakeIntersection rewrite = new FakeIntersection([
            new FakeComputedRelation(new String("editor")),
            new FakeComputedRelation(new String("owner")),
        ]);

        Assert.Equal(new IntersectionHash(rewrite), new RewriteHash(rewrite));
    }

    [Fact]
    public void DispatchesDifference()
    {
        FakeDifference rewrite = new FakeDifference(
            new FakeComputedRelation(new String("viewer")),
            new FakeComputedRelation(new String("blocked"))
        );

        Assert.Equal(new DifferenceHash(rewrite), new RewriteHash(rewrite));
    }

    [Fact]
    public void ThrowsForUnknownVariant()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            new RewriteHash(new UnknownRewrite()).ToArray()
        );

        Assert.Contains(
            typeof(UnknownRewrite).FullName!,
            exception.Message,
            StringComparison.Ordinal
        );
        Assert.Contains("IRewrite", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeComputedRelation rewrite = new FakeComputedRelation(new String("editor"));

        Assert.Equal(
            new ComputedRelationHash(rewrite),
            new UntypedBytes(new RewriteHash(rewrite))
        );
    }
}
