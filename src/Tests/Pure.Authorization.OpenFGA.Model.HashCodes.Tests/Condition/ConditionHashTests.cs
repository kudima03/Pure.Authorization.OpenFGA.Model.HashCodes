using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.Abstractions.Condition;
using Pure.Authorization.OpenFGA.Model.HashCodes.Condition;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Condition;

public sealed record ConditionHashTests
{
    private static readonly byte[] TypePrefix =
    [
        132,
        233,
        26,
        57,
        162,
        185,
        110,
        213,
        99,
        27,
        235,
        36,
        236,
        109,
        187,
        6,
    ];

    private static readonly IConditionParameter GrantTime = new FakeConditionParameter(
        new String("grant_time"),
        new FakeConditionParameterType(new String("timestamp"), [])
    );

    private static readonly IConditionParameter GrantDuration =
        new FakeConditionParameter(
            new String("grant_duration"),
            new FakeConditionParameterType(new String("duration"), [])
        );

    private static readonly IConditionParameter CurrentTime = new FakeConditionParameter(
        new String("current_time"),
        new FakeConditionParameterType(new String("timestamp"), [])
    );

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([
                .. TypePrefix,
                .. new DeterminedHash(new String("non_expired")),
                .. new DeterminedHash(new String("current_time < grant_time")),
                .. new DeterminedHash([
                    new ConditionParameterHash(GrantTime),
                    new ConditionParameterHash(CurrentTime),
                ]),
            ]),
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time < grant_time"),
                    [GrantTime, CurrentTime]
                )
            )
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        FakeCondition condition = new FakeCondition(
            new String("non_expired"),
            new String("current_time < grant_time"),
            [GrantTime, CurrentTime]
        );

        Assert.Equal(
            new ConditionHash(condition),
            new UntypedBytes(new ConditionHash(condition))
        );
    }

    [Fact]
    public void EqualForEqualConditions()
    {
        Assert.Equal<byte>(
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time < grant_time"),
                    [GrantTime, CurrentTime]
                )
            ),
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time < grant_time"),
                    [
                        new FakeConditionParameter(
                            new String("grant_time"),
                            new FakeConditionParameterType(new String("timestamp"), [])
                        ),
                        new FakeConditionParameter(
                            new String("current_time"),
                            new FakeConditionParameterType(new String("timestamp"), [])
                        ),
                    ]
                )
            )
        );
    }

    [Fact]
    public void ParametersOrderDoesNotMatter()
    {
        Assert.Equal<byte>(
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time < grant_time + grant_duration"),
                    [GrantTime, GrantDuration, CurrentTime]
                )
            ),
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time < grant_time + grant_duration"),
                    [CurrentTime, GrantTime, GrantDuration]
                )
            )
        );
    }

    [Fact]
    public void DiffersByName()
    {
        Assert.NotEqual<byte>(
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time < grant_time"),
                    [GrantTime, CurrentTime]
                )
            ),
            new ConditionHash(
                new FakeCondition(
                    new String("expired"),
                    new String("current_time < grant_time"),
                    [GrantTime, CurrentTime]
                )
            )
        );
    }

    [Fact]
    public void DiffersByExpression()
    {
        Assert.NotEqual<byte>(
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time < grant_time"),
                    [GrantTime, CurrentTime]
                )
            ),
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time > grant_time"),
                    [GrantTime, CurrentTime]
                )
            )
        );
    }

    [Fact]
    public void DiffersByParameters()
    {
        Assert.NotEqual<byte>(
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time < grant_time"),
                    [GrantTime, CurrentTime]
                )
            ),
            new ConditionHash(
                new FakeCondition(
                    new String("non_expired"),
                    new String("current_time < grant_time"),
                    [GrantTime, GrantDuration]
                )
            )
        );
    }
}
