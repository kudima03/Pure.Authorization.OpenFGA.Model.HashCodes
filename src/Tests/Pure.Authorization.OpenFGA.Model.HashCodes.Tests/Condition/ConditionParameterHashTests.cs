using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.Condition;
using Pure.Authorization.OpenFGA.Model.HashCodes.Condition;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Condition;

public sealed record ConditionParameterHashTests
{
    private static readonly byte[] TypePrefix =
    [
        188,
        141,
        249,
        8,
        132,
        224,
        157,
        196,
        236,
        228,
        4,
        94,
        72,
        67,
        182,
        32,
    ];

    private static readonly IConditionParameterType Timestamp =
        new FakeConditionParameterType(new String("timestamp"), []);

    private static readonly IConditionParameterType Duration =
        new FakeConditionParameterType(new String("duration"), []);

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("grant_time")),
                .. new ConditionParameterTypeHash(Timestamp),
            ]),
            new ConditionParameterHash(
                new FakeConditionParameter(new String("grant_time"), Timestamp)
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeConditionParameter parameter = new FakeConditionParameter(
            new String("grant_time"),
            Timestamp
        );

        Assert.Equal(
            new ConditionParameterHash(parameter),
            new UntypedBytes(new ConditionParameterHash(parameter))
        );
    }

    [Fact]
    public void EqualForEqualParameters()
    {
        Assert.Equal<byte>(
            new ConditionParameterHash(
                new FakeConditionParameter(new String("grant_time"), Timestamp)
            ),
            new ConditionParameterHash(
                new FakeConditionParameter(
                    new String("grant_time"),
                    new FakeConditionParameterType(new String("timestamp"), [])
                )
            )
        );
    }

    [Fact]
    public void DiffersByName()
    {
        Assert.NotEqual<byte>(
            new ConditionParameterHash(
                new FakeConditionParameter(new String("grant_time"), Timestamp)
            ),
            new ConditionParameterHash(
                new FakeConditionParameter(new String("current_time"), Timestamp)
            )
        );
    }

    [Fact]
    public void DiffersByType()
    {
        Assert.NotEqual<byte>(
            new ConditionParameterHash(
                new FakeConditionParameter(new String("grant_time"), Timestamp)
            ),
            new ConditionParameterHash(
                new FakeConditionParameter(new String("grant_time"), Duration)
            )
        );
    }
}
