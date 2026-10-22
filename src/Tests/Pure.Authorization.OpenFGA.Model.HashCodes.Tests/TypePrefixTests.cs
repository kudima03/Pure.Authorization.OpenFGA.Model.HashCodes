using System.Reflection;
using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests;

public sealed record TypePrefixTests
{
    [Fact]
    public void EveryConcreteHashHasSixteenBytePrefix()
    {
        Assert.Equal(
            [
                "AuthorizationModelHash",
                "ComputedRelationHash",
                "ConditionHash",
                "ConditionParameterHash",
                "ConditionParameterTypeHash",
                "ConditionalFormHash",
                "ConditionalTupleHash",
                "DifferenceHash",
                "DirectHash",
                "IntersectionHash",
                "ObjectHash",
                "RelationDefinitionHash",
                "RelationFromHash",
                "RelationshipConditionHash",
                "SubjectSetFormHash",
                "SubjectSetHash",
                "TupleHash",
                "TypeDefinitionHash",
                "TypeFormHash",
                "TypedWildcardHash",
                "UnionHash",
                "WildcardFormHash",
            ],
            Prefixes(typeof(ObjectHash).Assembly)
                .Where(x => x.Value.Length == 16)
                .Select(x => x.Key.Name)
                .Order(StringComparer.Ordinal)
        );
    }

    [Fact]
    public void PrefixesAreDistinct()
    {
        IReadOnlyCollection<string> prefixes =
        [
            .. Prefixes(typeof(ObjectHash).Assembly)
                .Concat(Prefixes(typeof(DeterminedHash).Assembly))
                .Select(x => Convert.ToHexString(x.Value)),
        ];

        Assert.True(prefixes.Count > 22);
        Assert.Equal(prefixes.Count, prefixes.Distinct(StringComparer.Ordinal).Count());
    }

    private static IEnumerable<KeyValuePair<Type, byte[]>> Prefixes(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Select(type => new KeyValuePair<Type, FieldInfo?>(
                type,
                type.GetField("TypePrefix", BindingFlags.NonPublic | BindingFlags.Static)
            ))
            .Where(x => x.Value is not null)
            .Select(x => new KeyValuePair<Type, byte[]>(
                x.Key,
                (byte[])x.Value!.GetValue(null)!
            ));
    }
}
