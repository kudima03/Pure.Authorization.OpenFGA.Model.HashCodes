using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Condition;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Condition;

/// <summary>
/// Deterministic hash of an <see cref="IConditionParameterType"/>: its name and its
/// generic type arguments. The order of the generic types matters.
/// </summary>
public sealed record ConditionParameterTypeHash : IDeterminedHash
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

    private readonly IConditionParameterType _parameterType;

    public ConditionParameterTypeHash(IConditionParameterType parameterType)
    {
        _parameterType = parameterType;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_parameterType.Name))
                .Concat(
                    new DeterminedHash(
                        _parameterType.GenericTypes.SelectMany(
                            x => new ConditionParameterTypeHash(x)
                        )
                    )
                )
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
