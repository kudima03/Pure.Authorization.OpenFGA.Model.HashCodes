using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Condition;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Condition;

/// <summary>
/// Deterministic hash of an <see cref="IConditionParameter"/>: its name and its type.
/// </summary>
public sealed record ConditionParameterHash : IDeterminedHash
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

    private readonly IConditionParameter _parameter;

    public ConditionParameterHash(IConditionParameter parameter)
    {
        _parameter = parameter;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_parameter.Name))
                .Concat(new ConditionParameterTypeHash(_parameter.Type))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
