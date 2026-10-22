using Pure.Authorization.OpenFGA.Model.HashCodes.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests;

public sealed record UnknownVariantTests
{
    [Fact]
    public void ThrowsForRewriteNestedInSet()
    {
        _ = Assert.Throws<ArgumentException>(() =>
            new UnionHash(
                new FakeUnion([
                    new FakeComputedRelation(new String("editor")),
                    new FakeDifference(
                        new UnknownRewrite(),
                        new FakeComputedRelation(new String("blocked"))
                    ),
                    new FakeComputedRelation(new String("owner")),
                ])
            ).ToArray()
        );
    }

    [Fact]
    public void ThrowsForSubjectFormNestedInSet()
    {
        _ = Assert.Throws<ArgumentException>(() =>
            new DirectHash(
                new FakeDirect([
                    new FakeTypeForm(new String("user")),
                    new FakeConditionalForm(
                        new UnknownSubjectForm(),
                        new String("non_expired")
                    ),
                    new FakeWildcardForm(new String("user")),
                ])
            ).ToArray()
        );
    }

    [Fact]
    public void ThrowsForRewriteNestedInModel()
    {
        _ = Assert.Throws<ArgumentException>(() =>
            new AuthorizationModelHash(
                new FakeAuthorizationModel(
                    new String("1.1"),
                    [
                        new FakeTypeDefinition(new String("user"), []),
                        new FakeTypeDefinition(
                            new String("document"),
                            [
                                new FakeRelationDefinition(
                                    new String("owner"),
                                    new FakeComputedRelation(new String("editor"))
                                ),
                                new FakeRelationDefinition(
                                    new String("viewer"),
                                    new FakeIntersection([
                                        new FakeComputedRelation(new String("owner")),
                                        new UnknownRewrite(),
                                    ])
                                ),
                            ]
                        ),
                    ],
                    []
                )
            ).ToArray()
        );
    }
}
