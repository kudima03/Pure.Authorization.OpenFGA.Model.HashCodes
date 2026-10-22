using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Model;

/// <summary>
/// Deterministic hash of an <see cref="IRelationDefinition"/>: its name and its rewrite.
/// </summary>
public sealed record RelationDefinitionHash : IDeterminedHash
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

    private readonly IRelationDefinition _relationDefinition;

    public RelationDefinitionHash(IRelationDefinition relationDefinition)
    {
        _relationDefinition = relationDefinition;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_relationDefinition.Name))
                .Concat(new RewriteHash(_relationDefinition.Rewrite))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
