using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Model;

public sealed record TypeDefinitionHashTests
{
    private static readonly byte[] TypePrefix =
    [
        149,
        120,
        138,
        220,
        207,
        54,
        224,
        224,
        91,
        196,
        227,
        106,
        16,
        250,
        215,
        182,
    ];

    private static readonly IRelationDefinition Owner = new FakeRelationDefinition(
        new String("owner"),
        new FakeDirect([new FakeTypeForm(new String("user"))])
    );

    private static readonly IRelationDefinition Editor = new FakeRelationDefinition(
        new String("editor"),
        new FakeUnion([
            new FakeDirect([new FakeTypeForm(new String("user"))]),
            new FakeComputedRelation(new String("owner")),
        ])
    );

    private static readonly IRelationDefinition Viewer = new FakeRelationDefinition(
        new String("viewer"),
        new FakeComputedRelation(new String("editor"))
    );

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("document")),
                .. new DeterminedHash([
                    new RelationDefinitionHash(Owner),
                    new RelationDefinitionHash(Editor),
                ]),
            ]),
            new TypeDefinitionHash(
                new FakeTypeDefinition(new String("document"), [Owner, Editor])
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeTypeDefinition type = new FakeTypeDefinition(
            new String("document"),
            [Owner, Editor]
        );

        Assert.Equal(
            new TypeDefinitionHash(type),
            new UntypedBytes(new TypeDefinitionHash(type))
        );
    }

    [Fact]
    public void EqualForEqualTypes()
    {
        Assert.Equal<byte>(
            new TypeDefinitionHash(
                new FakeTypeDefinition(new String("document"), [Viewer])
            ),
            new TypeDefinitionHash(
                new FakeTypeDefinition(
                    new String("document"),
                    [
                        new FakeRelationDefinition(
                            new String("viewer"),
                            new FakeComputedRelation(new String("editor"))
                        ),
                    ]
                )
            )
        );
    }

    [Fact]
    public void RelationsOrderDoesNotMatter()
    {
        Assert.Equal<byte>(
            new TypeDefinitionHash(
                new FakeTypeDefinition(new String("document"), [Owner, Editor, Viewer])
            ),
            new TypeDefinitionHash(
                new FakeTypeDefinition(new String("document"), [Viewer, Owner, Editor])
            )
        );
    }

    [Fact]
    public void DiffersByName()
    {
        Assert.NotEqual<byte>(
            new TypeDefinitionHash(
                new FakeTypeDefinition(new String("document"), [Owner])
            ),
            new TypeDefinitionHash(new FakeTypeDefinition(new String("folder"), [Owner]))
        );
    }

    [Fact]
    public void DiffersByRelations()
    {
        Assert.NotEqual<byte>(
            new TypeDefinitionHash(
                new FakeTypeDefinition(new String("document"), [Owner])
            ),
            new TypeDefinitionHash(
                new FakeTypeDefinition(new String("document"), [Owner, Viewer])
            )
        );
    }
}
