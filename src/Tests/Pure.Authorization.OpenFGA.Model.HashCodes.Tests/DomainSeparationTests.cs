using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Subject;
using Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests;

public sealed record DomainSeparationTests
{
    [Fact]
    public void SameStringInDifferentTypesHashesDifferently()
    {
        IReadOnlyCollection<string> hashes =
        [
            Convert.ToHexString([
                .. new TypeFormHash(new FakeTypeForm(new String("user"))),
            ]),
            Convert.ToHexString([
                .. new WildcardFormHash(new FakeWildcardForm(new String("user"))),
            ]),
            Convert.ToHexString([
                .. new TypedWildcardHash(new FakeTypedWildcard(new String("user"))),
            ]),
            Convert.ToHexString([
                .. new ComputedRelationHash(new FakeComputedRelation(new String("user"))),
            ]),
        ];

        Assert.Equal(hashes.Count, hashes.Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void SameFieldsInDifferentTypesHashDifferently()
    {
        IReadOnlyCollection<string> hashes =
        [
            Convert.ToHexString([
                .. new ObjectHash(
                    new FakeObject(new String("group"), new String("member"))
                ),
            ]),
            Convert.ToHexString([
                .. new SubjectSetFormHash(
                    new FakeSubjectSetForm(new String("group"), new String("member"))
                ),
            ]),
            Convert.ToHexString([
                .. new RelationFromHash(
                    new FakeRelationFrom(new String("group"), new String("member"))
                ),
            ]),
        ];

        Assert.Equal(hashes.Count, hashes.Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void UnionDiffersFromIntersection()
    {
        FakeComputedRelation editor = new FakeComputedRelation(new String("editor"));
        FakeComputedRelation owner = new FakeComputedRelation(new String("owner"));

        Assert.NotEqual(
            new UnionHash(new FakeUnion([editor, owner])),
            new IntersectionHash(new FakeIntersection([editor, owner]))
        );
    }
}
