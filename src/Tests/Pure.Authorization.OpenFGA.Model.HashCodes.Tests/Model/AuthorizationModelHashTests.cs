using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.Condition;
using Pure.Authorization.OpenFGA.Model.Abstractions.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Condition;
using Pure.Authorization.OpenFGA.Model.HashCodes.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Model;

public sealed record AuthorizationModelHashTests
{
    private static readonly byte[] TypePrefix =
    [
        241,
        251,
        246,
        170,
        118,
        57,
        209,
        66,
        188,
        78,
        77,
        6,
        253,
        40,
        133,
        135,
    ];

    private static readonly ITypeDefinition User = new FakeTypeDefinition(
        new String("user"),
        []
    );

    private static readonly ITypeDefinition Group = new FakeTypeDefinition(
        new String("group"),
        [
            new FakeRelationDefinition(
                new String("member"),
                new FakeDirect([new FakeTypeForm(new String("user"))])
            ),
        ]
    );

    private static readonly ITypeDefinition Document = new FakeTypeDefinition(
        new String("document"),
        [
            new FakeRelationDefinition(
                new String("viewer"),
                new FakeDirect([
                    new FakeConditionalForm(
                        new FakeTypeForm(new String("user")),
                        new String("non_expired")
                    ),
                    new FakeSubjectSetForm(new String("group"), new String("member")),
                ])
            ),
        ]
    );

    private static readonly ICondition NonExpired = new FakeCondition(
        new String("non_expired"),
        new String("current_time < grant_time"),
        [
            new FakeConditionParameter(
                new String("grant_time"),
                new FakeConditionParameterType(new String("timestamp"), [])
            ),
        ]
    );

    private static readonly ICondition InOffice = new FakeCondition(
        new String("in_office"),
        new String("ip in cidr"),
        [
            new FakeConditionParameter(
                new String("ip"),
                new FakeConditionParameterType(new String("ipaddress"), [])
            ),
        ]
    );

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("1.1")),
                .. new DeterminedHash([
                    new TypeDefinitionHash(User),
                    new TypeDefinitionHash(Document),
                ]),
                .. new DeterminedHash([
                    new ConditionHash(NonExpired),
                    new ConditionHash(InOffice),
                ]),
            ]),
            new AuthorizationModelHash(
                new FakeAuthorizationModel(
                    new String("1.1"),
                    [User, Document],
                    [NonExpired, InOffice]
                )
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeAuthorizationModel model = new FakeAuthorizationModel(
            new String("1.1"),
            [User, Group, Document],
            [NonExpired]
        );

        Assert.Equal(
            new AuthorizationModelHash(model),
            new UntypedBytes(new AuthorizationModelHash(model))
        );
    }

    [Fact]
    public void EqualForEqualModels()
    {
        Assert.Equal<byte>(
            new AuthorizationModelHash(
                new FakeAuthorizationModel(new String("1.1"), [User, Group], [NonExpired])
            ),
            new AuthorizationModelHash(
                new FakeAuthorizationModel(
                    new String("1.1"),
                    [
                        new FakeTypeDefinition(new String("user"), []),
                        new FakeTypeDefinition(
                            new String("group"),
                            [
                                new FakeRelationDefinition(
                                    new String("member"),
                                    new FakeDirect([new FakeTypeForm(new String("user"))])
                                ),
                            ]
                        ),
                    ],
                    [
                        new FakeCondition(
                            new String("non_expired"),
                            new String("current_time < grant_time"),
                            [
                                new FakeConditionParameter(
                                    new String("grant_time"),
                                    new FakeConditionParameterType(
                                        new String("timestamp"),
                                        []
                                    )
                                ),
                            ]
                        ),
                    ]
                )
            )
        );
    }

    [Fact]
    public void TypesOrderDoesNotMatter()
    {
        Assert.Equal<byte>(
            new AuthorizationModelHash(
                new FakeAuthorizationModel(new String("1.1"), [User, Group, Document], [])
            ),
            new AuthorizationModelHash(
                new FakeAuthorizationModel(new String("1.1"), [Document, User, Group], [])
            )
        );
    }

    [Fact]
    public void ConditionsOrderDoesNotMatter()
    {
        Assert.Equal<byte>(
            new AuthorizationModelHash(
                new FakeAuthorizationModel(
                    new String("1.1"),
                    [User],
                    [NonExpired, InOffice]
                )
            ),
            new AuthorizationModelHash(
                new FakeAuthorizationModel(
                    new String("1.1"),
                    [User],
                    [InOffice, NonExpired]
                )
            )
        );
    }

    [Fact]
    public void DiffersBySchemaVersion()
    {
        Assert.NotEqual<byte>(
            new AuthorizationModelHash(
                new FakeAuthorizationModel(new String("1.1"), [User], [NonExpired])
            ),
            new AuthorizationModelHash(
                new FakeAuthorizationModel(new String("1.2"), [User], [NonExpired])
            )
        );
    }

    [Fact]
    public void DiffersByTypes()
    {
        Assert.NotEqual<byte>(
            new AuthorizationModelHash(
                new FakeAuthorizationModel(new String("1.1"), [User], [NonExpired])
            ),
            new AuthorizationModelHash(
                new FakeAuthorizationModel(new String("1.1"), [User, Group], [NonExpired])
            )
        );
    }

    [Fact]
    public void DiffersByConditions()
    {
        Assert.NotEqual<byte>(
            new AuthorizationModelHash(
                new FakeAuthorizationModel(new String("1.1"), [User], [NonExpired])
            ),
            new AuthorizationModelHash(
                new FakeAuthorizationModel(new String("1.1"), [User], [InOffice])
            )
        );
    }
}
