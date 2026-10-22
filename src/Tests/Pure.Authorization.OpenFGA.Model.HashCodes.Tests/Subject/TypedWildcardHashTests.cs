using System.Security.Cryptography;
using Pure.Authorization.OpenFGA.Model.HashCodes.Subject;
using Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;
using Pure.HashCodes;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Subject;

public sealed record TypedWildcardHashTests
{
    private static readonly byte[] TypePrefix =
    [
        253,
        90,
        141,
        68,
        232,
        162,
        135,
        101,
        221,
        125,
        244,
        231,
        198,
        222,
        83,
        98,
    ];

    [Fact]
    public void CorrectComputingSteps()
    {
        Assert.Equal(
            SHA256.HashData([.. TypePrefix, .. new DeterminedHash(new String("user"))]),
            new TypedWildcardHash(new FakeTypedWildcard(new String("user")))
        );
    }

    [Fact]
    public void EnumeratesAsUntyped()
    {
        Assert.Equal(
            new TypedWildcardHash(new FakeTypedWildcard(new String("user"))),
            new UntypedBytes(
                new TypedWildcardHash(new FakeTypedWildcard(new String("user")))
            )
        );
    }

    [Fact]
    public void EqualForEqualWildcards()
    {
        Assert.Equal<byte>(
            new TypedWildcardHash(new FakeTypedWildcard(new String("user"))),
            new TypedWildcardHash(new FakeTypedWildcard(new String("user")))
        );
    }

    [Fact]
    public void DiffersByType()
    {
        Assert.NotEqual<byte>(
            new TypedWildcardHash(new FakeTypedWildcard(new String("user"))),
            new TypedWildcardHash(new FakeTypedWildcard(new String("group")))
        );
    }
}
