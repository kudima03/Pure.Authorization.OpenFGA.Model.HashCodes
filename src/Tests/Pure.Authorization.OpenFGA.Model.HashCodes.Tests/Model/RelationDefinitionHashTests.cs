using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Model;

public sealed record RelationDefinitionHashTests
{
    private static readonly byte[] TypePrefix =
    [
        133,
        76,
        168,
        220,
        11,
        86,
        23,
        200,
        104,
        217,
        225,
        224,
        39,
        197,
        23,
        14,
    ];

    private static readonly IRewrite Users = new FakeDirect([
        new FakeTypeForm(new String("user")),
    ]);

    private static readonly IRewrite Editor = new FakeComputedRelation(
        new String("editor")
    );

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("viewer")),
                .. new RewriteHash(Users),
            ]),
            new RelationDefinitionHash(
                new FakeRelationDefinition(new String("viewer"), Users)
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeRelationDefinition relation = new FakeRelationDefinition(
            new String("viewer"),
            Users
        );

        Assert.Equal(
            new RelationDefinitionHash(relation),
            new UntypedBytes(new RelationDefinitionHash(relation))
        );
    }

    [Fact]
    public void EqualForEqualRelations()
    {
        Assert.Equal<byte>(
            new RelationDefinitionHash(
                new FakeRelationDefinition(new String("viewer"), Users)
            ),
            new RelationDefinitionHash(
                new FakeRelationDefinition(
                    new String("viewer"),
                    new FakeDirect([new FakeTypeForm(new String("user"))])
                )
            )
        );
    }

    [Fact]
    public void DiffersByName()
    {
        Assert.NotEqual<byte>(
            new RelationDefinitionHash(
                new FakeRelationDefinition(new String("viewer"), Users)
            ),
            new RelationDefinitionHash(
                new FakeRelationDefinition(new String("editor"), Users)
            )
        );
    }

    [Fact]
    public void DiffersByRewrite()
    {
        Assert.NotEqual<byte>(
            new RelationDefinitionHash(
                new FakeRelationDefinition(new String("viewer"), Users)
            ),
            new RelationDefinitionHash(
                new FakeRelationDefinition(new String("viewer"), Editor)
            )
        );
    }
}
