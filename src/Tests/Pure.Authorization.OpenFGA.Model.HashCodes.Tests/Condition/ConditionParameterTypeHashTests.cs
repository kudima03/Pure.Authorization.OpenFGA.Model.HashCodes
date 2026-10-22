using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.Condition;
using Pure.Authorization.OpenFGA.Model.HashCodes.Condition;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Condition;

public sealed record ConditionParameterTypeHashTests
{
    private static readonly byte[] TypePrefix =
    [
        167,
        139,
        46,
        101,
        10,
        78,
        56,
        99,
        156,
        152,
        98,
        145,
        238,
        103,
        159,
        164,
    ];

    private static readonly IConditionParameterType StringType =
        new FakeConditionParameterType(new String("string"), []);

    private static readonly IConditionParameterType IntType =
        new FakeConditionParameterType(new String("int"), []);

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("map")),
                .. SHA256.HashData([
                    .. new ConditionParameterTypeHash(StringType),
                    .. new ConditionParameterTypeHash(IntType),
                ]),
            ]),
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(new String("map"), [StringType, IntType])
            )
        );
    }

    [Fact]
    public void CorrectComputingStepsWithoutGenericTypes()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("string")),
                .. SHA256.HashData([]),
            ]),
            new ConditionParameterTypeHash(StringType)
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeConditionParameterType type = new FakeConditionParameterType(
            new String("list"),
            [StringType]
        );

        Assert.Equal(
            new ConditionParameterTypeHash(type),
            new UntypedBytes(new ConditionParameterTypeHash(type))
        );
    }

    [Fact]
    public void EqualForEqualTypes()
    {
        Assert.Equal<byte>(
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(new String("list"), [StringType])
            ),
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(
                    new String("list"),
                    [new FakeConditionParameterType(new String("string"), [])]
                )
            )
        );
    }

    [Fact]
    public void GenericTypesOrderMatters()
    {
        Assert.NotEqual<byte>(
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(new String("map"), [StringType, IntType])
            ),
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(new String("map"), [IntType, StringType])
            )
        );
    }

    [Fact]
    public void DiffersByName()
    {
        Assert.NotEqual<byte>(
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(new String("list"), [StringType])
            ),
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(new String("map"), [StringType])
            )
        );
    }

    [Fact]
    public void DiffersByGenericTypes()
    {
        Assert.NotEqual<byte>(
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(new String("list"), [StringType])
            ),
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(new String("list"), [IntType])
            )
        );
    }

    [Fact]
    public void DiffersByNesting()
    {
        Assert.NotEqual<byte>(
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(new String("list"), [StringType])
            ),
            new ConditionParameterTypeHash(
                new FakeConditionParameterType(
                    new String("list"),
                    [new FakeConditionParameterType(new String("list"), [StringType])]
                )
            )
        );
    }
}
